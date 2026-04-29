using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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

        public async Task Connect(string ip)
        {
            IpAddress = ip;
            _client = new TcpClient();
           await _client.ConnectAsync(IpAddress, Port);
            _stream = _client.GetStream();
        }
        public async Task Connect(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
            _client = new TcpClient();
            await _client.ConnectAsync(IpAddress, Port);
            _stream = _client.GetStream();
        }
        public async Task Connect()
        {
            _client = new TcpClient();
            await _client.ConnectAsync(IpAddress, Port);
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
            if (!IsConnected())
            {
               _client.Connect(IpAddress, Port);
                if (!IsConnected())
                {
                    throw new Exception("TCP not connected");
                }
            }

            byte[] frame = BuildMbapHeader(pdu);

            _stream.Write(frame, 0, frame.Length);

            byte[] response = new byte[expectedLength + 7]; // +MBAP
            int read = 0;

            try
            {
                while (read < response.Length)
                    read += _stream.Read(response, read, response.Length - read);
            }
            catch
            {
                if (attempt == 10)
                    throw;

               _client.Connect(IpAddress, Port);
                return Exchange(pdu, expectedLength, attempt + 1);
            }

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

        public bool Write(ushort reg, byte[] value, byte func)
        {
            ushort addr = (ushort)(reg - 1);
            ushort val = (ushort)(value[1] << 8 | value[0]);

            byte[] pdu = new byte[]
            {
                func,
                (byte)(addr >> 8),
                (byte)(addr & 0xFF),
                (byte)(val >> 8),
                (byte)(val & 0xFF)
            };

            byte[] resp = Exchange(pdu, 5);

            byte[] result = [resp[3], resp[4]];
            return result.SequenceEqual(value);
        }

        public byte[] Read(ushort reg, byte func, ushort len)
        {
            ushort addr = (ushort)(reg - 1);

            byte[] pdu = new byte[]
            {
                func,
                (byte)(addr >> 8),
                (byte)(addr & 0xFF),
                (byte)(len >> 8),
                (byte)(len & 0xFF)
            };

            int expected = 2 + len * 2; // func + byte count + data

            return Exchange(pdu, expected);
        }

        public void WriteMultiple(ushort reg, byte[] values, byte func = 0x10)
        {
            ushort addr = (ushort)(reg - 1);
            ushort count = (ushort)(values.Length / 2);

            byte[] pdu = new byte[6 + values.Length];

            pdu[0] = func;
            pdu[1] = (byte)(addr >> 8);
            pdu[2] = (byte)(addr & 0xFF);
            pdu[3] = (byte)(count >> 8);
            pdu[4] = (byte)(count & 0xFF);
            pdu[5] = (byte)(values.Length);

            for (int i = 0; i < values.Length; i++)
                pdu[6 + i] = values[i];

            Exchange(pdu, 5);
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