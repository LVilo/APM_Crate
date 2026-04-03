using APM_Crate.Models.DevicesModel;
using APM_Crate.ViewModels;
using APM_Crate.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Service
{
    public static class Dialog
    {
        public static DialogService DialogService;
        public static async Task ShowBuild(string settings,string mes="Соберите схему.")
        {
            var vm = new BuildSchemeViewModel();
            vm.Messege = mes;
            vm.SetBitmap($"avares://APM_Crate/Assets/{settings}.png");
            await DialogService.ShowBuildAsync(vm);
        }
        public static async Task ShowParam()
        {
            var vm = new ParamDialogViewModel();
            await DialogService.ShowParamAsync(vm);
        }
        public static async Task ShowLiveCharts()
        {
            var vm = new LiveChartsViewModel();
            await DialogService.ShowLiveChartsAsync(vm);
        }
        public static async Task ShowConfirm(string mes, bool YesOrNot = false)
        {
            var vm = new ConfirmDialogViewModel();
            if (YesOrNot)
            {
                vm.ConfirmText = "Да";
                vm.CancelText = "Нет";
            }
            else
            {
                vm.ConfirmText = "Ок";
                vm.CancelText = "Отмена";
            }
            vm.Messege = mes;
            await DialogService.ShowConfirmAsync(vm);
        }
    }
}

