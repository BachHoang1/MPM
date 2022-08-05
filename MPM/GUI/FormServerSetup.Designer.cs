namespace MPM.GUI
{
    partial class FormServerSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServerSetup));
            this.checkBoxServer = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.maskedTextBoxIP4 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP3 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP2 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxIP1 = new System.Windows.Forms.MaskedTextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxPortNumber = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxServer
            // 
            this.checkBoxServer.AutoSize = true;
            this.checkBoxServer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxServer.ForeColor = System.Drawing.Color.White;
            this.checkBoxServer.Location = new System.Drawing.Point(28, 53);
            this.checkBoxServer.Name = "checkBoxServer";
            this.checkBoxServer.Size = new System.Drawing.Size(100, 17);
            this.checkBoxServer.TabIndex = 0;
            this.checkBoxServer.Text = "Is A Server";
            this.checkBoxServer.UseVisualStyleBackColor = true;
            this.checkBoxServer.CheckedChanged += new System.EventHandler(this.checkBoxServer_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP4);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP3);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP2);
            this.groupBox1.Controls.Add(this.maskedTextBoxIP1);
            this.groupBox1.Controls.Add(this.labelPort);
            this.groupBox1.Controls.Add(this.textBoxPortNumber);
            this.groupBox1.Controls.Add(this.checkBoxServer);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 186);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Check the box if this Display is a Server.  Otherwise, fill in the Server IP for " +
    "Client mode";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Client connects to Server IP:";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(121, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(5, 5);
            this.label3.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(174, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(5, 5);
            this.label2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(68, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(5, 5);
            this.label1.TabIndex = 17;
            // 
            // maskedTextBoxIP4
            // 
            this.maskedTextBoxIP4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP4.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP4.Location = new System.Drawing.Point(184, 106);
            this.maskedTextBoxIP4.Mask = "0##";
            this.maskedTextBoxIP4.Name = "maskedTextBoxIP4";
            this.maskedTextBoxIP4.PromptChar = ' ';
            this.maskedTextBoxIP4.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP4.TabIndex = 19;
            this.maskedTextBoxIP4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maskedTextBoxIP3
            // 
            this.maskedTextBoxIP3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP3.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP3.Location = new System.Drawing.Point(132, 106);
            this.maskedTextBoxIP3.Mask = "0##";
            this.maskedTextBoxIP3.Name = "maskedTextBoxIP3";
            this.maskedTextBoxIP3.PromptChar = ' ';
            this.maskedTextBoxIP3.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP3.TabIndex = 18;
            this.maskedTextBoxIP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maskedTextBoxIP2
            // 
            this.maskedTextBoxIP2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP2.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP2.Location = new System.Drawing.Point(78, 106);
            this.maskedTextBoxIP2.Mask = "0##";
            this.maskedTextBoxIP2.Name = "maskedTextBoxIP2";
            this.maskedTextBoxIP2.PromptChar = ' ';
            this.maskedTextBoxIP2.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP2.TabIndex = 16;
            this.maskedTextBoxIP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maskedTextBoxIP1
            // 
            this.maskedTextBoxIP1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.maskedTextBoxIP1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxIP1.ForeColor = System.Drawing.Color.White;
            this.maskedTextBoxIP1.HidePromptOnLeave = true;
            this.maskedTextBoxIP1.Location = new System.Drawing.Point(25, 106);
            this.maskedTextBoxIP1.Mask = "0##";
            this.maskedTextBoxIP1.Name = "maskedTextBoxIP1";
            this.maskedTextBoxIP1.PromptChar = ' ';
            this.maskedTextBoxIP1.Size = new System.Drawing.Size(40, 21);
            this.maskedTextBoxIP1.TabIndex = 15;
            this.maskedTextBoxIP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPort.Location = new System.Drawing.Point(22, 146);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(51, 13);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port #:";
            // 
            // textBoxPortNumber
            // 
            this.textBoxPortNumber.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPortNumber.Location = new System.Drawing.Point(82, 143);
            this.textBoxPortNumber.Name = "textBoxPortNumber";
            this.textBoxPortNumber.ReadOnly = true;
            this.textBoxPortNumber.Size = new System.Drawing.Size(100, 21);
            this.textBoxPortNumber.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(104, 215);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 48);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormServerSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(288, 276);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormServerSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Setup";
            this.Load += new System.EventHandler(this.FormServerSetup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxServer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPortNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP4;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP3;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIP1;
    }
}