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
            button1 = new Button();
            button2 = new Button();
            groupBox3 = new GroupBox();
            Samples = new Button();
            label13 = new Label();
            label2 = new Label();
            OrderNumber = new TextBox();
            TypePLC = new ComboBox();
            Coef_6_67 = new RadioButton();
            Coef_10 = new RadioButton();
            label5 = new Label();
            Modules = new ComboBox();
            label1 = new Label();
            backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            groupBox4 = new GroupBox();
            groupBox5 = new GroupBox();
            PrinterComboBox = new ComboBox();
            AgillentGroupBox.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Location = new Point(8, 187);
            Start.Margin = new Padding(4, 3, 4, 3);
            Start.Name = "Start";
            Start.Size = new Size(191, 27);
            Start.TabIndex = 0;
            Start.Text = "Начать настройку";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // Disconnect_Agillent
            // 
            Disconnect_Agillent.Location = new Point(122, 45);
            Disconnect_Agillent.Margin = new Padding(4, 3, 4, 3);
            Disconnect_Agillent.Name = "Disconnect_Agillent";
            Disconnect_Agillent.Size = new Size(100, 25);
            Disconnect_Agillent.TabIndex = 85;
            Disconnect_Agillent.Text = "Отключить";
            Disconnect_Agillent.UseVisualStyleBackColor = true;
            Disconnect_Agillent.Click += Disconnect_Agillent_Click;
            // 
            // Connect_Agillent
            // 
            Connect_Agillent.Location = new Point(10, 45);
            Connect_Agillent.Margin = new Padding(4, 3, 4, 3);
            Connect_Agillent.Name = "Connect_Agillent";
            Connect_Agillent.Size = new Size(100, 25);
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
            AgillentGroupBox.Location = new Point(10, 168);
            AgillentGroupBox.Margin = new Padding(4, 3, 4, 3);
            AgillentGroupBox.Name = "AgillentGroupBox";
            AgillentGroupBox.Padding = new Padding(4, 3, 4, 3);
            AgillentGroupBox.Size = new Size(230, 80);
            AgillentGroupBox.TabIndex = 83;
            AgillentGroupBox.TabStop = false;
            AgillentGroupBox.Text = "Порт вольтметра";
            // 
            // labelVmAddr
            // 
            labelVmAddr.AutoSize = true;
            labelVmAddr.Location = new Point(10, 20);
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
            AgillentPortBox.Size = new Size(100, 23);
            AgillentPortBox.TabIndex = 6;
            AgillentPortBox.SelectedIndexChanged += AgillentPortBox_SelectedIndexChanged;
            // 
            // UpdatePort
            // 
            UpdatePort.Location = new Point(10, 22);
            UpdatePort.Margin = new Padding(4, 3, 4, 3);
            UpdatePort.Name = "UpdatePort";
            UpdatePort.Size = new Size(230, 25);
            UpdatePort.TabIndex = 104;
            UpdatePort.Text = "Обновить порты";
            UpdatePort.UseVisualStyleBackColor = true;
            UpdatePort.Click += UpdatePort_Click;
            // 
            // LogTextBox
            // 
            LogTextBox.BackColor = SystemColors.Window;
            LogTextBox.Location = new Point(269, 244);
            LogTextBox.Margin = new Padding(4, 3, 4, 3);
            LogTextBox.Multiline = true;
            LogTextBox.Name = "LogTextBox";
            LogTextBox.ReadOnly = true;
            LogTextBox.ScrollBars = ScrollBars.Both;
            LogTextBox.Size = new Size(211, 175);
            LogTextBox.TabIndex = 89;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(10, 20);
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
            IPTextBox.Size = new Size(98, 23);
            IPTextBox.TabIndex = 98;
            IPTextBox.TextChanged += IPTextBox_TextChanged;
            // 
            // Disconnect_Generator
            // 
            Disconnect_Generator.Location = new Point(120, 73);
            Disconnect_Generator.Margin = new Padding(4, 3, 4, 3);
            Disconnect_Generator.Name = "Disconnect_Generator";
            Disconnect_Generator.Size = new Size(100, 25);
            Disconnect_Generator.TabIndex = 104;
            Disconnect_Generator.Text = "Отключить";
            Disconnect_Generator.UseVisualStyleBackColor = true;
            Disconnect_Generator.Click += Disconnect_Generator_Click;
            // 
            // Connect_Generator
            // 
            Connect_Generator.Location = new Point(8, 73);
            Connect_Generator.Margin = new Padding(4, 3, 4, 3);
            Connect_Generator.Name = "Connect_Generator";
            Connect_Generator.Size = new Size(100, 25);
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
            groupBox1.Location = new Point(10, 55);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(230, 107);
            groupBox1.TabIndex = 102;
            groupBox1.TabStop = false;
            groupBox1.Text = "Порт генератора";
            // 
            // labelGenChannel
            // 
            labelGenChannel.AutoSize = true;
            labelGenChannel.Location = new Point(10, 49);
            labelGenChannel.Margin = new Padding(4, 0, 4, 0);
            labelGenChannel.Name = "labelGenChannel";
            labelGenChannel.Size = new Size(43, 15);
            labelGenChannel.TabIndex = 116;
            labelGenChannel.Text = "Канал:";
            // 
            // labelGenAddr
            // 
            labelGenAddr.AutoSize = true;
            labelGenAddr.Location = new Point(10, 20);
            labelGenAddr.Margin = new Padding(4, 0, 4, 0);
            labelGenAddr.Name = "labelGenAddr";
            labelGenAddr.Size = new Size(43, 15);
            labelGenAddr.TabIndex = 115;
            labelGenAddr.Text = "Адрес:";
            // 
            // C2
            // 
            C2.AutoSize = true;
            C2.Location = new Point(175, 48);
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
            C1.Location = new Point(122, 48);
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
            comboBoxGenerator.Size = new Size(98, 23);
            comboBoxGenerator.TabIndex = 93;
            comboBoxGenerator.SelectedIndexChanged += comboBoxGenerator_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(button2);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(IPTextBox);
            groupBox2.Location = new Point(10, 254);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(230, 80);
            groupBox2.TabIndex = 103;
            groupBox2.TabStop = false;
            groupBox2.Text = "Крейт";
            // 
            // button1
            // 
            button1.Location = new Point(122, 45);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(100, 25);
            button1.TabIndex = 101;
            button1.Text = "Отключить";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(10, 45);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(100, 25);
            button2.TabIndex = 100;
            button2.Text = "Подключить";
            button2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(Samples);
            groupBox3.Controls.Add(label13);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(OrderNumber);
            groupBox3.Controls.Add(TypePLC);
            groupBox3.Controls.Add(Coef_6_67);
            groupBox3.Controls.Add(Coef_10);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(Modules);
            groupBox3.Controls.Add(Start);
            groupBox3.Controls.Add(label1);
            groupBox3.Location = new Point(269, 12);
            groupBox3.Margin = new Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4, 3, 4, 3);
            groupBox3.Size = new Size(211, 226);
            groupBox3.TabIndex = 109;
            groupBox3.TabStop = false;
            groupBox3.Text = "Настройка";
            // 
            // Samples
            // 
            Samples.Location = new Point(8, 154);
            Samples.Margin = new Padding(4, 3, 4, 3);
            Samples.Name = "Samples";
            Samples.Size = new Size(191, 27);
            Samples.TabIndex = 114;
            Samples.Text = "Выборки";
            Samples.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = SystemColors.GrayText;
            label13.Location = new Point(8, 106);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(85, 15);
            label13.TabIndex = 101;
            label13.Text = "Номер заказа:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 54);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 113;
            label2.Text = "Тип PLC:";
            // 
            // OrderNumber
            // 
            OrderNumber.Location = new Point(8, 123);
            OrderNumber.Margin = new Padding(4, 3, 4, 3);
            OrderNumber.Name = "OrderNumber";
            OrderNumber.Size = new Size(191, 23);
            OrderNumber.TabIndex = 100;
            // 
            // TypePLC
            // 
            TypePLC.Enabled = false;
            TypePLC.FormattingEnabled = true;
            TypePLC.Location = new Point(93, 51);
            TypePLC.Margin = new Padding(4, 3, 4, 3);
            TypePLC.Name = "TypePLC";
            TypePLC.Size = new Size(106, 23);
            TypePLC.TabIndex = 112;
            // 
            // Coef_6_67
            // 
            Coef_6_67.AutoSize = true;
            Coef_6_67.Location = new Point(145, 83);
            Coef_6_67.Name = "Coef_6_67";
            Coef_6_67.Size = new Size(46, 19);
            Coef_6_67.TabIndex = 111;
            Coef_6_67.TabStop = true;
            Coef_6_67.Text = "6.67";
            Coef_6_67.UseVisualStyleBackColor = true;
            // 
            // Coef_10
            // 
            Coef_10.AutoSize = true;
            Coef_10.Location = new Point(102, 83);
            Coef_10.Name = "Coef_10";
            Coef_10.Size = new Size(37, 19);
            Coef_10.TabIndex = 110;
            Coef_10.TabStop = true;
            Coef_10.Text = "10";
            Coef_10.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 25);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(77, 15);
            label5.TabIndex = 109;
            label5.Text = "Модуль PLC:";
            // 
            // Modules
            // 
            Modules.Enabled = false;
            Modules.FormattingEnabled = true;
            Modules.Location = new Point(93, 22);
            Modules.Margin = new Padding(4, 3, 4, 3);
            Modules.Name = "Modules";
            Modules.Size = new Size(106, 23);
            Modules.TabIndex = 91;
            Modules.SelectedIndexChanged += this.PLC_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 85);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 95;
            label1.Text = "Коэффициент:";
            // 
            // backgroundWorker3
            // 
            backgroundWorker3.DoWork += PLC_Updater;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(groupBox5);
            groupBox4.Controls.Add(UpdatePort);
            groupBox4.Controls.Add(groupBox1);
            groupBox4.Controls.Add(AgillentGroupBox);
            groupBox4.Controls.Add(groupBox2);
            groupBox4.Location = new Point(12, 12);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(250, 407);
            groupBox4.TabIndex = 113;
            groupBox4.TabStop = false;
            groupBox4.Text = "Устройства";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(PrinterComboBox);
            groupBox5.Location = new Point(10, 340);
            groupBox5.Margin = new Padding(4, 3, 4, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(4, 3, 4, 3);
            groupBox5.Size = new Size(230, 55);
            groupBox5.TabIndex = 104;
            groupBox5.TabStop = false;
            groupBox5.Text = "Принтер";
            // 
            // PrinterComboBox
            // 
            PrinterComboBox.FormattingEnabled = true;
            PrinterComboBox.Location = new Point(10, 22);
            PrinterComboBox.Name = "PrinterComboBox";
            PrinterComboBox.Size = new Size(210, 23);
            PrinterComboBox.TabIndex = 114;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(490, 430);
            Controls.Add(groupBox4);
            Controls.Add(LogTextBox);
            Controls.Add(groupBox3);
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
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Button Disconnect_Generator;
        private System.Windows.Forms.Button Connect_Generator;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxGenerator;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button UpdatePort;
        //private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox Modules;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.CheckBox C2;
        private System.Windows.Forms.CheckBox C1;
        private System.Windows.Forms.Label labelVmAddr;
        private System.Windows.Forms.Label labelGenChannel;
        private System.Windows.Forms.Label labelGenAddr;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox OrderNumber;
        private Button Save_IP;
        private Label label1;
        private Label label14;
        private TextBox textBoxSerialNum;
        private Button lvl1;
        private Button lvl2;
        private TextBox Lvl1_textbox;
        private TextBox Lvl2_textbox;
        private GroupBox CheckSet;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label ACC1;
        private Label SPD1;
        private Label ACC2;
        private Label SPD2;
        private GroupBox IEPE;
        private GroupBox groupBox4;
        private Button button1;
        private Button button2;
        private ComboBox PrinterComboBox;
        private GroupBox groupBox5;
        private RadioButton Coef_6_67;
        private RadioButton Coef_10;
        private Label label2;
        private ComboBox TypePLC;
        private Button Samples;
    }
}

