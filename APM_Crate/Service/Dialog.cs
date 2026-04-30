using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using APM_Crate.ViewModels;
using APM_Crate.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Controls.LineChart;
using DynamicData;
using System.Reactive.Linq;

namespace APM_Crate.Service
{
    public interface IGetVoltege
    {
        Task GetVolt();
    }
    public class ADC : IGetVoltege
    {
        public async Task GetVolt()
        {
            Devices.Multimeter.GetAmperage();
            await Task.Delay(500);
        }
    }
    public class DC : IGetVoltege
    {
        public async Task GetVolt()
        {
            Devices.Multimeter.GetVoltage("DC", 100);
            await Task.Delay(500);
        }
    }
    public class AC : IGetVoltege
    {
        public async Task GetVolt()
        {
            Devices.Multimeter.GetVoltage("AC", 100);
            await Task.Delay(500);
        }
    }
    public class A : IGetVoltege
    {
        public async Task GetVolt()
        {
            Devices.Multimeter.GetAmperage();
            await Task.Delay(500);
        }
    }
    public class Delay : IGetVoltege
    {
        public async Task GetVolt() => await Task.Delay(2000);
    }
    public static class Dialog
    {
        public static IGetVoltege Mult = new Delay();
        public static async Task WhileGetVoltAsync()
        {
            try
            {
                while (true)
                {
                    await Mult.GetVolt();
                }
            }
            catch
            {

            }
        }
        public static DialogService DialogService;
        public static async Task ShowBuild(string settings,string mes="Соберите схему.")
        {
            var vm = new BuildSchemeViewModel();
            vm.Messege = mes;
            vm.SetBitmap($"avares://APM_Crate/Assets/{settings}.png");
            await DialogService.ShowBuildAsync(vm);
        }
        public static async Task ShowParam()
        {
            var vm = new ParamDialogViewModel();
            await DialogService.ShowParamAsync(vm);
        }
        public static async Task ShowLiveCharts(byte[] data)
        {
            var vm = new LiveChartsViewModel();
            CheckFilePLC.GetValuesChannel(data, out ushort[] Channel1);
            CheckFilePLC.GetValuesChannel(data, out ushort[] Channel2);

            vm.Points_Chanel1 = new ObservableCollection<ChartPoint> { Channel1.Select((value, index) => new ChartPoint { X = index, Y = value }) };
            vm.Points_Chanel2 = new ObservableCollection<ChartPoint> { Channel2.Select((value, index) => new ChartPoint { X = index, Y = value }) };
            await DialogService.ShowLiveChartsAsync(vm);
        }
        public static async Task ShowResetting()
        {
            var vm = new ResettingViewModel();
            await DialogService.ShowResettingAsync(vm);
        }
        public static async Task ShowConfirm(string mes, IGetVoltege type, bool YesOrNot = false)
        {
            Mult = type;
            var vm = new ConfirmDialogViewModel();
            if (YesOrNot)
            {
                vm.ConfirmText = "Да";
                vm.CancelText = "Нет";
            }
            else
            {
                vm.ConfirmText = "Ок";
                vm.CancelText = "Отмена";
            }
            vm.Messege = mes;
            await DialogService.ShowConfirmAsync(vm);
            Mult = new Delay();
        }
        public static async Task ShowRestAPI_IP()
        {
            var vm = new RestAPIViewModel();
            await DialogService.ShowRestAPIAsync(vm);
        }
    }
}

