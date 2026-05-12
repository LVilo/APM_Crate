using APM_Crate.Models.DevicesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.Channels
{
    public class Channel_1 : Channel
    {
        public override string Num => "1";
        public override ushort Coef_ACC_A => (ushort)(60000 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_ACC_B => (ushort)(60002 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_A => (ushort)(60004 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_B => (ushort)(60006 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_A => (ushort)(60008 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_B => (ushort)(60010 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_A => (ushort)(60012 + 90 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_B => (ushort)(60014 + 90 * Crate.IndexSlotByBasket);
        public override ushort TypeTermo => (ushort)(60016 + 90 * Crate.IndexSlotByBasket);

        public override ushort PhysicalB0 => (ushort)(8018 + 100 * Crate.IndexSlotByBasket);
        public override ushort Physical => (ushort)(8020 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_A => (ushort)(8021 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_RMS => (ushort)(8022 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_PP => (ushort)(8023 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_A => (ushort)(8024 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_RMS => (ushort)(8025 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_PP => (ushort)(8026 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_A => (ushort)(8027 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_RMS => (ushort)(8028 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_PP => (ushort)(8029 + 100 * Crate.IndexSlotByBasket);
        public override ushort DC => (ushort)(8030 + 100 * Crate.IndexSlotByBasket);
        public override ushort T => (ushort)(8032 + 100 * Crate.IndexSlotByBasket);
        public override ushort ResistTermo => (ushort)(8036 + 100 * Crate.IndexSlotByBasket);

        public override ushort OnChannel => (ushort)(11056 + 600 * Crate.IndexSlotByBasket);
        public override ushort OnSaw => (ushort)(11110 + 600 * Crate.IndexSlotByBasket);
    }
    
}
