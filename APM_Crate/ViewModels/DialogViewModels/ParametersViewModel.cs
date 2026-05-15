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

        private bool _Coef;
        public bool Coef
        {
            get => _Coef;
            set { this.RaiseAndSetIfChanged(ref _Coef, value); }
        }
        private bool _Freq_Is_79_6;
        public bool Freq_Is_79_6
        {
            get => _Freq_Is_79_6;
            set { this.RaiseAndSetIfChanged(ref _Freq_Is_79_6, value); }
        }
        private string _SerialNumber;
        public string SerialNumber
        {
            get => _SerialNumber;
            set { this.RaiseAndSetIfChanged(ref _SerialNumber, value); }
        }

        public ParametersViewModel()
        {
            Coef = Setting.Coef_Is_10;
            Freq_Is_79_6 = Setting.Freq_Is_79_6;
            SerialNumber = SettingViewModel.SerialNumber.ToString();
        }
        protected override bool MethodAfterClickConfirm()
        {
            SettingViewModel.SerialNumber = Convert.ToUInt16(SerialNumber);
            Setting.Freq_Is_79_6 = Freq_Is_79_6;
            Setting.Coef_Is_10 = Coef;
            return true;
        }
    }
}
