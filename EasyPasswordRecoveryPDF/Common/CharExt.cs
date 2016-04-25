using System.Globalization;

namespace EasyPasswordRecoveryPDF.Common
{
    public class CharExt : ModelBase
    {
        #region [ Constructor ]

        public CharExt(char value)
        {
            CharValue = value;
        }

        #endregion

        #region [ Properties ]

        private char charValue = '\0';
        public char CharValue
        {
            get { return charValue; }
            private set { SetProperty(ref this.charValue, value); }
        }

        public char Glyph
        {
            get
            {
                if (char.IsControl(CharValue))
                {
                    return '\u0020';
                }
                else
                {
                    return CharValue;
                }
            }
        }

        public string Code
        {
            get { return string.Format("U+{0:X4}", (int)CharValue); }
        }

        public string Decimal
        {
            get { return string.Format("{0:d}", (int)CharValue); }
        }

        public UnicodeCategory Category
        {
            get { return char.GetUnicodeCategory(CharValue); }
        }

        #endregion
    }
}
