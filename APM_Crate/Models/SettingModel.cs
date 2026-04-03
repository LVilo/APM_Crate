using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using APM_Crate.ViewModels;
using PortsWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            private void Reset(SettingType setting)
            {
                switch (setting)
                {
                    case SettingType.IEPE:
                        Devices.Crate.WriteSwFloat(Coef_ACC_A, 1);
                        Devices.Crate.WriteSwFloat(Coef_ACC_B, 0);
                        Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
                        Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
                        //Devices.Crate.WriteSwFloat(Coef_4_20_A, 1);
                        //Devices.Crate.WriteSwFloat(Coef_4_20_B, 0);
                        //Devices.Crate.WriteSwFloat(Coef_T_A, 1);
                        //Devices.Crate.WriteSwFloat(Coef_T_B, 0);
                        break;
                    case SettingType._4_20:
                        Devices.Crate.WriteSwFloat(Coef_4_20_A, 1);
                        Devices.Crate.WriteSwFloat(Coef_4_20_B, 0);
                        //Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
                        //Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
                        break;
                    case SettingType.T:
                        Devices.Crate.WriteSwFloat(Coef_T_A, 1);
                        Devices.Crate.WriteSwFloat(Coef_T_B, 0);
                        //Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
                        //Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
                        break;
                    case SettingType.U:
                        Devices.Crate.WriteSwFloat(Coef_ACC_A, 1);
                        Devices.Crate.WriteSwFloat(Coef_ACC_B, 0);
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

            public bool CheckSettingFlag(SettingType type)
            {
                if (SettingChannel is false)
                {
                    switch (type)
                    {
                        case SettingType.IEPE: LogerViewModel.Instance.Write($"Настройка IEPE канала {Num} пропущена т.к. канал не был выбран"); break;
                        case SettingType._4_20: LogerViewModel.Instance.Write($"Настройка 4-20 канала {Num} пропущена т.к. канал не был выбран"); break;
                        case SettingType.T: LogerViewModel.Instance.Write($"Настройка термопреобразователя канала {Num} пропущена т.к. канал не был выбран"); break;
                        case SettingType.U: LogerViewModel.Instance.Write($"Настройка термопреобразователя канала {Num} пропущена т.к. канал не был выбран"); break;
                    }
                    return false;
                }
                else return true;
            }
            public void CheckSetting(ushort reg,float V, ValueType type)
            {
                float value = (type) switch
                {
                    ValueType.ACC => Devices.Crate.ReadUInt16(reg) / 100f,
                    ValueType.ACC_PP => Devices.Crate.ReadUInt16(reg) / 100f,
                    ValueType.Speed => Devices.Crate.ReadUInt16(reg) / 100f,
                    ValueType.Move=> Devices.Crate.ReadUInt16(reg) / 10f
                };
                CountRelative(value, V,out float relative);
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
                    throw new Exception($"❗ {s} канала {Num} настроено не корректно. Значение отклонено на {Math.Round(relative, 2)}%");
                }
            }
            public void WriteCoefs_ACC(float A,float B)
            {
                LogerViewModel.Instance.Write("Запись коэффициентов ускорения:");
                LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                Devices.Crate.WriteSwFloat(Coef_ACC_A, A);
                LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                Devices.Crate.WriteSwFloat(Coef_ACC_B, B);
            }
            public void WriteCoefs_Speed(float A, float B)
            {
                LogerViewModel.Instance.Write("Запись коэффициентов скорости:");
                LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                Devices.Crate.WriteSwFloat(Coef_Speed_A, A);
                LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                Devices.Crate.WriteSwFloat(Coef_Speed_B, B);
            }
            public void WriteCoefs_4_20(float A, float B)
            {
                LogerViewModel.Instance.Write("Запись коэффициентов 4-20:");
                LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                Devices.Crate.WriteSwFloat(Coef_4_20_A, A);
                LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                Devices.Crate.WriteSwFloat(Coef_4_20_B, B);
            }
            public void WriteCoefs_IEPE2(float A)
            {
                LogerViewModel.Instance.Write("Запись коэффициентов постоянной составляющей сигнала:");
                LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                Devices.Crate.WriteSwFloat(Coef_T_A, A);
                LogerViewModel.Instance.Write($"Коэффициент B: 0");
                Devices.Crate.WriteSwFloat(Coef_T_B, 0);
            }
            public void WriteCoefs_T(float A, float B)
            {
                LogerViewModel.Instance.Write("Запись коэффициентов Термопреобразователя:");
                LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                Devices.Crate.WriteSwFloat(Coef_T_A, A);
                LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                Devices.Crate.WriteSwFloat(Coef_T_B, B);
            }
            public void WriteCoefs_U(float A, float B)
            {
                LogerViewModel.Instance.Write($"Запись коэффициентов TIK_PLC 511.41 для {Num}-го канала:");
                LogerViewModel.Instance.Write($"Коэффициент A: {A}");
                Devices.Crate.WriteSwFloat(Coef_ACC_A, A);
                LogerViewModel.Instance.Write($"Коэффициент B: {B}");
                Devices.Crate.WriteSwFloat(Coef_ACC_B, B);
            }
            public async Task Setting_IEPE()
            {
                if (Devices.Multimeter.IsOpened() is false) throw new Exception("Необходимо подключить мультиметр");
                if (Devices.Generator.IsOpened() is false) throw new Exception("Необходимо подключить генератор");
                if (CheckSettingFlag(SettingType.IEPE) is false) return;
                LogerViewModel.Instance.Write($"Настройка IEPE, канала {Num}");
                await Dialog.ShowBuild("IEPE", $"Установите контакты для настройки IEPE {Num}-го канала");
                Reset(SettingType.IEPE);

                await ValidationVoltage(DC_Value, 0.5d);

                await SetVoltage(Point_1);
                GetVoltageAC(out float V1);
                AverageValue(ACC_RMS, out float Signal_1);
                AverageValue(Speed_RMS, out float Integral_1);

                await SetVoltage(Point_2);
                GetVoltageAC(out float V2);
                AverageValue(ACC_RMS, out float Signal_2);
                AverageValue(Speed_RMS, out float Integral_2);

                float A1 = (V2 - V1) / Coef / (Signal_2 - Signal_1);
                float B1 = V1 / Coef - A1 * Signal_1;
                float A2 = (V2 - V1) * 2 / Coef / (Integral_2 - Integral_1);
                float B2 = V1 * 2 / Coef - A2 * Integral_1;
                WriteCoefs_ACC(A1, B1);
                WriteCoefs_Speed(A2, B2);


                float dc = Devices.Crate.ReadSwFloat(PhysicalB0);
                float real_dc = (float)(Devices.Multimeter.GetVoltage("DC", 1000));
                WriteCoefs_IEPE2(dc / real_dc);
                await SetVoltage(Point_1);

                GetVoltageAC(out V1);

                CheckSetting(ACC_RMS, V1,ValueType.ACC);
                //AverageValue(ACC_RMS, out float ACC);
                //CountRelative(ACC * Coef, V1, out float relative);
                //CheckSetting(relative, SettingType.IEPE);
                

                await SetVoltage(Point_2);
                GetVoltageAC(out V2);

                float Move = Devices.Crate.ReadUInt16(Move_RMS) / 10f;
                CountRelative(Move * Coef / 4, V2, out float relative);
                // перенастройка перемещения.костыль
                if (relative <= -1)
                {
                    LogerViewModel.Instance.Write($"Переписывание коэффициента перемещения");
                    WriteCoefs_Speed( A2 + 0.04f, B2);
                }
                else if (relative >= 1)
                {
                    LogerViewModel.Instance.Write($"Переписывание коэффициента перемещения");
                    WriteCoefs_Speed( A2 - 0.02f, B2);
                }
                CheckSetting(ACC_RMS, V2, ValueType.ACC);
                CheckSetting(Speed_RMS, V2, ValueType.Speed);
                CheckSetting(Move_RMS, V2, ValueType.Move);

                //AverageValue(ACC_RMS, out float ACC_2);
                //CountRelative(ACC_2 * Coef, V2, out relative);
                //CheckSetting(relative, SettingType.ACC);

                //AverageValue(Speed_RMS, out float Speed);
                //CountRelative(Speed * Coef/2, V2, out relative);
                //CheckSetting(relative, SettingType.Speed);

                //AverageValue(Speed_RMS, out float Move_2);
                //CountRelative(Move_2 * Coef / 4, V2, out relative);
                //CheckSetting(relative, SettingType.Move);
                LogerViewModel.Instance.Write($"✔ Настройка IEPE, канала {Num} закончена");
            }
            public async Task Setting_4_20()
            {
                if (Devices.Multimeter.IsOpened() is false) throw new Exception("Необходимо подключить мультиметр");
                if (CheckSettingFlag(SettingType._4_20) is false) return;
                LogerViewModel.Instance.Write($"Настройка 4-20, канала {Num}");
                await Dialog.ShowBuild("4_20", $"Установите контакты для настройки 4-20 {Num}-го канала");
                Reset(SettingType._4_20);
                await ValidationVoltage(0.4,0.02);
                float I1 = Devices.Crate.ReadUInt16(DC);
                await ValidationVoltage(2, 0.02);
                float I2 = Devices.Crate.ReadUInt16(DC);
                float A = 16 / (I2 - I1);
                float B = 4 - A * I1;
                WriteCoefs_4_20(A, B);
                LogerViewModel.Instance.Write($"✔ Настройка 4-20, канала {Num} закончена");
            }
            public async Task Setting_T()
            {
                if (CheckSettingFlag(SettingType.T) is false) return;
                LogerViewModel.Instance.Write($"Настройка термопреобразователя, канала {Num}");
                await Dialog.ShowBuild("T", $"Установите контакты для настройки термопреобразователя {Num}-го канала");
                Reset(SettingType.T);
                await Dialog.ShowParam();
                float T1 = 0, T2 = 0;
                float Readed_T = 0f;
                ushort typetermo = (ushort)(TermoTypes.IndexOf(TermoType) + 1);
                SetTermoValues(out float R1, out float R2, out float Need_T1, out float Need_T2);
                Devices.Crate.WriteUInt16(TypeTermo, typetermo);
                await Dialog.ShowConfirm($"Установите на магазине сопротивлений 100 Ом");
                await Task.Delay(15000); // стоит потому что долго обновляется значение в крейте
                T1 = Devices.Crate.ReadUInt16(ResistTermo) / 1000f;
                await Dialog.ShowConfirm($"Установите на магазине сопротивлений 400 Ом");
                await Task.Delay(25000); // стоит потому что долго обновляется значение в крейте
                T2 = Devices.Crate.ReadUInt16(ResistTermo) / 1000f;
                float A = 3 / (T2 - T1);
                float B = 1 - A * T1;
                WriteCoefs_T(A,B);

                await Dialog.ShowConfirm($"Установите на магазине сопротивлений {R1} Ом");
                Thread.Sleep(15000);// стоит потому что долго обновляется значение в крейте
                Readed_T = Devices.Crate.ReadUInt16(T) / 10f;
                float relative = Math.Abs(Readed_T - Need_T1);
                if (relative > 1) throw new Exception($"Точка 1 не прошла проверку, значение отклонено от нормы на {relative}");

                await Dialog.ShowConfirm($"Установите на магазине сопротивлений {R2} Ом");
                Thread.Sleep(15000);// стоит потому что долго обновляется значение в крейте
                Readed_T = Devices.Crate.ReadUInt16(T) / 10f;
                relative = Math.Abs(Readed_T - Need_T2);
                if (relative > 1) throw new Exception($"Точка 2 не прошла проверку, значение отклонено от нормы на {relative}");

                LogerViewModel.Instance.Write($"✔ Настройка термопреобразователя, канала {Num} закончена");
            }
            public async Task Setting_U()
            {
                if (Devices.Multimeter.IsOpened() is false) throw new Exception("Необходимо подключить мультиметр");
                if (Devices.Generator.IsOpened() is false) throw new Exception("Необходимо подключить генератор");
                if (CheckSettingFlag(SettingType.U) is false) return;
                LogerViewModel.Instance.Write($"Настройка TIK-PLC.511, канала {Num}");
                await Dialog.ShowBuild("U", $"Установите контакты для настройки TIK-PLC 511.41 {Num}-го канала");
                Reset(SettingType.U);
                await ValidationVoltage(10,0.02,Physical);
                //await Dialog.ShowConfirm("Установите на калибраторе смещение 10В");
                Devices.Generator.SetFrequency(79.6);
                Devices.Generator.SetOffset(0);
                await SetVoltage(0.5d);
                float Signal_1 = Devices.Crate.ReadUInt16(ACC_PP) / 100f;
                await SetVoltage(20);
                GetVoltageAC(out float V1);
                float Signal_2 = Devices.Crate.ReadUInt16(ACC_PP) / 100f;
                GetVoltageAC(out float V2);

                float A1 = (V2 - V1) / Coef / (Signal_2 - Signal_1);
                float B1 = V1 / Coef - A1 * Signal_1;
                WriteCoefs_U(A1, B1);

                await SetVoltage(0.5d);
                GetVoltageAC(out V1);
                CheckSetting(ACC_PP,V1,ValueType.ACC);

                await SetVoltage(20d);
                GetVoltageAC(out V2);
                CheckSetting(ACC_PP, V2, ValueType.ACC);
                LogerViewModel.Instance.Write($"Настройка TIK-PLC.511, канала {Num} закончена");
            }
        }
        public class Channel_1 : Channel
        {
            public override string Num { get; } = "1";
            public override ushort Coef_ACC_A { get; } = (ushort)(60000 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B { get; } = (ushort)(60002 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A { get; } = (ushort)(60004 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B { get; } = (ushort)(60006 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A { get; } = (ushort)(60008 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B { get; } = (ushort)(60010 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A { get; } = (ushort)(60012 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_B { get; } = (ushort)(60014 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo { get; } = (ushort)(60016 + 90 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 { get; } = (ushort) (8018 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical { get; } = (ushort) (8020 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A { get; } = (ushort)(8021 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS { get; } = (ushort)(8022 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP { get; } = (ushort)(8023 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_A { get; } = (ushort)(8024 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS { get; } = (ushort)(8025 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP { get; } = (ushort)(8026 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A { get; } = (ushort)(8027 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS { get; } = (ushort)(8028 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP { get; } = (ushort)(8029 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort DC { get; } = (ushort)(8030 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T { get; } = (ushort)(8032 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ResistTermo { get; } = (ushort)(8036 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort OnChannel { get; } = (ushort)(11056 + 600 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnSaw { get; } = (ushort)(11110 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }
        public class Channel_2 : Channel
        {
            public override string Num { get; } = "2";
            public override ushort Coef_ACC_A { get; } = (ushort)(60030 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B { get; } = (ushort)(60032 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A { get; } = (ushort)(60034 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B { get; } = (ushort)(60036 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A { get; } = (ushort)(60038 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B { get; } = (ushort)(60040 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A { get; } = (ushort)(60042 + 90 * (Convert.ToInt16(ItemModule) - 1)); 
            public override ushort Coef_T_B { get; } = (ushort)(60044 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo { get; } = (ushort)(60046 + 90 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 { get; } = (ushort) (8043 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical { get; } = (ushort) (8045 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A { get; } = (ushort)(8046 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS { get; } = (ushort)(8047 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP { get; } = (ushort)(8048 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_A { get; } = (ushort)(8049 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS { get; } = (ushort)(8050 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP { get; } = (ushort)(8051 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A { get; } = (ushort)(8052 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS { get; } = (ushort)(8053 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP { get; } = (ushort)(8054 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort DC { get; } = (ushort)(8055 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T { get; } = (ushort)(8057 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ResistTermo { get; } = (ushort)(8061 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort OnChannel { get; } = (ushort)(11256 + 600 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnSaw { get; } = (ushort)(11310 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }
        public class Channel_3 : Channel
        {
            public override string Num { get; } = "3";
            public override ushort Coef_ACC_A { get; } = (ushort)(60060 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B { get; } = (ushort)(60062 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A { get; } = (ushort)(60064 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B { get; } = (ushort)(60066 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A { get; } = (ushort)(60068 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B { get; } = (ushort)(60070 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A { get; } = (ushort)(60072 + 90 * (Convert.ToInt16(ItemModule) - 1)); 
            public override ushort Coef_T_B { get; } = (ushort)(60074 + 90 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo { get; } = (ushort)(60076 + 90 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 { get; } = (ushort) (8068 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical { get; } = (ushort) (8070 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A { get; } = (ushort)(8071 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS { get; } = (ushort)(8072 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP { get; } = (ushort)(8073 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_A { get; } = (ushort)(8074 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS { get; } = (ushort)(8075 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP { get; } = (ushort)(8076 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A { get; } = (ushort)(8077 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS { get; } = (ushort)(8078 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP { get; } = (ushort)(8079 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort DC { get; } = (ushort)(8080 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T { get; } = (ushort)(8082 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort ResistTermo { get; } = (ushort)(8086 + 100 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort OnChannel { get; } = (ushort)(11456 + 600 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnSaw { get; } = (ushort)(11510 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }
        public class Channel_4 : Channel
        {
            public override string Num { get; } = "4";
            public override ushort Coef_ACC_A { get; } = (ushort)(61260 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_ACC_B { get; } = (ushort)(61262 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_A { get; } = (ushort)(61264 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_Speed_B { get; } = (ushort)(61266 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_A { get; } = (ushort)(61268 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_4_20_B { get; } = (ushort)(61270 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Coef_T_A { get; } = (ushort)(61272 + 30 * (Convert.ToInt16(ItemModule) - 1)); 
            public override ushort Coef_T_B { get; } = (ushort)(61274 + 30 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort TypeTermo { get; } = (ushort)(61276 + 30 * (Convert.ToInt16(ItemModule) - 1));

            public override ushort PhysicalB0 { get; } = (ushort) (9406 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Physical { get; } = (ushort) (9408 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_A { get; } = (ushort)(9409 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_RMS { get; } = (ushort)(9410 + 25 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ACC_PP { get; } = (ushort)(9411 + 100 * (Convert.ToInt16(ItemModule) - 1));
            #region Резерв
            public override ushort Speed_A { get; } = (ushort)(9412 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_RMS { get; } = (ushort)(9413 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Speed_PP { get; } = (ushort)(9414 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_A { get; } = (ushort)(9415 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_RMS { get; } = (ushort)(9416 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort Move_PP { get; } = (ushort)(9417 + 100 * (Convert.ToInt16(ItemModule) - 1));
            #endregion
            public override ushort DC { get; } = (ushort)(9418 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort T { get; } = (ushort)(9420 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort ResistTermo { get; } = (ushort)(9424 + 100 * (Convert.ToInt16(ItemModule) - 1));
            public override ushort OnChannel { get; } = 0;
            public override ushort OnSaw { get; } = (ushort)(19510 + 600 * (Convert.ToInt16(ItemModule) - 1));
        }

        public static Channel Channel1 { get; } = new Channel_1();
        public static Channel Channel2 { get; } = new Channel_2();
        public static Channel Channel3 { get; } = new Channel_3();
        public static Channel Channel4 { get; } = new Channel_4();

        public static ObservableCollection<string> Moduls = ["Не обнаружено","1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"];
        public static string ItemModule = "1";
        public static ObservableCollection<string> PLC { get; } = ["241", "242", "243","511", "371", "374", "375"];
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
        public static double Point_1 {get{return Coef_Is_10 ? 0.01d : 0.00667d;}}
        public static double Point_2 {get{return Coef_Is_10 ? 3d : 0.1d;} }
        public static float Coef {get{return Coef_Is_10 ? 10f : 6.67f;} }
        public static double Frequency { get; } = 80d;
        public static async Task Start(string item)
        {
            ushort type = Devices.Crate.ReadUInt16(Crate.Registers.Type);
            if (ItemPLC != PLC[type - 1])
            {
                await Dialog.ShowConfirm($"Выбранный тип контроллера не соответствует типу, записанному на контроллер. Настроить контроллер как тип PLC.{PLC[type - 1]} ?", true);
            }
            Devices.Crate.SetPassword();
            Devices.Generator.SetFrequency(Frequency);
            switch (item)
            {
                case "241":
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_4_20();
                    Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
                case "242":
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_IEPE();
                    Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
                case "243":
                    await Channel1.Setting_4_20();
                    await Channel2.Setting_4_20();
                    break;
                case "511":
                    await Channel1.Setting_U();
                    await Channel2.Setting_U();
                    await Channel3.Setting_U();
                    await Channel4.Setting_U();
                    break;
                case "371":
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_4_20();
                    await Channel3.Setting_T();
                    Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
                case "374":
                    await Channel1.Setting_4_20();
                    await Channel2.Setting_4_20();
                    await Channel3.Setting_4_20();
                    break;
                case "375":
                    await Channel1.Setting_IEPE();
                    await Channel2.Setting_4_20();
                    await Channel3.Setting_4_20();
                    Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
                    break;
            }
        }
        public static async Task ValidationVoltage(double V,double relative, string type = "DC")
        {
            bool Valid = false;
            string mes = $"При помощи магазина сопротивлений задайте {V} В";
            do
            {
                await Dialog.ShowConfirm(mes);
                double value = Devices.Multimeter.GetVoltage(type, 500);
                Valid = Math.Abs(value - V) < relative ? true : false;
                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} В.";
            }
            while (Valid is false);
        }
        public static async Task ValidationVoltage(double V, double relative,ushort reg)
        {
            bool Valid = false;
            string mes = $"При помощи калибратора задайте смещение {V} В";
            do
            {
                await Dialog.ShowConfirm(mes);
                double value = Devices.Crate.ReadUInt16(reg);
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

        private static void AverageValue(ushort Reg_adr, out float Value)
        {
            Value = 0;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(400);
                Value += Devices.Crate.ReadUInt16(Reg_adr) / 100f;
            }
            Value /= 10;
        }
        public static async Task SetVoltage(double V)
        {
            Devices.Generator.SetVoltage(ConvertValue.ToPP(V));
            await Task.Delay(5000);
        }
        public static void GetVoltageAC(out float V) => V = (float)Devices.Multimeter.GetVoltage("AC", 1000);
        public static void CountRelative(float value, float V, out float relative) => relative = Point_2 >= 1000 ? (value - V) / V * 100 : (value - V) / 1000 * 100; // (относительная/приведенная) погрешность
    }
}
