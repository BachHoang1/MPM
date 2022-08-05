namespace MPM.GUI
{
    partial class FormToolFace
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (m_penForLine != null)
                    m_penForLine.Dispose();
                if (m_penForCircle != null)
                    m_penForCircle.Dispose();
                if (m_penForCircleThin != null)
                    m_penForCircleThin.Dispose();
                if (m_brushSolidCircle != null)
                    m_brushSolidCircle.Dispose();
                if (m_brushPlots != null)
                    m_brushPlots.Dispose();
                if (m_brushPlots2 != null)
                    m_brushPlots2.Dispose();
                if (m_fontCenterTF != null)
                    m_fontCenterTF.Dispose();
                if (m_fontMediumCenterTF != null)
                    m_fontMediumCenterTF.Dispose();
                if (m_fontSmallCenterTF != null)
                    m_fontSmallCenterTF.Dispose();
                if (m_fontGeorgiaCenterTF != null)
                    m_fontGeorgiaCenterTF.Dispose();
                if (m_brushCenterTF != null)
                    m_brushCenterTF.Dispose();
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
            this.labelRight = new System.Windows.Forms.Label();
            this.labelLeft = new System.Windows.Forms.Label();
            this.labelDown = new System.Windows.Forms.Label();
            this.labelUp = new System.Windows.Forms.Label();
            this.pictureBoxToolFace = new System.Windows.Forms.PictureBox();
            this.userControlDPointArcAZM = new MPM.GUI.UserControlDPointArc();
            this.userControlDPointArcINC = new MPM.GUI.UserControlDPointArc();
            this.userControlDPointINC = new MPM.GUI.UserControlDPoint();
            this.userControlDPointAZM = new MPM.GUI.UserControlDPoint();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToolFace)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRight
            // 
            this.labelRight.AutoSize = true;
            this.labelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.labelRight.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRight.ForeColor = System.Drawing.Color.White;
            this.labelRight.Location = new System.Drawing.Point(444, 56);
            this.labelRight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRight.Name = "labelRight";
            this.labelRight.Size = new System.Drawing.Size(35, 13);
            this.labelRight.TabIndex = 4;
            this.labelRight.Text = "90 R";
            this.labelRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLeft
            // 
            this.labelLeft.AutoSize = true;
            this.labelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.labelLeft.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeft.ForeColor = System.Drawing.Color.White;
            this.labelLeft.Location = new System.Drawing.Point(374, 56);
            this.labelLeft.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLeft.Name = "labelLeft";
            this.labelLeft.Size = new System.Drawing.Size(34, 13);
            this.labelLeft.TabIndex = 3;
            this.labelLeft.Text = "90 L";
            this.labelLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDown
            // 
            this.labelDown.AutoSize = true;
            this.labelDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.labelDown.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDown.ForeColor = System.Drawing.Color.White;
            this.labelDown.Location = new System.Drawing.Point(296, 56);
            this.labelDown.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDown.Name = "labelDown";
            this.labelDown.Size = new System.Drawing.Size(52, 13);
            this.labelDown.TabIndex = 2;
            this.labelDown.Text = "180 Dn";
            this.labelDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUp
            // 
            this.labelUp.AutoSize = true;
            this.labelUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.labelUp.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUp.ForeColor = System.Drawing.Color.White;
            this.labelUp.Location = new System.Drawing.Point(223, 56);
            this.labelUp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUp.Name = "labelUp";
            this.labelUp.Size = new System.Drawing.Size(36, 13);
            this.labelUp.TabIndex = 1;
            this.labelUp.Text = "0 Up";
            this.labelUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxToolFace
            // 
            this.pictureBoxToolFace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.pictureBoxToolFace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxToolFace.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxToolFace.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxToolFace.Name = "pictureBoxToolFace";
            this.pictureBoxToolFace.Size = new System.Drawing.Size(826, 582);
            this.pictureBoxToolFace.TabIndex = 0;
            this.pictureBoxToolFace.TabStop = false;
            this.pictureBoxToolFace.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxToolFace_Paint);
            this.pictureBoxToolFace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxToolFace_MouseMove);
            // 
            // userControlDPointArcAZM
            // 
            this.userControlDPointArcAZM.BackColor = System.Drawing.Color.Maroon;
            this.userControlDPointArcAZM.Location = new System.Drawing.Point(679, 122);
            this.userControlDPointArcAZM.Margin = new System.Windows.Forms.Padding(2);
            this.userControlDPointArcAZM.Name = "userControlDPointArcAZM";
            this.userControlDPointArcAZM.Size = new System.Drawing.Size(90, 90);
            this.userControlDPointArcAZM.TabIndex = 9;
            this.userControlDPointArcAZM.Visible = false;
            // 
            // userControlDPointArcINC
            // 
            this.userControlDPointArcINC.BackColor = System.Drawing.Color.Maroon;
            this.userControlDPointArcINC.Location = new System.Drawing.Point(58, 122);
            this.userControlDPointArcINC.Margin = new System.Windows.Forms.Padding(2);
            this.userControlDPointArcINC.Name = "userControlDPointArcINC";
            this.userControlDPointArcINC.Size = new System.Drawing.Size(90, 90);
            this.userControlDPointArcINC.TabIndex = 8;
            this.userControlDPointArcINC.Visible = false;
            // 
            // userControlDPointINC
            // 
            this.userControlDPointINC.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.userControlDPointINC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlDPointINC.Location = new System.Drawing.Point(95, 48);
            this.userControlDPointINC.MaximumSize = new System.Drawing.Size(111, 69);
            this.userControlDPointINC.MinimumSize = new System.Drawing.Size(66, 69);
            this.userControlDPointINC.Name = "userControlDPointINC";
            this.userControlDPointINC.Size = new System.Drawing.Size(111, 69);
            this.userControlDPointINC.TabIndex = 7;
            // 
            // userControlDPointAZM
            // 
            this.userControlDPointAZM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.userControlDPointAZM.Location = new System.Drawing.Point(620, 48);
            this.userControlDPointAZM.MaximumSize = new System.Drawing.Size(111, 69);
            this.userControlDPointAZM.MinimumSize = new System.Drawing.Size(66, 69);
            this.userControlDPointAZM.Name = "userControlDPointAZM";
            this.userControlDPointAZM.Size = new System.Drawing.Size(111, 69);
            this.userControlDPointAZM.TabIndex = 6;
            // 
            // FormToolFace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(826, 582);
            this.Controls.Add(this.userControlDPointArcAZM);
            this.Controls.Add(this.userControlDPointArcINC);
            this.Controls.Add(this.userControlDPointINC);
            this.Controls.Add(this.userControlDPointAZM);
            this.Controls.Add(this.labelRight);
            this.Controls.Add(this.labelLeft);
            this.Controls.Add(this.labelUp);
            this.Controls.Add(this.labelDown);
            this.Controls.Add(this.pictureBoxToolFace);
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormToolFace";
            this.Text = "Tool Face";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormToolFace_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormToolFace_FormClosed);
            this.Load += new System.EventHandler(this.FormToolFace_Load);
            this.Resize += new System.EventHandler(this.FormToolFace_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToolFace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelUp;
        private System.Windows.Forms.Label labelDown;
        private System.Windows.Forms.Label labelLeft;
        private System.Windows.Forms.Label labelRight;
        private System.Windows.Forms.PictureBox pictureBoxToolFace;
        private UserControlDPoint userControlDPointAZM;
        private UserControlDPoint userControlDPointINC;
        private UserControlDPointArc userControlDPointArcINC;
        private UserControlDPointArc userControlDPointArcAZM;
    }
}