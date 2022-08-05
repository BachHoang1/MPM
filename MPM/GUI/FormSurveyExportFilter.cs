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
    public partial class FormSurveyExportFilter : Form
    {
        private bool m_bAll;
        private bool m_bUseNT;  // flag to use nanotesla units
        private float m_fStartSurveyDepth;
        private float m_fEndSurveyDepth;

        private CWidgetInfoLookupTable m_WidgetInfoLookup;

        public FormSurveyExportFilter(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            InitializeComponent();
            m_WidgetInfoLookup = widgetInfoLookup_;
        }

        public void SetDepths(float fStart_, float fEnd_)
        {
            m_fStartSurveyDepth = fStart_;
            m_fEndSurveyDepth = fEnd_;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonAll.Checked)
                m_bAll = true;
            else
                m_bAll = false;

            m_fStartSurveyDepth = (float)numericUpDownStartSurveyDepth.Value;
            m_fEndSurveyDepth = (float)numericUpDownEndSurveyDepth.Value;
            if (m_fStartSurveyDepth > m_fEndSurveyDepth)
            {
                MessageBox.Show("Start Survey Depth must be less than End Survey Depth.", "Depth Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (checkBoxUseNT.Checked)
                m_bUseNT = true;
            else
                m_bUseNT = false;

            Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormSurveyExportFilter_Load(object sender, EventArgs e)
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();
            string sAll = m_WidgetInfoLookup.GetValue(this.Name, radioButtonAll.Name);
            string sStartDepth = m_WidgetInfoLookup.GetValue(this.Name, numericUpDownStartSurveyDepth.Name);
            string sEndDepth = m_WidgetInfoLookup.GetValue(this.Name, numericUpDownEndSurveyDepth.Name);
            string sUseNT = m_WidgetInfoLookup.GetValue(this.Name, checkBoxUseNT.Name);

            if (sAll == "True")
                radioButtonAll.Checked = true;
            else
                radioButtonAccepted.Checked = true;

            numericUpDownStartSurveyDepth.Value = System.Convert.ToDecimal(sStartDepth);
            numericUpDownEndSurveyDepth.Value = System.Convert.ToDecimal(sEndDepth);
            if (sUseNT == "True")
                checkBoxUseNT.Checked = true;
            else
                checkBoxUseNT.Checked = false;

            //numericUpDownStartSurveyDepth.Value = (decimal)m_fStartSurveyDepth;
            //numericUpDownEndSurveyDepth.Value = (decimal)m_fEndSurveyDepth;
        }

        public void GetDepths(out float fStart_, out float fEnd_)
        {
            fStart_ = m_fStartSurveyDepth;
            fEnd_ = m_fEndSurveyDepth;
        }

        public bool IsAll()
        {
            return m_bAll;
        }

        public bool IsNT()
        {
            return m_bUseNT;
        }

        private void Save()
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();
            string sAll = radioButtonAll.Checked ? "True": "False";
            string sStartDepth = numericUpDownStartSurveyDepth.Value.ToString();
            string sEndDepth = numericUpDownEndSurveyDepth.Value.ToString();
            string sUseNT = checkBoxUseNT.Checked ? "True": "False";

            m_WidgetInfoLookup.SetValue(this.Name, radioButtonAll.Name, sAll);
            m_WidgetInfoLookup.SetValue(this.Name, numericUpDownStartSurveyDepth.Name, sStartDepth);
            m_WidgetInfoLookup.SetValue(this.Name, numericUpDownEndSurveyDepth.Name, sEndDepth);
            m_WidgetInfoLookup.SetValue(this.Name, checkBoxUseNT.Name, sUseNT);

            m_WidgetInfoLookup.Save();
        }
    }
}
