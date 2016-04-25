using EasyPasswordRecoveryPDF.Services.Interfaces;

namespace EasyPasswordRecoveryPDF.Services
{
    //http://www.codeproject.com/Articles/70223/Using-a-Service-Locator-to-Work-with-MessageBoxes
    public static class ServiceInjector
    {
        // Loads service objects into the ServiceContainer on startup.
        public static void InjectServices()
        {
            ServiceContainer svcMgr = ServiceContainer.Instance;
            // Dispatcher Service should be added first
            svcMgr.AddService<IDispatcherService>(new DispatcherService());
            svcMgr.AddService<IFileService>(new FileService(svcMgr.GetService<IDispatcherService>()));
            svcMgr.AddService<IFolderBrowserService>(new FolderBrowserService(svcMgr.GetService<IDispatcherService>()));
            svcMgr.AddService<IMessageService>(new MessageService(svcMgr.GetService<IDispatcherService>()));
            svcMgr.AddService<IDialogService>(new DialogService(svcMgr.GetService<IDispatcherService>()));
        }
    }
}
