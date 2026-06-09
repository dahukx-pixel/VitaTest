using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using VitaTest.AppCore.Enums;

namespace VitaTest.AppCore.Converters
{
    public class MessageTypeToPackIconKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NotifyType notifyType)
            {
                switch (notifyType)
                {
                    case NotifyType.Info:
                        return PackIconKind.Information;
                    case NotifyType.Error:
                        return PackIconKind.Alert;
                    case NotifyType.Warning:
                        return PackIconKind.Warning;
                    case NotifyType.None:
                    default:
                        return PackIconKind.None;
                }
            }

            return PackIconKind.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
