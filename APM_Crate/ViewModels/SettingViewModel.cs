using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using APM_Crate.Models.RestApiModel;
using APM_Crate.Models.SettingsModel.PLCs;


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
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;
using static APM_Crate.Models.SettingModel;
using static System.Net.Mime.MediaTypeNames;

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
        public ObservableCollection<string> Slots => Crate.Slots; 

        [JsonIgnore]
        public string Slot
        {
            get => Crate.Slot;
            set
            {
                //if(Devices.Crate.Connected)
                //{
                //   ushort type = await Devices.Crate.ReadUInt16(Registers.Type);
                //    if(type>0 && type < 8) ItemPLC = PLC[type-1];
                //}
              if(IsEnabledButtons) this.RaiseAndSetIfChanged(ref Crate.Slot, value);
            }
        }
        [JsonIgnore]
        public ObservableCollection<string> PLC => Crate.PLC;

        [JsonIgnore]
        public string ItemPLC
        {
            get => Crate.ItemPLC;
            set
            {
                if (IsEnabledButtons)
                {
                    Channel1.CanSetting = true;
                    Channel2.CanSetting = true;
                    Channel3.CanSetting = value is "371" or "374" or "375" or "511";
                    Channel4.CanSetting = value is "511";

                    Channel3.SettingChannel = value is "371" or "374" or "375" or "511";
                    Channel4.SettingChannel = value is "511";
                    this.RaiseAndSetIfChanged(ref Crate.ItemPLC, value);
                }
            }
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
        public int ProgressValue
        {
            get => SettingModel.ProgressValue;
            set { this.RaiseAndSetIfChanged(ref SettingModel.ProgressValue, value); }
        }
        public string ProgressText
        {
            get => SettingModel.ProgressText;
            set { this.RaiseAndSetIfChanged(ref SettingModel.ProgressText, value); }
        }
        public int ProgressValueChannel
        {
            get => SettingModel.ProgressValueChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.ProgressValueChannel, value); }
        }
        public string ProgressTextChannel
        {
            get => SettingModel.ProgressTextChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.ProgressTextChannel, value); }
        }
        private async Task Samples()
        {
            try
            {

                IsEnabledButtons = false;
                if (Devices.Crate.Connected is false) throw new Exception("Необходимо подключится к крейту");

                // меняю состав корзины под выбранный модуль
                await ChangeBasketForSelectModule();


                await Task.Delay(2000);

                //смотрю состояние модулей после изменения состава
                await ValidStatusModule();

                ushort type = await ValidIEPE_By_PLC();


                await SettingModel.Samples(type);

                //await Task.Delay(5000);
                //await Devices.Printer.PrintText(12.ToString());

            }
            catch (TaskCanceledException ex)
            {

            }
            catch (Exception ex)
            {
                await Dialog.ShowConfirm("❗ " +ex.Message,new Delay());
            }
            finally
            {
                IsEnabledButtons = true;
            }
        }
        Stopwatch stopwatch = new Stopwatch();

        public static PLC PLC = new PLC_241();
        private async Task Setting()
        {
            try
            {
                var progress = new Progress<ProgressReport>(p =>
                {
                    ProgressValue = p.Percent;
                    ProgressText = p.Message;
                });
                var wp = new WeightedProgress(progress, 100);

                var progressChannel = new Progress<ProgressReport>(p =>
                {
                    ProgressValueChannel = p.Percent;
                    ProgressTextChannel = p.Message;
                });
                var wp2 = new WeightedProgress(progressChannel, 100);
                //List<Config> list = await RestModel.GetListRecord(50, 23, null, null, RestModel.DeviceFamily);
                //foreach (var d in list)
                //{
                //    await RestModel.Delete(d.Id);
                //}
                //List<Config> list = await RestModel.GetListRecord(50, 12, null, null, RestModel.DeviceFamily);
                IsEnabledButtons = false;
                await wp.Step(5, "Валидация Вводимых данных", ValidSelected);
                await wp.Step(5, "Валидация подключённых устройств", ValidDevices);
                await wp.Step(5, $"Изменение состава корзины под слот {Slot}", ChangeBasketForSelectModule);
                await wp.Step(5, $"Валидация состояния модуля в слоте {Slot}", ValidStatusModule);
                await wp.Step(5, $"Валидация типа контроллера", ValidTypePLC);
                await wp.Step(5, $"Чтение серийного номера", GetSerialNumber);


                if ((SerialNumber >= 0 && SerialNumber <= 65535) is false)
                {
                    throw new Exception($"Серийный номер должен быть в диапазоне 0-65535.");
                }

                string starttime = String.Format($"{DateTime.Now.Hour}:{DateTime.Now.Minute}");

                stopwatch.Restart();
                settings = new List<Settings>();
                await Start(ItemPLC,wp,wp2);
                stopwatch.Stop();
                string endtime = String.Format($"{DateTime.Now.Hour}:{DateTime.Now.Minute}");
                Config config = new Config
                {
                    DeviceType = ItemPLC,
                    SerialNumber = SerialNumber,
                    OrderNumber = OrderNumber,
                    Settings = settings,
                };
                ushort type = (ItemPLC) switch
                {
                    "241" => 1,
                    "242" => 2,
                    "243" => 3,
                    "511" => 4,
                    "371" => 5,
                    "374" => 6,
                    "375" => 7,
                };
                if (!(type is 3 || type is 4 || type is 6))
                {
                    await LogerViewModel.Instance.Write("Проверка выборок устройства");
                    await wp.Step(5, $"Проверка выборок устройства",()=> CheckFilePLC.Start(type));
                    await CheckFilePLC.Start(type);
                    await LogerViewModel.Instance.Write("✔ Выборки соответствуют правильной форме");
                }
                await wp.Step(5, $"Запись в базу данных",() => PostNewDevice(config));
                //await SaveRegistersModel.MakeReportAsync(ItemPLC, OrderNumber, starttime,endtime,stopwatch.Elapsed);
                await LogerViewModel.Instance.Write($"✔ Настройка заняла {stopwatch.Elapsed:mm\\ss}");

            }
            catch (TaskCanceledException ex)
            {

            }
            catch (Exception ex)
            {
                await Dialog.ShowMessage("❗ " +ex.Message);
            }
            
            finally
            {
                IsEnabledButtons = true;
                ProgressValue = 0;
            }
        }
        private async Task ChangeBasketForSelectModule()
        {
            await LogerViewModel.Instance.Write($"Изменение состава корзины под {Slot} слот");
            string str = "0000000000000000";
            StringBuilder sb = new StringBuilder(new string(str.Reverse().ToArray()));
            sb[Crate.IndexSlotByBasket] = '1';
            str = sb.ToString();
            str = new string(str.Reverse().ToArray());
            ushort value = Convert.ToUInt16(str, 2);

            await Devices.Crate.SetPassword();
            await Devices.Crate.WriteUInt16(Registers.Basket, value);
            await Task.Delay(2000);
        }

        private async Task ValidStatusModule()
        {
            ushort status = await Devices.Crate.ReadUInt16(Registers.StatusModules);
            string s = new string(Convert.ToString(status, 2).Reverse().ToArray());
            if (s[Crate.IndexSlotByBasket] is not '0')
            {
                throw new Exception($"Выбранный модуль для настройки не подключен в корзину или не включен. Убедитесь в подключении модуля в слоте {Slot}");
            }
        }
        private async Task ValidDevices()
        {
            if(await RestModel.GetAPIStatus() is false) throw new Exception("RestAPI не отвечает на сообщения, проверьте подключение с сервером.");
            if (Devices.Crate.Connected is false) throw new Exception("Необходимо подключится к крейту");
            switch (ItemPLC)
            {
                case "243" or "374":
                    {
                        if (Devices.Multimeter.IsOpened() is false) throw new Exception("Необходимо подключить мультиметр");
                        break;
                    }
                default:
                    {
                        if (Devices.Multimeter.IsOpened() is false) throw new Exception("Необходимо подключить мультиметр");
                        if (Devices.Generator.IsOpened() is false) throw new Exception("Необходимо подключить генератор");
                        break;
                    }
            }
        }
        private async Task ValidSelected()
        {
            if (string.IsNullOrEmpty(OrderNumber))
            {
                throw new Exception($"Введите номер заказа.");
            }
            if (string.IsNullOrEmpty(Slot))
            {
                throw new Exception($"Выберите модуль для настройки.");
            }
            if (string.IsNullOrEmpty(ItemPLC))
            {
                throw new Exception($"Выберите PLC для настройки.");
            }
        }
        private async Task ValidTypePLC()
        {
            ushort type = await Devices.Crate.ReadUInt16(Crate.Registers.Type);
            if (type == 0 || type > 7)
            {
                await Dialog.ShowConfirm($"В выбранном контроллере записан неизвестный тип контроллера '{type}'. Настроить его как тип PLC.{ItemPLC} ?", new Delay(), true);
            }
            if (ItemPLC != PLC[type - 1])
            {
                await Dialog.ShowConfirm($"Выбранный тип контроллера не соответствует типу, записанному на контроллер. Настроить контроллер PLC.{PLC[type - 1]} как тип PLC.{ItemPLC} ?", new Delay(), true);
            }
        }
        private async Task GetSerialNumber()
        {
            SerialNumber = await Devices.Crate.ReadUInt16(Crate.Registers.SerialNum);
            if (SerialNumber is not 0xFFFF)
            {
                await LogerViewModel.Instance.Write("В устройстве записан серийный номер. Выберите каналы для перенастройки устройства");
                //в устройстве записан серийный номер
                await Dialog.ShowResetting();
                //List<Config> list = await RestModel.GetListRecord(50, SerialNumber, null,null, RestModel.DeviceFamily);
                IsResetting = true;

                //if (list.Count > 0) IsResetting = true; 
                //else IsResetting = false;// влияет на мягкое удаление записи из базы данных,т.к записи не нашлось удалять ничего не нужно
            }
            else
            {
                SerialNumber = Convert.ToUInt16(await RestModel.GetLastSerialNumber() + 1);
                IsResetting = false;
            }
        }
        private async Task PostNewDevice(Config config)
        {
            if (IsResetting is false)
            {
                await LogerViewModel.Instance.Write("Запись нового устройства в базу данных");
                await RestModel.Post(config);
                //await SQLModel.WriteNewDevice(SettingModel.SerialNumber,OrderNumber,ItemPLC);
                await Devices.Printer.PrintText($"C.№:{SerialNumber}.");
                await Devices.Crate.WriteSingleRegister(Crate.Registers.SetSerialNum, SettingModel.SerialNumber);
            }
            else
            {
                await LogerViewModel.Instance.Write("Добавление новой записи настройки устройства. Старая запись станет не актуальной");
                List<Config> list = await RestModel.GetListRecord(50, SerialNumber, null,null,RestModel.DeviceFamily);
                foreach(var d in list)
                {
                    await RestModel.Delete(d.Id);
                }
                //string id = list.First().Id;
                //await RestModel.Delete(id);
                await RestModel.Post(config);
            }
        }
        private async Task<ushort> ValidIEPE_By_PLC()
        {
            ushort type = await Devices.Crate.ReadUInt16(Crate.Registers.Type);
            bool valid = type is 3 || type is 4 || type is 6 ? false : true;
            if(valid is false)
            {
                throw new Exception("В данном типе PLC отсутствует канал IEPE");
            }
            return type;
        }
    }
}
