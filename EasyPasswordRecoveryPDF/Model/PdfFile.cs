using System;
using System.IO;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;
using EasyPasswordRecoveryPDF.Pdf.Decryption;

namespace EasyPasswordRecoveryPDF.Model
{
    public class PdfFile : ModelBase
    {
        #region [ Constructor ]

        public PdfFile(string filename)
        {
            Info = new FileInfo(filename);
            EncryptionRecordInfo = new EncryptionRecord() { encryptionType = PdfEncryptionType.None };
        }

        #endregion

        #region [ Methods ]

        public int Open(ref string errorMsg)
        {
            int result = Constants.Failure;

            try
            {
                PdfInfoReader pdfInfoReader = new PdfInfoReader(Info.FullName);
                pdfInfoReader.GetEncryptionProperties(ref encryptionRecordInfo);
                IsEncrypted = EncryptionRecordInfo.encryptionType != PdfEncryptionType.None;
                if (IsEncrypted)
                {   //Check if PDF password is empty;
                    IDecryptor pdfDecryptor = DecryptorFactory.Get(EncryptionRecordInfo);
                    PasswordValidity passwordValidity = pdfDecryptor.ValidatePassword(string.Empty, 
                        ValidationMode.ValidateUserPassword | ValidationMode.ValidateOwnerPassword);
                    UserPasswordIsSet = 
                        (passwordValidity & PasswordValidity.UserPasswordIsValid) != PasswordValidity.UserPasswordIsValid;
                    OwnerPasswordIsSet = 
                        (passwordValidity & PasswordValidity.OwnerPasswordIsValid) != PasswordValidity.OwnerPasswordIsValid;

                    MaxPasswordSize = EncryptionRecordInfo.MaxPasswordSize;
                    PasswordCharset = EncryptionRecordInfo.PasswordCharset;
                }

                result = Constants.Success;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }

        public void SetPassword(string password, PasswordValidity passwordValidity)
        {
            if ((passwordValidity & PasswordValidity.OwnerPasswordIsValid) == PasswordValidity.OwnerPasswordIsValid)
            {
                RecoveredOwnerPassword = password;
            }

            if ((passwordValidity & PasswordValidity.UserPasswordIsValid) == PasswordValidity.UserPasswordIsValid)
            {
                RecoveredUserPassword = password;
            }
        }

        public Dictionary<string, string> GetPdfFileInfo()
        {
            Dictionary<string, string> PdfFileInfo = new Dictionary<string, string>();
            PdfFileInfo.Add("Name", Info.Name);
            PdfFileInfo.Add("DirectoryName", Info.DirectoryName);
            PdfFileInfo.Add("Length", Info.Length.ToString() + "(bytes)");
            PdfFileInfo.Add("IsReadOnly", Info.IsReadOnly.ToString());
            PdfFileInfo.Add("CreationTime", Info.CreationTime.ToString());
            PdfFileInfo.Add("LastAccessTime", Info.LastAccessTime.ToString());
            PdfFileInfo.Add("LastWriteTime", Info.LastWriteTime.ToString());
            return PdfFileInfo;
        }

        public Dictionary<string, string> GetEncryptionInfo()
        {
            string hexValue = string.Empty;
            Dictionary<string, string> EncryptionInfo = new Dictionary<string, string>();
            EncryptionInfo.Add("IsEncrypted", IsEncrypted.ToString());
            if (IsEncrypted)
            {
                EncryptionInfo.Add("pdfVersion", EncryptionRecordInfo.pdfVersion.ToString());
                EncryptionInfo.Add("rValue", EncryptionRecordInfo.rValue.ToString());
                EncryptionInfo.Add("encryptionType", EncryptionRecordInfo.encryptionType.ToString());
                EncryptionInfo.Add("keyLength", EncryptionRecordInfo.keyLength.ToString());
                EncryptionInfo.Add("metadataIsEncrypted", EncryptionRecordInfo.metadataIsEncrypted.ToString());
                EncryptionInfo.Add("Password charset", EncryptionRecordInfo.PasswordCharset.ToString());
                EncryptionInfo.Add("Max password size", EncryptionRecordInfo.MaxPasswordSize.ToString());
                EncryptionInfo.Add("User password is set", UserPasswordIsSet.ToString());
                if (UserPasswordIsSet)
                    EncryptionInfo.Add("User password found", RecoveredUserPassword);
                EncryptionInfo.Add("Owner password is set", OwnerPasswordIsSet.ToString());
                if (OwnerPasswordIsSet)
                    EncryptionInfo.Add("Owner password found", RecoveredOwnerPassword);

                hexValue = string.Empty;
                foreach (byte b in EncryptionRecordInfo.documentID)
                    hexValue += string.Format("{0:X02} ", b);
                EncryptionInfo.Add(string.Format("documentID({0} bytes)", EncryptionRecordInfo.documentID.Length), hexValue);
                hexValue = string.Empty;
                foreach (byte b in EncryptionRecordInfo.oValue)
                    hexValue += string.Format("{0:X02} ", b);
                EncryptionInfo.Add(string.Format("oValue({0} bytes)", EncryptionRecordInfo.oValue.Length), hexValue);
                hexValue = string.Empty;
                foreach (byte b in EncryptionRecordInfo.uValue)
                    hexValue += string.Format("{0:X02} ", b);
                EncryptionInfo.Add(string.Format("uValue({0} bytes)", EncryptionRecordInfo.uValue.Length), hexValue);

                EncryptionInfo.Add("pValue", EncryptionRecordInfo.pValue.ToString());
                EncryptionInfo.Add("Modify annotations",
                    PdfEncryptor.IsModifyAnnotationsAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
                EncryptionInfo.Add("Modify content",
                    PdfEncryptor.IsModifyContentsAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
                EncryptionInfo.Add("Printing",
                    PdfEncryptor.IsPrintingAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
                EncryptionInfo.Add("Screen readers",
                    PdfEncryptor.IsScreenReadersAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
                EncryptionInfo.Add("Degraded printing",
                    PdfEncryptor.IsDegradedPrintingAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
                EncryptionInfo.Add("Fill In",
                    PdfEncryptor.IsFillInAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
                EncryptionInfo.Add("Copy",
                    PdfEncryptor.IsCopyAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
                EncryptionInfo.Add("Assembly",
                    PdfEncryptor.IsAssemblyAllowed((int)EncryptionRecordInfo.pValue) ? "Allowed" : "Not allowed");
            }
            return EncryptionInfo;
        }

        #endregion

        #region [ Properties ]

        private bool encrypted = false;
        public bool IsEncrypted
        {
            get { return encrypted; }
            private set { SetProperty(ref this.encrypted, value); }
        }

        private bool userPasswordIsSet = false;
        public bool UserPasswordIsSet
        {
            get { return userPasswordIsSet; }
            private set { SetProperty(ref this.userPasswordIsSet, value); }
        }

        private bool ownerPasswordIsSet = false;
        public bool OwnerPasswordIsSet
        {
            get { return ownerPasswordIsSet; }
            private set { SetProperty(ref this.ownerPasswordIsSet, value); }
        }

        private string recoveredUserPassword = string.Empty;
        public string RecoveredUserPassword
        {
            get { return recoveredUserPassword; }
            private set { SetProperty(ref this.recoveredUserPassword, value); }
        }

        private string recoveredOwnerPassword = string.Empty;
        public string RecoveredOwnerPassword
        {
            get { return recoveredOwnerPassword; }
            private set { SetProperty(ref this.recoveredOwnerPassword, value); }
        }

        private int maxPasswordSize = 0;
        public int MaxPasswordSize
        {
            get { return maxPasswordSize; }
            private set { SetProperty(ref this.maxPasswordSize, value); }
        }

        private CharsetEncoding passwordCharset = CharsetEncoding.None;
        public CharsetEncoding PasswordCharset
        {
            get { return passwordCharset; }
            private set { SetProperty(ref this.passwordCharset, value); }
        }

        private FileInfo fileInfo = null;
        public FileInfo Info
        {
            get { return fileInfo; }
            private set { SetProperty(ref this.fileInfo, value); }
        }

        private EncryptionRecord encryptionRecordInfo;
        public EncryptionRecord EncryptionRecordInfo
        {
            get { return encryptionRecordInfo; }
            private set { SetProperty(ref this.encryptionRecordInfo, value); }
        }

        #endregion
    }
}
