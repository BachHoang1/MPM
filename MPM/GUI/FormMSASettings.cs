// author: hoan chau
// purpose: display current connection parameters to the multi-station analysis server

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;

namespace MPM.GUI
{
    public partial class FormMSASettings : Form
    {
        private string m_sURL;
        private string m_sAPIKey;
        private bool m_bUseMSA;

        private string m_sJobID;
        private string m_sRig;
        private string m_sBHA;
        private string m_sUseAcceptRejectFeature;

        CJobInfo m_JobInfo;

        private bool m_bNewJob;  // indicates new job

        private CWidgetInfoLookupTable m_WidgetInfoLookup;

        CJobInfo.SURVEY_MANAGEMENT_MODE m_SvyMgmtMode;

        private bool m_bIsServer;

        public FormMSASettings()
        {
            InitializeComponent();
            m_bNewJob = false;
            m_bIsServer = true;
        }

        public void SetWidgetInfoLookup(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            m_WidgetInfoLookup = widgetInfoLookup_;
        }

        public void SetServerMode(bool bVal_)
        {
            m_bIsServer = bVal_;
        }

        public void SetJob(bool bNewJob_, CJobInfo jobInfo_)
        {
            m_bNewJob = bNewJob_;
            m_JobInfo = jobInfo_;            
        }

        private bool IsValid()
        {
            bool bRetVal = true;

            uint iVal;
            if (!uint.TryParse(textBoxBHA.Text, out iVal))  // don't save
            {
                MessageBox.Show("BHA must be a non-negative number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bRetVal = false;
            }
            else if (radioButtonSurveyMSAMode.Checked || radioButtonSurveyMSAMode.Checked)
            {
                if (m_sJobID != textBoxJobID.Text &&
                     m_sAPIKey == textBoxAPIKey.Text)
                {
                    MessageBox.Show("You've changed the Job ID but not the MSA API Key.\nEither disable the 'Use MSA' checkbox or enter a new API Key.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bRetVal = false;
                }
                else if (m_sJobID == textBoxJobID.Text &&
                         m_sAPIKey != textBoxAPIKey.Text &&
                         m_sAPIKey != "")  // don't allow changes from empty api key
                {
                    MessageBox.Show("You've changed the MSA API Key but not the Job ID.\nEither disable the 'Use MSA' checkbox or enter a new Job ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bRetVal = false;
                }
                else if (textBoxAPIKey.Text.Trim() == "")
                {
                    MessageBox.Show("You've checked 'Use MSA' but have not entered an MSA API Key.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bRetVal = false;
                }
            }
            
            return bRetVal;
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!IsValid())            
                return;            

            if (!radioButtonSurveyMSAMode.Checked ) // no msa means no api key
                textBoxAPIKey.Text = "";
            
            m_bUseMSA = radioButtonSurveyMSAMode.Checked;

            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();

            // for the following, don't save if there haven't been any changes
            if (m_sURL != textBoxURL.Text)
            {
                m_sURL = textBoxURL.Text;
                m_WidgetInfoLookup.SetValue(this.Name, textBoxURL.Name, m_sURL);
            }

            if (m_sUseAcceptRejectFeature.ToLower() != checkBoxUseAcceptRejectFeature.Checked.ToString().ToLower())
            {
                m_sUseAcceptRejectFeature = checkBoxUseAcceptRejectFeature.Checked.ToString();
                m_WidgetInfoLookup.SetValue(this.Name, checkBoxUseAcceptRejectFeature.Name, m_sUseAcceptRejectFeature);
            }

            m_WidgetInfoLookup.SetValue(this.Name, radioButtonSurveyFullAutomation.Name, radioButtonSurveyFullAutomation.Checked ? "True" : "False");
            m_WidgetInfoLookup.SetValue(this.Name, radioButtonSurveyConfirmationMode.Name, radioButtonSurveyConfirmationMode.Checked ? "True" : "False");
            m_WidgetInfoLookup.SetValue(this.Name, radioButtonSurveyMSAMode.Name, radioButtonSurveyMSAMode.Checked ? "True" : "False");

            m_WidgetInfoLookup.Save();

            if (radioButtonSurveyMSAMode.Checked)
                m_SvyMgmtMode = CJobInfo.SURVEY_MANAGEMENT_MODE.MSA;
            else if (radioButtonSurveyConfirmationMode.Checked)
                m_SvyMgmtMode = CJobInfo.SURVEY_MANAGEMENT_MODE.CONFIRMATION;
            else
                m_SvyMgmtMode = CJobInfo.SURVEY_MANAGEMENT_MODE.FULL_AUTOMATION;

            m_sAPIKey = textBoxAPIKey.Text;                        
            m_sJobID = textBoxJobID.Text; 
            m_sRig = textBoxRig.Text;
            m_sBHA = textBoxBHA.Text;
                                                                               
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public bool IsMSA()
        {
            return m_bUseMSA;
        }

        private void FormMSASettings_Load(object sender, EventArgs e)
        {
            if (m_bNewJob)
            {
                radioButtonSurveyFullAutomation.Checked = true;                

                // no msa means user can use or not use accept/reject feature
                checkBoxUseAcceptRejectFeature.Checked = true;
                checkBoxUseAcceptRejectFeature.Enabled = true;

                if (radioButtonSurveyMSAMode.Checked && tabControl1.TabCount < 2)
                    tabControl1.TabPages.Add(tabPageMSAProvider);
                else if (!radioButtonSurveyMSAMode.Checked && tabControl1.TabCount > 1)
                    tabControl1.TabPages.Remove(tabPageMSAProvider);

                //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
                //WidgetInfoLookupTbl.Load();
                m_sURL = m_WidgetInfoLookup.GetValue(this.Name, textBoxURL.Name);
                textBoxURL.Text = m_sURL;

                textBoxAPIKey.Text = "";
                textBoxAPIKey.Enabled = true;

                textBoxJobID.Text = "";
                textBoxJobID.Enabled = true;

                textBoxRig.Text = "";
                textBoxBHA.Text = "";
                m_sUseAcceptRejectFeature = "false";
            }
            else
            {
                //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
                //WidgetInfoLookupTbl.Load();
                m_sURL = m_WidgetInfoLookup.GetValue(this.Name, textBoxURL.Name);                

                m_sAPIKey = m_JobInfo.GetAPIKey();
                if (m_JobInfo.GetMSA() == 1)
                    radioButtonSurveyMSAMode.Checked = m_bUseMSA = true;
                else
                    radioButtonSurveyMSAMode.Checked = m_bUseMSA = false;

                // allow them to use MSA after job has been created
                if (radioButtonSurveyMSAMode.Checked)  // once it's turned "on", it stays "on"
                {
                    //groupBoxSurveyManagement.Enabled = false;
                    radioButtonSurveyFullAutomation.AutoCheck = false;
                    radioButtonSurveyConfirmationMode.AutoCheck = false;
                    radioButtonSurveyMSAMode.AutoCheck = false;
                }                    
                else  // allow them to turn it "on"
                    radioButtonSurveyMSAMode.Enabled = true;

                if (radioButtonSurveyMSAMode.Checked && tabControl1.TabCount < 2)
                    tabControl1.TabPages.Add(tabPageMSAProvider);
                else if (!radioButtonSurveyMSAMode.Checked && tabControl1.TabCount > 1)
                    tabControl1.TabPages.Remove(tabPageMSAProvider);
                
                textBoxURL.Text = m_sURL;
                textBoxAPIKey.Text = m_sAPIKey;

                // allow them to use MSA after job has been created
                if (radioButtonSurveyMSAMode.Checked)  // once it's turned "on", the key can't be changed
                    textBoxAPIKey.Enabled = false;
                else  // allow them to change the "key"
                    textBoxAPIKey.Enabled = true;

                // get job information
                m_sJobID = m_JobInfo.GetJobID();
                textBoxJobID.Text = m_sJobID;
                if (m_bUseMSA)  // MSA is related to Job ID so it can't change unless a new job is created
                    textBoxJobID.Enabled = false;
                else
                    textBoxJobID.Enabled = true;

                m_sRig = m_JobInfo.GetRig();
                textBoxRig.Text = m_sRig;

                m_sBHA = m_JobInfo.GetBHA().ToString();
                textBoxBHA.Text = m_sBHA;

                if (!m_bUseMSA)  // then it's probably going to default to full automation or confirmation mode
                {
                    string sSurveyFullAutomation = m_WidgetInfoLookup.GetValue(this.Name, radioButtonSurveyFullAutomation.Name);
                    string sSurveyConfirmationMode = m_WidgetInfoLookup.GetValue(this.Name, radioButtonSurveyConfirmationMode.Name);
                    if (sSurveyConfirmationMode.ToLower() == "false" && sSurveyFullAutomation.ToLower() == "false")
                    {
                        m_SvyMgmtMode = CJobInfo.SURVEY_MANAGEMENT_MODE.CONFIRMATION;
                        radioButtonSurveyConfirmationMode.Checked = true;
                    }                        
                    else if (sSurveyFullAutomation.ToLower() == "true")
                    {
                        m_SvyMgmtMode = CJobInfo.SURVEY_MANAGEMENT_MODE.FULL_AUTOMATION;
                        radioButtonSurveyFullAutomation.Checked = true;
                    }                            
                    else
                    {
                        m_SvyMgmtMode = CJobInfo.SURVEY_MANAGEMENT_MODE.CONFIRMATION;
                        radioButtonSurveyConfirmationMode.Checked = true;
                    }
                        
                }
                else
                    m_SvyMgmtMode = CJobInfo.SURVEY_MANAGEMENT_MODE.MSA;


                m_sUseAcceptRejectFeature = m_WidgetInfoLookup.GetValue(this.Name, checkBoxUseAcceptRejectFeature.Name);
                if (m_sUseAcceptRejectFeature.ToLower() == "false")
                    checkBoxUseAcceptRejectFeature.Checked = false;
                else
                    checkBoxUseAcceptRejectFeature.Checked = true;
            }
            
        }

        public void GetSettings(out string sURL_, out string sAPIKey_, out bool bUseAcceptRejectFeature_)
        {
            sURL_ = textBoxURL.Text;
            sAPIKey_ = textBoxAPIKey.Text;
            bUseAcceptRejectFeature_ = checkBoxUseAcceptRejectFeature.Checked;
        }        

        public void GetJobInfo(out string sJobID_, out string sRig_, out string sBHA_, out int iMSA_, out CJobInfo.SURVEY_MANAGEMENT_MODE iSvyMngmtMode_)
        {
            sJobID_ = m_sJobID;
            sRig_ = m_sRig;
            sBHA_ = m_sBHA;
            iMSA_ = m_bUseMSA ? 1 : 0;
            iSvyMngmtMode_ = m_SvyMgmtMode;
        }

        private void SetAcceptRejectOption()
        {
            if (radioButtonSurveyFullAutomation.Checked)  // accept/reject is not applicable
            {                
                checkBoxUseAcceptRejectFeature.AutoCheck = false;  // not applicable so turn it off
                checkBoxUseAcceptRejectFeature.Checked = false;                
            }
            else if (radioButtonSurveyConfirmationMode.Checked)  
            {
                checkBoxUseAcceptRejectFeature.AutoCheck = false;  // must use accept/reject 
                checkBoxUseAcceptRejectFeature.Checked = true;                                 
            }
            else // must be (radioButtonSurveyMSAMode.Checked)  // accept/reject is an option for client
            {
                if (m_bIsServer)  // server must have it checked
                    checkBoxUseAcceptRejectFeature.AutoCheck = false;
                else  // client can work around it
                    checkBoxUseAcceptRejectFeature.AutoCheck = true;
                checkBoxUseAcceptRejectFeature.Checked = true;                
            }
            m_sUseAcceptRejectFeature = checkBoxUseAcceptRejectFeature.Checked.ToString().ToLower();
        }

        private void radioButtonSurveyMSAMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSurveyMSAMode.Checked && tabControl1.TabCount < 2)
                tabControl1.TabPages.Add(tabPageMSAProvider);
            else if (!radioButtonSurveyMSAMode.Checked && tabControl1.TabCount > 1)
                tabControl1.TabPages.Remove(tabPageMSAProvider);

            SetAcceptRejectOption();
        }

        private void radioButtonSurveyFullAutomation_CheckedChanged(object sender, EventArgs e)
        {
            SetAcceptRejectOption();
        }

        private void radioButtonSurveyConfirmationMode_CheckedChanged(object sender, EventArgs e)
        {
            SetAcceptRejectOption();
        }
    }
}
