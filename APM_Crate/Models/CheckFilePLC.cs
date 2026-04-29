using APM_Crate.Models.DevicesModel;
using APM_Crate.ViewModels;
using Avalonia.Remote.Protocol.Designer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static APM_Crate.Models.SettingModel;

namespace APM_Crate.Models
{
    public static class CheckFilePLC
    {
        public static int LengthChannel { get; set; } = 64 * 1024 - 4;
        public static int IndexMudule { get; set; } = 0;
        public static int LengthModul { get; set; } = (LengthChannel + 4) * 4;

        public static async Task Start()
        {
            Devices.Crate.WriteUInt16(Channel1.OnSaw,1);
            Devices.Crate.WriteUInt16(Channel2.OnSaw,1);
            Devices.Crate.WriteUInt16(3,5);
            Thread.Sleep(3000);
            Devices.Crate.WriteUInt16(4, 1);
            do
            {
                Thread.Sleep(500);
            }
            while (GetBigEndian());
            byte[] data = await DownloadFile();
            Devices.Crate.WriteUInt16(Channel1.OnSaw, 0);
            Devices.Crate.WriteUInt16(Channel2.OnSaw, 0);
            GetValuesChannel( data, out ushort[] channel1);
            GetValuesChannel( data, out ushort[] channel2);
            CheckSawChannel(channel1,out uint errors1);
            CheckSawChannel(channel2, out uint errors2);
            uint result = errors1 + errors2;
            if (result > 0) throw new Exception($"Ошибка! Сигнал формы пилы неправильный. Зафиксировано ошибок: {result}");
        }
        public static bool GetBigEndian()
        {
           ushort value =  Devices.Crate.ReadUInt16(6);
           string str = Convert.ToString(value,2);
            str = new string(str.Reverse().ToArray());
            if (str.Length < 16)
            {
                str += string.Concat(Enumerable.Repeat("0", 16 - str.Length));
            }
            return str[15] is '1';
        }
        public static async Task<byte[]> DownloadFile()
        {
            string url = $"http://{Devices.Crate.IpAddress}/fast.dat";
            //string path = "Log";
            using HttpClient client = new HttpClient();
            return await client.GetByteArrayAsync(url);
            //await File.WriteAllBytesAsync(path, filebytes);
        }
        public static void CheckSawChannel(ushort[] channel, out uint errors)
        {
            errors = 0;
            //int v1 = 15500;
            //int v2 = 500;

            for (int i = 1; i < channel.Length; i++)
            {
                int prev = channel[i - 1];
                int current = channel[i];
                if (current > 16384)
                {
                    errors++;
                    LogerViewModel.Instance.WriteDebug("Ошибка! Значение точки пилы превышает 16384.");
                    continue;
                }
                     
                // 1) обычный рост на +1
                if (current == prev + 1) continue;

                // 2) падение → возможно новый период
                if (current < prev)
                {
                    if (current < 800) continue;// новый период 
                    else
                    {
                        errors++;
                        LogerViewModel.Instance.WriteDebug( $"Ошибка! Неправильная форма пилы индекс={i}, предыдущее значение={prev},текущее значение={current}");
                    }
                }
            }
        }
        public static void GetValuesChannel(byte[] data, out ushort[] channel)
        {
            channel = new ushort[LengthChannel];
            for (int i = 2 *IndexMudule * LengthModul; i < LengthChannel; i++)
            {
                byte b1 = data[i * 2];
                byte b2 = data[i * 2 + 1];
                channel[i] = (ushort)(b1 << 8 | +b2);
            }
            //data = data[((LengthChannel + 4) * 2)..];
        }
    }
}
