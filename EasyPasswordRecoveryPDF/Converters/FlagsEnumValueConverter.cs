using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

//http://compilewith.net/2008/12/wpf-flagsenumvalueconverter.html
namespace EasyPasswordRecoveryPDF.Converters
{
    public sealed class FlagsEnumValueConverter : IValueConverter
    {
        private int targetValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int mask = (int)parameter;
            this.targetValue = (int)value;

            return ((mask & this.targetValue) == mask);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            this.targetValue ^= (int)parameter;
            return Enum.Parse(targetType, this.targetValue.ToString());
        }
    }
}
