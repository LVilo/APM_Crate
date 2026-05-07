using APM_Crate.ViewModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APM_Crate.Models.DevicesModel.Crate;

namespace APM_Crate.Models.RestApiModel
{
    public class Config
    {
        public string? Id { get; set; } = null;
        public string Arm { get; set; } = RestModel.APM;

        public string UserName { get; set; } = Environment.UserName;
        public string Date { get; set; } = String.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
        public string DeviceFamily { get; set; } = RestModel.DeviceFamily;
        public string DeviceType { get; set; }
        public ulong SerialNumber { get; set; }
        public string OrderNumber { get; set; }
        public bool IsActual { get; set; } = true;
        public List<Settings> Settings { get; set; }
    }
    public class Settings
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
