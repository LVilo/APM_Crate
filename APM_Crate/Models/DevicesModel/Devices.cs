using PortsWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using APM_Crate.ViewModels;
using APM_Crate.Models.RestApiModel;

namespace APM_Crate.Models.DevicesModel
{
    public static class Devices
    {
        private static List<VisaDeviceInformation> usbDevicesInfo;


        public static PortGenerator Generator { get; set; } = new PortGenerator();
        public static PortMultimeter Multimeter { get; set; } = new PortMultimeter();
        public static Crate Crate {  get; set; } = new Crate();
        public static Printer Printer {  get; set; } = new Printer();
        public static SG004AProtocol sg004 { get; set; } = new SG004AProtocol();

        //public static RestModel Rest { get; set; } = new RestModel("");

        public static async Task<Port> SetMeasureDeviceName(Port device, string name)
         {

            if (name.Contains("COM") || name.Contains("/dev/ttyUSB") || name.Contains("/dev/usbtmc"))
            {
                await device.SetName(name);
            }
            else
            {
                VisaDeviceInformation info = usbDevicesInfo.Find(t => name.Contains(t.devType));
                device.usbInfo = info;
                await device.SetName(info.description);
            }
            return await device.IdentifyDeviceType();
        }
        public static string[] GetAllPorts()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                usbDevicesInfo = Port.FindVisaDevicesInfo();
                List<string> usbInfo = new List<string>();
                usbDevicesInfo.ForEach(t => usbInfo.Add(t.GetInfo()));
                return usbInfo.Concat(SerialPort.GetPortNames()).ToArray();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Linux.FindDevicesLinux().ToArray();
            }
            else
            {
                throw new PlatformNotSupportedException("Неподдерживаемая ОС");
            }
        }

    }
}
