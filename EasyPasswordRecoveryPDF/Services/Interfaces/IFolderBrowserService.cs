using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPasswordRecoveryPDF.Services.Interfaces
{
    public interface IFolderBrowserService
    {
        Task<string> ShowFolderBrowserDialogAsync(string title);
    }
}
