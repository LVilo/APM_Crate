using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
//using APM_Crate.Models.MainFunctions;
using APM_Crate.Service;
using PortsWork;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;

namespace APM_Crate.ViewModels
{
    public partial class SettingViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> SettingALL_Command { get; }
        //public ReactiveCommand<Unit, Unit> SettingOUT_1_Command { get; }
        //public ReactiveCommand<Unit, Unit> SettingOUT_2_Command { get; }
        //public ReactiveCommand<Unit, Unit> CheckSetting_Command { get; }
        //public ReactiveCommand<Unit, Unit> UserSettings_Command { get; }


        public double WidthTextBox { get; } = 150;
        public double WidthTextBlockTextBox { get; } = 120;
        public double WidthComboBox { get; } = 180;
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

        private string? _SerialNumber = "";

        [JsonIgnore]
        public string? SerialNumber
        {
            get { return _SerialNumber; }
            set
            {
                this.RaiseAndSetIfChanged(ref _SerialNumber, value);
                //throw new ArgumentException(nameof(OrderNumber), "Not a valid E-Mail-Address");
            }
        }
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
                CanSettingChannel_1 = true;
                CanSettingChannel_2 = true;
                CanSettingChannel_3 = value is "371" or "374" or "375" or "511";
                CanSettingChannel_4 = value is "511";
                this.RaiseAndSetIfChanged(ref SettingModel.ItemPLC, value);
            }
        }
        [JsonIgnore]
        public bool IsResetting
        {
            get => SettingModel.IsResetting;
            set { this.RaiseAndSetIfChanged(ref SettingModel.IsResetting, value); }
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
        public bool _CanSettingChannel_1 = true;
        public bool CanSettingChannel_1
        {
            get => _CanSettingChannel_1;
            set
            {
                this.RaiseAndSetIfChanged(ref _CanSettingChannel_1, value);
                SettingChannel_1 = value;
            }
        }
        public bool _CanSettingChannel_2 = true;
        public bool CanSettingChannel_2
        {
            get => _CanSettingChannel_2;
            set
            { 
                this.RaiseAndSetIfChanged(ref _CanSettingChannel_2, value);
                SettingChannel_2 = value;
            }
        }
        public bool _CanSettingChannel_3 = true;
        public bool CanSettingChannel_3
        {
            get => _CanSettingChannel_3;
            set 
            {
                this.RaiseAndSetIfChanged(ref _CanSettingChannel_3, value);
                SettingChannel_3 = value;
            }
        }
        public bool _CanSettingChannel_4 = false;
        public bool CanSettingChannel_4
        {
            get => _CanSettingChannel_4;
            set
            { 
                this.RaiseAndSetIfChanged(ref _CanSettingChannel_4, value);
                SettingChannel_4 = value;
            }
        }
        public bool SettingChannel_1
        {
            get => SettingModel.Channel1.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel1.SettingChannel, value); }
        }
        public bool SettingChannel_2
        {
            get => SettingModel.Channel2.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel2.SettingChannel, value); }
        }
        public bool SettingChannel_3
        {
            get => SettingModel.Channel3.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel3.SettingChannel, value); }
        }
        public bool SettingChannel_4
        {
            get => SettingModel.Channel4.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel4.SettingChannel, value); }
        }
        public SettingViewModel()
        {
            ItemPLC = "241";
            SettingALL_Command = ReactiveCommand.CreateFromTask(Setting);
            //SettingOUT_1_Command = ReactiveCommand.CreateFromTask(SettingOUT_1);
            //SettingOUT_2_Command = ReactiveCommand.CreateFromTask(SettingOUT_2);
            //CheckSetting_Command = ReactiveCommand.CreateFromTask(CheckSetting);
            //UserSettings_Command = ReactiveCommand.CreateFromTask(UserSetting);
        }
        private bool _IsEnabledButtons = true;
        public bool IsEnabledButtons
        {
            get { return _IsEnabledButtons; }
            set { this.RaiseAndSetIfChanged(ref _IsEnabledButtons, value); }
        }
        Stopwatch stopwatch = new Stopwatch();
        private async Task Setting()
        {
            try
            {
                //await Dialog.ShowConfirm("OpenText", false);

                IsEnabledButtons = false;

                await SQLModel.TableExistsCratePLCAsync();

                if (IsResetting is false) SerialNumber = (await SQLModel.GetSerialNumber() + 1).ToString();

                if (string.IsNullOrEmpty(SerialNumber))
                {
                    throw new Exception($"Введите серийный номер.");
                }
                if ((Convert.ToUInt16(SerialNumber) >= 0 && Convert.ToUInt16(SerialNumber) <= 65535) is false)
                {
                    throw new Exception($"Серийный номер должен быть в диапазоне 0-65535.");
                }
                if (string.IsNullOrEmpty(OrderNumber))
                {
                    throw new Exception($"Введите номер заказа.");
                }

                
                //MainSetting.CheckDeviceForSetting();
                //MainSetting.CheckDeviceForCheckSetting();
                string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                stopwatch.Restart();
                await SettingModel.Start(ItemPLC);
                stopwatch.Stop();
                string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                string namedevice = ItemPLC;
                string settings = IsResetting is false ? "Настройка" : "Перенастройка";
                await SaveRegistersModel.MakeReportAsync(namedevice,OrderNumber, settings, starttime,endtime,stopwatch.Elapsed);

                var setting = new ParametersSettingPLC
                {
                    UserName = Environment.UserName,
                    Date = String.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year),
                    Setting = settings,
                    StartTime = starttime,
                    EndTime = endtime,
                    DeviceName = ItemPLC,
                    TimeSetting = $"{stopwatch.Elapsed:mm\\:ss}",
                    SerialNumber = Convert.ToInt32(SerialNumber),
                    OrderNumber = OrderNumber,

                    Channel1_Coef_acc_A = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_ACC_A).ToString(),
                    Channel1_Coef_acc_B = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_ACC_B).ToString(),
                    Channel1_Coef_speed_A = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_Speed_A).ToString(),
                    Channel1_Coef_speed_B = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_Speed_B).ToString(),
                    Channel1_Coef_4_20_A = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_4_20_A).ToString(),
                    Channel1_Coef_4_20_B = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_4_20_B).ToString(),
                    Channel1_Coef_T_A = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_T_A).ToString(),
                    Channel1_Coef_T_B = Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_T_B).ToString(),
                    Channel1_T_Type = Devices.Crate.ReadUInt16(SettingModel.Channel1.TypeTermo).ToString(),

                    Channel2_Coef_acc_A = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_ACC_A).ToString(),
                    Channel2_Coef_acc_B = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_ACC_B).ToString(),
                    Channel2_Coef_speed_A = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_Speed_A).ToString(),
                    Channel2_Coef_speed_B = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_Speed_B).ToString(),
                    Channel2_Coef_4_20_A = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_4_20_A).ToString(),
                    Channel2_Coef_4_20_B = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_4_20_B).ToString(),
                    Channel2_Coef_T_A = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_T_A).ToString(),
                    Channel2_Coef_T_B = Devices.Crate.ReadSwFloat(SettingModel.Channel2.Coef_T_B).ToString(),
                    Channel2_T_Type = Devices.Crate.ReadUInt16(SettingModel.Channel2.TypeTermo).ToString(),

                    Channel3_Coef_acc_A = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_ACC_A).ToString(),
                    Channel3_Coef_acc_B = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_ACC_B).ToString(),
                    Channel3_Coef_speed_A = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_Speed_A).ToString(),
                    Channel3_Coef_speed_B = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_Speed_B).ToString(),
                    Channel3_Coef_4_20_A = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_4_20_A).ToString(),
                    Channel3_Coef_4_20_B = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_4_20_B).ToString(),
                    Channel3_Coef_T_A = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_T_A).ToString(),
                    Channel3_Coef_T_B = Devices.Crate.ReadSwFloat(SettingModel.Channel3.Coef_T_B).ToString(),
                    Channel3_T_Type = Devices.Crate.ReadUInt16(SettingModel.Channel3.TypeTermo).ToString(),

                    Channel4_Coef_acc_A = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_ACC_A).ToString(),
                    Channel4_Coef_acc_B = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_ACC_B).ToString(),
                    Channel4_Coef_speed_A = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_Speed_A).ToString(),
                    Channel4_Coef_speed_B = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_Speed_B).ToString(),
                    Channel4_Coef_4_20_A = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_4_20_A).ToString(),
                    Channel4_Coef_4_20_B = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_4_20_B).ToString(),
                    Channel4_Coef_T_A = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_T_A).ToString(),
                    Channel4_Coef_T_B = Devices.Crate.ReadSwFloat(SettingModel.Channel4.Coef_T_B).ToString(),
                    Channel4_T_Type = Devices.Crate.ReadUInt16(SettingModel.Channel4.TypeTermo).ToString()
                };
                if (IsResetting is false)
                {
                    await SQLModel.WriteNewDevice(Convert.ToInt32(SerialNumber),OrderNumber,ItemPLC);
                    await Devices.Printer.PrintText(SerialNumber);
                }
                await SQLModel.WriteNewParameters(setting);
                LogerViewModel.Instance.Write($"Настройка заняла {stopwatch.Elapsed:mm\\ss}");
            }
            catch (Exception e)
            {
                LogerViewModel.Instance.Write("❗" +e.Message);
            }
            finally
            {
                IsEnabledButtons = true;
                SaveRegistersModel.Parameters = new();
            }
        }
    }
}
