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
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;
using static PortsWork.Modbus;

namespace APM_Crate.ViewModels.DevicesViewModels
{
    public partial class CrateViewModel : DevicesContext
    {
        public CrateViewModel()
        {
            HeaderText = "Крейт";
        }
        private string? _IP = "10.21.12.70";
        public string? IP
        {
            get { return _IP; }
            set { this.RaiseAndSetIfChanged(ref _IP, value); }
        }

        
        protected override bool OpenPort_abstract()
        {
            if (string.IsNullOrEmpty(IP))
            {
                LogerViewModel.Instance.Write($"Для подключения необходимо заполнить IP устройства");
                return false;
            }
            PortItem = IP;
            Devices.Crate = new Crate();
            Devices.Crate.IPAddress = IP;
            Devices.Crate.Connect();
           
                UpdateStatusModules();
                UpdateStatusPLC(); 
                return true;
        }

        protected override void ClosePort_abstract()
        {
            Devices.Crate.Disconnect();
        }
        public override bool IsOpened() => Devices.Crate.Connected;

        public void UpdateStatusModules()
        {
            ushort value = Devices.Crate.ReadUInt16(Registers.StatusModules);
            string str = Convert.ToString(value, 2);
            str = new string(str.Reverse().ToArray());
            if (str.Length < 16)
            {
                str += string.Concat(Enumerable.Repeat("0", 16 - str.Length));
            }
            ObservableCollection<string?> strings = new ObservableCollection<string?>();
            for (int i = 0; i < 14; i++)
            {
                if (str[i] is '0') strings.Add($"{i+1}");
            }
            if (strings.Count is 0) { strings.Add("Не обнаружено"); }
            if (SettingModel.Moduls != strings) MainWindowViewModel.SettingViewModel.Modules = strings;
        }
        public void UpdateStatusPLC()
        {
            ushort value = Devices.Crate.ReadUInt16(Registers.Type);
            
            MainWindowViewModel.SettingViewModel.ItemPLC = MainWindowViewModel.SettingViewModel.PLC[value - 1];
        }
    }
}
