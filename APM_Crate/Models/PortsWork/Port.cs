using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using static System.Net.Mime.MediaTypeNames;

namespace PortsWork
{
	public class Port : SerialPort
	{
		public static bool stopFlag = false;

		public static int maxTimeoutForAnswer = 10000; // время ожидания появления ответа устройства

		public const int TO_MILLIVALUES = 1000;

		public const string MESSAGE_IDN = "*IDN?";
		public const string MESSAGE_REMOTEMODE = "SYST:REM";
		public const string MESSAGE_LOCALMODE = "SYST:LOC";

		protected const int BASE_BAUDRATE = 9600;
		protected const int BASE_DATABITS = 8;
		protected const int BASE_READTIMEOUT = 750;
		protected const int BASE_WRITETIMEOUT = 750;

        public VisaDeviceInformation usbInfo;
		//public UsbTmcDevice usbtmc;
        public Port()
		{
           // usbtmc = new UsbTmcDevice();
            BaudRate = BASE_BAUDRATE;
			DataBits = BASE_DATABITS;
			Parity = Parity.None;
			StopBits = StopBits.One;
			ReadTimeout = BASE_READTIMEOUT;
			WriteTimeout = BASE_WRITETIMEOUT;
			NewLine = "\n";
		}

		/// <summary>
		/// Возвращает имя порта
		/// </summary>
		/// <returns>Возвращает имя(адрес) порта</returns>
		public virtual string GetName()
		{
			return PortName;
		}

		/// <summary>
		/// Задаёт имя порта
		/// </summary>
		/// <param name="name">Имя(адрес), которое требуется задать</param>
		/// <returns>Успешность операции </returns>
		public virtual async Task<bool> SetName( string name )
		{

			if ( string.IsNullOrEmpty( name ) ) 
				return false;
			
                //usbtmc._path = name;
                PortName = name;
                return true;
		}

		/// <summary>
		/// Подключение порта внешнего устройства
		/// </summary>
		/// <returns>Успешность открытия порта для работы</returns>
		public virtual async Task<bool> OpenPort()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) Linux.Acsessusb(PortName);
			try
			{
				if (!IsOpen)
				{
					Open();
				}
				return true;
			}
			catch (Exception)
			{
				Close();
				return false;
			}
			//}
		}

		/// <summary>
		/// Закрытие работы с портом внешнего устройства
		/// </summary>
		public virtual async Task ClosePort()
		{
             Close();
        }

		public virtual bool IsOpened()
		{
			return IsOpen;
		}

		/// <summary>
		/// Отправка сообщения на внешнее устройство
		/// </summary>
		/// <param name="message">Команда для записи на устройство</param>
		protected virtual async Task WriteMessage( string message )
		{
				await CheckPort();
			DiscardOutBuffer();
			DiscardInBuffer();
			WriteLine(message);
            
		}
        /// <summary>
        /// Чтение с внешнего устройства
        /// </summary>
        /// <param name="message">Команда для записи на устройство</param>
        /// <returns>Ответ внешнего устройства</returns>
        public virtual async Task<string> ReadMessage( string message )
		{
				await CheckPort() ;
                DiscardOutBuffer();
                DiscardInBuffer();
                WriteLine(message);
            return await WaitPortAnswer(2000);
        }

		/// <summary>
		/// Ожидание ответа от внешнего устройства
		/// </summary>
		/// <returns>Наличие ответа</returns>
		protected async Task<string>  WaitPortAnswer()
		{
            return await WaitPortAnswer(maxTimeoutForAnswer);
		}

        /// <summary>
        /// Ожидание ответа от внешнего устройства
        /// </summary>
        /// <param name="time">Время ожидания ответа, мс</param>
        /// <returns>Наличие ответа</returns>
        protected async Task<string> WaitPortAnswer(int timeout)
        {
           string result = null;

            StringBuilder buffer = new StringBuilder();
            int endTime = Environment.TickCount + timeout;

            while (Environment.TickCount < endTime)
            {
                if (BytesToRead > 0)
                {
                    buffer.Append(ReadExisting());

                    // если строка завершается \n
                    if (buffer.Length > 0 && buffer[buffer.Length - 1] == '\n')
                    {
                        result = buffer.ToString().TrimEnd('\r', '\n');
                        return result;
                    }
                }

                Thread.Sleep(10);
            }

			throw new TimeoutException(); // таймаут
        }

        /// <summary>
        /// Подключение режима удалённой работы для внешних устройств
        /// </summary>
        /// <param name="needRemote">Режим удалённой/ручной работы</param>
        protected virtual async Task WriteRemoteMode( bool needRemote )
		{
			if ( needRemote )
			{
				await WriteMessage( MESSAGE_REMOTEMODE );
				await Sleep( 1000 );
			} else
			{
				await WriteMessage( MESSAGE_LOCALMODE );
			}
		}

		/// <summary>
		/// Проверка ответа из порта
		/// </summary>
		/// <returns>Успешность проверки</returns>
		public virtual async Task<bool> CheckPort()
		{
			try
			{
				DiscardOutBuffer();
				DiscardInBuffer();
                WriteLine(MESSAGE_IDN);
                string result = await WaitPortAnswer(2000);
				Console.WriteLine("name: " + result);
				return true;
			}
			catch (TimeoutException)
			{
			   throw new TimeoutException($"Ошибка устройства {PortName}. Время выполнения операции истекло.");
			}
            catch (InvalidOperationException e)
            {
                if (e.Message is "The port is closed.") throw new InvalidOperationException($"Ошибка устройства {PortName}. Порт Закрыт. Невозможно отправить или принять сообщения с устройства");
                else throw new InvalidOperationException(e.Message);
            }
        }

		/// <summary>
		/// Инициализация внешнего устройства конкретного типа
		/// </summary>
		/// <returns>Объект для работы с подключенным внешним устройством</returns>
		public virtual async Task<Port> IdentifyDeviceType()
		{
			return this;
		}

		/// <summary>
		/// Ищет подключенные к USB VISA-устройства
		/// </summary>
		/// <returns>Список описаний найденных устройств</returns>
		public static List<VisaDeviceInformation> FindVisaDevicesInfo()
		{
			VisaDevice device = new VisaDevice();
			List<VisaDeviceInformation> result = new List<VisaDeviceInformation>();
			
			List<string> descripts = device.FindVisaDevice();
			VisaDeviceInformation tmp;
			for ( int i = 0; i < descripts.Count; i++ )
			{
				tmp = new VisaDeviceInformation();
				tmp.description = descripts[ i ];
				if (!device.OpenVisaDevice( tmp.description))
				{
					continue;
				}
				device.GetIdnInfo( MESSAGE_IDN, out tmp.devType, out tmp.serialNum );
				result.Add( tmp );
				device.Close();
			}

			return result;
		}

		/// <summary>
		/// Ожидание системы
		/// </summary>
		/// <param name="time">Время ожидания, мс</param>
		public static async Task Sleep( int time )
		{
			//for ( int i = 0; i < time; i += 10 )
			//{
				await Task.Delay(time);
                if ( stopFlag )
                {
                    stopFlag = false;
                    throw new StopProcessException();
                }
			//}
		}

		/// <summary>
		/// Преобразовывает строку в число с правающей запятой
		/// </summary>
		/// <param name="s">Строка для преобразования в число</param>
		/// <returns>Найденное число</returns>
		protected double ConvertString( string s )
		{
			double d;
			bool findDouble = double.TryParse( s.Replace( ".", "," ), out d );
			if ( !findDouble )
			{
				Console.WriteLine( "I can't convert this value: " + s );
			}
			return d;
		}
        
    }

}
