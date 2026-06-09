using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VitaTest.AppCore.Converters
{
    public class BalanceToColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush ZeroBrush =
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D4E4D4"));

        private static readonly SolidColorBrush NonZeroBrush =
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E4E4E4"));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal balance && balance == 0)
                return ZeroBrush;

            return NonZeroBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
