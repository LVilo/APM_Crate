using APM_Crate.Models.DevicesModel;
using APM_Crate.Models.RestApiModel;
using APM_Crate.Models.SettingsModel.Channels;
using APM_Crate.Service;
using APM_Crate.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel
{
    public abstract class Setting
    {
        protected abstract string Name { get; set; }
        protected Channel Channel { get; set; }
        protected WeightedProgress WP {  get; set; }
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
        public static List<Settings> settings { get; set; }

        protected abstract Task Preparing();
        protected abstract Task CountCoefs();
        protected abstract Task WriteCoefsAbstract();
        protected abstract Task CheckSettingAbstract();
        public async Task Start(Channel channel, WeightedProgress wp)
        {
            Channel = channel;
            WP = wp;
            if (CheckSettingFlag() is false)
            {
                await WriteListValuesSetting();
                wp.Reset();
                return;
            }
            LogerViewModel.Instance.Write($"Настройка {Name}, канал {Channel.Num}");
            await Preparing();
            await WP.Step(5,"Сброс коэффициентов", ResetCoefs);
            await WP.Step(20,"Расчет коэффициентов",CountCoefs);
            await WriteCoefsAbstract();
            await CheckSettingAbstract();
            await WriteListValuesSetting();
            wp.Reset();
        }
        private bool CheckSettingFlag() => Channel.SettingChannel;


        protected abstract Task ResetCoefs();
        protected void WriteList(string str, float A, float B)
        {
            settings.Add(new Settings { Name = $"Канал {Channel.Num}.{str} Коэффициент А", Value = A.ToString() });
            settings.Add(new Settings { Name = $"Канал {Channel.Num}.{str} Коэффициент B", Value = B.ToString() });
        }
        protected abstract Task WriteListValuesSetting();










        protected async Task SourceOn()
        {
            await Devices.Crate.WriteUInt16(Channel.OnChannel, 1);
        }
        protected enum ValueType
        {
            DC,

            ACC_A,
            ACC_RMS,
            ACC_PP,

            Speed_A,
            Speed_RMS,
            Speed_PP,

            Move_A,
            Move_RMS,
            Move_PP,

            DC_Value,
            T,
            ResistT,
        }
        protected enum CoefType
        {
            ACC,
            Speed,
            Current,
            T,
        }
        protected async Task WriteCoefs(float A, float B, CoefType type)
        {
            string str = "";
            ushort reg_A = 0;
            ushort reg_B = 0;
            switch (type)
            {
                case CoefType.ACC: reg_A = Channel.Coef_ACC_A; reg_B = Channel.Coef_ACC_B; str = "Ускорения"; break;
                case CoefType.Speed: reg_A = Channel.Coef_Speed_A; reg_B = Channel.Coef_Speed_B; str = "Скорости"; break;
                case CoefType.Current: reg_A = Channel.Coef_4_20_A; reg_B = Channel.Coef_4_20_B; str = "Тока 4-20"; break;
                case CoefType.T: reg_A = Channel.Coef_T_A; reg_B = Channel.Coef_T_B; str = "Температуры"; break;
            }
            await LogerViewModel.Instance.Write($"Запись коэффициентов {str}. A-({A}),B-({B})");
            WP.Report(0, $"Запись коэффициентов {str}. A-({A}),B-({B})");
            //await LogerViewModel.Instance.Write($"Канал {Num}. Запись коэффициентов 4-20:");
            //await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
            await Devices.Crate.WriteSwFloat(reg_A, A);
            //await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
            await Devices.Crate.WriteSwFloat(reg_B, B);
            //WriteList("Ток 4-20", A, B);
            WP.Report(2, $"Запись коэффициентов {str}. A-({A}),B-({B}) ✔");
        }
        protected async Task<float> GetValue(ValueType type)
        {
            return (type) switch
            {
                ValueType.DC => await Devices.Crate.ReadUInt16(Channel.Physical) * 0.001f,

                ValueType.ACC_A => await Devices.Crate.ReadUInt16(Channel.ACC_A) * 0.01f,
                ValueType.ACC_RMS => await Devices.Crate.ReadUInt16(Channel.ACC_RMS) * 0.01f,
                ValueType.ACC_PP => await Devices.Crate.ReadUInt16(Channel.ACC_PP) * 0.01f,

                ValueType.Speed_A => await Devices.Crate.ReadUInt16(Channel.Speed_A) * 0.01f,
                ValueType.Speed_RMS => await Devices.Crate.ReadUInt16(Channel.Speed_RMS) * 0.01f,
                ValueType.Speed_PP => await Devices.Crate.ReadUInt16(Channel.Speed_PP) * 0.01f,

                ValueType.Move_A => await Devices.Crate.ReadUInt16(Channel.Move_A) * 0.1f,
                ValueType.Move_RMS => await Devices.Crate.ReadUInt16(Channel.Move_RMS) * 0.1f,
                ValueType.Move_PP => await Devices.Crate.ReadUInt16(Channel.Move_PP) * 0.1f,

                ValueType.DC_Value => await Devices.Crate.ReadSwFloat(Channel.DC),
                ValueType.T => await Devices.Crate.ReadInt16(Channel.T) * 0.1f,
                ValueType.ResistT => await Devices.Crate.ReadUInt16(Channel.ResistTermo) * 0.001f,
            };
        }
    }
}
