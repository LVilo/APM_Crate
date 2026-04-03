using System;
using System.IO.Ports;

namespace PortsWork
{
	public abstract class Modbus : Port
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
        protected abstract byte[] Exchange(byte[] frame, int expectedLength, uint attempt = 0);
        /// <summary>
        /// запись значения в регистр. Номер регистра задается +1 от настоящего(для удобства)
        /// </summary>
        /// <param name="reg">номре регистра(необходимо записать номер регистра в модскане)</param>
        /// <param name="value"></param>
        /// <param name="func">номер функции</param>
        /// <returns></returns>
        protected abstract bool Write(ushort reg, byte[] value, byte func);
        protected abstract byte[] Read(ushort reg, byte func, ushort len);
        protected abstract void WriteMultiple(ushort reg, byte[] values, byte func = 0x10);

        public void WriteUInt16(ushort reg,ushort value, WriteFunctions func = WriteFunctions.One_Holding)
        {
            byte[] b = ConvertUInt16ToByteMes(value);
            Write(reg, b, (byte)func);
        }
        public void WriteInt16(ushort reg, short value, WriteFunctions func = WriteFunctions.One_Holding)
        {
           byte[] b = ConvertInt16ToByteMes(value);
           Write(reg, b, (byte)func);
        }
        public void WriteSwFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            byte[] b = ConvertSWFloatToByteMes(value);
            WriteMultiple(reg, b, (byte)func);
        }
        public void WriteFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            byte[] b = ConvertFloatToByteMes(value);
            WriteMultiple(reg, b, (byte)func);
        }
        public float ReadFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[]? b = Read(reg, (byte)func, 2);
            byte[] value = [b[3], b[4], b[5], b[6]];
            return ConvertByteMesToFloat(value);
        }
        public float ReadSwFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[]? b = Read(reg, (byte)func, 2);
            byte[] value = [b[3], b[4], b[5], b[6]];
            return ConvertByteMesToSWFloat(value);
        }
        public ushort ReadUInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[]? b = Read(reg, (byte)func, 1);
            return ConvertByteMesToUInt16(b);
        }
        public short ReadInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[] b = Read(reg, (byte)func, 1);
            return ConvertByteMesToInt16(b);
        }

        public static byte[] ConvertFloatToByteMes(float value)
        {
            byte[] b = BitConverter.GetBytes(value);
            return [b[2], b[3], b[0], b[1]];
        }
        
        public static byte[] ConvertSWFloatToByteMes(float value)
        {
            byte[] b = BitConverter.GetBytes(value);
            return [b[1], b[0], b[3], b[2]];
        }
        
        public static byte[] ConvertUInt16ToByteMes(ushort value)
        {
            return BitConverter.GetBytes(value);
        }
        public static byte[] ConvertInt16ToByteMes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public static float ConvertByteMesToFloat(byte[] value)
        {
            value = [value[3], value[2], value[1], value[0]];
            return BitConverter.ToSingle(value);
        }
        public static float ConvertByteMesToSWFloat(byte[] value)
        {
            value = [value[1], value[0], value[3], value[2]];
            return BitConverter.ToSingle(value);
        }
        public static ushort ConvertByteMesToUInt16(byte[] value)
        {
            return (ushort)(value[3] << 8 | value[4]);
        }
        public static short ConvertByteMesToInt16(byte[] value)
        {
            return (short)(value[3] << 8 | value[4]);
        }
    }
}
