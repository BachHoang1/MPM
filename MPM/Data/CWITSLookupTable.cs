// author: hoan chau
// purpose: to look for WITS ID's given the APS message code

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
    public class CWITSLookupTable : IDisposable
    {
        private const string FILE_NAME = "XMLFileWITS.xml";
        private const string DEFAULT_SETTINGS_FILE = CCommonTypes.DATA_FOLDER + "XMLFileWITSDefault.xml";
        public struct WITSChannel
        {
            public int iAPSMessageCode;
            public string sID;
            public string sName;
            public string sUnits;  // of measurement     
            public string sMath;
            public string sSendIfError;
            public string sOutlinerMin;
            public string sOutlinerMax;
        }
        private DataSetWITS m_DataProtocol;
        private string m_sLocalSettingsFile;

        private string m_sDataPath;

        public CWITSLookupTable()
        {
            Init();                    
        }

        ~CWITSLookupTable()
        {
            Dispose(); 
        }

        public void Init()
        {
            CSettingsLookupTable settings = new CSettingsLookupTable();
            settings.Load();
            m_sDataPath = settings.GetJobPath() + FILE_NAME;

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
                DataTable tbl = SettingsDefault.Tables[0];
                foreach (DataRow dataDefault in SettingsDefault.Tables[0].Rows)
                {
                    foreach (DataRow dataLocal in SettingsLocal.Tables[0].Rows)
                    {
                        bool bChanged = false;
                        if (dataDefault.ItemArray.Count() == dataLocal.ItemArray.Count())
                        {
                            if (dataDefault["APSMessageCode"].Equals(dataLocal["APSMessageCode"]) &&
                                dataDefault["Name"].Equals(dataLocal["Name"]) &&
                               (!dataDefault["ID"].Equals(dataLocal["ID"]) ||
                                !dataDefault["Math"].Equals(dataLocal["Math"]) ||
                                !dataDefault["SendIfError"].Equals(dataLocal["SendIfError"]) ||
                                !dataDefault["OutlinerMin"].Equals(dataLocal["OutlinerMin"]) ||
                                !dataDefault["OutlinerMax"].Equals(dataLocal["OutlinerMax"])))  // overwrite with local settings
                            {
                                bChanged = true;
                            }
                            else if (!dataDefault["APSMessageCode"].Equals(dataLocal["APSMessageCode"]) &&
                                dataDefault["Name"].Equals(dataLocal["Name"]) &&
                                dataDefault["Source"].Equals(dataLocal["Source"]) &&
                                !dataDefault["Source"].Equals("Rig"))  // then use the default
                            {
                                bChanged = true;
                            }
                        }
                        else  // probably an upgrade from an earlier version that does not have the "math", "sendiferror", "outlinermin", and "outlinermax" columns
                        {
                            if (dataDefault["APSMessageCode"].Equals(dataLocal["APSMessageCode"]) &&
                                dataDefault["Name"].Equals(dataLocal["Name"]) &&
                               !dataDefault["ID"].Equals(dataLocal["ID"]))  // overwrite with local settings
                            {
                                bChanged = true;
                            }
                        }

                        if (bChanged)
                        {
                            string sExpression = "APSMessageCode = '" + dataDefault["APSMessageCode"] + "' AND " + "Name = '" + dataDefault["Name"] + "'";
                            DataRow[] dr = tbl.Select(sExpression);
                            if (dr.Count() > 0)
                            {
                                dr[0]["ID"] = dataLocal["ID"];
                                dr[0]["NativeUnit"] = dataLocal["NativeUnit"];
                                try
                                {
                                    dr[0]["Math"] = dataLocal["Math"];
                                    dr[0]["SendIfError"] = dataLocal["SendIfError"];
                                    dr[0]["OutlinerMin"] = dataLocal["OutlinerMin"];
                                    dr[0]["OutlinerMax"] = dataLocal["OutlinerMax"];
                                }
                                catch  // older versions don't have math, sendiferror, etc.
                                {
                                    // so there's nothing to copy
                                }
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
                if (m_DataProtocol != null)
                {
                    m_DataProtocol.Dispose();
                    m_DataProtocol = null;
                }
            }
            // free native resources if there are any.  
            //if (nativeResource != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(nativeResource);
            //    nativeResource = IntPtr.Zero;
            //}
        }

        public string Load(bool bUseDefault_ = false)
        {
            string sTbl = "NA";
            try
            {
                CSettingsLookupTable settings = new CSettingsLookupTable();
                settings.Load();
                m_sDataPath = settings.GetJobPath() + FILE_NAME;

                m_DataProtocol = new DataSetWITS();
                m_sLocalSettingsFile = bUseDefault_ ? DEFAULT_SETTINGS_FILE : m_sDataPath;
                XmlReadMode xmlrm = m_DataProtocol.ReadXml(m_sLocalSettingsFile, System.Data.XmlReadMode.InferSchema);
                if (m_DataProtocol.Tables.Count > 0)
                    sTbl = m_DataProtocol.Tables[0].TableName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Please use the installer!", "Loading Data\\XMLFileWITS.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return sTbl;
        }

        private WITSChannel Set(DataRow[] dr_)
        {
            WITSChannel dpiRet = new WITSChannel();
            dpiRet.iAPSMessageCode = Convert.ToInt16(dr_[0].Field<string>("APSMessageCode"));
            dpiRet.sID = dr_[0].Field<string>("ID");
            dpiRet.sName = dr_[0].Field<string>("Name");
            dpiRet.sUnits = dr_[0].Field<string>("NativeUnit");
            dpiRet.sMath = dr_[0].Field<string>("Math");
            dpiRet.sSendIfError = dr_[0].Field<string>("SendIfError");
            dpiRet.sOutlinerMin = dr_[0].Field<string>("OutlinerMin");
            dpiRet.sOutlinerMax = dr_[0].Field<string>("OutlinerMax");
            return dpiRet;
        }

        public WITSChannel Find(int iAPSMessageCode_)
        {
            WITSChannel wcRet = new WITSChannel();
            DataRow[] dr;
            string sExpression = "APSMessageCode = '" + iAPSMessageCode_.ToString() + "'";
            DataTable tbl = m_DataProtocol.Tables["Channel"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                wcRet = Set(dr);
            else  // invalid value
                wcRet.iAPSMessageCode = -1;

            return wcRet;
        }

        public WITSChannel Find(int iAPSMessageCode_, string sName_)
        {
            WITSChannel wcRet = new WITSChannel();
            DataRow[] dr;
            string sExpression = "APSMessageCode = '" + iAPSMessageCode_.ToString() + "' AND Name = '" + sName_ + "'";
            DataTable tbl = m_DataProtocol.Tables["Channel"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                wcRet = Set(dr);
            else  // invalid value
                wcRet.iAPSMessageCode = -1;

            return wcRet;
        }

        public string Find(string sName_)
        {
            WITSChannel wcRet = new WITSChannel();
            DataRow[] dr;
            string sExpression = "Name = '" + sName_ + "'";
            DataTable tbl = m_DataProtocol.Tables["Channel"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                wcRet = Set(dr);
            else  // invalid value
                wcRet.iAPSMessageCode = -1;

            return wcRet.sID;
        }

        public string Find2(int iAPSMessageCode_)
        {
            WITSChannel wcRet = new WITSChannel();
            DataRow[] dr;
            string sExpression = "APSMessageCode = '" + iAPSMessageCode_.ToString() + "'";
            DataTable tbl = m_DataProtocol.Tables["Channel"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                wcRet = Set(dr);
            else  // invalid value
                wcRet.iAPSMessageCode = -1;

            return wcRet.sID;
        }

        public List<WITSChannel> GetFromSource(string sVal_, string sExclude_)
        {
            List<WITSChannel> lstRet = new List<WITSChannel>();
            string sExpression = "Source = '" + sVal_ + "' AND APSMessageCode NOT IN " + sExclude_;
            DataTable tbl = m_DataProtocol.Tables["Channel"];
            DataRow[] dr = tbl.Select(sExpression);
            for (int i = 0; i < dr.Count(); i++)
            {
                WITSChannel wcRet = new WITSChannel();
                wcRet.iAPSMessageCode = Convert.ToInt16(dr[i].Field<string>("APSMessageCode"));
                wcRet.sID = dr[i].Field<string>("ID");
                wcRet.sName = dr[i].Field<string>("Name");
                wcRet.sUnits = dr[i].Field<string>("NativeUnit");
                wcRet.sMath = dr[i].Field<string>("Math");
                wcRet.sSendIfError = dr[i].Field<string>("SendIfError");
                wcRet.sOutlinerMin = dr[i].Field<string>("OutlinerMin");
                wcRet.sOutlinerMax = dr[i].Field<string>("OutlinerMax");
                
                lstRet.Add(wcRet);
            }
            return lstRet;
        }

        public DataTable GetAll()
        {
            return m_DataProtocol.Tables["Channel"];
        }

        public bool Save()
        {
            bool bRet = false;
            try
            {
                m_DataProtocol.WriteXml(m_sDataPath, XmlWriteMode.IgnoreSchema);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Saving " + m_sDataPath, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRet;
        }

        public bool Update(string[] sArrCols_)
        {
            bool bRetVal = false;

            DataTable tbl = m_DataProtocol.Tables["Channel"];
            // find the row index
            try
            {
                if (sArrCols_[1].Contains("APSMessageCode"))
                    sArrCols_[1] = sArrCols_[1].Replace("=", "='") + "'";
                DataRow row = tbl.Select(sArrCols_[1]).FirstOrDefault();
                if (row != null)
                {
                    for (int i = 2; i < sArrCols_.Length; i++)
                    {
                        string[] sVal = sArrCols_[i].Split('=');
                        row[sVal[0]] = sVal[1];
                    }
                }
                Save();
                bRetVal = true;
            }
            catch
            {

            }

            return bRetVal;
        }
    }
}
