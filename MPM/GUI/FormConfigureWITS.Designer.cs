namespace MPM.GUI
{
    partial class FormConfigureWITS
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                //if (m_WidgetInfoLookupTbl != null)
                //    m_WidgetInfoLookupTbl.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigureWITS));
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelCOMPort = new System.Windows.Forms.Label();
            this.numericUpDownCOMPort = new System.Windows.Forms.NumericUpDown();
            this.textBoxPingInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownCOMPortOut1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxFilterEM = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterMudPulse = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxWITSValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonTestSend = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxWITSChannel = new System.Windows.Forms.ComboBox();
            this.comboBoxWITSName = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCOMPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCOMPortOut1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.ForeColor = System.Drawing.Color.Black;
            this.buttonSave.Location = new System.Drawing.Point(90, 440);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "OK";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelCOMPort
            // 
            this.labelCOMPort.AutoSize = true;
            this.labelCOMPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCOMPort.ForeColor = System.Drawing.Color.White;
            this.labelCOMPort.Location = new System.Drawing.Point(19, 32);
            this.labelCOMPort.Name = "labelCOMPort";
            this.labelCOMPort.Size = new System.Drawing.Size(94, 13);
            this.labelCOMPort.TabIndex = 1;
            this.labelCOMPort.Text = "COM Port (#):";
            // 
            // numericUpDownCOMPort
            // 
            this.numericUpDownCOMPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownCOMPort.ForeColor = System.Drawing.Color.Black;
            this.numericUpDownCOMPort.Location = new System.Drawing.Point(174, 24);
            this.numericUpDownCOMPort.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownCOMPort.Name = "numericUpDownCOMPort";
            this.numericUpDownCOMPort.Size = new System.Drawing.Size(47, 21);
            this.numericUpDownCOMPort.TabIndex = 2;
            this.numericUpDownCOMPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBoxPingInterval
            // 
            this.textBoxPingInterval.Enabled = false;
            this.textBoxPingInterval.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPingInterval.ForeColor = System.Drawing.Color.Black;
            this.textBoxPingInterval.Location = new System.Drawing.Point(174, 55);
            this.textBoxPingInterval.Name = "textBoxPingInterval";
            this.textBoxPingInterval.ReadOnly = true;
            this.textBoxPingInterval.Size = new System.Drawing.Size(47, 21);
            this.textBoxPingInterval.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(19, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ping Interval (sec):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxPingInterval);
            this.groupBox1.Controls.Add(this.numericUpDownCOMPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelCOMPort);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 103);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WITS From Rig (ie Pason)";
            // 
            // numericUpDownCOMPortOut1
            // 
            this.numericUpDownCOMPortOut1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownCOMPortOut1.ForeColor = System.Drawing.Color.Black;
            this.numericUpDownCOMPortOut1.Location = new System.Drawing.Point(173, 23);
            this.numericUpDownCOMPortOut1.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownCOMPortOut1.Name = "numericUpDownCOMPortOut1";
            this.numericUpDownCOMPortOut1.Size = new System.Drawing.Size(47, 21);
            this.numericUpDownCOMPortOut1.TabIndex = 8;
            this.numericUpDownCOMPortOut1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "COM Port (#):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownCOMPortOut1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(13, 121);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 66);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WITS Out Only";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxFilterEM);
            this.groupBox3.Controls.Add(this.checkBoxFilterMudPulse);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 333);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 93);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter Output";
            // 
            // checkBoxFilterEM
            // 
            this.checkBoxFilterEM.AutoSize = true;
            this.checkBoxFilterEM.Location = new System.Drawing.Point(16, 59);
            this.checkBoxFilterEM.Name = "checkBoxFilterEM";
            this.checkBoxFilterEM.Size = new System.Drawing.Size(114, 17);
            this.checkBoxFilterEM.TabIndex = 1;
            this.checkBoxFilterEM.Text = "Send EM Data";
            this.checkBoxFilterEM.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilterMudPulse
            // 
            this.checkBoxFilterMudPulse.AutoSize = true;
            this.checkBoxFilterMudPulse.Location = new System.Drawing.Point(16, 27);
            this.checkBoxFilterMudPulse.Name = "checkBoxFilterMudPulse";
            this.checkBoxFilterMudPulse.Size = new System.Drawing.Size(161, 17);
            this.checkBoxFilterMudPulse.TabIndex = 0;
            this.checkBoxFilterMudPulse.Text = "Send Mud Pulse Data";
            this.checkBoxFilterMudPulse.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Channel:";
            // 
            // textBoxWITSValue
            // 
            this.textBoxWITSValue.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxWITSValue.ForeColor = System.Drawing.Color.Black;
            this.textBoxWITSValue.Location = new System.Drawing.Point(78, 81);
            this.textBoxWITSValue.Name = "textBoxWITSValue";
            this.textBoxWITSValue.Size = new System.Drawing.Size(75, 27);
            this.textBoxWITSValue.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(14, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Value:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(14, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Name:";
            // 
            // buttonTestSend
            // 
            this.buttonTestSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonTestSend.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTestSend.ForeColor = System.Drawing.Color.Black;
            this.buttonTestSend.Location = new System.Drawing.Point(165, 80);
            this.buttonTestSend.Name = "buttonTestSend";
            this.buttonTestSend.Size = new System.Drawing.Size(56, 30);
            this.buttonTestSend.TabIndex = 17;
            this.buttonTestSend.Text = "Test";
            this.buttonTestSend.UseVisualStyleBackColor = false;
            this.buttonTestSend.Click += new System.EventHandler(this.buttonTestSend_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxWITSName);
            this.groupBox4.Controls.Add(this.comboBoxWITSChannel);
            this.groupBox4.Controls.Add(this.buttonTestSend);
            this.groupBox4.Controls.Add(this.textBoxWITSValue);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(12, 196);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(239, 128);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Test COM Port with WITS Inputs";
            // 
            // comboBoxWITSChannel
            // 
            this.comboBoxWITSChannel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxWITSChannel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxWITSChannel.FormattingEnabled = true;
            this.comboBoxWITSChannel.Location = new System.Drawing.Point(78, 23);
            this.comboBoxWITSChannel.Name = "comboBoxWITSChannel";
            this.comboBoxWITSChannel.Size = new System.Drawing.Size(142, 21);
            this.comboBoxWITSChannel.TabIndex = 20;
            this.comboBoxWITSChannel.SelectedIndexChanged += new System.EventHandler(this.comboBoxWITSChannel_SelectedIndexChanged);
            // 
            // comboBoxWITSName
            // 
            this.comboBoxWITSName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxWITSName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxWITSName.FormattingEnabled = true;
            this.comboBoxWITSName.Location = new System.Drawing.Point(78, 52);
            this.comboBoxWITSName.Name = "comboBoxWITSName";
            this.comboBoxWITSName.Size = new System.Drawing.Size(142, 21);
            this.comboBoxWITSName.TabIndex = 21;
            this.comboBoxWITSName.SelectedIndexChanged += new System.EventHandler(this.comboBoxWITSName_SelectedIndexChanged);
            // 
            // FormConfigureWITS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(265, 512);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigureWITS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure WITS Communication";
            this.Load += new System.EventHandler(this.FormConfigureWITS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCOMPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCOMPortOut1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelCOMPort;
        private System.Windows.Forms.NumericUpDown numericUpDownCOMPort;
        private System.Windows.Forms.TextBox textBoxPingInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownCOMPortOut1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxFilterEM;
        private System.Windows.Forms.CheckBox checkBoxFilterMudPulse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWITSValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonTestSend;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBoxWITSChannel;
        private System.Windows.Forms.ComboBox comboBoxWITSName;
    }
}