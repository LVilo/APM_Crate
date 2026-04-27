using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using APM_Crate.ViewModels.DevicesViewModels;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models
{
    public class  SavePortsStateModel
    {
        public AgilentViewModel AgilentViewModel { get; set; }
        public GeneratorViewModel GeneratorViewModel { get; set; }
        public CrateViewModel CrateViewModel { get; set; }
        public string RestAPI_IP {  get; set; }
        //public SG004ViewModel SG004ViewModel { get; set; }
        //public TIK_BISViewModel TIK_BISViewModel { get; set; }
        //public MY210_402ViewModel MY210_402ViewModel { get; set; }
        //public CNVViewModel CNVViewModel { get; set; }
    }
}
