using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Controls;

public partial class UserLiveCharts : Control
{
    public UserLiveCharts()
    {
        InitializeComponent();
        AffectsRender<UserLiveCharts>(DataProperty);
    }

    private bool _isDragging = false;
    private Point _lastPointerPosition;


    public static readonly StyledProperty<ushort[]?> DataProperty =
            AvaloniaProperty.Register<UserLiveCharts, ushort[]?>(nameof(Data));

    public ushort[]? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public int StartIndex { get; private set; } = 0;
    public int VisibleCount { get; private set; } = 2048;

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var data = Data;
        if (data == null || data.Length < 2)
            return;

        double width = Bounds.Width;
        double height = Bounds.Height;

        if (width <= 1 || height <= 1)
            return;

        context.FillRectangle(Brushes.Black, new Rect(0, 0, width, height));
        DrawGrid(context, width, height);

        int endIndex = Math.Min(StartIndex + VisibleCount, data.Length);
        int count = endIndex - StartIndex;

        if (count < 2)
            return;

        var pen = new Pen(Brushes.Lime, 1);
        var geometry = new StreamGeometry();

        using (var geo = geometry.Open())
        {
            for (int x = 0; x < (int)width; x++)
            {
                int localIndex = x * count / (int)width;
                int dataIndex = StartIndex + localIndex;

                if (dataIndex >= data.Length)
                    break;

                ushort value = data[dataIndex];
                double y = height - (value / 16384.0) * height;

                if (x == 0)
                    geo.BeginFigure(new Point(x, y), false);
                else
                    geo.LineTo(new Point(x, y));
            }
        }

        context.DrawGeometry(null, pen, geometry);

        DrawInfo(context, width, height, count);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        var data = Data;
        if (data == null || data.Length < 2)
            return;

        int oldVisible = VisibleCount;

        if (e.Delta.Y > 0)
        {
            // Zoom in
            VisibleCount = Math.Max(32, VisibleCount / 2);
        }
        else if (e.Delta.Y < 0)
        {
            // Zoom out
            VisibleCount = Math.Min(data.Length, VisibleCount * 2);
        }

        // Центрируем zoom вокруг текущего центра окна
        int center = StartIndex + oldVisible / 2;
        StartIndex = center - VisibleCount / 2;

        if (StartIndex < 0)
            StartIndex = 0;

        if (StartIndex + VisibleCount > data.Length)
            StartIndex = data.Length - VisibleCount;

        if (StartIndex < 0)
            StartIndex = 0;

        InvalidateVisual();
    }

    private void DrawGrid(DrawingContext context, double width, double height)
    {
        var gridPen = new Pen(new SolidColorBrush(Color.FromArgb(60, 255, 255, 255)), 1);

        int verticalLines = 10;
        int horizontalLines = 8;

        for (int i = 1; i < verticalLines; i++)
        {
            double x = width * i / verticalLines;
            context.DrawLine(gridPen, new Point(x, 0), new Point(x, height));
        }

        for (int i = 1; i < horizontalLines; i++)
        {
            double y = height * i / horizontalLines;
            context.DrawLine(gridPen, new Point(0, y), new Point(width, y));
        }
    }

    private void DrawInfo(DrawingContext context, double width, double height, int count)
    {
        var text = new FormattedText(
            $"Start: {StartIndex}   Visible: {count}",
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Arial"),
            14,
            Brushes.White);

        context.DrawText(text, new Point(10, 10));
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        _isDragging = true;
        _lastPointerPosition = e.GetPosition(this);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        _isDragging = false;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!_isDragging || Data == null)
            return;

        var currentPos = e.GetPosition(this);
        double dx = currentPos.X - _lastPointerPosition.X;

        int shift = (int)(-dx * VisibleCount / Bounds.Width);

        StartIndex += shift;

        if (StartIndex < 0)
            StartIndex = 0;

        if (StartIndex + VisibleCount > Data.Length)
            StartIndex = Data.Length - VisibleCount;

        if (StartIndex < 0)
            StartIndex = 0;

        _lastPointerPosition = currentPos;
        InvalidateVisual();
    }
}