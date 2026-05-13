using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.PLCs
{
    public class PLC_511 :PLC
    {
        private Setting U { get; } = new U();
        public override async Task SettingStart(WeightedProgress wp, WeightedProgress wp2)
        {
            await wp.Step(15, "Настройка U, Канала 1.", () => U.Start(Channel1, wp2));
            await wp.Step(15, "Настройка U, Канала 2.", () => U.Start(Channel2, wp2));
            await wp.Step(15, "Настройка U, Канала 3.", () => U.Start(Channel3, wp2));
            await wp.Step(15, "Настройка U, Канала 4.", () => U.Start(Channel4, wp2));
        }
    }
}
