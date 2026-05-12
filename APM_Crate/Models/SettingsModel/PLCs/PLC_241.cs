using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.PLCs
{
    public class PLC_241 : PLC
    {
        private Setting IEPE { get; } = new IEPE();
        private Setting Current { get; } = new Current_4_20();
        public override async Task Setting()
        {
            await IEPE.Start(Channel1);
            await Current.Start(Channel2);
        }
    }
}
