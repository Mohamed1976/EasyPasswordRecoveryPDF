using System;
using System.Text;
using System.Security.Cryptography;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;

namespace EasyPasswordRecoveryPDF.Pdf.Decryption
{
    public class AESDecryptor256Bit : IDecryptor
    {
        #region [ Defines ] 

        private readonly ArrayMath arrayMath = null;
        private readonly SHA256CryptoServiceProvider sha256 = null;

        #endregion

        #region [ Constructor ]

        public AESDecryptor256Bit(EncryptionRecord encryptionInfo)
        {
            EncryptionInfo = encryptionInfo;
            sha256 = new SHA256CryptoServiceProvider();
            arrayMath = new ArrayMath();
        }

        #endregion

        #region [ Properties ]

        public EncryptionRecord EncryptionInfo
        {
            get;
            private set;
        }

        #endregion

        #region [ IDecryptor ]

        public PasswordValidity ValidatePassword(string password, ValidationMode mode)
        {
            PasswordValidity isValid = PasswordValidity.Invalid;

            if ((mode & ValidationMode.ValidateUserPassword) == ValidationMode.ValidateUserPassword &&
                ValidateUserPassword(password) != PasswordValidity.Invalid)
            {
                isValid |= PasswordValidity.UserPasswordIsValid;
            }

            if ((mode & ValidationMode.ValidateOwnerPassword) == ValidationMode.ValidateOwnerPassword &&
                ValidateOwnerPassword(password) != PasswordValidity.Invalid)
            {
                isValid |= PasswordValidity.OwnerPasswordIsValid;
            }

            return isValid;
        }

        #endregion

        #region [ Methods ]

        private PasswordValidity ValidateUserPassword(string userPassword)
        {
            byte[] paddedPassword = null, password = null;

            password = Encoding.UTF8.GetBytes(userPassword);
            Array.Resize(ref password, Math.Min(password.Length, Constants.MaxPasswordSizeV2));
            paddedPassword = new byte[password.Length + Constants.SaltLength];
            Array.Copy(password, 0, paddedPassword, 0, password.Length);
            Array.Copy(EncryptionInfo.uValue, Constants.SaltOffset, 
                paddedPassword, password.Length, Constants.SaltLength);
            byte [] hash = ValidatePassword(paddedPassword);

            if (arrayMath.ArraysAreEqual(hash, EncryptionInfo.uValue))
                return PasswordValidity.UserPasswordIsValid;
            else
                return PasswordValidity.Invalid;
        }

        private PasswordValidity ValidateOwnerPassword(string ownerPassword)
        {
            byte[] paddedPassword = null, password = null;

            password = Encoding.UTF8.GetBytes(ownerPassword);
            Array.Resize(ref password, Math.Min(password.Length, Constants.MaxPasswordSizeV2));
            paddedPassword = new byte[password.Length + Constants.SaltLength + 48];
            Array.Copy(password, 0, paddedPassword, 0, password.Length);
            Array.Copy(EncryptionInfo.oValue, Constants.SaltOffset, 
                paddedPassword, password.Length, Constants.SaltLength);
            Array.Copy(EncryptionInfo.uValue, 0, paddedPassword, 
                password.Length + Constants.SaltLength, 48);
            byte[] hash = ValidatePassword(paddedPassword);

            if (arrayMath.ArraysAreEqual(hash, EncryptionInfo.oValue))
                return PasswordValidity.OwnerPasswordIsValid;
            else
                return PasswordValidity.Invalid;
        }

        private byte[] ValidatePassword(byte[] paddedPassword)
        {
            sha256.Initialize();
            return sha256.ComputeHash(paddedPassword);
        }

        #endregion
    }
}
