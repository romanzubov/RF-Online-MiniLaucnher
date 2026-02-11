using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MiniLauncherStyle.Core
{
    /// <summary>
    /// Конвертер bool в Visibility.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool && (bool)value;
            
            // Если параметр "Invert", инвертируем логику
            if (parameter != null && parameter.ToString() == "Invert")
            {
                boolValue = !boolValue;
            }
            
            return boolValue ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            bool result = visibility == Visibility.Visible;
            
            if (parameter != null && parameter.ToString() == "Invert")
            {
                result = !result;
            }
            
            return result;
        }
    }
    
    /// <summary>
    /// Конвертер bool в Visibility с Collapsed вместо Hidden.
    /// </summary>
    public class BoolToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool && (bool)value;
            
            if (parameter != null && parameter.ToString() == "Invert")
            {
                boolValue = !boolValue;
            }
            
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            bool result = visibility == Visibility.Visible;
            
            if (parameter != null && parameter.ToString() == "Invert")
            {
                result = !result;
            }
            
            return result;
        }
    }
    
    /// <summary>
    /// Конвертер строки цвета в SolidColorBrush.
    /// </summary>
    public class StringToColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorName = value as string;
            if (string.IsNullOrEmpty(colorName))
            {
                return new SolidColorBrush(Colors.White);
            }
            
            switch (colorName.ToLower())
            {
                case "green":
                    return new SolidColorBrush(Colors.Green);
                case "red":
                    return new SolidColorBrush(Colors.Red);
                case "orange":
                    return new SolidColorBrush(Colors.Orange);
                case "white":
                    return new SolidColorBrush(Colors.White);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Конвертер bool в TextDecorations (Underline).
    /// </summary>
    public class BoolToUnderlineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool && (bool)value;
            return boolValue ? TextDecorations.Underline : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == TextDecorations.Underline;
        }
    }
    
    /// <summary>
    /// Конвертер bool в TextDecorations (Strikethrough).
    /// </summary>
    public class BoolToStrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool && (bool)value;
            return boolValue ? TextDecorations.Strikethrough : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == TextDecorations.Strikethrough;
        }
    }
}
