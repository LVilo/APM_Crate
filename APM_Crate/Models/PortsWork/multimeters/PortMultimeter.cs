using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PortsWork
{
	public class PortMultimeter : Port
	{
		public const int DEVICE_VOLTMETER = 0;
		public const int DEVICE_AMMETER = 1;

		public const string SIGNALTYPE_DC = "DC";
		public const string SIGNALTYPE_AC = "AC";

		protected const double DOUBLE_FALSEVALUE = 0;
		protected const string STRING_FALSEVALUE = "";

		protected const int ATTEMPTS_BASE = 3;
		protected const double START_VOLT_COEFF = 1; // was 1.88
		protected const string MESSAGE_BANDFILTER = "DET:BAND";
		protected const string MESSAGE_READ = "READ?";
		protected int[] BASE_FILTERS = { 3, 20, 200 };

		protected int typeMultimeter;
		protected int filter = 20;
		protected string typeSignal; // DC & AC
		protected int lowFilter = 10;
		protected int highFilter = 200;

		private double voltage = 0d;
		public PortMultimeter( int type )
		{
			DtrEnable = true;
			typeMultimeter = type;
		}

		public PortMultimeter()
		{
			ReadTimeout = 3000;
			WriteTimeout = 3000;
			DtrEnable = true;
			typeMultimeter = DEVICE_VOLTMETER;
		}

		/// <summary>
		/// Переключает мультиметр в режим вольтметра
		/// </summary>
		/// <param name="type">Задаёт тип считываемого сигнала (AC/DC)</param>
		public async Task VoltmeterMode( string type )
		{
			typeMultimeter = DEVICE_VOLTMETER;
			typeSignal = type;
			await WriteMessage( "FUNC \"VOLT:" + type + "\"" );
			await Sleep( 500 );
		}

		/// <summary>
		/// Переключает мультиметр в режим амперметра
		/// </summary>
		/// <param name="type">Задаёт тип считываемого сигнала (AC/DC)</param>
		public async Task AmmeterMode( string type )
		{
			typeMultimeter = DEVICE_AMMETER;
			typeSignal = type;
			await WriteMessage( "FUNC \"CURR:" + type + "\"" );
			await Sleep( 500 );

        }

		/// <summary>
		/// Переключает мультиметр в заданный режим
		/// </summary>
		/// <param name="workType">Режим мультиметра</param>
		/// <param name="signal">Тип считываемого сигнала (AC/DC)</param>
		protected async Task SetWorkType( int workType, string signal, bool canCheck )
		{
			if ( canCheck && ( workType == typeMultimeter ) && ( signal == typeSignal ) )
			{
				return;
			}
			if ( workType == DEVICE_VOLTMETER )
			{
				await VoltmeterMode( signal );
			} else
			{
				await AmmeterMode( signal );
			}
		}

		/// <summary>
		/// Задаёт тип частотной фильтрации
		/// </summary>
		/// <param name="filterValue"></param>
		protected async Task WriteBandFilter( string filterValue )
		{
			await WriteMessage( MESSAGE_BANDFILTER + " " + filterValue );
		}

		protected double CountStartVoltage( double resultVoltage, double freq )
		{
			return freq >= 20 ? resultVoltage / START_VOLT_COEFF : 
				Math.Pow( 20 / freq, 2 ) * resultVoltage / START_VOLT_COEFF;
		}

		/// <summary>
		/// Считывает ток
		/// </summary>
		/// <returns>Значение постоянного тока, в мА</returns>
		public virtual async Task<double> GetAmperage()
		{
			return 0;
		}

		/// <summary>
		/// Считывает среднее напряжение
		/// </summary>
		/// <param name="type">Тип сигнала (AC/DC)</param>
		/// <param name="time">Время ожидания ответа с мультиметра</param>
		/// <param name="iterations">Число точек для усрежнения</param>
		/// <returns>Значение напряжения, В</returns>
		public async Task<double> GetVoltage( string type, int time, int iterations )
		{
			double average = 0;
			for ( int i = 0; i < iterations; i++ )
			{
				double result = await GetVoltage( type, time );
				average += result;
			}
			average /= iterations;
			return average;
		}

		/// <summary>
		/// Считывает напряжение
		/// </summary>
		/// <param name="type">Тип сигнала (AC/DC)</param>
		/// <param name="time">Время ожидания ответа с мультиметра</param>
		/// <returns>Значение напряжения, В</returns>
		public virtual async Task<double> GetVoltage( string type, int time )
		{
			await VoltmeterMode(type);
            string result = await ReadMessage(MESSAGE_READ);
            if (double.TryParse(result, NumberStyles.Any, CultureInfo.InvariantCulture, out double voltage))
            {
				return voltage;
            }
            else
            {
				
                Console.WriteLine($"Невозможно преобразовать значение: {result}");
            }
            await Sleep(time);
			return 0;
		}

		/// <summary>
		/// Считывает получившееся значение напряжения после его задания на генераторе
		/// </summary>
		/// <param name="generator">Генератор, задающий сигнал</param>
		/// <param name="volt">Переменное напряжение, которое требуется задать, В</param>
		/// <returns>Действительное значение полученного переменного напряжения, В</returns>
		private async Task<double> RealVoltage( PortGenerator generator, double volt )
		{
			volt = Math.Round( volt, 6 );
            //MessageBox.Show( "Устанавливаю на генераторе: " + volt + " В" );
			await generator.SetVoltage( volt );
			double d = 0;
			for ( int i = 0; i < 2; i++ )
			{
				d += await GetVoltage( SIGNALTYPE_AC, 30 ) / 2.0;
			}
            //MessageBox.Show( "На вольтметре измерено: " + d.ToString() + " В" );
			return d;
		}

		/*
		 * 
		 * удалено тк нет надобности
		/// <summary>
		/// Установка переменного напряжения с проверкой после ожидания выхода системы на режим
		/// </summary>
		/// <param name="generator">Генератор, задающий сигнал</param>
		/// <param name="idealVolt">Напряжение, которое требуется получить, мВ</param>
		/// <param name="freq">Частота, для которой задаётся напряжение, Гц</param>
		/// <param name="accuracyPercent">Требуемая точность задания напряжения</param>
		/// <param name="sleep">Временная задержка после первичного задания напряжения, мс</param>
		/// <param name="attempts">Максимальное число попыток выставить требуемое напряжение (для 1 проверки)</param>
		/// <param name="realVoltage">Измеренное на мультиметре напряжение, мВ</param>
		/// <returns>Успешность задания напряжения с требуемой точностью</returns>
		public async Task<bool> SetVoltage( PortGenerator generator, double idealVolt, double freq,
		    double accuracyPercent, int sleep, int attempts, out double realVoltage )
		{
			realVoltage = 0;
			double voltPercentage = accuracyPercent;
			for ( int i = 0; ( i < ATTEMPTS_BASE ) && ( voltPercentage >= accuracyPercent ); i++ )
			{
				await SetVoltage( generator, idealVolt, freq, accuracyPercent, attempts );
				await Sleep( sleep );
				realVoltage = await GetVoltage( SIGNALTYPE_AC, 30 ) * TO_MILLIVALUES;
				voltPercentage = Math.Abs( realVoltage - idealVolt ) / idealVolt;
			}
			return (voltPercentage < accuracyPercent);
		}
		*/

		/// <summary>
		/// Установка переменного напряжения
		/// </summary>
		/// <param name="generator">Генератор, задающий сигнал</param>
		/// <param name="volt">Напряжение, которое требуется получить, мВ</param>
		/// <param name="freq">Частота, для которой задаётся напряжение, Гц</param>
		/// <param name="accuracyPercent">Требуемая точность задания напряжения</param>
		/// <param name="attempts">Максимальное число попыток выставить требуемое напряжение</param>
		/// <returns>Успешность задания напряжения с требуемой точностью</returns>
		public async Task<bool> SetVoltage( PortGenerator generator, double volt, double freq, double accuracyPercent, int attempts )
		{
			double coeff = 2 * Math.Sqrt(2);
			double voltPercentage = accuracyPercent;
			double t = CountStartVoltage( volt, freq ) * coeff / TO_MILLIVALUES;
			await ChangeFilter( freq );
			if ( volt == 0 )
			{
				await generator.SetZeroSignal();
				return !stopFlag;
			}
			for ( int i = 0; ( i < attempts ) && ( voltPercentage >= accuracyPercent ) ; i++ )
			{
				Console.WriteLine( "Попытка выставить напряжение на генераторе: " + t );
				double currVoltage = await RealVoltage( generator, t ) * TO_MILLIVALUES;
				voltPercentage = Math.Abs( currVoltage - volt ) / volt;
				Console.WriteLine( "Показания вольтметра: " + ( currVoltage ) + " мВ" );
				t = ( t * volt ) / ( currVoltage );
			}

			return ( voltPercentage < accuracyPercent );
		}

		/// <summary>
		/// Установка постоянного напряжения
		/// </summary>
		/// <param name="generator">Генератор, задающий сигнал</param>
		/// <param name="volt">Требуемое напряжение, В</param>
		/// <param name="accuracyPercent">Требуемая точность задания напряжения</param>
		/// <param name="attempts">Максимальное число попыток выставить требуемое напряжение</param>
		/// <returns>Успешность задания напряжения с требуемой точностью</returns>
		public async Task<bool> SetOffset( PortGenerator generator, double volt, double accuracyPercent, int attempts )
		{
			double voltPercentage = accuracyPercent;
			double t = volt;
			for ( int i = 0; (i < attempts) && (voltPercentage >= accuracyPercent ) ; i++ )
			{
				await generator.SetOffset( t );
				double currVoltage = await GetVoltage( SIGNALTYPE_DC, 30 );
				voltPercentage = Math.Abs( currVoltage - volt ) / volt;
				Console.WriteLine( "Показания вольтметра: " + ( currVoltage * TO_MILLIVALUES ) + " мВ" );
				t = ( t * volt ) / ( currVoltage );
			}
			return ( voltPercentage < accuracyPercent );
		}

		/// <summary>
		/// Задаёт частоты, для которых требуется сменить частотный фильтр вольтметра
		/// </summary>
		/// <param name="low">Частота включения фильтра для низких частот</param>
		/// <param name="high">Частота включения фильтра для высоких частот</param>
		public void SetFilters( int low, int high )
		{
			lowFilter = low;
			highFilter = high;
		}

		/// <summary>
		/// Устанавливает частотный фильтр
		/// </summary>
		/// <param name="freq">Частота, на которой будет фильтроваться сигнал</param>
		public async Task ChangeFilter( double freq )
		{
			if ( ( freq < lowFilter ) && ( filter != BASE_FILTERS[ 0 ] ) )
			{
				filter = BASE_FILTERS[ 0 ];
				await WriteBandFilter( filter.ToString() );
                await Sleep( 750 );
			} else if ( ( freq >= lowFilter ) && ( freq < highFilter ) && ( filter != BASE_FILTERS[ 1 ] ) )
			{
				filter = BASE_FILTERS[ 1 ];
                await WriteBandFilter( filter.ToString() );
                await Sleep( 750 );
			} else if ( ( freq >= highFilter ) && ( filter != BASE_FILTERS[ 2 ] ) )
			{
				filter = BASE_FILTERS[ 2 ];
                await WriteBandFilter( filter.ToString() );
                await Sleep( 750 );
			}
		}

		public override async Task<Port> IdentifyDeviceType()
		{
			try
			{
				Port result = new MultimeterAgilent();
				if (PortName.Contains("/dev/usbtmc"))
				{
                    result = new UsbTmcDevice();
                }
				else if (!PortName.Contains("COM"))
				{
                    result = new MultimeterPicotest();
				}
				await result.SetName(PortName);
                return result;
			} catch ( Exception e )
			{
				new Exception(e.Message);

                return null;
			}
		}

	}
}
