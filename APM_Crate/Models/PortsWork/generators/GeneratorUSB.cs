using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PortsWork
{
    public class GeneratorUSB : PortGenerator
    {
        public const string SIGNALTYPE_SIN = "SINE";
        public const string SIGNALTYPE_DCVOLTAGE = "DC";

        protected VisaDevice generator;

        public override string GetName()
        {
            return "USB";
        }

        public override async Task<bool> SetName( string name )
        {
            if ( string.IsNullOrEmpty( name ) || !generator.OpenVisaDevice( name ) )
            {
                return false;
            }
            await ClosePort();
            return true;
        }

        public override async Task<bool> OpenPort()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) Linux.Acsessusb(PortName);
            if ( generator.isOpened )
            {
                return true;
            }
            bool result = generator.OpenVisaDevice();
            await ChangeSignalType( SignalType.Sine );
            await SetZeroSignal();
            await SetChannel( channelNum );
            return result;
        }

        public override bool IsOpened()
        {
            return generator.isOpened;
        }

        protected override async Task WriteMessage( string message )
        {
            generator.Write( message );
            await Sleep( 10 );
        }
        public override async Task SetLOAD(string ohm)
        {
            await WriteMessage("C" + channelNum + ":BSWV LOAD," + ohm + "\n");
        }
        public override async Task SetFrequency( string freq )
        {
            await WriteMessage( "C" + channelNum + ":BSWV FRQ, " + freq.Replace( ",", "." ) + "\n" );
        }

        public override async Task SetVoltage( string volt )
        {
            await WriteMessage( "C" + channelNum + ":BSWV AMP, " + volt.Replace( ",", "." ) + "\n" );
        }

        public override async Task SetOffset( double value )
        {
            await WriteMessage( "C" + channelNum + ":BSWV OFST, " + value.ToString().Replace( ",", "." ) + "\n" );
        }

        public override async Task SetZeroSignal()
        {
            await SetVoltage( VOLTAGERANGE_MIN );
        }
        public override async Task<double> GetVoltage()
        {
            string mes = await ReadMessage($"C{channelNum}:BSWV AMP?");
            return ConvertValue.StringE_ToDouble(mes);
        }
        public override async Task ChanelOn(int num)
        {
            await WriteMessage($"C{num}:OUTP ON\n");
        }
        public override async Task ChanelOff(int num)
        {
            await WriteMessage($"C{num}:OUTP OFF\n");
        }
        public override async Task SetChannel( int num )
        {
            channelNum = num;
            await ChanelOn(channelNum);
        }

        public override async Task ClosePort()
        {
            generator.Close();
        }

    }
}
