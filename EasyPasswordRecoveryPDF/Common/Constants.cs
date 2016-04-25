using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPasswordRecoveryPDF.Common
{
    /// <summary>
    /// This class contains constants
    /// </summary>
    internal sealed class Constants
    {
        public const int KeySize = 16;
        public const int VectorSize = 16;
        public const int CompareSize = 32;
        public const int SaltLength = 8;
        public const int SaltOffset = 32;
        public const int MinPasswordSize = 1;
        public const int MaxPasswordSize = 32;
        public const int MaxPasswordSizeV2 = 127;

        public static readonly int Success = 0;
        public static readonly int Failure = 1;
        public static readonly int PdfException = 2;
        public static readonly int BadPdfFormatException = 3;
        public static readonly int PdfAConformanceException = 4;
        public static readonly int PdfIsoConformanceException = 5;
        public static readonly int BadPasswordException = 6;
        public static readonly int IllegalPdfSyntaxException = 7;
        public static readonly int InvalidImageException = 8;
        public static readonly int InvalidPdfException = 9;
        public static readonly int FileAlreadyExistsAtLocation = 10;
        public static readonly int FolderIsNotSpecified = 11;
        public static readonly int FolderHasInvalidPathChars = 12;
        public static readonly int FolderDoesNotExist = 13;
        public static readonly int FileNameIsNotSpecified = 14;
        public static readonly int FileNameHasInvalidPathChars = 15;
        public static readonly int PageRangeIsNullOrEmpty = 16;
        public static readonly int PageRangeSyntaxError = 17;
        public static readonly int SpecifiedPageRangeNotValid = 18;
        public static readonly int SpecifiedPageIntervalNotValid = 19;
        public static readonly int RangeAndIntervalAreNullOrEmpty = 20;

        //Default values
        public const int DefaultInitialPasswordLength = 8;
        public const int DefaultMaxInitialPasswordLength = 32;
        public const int DefaultMinInitialPasswordLength = 1;

        public const int DefaultMaxPasswordLength = 9;
        public const int DefaultMaxMaxPasswordLength = 32;
        public const int DefaultMinMaxPasswordLength = 9;

        public const int DefaultMinPasswordLength = 7;
        public const int DefaultMaxMinPasswordLength = 7;
        public const int DefaultMinMinPasswordLength = 1;

        public const int DefaultMaxRowsRegEx = 1000;
        public const int DefaultMinMaxRowsRegEx = 1;
        public const int DefaultMaxMaxRowsRegEx = 1000000;

        //Versions
        public const string GenerexVersion = "0.0.4";

    }
}
