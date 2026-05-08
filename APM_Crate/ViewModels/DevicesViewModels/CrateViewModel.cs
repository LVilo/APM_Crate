using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using Microsoft.VisualBasic;
using PortsWork;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;
using static PortsWork.Modbus;

namespace APM_Crate.ViewModels.DevicesViewModels
{
    public partial class CrateViewModel : DevicesContext
    {
        private Crate passwordClient = new Crate();
        public CrateViewModel()
        {
            HeaderText = "Крейт - Отключено";
        }
        private string? _IP = "10.21.12.70";
        public string? IP
        {
            get { return _IP; }
            set { this.RaiseAndSetIfChanged(ref _IP, value); }
        }

        public async Task Timer()
        {
            while (passwordClient.IsConnected())
            {
                await passwordClient.SetPassword();
                await Task.Delay(1000);
            }
        }
        protected override async Task<bool> OpenPort_abstract()
        {
            if (string.IsNullOrEmpty(IP))
            {
                await LogerViewModel.Instance.Write($"Для подключения необходимо заполнить IP устройства");
                return false;
            }
            PortItem = IP;
            Devices.Crate.IpAddress = IP;
            //Devices.Crate.IPAddress = IP;
            Devices.Crate.Port = 502;
            //Devices.Crate = new Crate();
            Devices.Crate.Connect(IP, 502);

            passwordClient.IpAddress = IP;
            passwordClient.Port = 502;
            passwordClient.Connect(IP, 502);
            //await Devices.Crate.ReadUInt16(60026);
            Timer();

            //float dc = await Devices.Crate.ReadSwFloat(8018);

            // ModbusTCP cc = new ModbusTCP();
            // cc.Connect("10.21.12.67");
            //ushort reg = cc.ReadUInt16(8022);
            //await LogerViewModel.Instance.Write(reg.ToString());
            //WhileUpdateModules();
            //int[] i = Devices.Crate.ReadHoldingRegisters(60025, 1);
            bool result = Devices.Crate.Connected;
            if (result is true) HeaderText = "Крейт - Подключено";
            else HeaderText = "Крейт - Отключено";
            return result;
        }

        protected override async Task ClosePort_abstract()
        {
            Devices.Crate.Disconnect();
            passwordClient.Disconnect();
            HeaderText = "Крейт - Отключено";
        }
        public override bool IsOpened() => Devices.Crate.Connected;

        //public void UpdateStatusModules()
        //{
        //    ushort value = await Devices.Crate.ReadUInt16(Registers.StatusModules);
        //    string str = Convert.ToString(value, 2);
        //    str = new string(str.Reverse().ToArray());
        //    if (str.Length < 16)
        //    {
        //        str += string.Concat(Enumerable.Repeat("0", 16 - str.Length));
        //    }
        //    ObservableCollection<string?> strings = new ObservableCollection<string?>();
        //    for (int i = 0; i < 14; i++)
        //    {
        //        if (str[i] is '0') strings.Add($"{i+1}");
        //    }
        //    if (strings.Count is 0) { strings = new(); }
        //    if (SettingModel.Moduls != strings) MainWindowViewModel.SettingViewModel.Modules = strings;
        //}
        //public void UpdateStatusPLC()
        //{

        //    if (SettingModel.Moduls.Contains(SettingModel.ItemModule) is false && SettingModel.Moduls.Count > 0)
        //    {
        //        MainWindowViewModel.SettingViewModel.ItemModule = SettingModel.Moduls.First();
        //    }
        //    else { MainWindowViewModel.SettingViewModel.ItemModule = null; }
        //    //ushort value = await Devices.Crate.ReadUInt16(Registers.Type);
            
        //    //MainWindowViewModel.SettingViewModel.ItemPLC = MainWindowViewModel.SettingViewModel.PLC[value - 1];
        //}
        //public async Task WhileUpdateModules()
        //{
        //    await Task.Run(async () =>
        //    {
        //        while (Devices.Crate.Connected)
        //        {
        //            try
        //            {
        //                UpdateStatusModules();
        //                UpdateStatusPLC();
        //            }
        //            catch (Exception ex)
        //            {
        //                Devices.Crate.Disconnect();
        //                await LogerViewModel.Instance.Write(ex.Message);
        //            }
        //            await Task.Delay(2000);
        //        }
        //    });
        //}
    }
}
