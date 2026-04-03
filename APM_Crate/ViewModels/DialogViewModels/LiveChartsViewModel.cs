using APM_Crate.Models;
using DynamicData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.ViewModels.DialogViewModels
{
    public class LiveChartsViewModel : DialogViewModel
    {
        public double[] Values { get; set; }

        public LiveChartsViewModel()
        {

            // Пример данных для графика
            double[] data = { 1, 3, 2, 5, 4, 6, 3 };

            // Передаём данные на Canvas
            Values = data;

            // Перерисовать Canvas
           
        }
    }
}
