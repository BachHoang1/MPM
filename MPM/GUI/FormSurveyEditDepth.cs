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
    public partial class FormSurveyEditDepth : Form
    {
        float m_fBitDepth;
        float m_fDirToBit;
        float m_fSurveyDepth;
        string m_sUnit;

        public FormSurveyEditDepth()
        {
            InitializeComponent();
        }

        public void SetDepthParameters(float fBitDepth_, float fDirToBit_, float fSurveyDepth_)
        {
            m_fBitDepth    = fBitDepth_;
            m_fDirToBit    = fDirToBit_;
            m_fSurveyDepth = fSurveyDepth_;
        }

        public void SetUnit(string sVal_)
        {
            m_sUnit = sVal_;
        }

        private bool IsValid()
        {
            bool bRetVal = false;

            float f;
            if (float.TryParse(textBoxSurveyDepth.Text, out f))
            {
                if (f < 0.0f)
                {
                    MessageBox.Show("Survey Depth cannot be negative. Please try again.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxSurveyDepth.Focus();
                }
                else
                {
                    m_fSurveyDepth = f;
                    m_fBitDepth = m_fSurveyDepth + m_fDirToBit;
                    textBoxBitDepth.Text = m_fBitDepth.ToString();
                    bRetVal = true;
                }
            }
            else
            {
                MessageBox.Show("Invalid character entered for Survey Depth. Please try again.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxSurveyDepth.Focus();
            }

            return bRetVal;
        }

        public float GetSurveyDepth()
        {
            return m_fSurveyDepth;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FormSurveyEditDepth_Load(object sender, EventArgs e)
        {
            textBoxSurveyDepth.Text = m_fSurveyDepth.ToString();
            textBoxBitDepth.Text = m_fBitDepth.ToString();
            textBoxOffset.Text = m_fDirToBit.ToString();
            labelUnit.Text = m_sUnit;
        }

        private void textBoxSurveyDepth_TextChanged(object sender, EventArgs e)
        {
            float f;
            if (float.TryParse(textBoxSurveyDepth.Text, out f))
            {
                if (f < 0.0f)
                {
                    //MessageBox.Show("Survey Depth cannot be negative. Please try again.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxSurveyDepth.Focus();
                }
                else
                {
                    m_fSurveyDepth = f;
                    m_fBitDepth = m_fSurveyDepth + m_fDirToBit;
                    textBoxBitDepth.Text = m_fBitDepth.ToString();                    
                }
            }
        }
    }
}
