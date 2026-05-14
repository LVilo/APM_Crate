using APM_Crate.Models;
using APM_Crate.Models.SettingsModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public class ParametersViewModel : DialogViewModel
    {
        public bool Coef
        {
            get => Setting.Coef_Is_10;
            set { this.RaiseAndSetIfChanged(ref Setting.Coef_Is_10, value); }
        }
        public bool Freq_Is_79_6
        {
            get => Setting.Freq_Is_79_6;
            set { this.RaiseAndSetIfChanged(ref Setting.Freq_Is_79_6, value); }
        }
        public string SerialNumber
        {
            get => Setting.SerialNumber;
            set { this.RaiseAndSetIfChanged(ref Setting.SerialNumber, value); }
        }
    }
}
