using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using Avalonia.Media;


using PortsWork;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DevicesViewModels
{
    public abstract partial class DevicesContext : ViewModelBase
    {
        [JsonIgnore]
        public ReactiveCommand<Unit, Unit> OpenPortCommand { get; }
        [JsonIgnore]
        public ReactiveCommand<Unit, Unit> ClosePortCommand { get; }
        //public ReactiveCommand<Unit, Unit> UpdatesPortsCommand { get; set; }

        [JsonIgnore]
        public string HeaderText { get; set; } = string.Empty;

        //protected string _PortText;
        //[JsonIgnore]
        //public string PortText
        //{
        //    get { return _PortText; }
        //    set { this.RaiseAndSetIfChanged(ref _PortText, value); }
        //}
        [JsonIgnore]
        public string CloseText { get; } = "Отключить";
        [JsonIgnore]
        public string OpenText { get; } = "Подключить";
        [JsonIgnore]
        public int WidthControl { get; } = 100;
        [JsonIgnore]
        public int WidthBorder { get; } = 220;

        public DevicesContext()
        {
            OpenPortCommand = ReactiveCommand.CreateFromTask(OpenPort);
            ClosePortCommand = ReactiveCommand.CreateFromTask(ClosePort);
            //UpdatesPortsCommand = ReactiveCommand.CreateFromTask(UpdatesPorts);
        }

        protected string _DeviceStateColor = "#F0F0F0"; // СЕРЫЙ

        [JsonIgnore]
        public string DeviceStateColor
        {
            get { return _DeviceStateColor; }
            set { this.RaiseAndSetIfChanged(ref _DeviceStateColor, value); }
        }

        protected string? _PortItem;
        public string? PortItem
        {
            get { return _PortItem; }
            set { this.RaiseAndSetIfChanged(ref _PortItem, value); }
        }

        protected string?[] _Ports;
        public string?[] Ports
        {
            get { return _Ports; }
            set
            {
                if (value.Contains(PortItem) is false) { PortItem = null; }
                if (PortItem is null && value is not null) { PortItem = value[0]; }
                this.RaiseAndSetIfChanged(ref _Ports, value);
            }
        }

        protected abstract bool OpenPort_abstract();
        protected abstract void ClosePort_abstract();
        public abstract bool IsOpened();


        private async Task OpenPort()
        {
            try
            {
                await Dialog.ShowLiveCharts();
                //await SQLModel.CreateTableCratePLC();
                await Task.Run(async () =>
                {
                    if (string.IsNullOrEmpty(PortItem))
                    {
                        throw new Exception($"Выбранный порт пуст. Повторно выберите порт для подключения устройства");
                    }

                    if (IsOpened())
                    {
                        LogerViewModel.Instance.Write($"{PortItem} уже подключен.");
                        return;
                    }
                    if (OpenPort_abstract() is true)
                    {
                        DeviceStateColor = "#1DEC1D";
                        LogerViewModel.Instance.Write($"{PortItem} подключен.");
                    }
                    else
                    {
                        ClosePort_abstract();
                        DeviceStateColor = "#F0F0F0";
                        LogerViewModel.Instance.Write($"Не удалось подключиться к {PortItem}.");
                    }
                });
            }
            catch (Exception ex)
            {
                ClosePort_abstract();
                LogerViewModel.Instance.Write("❗" + ex.Message);
            }
        }
        private async Task ClosePort()
        {
            try
            {
                await Task.Run(async () =>
                {
                    ClosePort_abstract();
                    LogerViewModel.Instance.Write($"{PortItem} отключен.");
                    DeviceStateColor = "#F0F0F0";
                });
            }
            catch (Exception ex)
            {
                LogerViewModel.Instance.Write(ex.Message);
            }
        }
        //private async Task UpdatesPorts()
        //{
        //    try
        //    {
        //        await Task.Run(async () =>
        //        {
        //            Ports = Devices.GetAllPorts();
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogerViewModel.Instance.Write(ex.Message);
        //    }
        //}
    }
}
