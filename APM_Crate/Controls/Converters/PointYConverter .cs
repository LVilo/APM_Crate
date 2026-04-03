using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Converters;
public class PointYConverter : IValueConverter
{
    public double Height { get; set; } = 300;
    public double TopPadding { get; set; } = 0;
    public double BottomPadding { get; set; } = 0;

    public double Min { get; set; } = 12;
    public double Max { get; set; } = 50;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return 0.0;

        double v = System.Convert.ToDouble(value);

        double chartHeight = Height - TopPadding - BottomPadding;
        double range = Math.Max(1, Max - Min);
        double normalized = (v - Min) / range;
        double y = TopPadding + chartHeight - normalized * chartHeight;

        return y - 3;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}