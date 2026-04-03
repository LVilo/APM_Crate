using Avalonia.Media.Imaging;
using Avalonia.Platform;


using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public partial class BuildSchemeViewModel : DialogViewModel
    {
        private string _title = "Схема";
        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }
        private string _messege = "Соберите схему";
        public string Messege
        {
            get { return _messege; }
            set { this.RaiseAndSetIfChanged(ref _messege, value); }
        }
        private string _confirmText = "ОК";
        public string ConfirmText
        {
            get { return _confirmText; }
            set { this.RaiseAndSetIfChanged(ref _confirmText, value); }
        }
        private string _cancelText = "Отмена";
        public string CancelText
        {
            get { return _cancelText; }
            set { this.RaiseAndSetIfChanged(ref _cancelText, value); }
        }

        private Bitmap pathPNG;
        public Bitmap PathPNG
        {
            get { return pathPNG; }
            set { this.RaiseAndSetIfChanged(ref pathPNG, value); }
        }
        private string _pathfile = "avares://APM_Crate/Assets/build.png";
        public string Pathfile
        {
            get { return _pathfile; }
            set { this.RaiseAndSetIfChanged(ref _pathfile, value); }
        }

        //[ObservableProperty] private int _width = 660;
        //[ObservableProperty] private int _height = 930;

        public void SetBitmap(string path)
        {
            var uri = new Uri(path);
            var asset = AssetLoader.Open(uri);
            PathPNG = new Bitmap(asset);
        }

    }
}
