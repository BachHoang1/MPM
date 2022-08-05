namespace License
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxOrganization = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonGenerateLicenseKey = new System.Windows.Forms.Button();
            this.textBoxLicenseKey = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxComputer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMACAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(29, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Computer Name:";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(169, 68);
            this.textBoxUser.MaxLength = 32;
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(149, 20);
            this.textBoxUser.TabIndex = 1;
            // 
            // textBoxOrganization
            // 
            this.textBoxOrganization.Location = new System.Drawing.Point(169, 99);
            this.textBoxOrganization.MaxLength = 32;
            this.textBoxOrganization.Name = "textBoxOrganization";
            this.textBoxOrganization.Size = new System.Drawing.Size(149, 20);
            this.textBoxOrganization.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(29, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Customer Name:";
            // 
            // buttonGenerateLicenseKey
            // 
            this.buttonGenerateLicenseKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonGenerateLicenseKey.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGenerateLicenseKey.Location = new System.Drawing.Point(20, 171);
            this.buttonGenerateLicenseKey.Name = "buttonGenerateLicenseKey";
            this.buttonGenerateLicenseKey.Size = new System.Drawing.Size(298, 47);
            this.buttonGenerateLicenseKey.TabIndex = 5;
            this.buttonGenerateLicenseKey.Text = "Generate License Key";
            this.buttonGenerateLicenseKey.UseVisualStyleBackColor = false;
            this.buttonGenerateLicenseKey.Click += new System.EventHandler(this.buttonGenerateLicenseKey_Click);
            // 
            // textBoxLicenseKey
            // 
            this.textBoxLicenseKey.Location = new System.Drawing.Point(22, 19);
            this.textBoxLicenseKey.Multiline = true;
            this.textBoxLicenseKey.Name = "textBoxLicenseKey";
            this.textBoxLicenseKey.Size = new System.Drawing.Size(255, 143);
            this.textBoxLicenseKey.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxLicenseKey);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(20, 231);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 185);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "License Key for Display 2.0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxOrganization);
            this.groupBox2.Controls.Add(this.textBoxUser);
            this.groupBox2.Controls.Add(this.textBoxComputer);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxMACAddress);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonGenerateLicenseKey);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Location = new System.Drawing.Point(12, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 437);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // textBoxComputer
            // 
            this.textBoxComputer.Location = new System.Drawing.Point(169, 33);
            this.textBoxComputer.MaxLength = 32;
            this.textBoxComputer.Name = "textBoxComputer";
            this.textBoxComputer.Size = new System.Drawing.Size(149, 20);
            this.textBoxComputer.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(17, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Company Name:";
            // 
            // textBoxMACAddress
            // 
            this.textBoxMACAddress.Location = new System.Drawing.Point(169, 135);
            this.textBoxMACAddress.MaxLength = 32;
            this.textBoxMACAddress.Name = "textBoxMACAddress";
            this.textBoxMACAddress.Size = new System.Drawing.Size(149, 20);
            this.textBoxMACAddress.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(17, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "MAC Address:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(370, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(370, 490);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "APS Display 2.0 License Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxOrganization;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonGenerateLicenseKey;
        private System.Windows.Forms.TextBox textBoxLicenseKey;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxMACAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxComputer;
        private System.Windows.Forms.Label label4;
    }
}

