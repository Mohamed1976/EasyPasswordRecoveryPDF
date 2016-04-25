using System;
using System.Windows;
using System.Diagnostics;
using EasyPasswordRecoveryPDF.Views;
using EasyPasswordRecoveryPDF.Services;
using EasyPasswordRecoveryPDF.ViewModels;
using EasyPasswordRecoveryPDF.Properties;

namespace EasyPasswordRecoveryPDF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region [ Constructor ]

        public App()
        {
            ServiceInjector.InjectServices();
        }

        #endregion

        #region [ Override Methods ]

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                MainView mainWindow = new MainView();
                MainViewModel mainViewModel = new MainViewModel();
                mainWindow.DataContext = mainViewModel;
                mainWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OnStartup" + ex.ToString());
            }
        }

        #endregion 
    }
}
