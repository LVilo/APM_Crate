using AutoSet_New.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoSet_New
{
    partial class Form1
    {
        private LogModel Log = new LogModel();

        /// <summary>
        /// запись в лог
        /// </summary>
        /// <param name="msg">Строка сообщения</param>
        public void Write(string msg)
        {
            LogTextBox.Text += $"{DateTime.Now:HH:mm:ss} {msg}\r\n";
            WriteDebug(msg);
        }
        /// <summary>
        /// Запись в текстовый файл и в консоль
        /// </summary>
        /// <param name="msg">Строка сообщения</param>
        public void WriteDebug(string msg)
        {
            Debug.WriteLine(msg);
            Console.WriteLine(msg);
            Log.Write(msg);
        }
        /// <summary>
        /// Очистка лога
        /// </summary>
        public void ClearLog()
        {
            LogTextBox.Text = null;
        }
        public void UpdateLastLine(string lastmes,string newmes) => LogTextBox.Text = LogTextBox.Text.Replace(lastmes, newmes);
    }
}
