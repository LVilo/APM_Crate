using Avalonia.Media;
using APM_Crate.Models.DevicesModel;

using PortsWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APM_Crate.Service;
using APM_Crate.Models;



namespace APM_Crate.ViewModels.DevicesViewModels
{
    public partial class AgilentViewModel : DevicesContext
    {

        public AgilentViewModel()
        {
            //PortText = "мультиметра";
            HeaderText = "Мультиметр - Отключено";
        }
        protected override async Task<bool> OpenPort_abstract()
        {
            Devices.Multimeter = new PortMultimeter();
            Devices.Multimeter = (PortMultimeter)await Devices.SetMeasureDeviceName(Devices.Multimeter, PortItem);

           if(await Devices.Multimeter.OpenPort() is true)
            {
                Dialog.Mult = new Delay();
                Dialog.WhileGetVoltAsync();
                HeaderText = "Мультиметр - Подключено";
                return true;
            }
            else
            {
                HeaderText = "Мультиметр - Отключено";
                return false;
            }
            
        }

        protected override async Task ClosePort_abstract()
        {
            await Devices.Multimeter.ClosePort();
            HeaderText = "Мультиметр - Отключено";
        }
        public override bool IsOpened() => Devices.Multimeter.IsOpened();
    }
}

