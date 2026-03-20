namespace AutoSet_New
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
            Worker = new System.ComponentModel.BackgroundWorker();
            Start = new Button();
            Disconnect_Agillent = new Button();
            Connect_Agillent = new Button();
            AgillentGroupBox = new GroupBox();
            labelVmAddr = new Label();
            AgillentPortBox = new ComboBox();
            UpdatePort = new Button();
            LogTextBox = new TextBox();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            CoefTextBox = new TextBox();
            label1 = new Label();
            label6 = new Label();
            IPTextBox = new TextBox();
            Disconnect_Generator = new Button();
            Connect_Generator = new Button();
            groupBox1 = new GroupBox();
            labelGenChannel = new Label();
            labelGenAddr = new Label();
            C2 = new CheckBox();
            C1 = new CheckBox();
            comboBoxGenerator = new ComboBox();
            groupBox2 = new GroupBox();
            Save_IP = new Button();
            IEPE = new GroupBox();
            SPD2 = new Label();
            ACC2 = new Label();
            SPD1 = new Label();
            ACC1 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label4 = new Label();
            DC_textBox = new TextBox();
            ResetCoef_2 = new Button();
            ReserCoef_1 = new Button();
            label3 = new Label();
            label2 = new Label();
            Point_2_textBox = new TextBox();
            Point_1_textBox = new TextBox();
            STOP = new Button();
            groupBox3 = new GroupBox();
            buttonReport = new Button();
            label12 = new Label();
            FreqTextBox = new TextBox();
            Termo = new TextBox();
            label7 = new Label();
            PLC_AutoCheck = new CheckBox();
            label5 = new Label();
            PLC = new ComboBox();
            backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            CheckSet = new GroupBox();
            Lvl2_textbox = new TextBox();
            Lvl1_textbox = new TextBox();
            lvl2 = new Button();
            lvl1 = new Button();
            groupBoxOrder = new GroupBox();
            label14 = new Label();
            textBoxSerialNum = new TextBox();
            label13 = new Label();
            textBoxOrderNum = new TextBox();
            AgillentGroupBox.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            IEPE.SuspendLayout();
            groupBox3.SuspendLayout();
            CheckSet.SuspendLayout();
            groupBoxOrder.SuspendLayout();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Location = new Point(122, 279);
            Start.Margin = new Padding(4, 3, 4, 3);
            Start.Name = "Start";
            Start.Size = new Size(98, 51);
            Start.TabIndex = 0;
            Start.Text = "Старт";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // Disconnect_Agillent
            // 
            Disconnect_Agillent.Location = new Point(122, 52);
            Disconnect_Agillent.Margin = new Padding(4, 3, 4, 3);
            Disconnect_Agillent.Name = "Disconnect_Agillent";
            Disconnect_Agillent.Size = new Size(98, 27);
            Disconnect_Agillent.TabIndex = 85;
            Disconnect_Agillent.Text = "Отключить";
            Disconnect_Agillent.UseVisualStyleBackColor = true;
            Disconnect_Agillent.Click += Disconnect_Agillent_Click;
            // 
            // Connect_Agillent
            // 
            Connect_Agillent.Location = new Point(18, 52);
            Connect_Agillent.Margin = new Padding(4, 3, 4, 3);
            Connect_Agillent.Name = "Connect_Agillent";
            Connect_Agillent.Size = new Size(98, 27);
            Connect_Agillent.TabIndex = 84;
            Connect_Agillent.Text = "Подключить";
            Connect_Agillent.UseVisualStyleBackColor = true;
            Connect_Agillent.Click += Connect_Agillent_Click;
            // 
            // AgillentGroupBox
            // 
            AgillentGroupBox.Controls.Add(labelVmAddr);
            AgillentGroupBox.Controls.Add(AgillentPortBox);
            AgillentGroupBox.Controls.Add(Disconnect_Agillent);
            AgillentGroupBox.Controls.Add(Connect_Agillent);
            AgillentGroupBox.Location = new Point(484, 182);
            AgillentGroupBox.Margin = new Padding(4, 3, 4, 3);
            AgillentGroupBox.Name = "AgillentGroupBox";
            AgillentGroupBox.Padding = new Padding(4, 3, 4, 3);
            AgillentGroupBox.Size = new Size(227, 87);
            AgillentGroupBox.TabIndex = 83;
            AgillentGroupBox.TabStop = false;
            AgillentGroupBox.Text = "Порт вольтметра";
            // 
            // labelVmAddr
            // 
            labelVmAddr.AutoSize = true;
            labelVmAddr.Location = new Point(14, 20);
            labelVmAddr.Margin = new Padding(4, 0, 4, 0);
            labelVmAddr.Name = "labelVmAddr";
            labelVmAddr.Size = new Size(43, 15);
            labelVmAddr.TabIndex = 86;
            labelVmAddr.Text = "Адрес:";
            // 
            // AgillentPortBox
            // 
            AgillentPortBox.FormattingEnabled = true;
            AgillentPortBox.Location = new Point(122, 16);
            AgillentPortBox.Margin = new Padding(4, 3, 4, 3);
            AgillentPortBox.Name = "AgillentPortBox";
            AgillentPortBox.Size = new Size(97, 23);
            AgillentPortBox.TabIndex = 6;
            AgillentPortBox.SelectedIndexChanged += AgillentPortBox_SelectedIndexChanged;
            // 
            // UpdatePort
            // 
            UpdatePort.Location = new Point(484, 22);
            UpdatePort.Margin = new Padding(4, 3, 4, 3);
            UpdatePort.Name = "UpdatePort";
            UpdatePort.Size = new Size(227, 27);
            UpdatePort.TabIndex = 104;
            UpdatePort.Text = "Обновить порты";
            UpdatePort.UseVisualStyleBackColor = true;
            UpdatePort.Click += UpdatePort_Click;
            // 
            // LogTextBox
            // 
            LogTextBox.BackColor = SystemColors.Window;
            LogTextBox.Location = new Point(250, 276);
            LogTextBox.Margin = new Padding(4, 3, 4, 3);
            LogTextBox.Multiline = true;
            LogTextBox.Name = "LogTextBox";
            LogTextBox.ReadOnly = true;
            LogTextBox.ScrollBars = ScrollBars.Both;
            LogTextBox.Size = new Size(461, 362);
            LogTextBox.TabIndex = 89;
            // 
            // CoefTextBox
            // 
            CoefTextBox.Location = new Point(122, 53);
            CoefTextBox.Margin = new Padding(4, 3, 4, 3);
            CoefTextBox.Name = "CoefTextBox";
            CoefTextBox.Size = new Size(97, 23);
            CoefTextBox.TabIndex = 94;
            CoefTextBox.Text = "10";
            CoefTextBox.TextChanged += CoefTextBox_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 57);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 95;
            label1.Text = "Коэффициент:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 20);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(60, 15);
            label6.TabIndex = 99;
            label6.Text = "IP крейта:";
            // 
            // IPTextBox
            // 
            IPTextBox.Location = new Point(122, 16);
            IPTextBox.Margin = new Padding(4, 3, 4, 3);
            IPTextBox.Name = "IPTextBox";
            IPTextBox.Size = new Size(97, 23);
            IPTextBox.TabIndex = 98;
            IPTextBox.TextChanged += IPTextBox_TextChanged;
            // 
            // Disconnect_Generator
            // 
            Disconnect_Generator.Location = new Point(122, 83);
            Disconnect_Generator.Margin = new Padding(4, 3, 4, 3);
            Disconnect_Generator.Name = "Disconnect_Generator";
            Disconnect_Generator.Size = new Size(98, 27);
            Disconnect_Generator.TabIndex = 104;
            Disconnect_Generator.Text = "Отключить";
            Disconnect_Generator.UseVisualStyleBackColor = true;
            Disconnect_Generator.Click += Disconnect_Generator_Click;
            // 
            // Connect_Generator
            // 
            Connect_Generator.Location = new Point(18, 83);
            Connect_Generator.Margin = new Padding(4, 3, 4, 3);
            Connect_Generator.Name = "Connect_Generator";
            Connect_Generator.Size = new Size(98, 27);
            Connect_Generator.TabIndex = 103;
            Connect_Generator.Text = "Подключить";
            Connect_Generator.UseVisualStyleBackColor = true;
            Connect_Generator.Click += Connect_Generator_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(labelGenChannel);
            groupBox1.Controls.Add(labelGenAddr);
            groupBox1.Controls.Add(C2);
            groupBox1.Controls.Add(C1);
            groupBox1.Controls.Add(Disconnect_Generator);
            groupBox1.Controls.Add(Connect_Generator);
            groupBox1.Controls.Add(comboBoxGenerator);
            groupBox1.Location = new Point(483, 55);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(227, 120);
            groupBox1.TabIndex = 102;
            groupBox1.TabStop = false;
            groupBox1.Text = "Порт генератора";
            // 
            // labelGenChannel
            // 
            labelGenChannel.AutoSize = true;
            labelGenChannel.Location = new Point(14, 58);
            labelGenChannel.Margin = new Padding(4, 0, 4, 0);
            labelGenChannel.Name = "labelGenChannel";
            labelGenChannel.Size = new Size(43, 15);
            labelGenChannel.TabIndex = 116;
            labelGenChannel.Text = "Канал:";
            // 
            // labelGenAddr
            // 
            labelGenAddr.AutoSize = true;
            labelGenAddr.Location = new Point(14, 20);
            labelGenAddr.Margin = new Padding(4, 0, 4, 0);
            labelGenAddr.Name = "labelGenAddr";
            labelGenAddr.Size = new Size(43, 15);
            labelGenAddr.TabIndex = 115;
            labelGenAddr.Text = "Адрес:";
            // 
            // C2
            // 
            C2.AutoSize = true;
            C2.Location = new Point(175, 57);
            C2.Margin = new Padding(4, 3, 4, 3);
            C2.Name = "C2";
            C2.Size = new Size(40, 19);
            C2.TabIndex = 114;
            C2.Text = "С2";
            C2.UseVisualStyleBackColor = true;
            C2.CheckedChanged += C2_CheckedChanged;
            // 
            // C1
            // 
            C1.AutoSize = true;
            C1.Checked = true;
            C1.CheckState = CheckState.Checked;
            C1.Location = new Point(122, 57);
            C1.Margin = new Padding(4, 3, 4, 3);
            C1.Name = "C1";
            C1.Size = new Size(40, 19);
            C1.TabIndex = 113;
            C1.Text = "С1";
            C1.UseVisualStyleBackColor = true;
            C1.CheckedChanged += C1_CheckedChanged;
            // 
            // comboBoxGenerator
            // 
            comboBoxGenerator.ForeColor = SystemColors.WindowText;
            comboBoxGenerator.FormattingEnabled = true;
            comboBoxGenerator.Location = new Point(122, 18);
            comboBoxGenerator.Margin = new Padding(4, 3, 4, 3);
            comboBoxGenerator.Name = "comboBoxGenerator";
            comboBoxGenerator.Size = new Size(97, 23);
            comboBoxGenerator.TabIndex = 93;
            comboBoxGenerator.SelectedIndexChanged += comboBoxGenerator_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(Save_IP);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(IPTextBox);
            groupBox2.Location = new Point(250, 105);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(227, 90);
            groupBox2.TabIndex = 103;
            groupBox2.TabStop = false;
            groupBox2.Text = "Крейт";
            // 
            // Save_IP
            // 
            Save_IP.Location = new Point(122, 46);
            Save_IP.Margin = new Padding(4, 3, 4, 3);
            Save_IP.Name = "Save_IP";
            Save_IP.Size = new Size(98, 27);
            Save_IP.TabIndex = 113;
            Save_IP.Text = "Сохранить IP";
            Save_IP.UseVisualStyleBackColor = true;
            Save_IP.Click += Save_IP_Click;
            // 
            // IEPE
            // 
            IEPE.Controls.Add(SPD2);
            IEPE.Controls.Add(ACC2);
            IEPE.Controls.Add(SPD1);
            IEPE.Controls.Add(ACC1);
            IEPE.Controls.Add(label8);
            IEPE.Controls.Add(label9);
            IEPE.Controls.Add(label10);
            IEPE.Controls.Add(label11);
            IEPE.Location = new Point(14, 14);
            IEPE.Margin = new Padding(4, 3, 4, 3);
            IEPE.Name = "IEPE";
            IEPE.Padding = new Padding(4, 3, 4, 3);
            IEPE.Size = new Size(227, 137);
            IEPE.TabIndex = 114;
            IEPE.TabStop = false;
            IEPE.Text = "IEPE";
            // 
            // SPD2
            // 
            SPD2.AutoSize = true;
            SPD2.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            SPD2.Location = new Point(119, 113);
            SPD2.Margin = new Padding(4, 0, 4, 0);
            SPD2.Name = "SPD2";
            SPD2.Size = new Size(14, 15);
            SPD2.TabIndex = 11;
            SPD2.Text = "0";
            // 
            // ACC2
            // 
            ACC2.AutoSize = true;
            ACC2.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ACC2.Location = new Point(119, 83);
            ACC2.Margin = new Padding(4, 0, 4, 0);
            ACC2.Name = "ACC2";
            ACC2.Size = new Size(14, 15);
            ACC2.TabIndex = 10;
            ACC2.Text = "0";
            // 
            // SPD1
            // 
            SPD1.AutoSize = true;
            SPD1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            SPD1.Location = new Point(119, 53);
            SPD1.Margin = new Padding(4, 0, 4, 0);
            SPD1.Name = "SPD1";
            SPD1.Size = new Size(14, 15);
            SPD1.TabIndex = 9;
            SPD1.Text = "0";
            // 
            // ACC1
            // 
            ACC1.AutoSize = true;
            ACC1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ACC1.Location = new Point(119, 23);
            ACC1.Margin = new Padding(4, 0, 4, 0);
            ACC1.Name = "ACC1";
            ACC1.Size = new Size(14, 15);
            ACC1.TabIndex = 8;
            ACC1.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 113);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(78, 15);
            label8.TabIndex = 7;
            label8.Text = "Скорость 2К:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 83);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(85, 15);
            label9.TabIndex = 6;
            label9.Text = "Ускорение 2К:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 53);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(78, 15);
            label10.TabIndex = 5;
            label10.Text = "Скорость 1К:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(12, 23);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(85, 15);
            label11.TabIndex = 4;
            label11.Text = "Ускорение 1К:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 121);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(44, 15);
            label4.TabIndex = 108;
            label4.Text = "DC (В):";
            // 
            // DC_textBox
            // 
            DC_textBox.Location = new Point(122, 118);
            DC_textBox.Margin = new Padding(4, 3, 4, 3);
            DC_textBox.Name = "DC_textBox";
            DC_textBox.Size = new Size(97, 23);
            DC_textBox.TabIndex = 107;
            DC_textBox.Text = "10";
            // 
            // ResetCoef_2
            // 
            ResetCoef_2.Location = new Point(122, 240);
            ResetCoef_2.Margin = new Padding(4, 3, 4, 3);
            ResetCoef_2.Name = "ResetCoef_2";
            ResetCoef_2.Size = new Size(98, 32);
            ResetCoef_2.TabIndex = 106;
            ResetCoef_2.Text = "Сброс IEPE2";
            ResetCoef_2.UseVisualStyleBackColor = true;
            ResetCoef_2.Click += ResetCoef_2_Click;
            // 
            // ReserCoef_1
            // 
            ReserCoef_1.Location = new Point(18, 240);
            ReserCoef_1.Margin = new Padding(4, 3, 4, 3);
            ReserCoef_1.Name = "ReserCoef_1";
            ReserCoef_1.Size = new Size(98, 32);
            ReserCoef_1.TabIndex = 105;
            ReserCoef_1.Text = "Сброс IEPE1";
            ReserCoef_1.UseVisualStyleBackColor = true;
            ReserCoef_1.Click += ResetCoef_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 182);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(78, 15);
            label3.TabIndex = 104;
            label3.Text = "Точка 2 (мВ):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 153);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(78, 15);
            label2.TabIndex = 103;
            label2.Text = "Точка 1 (мВ):";
            // 
            // Point_2_textBox
            // 
            Point_2_textBox.Location = new Point(122, 179);
            Point_2_textBox.Margin = new Padding(4, 3, 4, 3);
            Point_2_textBox.Name = "Point_2_textBox";
            Point_2_textBox.Size = new Size(97, 23);
            Point_2_textBox.TabIndex = 102;
            Point_2_textBox.Text = "3000";
            // 
            // Point_1_textBox
            // 
            Point_1_textBox.Location = new Point(122, 150);
            Point_1_textBox.Margin = new Padding(4, 3, 4, 3);
            Point_1_textBox.Name = "Point_1_textBox";
            Point_1_textBox.Size = new Size(97, 23);
            Point_1_textBox.TabIndex = 101;
            Point_1_textBox.Text = "10";
            // 
            // STOP
            // 
            STOP.Location = new Point(18, 279);
            STOP.Margin = new Padding(4, 3, 4, 3);
            STOP.Name = "STOP";
            STOP.Size = new Size(98, 51);
            STOP.TabIndex = 100;
            STOP.Text = "Стоп";
            STOP.UseVisualStyleBackColor = true;
            STOP.Click += STOP_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(buttonReport);
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(FreqTextBox);
            groupBox3.Controls.Add(Termo);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(PLC_AutoCheck);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(PLC);
            groupBox3.Controls.Add(Start);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(STOP);
            groupBox3.Controls.Add(CoefTextBox);
            groupBox3.Controls.Add(DC_textBox);
            groupBox3.Controls.Add(Point_1_textBox);
            groupBox3.Controls.Add(label1);
            groupBox3.Controls.Add(ResetCoef_2);
            groupBox3.Controls.Add(Point_2_textBox);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(ReserCoef_1);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new Point(14, 265);
            groupBox3.Margin = new Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4, 3, 4, 3);
            groupBox3.Size = new Size(227, 374);
            groupBox3.TabIndex = 109;
            groupBox3.TabStop = false;
            groupBox3.Text = "Настройка";
            // 
            // buttonReport
            // 
            buttonReport.Location = new Point(18, 337);
            buttonReport.Margin = new Padding(4, 3, 4, 3);
            buttonReport.Name = "buttonReport";
            buttonReport.Size = new Size(203, 27);
            buttonReport.TabIndex = 115;
            buttonReport.Text = "Записать в лог";
            buttonReport.UseVisualStyleBackColor = true;
            buttonReport.Click += buttonReport_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(8, 91);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(50, 15);
            label12.TabIndex = 114;
            label12.Text = "Частота";
            // 
            // FreqTextBox
            // 
            FreqTextBox.Location = new Point(122, 88);
            FreqTextBox.Margin = new Padding(4, 3, 4, 3);
            FreqTextBox.Name = "FreqTextBox";
            FreqTextBox.Size = new Size(97, 23);
            FreqTextBox.TabIndex = 113;
            FreqTextBox.Text = "80";
            // 
            // Termo
            // 
            Termo.Location = new Point(122, 209);
            Termo.Margin = new Padding(4, 3, 4, 3);
            Termo.Name = "Termo";
            Termo.Size = new Size(97, 23);
            Termo.TabIndex = 111;
            Termo.Text = "8";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 212);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(89, 15);
            label7.TabIndex = 112;
            label7.Text = "Датчик темпер";
            // 
            // PLC_AutoCheck
            // 
            PLC_AutoCheck.AutoSize = true;
            PLC_AutoCheck.Checked = true;
            PLC_AutoCheck.CheckState = CheckState.Checked;
            PLC_AutoCheck.Location = new Point(57, 21);
            PLC_AutoCheck.Margin = new Padding(4, 3, 4, 3);
            PLC_AutoCheck.Name = "PLC_AutoCheck";
            PLC_AutoCheck.Size = new Size(52, 19);
            PLC_AutoCheck.TabIndex = 110;
            PLC_AutoCheck.Text = "Авто";
            PLC_AutoCheck.UseVisualStyleBackColor = true;
            PLC_AutoCheck.CheckedChanged += PLC_AutoCheck_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 25);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(28, 15);
            label5.TabIndex = 109;
            label5.Text = "PLC";
            // 
            // PLC
            // 
            PLC.Enabled = false;
            PLC.FormattingEnabled = true;
            PLC.Location = new Point(122, 22);
            PLC.Margin = new Padding(4, 3, 4, 3);
            PLC.Name = "PLC";
            PLC.Size = new Size(97, 23);
            PLC.TabIndex = 91;
            PLC.SelectedIndexChanged += PLC_SelectedIndexChanged;
            // 
            // backgroundWorker3
            // 
            backgroundWorker3.DoWork += PLC_Updater;
            // 
            // CheckSet
            // 
            CheckSet.Controls.Add(Lvl2_textbox);
            CheckSet.Controls.Add(Lvl1_textbox);
            CheckSet.Controls.Add(lvl2);
            CheckSet.Controls.Add(lvl1);
            CheckSet.Location = new Point(14, 166);
            CheckSet.Margin = new Padding(4, 3, 4, 3);
            CheckSet.Name = "CheckSet";
            CheckSet.Padding = new Padding(4, 3, 4, 3);
            CheckSet.Size = new Size(227, 92);
            CheckSet.TabIndex = 111;
            CheckSet.TabStop = false;
            CheckSet.Text = "Проверка";
            // 
            // Lvl2_textbox
            // 
            Lvl2_textbox.Location = new Point(122, 27);
            Lvl2_textbox.Margin = new Padding(4, 3, 4, 3);
            Lvl2_textbox.Name = "Lvl2_textbox";
            Lvl2_textbox.Size = new Size(97, 23);
            Lvl2_textbox.TabIndex = 9;
            Lvl2_textbox.Text = "3000";
            // 
            // Lvl1_textbox
            // 
            Lvl1_textbox.Location = new Point(18, 27);
            Lvl1_textbox.Margin = new Padding(4, 3, 4, 3);
            Lvl1_textbox.Name = "Lvl1_textbox";
            Lvl1_textbox.Size = new Size(97, 23);
            Lvl1_textbox.TabIndex = 8;
            Lvl1_textbox.Text = "10";
            // 
            // lvl2
            // 
            lvl2.Location = new Point(122, 53);
            lvl2.Margin = new Padding(4, 3, 4, 3);
            lvl2.Name = "lvl2";
            lvl2.Size = new Size(98, 27);
            lvl2.TabIndex = 1;
            lvl2.Text = "Задать 2Ур";
            lvl2.UseVisualStyleBackColor = true;
            lvl2.Click += lvl2_Click;
            // 
            // lvl1
            // 
            lvl1.Location = new Point(18, 53);
            lvl1.Margin = new Padding(4, 3, 4, 3);
            lvl1.Name = "lvl1";
            lvl1.Size = new Size(98, 27);
            lvl1.TabIndex = 0;
            lvl1.Text = "Задать 1Ур";
            lvl1.UseVisualStyleBackColor = true;
            lvl1.Click += lvl1_Click;
            // 
            // groupBoxOrder
            // 
            groupBoxOrder.Controls.Add(label14);
            groupBoxOrder.Controls.Add(textBoxSerialNum);
            groupBoxOrder.Controls.Add(label13);
            groupBoxOrder.Controls.Add(textBoxOrderNum);
            groupBoxOrder.Location = new Point(248, 14);
            groupBoxOrder.Margin = new Padding(4, 3, 4, 3);
            groupBoxOrder.Name = "groupBoxOrder";
            groupBoxOrder.Padding = new Padding(4, 3, 4, 3);
            groupBoxOrder.Size = new Size(227, 84);
            groupBoxOrder.TabIndex = 112;
            groupBoxOrder.TabStop = false;
            groupBoxOrder.Text = "Заказ";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(8, 50);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(107, 15);
            label14.TabIndex = 103;
            label14.Text = "Серийный номер:";
            // 
            // textBoxSerialNum
            // 
            textBoxSerialNum.Location = new Point(122, 46);
            textBoxSerialNum.Margin = new Padding(4, 3, 4, 3);
            textBoxSerialNum.Name = "textBoxSerialNum";
            textBoxSerialNum.Size = new Size(97, 23);
            textBoxSerialNum.TabIndex = 102;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(8, 20);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(85, 15);
            label13.TabIndex = 101;
            label13.Text = "Номер заказа:";
            // 
            // textBoxOrderNum
            // 
            textBoxOrderNum.Location = new Point(122, 16);
            textBoxOrderNum.Margin = new Padding(4, 3, 4, 3);
            textBoxOrderNum.Name = "textBoxOrderNum";
            textBoxOrderNum.Size = new Size(97, 23);
            textBoxOrderNum.TabIndex = 100;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(726, 652);
            Controls.Add(IEPE);
            Controls.Add(groupBoxOrder);
            Controls.Add(CheckSet);
            Controls.Add(groupBox3);
            Controls.Add(UpdatePort);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(LogTextBox);
            Controls.Add(AgillentGroupBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Form1";
            Text = "АРМ проверки и настройки контроллеров ТИК-Крейт V: 0.10.4";
            FormClosing += Form_FormClosing;
            AgillentGroupBox.ResumeLayout(false);
            AgillentGroupBox.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            IEPE.ResumeLayout(false);
            IEPE.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            CheckSet.ResumeLayout(false);
            CheckSet.PerformLayout();
            groupBoxOrder.ResumeLayout(false);
            groupBoxOrder.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
        //private System.IO.Ports.SerialPort serialPort1;
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

