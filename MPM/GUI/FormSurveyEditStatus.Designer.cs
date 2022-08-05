namespace MPM.GUI
{
    partial class FormSurveyEditStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSurveyEditStatus));
            this.radioButtonAccept = new System.Windows.Forms.RadioButton();
            this.radioButtonReject = new System.Windows.Forms.RadioButton();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonAccept
            // 
            this.radioButtonAccept.AutoSize = true;
            this.radioButtonAccept.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonAccept.ForeColor = System.Drawing.Color.White;
            this.radioButtonAccept.Location = new System.Drawing.Point(32, 25);
            this.radioButtonAccept.Name = "radioButtonAccept";
            this.radioButtonAccept.Size = new System.Drawing.Size(89, 21);
            this.radioButtonAccept.TabIndex = 0;
            this.radioButtonAccept.TabStop = true;
            this.radioButtonAccept.Text = "ACCEPT";
            this.radioButtonAccept.UseVisualStyleBackColor = true;
            // 
            // radioButtonReject
            // 
            this.radioButtonReject.AutoSize = true;
            this.radioButtonReject.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonReject.ForeColor = System.Drawing.Color.White;
            this.radioButtonReject.Location = new System.Drawing.Point(32, 85);
            this.radioButtonReject.Name = "radioButtonReject";
            this.radioButtonReject.Size = new System.Drawing.Size(86, 21);
            this.radioButtonReject.TabIndex = 1;
            this.radioButtonReject.TabStop = true;
            this.radioButtonReject.Text = "REJECT";
            this.radioButtonReject.UseVisualStyleBackColor = true;
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNone.ForeColor = System.Drawing.Color.White;
            this.radioButtonNone.Location = new System.Drawing.Point(32, 145);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(70, 21);
            this.radioButtonNone.TabIndex = 2;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "NONE";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(52, 223);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 50);
            this.buttonOK.TabIndex = 34;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonNone);
            this.groupBox1.Controls.Add(this.radioButtonReject);
            this.groupBox1.Controls.Add(this.radioButtonAccept);
            this.groupBox1.Location = new System.Drawing.Point(16, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 183);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            // 
            // FormSurveyEditStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(182, 294);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSurveyEditStatus";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Survey Status";
            this.Load += new System.EventHandler(this.FormSurveyEditStatus_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonAccept;
        private System.Windows.Forms.RadioButton radioButtonReject;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}