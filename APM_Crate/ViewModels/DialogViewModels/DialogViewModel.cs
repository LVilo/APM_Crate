using APM_Crate.Models.SettingsModel;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        protected TaskCompletionSource<bool?> _closeTask =
            new TaskCompletionSource<bool?>();

        public Task<bool?> WaitAsync() => _closeTask.Task;

        public Action<bool>? CloseAction { get; set; }

        public bool? Confirmed { get; set; }
        // Команды

        public ReactiveCommand<Unit, Unit> ConfirmCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public DialogViewModel()
        {
            ConfirmCommand = ReactiveCommand.CreateFromTask(Confirm);
            CancelCommand = ReactiveCommand.CreateFromTask(Cancel);
        }
        protected virtual bool MethodAfterClickConfirm()
        {
            return true;
        }
        public async Task Close(bool result)
        {
            Confirmed = result;
            _closeTask.TrySetResult(result);

            CloseAction?.Invoke(result);
        }
        public async Task Confirm()
        {
            //было создано для валидации значений в ParamCNVDialogViewModel
            if (MethodAfterClickConfirm() is true) await Close(true);
        }
        public async Task Cancel()
        {
            await Close(false);
        }
    }
}