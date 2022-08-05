namespace MPM.GUI
{
    partial class FormPumpPowerTemp
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
            this.tableLayoutPanelPump = new System.Windows.Forms.TableLayoutPanel();
            this.userControlPump = new MPM.GUI.UserControlPump();
            this.userControlTemperature = new MPM.GUI.UserControlTemperature();
            this.userControlBattery = new MPM.GUI.UserControlBattery();
            this.userControlSNR = new MPM.GUI.UserControlSNR();
            this.userControlPulseHeight = new MPM.GUI.UserControlPulseHeight();
            this.splitContainerNET = new System.Windows.Forms.SplitContainer();
            this.labelNETValue = new System.Windows.Forms.Label();
            this.labelNETDate = new System.Windows.Forms.Label();
            this.splitContainerFormationResistance = new System.Windows.Forms.SplitContainer();
            this.labelFormationResistanceValue = new System.Windows.Forms.Label();
            this.labelFormationResistanceDate = new System.Windows.Forms.Label();
            this.tableLayoutPanelPump.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNET)).BeginInit();
            this.splitContainerNET.Panel1.SuspendLayout();
            this.splitContainerNET.Panel2.SuspendLayout();
            this.splitContainerNET.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFormationResistance)).BeginInit();
            this.splitContainerFormationResistance.Panel1.SuspendLayout();
            this.splitContainerFormationResistance.Panel2.SuspendLayout();
            this.splitContainerFormationResistance.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPump
            // 
            this.tableLayoutPanelPump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.tableLayoutPanelPump.ColumnCount = 1;
            this.tableLayoutPanelPump.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPump.Controls.Add(this.userControlPump, 0, 0);
            this.tableLayoutPanelPump.Controls.Add(this.userControlTemperature, 0, 3);
            this.tableLayoutPanelPump.Controls.Add(this.userControlBattery, 0, 2);
            this.tableLayoutPanelPump.Controls.Add(this.userControlSNR, 0, 4);
            this.tableLayoutPanelPump.Controls.Add(this.userControlPulseHeight, 0, 1);
            this.tableLayoutPanelPump.Controls.Add(this.splitContainerNET, 0, 5);
            this.tableLayoutPanelPump.Controls.Add(this.splitContainerFormationResistance, 0, 6);
            this.tableLayoutPanelPump.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPump.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanelPump.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPump.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelPump.Name = "tableLayoutPanelPump";
            this.tableLayoutPanelPump.RowCount = 7;
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.0785F));
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.70952F));
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.86199F));
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.34999F));
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPump.Size = new System.Drawing.Size(202, 594);
            this.tableLayoutPanelPump.TabIndex = 1;
            // 
            // userControlPump
            // 
            this.userControlPump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlPump.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlPump.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlPump.Location = new System.Drawing.Point(4, 4);
            this.userControlPump.Margin = new System.Windows.Forms.Padding(4);
            this.userControlPump.Name = "userControlPump";
            this.userControlPump.Size = new System.Drawing.Size(194, 121);
            this.userControlPump.TabIndex = 0;
            // 
            // userControlTemperature
            // 
            this.userControlTemperature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlTemperature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlTemperature.Location = new System.Drawing.Point(4, 331);
            this.userControlTemperature.Margin = new System.Windows.Forms.Padding(4);
            this.userControlTemperature.Name = "userControlTemperature";
            this.userControlTemperature.Size = new System.Drawing.Size(194, 120);
            this.userControlTemperature.TabIndex = 1;
            // 
            // userControlBattery
            // 
            this.userControlBattery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlBattery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlBattery.Location = new System.Drawing.Point(4, 208);
            this.userControlBattery.Margin = new System.Windows.Forms.Padding(4);
            this.userControlBattery.Name = "userControlBattery";
            this.userControlBattery.Size = new System.Drawing.Size(194, 115);
            this.userControlBattery.TabIndex = 2;
            // 
            // userControlSNR
            // 
            this.userControlSNR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlSNR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSNR.Location = new System.Drawing.Point(5, 460);
            this.userControlSNR.Margin = new System.Windows.Forms.Padding(5);
            this.userControlSNR.Name = "userControlSNR";
            this.userControlSNR.Size = new System.Drawing.Size(192, 87);
            this.userControlSNR.TabIndex = 9;
            this.userControlSNR.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.userControlSNR_MouseDoubleClick);
            // 
            // userControlPulseHeight
            // 
            this.userControlPulseHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlPulseHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlPulseHeight.Location = new System.Drawing.Point(4, 133);
            this.userControlPulseHeight.Margin = new System.Windows.Forms.Padding(4);
            this.userControlPulseHeight.Name = "userControlPulseHeight";
            this.userControlPulseHeight.Size = new System.Drawing.Size(194, 67);
            this.userControlPulseHeight.TabIndex = 10;
            this.userControlPulseHeight.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.userControlPulseHeight_MouseDoubleClick);
            // 
            // splitContainerNET
            // 
            this.splitContainerNET.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerNET.Location = new System.Drawing.Point(3, 555);
            this.splitContainerNET.Name = "splitContainerNET";
            // 
            // splitContainerNET.Panel1
            // 
            this.splitContainerNET.Panel1.Controls.Add(this.labelNETValue);
            // 
            // splitContainerNET.Panel2
            // 
            this.splitContainerNET.Panel2.Controls.Add(this.labelNETDate);
            this.splitContainerNET.Panel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainerNET.Size = new System.Drawing.Size(196, 14);
            this.splitContainerNET.SplitterDistance = 105;
            this.splitContainerNET.TabIndex = 11;
            // 
            // labelNETValue
            // 
            this.labelNETValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNETValue.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNETValue.Location = new System.Drawing.Point(0, 0);
            this.labelNETValue.Name = "labelNETValue";
            this.labelNETValue.Size = new System.Drawing.Size(105, 14);
            this.labelNETValue.TabIndex = 0;
            this.labelNETValue.Tag = "20002";
            this.labelNETValue.Text = "NET: ---";
            this.labelNETValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelNETDate
            // 
            this.labelNETDate.AutoSize = true;
            this.labelNETDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNETDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNETDate.Location = new System.Drawing.Point(0, 0);
            this.labelNETDate.Name = "labelNETDate";
            this.labelNETDate.Size = new System.Drawing.Size(86, 13);
            this.labelNETDate.TabIndex = 0;
            this.labelNETDate.Text = "12:00:00 AM";
            // 
            // splitContainerFormationResistance
            // 
            this.splitContainerFormationResistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFormationResistance.Location = new System.Drawing.Point(3, 575);
            this.splitContainerFormationResistance.Name = "splitContainerFormationResistance";
            // 
            // splitContainerFormationResistance.Panel1
            // 
            this.splitContainerFormationResistance.Panel1.Controls.Add(this.labelFormationResistanceValue);
            // 
            // splitContainerFormationResistance.Panel2
            // 
            this.splitContainerFormationResistance.Panel2.Controls.Add(this.labelFormationResistanceDate);
            this.splitContainerFormationResistance.Size = new System.Drawing.Size(196, 16);
            this.splitContainerFormationResistance.SplitterDistance = 105;
            this.splitContainerFormationResistance.TabIndex = 12;
            // 
            // labelFormationResistanceValue
            // 
            this.labelFormationResistanceValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFormationResistanceValue.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFormationResistanceValue.Location = new System.Drawing.Point(0, 0);
            this.labelFormationResistanceValue.Name = "labelFormationResistanceValue";
            this.labelFormationResistanceValue.Size = new System.Drawing.Size(105, 16);
            this.labelFormationResistanceValue.TabIndex = 0;
            this.labelFormationResistanceValue.Tag = "47";
            this.labelFormationResistanceValue.Text = "FrmRes: ---";
            this.labelFormationResistanceValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelFormationResistanceValue.DoubleClick += new System.EventHandler(this.labelFormationResistanceValue_DoubleClick);
            // 
            // labelFormationResistanceDate
            // 
            this.labelFormationResistanceDate.AutoSize = true;
            this.labelFormationResistanceDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFormationResistanceDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFormationResistanceDate.Location = new System.Drawing.Point(0, 0);
            this.labelFormationResistanceDate.Name = "labelFormationResistanceDate";
            this.labelFormationResistanceDate.Size = new System.Drawing.Size(86, 13);
            this.labelFormationResistanceDate.TabIndex = 1;
            this.labelFormationResistanceDate.Text = "12:00:00 AM";
            // 
            // FormPumpPowerTemp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.ClientSize = new System.Drawing.Size(202, 594);
            this.Controls.Add(this.tableLayoutPanelPump);
            this.HideOnClose = true;
            this.Name = "FormPumpPowerTemp";
            this.Text = "Pump, Power, Temp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPumpPowerTemp_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPumpPowerTemp_FormClosed);
            this.Load += new System.EventHandler(this.FormPumpPowerTemp_Load);
            this.Resize += new System.EventHandler(this.FormPumpPowerTemp_Resize);
            this.tableLayoutPanelPump.ResumeLayout(false);
            this.splitContainerNET.Panel1.ResumeLayout(false);
            this.splitContainerNET.Panel2.ResumeLayout(false);
            this.splitContainerNET.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNET)).EndInit();
            this.splitContainerNET.ResumeLayout(false);
            this.splitContainerFormationResistance.Panel1.ResumeLayout(false);
            this.splitContainerFormationResistance.Panel2.ResumeLayout(false);
            this.splitContainerFormationResistance.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFormationResistance)).EndInit();
            this.splitContainerFormationResistance.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlPump userControlPump;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPump;
        private GUI.UserControlTemperature userControlTemperature;
        private GUI.UserControlBattery userControlBattery;
        private UserControlSNR userControlSNR;
        private UserControlPulseHeight userControlPulseHeight;
        private System.Windows.Forms.SplitContainer splitContainerNET;
        private System.Windows.Forms.Label labelNETValue;
        private System.Windows.Forms.Label labelNETDate;
        private System.Windows.Forms.SplitContainer splitContainerFormationResistance;
        private System.Windows.Forms.Label labelFormationResistanceValue;
        private System.Windows.Forms.Label labelFormationResistanceDate;
    }
}