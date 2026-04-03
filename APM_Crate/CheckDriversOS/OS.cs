using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Desktop
{
    public abstract class OS
    {
        public abstract void CheckDrivers();
        protected abstract bool IsNet8_0_20_Installed();
        public static OS Detect()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new Windows();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new Linux();
            }
            else
            {
                throw new PlatformNotSupportedException("Неизвестная операционная система");
            }
        }

        protected static void ShowRMSisaErrorMessage()
        {
            string mes = "Для работы приложения требуется драйвер RS VISA 5.5.5.\n\n" +
                    "Пожалуйста, установите RS_VISA_Setup_Win_5_5_5 и перезапустите приложение.\n\n" +
                    "Драйвер можно установить из папки приложения.";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                MessageBox(IntPtr.Zero, mes,"Ошибка: RS VISA не установлен", 0x00000010 | 0x00000000);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine(mes);
            }
        }
        protected static void ShowNet8ErrorMessage()
        {
            string mes = "Для работы приложения требуется .NET Runtime версии **8.0.20** (win-x64).\n\n" +
                "Установите .NET 8.0.20 из папки приложения или скачайте с сайта Microsoft.";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                MessageBox(IntPtr.Zero, mes, "Ошибка: .NET 8.0.20 не установлен", 0x00000010 | 0x00000000);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine(mes);
            }
            
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        //public static async Task InfoMessage(string message, string caption)
        //{
        //    WindowInfo Info = new WindowInfo(message, caption);
        //    Info.Show();
        //}
    }
}
