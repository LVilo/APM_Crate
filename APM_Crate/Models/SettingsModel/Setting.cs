using APM_Crate.Models.RestApiModel;
using APM_Crate.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APM_Crate.Models.SettingModel;
using static APM_Crate.Models.SettingModel.Channel;

namespace APM_Crate.Models.SettingsModel
{
    public abstract class Setting
    {
        protected abstract string Name { get; set; }
        protected Channel Channel { get; set; }
        protected abstract Task Preparing();
        protected abstract Task CountCoefs();
        protected abstract Task WriteCoefs();
        protected abstract Task CheckSetting();
        protected abstract Task LastMethod();
        public async Task Start(Channel channel)
        {
            Channel = channel;
            await LogerViewModel.Instance.Write($"Настройка {Name}, канал {Channel.Num}");
            await Preparing();
            await CountCoefs();
            await WriteCoefs();
            await CheckSetting();
            await LastMethod();
            await Task.Run(() => { });
        }
        protected abstract Task CheckSettingFlag();
        protected abstract Task Reset();
        public void WriteList(string str, float A, float B)
        {
            settings.Add(new Settings { Name = $"Канал {Channel.Num}.{str} Коэффициент А", Value = A.ToString() });
            settings.Add(new Settings { Name = $"Канал {Channel.Num}.{str} Коэффициент B", Value = B.ToString() });
        }
    }
}
