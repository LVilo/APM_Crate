using AutoSet_New.Models;
using AutoSet_New.Models.RestApiModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSet_New.ViewModels.DialogViewModels
{
    public class RestAPIViewModel : DialogViewModel
    {

        public string IP
        {
            get => RestModel.IP;
            set { this.RaiseAndSetIfChanged(ref RestModel.IP, value); }
        }
    }
}
