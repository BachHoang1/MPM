namespace MPM.GUI
{
    partial class FormECD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormECD));
            this.labelAnnularPressure = new System.Windows.Forms.Label();
            this.textBoxAPressure = new System.Windows.Forms.TextBox();
            this.labelMudDensity = new System.Windows.Forms.Label();
            this.textBoxMudDensity = new System.Windows.Forms.TextBox();
            this.labelTVD = new System.Windows.Forms.Label();
            this.textBoxTVD = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAnnularPressure
            // 
            this.labelAnnularPressure.AutoSize = true;
            this.labelAnnularPressure.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAnnularPressure.ForeColor = System.Drawing.Color.White;
            this.labelAnnularPressure.Location = new System.Drawing.Point(32, 157);
            this.labelAnnularPressure.Name = "labelAnnularPressure";
            this.labelAnnularPressure.Size = new System.Drawing.Size(113, 13);
            this.labelAnnularPressure.TabIndex = 4;
            this.labelAnnularPressure.Text = "A Pressure (psi)";
            this.labelAnnularPressure.Visible = false;
            // 
            // textBoxAPressure
            // 
            this.textBoxAPressure.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAPressure.Location = new System.Drawing.Point(167, 149);
            this.textBoxAPressure.Name = "textBoxAPressure";
            this.textBoxAPressure.Size = new System.Drawing.Size(100, 21);
            this.textBoxAPressure.TabIndex = 3;
            this.textBoxAPressure.Visible = false;
            // 
            // labelMudDensity
            // 
            this.labelMudDensity.AutoSize = true;
            this.labelMudDensity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMudDensity.ForeColor = System.Drawing.Color.White;
            this.labelMudDensity.Location = new System.Drawing.Point(19, 75);
            this.labelMudDensity.Name = "labelMudDensity";
            this.labelMudDensity.Size = new System.Drawing.Size(142, 13);
            this.labelMudDensity.TabIndex = 6;
            this.labelMudDensity.Text = "Mud Density (lb/gal)";
            // 
            // textBoxMudDensity
            // 
            this.textBoxMudDensity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMudDensity.Location = new System.Drawing.Point(167, 72);
            this.textBoxMudDensity.Name = "textBoxMudDensity";
            this.textBoxMudDensity.Size = new System.Drawing.Size(100, 21);
            this.textBoxMudDensity.TabIndex = 1;
            // 
            // labelTVD
            // 
            this.labelTVD.AutoSize = true;
            this.labelTVD.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTVD.ForeColor = System.Drawing.Color.White;
            this.labelTVD.Location = new System.Drawing.Point(103, 33);
            this.labelTVD.Name = "labelTVD";
            this.labelTVD.Size = new System.Drawing.Size(58, 13);
            this.labelTVD.TabIndex = 8;
            this.labelTVD.Text = "TVD (ft)";
            // 
            // textBoxTVD
            // 
            this.textBoxTVD.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTVD.Location = new System.Drawing.Point(167, 30);
            this.textBoxTVD.Name = "textBoxTVD";
            this.textBoxTVD.Size = new System.Drawing.Size(100, 21);
            this.textBoxTVD.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxAPressure);
            this.groupBox1.Controls.Add(this.labelAnnularPressure);
            this.groupBox1.Controls.Add(this.labelTVD);
            this.groupBox1.Controls.Add(this.textBoxTVD);
            this.groupBox1.Controls.Add(this.labelMudDensity);
            this.groupBox1.Controls.Add(this.textBoxMudDensity);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 128);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enter Values";
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(118, 169);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(91, 46);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormECD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(336, 232);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormECD";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ECD Parameters";
            
            this.Load += new System.EventHandler(this.FormECD_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAnnularPressure;
        private System.Windows.Forms.TextBox textBoxAPressure;
        private System.Windows.Forms.Label labelMudDensity;
        private System.Windows.Forms.TextBox textBoxMudDensity;
        private System.Windows.Forms.Label labelTVD;
        private System.Windows.Forms.TextBox textBoxTVD;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOK;
    }
}