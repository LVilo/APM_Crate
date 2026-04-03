using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PortsWork
{
    public class ModbusTCP : Modbus
    {
        public const int PORT_CONNECTION = 502;

        private TcpClient client = new TcpClient();
        private NetworkStream stream;

        public byte SlaveAddress { get; set; } = 1;

        private ushort transactionId = 0;
        
        public override bool OpenPort()
        {
            client = new TcpClient(PortName, PORT_CONNECTION);
            IPAddress ip = IPAddress.Parse(PortName);
            client.Connect(ip, PORT_CONNECTION);
            stream = client.GetStream();
            return client.Connected;
        }

        public override void ClosePort()
        {
            stream?.Close();
            client?.Close();
        }

        public override bool IsOpened()
        {
            return client.Connected;
        }
        private byte[] BuildMBAP(byte[] pdu)
        {
            transactionId++;

            byte[] header = new byte[7];

            header[0] = (byte)(transactionId >> 8);
            header[1] = (byte)(transactionId & 0xFF);

            header[2] = 0;
            header[3] = 0;

            ushort len = (ushort)(pdu.Length + 1);

            header[4] = (byte)(len >> 8);
            header[5] = (byte)(len & 0xFF);

            header[6] = SlaveAddress;

            return header.Concat(pdu).ToArray();
        }

        protected override byte[] Exchange(byte[] frame, int expectedLength, uint attempt = 0)
        {
            if (client == null || !client.Connected)
                throw new Exception("Устройство не подключено");

            stream.Write(frame, 0, frame.Length);

            byte[] resp = new byte[expectedLength];
            int read = 0;

            while (read < expectedLength)
                read += stream.Read(resp, read, expectedLength - read);

            return resp;
        }
        
        protected override bool Write(ushort reg, byte[] value, byte func)
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

            byte[] frame = BuildMBAP(pdu);

            byte[] b = Exchange(frame, 12);
            byte[] result = [b[5], b[4]];
            if (result.SequenceEqual(value)) return true;
            else return false;
        }

        protected override byte[] Read(ushort reg, byte func, ushort len)
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

            byte[] frame = BuildMBAP(pdu);

            int expected = 9 + len * 2;

            return Exchange(frame, expected);
        }

        protected override void WriteMultiple(ushort reg, byte[] values, byte func = 0x10)
        {
            ushort addr = (ushort)(reg - 1);
            ushort count = (ushort)(values.Length / 2);

            byte[] pdu = new byte[6 + values.Length];

            pdu[0] = func;
            pdu[1] = (byte)(addr >> 8);
            pdu[2] = (byte)(addr & 0xFF);
            pdu[3] = (byte)(count >> 8);
            pdu[4] = (byte)(count & 0xFF);
            pdu[5] = (byte)values.Length;

            for (int i = 0; i < values.Length; i++)
                pdu[6 + i] = values[i];

            byte[] frame = BuildMBAP(pdu);

            Exchange(frame, 12);
        }
    }
}