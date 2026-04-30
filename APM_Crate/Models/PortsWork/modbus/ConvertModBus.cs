using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsWork
{
    public static class ConvertModBus
    {
        public static byte[] ConvertFloatToByteMes(float value)
        {
            byte[] b = BitConverter.GetBytes(value);
            return [b[2], b[3], b[0], b[1]];
        }

        public static byte[] ConvertSWFloatToByteMes(float value)
        {
            byte[] b = BitConverter.GetBytes(value);
            return [b[0], b[1], b[2], b[3]];
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
            return (ushort)(value[2] << 8 | value[3]);
        }
        public static short ConvertByteMesToInt16(byte[] value)
        {
            return (short)(value[2] << 8 | value[3]);
        }
        public static ushort[] ConvertSwFloatToUInt16(float value)
        {
            ushort[] result = new ushort[2];
            byte[] data = BitConverter.GetBytes(value);
            result[0] = BitConverter.ToUInt16(data, 0);
            result[1] = BitConverter.ToUInt16(data, 2);
            return result;
        }
        public static float ConvertRegistersToSwFloat(ushort[] value)
        {
            float result;
            byte[] data1 = BitConverter.GetBytes(value[0]);
            byte[] data2 = BitConverter.GetBytes(value[1]);
            byte[] data = { data1[0], data1[1], data2[0], data2[1] };
            result = BitConverter.ToSingle(data);
            return result;
        }
        public static float ConvertRegistersToFloat(ushort[] value)
        {
            float result;
            byte[] data1 = BitConverter.GetBytes(value[0]);
            byte[] data2 = BitConverter.GetBytes(value[1]);
            byte[] data = { data2[0], data2[1], data1[0], data1[1] };
            result = BitConverter.ToSingle(data);
            return result;
        }
    }
}
