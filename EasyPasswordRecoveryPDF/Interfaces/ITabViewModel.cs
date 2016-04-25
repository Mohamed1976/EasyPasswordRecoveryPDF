using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using EasyPasswordRecoveryPDF.Common;

namespace EasyPasswordRecoveryPDF.Interfaces
{
    public interface ITabViewModel
    {
        string Header { get; set; }
        DrawingBrush HeaderIcon { get; set; }
        IPasswordIterator PasswordIterator { get; set; }
    }
}
