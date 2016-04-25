using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPasswordRecoveryPDF.Common;

namespace EasyPasswordRecoveryPDF.Interfaces
{
    public interface IPasswordIterator
    {
        int Initialize(CharsetEncoding pdfPasswordEncoding, int maxPasswordLength, ref string errorMsg);
        CharsetCasing PasswordCasing { get; set; }
        bool RemoveWhitespace { get; set; }
        string GetNextPassword();
    }
}
