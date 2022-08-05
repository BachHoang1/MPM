// author: hoan chau
// purpose: show the last few surveys and qualifiers 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;
using MPM.DataAcquisition.Helpers;
using MPM.Utilities;

namespace MPM.GUI
{
    public partial class FormLastSurvey : ToolWindow
    {
        private const int HISTORY_SIZE = 3;
        private const int AGE = 10000;  // milliseconds to add to the last time the surveys were received


        private const int MAX_LABEL_WIDTH = 500;  // pixels
        private const int MIN_FONT_SIZE = 9;
        private const int FONT_RANGE_COUNT = 7;  // 9, 10, 11, 12, 13, 14, and 15

        private enum SURVEY_DPOINT_TYPE { SDT_PACKET_TYPE, SDT_INC, SDT_AZM, SDT_GRAVITY_FIELD, SDT_MAGNETIC_FIELD, SDT_DIP_ANGLE, SDT_MAX };

        
        private Font [] m_arrFont;  // use the appropriately scaled font when
                                   // form is resized

        private struct SURVEY
        {
            public bool bHasParityError;
            public string sValue;
            public SURVEY(bool bDecoded_, string sVal_)
            {
                bHasParityError = bDecoded_;
                sValue = sVal_;
            }
        }

        private bool m_bUnload;

        private CDetectDataLayer m_DataLayer;
        private SURVEY[] m_bArrDPoints = new SURVEY[(int)SURVEY_DPOINT_TYPE.SDT_MAX];

        private Thread m_threadTestPlot;
        private delegate void SafeCallDelegate(string text);

        // Thread Add Data delegate
        private delegate void CorrectLastSurveyDelegate();
        private CorrectLastSurveyDelegate correctLastSurveyDelegate;
        private CSurvey.REC m_recCorrected;

        private CWidgetInfoLookupTable m_widgetInfoLookup;
        private CDPointLookupTable m_DPointTable;

        public FormLastSurvey(ref CWidgetInfoLookupTable widgetInfoLookup_, ref CDPointLookupTable dpointTable_)
        {
            InitializeComponent();

            m_widgetInfoLookup = widgetInfoLookup_;
            m_DPointTable = dpointTable_;

            m_bUnload = false;
            m_threadTestPlot = new Thread(Worker);            

            m_arrFont = new Font[FONT_RANGE_COUNT];
            for (int i = 0; i < FONT_RANGE_COUNT; i++)
            {
                m_arrFont[i] = new Font("Verdana", i + MIN_FONT_SIZE, FontStyle.Bold);
            }

            correctLastSurveyDelegate += new CorrectLastSurveyDelegate(Correct);
        }

        static void Worker(object obj)
        {
            FormLastSurvey param = (FormLastSurvey)obj;
            
            try
            {
                while (true)
                {
                    if (param.m_bUnload)
                        break;
                    Thread.Sleep(AGE);
                    param.SetAge();                    
                }
            }
            catch (ThreadAbortException abortException)
            {
                System.Diagnostics.Debug.WriteLine((string)abortException.ExceptionState);
            }
        }

        private string GetAge(string sVal_)
        {
            string sRetVal = sVal_;

            if (sVal_.Contains(":"))  // add 10 seconds
            {
                string[] sArrTime = sVal_.Split(':');
                int iSeconds = int.Parse(sArrTime[1]);
                int iMinutes = int.Parse(sArrTime[0]);
                iSeconds += AGE / 1000;  // convert milliseconds to seconds
                if (iSeconds > 59)
                {
                    iMinutes++;
                    iSeconds %= 60;
                }
                sRetVal = iMinutes.ToString("00") + ":" + iSeconds.ToString("00");
            }
            
            return sRetVal;
        }

        private void WriteTextSafe(string text)
        {
            if (text == "1")
            {
                if (labelTime1.InvokeRequired)
                {
                    var d = new SafeCallDelegate(WriteTextSafe);
                    Invoke(d, new object[] { text });
                }
                else
                {
                    labelTime1.Text = GetAge(labelTime1.Text);
                }
            }            
            else if (text == "2")
            {
                if (labelTime2.InvokeRequired)
                {
                    var d = new SafeCallDelegate(WriteTextSafe);
                    Invoke(d, new object[] { text });
                }
                else
                {
                    labelTime2.Text = GetAge(labelTime2.Text);
                }
            }            
            else if (text == "3")
            {
                if (labelTime3.InvokeRequired)
                {
                    var d = new SafeCallDelegate(WriteTextSafe);
                    Invoke(d, new object[] { text });
                }
                else
                {
                    labelTime3.Text = GetAge(labelTime3.Text);
                }
            }            
        }

        public void Unload()
        {
            m_bUnload = true;
        }

        private void SetAge()
        {
            WriteTextSafe("1");
            WriteTextSafe("2");
            WriteTextSafe("3");
        }

        public void CorrectLastSurvey(object sender, CSurvey.REC rec_)
        {
            m_recCorrected = rec_;
            this.Invoke(correctLastSurveyDelegate);
        }

        private void Correct()
        {
            try
            {
                Swap();

                // newest: only inc and azm from msa; qualifiers from original survey
                labelTime1.Text = "00:00";
                labelINC1.Text = m_recCorrected.fInclination.ToString("F") + CCommonTypes.ROUND_LAB_DESCRIPTOR2;
                labelAZM1.Text = m_recCorrected.fAzimuth.ToString("F") + CCommonTypes.ROUND_LAB_DESCRIPTOR2;
                if (m_recCorrected.fGTotal > CCommonTypes.BAD_VALUE)
                    labelGravityField1.Text = m_recCorrected.fGTotal.ToString();
                if (m_recCorrected.fMTotal > CCommonTypes.BAD_VALUE)
                    labelMagneticField1.Text = m_recCorrected.fMTotal.ToString();
                if (m_recCorrected.fDipAngle > CCommonTypes.BAD_VALUE)
                    labelDipAngle1.Text = m_recCorrected.fDipAngle.ToString("F");

                ChangeColor();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at FormLastSurvey::CorrectLastSurvey " + ex.Message);
            }
        }

        private void ChangeColor()
        {            
            Label[] lblArrINC = new Label[HISTORY_SIZE] { labelINC1, labelINC2, labelINC3 };
            Label[] lblArrAZM = new Label[HISTORY_SIZE] { labelAZM1, labelAZM2, labelAZM3 };
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                if (lblArrINC[i].Text.Contains(CCommonTypes.ROUND_LAB_DESCRIPTOR2))
                    lblArrINC[i].ForeColor = Color.Lime;
                else
                    lblArrINC[i].ForeColor = Color.White;

                if (lblArrAZM[i].Text.Contains(CCommonTypes.ROUND_LAB_DESCRIPTOR2))
                    lblArrAZM[i].ForeColor = Color.Lime;
                else
                    lblArrAZM[i].ForeColor = Color.White;                
            }
        }

        private void FormLastSurvey_Load(object sender, EventArgs e)
        {
            RefreshScreen();   
        }

        public void RefreshScreen()
        {
            string sTime = "";
            string sTelemetryType = "";
            string sVal = "";
            bool bParityErr = false;            

            Label[] lblArrTime = new Label[HISTORY_SIZE] { labelTime1, labelTime2, labelTime3 };
            Label[] lblArrINC = new Label[HISTORY_SIZE] { labelINC1, labelINC2, labelINC3 };
            Label[] lblArrAZM = new Label[HISTORY_SIZE] { labelAZM1, labelAZM2, labelAZM3 };
            Label[] lblArrGt = new Label[HISTORY_SIZE] { labelGravityField1, labelGravityField2, labelGravityField3 };
            Label[] lblArrMt = new Label[HISTORY_SIZE] { labelMagneticField1, labelMagneticField2, labelMagneticField3 };
            Label[] lblArrDip = new Label[HISTORY_SIZE] { labelDipAngle1, labelDipAngle2, labelDipAngle3 };

            CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_PACKET_TYPE);  // packet type
            if (dpi.iMessageCode > -1)
            {
                labelTimeDesc.Tag = dpi.iMessageCode;
            }
            else  // must be missing for some reason.  user can manually alter the xml file to create it.
            {
                labelTimeDesc.Tag = (int)Command.COMMAND_RESP_PACKET_TYPE;
            }
            labelTime1.Text = "---";

            // read from last session
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, lblArrTime[i].Name, out sTime, out sTelemetryType, out bParityErr);
                lblArrTime[i].Text = sVal;
            }

            dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_INCLINATION);  // inclination
            if (dpi.iMessageCode > -1)
            {
                labelINCDesc.Text = dpi.sDisplayName + " (°)";
                labelINCDesc.Tag = dpi.iMessageCode;
                labelINC1.Tag = dpi.sUnits;
            }
            else
            {
                labelINCDesc.Text = "Inc (°)";
                labelINCDesc.Tag = (int)Command.COMMAND_RESP_INCLINATION;
                labelINC1.Tag = "°";
            }

            for (int i = 0; i < HISTORY_SIZE; i++)
                lblArrINC[i].Text = "--- " + labelINC1.Tag;

            // read from last session
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, lblArrINC[i].Name, out sTime, out sTelemetryType, out bParityErr);
                lblArrINC[i].Text = sVal;
            }



            dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_AZIMUTH);  // azimuth
            if (dpi.iMessageCode > -1)
            {
                labelAZMDesc.Text = dpi.sDisplayName + " (°)";
                labelAZMDesc.Tag = dpi.iMessageCode;
                labelAZM1.Tag = dpi.sUnits;
            }
            else
            {
                labelAZMDesc.Text = "Azm (°)";
                labelAZMDesc.Tag = (int)Command.COMMAND_RESP_AZIMUTH;
                labelAZM1.Tag = "°";
            }

            for (int i = 0; i < HISTORY_SIZE; i++)
                lblArrAZM[i].Text = "--- " + labelAZM1.Tag;

            // read from last session
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, lblArrAZM[i].Name, out sTime, out sTelemetryType, out bParityErr);
                lblArrAZM[i].Text = sVal;
            }

            dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_GT);  // gravity field
            if (dpi.iMessageCode > -1)
            {
                labelGravityFieldDesc.Text = dpi.sDisplayName + " (g)";
                labelGravityFieldDesc.Tag = dpi.iMessageCode;
                labelGravityField1.Tag = dpi.sUnits;
            }
            else
            {
                labelGravityFieldDesc.Text = "Gt (g)";
                labelGravityFieldDesc.Tag = (int)Command.COMMAND_RESP_GT;
                labelGravityField1.Tag = "g";
            }

            for (int i = 0; i < HISTORY_SIZE; i++)
                lblArrGt[i].Text = "--- " + labelGravityField1.Tag;

            // read from last session
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, lblArrGt[i].Name, out sTime, out sTelemetryType, out bParityErr);
                lblArrGt[i].Text = sVal;
            }

            dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_BT);  // magnetic field
            if (dpi.iMessageCode > -1)
            {
                labelMagneticFieldDesc.Text = dpi.sDisplayName + " (Gs)";
                labelMagneticFieldDesc.Tag = dpi.iMessageCode;
                labelMagneticField1.Tag = dpi.sUnits;
            }
            else
            {
                labelMagneticFieldDesc.Text = "Mt (Gs)";
                labelMagneticFieldDesc.Tag = (int)Command.COMMAND_RESP_BT;
                labelMagneticField1.Tag = "Gs";
            }

            for (int i = 0; i < HISTORY_SIZE; i++)
                lblArrMt[i].Text = "--- " + labelMagneticField1.Tag;

            // read from last session
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, lblArrMt[i].Name, out sTime, out sTelemetryType, out bParityErr);
                lblArrMt[i].Text = sVal;
            }

            dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_DIPANGLE);
            if (dpi.iMessageCode > -1)
            {
                labelDipAngleDesc.Text = "DipA (°)";
                labelDipAngleDesc.Tag = dpi.iMessageCode;
                labelDipAngle1.Tag = dpi.sUnits;
            }
            else
            {
                labelDipAngleDesc.Text = "DipA (°)";
                labelDipAngleDesc.Tag = (int)Command.COMMAND_RESP_DIPANGLE;
                labelDipAngle1.Tag = "°";
            }

            for (int i = 0; i < HISTORY_SIZE; i++)
                lblArrDip[i].Text = "--- " + labelDipAngle1.Tag;

            // read from last session
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, lblArrDip[i].Name, out sTime, out sTelemetryType, out bParityErr);
                lblArrDip[i].Text = sVal;
            }

            ChangeColor();
            InitSurvey();
        }

        private void InitSurvey()
        {
            for (int i = 0; i < (int)SURVEY_DPOINT_TYPE.SDT_MAX; i++)
            {
                m_bArrDPoints[i].bHasParityError = false;
                m_bArrDPoints[i].sValue = "";
            }
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ListChanged);
        }

        private void ListChanged(object sender, CEventDPoint e)
        {
            if ((int)labelTimeDesc.Tag == e.m_ID)  // reset for new packet type
            {
                InitSurvey();

                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_PACKET_TYPE].bHasParityError = false;
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_PACKET_TYPE].sValue = DateTime.Now.ToString("HH:mm:ss");
            }
            else if ((int)labelINCDesc.Tag == e.m_ID)
            {                
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_INC].bHasParityError = e.m_bIsParityError;
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_INC].sValue = string.Format("{0,6:###.00}", System.Convert.ToDecimal(e.m_sValue));              
            }
            else if ((int)labelAZMDesc.Tag == e.m_ID)
            {
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_AZM].bHasParityError = e.m_bIsParityError;
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_AZM].sValue = string.Format("{0,6:###.00}", System.Convert.ToDecimal(e.m_sValue));
            }
            else if ((int)labelGravityFieldDesc.Tag == e.m_ID)
            {
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_GRAVITY_FIELD].bHasParityError = e.m_bIsParityError;
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_GRAVITY_FIELD].sValue = e.m_sValue;
            }
            else if ((int)labelMagneticFieldDesc.Tag == e.m_ID)
            {
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_MAGNETIC_FIELD].bHasParityError = e.m_bIsParityError;
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_MAGNETIC_FIELD].sValue = e.m_sValue;
            }
            else if ((int)labelDipAngleDesc.Tag == e.m_ID)
            {
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_DIP_ANGLE].bHasParityError = e.m_bIsParityError;
                m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_DIP_ANGLE].sValue = string.Format("{0,6:###.00}", System.Convert.ToDecimal(e.m_sValue));
            }            

            // check full count
            bool bFullDPointCount = true;
            for (int i = 0; i < (int)SURVEY_DPOINT_TYPE.SDT_MAX; i++)
            {
                if (m_bArrDPoints[i].sValue == "" && !m_bArrDPoints[i].bHasParityError) // still missing one or one has an error
                {
                    bFullDPointCount = false;
                    break;
                }                    
            }
                
            try
            {
                if (bFullDPointCount)  // display it
                {
                    if (labelTime1.Text.Contains("---") || 
                        labelTime1.Text.Contains("00:00"))  // first time starts the thread
                        m_threadTestPlot.Start(this);

                    // push all the previous ones down one
                    Swap();

                    labelTime1.Text = "00:00";// m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_PACKET_TYPE].sValue;
                    labelINC1.Text = m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_INC].sValue;  // the tag stores the units
                    labelAZM1.Text = m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_AZM].sValue;
                    labelGravityField1.Text = m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_GRAVITY_FIELD].sValue;
                    labelMagneticField1.Text = m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_MAGNETIC_FIELD].sValue;
                    labelDipAngle1.Text = m_bArrDPoints[(int)SURVEY_DPOINT_TYPE.SDT_DIP_ANGLE].sValue;

                    ChangeColor();
                    InitSurvey();                    
                }
            }
            catch
            {

            }
        }

        private void Swap()
        {
            labelINC3.Text = labelINC2.Text;
            labelAZM3.Text = labelAZM2.Text;
            labelTime3.Text = labelTime2.Text;
            labelGravityField3.Text = labelGravityField2.Text;
            labelMagneticField3.Text = labelMagneticField2.Text;
            labelDipAngle3.Text = labelDipAngle2.Text;

            labelINC2.Text = labelINC1.Text;
            labelAZM2.Text = labelAZM1.Text;
            labelTime2.Text = labelTime1.Text;
            labelGravityField2.Text = labelGravityField1.Text;
            labelMagneticField2.Text = labelMagneticField1.Text;
            labelDipAngle2.Text = labelDipAngle1.Text;
        }
        
        private void FormLastSurvey_Resize(object sender, EventArgs e)
        {
            Label[] lblArrTime = new Label[HISTORY_SIZE] { labelTime1, labelTime2, labelTime3 };
            Label[] lblArrINC = new Label[HISTORY_SIZE] { labelINC1, labelINC2, labelINC3 };
            Label[] lblArrAZM = new Label[HISTORY_SIZE] { labelAZM1, labelAZM2, labelAZM3 };
            Label[] lblArrGt = new Label[HISTORY_SIZE] { labelGravityField1, labelGravityField2, labelGravityField3 };
            Label[] lblArrMt = new Label[HISTORY_SIZE] { labelMagneticField1, labelMagneticField2, labelMagneticField3 };
            Label[] lblArrDip = new Label[HISTORY_SIZE] { labelDipAngle1, labelDipAngle2, labelDipAngle3 };

            CUnitLength unitLength = new CUnitLength();
            // map width of the 
            int iFontSize = (int)unitLength.ConvertInterval(this.Width, 0, MAX_LABEL_WIDTH, MIN_FONT_SIZE - 1, MIN_FONT_SIZE + FONT_RANGE_COUNT);
            int iIndex = iFontSize - MIN_FONT_SIZE;
            if (iIndex > m_arrFont.Length - 1)
                iIndex = m_arrFont.Length - 1;
            else if (iIndex < 0)
                iIndex = 0;
           
            labelINCDesc.Font = m_arrFont[iIndex];
            labelAZMDesc.Font = m_arrFont[iIndex];
            labelTimeDesc.Font = m_arrFont[iIndex];
            labelMagneticFieldDesc.Font = m_arrFont[iIndex];
            labelGravityFieldDesc.Font = m_arrFont[iIndex];
            labelDipAngleDesc.Font = m_arrFont[iIndex];

            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                lblArrTime[i].Font = m_arrFont[iIndex];
                lblArrINC[i].Font = m_arrFont[iIndex];
                lblArrAZM[i].Font = m_arrFont[iIndex];
                lblArrGt[i].Font = m_arrFont[iIndex];
                lblArrMt[i].Font = m_arrFont[iIndex];
                lblArrDip[i].Font = m_arrFont[iIndex];
            }                                   
        }

        private void FormLastSurvey_FormClosing(object sender, FormClosingEventArgs e)
        {            
            Label[] lblArrTime = new Label[HISTORY_SIZE] { labelTime1, labelTime2, labelTime3 };
            Label[] lblArrINC = new Label[HISTORY_SIZE] { labelINC1, labelINC2, labelINC3 };
            Label[] lblArrAZM = new Label[HISTORY_SIZE] { labelAZM1, labelAZM2, labelAZM3 };
            Label[] lblArrGt = new Label[HISTORY_SIZE] { labelGravityField1, labelGravityField2, labelGravityField3 };
            Label[] lblArrMt = new Label[HISTORY_SIZE] { labelMagneticField1, labelMagneticField2, labelMagneticField3 };
            Label[] lblArrDip = new Label[HISTORY_SIZE] { labelDipAngle1, labelDipAngle2, labelDipAngle3 };

            // save for next session
            for (int i = 0; i < HISTORY_SIZE; i++)
            {        
                m_widgetInfoLookup.SetValue(this.Name, lblArrTime[i].Name, lblArrTime[i].Text);        
                m_widgetInfoLookup.SetValue(this.Name, lblArrINC[i].Name, lblArrINC[i].Text);        
                m_widgetInfoLookup.SetValue(this.Name, lblArrAZM[i].Name, lblArrAZM[i].Text);       
                m_widgetInfoLookup.SetValue(this.Name, lblArrGt[i].Name, lblArrGt[i].Text);        
                m_widgetInfoLookup.SetValue(this.Name, lblArrMt[i].Name, lblArrMt[i].Text);
                m_widgetInfoLookup.SetValue(this.Name, lblArrDip[i].Name, lblArrDip[i].Text);
            }
        }       
    }
}
