using System.Linq;
using System.Windows.Media;
using EasyPasswordRecoveryPDF.Interfaces;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Views;
using EasyPasswordRecoveryPDF.Model;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class SmartManagerViewModel : ViewModelBase, ITabViewModel
    {
        #region [ Constructor ]

        public SmartManagerViewModel(string header, DrawingBrush headerIcon)
        {
            Header = header;
            HeaderIcon = headerIcon;
            PasswordIterator = new RegularExpressions();
            InitializeCommands();
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

        #region [ Properties ]

        private RegularExpression selectedExpression = null;
        public RegularExpression SelectedExpression
        {
            get { return selectedExpression; }
            set { SetProperty(ref selectedExpression, value); }
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
            AddRegexView addRegexView = new AddRegexView();
            AddRegexViewModel addRegexViewModel = new AddRegexViewModel(addRegexView);
            bool? result = await dialogService.InitDialog(addRegexView, addRegexViewModel);
            if (result != null && result == true)
            {
                ((RegularExpressions)PasswordIterator).Add(addRegexViewModel.RegEx);
            }
            addRegexViewModel.Dispose();
        }

        private void OnRemoveCmdExecute()
        {
            int index = ((RegularExpressions)PasswordIterator).IndexOf(SelectedExpression);
            App.Current.Dispatcher.Invoke(delegate 
            { ((RegularExpressions)PasswordIterator).RemoveAt(index); });

            if (((RegularExpressions)PasswordIterator).Count > index)
            {
                SelectedExpression = ((RegularExpressions)PasswordIterator)[index];
            }
            else if (((RegularExpressions)PasswordIterator).Any())
            {
                SelectedExpression = ((RegularExpressions)PasswordIterator)[index - 1];
            }
        }

        private bool OnRemoveCmdCanExecute()
        {
            return !IsBusy && SelectedExpression != null;
        }

        private void MoveUpDownSelected(int direction)
        {
            int index = ((RegularExpressions)PasswordIterator).IndexOf(SelectedExpression);
            ((RegularExpressions)PasswordIterator).Move(index, index + direction);

            //Needed to trigger Selection Changed event In datagrid
            SelectedExpression = null;
            SelectedExpression = ((RegularExpressions)PasswordIterator)[index + direction];
        }

        private void OnMoveUpCmdExecute()
        {
            MoveUpDownSelected(-1);
        }

        private bool OnMoveUpCmdCanExecute()
        {
            return !IsBusy && SelectedExpression != null && 
                ((RegularExpressions)PasswordIterator).IndexOf(SelectedExpression) > 0;
        }

        private void OnMoveDownCmdExecute()
        {
            MoveUpDownSelected(1);
        }

        private bool OnMoveDownCmdCanExecute()
        {
            return !IsBusy && SelectedExpression != null &&
                ((RegularExpressions)PasswordIterator).IndexOf(SelectedExpression) 
                < ((RegularExpressions)PasswordIterator).Count - 1;
        }

        #endregion
    }
}
