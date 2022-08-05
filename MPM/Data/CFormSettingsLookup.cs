// author: hoan chau
// purpose: interface to the form settings table which is structured 
//          based on form name, widget name, and value; and sometimes
//          a message code

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace MPM.Data
{
    public class CWidgetInfoLookupTable: IDisposable
    {
        private DataSetFormSettings m_DataFormSettings;
        private const string FILE_NAME = "XMLFileFormSettings.xml";
        private const string DEFAULT_SETTINGS_FILE = CCommonTypes.DATA_FOLDER + "XMLFileFormSettingsDefault.xml";  // the file that is installed and used as a reference when local settings are not yet present
      
        
        private string m_sDataPath;

        public CWidgetInfoLookupTable()
        {                        
            Init();                                                    
        }

        public void Init()
        {
            CSettingsLookupTable settings = new CSettingsLookupTable();
            settings.Load();
            m_sDataPath = settings.GetJobPath() + FILE_NAME;

            // if the local file does not yet exist
            if (!File.Exists(m_sDataPath))
            {
                // simply copy the default to the local settings
                File.Copy(DEFAULT_SETTINGS_FILE, m_sDataPath);
            }
            else
            {
                // else
                // read the contents of the local settings
                DataSetFormSettings SettingsLocal = new DataSetFormSettings();
                XmlReadMode xmlrmLocal = SettingsLocal.ReadXml(m_sDataPath, System.Data.XmlReadMode.InferSchema);

                // load the default file
                DataSetFormSettings SettingsDefault = new DataSetFormSettings();
                XmlReadMode xmlrmDefault = SettingsDefault.ReadXml(DEFAULT_SETTINGS_FILE, System.Data.XmlReadMode.InferSchema);

                bool bHasChanges = false;
                DataTable tbl = SettingsDefault.Tables["Form"];
                foreach (DataRow dataDefault in SettingsDefault.Tables[0].Rows)
                {
                    foreach (DataRow dataLocal in SettingsLocal.Tables[0].Rows)
                    {
                        if (dataDefault["FormName"].Equals(dataLocal["FormName"]) &&
                            dataDefault["WidgetName"].Equals(dataLocal["WidgetName"]) &&
                            !dataDefault["Value"].Equals(dataLocal["Value"]))  // overwrite with local settings
                        {
                            string sExpression = "FormName = '" + dataDefault["FormName"] + "' AND " + "WidgetName = '" + dataDefault["WidgetName"] + "'";
                            DataRow[] dr = tbl.Select(sExpression);
                            if (dr.Count() > 0)
                            {
                                dr[0]["MessageCode"] = dataLocal["MessageCode"];
                                dr[0]["Value"] = dataLocal["Value"];
                                if (dataLocal.Table.Columns.Contains("Time"))
                                    dr[0]["Time"] = dataLocal["Time"];

                                if (dataLocal.Table.Columns.Contains("TelemetryType"))
                                    dr[0]["TelemetryType"] = dataLocal["TelemetryType"];

                                if (dataLocal.Table.Columns.Contains("ParityErr"))
                                    dr[0]["ParityErr"] = dataLocal["ParityErr"];


                                bHasChanges = true;
                            }

                            break;
                        }
                    }
                }

                // overwrite the local settings file with the new default settings file with local changes
                if (bHasChanges)
                {
                    SettingsLocal.Clear();
                    SettingsLocal = SettingsDefault;
                    SettingsLocal.WriteXml(m_sDataPath, XmlWriteMode.IgnoreSchema);
                }
            }
        }

        ~CWidgetInfoLookupTable()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  
                if (m_DataFormSettings != null)
                {
                    m_DataFormSettings.Dispose();
                    m_DataFormSettings = null;
                }
            }
            // free native resources if there are any.  
            //if (nativeResource != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(nativeResource);
            //    nativeResource = IntPtr.Zero;
            //}
        }
        public string Load()
        {
            string sTbl = "NA";
            try
            {
                CSettingsLookupTable settings = new CSettingsLookupTable();
                settings.Load();
                m_sDataPath = settings.GetJobPath() + FILE_NAME;

                m_DataFormSettings = new DataSetFormSettings();
                XmlReadMode xmlrm = m_DataFormSettings.ReadXml(m_sDataPath, System.Data.XmlReadMode.InferSchema);
                
                if (m_DataFormSettings.Tables.Count > 0)
                    sTbl = m_DataFormSettings.Tables[0].TableName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Please use the installer!", "Loading Data\\XMLFileFormSettings.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sTbl;
        }

        public int GetMessageCode(string sFormName_, string sWidgetName_)
        {
            int iMessageCode = -1;
            DataRow[] dr;
            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_  + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)                            
                iMessageCode = Convert.ToInt16(dr[0].Field<string>("MessageCode"));                            

            return iMessageCode;
        }

        public string GetValue(string sFormName_, string sWidgetName_)
        {
            string sValue = "No Value";
            DataRow[] dr;
            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                sValue = dr[0].Field<string>("Value");

            return sValue;
        }

        public string GetLastSessionInfo(string sFormName_, string sWidgetName_, out string sTime_, out string sTelemetryType_, out bool bParityErr_)
        {
            string sValue = "";
            sTime_ = "";
            sTelemetryType_ = "";
            bParityErr_ = false;

            DataRow[] dr;
            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                sValue = dr[0].Field<string>("Value");
                sTime_ = dr[0].Field<string>("Time");
                sTelemetryType_ = dr[0].Field<string>("TelemetryType");
                if (dr[0].Field<string>("ParityErr") != "")
                {
                    if (dr[0].Field<string>("ParityErr") == "1")
                        bParityErr_ = true;
                    else
                        bParityErr_ = false;
                }
                
            }
                            
            return sValue;
        }

        public string GetValue(string sFormName_, string sWidgetName_, string sPropertyName_)
        {
            string sValue = "No Value";
            DataRow[] dr;
            string sExpression = "FormName = '" + sFormName_ + "' AND WidgetName = '" + sWidgetName_ + "' AND PropertyName = '" + sPropertyName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                sValue = dr[0].Field<string>("Value");

            return sValue;
        }

        public bool Set(string sFormName_, string sWidgetName_, int iMessageCode_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["MessageCode"] = iMessageCode_.ToString();
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool SetSessionInfo(string sFormName_, string sWidgetName_, int iMessageCode_, string sVal_, string sTime_, string sTelemetryType_, bool bParityErr_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["MessageCode"] = iMessageCode_.ToString();
                dr[0]["Value"] = sVal_;
                dr[0]["Time"] = sTime_;
                dr[0]["TelemetryType"] = sTelemetryType_;
                dr[0]["ParityErr"] = bParityErr_ ? "1" : "0";
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool SetValue(string sFormName_, string sWidgetName_, int iVal_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["Value"] = iVal_.ToString();
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool SetValue(string sFormName_, string sWidgetName_, string sVal_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["Value"] = sVal_;
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool SetValue(string sFormName_, string sWidgetName_, string sPropertyName_, string sVal_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "FormName = '" + sFormName_ + "' AND WidgetName = '" + sWidgetName_ + "' AND PropertyName = '" +  sPropertyName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["Value"] = sVal_;
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        //public string ConstructServerPacket(string sFormName_, string sWidgetName_, string sValue_)
        //{
        //    return "FormName=" + sFormName_ + ";" + "WidgetName=" + sWidgetName_ + ";Value=" + sValue_;            
        //}

        public bool Set(string sFormName_, string sWidgetName_, int iMessageCode_, string sValue_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "FormName = '" + sFormName_ + "' AND " + "WidgetName = '" + sWidgetName_ + "'";
            DataTable tbl = m_DataFormSettings.Tables["Form"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["MessageCode"] = iMessageCode_.ToString();
                dr[0]["Value"] = sValue_;
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool Save()
        {
            bool bRet = false;

            try
            {                
                m_DataFormSettings.WriteXml(m_sDataPath, XmlWriteMode.IgnoreSchema);
                bRet = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Saving Data\\XMLFileFormSettings.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRet;
        }
    }

    
}
