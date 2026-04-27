using APM_Crate.Models.DevicesModel;

using APM_Crate.ViewModels;
using APM_Crate.ViewModels.DialogViewModels;
using APM_Crate.Views;
using APM_Crate.Views.DialogViews;
using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace APM_Crate.Service
{
    public class DialogService
    {
        private readonly Window _mainWindow;

        //public static PixelPoint? _lastDialogPosition { get; set; }

        public static int X { get; set; }
        public static int Y { get; set; }

        public static double W { get; set; }
        public static double H { get; set; }


        public DialogService(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }
        //public async Task ShowParamCapacityAsync(DialogViewModel vm)
        //{
        //    var dialog = new ParamCapacityDialogView();
        //    await Show(dialog, vm);
        //}
        //public async Task ShowParamCNVAsync(DialogViewModel vm)
        //{
        //    var dialog = new ParamDialogView();
        //    await Show(dialog, vm);
        //}
        //public async Task ShowGlobalSettingAsync(DialogViewModel vm)
        //{
        //    var dialog = new GlobalSettingsView();
        //    await Show(dialog, vm);
        //}
        //public async Task ShowCheckSettingAsync(DialogViewModel vm)
        //{
        //    var dialog = new CheckSettingView();
        //    await Show(dialog, vm);
        //}
        //public async Task ShowCheckSettingCNV157xAsync(DialogViewModel vm)
        //{
        //    var dialog = new CheckSettingsCNV157xView();
        //    await Show(dialog, vm);
        //}
        public async Task ShowBuildAsync(DialogViewModel vm)
        {
            var dialog = new BuildSchemeView();
            await Show(dialog, vm);
        }
        public async Task ShowConfirmAsync(DialogViewModel vm)
        {
            var dialog = new ConfirmDialogView();
            await Show(dialog, vm);
        }
        public async Task ShowParamAsync(DialogViewModel vm)
        {
            var dialog = new ParamDialogView();
            await Show(dialog, vm);
        }
        public async Task ShowLiveChartsAsync(DialogViewModel vm)
        {
            var dialog = new LiveCharts();
            await Show(dialog, vm);
        }
        public async Task ShowResettingAsync(DialogViewModel vm)
        {
            var dialog = new ResettingView();
            await Show(dialog, vm);
        }
        public async Task ShowRestAPIAsync(DialogViewModel vm)
        {
            var dialog = new RestAPIView();
            await Show(dialog, vm);
        }
        //public async Task ShowParamCNV127Async(DialogViewModel vm)
        //{
        //    var dialog = new ParamCNV127DialogView();

        //    await Show(dialog, vm);
        //}
        //public async Task ShowParamCNV137Async(DialogViewModel vm)
        //{
        //    var dialog = new ParamCNV137DialogView();

        //    await Show(dialog, vm);
        //}
        //public async Task ShowParamCNV157Async(DialogViewModel vm)
        //{
        //    var dialog = new ParamCNV157DialogView();
        //    await Show(dialog, vm);
        //}
        //public async Task ShowParamOtherAsync(DialogViewModel vm)
        //{
        //    var dialog = new ParamCNVOtherDialogView();
        //    await Show(dialog, vm);
        //}
        private async Task Show(Window dialog, DialogViewModel vm)
        {
            vm.CloseAction = result => dialog.Close(result);
            dialog.DataContext = vm;
            //dialog.Position = new PixelPoint(X, Y);
            dialog.Opened += (_, _) =>
            {
                var screen = dialog.Screens.Primary;
                var x = (screen.WorkingArea.Width - dialog.Width) / 2;
                var y = (screen.WorkingArea.Height - dialog.Height) / 2;

                dialog.Position = new PixelPoint((int)x, (int)y);
            };
            //dialog.PositionChanged += (_, _) =>
            //{
            //    X = dialog.Position.X;
            //    Y = dialog.Position.Y;
            //};
            if (dialog is BuildSchemeView build)
            {
                build.Width = W;
                build.Height = H;
                dialog.SizeChanged += (_, _) =>
                {
                    W = build.Width;
                    H = build.Height;
                };
            }
            dialog.Focus();
            if (await dialog.ShowDialog<bool>(_mainWindow) is false)
            {
                vm.Confirmed = false;
                throw new Exception("Отмена");
            }
        }
    }
}
