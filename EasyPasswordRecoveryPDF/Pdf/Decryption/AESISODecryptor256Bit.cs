using System;
using System.Text;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;
using System.Security.Cryptography;
using System.Numerics;
using System.Linq;

namespace EasyPasswordRecoveryPDF.Pdf.Decryption
{
    public class AESISODecryptor256Bit : IDecryptor
    {
        #region [ Defines ]

        private readonly ArrayMath arrayMath = null;
        private readonly SHA256CryptoServiceProvider sha256 = null;
        private readonly SHA384CryptoServiceProvider sha384 = null;
        private readonly SHA512CryptoServiceProvider sha512 = null;
        private readonly AesCryptoServiceProvider aes = null;

        #endregion

        #region [ Constructor ]

        public AESISODecryptor256Bit(EncryptionRecord encryptionInfo) : base()
        {
            EncryptionInfo = encryptionInfo;
            sha256 = new SHA256CryptoServiceProvider();
            sha384 = new SHA384CryptoServiceProvider();
            sha512 = new SHA512CryptoServiceProvider();
            arrayMath = new ArrayMath();
            aes = new AesCryptoServiceProvider()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                BlockSize = 128,
                KeySize = 128
            };
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
            byte[] paddedPassword = null, password = null;

            password = Encoding.UTF8.GetBytes(userPassword);
            Array.Resize(ref password, Math.Min(password.Length, Constants.MaxPasswordSizeV2));
            paddedPassword = new byte[password.Length + Constants.SaltLength];
            Array.Copy(password, 0, paddedPassword, 0, password.Length);
            Array.Copy(EncryptionInfo.uValue, Constants.SaltOffset, paddedPassword, 
                password.Length, Constants.SaltLength);
            byte[] hash = ValidatePassword(paddedPassword, password, new byte[0]);

            if (arrayMath.ArraysAreEqual(hash, EncryptionInfo.uValue, Constants.CompareSize))
                return PasswordValidity.UserPasswordIsValid;
            else
                return PasswordValidity.Invalid;
        }

        private PasswordValidity ValidateOwnerPassword(string ownerPassword)
        {
            byte[] paddedPassword = null, password = null;

            password = Encoding.UTF8.GetBytes(ownerPassword);
            Array.Resize(ref password, Math.Min(password.Length, Constants.MaxPasswordSizeV2));
            paddedPassword = new byte[password.Length + 56];
            Array.Copy(password, 0, paddedPassword, 0, password.Length);
            Array.Copy(EncryptionInfo.oValue, Constants.SaltOffset, 
                paddedPassword, password.Length, Constants.SaltLength);
            Array.Copy(EncryptionInfo.uValue, 0, paddedPassword, 
                password.Length + Constants.SaltLength, 48);
            byte[] hash = ValidatePassword(paddedPassword, password, EncryptionInfo.uValue);

            if (arrayMath.ArraysAreEqual(hash, EncryptionInfo.oValue, Constants.CompareSize))
                return PasswordValidity.OwnerPasswordIsValid;
            else
                return PasswordValidity.Invalid;
        }

        private byte[] ValidatePassword(byte[] paddedPassword, byte[] password, byte [] uValue)
        {
            byte[] key = new byte[Constants.KeySize];
            byte[] iv = new byte[Constants.VectorSize];
            byte[] E16 = new byte[16];
            byte[] array = null;
            byte[] E = null;
            byte[] K1 = null;
            int idx = 0;

            //Take the SHA - 256 hash of the original input to the algorithm 
            //and name the resulting 32 bytes, K.
            byte[] K = sha256.ComputeHash(paddedPassword, 0, paddedPassword.Length);

            //The conditional-OR operator (||) performs a logical - OR 
            //of its bool operands. If the first operand evaluates to true, 
            //the second operand isn't evaluated. If the first operand evaluates 
            //to false, the second operator determines whether the OR expression 
            //as a whole evaluates to true or false.
            while (idx < 64 || E[E.Length - 1] + 32 > idx)
            {
                Array.Copy(K, key, key.Length);
                Array.Copy(K, 16, iv, 0, iv.Length);

                //Make a new string, K1, consisting of 64 repetitions of the sequence: 
                //input password, K, the 48 - byte user key. The 48 byte user key is 
                //only used when checking the owner password or creating the owner key.If
                //checking the user password or creating the user key, K1 is the 
                //concatenation of the input password and K.
                K1 = new byte[(password.Length + K.Length + uValue.Length) * 64];
                array = arrayMath.ConcatByteArrays(password, K);
                array = arrayMath.ConcatByteArrays(array, uValue);

                for (int j = 0, pos = 0; j < 64; j++, pos += array.Length)
                {
                    Array.Copy(array, 0, K1, pos, array.Length);
                }

                //Encrypt K1 with the AES-128(CBC, no padding) algorithm, 
                //using the first 16 bytes of K as the key and the second 16 bytes of 
                //K as the initialization vector. The result of this encryption is E.
                E = aes.CreateEncryptor(key, iv).TransformFinalBlock(K1, 0, K1.Length);
                //Now we have to take the first 16 bytes of an unsigned big endian integer... 
                //and compute the remainder modulo 3. Taking the first 16 bytes of E 
                //as an unsigned big-endian integer, compute the remainder, modulo 3.
                Array.Copy(E, E16, E16.Length);
                BigInteger bigInteger = new BigInteger(E16.Reverse().Concat(new byte[] { 0x00 }).ToArray());
                byte[] result = BigInteger.Remainder(bigInteger, 3).ToByteArray();

                switch (result[0])
                {
                    case 0x00:
                        K = sha256.ComputeHash(E, 0, E.Length);
                        break;
                    case 0x01:
                        K = sha384.ComputeHash(E, 0, E.Length);
                        break;
                    case 0x02:
                        K = sha512.ComputeHash(E, 0, E.Length);
                        break;
                    default:
                        throw new Exception("Unexpected result while computing the remainder, modulo 3.");
                }
                idx++;
            }
            return K;
        }
        #endregion
    }
}
