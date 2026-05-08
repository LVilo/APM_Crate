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

        public static async Task Start(ushort type)
        {
            byte[] data = await GetDataSaw(type);
            uint errors1 = 0;
            uint errors2 = 0;
            data = data.Skip(2 * IndexMudule * LengthModul).ToArray();
            if (type is 2 || type is 5 || type is 7)
            {
                GetValuesChannel(data, out ushort[] channel1);
                CheckSawChannel(channel1,out errors1);
            }
            if (type is 1 || type is 2)
            {
                data = data.Skip(2 * LengthChannel).ToArray();
                GetValuesChannel(data, out ushort[] channel2);
                CheckSawChannel(channel2, out errors2);
            }

            
            uint result = errors1 + errors2;
            if (result > 0) throw new Exception($"Ошибка! Сигнал формы пилы неправильный. Зафиксировано ошибок: {result}");
        }
        public static async Task<byte[]> GetDataSaw(ushort type)
        {
            IndexMudule = Crate.IndexSlotByBasket;

            if (type is 2 || type is 5 || type is 7) await Channel1.SawOn();
            if (type is 1 || type is 2) await Channel2.SawOn();

            await Devices.Crate.WriteSingleRegister(3, 5);
            await Task.Delay(3000);
            await Devices.Crate.WriteSingleRegister(4, 1);
            do
            {
                await Task.Delay(500);
            }
            while (await GetBigEndian() is false);
            await Task.Delay(3000);
            byte[] data = await DownloadFile();
            if (type is 2 || type is 5 || type is 7) await Channel1.SawOff();
            if (type is 1 || type is 2) await Channel2.SawOff();
            return data;
        }


        public static async  Task<bool> GetBigEndian()
        {
           ushort value = await Devices.Crate.ReadUInt16(6);
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
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"http://{Devices.Crate.IpAddress}/");
            return await client.GetByteArrayAsync("fast.dat");
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
                    }
                }
                else errors++;
            }
        }
        public static void GetValuesChannel(byte[] data, out ushort[] channel)
        {
            channel = new ushort[LengthChannel];
            
            for (int i = 0; i < LengthChannel; i++)
            {
                byte b1 = data[i * 2];
                byte b2 = data[i * 2 + 1];
                channel[i] = (ushort)(b1 << 8 | +b2);
            }
            //data = data.Skip((LengthChannel + 4) * 2).ToArray();
            //data = data[((LengthChannel + 4) * 2)..];
        }
    }
}
