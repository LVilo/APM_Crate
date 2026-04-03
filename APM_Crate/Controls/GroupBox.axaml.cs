using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Controls;

public partial class GroupBox : UserControl
{
    public static readonly StyledProperty<string> HeaderProperty =
    AvaloniaProperty.Register<GroupBox, string>(
        nameof(Header), "Header",
        defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string> ColorProperty =
    AvaloniaProperty.Register<GroupBox, string>(
        nameof(Color),  "#F0F0F0",
        defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    //public static readonly StyledProperty<double> WidthBorderProperty =
    //AvaloniaProperty.Register<GroupBox, double>(
    //    nameof(WidthBorder), 330,
    //    defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<object?> ContentControlProperty =
    AvaloniaProperty.Register<GroupBox, object?>(
        nameof(ContentControl),
        defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    public string Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
    //public double WidthBorder
    //{
    //    get => GetValue(WidthBorderProperty);
    //    set => SetValue(WidthBorderProperty, value);
    //}
    public object? ContentControl
    {
        get => GetValue(ContentControlProperty);
        set => SetValue(ContentControlProperty, value);
    }
    public GroupBox()
    {
        InitializeComponent();
    }
}