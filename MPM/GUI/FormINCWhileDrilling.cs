// author: hoan chau
// purpose: display continuous inclination (aka inc while drilling) and survey inclination (aka static survey)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using MPM.Data;
using MPM.DataAcquisition.Helpers;
using System.Data.Common;
using MPM.Utilities;

namespace MPM.GUI
{
    public partial class FormINCWhileDrilling : ToolWindow
    {
        private enum INC_TYPE { WHILE_DRILLING, SURVEY };
        private const int TIME_TO_REMOVE_OLD_POINTS = 180;  // seconds
        private const int MAX_MINUTES_WITHOUT_SLIDING_OR_DRILLING = 5;

        private const int MAX_TEXTBOX_WIDTH = 300;  // pixels
        private const int MIN_FONT_SIZE = 9;
        private const int FONT_RANGE_COUNT = 7;  // 9, 10, 11, 12, 13, 14, and 15

        private enum DRILL_STRING_STATE { SURVEYING, DRILLING, SLIDING, UNKNOWN };
        // for testing fake data                
        //private Thread addDataRunner;
        private bool m_bToggleCsAndStaticSurvey = false;
        private int m_iFakeTFCounter = 0;

        CWITSLookupTable m_LookUpWITS;
        DbConnection m_dbConn;
        private CDPointLookupTable m_DPointTable;

        private CDetectDataLayer m_DataLayer;
        private bool m_bValueFromEvent;
        private float m_fSurveyInc;
        private CCommonTypes.TELEMETRY_TYPE m_iTechnology;

        // Thread Add Data delegate
        private delegate void AddDataDelegate();
        private AddDataDelegate addDataDel;

        // different types of inclination
        private CDPointLookupTable.DPointInfo m_dpiStaticINC;
        private CDPointLookupTable.DPointInfo m_dpiCSINC;
        private CDPointLookupTable.DPointInfo m_dpiAuxINC;
        private CDPointLookupTable.DPointInfo m_dpiNearBitINC;
        private CDPointLookupTable.DPointInfo m_dpiCurrentINCType;

        // flags to indicate state of the drill string
        private DRILL_STRING_STATE m_DrillState;
        private DateTime m_dtDrillState;
        private TimeSpan m_tmDiffSinceLastDrillStateChange;

        public delegate void EventHandler(object sender, CEventSendWITSData e);
        public event EventHandler Transmit;

        private Font[] m_arrFont;  // use the appropriately scaled font when
                                   // form is resized

        // for testing fake data            
        private void AddDataThreadLoop()
        {            
            while (true)
            {
                try
                {
                    // Invoke method must be used to interact with the chart
                    // control on the form!                                                            
                    Thread.Sleep(5000);                    
                    chart1.Invoke(addDataDel);
                }

                catch (Exception e)
                {
                    System.Diagnostics.Debug.Print(e.Message);
                    // Thread is aborted
                }
            }
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ListChanged);
        }

        private bool IsSurveyPacket(int iVal_)
        {
            bool bRetVal = false;
            Array values = Enum.GetValues(typeof(CCommonTypes.SURVEY_TYPE));
            foreach (CCommonTypes.SURVEY_TYPE val in values)
            {
                if (iVal_ == (int)val)
                {
                    bRetVal = true;
                    break;
                }
            }

            return bRetVal;
        }

        private void ListChanged(object sender, CEventDPoint e)
        {
            bool bIsValidINC = false;
            // System.Diagnostics.Debug.Print("This is called when the event fires.");  
                                    
            if (e.m_ID == (int)Command.COMMAND_RESP_PACKET_TYPE)
            {
                if (IsSurveyPacket(Convert.ToInt32(e.m_sValue)))
                {
                    m_DrillState = DRILL_STRING_STATE.SURVEYING;
                }
                else  // to be determined
                {
                    m_DrillState = DRILL_STRING_STATE.UNKNOWN;
                }
            }
            else
            {
                if (e.m_ID == (int)Command.COMMAND_RESP_TF ||
                    e.m_ID == (int)Command.COMMAND_RESP_AUX_TOOLFACE)  // toolfacing
                {
                    if (m_DrillState == DRILL_STRING_STATE.UNKNOWN && !e.m_bIsParityError)  // switch to sliding
                        m_DrillState = DRILL_STRING_STATE.SLIDING;                                       
                }
                else if (e.m_ID == (int)Command.COMMAND_RESP_SHOCKLEVEL ||
                    e.m_ID == (int)Command.COMMAND_RESP_VIBLEVEL ||
                    e.m_ID == (int)Command.COMMAND_RESP_VIBX ||
                    e.m_ID == (int)Command.COMMAND_RESP_VIBY ||
                    e.m_ID == (int)Command.COMMAND_RESP_VIB ||
                    e.m_ID == (int)Command.COMMAND_RESP_VIBXY)  // shock or one of the vibrations                
                {
                    if (m_DrillState == DRILL_STRING_STATE.UNKNOWN && !e.m_bIsParityError)
                        m_DrillState = DRILL_STRING_STATE.DRILLING;
                            
                    // is it a legit value
                }
                else if (e.m_ID == (int)Command.COMMAND_RESP_INCLINATION)  // static or continous survey inclination - depends drill state
                {
                    if (m_DrillState == DRILL_STRING_STATE.SURVEYING)  // static
                    {                        
                        if (!e.m_bIsParityError)
                        {
                            m_dpiCurrentINCType = m_dpiStaticINC;                            
                            bIsValidINC = true;
                        }                            
                    }
                    else  // continuous
                    {                        
                        if (!e.m_bIsParityError)
                        {
                            m_dpiCurrentINCType = m_dpiCSINC;
                            bIsValidINC = true;

                            // send to wits
                            CEventSendWITSData evtSvyWITS = new CEventSendWITSData();                            
                            string sWITSChannel = m_LookUpWITS.Find2((int)Command.COMMAND_CS_INC);
                            evtSvyWITS.m_sData = "&&\r\n" + sWITSChannel + e.m_sValue + "\r\n!!\r\n"; 
                            Transmit(this, evtSvyWITS);
                        }
                    }
                }
                else if (e.m_ID == (int)Command.COMMAND_RESP_AZIMUTH)
                {
                    if (m_DrillState == DRILL_STRING_STATE.SURVEYING)  // static
                    {
                        if (!e.m_bIsParityError)
                        {   
                            // do nothing
                        }
                    }
                    else  // continuous
                    {
                        if (!e.m_bIsParityError)
                        {
                            // send to wits
                            CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                            string sWITSChannel = m_LookUpWITS.Find2((int)Command.COMMAND_CS_AZM);
                            evtSvyWITS.m_sData = "&&\r\n" + sWITSChannel + e.m_sValue + "\r\n!!\r\n";
                            Transmit(this, evtSvyWITS);
                        }
                    }
                }
            }
                        
                                       
            if (bIsValidINC)
            {
                m_bValueFromEvent = true;
                m_fSurveyInc = Convert.ToSingle(e.m_sValue);
                m_iTechnology = e.m_iTechnology;
                chart1.Invoke(addDataDel);
            }      
            else
            {
                m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_NONE;
            }
        }

        public void AddData()
        {
            DateTime timeStamp = DateTime.Now;            
            
            if (m_bValueFromEvent)
            {
                string sDate = timeStamp.ToString("hh:mm tt");
                string sVal = string.Format("{0,6:###.00}", m_fSurveyInc);
                textBoxINCHistory.Text = sDate + "   " + sVal + "°" + "\r\n" + textBoxINCHistory.Text;

                //if (m_DrillState == DRILL_STRING_STATE.SURVEYING)  // must be doing a static survey
                //{
                //    if (m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                //        AddNewPoint(timeStamp, m_fSurveyInc, chart1.Series[0], chart1.Series[1], INC_TYPE.SURVEY);
                //    else
                //        AddNewPoint(timeStamp, m_fSurveyInc, chart1.Series[2], chart1.Series[3], INC_TYPE.SURVEY);
                //}                                 
                //else
                //{
                //    if (m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                //        AddNewPoint(timeStamp, m_fSurveyInc, chart1.Series[0], chart1.Series[1], INC_TYPE.WHILE_DRILLING);
                //    else
                //        AddNewPoint(timeStamp, m_fSurveyInc, chart1.Series[2], chart1.Series[3], INC_TYPE.WHILE_DRILLING);                    
                //}
                    
                m_bValueFromEvent = false;
            }
            else  // from testing
            {                
                //Random rand = new Random();
                float fVal = 45.0f; // rand.Next((int)m_fSurveyInc - 1, (int)m_fSurveyInc + 1);                
                if (m_bToggleCsAndStaticSurvey)
                    fVal = 48.0f;

                if (m_iFakeTFCounter > 5)
                    m_iFakeTFCounter = 0;
                else if (m_iFakeTFCounter > 2)
                {
                    m_DrillState = DRILL_STRING_STATE.SLIDING;
                    m_dtDrillState = DateTime.Now;                    
                }                

                m_tmDiffSinceLastDrillStateChange = DateTime.Now - m_dtDrillState;
                if (m_tmDiffSinceLastDrillStateChange.TotalSeconds > 4)  // must be static survey
                    m_DrillState = DRILL_STRING_STATE.SURVEYING;

                if (m_DrillState == DRILL_STRING_STATE.SLIDING ||
                   m_DrillState == DRILL_STRING_STATE.DRILLING) // continuous inc
                    //m_dpiCurrentINCType = m_dpiCSINC;
                    m_dpiCurrentINCType = m_dpiNearBitINC;
                else
                    m_dpiCurrentINCType = m_dpiStaticINC;

                if (m_DrillState == DRILL_STRING_STATE.SURVEYING)
                    AddNewPoint(timeStamp, fVal, chart1.Series[0], chart1.Series[1], INC_TYPE.SURVEY);
                else
                    AddNewPoint(timeStamp, fVal, chart1.Series[0], chart1.Series[1], INC_TYPE.WHILE_DRILLING);

                m_bToggleCsAndStaticSurvey = !m_bToggleCsAndStaticSurvey;
                m_iFakeTFCounter++;
            }
        }
        
        public FormINCWhileDrilling(ref CWITSLookupTable witsLookUpTbl_, ref DbConnection dbConn_, ref CDPointLookupTable dpointTable_)
        {
            InitializeComponent();

            m_LookUpWITS = witsLookUpTbl_;
            m_dbConn = dbConn_;
            m_DPointTable = dpointTable_;

            // for testing fake data            
            ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            //addDataRunner = new Thread(addDataThreadStart);
            addDataDel += new AddDataDelegate(AddData);
            
            m_bValueFromEvent = false;
            m_fSurveyInc = 0.0f;
            m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_NONE;
            m_DrillState = DRILL_STRING_STATE.UNKNOWN;

            m_arrFont = new Font[FONT_RANGE_COUNT];
            for (int i = 0; i < FONT_RANGE_COUNT; i++)
            {
                m_arrFont[i] = new Font("Verdana", i + MIN_FONT_SIZE, FontStyle.Bold);
            }
        }

        private void StartTestingFakeData()
        {                        
            //if (addDataRunner.IsAlive == true)
            //{
            //    // do nothing
            //}
            //else
            //{
            //    addDataRunner.Start();
            //}
        }

        private void FormINCWhileDrilling_Load(object sender, EventArgs e)
        {
            //StartTestingFakeData();
            
            m_dpiStaticINC = new CDPointLookupTable.DPointInfo();
            m_dpiStaticINC.iMessageCode = 8;
            m_dpiStaticINC = m_DPointTable.Get(m_dpiStaticINC.iMessageCode);

            m_dpiCSINC = new CDPointLookupTable.DPointInfo();
            m_dpiCSINC = m_dpiStaticINC;
            m_dpiCSINC.sDisplayName = "Cs INC";  // no message code exists for continuous inc so I made this up

            m_dpiAuxINC = new CDPointLookupTable.DPointInfo();
            m_dpiAuxINC.iMessageCode = 86;
            m_dpiAuxINC = m_DPointTable.Get(m_dpiAuxINC.iMessageCode);

            m_dpiNearBitINC = new CDPointLookupTable.DPointInfo();
            m_dpiNearBitINC.iMessageCode = 60;
            m_dpiNearBitINC = m_DPointTable.Get(m_dpiNearBitINC.iMessageCode);

            m_dtDrillState = new DateTime(2000, 1, 1, 0, 0, 0);  // put the date in the past so that the first inclination will be static survey

            CLogDataLayer log = new CLogDataLayer(ref m_dbConn);           
            List<CLogDataLayer.PLOT_DATA> lst = log.GetLast(20, (int)Command.COMMAND_RESP_INCLINATION);
            for (int i = 0; i < lst.Count(); i++)
            {
                DateTime dt = System.Convert.ToDateTime(lst[i].sDateTime);
                string sDate = dt.ToString("hh:mm tt");
                string sVal = string.Format("{0,6:###.00}", lst[i].fValue);
                textBoxINCHistory.Text = sDate + "   " + sVal + "°" + "\r\n" + textBoxINCHistory.Text;
            }
        }

        private void FormINCWhileDrilling_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_DataLayer.Changed -= new ChangedEventHandler(ListChanged);
            //addDataRunner.Abort();
        }

        private void AddNewPoint(DateTime timeStamp, float fVal_, System.Windows.Forms.DataVisualization.Charting.Series ptSeriesCsINC, System.Windows.Forms.DataVisualization.Charting.Series ptSeriesStaticINC, INC_TYPE icVal_)
        {
            
            // Add new data point to its series.
            //if (chart1.InvokeRequired)
            {
                if (icVal_ == INC_TYPE.WHILE_DRILLING)
                {
                    //ptSeriesCsINC.Name = m_dpiCurrentINCType.sDisplayName;
                    ptSeriesCsINC.Points.AddXY(timeStamp.ToOADate(), fVal_);
                }                                  
                else
                    ptSeriesStaticINC.Points.AddXY(timeStamp.ToOADate(), fVal_);

                // remove all points from the source series older than TIME_TO_REMOVE_OLD_POINTS
                double removeBefore = timeStamp.AddSeconds((double)(TIME_TO_REMOVE_OLD_POINTS) * (-1)).ToOADate();

                //remove oldest values to maintain a constant number of data points                
                while (ptSeriesCsINC.Points.Count > 0 && ptSeriesCsINC.Points[0].XValue < removeBefore)
                {
                    ptSeriesCsINC.Points.RemoveAt(0);
                }
                                                                
                while (ptSeriesStaticINC.Points.Count > 0 && ptSeriesStaticINC.Points[0].XValue < removeBefore)
                {
                    ptSeriesStaticINC.Points.RemoveAt(0);
                }
                                                                
                if (icVal_ == INC_TYPE.WHILE_DRILLING)  // ensure that we don't push static inc points off the plot area
                {
                    chart1.ChartAreas[0].AxisX.Minimum = ptSeriesCsINC.Points[0].XValue;
                    if (ptSeriesStaticINC.Points.Count > 0)
                        if (ptSeriesStaticINC.Points[0].XValue < chart1.ChartAreas[0].AxisX.Minimum)
                            chart1.ChartAreas[0].AxisX.Minimum = ptSeriesStaticINC.Points[0].XValue;

                    chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeriesCsINC.Points[0].XValue).AddSeconds(TIME_TO_REMOVE_OLD_POINTS).ToOADate();                                         
                }
                else
                {
                    if (ptSeriesStaticINC.Points.Count > 0)  // ensure that we don't push cs inc points off the plot area
                    {
                        chart1.ChartAreas[0].AxisX.Minimum = ptSeriesStaticINC.Points[0].XValue;
                        if (ptSeriesCsINC.Points.Count > 0)
                            if (ptSeriesCsINC.Points[0].XValue < chart1.ChartAreas[0].AxisX.Minimum)
                                chart1.ChartAreas[0].AxisX.Minimum = ptSeriesCsINC.Points[0].XValue;
                        chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeriesStaticINC.Points[0].XValue).AddSeconds(TIME_TO_REMOVE_OLD_POINTS).ToOADate();
                    }                                             
                }
                
                chart1.Invalidate();
            }

        }

        private void labelTitle_Click(object sender, EventArgs e)
        {

        }

        private void textBoxINCHistory_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxINCHistory_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxINCHistory.ScrollBars = ScrollBars.Vertical;
        }

        private void textBoxINCHistory_MouseLeave(object sender, EventArgs e)
        {
            textBoxINCHistory.ScrollBars = ScrollBars.None;
        }

        private void FormINCWhileDrilling_Resize(object sender, EventArgs e)
        {
            CUnitLength unitLength = new CUnitLength();
            // map width of the 
            int iFontSize = (int)unitLength.ConvertInterval(this.Width, 0, MAX_TEXTBOX_WIDTH, MIN_FONT_SIZE - 1, MIN_FONT_SIZE + FONT_RANGE_COUNT);
            int iIndex = iFontSize - MIN_FONT_SIZE;
            if (iIndex > m_arrFont.Length - 1)
                iIndex = m_arrFont.Length - 1;
            else if (iIndex < 0)
                iIndex = 0;

            textBoxINCHistory.Font = m_arrFont[iIndex];
        }
    }
}
