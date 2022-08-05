namespace MPM.GUI
{
    partial class FormUnitSelectionByDimension
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUnitSelectionByDimension));
            this.radioButtonFt = new System.Windows.Forms.RadioButton();
            this.radioButtonM = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonC = new System.Windows.Forms.RadioButton();
            this.radioButtonF = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonKpa = new System.Windows.Forms.RadioButton();
            this.radioButtonPsi = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonFt
            // 
            this.radioButtonFt.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonFt.ForeColor = System.Drawing.Color.White;
            this.radioButtonFt.Location = new System.Drawing.Point(120, 16);
            this.radioButtonFt.Name = "radioButtonFt";
            this.radioButtonFt.Size = new System.Drawing.Size(83, 38);
            this.radioButtonFt.TabIndex = 0;
            this.radioButtonFt.TabStop = true;
            this.radioButtonFt.Text = "ft, ft/hr";
            this.radioButtonFt.UseVisualStyleBackColor = true;
            // 
            // radioButtonM
            // 
            this.radioButtonM.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonM.ForeColor = System.Drawing.Color.White;
            this.radioButtonM.Location = new System.Drawing.Point(211, 16);
            this.radioButtonM.Name = "radioButtonM";
            this.radioButtonM.Size = new System.Drawing.Size(84, 38);
            this.radioButtonM.TabIndex = 1;
            this.radioButtonM.TabStop = true;
            this.radioButtonM.Text = "m, m/hr";
            this.radioButtonM.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Length, Rate:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButtonM);
            this.groupBox1.Controls.Add(this.radioButtonFt);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 63);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.radioButtonC);
            this.groupBox2.Controls.Add(this.radioButtonF);
            this.groupBox2.Location = new System.Drawing.Point(12, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 63);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(17, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Temperature:";
            // 
            // radioButtonC
            // 
            this.radioButtonC.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonC.ForeColor = System.Drawing.Color.White;
            this.radioButtonC.Location = new System.Drawing.Point(211, 16);
            this.radioButtonC.Name = "radioButtonC";
            this.radioButtonC.Size = new System.Drawing.Size(64, 38);
            this.radioButtonC.TabIndex = 1;
            this.radioButtonC.TabStop = true;
            this.radioButtonC.Text = "°C";
            this.radioButtonC.UseVisualStyleBackColor = true;
            // 
            // radioButtonF
            // 
            this.radioButtonF.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonF.ForeColor = System.Drawing.Color.White;
            this.radioButtonF.Location = new System.Drawing.Point(123, 16);
            this.radioButtonF.Name = "radioButtonF";
            this.radioButtonF.Size = new System.Drawing.Size(64, 38);
            this.radioButtonF.TabIndex = 0;
            this.radioButtonF.TabStop = true;
            this.radioButtonF.Text = "°F";
            this.radioButtonF.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.radioButtonKpa);
            this.groupBox3.Controls.Add(this.radioButtonPsi);
            this.groupBox3.Location = new System.Drawing.Point(12, 103);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(314, 63);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(16, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pressure:";
            // 
            // radioButtonKpa
            // 
            this.radioButtonKpa.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonKpa.ForeColor = System.Drawing.Color.White;
            this.radioButtonKpa.Location = new System.Drawing.Point(211, 15);
            this.radioButtonKpa.Name = "radioButtonKpa";
            this.radioButtonKpa.Size = new System.Drawing.Size(64, 38);
            this.radioButtonKpa.TabIndex = 1;
            this.radioButtonKpa.TabStop = true;
            this.radioButtonKpa.Text = "kPa";
            this.radioButtonKpa.UseVisualStyleBackColor = true;
            // 
            // radioButtonPsi
            // 
            this.radioButtonPsi.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonPsi.ForeColor = System.Drawing.Color.White;
            this.radioButtonPsi.Location = new System.Drawing.Point(121, 15);
            this.radioButtonPsi.Name = "radioButtonPsi";
            this.radioButtonPsi.Size = new System.Drawing.Size(64, 38);
            this.radioButtonPsi.TabIndex = 0;
            this.radioButtonPsi.TabStop = true;
            this.radioButtonPsi.Text = "PSI";
            this.radioButtonPsi.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(130, 267);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(88, 52);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormUnitSelectionByDimension
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(349, 342);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUnitSelectionByDimension";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Units By Group";
            this.Load += new System.EventHandler(this.FormUnitSelectionByDimension_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonFt;
        private System.Windows.Forms.RadioButton radioButtonM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonC;
        private System.Windows.Forms.RadioButton radioButtonF;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonKpa;
        private System.Windows.Forms.RadioButton radioButtonPsi;
        private System.Windows.Forms.Button buttonOK;
    }
}