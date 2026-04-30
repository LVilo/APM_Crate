using EasyModbus;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSet_New
{
    class Crate_TCP : ModbusClient
    {
        private object methodLock { get; set; } = new object();
        private bool methodRunning { get; set; } = false;
        public void SetPassword()
        {
            int version = ReadReg(Registers.REGISTER_VERSION_MI);
            if (version < Registers.VALUE_VERSION_NEW)
            {
                WritePassword(Registers.VALUE_PASSWORD_OLD);
            }
            else
            {
                WritePassword(Registers.VALUE_PASSWORD_NEW);
            }
        }

        private void WritePassword(int value)
        {
            for (byte j = 0; j < 20; j++)
            {
                //пароль
                WriteReg(Registers.REGISTER_PASSWORD, value);
                Thread.Sleep(300);
                if (ReadReg(Registers.REGISTER_PASSWORD) == value)
                    break;
                if (j == 19)
                    throw new Exception("Не получается записать пароль");
            }
        }

        public void SetOn(int reg_on)
        {
            Thread.Sleep(300);
            WriteReg(reg_on, 1);
            Thread.Sleep(300);
        }

        public float ReadVersion(int register)
        {
            UInt16 baseValue = (UInt16)ReadReg(register);
            byte[] bytes = BitConverter.GetBytes(baseValue);
            return bytes[0] + 0.01f * bytes[1];
        }

        private void WriteCoeffs(int start_reg_adr_coef, int len)
        {
            //записываю 0 во всерегистры
            for (int j = 0; j < len; j++)
            {
                for (byte count = 0; ; count++)
                {
                    WriteReg(start_reg_adr_coef + j, 0);
                    Thread.Sleep(300);
                    if (ReadReg(start_reg_adr_coef + j) == 0)
                        break;
                    if (count == 29)
                        throw new Exception($"Не получается записать регистр {start_reg_adr_coef + j + 1}");
                }
            }
            //записываю 1
            for (int j = 1; j < len - 2; j += 4)
            {
                for (byte count = 0; ; count++)
                {
                    WriteReg(start_reg_adr_coef + j, 16256);
                    Thread.Sleep(300);
                    if (ReadReg(start_reg_adr_coef + j) == 16256)
                        break;
                    if (count == 29)
                        throw new Exception($"Не получается записать регистр {start_reg_adr_coef + j + 1}");
                }
            }
        }

        public void WriteCoeffs(int start_addr, float a, float b)
        {
            DevicesCommunication.Log.Write("Запись коэффициентов");
            DevicesCommunication.Log.Write($"Коэффициент A: {a}");
            WriteReg(start_addr, ConvertFloatToRegisters(a));
            Thread.Sleep(1000);
            DevicesCommunication.Log.Write($"Коэффициент B: {b}");
            WriteReg(start_addr + 2, ConvertFloatToRegisters(b));
            Thread.Sleep(1000);
        }

        public void ResetCoef(int start_reg_adr_coef, int reg_on, int len)
        {
            DevicesCommunication.Log.Write("Сброс коэффициентов");
            SetPassword();
            SetOn(reg_on);

            WriteCoeffs(start_reg_adr_coef, len);
        }

        public void ResetCoef(int start_reg_adr_coef, int len)
        {
            DevicesCommunication.Log.Write("Сброс коэффициентов");
            SetPassword();

            WriteCoeffs(start_reg_adr_coef, len);
        }

        public void SetSerialNum(int num)
        {
            DevicesCommunication.Log.Write("Запись серийного номера");
            SetPassword();

            WriteReg(Registers.REGISTER_SERIAL_NUM, num);
        }

        public void WriteReg(int adr, int value)
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
                MessageBox.Show(ex.Message);
                int[] Reg = new int[1];
                return Reg;
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
