using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using APM_Crate.Models.SettingsModel;
using APM_Crate.Service;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public partial class ParamDialogViewModel : DialogViewModel
    {
        public ParamDialogViewModel()
        {

        }

        public static ObservableCollection<string> ItemsSource => Setting.TermoTypes;
        public string ItemSelected
        {
            get => Setting.TermoType;
            set
            {
                this.RaiseAndSetIfChanged(ref Setting.TermoType, value);
            }
        }

        public string _TextBorder = "Выберите тип датчика температуры.";
        public string TextBorder
        {
            get { return _TextBorder; }
            set { this.RaiseAndSetIfChanged(ref _TextBorder, value); }
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

        protected override bool MethodAfterClickConfirm()
        {
           
            return true; 
        }

    }
}
