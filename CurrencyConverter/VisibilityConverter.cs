using System;
using System.Globalization;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CurrencyConverter
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || (bool)value)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            string paramValue = (string)parameter;
            if (value == null || (Visibility)value == Visibility.Visible)
            {
                return paramValue != "Collapsed";
            }

            return paramValue == "Collapsed";
        }

    }
}