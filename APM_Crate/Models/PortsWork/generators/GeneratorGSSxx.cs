using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PortsWork
{
	public class GeneratorGSSxx : PortGenerator
	{
		public const string SIGNALTYPE_SIN = "SIN";
		public const string SIGNALTYPE_DCVOLTAGE = "P_DC";

		public GeneratorGSSxx()
		{
			VOLTAGERANGE_MIN = 0.002;
		}

		public override async Task<bool> OpenPort()
		{

			try
			{
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) Linux.Acsessusb(PortName);
                if ( !IsOpen )
				{
					Open();
					await Sleep(100);
					await ChangeSignalType( SignalType.Sine );				
					await Sleep(1000);
				}
				return await CheckPort();
			} catch
			{
				new Exception("Ошибка открытия порта " + PortName);
				Close();
				return false;
			}
		}

		public override async Task SetFrequency( string freq )
		{
            await WriteMessage( "FREQ " + freq.Replace( ",", "." ) + " HZ" );
            await Sleep( 50 );
		}

		public override async Task SetVoltage( string volt )
		{
            await WriteMessage( @"VOLT " + volt.Replace( ",", "." ) );
            await Sleep( 50 );
		}
        public override async Task<double> GetVoltage()
        {
            string mes = await ReadMessage(@"VOLT?");
            return ConvertValue.StringE_ToDouble(mes);
        }
        public override async Task SetOffset( double value )
		{
            await SetVoltage( ( value / 2 ).ToString() );
		}

		public override async Task ClosePort()
		{
			if ( IsOpen )
			{
                await WriteRemoteMode( false );
			}
			await base.ClosePort();
		}

		public override async Task ChangeSignalType( SignalType type )
		{
			string typeText = "";
			switch (type )
			{
				case SignalType.Sine:
					typeText = SIGNALTYPE_SIN;
					break;
				case SignalType.DC:
					typeText = SIGNALTYPE_DCVOLTAGE;
					break;
			}
            await WriteMessage( "FUNC:SHAPE " + typeText );
            await Sleep( 50 );
		}

		public override async Task SetZeroSignal()
		{
            await SetVoltage( VOLTAGERANGE_MIN );
            await SetFrequency( "1 MHZ" );
		}
	}
}
