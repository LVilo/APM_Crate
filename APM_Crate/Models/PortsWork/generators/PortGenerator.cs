using System;
using System.Threading.Tasks;

namespace PortsWork
{
	public class PortGenerator : Port
	{
		public enum SignalType { Sine, DC }

        public int channelNum = 1;

        public double VOLTAGERANGE_MIN
		{
			get;
			protected set;
		}

        //public static void ConvertPPToRMS(double value, out double ConvertValue) => ConvertValue = value / 2d / Math.Sqrt(2d);
        //public static void ConvertRMS(double value, out double ConvertValue) => ConvertValue = value * 2d * Math.Sqrt(2d);
        /// <summary>
        /// Задание сопротивления
        /// </summary>
        /// <param name="ohm">Сопротивление, Ом</param>
        public virtual async Task SetLOAD(string ohm)
        {
            await WriteMessage("SOURCE" + channelNum + ":LOAD " + ohm);
             await Sleep(500);
        }

        /// <summary>
        /// Задание сопротивления
        /// </summary>
        /// <param name="ohm">Частота, Гц</param>
        public async Task SetLOAD(uint ohm)
        {
            await SetLOAD(ohm.ToString());
        }

        /// <summary>
        /// Задание частоты
        /// </summary>
        /// <param name="freq">Частота, Гц</param>
        public virtual async Task SetFrequency( string freq )
		{
            await WriteMessage("SOURCE" + channelNum + ":FREQ " + freq.Replace(",", ".") + " HZ");
             await Sleep(500);
        }

		/// <summary>
		/// Задание частоты
		/// </summary>
		/// <param name="freq">Частота, Гц</param>
		public async Task SetFrequency( double freq )
		{
			freq = Math.Round( freq, 6 );
			await SetFrequency( freq.ToString() );
		}

        public virtual async Task ChanelOn(int num)
        {

        }
        public virtual async Task ChanelOff(int num)
        {

        }
        /// <summary>
        /// Задание амплитуды
        /// </summary>
        /// <param name="volt">Напряжение, В</param>
        public virtual async Task SetVoltage( string volt )
		{
		}
        public virtual async Task<double> GetVoltage()
        {
            return 0d;
        }
        public virtual async Task<double> GetMaxPP()
        {
            await SetOffset( 0 );
            await Sleep( 500 );
            await SetVoltage("30");
           return await GetVoltage();
        }
        public virtual async Task<double> GetMaxRMS()
        {
            await SetOffset(0);
             await Sleep(500);
            await SetVoltage("30");
            return ConvertValue.ToRMS(await GetVoltage());
        }
        /// <summary>
        /// Задание амплитуды
        /// </summary>
        /// <param name="volt">Напряжение, В</param>
        public async Task SetVoltage( double volt )
		{
			volt = Math.Round( volt, 6 );
			await SetVoltage( volt.ToString() );
		}

        public virtual async Task SetOffset(string value)
        {
            //SetVoltage( ( value / 2 ).ToString() ); // ??????????? 
            await WriteMessage(@"SOURCE" + channelNum + ":OFST " + value.Replace(",", "."));
             await Sleep(500);
        }
        
        /// <summary>
        /// Выставление типа сигнала
        /// </summary>
        /// <param name="type">Вид сигнала</param>
        public virtual async Task ChangeSignalType( SignalType type )
		{
		}

		/// <summary>
		/// Выставление нулевой амплитуды
		/// </summary>
		public virtual async Task SetZeroSignal()
		{
		}

		/// <summary>
		/// Задание постоянного напряжения
		/// </summary>
		/// <param name="value"></param>
		public virtual async Task SetOffset( double value )
		{

		}

        public virtual async Task SetChannel( int num )
        {

        }

		public override async Task<Port> IdentifyDeviceType()
		{
            try
			{
                Port result = new PortGenerator();
                if ( !GetName().Contains( "USB" )  || GetName().Contains("ttyUSB"))
                {
                    
                    if (!await OpenPort())
                    {
                        await result.SetName(GetName());
                        return result;
                    }
                    string idn = await ReadMessage(MESSAGE_IDN);
                    await ClosePort();
                    if (idn.Contains("Suin"))
                    {
                        result = new GeneratorAKIP3407x();
                    }
                    else if (idn.Contains("GSS"))
                    {
                        result = new GeneratorGSSxx();
                    }
                    await result.SetName(GetName());
                    return result;
                }
				else
				{
                    if (usbInfo.devType.Contains("AKIP-3409"))
                    {
                        result = new GeneratorAKIP3409x();
                    }
                    else
                    {
                        result = new GeneratorSDG1xxx();
                    }
                    await result.SetName(GetName());
                    return result;
                }
				
			} catch ( Exception e )
			{
                new Exception(e.Message);
				Port result = new PortGenerator();
				await result.SetName( GetName() );
				return result;
			}
		}
	}
}
