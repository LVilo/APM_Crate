using APM_Crate.Models;
using Avalonia;
using DynamicData;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public class LiveChartsViewModel : DialogViewModel
    {
        private Points _PolylinePoints = new();
        public Points PolylinePoints { get => _PolylinePoints; set { this.RaiseAndSetIfChanged(ref _PolylinePoints, value); } }

        private double _Coef = 0.03d;
        public double Coef
        {
            get => _Coef;
            set
            {
                for (int i = 0; i < PolylinePoints.Count; i++)
                {
                    PolylinePoints[i] = new Point(PolylinePoints[i].X / _Coef * value, (PolylinePoints[i].Y-20d) / _Coef * value+20d);
                }
                this.RaiseAndSetIfChanged(ref _Coef, value);
            }
        }
        public LiveChartsViewModel()
        {
            string path = "C:\\Users\\ivanp\\Downloads\\fast(2).dat";
            byte[] data = File.ReadAllBytes(path);
            // Пример данных для графика
            CheckFilePLC.GetValuesChannel(ref data, out ushort[] channel1);
            CheckFilePLC.GetValuesChannel(ref data, out ushort[] channel2);
            BuildChart(channel1);
        }
        public void BuildChart(ushort[] source)
        {
            PolylinePoints = new Points();
            double Height = source.Max();
            for (int i = 0; i < source.Length; i++)
            {
                PolylinePoints.Add(new Point(i * Coef, (Height - source[i])*Coef + 20d));

            }
        }
        public Point ChangePoint(double x,double y,double newCoef)
        {
            return new Point(x * newCoef, y * newCoef);
        }
    }
}
