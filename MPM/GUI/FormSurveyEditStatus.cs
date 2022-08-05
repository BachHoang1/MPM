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
    public partial class FormSurveyEditStatus : Form
    {
        CSurvey.STATUS m_status;

        public FormSurveyEditStatus()
        {
            InitializeComponent();
        }

        public void Init(string sVal_)
        {
            if (sVal_ == CSurvey.STATUS.ACCEPT.ToString())
                m_status = CSurvey.STATUS.ACCEPT;
            else if (sVal_ == CSurvey.STATUS.REJECT.ToString())
                m_status = CSurvey.STATUS.REJECT;
            else
                m_status = CSurvey.STATUS.NONE;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonAccept.Checked)
                m_status = CSurvey.STATUS.ACCEPT;
            else if (radioButtonReject.Checked)
                m_status = CSurvey.STATUS.REJECT;
            else
                m_status = CSurvey.STATUS.NONE;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormSurveyEditStatus_Load(object sender, EventArgs e)
        {
            if (m_status == CSurvey.STATUS.ACCEPT)
                radioButtonAccept.Checked = true;
            else if (m_status == CSurvey.STATUS.REJECT)
                radioButtonReject.Checked = true;
            else
                radioButtonNone.Checked = true;
        }

        public string GetStatus()
        {
            return m_status.ToString();
        }
    }
}
