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
            groupBox3 = new GroupBox();
            label5 = new Label();
            PLC = new ComboBox();
            backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            groupBoxOrder = new GroupBox();
            label14 = new Label();
            textBoxSerialNum = new TextBox();
            label13 = new Label();
            textBoxOrderNum = new TextBox();
            label8 = new Label();
            comboBox1 = new ComboBox();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            button1 = new Button();
            label2 = new Label();
            AgillentGroupBox.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBoxOrder.SuspendLayout();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Location = new Point(8, 181);
            Start.Margin = new Padding(4, 3, 4, 3);
            Start.Name = "Start";
            Start.Size = new Size(211, 29);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 86);
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
            // groupBox3
            // 
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(button1);
            groupBox3.Controls.Add(radioButton2);
            groupBox3.Controls.Add(radioButton1);
            groupBox3.Controls.Add(textBoxOrderNum);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(comboBox1);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(PLC);
            groupBox3.Controls.Add(Start);
            groupBox3.Controls.Add(label1);
            groupBox3.Location = new Point(13, 14);
            groupBox3.Margin = new Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4, 3, 4, 3);
            groupBox3.Size = new Size(227, 520);
            groupBox3.TabIndex = 109;
            groupBox3.TabStop = false;
            groupBox3.Text = "Настройка";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 56);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(54, 15);
            label5.TabIndex = 109;
            label5.Text = "Тип PLC:";
            // 
            // PLC
            // 
            PLC.Enabled = false;
            PLC.FormattingEnabled = true;
            PLC.Location = new Point(122, 53);
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
            // groupBoxOrder
            // 
            groupBoxOrder.Controls.Add(label14);
            groupBoxOrder.Controls.Add(textBoxSerialNum);
            groupBoxOrder.Controls.Add(label13);
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
            textBoxOrderNum.Location = new Point(8, 117);
            textBoxOrderNum.Margin = new Padding(4, 3, 4, 3);
            textBoxOrderNum.Name = "textBoxOrderNum";
            textBoxOrderNum.Size = new Size(211, 23);
            textBoxOrderNum.TabIndex = 100;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(8, 26);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(53, 15);
            label8.TabIndex = 117;
            label8.Text = "Модуль:";
            label8.Click += this.label8_Click;
            // 
            // comboBox1
            // 
            comboBox1.Enabled = false;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(122, 23);
            comboBox1.Margin = new Padding(4, 3, 4, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(97, 23);
            comboBox1.TabIndex = 116;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(122, 86);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(37, 19);
            radioButton1.TabIndex = 118;
            radioButton1.TabStop = true;
            radioButton1.Text = "10";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(165, 86);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(46, 19);
            radioButton2.TabIndex = 119;
            radioButton2.TabStop = true;
            radioButton2.Text = "6.67";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(8, 146);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(211, 29);
            button1.TabIndex = 120;
            button1.Text = "Выборки";
            button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ButtonShadow;
            label2.Location = new Point(8, 101);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 104;
            label2.Text = "Номер заказа:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(726, 652);
            Controls.Add(groupBoxOrder);
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
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
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
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.ComboBox PLC;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.Button Save_IP;
        private System.Windows.Forms.CheckBox C2;
        private System.Windows.Forms.CheckBox C1;
        private System.Windows.Forms.Label labelVmAddr;
        private System.Windows.Forms.Label labelGenChannel;
        private System.Windows.Forms.Label labelGenAddr;
        private System.Windows.Forms.GroupBox groupBoxOrder;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxSerialNum;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxOrderNum;
        private Label label8;
        private ComboBox comboBox1;
        private RadioButton radioButton1;
        private Label label2;
        private Button button1;
        private RadioButton radioButton2;
    }
}

