using APM_Crate.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using static System.Net.Mime.MediaTypeNames;


namespace APM_Crate.ViewModels
{
    public partial class LogerViewModel : ViewModelBase
    {
        private static readonly Lazy<LogerViewModel> _instance = new Lazy<LogerViewModel>(() => new LogerViewModel());

        public static LogerViewModel Instance => _instance.Value;

        private string? _logText;

        public string? LogText
        {
            get { return _logText; }
            set { this.RaiseAndSetIfChanged(ref _logText, value); }
        }
        private LogModel Log = new LogModel();

        /// <summary>
        /// запись в лог
        /// </summary>
        /// <param name="msg">Строка сообщения</param>
        public async Task Write(string msg)
        {
            LogText += $"{DateTime.Now:HH:mm:ss} {msg}\r\n";
            await WriteDebug(msg);
        }
        /// <summary>
        /// Запись в текстовый файл и в консоль
        /// </summary>
        /// <param name="msg">Строка сообщения</param>
        public async Task WriteDebug(string msg)
        {
            Debug.WriteLine(msg);
            Console.WriteLine(msg);
            Log.Write(msg);
        }
        /// <summary>
        /// Очистка лога
        /// </summary>
        public async Task ClearLog()
        {
            LogText = null;
        }
        public void UpdateLastLine(string lastmes,string newmes) => LogText = LogText.Replace(lastmes, newmes);
    }
}
