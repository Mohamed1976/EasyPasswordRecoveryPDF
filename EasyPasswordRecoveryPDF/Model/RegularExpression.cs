using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Interfaces;
using com.mifmif.common.regex.util;
using com.mifmif.common.regex;

namespace EasyPasswordRecoveryPDF.Model
{
    public class RegularExpressions : ObservableCollection<RegularExpression>, IPasswordIterator
    {
        #region [ Constructor ]

        public RegularExpressions()
        {
        }

        #endregion

        #region [ IPasswordIterator ] 

        public int Initialize(CharsetEncoding pdfPasswordEncoding, int maxPasswordLength, ref string errorMsg)
        {
            int isValid = Constants.Failure;
            CurrentIndex = 0;

            if (this.Count == 0)
            {
                errorMsg = "Please add a regular expression to the list.";
            }
            else
            {
                int validCount = 0;

                while (validCount < this.Count &&
                    this.ElementAt(validCount).Initialize(ref errorMsg) == Constants.Success)
                {
                    validCount++;
                };

                if(this.Count == validCount)
                {
                    isValid = Constants.Success;
                }
            }

            return isValid;
        }

        public string GetNextPassword()
        {
            string result = this.ElementAt(CurrentIndex).GetNextPassword();
            if (string.IsNullOrEmpty(result) && ++CurrentIndex < this.Count)
            {
                result = this.ElementAt(CurrentIndex).GetNextPassword();
            }

            return result;
        }

        public CharsetCasing PasswordCasing { get; set; }
        public bool RemoveWhitespace { get; set; }

        #endregion

        #region [ Properties ]

        private int currentIndex = 0;
        private int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        #endregion
    }

    public class RegularExpression : ModelBase
    {
        #region [ Constructor ]

        public RegularExpression()
        {
        }

        #endregion

        #region [ Properties ]

        private string regEx = string.Empty;
        public string RegEx
        {
            get { return regEx; }
            set { SetProperty(ref this.regEx, value); }
        }

        private uint progress = 0;
        public uint Progress
        {
            get { return progress; }
            set { SetProperty(ref this.progress, value); }
        }

        private Iterator regExIterator = null;
        public Iterator RegExIterator
        {
            get { return regExIterator; }
            set { SetProperty(ref this.regExIterator, value); }
        }

        #endregion

        #region [ Methods ]

        public int Initialize(ref string errorMsg)
        {
            int result = Constants.Failure;
            RegExIterator = null;
            Progress = 0;

            try
            {
                ClearAllErrors();
                if (string.IsNullOrEmpty(RegEx) || RegEx.Trim().Length == 0)
                {
                    errorMsg = "The specified Regex is empty.";
                    SetErrors("RegEx", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
                }
                else
                {
                    Generex generex = new Generex(RegEx);
                    Iterator iterator = generex.iterator();
                    if (!iterator.hasNext())
                    {
                        errorMsg = "Unable to generate strings that matches the specified Regex.";
                        SetErrors("RegEx", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
                    }
                    else
                    {
                        RegExIterator = generex.iterator();
                        result = Constants.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                SetErrors("RegEx", new List<ValidationResult>() { new ValidationResult(false, errorMsg) });
            }

            return result;
        }

        public string GetNextPassword()
        {
            string result = string.Empty;
            if(RegExIterator.hasNext())
            {
                result = RegExIterator.next();
                Progress++;
            }

            return result;
        }

        #endregion
    }
}
