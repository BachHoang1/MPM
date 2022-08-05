namespace MPM.GUI
{
    partial class FormSurveyExportFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSurveyExportFilter));
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonAccepted = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownStartSurveyDepth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEndSurveyDepth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxUseNT = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartSurveyDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndSurveyDepth)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonAll.ForeColor = System.Drawing.Color.White;
            this.radioButtonAll.Location = new System.Drawing.Point(15, 17);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(42, 17);
            this.radioButtonAll.TabIndex = 0;
            this.radioButtonAll.Text = "All";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonAccepted
            // 
            this.radioButtonAccepted.AutoSize = true;
            this.radioButtonAccepted.Checked = true;
            this.radioButtonAccepted.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonAccepted.ForeColor = System.Drawing.Color.White;
            this.radioButtonAccepted.Location = new System.Drawing.Point(101, 17);
            this.radioButtonAccepted.Name = "radioButtonAccepted";
            this.radioButtonAccepted.Size = new System.Drawing.Size(118, 17);
            this.radioButtonAccepted.TabIndex = 1;
            this.radioButtonAccepted.TabStop = true;
            this.radioButtonAccepted.Text = "Accepted Only";
            this.radioButtonAccepted.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(25, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start ";
            // 
            // numericUpDownStartSurveyDepth
            // 
            this.numericUpDownStartSurveyDepth.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownStartSurveyDepth.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownStartSurveyDepth.Location = new System.Drawing.Point(86, 35);
            this.numericUpDownStartSurveyDepth.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDownStartSurveyDepth.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownStartSurveyDepth.Name = "numericUpDownStartSurveyDepth";
            this.numericUpDownStartSurveyDepth.Size = new System.Drawing.Size(77, 27);
            this.numericUpDownStartSurveyDepth.TabIndex = 3;
            // 
            // numericUpDownEndSurveyDepth
            // 
            this.numericUpDownEndSurveyDepth.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownEndSurveyDepth.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownEndSurveyDepth.Location = new System.Drawing.Point(86, 82);
            this.numericUpDownEndSurveyDepth.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDownEndSurveyDepth.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownEndSurveyDepth.Name = "numericUpDownEndSurveyDepth";
            this.numericUpDownEndSurveyDepth.Size = new System.Drawing.Size(77, 27);
            this.numericUpDownEndSurveyDepth.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(25, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "End";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownEndSurveyDepth);
            this.groupBox1.Controls.Add(this.numericUpDownStartSurveyDepth);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(15, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(204, 130);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Survey Depth";
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(80, 271);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(85, 50);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxUseNT
            // 
            this.checkBoxUseNT.AutoSize = true;
            this.checkBoxUseNT.Checked = true;
            this.checkBoxUseNT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseNT.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUseNT.ForeColor = System.Drawing.Color.White;
            this.checkBoxUseNT.Location = new System.Drawing.Point(15, 197);
            this.checkBoxUseNT.Name = "checkBoxUseNT";
            this.checkBoxUseNT.Size = new System.Drawing.Size(122, 17);
            this.checkBoxUseNT.TabIndex = 8;
            this.checkBoxUseNT.Text = "Use NanoTesla";
            this.checkBoxUseNT.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxUseNT);
            this.groupBox2.Controls.Add(this.radioButtonAccepted);
            this.groupBox2.Controls.Add(this.radioButtonAll);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Location = new System.Drawing.Point(9, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 238);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            // 
            // FormSurveyExportFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(256, 345);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSurveyExportFilter";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Survey Export Filter";
            this.Load += new System.EventHandler(this.FormSurveyExportFilter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartSurveyDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndSurveyDepth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.RadioButton radioButtonAccepted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownStartSurveyDepth;
        private System.Windows.Forms.NumericUpDown numericUpDownEndSurveyDepth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxUseNT;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}