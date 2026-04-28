using APM_Crate.Models.DevicesModel;
using APM_Crate.Service;
using APM_Crate.ViewModels;
using APM_Crate.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM_Crate.Models
{
    public static class SaveRegistersModel
    {
        public class Parameter
        {
            public string Name { get; set; }
            public byte Value { get; set; }
        }

        public static List<Parameter> Parameters { get; set; }
        public readonly static string PathToDirectoryServer = "\\\\files\\Общее\\Прошивки и методики проверки\\Прикладное ПО\\АРМ настройки TIK-BIS\\CommonLogs";
        public readonly static string PathToDirectoryLocal = "Log";
        public static async Task<string> MakeReportAsync(string Device, string ordernumber, string starttime, string endtime, TimeSpan time_settings)
        {
            LogerViewModel.Instance.Write($"Запись регистров в файл");
            string date = String.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            string registers = "";
            string setting = SettingModel.IsResetting ? "Перенастройка" : "Настройка";
            await Task.Run(async () =>
            {
                foreach (var value in Parameters)
                {
                    registers += value.Value.ToString() + ";";
                }
            });


            string line = $"{Environment.UserName};{date};{setting};{starttime};{endtime};{Device};{time_settings:mm\\:ss};{ordernumber};{registers}\r\n";

            await WriteLineToFile(line, $"{PathToDirectoryLocal}\\{ordernumber}.csv");

            return await WriteLineToFile(line, $"{PathToDirectoryServer}\\{ordernumber}.csv");
        }
        private async static Task<string> WriteLineToFile(string line, string fileName)
        {
            //проверка существования папок
            try
            {
                await Task.Run(async () =>
                {
                    if (!Directory.Exists(PathToDirectoryLocal))
                    {
                        Directory.CreateDirectory(PathToDirectoryLocal);
                    }
                    if (!Directory.Exists(PathToDirectoryServer))
                    {
                        Directory.CreateDirectory(PathToDirectoryServer);
                    }
                });
                if (!File.Exists(fileName))
                {
                    File.WriteAllBytes(fileName, new byte[3] { 0xEF, 0xBB, 0xBF }); //указание на utf-8
                    string nameregisters = "";
                    foreach (var name in Parameters)
                    {
                        nameregisters += name.Name + ";";
                    }
                       await File.AppendAllTextAsync(fileName, $"Имя пользователя;Дата;Настройка;Время начала настройки;Время окончания настройки;Устройство;Общее время настройки;№ заказа;{nameregisters}\r\n");
                }

                using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) // проверяю на открытие файла(открыт ли он сейчас у пользователя или нет)
                {
                    stream.Close();
                       await File.AppendAllTextAsync(fileName, line);
                    return $"Записал настройки в {fileName}";
                }
            }
            catch (IOException)
            {
                await Dialog.ShowConfirm( "Файл занят другим процессом. Закройте файл, или нажмите \"Отмена\", но в таком случае данные не сохранятся в файл",new Delay(), false);
                return await WriteLineToFile(line, fileName);
            }

        }
        public async static Task MakeReportDatabase(string Device, string ordernumber,string serialumber, string setting, string starttime, string endtime, TimeSpan time_settings)
        {
            



        }

    }
}


