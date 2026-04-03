using APM_Crate.Service;
using APM_Crate.ViewModels;
using APM_Crate.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace APM_Crate
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                Dialog.DialogService = new DialogService(desktop.MainWindow);
                desktop.MainWindow.DataContext = new MainWindowViewModel();
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}