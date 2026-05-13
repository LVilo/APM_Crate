using APM_Crate.Service;
using APM_Crate.ViewModels;
using EasyModbus;
using PortsWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;

namespace APM_Crate.Models.DevicesModel
{
    public class Crate : ModbusTCP
    {
        public static ObservableCollection<string> Slots => ["7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"];
        public static string Slot = "7";
        public static ushort IndexSlotByBasket => (ushort)(Convert.ToUInt16(Slot) - 7);
        public static ObservableCollection<string> PLCSource => ["241", "242", "243", "511", "371", "374", "375"];
        public static string ItemPLC = "241";
        public static class Registers
        {
            public static ushort MI_Version => 1795;
            public static ushort Password => 3496;
            public static ushort StatusModules => 109;
            public static ushort Basket => 10;
            //public static ushort Type_PLC => PLC.;
            public static ushort Type => Convert.ToUInt16(60026 + 90 * IndexSlotByBasket);

            public static ushort Coefficient => (ushort)(60056 + 90 * IndexSlotByBasket);
            public static ushort SerialNum => (ushort)(8008 + 100 * IndexSlotByBasket);
            public static ushort SetSerialNum => (ushort)(60027 + 90 * IndexSlotByBasket);
            public static ushort VerPLC => (ushort)(8009 + 100 * IndexSlotByBasket);

        }
        public static class Values
        {
            public const ushort Password_Old = 0xABCD;
            public const ushort Password_New = 0xDCBA;
            public const ushort MI_Version_New = 0x512;
        }
        public async Task SetPassword()
        {
            int version = await ReadUInt16(Registers.MI_Version);
            if (version < Values.MI_Version_New)
            {
                await WriteUInt16(Registers.Password, Values.Password_Old);
            }
            else
            {
                await WriteUInt16(Registers.Password, Values.Password_New);
            }
            await Task.Delay(300);
        }
    }
}
