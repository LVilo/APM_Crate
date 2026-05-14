using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace APM_Crate.Models.DevicesModel
{
    public class PrinterInfo
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public bool IsDefault { get; set; }
    }

    public static class PrinterHelper
    {
        public static string[] GetAvailablePrinters()
        {
            List<string> printers = new List<string>();

            using var searcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            foreach (ManagementObject printer in searcher.Get())
            {
                string name = printer["Name"]?.ToString();

                bool workOffline = Convert.ToBoolean(
                    printer["WorkOffline"] ?? false);

                bool isDefault = Convert.ToBoolean(
                    printer["Default"] ?? false);

                bool isOnline = !workOffline;

                if(isOnline is true)
                {
                    printers.Add(name);
                }
            }

            return printers.ToArray();
        }
    }
    public class Printer : PrintDocument
    {
        public string? PrinterName;
        public string[]? Printers;
        public float FontSize = 20;

        public int PaperWidth => PrinterSettings.DefaultPageSettings.PaperSize.Width;
        public int PaperHeight => PrinterSettings.DefaultPageSettings.PaperSize.Height;

        public bool IsWindows { get; set; }

        public Printer()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { IsWindows = true; }
            else { IsWindows = false; }
        }
        public async Task<string[]> GetPrinters()
        {
            if (IsWindows)
            {
                return PrinterHelper.GetAvailablePrinters();
            }
            else
            {
                string output = await RunProcessAsync("lpstat", "-p");

                var printers = new List<string>();

                foreach (string line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = line.Trim();

                    // Пример строки:
                    // printer HP_LaserJet is idle. enabled since ...
                    if (trimmed.StartsWith("printer "))
                    {
                        string rest = trimmed.Substring("printer ".Length);
                        string printerName = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(printerName))
                            printers.Add(printerName);
                    }
                }

                return printers
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }
        }
        
        public async Task<bool> PrinterExist(string printerName)
        {
            string[] printers = await GetPrinters();
            return printers.Contains(printerName);
        }
        public async Task PrintText(string text)
        {
            if (IsWindows)
            {
                if (!await PrinterExist(PrinterName))
                    throw new Exception($"Принтер {PrinterName} не найден.");

                PrinterSettings.PrinterName = PrinterName;
                

                PrintPageEventHandler handler = null;

                handler = (sender, e) =>
                {
                    using Font font = new Font("Arial", FontSize);

                    SizeF textSize = e.Graphics.MeasureString(text, font);
                    float textWidth = textSize.Width;
                    float textHeight = textSize.Height;

                    float x = (PaperWidth - textWidth) / 2;
                    float y = (PaperHeight - textHeight) / 2;
                    //расположение текста по центру
                    float lineHeight = font.GetHeight(e.Graphics);

                    foreach (string line in text.Split('\n'))
                    {
                        e.Graphics.DrawString(line, font,Brushes.Black, x, y);
                        y += lineHeight;
                    }

                    e.HasMorePages = false;
                };

                try
                {
                    PrintPage += handler;
                    await Task.Run(Print);
                }
                finally
                {
                    PrintPage -= handler;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(PrinterName))
                    throw new ArgumentException("Имя принтера не указано.");

                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentException("Текст для печати пустой.");

                string tempFile = Path.Combine(Path.GetTempPath(), $"print_{Guid.NewGuid():N}.txt");

                await File.WriteAllTextAsync(tempFile, text);

                try
                {
                    await RunProcessAsync("lp", $"-d \"{PrinterName}\" \"{tempFile}\"");
                }
                finally
                {
                    if (File.Exists(tempFile))
                        File.Delete(tempFile);
                }
            }
        }
        private async Task<string> RunProcessAsync(string fileName, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
                throw new Exception($"Ошибка при выполнении {fileName} {arguments}\n{error}");

            return output;
        }
    }
}
