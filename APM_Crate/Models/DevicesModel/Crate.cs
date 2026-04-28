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
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace APM_Crate.Models.DevicesModel
{
    public class Crate : ModbusClient
    {
        private object methodLock { get; set; } = new object();
        private bool methodRunning { get; set; } = false;
        public static class Registers
        {
            public static ushort MI_Version => 1795;
            public static ushort Password => 3496;
            public static ushort StatusModules => 109;
            //public static ushort Type_PLC => PLC.;
            public static ushort Type => Convert.ToUInt16(60026 + 90 * (Convert.ToInt16(SettingModel.ItemModule) - 1));

            public static ushort Coefficient => 60056;
            public static ushort SerialNum => (ushort)(8008 + 100 * (Convert.ToInt16(SettingModel.ItemModule) - 1));
            public static ushort VerPLC => (ushort)(8009 + 100 * (Convert.ToInt16(SettingModel.ItemModule) - 1));
        }
        public static class Values
        {
            public const ushort Password_Old = 0xABCD;
            public const ushort Password_New = 0xDCBA;
            public const ushort MI_Version_New = 1298;
        }

        public void SetPassword()
        {
            int version = ReadUInt16(Registers.MI_Version);
            if (version < Values.MI_Version_New)
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
        public void WriteUInt16(ushort reg, ushort value, WriteFunctions func = WriteFunctions.One_Holding)
        {
            //byte[] b = ConvertModBus.ConvertUInt16ToByteMes(value);
            //Write(reg, b, (byte)func);
            
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                reg -= 1;
                WriteSingleRegister(reg, value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
        }
        public void WriteInt16(ushort reg, short value, WriteFunctions func = WriteFunctions.One_Holding)
        {
            //byte[] b = ConvertModBus.ConvertInt16ToByteMes(value);
            //Write(reg, b, (byte)func);
            
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                reg -= 1;
                WriteSingleRegister(reg, value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
        }
        public void WriteSwFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                byte[] b = ConvertModBus.ConvertSWFloatToByteMes(value);
                //WriteMultiple(reg, b, (byte)func);
                int[] v = new int[2];
                v[0] = BitConverter.ToUInt16(b, 0);
                v[1] = BitConverter.ToUInt16(b, 2);
                reg -= 1;
                WriteMultipleRegisters(reg, v);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
        }
        public void WriteFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                byte[] b = ConvertModBus.ConvertFloatToByteMes(value);
                //WriteMultiple(reg, b, (byte)func);
                int[] v = new int[2];
                v[0] = BitConverter.ToUInt16(b, 0);
                v[1] = BitConverter.ToUInt16(b, 2);
                reg -= 1;
                WriteMultipleRegisters(reg, v);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
        }
        public float ReadFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                reg -= 1;
                int[] i = ReadHoldingRegisters(reg, 2);
                return ConvertRegistersToFloat(i);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
            //return (short)i[0];

            //byte[]? b = Read(reg, (byte)func, 2);
            //byte[] value = [b[3], b[4], b[5], b[6]];
            //return ConvertModBus.ConvertByteMesToFloat(value);
        }
        public float ReadSwFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            //byte[]? b = Read(reg, (byte)func, 2);
            //byte[] value = [b[3], b[4], b[5], b[6]];
            //return ConvertModBus.ConvertByteMesToSWFloat(value);
            //reg -= 1;
            
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                reg -= 1;
                int[] i = ReadHoldingRegisters(reg, 2);
                return ConvertRegistersToFloat(i);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
        }
        public ushort ReadUInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            //byte[]? b = Read(reg, (byte)func, 1);
            //return ConvertModBus.ConvertByteMesToUInt16(b);
            //reg -= 1;
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                reg -= 1;
                int[] i = ReadHoldingRegisters(reg, 1);
                return (ushort)i[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
            
        }
        public short ReadInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            //byte[] b = Read(reg, (byte)func, 1);
            //return ConvertModBus.ConvertByteMesToInt16(b);
            lock (methodLock)
            {
                while (methodRunning)
                {
                    Monitor.Wait(methodLock);
                }
                methodRunning = true;
            }

            try
            {
                reg -= 1;
                int[] i = ReadHoldingRegisters(reg, 1);
                return (short)i[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lock (methodLock)
                {
                    methodRunning = false;
                    Monitor.Pulse(methodLock);
                }
            }
            
        }
    }
}
