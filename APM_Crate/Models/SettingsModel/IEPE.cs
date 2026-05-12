using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using APM_Crate.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APM_Crate.Models.SettingModel.Channel;

namespace APM_Crate.Models.SettingsModel
{
    public class IEPE : Setting
    {
        protected override string Name { get; set; } = "IEPE";
        protected override async Task Preparing()
        {
            if (Channel.SettingChannel is false)
            {
                float CoefA1 = await Devices.Crate.ReadSwFloat(Channel.Coef_ACC_A);
                float CoefB1 = await Devices.Crate.ReadSwFloat(Channel.Coef_ACC_B);

                float CoefA2 = await Devices.Crate.ReadSwFloat(Channel.Coef_Speed_A);
                float CoefB2 = await Devices.Crate.ReadSwFloat(Channel.Coef_Speed_B);

                WriteList("Ускорение", CoefA1, CoefB1);
                WriteList("Скорость", CoefA2, CoefB2);
                return;
            }
            await Devices.Generator.SetFrequency(Frequency);
            await LogerViewModel.Instance.Write($"Настройка IEPE, канал {Channel.Num}");
            await Dialog.ShowBuild("IEPE", $"Установите контакты для настройки IEPE {Channel.Num}-го канала.\r\n" +
                (Channel.Num) switch
                {
                    "1" => "In-2 GND-3",
                    "2" => "In-5 GND-6",
                });
            //$"In-2 GND-3 для 1 канала\r\n" +
            //$"In-5 GND-6 для 2 канала");

            await SourceOn();
            await Reset();
            await ValidationVoltage(DC_Value, 0.5d);
        }
        protected override async Task CountCoefs()
        {

        }
        protected override async Task WriteCoefs()
        {

        }
        protected override async Task CheckSetting()
        {

        }
        protected override async Task LastMethod()
        {

        }
        protected override async Task Reset()
        {

        }
    }
}
