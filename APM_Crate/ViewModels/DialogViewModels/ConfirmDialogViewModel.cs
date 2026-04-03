

using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public partial class ConfirmDialogViewModel : DialogViewModel
    {
        private string _title = "Настройка";
        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }
        private string _messege = "Текст";
        public string Messege
        {
            get { return _messege; }
            set { this.RaiseAndSetIfChanged(ref _messege, value); }
        }
        private string _cancelText = "Отмена";
        public string CancelText
        {
            get { return _cancelText; }
            set { this.RaiseAndSetIfChanged(ref _cancelText, value); }
        }
        private string _confirmText = "ОК";
        public string ConfirmText
        {
            get { return _confirmText; }
            set { this.RaiseAndSetIfChanged(ref _confirmText, value); }
        }
        private string _SkipText = "Пропустить";
        public string SkipText
        {
            get { return _SkipText; }
            set { this.RaiseAndSetIfChanged(ref _SkipText, value); }
        }
        private string _icontext = "\xe3e8";
        public string Icontext
        {
            get { return _icontext; }
            set { this.RaiseAndSetIfChanged(ref _icontext, value); }
        }

    }
}
