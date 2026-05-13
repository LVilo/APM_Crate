using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel
{
    public class U : Setting
    {
        protected override string Name { get; set; } = "U";


        protected float Coef_A {  get; set; }
        protected float Coef_B {  get; set; }
        protected override async Task Preparing()
        {
            await Dialog.ShowBuild("U", $"Установите контакты для настройки TIK-PLC 511.41 {Channel.Num}-го канала \r\n" +
                    (Channel.Num) switch
                    {
                        "1" => "In-2 GND-3",
                        "2" => "In-4 GND-5",
                        "3" => "In-6 GND-7",
                        "4" => "In-8 GND-9"
                    });
            //$"In+-2 GND-3 для 1 канала\r\n" +
            //$"In+-4 GND-5 для 2 канала\r\n" +
            //$"In+-6 GND-7 для 3 канала\r\n" +
            //$"In+-8 GND-9 для 4 канала");
            await ValidationVoltageByCalibrator(10, 0.2);
            await Devices.Generator.SetFrequency(Frequency);
            await Devices.Generator.SetOffset(0);
            await Devices.Multimeter.VoltmeterMode("AC");
        }
        protected override async Task CountCoefs()
        {
            await Devices.Generator.SetVoltage(0.5);
            await Task.Delay(5000);

            float V1 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);
            float Signal_1 = await GetValue(ValueType.ACC_RMS);
            //float Signal_1 = await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f;
            await Devices.Generator.SetVoltage(20);
            //await SetVoltage(20);
            await Task.Delay(8000);
            await WaitForChangeRegisters(Channel.ACC_RMS, Signal_1, 0.01f);
            //float V2 = await GetVoltageAC() * 2f * (float)Math.Sqrt(2f) * 0.001f;


            // Ставлю значения с генератора, потому что вольтметр измеряет СКЗ, а не размах
            float V2 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);

            float Signal_2 = await GetValue(ValueType.ACC_RMS);
            //float Signal_2 = await Devices.Crate.ReadUInt16(ACC_PP) * 0.01f;

            Coef_A = (V2 - V1) / (Signal_2 - Signal_1);
            Coef_B = V1 - Coef_A * Signal_1;
        }
        protected override async Task WriteCoefsAbstract()
        {
            await WriteCoefs(Coef_A, Coef_B,CoefType.ACC);
        }
        protected override async Task CheckSettingAbstract()
        {
            WP.Report(0,"Проверка настроек");
            await Devices.Generator.SetVoltage(0.5d);
            await Task.Delay(5000);
            float V1 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);
            float value_1 = await GetValue(ValueType.ACC_RMS);
            CountRelative(value_1, V1, out float relative_1);
            if (relative_1 >= 1) throw new Exception($"Ускорение канала {Channel.Num} настроено не корректно. Значение отклонено на {Math.Round(relative_1, 2)}%");
            //await CheckSetting(V1, ValueType.ACC);

            await Devices.Generator.SetVoltage(20d);
            await Task.Delay(8000);
            float V2 = (float)await Devices.Multimeter.GetVoltage("AC", 1000);
            float value_2 = await GetValue(ValueType.ACC_RMS);
            CountRelative(value_2, V2, out float relative_2);
            if (relative_2 >= 1) throw new Exception($"Ускорение канала {Channel.Num} настроено не корректно. Значение отклонено на {Math.Round(relative_2, 2)}%");
            WP.Report(2,"Проверка настроек ✔");
        }


        protected override async Task ResetCoefs()
        {
            await Devices.Crate.WriteSwFloat(Channel.Coef_ACC_A, 1);
            await Devices.Crate.WriteSwFloat(Channel.Coef_ACC_B, 0);
        }
        protected override async Task WriteListValuesSetting()
        {
            float CoefA1 = await Devices.Crate.ReadSwFloat(Channel.Coef_ACC_A);
            float CoefB1 = await Devices.Crate.ReadSwFloat(Channel.Coef_ACC_B);

            WriteList("Ускорение", CoefA1, CoefB1);
        }





        protected async Task ValidationVoltageByCalibrator(double V, double relative)
        {
            bool Valid = false;
            string mes = $"При помощи калибратора задайте смещение {V} В";
            if (Devices.Generator.IsOpened()) await Devices.Generator.SetVoltage(0d);
            do
            {
                await Dialog.ShowConfirm(mes, new DC());
                double value = await Devices.Multimeter.GetVoltage("DC", 500);
                Valid = Math.Abs(value - V) < relative ? true : false;
                mes = $"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {V - relative} до {V + relative} В.";
            }
            while (Valid is false);
            if (Devices.Generator.IsOpened()) await Devices.Generator.SetVoltage(0.5d);
        }
        protected async Task WaitForChangeRegisters(ushort reg, float FirstsValue, float coefreg)
        {
            int sec = 0;
            bool IsChange = false;
            while (IsChange is false && sec < 30)
            {
                IsChange = await Devices.Crate.ReadUInt16(reg) * coefreg == FirstsValue ? false : true;
                await Task.Delay(1000);
                sec++;
            }
            if (sec < 30) await Task.Delay(4000);
            if (IsChange is false) { throw new Exception("Значения обновляются слишком долго."); }
        }
        private void CountRelative(float value, float V, out float relative) => relative = V >= 1000 ? (value - V) / V * 100 : (value - V) / 1000 * 100; // (относительная/приведенная) погрешность

    }
}
