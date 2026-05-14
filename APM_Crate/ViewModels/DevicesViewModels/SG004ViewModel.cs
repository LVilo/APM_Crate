using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using Avalonia.Media;
using PortsWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APM_Crate.ViewModels.DevicesViewModels
{
    public interface IWorkWithSG004
    {
        Task<float> WriteResistance(float value);
        Task WriteVolt(float value);
        Task Write_mA(float value);
    }
    public class WorkWithSG004 : IWorkWithSG004
    {
        public async Task<float> WriteResistance(float value)
        {
            //if(value > 400)
            //{
            //    Devices.Work = new WorkWithoutSG004();
            //    await Devices.Work.WriteResistance(value);
            //    return;
            //}
            await Devices.sg004.ChangeOutputSignal(SG004AProtocol.OutputMode.ohm);
            await Devices.sg004.WriteValue(value);
            return await Devices.sg004.ReadWritingValue();
        }
        public async Task WriteVolt(float value)
        {
            await Devices.sg004.ChangeOutputSignal(SG004AProtocol.OutputMode.V);
            await Devices.sg004.WriteValue(value);
        }
        public async Task Write_mA(float value)
        {
            await Devices.sg004.ChangeOutputSignal(SG004AProtocol.OutputMode.mA);
            await Devices.sg004.WriteValue(value);
        }

    }
    public class WorkWithoutSG004 : IWorkWithSG004
    {
        public async Task<float> WriteResistance(float value)
        {
            await Dialog.ShowMessage($"На магазине сопротивлений установите {value} Ом");
            return value;
        }
        public async Task WriteVolt(float value)
        {
            await Dialog.ShowMessage($"На источнике с настраиваемым выходом установить напряжение {value} В");
        }
        public async Task Write_mA(float value)
        {
            await Dialog.ShowMessage($"При помощи магазина сопротивлений задайте {value} мА");

        }
    }
    public partial class SG004ViewModel : DevicesContext
    {
        public SG004ViewModel()
        {
            HeaderText = "SG-004 - Отключено";
        }
        public static IWorkWithSG004 Work { get; set; } = new WorkWithoutSG004();

        protected override async  Task<bool> OpenPort_abstract()
        {
            Devices.sg004 = new SG004AProtocol();
            Devices.sg004 = (SG004AProtocol)await Devices.SetMeasureDeviceName(Devices.sg004, PortItem);

           if( await Devices.sg004.OpenPort() is true)
            {
                //float f = await Devices.sg004.ReadWritingValue();
                Work = new WorkWithSG004();
                return Devices.sg004.IsOpened();
            }
            else
            {
                Work = new WorkWithoutSG004();
                return false;
            }
        }
       
        protected override async Task ClosePort_abstract()
        {
            Work = new WorkWithoutSG004();
            await Devices.sg004.ClosePort();
        }
        public override bool IsOpened() => Devices.sg004.IsOpened();
    }
}

