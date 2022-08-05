using MPM.Data;
using MPM.DataAcquisition.Helpers;
using MPM.DataAcquisition.MultiStationAnalysis;
using MPM.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.GUI
{
    public partial class FormSurveyAcceptRejectInfinite : Form
    {        
        enum COLUMNS {  ID, CREATED, STATUS, SVY_DEPTH, INC, AZM, MTOT, DIPA, GTOT, BX, BY, BZ, GX, GY, GZ};

        DbConnection m_dbCnn;
        CWITSLookupTable m_LookUpWITS;
        FormSurveyLog m_frmSurveySensorLog;

        List<CEventSurvey> m_lstRec;
        DataTable m_tblRawSurvey;

        bool m_bIsTableCreated;

        private float m_fLastSurveyDepth;
        private float m_fSelectedSurveyDepth;
        int m_iRowSelected;
        string m_sStatus;

        string m_sDepthUnit;
        CCommonTypes.UNIT_SET m_units;
        CUnitLength m_unitLength;

        private string m_sBHA;
        private Thread threadMSAClientConnect;
        public delegate void EventHandler(object sender, CEventSendWITSData e);
        public event EventHandler Transmit;

        public delegate void EventHandlerUpdateRaw(object sender);
        public event EventHandlerUpdateRaw UpdateRaw;

        private CMSAClient m_msaClient;
        private string m_sMSAJobID = "";
        private bool m_bUnload;


        public FormSurveyAcceptRejectInfinite(ref DbConnection dbCnn_, ref CWITSLookupTable witsLookUpTbl_, ref FormSurveyLog frmSvyLog_)
        {
            InitializeComponent();
            m_dbCnn = dbCnn_;
            m_LookUpWITS = witsLookUpTbl_;
            m_frmSurveySensorLog = frmSvyLog_;

            m_lstRec = new List<CEventSurvey>();
            m_tblRawSurvey = new DataTable();

            m_sDepthUnit = "ft";
            m_units = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
            m_unitLength = new CUnitLength();
                     
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

            m_bIsTableCreated = false;
            m_iRowSelected = -1;
            m_sStatus = "";
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
                        else
                            m_sMSAJobID = "";
                    }                    

                    Thread.Sleep(5000);
                }
                catch (TimeoutException) { }
            }
        }

        public void ClearMSAJobID()
        {
            m_sMSAJobID = "";
        }

        private void RefreshLengthUnit()
        {
            CDPointLookupTable tbl = new CDPointLookupTable();
            tbl.Load();
            CDPointLookupTable.DPointInfo dpi = tbl.Get((int)Command.COMMAND_BIT_DEPTH);
            m_sDepthUnit = dpi.sUnits;
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET unitSetNew_)
        {
            if (unitSetNew_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT ||
                unitSetNew_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)
                RefreshLengthUnit();
            m_units = unitSetNew_;
            if (m_tblRawSurvey != null && m_tblRawSurvey.Columns.Count > (int)COLUMNS.SVY_DEPTH)
                m_tblRawSurvey.Columns[(int)COLUMNS.SVY_DEPTH].ColumnName = "Survey Depth (" + GetLengthUnit() + ")";
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

        private void FormSurveyAcceptRejectInfinite_Load(object sender, EventArgs e)
        {
            //RefreshLengthUnit();                        
            if (!m_bIsTableCreated)
                CreateTable();

            // populate with data
            //for (int i = 0; i < 9; i++)
            //{
            //    DataRow row = m_tblRawSurvey.NewRow();
            //    row.SetField(0, i);
            //    row.SetField(1, DateTime.Now);
            //    if (i == 0)
            //        row.SetField(2, "ACCEPT");
            //    else if (i == 1)
            //        row.SetField(2, "REJECT");
            //    else
            //        row.SetField(2, "NONE");
            //    row.SetField(3, "100");
            //    row.SetField(4, "90.00");
            //    row.SetField(5, "270.00");
            //    row.SetField(6, "1.00000");
            //    row.SetField(7, "45.00");
            //    row.SetField(8, "1.00000");

            //    m_tblRawSurvey.Rows.Add(row);
            //}


            //dataGridViewSurveyRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            //for (int iCount = 0; iCount < dataGridViewSurveyRec.Rows.Count - 1; iCount++)
            //{
            //    if (Convert.ToString(dataGridViewSurveyRec.Rows[iCount].Cells[2].Value) == "REJECT")
            //        dataGridViewSurveyRec.Rows[iCount].DefaultCellStyle.BackColor = Color.Red;
            //    else if (Convert.ToString(dataGridViewSurveyRec.Rows[iCount].Cells[2].Value) == "ACCEPT")
            //        dataGridViewSurveyRec.Rows[iCount].DefaultCellStyle.BackColor = Color.FromArgb(0, 192, 0);
            //    //else
            //      //  dataGridViewSurveyRec.Rows[iCount].DefaultCellStyle.BackColor = Color.White;
            //}
        }

        private void CreateTable()
        {            
            string[] sHeaderArr = { "#", "Created", "Status", "SurveyDepth (" +  GetLengthUnit() + ")", "Inc (°)", "Azm (°)", "MTotal (Gs)", "DipA (°)", "GTotal (g)", "Bx", "By", "Bz", "Gx", "Gy", "Gz" };
            for (int i = 0; i < sHeaderArr.Length; i++)
            {
                DataColumn column = new DataColumn();                
                column.ColumnName = sHeaderArr[i];                
                m_tblRawSurvey.Columns.Add(column);
            }
            m_bIsTableCreated = true;

            dataGridViewSurveyRec.DataSource = m_tblRawSurvey;
            dataGridViewSurveyRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;            
        }

        public void Add(CEventSurvey rec_)
        {            
            m_lstRec.Add(rec_);

            if (!m_bIsTableCreated)
                CreateTable();

            DataRow row = m_tblRawSurvey.NewRow();
            row.SetField((int)COLUMNS.ID, rec_.m_iDatabaseID.ToString());
            row.SetField((int)COLUMNS.CREATED, rec_.rec.dtCreated);            
            row.SetField((int)COLUMNS.STATUS, rec_.rec.Status);
            row.SetField((int)COLUMNS.SVY_DEPTH, rec_.rec.fBitDepth - rec_.rec.fDirToBit);
            row.SetField((int)COLUMNS.INC, rec_.rec.fInclination);
            row.SetField((int)COLUMNS.AZM, rec_.rec.fAzimuth);
            row.SetField((int)COLUMNS.MTOT, rec_.rec.fMTotal);
            row.SetField((int)COLUMNS.DIPA, rec_.rec.fDipAngle);
            row.SetField((int)COLUMNS.GTOT, rec_.rec.fGTotal);
            row.SetField((int)COLUMNS.BX, rec_.rec.fMX);
            row.SetField((int)COLUMNS.BY, rec_.rec.fMY);
            row.SetField((int)COLUMNS.BZ, rec_.rec.fMZ);
            row.SetField((int)COLUMNS.GX, rec_.rec.fAX);
            row.SetField((int)COLUMNS.GY, rec_.rec.fAY);
            row.SetField((int)COLUMNS.GZ, rec_.rec.fAZ);


            m_tblRawSurvey.Rows.Add(row);   
            if (m_tblRawSurvey.Rows.Count == 1)
            {
                dataGridViewSurveyRec.Rows[0].Selected = true;
                m_iRowSelected = 0;
                GetInfoForRow(m_iRowSelected);                
            }
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

        public void Unload()
        {
            m_bUnload = true;
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

        private void buttonEditSurveyDepth_Click(object sender, EventArgs e)
        {
            if (m_iRowSelected < 0)
                MessageBox.Show("No row is selected. Select one first before editing.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                // launch the window showing the bit depth - dir to bit = survey depth
                FormSurveyEditDepth frm = new FormSurveyEditDepth();
                frm.SetUnit(GetLengthUnit());
                frm.SetDepthParameters(m_lstRec[m_iRowSelected].rec.fBitDepth, m_lstRec[m_iRowSelected].rec.fDirToBit, m_fSelectedSurveyDepth);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    float fNewSurveyDepth = frm.GetSurveyDepth();
                    CSurvey.REC recEdit = m_lstRec[m_iRowSelected].rec;
                    recEdit.fSurveyDepth = fNewSurveyDepth;
                    recEdit.fBitDepth = fNewSurveyDepth + recEdit.fDirToBit;
                    m_lstRec[m_iRowSelected].rec = recEdit;

                    m_fSelectedSurveyDepth = fNewSurveyDepth;
                    dataGridViewSurveyRec.Rows[m_iRowSelected].Cells[(int)COLUMNS.SVY_DEPTH].Value = fNewSurveyDepth;                                     
                }
            }
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            
        }

        private void SetButtons(bool bVal_)
        {
            buttonEditSurveyDepth.Enabled = bVal_;
            buttonAccept.Enabled = bVal_;
            buttonReject.Enabled = bVal_;
            buttonAcceptOnly.Enabled = bVal_;
            if (!bVal_)
            {
                buttonAccept.BackColor = Color.Gray;
                buttonReject.BackColor = Color.Gray;
                buttonAcceptOnly.BackColor = Color.Gray;
            }
            else
            {
                if (m_sMSAJobID.Length > 0)
                    buttonAccept.BackColor = Color.FromArgb(0, 192, 0);
                else
                {
                    buttonAccept.BackColor = Color.Gray;
                    buttonAccept.Enabled = false;
                }
                    
                buttonReject.BackColor = Color.Red;
                buttonAcceptOnly.BackColor = Color.FromArgb(0, 192, 0);
            }
        }

        private void SendSurvey(CEventSurvey svy_, bool bAcceptOnly_)
        {
            if (bAcceptOnly_)  // send record to WITS EDR  
            {
                CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                evtSvyWITS.m_sData = GatherWITS(svy_);
                Transmit(this, evtSvyWITS);
            }
            else if (m_sMSAJobID.Length > 0)  // send record to MSA
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

        public void DeleteSurvey(object sender, CEventDeleteSurvey e)
        {
            //Debug.WriteLine("deleted");
            m_msaClient.DeleteSurvey(e.ID);
        }

        public void UpdateSurvey(object sender, CEventUpdateSurvey e)
        {
            m_msaClient.UpdateSurvey(e.ID, e.iBHA, e.rec);
        }

        public void AddSurvey(object sender, CEventUpdateSurvey e)
        {
            m_msaClient.SendQuickSurvey(e.rec);
        }

        public void UpdateRawSurveyList(object sender)
        {

        }

        private void dataGridViewSurveyRec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                try
                {
                    m_iRowSelected = e.RowIndex;
                    GetInfoForRow(m_iRowSelected);
                }
                catch (Exception ex)
                {                    
                    Debug.WriteLine("Error FormSurveyAcceptRejectInfinite::dataGridViewSurveyRec_CellClick: " + ex.Message);
                }

            }
        }

        private void GetInfoForRow(int iRow_)
        {
            m_sStatus = Convert.ToString(dataGridViewSurveyRec.Rows[iRow_].Cells[(int)COLUMNS.STATUS].Value);
            if (m_sStatus == CSurvey.STATUS.ACCEPT.ToString() ||
                m_sStatus == CSurvey.STATUS.REJECT.ToString())
            {
                SetButtons(false);
            }
            else
            {
                SetButtons(true);
            }

            m_fSelectedSurveyDepth = Convert.ToSingle(dataGridViewSurveyRec.Rows[iRow_].Cells[(int)COLUMNS.SVY_DEPTH].Value);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridViewSurveyRec_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            m_iRowSelected = e.RowIndex;
            // launch the window showing the bit depth - dir to bit = survey depth
            FormSurveyEditDepth frm = new FormSurveyEditDepth();
            frm.SetUnit(GetLengthUnit());
            frm.SetDepthParameters(m_lstRec[m_iRowSelected].rec.fBitDepth, m_lstRec[m_iRowSelected].rec.fDirToBit, m_fSelectedSurveyDepth);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                float fNewSurveyDepth = frm.GetSurveyDepth();
                CSurvey.REC recEdit = m_lstRec[m_iRowSelected].rec;
                recEdit.fSurveyDepth = fNewSurveyDepth;
                recEdit.fBitDepth = fNewSurveyDepth + recEdit.fDirToBit;
                m_lstRec[m_iRowSelected].rec = recEdit;

                m_fSelectedSurveyDepth = fNewSurveyDepth;
                dataGridViewSurveyRec.Rows[m_iRowSelected].Cells[(int)COLUMNS.SVY_DEPTH].Value = fNewSurveyDepth;
            }
        }

        private void buttonAcceptOnly_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonAcceptOnly_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_iRowSelected < 0)
                MessageBox.Show("No row is selected. Select one first before accepting.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //else if (m_fLastSurveyDepth >= m_fSelectedSurveyDepth)
            //{
            //    string sMsg = string.Format("Survey depth, {0}, must be greater than or equal to the last value of {1}.", m_fSelectedSurveyDepth, m_fLastSurveyDepth);
            //    MessageBox.Show(sMsg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            else
            {
                dataGridViewSurveyRec.Rows[m_iRowSelected].Cells[(int)COLUMNS.STATUS].Value = CSurvey.STATUS.ACCEPT.ToString();
                dataGridViewSurveyRec.Rows[m_iRowSelected].DefaultCellStyle.BackColor = Color.FromArgb(0, 192, 0);
                m_fLastSurveyDepth = m_fSelectedSurveyDepth;

                CEventSurvey evt = m_lstRec[m_iRowSelected];
                evt.rec.Status = CSurvey.STATUS.ACCEPT;
                evt.rec.fSurveyDepth = m_fLastSurveyDepth;
                evt.rec.fBitDepth = m_fLastSurveyDepth + evt.rec.fDirToBit;
                m_lstRec[m_iRowSelected] = evt;

                CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[m_iRowSelected].rec.fBitDepth);

                SendSurvey(evt, true);

                // send an update survey list
                UpdateRaw(this);

                m_lstRec.Clear();
                m_tblRawSurvey.Clear();
                this.Hide();
            }
        }

        private void buttonAccept_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_iRowSelected < 0)
                MessageBox.Show("No row is selected. Select one first before accepting.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //else if (m_fLastSurveyDepth > m_fSelectedSurveyDepth)
            //{
            //    string sMsg = string.Format("Survey depth, {0}, must be greater than the last value of {1}.", m_fSelectedSurveyDepth, m_fLastSurveyDepth);
            //    MessageBox.Show(sMsg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            else
            {
                dataGridViewSurveyRec.Rows[m_iRowSelected].Cells[(int)COLUMNS.STATUS].Value = CSurvey.STATUS.ACCEPT.ToString();
                dataGridViewSurveyRec.Rows[m_iRowSelected].DefaultCellStyle.BackColor = Color.FromArgb(0, 192, 0);
                m_fLastSurveyDepth = m_fSelectedSurveyDepth;

                CEventSurvey evt = m_lstRec[m_iRowSelected];
                evt.rec.Status = CSurvey.STATUS.ACCEPT;
                evt.rec.fSurveyDepth = m_fLastSurveyDepth;
                evt.rec.fBitDepth = m_fLastSurveyDepth + evt.rec.fDirToBit;
                m_lstRec[m_iRowSelected] = evt;

                CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[m_iRowSelected].rec.fBitDepth);

                SendSurvey(evt, false);

                // send an update survey list
                UpdateRaw(this);

                m_lstRec.Clear();
                m_tblRawSurvey.Clear();
                this.Hide();
            }
        }

        private void buttonClose_MouseClick(object sender, MouseEventArgs e)
        {
            m_lstRec.Clear();
            m_tblRawSurvey.Clear();
            this.Hide();
        }

        private void buttonReject_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_iRowSelected < 0)
                MessageBox.Show("No row is selected. Select one first before rejecting.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                dataGridViewSurveyRec.Rows[m_iRowSelected].Cells[(int)COLUMNS.STATUS].Value = CSurvey.STATUS.REJECT.ToString();
                dataGridViewSurveyRec.Rows[m_iRowSelected].DefaultCellStyle.BackColor = Color.Red;

                CEventSurvey evt = m_lstRec[m_iRowSelected];
                evt.rec.Status = CSurvey.STATUS.REJECT;
                m_lstRec[m_iRowSelected] = evt;

                CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                log.Update(evt.rec.dtCreated, evt.rec.Status.ToString(), m_lstRec[m_iRowSelected].rec.fBitDepth);

                // send an update survey list
                UpdateRaw(this);

                SetButtons(false);
                if (m_lstRec.Count == 1)
                {
                    m_lstRec.Clear();
                    m_tblRawSurvey.Clear();
                    this.Hide();
                }
            }
        }
    }
}
