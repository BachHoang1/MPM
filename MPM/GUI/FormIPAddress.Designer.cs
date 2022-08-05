namespace MPM.GUI
{
    partial class FormIPAddress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIPAddress));
            this.checkBoxLocalHost = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPortNumberMP = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPortNumberEM = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.maskedTextBoxIPMP4 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIPMP3 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIPMP2 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIPMP1 = new System.Windows.Forms.MaskedTextBox();
            this.checkBoxLocalHostMP = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.maskedTextBoxIP4 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP3 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP2 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP1 = new System.Windows.Forms.MaskedTextBox();
            this.checkBoxAPSMP = new System.Windows.Forms.CheckBox();
            this.checkBoxAPSEM = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxLocalHost
            // 
            this.checkBoxLocalHost.AutoSize = true;
            this.checkBoxLocalHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.checkBoxLocalHost.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxLocalHost.ForeColor = System.Drawing.Color.White;
            this.checkBoxLocalHost.Location = new System.Drawing.Point(13, 49);
            this.checkBoxLocalHost.Name = "checkBoxLocalHost";
            this.checkBoxLocalHost.Size = new System.Drawing.Size(85, 17);
            this.checkBoxLocalHost.TabIndex = 0;
            this.checkBoxLocalHost.Text = "Set Local";
            this.checkBoxLocalHost.UseVisualStyleBackColor = false;
            this.checkBoxLocalHost.CheckedChanged += new System.EventHandler(this.checkBoxLocalHost_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(86, 339);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 48);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxAPSEM);
            this.groupBox1.Controls.Add(this.checkBoxAPSMP);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxPortNumberMP);
            this.groupBox1.Controls.Add(this.labelPort);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxPortNumberEM);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.maskedTextBoxIPMP4);
            this.groupBox1.Controls.Add(this.maskedTextBoxIPMP3);
            this.groupBox1.Controls.Add(this.maskedTextBoxIPMP2);
            this.groupBox1.Controls.Add(this.maskedTextBoxIPMP1);
            this.groupBox1.Controls.Add(this.checkBoxLocalHostMP);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP4);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP3);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP2);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP1);
            this.groupBox1.Controls.Add(this.checkBoxLocalHost);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 312);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(14, 276);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Port #:";
            // 
            // textBoxPortNumberMP
            // 
            this.textBoxPortNumberMP.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPortNumberMP.Location = new System.Drawing.Point(67, 273);
            this.textBoxPortNumberMP.Name = "textBoxPortNumberMP";
            this.textBoxPortNumberMP.Size = new System.Drawing.Size(100, 21);
            this.textBoxPortNumberMP.TabIndex = 11;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPort.ForeColor = System.Drawing.Color.White;
            this.labelPort.Location = new System.Drawing.Point(14, 114);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(51, 13);
            this.labelPort.TabIndex = 17;
            this.labelPort.Text = "Port #:";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(109, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(5, 5);
            this.label4.TabIndex = 22;
            // 
            // textBoxPortNumberEM
            // 
            this.textBoxPortNumberEM.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPortNumberEM.Location = new System.Drawing.Point(67, 111);
            this.textBoxPortNumberEM.Name = "textBoxPortNumberEM";
            this.textBoxPortNumberEM.Size = new System.Drawing.Size(100, 21);
            this.textBoxPortNumberEM.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(162, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(5, 5);
            this.label5.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(56, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(5, 5);
            this.label6.TabIndex = 18;
            // 
            // maskedTextBoxIPMP4
            // 
            this.maskedTextBoxIPMP4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIPMP4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIPMP4.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIPMP4.Location = new System.Drawing.Point(172, 240);
            this.maskedTextBoxIPMP4.Mask = "0##";
            this.maskedTextBoxIPMP4.Name = "maskedTextBoxIPMP4";
            this.maskedTextBoxIPMP4.PromptChar = ' ';
            this.maskedTextBoxIPMP4.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIPMP4.TabIndex = 10;
            this.maskedTextBoxIPMP4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIPMP4.Enter += new System.EventHandler(this.maskedTextBoxIPMP4_Enter);
            // 
            // maskedTextBoxIPMP3
            // 
            this.maskedTextBoxIPMP3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIPMP3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIPMP3.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIPMP3.Location = new System.Drawing.Point(120, 240);
            this.maskedTextBoxIPMP3.Mask = "0##";
            this.maskedTextBoxIPMP3.Name = "maskedTextBoxIPMP3";
            this.maskedTextBoxIPMP3.PromptChar = ' ';
            this.maskedTextBoxIPMP3.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIPMP3.TabIndex = 9;
            this.maskedTextBoxIPMP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIPMP3.Enter += new System.EventHandler(this.maskedTextBoxIPMP3_Enter);
            // 
            // maskedTextBoxIPMP2
            // 
            this.maskedTextBoxIPMP2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIPMP2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIPMP2.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIPMP2.Location = new System.Drawing.Point(66, 240);
            this.maskedTextBoxIPMP2.Mask = "0##";
            this.maskedTextBoxIPMP2.Name = "maskedTextBoxIPMP2";
            this.maskedTextBoxIPMP2.PromptChar = ' ';
            this.maskedTextBoxIPMP2.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIPMP2.TabIndex = 8;
            this.maskedTextBoxIPMP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIPMP2.Enter += new System.EventHandler(this.maskedTextBoxIPMP2_Enter);
            // 
            // maskedTextBoxIPMP1
            // 
            this.maskedTextBoxIPMP1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIPMP1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIPMP1.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIPMP1.HidePromptOnLeave = true;
            this.maskedTextBoxIPMP1.Location = new System.Drawing.Point(13, 240);
            this.maskedTextBoxIPMP1.Mask = "0##";
            this.maskedTextBoxIPMP1.Name = "maskedTextBoxIPMP1";
            this.maskedTextBoxIPMP1.PromptChar = ' ';
            this.maskedTextBoxIPMP1.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIPMP1.TabIndex = 7;
            this.maskedTextBoxIPMP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIPMP1.Enter += new System.EventHandler(this.maskedTextBoxIPMP1_Enter);
            // 
            // checkBoxLocalHostMP
            // 
            this.checkBoxLocalHostMP.AutoSize = true;
            this.checkBoxLocalHostMP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.checkBoxLocalHostMP.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxLocalHostMP.ForeColor = System.Drawing.Color.White;
            this.checkBoxLocalHostMP.Location = new System.Drawing.Point(13, 212);
            this.checkBoxLocalHostMP.Name = "checkBoxLocalHostMP";
            this.checkBoxLocalHostMP.Size = new System.Drawing.Size(85, 17);
            this.checkBoxLocalHostMP.TabIndex = 6;
            this.checkBoxLocalHostMP.Text = "Set Local";
            this.checkBoxLocalHostMP.UseVisualStyleBackColor = false;
            this.checkBoxLocalHostMP.CheckedChanged += new System.EventHandler(this.checkBoxLocalHostMP_CheckedChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(109, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(5, 5);
            this.label3.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(162, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(5, 5);
            this.label2.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(56, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(5, 5);
            this.label1.TabIndex = 11;
            // 
            // maskedTextBoxIP4
            // 
            this.maskedTextBoxIP4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP4.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP4.Location = new System.Drawing.Point(172, 79);
            this.maskedTextBoxIP4.Mask = "0##";
            this.maskedTextBoxIP4.Name = "maskedTextBoxIP4";
            this.maskedTextBoxIP4.PromptChar = ' ';
            this.maskedTextBoxIP4.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP4.TabIndex = 4;
            this.maskedTextBoxIP4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP4.Enter += new System.EventHandler(this.maskedTextBoxIP4_Enter);
            // 
            // maskedTextBoxIP3
            // 
            this.maskedTextBoxIP3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP3.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP3.Location = new System.Drawing.Point(120, 79);
            this.maskedTextBoxIP3.Mask = "0##";
            this.maskedTextBoxIP3.Name = "maskedTextBoxIP3";
            this.maskedTextBoxIP3.PromptChar = ' ';
            this.maskedTextBoxIP3.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP3.TabIndex = 3;
            this.maskedTextBoxIP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP3.Enter += new System.EventHandler(this.maskedTextBoxIP3_Enter);
            // 
            // maskedTextBoxIP2
            // 
            this.maskedTextBoxIP2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP2.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP2.Location = new System.Drawing.Point(66, 79);
            this.maskedTextBoxIP2.Mask = "0##";
            this.maskedTextBoxIP2.Name = "maskedTextBoxIP2";
            this.maskedTextBoxIP2.PromptChar = ' ';
            this.maskedTextBoxIP2.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP2.TabIndex = 2;
            this.maskedTextBoxIP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP2.Enter += new System.EventHandler(this.maskedTextBoxIP2_Enter);
            // 
            // maskedTextBoxIP1
            // 
            this.maskedTextBoxIP1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP1.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP1.HidePromptOnLeave = true;
            this.maskedTextBoxIP1.Location = new System.Drawing.Point(13, 79);
            this.maskedTextBoxIP1.Mask = "0##";
            this.maskedTextBoxIP1.Name = "maskedTextBoxIP1";
            this.maskedTextBoxIP1.PromptChar = ' ';
            this.maskedTextBoxIP1.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP1.TabIndex = 1;
            this.maskedTextBoxIP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxIP1.Enter += new System.EventHandler(this.maskedTextBoxIP1_Enter);
            // 
            // checkBoxAPSMP
            // 
            this.checkBoxAPSMP.AutoSize = true;
            this.checkBoxAPSMP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.checkBoxAPSMP.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAPSMP.ForeColor = System.Drawing.Color.White;
            this.checkBoxAPSMP.Location = new System.Drawing.Point(13, 184);
            this.checkBoxAPSMP.Name = "checkBoxAPSMP";
            this.checkBoxAPSMP.Size = new System.Drawing.Size(157, 17);
            this.checkBoxAPSMP.TabIndex = 23;
            this.checkBoxAPSMP.Text = "Detect MP Computer";
            this.checkBoxAPSMP.UseVisualStyleBackColor = false;
            this.checkBoxAPSMP.CheckedChanged += new System.EventHandler(this.checkBoxAPSMP_CheckedChanged);
            // 
            // checkBoxAPSEM
            // 
            this.checkBoxAPSEM.AutoSize = true;
            this.checkBoxAPSEM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.checkBoxAPSEM.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAPSEM.ForeColor = System.Drawing.Color.White;
            this.checkBoxAPSEM.Location = new System.Drawing.Point(13, 19);
            this.checkBoxAPSEM.Name = "checkBoxAPSEM";
            this.checkBoxAPSEM.Size = new System.Drawing.Size(157, 17);
            this.checkBoxAPSEM.TabIndex = 24;
            this.checkBoxAPSEM.Text = "Detect EM Computer";
            this.checkBoxAPSEM.UseVisualStyleBackColor = false;
            this.checkBoxAPSEM.CheckedChanged += new System.EventHandler(this.checkBoxAPSEM_CheckedChanged);
            // 
            // FormIPAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(247, 403);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormIPAddress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IP Address";
            this.Load += new System.EventHandler(this.FormIPAddress_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxLocalHost;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP3;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIPMP4;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIPMP3;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIPMP2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIPMP1;
        private System.Windows.Forms.CheckBox checkBoxLocalHostMP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPortNumberMP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPortNumberEM;
        private System.Windows.Forms.CheckBox checkBoxAPSEM;
        private System.Windows.Forms.CheckBox checkBoxAPSMP;
    }
}