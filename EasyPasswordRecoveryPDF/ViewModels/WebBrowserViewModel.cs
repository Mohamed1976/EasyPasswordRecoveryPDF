using System;
using System.Windows;
using System.Windows.Controls;
using EasyPasswordRecoveryPDF.Common;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class WebBrowserViewModel : ViewModelBase
    {
        #region [ Defines ]

        private Window webBrowserView = null;

        #endregion

        #region[ Constructor ]

        public WebBrowserViewModel(Window view, string path)
        {
            webBrowserView = view;
            BrowserUri = new Uri(path);
            FileName = path;
            InitializeCommands();
        }

        #endregion

        #region [ Commands]

        public RelayCommand<object> CleanUpCmd { get; private set; }
        public RelayCommand CloseCmd { get; private set; }

        private void InitializeCommands()
        {
            CleanUpCmd = new RelayCommand<object>(OnCleanUpCmdExecute);
            CloseCmd = new RelayCommand(OnCloseCmdExecute, OnCloseCmdCanExecute);
        }

        //Avoid PDF lock by AcroRd32.dll after closure
        private async void OnCleanUpCmdExecute(object parameter)
        {
            WebBrowser webBrowser = (WebBrowser)parameter;
            if (webBrowserView != null)
            {
                await App.Current.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    webBrowser.NavigateToString("about:blank");
                }));
            }
        }

        private bool OnCloseCmdCanExecute()
        {
            return webBrowserView != null;
        }

        private void OnCloseCmdExecute()
        {
            webBrowserView.DialogResult = false;
            SystemCommands.CloseWindow(webBrowserView);
        }

        #endregion

        #region [ Properties ]

        private Uri browserUri = null;
        public Uri BrowserUri
        {
            get { return browserUri; }
            private set { SetProperty(ref this.browserUri, value); }
        }

        private string fileName = string.Empty;
        public string FileName
        {
            get { return fileName; }
            private set { SetProperty(ref this.fileName, value); }
        }

        #endregion
    }
}
