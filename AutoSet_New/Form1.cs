using PortsWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSet_New
{
    public partial class Form1 : Form
    {
        List<VisaDeviceInformation> usbDevicesInfo;
        DevicesCommunication devices;
        Setting Calibration;

        private System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();



            devices = new DevicesCommunication();
            devices.InitializeCrateAddress(Properties.Settings.Default.IP, 502);

            Calibration = new Setting(devices);
            PLC.Items.AddRange(new string[8] { "241", "242", "243", "511", "371", "374", "375", "Не подключен" });
            PLC.SelectedIndex = 7;

            IPTextBox.Text = Properties.Settings.Default.IP;

            PortsListReload();
            FillSavedPorts();

            #region DEV_UPDATE // Инициализация backgroundworker для вольтметра
            this.backgroundWorker2.DoWork += backgroundWorker2_CurrentUpdater;
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            #endregion
            Worker.DoWork += StartWorker_DoWork;
            Worker.WorkerSupportsCancellation = true;

            backgroundWorker3.DoWork += PLC_Updater;
            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker3.RunWorkerAsync();

            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)  //Опрос регистров 
        {
            await Task.Run(() =>
            {
                if (devices.Crate.Connected)
                {
                    try
                    {
                        string acc1Value = Convert.ToString(
                            (ushort)devices.Crate.ReadReg(Registers.REGISTER_VAL_RMS_CHANNEL1) / 100f);
                        string spd1Value = Convert.ToString(
                            (ushort)devices.Crate.ReadReg(Registers.REGISTER_VAL_RMS_CHANNEL1 + 3) / 100f);
                        string acc2Value = Convert.ToString(
                            (ushort)devices.Crate.ReadReg(Registers.REGISTER_VAL_RMS_CHANNEL2) / 100f);
                        string spd2Value = Convert.ToString(
                            (ushort)devices.Crate.ReadReg(Registers.REGISTER_VAL_RMS_CHANNEL2 + 3) / 100f);

                        // Обновление UI в основном потоке
                        BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                        {
                            ACC1.Text = acc1Value;
                            SPD1.Text = spd1Value;
                            ACC2.Text = acc2Value;
                            SPD2.Text = spd2Value;
                        }));
                    }
                    catch (Exception ex)
                    {
                        devices.Crate.Disconnect();
                        LogWrite(ex.Message);
                    }
                }
            });
        }

        private void PLC_Updater(object sender, DoWorkEventArgs e)
        {
            try
            {
                devices.Crate.Connect();
                while (!backgroundWorker3.CancellationPending)
                {
                    if (Convert.ToString(devices.Crate.ReadReg(108), 2).Reverse().ToArray()[0] == '0')
                    {
                        Thread.Sleep(500);
                        int reg = devices.Crate.ReadReg(Registers.REGISTER_CONTROLLER_TYPE);
                        if (reg >= 1 && reg <= 7)
                        {
                            BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                            {
                                if (PLC.SelectedIndex != devices.Crate.ReadReg(Registers.REGISTER_CONTROLLER_TYPE) - 1)
                                {
                                    PLC.SelectedIndex = devices.Crate.ReadReg(Registers.REGISTER_CONTROLLER_TYPE) - 1;
                                    float version = devices.Crate.ReadVersion(Registers.REGISTER_VERSION_PLC);
                                    if ((int)version == 4)
                                    {
                                        CoefTextBox.Text = "6,67";
                                    }
                                    else if ((int)version == 6)
                                    {
                                        CoefTextBox.Text = "10";
                                    }
                                }

                            }));
                        }
                        else
                        {
                            LogWrite("Данный контроллер не поддерживает отображение своего типа, пожалуйста выберите нужный контроллер из списка. По умолчанию будет стоять 242");

                            PLC_AutoCheck.CheckedChanged -= PLC_AutoCheck_CheckedChanged;
                            BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                            {
                                PLC.SelectedIndex = 1;
                                PLC_AutoCheck.Checked = false;
                                PLC.Enabled = true;
                            }));
                            backgroundWorker3.CancelAsync();
                            PLC_AutoCheck.CheckedChanged += PLC_AutoCheck_CheckedChanged;
                        }
                    }
                    else
                    {
                        BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                        {
                            PLC.SelectedIndex = 7;
                        }));
                    }

                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                {
                    PLC.SelectedIndex = 7;
                }));
                LogWrite(ex.Message);
            }
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // команды при закрытии формы
            Calibration.IsRunning = false;
            backgroundWorker2.CancelAsync();
            Thread.Sleep(700);
            devices.CloseConnection();

            Properties.Settings.Default.portGenerator = comboBoxGenerator.Text;
            Properties.Settings.Default.portMultimeter = AgillentPortBox.Text;
            Properties.Settings.Default.Save();
        }

        private string[] GetAllPorts()
        {
            devices.usbDevicesInfo = Port.FindVisaDevicesInfo();
            List<string> usbInfo = new List<string>();
            devices.usbDevicesInfo.ForEach(t => usbInfo.Add(t.GetInfo()));

            return usbInfo.Concat(SerialPort.GetPortNames()).ToArray();
        }

        private void PortsListReload()
        {
            string[] ports = GetAllPorts();

            AgillentPortBox.Items.Clear();
            AgillentPortBox.Items.AddRange(ports);

            comboBoxGenerator.Items.Clear();
            comboBoxGenerator.Items.AddRange(ports);
        }

        private string[] GetAdditionalPorts()
        {
            usbDevicesInfo = Port.FindVisaDevicesInfo();
            List<string> result = new List<string>();

            for (int i = 0; i < usbDevicesInfo.Count; i++)
            {
                result.Add(usbDevicesInfo[i].description);
            }

            return result.ToArray();
        }

        private void FillSavedPorts()
        {
            if (Properties.Settings.Default.portGenerator.Contains("COM"))
            {
                comboBoxGenerator.SelectedItem = Properties.Settings.Default.portGenerator;
            }
            if (Properties.Settings.Default.portMultimeter.Contains("COM"))
            {
                AgillentPortBox.SelectedItem = Properties.Settings.Default.portMultimeter;
            }
        }

        private void LogWrite(string message)
        {
            try
            {
                if (LogTextBox.InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        LogTextBox.AppendText(String.Format("{0:hh:mm:ss} ", DateTime.Now) + message + "\r\n");
                    }));
                    return;
                }
                LogTextBox.AppendText(String.Format("{0:hh:mm:ss} ", DateTime.Now) + message + "\r\n");
            }
            catch { }
        }

        private void backgroundWorker2_CurrentUpdater(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker2.CancellationPending)
            {
                while (Calibration.IsRunning)
                {
                    if (devices.DC_Read && devices.multimeter.OpenPort())
                    {
                        try
                        {
                            devices.currentVolt = devices.multimeter.GetVoltage(PortMultimeter.SIGNALTYPE_DC, 100);
                        }
                        catch (InvalidOperationException ex)
                        {
                            LogWrite(ex.Message);
                        }
                    }
                }
                Thread.Sleep(300);
            }
        }

        private void StartWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                LogWrite($"{Convert.ToSingle(CoefTextBox.Text)}");
                int PLC = 0;
                devices.CrateOpenPort();
                Thread.Sleep(100);

                BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                {
                    PLC = this.PLC.SelectedIndex;
                    Calibration.coef = Convert.ToSingle(CoefTextBox.Text);
                    Calibration.Point_1 = Convert.ToSingle(Point_1_textBox.Text);
                    Calibration.Point_2 = Convert.ToSingle(Point_2_textBox.Text);
                    Calibration.DC = DC_textBox.Text;
                    Calibration.frequency = Convert.ToDouble(FreqTextBox.Text);
                }));
                Thread.Sleep(2000);
                devices.Crate.SetPassword();

                int serial = 0;
                if (int.TryParse(textBoxSerialNum.Text, out serial))
                {
                    devices.Crate.SetSerialNum(serial);
                }
                else
                {
                    LogWrite("Не указан серийный номер");
                }

                //string compound = new string(Convert.ToString(Сalibration.Reg[0], 2).Reverse().ToArray());

                if (Convert.ToString(devices.Crate.ReadReg(108), 2).Reverse().ToArray()[0] != '0')
                    throw new Exception("Контроллер не подключен в 7 слот");

                devices.Crate.WriteReg(108, 16382);//меняю состав корзины под 7 слот
                if (!(PLC == 2 || PLC == 5))
                {
                    if (!devices.generator.OpenPort())
                    {
                        LogWrite("Порт генератора не открыт");
                        return;
                    }
                }
                if (!devices.multimeter.OpenPort())
                {
                    LogWrite("Порт мультиметра не открыт");
                    return;
                }
                if (!backgroundWorker2.IsBusy)
                {
                    backgroundWorker2.RunWorkerAsync();
                }

                Calibration.Run_Setting(PLC, Convert.ToInt16(Termo.Text));

                if (!Calibration.IsRunning)
                    LogWrite($"СТОП");
            }
            catch (EasyModbus.Exceptions.ConnectionException)
            {
                LogWrite($"Произошла ошибка: время соединения истекло");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //LogWrite($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                Calibration.IsRunning = false;
            }
        }

        private async void PLC_AutoCheck_CheckedChanged(object sender, EventArgs e)
        {
            PLC_AutoCheck.Enabled = false;
            if (PLC_AutoCheck.Checked)
            {
                PLC.Enabled = false;
                await Task.Run(() =>
                {
                    //Thread.Sleep(2000);
                    while (backgroundWorker3.IsBusy)
                        Thread.Sleep(1);
                    if (!backgroundWorker3.IsBusy)
                    {
                        backgroundWorker3.RunWorkerAsync();
                    }
                });
            }
            else
            {
                backgroundWorker3.CancelAsync();
                PLC.Enabled = true;
            }
            await Task.Delay(2500);
            PLC_AutoCheck.Enabled = true;
        }

        private void PLC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (PLC.SelectedIndex)
                {
                    case 0:
                        ReserCoef_1.Enabled = true;
                        ResetCoef_2.Enabled = false;
                        FreqTextBox.Enabled = true;
                        STOP.Enabled = true;
                        Start.Enabled = true;
                        CoefTextBox.Enabled = true;
                        DC_textBox.Enabled = true;
                        Point_1_textBox.Enabled = true;
                        Point_2_textBox.Enabled = true;
                        Termo.Enabled = false;
                        break;
                    case 1:
                        ReserCoef_1.Enabled = true;
                        ResetCoef_2.Enabled = true;
                        FreqTextBox.Enabled = true;
                        STOP.Enabled = true;
                        Start.Enabled = true;
                        CoefTextBox.Enabled = true;
                        DC_textBox.Enabled = true;
                        Point_1_textBox.Enabled = true;
                        Point_2_textBox.Enabled = true;
                        Termo.Enabled = false;
                        break;
                    case 2:
                        ReserCoef_1.Enabled = false;
                        ResetCoef_2.Enabled = false;
                        FreqTextBox.Enabled = false;
                        STOP.Enabled = true;
                        Start.Enabled = true;
                        CoefTextBox.Enabled = false;
                        DC_textBox.Enabled = false;
                        Point_1_textBox.Enabled = false;
                        Point_2_textBox.Enabled = false;
                        Termo.Enabled = false;
                        break;
                    case 3:
                        ReserCoef_1.Enabled = false;
                        ResetCoef_2.Enabled = false;
                        FreqTextBox.Enabled = false;
                        STOP.Enabled = true;
                        Start.Enabled = true;
                        CoefTextBox.Enabled = false;
                        DC_textBox.Enabled = false;
                        Point_1_textBox.Enabled = false;
                        Point_2_textBox.Enabled = false;
                        Termo.Enabled = false;
                        break;
                    case 4:
                        ReserCoef_1.Enabled = true;
                        ResetCoef_2.Enabled = false;
                        FreqTextBox.Enabled = true;
                        STOP.Enabled = true;
                        Start.Enabled = true;
                        CoefTextBox.Enabled = true;
                        DC_textBox.Enabled = true;
                        Point_1_textBox.Enabled = true;
                        Point_2_textBox.Enabled = true;
                        Termo.Enabled = true;
                        break;
                    case 5:
                        ReserCoef_1.Enabled = false;
                        ResetCoef_2.Enabled = false;
                        FreqTextBox.Enabled = false;
                        STOP.Enabled = true;
                        Start.Enabled = true;
                        CoefTextBox.Enabled = false;
                        DC_textBox.Enabled = false;
                        Point_1_textBox.Enabled = false;
                        Point_2_textBox.Enabled = false;
                        Termo.Enabled = false;
                        break;
                    case 6:
                        ReserCoef_1.Enabled = true;
                        ResetCoef_2.Enabled = false;
                        FreqTextBox.Enabled = true;
                        STOP.Enabled = true;
                        Start.Enabled = true;
                        CoefTextBox.Enabled = true;
                        DC_textBox.Enabled = true;
                        Point_1_textBox.Enabled = true;
                        Point_2_textBox.Enabled = true;
                        Termo.Enabled = false;

                        break;
                    case 7:
                        CoefTextBox.Enabled = false;
                        DC_textBox.Enabled = false;
                        FreqTextBox.Enabled = false;
                        Point_1_textBox.Enabled = false;
                        Point_2_textBox.Enabled = false;
                        Termo.Enabled = false;
                        ReserCoef_1.Enabled = false;
                        ResetCoef_2.Enabled = false;
                        STOP.Enabled = false;
                        Start.Enabled = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogWrite("PLC: " + ex.Message);
            }
        }

        private void C2_CheckedChanged(object sender, EventArgs e)
        {
            C1.CheckedChanged -= C1_CheckedChanged;
            if (C2.Checked)
            {
                C1.Checked = false;
                devices.generator.SetChannel(2);
            }
            C1.CheckedChanged += C1_CheckedChanged;
        }

        private void C1_CheckedChanged(object sender, EventArgs e)
        {
            C2.CheckedChanged -= C2_CheckedChanged;
            if (C1.Checked)
            {
                C2.Checked = false;
                devices.generator.SetChannel(1);
            }
            C2.CheckedChanged += C2_CheckedChanged;
        }

        #region Кнопки
        private async void Start_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(textBoxOrderNum.Text)) throw new Exception("Номер заказа не указан");
                    if (string.IsNullOrEmpty(textBoxSerialNum.Text)) throw new Exception("Серийный номер не указан");
                    if (string.IsNullOrEmpty(CoefTextBox.Text)) throw new Exception("Коэффициент не заполнен");

                    if (!OrderNumberCheck())
                    {
                        throw new Exception("Некорректный номер заказа:\r\n\t" +
                            "Не допускаются символы \\ / ^ * ? \" < > |\r\n\t" +
                            "Введите корректный номер заказа и запустите запись файла повторно");
                    }
                    Calibration.orderNum = textBoxOrderNum.Text;
                    if (!Worker.IsBusy) Worker.RunWorkerAsync();

                });
            }
            catch (Exception ex)
            {
                LogWrite(ex.Message);
            }

        }

        private bool OrderNumberCheck()
        {
            return !textBoxOrderNum.Text.Contains("\\") &&
                !textBoxOrderNum.Text.Contains("/") &&
                !textBoxOrderNum.Text.Contains(":") &&
                !textBoxOrderNum.Text.Contains("*") &&
                !textBoxOrderNum.Text.Contains("?") &&
                !textBoxOrderNum.Text.Contains("\"") &&
                !textBoxOrderNum.Text.Contains("<") &&
                !textBoxOrderNum.Text.Contains(">") &&
                !textBoxOrderNum.Text.Contains("|");
        }

        private async void STOP_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                Worker.CancelAsync();
                Calibration.IsRunning = false;
                Thread.Sleep(4000);
                devices.DC_Read = true;
            });
        }

        private void AgillentPortBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            devices.multName = AgillentPortBox.Text;
        }

        private void comboBoxGenerator_SelectedIndexChanged(object sender, EventArgs e)
        {
            devices.genName = comboBoxGenerator.Text;
            devices.generator = (PortGenerator)devices.SetMeasureDeviceName(devices.generator, devices.genName);
        }

        private async void Connect_Agillent_Click(object sender, EventArgs e)
        {
            await (Task.Run(() =>
            {
                try
                {
                    devices.multimeter = (PortMultimeter)devices.SetMeasureDeviceName(devices.multimeter, devices.multName);
                    if (devices.multimeter.OpenPort())
                    {
                        LogWrite("Мультиметр подключен");
                        devices.multimeter.VoltmeterMode(PortMultimeter.SIGNALTYPE_DC);
                        backgroundWorker2.RunWorkerAsync();
                    }
                    else
                    {
                        LogWrite("Не удалось подключиться к мультиметру");
                    }
                }
                catch (Exception Ex)
                {
                    LogWrite("Multimeter:\r\n" + Ex.Message);
                }
            }));
        }
        private async void Disconnect_Agillent_Click(object sender, EventArgs e)
        {
            devices.DC_Read = false;
            backgroundWorker2.CancelAsync();
            await Task.Delay(600);

            await Task.Run(() =>
            {
                devices.multimeter.ClosePort();
                LogWrite("Мультиметр отключен");
            });
        }

        private void Connect_Generator_Click(object sender, EventArgs e)
        {
            devices.generator.PortName = comboBoxGenerator.SelectedItem.ToString();
            try
            {
                if (devices.generator.OpenPort())
                {
                    LogWrite("Генератор подключен");
                }
                else
                {
                    LogWrite("Не удалось подключить генератор");
                }
            }
            catch (Exception Ex)
            {
                LogWrite("Generator:\r\n" + Ex.Message);
            }
        }

        private void Disconnect_Generator_Click(object sender, EventArgs e)
        {
            devices.generator.ClosePort();
            LogWrite("Генератор отключен");
        }

        private void UpdatePort_Click(object sender, EventArgs e)
        {
            PortsListReload();
        }

        private async void ResetCoef_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        ReserCoef_1.Enabled = false;
                    }));


                    if (!devices.Crate.Connected)
                    {
                        devices.Crate.Connect();
                    }
                    LogWrite("Сбрасываю коэффициенты");
                    devices.Crate.ResetCoef(Registers.REGISTER_COEFF_A_CHANNEL1,
                        Registers.REGISTER_ADDR_ON_CHANNEL1, 8);
                    LogWrite("Коэффициенты сброшены");
                }
                catch (Exception ex)
                {
                    LogWrite($"Произошла ошибка: {ex.Message}");
                }
                finally
                {
                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        ReserCoef_1.Enabled = true;
                    }));
                }
            });
        }
        private async void ResetCoef_2_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    ReserCoef_1.Enabled = false;
                    if (!devices.Crate.Connected)
                    {
                        devices.Crate.Connect();
                    }
                    LogWrite("Сбрасываю коэффициенты");
                    devices.Crate.ResetCoef(Registers.REGISTER_COEFF_A_CHANNEL2,
                        Registers.REGISTER_ADDR_ON_CHANNEL2, 8);
                    LogWrite("Коэффициенты сброшены");
                }
                catch (Exception ex)
                {
                    LogWrite($"Произошла ошибка: {ex.Message}");
                }
                finally
                {
                    ReserCoef_1.Enabled = true;
                }
            });
        }

        private async void Save_IP_Click(object sender, EventArgs e)
        {
            if (PLC_AutoCheck.Checked)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        backgroundWorker3.CancelAsync();
                        Thread.Sleep(1000);
                        devices.Crate.Disconnect();
                        Thread.Sleep(1000);
                        Properties.Settings.Default.IP = IPTextBox.Text;
                        devices.Crate.IPAddress = Properties.Settings.Default.IP;
                        LogWrite("IP сохранен");
                        if (!backgroundWorker3.IsBusy)
                            backgroundWorker3.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        LogWrite(ex.Message);
                    }
                });
            }
            else
            {
                await Task.Run(() =>
                {
                    try
                    {
                        devices.Crate.Disconnect();
                        Thread.Sleep(1000);
                        Properties.Settings.Default.IP = IPTextBox.Text;
                        devices.Crate.IPAddress = Properties.Settings.Default.IP;
                        LogWrite("IP сохранен");
                        devices.Crate.Connect();
                    }
                    catch (Exception ex)
                    {
                        BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                        {
                            PLC.SelectedIndex = 7;
                        }));
                        LogWrite(ex.Message);
                    }
                });
            }
        }

        private async void lvl1_Click(object sender, EventArgs e)
        {
            await (Task.Run(() =>
            {
                try
                {
                    Calibration.IsRunning = true;
                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        Calibration.frequency = Convert.ToDouble(FreqTextBox.Text);
                    }));

                    string message;
                    if (!devices.CheckExtDevices(out message))
                    {
                        LogWrite(message);
                        return;
                    }

                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        SetButtonsEnabled(false);
                    }));

                    Calibration.SetGeneratorPoint(Convert.ToSingle(Lvl1_textbox.Text));
                    if (!Calibration.IsRunning)
                    {
                        LogWrite("СТОП");
                        return;
                    }
                    LogWrite($"Установлено {Lvl1_textbox.Text} мВ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        SetButtonsEnabled(true);
                    }));
                }
            }));
        }

        private async void lvl2_Click(object sender, EventArgs e)
        {
            await (Task.Run(() =>
            {
                try
                {
                    Calibration.IsRunning = true;
                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        Calibration.frequency = Convert.ToDouble(FreqTextBox.Text);
                    }));

                    string message;
                    if (!devices.CheckExtDevices(out message))
                    {
                        LogWrite(message);
                        return;
                    }

                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        SetButtonsEnabled(false);
                    }));

                    Calibration.SetGeneratorPoint(Convert.ToSingle(Lvl2_textbox.Text));
                    if (!Calibration.IsRunning)
                    {
                        LogWrite("СТОП");
                        return;
                    }
                    LogWrite($"Установлено {Lvl2_textbox.Text} мВ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        SetButtonsEnabled(true);
                    }));
                }
            }));
        }

        private void SetButtonsEnabled(bool enabled)
        {
            lvl1.Enabled = enabled;
            lvl2.Enabled = enabled;
            Disconnect_Agillent.Enabled = enabled;
            Connect_Agillent.Enabled = enabled;
            Disconnect_Generator.Enabled = enabled;
            Connect_Generator.Enabled = enabled;
            Start.Enabled = enabled;
        }
        #endregion

        #region Фильтры Текста
        private void IPTextBox_TextChanged(object sender, EventArgs e)
        {
            int previousCursorPosition = IPTextBox.SelectionStart;
            IPTextBox.TextChanged -= IPTextBox_TextChanged;
            IPTextBox.Text = new string(Array.FindAll(IPTextBox.Text.ToCharArray(), c => char.IsDigit(c) || c == '.'));
            IPTextBox.SelectionStart = previousCursorPosition;
            IPTextBox.SelectionLength = 0;
            IPTextBox.TextChanged += IPTextBox_TextChanged;
        }

        private void CoefTextBox_TextChanged(object sender, EventArgs e)
        {
            CoefTextBox.TextChanged -= CoefTextBox_TextChanged;

            if (CoefTextBox.Text == "6,67")
            {
                DC_textBox.Text = "12";
                Point_1_textBox.Text = "6,67";
                Point_2_textBox.Text = "100";
                Lvl1_textbox.Text = "6,67";
                Lvl2_textbox.Text = "100";
            }
            else if (CoefTextBox.Text == "10")
            {
                DC_textBox.Text = "10";
                Point_1_textBox.Text = "10";
                Point_2_textBox.Text = "3000";
                Lvl1_textbox.Text = "10";
                Lvl2_textbox.Text = "3000";
            }
            string filteredText = FilterText(CoefTextBox.Text);

            CoefTextBox.Text = filteredText;
            CoefTextBox.SelectionStart = filteredText.Length;
            CoefTextBox.TextChanged += CoefTextBox_TextChanged;
        }
        private void Point_1_textBox_TextChanged(object sender, EventArgs e)
        {
            Point_1_textBox.TextChanged -= Point_1_textBox_TextChanged;

            string filteredText = FilterText(Point_1_textBox.Text);

            Point_1_textBox.Text = filteredText;
            Point_1_textBox.SelectionStart = filteredText.Length;
            Point_1_textBox.TextChanged += Point_1_textBox_TextChanged;
        }

        private void Point_2_textBox_TextChanged(object sender, EventArgs e)
        {
            Point_2_textBox.TextChanged -= Point_2_textBox_TextChanged;

            string filteredText = FilterText(Point_2_textBox.Text);

            Point_2_textBox.Text = filteredText;
            Point_2_textBox.SelectionStart = filteredText.Length;
            Point_2_textBox.TextChanged += Point_2_textBox_TextChanged;
        }
        private void DC_textBox_TextChanged(object sender, EventArgs e)
        {
            DC_textBox.TextChanged -= DC_textBox_TextChanged;

            string filteredText = FilterText(DC_textBox.Text);

            DC_textBox.Text = filteredText;
            DC_textBox.SelectionStart = filteredText.Length;
            DC_textBox.TextChanged += DC_textBox_TextChanged;
        }
        private void FreqTextBox_TextChanged(object sender, EventArgs e)
        {
            FreqTextBox.TextChanged -= FreqTextBox_TextChanged;

            string filteredText = FilterText(FreqTextBox.Text);

            FreqTextBox.Text = filteredText;
            FreqTextBox.SelectionStart = filteredText.Length;
            FreqTextBox.TextChanged += FreqTextBox_TextChanged;
        }

        private string FilterText(string text)
        {
            string filteredText = "";
            bool foundComma = false;
            int commaCount = 0;
            foreach (char c in text)
            {
                if (char.IsDigit(c) || (c == ',' && !foundComma))
                {
                    filteredText += c;
                    if (c == ',')
                    {
                        foundComma = true;
                        commaCount++;
                    }
                }
            }
            int commaIndex = filteredText.IndexOf(',');
            if (commaIndex != -1 && filteredText.Length - commaIndex > 4) // 3, потому что один символ для запятой
            {
                filteredText = filteredText.Substring(0, commaIndex + 4);
            }
            return filteredText;
        }
        #endregion

        private void buttonReport_Click(object sender, EventArgs e)
        {
            Calibration.MakeReport(PLC.SelectedIndex);
        }

        private void textBoxOrderNum_TextChanged(object sender, EventArgs e)
        {

            string text = "";
            int cursorPos = textBoxOrderNum.SelectionStart;
            string original = textBoxOrderNum.Text;
            uint length = 0;
            foreach (char ch in textBoxOrderNum.Text)
            {
                if (char.IsLetterOrDigit(ch)) text += ch;
                length++;
                if (length == 13) break;
            }
            if (original != text)
            {
                textBoxOrderNum.Text = text;
                textBoxOrderNum.SelectionStart = Math.Min(cursorPos, textBoxOrderNum.Text.Length);
            }
        }

        private void textBoxSerialNum_TextChanged(object sender, EventArgs e)
        {
            string text = "";
            int cursorPos = textBoxSerialNum.SelectionStart;
            string original = textBoxSerialNum.Text;
            uint length = 0;
            foreach (char ch in textBoxSerialNum.Text)
            {

                if (char.IsDigit(ch)) text += ch;
                length++;
                if (length == 13) break;
            }
            if (original != text)
            {
                textBoxSerialNum.Text = text;

                textBoxSerialNum.SelectionStart = cursorPos;
            }
        }
    }
}
