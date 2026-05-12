using APM_Crate.Models.RestApiModel;
using APM_Crate.Models.SettingsModel.Channels;
using APM_Crate.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APM_Crate.Models.SettingModel.Channel;

namespace APM_Crate.Models.SettingsModel
{
    public abstract class Setting
    {
        protected abstract string Name { get; set; }
        protected Channel Channel { get; set; }
        public static List<Settings> config { get; set; }
        public static ObservableCollection<string> TermoTypes { get; } =
            [
            "1 - 50М (ɑ = 0,00428 °Сˉ¹)",
            "2 - 100М (ɑ = 0,00428 °Сˉ¹)",
            "3 - 50М (ɑ = 0,00426 °Сˉ¹",
            "4 - 100М (ɑ = 0,00426 °Сˉ¹)",
            "5 - 50П (ɑ = 0,00391 °Сˉ¹)",
            "6 - 100П (ɑ = 0,00391 °Сˉ¹)",
            "7 - Pt50 (ɑ = 0,00385 °Сˉ¹)",
            "8 - Pt100 (ɑ = 0,00385 °Сˉ¹)"
            ];
        public static string TermoType = TermoTypes[3];

        public static bool IsResetting = false;
        public static bool Coef_Is_10 = true;
        public static bool Freq_Is_79_6 = true;
        public static bool CoefEnaibled = true;
        public static int ProgressValue = 0;
        public static string ProgressText = "";
        public static int ProgressValueChannel = 0;
        public static string ProgressTextChannel = "";

        public static double DC_Value { get { return Coef_Is_10 ? 10d : 12d; } }
        public static double Point_1 { get { return Coef_Is_10 ? 10d : 6.67; } }
        public static double Point_2 { get { return Coef_Is_10 ? 3000d : 100d; } }
        public static float Coef { get { return Coef_Is_10 ? 10f : 6.67f; } }
        public static double Frequency { get { return Freq_Is_79_6 ? 79.6d : 80d; } }
        public static ushort SerialNumber { get; set; }
        public static List<Settings> settings { get; set; }

        protected abstract Task Preparing();
        protected abstract Task CountCoefs();
        protected abstract Task WriteCoefs();
        protected abstract Task CheckSetting();
        protected virtual async Task LastMethod() { }
        public async Task Start(Channel channel)
        {
            Channel = channel;
            await LogerViewModel.Instance.Write($"Настройка {Name}, канал {Channel.Num}");
            await Preparing();
            await CountCoefs();
            await WriteCoefs();
            await CheckSetting();
            await LastMethod();
        }
        protected abstract Task CheckSettingFlag();
        protected abstract Task Reset();
        public void WriteList(string str, float A, float B)
        {
            config.Add(new Settings { Name = $"Канал {Channel.Num}.{str} Коэффициент А", Value = A.ToString() });
            config.Add(new Settings { Name = $"Канал {Channel.Num}.{str} Коэффициент B", Value = B.ToString() });
        }
    }
}
