namespace MPM.GUI
{
    partial class FormRegisterProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegisterProduct));
            this.buttonRegister = new System.Windows.Forms.Button();
            this.textBoxLicenseKey = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxRegistrationInfo = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRegister
            // 
            this.buttonRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonRegister.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRegister.Location = new System.Drawing.Point(137, 238);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(95, 49);
            this.buttonRegister.TabIndex = 0;
            this.buttonRegister.Text = "Register";
            this.buttonRegister.UseVisualStyleBackColor = false;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // textBoxLicenseKey
            // 
            this.textBoxLicenseKey.Location = new System.Drawing.Point(22, 29);
            this.textBoxLicenseKey.Multiline = true;
            this.textBoxLicenseKey.Name = "textBoxLicenseKey";
            this.textBoxLicenseKey.Size = new System.Drawing.Size(296, 89);
            this.textBoxLicenseKey.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxLicenseKey);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(18, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 141);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enter License Key";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxRegistrationInfo);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.buttonRegister);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 302);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // textBoxRegistrationInfo
            // 
            this.textBoxRegistrationInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.textBoxRegistrationInfo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRegistrationInfo.ForeColor = System.Drawing.Color.White;
            this.textBoxRegistrationInfo.Location = new System.Drawing.Point(18, 19);
            this.textBoxRegistrationInfo.Multiline = true;
            this.textBoxRegistrationInfo.Name = "textBoxRegistrationInfo";
            this.textBoxRegistrationInfo.ReadOnly = true;
            this.textBoxRegistrationInfo.Size = new System.Drawing.Size(338, 52);
            this.textBoxRegistrationInfo.TabIndex = 4;
            this.textBoxRegistrationInfo.Text = "This product is licensed to:";
            // 
            // FormRegisterProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(397, 326);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegisterProduct";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Register Product";
            this.Load += new System.EventHandler(this.FormRegisterProduct_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.TextBox textBoxLicenseKey;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxRegistrationInfo;
    }
}