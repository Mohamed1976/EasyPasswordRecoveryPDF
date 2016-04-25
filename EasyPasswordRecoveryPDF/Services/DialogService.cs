using System;
using System.Windows;
using System.Threading.Tasks;
using EasyPasswordRecoveryPDF.Services.Interfaces;

namespace EasyPasswordRecoveryPDF.Services
{
    public class DialogService : IDialogService
    {
        private readonly IDispatcherService _dispatcherService;

        public DialogService(IDispatcherService service)
        {
            _dispatcherService = service;
        }

        public Task<bool?> InitDialog(object view, object viewModel)
        {
            var tcs = new TaskCompletionSource<bool?>();

            _dispatcherService.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                bool? result = null;
                Window activeWindow = null;
                for (var i = 0; i < Application.Current.Windows.Count; i++)
                {
                    var win = Application.Current.Windows[i];
                    if ((win != null) && (win.IsActive))
                    {
                        activeWindow = win;
                        break;
                    }
                }

                if (activeWindow != null)
                {
                    Window window = (Window)view;
                    window.DataContext = viewModel;
                    result = window.ShowDialog();
                }

                tcs.SetResult(result);
            }));

            return tcs.Task;
        }
    }
}
