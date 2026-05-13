using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.PLCs
{
    public class PLC_243 : PLC
    {
        private Setting Current { get; } = new Current_4_20();
        public override async Task SettingStart(WeightedProgress wp, WeightedProgress wp2)
        {
            await wp.Step(30, "Настройка Тока 4-20, Канала 1.", () => Current.Start(Channel1, wp2));
            await wp.Step(30, "Настройка Тока 4-20, Канала 2.", () => Current.Start(Channel2, wp2));
        }
    }
}
