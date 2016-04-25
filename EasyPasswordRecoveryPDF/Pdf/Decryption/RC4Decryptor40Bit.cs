using System;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;

namespace EasyPasswordRecoveryPDF.Pdf.Decryption
{
    public class RC4Decryptor40Bit : RC4DecryptorBase, IDecryptor
    {
        #region [ Constructor ]

        public RC4Decryptor40Bit(EncryptionRecord encryptionInfo) : base(encryptionInfo)
        {
            EncryptionInfo = encryptionInfo;
        }

        #endregion

        #region [ Properties ]

        public EncryptionRecord EncryptionInfo
        {
            get;
            private set;
        }

        #endregion

        #region [ Methods ]

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

        private PasswordValidity ValidateUserPassword(string userPassword)
        {
            byte[] paddedPassword = PadPassword(userPassword.ToLatin1Bytes());
            return ValidatePassword(paddedPassword) == true ?
                PasswordValidity.UserPasswordIsValid : PasswordValidity.Invalid;
        }

        private PasswordValidity ValidateOwnerPassword(string ownerPassword)
        {
            md5.Initialize();
            byte[] computedHash = md5.ComputeHash(PadPassword(ownerPassword.ToLatin1Bytes()));
            PrepareRC4Key(computedHash, 0, 5);
            EncryptRC4(EncryptionInfo.oValue, 0,
                EncryptionInfo.oValue.Length, userKey);
            return ValidatePassword(userKey) == true ?
                PasswordValidity.OwnerPasswordIsValid : PasswordValidity.Invalid;
        }

        private bool ValidatePassword(byte [] paddedPassword)
        {
            md5.Initialize();
            md5.TransformBlock(paddedPassword, 0, paddedPassword.Length, paddedPassword, 0);
            md5.TransformBlock(EncryptionInfo.oValue, 0, EncryptionInfo.oValue.Length,
                EncryptionInfo.oValue, 0);
            md5.TransformBlock(permission, 0, permission.Length, permission, 0);
            md5.TransformBlock(EncryptionInfo.documentID, 0,
                EncryptionInfo.documentID.Length, EncryptionInfo.documentID, 0);
            md5.TransformFinalBlock(permission, 0, 0);

            byte[] computedHash = md5.Hash;
            md5.Initialize();
            Array.Copy(computedHash, 0, mkey, 0, mkey.Length);
            PrepareRC4Key(mkey, 0, mkey.Length);
            EncryptRC4(PasswordPadding, 0, PasswordPadding.Length, userKey);

            if (arrayMath.ArraysAreEqual(userKey, EncryptionInfo.uValue))
                return true;
            else
                return false;
        }

        #endregion
    }
}
