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

namespace AutoSet_New.Models.DevicesModel
{
    public class Crate : ModbusTCP
    {
     
        private object methodLock { get; set; } = new object();
        private bool methodRunning { get; set; } = false;

        public static class Registers
        {
            public static ushort MI_Version => 1795;
            public static ushort Password => 3496;
            public static ushort StatusModules => 109;
            public static ushort Basket => 10;
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
            public const ushort MI_Version_New = 0x512;
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
        //public void WriteUInt16(ushort reg, ushort value, WriteFunctions func = WriteFunctions.One_Holding)
        //{
        //    //byte[] b = ConvertModBus.ConvertUInt16ToByteMes(value);
        //    //Write(reg, b, (byte)func);


        //    WriteReg(reg, value);
        //}
        //public void WriteInt16(ushort reg, short value, WriteFunctions func = WriteFunctions.One_Holding)
        //{
        //    //byte[] b = ConvertModBus.ConvertInt16ToByteMes(value);
        //    //Write(reg, b, (byte)func);

        //    WriteReg(reg, value);
        //}
        //public void WriteSwFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        //{
        //        byte[] b = ConvertModBus.ConvertSWFloatToByteMes(value);
        //        //WriteMultiple(reg, b, (byte)func);
        //        int[] v = new int[2];
        //        v[0] = BitConverter.ToUInt16(b, 0);
        //        v[1] = BitConverter.ToUInt16(b, 2);

        //     WriteReg(reg, v[0]);
        //     WriteReg((ushort)(reg + 1), v[1]);
        //}
        //public void WriteFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        //{
        //        byte[] b = ConvertModBus.ConvertFloatToByteMes(value);
        //        //WriteMultiple(reg, b, (byte)func);
        //        int[] v = new int[2];
        //        v[0] = BitConverter.ToUInt16(b, 0);
        //        v[1] = BitConverter.ToUInt16(b, 2);
        //     WriteReg(reg, v[0]);
        //     WriteReg((ushort)(reg + 1), v[1]);
        //}
        //public float ReadFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        //{
           
        //        int[] i = ReadReg(reg, 2);
        //        return ConvertRegistersToFloat(i);

        //    //return (short)i[0];

        //    //byte[]? b = Read(reg, (byte)func, 2);
        //    //byte[] value = [b[3], b[4], b[5], b[6]];
        //    //return ConvertModBus.ConvertByteMesToFloat(value);
        //}
        //public float ReadSwFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        //{
        //    //byte[]? b = Read(reg, (byte)func, 2);
        //    //byte[] value = [b[3], b[4], b[5], b[6]];
        //    //return ConvertModBus.ConvertByteMesToSWFloat(value);
           
        //        int[] i = ReadReg(reg, 2);
        //        return ConvertRegistersToFloat(i);
        //}
        //public ushort ReadUInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        //{
        //    //byte[]? b = Read(reg, (byte)func, 1);
        //    //return ConvertModBus.ConvertByteMesToUInt16(b);
        //    //reg -= 1;
        //    int[] i = ReadReg(reg, 1);
        //    return (ushort)i[0];

        //}
        //public short ReadInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        //{
        //    //byte[] b = Read(reg, (byte)func, 1);
        //    //return ConvertModBus.ConvertByteMesToInt16(b);
            
        //        int[] i = ReadReg(reg, 1);
        //        return (short)i[0];
            
        //}
        //public int[] ReadReg(ushort reg, ushort quantity)
        //{
        //    lock (methodLock)
        //    {
        //        while (methodRunning)
        //        {
        //            Monitor.Wait(methodLock);
        //        }
        //        methodRunning = true;
        //    }
        //    try
        //    {
        //        reg -= 1;
        //        return TryReadReg(reg, quantity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Connect(IP, 502);
        //        Thread.Sleep(200);
        //        return TryReadReg(reg, quantity);
        //    }
        //    finally
        //    {
        //        lock (methodLock)
        //        {
        //            methodRunning = false;
        //            Monitor.Pulse(methodLock);
        //        }
        //    }
        //}
        //public int[] TryReadReg(ushort reg, ushort quantity)
        //{
        //    List<int[]> list = new List<int[]>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        list.Add(ReadHoldingRegisters(reg, quantity));
        //        Thread.Sleep(50);
        //    }
        //    int count = 0;
        //    List<int[]> values = new List<int[]>();
        //    values.Add(list.First());
        //    foreach (int[] i in list)
        //    {
        //        if (values.Any(arr => arr.SequenceEqual(i))) count++;
        //        else values.Add(i);
        //    }
        //    if (values.Count is 1) return list.First();
        //    else if (values.Count is 2)
        //    {
        //        if (quantity > 1)
        //        {
        //            if (values[0][0] is 0 && values[0][1] is 0) return values.Last();
        //            if (values[1][0] is 0 && values[1][1] is 0) return values.First();
        //        }
        //    }
        //    throw new Exception("Не удалось прочитать значение с регистра.");
        //}


        //public void WriteReg(ushort reg, int value, int attempt = 0)
        //{
        //    lock (methodLock)
        //    {
        //        while (methodRunning)
        //        {
        //            Monitor.Wait(methodLock);
        //        }
        //        methodRunning = true;
        //    }

        //    try
        //    {
        //        reg -= 1;
        //        int attamts = 0;
        //        while ((ushort)ReadHoldingRegisters(reg, 1)[0] != value)
        //        {
        //            WriteSingleRegister(reg, value);
        //            Thread.Sleep(200);
        //            if (attamts == 10) break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Connect(IP, 502);

        //            Thread.Sleep(200);
        //        WriteSingleRegister(reg, value);
        //        Thread.Sleep(200);
        //        //LogerViewModel.Instance.Write(IPAddress);
        //    }
        //    finally
        //    {
        //        lock (methodLock)
        //        {
        //            methodRunning = false;
        //            Monitor.Pulse(methodLock);
        //        }
        //    }
        //}
        //public void WriteReg(ushort reg, int[] value)
        //{
        //    lock (methodLock)
        //    {
        //        while (methodRunning)
        //        {
        //            Monitor.Wait(methodLock);
        //        }
        //        methodRunning = true;
        //    }

        //    try
        //    {
        //        reg -= 1;
        //        WriteMultipleRegisters(reg, value);
        //        Thread.Sleep(200);
        //    }
        //    catch (Exception ex)
        //    {
        //        Connect(IP, 502);

        //            Thread.Sleep(200);
        //        WriteMultipleRegisters(reg, value);
        //        Thread.Sleep(200);
        //        //LogerViewModel.Instance.Write(IPAddress);
        //    }
        //    finally
        //    {
        //        lock (methodLock)
        //        {
        //            methodRunning = false;
        //            Monitor.Pulse(methodLock);
        //        }
        //    }
        //}
    }
}
