using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using APM_Crate.Models.RestApiModel;

//using APM_Crate.Models.MainFunctions;
using APM_Crate.Service;
using APM_Crate.Views.DialogViews;
using PortsWork;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;
using static APM_Crate.Models.SettingModel;

namespace APM_Crate.ViewModels
{
    public partial class SettingViewModel : ViewModelBase
    {

        [JsonIgnore]
        public ReactiveCommand<Unit, Unit> SettingALL_Command { get; }
        [JsonIgnore]
        public ReactiveCommand<Unit, Unit> Samples_Command { get; }


        [JsonIgnore]
        public double WidthTextBox { get; } = 150;
        [JsonIgnore]
        public double WidthTextBlockTextBox { get; } = 120;
        [JsonIgnore]
        public double WidthComboBox { get; } = 180;
        [JsonIgnore]
        public double WidthTextBlockComboBox { get; } = 170;
        private string? _OrderNumber = "";
        [JsonIgnore]
        public string? OrderNumber
        {
            get { return _OrderNumber; }
            set
            {
                this.RaiseAndSetIfChanged(ref _OrderNumber, value);
                //throw new ArgumentException(nameof(OrderNumber), "Not a valid E-Mail-Address");
            }
        }

        //private string? _SerialNumber = "";

        //[JsonIgnore]
        //public string? SerialNumber
        //{
        //    get { return _SerialNumber; }
        //    set
        //    {
        //        this.RaiseAndSetIfChanged(ref _SerialNumber, value);
        //        //throw new ArgumentException(nameof(OrderNumber), "Not a valid E-Mail-Address");
        //    }
        //}
        [JsonIgnore]
        public ObservableCollection<string> Modules { get => SettingModel.Moduls; set { this.RaiseAndSetIfChanged(ref SettingModel.Moduls, value); } }
        public string ItemModule
        {
            get => SettingModel.ItemModule;
            set { this.RaiseAndSetIfChanged(ref SettingModel.ItemModule, value); }
        }
        [JsonIgnore]
        public ObservableCollection<string> PLC { get; } = SettingModel.PLC;
        public string ItemPLC
        {
            get => SettingModel.ItemPLC;
            set
            { 
                Coef_Enaibled = value is "241" or "242" or "371" or "375";
                Channel1.CanSetting = true;
                Channel2.CanSetting = true;
                Channel3.CanSetting = value is "371" or "374" or "375" or "511";
                Channel4.CanSetting = value is "511";

                Channel3.SettingChannel = value is "371" or "374" or "375" or "511";
                Channel4.SettingChannel = value is "511";
                this.RaiseAndSetIfChanged(ref SettingModel.ItemPLC, value);
            }
        }
        [JsonIgnore]
        public bool Coef
        {
            get => SettingModel.Coef_Is_10;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Coef_Is_10, value); }
        }
        public bool Coef_Enaibled
        {
            get => SettingModel.CoefEnaibled;
            set {this.RaiseAndSetIfChanged(ref SettingModel.CoefEnaibled, value);  }
        }
        public SettingViewModel()
        {
            ItemPLC = "241";
            SettingALL_Command = ReactiveCommand.CreateFromTask(Setting);
            Samples_Command = ReactiveCommand.CreateFromTask(Samples);

        }
        private bool _IsEnabledButtons = true;
        [JsonIgnore]
        public bool IsEnabledButtons
        {
            get { return _IsEnabledButtons; }
            set { this.RaiseAndSetIfChanged(ref _IsEnabledButtons, value); }
        }
        private async Task Samples()
        {
            try
            {
                IsEnabledButtons = false;
                if (Devices.Crate.Connected is false) throw new Exception("Необходимо подключится к крейту");
                CheckFilePLC.IndexMudule = Convert.ToInt32(ItemModule) - 1;
                Devices.Crate.WriteUInt16(Channel1.OnSaw, 1);
                Devices.Crate.WriteUInt16(Channel2.OnSaw, 1);
                Devices.Crate.WriteUInt16(3, 5);
                Thread.Sleep(3000);
                Devices.Crate.WriteUInt16(4, 1);
                do
                {
                    Thread.Sleep(500);
                }
                while (CheckFilePLC.GetBigEndian());
                byte[] data = await CheckFilePLC.DownloadFile();
                Devices.Crate.WriteUInt16(Channel1.OnSaw, 0);
                Devices.Crate.WriteUInt16(Channel2.OnSaw, 0);
                await Dialog.ShowLiveCharts(data);
            }
            catch (Exception ex)
            {
                LogerViewModel.Instance.Write("❗ " +ex.Message);
            }
            finally
            {
                IsEnabledButtons = true;
            }
        }
        Stopwatch stopwatch = new Stopwatch();
        private async Task Setting()
        {
            try
            {
                RestModel.SetUri();
                IsEnabledButtons = false;
                if (Devices.Crate.Connected is false) throw new Exception("Необходимо подключится к крейту");
                if (string.IsNullOrEmpty(OrderNumber))
                {
                    throw new Exception($"Введите номер заказа.");
                }
                ushort type = Devices.Crate.ReadUInt16(Crate.Registers.Type);
                if (ItemPLC != PLC[type - 1])
                {
                    await Dialog.ShowConfirm($"Выбранный тип контроллера не соответствует типу, записанному на контроллер. Настроить контроллер PLC.{PLC[type - 1]} как тип PLC.{ItemPLC} ?", true);
                }

                //await SQLModel.TableExistsCratePLCAsync();

                SerialNumber = Devices.Crate.ReadUInt16(Crate.Registers.SerialNum);
                if (SerialNumber != 65535)
                {
                    await Dialog.ShowResetting();
                    IsResetting = true;//в устройстве записан серийный номер
                }
                else
                {
                    SerialNumber = Convert.ToUInt16(await RestModel.GetLastSerialNumber() + 1);
                    IsResetting = false;
                }



                //if (IsResetting is false) 

                //if (SerialNumber is 0)
                //{
                //    throw new Exception($"Введите серийный номер.");
                //}
                if ((SerialNumber >= 0 && SerialNumber <= 65535) is false)
                {
                    throw new Exception($"Серийный номер должен быть в диапазоне 0-65535.");
                }
                
                string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                
                stopwatch.Restart();
                settings = new List<Settings>();
                await Start(ItemPLC);
                stopwatch.Stop();
                string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                Config config = new Config
                {
                    DeviceType = ItemPLC,
                    SerialNumber = SerialNumber,
                    OrderNumber = OrderNumber,
                    Settings = settings,
                };

                await SaveRegistersModel.MakeReportAsync(ItemPLC, OrderNumber, starttime,endtime,stopwatch.Elapsed);


                if (IsResetting is false)
                {
                    await RestModel.Post(config);
                    //await SQLModel.WriteNewDevice(SettingModel.SerialNumber,OrderNumber,ItemPLC);
                    await Devices.Printer.PrintText(SettingModel.SerialNumber.ToString());
                    Devices.Crate.WriteUInt16(Crate.Registers.SerialNum, SettingModel.SerialNumber);
                }
                else
                {
                    List<Config>list =  await RestModel.GetListRecord(50, SerialNumber, OrderNumber, ItemPLC);
                    string id = list[0].Id;
                    await RestModel.Delete(id);
                    await RestModel.Post(config);
                }
                //LogerViewModel.Instance.Write($"Настройка заняла {stopwatch.Elapsed:mm\\ss}");
            }
            catch (Exception e)
            {
                LogerViewModel.Instance.Write("❗ " +e.Message);
            }
            finally
            {
                IsEnabledButtons = true;
                SaveRegistersModel.Parameters = new();
            }
        }
    }
}
