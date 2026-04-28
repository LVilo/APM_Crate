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
        private async Task OpenPort()
        {
            try
            {
                ////RestModel.IP = "http://127.0.0.1:5000/";
                //RestModel.SetUri();
                //Config conf = new Config
                //{
                //    DeviceType = "241",
                //    SerialNumber = 2320,
                //    OrderNumber = "2314",
                //    Settings = [new Settings { Name = "param1", Value = "321235" }]
                //};

                ////string json = JsonSerializer.Serialize(conf);
                ////HttpClient http = new HttpClient();
                ////http.BaseAddress = new Uri("http://localhost:5000/");
                ////var content = new StringContent(json, Encoding.UTF8, "application/json");
                ////HttpResponseMessage response = await http.PostAsJsonAsync("Configurations", content);
                ////string errorBody = await response.Content.ReadAsStringAsync();


                //await RestModel.Post(conf);
                //await RestModel.GetLastSerialNumber();


                //await http.PostAsJsonAsync("http://localhost:5186/swagger/", content);


                //var response1 = await http.GetFromJsonAsync<object>("Configurations/last-serial/PLC");

                //await SQLModel.TableExistsCratePLCAsync();
                //await SQLModel.TableExistsSettingsAsync();
                //int serial = await SQLModel.GetSerialNumber()+1;
                //string order = "123";
                //string PLC = "PLC241";
                //await SQLModel.WriteNewDevice(serial, order, PLC);

                //var setting = new ParametersSettingPLC
                //(
                //    Environment.UserName,
                //    System.String.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year),
                //    "1",
                //    "IEPE перенастройка",
                //    "23498",
                //    "20:23",
                //    PLC,
                //    "20:43",
                //    serial,
                //    order,

                //   "2",
                //    "1",
                //    "1",
                //   "1",
                //    "1",
                //   "1",
                //    "1",
                //    "1",
                //    "1"
                //);
                //await SQLModel.WriteNewParameters(setting);
                //await Dialog.ShowLiveCharts();
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
