using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel
{
    public class Current_4_20 : Setting
    {
        protected override string Name { get; set; } = "Ток 4-20";

        protected float Coef_A {  get; set; }
        protected float Coef_B {  get; set; }

        protected override async Task Preparing()
        {
            await WP.Step(20, "Сборка схемы", () => Dialog.ShowBuild("4_20", $"Установите контакты для настройки тока 4-20 {Channel.Num}-го канала. " +
                    (Channel.Num) switch
                    {
                        "1" => "In-2 GND-3",
                        "2" => "In-5 GND-6",
                        "3" => "In-7 GND-8"
                    }));
            //$"In-2 GND-3 для 1 канала\r\n" +
            //$"In-5 GND-6 для 2 канала\r\n" +
            //$"In-7 GND-8 для 3 канала");
            await SourceOn();
        }
        protected override async Task CountCoefs()
        {
            await WP.Step(15,"Установка 4 мА",()=> Validation_mA(4, 0.02));
            await Task.Delay(2000);
            float I1 = await GetValue(ValueType.DC_Value);
            await WP.Step(15, "Установка 20 мА", () => Validation_mA(20, 0.02));
            await Task.Delay(2000);
            float I2 = await GetValue(ValueType.DC_Value);
            Coef_A = 16 / (I2 - I1);
            Coef_B = 4 - Coef_A * I1;
        }
        protected override async Task WriteCoefsAbstract()
        {
            await WriteCoefs(Coef_A, Coef_B,CoefType.Current);
        }
        protected override async Task CheckSettingAbstract()
        {
            WP.Report(0,"Проверка настроек тока");
            float I = await GetValue(ValueType.DC_Value);
            double mA = await Devices.Multimeter.GetAmperage();

            if ((I - mA) / mA * 100 > 1) throw new Exception("Канал тока 4-20 настроился не корректно");
            WP.Report(15,"Проверка настроек тока ✔");
        }


        protected override async Task ResetCoefs()
        {
            await Devices.Crate.WriteSwFloat(Channel.Coef_4_20_A, 1);
            await Devices.Crate.WriteSwFloat(Channel.Coef_4_20_B, 0);
        }
        protected override async Task WriteListValuesSetting()
        {
            float CoefA1 = await Devices.Crate.ReadSwFloat(Channel.Coef_4_20_A);
            float CoefB1 = await Devices.Crate.ReadSwFloat(Channel.Coef_4_20_B);

            WriteList("Ток 4-20", CoefA1, CoefB1);
        }




        protected async Task Validation_mA(double V, double relative)
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
    }
}
