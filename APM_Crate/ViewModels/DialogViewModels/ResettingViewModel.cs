using APM_Crate.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public partial class ResettingViewModel :DialogViewModel
    {
        public ResettingViewModel()
        {
            switch (MainWindowViewModel.SettingViewModel.ItemPLC)
            {
                case "371" or "374" or "375": CanSettingChannel_3 = true; break;
                case "511": CanSettingChannel_3 = true; CanSettingChannel_4 = true; break;
                default: SettingChannel_3 = false; SettingChannel_4 = false; CanSettingChannel_3=false; CanSettingChannel_4 = false; break;
            }

        }
        public bool SettingChannel_1
        {
            get => SettingModel.Channel1.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel1.SettingChannel, value); }
        }
        public bool SettingChannel_2
        {
            get => SettingModel.Channel2.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel2.SettingChannel, value); }
        }
        public bool SettingChannel_3
        {
            get => SettingModel.Channel3.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel3.SettingChannel, value); }
        }
        public bool SettingChannel_4
        {
            get => SettingModel.Channel4.SettingChannel;
            set { this.RaiseAndSetIfChanged(ref SettingModel.Channel4.SettingChannel, value); }
        }
        public bool CanSettingChannel_1
        {
            get => SettingModel.Channel1.CanSetting;
            set
            {
                this.RaiseAndSetIfChanged(ref SettingModel.Channel1.CanSetting, value);
                SettingChannel_1 = value;
            }
        }
        
        public bool CanSettingChannel_2
        {
            get => SettingModel.Channel2.CanSetting;
            set
            {
                this.RaiseAndSetIfChanged(ref SettingModel.Channel2.CanSetting, value);
                SettingChannel_2 = value;
            }
        }
        
        public bool CanSettingChannel_3
        {
            get => SettingModel.Channel3.CanSetting;
            set
            {
                this.RaiseAndSetIfChanged(ref SettingModel.Channel3.CanSetting, value);
                SettingChannel_3 = value;
            }
        }
        
        public bool CanSettingChannel_4
        {
            get => SettingModel.Channel4.CanSetting;
            set
            {
                this.RaiseAndSetIfChanged(ref SettingModel.Channel4.CanSetting, value);
                SettingChannel_4 = value;
            }
        }
    }
}
