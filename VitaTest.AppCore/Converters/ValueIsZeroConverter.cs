using System.Globalization;
using System.Windows.Data;

namespace VitaTest.AppCore.Converters
{
    public class ValueIsZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal val)
                return val == 0;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
