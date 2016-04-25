using System;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;

namespace EasyPasswordRecoveryPDF.Pdf.Decryption
{
    public class RC4Decryptor128Bit : RC4DecryptorBase, IDecryptor
    {
        #region [ Constructor ] 

        public RC4Decryptor128Bit(EncryptionRecord encryptionInfo) : base(encryptionInfo)
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
            byte[] paddedPassword = PadPassword(ownerPassword.ToLatin1Bytes());
            md5.Initialize();
            byte[] computedHash = md5.ComputeHash(paddedPassword);

            for (int idx = 0; idx < 50; idx++)
                computedHash = md5.ComputeHash(computedHash);

            Array.Copy(EncryptionInfo.oValue, 0, paddedPassword, 0, paddedPassword.Length);

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < mkey.Length; ++j)
                    mkey[j] = (byte)(computedHash[j] ^ i);
                PrepareRC4Key(mkey, 0, mkey.Length);
                EncryptRC4(paddedPassword, 0, paddedPassword.Length, paddedPassword);
            }

            return ValidatePassword(paddedPassword) == true ? 
                PasswordValidity.OwnerPasswordIsValid : PasswordValidity.Invalid;
        }

        private bool ValidatePassword(byte[] paddedPassword)
        {
            md5.Initialize();
            md5.TransformBlock(paddedPassword, 0, paddedPassword.Length, paddedPassword, 0);
            md5.TransformBlock(EncryptionInfo.oValue, 0, EncryptionInfo.oValue.Length,
                EncryptionInfo.oValue, 0);
            md5.TransformBlock(permission, 0, permission.Length, permission, 0);
            md5.TransformBlock(EncryptionInfo.documentID, 0,
                EncryptionInfo.documentID.Length, EncryptionInfo.documentID, 0);
            if (!EncryptionInfo.metadataIsEncrypted)
                md5.TransformBlock(metadataPad, 0, metadataPad.Length, metadataPad, 0);
            md5.TransformFinalBlock(permission, 0, 0);

            Array.Copy(md5.Hash, 0, digest, 0, digest.Length);
            md5.Initialize();
            for (int k = 0; k < 50; ++k)
            {
                Array.Copy(md5.ComputeHash(digest), 0, digest, 0, digest.Length);
                md5.Initialize();
            }

            Array.Copy(digest, 0, mkey, 0, mkey.Length);
            md5.TransformBlock(PasswordPadding, 0, PasswordPadding.Length, PasswordPadding, 0);
            md5.TransformFinalBlock(EncryptionInfo.documentID, 0, EncryptionInfo.documentID.Length);

            byte[] computedHash = md5.Hash;

            Array.Copy(computedHash, 0, userKey, 0, 16);
            for (int idx = 16; idx < 32; idx++)
                userKey[idx] = 0;

            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < mkey.Length; ++j)
                    computedHash[j] = (byte)(mkey[j] ^ i);

                PrepareRC4Key(computedHash, 0, mkey.Length);
                EncryptRC4(userKey, 0, 16, userKey);
            }

            if (arrayMath.ArraysAreEqual(userKey, EncryptionInfo.uValue))
                return true;
            else
                return false;
        }

        #endregion
    }
}
