// author: hoan chau
// purpose: houses all the known APS dpoints and their related information

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
    public class CDPointLookupTable : IDisposable
    {
        public enum UNIT_TYPE { UT_INT, UT_DOUBLE, UT_NUMBER, UT_MAP, UT_UNKNOWN };
        private const string FILE_NAME = "XMLFileProtocolCommands.xml";        
        private const string DEFAULT_SETTINGS_FILE = CCommonTypes.DATA_FOLDER + "XMLFileProtocolCommandsDefault.xml";
        private bool m_bIsLoaded;
        public struct DPointInfo
        {
            public int iMessageCode;
            public string sDescriptiveName;
            public string sDisplayName;
            public string sUnits;  // of measurement            
            public UNIT_TYPE utDataType;
            public string sValueMapping;  // allows for some flexibility in mapping values to other data
            public string sPurpose;
            public string sThresholdColors;  // some d-points need colored alerts like shock and vibration    
            public string sLowThreshold;  // some d-points have thresholds like snr and pulse height
            public string sHighThreshold;
        }
        
        private DataSetProtocolCommands m_DataProtocol;
        private string m_sLocalSettingsFile;        

        private string m_sDataPath;

        public CDPointLookupTable()
        {            
            Init();                                 
        }

        ~CDPointLookupTable()
        {
            Dispose();
        }

        public void Init()
        {
            m_bIsLoaded = false;

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
                            if (dataDefault["MessageCode"].Equals(dataLocal["MessageCode"]) &&
                                dataDefault["APSName"].Equals(dataLocal["APSName"]) &&
                               (!dataDefault["NativeUnit"].Equals(dataLocal["NativeUnit"])))  // overwrite with local settings
                            {
                                bChanged = true;
                            }
                        }

                        if (bChanged)
                        {
                            string sExpression = "MessageCode = '" + dataDefault["MessageCode"] + "' AND " + "APSName = '" + dataDefault["APSName"] + "'";
                            DataRow[] dr = tbl.Select(sExpression);
                            if (dr.Count() > 0)
                            {
                                try
                                {
                                    dr[0]["NativeUnit"] = dataLocal["NativeUnit"];
                                }
                                catch  // 
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
            m_bIsLoaded = false;
            try
            {
                CSettingsLookupTable settings = new CSettingsLookupTable();
                settings.Load();
                m_sDataPath = settings.GetJobPath() + FILE_NAME;

                m_DataProtocol = new DataSetProtocolCommands();
                m_sLocalSettingsFile = bUseDefault_ ? DEFAULT_SETTINGS_FILE : m_sDataPath;
                XmlReadMode xmlrm = m_DataProtocol.ReadXml(m_sLocalSettingsFile, System.Data.XmlReadMode.InferSchema);                
                if (m_DataProtocol.Tables.Count > 0)
                {
                    sTbl = m_DataProtocol.Tables[0].TableName;
                    m_bIsLoaded = true;
                }
                    
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Please use the installer!", "Loading Data\\XMLFileProtocolCommands.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return sTbl;
        }

        public bool IsLoaded()
        {
            return m_bIsLoaded;
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

        private DPointInfo Set(DataRow[] dr_)
        {
            DPointInfo dpiRet = new DPointInfo();
            dpiRet.iMessageCode = Convert.ToInt16(dr_[0].Field<string>("MessageCode"));
            dpiRet.sDescriptiveName = dr_[0].Field<string>("APSName");
            dpiRet.sDisplayName = dr_[0].Field<string>("DisplayName");
            dpiRet.sUnits = dr_[0].Field<string>("NativeUnit");
            dpiRet.utDataType = DetermineDataType(dr_[0].Field<string>("DataType"));
            dpiRet.sValueMapping = dr_[0].Field<string>("DataType");
            dpiRet.sValueMapping = dpiRet.sValueMapping.Replace("MAP:", "");  // prepare it for value indexing
            dpiRet.sPurpose = dr_[0].Field<string>("Purpose");

            if (dr_[0].Field<string>("ColorMap") == null)
                dpiRet.sThresholdColors = "none";
            else
                dpiRet.sThresholdColors = dr_[0].Field<string>("ColorMap");

            if (dr_[0].Field<string>("LowThreshold") != null)
                if (dr_[0].Field<string>("LowThreshold").Length > 0)
                    dpiRet.sLowThreshold = dr_[0].Field<string>("LowThreshold");

            if (dr_[0].Field<string>("HighThreshold") != null)
                if (dr_[0].Field<string>("HighThreshold").Length > 0)
                    dpiRet.sHighThreshold = dr_[0].Field<string>("HighThreshold");

            return dpiRet;
        }

        public DPointInfo Get(int iMessageCode_)
        {
            DPointInfo dpiRet = new DPointInfo();
            DataRow[] dr;
            string sExpression = "MessageCode = '" + iMessageCode_.ToString() + "'";
            DataTable tbl = m_DataProtocol.Tables["Command"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)            
                dpiRet = Set(dr);                            
            else  // invalid value
                dpiRet.iMessageCode = -1;

            return dpiRet;
        }

        private UNIT_TYPE DetermineDataType(string sDesc_)
        {
            UNIT_TYPE utRet = UNIT_TYPE.UT_UNKNOWN;
            switch (sDesc_)
            {
                case "INT":
                    utRet = UNIT_TYPE.UT_INT;
                    break;
                case "DOUBLE":
                    utRet = UNIT_TYPE.UT_DOUBLE;
                    break;
                case "NUMBER":
                    utRet = UNIT_TYPE.UT_NUMBER;
                    break;                
                default:
                    if (sDesc_.Contains("MAP"))
                        utRet = UNIT_TYPE.UT_MAP;
                    break;
            }

            return utRet;
        }

        public bool SetDisplayName(Int16 iMessageCode_, string sAPSName_, string sNewDisplayName_)
        {
            bool bRet = false;            
            DataRow[] dr;

            string sExpression = "MessageCode = '" + iMessageCode_.ToString() + "' AND APSName = '" + sAPSName_ + "'";
            DataTable tbl = m_DataProtocol.Tables["Command"];   
                 
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["DisplayName"] = sNewDisplayName_;
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool SetUnitName(Int16 iMessageCode_, string sAPSName_, string sNewUnitOfMeasurement_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "MessageCode = '" + iMessageCode_.ToString() + "' AND APSName = '" + sAPSName_ + "'";
            DataTable tbl = m_DataProtocol.Tables["Command"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["NativeUnit"] = sNewUnitOfMeasurement_;
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool SetThresholds(Int16 iMessageCode_, string sLow_, string sHigh_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "MessageCode = '" + iMessageCode_.ToString() + "' ";
            DataTable tbl = m_DataProtocol.Tables["Command"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                dr[0]["LowThreshold"] = sLow_;
                dr[0]["HighThreshold"] = sHigh_;
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public bool SetUnits(string sFrom_, string sTo_)
        {
            bool bRet = false;
            DataRow[] dr;

            string sExpression = "NativeUnit = '" + sFrom_ + "' ";
            DataTable tbl = m_DataProtocol.Tables["Command"];

            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
            {
                for (int i = 0; i < dr.Count(); i++)
                    dr[i]["NativeUnit"] = sTo_;
                bRet = true;
            }
            else
            {
                // do nothing
            }

            return bRet;
        }

        public DataTable GetAllCommands()
        {
            return m_DataProtocol.Tables["Command"];
        }

        public DPointInfo Find(string sDetectName_)
        {
            DPointInfo dpiRet = new DPointInfo();
            DataRow[] dr;
            string sExpression = "APSName = '" + sDetectName_ + "'";
            DataTable tbl = m_DataProtocol.Tables["Command"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)            
                dpiRet = Set(dr);                           
            else  // invalid value
                dpiRet.iMessageCode = -1;


            return dpiRet;
        }

        public DPointInfo Find(int iMessageCode_)
        {
            DPointInfo dpiRet = new DPointInfo();
            DataRow[] dr;
            string sExpression = "MessageCode = '" + iMessageCode_.ToString() + "'";
            DataTable tbl = m_DataProtocol.Tables["Command"];
            dr = tbl.Select(sExpression);
            if (dr.Count() > 0)
                dpiRet = Set(dr);
            else  // invalid value
                dpiRet.iMessageCode = -1;

            return dpiRet;
        }

        public DataTable GetDPointAndUnit()
        {            
            string[] selectedColumns = new[] { "MessageCode", "APSName", "DisplayName", "NativeUnit" };
            DataTable dt = new DataView(m_DataProtocol.Tables["Command"]).ToTable(false, selectedColumns);
            return dt;            
        }
    }
}
 