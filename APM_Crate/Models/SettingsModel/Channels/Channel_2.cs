using APM_Crate.Models.DevicesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.Channels
{
    public class Channel_2 : Channel
    {
        public override string Num => "2";
        public override ushort Coef_ACC_A => (ushort)(60030 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_ACC_B => (ushort)(60032 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_A => (ushort)(60034 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_B => (ushort)(60036 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_A => (ushort)(60038 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_B => (ushort)(60040 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_A => (ushort)(60042 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_B => (ushort)(60044 + 90 * Crate.IndexSlotByBasket);
        public override ushort TypeTermo => (ushort)(60046 + 90 * Crate.IndexSlotByBasket);

        public override ushort PhysicalB0 => (ushort)(8043 + 100 * Crate.IndexSlotByBasket);
        public override ushort Physical => (ushort)(8045 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_A => (ushort)(8046 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_RMS => (ushort)(8047 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_PP => (ushort)(8048 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_A => (ushort)(8049 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_RMS => (ushort)(8050 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_PP => (ushort)(8051 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_A => (ushort)(8052 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_RMS => (ushort)(8053 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_PP => (ushort)(8054 + 100 * Crate.IndexSlotByBasket);
        public override ushort DC => (ushort)(8055 + 100 * Crate.IndexSlotByBasket);
        public override ushort T => (ushort)(8057 + 100 * Crate.IndexSlotByBasket);
        public override ushort ResistTermo => (ushort)(8061 + 100 * Crate.IndexSlotByBasket);

        public override ushort OnChannel => (ushort)(11256 + 600 * Crate.IndexSlotByBasket);
        public override ushort OnSaw => (ushort)(11310 + 600 * Crate.IndexSlotByBasket);
    }
    
}
