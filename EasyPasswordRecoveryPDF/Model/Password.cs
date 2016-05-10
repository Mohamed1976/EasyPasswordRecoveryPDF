using System;
using System.Linq;
using EasyPasswordRecoveryPDF.Interfaces;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using EasyPasswordRecoveryPDF.Common;
using System.Globalization;
using System.Windows.Controls;

namespace EasyPasswordRecoveryPDF.Model
{
    #region [ Defines ]

    public enum IteratorStatus
    {
        ExceedsLimit = 0,
        Good = 1,
        Finished = 2
    }

    #endregion

    public class IteratorCounter : ModelBase
    {
        #region [ Properties ]

        private IteratorStatus status = IteratorStatus.ExceedsLimit;
        public IteratorStatus Status
        {
            get { return status; }
            set { SetProperty(ref this.status, value); }
        }

        private int passwordLenght = 0;
        public int PasswordLenght
        {
            get { return passwordLenght; }
            set { SetProperty(ref this.passwordLenght, value); }
        }

        private ulong count = 0;
        public ulong Count
        {
            get { return count; }
            set { SetProperty(ref this.count, value); }
        }

        private ulong maxCount = 0;
        public ulong MaxCount
        {
            get { return maxCount; }
            set { SetProperty(ref this.maxCount, value); }
        }

        #endregion
    }

    public class Password : ModelBase, IPasswordIterator
    {
        #region [ Defines ]

        private static readonly char[] dash = new char[] { '-' };
        private static readonly char[] comma = new char[] { ',' };

        //Unicode \u0000-\uFFFF
        private static readonly string unicodeRangePattern
            = @"^(([a-f0-9]{4}-[a-f0-9]{4},)*|([a-f0-9]{4},)*)*((([a-f0-9]{4},)*([a-f0-9]{4}))|([a-f0-9]{4}-[a-f0-9]{4})){1};?$";

        //ASCII \u0000-\u00FF
        private static readonly string asciiUnicodeRangePattern 
            = @"^((00[a-f0-9]{2}-00[a-f0-9]{2},)*|(00[a-f0-9]{2},)*)*(((00[a-f0-9]{2},)*(00[a-f0-9]{2}))|(00[a-f0-9]{2}-00[a-f0-9]{2})){1};?$";

        private static readonly Char[] DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static readonly Char[] LOWER_CASE_LETTERS = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static readonly Char[] UPPER_CASE_LETTERS = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static readonly Char[] PUNCTUATION_MARKS = { '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~' };
        private static readonly Char[] SPACE = { ' ' };

        #endregion

        #region [ Constructor ]

        public Password()
        {
            IteratorCounters = new ObservableCollection<IteratorCounter>();
            CharactersToUse = new ObservableCollectionExt<CharExt>();
        }

    #endregion

        #region [ Properties ]

        private bool increasePswLength = false;
        public bool IncreasePswLength
        {
            get { return increasePswLength; }
            set { SetProperty(ref this.increasePswLength, value); }
        }

        private bool decreasePswLength = false;
        public bool DecreasePswLength
        {
            get { return decreasePswLength; }
            set { SetProperty(ref this.decreasePswLength, value); }
        }

        private Charset passwordCharset = Charset.All;
        public Charset PasswordCharset
        {
            get { return passwordCharset; }
            set { SetProperty(ref this.passwordCharset, value); }
        }

        private CharsetEncoding passwordEncoding = CharsetEncoding.Ascii;
        public CharsetEncoding PasswordEncoding
        {
            get { return passwordEncoding; }
            set { SetProperty(ref this.passwordEncoding, value); }
        }

        private int currentIndex = 0;
        private int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        #region [ InitialPasswordLength ] 

        private int initialPasswordLength = Constants.DefaultInitialPasswordLength;
        public int InitialPasswordLength
        {
            get { return initialPasswordLength; }
            set
            {
                if (value == Constants.MaxPasswordSize)
                {
                    MinMaxPasswordLength = value;
                }
                else
                {
                    MinMaxPasswordLength = value + 1;
                }

                if(MinMaxPasswordLength > MaxPasswordLength)
                {
                    MaxPasswordLength = MinMaxPasswordLength;
                }

                if (value == Constants.MinPasswordSize)
                {
                    MaxMinPasswordLength = value;
                }
                else
                {
                    MaxMinPasswordLength = value - 1;
                }

                if (MaxMinPasswordLength < MinPasswordLength)
                {
                    MinPasswordLength = MaxMinPasswordLength;
                }

                SetProperty(ref this.initialPasswordLength, value);
            }
        }

        private int maxInitialPasswordLength = Constants.DefaultMaxInitialPasswordLength;
        public int MaxInitialPasswordLength
        {
            get { return maxInitialPasswordLength; }
            set { SetProperty(ref this.maxInitialPasswordLength, value); }
        }

        private int minInitialPasswordLength = Constants.DefaultMinInitialPasswordLength;
        public int MinInitialPasswordLength
        {
            get { return minInitialPasswordLength; }
            set { SetProperty(ref this.minInitialPasswordLength, value); }
        }

        #endregion

        #region [ MaxPasswordLength ] 

        private int maxPasswordLength = Constants.DefaultMaxPasswordLength;
        public int MaxPasswordLength
        {
            get { return maxPasswordLength; }
            set { SetProperty(ref this.maxPasswordLength, value); }
        }

        private int maxMaxPasswordLength = Constants.DefaultMaxMaxPasswordLength;
        public int MaxMaxPasswordLength
        {
            get { return maxMaxPasswordLength; }
            set { SetProperty(ref this.maxMaxPasswordLength, value); }
        }

        private int minMaxPasswordLength = Constants.DefaultMinMaxPasswordLength;
        public int MinMaxPasswordLength
        {
            get { return minMaxPasswordLength; }
            set { SetProperty(ref this.minMaxPasswordLength, value); }
        }

        #endregion

        #region [ MinPasswordLength ] 

        private int minPasswordLength = Constants.DefaultMinPasswordLength;
        public int MinPasswordLength
        {
            get { return minPasswordLength; }
            set { SetProperty(ref this.minPasswordLength, value); }
        }

        private int maxMinPasswordLength = Constants.DefaultMaxMinPasswordLength;
        public int MaxMinPasswordLength
        {
            get { return maxMinPasswordLength; }
            set { SetProperty(ref this.maxMinPasswordLength, value); }
        }

        private int minMinPasswordLength = Constants.DefaultMinMinPasswordLength;
        public int MinMinPasswordLength
        {
            get { return minMinPasswordLength; }
            set { SetProperty(ref this.minMinPasswordLength, value); }
        }

        #endregion

        private string unicodeCharset = string.Empty;
        public string UnicodeCharset
        {
            get { return unicodeCharset; }
            set { SetProperty(ref this.unicodeCharset, value); }
        }

        private ObservableCollectionExt<CharExt> charactersToUse;
        public ObservableCollectionExt<CharExt> CharactersToUse
        {
            get { return charactersToUse; }
            private set { SetProperty(ref this.charactersToUse, value); }
        }

        private ObservableCollection<IteratorCounter> iteratorCounters = null;
        public ObservableCollection<IteratorCounter> IteratorCounters
        {
            get { return iteratorCounters; }
            private set { SetProperty(ref this.iteratorCounters, value); }
        }

        #endregion

        #region [ Methods ]

        private void InitializeIterator()
        {
            int passwordSweepSize = 0;
            ulong maxCount = 0;
            IteratorCounter iteratorCounter;

            IteratorCounters.Clear();
            if (IncreasePswLength)
                passwordSweepSize += MaxPasswordLength - InitialPasswordLength;

            if (DecreasePswLength)
                passwordSweepSize += InitialPasswordLength - MinPasswordLength;

            //Initial password
            iteratorCounter = new IteratorCounter { Status = IteratorStatus.ExceedsLimit, PasswordLenght = InitialPasswordLength};
              
            if (GetNrOfPossibleIterations(ref maxCount,
                CharactersToUse, 
                InitialPasswordLength) == Constants.Success)
            {
                iteratorCounter.Status = IteratorStatus.Good;
                iteratorCounter.MaxCount = maxCount;
                iteratorCounter.Count = 0;
            }

            iteratorCounters.Add(iteratorCounter);

            int incCount = 0, decCount = 0;
            while ((incCount + decCount) < passwordSweepSize)
            {
                if (IncreasePswLength && 
                    MaxPasswordLength > (InitialPasswordLength + incCount))
                {
                    incCount++;
                    iteratorCounter = new IteratorCounter { Status = IteratorStatus.ExceedsLimit, PasswordLenght = InitialPasswordLength + incCount };
                    if (GetNrOfPossibleIterations(ref maxCount,
                        CharactersToUse,
                        InitialPasswordLength + incCount) == Constants.Success)
                    {
                        iteratorCounter.Status = IteratorStatus.Good;
                        iteratorCounter.MaxCount = maxCount;
                        iteratorCounter.Count = 0;
                    }

                    iteratorCounters.Add(iteratorCounter);
                }

                if (DecreasePswLength &&
                    MinPasswordLength < (InitialPasswordLength - decCount))
                {
                    decCount++;
                    iteratorCounter = new IteratorCounter { Status = IteratorStatus.ExceedsLimit, PasswordLenght = InitialPasswordLength - decCount };
                    if (GetNrOfPossibleIterations(ref maxCount,
                        CharactersToUse,
                        InitialPasswordLength - decCount) == Constants.Success)
                    {
                        iteratorCounter.Status = IteratorStatus.Good;
                        iteratorCounter.MaxCount = maxCount;
                        iteratorCounter.Count = 0;
                    }

                    iteratorCounters.Add(iteratorCounter);
                }
            }
        }

        private int GetNrOfPossibleIterations(ref ulong nrOfPossibleIterations,
            ObservableCollectionExt<CharExt> charactersToUse,
            int passwordLength)
        {
            int result = Constants.Failure;
            try
            {
                nrOfPossibleIterations =
                    (ulong)Math.Pow((double)charactersToUse.Count, (double)passwordLength);
                result = nrOfPossibleIterations > 0 ? Constants.Success : Constants.Failure;
            }
            catch(OverflowException){}
            return result;
        }

        private void InitializeUnicodeCharset()
        {
            char ch = '\0';

            charactersToUse.BeginUpdate();
            string[] charsets = UnicodeCharset.Split(comma, StringSplitOptions.RemoveEmptyEntries);
            foreach(string charset in charsets)
            {
                if(charset.IndexOfAny(dash) > 0)
                {
                    int[] charsetRange = 
                        charset.Split(dash).Select(str => int.Parse(str, NumberStyles.HexNumber)).OrderBy(i => i).ToArray();

                    for(int idx = charsetRange[0]; idx <= charsetRange[1]; idx++)
                    {
                        ch = Convert.ToChar(idx);
                        if (!charactersToUse.Any(c => c.CharValue == ch))
                        {
                            charactersToUse.Add(new CharExt(ch));
                        }
                    }
                }
                else
                {
                    ch = Convert.ToChar(int.Parse(charset, NumberStyles.HexNumber));
                    if (!charactersToUse.Any(c => c.CharValue == ch))
                    {
                        charactersToUse.Add(new CharExt(ch));
                    }
                }
            }
            charactersToUse.EndUpdate();
        }

        private void AddCharset(ref ObservableCollectionExt<CharExt> charactersToUse, char [] charset)
        {
            foreach(char ch in charset)
            {
                charactersToUse.Add(new CharExt(ch));
            }
        }

        private void InitializeAsciiCharset()
        {
            charactersToUse.BeginUpdate();
            if ((PasswordCharset & Charset.LowerCaseLetters) == Charset.LowerCaseLetters)
            {
                AddCharset(ref charactersToUse, LOWER_CASE_LETTERS);
            }

            if ((PasswordCharset & Charset.UpperCaseLetters) == Charset.UpperCaseLetters)
            {
                AddCharset(ref charactersToUse, UPPER_CASE_LETTERS);
            }

            if ((PasswordCharset & Charset.Digits) == Charset.Digits)
            {
                AddCharset(ref charactersToUse, DIGITS);
            }

            if ((PasswordCharset & Charset.PunctuationMarks) == Charset.PunctuationMarks)
            {
                AddCharset(ref charactersToUse, PUNCTUATION_MARKS);
            }

            if ((PasswordCharset & Charset.Space) == Charset.Space)
            {
                AddCharset(ref charactersToUse, SPACE);
            }
            charactersToUse.EndUpdate();
        }

        #endregion

        #region [ IPasswordIterator ]

        public int Initialize(CharsetEncoding pdfPasswordEncoding, int maxPasswordLength, ref string errorMsg)
        {
            CurrentIndex = 0;
            errorMsg = string.Empty;
            int minAllowedPasswordLength = 1;
            int maxAllowedPasswordLength = maxPasswordLength;

            CharactersToUse.Clear();

            ClearAllErrors();

            if (InitialPasswordLength > maxAllowedPasswordLength ||
                InitialPasswordLength < minAllowedPasswordLength)
            {
                errorMsg = string.Format("Initial password must be equal or between {0} and {1} chars.",
                    minAllowedPasswordLength, maxAllowedPasswordLength);
                SetErrors("InitialPasswordLength", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
            }

            if (errorMsg == string.Empty &&
                IncreasePswLength &&
                (MaxPasswordLength > maxAllowedPasswordLength ||
                MaxPasswordLength <= InitialPasswordLength))
            {
                if (InitialPasswordLength == maxAllowedPasswordLength)
                    errorMsg = string.Format("Cannot increase password beyond initial password of {0} chars.", InitialPasswordLength);
                else if (MaxPasswordLength > maxAllowedPasswordLength)
                    errorMsg = string.Format("Cannot increase password beyond limit of {0} chars.", maxAllowedPasswordLength);
                else
                    errorMsg = string.Format("Max password must be equal or between {0} and {1} chars.", InitialPasswordLength + 1, maxAllowedPasswordLength);

                SetErrors("MaxPasswordLength", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
            }

            if (errorMsg == string.Empty &&
                DecreasePswLength &&
                (MinPasswordLength < minAllowedPasswordLength ||
                MinPasswordLength >= InitialPasswordLength))
            {
                if (InitialPasswordLength == minAllowedPasswordLength)
                    errorMsg = string.Format("Cannot decrease password beyond initial password of {0} chars.", InitialPasswordLength);
                else if (MinPasswordLength < minAllowedPasswordLength)
                    errorMsg = string.Format("Cannot decrease password beyond limit of {0} chars.", minAllowedPasswordLength);
                else
                    errorMsg = string.Format("Min password must be equal or between {0} and {1} chars.", minAllowedPasswordLength, InitialPasswordLength - 1);

                SetErrors("MinPasswordLength", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
            }

            if (errorMsg == string.Empty &&
                PasswordEncoding == CharsetEncoding.Ascii &&
                PasswordCharset == Charset.None)
            {
                errorMsg = string.Format("Please specify the charset to use.");
                SetErrors("PasswordCharset", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
            }

            if (errorMsg == string.Empty &&
                PasswordEncoding == CharsetEncoding.Unicode)
            {
                if (string.IsNullOrEmpty(UnicodeCharset))
                    errorMsg = string.Format("Please specify the UnicodeCharset to use.");
                else if (pdfPasswordEncoding == CharsetEncoding.Unicode &&
                    !Regex.IsMatch(UnicodeCharset, unicodeRangePattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled))
                    errorMsg = string.Format("Please check the charset format.");
                else if (pdfPasswordEncoding == CharsetEncoding.Ascii &&
                    !Regex.IsMatch(UnicodeCharset, asciiUnicodeRangePattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled))
                    errorMsg = string.Format("Pdf version only allows ASCII chars [0000 - 00FF].");

                if (errorMsg != string.Empty)
                    SetErrors("UnicodeCharset", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
            }

            if (IsValid)
            {
                if (PasswordEncoding == CharsetEncoding.Ascii)
                    InitializeAsciiCharset();
                else if (PasswordEncoding == CharsetEncoding.Unicode)
                    InitializeUnicodeCharset();
                InitializeIterator();
            }

            return base.IsValid ? Constants.Success : Constants.Failure;
        }

        public string GetNextPassword()
        {
            string result = string.Empty;

            while(IteratorCounters.Any() &&
                CurrentIndex < IteratorCounters.Count &&
                IteratorCounters[CurrentIndex].Status != IteratorStatus.Good)
            {
                CurrentIndex++;
            }             

            if(IteratorCounters.Any() &&
                CurrentIndex < IteratorCounters.Count &&
                IteratorCounters[CurrentIndex].Status == IteratorStatus.Good)
            {
                ulong val = IteratorCounters[CurrentIndex].Count;
                for (int j = 0; j < IteratorCounters[CurrentIndex].PasswordLenght; j++)
                {
                    int ch = (int)(val % (ulong)charactersToUse.Count);
                    result = charactersToUse[ch].CharValue + result;
                    val = val / (ulong)charactersToUse.Count;
                }

                iteratorCounters[CurrentIndex].Count++;
                if (iteratorCounters[CurrentIndex].Count >=
                    iteratorCounters[CurrentIndex].MaxCount)
                    IteratorCounters[CurrentIndex].Status = IteratorStatus.Finished;
            }
            return result;
        }

        public CharsetCasing PasswordCasing { get; set; }
        public bool RemoveWhitespace { get; set; }

        #endregion
    }
}
