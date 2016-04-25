using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyPasswordRecoveryPDF.Common
{
    public static class FileHelpers
    {
        public static int FolderIsValid(string folderPath, out string errorMsg)
        {
            int result = Constants.Success;
            errorMsg = string.Empty;

            if (string.IsNullOrEmpty(folderPath))
            {
                errorMsg = "Folder is not specified.";
                result = Constants.FolderIsNotSpecified;
            }
            else if (folderPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                string stringResult =
                    new string(System.IO.Path.GetInvalidPathChars().Where(c => !char.IsControl(c)).ToArray());
                errorMsg = string.Format("Folder path contains invalid characters: [{0}].", stringResult);
                result = Constants.FolderHasInvalidPathChars;
            }
            else if (!Directory.Exists(folderPath))
            {
                errorMsg = "Specified folder does not exist.";
                result = Constants.FolderDoesNotExist;
            }
            return result;
        }

        public static int FileNameIsValid(string fileName, out string errorMsg)
        {
            int result = Constants.Success;
            errorMsg = string.Empty;

            if (string.IsNullOrEmpty(fileName))
            {
                errorMsg = "File name is not specified.";
                result = Constants.FileNameIsNotSpecified;
            }
            else if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                string stringResult = new string(System.IO.Path.GetInvalidFileNameChars().Where(c => !char.IsControl(c)).ToArray());
                errorMsg = string.Format("File name contains invalid characters: [{0}].", stringResult);
                result = Constants.FileNameHasInvalidPathChars;
            }
            return result;
        }

        public static string TrimAllWithInplaceCharArray(string str)
        {
            var len = str.Length;
            var src = str.ToCharArray();
            int dstIdx = 0;
            for (int i = 0; i < len; i++)
            {
                var ch = src[i];
                if (!isWhiteSpace(ch))
                    src[dstIdx++] = ch;
            }
            return new string(src, 0, dstIdx);
        }

        // whitespace detection method: very fast, a lot faster than Char.IsWhiteSpace
        [MethodImpl(MethodImplOptions.AggressiveInlining)] // if it's not inlined then it will be slow!!!
        public static bool isWhiteSpace(char ch)
        {
            // this is surprisingly faster than the equivalent if statement
            switch (ch)
            {
                case '\u0009':
                case '\u000A':
                case '\u000B':
                case '\u000C':
                case '\u000D':
                case '\u0020':
                case '\u0085':
                case '\u00A0':
                case '\u1680':
                case '\u2000':
                case '\u2001':
                case '\u2002':
                case '\u2003':
                case '\u2004':
                case '\u2005':
                case '\u2006':
                case '\u2007':
                case '\u2008':
                case '\u2009':
                case '\u200A':
                case '\u2028':
                case '\u2029':
                case '\u202F':
                case '\u205F':
                case '\u3000':
                    return true;
                default:
                    return false;
            }
        }

        public static async Task WriteToFileAsync(string destinationFilePath, FileMode fileMode, string summaryMsg)
        {
            int offset = 0;
            int sizeOfBuffer = 4096;
            FileStream fileStream = null;

            if (string.IsNullOrEmpty(destinationFilePath))
            {
                throw new ArgumentNullException("destinationFilePath");
            }
            else if (string.IsNullOrEmpty(summaryMsg))
            {
                throw new ArgumentNullException("summaryMsg");
            }

            byte[] buffer = Encoding.Unicode.GetBytes(summaryMsg);

            try
            {
                fileStream = new FileStream(destinationFilePath, fileMode, FileAccess.Write,
                    FileShare.None, bufferSize: sizeOfBuffer, useAsync: true);
                await fileStream.WriteAsync(buffer, offset, buffer.Length);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                    fileStream = null;
                }
            }
        }
    }
}
