using APM_Crate.Models;
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
            get => SettingModel.Coef_Is_10;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Coef_Is_10, value); }
        }
        public bool Freq_Is_79_6
        {
            get => SettingModel.Freq_Is_79_6;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Freq_Is_79_6, value); }
        }
    }
}
