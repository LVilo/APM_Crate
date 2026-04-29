using System;
using System.Threading.Tasks;

namespace AutoSet_New.ViewModels.DialogViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        protected TaskCompletionSource<bool?> _closeTask =
            new TaskCompletionSource<bool?>();

        public Task<bool?> WaitAsync() => _closeTask.Task;

        public Action<bool>? CloseAction { get; set; }

        private bool? _confirmed;
        public bool? Confirmed
        {
            get => _confirmed;
            set => this.RaiseAndSetIfChanged(ref _confirmed, value);
        }

        // Команды

        public ReactiveCommand<Unit, Unit> ConfirmCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public DialogViewModel()
        {
            ConfirmCommand = ReactiveCommand.Create(Confirm);
            CancelCommand = ReactiveCommand.Create(Cancel);
        }
        protected virtual bool MethodAfterClickConfirm()
        {
            return true;
        }
        public void Close(bool result)
        {
            Confirmed = result;
            _closeTask.TrySetResult(result);

            CloseAction?.Invoke(result);
        }
        public void Confirm()
        {
            //было создано для валидации значений в ParamCNVDialogViewModel
            if (MethodAfterClickConfirm() is true) Close(true);
        }
        public void Cancel()
        {
             Close(false);
        }
    }
}