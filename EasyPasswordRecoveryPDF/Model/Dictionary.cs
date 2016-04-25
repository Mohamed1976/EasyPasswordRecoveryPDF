using System;
using System.IO;
using System.Globalization;
using System.Collections.ObjectModel;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Interfaces;
using System.Linq;

namespace EasyPasswordRecoveryPDF.Model
{
    public class Dictionaries : ObservableCollection<Dictionary>, IPasswordIterator
    {
        #region [ Constructor ]

        public Dictionaries()
        {
        }

        #endregion

        #region [ Properties ]

        private int currentIndex = 0;
        private int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        #endregion

        #region [ IPasswordIterator ] 

        public int Initialize(CharsetEncoding pdfPasswordEncoding, int maxPasswordLength, ref string errorMsg)
        {
            int isValid = Constants.Failure;
            CurrentIndex = 0;

            if (this.Count == 0)
            {
                errorMsg = "Please add a dictionary to the list.";
            }
            else
            {
                int validCount = 0;

                while (validCount < this.Count &&
                    this.ElementAt(validCount).Initialize(ref errorMsg) == Constants.Success)
                {
                    validCount++;
                };

                if (this.Count == validCount)
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

            if((RemoveWhitespace || PasswordCasing != CharsetCasing.None) && !string.IsNullOrEmpty(result))
            {
                if(RemoveWhitespace)
                {
                    result = FileHelpers.TrimAllWithInplaceCharArray(result);
                }

                if(PasswordCasing == CharsetCasing.LowerCase)
                {
                    result = CultureInfo.CurrentCulture.TextInfo.ToLower(result);
                }
                else if(PasswordCasing == CharsetCasing.UpperCase)
                {
                    result = CultureInfo.CurrentCulture.TextInfo.ToUpper(result);
                }
                else if (PasswordCasing == CharsetCasing.TitleCase)
                {
                    result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
                }
            }

            return result;
        }

        public CharsetCasing PasswordCasing {get; set; }
        public bool RemoveWhitespace { get; set; }

        #endregion
    }

    public class Dictionary : ModelBase
    {
        #region [ Defines ]

        private StreamReader reader = null;

        #endregion

        #region [ Constructor ]

        public Dictionary(string path)
        {
            Info = new FileInfo(path);
        }

        #endregion

        #region [ Methods ]

        public int Initialize(ref string errorMsg)
        {
            int result = Constants.Failure;

            try
            {
                if(reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }

                Progress = 0;
                reader = new StreamReader(Info.FullName);
                reader.Peek();
                result = Constants.Success;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return result;
        }

        public string GetNextPassword()
        {
            string result = string.Empty;

            if (reader != null)
            {
                while((result = reader.ReadLine()) != null &&
                    (string.IsNullOrEmpty(result) || 
                    string.IsNullOrWhiteSpace(result))) { };

                if (!string.IsNullOrEmpty(result))
                {
                    Progress++;
                }
                else
                {
                    reader.Close();
                    reader = null;
                }
            }

            return result;
        }

        #endregion

        #region [ Properties ]

        private FileInfo info = null;
        public FileInfo Info
        {
            get { return info; }
            set { SetProperty(ref this.info, value); }
        }

        private uint progress = 0;
        public uint Progress
        {
            get { return progress; }
            set { SetProperty(ref this.progress, value); }
        }

        #endregion
    }
}
