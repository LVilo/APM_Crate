using APM_Crate.Service;
using APM_Crate.ViewModels;
using EasyModbus;
using PortsWork;
using ReactiveUI;
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
            int version = ReadReg(Registers.MI_Version);
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
                WriteReg(Registers.Password, value);
                Thread.Sleep(300);
                if (ReadReg(Registers.Password) == value)
                    break;
                if (j == 19)
                    throw new Exception("Не получается записать пароль");
            }
        }
        public void WriteReg(int adr, int valuSe)
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
                WriteSingleRegister(adr, value);
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
        public void WriteReg(int adr, int[] value)
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
                WriteMultipleRegisters(adr, value);
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
        public int ReadReg(int Reg_adr)
        {
            int[] Reg = new int[1];
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
                Reg = ReadHoldingRegisters(Reg_adr, 1);
                return Reg[0];
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
        public int[] ReadReg(int Reg_adr, int quantity)
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
                return ReadHoldingRegisters(Reg_adr, quantity);
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

        public float ReadValue(int reg_addr)
        {
            int[] registers = ReadReg(reg_addr, 2);

            return ConvertRegistersToFloat(registers);
        }
    }
}
