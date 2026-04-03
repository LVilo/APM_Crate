using APM_Crate.Models;
using APM_Crate.Models.DevicesModel;
using APM_Crate.ViewModels.DevicesViewModels;
using APM_Crate.Views.DevicesViews;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows.Input;
namespace APM_Crate.ViewModels
{
    public partial class DevicesViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> UpdatesPortsCommand { get; set; }

        //public class ViewItem
        //{
        //    public string Name { get; set; }
        //    public UserControl View { get; set; }
        //    public DevicesContext ViewModel { get; set; }
        //}

        public static AgilentViewModel Agilent { get; set; } = new AgilentViewModel();
        //public static CNVViewModel CNV { get; set; } = new CNVViewModel();
        public static GeneratorViewModel Generator { get; set; } = new GeneratorViewModel();
        public static CrateViewModel Crate { get; set; } = new CrateViewModel();
        public static PrinterViewModel Printer { get; set; } = new PrinterViewModel();
        //public static SG004ViewModel SG004 { get; set; } = new SG004ViewModel();
        //public static TIK_BISViewModel TIK_BIS { get; set; } = new TIK_BISViewModel();
        //public static MY210_402ViewModel MY210_402 { get; set; } = new MY210_402ViewModel();

        //public static List<ViewItem> Views { get; } = new()
        //{
        //    new ViewItem { Name="Генератор", ViewModel=Generator,View = new GeneratorView() },
        //    new ViewItem { Name="Мультиметр", ViewModel=Agilent, View = new AgilentView() },
        //    new ViewItem { Name="Калибратор", ViewModel=SG004, View = new SG004View() },
        //    new ViewItem { Name="Преобразователь", ViewModel=TIK_BIS, View = new TIK_BISView() },
        //    new ViewItem { Name="Реле", ViewModel=MY210_402, View = new MY210_402View() }
        //};
        //private ViewItem _SelectedView = Views[0];

        //public ViewItem SelectedView
        //{
        //    get { return _SelectedView; }
        //    set { this.RaiseAndSetIfChanged(ref _SelectedView, value); }
        //}


        public DevicesViewModel()
        {
            UpdatesPortsCommand = ReactiveCommand.CreateFromTask(UpdatesPorts);
            PortsStorage.Load();
            UpdatesPorts();

            //foreach (ViewItem view in Views)
            //{
            //    view.View.DataContext = view.ViewModel;
            //}

        }
        public async Task UpdatesPorts()
        {
            try
            {
                await Task.Run(async () =>
                {
                    string[] ports = Devices.GetAllPorts();
                    //foreach (ViewItem view in Views)
                    //{
                    //    view.ViewModel.Ports = ports;
                    //}
                    Agilent.Ports = ports;
                    Generator.Ports = ports;
                    Crate.Ports = ports;
                    await Printer.UpdatePrinters();
                    //SG004.Ports = ports;
                    //TIK_BIS.Ports = ports;
                    //MY210_402.Ports = ports;
                });
            }
            catch (Exception ex)
            {
                LogerViewModel.Instance.Write(ex.Message);
            }
        }

    }
}

