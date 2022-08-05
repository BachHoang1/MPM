namespace MPM.GUI
{
    partial class FormMSASettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMSASettings));
            this.labelURL = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.textBoxAPIKey = new System.Windows.Forms.TextBox();
            this.labelAPIKey = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSupportNum = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxJobID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRig = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBHA = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageJobInfo = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonSurveyMSAMode = new System.Windows.Forms.RadioButton();
            this.radioButtonSurveyConfirmationMode = new System.Windows.Forms.RadioButton();
            this.radioButtonSurveyFullAutomation = new System.Windows.Forms.RadioButton();
            this.checkBoxUseAcceptRejectFeature = new System.Windows.Forms.CheckBox();
            this.groupBoxSurveyManagement = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPageMSAProvider = new System.Windows.Forms.TabPage();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageJobInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxSurveyManagement.SuspendLayout();
            this.tabPageMSAProvider.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelURL.ForeColor = System.Drawing.Color.White;
            this.labelURL.Location = new System.Drawing.Point(44, 146);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(35, 13);
            this.labelURL.TabIndex = 0;
            this.labelURL.Text = "URL:";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxURL.Location = new System.Drawing.Point(83, 139);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(352, 21);
            this.textBoxURL.TabIndex = 0;
            // 
            // textBoxAPIKey
            // 
            this.textBoxAPIKey.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAPIKey.Location = new System.Drawing.Point(82, 176);
            this.textBoxAPIKey.Name = "textBoxAPIKey";
            this.textBoxAPIKey.Size = new System.Drawing.Size(353, 21);
            this.textBoxAPIKey.TabIndex = 2;
            this.textBoxAPIKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelAPIKey
            // 
            this.labelAPIKey.AutoSize = true;
            this.labelAPIKey.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAPIKey.ForeColor = System.Drawing.Color.White;
            this.labelAPIKey.Location = new System.Drawing.Point(18, 183);
            this.labelAPIKey.Name = "labelAPIKey";
            this.labelAPIKey.Size = new System.Drawing.Size(62, 13);
            this.labelAPIKey.TabIndex = 2;
            this.labelAPIKey.Text = "API Key:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelSupportNum);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.textBoxAPIKey);
            this.groupBox1.Controls.Add(this.labelAPIKey);
            this.groupBox1.Controls.Add(this.textBoxURL);
            this.groupBox1.Controls.Add(this.labelURL);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(455, 218);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // labelSupportNum
            // 
            this.labelSupportNum.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSupportNum.ForeColor = System.Drawing.Color.White;
            this.labelSupportNum.Location = new System.Drawing.Point(292, 51);
            this.labelSupportNum.Name = "labelSupportNum";
            this.labelSupportNum.Size = new System.Drawing.Size(157, 66);
            this.labelSupportNum.TabIndex = 4;
            this.labelSupportNum.Text = "Support: 1-844-eddilab        1-844-333-4522";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MPM.Properties.Resources.RoundLAB_Logo_Red_White;
            this.pictureBox1.Location = new System.Drawing.Point(21, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 93);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(213, 436);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 48);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxJobID
            // 
            this.textBoxJobID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxJobID.Location = new System.Drawing.Point(95, 25);
            this.textBoxJobID.Name = "textBoxJobID";
            this.textBoxJobID.Size = new System.Drawing.Size(336, 21);
            this.textBoxJobID.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(38, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Job ID:";
            // 
            // textBoxRig
            // 
            this.textBoxRig.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRig.Location = new System.Drawing.Point(95, 56);
            this.textBoxRig.Name = "textBoxRig";
            this.textBoxRig.Size = new System.Drawing.Size(336, 21);
            this.textBoxRig.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(59, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Rig:";
            // 
            // textBoxBHA
            // 
            this.textBoxBHA.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBHA.Location = new System.Drawing.Point(95, 87);
            this.textBoxBHA.Name = "textBoxBHA";
            this.textBoxBHA.Size = new System.Drawing.Size(336, 21);
            this.textBoxBHA.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(41, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "BHA #:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageJobInfo);
            this.tabControl1.Controls.Add(this.tabPageMSAProvider);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(479, 391);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPageJobInfo
            // 
            this.tabPageJobInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.tabPageJobInfo.Controls.Add(this.groupBox2);
            this.tabPageJobInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageJobInfo.Name = "tabPageJobInfo";
            this.tabPageJobInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJobInfo.Size = new System.Drawing.Size(471, 365);
            this.tabPageJobInfo.TabIndex = 0;
            this.tabPageJobInfo.Text = "Job Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonSurveyMSAMode);
            this.groupBox2.Controls.Add(this.radioButtonSurveyConfirmationMode);
            this.groupBox2.Controls.Add(this.radioButtonSurveyFullAutomation);
            this.groupBox2.Controls.Add(this.checkBoxUseAcceptRejectFeature);
            this.groupBox2.Controls.Add(this.textBoxJobID);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxBHA);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxRig);
            this.groupBox2.Controls.Add(this.groupBoxSurveyManagement);
            this.groupBox2.Location = new System.Drawing.Point(12, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 323);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // radioButtonSurveyMSAMode
            // 
            this.radioButtonSurveyMSAMode.AutoSize = true;
            this.radioButtonSurveyMSAMode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSurveyMSAMode.ForeColor = System.Drawing.Color.White;
            this.radioButtonSurveyMSAMode.Location = new System.Drawing.Point(37, 218);
            this.radioButtonSurveyMSAMode.Name = "radioButtonSurveyMSAMode";
            this.radioButtonSurveyMSAMode.Size = new System.Drawing.Size(90, 17);
            this.radioButtonSurveyMSAMode.TabIndex = 18;
            this.radioButtonSurveyMSAMode.TabStop = true;
            this.radioButtonSurveyMSAMode.Text = "MSA Mode";
            this.toolTip.SetToolTip(this.radioButtonSurveyMSAMode, "Full managed Survey Mode with interaction with Survey Management Groups.");
            this.radioButtonSurveyMSAMode.UseVisualStyleBackColor = true;
            this.radioButtonSurveyMSAMode.CheckedChanged += new System.EventHandler(this.radioButtonSurveyMSAMode_CheckedChanged);
            // 
            // radioButtonSurveyConfirmationMode
            // 
            this.radioButtonSurveyConfirmationMode.AutoSize = true;
            this.radioButtonSurveyConfirmationMode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSurveyConfirmationMode.ForeColor = System.Drawing.Color.White;
            this.radioButtonSurveyConfirmationMode.Location = new System.Drawing.Point(37, 186);
            this.radioButtonSurveyConfirmationMode.Name = "radioButtonSurveyConfirmationMode";
            this.radioButtonSurveyConfirmationMode.Size = new System.Drawing.Size(147, 17);
            this.radioButtonSurveyConfirmationMode.TabIndex = 17;
            this.radioButtonSurveyConfirmationMode.TabStop = true;
            this.radioButtonSurveyConfirmationMode.Text = "Confirmation Mode";
            this.toolTip.SetToolTip(this.radioButtonSurveyConfirmationMode, "This will force the Confirmation of the surveys before sending WITS.");
            this.radioButtonSurveyConfirmationMode.UseVisualStyleBackColor = true;
            this.radioButtonSurveyConfirmationMode.CheckedChanged += new System.EventHandler(this.radioButtonSurveyConfirmationMode_CheckedChanged);
            // 
            // radioButtonSurveyFullAutomation
            // 
            this.radioButtonSurveyFullAutomation.AutoSize = true;
            this.radioButtonSurveyFullAutomation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSurveyFullAutomation.ForeColor = System.Drawing.Color.White;
            this.radioButtonSurveyFullAutomation.Location = new System.Drawing.Point(37, 154);
            this.radioButtonSurveyFullAutomation.Name = "radioButtonSurveyFullAutomation";
            this.radioButtonSurveyFullAutomation.Size = new System.Drawing.Size(128, 17);
            this.radioButtonSurveyFullAutomation.TabIndex = 16;
            this.radioButtonSurveyFullAutomation.TabStop = true;
            this.radioButtonSurveyFullAutomation.Text = "Full Automation";
            this.toolTip.SetToolTip(this.radioButtonSurveyFullAutomation, "Send All Surveys without confirmation - Vector or Calculated.  This is for surfac" +
        "e hole and one man operations.  WITS will broadcast without confirmation.");
            this.radioButtonSurveyFullAutomation.UseVisualStyleBackColor = true;
            this.radioButtonSurveyFullAutomation.CheckedChanged += new System.EventHandler(this.radioButtonSurveyFullAutomation_CheckedChanged);
            // 
            // checkBoxUseAcceptRejectFeature
            // 
            this.checkBoxUseAcceptRejectFeature.AutoSize = true;
            this.checkBoxUseAcceptRejectFeature.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxUseAcceptRejectFeature.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUseAcceptRejectFeature.ForeColor = System.Drawing.Color.White;
            this.checkBoxUseAcceptRejectFeature.Location = new System.Drawing.Point(70, 250);
            this.checkBoxUseAcceptRejectFeature.Name = "checkBoxUseAcceptRejectFeature";
            this.checkBoxUseAcceptRejectFeature.Size = new System.Drawing.Size(206, 17);
            this.checkBoxUseAcceptRejectFeature.TabIndex = 14;
            this.checkBoxUseAcceptRejectFeature.Text = "Use Accept/Reject Feature:";
            this.checkBoxUseAcceptRejectFeature.UseVisualStyleBackColor = true;
            // 
            // groupBoxSurveyManagement
            // 
            this.groupBoxSurveyManagement.Controls.Add(this.label4);
            this.groupBoxSurveyManagement.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSurveyManagement.ForeColor = System.Drawing.Color.White;
            this.groupBoxSurveyManagement.Location = new System.Drawing.Point(18, 128);
            this.groupBoxSurveyManagement.Name = "groupBoxSurveyManagement";
            this.groupBoxSurveyManagement.Size = new System.Drawing.Size(413, 177);
            this.groupBoxSurveyManagement.TabIndex = 19;
            this.groupBoxSurveyManagement.TabStop = false;
            this.groupBoxSurveyManagement.Text = "Survey Management";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Yellow;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(8, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(397, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "NOTE: Accept/Reject feature only applies to client Displays.";
            // 
            // tabPageMSAProvider
            // 
            this.tabPageMSAProvider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.tabPageMSAProvider.Controls.Add(this.groupBox1);
            this.tabPageMSAProvider.Location = new System.Drawing.Point(4, 22);
            this.tabPageMSAProvider.Name = "tabPageMSAProvider";
            this.tabPageMSAProvider.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMSAProvider.Size = new System.Drawing.Size(471, 365);
            this.tabPageMSAProvider.TabIndex = 1;
            this.tabPageMSAProvider.Text = "Multi-Station Analysis Provider";
            // 
            // FormMSASettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(504, 496);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMSASettings";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Job Information and MSA";
            this.Load += new System.EventHandler(this.FormMSASettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageJobInfo.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxSurveyManagement.ResumeLayout(false);
            this.groupBoxSurveyManagement.PerformLayout();
            this.tabPageMSAProvider.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.TextBox textBoxAPIKey;
        private System.Windows.Forms.Label labelAPIKey;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelSupportNum;
        private System.Windows.Forms.TextBox textBoxJobID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRig;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBHA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageJobInfo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabPage tabPageMSAProvider;
        private System.Windows.Forms.CheckBox checkBoxUseAcceptRejectFeature;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonSurveyMSAMode;
        private System.Windows.Forms.RadioButton radioButtonSurveyConfirmationMode;
        private System.Windows.Forms.RadioButton radioButtonSurveyFullAutomation;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox groupBoxSurveyManagement;
    }
}