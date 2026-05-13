using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using APM_Crate.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel
{
    public class IEPE : Setting
    {
        protected override string Name { get; set; } = "IEPE";

        protected float Coef_A1 {  get; set; }
        protected float Coef_B1 {  get; set; }
        protected float Coef_A2 {  get; set; }
        protected float Coef_B2 {  get; set; }

        protected float Coef_DC {  get; set; }
        protected override async Task Preparing()
        {
            await Devices.Generator.SetFrequency(Frequency);
            await WP.Step(10,"Сборка схемы",()=> Dialog.ShowBuild("IEPE", $"Установите контакты для настройки IEPE {Channel.Num}-го канала.\r\n" +
                (Channel.Num) switch
                {
                    "1" => "In-2 GND-3",
                    "2" => "In-5 GND-6",
                }));
            //$"In-2 GND-3 для 1 канала\r\n" +
            //$"In-5 GND-6 для 2 канала");

            await SourceOn();
            await ValidationVoltageDC(DC_Value, 0.5d);
        }
        protected override async Task CountCoefs()
        {
            await Devices.Multimeter.VoltmeterMode("AC");
            await SetVoltage(Point_1);
            float V1 = await GetVoltageAC();
            float Signal_1 = await GetValue(ValueType.ACC_RMS);
            float Integral_1 = await GetValue(ValueType.Speed_RMS);


            await SetVoltage(Point_2);
            await WaitForChangeRegisters(Channel.ACC_RMS, Channel.Speed_RMS, Signal_1, Integral_1, 0.01f);
            float V2 = await GetVoltageAC();
            float Signal_2 = await GetValue(ValueType.ACC_RMS);
            float Integral_2 = await GetValue(ValueType.Speed_RMS);


            float dc = await GetValue(ValueType.DC);
            await Devices.Generator.ChanelOff(Devices.Generator.channelNum);
            await Devices.Multimeter.VoltmeterMode("DC");
            float real_dc = (float)(await Devices.Multimeter.GetVoltage("DC", 1000));
            await Devices.Generator.ChanelOn(Devices.Generator.channelNum);


            Coef_A1 = (V2 - V1) / Coef / (Signal_2 - Signal_1);
            Coef_B1 = V1 / Coef - Coef_A1 * Signal_1;
            Coef_A2 = (V2 - V1) * 2 / Coef / (Integral_2 - Integral_1);
            Coef_B2 = V1 * 2 / Coef - Coef_A2 * Integral_1;

            Coef_DC = real_dc / dc;
        }
        protected override async Task WriteCoefsAbstract()
        {
            await WriteCoefs(Coef_A1, Coef_B1,CoefType.ACC);
            await WriteCoefs(Coef_A2, Coef_B2, CoefType.Speed);
            await WriteCoefs(Coef_DC, 0, CoefType.T);
        }
        protected override async Task CheckSettingAbstract()
        {
            await LogerViewModel.Instance.Write("Проверка настроек");
            await SetVoltage(Point_1);
            float V1 = await GetVoltageAC();
            float Signal_1 = await GetValue(ValueType.ACC_RMS);
            float Integral_1 = await GetValue(ValueType.Speed_RMS);

            await WP.Step(5, "Проверка настроек ускорения", () => CheckSetting(V1, ValueType.ACC_RMS));

            await SetVoltage(Point_2);
            float V2 = await GetVoltageAC();

            await WaitForChangeRegisters(Channel.ACC_RMS, Channel.Speed_RMS, Signal_1, Integral_1, 0.01f);

            float Move = await GetValue(ValueType.Move_RMS);
            CountRelative(Move * Coef / 4, V2, out float relative);

            // перенастройка перемещения.костыль
            if (relative <= -1)
            {
                Coef_A2 += 0.04f;
                await WriteCoefs(Coef_A2, Coef_B2,CoefType.Speed);
            }
            else if (relative >= 1)
            {
                Coef_A2 -= 0.02f;
                await WriteCoefs(Coef_A2, Coef_B2, CoefType.Speed);
            }

            await WP.Step(5, "Проверка настроек ускорения", () => CheckSetting(V2, ValueType.ACC_RMS));
            await WP.Step(5, "Проверка настроек скорости", () => CheckSetting(V2, ValueType.Speed_RMS));
            await WP.Step(5, "Проверка настроек перемещения", () => CheckSetting(V2, ValueType.Move_RMS));

        }

        protected override async Task ResetCoefs()
        {
            await Devices.Crate.WriteSwFloat(Channel.Coef_ACC_A, 1);
            await Devices.Crate.WriteSwFloat(Channel.Coef_ACC_B, 0);
            await Devices.Crate.WriteSwFloat(Channel.Coef_Speed_A, 1);
            await Devices.Crate.WriteSwFloat(Channel.Coef_Speed_B, 0);
            //await Devices.Crate.WriteSwFloat(Coef_4_20_A, 1);
            //await Devices.Crate.WriteSwFloat(Coef_4_20_B, 0);
            await Devices.Crate.WriteSwFloat(Channel.Coef_T_A, 1);
            await Devices.Crate.WriteSwFloat(Channel.Coef_T_B, 0);
        }
        protected override async Task WriteListValuesSetting()
        {
            float CoefA1 = await Devices.Crate.ReadSwFloat(Channel.Coef_ACC_A);
            float CoefB1 = await Devices.Crate.ReadSwFloat(Channel.Coef_ACC_B);

            float CoefA2 = await Devices.Crate.ReadSwFloat(Channel.Coef_Speed_A);
            float CoefB2 = await Devices.Crate.ReadSwFloat(Channel.Coef_Speed_B);

            WriteList("Ускорение", CoefA1, CoefB1);
            WriteList("Скорость", CoefA2, CoefB2);
        }






        private async Task ValidationVoltageDC(double V, double relative)
        {
            bool Valid = false;
            string mes = $"При помощи магазина сопротивлений задайте {V} В";
            if (Devices.Generator.IsOpened()) await Devices.Generator.ChanelOff(Devices.Generator.channelNum);
            do
            {
                await Dialog.ShowConfirm(mes, new DC());
                double value = await Devices.Multimeter.GetVoltage("DC", 500);
                Valid = Math.Abs(value - V) < relative ? true : false;
                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} В.";
            }
            while (Valid is false);
            if (Devices.Generator.IsOpened()) await Devices.Generator.ChanelOn(Devices.Generator.channelNum);
        }
        private async Task SetVoltage(double V)
        {
            await WP.Step(5, $"Установка напряжения {V} мВ AC",() => Devices.Multimeter.SetVoltage(Devices.Generator, V, Frequency, 0.0005, 3));
            await Task.Delay(5000);
        }
        private async Task<float> GetVoltageAC()
        {
            WP.Report(0, "Чтение значения из вольтметра");
            await Devices.Multimeter.VoltmeterMode("AC");
            float result = (float)(await Devices.Multimeter.GetVoltage("AC", 1000) * 1000);
            WP.Report(5, "Чтение значения из вольтметра ✔");
            return result;
        }
        private async Task WaitForChangeRegisters(ushort reg1, ushort reg2, float FirstsValue1, float FirstsValue2, float coefreg)
        {
            WP.Report(0, $"Ожидание изменения регистров {reg1} и {reg2}");
            int sec = 0;
            bool IsChange1 = false;
            bool IsChange2 = false;
            while ((IsChange1 is false || IsChange2 is false) && sec < 60)
            {
                IsChange1 = await Devices.Crate.ReadUInt16(reg1) * coefreg == FirstsValue1 ? false : true;
                IsChange2 = await Devices.Crate.ReadUInt16(reg2) * coefreg == FirstsValue2 ? false : true;
                await Task.Delay(1000);
                sec++;
            }
            if (sec < 60) await Task.Delay(4000);

            if (IsChange1 is false || IsChange2 is false) { throw new Exception("Значения обновляются слишком долго."); }
            WP.Report(5, $"Ожидание изменения регистров {reg1} и {reg2} ✔");
        }
        private async Task CheckSetting(float V, ValueType type)
        {
            float mkm = 0f;
            string s = "";
            switch (type)
            {
                case ValueType.ACC_A: mkm = 1; s = "Ускорение. Амплитуда"; break;
                case ValueType.ACC_RMS: mkm = 1; s = "Ускорение. СКЗ"; break;
                case ValueType.ACC_PP: mkm = 1; s = "Ускорение. Размах"; break;

                case ValueType.Speed_A: mkm = 2; s = "Скорость. Амплитуда"; break;
                case ValueType.Speed_RMS: mkm = 2; s = "Скорость. СКЗ"; break;
                case ValueType.Speed_PP: mkm = 2; s = "Скорость. Размах"; break;

                case ValueType.Move_A: mkm = 4; s = "Перемещение. Амплитуда"; break;
                case ValueType.Move_RMS: mkm = 4; s = "Перемещение. СКЗ"; break;
                case ValueType.Move_PP: mkm = 4; s = "Перемещение. Размах"; break;
            }
            float value = await GetValue(type);
            float relative = 0;
            CountRelative(value * Coef / mkm, V, out relative);
            relative = Math.Abs(relative);
            if (relative >= 1)
            {
                throw new Exception($"{s} канала {Channel.Num} настроено не корректно. Значение отклонено на {Math.Round(relative, 2)}%");
            }
        }
        private void CountRelative(float value, float V, out float relative) => relative = V >= 1000 ? (value - V) / V * 100 : (value - V) / 1000 * 100; // (относительная/приведенная) погрешность

    }
}
