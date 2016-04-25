using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace EasyPasswordRecoveryPDF.Converters
{
    [ValueConversion(typeof(string), typeof(bool))]
    public sealed class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
