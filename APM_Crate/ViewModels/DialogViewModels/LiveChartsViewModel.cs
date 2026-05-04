using APM_Crate.Models;
using Controls.LineChart;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public class LiveChartsViewModel : DialogViewModel
    {
        private ObservableCollection<ChartPoint> _Points_Chanel1 = new();
        public ObservableCollection<ChartPoint> Points_Chanel1 { get => _Points_Chanel1; set { this.RaiseAndSetIfChanged(ref _Points_Chanel1, value); } }

        private ObservableCollection<ChartPoint> _Points_Chanel2 = new();
        public ObservableCollection<ChartPoint> Points_Chanel2 { get => _Points_Chanel2; set { this.RaiseAndSetIfChanged(ref _Points_Chanel2, value); } }

        private string _TextGrid_1 = "Канал 1";
        public string TextGrid_1
        {
            get => _TextGrid_1;
            set { this.RaiseAndSetIfChanged(ref _TextGrid_1, value); }
        }
        private string _TextGrid_2 = "Канал 2";
        public string TextGrid_2
        {
            get => _TextGrid_2;
            set { this.RaiseAndSetIfChanged(ref _TextGrid_2, value); }
        }
    }
}
