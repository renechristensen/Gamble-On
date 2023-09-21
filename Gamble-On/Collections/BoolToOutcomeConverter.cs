using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Gamble_On.Converters
{
    public class BoolToOutcomeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? "Win" : "Lose";
            }
            return "Unknown";  // or return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

