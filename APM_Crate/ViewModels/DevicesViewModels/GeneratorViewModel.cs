using APM_Crate.Models.DevicesModel;

using Avalonia.Media;


using PortsWork;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DevicesViewModels
{
    public partial class GeneratorViewModel : DevicesContext
    {
        public GeneratorViewModel()
        {
            //PortText = "генератора";
            HeaderText = "Генератор";
        }

        private bool _chanel_1 = true;
        public bool Chanel_1
        {
            get { return _chanel_1; }
            set
            { 
                this.RaiseAndSetIfChanged(ref _chanel_1, value);
                this.RaiseAndSetIfChanged(ref _chanel_2, !value);
                Devices.Generator.channelNum = 1;
            }
        }
        private bool _chanel_2 = false;
        public bool Chanel_2
        {
            get { return _chanel_2; }
            set 
            {
                this.RaiseAndSetIfChanged(ref _chanel_2, value);
                this.RaiseAndSetIfChanged(ref _chanel_1, !value);
                Devices.Generator.channelNum = 2;
            }
        }

        protected override async Task<bool> OpenPort_abstract()
        {
            if (string.IsNullOrEmpty(PortItem))
            {
                LogerViewModel.Instance.Write($"Выбранный порт пуст. Повторно выберите порт для подключения устройства");
                return false;
            }
            Devices.Generator = new PortGenerator();
            Devices.Generator = (PortGenerator)Devices.SetMeasureDeviceName(Devices.Generator,  PortItem);
            if( Devices.Generator.OpenPort() is true)
            {
                
                int chanel = Chanel_1 is true ? 1 : 2;
                Devices.Generator.SetChannel(chanel);
                return Devices.Generator.IsOpened();
            }
            return false;
        }

        protected override void ClosePort_abstract()
        {
            Devices.Generator.ChanelOff(Devices.Generator.channelNum);
            Devices.Generator.ClosePort();
        }
        public override bool IsOpened() => Devices.Generator.IsOpened();
    }
}
