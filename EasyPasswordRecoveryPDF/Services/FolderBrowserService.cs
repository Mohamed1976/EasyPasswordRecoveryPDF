using System;
using System.Threading.Tasks;
using EasyPasswordRecoveryPDF.Services.Interfaces;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;


namespace EasyPasswordRecoveryPDF.Services
{
    public class FolderBrowserService : IFolderBrowserService
    {
        private readonly IDispatcherService _dispatcherService;

        public FolderBrowserService(IDispatcherService service)
        {
            _dispatcherService = service;
        }

        public Task<string> ShowFolderBrowserDialogAsync(string title)
        {
            var tcs = new TaskCompletionSource<string>();

            _dispatcherService.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                var result = string.Empty;
                // Create a CommonOpenFileDialog to select source file
                CommonOpenFileDialog cfd = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    Title = title ?? "Select File",
                };

                if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    ShellObject selectedObj = null;

                    try
                    {
                        // Try to get the selected item 
                        selectedObj = cfd.FileAsShellObject;
                    }
                    catch
                    {
                        //MessageBox.Show("Could not create a ShellObject from the selected item");
                    }

                    if (selectedObj != null)
                    {
                        // Get the file name
                        result = selectedObj.ParsingName;
                    }
                }

                tcs.SetResult(result);
            }));

            return tcs.Task; ;
        }
    }
}
