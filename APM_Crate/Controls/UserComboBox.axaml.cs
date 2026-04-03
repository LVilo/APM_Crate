using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using System.Collections;


namespace Controls;

public partial class UserComboBox : UserControl
{
    public UserComboBox()
    {
        InitializeComponent();
    }
    public static readonly StyledProperty<string> HeaderProperty = AvaloniaProperty.Register<UserComboBox, string>( nameof(Header), "Header:", defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    public static readonly StyledProperty<IEnumerable?> UserItemsProperty = AvaloniaProperty.Register<UserComboBox, IEnumerable?>( nameof(UserItems),defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
    public IEnumerable? UserItems
    {
        get => GetValue(UserItemsProperty);
        set => SetValue(UserItemsProperty, value);
    }
    public static readonly StyledProperty<string> UserSelectedItemProperty = AvaloniaProperty.Register<UserComboBox, string>(nameof(UserSelectedItem), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
    public string UserSelectedItem
    {
        get => GetValue(UserSelectedItemProperty);
        set => SetValue(UserSelectedItemProperty, value);
    }
    public static readonly StyledProperty<double> UserWidthProperty = AvaloniaProperty.Register<UserComboBox, double>(nameof(UserWidth),70d, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);
    public double UserWidth
    {
        get => GetValue(UserWidthProperty);
        set => SetValue(UserWidthProperty, value);
    }
}