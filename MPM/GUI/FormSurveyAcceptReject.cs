// author: hoan chau
// purpose: user interface that allows surveys to queue up; and then to be accepted or rejected

using MPM.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MPM.Utilities;
using System.Data.Common;
using MPM.DataAcquisition.Helpers;
using MPM.DataAcquisition.MultiStationAnalysis;
using System.Threading;
using System.Diagnostics;

namespace MPM.GUI
{

    public partial class FormSurveyAcceptReject : Form
    {
        // prevent the grid from trying to redraw when the panel containing
        // the grid for raw axes expands and collapses.
        // the redrawing makes the grid flicker and slows the expansion and contraction
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        private bool m_bExpanded = false;  // flag for expanding or collapsing raw axes grid panel
        private const int EXPANSION_PER_TICK = 7;

        private const int DIR_DEPTH_COMMAND = 10708;
        private const int BUTTONS = 3;
        
        List<CEventSurvey> m_lstRec;
        
        int m_iCounter;  // counts the number of surveys modulo 3

        string m_sDepthUnit;
        CCommonTypes.UNIT_SET m_units;
        CUnitLength m_unitLength;

        bool m_bIgnoreValidation;  // flag to ignore unnecessary pop-ups when adding a record that may result in invalid initial values

        DbConnection m_dbCnn;
        CWITSLookupTable m_LookUpWITS;

        public delegate void EventHandler(object sender, CEventSendWITSData e);
        public event EventHandler Transmit;

        private CMSAClient m_msaClient;
        private string m_sMSAJobID = "";
        private bool m_bUnload;

        private string m_sBHA;
        private Thread threadMSAClientConnect;

        private GroupBox[] gbArr;
        private TextBox[] tbBitDepth;
        private TextBox[] tbOffset;
        private TextBox[] tbSurveyDepth;
        private TextBox[] tbInc;
        private TextBox[] tbAzm;
        private TextBox[] tbGTot;
        private TextBox[] tbMTot;
        private TextBox[] tbDip;
        private Button[] btnReject;
        private Button[] btnAccept;
        private DataGridView []gridView;
        private bool m_bLoaded;

        CSurvey.STATUS[] m_Status;

        private float m_fLastSurveyDepth;

        FormSurveyLog m_frmSurveySensorLog;

        public FormSurveyAcceptReject(ref DbConnection dbCnn_, ref CWITSLookupTable witsLookUpTbl_, ref FormSurveyLog frmSvyLog_)
        {
            InitializeComponent();
            m_dbCnn = dbCnn_;
            m_LookUpWITS = witsLookUpTbl_;
            m_frmSurveySensorLog = frmSvyLog_;

            m_lstRec = new List<CEventSurvey>();
            
            m_iCounter = 0;
            m_bLoaded = false;

            m_sDepthUnit = "ft";
            m_units = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
            m_unitLength = new CUnitLength();

            m_bIgnoreValidation = true;
            m_sBHA = "0";

            m_msaClient = new CMSAClient();
            m_msaClient.SvyDeleted += new CMSAClient.SvyDeletedEventHandler(m_frmSurveySensorLog.OnSurveyDeleted);
            m_msaClient.SvyUpdated += new CMSAClient.SvyUpdatedEventHandler(m_frmSurveySensorLog.OnSurveyUpdated);
            if (m_msaClient.Connect())
                m_sMSAJobID = m_msaClient.GetJobID();
            else
                m_sMSAJobID = "";

            m_bUnload = false;
            threadMSAClientConnect = new Thread(GetMSAJobID);
            threadMSAClientConnect.Start();

            CSurveyLog log = new CSurveyLog(ref m_dbCnn);
            m_fLastSurveyDepth = log.GetLastSurveyDepth();            
        }

        private void GetMSAJobID()
        {
            while (true)
            {
                try
                {
                    if (m_bUnload)
                        break;

                    if (m_sMSAJobID.Length == 0)  // try to get a job id
                    {
                        if (m_msaClient.Connect())
                            m_sMSAJobID = m_msaClient.GetJobID();
                    }

                    Thread.Sleep(5000);
                }
                catch (TimeoutException) { }
            }
        }

        private void buttonShowRawAxes_Click(object sender, EventArgs e)
        {
            panel2.SuspendLayout();
            SendMessage(panel2.Handle, WM_SETREDRAW, false, 0);
            if (m_bExpanded)
            {
                buttonShowRawAxes.Text = "Show Raw Axes";
                timerCollapse.Enabled = true;
                timerExpand.Enabled = false;
            }
            else
            {
                buttonShowRawAxes.Text = "Hide Raw Axes";
                timerExpand.Enabled = true;
                timerCollapse.Enabled = false;
            }
            m_bExpanded = !m_bExpanded;
        }

        private void timerCollapse_Tick(object sender, EventArgs e)
        {                        
            int iNewHeight = panel2.Height - EXPANSION_PER_TICK;            
            if (iNewHeight <= panel2.MinimumSize.Height)
            {
                timerCollapse.Enabled = false;
                iNewHeight = panel2.MinimumSize.Height;
                panel2.ResumeLayout();
                SendMessage(panel2.Handle, WM_SETREDRAW, true, 0);
                panel2.Refresh();
            }

            panel2.Height = iNewHeight;
            this.Height -= EXPANSION_PER_TICK;
        }

        private void timerExpand_Tick(object sender, EventArgs e)
        {            
            int iNewHeight = panel2.Height + EXPANSION_PER_TICK;
            if (iNewHeight >= panel2.MaximumSize.Height)
            {
                timerExpand.Enabled = false;
                iNewHeight = panel2.MaximumSize.Height;
                panel2.ResumeLayout();
                SendMessage(panel2.Handle, WM_SETREDRAW, true, 0);
                panel2.Refresh();
            }

            panel2.Height = iNewHeight;
            this.Height += EXPANSION_PER_TICK;            
        }

        private void FormSurveyAcceptReject_Load(object sender, EventArgs e)
        {
            this.Height -= panel2.Height;
            panel2.Height = 0;

            RefreshLengthUnit();

            SetupColumns();

            AssignWidgetArrays();
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET unitSetNew_)
        {                 
            if (unitSetNew_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT ||
                unitSetNew_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)
                RefreshLengthUnit();
            m_units = unitSetNew_;            
        }

        private void RefreshLengthUnit()
        {
            CDPointLookupTable tbl = new CDPointLookupTable();
            tbl.Load();
            CDPointLookupTable.DPointInfo dpi = tbl.Get(DIR_DEPTH_COMMAND);
            m_sDepthUnit = dpi.sUnits;
        }

        private string GetLengthUnit()
        {
            string sLengthUnit = "ft";
            
            if (m_units == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                sLengthUnit = m_unitLength.GetImperialUnitDesc();
            else if (m_units == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                sLengthUnit = m_unitLength.GetMetricUnitDesc();
            else  // get the value unit from the specific d-point                            
                sLengthUnit = m_sDepthUnit;

            return sLengthUnit;
        }

        private void AssignWidgetArrays()
        {
            if (!m_bLoaded)
            {
                m_bLoaded = true;
                gbArr = new GroupBox[3] { groupBoxSurvey1, groupBoxSurvey2, groupBoxSurvey3 };
                tbBitDepth = new TextBox[3] { textBoxBitDepth1, textBoxBitDepth2, textBoxBitDepth3 };
                tbOffset = new TextBox[3] { textBoxOffset1, textBoxOffset2, textBoxOffset3 };
                tbSurveyDepth = new TextBox[3] { textBoxSurveyDepth1, textBoxSurveyDepth2, textBoxSurveyDepth3 };
                tbInc = new TextBox[3] { textBoxInc1, textBoxInc2, textBoxInc3 };
                tbAzm = new TextBox[3] { textBoxAzm1, textBoxAzm2, textBoxAzm3 };
                tbGTot = new TextBox[3] { textBoxGTotal1, textBoxGTotal2, textBoxGTotal3 };
                tbMTot = new TextBox[3] { textBoxMTotal1, textBoxMTotal2, textBoxMTotal3 };
                tbDip = new TextBox[3] { textBoxDipAngle1, textBoxDipAngle2, textBoxDipAngle3 };
                btnReject = new Button[3] { buttonReject1, buttonReject2, buttonReject3 };
                btnAccept = new Button[3] { buttonAccept1, buttonAccept2, buttonAccept3 };
                gridView = new DataGridView[3] { dataGridViewRawAxes1, dataGridViewRawAxes2, dataGridViewRawAxes3 };

                m_Status = new CSurvey.STATUS[3] { CSurvey.STATUS.NONE, CSurvey.STATUS.NONE, CSurvey.STATUS.NONE };
            }            
        }

        private void ClearDisplayFields()
        {            
            for (int i = 0; i < BUTTONS; i++)
            {
                gbArr[i].Text = "Survey #x";
                tbBitDepth[i].Text = "0";
                tbOffset[i].Text = "0";
                tbSurveyDepth[i].Text = "0";
                tbInc[i].Text = "";
                tbAzm[i].Text = "";
                tbGTot[i].Text = "";
                tbMTot[i].Text = "";
                tbDip[i].Text = "";
                gridView[i].Rows.Clear();

                m_Status[i] = CSurvey.STATUS.NONE;
            }
        }

        private void DisableAccept()
        {
            for (int i = 0; i < BUTTONS; i++)
            {
                btnAccept[i].Enabled = false;
                btnAccept[i].BackColor = Color.Gray;
            }
                
        }

        private void UpdateFields(int iIndx_, CEventSurvey evt_)
        {
            btnReject[iIndx_].Enabled = true;
            btnReject[iIndx_].BackColor = Color.Red;
            btnAccept[iIndx_].Enabled = true;
            btnAccept[iIndx_].BackColor = Color.FromArgb(0, 192, 0);

            gbArr[iIndx_].Text = "Survey #" + evt_.m_iDatabaseID.ToString() + " " + (evt_.rec.TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP ? "Mudpulse": "EM");
            gbArr[iIndx_].ForeColor = evt_.rec.TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP ? Color.Fuchsia : Color.Orange;

            // determine the type of unit to use
            string sLengthUnit = GetLengthUnit();

            tbBitDepth[iIndx_].Text = evt_.rec.fBitDepth.ToString() + " " + sLengthUnit;
            tbOffset[iIndx_].Text = evt_.rec.fDirToBit.ToString() + " " + sLengthUnit;
            evt_.rec.fSurveyDepth = evt_.rec.fBitDepth - evt_.rec.fDirToBit;
            tbSurveyDepth[iIndx_].Text = (evt_.rec.fSurveyDepth).ToString();

            tbInc[iIndx_].Text = evt_.rec.fInclination.ToString() + "°";
            tbAzm[iIndx_].Text = evt_.rec.fAzimuth.ToString() + "°";
            tbGTot[iIndx_].Text = evt_.rec.fGTotal.ToString() + " g";
            tbMTot[iIndx_].Text = evt_.rec.fMTotal.ToString() + " Gs";
            tbDip[iIndx_].Text = evt_.rec.fDipAngle.ToString() + "°";

            SetupColumns();

            gridView[iIndx_].Rows.Clear();
            gridView[iIndx_].Rows.Add(
                "MAG (Gs)",
                evt_.rec.fMX.ToString(),
                evt_.rec.fMY.ToString(),
                evt_.rec.fMZ.ToString());

            gridView[iIndx_].Rows.Add(
                "GRAV (g)",
                evt_.rec.fAX.ToString(),
                evt_.rec.fAY.ToString(),
                evt_.rec.fAZ.ToString());

            if (evt_.rec.fAX > CCommonTypes.BAD_VALUE)
                buttonShowRawAxes.Enabled = true;
            else            
                buttonShowRawAxes.Enabled = false;            
                
        }

        private void SetupColumns()
        {
            for (int i = 0; i < BUTTONS; i++)
            {
                if (gridView[i].Columns.Count == 0)  // first time needs setup
                {
                    gridView[i].Columns.Add("Sensor", "Sensor");
                    gridView[i].Columns.Add("X", "X");
                    gridView[i].Columns.Add("Y", "Y");
                    gridView[i].Columns.Add("Z", "Z");

                    gridView[i].AutoResizeColumns();
                    for (int k = 0; k < gridView[i].Columns.Count; k++)
                    {
                        gridView[i].Columns[k].ReadOnly = true;
                        gridView[i].Columns[k].Width = gridView[i].Width / gridView[i].Columns.Count;
                    }
                    gridView[i].RowHeadersVisible = false;
                }
            }
            
        }       

        private void DisableButtons(int iIndex)
        {           
            btnAccept[iIndex].Enabled = false;
            btnReject[iIndex].Enabled = false;
            btnAccept[iIndex].BackColor = Color.Gray;
            btnReject[iIndex].BackColor = Color.Gray;                                   
        }


        public void Add(CEventSurvey rec_)
        {
            AssignWidgetArrays();

            m_bIgnoreValidation = true;
            if (m_lstRec.Count < BUTTONS)
            {
                if (m_lstRec.Count == 0)
                {
                    ClearDisplayFields();
                    DisableAccept();
                }
                    
                m_lstRec.Add(rec_);                
                UpdateFields(m_iCounter, m_lstRec[m_iCounter]);
                m_iCounter++;                
            }
            
            m_bIgnoreValidation = false;
        }

        private CEventSurvey GetNextRecord(out int iIndex_)
        {
            CEventSurvey rec = new CEventSurvey();
            iIndex_ = -1;
            for (int i = 0; i < m_lstRec.Count; i++)
            {
                if (m_lstRec[i].rec.Status == CSurvey.STATUS.NONE)
                {
                    rec = m_lstRec[i];
                    iIndex_ = i;
                    break;
                }
            }

            return rec;
        }

        private string GatherWITS(CEventSurvey svy_)
        {
            string sRetVal = "";            

            CWITSRecordSurvey rec = new CWITSRecordSurvey();

            //************************************
            // add channels
            //************************************
            rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_INCLINATION));
            rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_AZIMUTH));
            rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GT));
            rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BT));
            rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_DIPANGLE));
            rec.AddChannel(m_LookUpWITS.Find2(10708));


            //************************************
            // set values
            //************************************
            rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_INCLINATION), svy_.rec.fInclination);
            rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_AZIMUTH), svy_.rec.fAzimuth);
            rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GT), svy_.rec.fGTotal);
            rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BT), svy_.rec.fMTotal);
            rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_DIPANGLE), svy_.rec.fDipAngle);
            rec.SetValue(m_LookUpWITS.Find2(10708), svy_.rec.fSurveyDepth);

            if (svy_.rec.Type.ToString().ToLower() == "vector")
            {
                // raw axes
                rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GX));
                rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GY));
                rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GZ));

                rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BX));
                rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BY));
                rec.AddChannel(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BZ));

                rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GX), svy_.rec.fAX);
                rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GY), svy_.rec.fAY);
                rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GZ), svy_.rec.fAZ);

                rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BX), svy_.rec.fMX);
                rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BY), svy_.rec.fMY);
                rec.SetValue(m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BZ), svy_.rec.fMZ);
            }

            sRetVal = rec.GatherData();
            sRetVal = "&&\r\n" + sRetVal + "!!\r\n";  // prepend and append wits flags
            return sRetVal;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            if (!IsValid(iIndex, true))
                return;

            CEventSurvey evt = m_lstRec[iIndex];
            evt.rec.Status = m_Status[iIndex] = CSurvey.STATUS.ACCEPT;
            m_lstRec[iIndex] = evt;
            
            m_fLastSurveyDepth = System.Convert.ToSingle(tbSurveyDepth[iIndex].Text);
            
            CSurveyLog log = new CSurveyLog(ref m_dbCnn);            
            log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[iIndex].rec.fBitDepth);


            btnAccept[iIndex].Enabled = false;
            btnReject[iIndex].Enabled = false;

            SendSurvey(evt);

            //if (IsAllChecked())
            //{
                m_lstRec.Clear();
                m_iCounter = 0;
                this.Hide();
            //}
                
        }               

        private void buttonReject_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            if (!IsValid(iIndex, false))
                return;

            CEventSurvey evt = m_lstRec[iIndex];
            evt.rec.Status = m_Status[iIndex] = CSurvey.STATUS.REJECT;
            m_lstRec[iIndex] = evt;

            m_fLastSurveyDepth = System.Convert.ToSingle(tbSurveyDepth[iIndex].Text);

            CSurveyLog log = new CSurveyLog(ref m_dbCnn);            
            log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[iIndex].rec.fBitDepth);

            DisableButtons(iIndex);

            if (IsAllChecked())
            {
                m_lstRec.Clear();
                m_iCounter = 0;
                this.Hide();
            }
        }

        private void SendSurvey(CEventSurvey svy_)
        {
            if (m_sMSAJobID.Length > 0)  // send record to MSA
            {
                if (svy_.rec.Type.ToString().Contains("VECTOR"))
                    m_msaClient.SendVectorSurvey(svy_.rec);
                else
                    m_msaClient.SendQuickSurvey(svy_.rec);
            }
            else  // send record to WITS EDR
            {
                CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                evtSvyWITS.m_sData = GatherWITS(svy_);
                Transmit(this, evtSvyWITS);
            }
        }

        private bool IsAllChecked()
        {
            bool bRetVal = false;
            int iCount = 0;
            for (int i = 0; i < m_lstRec.Count; i++)
            {
                CEventSurvey evt = m_lstRec[i];
                if (evt.rec.Status != CSurvey.STATUS.NONE)
                    iCount++;
            }
            if (iCount == m_lstRec.Count)
                bRetVal = true;

            return bRetVal;
        }

        private bool IsAllCheckedAlternative(int iIndx_)
        {
            bool bRetVal = false;
            int iCount = 0;
                 
            if (iIndx_ == 1)  
            {
                // first accept/reject pair could be either because how else would the window have shown up in the first place?
                if (btnAccept[0].Enabled == false ||
                    btnReject[0].Enabled == false)
                        iCount++;

                if (btnReject[1].Enabled == false)
                    iCount++;

                if (btnReject[2].Enabled == false)
                    iCount++;
            }
            else if (iIndx_ == 2)
            {
                // first accept/reject pair could be either because how else would the window have shown up in the first place?
                if (btnAccept[0].Enabled == false ||
                    btnReject[0].Enabled == false)
                    iCount++;

                // first 2 accept/reject pairs could be either because the 2 surveys could have arrived but the third did not
                if (btnAccept[1].Enabled == false ||
                    btnReject[1].Enabled == false)
                    iCount++;

                if (btnReject[2].Enabled == false)
                    iCount++;
            }
                                           
            if (iCount == BUTTONS)
                bRetVal = true;

            return bRetVal;
        }

        private void textBoxDepth_TextChanged(object sender, EventArgs e)
        {       
            if (!m_bIgnoreValidation)
                IsValid(0, false);
        }

        private bool IsValid(int iIndx_, bool isAcceptButton_)
        {
            bool bRetVal = false;

            float f;
            if (float.TryParse(tbSurveyDepth[iIndx_].Text, out f))
            {                
                if (f < 0.0f)
                {
                    MessageBox.Show("Survey Depth cannot be negative. Please try again.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbSurveyDepth[iIndx_].Focus();
                }                
                else
                {
                    if (iIndx_ < m_lstRec.Count)
                    {
                        if (f <= m_fLastSurveyDepth)
                        {
                            if (isAcceptButton_)
                            {
                                string sMsg = string.Format("Survey Depth must be greater than the previous accepted value {0}. Please try again.", m_fLastSurveyDepth);
                                MessageBox.Show(sMsg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                bRetVal = false;
                            }
                            else  // rejected surveys don't count 
                                bRetVal = true;
                            
                        }
                        else
                        {
                            CEventSurvey evt = m_lstRec[iIndx_];
                            evt.rec.fSurveyDepth = f;

                            // recalculate the bit depth
                            evt.rec.fBitDepth = evt.rec.fSurveyDepth + evt.rec.fDirToBit;
                            m_lstRec[iIndx_] = evt;

                            tbBitDepth[iIndx_].Text = evt.rec.fBitDepth.ToString() + ' ' + GetLengthUnit();
                            
                            bRetVal = true;
                        }
                        
                    }
                    else
                    {
                        if (tbInc[iIndx_].Text.Trim() == "")  // empty survey.  let them pass
                            bRetVal = true;
                        else if (f < m_fLastSurveyDepth)
                        {
                            string sMsg = string.Format("Survey Depth must be greater than the previous accepted value {0}. Please try again.", m_fLastSurveyDepth);
                            MessageBox.Show(sMsg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            bRetVal = false;
                        }
                    }
                    
                }                
            }
            else
            {
                MessageBox.Show("Invalid character entered for Survey Depth. Please try again.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbSurveyDepth[iIndx_].Focus();                
            }

            return bRetVal;
        }

        public void SetBHA(string sVal_)
        {
            m_sBHA = sVal_;
            m_msaClient.SetBHA(sVal_);
        }

        public void SetInfo(string sURL_, string sAPIKey_)
        {
            m_msaClient.SetInfo(sURL_, sAPIKey_);            
        }

        private void FormSurveyAcceptReject_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bUnload = true;
        }

        public void Unload()
        {
            m_bUnload = true;
        }

        private bool IsEmptySurvey(int iIndx_)
        {
            bool bRetVal = false;

            if (iIndx_ > m_lstRec.Count - 1)  // must be an empty survey (i.e., survey didn't decode?)
                bRetVal = true;

            return bRetVal;
        }



        private void buttonReject2_Click(object sender, EventArgs e)
        {            
            int iIndex = 1;
            if (!IsValid(iIndex, false))
            {
                return;             
            }
            
            if (IsEmptySurvey(iIndex))
            {
                btnReject[iIndex].Enabled = false;
                if (IsAllCheckedAlternative(iIndex))
                {
                    m_lstRec.Clear();
                    m_iCounter = 0;
                    this.Hide();
                }
            }
            else
            {
                CEventSurvey evt = m_lstRec[iIndex];
                evt.rec.Status = m_Status[iIndex] = CSurvey.STATUS.REJECT;
                m_lstRec[iIndex] = evt;

                m_fLastSurveyDepth = System.Convert.ToSingle(tbSurveyDepth[iIndex].Text);

                CSurveyLog log = new CSurveyLog(ref m_dbCnn);                
                log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[iIndex].rec.fBitDepth);

                DisableButtons(iIndex);

                if (IsAllChecked())
                {
                    m_lstRec.Clear();
                    m_iCounter = 0;
                    this.Hide();
                }
            }                                                   
        }

        private void buttonAccept2_Click(object sender, EventArgs e)
        {
            int iIndex = 1;
            if (!IsValid(iIndex, true))
                return;

            CEventSurvey evt = m_lstRec[iIndex];
            evt.rec.Status = m_Status[iIndex] = CSurvey.STATUS.ACCEPT;
            m_lstRec[iIndex] = evt;

            m_fLastSurveyDepth = System.Convert.ToSingle(tbSurveyDepth[iIndex].Text);

            CSurveyLog log = new CSurveyLog(ref m_dbCnn);
            log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[iIndex].rec.fBitDepth);

            btnAccept[iIndex].Enabled = false;
            btnReject[iIndex].Enabled = false;

            SendSurvey(evt);

            //if (IsAllChecked())
            //{
                m_lstRec.Clear();
                m_iCounter = 0;
                this.Hide();
            //}
        }

        private void buttonReject3_Click(object sender, EventArgs e)
        {
            int iIndex = 2;
            if (!IsValid(iIndex, false))
            {
                return;
            }

            if (IsEmptySurvey(iIndex))
            {
                btnReject[iIndex].Enabled = false;
                if (IsAllCheckedAlternative(iIndex))
                {
                    m_lstRec.Clear();
                    m_iCounter = 0;
                    this.Hide();
                }
            }
            else
            {
                CEventSurvey evt = m_lstRec[iIndex];
                evt.rec.Status = m_Status[iIndex] = CSurvey.STATUS.REJECT;
                m_lstRec[iIndex] = evt;

                m_fLastSurveyDepth = System.Convert.ToSingle(tbSurveyDepth[iIndex].Text);

                CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[iIndex].rec.fBitDepth);

                DisableButtons(iIndex);

                if (IsAllChecked())
                {
                    m_lstRec.Clear();
                    m_iCounter = 0;
                    this.Hide();
                }
            }
            
        }

        private void buttonAccept3_Click(object sender, EventArgs e)
        {
            int iIndex = 2;
            if (!IsValid(iIndex, true))
                return;

            CEventSurvey evt = m_lstRec[iIndex];
            evt.rec.Status = m_Status[iIndex] = CSurvey.STATUS.ACCEPT;
            m_lstRec[iIndex] = evt;

            m_fLastSurveyDepth = System.Convert.ToSingle(tbSurveyDepth[iIndex].Text);

            CSurveyLog log = new CSurveyLog(ref m_dbCnn);           
            log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[iIndex].rec.fBitDepth);

            btnAccept[iIndex].Enabled = false;
            btnReject[iIndex].Enabled = false;

            SendSurvey(evt);

            //if (IsAllChecked())
            //{
                m_lstRec.Clear();
                m_iCounter = 0;
                this.Hide();
            //}
        }

        private void textBoxSurveyDepth2_TextChanged(object sender, EventArgs e)
        {
            if (!m_bIgnoreValidation)
                IsValid(1, false);
        }

        private void textBoxSurveyDepth3_TextChanged(object sender, EventArgs e)
        {
            if (!m_bIgnoreValidation)
                IsValid(2, false);
        }

        public void DeleteSurvey(object sender, CEventDeleteSurvey e)
        {
            Debug.WriteLine("deleted");
            m_msaClient.DeleteSurvey(e.ID);
        }

        public void UpdateSurvey(object sender, CEventUpdateSurvey e)
        {
            m_msaClient.UpdateSurvey(e.ID, e.iBHA, e.rec);
        }
    }
}
