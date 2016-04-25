using System;
using EasyPasswordRecoveryPDF.Common;
using System.Security.Cryptography;

namespace EasyPasswordRecoveryPDF.Pdf.Decryption
{
    public class RC4DecryptorBase
    {
        #region [ Defines ]

        /// <summary>
        /// Bytes used for RC4 encryption. 
        /// </summary>
        protected readonly ArrayMath arrayMath = null;
        protected readonly byte[] state = null;
        protected readonly byte[] userKey = null;
        // Split permission into 4 bytes
        protected readonly byte[] permission = null;
        protected readonly MD5 md5 = null;
        protected readonly byte[] mkey = null;
        protected readonly byte[] digest = null;

        /// <summary>
        /// Pads a password to a 32 byte array.
        /// 32 bytes password padding defined by Adobe
        /// </summary>
        protected static readonly byte[] PasswordPadding =
        {
              0x28, 0xBF, 0x4E, 0x5E, 0x4E, 0x75, 0x8A, 0x41, 0x64, 0x00, 0x4E, 0x56, 0xFF, 0xFA, 0x01, 0x08,
              0x2E, 0x2E, 0x00, 0xB6, 0xD0, 0x68, 0x3E, 0x80, 0x2F, 0x0C, 0xA9, 0xFE, 0x64, 0x53, 0x69, 0x7A,
        };

        protected static readonly byte[] metadataPad = { 0xFF, 0xFF, 0xFF, 0xFF };

        #endregion

        #region [ Methods ] 

        protected static byte[] PadPassword(byte[] password)
        {
            byte[] padded = new byte[Constants.MaxPasswordSize];

            if (password.Length > 0)
            {
                Array.Copy(password, 0, padded,
                    0, Math.Min(password.Length, Constants.MaxPasswordSize));
                if (password.Length < Constants.MaxPasswordSize)
                {
                    Array.Copy(PasswordPadding, 0, padded, password.Length, Constants.MaxPasswordSize - password.Length);
                }
            }
            else
            {
                Array.Copy(PasswordPadding, 0, padded, 0, Constants.MaxPasswordSize);
            }

            return padded;
        }

        protected RC4DecryptorBase(EncryptionRecord encryptionInfo)
        {
            state = new byte[256];
            userKey = new byte[32];
            permission = new byte[4];
            arrayMath = new ArrayMath();
            md5 = new MD5CryptoServiceProvider();
            mkey = new byte[encryptionInfo.keyLength / 8];
            digest = new byte[encryptionInfo.keyLength / 8];
            permission[0] = (byte)encryptionInfo.pValue;
            permission[1] = (byte)(encryptionInfo.pValue >> 8);
            permission[2] = (byte)(encryptionInfo.pValue >> 16);
            permission[3] = (byte)(encryptionInfo.pValue >> 24);
        }

        /// <summary>
        /// Prepare the encryption key.
        /// </summary>
        protected void PrepareRC4Key(byte[] key, int offset, int length)
        {
            int idx1 = 0;
            int idx2 = 0;
            for (int idx = 0; idx < 256; idx++)
                state[idx] = (byte)idx;
            byte tmp;
            for (int idx = 0; idx < 256; idx++)
            {
                idx2 = (key[idx1 + offset] + state[idx] + idx2) & 255;
                tmp = state[idx];
                state[idx] = state[idx2];
                state[idx2] = tmp;
                idx1 = (idx1 + 1) % length;
            }
        }

        /// <summary>
        /// Encrypts the data.
        /// </summary>
        protected void EncryptRC4(byte[] inputData, int offset, int length, byte[] outputData)
        {
            length += offset;
            int x = 0, y = 0;
            byte b;
            for (int idx = offset; idx < length; idx++)
            {
                x = (x + 1) & 255;
                y = (state[x] + y) & 255;
                b = state[x];
                state[x] = state[y];
                state[y] = b;
                outputData[idx] = (byte)(inputData[idx] ^ state[(state[x] + state[y]) & 255]);
            }
        }

        #endregion 
    }
}
