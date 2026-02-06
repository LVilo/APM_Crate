namespace AutoSet
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Worker = new System.ComponentModel.BackgroundWorker();
            this.Start = new System.Windows.Forms.Button();
            this.Disconnect_Agillent = new System.Windows.Forms.Button();
            this.Connect_Agillent = new System.Windows.Forms.Button();
            this.AgillentGroupBox = new System.Windows.Forms.GroupBox();
            this.labelVmAddr = new System.Windows.Forms.Label();
            this.AgillentPortBox = new System.Windows.Forms.ComboBox();
            this.UpdatePort = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.CoefTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.Disconnect_Generator = new System.Windows.Forms.Button();
            this.Connect_Generator = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelGenChannel = new System.Windows.Forms.Label();
            this.labelGenAddr = new System.Windows.Forms.Label();
            this.C2 = new System.Windows.Forms.CheckBox();
            this.C1 = new System.Windows.Forms.CheckBox();
            this.comboBoxGenerator = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Save_IP = new System.Windows.Forms.Button();
            this.IEPE = new System.Windows.Forms.GroupBox();
            this.SPD2 = new System.Windows.Forms.Label();
            this.ACC2 = new System.Windows.Forms.Label();
            this.SPD1 = new System.Windows.Forms.Label();
            this.ACC1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DC_textBox = new System.Windows.Forms.TextBox();
            this.ResetCoef_2 = new System.Windows.Forms.Button();
            this.ReserCoef_1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Point_2_textBox = new System.Windows.Forms.TextBox();
            this.Point_1_textBox = new System.Windows.Forms.TextBox();
            this.STOP = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonReport = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.FreqTextBox = new System.Windows.Forms.TextBox();
            this.Termo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.PLC_AutoCheck = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PLC = new System.Windows.Forms.ComboBox();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.CheckSet = new System.Windows.Forms.GroupBox();
            this.Lvl2_textbox = new System.Windows.Forms.TextBox();
            this.Lvl1_textbox = new System.Windows.Forms.TextBox();
            this.lvl2 = new System.Windows.Forms.Button();
            this.lvl1 = new System.Windows.Forms.Button();
            this.groupBoxOrder = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxSerialNum = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxOrderNum = new System.Windows.Forms.TextBox();
            this.AgillentGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.IEPE.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.CheckSet.SuspendLayout();
            this.groupBoxOrder.SuspendLayout();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(105, 242);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(84, 44);
            this.Start.TabIndex = 0;
            this.Start.Text = "Старт";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Disconnect_Agillent
            // 
            this.Disconnect_Agillent.Location = new System.Drawing.Point(105, 45);
            this.Disconnect_Agillent.Name = "Disconnect_Agillent";
            this.Disconnect_Agillent.Size = new System.Drawing.Size(84, 23);
            this.Disconnect_Agillent.TabIndex = 85;
            this.Disconnect_Agillent.Text = "Отключить";
            this.Disconnect_Agillent.UseVisualStyleBackColor = true;
            this.Disconnect_Agillent.Click += new System.EventHandler(this.Disconnect_Agillent_Click);
            // 
            // Connect_Agillent
            // 
            this.Connect_Agillent.Location = new System.Drawing.Point(15, 45);
            this.Connect_Agillent.Name = "Connect_Agillent";
            this.Connect_Agillent.Size = new System.Drawing.Size(84, 23);
            this.Connect_Agillent.TabIndex = 84;
            this.Connect_Agillent.Text = "Подключить";
            this.Connect_Agillent.UseVisualStyleBackColor = true;
            this.Connect_Agillent.Click += new System.EventHandler(this.Connect_Agillent_Click);
            // 
            // AgillentGroupBox
            // 
            this.AgillentGroupBox.Controls.Add(this.labelVmAddr);
            this.AgillentGroupBox.Controls.Add(this.AgillentPortBox);
            this.AgillentGroupBox.Controls.Add(this.Disconnect_Agillent);
            this.AgillentGroupBox.Controls.Add(this.Connect_Agillent);
            this.AgillentGroupBox.Location = new System.Drawing.Point(415, 158);
            this.AgillentGroupBox.Name = "AgillentGroupBox";
            this.AgillentGroupBox.Size = new System.Drawing.Size(195, 75);
            this.AgillentGroupBox.TabIndex = 83;
            this.AgillentGroupBox.TabStop = false;
            this.AgillentGroupBox.Text = "Порт вольтметра";
            // 
            // labelVmAddr
            // 
            this.labelVmAddr.AutoSize = true;
            this.labelVmAddr.Location = new System.Drawing.Point(12, 17);
            this.labelVmAddr.Name = "labelVmAddr";
            this.labelVmAddr.Size = new System.Drawing.Size(41, 13);
            this.labelVmAddr.TabIndex = 86;
            this.labelVmAddr.Text = "Адрес:";
            // 
            // AgillentPortBox
            // 
            this.AgillentPortBox.FormattingEnabled = true;
            this.AgillentPortBox.Location = new System.Drawing.Point(105, 14);
            this.AgillentPortBox.Name = "AgillentPortBox";
            this.AgillentPortBox.Size = new System.Drawing.Size(84, 21);
            this.AgillentPortBox.TabIndex = 6;
            this.AgillentPortBox.SelectedIndexChanged += new System.EventHandler(this.AgillentPortBox_SelectedIndexChanged);
            // 
            // UpdatePort
            // 
            this.UpdatePort.Location = new System.Drawing.Point(415, 19);
            this.UpdatePort.Name = "UpdatePort";
            this.UpdatePort.Size = new System.Drawing.Size(195, 23);
            this.UpdatePort.TabIndex = 104;
            this.UpdatePort.Text = "Обновить порты";
            this.UpdatePort.UseVisualStyleBackColor = true;
            this.UpdatePort.Click += new System.EventHandler(this.UpdatePort_Click);
            // 
            // LogTextBox
            // 
            this.LogTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.LogTextBox.Location = new System.Drawing.Point(214, 239);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogTextBox.Size = new System.Drawing.Size(396, 314);
            this.LogTextBox.TabIndex = 89;
            // 
            // CoefTextBox
            // 
            this.CoefTextBox.Location = new System.Drawing.Point(105, 46);
            this.CoefTextBox.Name = "CoefTextBox";
            this.CoefTextBox.Size = new System.Drawing.Size(84, 20);
            this.CoefTextBox.TabIndex = 94;
            this.CoefTextBox.Text = "10";
            this.CoefTextBox.TextChanged += new System.EventHandler(this.CoefTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 95;
            this.label1.Text = "Коэффициент:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 99;
            this.label6.Text = "IP крейта:";
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(105, 14);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(84, 20);
            this.IPTextBox.TabIndex = 98;
            this.IPTextBox.TextChanged += new System.EventHandler(this.IPTextBox_TextChanged);
            // 
            // Disconnect_Generator
            // 
            this.Disconnect_Generator.Location = new System.Drawing.Point(105, 72);
            this.Disconnect_Generator.Name = "Disconnect_Generator";
            this.Disconnect_Generator.Size = new System.Drawing.Size(84, 23);
            this.Disconnect_Generator.TabIndex = 104;
            this.Disconnect_Generator.Text = "Отключить";
            this.Disconnect_Generator.UseVisualStyleBackColor = true;
            this.Disconnect_Generator.Click += new System.EventHandler(this.Disconnect_Generator_Click);
            // 
            // Connect_Generator
            // 
            this.Connect_Generator.Location = new System.Drawing.Point(15, 72);
            this.Connect_Generator.Name = "Connect_Generator";
            this.Connect_Generator.Size = new System.Drawing.Size(84, 23);
            this.Connect_Generator.TabIndex = 103;
            this.Connect_Generator.Text = "Подключить";
            this.Connect_Generator.UseVisualStyleBackColor = true;
            this.Connect_Generator.Click += new System.EventHandler(this.Connect_Generator_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelGenChannel);
            this.groupBox1.Controls.Add(this.labelGenAddr);
            this.groupBox1.Controls.Add(this.C2);
            this.groupBox1.Controls.Add(this.C1);
            this.groupBox1.Controls.Add(this.Disconnect_Generator);
            this.groupBox1.Controls.Add(this.Connect_Generator);
            this.groupBox1.Controls.Add(this.comboBoxGenerator);
            this.groupBox1.Location = new System.Drawing.Point(414, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 104);
            this.groupBox1.TabIndex = 102;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Порт генератора";
            // 
            // labelGenChannel
            // 
            this.labelGenChannel.AutoSize = true;
            this.labelGenChannel.Location = new System.Drawing.Point(12, 50);
            this.labelGenChannel.Name = "labelGenChannel";
            this.labelGenChannel.Size = new System.Drawing.Size(41, 13);
            this.labelGenChannel.TabIndex = 116;
            this.labelGenChannel.Text = "Канал:";
            // 
            // labelGenAddr
            // 
            this.labelGenAddr.AutoSize = true;
            this.labelGenAddr.Location = new System.Drawing.Point(12, 17);
            this.labelGenAddr.Name = "labelGenAddr";
            this.labelGenAddr.Size = new System.Drawing.Size(41, 13);
            this.labelGenAddr.TabIndex = 115;
            this.labelGenAddr.Text = "Адрес:";
            // 
            // C2
            // 
            this.C2.AutoSize = true;
            this.C2.Location = new System.Drawing.Point(150, 49);
            this.C2.Name = "C2";
            this.C2.Size = new System.Drawing.Size(39, 17);
            this.C2.TabIndex = 114;
            this.C2.Text = "С2";
            this.C2.UseVisualStyleBackColor = true;
            this.C2.CheckedChanged += new System.EventHandler(this.C2_CheckedChanged);
            // 
            // C1
            // 
            this.C1.AutoSize = true;
            this.C1.Checked = true;
            this.C1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.C1.Location = new System.Drawing.Point(105, 49);
            this.C1.Name = "C1";
            this.C1.Size = new System.Drawing.Size(39, 17);
            this.C1.TabIndex = 113;
            this.C1.Text = "С1";
            this.C1.UseVisualStyleBackColor = true;
            this.C1.CheckedChanged += new System.EventHandler(this.C1_CheckedChanged);
            // 
            // comboBoxGenerator
            // 
            this.comboBoxGenerator.FormattingEnabled = true;
            this.comboBoxGenerator.Location = new System.Drawing.Point(105, 16);
            this.comboBoxGenerator.Name = "comboBoxGenerator";
            this.comboBoxGenerator.Size = new System.Drawing.Size(84, 21);
            this.comboBoxGenerator.TabIndex = 93;
            this.comboBoxGenerator.SelectedIndexChanged += new System.EventHandler(this.comboBoxGenerator_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Save_IP);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.IPTextBox);
            this.groupBox2.Location = new System.Drawing.Point(214, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 78);
            this.groupBox2.TabIndex = 103;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Крейт";
            // 
            // Save_IP
            // 
            this.Save_IP.Location = new System.Drawing.Point(105, 40);
            this.Save_IP.Name = "Save_IP";
            this.Save_IP.Size = new System.Drawing.Size(84, 23);
            this.Save_IP.TabIndex = 113;
            this.Save_IP.Text = "Сохранить IP";
            this.Save_IP.UseVisualStyleBackColor = true;
            this.Save_IP.Click += new System.EventHandler(this.Save_IP_Click);
            // 
            // IEPE
            // 
            this.IEPE.Controls.Add(this.SPD2);
            this.IEPE.Controls.Add(this.ACC2);
            this.IEPE.Controls.Add(this.SPD1);
            this.IEPE.Controls.Add(this.ACC1);
            this.IEPE.Controls.Add(this.label8);
            this.IEPE.Controls.Add(this.label9);
            this.IEPE.Controls.Add(this.label10);
            this.IEPE.Controls.Add(this.label11);
            this.IEPE.Location = new System.Drawing.Point(12, 12);
            this.IEPE.Name = "IEPE";
            this.IEPE.Size = new System.Drawing.Size(195, 119);
            this.IEPE.TabIndex = 114;
            this.IEPE.TabStop = false;
            this.IEPE.Text = "IEPE";
            // 
            // SPD2
            // 
            this.SPD2.AutoSize = true;
            this.SPD2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SPD2.Location = new System.Drawing.Point(102, 98);
            this.SPD2.Name = "SPD2";
            this.SPD2.Size = new System.Drawing.Size(14, 15);
            this.SPD2.TabIndex = 11;
            this.SPD2.Text = "0";
            // 
            // ACC2
            // 
            this.ACC2.AutoSize = true;
            this.ACC2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ACC2.Location = new System.Drawing.Point(102, 72);
            this.ACC2.Name = "ACC2";
            this.ACC2.Size = new System.Drawing.Size(14, 15);
            this.ACC2.TabIndex = 10;
            this.ACC2.Text = "0";
            // 
            // SPD1
            // 
            this.SPD1.AutoSize = true;
            this.SPD1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SPD1.Location = new System.Drawing.Point(102, 46);
            this.SPD1.Name = "SPD1";
            this.SPD1.Size = new System.Drawing.Size(14, 15);
            this.SPD1.TabIndex = 9;
            this.SPD1.Text = "0";
            // 
            // ACC1
            // 
            this.ACC1.AutoSize = true;
            this.ACC1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ACC1.Location = new System.Drawing.Point(102, 20);
            this.ACC1.Name = "ACC1";
            this.ACC1.Size = new System.Drawing.Size(14, 15);
            this.ACC1.TabIndex = 8;
            this.ACC1.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Скорость 2К:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Ускорение 2К:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Скорость 1К:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Ускорение 1К:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 108;
            this.label4.Text = "DC (В):";
            // 
            // DC_textBox
            // 
            this.DC_textBox.Location = new System.Drawing.Point(105, 102);
            this.DC_textBox.Name = "DC_textBox";
            this.DC_textBox.Size = new System.Drawing.Size(84, 20);
            this.DC_textBox.TabIndex = 107;
            this.DC_textBox.Text = "10";
            this.DC_textBox.TextChanged += new System.EventHandler(this.DC_textBox_TextChanged);
            // 
            // ResetCoef_2
            // 
            this.ResetCoef_2.Location = new System.Drawing.Point(105, 208);
            this.ResetCoef_2.Name = "ResetCoef_2";
            this.ResetCoef_2.Size = new System.Drawing.Size(84, 28);
            this.ResetCoef_2.TabIndex = 106;
            this.ResetCoef_2.Text = "Сброс IEPE2";
            this.ResetCoef_2.UseVisualStyleBackColor = true;
            this.ResetCoef_2.Click += new System.EventHandler(this.ResetCoef_2_Click);
            // 
            // ReserCoef_1
            // 
            this.ReserCoef_1.Location = new System.Drawing.Point(15, 208);
            this.ReserCoef_1.Name = "ReserCoef_1";
            this.ReserCoef_1.Size = new System.Drawing.Size(84, 28);
            this.ReserCoef_1.TabIndex = 105;
            this.ReserCoef_1.Text = "Сброс IEPE1";
            this.ReserCoef_1.UseVisualStyleBackColor = true;
            this.ReserCoef_1.Click += new System.EventHandler(this.ResetCoef_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 104;
            this.label3.Text = "Точка 2 (мВ):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 103;
            this.label2.Text = "Точка 1 (мВ):";
            // 
            // Point_2_textBox
            // 
            this.Point_2_textBox.Location = new System.Drawing.Point(105, 155);
            this.Point_2_textBox.Name = "Point_2_textBox";
            this.Point_2_textBox.Size = new System.Drawing.Size(84, 20);
            this.Point_2_textBox.TabIndex = 102;
            this.Point_2_textBox.Text = "3000";
            this.Point_2_textBox.TextChanged += new System.EventHandler(this.Point_2_textBox_TextChanged);
            // 
            // Point_1_textBox
            // 
            this.Point_1_textBox.Location = new System.Drawing.Point(105, 130);
            this.Point_1_textBox.Name = "Point_1_textBox";
            this.Point_1_textBox.Size = new System.Drawing.Size(84, 20);
            this.Point_1_textBox.TabIndex = 101;
            this.Point_1_textBox.Text = "10";
            this.Point_1_textBox.TextChanged += new System.EventHandler(this.Point_1_textBox_TextChanged);
            // 
            // STOP
            // 
            this.STOP.Location = new System.Drawing.Point(15, 242);
            this.STOP.Name = "STOP";
            this.STOP.Size = new System.Drawing.Size(84, 44);
            this.STOP.TabIndex = 100;
            this.STOP.Text = "Стоп";
            this.STOP.UseVisualStyleBackColor = true;
            this.STOP.Click += new System.EventHandler(this.STOP_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonReport);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.FreqTextBox);
            this.groupBox3.Controls.Add(this.Termo);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.PLC_AutoCheck);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.PLC);
            this.groupBox3.Controls.Add(this.Start);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.STOP);
            this.groupBox3.Controls.Add(this.CoefTextBox);
            this.groupBox3.Controls.Add(this.DC_textBox);
            this.groupBox3.Controls.Add(this.Point_1_textBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.ResetCoef_2);
            this.groupBox3.Controls.Add(this.Point_2_textBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.ReserCoef_1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(195, 324);
            this.groupBox3.TabIndex = 109;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройка";
            // 
            // buttonReport
            // 
            this.buttonReport.Location = new System.Drawing.Point(15, 292);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(174, 23);
            this.buttonReport.TabIndex = 115;
            this.buttonReport.Text = "Записать в лог";
            this.buttonReport.UseVisualStyleBackColor = true;
            this.buttonReport.Click += new System.EventHandler(this.buttonReport_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 79);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 13);
            this.label12.TabIndex = 114;
            this.label12.Text = "Частота";
            // 
            // FreqTextBox
            // 
            this.FreqTextBox.Location = new System.Drawing.Point(105, 76);
            this.FreqTextBox.Name = "FreqTextBox";
            this.FreqTextBox.Size = new System.Drawing.Size(84, 20);
            this.FreqTextBox.TabIndex = 113;
            this.FreqTextBox.Text = "80";
            this.FreqTextBox.TextChanged += new System.EventHandler(this.FreqTextBox_TextChanged);
            // 
            // Termo
            // 
            this.Termo.Location = new System.Drawing.Point(105, 181);
            this.Termo.Name = "Termo";
            this.Termo.Size = new System.Drawing.Size(84, 20);
            this.Termo.TabIndex = 111;
            this.Termo.Text = "8";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 112;
            this.label7.Text = "Датчик темпер";
            // 
            // PLC_AutoCheck
            // 
            this.PLC_AutoCheck.AutoSize = true;
            this.PLC_AutoCheck.Checked = true;
            this.PLC_AutoCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PLC_AutoCheck.Location = new System.Drawing.Point(49, 18);
            this.PLC_AutoCheck.Name = "PLC_AutoCheck";
            this.PLC_AutoCheck.Size = new System.Drawing.Size(50, 17);
            this.PLC_AutoCheck.TabIndex = 110;
            this.PLC_AutoCheck.Text = "Авто";
            this.PLC_AutoCheck.UseVisualStyleBackColor = true;
            this.PLC_AutoCheck.CheckedChanged += new System.EventHandler(this.PLC_AutoCheck_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 109;
            this.label5.Text = "PLC";
            // 
            // PLC
            // 
            this.PLC.Enabled = false;
            this.PLC.FormattingEnabled = true;
            this.PLC.Location = new System.Drawing.Point(105, 19);
            this.PLC.Name = "PLC";
            this.PLC.Size = new System.Drawing.Size(84, 21);
            this.PLC.TabIndex = 91;
            this.PLC.SelectedIndexChanged += new System.EventHandler(this.PLC_SelectedIndexChanged);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PLC_Updater);
            // 
            // CheckSet
            // 
            this.CheckSet.Controls.Add(this.Lvl2_textbox);
            this.CheckSet.Controls.Add(this.Lvl1_textbox);
            this.CheckSet.Controls.Add(this.lvl2);
            this.CheckSet.Controls.Add(this.lvl1);
            this.CheckSet.Location = new System.Drawing.Point(12, 144);
            this.CheckSet.Name = "CheckSet";
            this.CheckSet.Size = new System.Drawing.Size(195, 80);
            this.CheckSet.TabIndex = 111;
            this.CheckSet.TabStop = false;
            this.CheckSet.Text = "Проверка";
            // 
            // Lvl2_textbox
            // 
            this.Lvl2_textbox.Location = new System.Drawing.Point(105, 23);
            this.Lvl2_textbox.Name = "Lvl2_textbox";
            this.Lvl2_textbox.Size = new System.Drawing.Size(84, 20);
            this.Lvl2_textbox.TabIndex = 9;
            this.Lvl2_textbox.Text = "3000";
            // 
            // Lvl1_textbox
            // 
            this.Lvl1_textbox.Location = new System.Drawing.Point(15, 23);
            this.Lvl1_textbox.Name = "Lvl1_textbox";
            this.Lvl1_textbox.Size = new System.Drawing.Size(84, 20);
            this.Lvl1_textbox.TabIndex = 8;
            this.Lvl1_textbox.Text = "10";
            // 
            // lvl2
            // 
            this.lvl2.Location = new System.Drawing.Point(105, 46);
            this.lvl2.Name = "lvl2";
            this.lvl2.Size = new System.Drawing.Size(84, 23);
            this.lvl2.TabIndex = 1;
            this.lvl2.Text = "Задать 2Ур";
            this.lvl2.UseVisualStyleBackColor = true;
            this.lvl2.Click += new System.EventHandler(this.lvl2_Click);
            // 
            // lvl1
            // 
            this.lvl1.Location = new System.Drawing.Point(15, 46);
            this.lvl1.Name = "lvl1";
            this.lvl1.Size = new System.Drawing.Size(84, 23);
            this.lvl1.TabIndex = 0;
            this.lvl1.Text = "Задать 1Ур";
            this.lvl1.UseVisualStyleBackColor = true;
            this.lvl1.Click += new System.EventHandler(this.lvl1_Click);
            // 
            // groupBoxOrder
            // 
            this.groupBoxOrder.Controls.Add(this.label14);
            this.groupBoxOrder.Controls.Add(this.textBoxSerialNum);
            this.groupBoxOrder.Controls.Add(this.label13);
            this.groupBoxOrder.Controls.Add(this.textBoxOrderNum);
            this.groupBoxOrder.Location = new System.Drawing.Point(213, 12);
            this.groupBoxOrder.Name = "groupBoxOrder";
            this.groupBoxOrder.Size = new System.Drawing.Size(195, 73);
            this.groupBoxOrder.TabIndex = 112;
            this.groupBoxOrder.TabStop = false;
            this.groupBoxOrder.Text = "Заказ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 13);
            this.label14.TabIndex = 103;
            this.label14.Text = "Серийный номер:";
            // 
            // textBoxSerialNum
            // 
            this.textBoxSerialNum.Location = new System.Drawing.Point(105, 40);
            this.textBoxSerialNum.Name = "textBoxSerialNum";
            this.textBoxSerialNum.Size = new System.Drawing.Size(84, 20);
            this.textBoxSerialNum.TabIndex = 102;
            this.textBoxSerialNum.TextChanged += new System.EventHandler(this.textBoxSerialNum_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 13);
            this.label13.TabIndex = 101;
            this.label13.Text = "Номер заказа:";
            // 
            // textBoxOrderNum
            // 
            this.textBoxOrderNum.Location = new System.Drawing.Point(105, 14);
            this.textBoxOrderNum.Name = "textBoxOrderNum";
            this.textBoxOrderNum.Size = new System.Drawing.Size(84, 20);
            this.textBoxOrderNum.TabIndex = 100;
            this.textBoxOrderNum.TextChanged += new System.EventHandler(this.textBoxOrderNum_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 565);
            this.Controls.Add(this.IEPE);
            this.Controls.Add(this.groupBoxOrder);
            this.Controls.Add(this.CheckSet);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.UpdatePort);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.AgillentGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "АРМ проверки и настройки контроллеров ТИК-Крейт";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.AgillentGroupBox.ResumeLayout(false);
            this.AgillentGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.IEPE.ResumeLayout(false);
            this.IEPE.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.CheckSet.ResumeLayout(false);
            this.CheckSet.PerformLayout();
            this.groupBoxOrder.ResumeLayout(false);
            this.groupBoxOrder.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker Worker;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Disconnect_Agillent;
        private System.Windows.Forms.Button Connect_Agillent;
        private System.Windows.Forms.GroupBox AgillentGroupBox;
        private System.Windows.Forms.ComboBox AgillentPortBox;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.TextBox CoefTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Button Disconnect_Generator;
        private System.Windows.Forms.Button Connect_Generator;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxGenerator;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button STOP;
        private System.Windows.Forms.Button UpdatePort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Point_2_textBox;
        private System.Windows.Forms.TextBox Point_1_textBox;
        private System.Windows.Forms.Button ReserCoef_1;
        private System.Windows.Forms.Button ResetCoef_2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DC_textBox;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox PLC;
        private System.Windows.Forms.CheckBox PLC_AutoCheck;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.TextBox Termo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Save_IP;
        private System.Windows.Forms.GroupBox CheckSet;
        private System.Windows.Forms.TextBox Lvl2_textbox;
        private System.Windows.Forms.TextBox Lvl1_textbox;
        private System.Windows.Forms.Button lvl2;
        private System.Windows.Forms.Button lvl1;
        private System.Windows.Forms.GroupBox IEPE;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox C2;
        private System.Windows.Forms.CheckBox C1;
        private System.Windows.Forms.Label SPD2;
        private System.Windows.Forms.Label ACC2;
        private System.Windows.Forms.Label SPD1;
        private System.Windows.Forms.Label ACC1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox FreqTextBox;
        private System.Windows.Forms.Label labelVmAddr;
        private System.Windows.Forms.Label labelGenChannel;
        private System.Windows.Forms.Label labelGenAddr;
        private System.Windows.Forms.GroupBox groupBoxOrder;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxSerialNum;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxOrderNum;
        private System.Windows.Forms.Button buttonReport;
    }
}

