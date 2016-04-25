using EasyPasswordRecoveryPDF.Common;

namespace EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces
{
    public interface IDecryptor
    {
        PasswordValidity ValidatePassword(string password, ValidationMode mode);
    }
}
