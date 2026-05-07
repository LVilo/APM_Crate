using APM_Crate.Models;
using APM_Crate.Models.RestApiModel;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using System.Text;
using System.Text.Json;
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

        public bool _IsEnabled = true;
        [JsonIgnore]
        public bool IsEnabled
        {
            get => _IsEnabled;
            set { this.RaiseAndSetIfChanged(ref _IsEnabled, value); }
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

        protected abstract Task<bool> OpenPort_abstract();
        protected abstract Task ClosePort_abstract();
        public abstract bool IsOpened();


        //public class DeviceConfig
        //{
        //    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        //    public string? Id { get; set; } = null;

        //    public string Arm { get; set; }
        //    public string DeviceFamily { get; set; }
        //    public string DeviceType { get; set; }
        //    public ulong SerialNumber { get; set; }
        //    public string OrderNumber { get; set; }
        //    public bool IsActual { get; set; }
        //    public List<Settings> Settings { get; set; }
        //}
        //public class Settings
        //{
        //    public string Name { get; set; }
        //    public string Value { get; set; }
        //}
        public async Task OpenPort()
        {
            try
            {
                IsEnabled = false;
                if (string.IsNullOrEmpty(PortItem))
                {
                    throw new Exception($"Выбранный порт пуст. Повторно выберите порт для подключения устройства");
                }

                if (IsOpened())
                {
                    await LogerViewModel.Instance.Write($"{PortItem} уже подключен.");
                    return;
                }
                if (await OpenPort_abstract() is true)
                {
                    DeviceStateColor = "#1DEC1D";
                    await LogerViewModel.Instance.Write($"{PortItem} подключен.");
                }
                else
                {
                    await ClosePort_abstract();
                    DeviceStateColor = "#F0F0F0";
                    await LogerViewModel.Instance.Write($"Не удалось подключиться к {PortItem}.");
                }
            }
            catch (Exception ex)
            {
                await ClosePort_abstract();
                await LogerViewModel.Instance.Write("❗" + ex.Message);
            }
            finally
            {
                IsEnabled = true;
            }
        }
        public async Task ClosePort()
        {
            try
            {
                IsEnabled = false;
                await ClosePort_abstract();
                await LogerViewModel.Instance.Write($"{PortItem} отключен.");
                DeviceStateColor = "#F0F0F0";
            }
            catch (Exception ex)
            {
                await LogerViewModel.Instance.Write(ex.Message);
            }
            finally
            {
                IsEnabled = true;
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
        //        await LogerViewModel.Instance.Write(ex.Message);
        //    }
        //}
    }
}
