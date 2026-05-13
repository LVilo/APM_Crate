//using APM_Crate.Models.DevicesModel;
//using APM_Crate.Models.RestApiModel;
//using APM_Crate.Service;
//using APM_Crate.ViewModels;
//using PortsWork;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using APM_Crate.Models.SettingsModel;

//namespace APM_Crate.Models
//{
//    public class SettingModel
//    {
//        public abstract class Channel
//        {
//            //public Stopwatch stopwatch { get; } = new Stopwatch();

//            public abstract string Num { get; }
//            public abstract ushort Coef_ACC_A { get; }
//            public abstract ushort Coef_ACC_B { get; }

//            public abstract ushort Coef_Speed_A { get; }
//            public abstract ushort Coef_Speed_B { get; }
//            public abstract ushort Coef_4_20_A { get; }
//            public abstract ushort Coef_4_20_B { get; }
//            public abstract ushort Coef_T_A { get; }
//            public abstract ushort Coef_T_B { get; }
//            public abstract ushort TypeTermo { get; }

//            public abstract ushort PhysicalB0 { get; }
//            public abstract ushort Physical { get; }
//            //Значения прочтенные с этих регистров делить на 100 ↓
//            public abstract ushort ACC_A { get; }
//            public abstract ushort ACC_RMS { get; }
//            public abstract ushort ACC_PP { get; }
//            public abstract ushort Speed_A { get; }
//            public abstract ushort Speed_RMS { get; }
//            public abstract ushort Speed_PP { get; }
//            //делить на 10↓
//            public abstract ushort Move_A { get; }
//            public abstract ushort Move_RMS { get; }
//            public abstract ushort Move_PP { get; }
//            public abstract ushort DC { get; }
//            public abstract ushort T { get; }
//            //↑
//            public abstract ushort ResistTermo { get; }
//            public abstract ushort OnChannel { get; }
//            public abstract ushort OnSaw { get; } 

//            public bool SettingChannel = true;
//            public bool CanSetting = true;

//            private async Task Reset(SettingType setting,WeightedProgress wp)
//            {
//                wp.Report(0, "Сброс коэффициентов");
//                switch (setting)
//                {
//                    case SettingType.IEPE:
//                        await Devices.Crate.WriteSwFloat(Coef_ACC_A, 1);
//                        await Devices.Crate.WriteSwFloat(Coef_ACC_B, 0);
//                        await Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
//                        await Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
//                        //await Devices.Crate.WriteSwFloat(Coef_4_20_A, 1);
//                        //await Devices.Crate.WriteSwFloat(Coef_4_20_B, 0);
//                        await Devices.Crate.WriteSwFloat(Coef_T_A, 1);
//                        await Devices.Crate.WriteSwFloat(Coef_T_B, 0);
//                        break;
//                    case SettingType._4_20:
//                        await Devices.Crate.WriteSwFloat(Coef_4_20_A, 1);
//                        await Devices.Crate.WriteSwFloat(Coef_4_20_B, 0);
//                        //await Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
//                        //await Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
//                        break;
//                    case SettingType.T:
//                        await Devices.Crate.WriteSwFloat(Coef_T_A, 1);
//                        await Devices.Crate.WriteSwFloat(Coef_T_B, 0);
//                        //await Devices.Crate.WriteSwFloat(Coef_Speed_A, 1);
//                        //await Devices.Crate.WriteSwFloat(Coef_Speed_B, 0);
//                        break;
//                    case SettingType.U:
//                        await Devices.Crate.WriteSwFloat(Coef_ACC_A, 1);
//                        await Devices.Crate.WriteSwFloat(Coef_ACC_B, 0);
//                        break;
//                }
//                wp.Report(2, "Сброс коэффициентов ✔");
//            }
//            public enum SettingType
//            {
//                IEPE,
//                _4_20,
//                T,
//                U
//            }
//            public enum ValueType
//            {
//                ACC,
//                ACC_PP,
//                Speed,
//                Move
//            }

//            public async Task<bool> CheckSettingFlag(SettingType type)
//            {
//                if (SettingChannel is false)
//                {
//                    switch (type)
//                    {
//                        case SettingType.IEPE:  await LogerViewModel.Instance.Write($"Настройка IEPE канала {Num} пропущена т.к. канал не был выбран"); break;
//                        case SettingType._4_20: await LogerViewModel.Instance.Write($"Настройка 4-20 канала {Num} пропущена т.к. канал не был выбран"); break;
//                        case SettingType.T: await LogerViewModel.Instance.Write($"Настройка термопреобразователя канала {Num} пропущена т.к. канал не был выбран"); break;
//                        case SettingType.U: await LogerViewModel.Instance.Write($"Настройка PLC.511 канала {Num} пропущена т.к. канал не был выбран"); break;
//                    }
//                    return false;
//                }
//                else return true;
//            }
//            public async Task SawOn()
//            {
//                await Devices.Crate.WriteUInt16(OnSaw, 1);
//            }
//            public async Task SawOff()
//            {
//                await Devices.Crate.WriteUInt16(OnSaw, 0);
//            }
//            public async Task SourceOn(WeightedProgress wp)
//            {
//                await wp.Step(1,"Включение источника",()=> Devices.Crate.WriteUInt16(OnChannel, 1));
//            }
//            public async Task SourceOff()
//            {
//                await Devices.Crate.WriteUInt16(OnChannel,0);
//            }
//            public async Task<float> GetValue(ValueType type)
//            {
//                return (type) switch
//                {
//                    ValueType.ACC => await Devices.Crate.ReadUInt16(ACC_RMS) * 0.01f,
//                    ValueType.ACC_PP => await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f,
//                    ValueType.Speed => await Devices.Crate.ReadUInt16(Speed_RMS) * 0.01f,
//                    ValueType.Move => await Devices.Crate.ReadUInt16(Move_RMS) * 0.1f
//                };
//            }
//            public async Task CheckSetting(float V, ValueType type)
//            {
//                float mkm = (type) switch
//                {
//                    ValueType.ACC =>1,
//                    ValueType.ACC_PP =>1,
//                    ValueType.Speed =>2,
//                    ValueType.Move  =>4
//                };
//                float value = await GetValue(type);
//                float relative = 0;
//                 CountRelative(value * Coef / mkm, V,out relative);
//                relative = Math.Abs(relative);
//                if (relative >= 1)
//                {
//                    string s = "";
//                    switch (type)
//                    {
//                        case ValueType.ACC: s = "Ускорение"; break;
//                        case ValueType.ACC_PP: s = "Ускорение. Размах"; break;
//                        case ValueType.Speed: s = "Скорость"; break;
//                        case ValueType.Move: s = "Перемещение"; break;
//                    }
//                    throw new Exception($"{s} канала {Num} настроено не корректно. Значение отклонено на {Math.Round(relative, 2)}%");
//                }
//            }
//            public async Task WriteCoefs_ACC(float A,float B,WeightedProgress wp)
//            {
//                wp.Report(0, "Запись коэффициентов ускорения");
//                await LogerViewModel.Instance.Write($"Канал {Num}. Запись коэффициентов ускорения:");
//                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
//                await Devices.Crate.WriteSwFloat(Coef_ACC_A, A);
//                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
//                await Devices.Crate.WriteSwFloat(Coef_ACC_B, B);
//                WriteList("Ускорение", A,B);
//                wp.Report(2, "Запись коэффициентов ускорения ✔");
//            }
//            public async Task WriteCoefs_Speed(float A, float B, WeightedProgress wp)
//            {
//                wp.Report(0, "Запись коэффициентов скорости");
//                await LogerViewModel.Instance.Write($"Канал {Num}. Запись коэффициентов скорости:");
//                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
//                await Devices.Crate.WriteSwFloat(Coef_Speed_A, A);
//                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
//                await Devices.Crate.WriteSwFloat(Coef_Speed_B, B);
//                WriteList("Скорость", A,B);
//                wp.Report(2, "Запись коэффициентов скорости ✔");
//            }
//            public async Task WriteCoefs_4_20(float A, float B, WeightedProgress wp)
//            {
//                wp.Report(0, "Запись коэффициентов 4-20");
//                await LogerViewModel.Instance.Write($"Канал {Num}. Запись коэффициентов 4-20:");
//                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
//                await Devices.Crate.WriteSwFloat(Coef_4_20_A, A);
//                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
//                await Devices.Crate.WriteSwFloat(Coef_4_20_B, B);
//                WriteList("Ток 4-20", A,B);
//                wp.Report(2, "Запись коэффициентов 4-20 ✔");
//            }
//            public async Task WriteCoefs_T(float A, float B, WeightedProgress wp)
//            {
//                wp.Report(0, "Запись коэффициентов Термопреобразователя");
//                await LogerViewModel.Instance.Write($"Канал {Num}. Запись коэффициентов Термопреобразователя:");
//                await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
//                await Devices.Crate.WriteSwFloat(Coef_T_A, A);
//                await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
//                await Devices.Crate.WriteSwFloat(Coef_T_B, B);
//                WriteList("Температура",A,B);
//                wp.Report(2, "Запись коэффициентов Термопреобразователя ✔");
//            }
//            //public async Task WriteCoefs_U(float A, float B)
//            //{
//            //    await LogerViewModel.Instance.Write($"Запись коэффициентов TIK_PLC 511.41 для {Num}-го канала:");
//            //    await WriteCoefs_ACC(A, B);
//            //    //await LogerViewModel.Instance.Write($"Коэффициент A: {A}");
//            //    //await Devices.Crate.WriteSwFloat(Coef_ACC_A, A);
//            //    //await LogerViewModel.Instance.Write($"Коэффициент B: {B}");
//            //    //await Devices.Crate.WriteSwFloat(Coef_ACC_B, B);
//            //}
//            public void WriteList(string str,float A, float B)
//            {
//                settings.Add(new Settings { Name = $"Канал {Num}.{str} Коэффициент А", Value = A.ToString() });
//                settings.Add(new Settings { Name = $"Канал {Num}.{str} Коэффициент B", Value = B.ToString() });
//            }
//            public async Task Setting_IEPE(WeightedProgress wp)
//            {

//                if (await CheckSettingFlag(SettingType.IEPE) is false)
//                {
//                    float CoefA1 = await Devices.Crate.ReadSwFloat(Coef_ACC_A);
//                    float CoefB1 = await Devices.Crate.ReadSwFloat(Coef_ACC_B);

//                    float CoefA2 = await Devices.Crate.ReadSwFloat(Coef_Speed_A);
//                    float CoefB2 = await Devices.Crate.ReadSwFloat(Coef_Speed_B);

//                    WriteList("Ускорение", CoefA1, CoefB1);
//                    WriteList("Скорость", CoefA2, CoefB2);
//                    wp.Reset();
//                    return;
//                }
//                await Devices.Generator.SetFrequency(Frequency);
//                await LogerViewModel.Instance.Write($"Настройка IEPE, канал {Num}");
//                await wp.Step(2,"Сборка схемы",()=> Dialog.ShowBuild("IEPE", $"Установите контакты для настройки IEPE {Num}-го канала.\r\n" +
//                    (Num) switch
//                    {
//                        "1" => "In-2 GND-3",
//                        "2" => "In-5 GND-6",
//                    }));
//                //$"In-2 GND-3 для 1 канала\r\n" +
//                //$"In-5 GND-6 для 2 канала");

//                await SourceOn(wp);
//                await Reset(SettingType.IEPE,wp);

//                await ValidationVoltage(DC_Value, 0.5d,wp);


//                await Devices.Multimeter.VoltmeterMode("AC");
//                await SetVoltage(Point_1,wp);
//                float V1 = await GetVoltageAC( wp);

//                float Signal_1 = await GetValue(ValueType.ACC);
//                float Integral_1 = await GetValue(ValueType.Speed);
//                await SetVoltage(Point_2, wp);
//                float V2 = await GetVoltageAC(wp);
//                await WaitForChangeRegisters(ACC_RMS, Speed_RMS,Signal_1, Integral_1,0.01f,wp);

//                float Signal_2 = await GetValue(ValueType.ACC);
//                float Integral_2 = await GetValue(ValueType.Speed);

//                float A1 = (V2 - V1) / Coef / (Signal_2 - Signal_1);
//                float B1 = V1 / Coef - A1 * Signal_1;
//                float A2 = (V2 - V1) * 2 / Coef / (Integral_2 - Integral_1);
//                float B2 = V1 * 2 / Coef - A2 * Integral_1;

//                float dc = await Devices.Crate.ReadUInt16(Physical) * 0.001f;
//                await Devices.Generator.ChanelOff(Devices.Generator.channelNum);
//                await Devices.Multimeter.VoltmeterMode("DC");
//                float real_dc = (float)(await Devices.Multimeter.GetVoltage("DC", 1000));
//                await Devices.Generator.ChanelOn(Devices.Generator.channelNum);


//                await WriteCoefs_ACC(A1, B1,wp);
//                await WriteCoefs_Speed(A2, B2, wp);
//                await WriteCoefs_T(real_dc / dc,0, wp);


//                await LogerViewModel.Instance.Write("Проверка настроек");
//                await SetVoltage(Point_1, wp);
//                V1 = await GetVoltageAC(wp);
//                Signal_1 = await GetValue(ValueType.ACC);
//                Integral_1 = await GetValue(ValueType.Speed);

//                await wp.Step(5,"Проверка настроек ускорения",()=> CheckSetting(V1,ValueType.ACC));

//                await SetVoltage(Point_2, wp);
//                V2 = await GetVoltageAC(wp);

//                await WaitForChangeRegisters(ACC_RMS, Speed_RMS,Signal_1, Integral_1,0.01f,wp);

//                float Move = await Devices.Crate.ReadUInt16(Move_RMS) * 0.1f;
//                CountRelative(Move * Coef / 4, V2, out float relative);

//                // перенастройка перемещения.костыль
//                if (relative <= -1)
//                {
//                    await LogerViewModel.Instance.Write($"Переписывание коэффициента скорости");
//                    A2 += 0.04f;
//                    await WriteCoefs_Speed( A2, B2, wp);
//                }
//                else if (relative >= 1)
//                {
//                    await LogerViewModel.Instance.Write($"Переписывание коэффициента скорости");
//                    A2 -= 0.02f;
//                    await WriteCoefs_Speed( A2, B2, wp);
//                }
//                await wp.Step(5, "Проверка настроек ускорения", () => CheckSetting( V2, ValueType.ACC));
//                await wp.Step(5, "Проверка настроек скорости", () => CheckSetting( V2, ValueType.Speed));
//                await wp.Step(5, "Проверка настроек перемещения", () => CheckSetting( V2, ValueType.Move));

//                await LogerViewModel.Instance.Write($"✔ Настройка IEPE, канал {Num} закончена");
//                wp.Reset();
//            }
//            public async Task Setting_4_20(WeightedProgress wp)
//            {
//                if (await CheckSettingFlag(SettingType._4_20) is false)
//                {
//                    float CoefA1 = await Devices.Crate.ReadSwFloat(Coef_4_20_A);
//                    float CoefB1 = await Devices.Crate.ReadSwFloat(Coef_4_20_B);

//                    WriteList("Ток 4-20", CoefA1, CoefB1);

//                    return;
//                }
//                //string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
//                //stopwatch.Restart();
//                await LogerViewModel.Instance.Write($"Настройка тока 4-20, канал {Num}");
//                await Dialog.ShowBuild("4_20", $"Установите контакты для настройки тока 4-20 {Num}-го канала.\r\n" +
//                    (Num) switch
//                    { 
//                        "1" => "In-2 GND-3",
//                        "2" => "In-5 GND-6",
//                        "3" => "In-7 GND-8"
//                    });
//                    //$"In-2 GND-3 для 1 канала\r\n" +
//                    //$"In-5 GND-6 для 2 канала\r\n" +
//                    //$"In-7 GND-8 для 3 канала");
//                await Reset(SettingType._4_20, wp);
//                await SourceOn(wp);
//                await Validation_mA(4,0.02);
//                float I1 = await Devices.Crate.ReadSwFloat(DC);
//                await Validation_mA(20, 0.02);
//                float I2 = await Devices.Crate.ReadSwFloat(DC);
//                float A = 16 / (I2 - I1);
//                float B = 4 - A * I1;

//                await WriteCoefs_4_20(A, B, wp);


//                await LogerViewModel.Instance.Write("Проверка настроек");
//                float I = await Devices.Crate.ReadSwFloat(DC);
//                double mA = await Devices.Multimeter.GetAmperage();

//                if ((I - mA) / mA * 100 > 1) throw new Exception("Канал тока 4-20 настроился не правильно");

//                await LogerViewModel.Instance.Write($"✔ Настройка тока 4-20, канал {Num} закончена");
//            }
//            public async Task Setting_T(WeightedProgress wp)
//            {
//                if (await CheckSettingFlag(SettingType.T) is false)
//                {
//                    float CoefA1 = await Devices.Crate.ReadSwFloat(Coef_T_A);
//                    float CoefB1 = await Devices.Crate.ReadSwFloat(Coef_T_B);

//                    WriteList("Температура", CoefA1, CoefB1);
//                    return;
//                }
//                //stopwatch.Restart();
//                //string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
//                await LogerViewModel.Instance.Write($"Настройка термопреобразователя, канал {Num}");
//                await Reset(SettingType.T, wp);
//                await Dialog.ShowBuild("T", $"Установите контакты для настройки\r\n термопреобразователя {Num}-го канала.\r\n" +
//                    $"In+-9 In--8 GND-7");
//                await Dialog.ShowParam();
//                float T1 = 0, T2 = 0;
//                float Readed_T = 0f;
//                ushort typetermo = (ushort)(TermoTypes.IndexOf(TermoType) + 1);
//                SetTermoValues(out float R1, out float R2, out float Need_T1, out float Need_T2);
//                await Devices.Crate.WriteUInt16(TypeTermo, typetermo);

//                await Dialog.ShowConfirm($"Установите на магазине сопротивлений 100 Ом", new Delay());
//                await Task.Delay(15000); // стоит потому что долго обновляется значение в крейте
//                T1 = await Devices.Crate.ReadUInt16(ResistTermo) * 0.001f;
//                await Dialog.ShowConfirm($"Установите на магазине сопротивлений 400 Ом", new Delay());
//                await Task.Delay(25000); // стоит потому что долго обновляется значение в крейте
//                T2 = await Devices.Crate.ReadUInt16(ResistTermo) * 0.001f;
//                float A = 3 / (T2 - T1);
//                float B = 1 - A * T1;

//                await WriteCoefs_T(A,B, wp);

//                await LogerViewModel.Instance.Write("Проверка настроек");
//                await Dialog.ShowConfirm($"Установите на магазине сопротивлений {R1} Ом", new Delay());
//                await Task.Delay(15000);// стоит потому что долго обновляется значение в крейте
//                Readed_T = await Devices.Crate.ReadInt16(T) * 0.1f;
//                float relative = Math.Abs(Readed_T - Need_T1);
//                if (relative > 1) throw new Exception($"Точка 1 не прошла проверку, значение отклонено от нормы на {relative}");

//                await Dialog.ShowConfirm($"Установите на магазине сопротивлений {R2} Ом", new Delay());
//                await Task.Delay(15000);// стоит потому что долго обновляется значение в крейте
//                Readed_T = await Devices.Crate.ReadUInt16(T) * 0.1f;
//                relative = Math.Abs(Readed_T - Need_T2);
//                if (relative > 1) throw new Exception($"Точка 2 не прошла проверку, значение отклонено от нормы на {relative}");
//                //stopwatch.Stop();
//                //string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");

//                //string setting = IsResetting ? "Перенастройка T": "T";
//                //await WriteParam(setting, starttime,endtime);
//                await LogerViewModel.Instance.Write($"✔ Настройка термопреобразователя, канал {Num} закончена");
//            }
//            public async Task Setting_U(WeightedProgress wp)
//            {
//                if (await CheckSettingFlag(SettingType.U) is false)
//                {
//                    float CoefA1 = await Devices.Crate.ReadSwFloat(Coef_ACC_A);
//                    float CoefB1 = await Devices.Crate.ReadSwFloat(Coef_ACC_B);

//                    WriteList("Ускорение", CoefA1, CoefB1);
//                    return;
//                }
//                //string starttime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");
//                await LogerViewModel.Instance.Write($"Настройка TIK-PLC.511, канал {Num}");
//                await Reset(SettingType.U, wp);
//                await Dialog.ShowBuild("U", $"Установите контакты для настройки TIK-PLC 511.41 {Num}-го канала \r\n" +
//                    (Num) switch
//                    {
//                        "1" => "In-2 GND-3",
//                        "2" => "In-4 GND-5",
//                        "3" => "In-6 GND-7",
//                        "4" => "In-8 GND-9"
//                    });
//                //$"In+-2 GND-3 для 1 канала\r\n" +
//                //$"In+-4 GND-5 для 2 канала\r\n" +
//                //$"In+-6 GND-7 для 3 канала\r\n" +
//                //$"In+-8 GND-9 для 4 канала");
//                await ValidationVoltageByCalibrator(10,0.2);
//                //await Dialog.ShowConfirm("Установите на калибраторе смещение 10В");
//                await Devices.Generator.SetFrequency(Frequency);
//                await Devices.Generator.SetOffset(0);
//                await Devices.Multimeter.VoltmeterMode("AC");



//                await Devices.Generator.SetVoltage(0.5);

//                //await SetVoltage(0.5d);
//                await Task.Delay(5000);
//                //float V1 = await GetVoltageAC() * 2f * (float)Math.Sqrt(2f) * 0.001f;

//                // Ставлю значения с генератора, потому что вольтметр измеряет СКЗ, а не размах
//                float V1 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);
//                float Signal_1 = await GetValue(ValueType.ACC);
//                //float Signal_1 = await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f;
//                await Devices.Generator.SetVoltage(20);
//                //await SetVoltage(20);
//                await Task.Delay(8000);
//                await WaitForChangeRegisters(ACC_PP, Signal_1, 0.01f);
//                //float V2 = await GetVoltageAC() * 2f * (float)Math.Sqrt(2f) * 0.001f;


//                // Ставлю значения с генератора, потому что вольтметр измеряет СКЗ, а не размах
//                float V2 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);

//                float Signal_2 = await GetValue(ValueType.ACC);
//                //float Signal_2 = await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f;

//                float A1 = (V2 - V1) / (Signal_2 - Signal_1);
//                float B1 = V1 - A1 * Signal_1;
//                await WriteCoefs_ACC(A1, B1, wp);
//                await LogerViewModel.Instance.Write("Проверка настроек");
//                await Devices.Generator.SetVoltage(0.5d);
//                await Task.Delay(5000);
//                V1 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);
//                float value_1 = await GetValue(ValueType.ACC);
//                CountRelative(value_1, V1, out float relative_1);
//                if (relative_1 >= 1) throw new Exception($"Ускорение канала {Num} настроено не корректно. Значение отклонено на {Math.Round(relative_1, 2)}%");
//                //await CheckSetting(V1, ValueType.ACC);

//                await Devices.Generator.SetVoltage(20d);
//                await Task.Delay(8000);
//                V2 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);
//                float value_2 = await GetValue(ValueType.ACC);
//                CountRelative(value_2, V2, out float relative_2);
//                if (relative_2 >= 1) throw new Exception($"Ускорение канала {Num} настроено не корректно. Значение отклонено на {Math.Round(relative_2, 2)}%");
//                //await CheckSetting( V2, ValueType.ACC);

//                //string endtime = String.Format($"{DateTime.Now.Hour}.{DateTime.Now.Minute}");

//                //string setting = IsResetting ? "Перенастройка U":"U";

//                //await WriteParam(setting, starttime, endtime);
//                await LogerViewModel.Instance.Write($"✔ Настройка TIK-PLC.511, канал {Num} закончена");
//            }
//        }
        

//        //public static Channel Channel1 { get; } = new Channel_1();
//        //public static Channel Channel2 { get; } = new Channel_2();
//        //public static Channel Channel3 { get; } = new Channel_3();
//        //public static Channel Channel4 { get; } = new Channel_4();

//        //public static ObservableCollection<string> Moduls => ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"];
//        //public static string Crate.Slot = "1";
//        //public static ObservableCollection<string> PLC => ["241", "242", "243", "511", "371", "374", "375"];
//        //public static string ItemPLC = "241";

        

//        //public static async Task Samples(ushort type)
//        //{
//        //    byte[] data = await CheckFilePLC.GetDataSaw(type);   
//        //    await Dialog.ShowLiveCharts(data,type);

//        //    //CheckFilePLC.GetValuesChannel(data, out ushort[] channel1);
//        //    //CheckFilePLC.GetValuesChannel(data, out ushort[] channel2);
//        //    //CheckFilePLC.CheckSawChannel(channel1, out uint errors1);
//        //    //CheckFilePLC.CheckSawChannel(channel2, out uint errors2);
//        //    //await LogerViewModel.Instance.Write($"{errors1+errors2}");
//        //}
//        public static async Task Start(string item, WeightedProgress wp, WeightedProgress wp2)
//        {
//            switch (item)
//            {
//                case "241":
//                    await wp.Step(30, "Настройка канала 1, IEPE", () => Channel1.Setting_IEPE(wp2));
//                    await wp.Step(30, "Настройка канала 2, Ток 4-20", () => Channel2.Setting_4_20(wp2));
//                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
//                    break;
//                case "242":
//                    await wp.Step(30, "Настройка канала 1, IEPE", () => Channel1.Setting_IEPE(wp2));
//                    await wp.Step(30, "Настройка канала 2, IEPE", () => Channel2.Setting_IEPE(wp2));
//                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
//                    break;
//                case "243":
//                    await wp.Step(30, "Настройка канала 1, Ток 4-20", () => Channel1.Setting_4_20(wp2));
//                    await wp.Step(30, "Настройка канала 2, Ток 4-20", () => Channel2.Setting_4_20(wp2));
//                    break;
//                case "511":
//                    await wp.Step(15, "Настройка канала 1, U", () => Channel1.Setting_U(wp2));
//                    await wp.Step(15, "Настройка канала 2, U", () => Channel2.Setting_U(wp2));
//                    await wp.Step(15, "Настройка канала 3, U", () => Channel3.Setting_U(wp2));
//                    await wp.Step(15, "Настройка канала 4, U", () => Channel4.Setting_U(wp2));
//                    break;
//                case "371":
//                    await wp.Step(20, "Настройка канала 1, IEPE", () => Channel1.Setting_IEPE(wp2));
//                    await wp.Step(20, "Настройка канала 2, Ток 4-20", () => Channel2.Setting_4_20(wp2));
//                    await wp.Step(20, "Настройка канала 3, Температура", () => Channel3.Setting_T(wp2));
//                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
//                    break;
//                case "374":
//                    await wp.Step(20, "Настройка канала 1, Ток 4-20", () => Channel1.Setting_4_20(wp2));
//                    await wp.Step(20, "Настройка канала 2, Ток 4-20", () => Channel2.Setting_4_20(wp2));
//                    await wp.Step(20, "Настройка канала 3, Ток 4-20", () => Channel3.Setting_4_20(wp2));
//                    break;
//                case "375":
//                    await wp.Step(20, "Настройка канала 1, IEPE", () => Channel1.Setting_IEPE(wp2));
//                    await wp.Step(20, "Настройка канала 2, Ток 4-20", () => Channel2.Setting_4_20(wp2));
//                    await wp.Step(20, "Настройка канала 3, Ток 4-20", () => Channel3.Setting_4_20(wp2));
//                    await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Coef);
//                    break;
//            }
//        }
//        public static async Task Validation_mA(double V, double relative)
//        {
//            bool Valid = false;
//            string mes = $"Установите щупы мультиметра в режим измерения постоянного тока и при помощи магазина сопротивлений задайте {V} мА";
//            do
//            {
//                IGetVoltege mult = new A();
//                await Devices.Multimeter.AmmeterMode("DC");
//                await Dialog.ShowConfirm(mes, mult);
//                double value = await Devices.Multimeter.GetAmperage();
//                Valid = Math.Abs(value - V) < relative ? true : false;
//                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} мА.";
//            }
//            while (Valid is false);
//        }
//        public static async Task ValidationVoltageDC(double V, double relative)
//        {
//            bool Valid = false;
//            string mes = $"При помощи магазина сопротивлений задайте {V} В";
//            if (Devices.Generator.IsOpened()) await Devices.Generator.ChanelOff(Devices.Generator.channelNum);
//            do
//            {
//                await Dialog.ShowConfirm(mes, new DC());
//                double value = await Devices.Multimeter.GetVoltage("DC", 500);
//                Valid = Math.Abs(value - V) < relative ? true : false;
//                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} В.";
//            }
//            while (Valid is false);
//            if (Devices.Generator.IsOpened()) await Devices.Generator.ChanelOn(Devices.Generator.channelNum);
//        }
//        public static async Task ValidationVoltageByCalibrator(double V, double relative)
//        {
//            bool Valid = false;
//            string mes = $"При помощи калибратора задайте смещение {V} В";
//            if (Devices.Generator.IsOpened()) await Devices.Generator.SetVoltage(0d);
//            do
//            {
//                await Dialog.ShowConfirm(mes, new DC());
//                double value = await Devices.Multimeter.GetVoltage("DC", 500);
//                Valid = Math.Abs(value - V) < relative ? true : false;
//                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} В.";
//            }
//            while (Valid is false);
//            if (Devices.Generator.IsOpened()) await Devices.Generator.SetVoltage(0.5d);
//        }
//        private static void SetTermoValues(out float Resist_1,out float Resist_2,out float Need_T_1,out float Need_T_2)
//        {
//            Resist_1 = 78.7f;
//            Resist_2 = 185.2f;
//            Need_T_1 = -50;
//            Need_T_2 = 200;
//            // Взято с https://cdn.termexlab.ru/files/9e48a99f/9525/4985/9662/23c12e3a32c2.pdf
//            switch (TermoTypes.IndexOf(TermoType))
//            {
//                case 0:
//                    Resist_1 = 10.265f;
//                    Resist_2 = 92.8f;
//                    Need_T_1 = -180;
//                    Need_T_2 = 200;
//                    break;
//                case 1:
//                    Resist_1 = 20.53f;
//                    Resist_2 = 185.6f;
//                    Need_T_1 = -180;
//                    Need_T_2 = 200;
//                    break;
//                case 2:
//                    Resist_1 = 39.35f;
//                    Resist_2 = 92.6f;
//                    Need_T_1 = -50;
//                    Need_T_2 = 200;
//                    break;
//                case 3:
//                    Resist_1 = 78.7f;
//                    Resist_2 = 185.2f;
//                    Need_T_1 = -50;
//                    Need_T_2 = 200;
//                    break;
//                case 4:
//                    Resist_1 = 8.62f;
//                    Resist_2 = 197.58f;
//                    Need_T_1 = -200;
//                    Need_T_2 = 850;
//                    break;
//                case 5:
//                    Resist_1 = 17.24f;
//                    Resist_2 = 395.16f;
//                    Need_T_1 = -200;
//                    Need_T_2 = 850;
//                    break;
//                case 6:
//                    Resist_1 = 9.26f;
//                    Resist_2 = 195.24f;
//                    Need_T_1 = -200;
//                    Need_T_2 = 850;
//                    break;
//                case 7:
//                    Resist_1 = 18.52f;
//                    Resist_2 = 390.48f;
//                    Need_T_1 = -200;
//                    Need_T_2 = 850;
//                    break;

//            }
//        }

//        private static async Task<float> AverageValue(ushort Reg_adr)
//        {
//            float Value = 0;
//            for (int i = 0; i < 10; i++)
//            {
//                await Task.Delay(400);
//                Value += await Devices.Crate.ReadUInt16(Reg_adr) * 0.01f;
//            }
//            Value /= 10;
//            return Value;
//        }
//        public static async Task SetVoltage(double V,WeightedProgress wp)
//        {
//            //Devices.Generator.SetVoltage(ConvertValue.ToPP(V));
//            wp.Report(0, $"Установка напряжения {V} AC");
//            await Devices.Multimeter.SetVoltage(Devices.Generator, V, Frequency, 0.0005, 3);
//            wp.Report(2, $"Установка напряжения {V} AC ✔");
//            await Task.Delay(5000);
//        }
//        public static async Task WaitForChangeRegisters(ushort reg, float FirstsValue, float coefreg)
//        {
//            int sec = 0;
//            bool IsChange = false;
//            while (IsChange is false && sec < 30)
//            {
//                IsChange = await Devices.Crate.ReadUInt16(reg) * coefreg == FirstsValue ? false : true;
//                await Task.Delay(1000);
//                sec++;
//            }
//            if(sec<30) await Task.Delay(4000);
//            if (IsChange is false) { throw new Exception("Значения обновляются слишком долго."); }
//        }
//        public static async Task WaitForChangeRegisters(ushort reg1, ushort reg2, float FirstsValue1,float FirstsValue2,float coefreg,WeightedProgress wp)
//        {
//            wp.Report(0,$"Ожидание изменения регистров {reg1} и {reg2}");
//            int sec = 0;
//            bool IsChange1 = false;
//            bool IsChange2 = false;
//            while ((IsChange1 is false || IsChange2 is false) && sec <45)
//            {
//                IsChange1 = await Devices.Crate.ReadUInt16(reg1) * coefreg == FirstsValue1 ? false :true;
//                IsChange2 = await Devices.Crate.ReadUInt16(reg2) * coefreg == FirstsValue2 ? false :true;
//                await Task.Delay(1000);
//                sec++;
//            }
//            if(sec<60) await Task.Delay(4000);

//            if (IsChange1 is false || IsChange2 is false) { throw new Exception("Значения обновляются слишком долго."); }
//            wp.Report(2,$"Ожидание изменения регистров {reg1} и {reg2} ✔");
//        }
//        public static async Task<float> GetVoltageAC(WeightedProgress wp)
//        {
//            wp.Report(0, "Чтение значения из вольтметра");
//            await Devices.Multimeter.VoltmeterMode("AC");
//            float result = (float)(await Devices.Multimeter.GetVoltage("AC", 1000) * 1000);
//            wp.Report(1, "Чтение значения из вольтметра ✔");
//            return result;
//        }
//        public static void CountRelative(float value, float V, out float relative) => relative = V >= 1000 ? (value - V) / V * 100 : (value - V) / 1000 * 100; // (относительная/приведенная) погрешность
//    }
//}
