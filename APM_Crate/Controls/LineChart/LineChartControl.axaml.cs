using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;

namespace Controls.LineChart;

public partial class LineChartControl : UserControl
{
    public static readonly StyledProperty<ObservableCollection<ChartPoint>?> PointsProperty =
        AvaloniaProperty.Register<LineChartControl, ObservableCollection<ChartPoint>?>(nameof(Points));

    public ObservableCollection<ChartPoint>? Points
    {
        get => GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public LineChartControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}