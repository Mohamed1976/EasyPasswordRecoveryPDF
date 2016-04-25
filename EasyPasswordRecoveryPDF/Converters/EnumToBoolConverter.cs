using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyPasswordRecoveryPDF.Converters
{
    [ValueConversion(typeof(bool), typeof(Enum))]
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            else
                return value.Equals(parameter);
        }

        public object ConvertBack(object value,
            Type targetTypes, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            else if ((bool)value)
                return parameter;
            else
                return Binding.DoNothing;
        }
    }
}
