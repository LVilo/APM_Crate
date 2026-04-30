using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortsWork;
using System.Threading;
using System.Windows.Forms;
using EasyModbus;
using System.IO;

namespace AutoSet_New
{
    class Setting
    {
        private DevicesCommunication devices;

        public float Point_1 = 0, Point_2 = 0, coef = 0;
        public bool IsRunning = true;

        private int Reg_adr = 0, Reg_adr_coef = 0, Reg_adr_on = 0;
        public float A1, A2, B1, B2;
        public string DC = "";
        public bool Survey = false;

        private double _frequency = 0d;
        public double frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public string orderNum = "_";

        public Setting(DevicesCommunication devList)
        {
            devices = devList;
        }
        private float AverageValue(int Reg_adr)
        {
            float Value = 0;
            //Agil.GetVolt("AC", 100);
            //Creyt.ReadReg(Reg_signal);
            Value = (ushort)devices.Crate.ReadReg(Reg_adr) / 100f;
            for (int j = 0; j < 9; j++)
            {
                Thread.Sleep(400);
                //Agil.GetVolt("AC", 100);
                //Creyt.ReadReg(Reg_signal);
                Value += (ushort)devices.Crate.ReadReg(Reg_adr) / 100f;
                if (IsRunning == false)
                    throw new Exception("Стоп");
            }
            return Value / 10;
        }

        public void SetGeneratorPoint(float voltage)
        {
            devices.DC_Read = false;
            Thread.Sleep(1000);
            devices.multimeter.VoltmeterMode(PortMultimeter.SIGNALTYPE_AC);
            Thread.Sleep(1000);

            devices.generator.ChangeSignalType(PortGenerator.SignalType.Sine);
            devices.generator.SetFrequency(frequency);

            SetVolt(voltage);
        }

        private void Count_Coef()
        {

            float signal_1 = 0, integral_1 = 0, signal_2 = 0, integral_2 = 0;
            IsRunning = true;
            try
            {
                SetGeneratorPoint(Point_1);
                Thread.Sleep(5000);
                float realPoint_1 = (float)(devices.multimeter.GetVoltage(PortMultimeter.SIGNALTYPE_AC, 100) * 1000.0);

                if (!IsRunning)
                    return;
                signal_1 = AverageValue(Reg_adr);
                integral_1 = AverageValue(Reg_adr + 3);

                SetVolt(Point_2);
                Thread.Sleep(5000);
                float realPoint_2 = (float)(devices.multimeter.GetVoltage(PortMultimeter.SIGNALTYPE_AC, 100) * 1000.0);

                if (!IsRunning)
                    return;
                signal_2 = AverageValue(Reg_adr);
                integral_2 = AverageValue(Reg_adr + 3);

                A1 = ((int)Math.Round((realPoint_2 - realPoint_1) / coef)) / (signal_2 - signal_1);
                B1 = realPoint_1 / coef - A1 * signal_1;
                A2 = ((int)Math.Round((realPoint_2 - realPoint_1) * 2 / coef)) / (integral_2 - integral_1);
                B2 = realPoint_1 * 2 / coef - A2 * integral_1;
            }
            catch (Exception)
            {
                throw new Exception("Что-то пошло не так");
            }
        }
        public void SetVolt(float Volt)
        {
            try
            {
                //Проверка галочки IsRunning!!
                devices.multimeter.SetVoltage(devices.generator, Volt, frequency, 0.0005, 3);

            }
            catch (InvalidOperationException)
            {
                throw;
            }

        }
        private void Set_reg_adr(int Channel)
        {
            switch (Channel)
            {
                case 1:
                    Reg_adr_coef = Registers.REGISTER_COEFF_A_CHANNEL1;
                    Reg_adr_on = Registers.REGISTER_ADDR_ON_CHANNEL1;
                    Reg_adr = Registers.REGISTER_VAL_RMS_CHANNEL1;
                    break;
                case 2:
                    Reg_adr_coef = Registers.REGISTER_COEFF_A_CHANNEL2;
                    Reg_adr_on = Registers.REGISTER_ADDR_ON_CHANNEL2;
                    Reg_adr = Registers.REGISTER_VAL_RMS_CHANNEL2;
                    break;
                case 3:
                    Reg_adr_coef = Registers.REGISTER_COEFF_A_CHANNEL3;
                    Reg_adr_on = Registers.REGISTER_ADDR_ON_CHANNEL3;
                    Reg_adr = Registers.REGISTER_VAL_RMS_CHANNEL3;
                    break;
                case 4:
                    Reg_adr_coef = Registers.REGISTER_COEFF_A_CHANNEL4;
                    //Reg_adr_on = Registers.REGISTER_ADDR_ON_CHANNEL4; --- мне для 4 канала это вообще может быть нужно?
                    Reg_adr = Registers.REGISTER_VAL_RMS_CHANNEL4;
                    break;
            }
        }

        private void SetCoeff_Volt(int channel)
        {
            IsRunning = true;
            if (MessageBox.Show("Установите напряжение 10В на калибраторе", "Настройка канала по напряжению",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Cancel)
            {
                IsRunning = false;
                return;
            }

            Set_reg_adr(channel);
            devices.DC_Read = false;
            Thread.Sleep(1000);
            devices.multimeter.VoltmeterMode(PortMultimeter.SIGNALTYPE_AC);
            devices.DC_Read = true;

            devices.Crate.ResetCoef(Reg_adr_coef, Reg_adr_on, 8);
            if (!IsRunning)
                return;
        }

        private void SetCoef_IEPE(int Channel)
        {
            DevicesCommunication.Log.Write($"Настройка канала {Channel} / IEPE");
            IsRunning = true;
            if (MessageBox.Show($"Установите контакты для настройки IEPE {Channel}-го канала",
                "Настройка IEPE", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Cancel)
            {
                return;
            }

            Set_reg_adr(Channel);
            devices.DC_Read = false;
            Thread.Sleep(1000);
            devices.multimeter.VoltmeterMode(PortMultimeter.SIGNALTYPE_DC);
            devices.DC_Read = true;

            devices.Crate.ResetCoef(Reg_adr_coef, Reg_adr_on, 8);
            if (!IsRunning)
                return;

            DialogResult result;
            bool isCorrectValue = false;
            do
            {
                result = MessageBox.Show($"При помощи магазина сопротивлений задайте {DC}В",
                    "Проверка", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Cancel)
                    return;
                else if (result == DialogResult.OK)
                {
                    if (devices.currentVolt > Convert.ToDouble(DC) - 0.5 && devices.currentVolt < Convert.ToDouble(DC) + 0.5)
                        isCorrectValue = true;
                    else
                        MessageBox.Show($"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {Convert.ToDouble(DC) - 0.5} до {Convert.ToDouble(DC) + 0.5} В.");
                }
            } while (!isCorrectValue);
            if (!IsRunning)
                return;

            Count_Coef(); //регулирую напряжение на генераторе чтобы он выдавал Vrms вместо Vpp
            if (!IsRunning)
                return;
            //LogWrite("Записываю коэфициенты");
            devices.Crate.SetPassword();
            if (!IsRunning)
                return;
            devices.Crate.WriteCoeffs(Reg_adr_coef, A1, B1);
            devices.Crate.WriteCoeffs(Reg_adr_coef + 4, A2, B2);



            #region Проверка настройки канала


            Thread.Sleep(400);
            SetVolt(Point_1);
            Thread.Sleep(5000);
            float realPoint_1 = (float)(devices.multimeter.GetVoltage(PortMultimeter.SIGNALTYPE_AC, 100) * 1000.0);
            float readedValue = (ushort)devices.Crate.ReadReg(Reg_adr) / 100f;

            readedValue = (ushort)devices.Crate.ReadReg(Reg_adr) / 100f;

            float relative = Math.Round(realPoint_1) >= 1000 ? ((readedValue * coef) - realPoint_1) / realPoint_1 * 100 : ((readedValue * coef) - realPoint_1) / 1000 * 100;//расчет погрешности на текущей точке. относительная / приведенная 

            if (Math.Abs(relative) >= 1)
            {
                DevicesCommunication.Log.Write($"Не получилось настроить ускорение устройства {relative} >= 1." +
                    $" канал: {Channel},значение регистра: {readedValue},значение регистра без преобразования: {(ushort)devices.Crate.ReadReg(Reg_adr)},точка: {Point_1},коэффициент: {coef},прочтено с прибора: {realPoint_1}, погрешность: {relative}");

                result = MessageBox.Show($"Ускорение канала {Channel} настроено не корректно. Значение отклонено на {Math.Round(relative, 2)}%\nПродолжить настройку?",
                    "Результат", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Cancel)
                {
                    IsRunning = false;
                }
                return;
            }

            SetVolt(Point_2);
            Thread.Sleep(5000);
            float realPoint_2 = (float)(devices.multimeter.GetVoltage(PortMultimeter.SIGNALTYPE_AC, 100) * 1000.0);

            // перенастройка перемещения.костыль
            readedValue = (ushort)devices.Crate.ReadReg(Reg_adr + 6) / 10f;
            relative = Point_2 >= 1000 ? ((readedValue * coef / 4) - realPoint_2) / realPoint_2 * 100 : ((readedValue * coef / 4) - realPoint_2) / 1000 * 100;//расчет погрешности на текущей точке. относительная / приведенная 

            if (relative <= -1)
            {
                DevicesCommunication.Log.Write($"Переписывание коэффициента перемещения");
                devices.Crate.WriteCoeffs(Reg_adr_coef + 4, A2 + 0.04f, B2);
            }
            else if (relative >= 1)
            {
                DevicesCommunication.Log.Write($"Переписывание коэффициента перемещения");
                devices.Crate.WriteCoeffs(Reg_adr_coef + 4, A2 - 0.02f, B2);
            }


            readedValue = (ushort)devices.Crate.ReadReg(Reg_adr) / 100f;
            relative = Math.Round(realPoint_2) >= 1000 ? ((readedValue * coef) - realPoint_2) / realPoint_2 * 100 : ((readedValue * coef) - realPoint_2) / 1000 * 100;//расчет погрешности на текущей точке. относительная / приведенная 

            if (Math.Abs(relative) >= 1)
            {
                DevicesCommunication.Log.Write($"Не получилось настроить ускорение устройства {relative} >= 1." +
                    $" канал: {Channel},значение регистра: {readedValue},значение регистра без преобразования: {(ushort)devices.Crate.ReadReg(Reg_adr)},точка: {Point_2},коэффициент: {coef},прочтено с прибора: {realPoint_2}, погрешность: {relative}");

                result = MessageBox.Show($"Ускорение канала {Channel} настроено не корректно. Значение отклонено на {Math.Round(relative, 2)}%\nПродолжить настройку?",
                    "Результат", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Cancel)
                {
                    IsRunning = false;
                }
                return;
            }

            readedValue = (ushort)devices.Crate.ReadReg(Reg_adr + 3) / 100f;
            relative = Math.Round(realPoint_2) >= 1000 ? ((readedValue * coef / 2) - realPoint_2) / realPoint_2 * 100 : ((readedValue * coef / 2) - realPoint_2) / 1000 * 100;//расчет погрешности на текущей точке. относительная / приведенная 

            if (Math.Abs(relative) >= 1)
            {
                DevicesCommunication.Log.Write($"Не получилось настроить ускорение устройства {relative} >= 1." +
                    $" канал: {Channel},значение регистра: {readedValue},значение регистра без преобразования: {(ushort)devices.Crate.ReadReg(Reg_adr)},точка: {Point_2},коэффициент: {coef},прочтено с прибора: {realPoint_2}, погрешность: {relative}");

                result = MessageBox.Show($"Скорость канала {Channel} настроена не корректно. Значение отклонено на {Math.Round(relative, 2)}%\nПродолжить настройку?",
                   "Результат", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Cancel)
                {
                    IsRunning = false;
                }
                return;
            }
            readedValue = (ushort)devices.Crate.ReadReg(Reg_adr + 6) / 10f;
            relative = Math.Round(realPoint_2) >= 1000 ? ((readedValue * coef / 4) - realPoint_2) / realPoint_2 * 100 : ((readedValue * coef / 4) - realPoint_2) / 1000 * 100;//расчет погрешности на текущей точке. относительная / приведенная 

            if (Math.Abs(relative) >= 1)
            {
                DevicesCommunication.Log.Write($"Не получилось настроить ускорение устройства {relative} >= 1." +
                    $" канал: {Channel},значение регистра: {readedValue},значение регистра без преобразования: {(ushort)devices.Crate.ReadReg(Reg_adr)},точка: {Point_2},коэффициент: {coef},прочтено с прибора: {realPoint_2}, погрешность: {relative}");

                result = MessageBox.Show($"Перемещение канала {Channel} настроена не корректно. Значение отклонено на {Math.Round(relative, 2)}%\nПродолжить настройку?",
                   "Результат", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Cancel)
                {
                    IsRunning = false;
                }
                return;
            }
            MessageBox.Show($"Канал {Channel} настроен", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            #endregion
        }

        private bool Correct(double mA)
        {
            bool isCorrectValue = false;

            do
            {
                DialogResult result = MessageBox.Show($"При помощи магазина сопротивлений задайте {mA} В", "Проверка",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Cancel)
                    return false;
                else if (result == DialogResult.OK)
                {
                    if (devices.currentVolt > mA - 0.02 && devices.currentVolt < mA + 0.02)
                        return isCorrectValue = true;
                    else
                        MessageBox.Show($"Неправильное значение! Пожалуйста, установите напряжение в диапазоне от {mA - 0.02} до {mA + 0.02} В.");
                    //if (Worker.CancellationPending)
                    //    return;
                }
            } while (!isCorrectValue);
            return false;
        }
        private void SetCoef_I(int Channel)
        {
            DevicesCommunication.Log.Write($"Настройка канала {Channel} / I");
            IsRunning = true;
            if (MessageBox.Show($"Установите контакты для настройки 4-20 мА {Channel}-го канала", "Настройка 4-20 мА",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
            {
                devices.DC_Read = false;
                Thread.Sleep(1000);
                devices.multimeter.VoltmeterMode(PortMultimeter.SIGNALTYPE_DC);
                devices.DC_Read = true;

                float I1 = 0, I2 = 0;
                Set_reg_adr(Channel);
                devices.Crate.ResetCoef(Reg_adr_coef + 8, Reg_adr_on, 4);
                if (Correct(0.4d))
                {
                    I1 = devices.Crate.ReadValue(Reg_adr + 8);
                    if (Correct(2d))
                    {
                        devices.Crate.SetPassword();
                        I2 = devices.Crate.ReadValue(Reg_adr + 8);
                        devices.Crate.WriteReg(Reg_adr_coef + 8, ModbusClient.ConvertFloatToRegisters(Convert.ToSingle(16 / (I2 - I1))));
                        Thread.Sleep(1000);
                        devices.Crate.WriteReg(Reg_adr_coef + 10, ModbusClient.ConvertFloatToRegisters(Convert.ToSingle(4 - (16 / (I2 - I1)) * I1)));
                        Thread.Sleep(1000);
                        MessageBox.Show($"4-20 мА канала {Channel} настроено", "Результат", MessageBoxButtons.OK,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
            }
        }
        private void SetCoef_T(int Channel, int type_Termo)
        {
            DevicesCommunication.Log.Write($"Настройка канала {Channel} / T");
            float Resist_1 = 0;
            float Resist_2 = 0;
            int Need_T_1 = 0;
            int Need_T_2 = 0;
            float Readed_T = 0f;
            // Взято с https://cdn.termexlab.ru/files/9e48a99f/9525/4985/9662/23c12e3a32c2.pdf
            switch (type_Termo)
            {
                case 1:
                    Resist_1 = 10.265f;
                    Resist_2 = 92.8f;
                    Need_T_1 = -180;
                    Need_T_2 = 200;
                    break;
                case 2:
                    Resist_1 = 20.53f;
                    Resist_2 = 185.6f;
                    Need_T_1 = -180;
                    Need_T_2 = 200;
                    break;
                case 3:
                    Resist_1 = 39.35f;
                    Resist_2 = 92.6f;
                    Need_T_1 = -50;
                    Need_T_2 = 200;
                    break;
                case 4:
                    Resist_1 = 78.7f;
                    Resist_2 = 185.2f;
                    Need_T_1 = -50;
                    Need_T_2 = 200;
                    break;
                case 5:
                    Resist_1 = 8.62f;
                    Resist_2 = 197.58f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 6:
                    Resist_1 = 17.24f;
                    Resist_2 = 395.16f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 7:
                    Resist_1 = 9.26f;
                    Resist_2 = 195.24f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;
                case 8:
                    Resist_1 = 18.52f;
                    Resist_2 = 390.48f;
                    Need_T_1 = -200;
                    Need_T_2 = 850;
                    break;

            }
            IsRunning = true;
            if (MessageBox.Show($"Установите контакты для настройки термопреобразователя {Channel}-го канала",
                "Настройка термопреобразователя", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
            {
                //Creyt.Connect();
                float T1 = 0, T2 = 0;
                Set_reg_adr(Channel);
                devices.Crate.ResetCoef(Reg_adr_coef + 12, 4);
                devices.Crate.WriteReg(Reg_adr_coef + 16, type_Termo);

                if (MessageBox.Show("Установите на магазине сопротивлений 100 Ом", "Настройка термопреобразователя",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
                {
                    Thread.Sleep(15000); // стоит потому что долго обновляется значение в крейте
                    T1 = ((ushort)(devices.Crate.ReadReg(Reg_adr + 14))) / 1000f;
                    if (MessageBox.Show("Установите на магазине сопротивлений 400 Ом", "Настройка термопреобразователя",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
                    {
                        Thread.Sleep(25000);// стоит потому что долго обновляется значение в крейте
                        devices.Crate.SetPassword();
                        T2 = ((ushort)(devices.Crate.ReadReg(Reg_adr + 14))) / 1000f;
                        devices.Crate.WriteReg(Reg_adr_coef + 12, ModbusClient.ConvertFloatToRegisters(Convert.ToSingle(3 / (T2 - T1))));
                        Thread.Sleep(1000);
                        devices.Crate.WriteReg(Reg_adr_coef + 14, ModbusClient.ConvertFloatToRegisters(Convert.ToSingle(1 - (3 / (T2 - T1)) * T1)));
                        if (MessageBox.Show($"Установите на магазине сопротивлений {Resist_1} Ом", "Настройка термопреобразователя",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
                        {
                            Thread.Sleep(15000);// стоит потому что долго обновляется значение в крейте
                            Readed_T = devices.Crate.ReadReg(Reg_adr + 10) / 10f;

                            if (Readed_T < Need_T_1 + 1 && Readed_T > Need_T_1 - 1)
                            {
                                if (MessageBox.Show($"Установите на магазине сопротивлений {Resist_2} Ом", "Настройка термопреобразователя",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
                                {
                                    Thread.Sleep(15000);// стоит потому что долго обновляется значение в крейте
                                    Readed_T = devices.Crate.ReadReg(Reg_adr + 10) / 10f;
                                    if (Readed_T < Need_T_2 + 1 && Readed_T > Need_T_2 - 1)
                                    {
                                        MessageBox.Show($"Термопреобразователь канала {Channel} настроено", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                    else
                                    {
                                        MessageBox.Show($"точка 2 не прошла проверку, значение отклонено от нормы на {Readed_T - Need_T_2}", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        return;
                                    }
                                }
                                else return;
                            }
                            else
                            {
                                MessageBox.Show($"точка 1 не прошла проверку, значение отклонено от нормы на {Readed_T - Need_T_1}", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                return;
                            }
                        }
                        else return;
                    }
                    else return;
                }
                else return;
            }
        }
        public void Run_Setting(int PLC, int Type_Termo)
        {
            try
            {
                devices.generator.SetFrequency(frequency);
                switch (PLC)
                {
                    case 0://241
                        SetCoef_IEPE(1);
                        if (!IsRunning)
                            return;
                        SetCoef_I(2);
                        devices.Crate.WriteReg(Registers.REGISTER_COEFFICIENT, ModbusClient.ConvertFloatToRegisters(coef));
                        break;
                    case 1://242
                        SetCoef_IEPE(1);
                        if (!IsRunning)
                            return;
                        SetCoef_IEPE(2);
                        devices.Crate.WriteReg(Registers.REGISTER_COEFFICIENT, ModbusClient.ConvertFloatToRegisters(coef));
                        break;
                    case 2://243
                        SetCoef_I(1);
                        if (!IsRunning)
                            return;
                        SetCoef_I(2);
                        break;
                    case 3://511

                        break;
                    case 4://371
                        SetCoef_IEPE(1);
                        if (!IsRunning)
                            return;
                        SetCoef_I(2);
                        if (!IsRunning)
                            return;
                        SetCoef_T(3, Type_Termo);
                        devices.Crate.WriteReg(Registers.REGISTER_COEFFICIENT, ModbusClient.ConvertFloatToRegisters(coef));
                        break;
                    case 5://374
                        SetCoef_I(1);
                        if (!IsRunning)
                            return;
                        SetCoef_I(2);
                        if (!IsRunning)
                            return;
                        SetCoef_I(3);
                        break;
                    case 6://375
                        SetCoef_IEPE(1);
                        if (!IsRunning)
                            return;
                        SetCoef_I(2);
                        if (!IsRunning)
                            return;
                        SetCoef_I(3);
                        devices.Crate.WriteReg(Registers.REGISTER_COEFFICIENT, ModbusClient.ConvertFloatToRegisters(coef));
                        break;
                }
                MakeReport(PLC);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void MakeReport(int PLC)
        {
            string date = String.Format("{0}.{1}.{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            string time = String.Format("{0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
            int serialNum = devices.Crate.ReadReg(Registers.REGISTER_SERIAL_NUM_READ);
            string coefficient = devices.Crate.ReadReg(Registers.REGISTER_COEFFICIENT).ToString();
            string plcType = "---";
            Set_reg_adr(1);
            string a1 = devices.Crate.ReadValue(Reg_adr_coef).ToString();
            string b1 = devices.Crate.ReadValue(Reg_adr_coef + 2).ToString();
            Set_reg_adr(2);
            string a2 = devices.Crate.ReadValue(Reg_adr_coef).ToString();
            string b2 = devices.Crate.ReadValue(Reg_adr_coef + 2).ToString();

            string a3 = "---";
            string b3 = "---";
            string a4 = "---";
            string b4 = "---";
            string type1 = "---";
            string type2 = "---";
            string type3 = "---";
            string type4 = "---";

            switch (PLC)
            {
                case 0: //241
                    plcType = "241";
                    type1 = "IEPE";
                    type2 = "4-20";
                    break;
                case 1: //242
                    plcType = "242";
                    type1 = "IEPE";
                    type2 = "IEPE";
                    break;
                case 2: //243
                    plcType = "243";
                    type1 = "4-20";
                    type2 = "4-20";
                    break;
                case 3: //511
                    plcType = "511";
                    type1 = "Напряжение";
                    type2 = "Напряжение";
                    type3 = "Напряжение";
                    type4 = "Напряжение";
                    Set_reg_adr(3);
                    a3 = devices.Crate.ReadValue(Reg_adr_coef).ToString();
                    b3 = devices.Crate.ReadValue(Reg_adr_coef + 2).ToString();
                    Set_reg_adr(4);
                    a4 = devices.Crate.ReadValue(Reg_adr_coef).ToString();
                    b4 = devices.Crate.ReadValue(Reg_adr_coef + 2).ToString();
                    break;
                case 4: //371
                    plcType = "371";
                    type1 = "IEPE";
                    type2 = "4-20";
                    type3 = "Температура";
                    Set_reg_adr(3);
                    a3 = devices.Crate.ReadValue(Reg_adr_coef).ToString();
                    b3 = devices.Crate.ReadValue(Reg_adr_coef + 2).ToString();
                    break;
                case 5: //374
                    plcType = "374";
                    type1 = "4-20";
                    type2 = "4-20";
                    type3 = "4-20";
                    Set_reg_adr(3);
                    a3 = devices.Crate.ReadValue(Reg_adr_coef).ToString();
                    b3 = devices.Crate.ReadValue(Reg_adr_coef + 2).ToString();
                    break;
                case 6: //375
                    plcType = "375";
                    type1 = "IEPE";
                    type2 = "4-20";
                    type3 = "4-20";
                    Set_reg_adr(3);
                    a3 = devices.Crate.ReadValue(Reg_adr_coef).ToString();
                    b3 = devices.Crate.ReadValue(Reg_adr_coef + 2).ToString();
                    break;
            }
            string line = $"{date};{time};{orderNum};{serialNum};{plcType};{coefficient};{type1};{a1};{b1};{type2};{a2};{b2};{type3};{a3};{b3};{type4};{a4};{b4}\r\n";

            string fileName = "Log//" + orderNum + ".csv";
            WriteLineToFile(line, fileName);

            fileName = "\\\\files\\Общее\\Прошивки и методики проверки\\Прикладное ПО\\АРМ настройки крейтов\\CommonLogs\\" + orderNum + ".csv";
            WriteLineToFile(line, fileName);
        }

        private void WriteLineToFile(string line, string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllBytes(fileName, new byte[3] { 0xEF, 0xBB, 0xBF }); //указание на utf-8
                File.AppendAllText(fileName, "Дата;Время;№ заказа;Серийный №;PLC;Коэфф;Канал 1;Коэфф А;Коэфф В;" +
                    "Канал 2;Коэфф А;Коэфф В;Канал 3;Коэфф А;Коэфф В\r\n");
            }
            File.AppendAllText(fileName, line);
        }
    }
}
