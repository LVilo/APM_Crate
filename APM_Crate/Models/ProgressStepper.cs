using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models
{
    public class ProgressReport
    {
        public int Percent { get; set; }
        public string Message { get; set; } = "";
    }
    public class ProgressStepper
    {
        private readonly IProgress<ProgressReport> _progress;
        private readonly int _totalSteps;
        private int _currentStep = 0;

        public ProgressStepper(IProgress<ProgressReport> progress, int totalSteps)
        {
            _progress = progress;
            _totalSteps = totalSteps;
        }

        public void Step(string message)
        {
            _currentStep++;
            int percent = (int)((double)_currentStep / _totalSteps * 100);

            _progress.Report(new ProgressReport
            {
                Percent = percent,
                Message = message
            });
        }
    }
}
