using APM_Crate.Models.RestApiModel;
using APM_Crate.Service;
using Avalonia.Logging;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ClearLogCommand { get; set; }
        public ReactiveCommand<Unit, Unit> OpenIpRestCommand { get; set; }

        public static DevicesViewModel DevicesViewModel { get; } = new DevicesViewModel();
        public static SettingViewModel SettingViewModel { get; } = new SettingViewModel();
        public MainWindowViewModel()
        {
            ClearLogCommand = ReactiveCommand.Create(LogerViewModel.ClearLog);
            OpenIpRestCommand = ReactiveCommand.CreateFromTask(OpenIpRest);
            //ushort i = 0b0011_1111_1111_1110;
            //string s = Convert.ToString(i, 2);
            //s = new string(s.Reverse().ToArray());
            //s += string.Concat(Enumerable.Repeat("0", 16 - s.Length));
            //int inter = 0;
            //foreach (char c in s)
            //{
            //    inter++;
            //}
            //LogerViewModel.Write($"{i}");
        }
        public async Task OpenIpRest()
        {
            try
            {
                await Dialog.ShowRestAPI_IP();
            }
            catch (Exception ex)
            {
                LogerViewModel.Write($"❗ {ex.Message}");
            }
        }


        public LogerViewModel LogerViewModel { get; } = LogerViewModel.Instance;

        public static string Version
        {
            get
            {
                string version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                return version[..version.IndexOf('+')];
            }
        }
    }
}
