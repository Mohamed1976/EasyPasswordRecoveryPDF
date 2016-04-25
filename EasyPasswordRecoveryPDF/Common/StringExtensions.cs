using System.Text;

namespace EasyPasswordRecoveryPDF.Common
{
    public static class StringExtensions
    {
        private static readonly Encoding latin1 = Encoding.GetEncoding("iso-8859-1",
            new EncoderReplacementFallback(string.Empty),
            new DecoderExceptionFallback());

        private static Encoding unicode = Encoding.Unicode;

        public static byte[] ToLatin1Bytes(this string unicodeString)
        {
            byte[] unicodeBytes = unicode.GetBytes(unicodeString);
            return Encoding.Convert(unicode, latin1, unicodeBytes);
        }
    }
}
