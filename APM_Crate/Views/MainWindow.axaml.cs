using APM_Crate.Models;
using APM_Crate.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Input;
using Avalonia.Interactivity;
using APM_Crate.Models.DevicesModel;
using Avalonia.Fonts.Inter;
using System.Threading.Tasks;

namespace APM_Crate.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;

            

        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LogerViewModel.Instance.Write("Приложение закрывается");
            //ControllerStorage.Save();
            PortsStorage.Save();
            //Devices.cnv.ClosePort();
            Devices.Multimeter.ClosePort();
            Devices.Generator.ClosePort();
            Devices.Crate.Disconnect();
            //Devices.Sg004.ClosePort();
            //Devices.TIK_BIS.ClosePort();
            //Devices.MY210_402.ClosePort();
        }
        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }
        private void CloseWin(object? sender, RoutedEventArgs e) => Close();
        private void MinimizeWin(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}