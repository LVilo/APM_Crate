using DynamicData;
using PortsWork;
using Splat.ModeDetection;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace APM_Crate.Models.DevicesModel
{
    public class SG004AProtocol : ModbusRTU
    {
        private const byte FUNC_READ_UINT16 = 0x64;  // 100
        private const byte FUNC_WRITE_UINT16 = 0x65; // 101
        private const byte FUNC_READ_FLOAT = 0x66;   // 102
        private const byte FUNC_WRITE_FLOAT = 0x67;  // 103

        //+1
        public const ushort REG_FIRMWARE_VERSION = 40002;
        public const ushort REG_INPUT_SIGNAL = 40003;
        public const ushort REG_OUTPUT_SIGNAL = 40004;
        public const ushort REG_INPUT_VALUE = 40005;
        public const ushort REG_OUTPUT_VALUE = 40007;
        public const ushort REG_OUTPUT_SWITCH = 40009;
        //

        public const byte SIGNAL_TYPE_CURRENT = 0x01;
        public const byte SIGNAL_TYPE_VOLTAGE = 0x02;
        public const byte SIGNAL_TYPE_FREQUENCY = 0x04;
        public const byte SIGNAL_TYPE_MILLIVOLT = 0x05;
        public const byte SIGNAL_TYPE_RESISTANCE = 0x06;

        public const byte SENSOR_NONE = 0x0;
        public const byte SENSOR_TYPE_S = 0x1;
        public const byte SENSOR_TYPE_B = 0x2;
        public const byte SENSOR_TYPE_E = 0x3;
        public const byte SENSOR_TYPE_K = 0x4;
        public const byte SENSOR_TYPE_R = 0x5;
        public const byte SENSOR_TYPE_J = 0x6;
        public const byte SENSOR_TYPE_T = 0x7;
        public const byte SENSOR_TYPE_N = 0x8;

        public const byte MODE_MV = 0x1;
        public const byte MODE_THERMOCOUPLE = 0x2;
        public const byte MODE_WR_THERMOCOUPLE = 0x3;


        public const byte OUTPUT_OFF = 0;
        public const byte OUTPUT_ON = 1;

        public SG004AProtocol()
        {

            ReadTimeout = 1000;
            WriteTimeout = 1000;
            BaudRate = 115200;
            Parity = Parity.None;
            DataBits = 8;
            StopBits = StopBits.One;
            SlaveAddress = 1;
        }
        public override async Task<bool> OpenPort()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) Linux.Acsessusb(PortName);
                Open();
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return IsOpen;
        }
        public async Task<ushort> ReadUint16(ushort regAddr)
        {
            byte[]? b = await Read(regAddr, FUNC_READ_UINT16, 1);
            if (b != null)
            {
                ushort result =(ushort)(b[3] << 8 | b[4]);
                Debug.WriteLine($"Прочтено из регистра {regAddr} значение {result}");
                return result;
            }
            else return 0;
        }
        public async Task WriteUint16(ushort regAddr, ushort value)
        {
            byte[] b = BitConverter.GetBytes(value);
            await Write(regAddr, b, FUNC_WRITE_UINT16);
        }

        public async Task<float> ReadFloat(ushort regAddr)
        {
            byte[]? b = await Read(regAddr, FUNC_READ_FLOAT, 2);
            if (b != null)
            {
                byte[] value = [b[3], b[4], b[5], b[6]];
                float result = ConvertModBus.ConvertByteMesToFloat(value);
                //Debug.WriteLine($"Прочтено из регистра {regAddr} значение {result}");
                return result;
            }
            else return 0f;
        }
        public async Task WriteFloat(ushort regAddr, float value)
        {
            Debug.WriteLine($"Запись в регистр {regAddr} значение {value}");
            byte[] b = BitConverter.GetBytes(value);
            if(BitConverter.IsLittleEndian)
            Array.Reverse(b);
            await WriteMultiple(regAddr, b, FUNC_WRITE_FLOAT);
        }
        
        public async Task<ushort> ReadOutputMode() => await ReadUint16(REG_OUTPUT_SIGNAL);
        public async Task<ushort> ReadInputMode() => await ReadUint16(REG_INPUT_SIGNAL);

        public async Task<float> ReadValue()
        {
            ushort mode = await ReadInputMode();
            if (mode is (ushort)InputMode.V) return await ReadFloat(REG_INPUT_VALUE) / 1000f;
            else if (mode is (ushort)InputMode.ohm) return await ReadFloat(REG_INPUT_VALUE) / 100f;
            return await ReadFloat(REG_INPUT_VALUE) / 1000f;
        }
        public async Task<float> ReadWritingValue()
        {
            ushort mode = await ReadOutputMode();
            if (mode is (ushort)OutputMode.V) return await ReadFloat(REG_OUTPUT_VALUE) / 1000f;
            else if (mode is (ushort)OutputMode.ohm) return await ReadFloat(REG_OUTPUT_VALUE) / 10f;
            return await ReadFloat(REG_OUTPUT_VALUE) / 1000f;
        }
        public async Task<float> WriteValue(float value)
        {
            ushort mode = await ReadOutputMode();
            if (mode is (ushort)OutputMode.ohm) { value = (float)(Math.Round(value, 1) * 10); }
            else value *= 1000f;
            await WriteFloat(REG_OUTPUT_VALUE, value);
            await WriteOutputSwitch(true);
            return await ReadFloat(REG_OUTPUT_VALUE);
        }

        public enum OutputMode
        {
            mA = 0x0101,
            V = 0x0201,
            XMT = 0x0301,
            Hz = 0x0401,
            mV = 0x0501,
            V24 = 0x0601,
            ohm = 0x0703,
        }
        public enum InputMode
        {
            mA = 0x0101,
            V = 0x0201,
            Hz = 0x0301,
            mV = 0x0401,
            ohm = 0x0503,
        }
        public async Task WriteOutputSwitch(bool enabled)
        {
            ushort sv = (ushort)((enabled ? 1 : 0) + 0x0101);
            await WriteUint16(REG_OUTPUT_SWITCH, sv);
        }
        public async Task ChangeOutputSignal(OutputMode mode)
        {
            await WriteUint16(REG_OUTPUT_SIGNAL, (ushort)mode); 
        }
        public async Task ChangeOutputSignal(ushort mode)
        {
            await WriteUint16(REG_OUTPUT_SIGNAL, mode);
        }

        public async Task ChangeInputSignal(InputMode mode)
        {
            await WriteUint16(REG_INPUT_SIGNAL, (ushort)mode);
            await Task.Delay(100);
        }
        public async Task ChangeInputSignal(ushort mode)
        {
            await WriteUint16(REG_INPUT_SIGNAL, mode);
            await Task.Delay(100);
        }
        public override async Task ClosePort()
        {
            if(IsOpened()) await WriteOutputSwitch(false);
            Close();
        }
    }
}