using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyPasswordRecoveryPDF.Converters
{
    [ValueConversion(typeof(long), typeof(string))]
    public class BytesSuffixConverter : IValueConverter
    {
        private readonly string[] Suffix = { "B", "KB", "MB", "GB", "TB" };

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            long bytes = (long)value;
            return formatBytes(bytes);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string formatBytes(long bytes)
        {
            int index = 0;
            double fileSize = bytes;
            while ((int)((double)fileSize / 1024.0f) > 0)
            {
                fileSize /= 1024.0f;
                index++;
            }

            return String.Format("{0:0.00} {1}", fileSize, Suffix[index]);
        }
    }
}
