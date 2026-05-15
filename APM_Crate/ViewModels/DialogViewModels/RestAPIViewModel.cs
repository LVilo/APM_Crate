using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using APM_Crate.Models.RestApiModel;
using APM_Crate.Models.SettingsModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public class RestAPIViewModel : DialogViewModel
    {

        private string _IP;
        public string IP
        {
            get => _IP;
            set { this.RaiseAndSetIfChanged(ref _IP, value); }
        }
        public RestAPIViewModel()
        {
            IP = RestModel.IP;
        }
        protected override bool MethodAfterClickConfirm()
        {
            RestModel.IP = IP;
            return true;
        }
    }
}
