using Application.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Application.Conventers;

public class ColorHexConventer : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (value is string color)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            }
            if (value is List<ColorItem> colors)
            {
                var gradients = colors.Select(a =>
                {
                    var color = ColorConverter.ConvertFromString(a.Color);
                    return new GradientStop((Color)color, a.Offset);
                });
                var collection = new GradientStopCollection(gradients);
                return new LinearGradientBrush(collection, new Point(0.5, 0), new Point(0.5, 1));
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
