using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using VitaTest.AppCore.Enums;

namespace VitaTest.AppCore.Converters
{
    public class MessageTypeToBrushConverter : IValueConverter
    {
        private static readonly Brush InfoBrush = CreateAndFreeze(33, 150, 243);
        private static readonly Brush ErrorBrush = CreateAndFreeze(244, 67, 54);
        private static readonly Brush WarningBrush = CreateAndFreeze(255, 152, 0);
        private static readonly Brush NoneBrush = Brushes.Transparent;

        private static SolidColorBrush CreateAndFreeze(byte r, byte g, byte b)
        {
            var brush = new SolidColorBrush(Color.FromRgb(r, g, b));
            brush.Freeze();
            return brush;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = value is NotifyType notifyType ? notifyType switch
            {
                NotifyType.Info => InfoBrush,
                NotifyType.Error => ErrorBrush,
                NotifyType.Warning => WarningBrush,
                NotifyType.None => NoneBrush,
                _ => NoneBrush
            } : NoneBrush;

            return targetType == typeof(Color) ? ((SolidColorBrush)brush).Color : brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
