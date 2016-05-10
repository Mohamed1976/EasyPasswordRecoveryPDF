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
        [TestCase("laboratory-report-rc-40bit-no-userpw.pdf", true)]
        [TestCase("laboratory-report-rc-128bit-no-userpw.pdf", true)]
        [TestCase("laboratory-report-aes-128bit-no-userpw.pdf", true)]
        [TestCase("laboratory-report-aes-256bit-no-userpw.pdf", true)]
        [TestCase("laboratory-report-rc-40bit-userpw.pdf", true)]
        [TestCase("laboratory-report-rc-128bit-userpw.pdf", true)]
        [TestCase("laboratory-report-aes-128bit-userpw.pdf", true)]
        [TestCase("laboratory-report-aes-256bit-userpw.pdf", true)]
        [TestCase("fact-sheet-rc-40bit-no-userpw.pdf", true)]
        [TestCase("fact-sheet-rc-128bit-no-userpw.pdf", true)]
        [TestCase("fact-sheet-aes-128bit-no-userpw.pdf", true)]
        [TestCase("fact-sheet-aes-256bit-no-userpw.pdf", true)]
        [TestCase("fact-sheet-rc-40bit-userpw.pdf", true)]
        [TestCase("fact-sheet-rc-128bit-userpw.pdf", true)]
        [TestCase("fact-sheet-aes-128bit-userpw.pdf", true)]
        [TestCase("fact-sheet-aes-256bit-userpw.pdf", true)]
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
        [TestCase("unicodeTestcase.pdf", true)]
        [TestCase("testcase022_aes256iso_uo_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase023_aes256iso_uo_set_metadata_not_encrypted.pdf", true)]
        [TestCase("testcase024_aes256iso_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase025_aes256iso_o_set_metadata_not_encrypted.pdf", true)]
        [TestCase("testcase026_aes256_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase027_aes256_ou_set_metadata_not_encrypted.pdf", true)]
        [TestCase("testcase028_aes256_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase029_aes256_o_set_metadata_not_encrypted.pdf", true)]
        [TestCase("testcase030_aes128_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase031_aes128_ou_set_metadata_not_encrypted.pdf", true)]
        [TestCase("testcase032_aes128_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase033_aes128_o_set_metadata_not_encrypted.pdf", true)]
        [TestCase("testcase034_standard128_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase035_standard128_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase036_standard128.pdf", false)]
        [TestCase("testcase037_standard40_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase038_standard40_o_set_metadata_encrypted.pdf", true)]
        [TestCase("invoicesample.pdf", false)]
        [TestCase("pdf-sample.pdf", false)]
        [TestCase("invoice.pdf", false)]
        [TestCase("order-receipt.pdf", false)]
        [TestCase("OoPdfFormExample.pdf", false)]
        [TestCase("pdf-example-encryption.original_o_set.pdf", true)]
        [TestCase("pdf-example-password.original_uo_set.pdf", true)]
        [TestCase("Encryption.pdf", true)]
        [TestCase("pdf-example-watermarks.original.pdf", false)]
        [TestCase("pdf.pdf", false)]
        [TestCase("sales-quote.pdf", true)]
        [TestCase("testcase42.pdf", true)]
        [TestCase("testcase43.pdf", true)]
        [TestCase("testcase44.pdf", true)]
        [TestCase("testcase45.pdf", true)]
        [TestCase("testcase46.pdf", true)]
        [TestCase("testcase47.pdf", true)]
        [TestCase("testcase48.pdf", true)]
        [TestCase("testcase49.pdf", true)]
        [TestCase("testcase50.pdf", true)]
        [TestCase("testcase51.pdf", true)]
        [TestCase("testcase52.pdf", true)]
        [TestCase("testcase53.pdf", true)]
        [TestCase("testcase54.pdf", true)]
        [TestCase("testcase55.pdf", true)]
        [TestCase("testcase56.pdf", true)]
        [TestCase("testcase57.pdf", true)]
        [TestCase("testcase58.pdf", true)]
        [TestCase("testcase59.pdf", true)]
        [TestCase("testcase60.pdf", true)]
        [TestCase("testcase61.pdf", true)]
        [TestCase("testcase62.pdf", true)]
        [TestCase("testcase63.pdf", true)]
        [TestCase("testcase64.pdf", true)]
        [TestCase("testcase65.pdf", true)]
        [TestCase("testcase66.pdf", true)]
        [TestCase("testcase67.pdf", true)]
        [TestCase("testcase68.pdf", true)]
        [TestCase("testcase69.pdf", true)]
        [TestCase("testcase70.pdf", true)]
        [TestCase("testcase71.pdf", true)]
        [TestCase("testcase72.pdf", true)]
        [TestCase("testcase73.pdf", true)]
        [TestCase("testcase74.pdf", true)]
        [TestCase("testcase75.pdf", true)]
        [TestCase("testcase76.pdf", true)]
        [TestCase("testcase77.pdf", true)]
        public void EncryptionDictionary(string PDF, bool hasEncryptionDictionary)
        {
            string errorMsg = string.Empty;

            string filePath = Path.Combine(TestHelper.DataFolder, PDF);
            Assert.IsTrue(File.Exists(filePath));
            PdfFile pdfFile = new PdfFile(filePath);
            pdfFile.Open(ref errorMsg);
            Assert.IsTrue(string.IsNullOrEmpty(errorMsg));
            Assert.AreEqual(pdfFile.EncryptionRecordInfo.isEncrypted, hasEncryptionDictionary);
        }

        [Test]
        [TestCase("laboratory-report-rc-40bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-rc-128bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-aes-128bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-aes-256bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-rc-40bit-userpw.pdf", "uknowit", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-rc-128bit-userpw.pdf", "uknowit", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-aes-128bit-userpw.pdf", "uknowit", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("laboratory-report-aes-256bit-userpw.pdf", "uknowit", PasswordValidity.UserPasswordIsValid, "secret", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-rc-40bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-rc-128bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-aes-128bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-aes-256bit-no-userpw.pdf", "", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-rc-40bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-rc-128bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-aes-128bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("fact-sheet-aes-256bit-userpw.pdf", "spooky", PasswordValidity.UserPasswordIsValid, "magic", PasswordValidity.OwnerPasswordIsValid)]
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
        [TestCase("testcase022_aes256iso_uo_set_metadata_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase023_aes256iso_uo_set_metadata_not_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase024_aes256iso_o_set_metadata_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase025_aes256iso_o_set_metadata_not_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase026_aes256_ou_set_metadata_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase027_aes256_ou_set_metadata_not_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase028_aes256_o_set_metadata_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase029_aes256_o_set_metadata_not_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase030_aes128_ou_set_metadata_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase031_aes128_ou_set_metadata_not_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase032_aes128_o_set_metadata_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase033_aes128_o_set_metadata_not_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase034_standard128_ou_set_metadata_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase035_standard128_o_set_metadata_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase037_standard40_ou_set_metadata_encrypted.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase038_standard40_o_set_metadata_encrypted.pdf", "", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("pdf-example-password.original_uo_set.pdf", "test", PasswordValidity.UserPasswordIsValid, "test", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("pdf-example-encryption.original_o_set.pdf", "", PasswordValidity.UserPasswordIsValid, "test", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("Encryption.pdf", "userpass", PasswordValidity.UserPasswordIsValid, "ownerpass", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("sales-quote.pdf", "", PasswordValidity.UserPasswordIsValid, "Unknown?", PasswordValidity.Invalid)]
        [TestCase("testcase42.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase43.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase44.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase45.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase46.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase47.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase48.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase49.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase50.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase51.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase52.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase53.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase54.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase55.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase56.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase57.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase58.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase59.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase60.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase61.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase62.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase63.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase64.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase65.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase66.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase67.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase68.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase69.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase70.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase71.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase72.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase73.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase74.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase75.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase76.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
        [TestCase("testcase77.pdf", "userPwd", PasswordValidity.UserPasswordIsValid, "ownerPwd", PasswordValidity.OwnerPasswordIsValid)]
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
        [TestCase("laboratory-report-rc-40bit-no-userpw.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("laboratory-report-rc-128bit-no-userpw.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("laboratory-report-aes-128bit-no-userpw.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("laboratory-report-aes-256bit-no-userpw.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("laboratory-report-rc-40bit-userpw.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("laboratory-report-rc-128bit-userpw.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("laboratory-report-aes-128bit-userpw.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("laboratory-report-aes-256bit-userpw.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("fact-sheet-rc-40bit-no-userpw.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("fact-sheet-rc-128bit-no-userpw.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("fact-sheet-aes-128bit-no-userpw.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("fact-sheet-aes-256bit-no-userpw.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("fact-sheet-rc-40bit-userpw.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("fact-sheet-rc-128bit-userpw.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("fact-sheet-aes-128bit-userpw.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("fact-sheet-aes-256bit-userpw.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
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
        [TestCase("unicodeTestcase.pdf", PdfEncryptionType.AesEncryption256Bit)]
        [TestCase("testcase022_aes256iso_uo_set_metadata_encrypted.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("testcase023_aes256iso_uo_set_metadata_not_encrypted.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("testcase024_aes256iso_o_set_metadata_encrypted.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("testcase025_aes256iso_o_set_metadata_not_encrypted.pdf", PdfEncryptionType.AesIsoEncryption256Bit)]
        [TestCase("testcase026_aes256_ou_set_metadata_encrypted.pdf", PdfEncryptionType.AesEncryption256Bit)]
        [TestCase("testcase027_aes256_ou_set_metadata_not_encrypted.pdf", PdfEncryptionType.AesEncryption256Bit)]
        [TestCase("testcase028_aes256_o_set_metadata_encrypted.pdf", PdfEncryptionType.AesEncryption256Bit)]
        [TestCase("testcase029_aes256_o_set_metadata_not_encrypted.pdf", PdfEncryptionType.AesEncryption256Bit)]
        [TestCase("testcase030_aes128_ou_set_metadata_encrypted.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase031_aes128_ou_set_metadata_not_encrypted.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase032_aes128_o_set_metadata_encrypted.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase033_aes128_o_set_metadata_not_encrypted.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase034_standard128_ou_set_metadata_encrypted.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase035_standard128_o_set_metadata_encrypted.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase037_standard40_ou_set_metadata_encrypted.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase038_standard40_o_set_metadata_encrypted.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("pdf-example-encryption.original_o_set.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("pdf-example-password.original_uo_set.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("Encryption.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("sales-quote.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase42.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase43.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase44.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase45.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase46.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase47.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase48.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase49.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase50.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase51.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase52.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase53.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase54.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase55.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase56.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase57.pdf", PdfEncryptionType.StandardEncryption40Bit)]
        [TestCase("testcase58.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase59.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase60.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase61.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase62.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase63.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase64.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase65.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase66.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase67.pdf", PdfEncryptionType.StandardEncryption128Bit)]
        [TestCase("testcase68.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase69.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase70.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase71.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase72.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase73.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase74.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase75.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase76.pdf", PdfEncryptionType.AesEncryption128Bit)]
        [TestCase("testcase77.pdf", PdfEncryptionType.AesEncryption128Bit)]
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

        [Test]
        [TestCase("testcase022_aes256iso_uo_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase023_aes256iso_uo_set_metadata_not_encrypted.pdf", false)]
        [TestCase("testcase024_aes256iso_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase025_aes256iso_o_set_metadata_not_encrypted.pdf", false)]
        [TestCase("testcase026_aes256_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase027_aes256_ou_set_metadata_not_encrypted.pdf", false)]
        [TestCase("testcase028_aes256_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase029_aes256_o_set_metadata_not_encrypted.pdf", false)]
        [TestCase("testcase030_aes128_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase031_aes128_ou_set_metadata_not_encrypted.pdf", false)]
        [TestCase("testcase032_aes128_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase033_aes128_o_set_metadata_not_encrypted.pdf", false)]
        [TestCase("testcase034_standard128_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase035_standard128_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase037_standard40_ou_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase038_standard40_o_set_metadata_encrypted.pdf", true)]
        [TestCase("testcase42.pdf", true)]
        [TestCase("testcase43.pdf", true)]
        [TestCase("testcase44.pdf", true)]
        [TestCase("testcase45.pdf", true)]
        [TestCase("testcase46.pdf", true)]
        [TestCase("testcase47.pdf", true)]
        [TestCase("testcase48.pdf", true)]
        [TestCase("testcase49.pdf", true)]
        [TestCase("testcase50.pdf", true)]
        [TestCase("testcase51.pdf", true)]
        [TestCase("testcase52.pdf", true)]
        [TestCase("testcase53.pdf", true)]
        [TestCase("testcase54.pdf", true)]
        [TestCase("testcase55.pdf", true)]
        [TestCase("testcase56.pdf", true)]
        [TestCase("testcase57.pdf", true)]
        [TestCase("testcase58.pdf", true)]
        [TestCase("testcase59.pdf", true)]
        [TestCase("testcase60.pdf", true)]
        [TestCase("testcase61.pdf", true)]
        [TestCase("testcase62.pdf", true)]
        [TestCase("testcase63.pdf", true)]
        [TestCase("testcase64.pdf", true)]
        [TestCase("testcase65.pdf", true)]
        [TestCase("testcase66.pdf", true)]
        [TestCase("testcase67.pdf", true)]
        [TestCase("testcase68.pdf", true)]
        [TestCase("testcase69.pdf", false)]
        [TestCase("testcase70.pdf", true)]
        [TestCase("testcase71.pdf", false)]
        [TestCase("testcase72.pdf", true)]
        [TestCase("testcase73.pdf", false)]
        [TestCase("testcase74.pdf", true)]
        [TestCase("testcase75.pdf", false)]
        [TestCase("testcase76.pdf", true)]
        [TestCase("testcase77.pdf", false)]
        public void MetadataEncryption(string PDF, bool metadataIsEncrypted)
        {
            string errorMsg = string.Empty;

            string filePath = Path.Combine(TestHelper.DataFolder, PDF);
            Assert.IsTrue(File.Exists(filePath));
            PdfFile pdfFile = new PdfFile(filePath);
            pdfFile.Open(ref errorMsg);
            Assert.IsTrue(string.IsNullOrEmpty(errorMsg));
            Assert.AreEqual(pdfFile.EncryptionRecordInfo.metadataIsEncrypted, metadataIsEncrypted);
        }

        [Test]
        [TestCase("testcase42.pdf", -64)] //RC40 -no-edit -no-print -no-copy -no-annot
        [TestCase("testcase43.pdf",-4)]   //RC40 All allowed
        [TestCase("testcase44.pdf", -12)] //RC40 -no-edit
        [TestCase("testcase45.pdf", -8)]  //RC40 -no-print
        [TestCase("testcase46.pdf", -20)] //RC40 -no-copy
        [TestCase("testcase47.pdf", -36)] //RC40 -no-annot
        [TestCase("testcase48.pdf", -32)] //RC40 -no-edit -no-print -no-copy
        [TestCase("testcase49.pdf", -48)] //RC40 -no-edit -no-print -no-annot
        [TestCase("testcase50.pdf", -60)] //RC40 -no-edit -no-copy -no-annot
        [TestCase("testcase51.pdf", -56)] //RC40 -no-print -no-copy -no-annot
        [TestCase("testcase52.pdf", -16)] //RC40 -no-edit -no-print
        [TestCase("testcase53.pdf", -28)] //RC40 -no-edit -no-copy
        [TestCase("testcase54.pdf", -44)] //RC40 -no-edit -no-annot
        [TestCase("testcase55.pdf", -24)] //RC40 -no-print -no-copy
        [TestCase("testcase56.pdf", -40)] //RC40 -no-print -no-annot
        [TestCase("testcase57.pdf", -52)] //RC40 -no-copy -no-annot
        [TestCase("testcase58.pdf", -4)]  //RC128 All allowed
        [TestCase("testcase59.pdf", -12)]  //RC128 -no-edit
        [TestCase("testcase60.pdf", -16)]  //RC128 -no-edit -no-print
        [TestCase("testcase61.pdf", -32)]  //RC128 -no-edit -no-print -no-copy
        [TestCase("testcase62.pdf", -64)]  //RC128 -no-edit -no-print -no-copy -no-annot
        [TestCase("testcase63.pdf", -320)]  //RC128 -no-edit -no-print -no-copy -no-annot -no-forms
        [TestCase("testcase64.pdf", -832)]  //RC128 -no-edit -no-print -no-copy -no-annot -no-forms -no-extract
        [TestCase("testcase65.pdf", -1856)]  //RC128 -no-edit -no-print -no-copy -no-annot -no-forms -no-extract -no-assemble
        [TestCase("testcase66.pdf", -3904)]  //RC128 -no-edit -no-print -no-copy -no-annot -no-forms -no-extract -no-assemble -no-hq-print
        [TestCase("testcase67.pdf", -3900)]  //RC128 -no-edit -no-copy -no-annot -no-forms -no-extract -no-assemble -no-hq-print
        [TestCase("testcase68.pdf", -4)]  //AES128 All allowed
        [TestCase("testcase69.pdf", -12)]  //AES128  -no-edit
        [TestCase("testcase70.pdf", -16)]  //AES128  -no-edit -no-print
        [TestCase("testcase71.pdf", -32)]  //AES128  -no-edit -no-print -no-copy 
        [TestCase("testcase72.pdf", -64)]  //AES128  -no-edit -no-print -no-copy -no-annot
        [TestCase("testcase73.pdf", -320)]  //AES128   -no-edit -no-print -no-copy -no-annot -no-forms
        [TestCase("testcase74.pdf", -832)]  //AES128 -no-edit -no-print -no-copy -no-annot -no-forms -no-extract
        [TestCase("testcase75.pdf", -1856)]  //AES128 -no-edit -no-print -no-copy -no-annot -no-forms -no-extract -no-assemble
        [TestCase("testcase76.pdf", -3904)]  //AES128 -no-edit -no-print -no-copy -no-annot -no-forms -no-extract -no-assemble -no-hq-print
        [TestCase("testcase77.pdf", -3900)]  //AES128 -no-edit -no-copy -no-annot -no-forms -no-extract -no-assemble -no-hq-print
        public void PDFPermissions(string PDF, long pValue)
        {
            string errorMsg = string.Empty;

            string filePath = Path.Combine(TestHelper.DataFolder, PDF);
            Assert.IsTrue(File.Exists(filePath));
            PdfFile pdfFile = new PdfFile(filePath);
            pdfFile.Open(ref errorMsg);
            Assert.IsTrue(string.IsNullOrEmpty(errorMsg));
            Assert.AreEqual(pdfFile.EncryptionRecordInfo.pValue, pValue);
        }

        [Test]
        [TestCase("testcase041_corrupted.pdf")]
        [TestCase("testcase040_corrupted.pdf")]
        [TestCase("testcase039_corrupted.pdf")]
        public void IsCorrupted(string PDF)
        {
            string errorMsg = string.Empty;

            string filePath = Path.Combine(TestHelper.DataFolder, PDF);
            Assert.IsTrue(File.Exists(filePath));
            PdfFile pdfFile = new PdfFile(filePath);
            pdfFile.Open(ref errorMsg);
            Assert.IsFalse(string.IsNullOrEmpty(errorMsg));
        }
    }
}
