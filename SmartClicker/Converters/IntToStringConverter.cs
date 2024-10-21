using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SmartClicker.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue && int.TryParse(strValue, out int intValue))
            {
                return intValue;
            }
            return 0;
        }
    }
}
