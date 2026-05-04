using APM_Crate.Models.DevicesModel;
using APM_Crate.Models.RestApiModel;
using APM_Crate.Service;
using APM_Crate.ViewModels;
using PortsWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APM_Crate.Models
{
    public class SettingModel
    {

        public abstract class Channel
        {
            //public Stopwatch stopwatch { get; } = new Stopwatch();

            public abstract string Num { get; }
            public abstract ushort Coef_ACC_A { get; }
            public abstract ushort Coef_ACC_B { get; }

            public abstract ushort Coef_Speed_A { get; }
            public abstract ushort Coef_Speed_B { get; }
            public abstract ushort Coef_4_20_A { get; }
            public abstract ushort Coef_4_20_B { get; }
            public abstract ushort Coef_T_A { get; }
            public abstract ushort Coef_T_B { get; }
            public abstract ushort TypeTermo { get; }

            public abstract ushort PhysicalB0 { get; }
            public abstract ushort Physical { get; }
            //Значения прочтенные с этих регистров делить на 100 ↓
            public abstract ushort ACC_A { get; }
            public abstract ushort ACC_RMS { get; }
            public abstract ushort ACC_PP { get; }
            public abstract ushort Speed_A { get; }
            public abstract ushort Speed_RMS { get; }
            public abstract ushort Speed_PP { get; }
            //делить на 10↓
            public abstract ushort Move_A { get; }
            public abstract ushort Move_RMS { get; }
            public abstract ushort Move_PP { get; }
            public abstract ushort DC { get; }
            public abstract ushort T { get; }
            //↑
            public abstract ushort ResistTermo { get; }
            public abstract ushort OnChannel { get; }
            public abstract ushort OnSaw { get; } 

            public bool SettingChannel = true;
            public bool CanSetting = true;

            private async Task Reset(SettingType setting)
            {
                switch (setting)
                {
                    case SettingType.IEPE:
                        await Devices.Crate.WriteSwFloat(Coef_ACC_A, 1);
                        await Devices.Crate.WriteSwFloat(Coef_ACC_B, 0);
                        await Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
                        await Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
                        //await Devices.Crate.WriteSwFloat(Coef_4_20_A, 1);
                        //await Devices.Crate.WriteSwFloat(Coef_4_20_B, 0);
                        await Devices.Crate.WriteSwFloat(Coef_T_A, 1);
                        await Devices.Crate.WriteSwFloat(Coef_T_B, 0);
                        break;
                    case SettingType._4_20:
                        await Devices.Crate.WriteSwFloat(Coef_4_20_A, 1);
                        await Devices.Crate.WriteSwFloat(Coef_4_20_B, 0);
                        //await Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
                        //await Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
                        break;
                    case SettingType.T:
                        await Devices.Crate.WriteSwFloat(Coef_T_A, 1);
                        await Devices.Crate.WriteSwFloat(Coef_T_B, 0);
                        //await Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
                        //await Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
                        break;
                    case SettingType.U:
                        await Devices.Crate.WriteSwFloat(Coef_ACC_A, 1);
                        await Devices.Crate.WriteSwFloat(Coef_ACC_B, 0);
                        break;
                }

            }
            public enum SettingType
            {
                IEPE,
                _4_20,
                T,
                U
            }
            public enum ValueType
            {
                ACC,
                ACC_PP,
                Speed,
                Move
            }

            public async Task<bool> CheckSettingFlag(SettingType type)
            {
                if (SettingChannel is false)
                {
                    switch (type)
                    {
                        case SettingType.IEPE:  await LogerViewModel.Instance.Write($"Настройка IEPE канала {Num} пропущена т.к. канал не был выбран"); break;
                        case SettingType._4_20: await LogerViewModel.Instance.Write($"Настройка 4-20 канала {Num} пропущена т.к. канал не был выбран"); break;
                        case SettingType.T: await LogerViewModel.Instance.Write($"Настройка термопреобразователя канала {Num} пропущена т.к. канал не был выбран"); break;
                        case SettingType.U: await LogerViewModel.Instance.Write($"Настройка термопреобразователя канала {Num} пропущена т.к. канал не был выбран"); break;
                    }
                    return false;
                }
                else return true;
            }
            public async Task SawOn()
            {
                await Devices.Crate.WriteUInt16(OnSaw, 1);
            }
            public async Task SawOff()
            {
                await Devices.Crate.WriteUInt16(OnSaw, 0);
            }
            public async Task SourceOn()
            {
                await Devices.Crate.WriteUInt16(OnChannel, 1);
            }
            public async Task SourceOff()
            {
                await Devices.Crate.WriteUInt16(OnChannel,0);
            }
            public async Task<float> GetValue(ValueType type)
            {
                return (type) switch
                {
                    ValueType.ACC => await Devices.Crate.ReadUInt16(ACC_RMS) * 0.01f,
                    ValueType.ACC_PP => await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f,
                    ValueType.Speed => await Devices.Crate.ReadUInt16(Speed_RMS) * 0.01f,
                    ValueType.Move => await Devices.Crate.ReadUInt16(Move_RMS) * 0.1f
                };
            }
            public async Task CheckSetting(ushort reg,float V, ValueType type)
            {
                //float value = (type) switch
                //{
                //    ValueType.ACC => await Devices.Crate.ReadUInt16(reg) * 0.01f,
                //    ValueType.ACC_PP => await Devices.Crate.ReadUInt16(reg) * 0.01f,
                //    ValueType.Speed => await Devices.Crate.ReadUInt16(reg) * 0.01f,
                //    ValueType.Move=> await Devices.Crate.ReadUInt16(reg) * 0.1f
                //};
                float mkm = (type) switch
                {
                    ValueType.ACC =>1,
                    ValueType.Speed =>2,
                    ValueType.Move  =>4
                };
                float value = await GetValue(type);
                CountRelative(value * Coef / mkm, V,out float relative);
                relative = Math.Abs(relative);
                if (relative >= 1)
                {
                    string s = "";
                    switch (type)
                    {
                        case ValueType.ACC: s = "Ускорение"; break;
                        case ValueType.ACC_PP: s = "Ускорение. Размах"; break;
                        case ValueType.Speed: s = "Скорость"; break;
                        case ValueType.Move: s = "Перемещение"; break;
                    }
                    throw new Exception($"{s} канала {Num} настроено не корректно. Значение отклонено на {Math.Round(relative, 2)}%");
                }
            }
            public async Task WriteCoefs_ACC(float A,float B)
            {
                await LogerViewModel.Instance.Write($"Канал {Num}. Запись коэффициентов ускорения:");
                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                await Devices.Crate.WriteSwFloat(Coef_ACC_A, A);
                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                await Devices.Crate.WriteSwFloat(Coef_ACC_B, B);
                WriteList("Ускорение", A,B);
                //settings.Add(new Settings { Name = $"Канал {Num}.Ускорение Коэффициент А", Value = A.ToString() });
                //settings.Add(new Settings { Name = $"Канал {Num}.Ускорение Коэффициент B", Value = B.ToString() });
            }
            public async Task WriteCoefs_Speed(float A, float B)
            {
                await LogerViewModel.Instance.Write("Запись коэффициентов скорости:");
                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                await Devices.Crate.WriteSwFloat(Coef_Speed_A, A);
                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                await Devices.Crate.WriteSwFloat(Coef_Speed_B, B);
                WriteList("Скорость", A,B);
                //settings.Add(new Settings { Name = $"Канал {Num}.Скорость Коэффициент А", Value = A.ToString() });
                //settings.Add(new Settings { Name = $"Канал {Num}.Скорость Коэффициент B", Value = B.ToString() });
            }
            public async Task WriteCoefs_4_20(float A, float B)
            {
                await LogerViewModel.Instance.Write("Запись коэффициентов 4-20:");
                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                await Devices.Crate.WriteSwFloat(Coef_4_20_A, A);
                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                await Devices.Crate.WriteSwFloat(Coef_4_20_B, B);
                WriteList("Ток 4-20", A,B);
                //settings.Add(new Settings { Name = $"Канал {Num}.Ток 4-20 Коэффициент А", Value = A.ToString() });
                //settings.Add(new Settings { Name = $"Канал {Num}.Ток 4-20 Коэффициент B", Value = B.ToString() });
            }
            public async Task WriteCoefs_IEPE2(float A)
            {
                await LogerViewModel.Instance.Write("Запись коэффициентов постоянной составляющей сигнала:");
                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                await Devices.Crate.WriteSwFloat(Coef_T_A, A);
                await LogerViewModel.Instance.Write($"Коэффициент B: 0");
                await Devices.Crate.WriteSwFloat(Coef_T_B, 0);
            }
            public async Task WriteCoefs_T(float A, float B)
            {
                await LogerViewModel.Instance.Write("Запись коэффициентов Термопреобразователя:");
                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                await Devices.Crate.WriteSwFloat(Coef_T_A, A);
                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                await Devices.Crate.WriteSwFloat(Coef_T_B, B);
                WriteList("Температура",A,B);
                //settings.Add(new Settings { Name = $"Канал {Num}.Температура Коэффициент А", Value = A.ToString() });
                //settings.Add(new Settings { Name = $"Канал {Num}.Температура Коэффициент B", Value = B.ToString() });
            }
            public async Task WriteCoefs_U(float A, float B)
            {
                await LogerViewModel.Instance.Write($"Запись коэффициентов TIK_PLC 511.41 для {Num}-го канала:");
                await WriteCoefs_ACC(A, B);
                //await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                //await Devices.Crate.WriteSwFloat(Coef_ACC_A, A);
                //await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                //await Devices.Crate.WriteSwFloat(Coef_ACC_B, B);
            }
            public void WriteList(string str,float A, float B)
            {
                settings.Add(new Settings { Name = $"Канал {Num}.{str} Коэффициент А", Value = A.ToString() });
                settings.Add(new Settings { Name = $"Канал {Num}.{str} Коэффициент B", Value = B.ToString() });
            }
            public async Task Setting_IEPE()
            {
                if (await CheckSettingFlag(SettingType.IEPE) is false) return;
                //stopwatch.Restart();
                //string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                await LogerViewModel.Instance.Write($"Настройка IEPE, канала {Num}");
                await Dialog.ShowBuild("IEPE", $"Установите контакты для настройки IEPE {Num}-го канала");

                await SourceOn();
                await Reset(SettingType.IEPE);

                await ValidationVoltage(DC_Value, 0.5d);
                await Devices.Multimeter.VoltmeterMode("AC");
                await SetVoltage(Point_1);
                float V1 = await GetVoltageAC( );

                float Signal_1 = await GetValue(ValueType.ACC);
                float Integral_1 = await GetValue(ValueType.Speed);
                //float Signal_1 = await Devices.Crate.ReadUInt16(ACC_RMS) * 0.01f;
                //float Integral_1 = await Devices.Crate.ReadUInt16(Speed_RMS) * 0.01f;

                //ModbusTCP cc = new ModbusTCP();
                //cc.Connect("10.21.12.67");
                //ushort reg = cc.ReadUInt16(8022);
                //ushort reg1 = cc.ReadUInt16(8023);
                //AverageValue(ACC_RMS, out float Signal_1);
                //AverageValue(Speed_RMS, out float Integral_1);

                await SetVoltage(Point_2);
                float V2 = await GetVoltageAC( );
                await WaitForChangeRegisters(ACC_RMS, Speed_RMS,Signal_1, Integral_1,0.01f);

                float Signal_2 = await GetValue(ValueType.ACC);
                float Integral_2 = await GetValue(ValueType.Speed);
                //float Signal_2 = await Devices.Crate.ReadUInt16(ACC_RMS) * 0.01f;
                //float Integral_2 = await Devices.Crate.ReadUInt16(Speed_RMS) * 0.01f;
                //AverageValue(ACC_RMS, out float Signal_2);
                //AverageValue(Speed_RMS, out float Integral_2);

                float A1 = (V2 - V1) / Coef / (Signal_2 - Signal_1);
                float B1 = V1 / Coef - A1 * Signal_1;
                float A2 = (V2 - V1) * 2 / Coef / (Integral_2 - Integral_1);
                float B2 = V1 * 2 / Coef - A2 * Integral_1;
                await WriteCoefs_ACC(A1, B1);
                await WriteCoefs_Speed(A2, B2);


                float dc = await Devices.Crate.ReadSwFloat(PhysicalB0);
                await Devices.Generator.ChanelOff(Devices.Generator.channelNum);
                float real_dc = (float)(await Devices.Multimeter.GetVoltage("DC", 1000));
                await Devices.Generator.ChanelOn(Devices.Generator.channelNum);
                await WriteCoefs_IEPE2(real_dc / dc);
                await SetVoltage(Point_1);
                V1 = await GetVoltageAC();

                Signal_1 = await GetValue(ValueType.ACC);
                Integral_1 = await GetValue(ValueType.Speed);

                await CheckSetting(ACC_RMS, V1,ValueType.ACC);
                //AverageValue(ACC_RMS, out float ACC);
                //CountRelative(ACC * Coef, V1, out float relative);
                //CheckSetting(relative, SettingType.IEPE);
                

                await SetVoltage(Point_2);
                V2 = await GetVoltageAC();

                await WaitForChangeRegisters(ACC_RMS, Speed_RMS,Signal_1, Integral_1,0.01f);

                float Move = await Devices.Crate.ReadUInt16(Move_RMS) * 0.1f;
                CountRelative(Move * Coef / 4, V2, out float relative);
                // перенастройка перемещения.костыль
                if (relative <= -1)
                {
                    await LogerViewModel.Instance.Write($"Переписывание коэффициента скорости");
                    A2 += 0.04f;
                    await WriteCoefs_Speed( A2, B2);
                }
                else if (relative >= 1)
                {
                    await LogerViewModel.Instance.Write($"Переписывание коэффициента скорости");
                    A2 -= 0.02f;
                    await WriteCoefs_Speed( A2, B2);
                }
                await CheckSetting(ACC_RMS, V2, ValueType.ACC);
                await CheckSetting(Speed_RMS, V2, ValueType.Speed);
                await CheckSetting(Move_RMS, V2, ValueType.Move);

                //AverageValue(ACC_RMS, out float ACC_2);
                //CountRelative(ACC_2 * Coef, V2, out relative);
                //CheckSetting(relative, SettingType.ACC);

                //AverageValue(Speed_RMS, out float Speed);
                //CountRelative(Speed * Coef/2, V2, out relative);
                //CheckSetting(relative, SettingType.Speed);

                //AverageValue(Speed_RMS, out float Move_2);
                //CountRelative(Move_2 * Coef / 4, V2, out relative);
                //CheckSetting(relative, SettingType.Move);
                //stopwatch.Stop();
                //string setting = IsResetting ? "Перенастройка IEPE" : "IEPE";
                //string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                //await WriteParam(setting, starttime, endtime);
                
                await LogerViewModel.Instance.Write($"✔ Настройка IEPE, канала {Num} закончена");
            }
            public async Task Setting_4_20()
            {
                if (await CheckSettingFlag(SettingType._4_20) is false) return;
                //string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                //stopwatch.Restart();
                await LogerViewModel.Instance.Write($"Настройка тока 4-20, канала {Num}");
                await Dialog.ShowBuild("4_20", $"Установите контакты для настройки тока 4-20 {Num}-го канала");
                await Reset(SettingType._4_20);
                await SourceOn();
                await Validation_mA(4,0.02);
                float I1 = await Devices.Crate.ReadSwFloat(DC);
                await Validation_mA(20, 0.02);
                float I2 = await Devices.Crate.ReadSwFloat(DC);
                float A = 16 / (I2 - I1);
                float B = 4 - A * I1;
                await WriteCoefs_4_20(A, B);


                float I = await Devices.Crate.ReadSwFloat(DC);
                double mA = await Devices.Multimeter.GetAmperage();
                if ((I - mA) / mA * 100 > 1) throw new Exception("Канал тока 4-20 настроился не правильно");
                //stopwatch.Stop();
                //string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");

                //string setting = IsResetting ? "Перенастройка 4-20" : "4-20" ;

                //await WriteParam(setting, starttime, endtime);
                await LogerViewModel.Instance.Write($"✔ Настройка тока 4-20, канала {Num} закончена");
            }
            public async Task Setting_T()
            {
                if (await CheckSettingFlag(SettingType.T) is false) return;
                //stopwatch.Restart();
                //string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                await LogerViewModel.Instance.Write($"Настройка термопреобразователя, канала {Num}");
                await Dialog.ShowBuild("T", $"Установите контакты для настройки термопреобразователя {Num}-го канала");
                await Reset(SettingType.T);
                await Dialog.ShowParam();
                float T1 = 0, T2 = 0;
                float Readed_T = 0f;
                ushort typetermo = (ushort)(TermoTypes.IndexOf(TermoType) + 1);
                SetTermoValues(out float R1, out float R2, out float Need_T1, out float Need_T2);
                await Devices.Crate.WriteUInt16(TypeTermo, typetermo);
                await Dialog.ShowConfirm($"Установите на магазине сопротивлений 100 Ом", new Delay());
                await Task.Delay(15000); // стоит потому что долго обновляется значение в крейте
                T1 = await Devices.Crate.ReadUInt16(ResistTermo) / 1000f;
                await Dialog.ShowConfirm($"Установите на магазине сопротивлений 400 Ом", new Delay());
                await Task.Delay(25000); // стоит потому что долго обновляется значение в крейте
                T2 = await Devices.Crate.ReadUInt16(ResistTermo) / 1000f;
                float A = 3 / (T2 - T1);
                float B = 1 - A * T1;
                await WriteCoefs_T(A,B);

                await Dialog.ShowConfirm($"Установите на магазине сопротивлений {R1} Ом", new Delay());
                await Task.Delay(15000);// стоит потому что долго обновляется значение в крейте
                Readed_T = await Devices.Crate.ReadUInt16(T) * 0.1f;
                float relative = Math.Abs(Readed_T - Need_T1);
                if (relative > 1) throw new Exception($"Точка 1 не прошла проверку, значение отклонено от нормы на {relative}");

                await Dialog.ShowConfirm($"Установите на магазине сопротивлений {R2} Ом", new Delay());
                await Task.Delay(15000);// стоит потому что долго обновляется значение в крейте
                Readed_T = await Devices.Crate.ReadUInt16(T) * 0.1f;
                relative = Math.Abs(Readed_T - Need_T2);
                if (relative > 1) throw new Exception($"Точка 2 не прошла проверку, значение отклонено от нормы на {relative}");
                //stopwatch.Stop();
                //string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");

                //string setting = IsResetting ? "Перенастройка T": "T";
                //await WriteParam(setting, starttime,endtime);
                await LogerViewModel.Instance.Write($"✔ Настройка термопреобразователя, канала {Num} закончена");
            }
            public async Task Setting_U()
            {
                if (await CheckSettingFlag(SettingType.U) is false) return;
                //string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
                await LogerViewModel.Instance.Write($"Настройка TIK-PLC.511, канала {Num}");
                await Dialog.ShowBuild("U", $"Установите контакты для настройки TIK-PLC 511.41 {Num}-го канала");
                await Reset(SettingType.U);
                await ValidationVoltage(10,0.02,Physical);
                //await Dialog.ShowConfirm("Установите на калибраторе смещение 10В");
                await Devices.Generator.SetFrequency(79.6);
                await Devices.Generator.SetOffset(0);
                await Devices.Multimeter.VoltmeterMode("AC");
                await SetVoltage(0.5d);

                float Signal_1 = await GetValue(ValueType.ACC_PP);
                //float Signal_1 = await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f;
                await SetVoltage(20);
                float V1 = await GetVoltageAC();
                await WaitForChangeRegisters(ACC_PP, Signal_1, 0.01f);
                float Signal_2 = await GetValue(ValueType.ACC_PP);
                //float Signal_2 = await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f;
                float V2 = await GetVoltageAC();

                float A1 = (V2 - V1) / Coef / (Signal_2 - Signal_1);
                float B1 = V1 / Coef - A1 * Signal_1;
                await WriteCoefs_U(A1, B1);

                await SetVoltage(0.5d);
                V1 = await GetVoltageAC();
                await CheckSetting(ACC_PP,V1,ValueType.ACC);

                await SetVoltage(20d);
                V2 = await GetVoltageAC();
                await CheckSetting(ACC_PP, V2, ValueType.ACC);
                //string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");

                //string setting = IsResetting ? "Перенастройка U":"U";

                //await WriteParam(setting, starttime, endtime);
                await LogerViewModel.Instance.Write($"Настройка TIK-PLC.511, канала {Num} закончена");
            }
            //public async Task WriteParam(string settings, string starttime,string endtime)
            //{
            //    var setting = new ParametersSettingPLC
            //    (
            //        Environment.UserName,
            //        String.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year),
            //        Num,
            //        settings,
            //        starttime,
            //        endtime,
            //        ItemPLC,
            //        $"{stopwatch.Elapsed:mm\\:ss}",
            //        SerialNumber,
            //        MainWindowViewModel.SettingViewModel.OrderNumber,

            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_ACC_A).ToString(),
            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_ACC_B).ToString(),
            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_Speed_A).ToString(),
            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_Speed_B).ToString(),
            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_4_20_A).ToString(),
            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_4_20_B).ToString(),
            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_T_A).ToString(),
            //        await Devices.Crate.ReadSwFloat(SettingModel.Channel1.Coef_T_B).ToString(),
            //        await Devices.Crate.ReadUInt16(SettingModel.Channel1.TypeTermo).ToString()
            //    );
            //    await SQLModel.WriteNewParameters(setting);
            //}
        }
        public class Channel_1 : Channel
        {
            public override string Num => "1";
            public override ushort Coef_ACC_A => (ushort)(60000 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B => (ushort)(60002 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A => (ushort)(60004 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B => (ushort)(60006 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A => (ushort)(60008 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B => (ushort)(60010 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A => (ushort)(60012 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_B => (ushort)(60014 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo => (ushort)(60016 + 90 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 => (ushort) (8018 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical => (ushort) (8020 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A => (ushort)(8021 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS => (ushort)(8022 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP => (ushort)(8023 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_A => (ushort)(8024 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS => (ushort)(8025 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP => (ushort)(8026 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A => (ushort)(8027 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS => (ushort)(8028 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP => (ushort)(8029 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort DC => (ushort)(8030 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T => (ushort)(8032 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ResistTermo => (ushort)(8036 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort OnChannel => (ushort)(11056 + 600 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnSaw => (ushort)(11110 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }
        public class Channel_2 : Channel
        {
            public override string Num => "2";
            public override ushort Coef_ACC_A => (ushort)(60030 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B => (ushort)(60032 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A => (ushort)(60034 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B => (ushort)(60036 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A => (ushort)(60038 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B => (ushort)(60040 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A => (ushort)(60042 + 90 * (Convert.ToInt16(ItemModule) - 1)); 
            public override ushort Coef_T_B => (ushort)(60044 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo => (ushort)(60046 + 90 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 => (ushort) (8043 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical => (ushort) (8045 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A => (ushort)(8046 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS => (ushort)(8047 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP => (ushort)(8048 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_A => (ushort)(8049 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS => (ushort)(8050 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP => (ushort)(8051 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A => (ushort)(8052 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS => (ushort)(8053 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP => (ushort)(8054 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort DC => (ushort)(8055 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T => (ushort)(8057 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ResistTermo => (ushort)(8061 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort OnChannel => (ushort)(11256 + 600 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnSaw => (ushort)(11310 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }
        public class Channel_3 : Channel
        {
            public override string Num => "3";
            public override ushort Coef_ACC_A => (ushort)(60060 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B => (ushort)(60062 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A => (ushort)(60064 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B => (ushort)(60066 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A => (ushort)(60068 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B => (ushort)(60070 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A => (ushort)(60072 + 90 * (Convert.ToInt16(ItemModule) - 1)); 
            public override ushort Coef_T_B => (ushort)(60074 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo => (ushort)(60076 + 90 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 => (ushort) (8068 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical => (ushort) (8070 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A => (ushort)(8071 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS => (ushort)(8072 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP => (ushort)(8073 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_A => (ushort)(8074 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS => (ushort)(8075 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP => (ushort)(8076 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A => (ushort)(8077 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS => (ushort)(8078 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP => (ushort)(8079 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort DC => (ushort)(8080 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T => (ushort)(8082 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort ResistTermo => (ushort)(8086 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort OnChannel => (ushort)(11456 + 600 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnSaw => (ushort)(11510 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }
        public class Channel_4 : Channel
        {
            public override string Num => "4";
            public override ushort Coef_ACC_A => (ushort)(61260 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B => (ushort)(61262 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A => (ushort)(61264 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B => (ushort)(61266 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A => (ushort)(61268 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B => (ushort)(61270 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A => (ushort)(61272 + 30 * (Convert.ToInt16(ItemModule) - 1)); 
            public override ushort Coef_T_B => (ushort)(61274 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo => (ushort)(61276 + 30 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 => (ushort) (9406 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical => (ushort) (9408 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A => (ushort)(9409 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS => (ushort)(9410 + 25 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP => (ushort)(9411 + 100 * (Convert.ToInt16(ItemModule) - 1));
            #region Резерв
            public override ushort Speed_A => (ushort)(9412 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS => (ushort)(9413 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP => (ushort)(9414 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A => (ushort)(9415 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS => (ushort)(9416 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP => (ushort)(9417 + 100 * (Convert.ToInt16(ItemModule) - 1));
            #endregion
            public override ushort DC => (ushort)(9418 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T => (ushort)(9420 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ResistTermo => (ushort)(9424 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnChannel => 0;
            public override ushort OnSaw => (ushort)(19510 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }

        public static Channel Channel1 { get; } = new Channel_1();
        public static Channel Channel2 { get; } = new Channel_2();
        public static Channel Channel3 { get; } = new Channel_3();
        public static Channel Channel4 { get; } = new Channel_4();

        public static ObservableCollection<string> Moduls => ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"];
        public static string ItemModule = "1";
        public static ObservableCollection<string> PLC => ["241", "242", "243","511", "371", "374", "375"];
        public static string ItemPLC = "241";

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
        public static bool CoefEnaibled = true;

        public static double DC_Value {get{return Coef_Is_10 ? 10d : 12d;}}
        public static double Point_1 {get{return Coef_Is_10 ? 10d : 6.67;}}
        public static double Point_2 {get{return Coef_Is_10 ? 3000d : 100d;} }
        public static float Coef {get{return Coef_Is_10 ? 10f : 6.67f;} }
        public static double Frequency { get; } = 80d;
        public static ushort SerialNumber { get; set; }
        public static List<Settings> settings { get; set; }

        public static async Task Samples(ushort type)
        {
            byte[] data = await CheckFilePLC.GetDataSaw(type);   
            await Dialog.ShowLiveCharts(data,type);

            //CheckFilePLC.GetValuesChannel(data, out ushort[] channel1);
            //CheckFilePLC.GetValuesChannel(data, out ushort[] channel2);
            //CheckFilePLC.CheckSawChannel(channel1, out uint errors1);
            //CheckFilePLC.CheckSawChannel(channel2, out uint errors2);
            //await LogerViewModel.Instance.Write($"{errors1+errors2}");
        }
        public static async Task Start(string item)
        {
            await Devices.Crate.SetPassword();
            await Devices.Generator.SetFrequency(Frequency);
            ushort type = 0;
            switch (item)
            {
                case "241":
                    type = 1;
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_4_20();
                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
                case "242":
                    type = 2;
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_IEPE();
                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
                case "243":
                    type = 3;
                    await Channel1.Setting_4_20();
                    await Channel2.Setting_4_20();
                    break;
                case "511":
                    type = 4;
                    await Channel1.Setting_U();
                    await Channel2.Setting_U();
                    await Channel3.Setting_U();
                    await Channel4.Setting_U();
                    break;
                case "371":
                    type = 5;
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_4_20();
                    await Channel3.Setting_T();
                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
                case "374":
                    type = 6;
                    await Channel1.Setting_4_20();
                    await Channel2.Setting_4_20();
                    await Channel3.Setting_4_20();
                    break;
                case "375":
                    type = 7;
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_4_20();
                    await Channel3.Setting_4_20();
                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
            }
            if (!(type is 3 || type is 4 || type is 6))
            {
                await CheckFilePLC.Start(type);
                await LogerViewModel.Instance.Write("✔ Выборки соответствуют правильной форме");
            }
        }
        public static async Task Validation_mA(double V, double relative)
        {
            bool Valid = false;
            string mes = $"Установите щупы мультиметра в режим измерения постоянного тока и при помощи магазина сопротивлений задайте {V} мА";
            do
            {
                IGetVoltege mult = new A();
                await Devices.Multimeter.AmmeterMode("DC");
                await Dialog.ShowConfirm(mes, mult);
                double value = await Devices.Multimeter.GetAmperage();
                Valid = Math.Abs(value - V) < relative ? true : false;
                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} мА.";
            }
            while (Valid is false);
        }
        public static async Task ValidationVoltage(double V,double relative, string type = "DC")
        {
            bool Valid = false;
            string mes = $"При помощи магазина сопротивлений задайте {V} В";
           if(Devices.Generator.IsOpened()) await Devices.Generator.ChanelOff(Devices.Generator.channelNum);
            do
            {
                IGetVoltege mult = type is "DC" ? new DC() : new AC();
                await Dialog.ShowConfirm(mes, mult);
                double value = await Devices.Multimeter.GetVoltage(type, 500);
                Valid = Math.Abs(value - V) < relative ? true : false;
                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} В.";
            }
            while (Valid is false);
            if (Devices.Generator.IsOpened()) await Devices.Generator.ChanelOn(Devices.Generator.channelNum);
        }
        public static async Task ValidationVoltage(double V, double relative,ushort reg)
        {
            bool Valid = false;
            string mes = $"При помощи калибратора задайте смещение {V} В";
            do
            {
                await Dialog.ShowConfirm(mes, new Delay());
                double value = await Devices.Crate.ReadUInt16(reg);
                Valid = Math.Abs(value - V) < relative ? true : false;
                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} В.";
            }
            while (Valid is false);
        }
        private static void SetTermoValues(out float Resist_1,out float Resist_2,out float Need_T_1,out float Need_T_2)
        {
            //Resist_1 = TermoTypes.IndexOf(TermoType) switch
            //{
            //    0 => 10.265f,
            //    1 => 20.53f,
            //    2 => 39.35f,
            //    3 => 78.7f,
            //    4 => 8.62f,
            //    5 => 17.24f,
            //    6 => 9.26f,
            //    7 => 18.52f
            //};
            //Resist_2 = TermoTypes.IndexOf(TermoType) switch
            //{
            //    0 => 92.8f,
            //    1 => 185.6f,
            //    2 => 92.6f,
            //    3 => 185.2f,
            //    4 => 197.58f,
            //    5 => 395.16f,
            //    6 => 195.24f,
            //    7 => 390.48f
            //};
            //Need_T_1 = TermoTypes.IndexOf(TermoType) switch
            //{
            //    0 => -180,
            //    1 => -180,
            //    2 => -50,
            //    3 => -50,
            //    4 => -200,
            //    5 => -200,
            //    6 => -200,
            //    7 => -200
            //};
            //Need_T_2 = TermoTypes.IndexOf(TermoType) switch
            //{
            //    0 => 200,
            //    1 => 200,
            //    2 => 200,
            //    3 => 200,
            //    4 => 850,
            //    5 => 850,
            //    6 => 850,
            //    7 => 850
            //};
            Resist_1 = 78.7f;
            Resist_2 = 185.2f;
            Need_T_1 = -50;
            Need_T_2 = 200;
            // Взято с https://cdn.termexlab.ru/files/9e48a99f/9525/4985/9662/23c12e3a32c2.pdf
            switch (TermoTypes.IndexOf(TermoType))
            {
                case 0:
                    Resist_1 = 10.265f;
                    Resist_2 = 92.8f;
                    Need_T_1 = -180;
                    Need_T_2 = 200;
                    break;
                case 1:
                    Resist_1 = 20.53f;
                    Resist_2 = 185.6f;
                    Need_T_1 = -180;
                    Need_T_2 = 200;
                    break;
                case 2:
                    Resist_1 = 39.35f;
                    Resist_2 = 92.6f;
                    Need_T_1 = -50;
                    Need_T_2 = 200;
                    break;
                case 3:
                    Resist_1 = 78.7f;
                    Resist_2 = 185.2f;
                    Need_T_1 = -50;
                    Need_T_2 = 200;
                    break;
                case 4:
                    Resist_1 = 8.62f;
                    Resist_2 = 197.58f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 5:
                    Resist_1 = 17.24f;
                    Resist_2 = 395.16f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 6:
                    Resist_1 = 9.26f;
                    Resist_2 = 195.24f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 7:
                    Resist_1 = 18.52f;
                    Resist_2 = 390.48f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;

            }
        }

        private static async Task<float> AverageValue(ushort Reg_adr)
        {
            float Value = 0;
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(400);
                Value += await Devices.Crate.ReadUInt16(Reg_adr) * 0.01f;
            }
            Value /= 10;
            return Value;
        }
        public static async Task SetVoltage(double V)
        {
            //Devices.Generator.SetVoltage(ConvertValue.ToPP(V));
            await Devices.Multimeter.SetVoltage(Devices.Generator, V, Frequency, 0.0005, 3);
            await Task.Delay(5000);
        }
        public static async Task WaitForChangeRegisters(ushort reg, float FirstsValue, float coefreg)
        {
            int sec = 0;
            bool IsChange = false;
            while (IsChange is false && sec < 60)
            {
                IsChange = await Devices.Crate.ReadUInt16(reg) * coefreg == FirstsValue ? false : true;
                await Task.Delay(1000);
                sec++;
            }
            if (IsChange is false) { throw new Exception("Значения обновляются слишком долго."); }
        }
        public static async Task WaitForChangeRegisters(ushort reg1, ushort reg2, float FirstsValue1,float FirstsValue2,float coefreg)
        {
            int sec = 0;
            bool IsChange1 = false;
            bool IsChange2 = false;
            while ((IsChange1 is false || IsChange2 is false) && sec <60)
            {
                IsChange1 = await Devices.Crate.ReadUInt16(reg1) * coefreg == FirstsValue1 ? false :true;
                IsChange2 = await Devices.Crate.ReadUInt16(reg2) * coefreg == FirstsValue2 ? false :true;
                await Task.Delay(1000);
                sec++;
            }
            if(sec<60) await Task.Delay(4000);

            if (IsChange1 is false || IsChange2 is false) { throw new Exception("Значения обновляются слишком долго."); }
        }
        public static async Task<float> GetVoltageAC() => (float)(await Devices.Multimeter.GetVoltage("AC", 1000) * 1000);
        public static void CountRelative(float value, float V, out float relative) => relative = V >= 1000 ? (value - V) / V * 100 : (value - V) / 1000 * 100; // (относительная/приведенная) погрешность
    }
}
