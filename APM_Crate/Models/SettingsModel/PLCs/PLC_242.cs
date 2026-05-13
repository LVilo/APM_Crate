using APM_Crate.Models.DevicesModel;
using APM_Crate.Models.SettingsModel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.PLCs
{
    public class PLC_242 : PLC
    {
        private Setting IEPE { get; } = new IEPE();
        public override async Task SettingStart(WeightedProgress wp, WeightedProgress wp2)
        {
            await wp.Step(20, "Настройка IEPE, Канала 1.", ()=> IEPE.Start(Channel1, wp2));
            await wp.Step(20, "Настройка IEPE, Канала 2.", () => IEPE.Start(Channel2, wp2));
            await Devices.Crate.WriteSwFloat(Crate.Registers.Coefficient, Setting.Coef);
            await wp.Step(5, $"Проверка выборок устройства", () => CheckFilePLC.Start(2));

        }
    }
}
