namespace MPM.GUI
{
    partial class UserControlDPoint
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
                if (m_stringFormat != null)
                    m_stringFormat.Dispose();
                if (m_stringFormatHorizontal != null)
                    m_stringFormatHorizontal.Dispose();
                if (m_fontDesc != null)
                    m_fontDesc.Dispose();
                if (m_fontVal != null)
                    m_fontVal.Dispose();
                if (m_fontDate != null)
                    m_fontDate.Dispose();

                if (m_fontSmallDesc != null)
                    m_fontSmallDesc.Dispose();
                if (m_fontSmallVal != null)
                    m_fontSmallVal.Dispose();
                if (m_fontSmallDate != null)
                    m_fontSmallDate.Dispose();
                if (m_fontTinyVal != null)
                    m_fontTinyVal.Dispose();

                // bad decodes use strikethrough
                if (m_fontValStrikeThrough != null)
                    m_fontValStrikeThrough.Dispose();
                if (m_fontSmallValStrikeThrough != null)
                    m_fontSmallValStrikeThrough.Dispose();
                if (m_fontTinyValStrikeThrough != null)
                    m_fontTinyValStrikeThrough.Dispose();
                if (m_brush != null)
                    m_brush.Dispose();
                if (m_brushGood != null)
                    m_brushGood.Dispose();

                // alert colors
                if (m_brushAlertRed != null)
                    m_brushAlertRed.Dispose();
                if (m_brushAlertOrange != null)
                    m_brushAlertOrange.Dispose();
                if (m_brushAlertYellow != null)
                    m_brushAlertYellow.Dispose();
                if (m_brushAlertGreen != null)
                    m_brushAlertGreen.Dispose();

                // threshold colors
                if (m_brushLowThreshold != null)
                    m_brushLowThreshold.Dispose();

                if (m_brushHighThreshold != null)
                    m_brushHighThreshold.Dispose();
            }            
                        
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxInfo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxInfo
            // 
            this.pictureBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxInfo.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxInfo.Name = "pictureBoxInfo";
            this.pictureBoxInfo.Size = new System.Drawing.Size(171, 149);
            this.pictureBoxInfo.TabIndex = 0;
            this.pictureBoxInfo.TabStop = false;
            this.pictureBoxInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxInfo_Paint);
            this.pictureBoxInfo.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxInfo_MouseDoubleClick);
            // 
            // UserControlDPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Controls.Add(this.pictureBoxInfo);
            this.Name = "UserControlDPoint";
            this.Size = new System.Drawing.Size(171, 149);
            this.Resize += new System.EventHandler(this.UserControlDPoint_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBoxInfo;
    }
}
