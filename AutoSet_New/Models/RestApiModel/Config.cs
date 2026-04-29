using AutoSet_New.ViewModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSet_New.Models.RestApiModel
{
    public class Config
    {
        public string? Id { get; set; } = null;
        public string Arm { get; set; } = RestModel.APM;
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
