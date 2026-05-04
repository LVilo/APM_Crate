using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsWork
{
	public class MultimeterPicotest : PortMultimeter
	{
		private const int SLEEP_READ = 7100;

		private VisaDevice multimeter;

		public MultimeterPicotest()
		{
			multimeter = new VisaDevice();
		}

		public override string GetName()
		{
			return "USB";
		}

		public override async Task<bool> SetName( string name )
		{
			if ( string.IsNullOrEmpty( name ) || !multimeter.OpenVisaDevice( name ) )
			{
				return false;
			}
			await ClosePort();
			return true;
		}

		public override async Task<bool> OpenPort()
		{
            if ( multimeter.isOpened )
			{
				return true;
			}
			Console.WriteLine(GetName());
            multimeter.Close();
            bool result = multimeter.OpenVisaDevice();
            //Debug.WriteLine($"result {result}");
            //writeremotemode
            await WriteBandFilter( BASE_FILTERS[ 1 ].ToString() );
            await SetWorkType( typeMultimeter, typeSignal, false );
			return result;
		}

		public override bool IsOpened()
		{
			return multimeter.isOpened;
		}

		protected override async Task WriteMessage(string message)
		{
			multimeter.Write(message);
		}

		public override async Task<double> GetVoltage( string type, int time )
		{
            await SetWorkType( DEVICE_VOLTMETER, type, true );
            await Sleep(100);
            try
			{
				string result = "";
				if ( ( filter != BASE_FILTERS[ 0 ] ) )
				{
					result = multimeter.Query( MESSAGE_READ );
                    await Sleep( 10 );			
				} else
				{
					multimeter.Write( MESSAGE_READ );
                    await Sleep( SLEEP_READ );
					result = multimeter.Read();
					//System.Windows.Forms.MessageBox.Show( "readed " + result );
				}
				return ConvertString( result.Replace( ".", "," ) );
			} catch ( TimeoutException )
			{
				return DOUBLE_FALSEVALUE;
			}
		}

		public override async Task<double> GetAmperage()
		{
            await SetWorkType( DEVICE_AMMETER, SIGNALTYPE_DC, true );
			string result = multimeter.Query( MESSAGE_READ );
            await Sleep( 10 );
			return ConvertString( result.Replace( ".", "," ) ) * TO_MILLIVALUES;
		}

		public override async Task ClosePort()
		{
			//writelocalmode
			if ( multimeter.isOpened )
			{
                await WriteRemoteMode( false );
			}
			multimeter.Close();
		}
	}
}
