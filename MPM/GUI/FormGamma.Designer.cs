namespace MPM.GUI
{
    partial class FormGamma
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
            this.tableLayoutPanelGammaInfo = new System.Windows.Forms.TableLayoutPanel();
            this.userControlDPointROP = new MPM.GUI.UserControlDPoint();
            this.userControlDPointGammaDepth = new MPM.GUI.UserControlDPoint();
            this.userControlDPointGamma = new MPM.GUI.UserControlDPoint();
            this.tableLayoutPanelGammaInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelGammaInfo
            // 
            this.tableLayoutPanelGammaInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.tableLayoutPanelGammaInfo.ColumnCount = 1;
            this.tableLayoutPanelGammaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGammaInfo.Controls.Add(this.userControlDPointROP, 0, 2);
            this.tableLayoutPanelGammaInfo.Controls.Add(this.userControlDPointGammaDepth, 0, 1);
            this.tableLayoutPanelGammaInfo.Controls.Add(this.userControlDPointGamma, 0, 0);
            this.tableLayoutPanelGammaInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGammaInfo.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelGammaInfo.Name = "tableLayoutPanelGammaInfo";
            this.tableLayoutPanelGammaInfo.RowCount = 3;
            this.tableLayoutPanelGammaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelGammaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelGammaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelGammaInfo.Size = new System.Drawing.Size(255, 168);
            this.tableLayoutPanelGammaInfo.TabIndex = 3;
            // 
            // userControlDPointROP
            // 
            this.userControlDPointROP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlDPointROP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDPointROP.Location = new System.Drawing.Point(3, 115);
            this.userControlDPointROP.Name = "userControlDPointROP";
            this.userControlDPointROP.Size = new System.Drawing.Size(249, 50);
            this.userControlDPointROP.TabIndex = 2;
            // 
            // userControlDPointGammaDepth
            // 
            this.userControlDPointGammaDepth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlDPointGammaDepth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDPointGammaDepth.Location = new System.Drawing.Point(3, 59);
            this.userControlDPointGammaDepth.Name = "userControlDPointGammaDepth";
            this.userControlDPointGammaDepth.Size = new System.Drawing.Size(249, 50);
            this.userControlDPointGammaDepth.TabIndex = 1;
            // 
            // userControlDPointGamma
            // 
            this.userControlDPointGamma.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlDPointGamma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDPointGamma.Location = new System.Drawing.Point(3, 3);
            this.userControlDPointGamma.Name = "userControlDPointGamma";
            this.userControlDPointGamma.Size = new System.Drawing.Size(249, 50);
            this.userControlDPointGamma.TabIndex = 0;
            // 
            // FormGamma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.ClientSize = new System.Drawing.Size(255, 168);
            this.Controls.Add(this.tableLayoutPanelGammaInfo);
            this.HideOnClose = true;
            this.Name = "FormGamma";
            this.Text = "Gamma";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGamma_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormGamma_FormClosed);
            this.Load += new System.EventHandler(this.FormGamma_Load);
            this.Resize += new System.EventHandler(this.FormGamma_Resize);
            this.tableLayoutPanelGammaInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlDPoint userControlDPointGamma;
        private UserControlDPoint userControlDPointGammaDepth;
        private UserControlDPoint userControlDPointROP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGammaInfo;
    }
}