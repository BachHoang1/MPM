namespace MPM.GUI
{
    partial class FormThresholdDPoint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThresholdDPoint));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelHigh = new System.Windows.Forms.Label();
            this.textBoxHighThreshold = new System.Windows.Forms.TextBox();
            this.labelLow = new System.Windows.Forms.Label();
            this.textBoxLowThreshold = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelHigh);
            this.groupBox1.Controls.Add(this.textBoxHighThreshold);
            this.groupBox1.Controls.Add(this.labelLow);
            this.groupBox1.Controls.Add(this.textBoxLowThreshold);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 101);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // labelHigh
            // 
            this.labelHigh.AutoSize = true;
            this.labelHigh.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHigh.ForeColor = System.Drawing.Color.White;
            this.labelHigh.Location = new System.Drawing.Point(23, 32);
            this.labelHigh.Name = "labelHigh";
            this.labelHigh.Size = new System.Drawing.Size(36, 13);
            this.labelHigh.TabIndex = 9;
            this.labelHigh.Text = "High";
            // 
            // textBoxHighThreshold
            // 
            this.textBoxHighThreshold.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxHighThreshold.Location = new System.Drawing.Point(65, 29);
            this.textBoxHighThreshold.Name = "textBoxHighThreshold";
            this.textBoxHighThreshold.Size = new System.Drawing.Size(91, 21);
            this.textBoxHighThreshold.TabIndex = 0;
            // 
            // labelLow
            // 
            this.labelLow.AutoSize = true;
            this.labelLow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLow.ForeColor = System.Drawing.Color.White;
            this.labelLow.Location = new System.Drawing.Point(23, 63);
            this.labelLow.Name = "labelLow";
            this.labelLow.Size = new System.Drawing.Size(32, 13);
            this.labelLow.TabIndex = 7;
            this.labelLow.Text = "Low";
            // 
            // textBoxLowThreshold
            // 
            this.textBoxLowThreshold.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLowThreshold.Location = new System.Drawing.Point(65, 60);
            this.textBoxLowThreshold.Name = "textBoxLowThreshold";
            this.textBoxLowThreshold.Size = new System.Drawing.Size(91, 21);
            this.textBoxLowThreshold.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(70, 115);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(63, 47);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormThresholdDPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(199, 174);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormThresholdDPoint";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Thresholds";
            this.Load += new System.EventHandler(this.FormThresholdDPoint_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelHigh;
        private System.Windows.Forms.TextBox textBoxHighThreshold;
        private System.Windows.Forms.Label labelLow;
        private System.Windows.Forms.TextBox textBoxLowThreshold;
        private System.Windows.Forms.Button buttonOK;
    }
}