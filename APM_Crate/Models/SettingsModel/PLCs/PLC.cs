using APM_Crate.Models.SettingsModel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.PLCs
{
    public abstract class PLC
    {
        public abstract Task SettingStart(WeightedProgress wp, WeightedProgress wp2);
        public Channel Channel1 { get; } = new Channel_1();
        public Channel Channel2 { get; } = new Channel_2();
        public Channel Channel3 { get; } = new Channel_3();
        public Channel Channel4 { get; } = new Channel_4();
    }
}
