using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public partial class UserSettingViewModel : DialogViewModel
    {
        public string Title { get; } = "Пользовательская настройка";

        private byte _Pot_1 = 0;
        public byte Pot_1
        {
            get { return _Pot_1; }
            set { this.RaiseAndSetIfChanged(ref _Pot_1, value); }
        }
        private byte _Pot_2 = 0;
        public byte Pot_2
        {
            get { return _Pot_2; }
            set { this.RaiseAndSetIfChanged(ref _Pot_2, value); }
        }
    }
}
