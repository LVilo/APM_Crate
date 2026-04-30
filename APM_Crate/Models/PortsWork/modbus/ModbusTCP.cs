using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;

namespace PortsWork
{
    public class ModbusTCP
    {
        private TcpClient _client = new TcpClient();
        private NetworkStream _stream;

        private ushort _transactionId = 0;

        public string IpAddress { get; set; }
        public int Port { get; set; } = 502;

        public byte UnitId { get; set; } = 1;

        public void Connect(string ip)
        {
            IpAddress = ip;
            _client = new TcpClient();
           _client.Connect(IpAddress, Port);
            _stream = _client.GetStream();
        }
        public void Connect(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
            _client = new TcpClient();
            _client.Connect(IpAddress, Port);
            _stream = _client.GetStream();
        }
        public void Connect()
        {
            _client = new TcpClient();
            _client.Connect(IpAddress, Port);
            _stream = _client.GetStream();
        }
        public void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
        }
        public bool Connected => _client.Connected;
        public bool IsConnected()
        {
            return _client?.Connected ?? false;
        }

        public byte[] Exchange(byte[] pdu, int expectedLength, uint attempt = 0)
        {
            string mes = "";
            
            if (!IsConnected())
            {
                Connect(IpAddress, Port);
                if (!IsConnected())
                {
                    throw new Exception("TCP not connected");
                }
            }

            byte[] frame = BuildMbapHeader(pdu);

            for (int i = 0; i < frame.Length; i++)
                mes += $"[{Convert.ToString(frame[i], 16)}] ";
            Debug.WriteLine($"{IpAddress}<<{mes}");

            _stream.Write(frame, 0, frame.Length);

            byte[] response = new byte[expectedLength + 7]; // +MBAP
            int read = 0;

            try
            {
                while (read < response.Length)
                {
                    Thread.Sleep(200);
                    read += _stream.Read(response, read, response.Length - read);
                }
            }
            catch
            {
                if (attempt == 10)
                    throw;

                Connect(IpAddress, Port);
                Thread.Sleep(1000);
                return Exchange(pdu, expectedLength, attempt + 1);
            }
            mes = "";
            for (int i = 0; i < response.Length; i++)
                mes += $"[{Convert.ToString(response[i], 16)}] ";
            Debug.WriteLine($"{IpAddress}>>{mes}");

            // убираем MBAP
            return response.Skip(7).ToArray();
        }

        private byte[] BuildMbapHeader(byte[] pdu)
        {
            _transactionId++;

            byte[] header = new byte[7];

            header[0] = (byte)(_transactionId >> 8);
            header[1] = (byte)(_transactionId & 0xFF);

            header[2] = 0;
            header[3] = 0;

            ushort length = (ushort)(pdu.Length + 1); // + UnitId
            header[4] = (byte)(length >> 8);
            header[5] = (byte)(length & 0xFF);

            header[6] = UnitId;

            return header.Concat(pdu).ToArray();
        }

        public void WriteSingleRegister(ushort reg, ushort value)
        {
            ushort addr = (ushort)(reg - 1);
            //ushort val = (ushort)(value[1] << 8 | value[0]);

            byte[] pdu = new byte[]
            {
                0x06,
                (byte)(addr >> 8),
                (byte)(addr & 0xFF),
                (byte)(value >> 8),
                (byte)(value & 0xFF)
            };

             Exchange(pdu, 5);

            //byte[] result = [resp[4], resp[3]];
            //return result.SequenceEqual(value);
        }

        public ushort[] ReadHoldingRegisters(ushort reg, ushort len)
        {
            ushort addr = (ushort)(reg - 1);

            byte[] pdu = new byte[]
            {
                0x03,
                (byte)(addr >> 8),
                (byte)(addr & 0xFF),
                (byte)(len >> 8),
                (byte)(len & 0xFF)
            };

            int expected = 2 + len * 2; // func + byte count + data
            byte[] data = Exchange(pdu, expected).Skip(2).ToArray();
            ushort[] result = new ushort[len];
            for(int i = 0; i<len; i++)
            {
                byte[] b = { data[1], data[0] };
                result[i] = BitConverter.ToUInt16(b);
                data = data.Skip(2).ToArray();
            }
            return result;
        }

        //public void WriteMultipleRegisters(ushort reg, ushort[] values)
        //{
        //    ushort addr = (ushort)(reg - 1);
        //    ushort count = (ushort)values.Length;

        //    byte[] pdu = new byte[6 + values.Length*2];

        //    pdu[0] = 0x10;
        //    pdu[1] = (byte)(addr >> 8);
        //    pdu[2] = (byte)(addr & 0xFF);
        //    pdu[3] = (byte)(count >> 8);
        //    pdu[4] = (byte)(count & 0xFF);
        //    pdu[5] = (byte)(values.Length);

        //    for (int i = 0; i < values.Length; i+=2)
        //    {
        //        pdu[6 + i] = (byte)(values[0]>>8);
        //        pdu[6 + i + 1] = (byte)(values[1] & 0xFF);
        //    }

        //    byte[] result = Exchange(pdu, 5);
        //}
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
        //public void WriteUInt16(ushort reg, ushort value, WriteFunctions func = WriteFunctions.One_Holding)
        //{
        //    byte[] b = ConvertModBus.ConvertUInt16ToByteMes(value);
        //    Write(reg, value);
        //}
        ////public void WriteInt16(ushort reg, short value, WriteFunctions func = WriteFunctions.One_Holding)
        ////{
        ////    //byte[] b = ConvertModBus.ConvertInt16ToByteMes(value);
        ////    Write(reg, value);
        ////}
        //public void WriteSwFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        //{
        //    //byte[] b = ConvertModBus.ConvertSWFloatToByteMes(value);
        //    ushort[] values = ConvertModBus.ConvertSwFloatToUInt16(value);
        //    WriteUInt16(reg, values[0]);
        //    WriteUInt16((ushort)(reg+1), values[1]);
        //    //WriteMultiple(reg, b, (byte)func);
        //}
        //public void WriteFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        //{
        //    ushort[] values = ConvertModBus.ConvertSwFloatToUInt16(value);
        //    WriteUInt16(reg, values[0]);
        //    WriteUInt16((ushort)(reg + 1), values[1]);
        //    byte[] b = ConvertModBus.ConvertFloatToByteMes(value);
        //    WriteMultiple(reg, b, (byte)func);
        //}
        //public float ReadFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        //{
        //    ushort[] value = Read(reg, (byte)func, 2);
        //    //byte[] value = [b[3], b[4], b[5], b[6]];
        //    return ConvertModBus.ConvertUInt16ToFloat(value);
        //}
        //public float ReadSwFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        //{
        //    ushort[] value = Read(reg, (byte)func, 2);
        //    //byte[] value = [b[3], b[4], b[5], b[6]];
        //    return ConvertModBus.ConvertUInt16ToSwFloat(value);
        //}
        //public ushort ReadUInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        //{
        //    ushort[] value = Read(reg, (byte)func, 1);
        //    return value[0];
        //}
        ////public short ReadInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        ////{
        ////    ushort[] value = Read(reg, (byte)func, 1);
        ////    return value[0];
        ////}
        ///


        public void WriteUInt16(ushort reg, ushort value, WriteFunctions func = WriteFunctions.One_Holding)
        {
            //byte[] b = ConvertModBus.ConvertUInt16ToByteMes(value);
            //Write(reg, b, (byte)func);


            WriteReg(reg, value);
        }
        //public void WriteInt16(ushort reg, short value, WriteFunctions func = WriteFunctions.One_Holding)
        //{
        //    //byte[] b = ConvertModBus.ConvertInt16ToByteMes(value);
        //    //Write(reg, b, (byte)func);

        //    WriteReg(reg, value);
        //}
        public void WriteSwFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            byte[] b = ConvertModBus.ConvertSWFloatToByteMes(value);
            //WriteMultiple(reg, b, (byte)func);
            ushort[] v = new ushort[2];
            v[0] = BitConverter.ToUInt16(b, 0);
            v[1] = BitConverter.ToUInt16(b, 2);

            WriteReg(reg, v[0]);
            WriteReg((ushort)(reg + 1), v[1]);
        }
        public void WriteFloat(ushort reg, float value, WriteFunctions func = WriteFunctions.Many_Holding)
        {
            byte[] b = ConvertModBus.ConvertFloatToByteMes(value);
            //WriteMultiple(reg, b, (byte)func);
            ushort[] v = new ushort[2];
            v[0] = BitConverter.ToUInt16(b, 0);
            v[1] = BitConverter.ToUInt16(b, 2);
            WriteReg(reg, v[0]);
            WriteReg((ushort)(reg + 1), v[1]);
        }
        public float ReadFloat(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {

            ushort[] i = ReadReg(reg, 2);
            return ConvertModBus.ConvertRegistersToFloat(i);

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

            ushort[] i = ReadReg(reg, 2);
            return ConvertModBus.ConvertRegistersToSwFloat(i);
        }
        public ushort ReadUInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            //byte[]? b = Read(reg, (byte)func, 1);
            //return ConvertModBus.ConvertByteMesToUInt16(b);
            //
            ushort[] i = ReadReg(reg, 1);
            return (ushort)i[0];

        }
        public short ReadInt16(ushort reg, ReadFunctions func = ReadFunctions.Holding)
        {
            //byte[] b = Read(reg, (byte)func, 1);
            //return ConvertModBus.ConvertByteMesToInt16(b);

            ushort[] i = ReadReg(reg, 1);
            return (short)i[0];

        }
        
        public ushort[] TryReadReg(ushort reg, ushort quantity)
        {
            List<ushort[]> list = new List<ushort[]>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(ReadHoldingRegisters(reg, quantity));
                Thread.Sleep(50);
            }
            int count = 0;
            List<ushort[]> values = new List<ushort[]>();
            values.Add(list.First());
            foreach (ushort[] i in list)
            {
                if (values.Any(arr => arr.SequenceEqual(i))) count++;
                else values.Add(i);
            }
            if (values.Count is 1) return list.First();
            else if (values.Count is 2)
            {
                if (quantity > 1)
                {
                    if (values[0][0] is 0 && values[0][1] is 0) return values.Last();
                    if (values[1][0] is 0 && values[1][1] is 0) return values.First();
                }
            }
            throw new Exception("Не удалось прочитать значение с регистра.");
        }

        public ushort[] ReadReg(ushort reg, ushort quantity)
        {
            try
            {
                return ReadHoldingRegisters(reg, quantity);
            }
            catch (Exception ex)
            {
                Connect(IpAddress, 502);
                Thread.Sleep(1000);
                return ReadHoldingRegisters(reg, quantity);
            }
        }
        public void WriteReg(ushort reg, ushort value, int attempt = 0)
        {
            try
            {
                
                int attamts = 0;
                while ((ushort)ReadHoldingRegisters(reg, 1)[0] != value)
                {
                    WriteSingleRegister(reg, value);
                    Thread.Sleep(200);
                    if (attamts == 10) break;
                }
            }
            catch (Exception ex)
            {
                Connect(IpAddress, 502);
                Thread.Sleep(1000);
                WriteSingleRegister(reg, value);
                //LogerViewModel.Instance.Write(IPAddress);
            }
        }
        //public void WriteReg(ushort reg, int[] value)
        //{
        //    try
        //    {
        //        
        //        WriteMultipleRegisters(reg, value);
        //        Thread.Sleep(200);
        //    }
        //    catch (Exception ex)
        //    {
        //        Connect(IpAddress, 502);

        //        Thread.Sleep(200);
        //        WriteMultipleRegisters(reg, value);
        //        Thread.Sleep(200);
        //        //LogerViewModel.Instance.Write(IPAddress);
        //    }
        //}
    }
}