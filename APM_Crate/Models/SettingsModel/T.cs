using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models.SettingsModel
{
    public class T : Setting
    {
        protected override string Name { get; set; } = "Температура";

        protected float Coef_A {  get; set; }
        protected float Coef_B {  get; set; }

        protected float Resist_1 { get; private set; }
        protected float Resist_2 { get; private set; }
        protected float Need_T_1 { get; private set; }
        protected float Need_T_2 { get; private set; }

        protected override async Task Preparing()
        {
            await Dialog.ShowBuild("T", $"Установите контакты для настройки\r\n термопреобразователя {Channel.Num}-го канала.\r\n" +
                    $"In+-9 In--8 GND-7");
            await Dialog.ShowParam();
            float Readed_T = 0f;
            ushort typetermo = (ushort)(TermoTypes.IndexOf(TermoType) + 1);
            SetTermoValues();
            await Devices.Crate.WriteUInt16(Channel.TypeTermo, typetermo);
        }
        protected override async Task CountCoefs()
        {
            await Dialog.ShowConfirm($"Установите на магазине сопротивлений 100 Ом", new Delay());
            await Task.Delay(15000); // стоит потому что долго обновляется значение в крейте
            float T1 = await GetValue(ValueType.ResistT);
            await Dialog.ShowConfirm($"Установите на магазине сопротивлений 400 Ом", new Delay());
            await Task.Delay(25000); // стоит потому что долго обновляется значение в крейте
            float T2 = await GetValue(ValueType.ResistT);
            float A = 3 / (T2 - T1);
            float B = 1 - A * T1;
        }
        protected override async Task WriteCoefsAbstract()
        {
            await WriteCoefs(Coef_A, Coef_B, CoefType.T);
        }
        protected override async Task CheckSettingAbstract()
        {
            WP.Report(0, "Проверка настроек температуры");
            await Dialog.ShowConfirm($"Установите на магазине сопротивлений {Resist_1} Ом", new Delay());
            await Task.Delay(15000);// стоит потому что долго обновляется значение в крейте
            float Readed_T = await GetValue(ValueType.T);
            float relative = Math.Abs(Readed_T - Need_T_1);
            if (relative > 1) throw new Exception($"Точка 1 не прошла проверку, значение отклонено от нормы на {relative}");

            await Dialog.ShowConfirm($"Установите на магазине сопротивлений {Resist_2} Ом", new Delay());
            await Task.Delay(15000);// стоит потому что долго обновляется значение в крейте
            Readed_T = await GetValue(ValueType.T);
            relative = Math.Abs(Readed_T - Need_T_2);
            if (relative > 1) throw new Exception($"Точка 2 не прошла проверку, значение отклонено от нормы на {relative}");
            WP.Report(2,"Проверка настроек температуры ✔");
        }


        protected override async Task ResetCoefs()
        {
            await Devices.Crate.WriteSwFloat(Channel.Coef_T_A, 1);
            await Devices.Crate.WriteSwFloat(Channel.Coef_T_B, 0);
        }
        protected override async Task WriteListValuesSetting()
        {
            float CoefA1 = await Devices.Crate.ReadSwFloat(Channel.Coef_T_A);
            float CoefB1 = await Devices.Crate.ReadSwFloat(Channel.Coef_T_B);

            WriteList("Температура", CoefA1, CoefB1);
        }








        private void SetTermoValues()
        {
            //Resist_1 = 78.7f;
            //Resist_2 = 185.2f;
            //Need_T_1 = -50;
            //Need_T_2 = 200;
            // Взято с https://cdn.termexlab.ru/files/9e48a99f/9525/4985/9662/23c12e3a32c2.pdf
            switch (TermoTypes.IndexOf(TermoType))
            {
                case 0:
                    Resist_1 = 10.265f;
                    Resist_2 = 92.8f;
                    Need_T_1 = -180;
                    Need_T_2 = 200;
                    break;
                case 1:
                    Resist_1 = 20.53f;
                    Resist_2 = 185.6f;
                    Need_T_1 = -180;
                    Need_T_2 = 200;
                    break;
                case 2:
                    Resist_1 = 39.35f;
                    Resist_2 = 92.6f;
                    Need_T_1 = -50;
                    Need_T_2 = 200;
                    break;
                case 3:
                    Resist_1 = 78.7f;
                    Resist_2 = 185.2f;
                    Need_T_1 = -50;
                    Need_T_2 = 200;
                    break;
                case 4:
                    Resist_1 = 8.62f;
                    Resist_2 = 197.58f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 5:
                    Resist_1 = 17.24f;
                    Resist_2 = 389.19f;// было 395.16f
                    Need_T_1 = -200;
                    Need_T_2 = 830;// было 850
                    break;
                case 6:
                    Resist_1 = 9.26f;
                    Resist_2 = 195.24f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 7:
                    Resist_1 = 18.52f;
                    Resist_2 = 387.55f;// было 390.48f
                    Need_T_1 = -200;
                    Need_T_2 = 840;// было 850
                    break;
            }
        }
    }
}
