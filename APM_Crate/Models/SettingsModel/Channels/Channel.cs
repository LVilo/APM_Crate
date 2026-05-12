using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.Channels
{
    public abstract class Channel
    {
        public bool SettingChannel = true;
        public bool CanSetting = true;

        public abstract string Num { get; }
        public abstract ushort Coef_ACC_A { get; }
        public abstract ushort Coef_ACC_B { get; }

        public abstract ushort Coef_Speed_A { get; }
        public abstract ushort Coef_Speed_B { get; }
        public abstract ushort Coef_4_20_A { get; }
        public abstract ushort Coef_4_20_B { get; }
        public abstract ushort Coef_T_A { get; }
        public abstract ushort Coef_T_B { get; }
        public abstract ushort TypeTermo { get; }

        public abstract ushort PhysicalB0 { get; }
        public abstract ushort Physical { get; }
        //Значения прочтенные с этих регистров делить на 100 ↓
        public abstract ushort ACC_A { get; }
        public abstract ushort ACC_RMS { get; }
        public abstract ushort ACC_PP { get; }
        public abstract ushort Speed_A { get; }
        public abstract ushort Speed_RMS { get; }
        public abstract ushort Speed_PP { get; }
        //делить на 10↓
        public abstract ushort Move_A { get; }
        public abstract ushort Move_RMS { get; }
        public abstract ushort Move_PP { get; }
        public abstract ushort DC { get; }
        public abstract ushort T { get; }
        //↑
        public abstract ushort ResistTermo { get; }
        public abstract ushort OnChannel { get; }
        public abstract ushort OnSaw { get; }
    }
}
