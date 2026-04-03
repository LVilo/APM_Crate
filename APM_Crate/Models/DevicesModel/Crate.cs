using APM_Crate.Service;
using APM_Crate.ViewModels;
using EasyModbus;
using PortsWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APM_Crate.Models.DevicesModel
{
    public class Crate : ModbusClient
    {
        public enum WriteFunctions
        {
            One_Flag = 0x05,
            One_Holding = 0x06,
            Many_Holding = 0x10
        }
        public enum ReadFunctions
        {
            Coil = 0x01,
            DiscreteInputs = 0x02,
            Holding = 0x03,
            Input = 0x04
        }
        public ushort ReadUInt16(ushort reg)
        {
            reg -= 1;
            int[] i =ReadHoldingRegisters(reg, 1);
            return (ushort)i[0];

        }
        public void WriteUInt16(ushort reg, ushort value)
        {
            reg -= 1;
            WriteSingleRegister(reg, value);
        }
        public short ReadInt16(ushort reg)
        {
            reg -= 1;
            int[] i = ReadHoldingRegisters(reg, 1);
            return (short)i[0];
        }
        public void WriteInt16(ushort reg, short value)
        {
            reg -= 1;
            WriteSingleRegister(reg, value);
        }
        public float ReadFloat(ushort reg)
        {
            reg -= 1;
            int[] i = ReadHoldingRegisters(reg, 2);
            return ConvertRegistersToFloat(i);
        }
        public void WriteFloat(ushort reg, float value)
        {
            reg -= 1;
            int[] i = ConvertFloatToRegisters(value);
            WriteMultipleRegisters(reg, i);
        }
        public float ReadSwFloat(ushort reg)
        {
            reg -= 1;
            int[] i = ReadHoldingRegisters(reg, 2);
            return ConvertRegistersToFloat(i);
        }
        public void WriteSwFloat(ushort reg, float value)
        {
            reg -= 1;
            int[] i = ConvertFloatToRegisters(value);
            WriteMultipleRegisters(reg, i);
        }
        public static class Registers
        {
            public static ushort MI_Version { get; } = 1795;
            public static ushort Password { get; } = 3496;
            public static ushort StatusModules { get; } = 109;
            //public static ushort Type_PLC => PLC.;
            public static ushort Type { get; } = (ushort)(60026 + 90 * (Convert.ToInt16(SettingModel.ItemModule) - 1));

            public static ushort Coefficient { get; } = 60056;
            public static ushort SerialNum { get; } = (ushort)(8008 + 100 * (Convert.ToInt16(SettingModel.ItemModule) - 1));
            public static ushort VerPLC { get; } = (ushort)(8009 + 100 * (Convert.ToInt16(SettingModel.ItemModule) - 1));
        }
        public static class Values
        {
            public const ushort Password_New = 0xABCD;
            public const ushort Password_Old = 0xDCBA;
            public const ushort MI_Version_New = 1298;
        }

        public void SetPassword()
        {
            int version = ReadUInt16(Registers.MI_Version);
            if (version  < Values.MI_Version_New)
            {
                WritePassword(Values.Password_Old);
            }
            else
            {
                WritePassword(Values.Password_New);
            }
        }
        private void WritePassword(ushort value)
        {
            for (byte j = 0; j < 20; j++)
            {
                //пароль
                WriteUInt16(Registers.Password, value);
                Thread.Sleep(300);
                if (ReadUInt16(Registers.Password) == value)
                    break;
                if (j == 19)
                    throw new Exception("Не получается записать пароль");
            }
        }
        
    }
}
