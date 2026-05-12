using APM_Crate.Models.DevicesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.Channels
{
    public class Channel_3 : Channel
    {
        public override string Num => "3";
        public override ushort Coef_ACC_A => (ushort)(60060 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_ACC_B => (ushort)(60062 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_A => (ushort)(60064 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_B => (ushort)(60066 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_A => (ushort)(60068 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_B => (ushort)(60070 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_A => (ushort)(60072 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_B => (ushort)(60074 + 90 * Crate.IndexSlotByBasket);
        public override ushort TypeTermo => (ushort)(60076 + 90 * Crate.IndexSlotByBasket);

        public override ushort PhysicalB0 => (ushort)(8068 + 100 * Crate.IndexSlotByBasket);
        public override ushort Physical => (ushort)(8070 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_A => (ushort)(8071 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_RMS => (ushort)(8072 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_PP => (ushort)(8073 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_A => (ushort)(8074 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_RMS => (ushort)(8075 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_PP => (ushort)(8076 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_A => (ushort)(8077 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_RMS => (ushort)(8078 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_PP => (ushort)(8079 + 100 * Crate.IndexSlotByBasket);
        public override ushort DC => (ushort)(8080 + 100 * Crate.IndexSlotByBasket);
        public override ushort T => (ushort)(8082 + 100 * Crate.IndexSlotByBasket);

        public override ushort ResistTermo => (ushort)(8086 + 100 * Crate.IndexSlotByBasket);

        public override ushort OnChannel => (ushort)(11456 + 600 * Crate.IndexSlotByBasket);
        public override ushort OnSaw => (ushort)(11510 + 600 * Crate.IndexSlotByBasket);
    }
    
}
