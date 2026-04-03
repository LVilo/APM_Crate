using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace Converters;

public class ValuesToPointsConverter : IValueConverter
{
    public double Width { get; set; } = 600;
    public double Height { get; set; } = 300;

    public double LeftPadding { get; set; } = 0;
    public double RightPadding { get; set; } = 0;
    public double TopPadding { get; set; } = 10;
    public double BottomPadding { get; set; } = 50;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IEnumerable enumerable)
            return new Points();

        var values = enumerable.Cast<double>().ToList();

        if (values.Count == 0)
            return new Points();

        double chartWidth = Width - LeftPadding - RightPadding;
        double chartHeight = Height - TopPadding - BottomPadding;

        double min = values.Min();
        double max = values.Max();
        double range = Math.Max(1, max - min);

        var points = new Points();

        for (int i = 0; i < values.Count; i++)
        {
            double x = LeftPadding + i * (chartWidth / Math.Max(1, values.Count - 1));
            double normalized = (values[i] - min) / range;
            double y = TopPadding + chartHeight - normalized * chartHeight;

            points.Add(new Point(x, y));
        }

        return points;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}