using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsWork
{
    public static class ConvertValue
    {
        public static double StringE_ToDouble(string value)
        {
            if(!value.Contains('E')) return Convert.ToDouble(value);
            int indexValue2 = value.IndexOf('E') + 2;
            double d1 = Convert.ToDouble(value[..value.IndexOf('E')].Replace('.',','));
            double d2 = Convert.ToDouble(value[indexValue2..].Replace('.', ',')) * 10;

            double result = value.Contains('+') ? d1 * d2 : d1 / d2;
            return result;
        }
        public static double ToRMS(double value)
        {
            return value / 2d / Math.Sqrt(2d);
        }
        public static double ToPP(double value)
        {
            return value * 2d * Math.Sqrt(2d);
        }
    }
}
