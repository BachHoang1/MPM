// author: hoan chau
// purpose: to display information that a user can enter for exporting to LAS files

using MPM.DataAcquisition.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MPM.Data
{
    

    public class FormatStringConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> list = new List<String>();
            list.Add("0.016667");  // for time so that export can be done for every one second
            list.Add("0.1");
            list.Add("0.25");
            list.Add("0.5");
            list.Add("1.0");            
            return new StandardValuesCollection(list);
        }
    }
    public class CLASJobInfo
    {
        public enum EXPORT_DATA_TYPE { MD, TIME, TVD, TVD_FROM_CORRECTED_SVY };
        public enum TIME_UNITS {UNIX, DATE_TIME};
        public enum COUNTRY { CANADA, USA };     

        // location
        private string m_sCompany = "Company J";
        private string m_sField = "Field X";
        private string m_sWell = "Well Y";
        private COUNTRY m_Country = COUNTRY.CANADA;
        private string m_sArea = "CA";
        private string m_sFacility = "Building A";
        private string m_sServiceCompany = "A1 Service";
        private DateTime m_dtJobEndDate = DateTime.Now;

        // export options
        private EXPORT_DATA_TYPE m_exportType = EXPORT_DATA_TYPE.TIME;
        private float m_fStartDepth = 0.0f;
        private float m_fStopDepth = 1000.0f;
        private DateTime m_dtStartTime;
        private DateTime m_dtStopTime;
        private TIME_UNITS m_tmUnit = TIME_UNITS.DATE_TIME;
        private string m_sStep = "0";
        private CCommonTypes.TELEMETRY_TYPE_FOR_LAS m_ttType;
        CWidgetInfoLookupTable m_WidgetInfoLookupTbl;



        public CLASJobInfo(ref DbConnection dbCnn_, ref CWidgetInfoLookupTable widgetInfoLookup_)  // constructor
        {
            // load up settings from last session            
            m_WidgetInfoLookupTbl = widgetInfoLookup_; // new CWidgetInfoLookupTable();
            //m_WidgetInfoLookupTbl.Load();
            
            // well info
            m_sCompany = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Company");            
            m_sWell = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Well");
            m_sField = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Field");
            m_sFacility = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Facility");
            m_Country = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Country") == "COUNTRY.USA" ? COUNTRY.USA: COUNTRY.CANADA;
            m_sArea = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Area");
            m_sServiceCompany = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "ServiceCompany");

            // export options
            m_exportType = EXPORT_DATA_TYPE.MD; // m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Area") == "EXPORT_DATA_TYPE.GAMMA" ? EXPORT_DATA_TYPE.GAMMA : EXPORT_DATA_TYPE.PRESSURE;

            CDrillingBitDepth depthObj = new CDrillingBitDepth(ref dbCnn_, (int)Command.COMMAND_BIT_DEPTH);
            float fMinDepth = 0, fMaxDepth = 0;
            if (depthObj.GetDepthRange(out fMinDepth, out fMaxDepth))
            {
                m_fStartDepth = fMinDepth;
                m_fStopDepth = fMaxDepth;
            }
            else
            {
                m_fStartDepth = Convert.ToSingle(m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "StartDepth"));
                m_fStopDepth = Convert.ToSingle(m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "StopDepth"));
            }
            
            m_tmUnit = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Area") == "TIME_UNITS.DATE_TIME" ? TIME_UNITS.DATE_TIME : TIME_UNITS.UNIX;
            m_sStep = m_WidgetInfoLookupTbl.GetValue("FormLASExport", "propertyGridInfo", "Step");

            string sStartTime, sStopTime;
            if (depthObj.GetTimeRange(out sStartTime, out sStopTime))
            {
                m_dtStartTime = System.Convert.ToDateTime(sStartTime);
                m_dtStopTime = System.Convert.ToDateTime(sStopTime);
            }
            else
            {
                m_dtStartTime = DateTime.Now;
                m_dtStopTime = DateTime.Now;
            }
            
        }        

        public void Save()
        {
            m_WidgetInfoLookupTbl.Save();
        }
     

        [Category("Well Information")]
        public string Company
        {
            get { return m_sCompany; }
            set { m_sCompany = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "Company", m_sCompany); }
        }

        [Category("Well Information")]
        public string Well
        {
            get { return m_sWell; }
            set { m_sWell = value;
                 m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "Well", m_sWell); }
        }

        [Category("Well Information")]
        public string Field
        {
            get { return m_sField; }
            set { m_sField = value;
                  m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "Field", m_sField); }
        }

        [Category("Well Information")]
        public string Facility
        {
            get { return m_sFacility; }
            set { m_sFacility = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "Facility", m_sFacility);}
        }

        [Category("Well Information")]
        public COUNTRY Country
        {
            get { return m_Country; }
            set { m_Country = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "Country", m_Country == COUNTRY.USA ? "COUNTRY.USA" : "COUNTRY.CANADA"); }
        }

        [Category("Well Information")]
        public string State
        {
            get { return m_sArea; }
            set { m_sArea = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "Area", m_sArea); }
        }

        [Category("Well Information")]
        [DisplayName("Service Company")]
        public string Service
        {
            get { return m_sServiceCompany; }
            set
            {
                m_sServiceCompany = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "ServiceCompany", m_sServiceCompany);
            }
        }

        [Category("Well Information")]
        [DisplayName("Job End Date")]
        public DateTime EndDate
        {
            get { return m_dtJobEndDate; }
            set
            {
                m_dtJobEndDate = value;
            }
        }

        [Category("Export Options")]
        [System.ComponentModel.RefreshProperties(RefreshProperties.All)]
        public EXPORT_DATA_TYPE Export_Type
        {
            get { return m_exportType; }
            set { m_exportType = value;

                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "ExportType", m_exportType == EXPORT_DATA_TYPE.TIME ? "EXPORT_DATA_TYPE.TIME" : "EXPORT_DATA_TYPE.MD");

                bool newValue = (value != EXPORT_DATA_TYPE.TIME);
                if (value == EXPORT_DATA_TYPE.TIME)
                {
                    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["Time_Start"];
                    ReadOnlyAttribute attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    FieldInfo isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, false);

                    descriptor = TypeDescriptor.GetProperties(this.GetType())["Time_Stop"];
                    attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, false);

                    descriptor = TypeDescriptor.GetProperties(this.GetType())["Depth_Start"];
                    attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, true);

                    descriptor = TypeDescriptor.GetProperties(this.GetType())["Depth_Stop"];
                    attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, true);

                }
                else
                {
                    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["Depth_Start"];
                    ReadOnlyAttribute attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    FieldInfo isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, false);

                    descriptor = TypeDescriptor.GetProperties(this.GetType())["Depth_Stop"];
                    attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, false);

                    descriptor = TypeDescriptor.GetProperties(this.GetType())["Time_Start"];
                    attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, true);

                    descriptor = TypeDescriptor.GetProperties(this.GetType())["Time_Stop"];
                    attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    isReadOnly.SetValue(attrib, true);
                }
            }
        }
        [Category("Export Options")]
        [ReadOnly(false)]
        public float Depth_Start
        {
            get { return m_fStartDepth; }
            set { m_fStartDepth = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "StartDepth", m_fStartDepth.ToString()); }
        }

        [Category("Export Options")]
        [ReadOnly(false)]
        public float Depth_Stop
        {
            get { return m_fStopDepth; }
            set { m_fStopDepth = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "StopDepth", m_fStopDepth.ToString()); }
        }

        [Category("Export Options")]
        [ReadOnly(true)]
        [Editor(typeof(DateTime), typeof(DateTime))]
        public String Time_Start
        {
            get { return m_dtStartTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            set
            {
                m_dtStartTime = System.Convert.ToDateTime(value);
                //m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "StartTime", m_dtStartTime.ToString());
            }
        }
       

        [Category("Export Options")]
        [ReadOnly(true)]
        [Editor(typeof(DateTime), typeof(DateTime))]
        public String Time_Stop
        {
            get { return m_dtStopTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            set
            {
                m_dtStopTime = System.Convert.ToDateTime(value);
                //m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "StopTime", m_dtStopTime.ToString());
            }
        }

        [Category("Export Options")]
        public TIME_UNITS Time_Type
        {
            get { return m_tmUnit; }
            set { m_tmUnit = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "TimeType", m_tmUnit == TIME_UNITS.DATE_TIME ? "TIME_UNITS.DATE_TIME" : "TIME_UNITS.UNIX"); }
        }

        [Category("Export Options"),
        TypeConverter(typeof(FormatStringConverter))]
        public string Step
        {
            get { return m_sStep; }
            set { m_sStep = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "Step", m_sStep); }
        }

        [Category("Export Options")]       
        public CCommonTypes.TELEMETRY_TYPE_FOR_LAS Telemetry_Type
        {
            get { return m_ttType; }
            set
            {
                m_ttType = value;
                m_WidgetInfoLookupTbl.SetValue("FormLASExport", "propertyGridInfo", "TelemetryType", m_ttType.ToString());
            }
        }
    }
}
