using System.Windows;
using System.Collections.Generic;
using EasyPasswordRecoveryPDF.Model;
using EasyPasswordRecoveryPDF.Common;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class FilePropertiesViewModel : ViewModelBase
    {
        #region [ Defines ]

        private Window FilePropertiesView = null;

        #endregion

        #region [ Constructor ]

        public FilePropertiesViewModel(Window filePropertiesView, PdfFile pdfFile)
        {
            FilePropertiesView = filePropertiesView;
            Properties = pdfFile.GetPdfFileInfo();
            Info = pdfFile.GetEncryptionInfo();
            InitializeCommands();
        }

        #endregion

        #region [ Properties ]

        private Dictionary<string, string> properties = null;
        public Dictionary<string, string> Properties
        {
            get { return properties; }
            private set { SetProperty(ref this.properties, value); }
        }

        private Dictionary<string, string> info = null;
        public Dictionary<string, string> Info
        {
            get { return info; }
            private set { SetProperty(ref this.info, value); }
        }

        #endregion

        #region [ Commands ]

        private void InitializeCommands()
        {
            CloseWindowCmd = new RelayCommand(OnCloseWindowCmdExecute, OnCloseWindowCmdCanExecute);
        }

        public RelayCommand CloseWindowCmd { get; private set; }

        public bool OnCloseWindowCmdCanExecute()
        {
            return FilePropertiesView != null;
        }

        public void OnCloseWindowCmdExecute()
        {
            SystemCommands.CloseWindow(FilePropertiesView);
        }
        
        #endregion
    }
}
