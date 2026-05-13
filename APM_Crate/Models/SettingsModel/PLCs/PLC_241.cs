using APM_Crate.Models.DevicesModel;
using APM_Crate.ViewModels;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.PLCs
{
    public class PLC_241 : PLC
    {
        private Setting IEPE { get; } = new IEPE();
        private Setting Current { get; } = new Current_4_20();
        public override async Task SettingStart(WeightedProgress wp,WeightedProgress wp2)
        {
            await wp.Step(30,"Настройка IEPE, Канала 1.",()=>IEPE.Start(Channel1, wp2));
            await wp.Step(30, "Настройка Тока 4-20, Канала 2.", ()=> Current.Start(Channel2, wp2));
            await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Setting.Coef);

            await wp.Step(5, $"Проверка выборок устройства", () => CheckFilePLC.Start(1));

        }
    }
}
