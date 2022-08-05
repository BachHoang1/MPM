namespace MPM.GUI
{
    partial class UserControlTemperature
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
                if (m_penForThreeSidedRect != null)
                    m_penForThreeSidedRect.Dispose();
                if (m_penForBulb != null)
                    m_penForBulb.Dispose();
                if (m_penForMercury != null)
                    m_penForMercury.Dispose();
                if (m_penMercuryTick != null)
                    m_penMercuryTick.Dispose();
                if (m_brushBackgroundBulb != null)
                    m_brushBackgroundBulb.Dispose();
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
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
    }
}
