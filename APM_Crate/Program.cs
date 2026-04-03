using Avalonia;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using Desktop;
using Serilog;
using System;

namespace APM_Crate
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                OS system = OS.Detect();
                system.CheckDrivers();
                BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                OS.MessageBox(IntPtr.Zero, ex.Message, "Ошибка", 0x00000010 | 0x00000000);
            }

        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}
