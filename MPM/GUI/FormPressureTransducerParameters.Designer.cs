namespace MPM.GUI
{
    partial class FormPressureTransducerParameters
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPressureTransducerParameters));
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPressureUnits = new System.Windows.Forms.ComboBox();
            this.textBoxTransducerGain = new System.Windows.Forms.TextBox();
            this.textBoxPressureOffset = new System.Windows.Forms.TextBox();
            this.textBoxTransducerScale = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.RECEIVER_TYPE_COMBOBOX = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewLog = new System.Windows.Forms.DataGridView();
            this.comboBoxTransducerType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(237, 394);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 48);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(310, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Offset:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(267, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Scale Factor:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(318, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Gain:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(50, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Pressure Units:";
            // 
            // comboBoxPressureUnits
            // 
            this.comboBoxPressureUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPressureUnits.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxPressureUnits.FormattingEnabled = true;
            this.comboBoxPressureUnits.Items.AddRange(new object[] {
            "Psi",
            "kPa"});
            this.comboBoxPressureUnits.Location = new System.Drawing.Point(154, 107);
            this.comboBoxPressureUnits.Name = "comboBoxPressureUnits";
            this.comboBoxPressureUnits.Size = new System.Drawing.Size(107, 21);
            this.comboBoxPressureUnits.TabIndex = 0;
            this.comboBoxPressureUnits.SelectedIndexChanged += new System.EventHandler(this.comboBoxPressureUnits_SelectedIndexChanged);
            // 
            // textBoxTransducerGain
            // 
            this.textBoxTransducerGain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTransducerGain.Location = new System.Drawing.Point(356, 70);
            this.textBoxTransducerGain.Name = "textBoxTransducerGain";
            this.textBoxTransducerGain.Size = new System.Drawing.Size(107, 21);
            this.textBoxTransducerGain.TabIndex = 4;
            // 
            // textBoxPressureOffset
            // 
            this.textBoxPressureOffset.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPressureOffset.Location = new System.Drawing.Point(357, 107);
            this.textBoxPressureOffset.Name = "textBoxPressureOffset";
            this.textBoxPressureOffset.Size = new System.Drawing.Size(107, 21);
            this.textBoxPressureOffset.TabIndex = 1;
            // 
            // textBoxTransducerScale
            // 
            this.textBoxTransducerScale.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTransducerScale.Location = new System.Drawing.Point(357, 31);
            this.textBoxTransducerScale.Name = "textBoxTransducerScale";
            this.textBoxTransducerScale.ReadOnly = true;
            this.textBoxTransducerScale.Size = new System.Drawing.Size(107, 21);
            this.textBoxTransducerScale.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.RECEIVER_TYPE_COMBOBOX);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.comboBoxTransducerType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxTransducerScale);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxTransducerGain);
            this.groupBox1.Controls.Add(this.textBoxPressureOffset);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxPressureUnits);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 367);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(51, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Receiver Type:";
            // 
            // RECEIVER_TYPE_COMBOBOX
            // 
            this.RECEIVER_TYPE_COMBOBOX.AutoCompleteCustomSource.AddRange(new string[] {
            "560C",
            "560R"});
            this.RECEIVER_TYPE_COMBOBOX.FormattingEnabled = true;
            this.RECEIVER_TYPE_COMBOBOX.Items.AddRange(new object[] {
            "560R",
            "560C"});
            this.RECEIVER_TYPE_COMBOBOX.Location = new System.Drawing.Point(154, 72);
            this.RECEIVER_TYPE_COMBOBOX.Margin = new System.Windows.Forms.Padding(2);
            this.RECEIVER_TYPE_COMBOBOX.Name = "RECEIVER_TYPE_COMBOBOX";
            this.RECEIVER_TYPE_COMBOBOX.Size = new System.Drawing.Size(107, 21);
            this.RECEIVER_TYPE_COMBOBOX.TabIndex = 21;
            this.RECEIVER_TYPE_COMBOBOX.SelectedIndexChanged += new System.EventHandler(this.RECEIVER_TYPE_COMBOBOX_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewLog);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(14, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(499, 209);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Change Log";
            // 
            // dataGridViewLog
            // 
            this.dataGridViewLog.AllowUserToAddRows = false;
            this.dataGridViewLog.AllowUserToDeleteRows = false;
            this.dataGridViewLog.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewLog.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewLog.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewLog.GridColor = System.Drawing.Color.White;
            this.dataGridViewLog.Location = new System.Drawing.Point(15, 20);
            this.dataGridViewLog.Name = "dataGridViewLog";
            this.dataGridViewLog.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewLog.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.dataGridViewLog.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewLog.RowTemplate.ReadOnly = true;
            this.dataGridViewLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLog.Size = new System.Drawing.Size(463, 177);
            this.dataGridViewLog.TabIndex = 18;
            // 
            // comboBoxTransducerType
            // 
            this.comboBoxTransducerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTransducerType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTransducerType.FormattingEnabled = true;
            this.comboBoxTransducerType.Items.AddRange(new object[] {
            "3,000",
            "5,000",
            "10,000"});
            this.comboBoxTransducerType.Location = new System.Drawing.Point(154, 31);
            this.comboBoxTransducerType.Name = "comboBoxTransducerType";
            this.comboBoxTransducerType.Size = new System.Drawing.Size(107, 21);
            this.comboBoxTransducerType.TabIndex = 16;
            this.comboBoxTransducerType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTransducerType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(32, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Transducer Type:";
            // 
            // FormPressureTransducerParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(556, 464);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPressureTransducerParameters";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pressure Transducer Settings";
            this.Load += new System.EventHandler(this.FormPressureTransducerParameters_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPressureUnits;
        private System.Windows.Forms.TextBox textBoxTransducerGain;
        private System.Windows.Forms.TextBox textBoxPressureOffset;
        private System.Windows.Forms.TextBox textBoxTransducerScale;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxTransducerType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridViewLog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox RECEIVER_TYPE_COMBOBOX;
    }
}