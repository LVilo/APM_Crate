using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsWork
{
	public class MultimeterAgilent : PortMultimeter
	{
		public MultimeterAgilent( int type )
		{
			DtrEnable = true;
			typeMultimeter = type;
			typeSignal = SIGNALTYPE_AC;
		}

		public MultimeterAgilent()
		{
			ReadTimeout = 3000;
			WriteTimeout = 3000;
			DtrEnable = true;
			typeMultimeter = DEVICE_VOLTMETER;
			typeSignal = SIGNALTYPE_AC;
		}

		public override async Task<bool> OpenPort()
		{
			try
			{
				if ( !IsOpen )
				{
					Open();
					await Sleep( 100 );
                    await WriteRemoteMode( true );
                    await WriteBandFilter( BASE_FILTERS[ 1 ].ToString() );
                    await Sleep( 1000 );
                    await SetWorkType( typeMultimeter, typeSignal, false );
                    await Sleep( 1000 );
					
				}
				return await CheckPort();
			} catch
			{
				new Exception("Ошибка открытия порта " + PortName);
				Close();
				return false;
			}
		}

		public override async Task<double> GetVoltage( string type, int time )
		{
            await SetWorkType( DEVICE_VOLTMETER, type, true );
			try
			{
                await WriteMessage( MESSAGE_READ );
				string result = await WaitPortAnswer();
				return ConvertString(result.Replace(".", ","));
			} catch ( TimeoutException )
			{
				return DOUBLE_FALSEVALUE;
			}
        }

		public override async Task<double> GetAmperage()
		{
			await SetWorkType( DEVICE_AMMETER, SIGNALTYPE_DC, true );
            try
            {
				await WriteMessage( MESSAGE_READ );
                string result = await WaitPortAnswer();
                double res = ConvertString(result.Replace(".", ",")) * TO_MILLIVALUES;
                Console.WriteLine("Считанный ток " + res);
                return res;
            }
            catch (TimeoutException)
            {
                return DOUBLE_FALSEVALUE;
            }
			
		}

		public override async Task ClosePort()
		{
			if ( IsOpen )
			{
				await WriteRemoteMode( false );
			}
            await base.ClosePort();
		}
	}
}
