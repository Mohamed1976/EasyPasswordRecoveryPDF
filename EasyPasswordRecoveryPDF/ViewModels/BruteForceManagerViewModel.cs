using System.Windows.Media;
using EasyPasswordRecoveryPDF.Views;
using EasyPasswordRecoveryPDF.Interfaces;
using EasyPasswordRecoveryPDF.Model;
using EasyPasswordRecoveryPDF.Common;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class BruteForceManagerViewModel : ViewModelBase, ITabViewModel
    {
        #region [ Constructor ]

        public BruteForceManagerViewModel(string header, DrawingBrush headerIcon)
        {
            Header = header;
            HeaderIcon = headerIcon;
            PasswordIterator = new Password();
            InitializeCommands();
        }

        #endregion

        #region [ Commands ]

        private void InitializeCommands()
        {
            PreviewCmd = new RelayCommand(OnPreviewCmdExecute);
        }

        public RelayCommand PreviewCmd { get; private set; }

        private async void OnPreviewCmdExecute()
        {
            string errorMsg = string.Empty;

            if (PasswordIterator.Initialize(CharsetEncoding.Unicode, 
                Constants.MaxPasswordSize, 
                ref errorMsg) == Constants.Success)
            {
                BruteForcePreviewView bruteForcePreviewView = new BruteForcePreviewView();
                BruteForcePreviewViewModel bruteForcePreviewViewModel =
                    new BruteForcePreviewViewModel(bruteForcePreviewView,
                    ((Password)PasswordIterator).IteratorCounters,
                    ((Password)PasswordIterator).CharactersToUse);
                bool? result = await dialogService.InitDialog(bruteForcePreviewView, bruteForcePreviewViewModel);
                bruteForcePreviewViewModel.Dispose();
            }
        }

        #endregion

        #region[ ITabViewModel ]

        private string header = string.Empty;
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        private DrawingBrush headerIcon = null;
        public DrawingBrush HeaderIcon
        {
            get { return headerIcon; }
            set { SetProperty(ref headerIcon, value); }
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
