// author: hoan chau
// purpose: interface to the settings table based simply on name and value

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
    public class CSettingsLookupTable: IDisposable
    {
        private DataSetSettings m_DataSettings;
        private const string FILE_NAME = "XMLFileSettings.xml";

        private const string SETTINGS_FILE = CCommonTypes.DATA_FOLDER + FILE_NAME;  // the file that user saves and loads from
        private const string DEFAULT_SETTINGS_FILE = CCommonTypes.DATA_FOLDER + "XMLFileSettingsDefault.xml";  // the file that is installed and used as a reference when local settings are not yet present
        private static bool m_bLoadFirstTime = false;

        private string m_sDataPath = SETTINGS_FILE;  // default

        public CSettingsLookupTable()
        {
            if (!m_bLoadFirstTime)
            {
                m_bLoadFirstTime = true;
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
                    DataTable tbl = SettingsDefault.Tables["Setting"];
                    foreach (DataRow dataDefault in SettingsDefault.Tables[0].Rows)
                    {
                        foreach (DataRow dataLocal in SettingsLocal.Tables[0].Rows)
                        {
                            if (dataDefault["Name"].Equals(dataLocal["Name"]) &&
                                !dataDefault["Value"].Equals(dataLocal["Value"]))  // overwrite with local settings
                            {
                                string sExpression = "Name = '" + dataDefault["Name"] + "'";
                                DataRow[] dr = tbl.Select(sExpression);
                                if (dr.Count() > 0)
                                {
                                    dr[0]["Value"] = dataLocal["Value"];
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
        }

        ~CSettingsLookupTable()
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
                if (m_DataSettings != null)
                {
                    m_DataSettings.Dispose();
                    m_DataSettings = null;
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
                m_DataSettings = new DataSetSettings();
                XmlReadMode xmlrm = m_DataSettings.ReadXml(m_sDataPath, System.Data.XmlReadMode.InferSchema);
                
                if (m_DataSettings.Tables.Count > 0)
                    sTbl = m_DataSettings.Tables[0].TableName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Please use the installer!", "Loading Data\\XMLFileSettings.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sTbl;
        }      

        public string GetValue(string sName_)
        {
            string sValue = "No Value";
            DataRow[] dr;
            string sExpression = "Name = '" + sName_ +  "'";
            DataTable tbl = m_DataSettings.Tables["Setting"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                sValue = dr[0].Field<string>("Value");

            return sValue;
        }     
        
        public string GetJobPath()
        {
            string sRetVal = CCommonTypes.DATA_FOLDER;

            string sDataSource = GetValue("DATA_SOURCE");
            if (sDataSource.Length > 0)
            {
                sRetVal = sDataSource.ToLower().Replace("data source=", "").Replace("log.db", "");
            }

            return sRetVal;
        }

        public bool Set(string sName_, string sValue_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "Name = '" + sName_ + "'";
            DataTable tbl = m_DataSettings.Tables["Setting"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
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
                m_DataSettings.WriteXml(m_sDataPath, XmlWriteMode.IgnoreSchema);
                bRet = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Saving Data\\XMLFileSettings.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRet;
        }
    }

    
}
