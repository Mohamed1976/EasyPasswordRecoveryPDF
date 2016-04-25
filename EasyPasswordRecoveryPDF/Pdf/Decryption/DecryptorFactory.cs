using System;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;

namespace EasyPasswordRecoveryPDF.Pdf.Decryption
{
    public class DecryptorFactory
    {
        #region [ Methods ]

        public static IDecryptor Get(EncryptionRecord encryptionInfo)
        {
            switch (encryptionInfo.encryptionType)
            {
                case PdfEncryptionType.StandardEncryption40Bit:
                    return new RC4Decryptor40Bit(encryptionInfo);
                case PdfEncryptionType.StandardEncryption128Bit:
                case PdfEncryptionType.AesEncryption128Bit:
                    return new RC4Decryptor128Bit(encryptionInfo);
                case PdfEncryptionType.AesEncryption256Bit:
                    return new AESDecryptor256Bit(encryptionInfo);
                case PdfEncryptionType.AesIsoEncryption256Bit :
                    return new AESISODecryptor256Bit(encryptionInfo);
                default:
                    throw new Exception("Unsupported encryption type.");
            }
        }

        #endregion
    }
}
