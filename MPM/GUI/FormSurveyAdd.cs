using MPM.Data;
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
    public partial class FormSurveyAdd : Form
    {
        private float m_fLastSurveyDepth;
        private CSurvey.REC m_SvyRec;

        public FormSurveyAdd()
        {
            InitializeComponent();
        }

        private bool IsValid()
        {
            bool bRetVal = true;

            TextBox[] arrTextBox = new TextBox[6] {textBoxSurveyDepth, textBoxInc, textBoxAzm,
                                                    textBoxMTot, textBoxGTot, textBoxDipA };
            // check that all values are numeric
            // check that the survey depth is greater than the last depth
            for (int i = 0; i < arrTextBox.Length; i++)
            {
                if (!IsNumeric(arrTextBox[i].Text))
                {
                    string sTextBoxName = arrTextBox[i].Name;
                    sTextBoxName = sTextBoxName.Substring(7);  // remove the "textBox" and leave the rest of the widget name
                    MessageBox.Show("The value is not numeric for " + sTextBoxName + ". Try again.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bRetVal = false;
                    break;
                }                    
            }

            if (bRetVal)  // passed numeric check.  try the last depth
            {
                if (m_fLastSurveyDepth >= System.Convert.ToSingle(textBoxSurveyDepth.Text))
                {
                    MessageBox.Show("The survey depth must be greater than the previous value of " + m_fLastSurveyDepth.ToString() + ".", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bRetVal = false;
                }
            }
            
            return bRetVal;
        }

        private bool IsNumeric(string sVal_)
        {
            bool bRetVal = true;
            float fRes = 0.0f;
            bool b = float.TryParse(sVal_, out fRes);
            if (!b)
                bRetVal = false;

            return bRetVal;
        }

        public void SetLastSurveyDepth(float fVal_)
        {
            m_fLastSurveyDepth = fVal_;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                m_SvyRec.dtCreated = DateTime.Now;
                m_SvyRec.fSurveyDepth = System.Convert.ToSingle(textBoxSurveyDepth.Text);
                m_SvyRec.fInclination = System.Convert.ToSingle(textBoxInc.Text);
                m_SvyRec.fAzimuth = System.Convert.ToSingle(textBoxAzm.Text);
                m_SvyRec.fMTotal = System.Convert.ToSingle(textBoxMTot.Text);
                m_SvyRec.fGTotal = System.Convert.ToSingle(textBoxGTot.Text);
                m_SvyRec.fDipAngle = System.Convert.ToSingle(textBoxDipA.Text);
                m_SvyRec.Status = CSurvey.STATUS.ACCEPT;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public CSurvey.REC GetRec()
        {
            return m_SvyRec;
        }

        private void FormSurveyAdd_Load(object sender, EventArgs e)
        {
            textBoxSurveyDepth.Text = "0";
            textBoxInc.Text = "0";
            textBoxAzm.Text = "0";
            textBoxMTot.Text = "0";
            textBoxGTot.Text = "0";
            textBoxDipA.Text = "0";
        }
    }
}
