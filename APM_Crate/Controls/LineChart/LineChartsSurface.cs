using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Controls.LineChart;

public class LineChartSurface : Control
{
    public static readonly StyledProperty<ObservableCollection<ChartPoint>?> PointsProperty =
        AvaloniaProperty.Register<LineChartSurface, ObservableCollection<ChartPoint>?>(nameof(Points));

    public ObservableCollection<ChartPoint>? Points
    {
        get => GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    private const double LeftPadding = 20;
    private const double RightPadding = 20;
    private const double TopPadding = 20;
    private const double BottomPadding = 20;

    static LineChartSurface()
    {
        AffectsRender<LineChartSurface>(PointsProperty);
    }

    public LineChartSurface()
    {
        this.GetObservable(PointsProperty).Subscribe(OnPointsChanged);
    }

    private ObservableCollection<ChartPoint>? _oldPoints;

    private void OnPointsChanged(ObservableCollection<ChartPoint>? newPoints)
    {
        if (_oldPoints != null)
        {
            _oldPoints.CollectionChanged -= Points_CollectionChanged;

            foreach (var p in _oldPoints)
                p.PropertyChanged -= Point_PropertyChanged;
        }

        _oldPoints = newPoints;

        if (newPoints != null)
        {
            newPoints.CollectionChanged += Points_CollectionChanged;

            foreach (var p in newPoints)
                p.PropertyChanged += Point_PropertyChanged;
        }

        InvalidateVisual();
    }

    private void Points_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (ChartPoint p in e.OldItems)
                p.PropertyChanged -= Point_PropertyChanged;
        }

        if (e.NewItems != null)
        {
            foreach (ChartPoint p in e.NewItems)
                p.PropertyChanged += Point_PropertyChanged;
        }

        InvalidateVisual();
    }

    private void Point_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        context.FillRectangle(Brushes.Transparent, bounds);

        double chartWidth = Math.Max(1, bounds.Width - LeftPadding - RightPadding);
        double chartHeight = Math.Max(1, bounds.Height - TopPadding - BottomPadding);

        var chartRect = new Rect(LeftPadding, TopPadding, chartWidth, chartHeight);

        DrawAxesFrame(context, chartRect);

        if (Points == null || Points.Count == 0)
            return;

        double minX = Points.Min(p => p.X);
        double maxX = Points.Max(p => p.X);
        double minY = Points.Min(p => p.Y);
        double maxY = Points.Max(p => p.Y);

        AddMargins(ref minX, ref maxX, ref minY, ref maxY);

        double stepX = CalculateNiceStep(maxX - minX, 8);
        double stepY = CalculateNiceStep(maxY - minY, 6);

        minX = Math.Floor(minX / stepX) * stepX;
        maxX = Math.Ceiling(maxX / stepX) * stepX;
        minY = Math.Floor(minY / stepY) * stepY;
        maxY = Math.Ceiling(maxY / stepY) * stepY;

        DrawGrid(context, chartRect, minX, maxX, minY, maxY, stepX, stepY);
        DrawZeroAxes(context, chartRect, minX, maxX, minY, maxY);

        using (context.PushClip(chartRect))
        {
            DrawLineSeries(context, chartRect, minX, maxX, minY, maxY);
        }
    }

    private void DrawAxesFrame(DrawingContext context, Rect chartRect)
    {
        var axisPen = new Pen(Brushes.Black, 1);

        context.DrawLine(axisPen,
            new Point(chartRect.Left, chartRect.Bottom),
            new Point(chartRect.Right, chartRect.Bottom));
                        context.DrawLine(axisPen,
                        new Point(chartRect.Left, chartRect.Top),
                        new Point(chartRect.Left, chartRect.Bottom));
    }

    private void DrawZeroAxes(DrawingContext context, Rect chartRect, double minX, double maxX, double minY, double maxY)
    {
        var zeroPen = new Pen(new SolidColorBrush(Color.Parse("#666666")), 1.5);

        if (minY <= 0 && maxY >= 0)
        {
            double y0 = DataToScreenY(0, minY, maxY, chartRect);
            context.DrawLine(zeroPen, new Point(chartRect.Left, y0), new Point(chartRect.Right, y0));
        }

        if (minX <= 0 && maxX >= 0)
        {
            double x0 = DataToScreenX(0, minX, maxX, chartRect);
            context.DrawLine(zeroPen, new Point(x0, chartRect.Top), new Point(x0, chartRect.Bottom));
        }
    }

    private void DrawGrid(
        DrawingContext context,
        Rect chartRect,
        double minX, double maxX,
        double minY, double maxY,
        double stepX, double stepY)
    {
        var gridPen = new Pen(new SolidColorBrush(Color.Parse("#000000")), 1);
        var textBrush = Brushes.Black;

        for (double x = minX; x <= maxX + stepX * 0.5; x += stepX)
        {
            double sx = DataToScreenX(x, minX, maxX, chartRect);

            context.DrawLine(gridPen, new Point(sx, chartRect.Top), new Point(sx, chartRect.Bottom));
            DrawText(context, x.ToString("0.##", CultureInfo.InvariantCulture),
                new Point(sx - 12, chartRect.Bottom + 6), textBrush, 12);
        }

        for (double y = minY; y <= maxY + stepY * 0.5; y += stepY)
        {
            double sy = DataToScreenY(y, minY, maxY, chartRect);

            context.DrawLine(gridPen, new Point(chartRect.Left, sy), new Point(chartRect.Right, sy));
            DrawText(context, y.ToString("0.##", CultureInfo.InvariantCulture),
                new Point(20, sy - 15), textBrush, 12);
        }
    }

    private void DrawLineSeries(
        DrawingContext context,
        Rect chartRect,
        double minX, double maxX,
        double minY, double maxY)
    {
        if (Points == null)
            return;

        var ordered = Points.OrderBy(p => p.X).ToList();

        var linePen = new Pen(Brushes.Blue, 2);
        //var pointPen = new Pen(Brushes.Orange, 1);

        Point? prev = null;

        foreach (var p in ordered)
        {
            double sx = DataToScreenX(p.X, minX, maxX, chartRect);
            double sy = DataToScreenY(p.Y, minY, maxY, chartRect);

            var screen = new Point(sx, sy);

            if (prev != null)
                context.DrawLine(linePen, prev.Value, screen);

            //context.DrawEllipse(Brushes.Orange, pointPen, screen, 4, 4);

            prev = screen;
        }
    }

    private double DataToScreenX(double x, double minX, double maxX, Rect chartRect)
    {
        double range = Math.Max(1e-9, maxX - minX);
        return chartRect.Left + ((x - minX) / range) * chartRect.Width;
    }

    private double DataToScreenY(double y, double minY, double maxY, Rect chartRect)
    {
        double range = Math.Max(1e-9, maxY - minY);
        return chartRect.Bottom - ((y - minY) / range) * chartRect.Height;
    }

    private void AddMargins(ref double minX, ref double maxX, ref double minY, ref double maxY)
    {
        double rangeX = maxX - minX;
        double rangeY = maxY - minY;

        if (rangeX == 0)
        {
            minX -= 1;
            maxX += 1;
        }
        else
        {
            double padX = rangeX * 0.08;
            minX -= padX;
            maxX += padX;
        }

        if (rangeY == 0)
        {
            minY -= 1;
            maxY += 1;
        }
        else
        {
            double padY = rangeY * 0.08;
            minY -= padY;
            maxY += padY;
        }
    }

    private double CalculateNiceStep(double range, int targetLines)
    {
        if (range <= 0)
            return 1;
        double rough = range / targetLines;
        double exponent = Math.Floor(Math.Log10(rough));
        double fraction = rough / Math.Pow(10, exponent);

        double niceFraction =
            fraction <= 1 ? 1 :
            fraction <= 2 ? 2 :
            fraction <= 5 ? 5 : 10;

        return niceFraction * Math.Pow(10, exponent);
    }

    private void DrawText(DrawingContext context, string text, Point origin, IBrush brush, double size)
    {
        var ft = new FormattedText(
            text,
            CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Segoe UI"),
            size,
            brush);

        context.DrawText(ft, origin);
    }
}
