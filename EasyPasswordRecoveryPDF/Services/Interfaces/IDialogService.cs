using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPasswordRecoveryPDF.Services.Interfaces
{
    public interface IDialogService
    {
        Task<bool?> InitDialog(object view, object viewModel);
    }
}
