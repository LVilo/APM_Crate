using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortsWork
{
	public class GeneratorAKIP3407x : PortGenerator
	{
		public const string SIGNALTYPE_SIN = "SIN";
		public const string SIGNALTYPE_DCVOLTAGE = "P_DC";

		public GeneratorAKIP3407x()
		{
			VOLTAGERANGE_MIN = 0.002;
		}

		public override async Task<bool> OpenPort()
		{
			Console.WriteLine("------------OpenPort");

            try
			{
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) Linux.Acsessusb(PortName);
                if ( !IsOpen )
				{
                    Console.WriteLine("------------if");
                    Console.WriteLine(GetName());
                    Open();
					 await Sleep( 100 );
					await ChangeSignalType( SignalType.Sine );				
					 await Sleep( 100 );

                    await SetChannel( channelNum );
					 await Sleep( 100 );

				}
				return await CheckPort();
			} catch
			{
				new Exception("Ошибка открытия порта " + PortName);
				Close();
				return false;
			}
		}
		public override async Task SetLOAD(string load)
		{
			await Sleep(500);
			await WriteMessage("OUTP" + channelNum + ":LOAD " + load.Replace(",", "."));
            await Sleep(500);
		}
        public override async Task SetFrequency( string freq )
		{
             await Sleep(500);
			await WriteMessage( "SOURCE" + channelNum + ":FREQ " + freq.Replace( ",", "." ) + " HZ" );
             await Sleep( 500 );
		}

		public override async Task SetVoltage( string volt )
		{
             await Sleep(500);
			await WriteMessage( @"SOURCE" + channelNum + ":VOLT " + volt.Replace( ",", "." ) );
             await Sleep( 500 );
		}
        
        public override async Task SetOffset( string value ) 
		{
             await Sleep(500);
            //SetVoltage( ( value / 2 ).ToString() ); // ??????????? 
            await WriteMessage(@"SOURCE" + channelNum + ":VOLT:OFFSET " + value.Replace(",", "."));
             await Sleep(500);
        }
        public override async Task SetOffset(double value)
        {
             await Sleep(500);
            //SetVoltage( ( value / 2 ).ToString() ); // ??????????? 
            value = Math.Round(value, 6);
            await SetOffset(value.ToString());
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
             await Sleep(500);
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
			await WriteMessage( "SOURCE" + channelNum + ":FUNC " + typeText );
			 await Sleep( 500 );
		}
        public override async Task<double> GetVoltage()
        {
            string mes = await ReadMessage(@"SOURCE" + channelNum + ":VOLT?");
            return ConvertValue.StringE_ToDouble(mes);
        }
        public override async Task ChanelOn(int num)
        {
             await Sleep(500);
            await WriteMessage($"OUTP{num}:STATE ON");
             await Sleep(500);
        }
        public override async Task ChanelOff(int num)
        {
             await Sleep(500);
            await WriteMessage($"OUTP{num}:STATE OFF");
             await Sleep(500);
        }
        public override async Task SetChannel( int num )
        {
			 await Sleep(500);
            channelNum = num;
			await ChanelOn(channelNum);
             await Sleep( 500 );
        }

        public override async Task SetZeroSignal()
		{
             await Sleep(500);
            await SetVoltage( VOLTAGERANGE_MIN );
			await SetFrequency( "10000" );
		}
	}
}
