using AutoSet_New.Models.DevicesModel;
using AutoSet_New.Service;
using AutoSet_New.ViewModels.DevicesViewModels;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSet_New.Models
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
