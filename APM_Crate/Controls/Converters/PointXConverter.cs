using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Converters;
public class PointXConverter : IValueConverter
{
    public double Width { get; set; } = 600;
    public double LeftPadding { get; set; } = 0;
    public double RightPadding { get; set; } = 0;
    public int Count { get; set; } = 9;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return 0.0;

        int index = System.Convert.ToInt32(value);

        double chartWidth = Width - LeftPadding - RightPadding;
        return LeftPadding + index * (chartWidth / Math.Max(1, Count - 1)) - 3;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}