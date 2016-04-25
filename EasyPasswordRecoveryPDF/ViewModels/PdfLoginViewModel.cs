using System.Windows;
using System.Windows.Media;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf.Decryption;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class PdfLoginViewModel : ViewModelBase
    {
        #region [ Defines ]

        private Window pdfLoginView = null;

        #endregion

        #region [ Constructor ]

        public PdfLoginViewModel(Window pdfLoginWindow, EncryptionRecord encryptionRecord)
        {
            PdfDecryptor = DecryptorFactory.Get(encryptionRecord);
            pdfLoginView = pdfLoginWindow;
            InitializeCommands();
        }

        #endregion

        #region [ Command properties ]

        private void InitializeCommands()
        {
            OkCmd = new RelayCommand(OnOkCmdExecute, OnOkCmdCanExecute);
            CancelCmd = new RelayCommand(OnCancelCmdExecute);
        }

        public RelayCommand OkCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        private void OnOkCmdExecute()
        {
            PasswordValidity validity = PdfDecryptor.ValidatePassword(Password, 
                ValidationMode.ValidateUserPassword | ValidationMode.ValidateOwnerPassword);

            if (validity == PasswordValidity.Invalid)
            {
                ErrorContent = "Failed to unlock PDF, please try again [" + Password + "]";
                ForegroundColor = Brushes.Red;
                HasError = true;
            }
            else
            {
                ForegroundColor = Brushes.Green;
                HasError = false;

                if ((validity & PasswordValidity.UserPasswordIsValid) == PasswordValidity.UserPasswordIsValid &&
                    (validity & PasswordValidity.OwnerPasswordIsValid) == PasswordValidity.OwnerPasswordIsValid)
                {
                    RecoveredUserPassword = Password;
                    RecoveredOwnerPassword = Password;
                    ErrorContent = string.Format("PDF unlocked with User and Owner password: [{0}].", Password);
                }
                else
                {
                    if ((validity & PasswordValidity.UserPasswordIsValid) == PasswordValidity.UserPasswordIsValid)
                    {
                        RecoveredUserPassword = Password;
                        ErrorContent = string.Format("PDF unlocked with User password: [{0}].", Password);
                    }

                    if ((validity & PasswordValidity.OwnerPasswordIsValid) == PasswordValidity.OwnerPasswordIsValid)
                    {
                        RecoveredOwnerPassword = Password;
                        ErrorContent = string.Format("PDF unlocked with Owner password: [{0}].", Password);
                    }
                }
            }
            Password = string.Empty;
        }

        private bool OnOkCmdCanExecute()
        {
            return Password.Length > 0;
        }

        private void OnCancelCmdExecute()
        {
            pdfLoginView.DialogResult = false;
            pdfLoginView.Close();
        }

        #endregion

        #region [ Properties ]

        private IDecryptor pdfDecryptor = null;
        public IDecryptor PdfDecryptor
        {
            get { return pdfDecryptor; }
            private set { SetProperty(ref this.pdfDecryptor, value); }
        }

        private Brush foregroundColor = Brushes.Black;
        public Brush ForegroundColor
        {
            get { return foregroundColor; }
            private set { SetProperty(ref this.foregroundColor, value); } 
        }

        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            private set { SetProperty(ref this.password, value); }
        }

        private string recoveredUserPassword = string.Empty;
        public string RecoveredUserPassword
        {
            get { return recoveredUserPassword; }
            private set { SetProperty(ref this.recoveredUserPassword, value); }
        }

        private string recoveredOwnerPassword = string.Empty;
        public string RecoveredOwnerPassword
        {
            get { return recoveredOwnerPassword; }
            private set { SetProperty(ref this.recoveredOwnerPassword, value); }
        }

        private string errorContent = string.Empty;
        public string ErrorContent
        {
            get { return errorContent; }
            private set { SetProperty(ref this.errorContent, value); }
        }

        private bool hasError = false;
        public bool HasError
        {
            get { return hasError; }
            private set { SetProperty(ref this.hasError, value); }
        }

        #endregion
    }
}
