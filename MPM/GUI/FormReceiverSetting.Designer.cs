namespace MPM.GUI
{
    partial class FormReceiverSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReceiverSetting));
            this.comboBoxReceiverType = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelReceiverType = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxReceiverType
            // 
            this.comboBoxReceiverType.FormattingEnabled = true;
            this.comboBoxReceiverType.Items.AddRange(new object[] {
            "None",
            "560B",
            "560C",
            "560R"});
            this.comboBoxReceiverType.Location = new System.Drawing.Point(34, 55);
            this.comboBoxReceiverType.Name = "comboBoxReceiverType";
            this.comboBoxReceiverType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxReceiverType.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(67, 131);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(90, 47);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelReceiverType
            // 
            this.labelReceiverType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReceiverType.ForeColor = System.Drawing.Color.White;
            this.labelReceiverType.Location = new System.Drawing.Point(6, 16);
            this.labelReceiverType.Name = "labelReceiverType";
            this.labelReceiverType.Size = new System.Drawing.Size(190, 33);
            this.labelReceiverType.TabIndex = 2;
            this.labelReceiverType.Text = "Specify the receiver type to enable plot optimizations.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelReceiverType);
            this.groupBox1.Controls.Add(this.comboBoxReceiverType);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 101);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // FormReceiverSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(221, 197);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormReceiverSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receiver Settings";
            this.Load += new System.EventHandler(this.FormReceiverSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxReceiverType;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelReceiverType;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}