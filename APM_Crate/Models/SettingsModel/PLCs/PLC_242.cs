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
        public override async Task Setting()
        {
            await IEPE.Start(Channel1);
            await IEPE.Start(Channel2);
        }
    }
}
