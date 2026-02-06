using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortsWork;

namespace AutoSet_New
{
    class DevicesCommunication
    {
        public PortMultimeter multimeter;
        public PortGenerator generator;
        public Crate_TCP Crate;

        public List<VisaDeviceInformation> usbDevicesInfo;

        public string multName;
        public string genName;

        public double currentVolt;
        public bool DC_Read = false;

        public DevicesCommunication()
        {
            multimeter = new PortMultimeter();
            generator = new PortGenerator();
            Crate = new Crate_TCP();
        }

        public void InitializeCrateAddress( string ip, int port )
        {
            Crate.IPAddress = ip;
            Crate.Port = port;
        }

        public void CloseConnection()
        {
            multimeter.ClosePort();
            Crate.Disconnect();
            generator.ClosePort();
        }

        public void CrateOpenPort()
        {
            if ( !Crate.Connected )
                Crate.Connect();
        }

        public bool CheckExtDevices( out string checkText )
        {
            checkText = "";
            if ( !multimeter.OpenPort() )
            {
                checkText = "Мультиметр не подключен";
                return false;
            }
            if ( !generator.OpenPort())
            {
                checkText = "Генератор не подключен";
                return false;
            }
            return true;
        }

        public Port SetMeasureDeviceName( Port device, string name )
        {
            if ( name.Contains( "COM" ))
            {
                device.SetName( name );
            } else
            {
                VisaDeviceInformation info = usbDevicesInfo.Find( t => name.Contains( t.devType ) );
                device.usbInfo = info;
                device.SetName( info.description );
            }
            return device.IdentifyDeviceType();
        }

    }
}
