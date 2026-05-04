using PortsWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

 public class UsbTmcDevice : PortMultimeter
{
    private FileStream _stream;
    private readonly object _lock = new object(); // для потокобезопасности

    public override string GetName()
    {
        return PortName;
    }
    public override async Task<bool> SetName(string name)
    {

        if (string.IsNullOrEmpty(name))
            return false;

        PortName = name;
        return true;
    }
    public override async Task<bool> OpenPort()
    {
        Console.WriteLine(PortName);
        Linux.Acsessusb(PortName);
        if (!File.Exists(PortName))
        {
            Console.WriteLine($"Устройство {PortName} не найдено.");
            return false;
        }

        try
        {
            _stream = new FileStream(PortName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            Console.WriteLine($"Открыто устройство {PortName}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при открытии {PortName}: {ex.Message}");
            return false;
        }
    }
    protected override async Task WriteMessage(string message)
    {
        if (_stream == null)
            throw new InvalidOperationException("Устройство не открыто.");

        byte[] buffer = Encoding.ASCII.GetBytes(message + "\n");

        lock (_lock)
        {
            _stream.Write(buffer, 0, buffer.Length);
            _stream.Flush();
        }
    }
    public override async Task<string> ReadMessage(string message)
    {
        await WriteMessage(message);
        await WaitPortAnswer(300);
        return ReadLine();
    }
    public override async Task<double> GetVoltage(string type, int time)
    {
        await SetWorkType(DEVICE_VOLTMETER, type, true);
        try
        {
            string result = await ReadMessage(MESSAGE_READ);
            return ConvertString(result.Replace(".", ","));
        }
        catch (TimeoutException)
        {
            return DOUBLE_FALSEVALUE;
        }
    }
    private new async Task WaitPortAnswer(int timeoutMs)
    {
        if (_stream == null)
            throw new InvalidOperationException("Устройство не открыто.");

        int waited = 0;
        while (_stream.Length == 0 && waited < timeoutMs)
        {
            await Task.Delay(10);
            waited += 10;
        }
    }
    private new string ReadLine()
    {
        if (_stream == null)
            throw new InvalidOperationException("Устройство не открыто.");

        byte[] buffer = new byte[4096];
        int bytesRead = _stream.Read(buffer, 0, buffer.Length);

        return Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
    }

    public override async Task ClosePort()
    {
        if (_stream != null)
        {
            await WriteRemoteMode(false);
            _stream.Close();
            _stream.Dispose();
            _stream = null;
            Console.WriteLine("Порт закрыт");
        }
    }
    
}
