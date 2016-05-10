using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPasswordRecoveryPDF.Common
{
    [Flags]
    public enum Charset
    {
        None = 0,
        UpperCaseLetters = 1,
        LowerCaseLetters = 2,
        PunctuationMarks = 4,
        Digits = 8,
        Space = 16,
        All = UpperCaseLetters | LowerCaseLetters | PunctuationMarks | Digits | Space
    }

    [Serializable]
    public enum CharsetCasing
    {
        None = 0,
        LowerCase,
        UpperCase,
        TitleCase
    }

    public enum SaveOptions
    {
        UseSourceFolder = 0,
        UseCustomFolder
    }

    public enum CharsetEncoding
    {
        None,
        Ascii,
        Unicode,
    }

    public enum AvailableViews
    {
        None = 0,
        Home,
        Settings,
        About
    }

    [Flags]
    public enum PasswordValidity
    {
        Invalid = 0,
        UserPasswordIsValid = 1,
        OwnerPasswordIsValid = 2
    }

    [Flags]
    public enum ValidationMode
    {
        None = 0,
        ValidateUserPassword = 1,
        ValidateOwnerPassword = 2
    }

    public enum PdfEncryptionType
    {
        None = 0,
        StandardEncryption40Bit,
        StandardEncryption128Bit,
        AesEncryption128Bit,
        AesEncryption256Bit,
        AesIsoEncryption256Bit
    }

    public struct EncryptionRecord
    {
        public bool isEncrypted;
        public char pdfVersion;
        public long fileLength;
        public byte[] documentID;
        public byte[] uValue;
        public byte[] oValue;
        public long pValue;
        public int rValue;
        public PdfEncryptionType encryptionType;
        public int keyLength;
        public bool metadataIsEncrypted;
        public CharsetEncoding PasswordCharset;
        public int MaxPasswordSize;
    }
}
