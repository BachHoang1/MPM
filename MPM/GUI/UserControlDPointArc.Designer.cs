namespace MPM.GUI
{
    partial class UserControlDPointArc
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
                if (m_penForCircle != null)
                    m_penForCircle.Dispose();
                if (m_penThinForCircle != null)
                    m_penThinForCircle.Dispose();
                if (m_brushSolidCircle != null)
                    m_brushSolidCircle.Dispose();
                if (m_brushBackgroundCircle != null)
                    m_brushBackgroundCircle.Dispose();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxInfo
            // 
            this.pictureBoxInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.pictureBoxInfo.Size = new System.Drawing.Size(164, 142);
            // 
            // UserControlDPointArc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControlDPointArc";
            this.Size = new System.Drawing.Size(164, 142);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
