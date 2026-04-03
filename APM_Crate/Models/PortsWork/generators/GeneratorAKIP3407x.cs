using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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

		public override bool OpenPort()
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
					Sleep( 100 );
					ChangeSignalType( SignalType.Sine );				
					Sleep( 100 );

                    SetChannel( channelNum );
					Sleep( 100 );

				}
				return CheckPort();
			} catch
			{
				new Exception("Ошибка открытия порта " + PortName);
				Close();
				return false;
			}
		}
		public override void SetLOAD(string load)
		{
			Sleep(500);
			WriteMessage("OUTP" + channelNum + ":LOAD " + load.Replace(",", "."));
			Sleep(500);
		}
        public override void SetFrequency( string freq )
		{
            Sleep(500);
			WriteMessage( "SOURCE" + channelNum + ":FREQ " + freq.Replace( ",", "." ) + " HZ" );
            Sleep( 500 );
		}

		public override void SetVoltage( string volt )
		{
            Sleep(500);
			WriteMessage( @"SOURCE" + channelNum + ":VOLT " + volt.Replace( ",", "." ) );
            Sleep( 500 );
		}
        
        public override void SetOffset( string value ) 
		{
            Sleep(500);
            //SetVoltage( ( value / 2 ).ToString() ); // ??????????? 
            WriteMessage(@"SOURCE" + channelNum + ":VOLT:OFFSET " + value.Replace(",", "."));
            Sleep(500);
        }
        public override void SetOffset(double value)
        {
            Sleep(500);
            //SetVoltage( ( value / 2 ).ToString() ); // ??????????? 
            value = Math.Round(value, 6);
            SetOffset(value.ToString());
        }
        public override void ClosePort()
		{
			if ( IsOpen )
			{
				WriteRemoteMode( false );
			}
			base.ClosePort();
		}

		public override void ChangeSignalType( SignalType type )
		{
            Sleep(500);
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
			WriteMessage( "SOURCE" + channelNum + ":FUNC " + typeText );
			Sleep( 500 );
		}
        public override double GetVoltage()
        {
            string mes = ReadMessage(@"SOURCE" + channelNum + ":VOLT?");
            return ConvertValue.StringE_ToDouble(mes);
        }
        public override void ChanelOn(int num)
        {
            Sleep(500);
            WriteMessage($"OUTP{num}:STATE ON");
            Sleep(500);
        }
        public override void ChanelOff(int num)
        {
            Sleep(500);
            WriteMessage($"OUTP{num}:STATE OFF");
            Sleep(500);
        }
        public override void SetChannel( int num )
        {
			Sleep(500);
            channelNum = num;
			ChanelOn(channelNum);
            Sleep( 500 );
        }

        public override void SetZeroSignal()
		{
            Sleep(500);
            SetVoltage( VOLTAGERANGE_MIN );
			SetFrequency( "10000" );
		}
	}
}
