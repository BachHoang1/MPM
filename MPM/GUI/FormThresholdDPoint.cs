using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.GUI
{
    public partial class FormThresholdDPoint : Form
    {

        private float m_fThresholdLow;
        private float m_fThresholdHigh;
        public FormThresholdDPoint()
        {
            InitializeComponent();
        }

        public void SetThresholds(string sLow_, string sHigh_)
        {
            float fLow = 0.0f;
            float fHigh = 0.0f;
            bool b = float.TryParse(sLow_, out fLow);
            if (b)
                m_fThresholdLow = fLow;

            b = float.TryParse(sHigh_, out fHigh);
            if (b)
                m_fThresholdHigh = fHigh;
        }

        public void GetThresholds(out float fLow_, out float fHigh_)
        {
            fLow_ = m_fThresholdLow;
            fHigh_ = m_fThresholdHigh;
        }

        private bool IsValid()
        {
            bool bRetVal = true;
            float fLow, fHigh;
            bool b = float.TryParse(textBoxHighThreshold.Text, out fHigh);
            if (!b)
            {
                MessageBox.Show("High Threshold value must be numeric.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            b = float.TryParse(textBoxLowThreshold.Text, out fLow);
            if (!b)
            {
                MessageBox.Show("Low Threshold value must be numeric.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (fLow > fHigh)
            {
                MessageBox.Show("Low Threshold value must be less than High Threshold.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return bRetVal;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!IsValid())
                return;

            m_fThresholdHigh = Convert.ToSingle(textBoxHighThreshold.Text);
            m_fThresholdLow = Convert.ToSingle(textBoxLowThreshold.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormThresholdDPoint_Load(object sender, EventArgs e)
        {
            textBoxHighThreshold.Text = m_fThresholdHigh.ToString();
            textBoxLowThreshold.Text = m_fThresholdLow.ToString();
        }
    }
}
