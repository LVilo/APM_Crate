using Avalonia.Media;
using System;
using System.Collections.Generic;
using Avalonia.Data.Converters;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converters
{
    public class PortStatusConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string hex)
            {
                try
                {
                    return Color.Parse(hex);
                }
                catch
                {
                    return Brushes.Transparent;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
