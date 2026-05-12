using APM_Crate.Models.DevicesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel.Channels
{
    public class Channel_4 : Channel
    {
        public override string Num => "4";
        public override ushort Coef_ACC_A => (ushort)(61260 + 30 * Crate.IndexSlotByBasket);
        public override ushort Coef_ACC_B => (ushort)(61262 + 30 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_A => (ushort)(61264 + 30 * Crate.IndexSlotByBasket);
        public override ushort Coef_Speed_B => (ushort)(61266 + 30 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_A => (ushort)(61268 + 30 * Crate.IndexSlotByBasket);
        public override ushort Coef_4_20_B => (ushort)(61270 + 30 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_A => (ushort)(61272 + 30 * Crate.IndexSlotByBasket);
        public override ushort Coef_T_B => (ushort)(61274 + 30 * Crate.IndexSlotByBasket);
        public override ushort TypeTermo => (ushort)(61276 + 30 * Crate.IndexSlotByBasket);

        public override ushort PhysicalB0 => (ushort)(9406 + 100 * Crate.IndexSlotByBasket);
        public override ushort Physical => (ushort)(9408 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_A => (ushort)(9409 + 100 * Crate.IndexSlotByBasket);
        public override ushort ACC_RMS => (ushort)(9410 + 25 * Crate.IndexSlotByBasket);
        public override ushort ACC_PP => (ushort)(9411 + 100 * Crate.IndexSlotByBasket);
        #region Резерв
        public override ushort Speed_A => (ushort)(9412 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_RMS => (ushort)(9413 + 100 * Crate.IndexSlotByBasket);
        public override ushort Speed_PP => (ushort)(9414 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_A => (ushort)(9415 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_RMS => (ushort)(9416 + 100 * Crate.IndexSlotByBasket);
        public override ushort Move_PP => (ushort)(9417 + 100 * Crate.IndexSlotByBasket);
        #endregion
        public override ushort DC => (ushort)(9418 + 100 * Crate.IndexSlotByBasket);
        public override ushort T => (ushort)(9420 + 100 * Crate.IndexSlotByBasket);
        public override ushort ResistTermo => (ushort)(9424 + 100 * Crate.IndexSlotByBasket);
        public override ushort OnChannel => 0;
        public override ushort OnSaw => (ushort)(19510 + 600 * Crate.IndexSlotByBasket);
    }
}
