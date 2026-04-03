using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models
{
    public class ParametersSettingPLC
    {
        public int SettingId { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string Setting { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DeviceName { get; set; }
        public string TimeSetting { get; set; }
        public int SerialNumber { get; set; }
        public string OrderNumber { get; set; }

        // Каналы 1-4
        public string Channel1_Coef_acc_A { get; set; }
        public string Channel1_Coef_acc_B { get; set; }
        public string Channel1_Coef_speed_A { get; set; }
        public string Channel1_Coef_speed_B { get; set; }
        public string Channel1_Coef_4_20_A { get; set; }
        public string Channel1_Coef_4_20_B { get; set; }
        public string Channel1_Coef_T_A { get; set; }
        public string Channel1_Coef_T_B { get; set; }
        public string Channel1_T_Type { get; set; }

        public string Channel2_Coef_acc_A { get; set; }
        public string Channel2_Coef_acc_B { get; set; }
        public string Channel2_Coef_speed_A { get; set; }
        public string Channel2_Coef_speed_B { get; set; }
        public string Channel2_Coef_4_20_A { get; set; }
        public string Channel2_Coef_4_20_B { get; set; }
        public string Channel2_Coef_T_A { get; set; }
        public string Channel2_Coef_T_B { get; set; }
        public string Channel2_T_Type { get; set; }

        public string Channel3_Coef_acc_A { get; set; }
        public string Channel3_Coef_acc_B { get; set; }
        public string Channel3_Coef_speed_A { get; set; }
        public string Channel3_Coef_speed_B { get; set; }
        public string Channel3_Coef_4_20_A { get; set; }
        public string Channel3_Coef_4_20_B { get; set; }
        public string Channel3_Coef_T_A { get; set; }
        public string Channel3_Coef_T_B { get; set; }
        public string Channel3_T_Type { get; set; }

        public string Channel4_Coef_acc_A { get; set; }
        public string Channel4_Coef_acc_B { get; set; }
        public string Channel4_Coef_speed_A { get; set; }
        public string Channel4_Coef_speed_B { get; set; }
        public string Channel4_Coef_4_20_A { get; set; }
        public string Channel4_Coef_4_20_B { get; set; }
        public string Channel4_Coef_T_A { get; set; }
        public string Channel4_Coef_T_B { get; set; }
        public string Channel4_T_Type { get; set; }
    }
}
