using AutoSet_New.Models.DevicesModel;
using AutoSet_New.Models.RestApiModel;
using AutoSet_New.Service;
using AutoSet_New.ViewModels;
using AutoSet_New.ViewModels.DevicesViewModels;
using System.IO;
using System.Text.Json;

namespace AutoSet_New.Models
{
    public class PortsStorage
    {
        public static void Save()
        {
            var data = new SavePortsStateModel
            {
                AgilentViewModel = DevicesViewModel.Agilent,
                GeneratorViewModel = DevicesViewModel.Generator,
                //CNVViewModel = DevicesViewModel.CNV,
                CrateViewModel = DevicesViewModel.Crate,
                //SG004ViewModel = DevicesViewModel.SG004,
                //TIK_BISViewModel = DevicesViewModel.TIK_BIS,
                //MY210_402ViewModel = DevicesViewModel.MY210_402,
                RestAPI_IP = RestModel.IP
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {   
                WriteIndented = true
            });
            File.WriteAllText("Ports.json", json);
        }

        public static void Load()
        {
            if (!File.Exists("Ports.json"))
            {
                return;
            }
            var json = File.ReadAllText("Ports.json");
            var data = JsonSerializer.Deserialize<SavePortsStateModel>(json);

            if (data == null) return;

            DevicesViewModel.Generator = data.GeneratorViewModel ?? new();
            DevicesViewModel.Agilent = data.AgilentViewModel ?? new();
            DevicesViewModel.Crate = data.CrateViewModel ?? new();
            RestModel.IP = data.RestAPI_IP;
            //DevicesViewModel.SG004 = data.SG004ViewModel ?? new();
            //DevicesViewModel.TIK_BIS = data.TIK_BISViewModel ?? new();
            //DevicesViewModel.MY210_402 = data.MY210_402ViewModel ?? new();
            //DevicesViewModel.CNV = data.CNVViewModel ?? new();

        }
    }
}