using System.IO;
using NUnit.Framework;
using UnitTest.Common;
using EasyPasswordRecoveryPDF.Model;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Pdf.Decryption;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;
using System.Linq;

namespace UnitTest.UnitTests
{
    public class UnitTest
    {
        [SetUp]
        public void Initialize()
        {
        }
        [TearDown]
        public void CleanUp()
        {
        }

        [Test]
        [TestCase("testcase001_rc40_uo_set.pdf", true)]
        [TestCase("testcase002_rc40_uo_set.pdf", true)]
        [TestCase("testcase003_rc40_o_set.pdf", true)]
        [TestCase("testcase004_rc40.pdf", true)]
        [TestCase("testcase005_rc128_uo_set.pdf", true)]
        [TestCase("testcase006_rc128_uo_set.pdf", true)]
        [TestCase("testcase007_rc128_o_set.pdf", true)]
        [TestCase("testcase008_rc128.pdf", true)]
        [TestCase("testcase009_aes128_uo_set.pdf", true)]
        [TestCase("testcase010_aes128_uo_set.pdf", true)]
        [TestCase("testcase011_aes128_o_set.pdf", true)]
        [TestCase("testcase012_aes128.pdf", true)]
        [TestCase("testcase013_aes128_uo_set.pdf", true)]
        [TestCase("testcase014_aes256_uo_set.pdf", true)]
        [TestCase("testcase015_aes256_uo_set.pdf", true)]
        [TestCase("testcase016_aes256_o_set.pdf", true)]
        [TestCase("testcase017_aes256.pdf", true)]
        [TestCase("testcase018_aes256iso_uo_set.pdf", true)]
        [TestCase("testcase019_aes256iso_uo_set.pdf", true)]
        [TestCase("testcase020_aes256iso_o_set.pdf", true)]
        [TestCase("testcase021_aes256iso.pdf", true)]
        [TestCase("yearly-sales-report.pdf", true)]
        [TestCase("ProtectDocument_output.pdf", true)]
        [TestCase("testcase.protected001.pdf", true)]
        [TestCase("testcase.protected002.pdf", true)]
        [TestCase("testcase.protected003.pdf", true)]
        [TestCase("0cifk-1hhnv.pdf", true)]
        [TestCase("testcase.protected004.pdf", true)]
        [TestCase("fact-sheet-aes-256bit-userpw.pdf", true)]
        [TestCase("fact-sheet-aes-128bit-userpw.pdf", true)]
        [TestCase("fact-sheet-rc-128bit-userpw.pdf", true)]
        [TestCase("fact-sheet-rc-40bit-userpw.pdf", true)]
        [TestCase("laboratory-report-aes-256bit-no-userpw.pdf", true)]
        [TestCase("unicodeTestcase.pdf", true)]
        public void EncryptionDictionary(string PDF, bool hasEncryptionDictionary)
        {
            string errorMsg = string.Empty;

            string filePath = Path.Combine(TestHelper.DataFolder, PDF);
            Assert.IsTrue(File.Exists(filePath));
            PdfFile pdfFile = new PdfFile(filePath);
            pdfFile.Open(ref errorMsg);
            Assert.IsTrue(string.IsNullOrEmpty(errorMsg));
            Assert.AreEqual(pdfFile.IsEncrypted, hasEncryptionDictionary);
        }

        [Test]
        [TestCase("testcase001_rc40_uo_set.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase001_rc40_uo_set.pdf", "UserPwd", PasswordValidity.Invalid, "OwnerPwd", PasswordValidity.Invalid)]
        [TestCase("testcase002_rc40_uo_set.pdf", "abcdefghijklmnopqrstuvwxyz1234567", PasswordValidity.UserPasswordIsValid, "abcdefghijklmnopqrstuvwxyz1234567", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase002_rc40_uo_set.pdf", "abcdefghijklmnopqrstuvwxyz123456", PasswordValidity.UserPasswordIsValid, "abcdefghijklmnopqrstuvwxyz123456", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase002_rc40_uo_set.pdf", "abcdefghijklmnopqrstuvwxyz12345", PasswordValidity.Invalid, "abcdefghijklmnopqrstuvwxyz12345", PasswordValidity.Invalid)]
        [TestCase("testcase003_rc40_o_set.pdf", "", PasswordValidity.UserPasswordIsValid, "abcdefghijklmnopqrstuvwxyz123456", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase004_rc40.pdf", "", PasswordValidity.UserPasswordIsValid, "", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase005_rc128_uo_set.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase005_rc128_uo_set.pdf", "UserPwd", PasswordValidity.Invalid, "OwnerPwd", PasswordValidity.Invalid)]
        [TestCase("testcase006_rc128_uo_set.pdf", "@#$%&\\()*+,-qwerqdfqrstuvwxyz1234", PasswordValidity.UserPasswordIsValid, "@#$%&\\()*+,-qwerqdfqrstuvwxyz1234", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase006_rc128_uo_set.pdf", "@#$%&\\()*+,-qwerqdfqrstuvwxyz123", PasswordValidity.UserPasswordIsValid, "@#$%&\\()*+,-qwerqdfqrstuvwxyz123", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase006_rc128_uo_set.pdf", "@#$%&\\()*+,-qwerqdfqrstuvwxyz12", PasswordValidity.Invalid, "@#$%&\\()*+,-qwerqdfqrstuvwxyz12", PasswordValidity.Invalid)]
        [TestCase("testcase007_rc128_o_set.pdf", "", PasswordValidity.UserPasswordIsValid, "@#$%&\\()*+,-/. rqdfqr,;:^=`|{}~^", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase008_rc128.pdf", "", PasswordValidity.UserPasswordIsValid, "", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase009_aes128_uo_set.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase009_aes128_uo_set.pdf", "UserPwd", PasswordValidity.Invalid, "OwnerPwd", PasswordValidity.Invalid)]
        [TestCase("testcase010_aes128_uo_set.pdf", "ABCDEFghijklmnopqrstuvwxyz1234567", PasswordValidity.UserPasswordIsValid, "ABCDEFghijklmnopqrstuvwxyz1234567", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase010_aes128_uo_set.pdf", "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.UserPasswordIsValid, "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase010_aes128_uo_set.pdf", "ABCDEFghijklmnopqrstuvwxyz12345", PasswordValidity.Invalid, "ABCDEFghijklmnopqrstuvwxyz12345", PasswordValidity.Invalid)]
        [TestCase("testcase011_aes128_o_set.pdf", "", PasswordValidity.UserPasswordIsValid, "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase012_aes128.pdf", "", PasswordValidity.UserPasswordIsValid, "", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase013_aes128_uo_set.pdf", "!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~", PasswordValidity.UserPasswordIsValid, "!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase014_aes256_uo_set.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase014_aes256_uo_set.pdf", "UserPwd", PasswordValidity.Invalid, "OwnerPwd", PasswordValidity.Invalid)]
        [TestCase("testcase015_aes256_uo_set.pdf",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCD",
            PasswordValidity.UserPasswordIsValid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCD", 
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase015_aes256_uo_set.pdf",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABC",
            PasswordValidity.UserPasswordIsValid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABC",
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase015_aes256_uo_set.pdf",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789AB",
            PasswordValidity.Invalid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789AB",
            PasswordValidity.Invalid)]
        [TestCase("testcase016_aes256_o_set.pdf", "", PasswordValidity.UserPasswordIsValid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABC",
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase017_aes256.pdf", "", PasswordValidity.UserPasswordIsValid, "", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase018_aes256iso_uo_set.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase018_aes256iso_uo_set.pdf", "UserPwd", PasswordValidity.Invalid, "OwnerPwd", PasswordValidity.Invalid)]
        [TestCase("testcase019_aes256iso_uo_set.pdf",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCD",
            PasswordValidity.UserPasswordIsValid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCD",
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase019_aes256iso_uo_set.pdf",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABC",
            PasswordValidity.UserPasswordIsValid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABC",
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase019_aes256iso_uo_set.pdf",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789AB",
            PasswordValidity.Invalid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789AB",
            PasswordValidity.Invalid)]
        [TestCase("testcase020_aes256iso_o_set.pdf", "", PasswordValidity.UserPasswordIsValid,
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABC",
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase021_aes256iso.pdf", "", PasswordValidity.UserPasswordIsValid, "", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("yearly-sales-report.pdf", "sample", PasswordValidity.UserPasswordIsValid, "", PasswordValidity.Invalid)]
        [TestCase("ProtectDocument_output.pdf", "user", PasswordValidity.UserPasswordIsValid, "owner", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase.protected001.pdf", "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.UserPasswordIsValid, "ABCDEFghijklmnopqrstuvwxyz654321", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase.protected002.pdf", "ABCDEFghijklmnopqrstuvwxyz654321", PasswordValidity.UserPasswordIsValid, "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase.protected003.pdf", "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.UserPasswordIsValid, "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("0cifk-1hhnv.pdf", "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.UserPasswordIsValid, "ABCDEFghijklmnopqrstuvwxyz654321", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase.protected004.pdf", "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.UserPasswordIsValid, "ABCDEFghijklmnopqrstuvwxyz123456", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-aes-256bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-aes-128bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-rc-128bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-rc-40bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-aes-256bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("unicodeTestcase.pdf",
            "ΣΣΣΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶@ΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶",
            PasswordValidity.UserPasswordIsValid,
            "ΣΣΣΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶@ΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶",      
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("unicodeTestcase.pdf",
            "ΣΣΣΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶@",
            PasswordValidity.UserPasswordIsValid,
            "ΣΣΣΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶@",
            PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("unicodeTestcase.pdf",
            "ΣΣΣΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶",
            PasswordValidity.Invalid,
            "ΣΣΣΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθ㕯㕰㕱㕲㕳㕴㕵㕶",
            PasswordValidity.Invalid)]
        public void Security(string PDF,
            string userPassword,
            PasswordValidity userPasswordValidity,
            string ownerPassword,
            PasswordValidity ownerPasswordValidity)
        {
            string errorMsg = string.Empty;

            string filePath = Path.Combine(TestHelper.DataFolder, PDF);
            Assert.IsTrue(File.Exists(filePath));
            PdfFile pdfFile = new PdfFile(filePath);
            pdfFile.Open(ref errorMsg);
            Assert.IsTrue(string.IsNullOrEmpty(errorMsg));
            IDecryptor pdfDecryptor = DecryptorFactory.Get(pdfFile.EncryptionRecordInfo);

            PasswordValidity passwordValidity = 
                pdfDecryptor.ValidatePassword(userPassword, ValidationMode.ValidateUserPassword);
            Assert.AreEqual(passwordValidity, userPasswordValidity);

            passwordValidity = pdfDecryptor.ValidatePassword(ownerPassword, ValidationMode.ValidateOwnerPassword);
            Assert.AreEqual(passwordValidity, ownerPasswordValidity);
        }

        [Test]
        [TestCase("testcase001_rc40_uo_set.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase005_rc128_uo_set.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase009_aes128_uo_set.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase014_aes256_uo_set.pdf", PdfEncryptionType.AesEncryption256Bit)]
        [TestCase("testcase018_aes256iso_uo_set.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("yearly-sales-report.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("ProtectDocument_output.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase.protected001.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase.protected002.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase.protected003.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("0cifk-1hhnv.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase.protected004.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("fact-sheet-aes-256bit-userpw.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("fact-sheet-aes-128bit-userpw.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("fact-sheet-rc-128bit-userpw.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("fact-sheet-rc-40bit-userpw.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("laboratory-report-aes-256bit-no-userpw.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("unicodeTestcase.pdf", PdfEncryptionType.AesEncryption256Bit)]
        public void EncryptionType(string PDF, 
            PdfEncryptionType pdfEncryptionType)
        {
            string errorMsg = string.Empty;

            string filePath = Path.Combine(TestHelper.DataFolder, PDF);
            Assert.IsTrue(File.Exists(filePath));
            PdfFile pdfFile = new PdfFile(filePath);
            pdfFile.Open(ref errorMsg);
            Assert.IsTrue(string.IsNullOrEmpty(errorMsg));
            Assert.AreEqual(pdfFile.EncryptionRecordInfo.encryptionType, pdfEncryptionType);
        }
    }
}
