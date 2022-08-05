// author: hoan chau
// purpose: encapsulate job information for the currently loaded database

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.Data
{
    public class CJobInfo
    {        
        const int MSA_FLAG_ADDED_VERSION = 2;

        public enum SURVEY_MANAGEMENT_MODE { FULL_AUTOMATION, CONFIRMATION, MSA };

        private DbConnection m_dbCnn;

        private string m_sJob;
        private string m_sRig;
        private int m_iBHA;
        private int m_iMSA;
        private int m_iTableVersion;
        private string m_sAPIKey;

        private SURVEY_MANAGEMENT_MODE m_SurveyManagementMode;
        private CWidgetInfoLookupTable m_WidgetInfoLookup;

        public CJobInfo(ref DbConnection dbCnn_, ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            m_iMSA = 0;
            m_sAPIKey = "";
            m_dbCnn = dbCnn_;
            m_WidgetInfoLookup = widgetInfoLookup_;
            m_iTableVersion = GetTblVersion();
        }

        public void Load()
        {
            string sQuery = String.Format("SELECT job, rig, bha, msa, apiKey FROM tblJobInfo");
            if (m_iTableVersion < MSA_FLAG_ADDED_VERSION)
                sQuery = String.Format("SELECT job, rig, bha FROM tblJobInfo");

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                m_sJob = reader[0].ToString();
                m_sRig = reader[1].ToString();
                m_iBHA = System.Convert.ToInt32(reader[2]);
                if (m_iTableVersion >= MSA_FLAG_ADDED_VERSION)
                {
                    m_iMSA = System.Convert.ToInt32(reader[3]);
                    m_sAPIKey = reader[4].ToString();
                }                    
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            // read from the form settings
            string sSurveyFullAutomation = m_WidgetInfoLookup.GetValue("FormMSASettings", "radioButtonSurveyFullAutomation");
            string sSurveyConfirmationMode = m_WidgetInfoLookup.GetValue("FormMSASettings", "radioButtonSurveyConfirmationMode");
            string sSurveyMSAMode = m_WidgetInfoLookup.GetValue("FormMSASettings", "radioButtonSurveyMSAMode");
            m_SurveyManagementMode = SURVEY_MANAGEMENT_MODE.CONFIRMATION;  // default
            if (sSurveyFullAutomation.ToLower() == "true")
                m_SurveyManagementMode = SURVEY_MANAGEMENT_MODE.FULL_AUTOMATION;
            else if (sSurveyConfirmationMode.ToLower() == "true")
                m_SurveyManagementMode = SURVEY_MANAGEMENT_MODE.CONFIRMATION;
            else if (sSurveyMSAMode.ToLower() == "true")
                m_SurveyManagementMode = SURVEY_MANAGEMENT_MODE.MSA;
        }

        public int GetTblVersion()
        {
            int iRetVal = 1;  // should be at least 1
            string sQuery = String.Format("SELECT version FROM tblVersions WHERE tblName = 'tblJob'");
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                iRetVal = System.Convert.ToInt32(reader[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return iRetVal;
        }

        public bool Create(string sJob_, string sRig_, int iBHA_, int iMSA_, string sAPIKey_)
        {
            bool bRetVal = false;

            DateTime dtCreated = DateTime.Now;
            string sQuery = string.Format("UPDATE tblJobInfo SET created = '{0}', job = '{1}', rig = '{2}', bha = {3}, msa = {4}, apiKey = '{5}'",
                                           dtCreated, sJob_, sRig_, iBHA_, iMSA_, sAPIKey_);
            
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                int iRowsAffected = command.ExecuteNonQuery();

                //Console.WriteLine("Updated {0} rows.", iRowsAffected);
                bRetVal = iRowsAffected == 1 ? true : false;

                m_sJob = sJob_;
                m_sRig = sRig_;
                m_iBHA = iBHA_;                
                m_iMSA = iMSA_;
                m_sAPIKey = sAPIKey_;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating job information: " + ex.Message + ".", "Update Job Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRetVal;
        }

        public bool Save()
        {
            return Update(m_sJob, m_sRig, m_iBHA, m_iMSA, m_sAPIKey);            
        }

        public bool SaveSession()
        {
            bool bRetVal = false;
            try
            {

                if (m_SurveyManagementMode == SURVEY_MANAGEMENT_MODE.FULL_AUTOMATION)
                    m_WidgetInfoLookup.SetValue("FormMSASettings", "radioButtonSurveyFullAutomation", "True");
                else
                    m_WidgetInfoLookup.SetValue("FormMSASettings", "radioButtonSurveyFullAutomation", "False");

                if (m_SurveyManagementMode == SURVEY_MANAGEMENT_MODE.CONFIRMATION)
                    m_WidgetInfoLookup.SetValue("FormMSASettings", "radioButtonSurveyConfirmationMode", "True");
                else
                    m_WidgetInfoLookup.SetValue("FormMSASettings", "radioButtonSurveyConfirmationMode", "False");

                if (m_SurveyManagementMode == SURVEY_MANAGEMENT_MODE.MSA)
                    m_WidgetInfoLookup.GetValue("FormMSASettings", "radioButtonSurveyMSAMode", "True");
                else
                    m_WidgetInfoLookup.GetValue("FormMSASettings", "radioButtonSurveyMSAMode", "False");

                m_WidgetInfoLookup.Save();

                bRetVal = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in CJobInfo::SaveSession: " + ex.Message, "Error Saving to Session", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }

            return bRetVal;
        }

        public bool Update(string sJob_, string sRig_, int iBHA_, int iMSA_, string sAPIKey_)
        {
            bool bRetVal = false;

            DateTime dtCreated = DateTime.Now;            
            string sQuery = string.Format("UPDATE tblJobInfo SET job = '{0}', rig = '{1}', bha = {2}, msa = {3}, apiKey = '{4}'",
                                    sJob_, sRig_, iBHA_, iMSA_, sAPIKey_);

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                int iRowsAffected = command.ExecuteNonQuery();

                //Console.WriteLine("Updated {0} rows.", iRowsAffected);
                bRetVal = iRowsAffected == 1 ? true : false;

                m_sJob = sJob_;
                m_sRig = sRig_;
                m_iBHA = iBHA_;                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating job information: " + ex.Message + ".", "Update Job Information", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        
            return bRetVal;
        }

        public string GetJobID()
        {
            return m_sJob;
        }

        public void SetJobID(string sVal_)
        {
            m_sJob = sVal_;
        }

        public string GetRig()
        {
            return m_sRig;
        }

        public void SetRig(string sVal_)
        {
            m_sRig = sVal_;
        }

        public int GetBHA()
        {
            return m_iBHA;
        }

        public void SetBHA(int iVal_)
        {
            m_iBHA = iVal_;
        }

        public int GetMSA()
        {
            return m_iMSA;
        }

        public string GetAPIKey()
        {
            return m_sAPIKey;
        }

        public void SetAPIKey(string sVal_)
        {
            m_sAPIKey = sVal_;
        }

        public void SetSurveyManagmentMode(SURVEY_MANAGEMENT_MODE mode_)
        {
            m_SurveyManagementMode = mode_;
        }

        public SURVEY_MANAGEMENT_MODE GetSurveyManagementMode()
        {
            return m_SurveyManagementMode;
        }
    }
}
