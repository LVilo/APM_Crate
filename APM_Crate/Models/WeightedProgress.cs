using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APM_Crate.ViewModels.SettingViewModel;

namespace APM_Crate.Models
{
    public class WeightedProgress
    {
        private readonly IProgress<ProgressReport> _progress;
        private readonly double _totalWeight;
        private double _current = 0;

        public WeightedProgress(IProgress<ProgressReport> progress, double totalWeight)
        {
            _progress = progress;
            _totalWeight = totalWeight;
        }

        public async Task Step(double weight, string message, Func<Task> action)
        {
            Report(0, message);
            await action();
            Report(weight, message + " ✔️");
        }
        public void Step(double weight, string message, Action action)
        {
            Report(0, message);
            action();
            Report(weight, message + " ✔️");
        }
        public async Task<float> Step(double weight, string message, Func<Task<float>> action)
        {
            Report(0, message);
            float result = await action();
            Report(weight, message + " ✔️");
            return result;
        }
        public async Task<double> Step(double weight, string message, Func<Task<double>> action)
        {
            Report(0, message);
            double result = await action();
            Report(weight, message + " ✔️");
            return result;
        }
        public async Task<ushort> Step(double weight, string message, Func<Task<ushort>> action)
        {
            Report(0, message);
            ushort result = await action();
            Report(weight, message + " ✔️");
            return result;
        }
        public void Report(double weight, string message)
        {
            _current += weight;
            _progress.Report(new ProgressReport
            {
                Percent = (int)(_current / _totalWeight * 100),
                Message = message
            });
        }
        public void Reset()
        {
            _current = 0;
            Report(0, "");
        }
    }
}
