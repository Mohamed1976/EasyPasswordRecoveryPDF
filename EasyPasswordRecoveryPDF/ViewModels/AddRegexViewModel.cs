using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Model;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class AddRegexViewModel : ViewModelBase
    {
        #region [ Defines ]

        private Window AddRegexView = null;

        #endregion

        #region [ Constructor ]

        public AddRegexViewModel(Window addRegexView)
        {
            AddRegexView = addRegexView;
            RegEx = new RegularExpression();
            RegExMatches = new ObservableCollectionExt<string>();
            InitializeBackgroundWorkers();
            InitializeCommands();
        }

        #endregion

        #region [ Properties ]

        private RegularExpression regEx = null;
        public RegularExpression RegEx
        {
            get { return regEx; }
            set { SetProperty(ref this.regEx, value); }
        }

        #region [ MaxRows ]

        private int maxRows = Constants.DefaultMaxRowsRegEx;
        public int MaxRows
        {
            get { return maxRows; }
            set { SetProperty(ref this.maxRows, value); }
        }

        private int maxMaxRows = Constants.DefaultMaxMaxRowsRegEx;
        public int MaxMaxRows
        {
            get { return maxMaxRows; }
            set { SetProperty(ref this.maxMaxRows, value); }
        }

        private int minMaxRows = Constants.DefaultMinMaxRowsRegEx;
        public int MinMaxRows
        {
            get { return minMaxRows; }
            set { SetProperty(ref this.minMaxRows, value); }
        }

        #endregion

        /// <summary>
        /// Gets or sets the Status property. This property displays
        /// the status message.
        /// </summary>
        private string status = string.Empty;
        public string Status
        {
            get { return status; }
            set { this.SetProperty(ref this.status, value); }
        }

        private ObservableCollectionExt<string> regExMatches = null;
        public ObservableCollectionExt<string> RegExMatches
        {
            get { return regExMatches; }
            set { this.SetProperty(ref this.regExMatches, value); }
        }

        #endregion

        #region [ BackgroundWorkers ] 

        #region [ InitializeBackgroundWorkers ] 

        private BackgroundWorker previewBackgroundWorker = null;

        private void InitializeBackgroundWorkers()
        {
            previewBackgroundWorker = new BackgroundWorker();
            previewBackgroundWorker.WorkerSupportsCancellation = true;
            previewBackgroundWorker.DoWork +=
                new DoWorkEventHandler(PreviewWorkerDoWork);
            previewBackgroundWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(PreviewWorkerRunWorkerCompleted);
        }

        #endregion

        #region [ PreviewBackgroundWorker ] 

        private void PreviewWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            List<string> regExList = new List<string>();
            const int maxCount = 100;
            object[] args = (object[])e.Argument;
            RegularExpression passwordIterator = (RegularExpression)args[0];
            int maxRows = (int)args[1];

            string currentPassword = passwordIterator.GetNextPassword();
            while (!string.IsNullOrEmpty(currentPassword) &&
                !previewBackgroundWorker.CancellationPending &&
                passwordIterator.Progress < maxRows)
            {
                regExList.Add(currentPassword);
                currentPassword = passwordIterator.GetNextPassword();

                if (string.IsNullOrEmpty(currentPassword) ||
                    regExList.Count > maxCount ||
                    passwordIterator.Progress >= maxRows)
                {
                    App.Current.Dispatcher.Invoke(delegate
                    {
                        RegExMatches.BeginUpdate();
                        foreach (string regEx in regExList)
                        {
                            RegExMatches.Add(regEx);
                        }
                        RegExMatches.EndUpdate();
                    });

                    regExList.Clear();
                }
            }

            if (previewBackgroundWorker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = passwordIterator.Progress;
            }
        }

        private void PreviewWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            string result = string.Empty;

            if (e.Error != null)
            {
                result = string.Format("An unexpected error occured: {0}.", e.Error.Message);
            }
            else if (e.Cancelled)
            {
                result = "Preview process was stopped by user.";
            }
            else
            {
                result = string.Format("Finished preview process, {0} lines added.", e.Result);
            }

            Status = result;
            IsBusy = false; 
        }

        #endregion

        #endregion

        #region [ Commands ]

        private void InitializeCommands()
        {
            OkCmd = new RelayCommand(OnOkCmdExecute);
            CancelCmd = new RelayCommand(OnCancelCmdExecute);
            StartCmd = new RelayCommand(OnStartCmdExecute, OnStartCmdCanExecute);
            StopCmd = new RelayCommand(OnStopCmdExecute, OnStopCmdCanExecute);
        }

        public RelayCommand OkCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand StartCmd { get; private set; }
        public RelayCommand StopCmd { get; private set; }

        private void OnOkCmdExecute()
        {
            string errorMsg = string.Empty;

            if (previewBackgroundWorker.IsBusy)
            {
                errorMsg = "Please stop the preview process.";
            }
            else if(RegEx.Initialize(ref errorMsg) == Constants.Success)
            {
                AddRegexView.DialogResult = true;
                AddRegexView.Close();           
            }

            Status = errorMsg;
        }

        private void OnCancelCmdExecute()
        {
            if(previewBackgroundWorker.IsBusy)
            {
                previewBackgroundWorker.CancelAsync();
            }

            AddRegexView.DialogResult = false;
            AddRegexView.Close();
        }

        private void OnStartCmdExecute()
        {
            Status = string.Empty;

            string errorMsg = string.Empty;
            if (RegEx.Initialize(ref errorMsg) == Constants.Success)
            {
                IsBusy = true;
                RegExMatches.Clear();                
                previewBackgroundWorker.RunWorkerAsync(new object[] { RegEx, MaxRows });
            }
            else
            {
                Status = errorMsg;
            }
        }

        private bool OnStartCmdCanExecute()
        {
            return !previewBackgroundWorker.IsBusy;
        }

        private void OnStopCmdExecute()
        {
            previewBackgroundWorker.CancelAsync();
        }

        private bool OnStopCmdCanExecute()
        {
            return previewBackgroundWorker.IsBusy;
        }

        #endregion
    }
}
