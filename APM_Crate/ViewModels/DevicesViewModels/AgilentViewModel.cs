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
            HeaderText = "Мультиметр";
        }
        protected override bool OpenPort_abstract()
        {
            Devices.Multimeter = new PortMultimeter();
            Devices.Multimeter = (PortMultimeter)Devices.SetMeasureDeviceName(Devices.Multimeter, PortItem);

           if(Devices.Multimeter.OpenPort() is true)
            {
                Dialog.Mult = new Delay();
                Dialog.WhileGetVoltAsync();
                return Devices.Multimeter.IsOpened();
            }
            return false;
        }

        protected override void ClosePort_abstract()
        {
            Devices.Multimeter.ClosePort();

        }
        public override bool IsOpened() => Devices.Multimeter.IsOpened();
    }
}

