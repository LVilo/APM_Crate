using APM_Crate.ViewModels;
using APM_Crate.ViewModels.DialogViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SkiaSharp;

namespace APM_Crate;

public partial class LiveCharts : Window
{
    public LiveCharts()
    {
        InitializeComponent();
    }
    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var isCtrlPressed = e.KeyModifiers.HasFlag(KeyModifiers.Control);

        if (!isCtrlPressed)
            return;

        if (DataContext is LiveChartsViewModel vm)
        {
            if (e.Delta.Y > 0)
            {
                vm.Coef += 0.01d;
            }
            else if (e.Delta.Y < 0)
            {
                vm.Coef -= 0.01d;
            }
        }

        e.Handled = true;
    }
}