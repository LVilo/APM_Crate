using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortsWork
{
    public class ModbusRTU : Port
    {
        public byte SlaveAddress { get; set; } = 1;

        public void SetParameters(int baudrate, StopBits stop)
        {
            BaudRate = baudrate;
            StopBits = stop;
        }

        public byte[] Exchange(byte[] frame, int expectedLength, uint attempt = 0)
        {
            DiscardInBuffer();
            DiscardOutBuffer();
            Write(frame, 0, frame.Length);

            Thread.Sleep(10);

            byte[] resp = new byte[expectedLength];
            int read = 0;
            try
            {
                while (read < expectedLength)
                    read += Read(resp, read, expectedLength - read);
            }
            catch (TimeoutException)
            {
                if (attempt == 9)
                {
                    throw new TimeoutException();
                }
                return Exchange(frame, expectedLength, attempt + 1);
            }
            ValidateCRC(resp);

            return resp;
        }

        private void ValidateCRC(byte[] data)
        {
            ushort crcR = (ushort)(data[^2] | data[^1] << 8);
            ushort crcC = ComputeCRC(data.Take(data.Length - 2).ToArray());

            if (crcR != crcC)
                throw new Exception("CRC error");
        }

        protected static ushort ComputeCRC(byte[] data)
        {
            ushort crc = 0xFFFF;

            foreach (byte b in data)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                    crc = (ushort)((crc & 1) != 0 ? (crc >> 1) ^ 0xA001 : crc >> 1);
            }

            return crc;
        }

        protected static byte[] AddCRC(byte[] frame)
        {
            ushort crc = ComputeCRC(frame);
            return frame.Concat(new byte[] { (byte)(crc & 0xFF), (byte)(crc >> 8) }).ToArray();
        }

        public bool Write(ushort reg, byte[] value, byte func)
        {
            if (IsOpened() is false) throw new Exception($"Порт {PortName} не подключен");

            ushort addr = (ushort)(reg - 1);
            ushort val = (ushort)(value[1] << 8 | value[0]);

            byte[] frame = new byte[]
            {
                SlaveAddress,
                func,
                (byte)(addr >> 8),
                (byte)(addr & 0xFF),
                (byte)(val >> 8),
                (byte)(val & 0xFF)
            };

            frame = AddCRC(frame);

            byte[] b = Exchange(frame, 8);
            byte[] result = [b[5], b[4]];
            if (result.SequenceEqual(value)) return true;
            else return false;
        }

        public byte[] Read(ushort reg, byte func, ushort len)
        {
            if (IsOpened() is false) throw new Exception($"Порт {PortName} не подключен");

            ushort addr = (ushort)(reg - 1);

            byte[] frame = new byte[]
            {
                SlaveAddress,
                func,
                (byte)(addr >> 8),
                (byte)(addr & 0xFF),
                (byte)(len >> 8),
                (byte)(len & 0xFF)
            };

            frame = AddCRC(frame);

            int expected = 5 + len * 2;

            return Exchange(frame, expected);
        }
        public void WriteMultiple(ushort reg, byte[] values, byte func = 0x10)
        {
            if (IsOpened() is false) throw new Exception($"Порт {PortName} не подключен");

            ushort addr = (ushort)(reg - 1);
            ushort count = (ushort)(values.Length / 2);

            byte[] frame = new byte[7 + values.Length];

            frame[0] = SlaveAddress;
            frame[1] = func;
            frame[2] = (byte)(addr >> 8);
            frame[3] = (byte)(addr & 0xFF);
            frame[4] = (byte)(count >> 8);
            frame[5] = (byte)(count & 0xFF);
            frame[6] = (byte)(values.Length);

            for (int i = 0; i < values.Length; i++)
                frame[7 + i] = values[i];

            frame = AddCRC(frame);

            Exchange(frame, 8);

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
            byte[] b = ConvertModBus.ConvertUInt16ToByteMes(value);
            Write(reg, b, (byte)func);
        }
        public void WriteInt16(ushort reg, short value, WriteFunctions func = WriteFunctions.One_Holding)
        {
            byte[] b = ConvertModBus.ConvertInt16ToByteMes(value);
            Write(reg, b, (byte)func);
        }
        public void WriteSwFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            byte[] b = ConvertModBus.ConvertSWFloatToByteMes(value);
            WriteMultiple(reg, b, (byte)func);
        }
        public void WriteFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            byte[] b = ConvertModBus.ConvertFloatToByteMes(value);
            WriteMultiple(reg, b, (byte)func);
        }
        public float ReadFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[]? b = Read(reg, (byte)func, 2);
            byte[] value = [b[3], b[4], b[5], b[6]];
            return ConvertModBus.ConvertByteMesToFloat(value);
        }
        public float ReadSwFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[]? b = Read(reg, (byte)func, 2);
            byte[] value = [b[3], b[4], b[5], b[6]];
            return ConvertModBus.ConvertByteMesToSWFloat(value);
        }
        public ushort ReadUInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[]? b = Read(reg, (byte)func, 1);
            return ConvertModBus.ConvertByteMesToUInt16(b);
        }
        public short ReadInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            byte[] b = Read(reg, (byte)func, 1);
            return ConvertModBus.ConvertByteMesToInt16(b);
        }
    }
}
