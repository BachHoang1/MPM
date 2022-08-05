// author: hoan chau
// purpose: group together those settings for LAS export.  Using the grid property control
//          will automatically generate the proper GUI field given a data type

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;
using MPM.DataAcquisition.Helpers;
using MPM.DataAcquisition.MultiStationAnalysis;
using MPM.Utilities;

namespace MPM.GUI
{

    public partial class FormLASExport : Form
    {
        private CLASJobInfo m_Job;
        private string m_sMSAAPIKey;
        private CCommonTypes.UNIT_SET m_iUnitSet = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;

        private DbConnection m_dbCnn;
        private CMSAHubClient m_msaHubClient;
        private CWidgetInfoLookupTable m_widgetInfoLookup;
        public FormLASExport(ref DbConnection dbCnn_, CMSAHubClient msaHubClient_, ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            InitializeComponent();
            m_dbCnn = dbCnn_;
            m_msaHubClient = msaHubClient_;
            m_widgetInfoLookup = widgetInfoLookup_;
        }

        private void FormLASExport_Load(object sender, EventArgs e)
        {
            m_Job = new CLASJobInfo(ref m_dbCnn, ref m_widgetInfoLookup);
            propertyGridInfo.PropertySort = PropertySort.Categorized; // leaves order the way it is in code            
            propertyGridInfo.HelpVisible = false;  // this isn't necessary and just takes up real estate
            propertyGridInfo.SelectedObject = m_Job;  // automatically loads up the entry fields based on data type
            propertyGridInfo.Select();

            propertyGridInfo.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);
        }

        void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //bool newValue = !(((CLASJobInfo)propertyGridInfo.SelectedObject).Export_Type == CLASJobInfo.EXPORT_DATA_TYPE.TIME);
            //PropertyDescriptor descriptor = TypeDescriptor.GetProperties(propertyGridInfo.SelectedObject.GetType())["Time_Start"];
            //ReadOnlyAttribute attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
            //FieldInfo isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
            //isReadOnly.SetValue(attrib, newValue);

            //propertyGridInfo.SelectedObject = propertyGridInfo.SelectedObject;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {                        
            // gather all the settings and pass them to the context object                        
            CWellJob job = new CWellJob();
            job.Client = m_Job.Company;
            job.WellId = m_Job.Well;
            job.Field = m_Job.Field;
            job.Facility = m_Job.Facility;
            job.Country = m_Job.Country.ToString();            
            job.Area = m_Job.State;
            job.EndDate = m_Job.EndDate;
            job.ServiceCompany = m_Job.Service;
            
            CLogDataLayer log = new CLogDataLayer(ref m_dbCnn);

            CLASJobInfo.EXPORT_DATA_TYPE exportType = m_Job.Export_Type;
            
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref m_dbCnn, m_msaHubClient, 7, 40, m_iUnitSet);
            
            
            float fStartDepth = m_Job.Depth_Start;
            float fStopDepth = m_Job.Depth_Stop;
            DateTime dtStartTime = System.Convert.ToDateTime(m_Job.Time_Start);
            DateTime dtStopTime = System.Convert.ToDateTime(m_Job.Time_Stop);
            bool bShowUnixTime = m_Job.Time_Type== CLASJobInfo.TIME_UNITS.UNIX;
            //CCommonTypes.StepLevelEnum stepLevel = CCommonTypes.StepLevelEnum.Step_1_0;
            float fStep = System.Convert.ToSingle(m_Job.Step);
            CCommonTypes.TELEMETRY_TYPE_FOR_LAS ttType = m_Job.Telemetry_Type;

            List<CLogASCIIStandard.CURVE_INFO> lstSelectedCurves = new List<CLogASCIIStandard.CURVE_INFO>();
            
            DataTable tbl = log.GetMessageCodes(exportType, m_iUnitSet);
            FormLASCurveSelection frmLASCrvSel = new FormLASCurveSelection();
            frmLASCrvSel.SetData(tbl);
            if (frmLASCrvSel.ShowDialog() == DialogResult.OK)
            {
                lstSelectedCurves = frmLASCrvSel.GetCurveSelections();
                if (lstSelectedCurves.Count < 1)                
                    return;   
                else if (lstSelectedCurves.Count > 1)  // make sure that if md or tvd is selected, they are positioned at the start
                {
                    for (int i = 0; i < lstSelectedCurves.Count; i++)
                    {
                        if (lstSelectedCurves[i].iMsgCode == (long)(Command.COMMAND_BIT_DEPTH) ||
                            lstSelectedCurves[i].iMsgCode == (long)(Command.COMMAND_TVD))
                        {
                            CLogASCIIStandard.CURVE_INFO ciDepth = lstSelectedCurves[i];
                            lstSelectedCurves.RemoveAt(i);
                            lstSelectedCurves.Insert(0, ciDepth);
                            break;
                        }
                    }
                }
            }
            else
                return;

            // open a dialog to allow user to choose where they want it exported
            SaveFileDialog fileSave = new SaveFileDialog();
            fileSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileSave.FileName = m_Job.Export_Type.ToString() + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + ".LAS";
            if (fileSave.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = fileSave.FileName;

            // do the export
            Cursor.Current = Cursors.WaitCursor;
            lasMgr.CreateLasFile(job, exportType, sFileName, fStartDepth, fStopDepth, dtStartTime, dtStopTime, bShowUnixTime, fStep, ttType, lstSelectedCurves);
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Export Complete.", "LAS Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start("Notepad", sFileName);
        }

        private void FormLASExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Job.Save();
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET iVal_)
        {
            m_iUnitSet = iVal_;
        }

        public void SetAPIKey(string sVal_)
        {
            m_sMSAAPIKey = sVal_;            
        }

        private void propertyGridInfo_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            GridItem itemNew = e.NewSelection;
            GridItem itemOld = e.OldSelection;
        }

        private void propertyGridInfo_SelectedObjectsChanged(object sender, EventArgs e)
        {
            
        }

        private void propertyGridInfo_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            GridItem itemChanged = e.ChangedItem;
            object x = e.OldValue;
            object y = itemChanged.Value;
            if (itemChanged.Label == "Export_Type")
            {
                if (y.ToString() == "MD")
                {
                    // get the minimum and the maxium depths
                    CDrillingBitDepth depthObj = new CDrillingBitDepth(ref m_dbCnn, (int)Command.COMMAND_BIT_DEPTH);
                    float fMinDepth = 0, fMaxDepth = 0;
                    if (depthObj.GetDepthRange(out fMinDepth, out fMaxDepth))
                    {
                        m_Job.Depth_Start = fMinDepth;
                        m_Job.Depth_Stop = fMaxDepth;
                    }                    
                }
                else if (y.ToString() == "TIME")
                {
                    // get the start and stop time
                    CDrillingBitDepth depthObj = new CDrillingBitDepth(ref m_dbCnn, (int)Command.COMMAND_BIT_DEPTH);
                    string sStartTime, sEndTime;
                    if (depthObj.GetTimeRange(out sStartTime, out sEndTime))
                    {
                        m_Job.Time_Start = sStartTime;
                        m_Job.Time_Stop = sEndTime;
                    }
                }
                else if (y.ToString() == "TVD") 
                {
                    float fStartDepth, fStopDepth;
                    CalculateTVD(CCommonTypes.TELEMETRY_TYPE.TT_EM, out fStartDepth, out fStopDepth);
                    m_Job.Depth_Start = fStartDepth;
                    m_Job.Depth_Stop = fStopDepth;
                }
                else  // must be TVD from corrected surveys
                {
                    if (m_sMSAAPIKey.Trim().Length == 0)
                    {
                        MessageBox.Show("No TVD exists for corrected Surveys. This only works with MSA enabled.  See menu System Settings->Job and Survey Management.", "TVD from Corrected Surveys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        float fStartDepth, fStopDepth;
                        m_msaHubClient.GetTVDRange(out fStartDepth, out fStopDepth);
                        m_Job.Depth_Start = fStartDepth;
                        m_Job.Depth_Stop = fStopDepth;
                    }
                    
                }
            }
        }

        private void CalculateTVD(CCommonTypes.TELEMETRY_TYPE ttType_, out float fStartDepth_, out float fStopDepth_)
        {
            // get all the surveys and compute the tvd for all of them
            CSurveyLog log = new CSurveyLog(ref m_dbCnn);
            DataTable tblSurvey = log.Get(ttType_.ToString(), "ACCEPT");

            double dblTVD = 0;
            CSurveyCalculation calc = new CSurveyCalculation();           
            List<double> lstTVDRef = new List<double>();

            // first survey depth should also be the first tvd depth
            CSurvey.REC rec0 = new CSurvey.REC();
            if (tblSurvey.Rows.Count > 0)
            {
                rec0.fBitDepth = (float)tblSurvey.Rows[0].Field<decimal>("bitDepth");
                lstTVDRef.Add(rec0.fBitDepth);
            }

            for (int i = 0; i < tblSurvey.Rows.Count - 1; i++)
            {               
                CSurvey.REC rec1 = new CSurvey.REC();
                CSurvey.REC rec2 = new CSurvey.REC();

                rec1.fAzimuth = (float)tblSurvey.Rows[i].Field<decimal>("azm");
                rec1.fInclination = (float)tblSurvey.Rows[i].Field<decimal>("inc");
                rec1.fBitDepth = (float)tblSurvey.Rows[i].Field<decimal>("bitDepth");

                rec2.fAzimuth = (float)tblSurvey.Rows[i + 1].Field<decimal>("azm");
                rec2.fInclination = (float)tblSurvey.Rows[i + 1].Field<decimal>("inc");
                rec2.fBitDepth = (float)tblSurvey.Rows[i + 1].Field<decimal>("bitDepth");

                double dblTVDTemp = calc.GetTVD(rec1, rec2);
                dblTVD += dblTVDTemp;
                lstTVDRef.Add(dblTVD);
            }

            if (lstTVDRef.Count > 0)
            {
                fStartDepth_ = (float)lstTVDRef[0];
                fStopDepth_ = (float)lstTVDRef[lstTVDRef.Count - 1];
            }
            else
            {
                fStartDepth_ = 0;
                fStopDepth_ = 0;
            }
            
        }
    }
}
