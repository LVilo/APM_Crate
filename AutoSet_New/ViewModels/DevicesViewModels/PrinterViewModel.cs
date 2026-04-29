using AutoSet_New.Models.DevicesModel;
using PortsWork;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoSet_New.ViewModels.DevicesViewModels
{
    public class PrinterViewModel : ViewModelBase
    {
        public PrinterViewModel()
        {

        }
        public string[]? Printers
        {
            get => Devices.Printer.Printers;
            set
            {
                this.RaiseAndSetIfChanged(ref Devices.Printer.Printers, value);
            }
        }
        public string? PrinterName
        {
            get => Devices.Printer.PrinterName;
            set
            {
                this.RaiseAndSetIfChanged(ref Devices.Printer.PrinterName, value);
            }
        }
        public async Task UpdatePrinters()
        {
            Printers = await  Devices.Printer.GetPrinters();
            if (PrinterName is null && Printers is not null) PrinterName = Printers[0];
        }

        //public PrinterViewModel()
        //{
        //    //HeaderText = "Принтер";
        //}
        //protected override bool OpenPort_abstract()
        //{
        //    //Devices.Printer = new Printer();
        //    //Devices.Printer = (Printer)Devices.SetMeasureDeviceName(Devices.Printer, PortItem);

        //    if (Devices.Printer.OpenPort() is true)
        //    {
        //        //Settings.Mult = new Delay();
        //        //Settings.WhileGetVoltAsync();
        //        return Devices.Printer.IsOpened();
        //    }
        //    return false;
        //}

        //protected override void ClosePort_abstract()
        //{
        //    Devices.Printer.ClosePort();

        //}
        //public override bool IsOpened() => Devices.Printer.IsOpened();
    }
}
