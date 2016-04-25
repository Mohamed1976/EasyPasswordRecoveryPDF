using System;
using System.Linq;
using System.Windows.Media;
using EasyPasswordRecoveryPDF.Model;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Interfaces;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class DictionaryManagerViewModel : ViewModelBase, ITabViewModel
    {
        #region [ Constructor ]

        public DictionaryManagerViewModel(string header, DrawingBrush headerIcon)
        {
            Header = header;
            HeaderIcon = headerIcon;
            PasswordIterator = new Dictionaries();
            InitializeCommands();
        }

        #endregion

        #region [ Properties ]

        private Dictionary selectedDictionary = null;
        public Dictionary SelectedDictionary
        {
            get { return selectedDictionary; }
            set { SetProperty(ref this.selectedDictionary, value); }
        }

        #endregion

        #region [ Commands ]

        private void InitializeCommands()
        {
            AddCmd = new RelayCommand(OnAddCmdExecute);
            RemoveCmd = new RelayCommand(OnRemoveCmdExecute, OnRemoveCmdCanExecute);
            MoveUpCmd = new RelayCommand(OnMoveUpCmdExecute, OnMoveUpCmdCanExecute);
            MoveDownCmd = new RelayCommand(OnMoveDownCmdExecute, OnMoveDownCmdCanExecute);
        }

        public RelayCommand AddCmd { get; private set; }
        public RelayCommand RemoveCmd { get; private set; }
        public RelayCommand MoveUpCmd { get; private set; }
        public RelayCommand MoveDownCmd { get; private set; }

        private async void OnAddCmdExecute()
        {
            string errorMsg = string.Empty;
            string[] filenames = await fileService.ShowOpenFileDialogAsync("Select password dictionaries to use",
                ".txt", true, Tuple.Create("TXT Files", "*.txt"));

            if (filenames != null)
            {
                foreach (string filename in filenames)
                {
                    ((Dictionaries)PasswordIterator).Add(new Dictionary(filename));
                }
            }
        }

        private void OnRemoveCmdExecute()
        {
            int index = ((Dictionaries)PasswordIterator).IndexOf(SelectedDictionary);
            App.Current.Dispatcher.Invoke(delegate
            { ((Dictionaries)PasswordIterator).RemoveAt(index); });

            if (((Dictionaries)PasswordIterator).Count > index)
            {
                SelectedDictionary = ((Dictionaries)PasswordIterator)[index];
            }
            else if (((Dictionaries)PasswordIterator).Any())
            {
                SelectedDictionary = ((Dictionaries)PasswordIterator)[index - 1];
            }
        }

        private bool OnRemoveCmdCanExecute()
        {
            return !IsBusy && SelectedDictionary != null;
        }

        private void MoveUpDownSelected(int direction)
        {
            int index = ((Dictionaries)PasswordIterator).IndexOf(SelectedDictionary);
            ((Dictionaries)PasswordIterator).Move(index, index + direction);

            //Needed to trigger Selection Changed event In datagrid
            SelectedDictionary = null;
            SelectedDictionary = ((Dictionaries)PasswordIterator)[index + direction];
        }

        private void OnMoveUpCmdExecute()
        {
            MoveUpDownSelected(-1);
        }

        private bool OnMoveUpCmdCanExecute()
        {
            return !IsBusy && SelectedDictionary != null &&
                ((Dictionaries)PasswordIterator).IndexOf(SelectedDictionary) > 0;
        }

        private void OnMoveDownCmdExecute()
        {
            MoveUpDownSelected(1);
        }

        private bool OnMoveDownCmdCanExecute()
        {
            return !IsBusy && SelectedDictionary != null &&
                ((Dictionaries)PasswordIterator).IndexOf(SelectedDictionary)
                < ((Dictionaries)PasswordIterator).Count - 1;
        }

        #endregion

        #region[ ITabViewModel ]

        private string header = string.Empty;
        public string Header
        {
            get { return header; }
            set { SetProperty(ref this.header, value); }
        }

        private DrawingBrush headerIcon = null;
        public DrawingBrush HeaderIcon
        {
            get { return headerIcon; }
            set { SetProperty(ref this.headerIcon, value); }
        }

        private IPasswordIterator passwordIterator = null;
        public IPasswordIterator PasswordIterator
        {
            get { return this.passwordIterator; }
            set { SetProperty(ref this.passwordIterator, value); }
        }

        #endregion
    }
}
