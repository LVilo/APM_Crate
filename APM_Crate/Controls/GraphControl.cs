using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using LiveChartsCore.Measure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls
{
    public class GraphControl : Control
    {
        private double[] values  = new double[0];
        //public double[] Values { get=> values; set { InvalidateVisual(); values = value; } }
        public double[] Values { get; set; } = new double[0];

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (Values.Length == 0)
                return;

            double width = Bounds.Width;
            double height = Bounds.Height;

            double maxValue = double.MinValue;
            double minValue = double.MaxValue;

            foreach (var v in Values)
            {
                if (v > maxValue) maxValue = v;
                if (v < minValue) minValue = v;
            }

            double xStep = width / (Values.Length - 1);

            for (int i = 0; i < Values.Length - 1; i++)
            {
                double x1 = i * xStep;
                double y1 = height - (Values[i] - minValue) / (maxValue - minValue) * height;
                double x2 = (i + 1) * xStep;
                double y2 = height - (Values[i + 1] - minValue) / (maxValue - minValue) * height;

                context.DrawLine(new Pen(Brushes.Blue, 2), new Point(x1, y1), new Point(x2, y2));
            }
        }
    }
}
