using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace EasyPasswordRecoveryPDF.Converters
{
    [ValueConversion(typeof(Enum),typeof(bool))]
    public class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            if (value != null && 
                parameter != null &&
                value.Equals(parameter))
                result = Visibility.Visible;

            return result;
        }

        public object ConvertBack(object value, 
            Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
