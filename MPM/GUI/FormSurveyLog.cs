// author: hoan chau
// purpose: show history of surveys

using MPM.Data;
using MPM.DataAcquisition.MultiStationAnalysis;
using MPM.Utilities;
using RoundLAB.eddi.Classes.Survey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.DataAcquisition.Helpers;

namespace MPM.GUI
{
    public partial class FormSurveyLog : Form
    {
        const int DEPTH_COLUMN = 6;
        const int DECIMAL_PRECISION_SHORT = 3;
        const int DECIMAL_PRECISION_LONG = 7;

        const int RAW_SURVEY_DEPTH_COLUMN = 6;
        const int RAW_STATUS_COLUMN = 2;

        private DataTable m_table;

        private int m_iRowSelected;

        CCommonTypes.UNIT_SET m_units;

        // reference values for editing msa
        private int m_iSurveyID;
        private int m_iBHA;
        private float m_fInc;
        private float m_fAzm;
        private float m_fDepth;
        private DateTime m_dtUTC;

        // values for editing raw
        private DateTime m_dtCreated;
        private bool m_bUseNT;

        public delegate void EventHandler(object sender, CEventDeleteSurvey e);
        public event EventHandler DeleteSurvey;

        public delegate void EventUpdate(object sender, CEventUpdateSurvey e);
        public event EventUpdate UpdateSurvey;

        public delegate void EventAdd(object sender, CEventUpdateSurvey e);
        public event EventAdd AddSurvey;

        // flag to indicate if user can edit data
        private bool m_bEditMSA;  
        private bool m_bEditRaw;

        private CMSAHubClient m_MSAHub;

        private DbConnection m_dbCnn;
        private float m_fLastDirToBit;

        private CSurveyLog m_SurveyLog;
        private CDPointLookupTable m_DPointTable;
        CWITSLookupTable m_LookUpWITS;
        CWidgetInfoLookupTable m_WidgetInfoLookup;

        public delegate void EventHandlerWITS(object sender, CEventSendWITSData e);
        public event EventHandlerWITS TransmitWITS;
        
        public FormSurveyLog(ref DbConnection dbCnn_, ref CDPointLookupTable DPointTable_, ref CWITSLookupTable witsLookUpTbl_, ref CWidgetInfoLookupTable widgetInfoLookup_, bool bEditMSA_ = false, bool bEditRaw_ = false)
        {
            InitializeComponent();
            m_bEditMSA = bEditMSA_;
            m_bEditRaw = bEditRaw_;
            m_iSurveyID = -1;
            m_iRowSelected = -1;
            m_dbCnn = dbCnn_;
            m_DPointTable = DPointTable_;
            m_LookUpWITS = witsLookUpTbl_;
            m_WidgetInfoLookup = widgetInfoLookup_;
            m_fLastDirToBit = 0.0f;
        }

        private void SetupEditSurveyGrid()
        {
            dataGridViewLog.MultiSelect = true;

            foreach (DataGridViewColumn dc in dataGridViewLog.Columns)
            {
                if (dc.Index.Equals(DEPTH_COLUMN))  // allow depth edits                
                    dc.ReadOnly = false;                                                    
                else
                    dc.ReadOnly = true;

                int iPositionSquareBracket = dc.Name.IndexOf('[');
                if (iPositionSquareBracket > -1)
                    dc.HeaderText = dc.Name.Substring(0, iPositionSquareBracket);                
            }

            // limit decimal precision
            for (int k = 0; k < dataGridViewLog.Rows.Count; k++)
            {
                for (int j = 0; j < dataGridViewLog.Columns.Count; j++)
                {
                    if (!dataGridViewLog.Columns[j].Name.ToLower().Contains("datetime"))
                    {
                        int iDecimalPosition = dataGridViewLog.Rows[k].Cells[j].Value.ToString().IndexOf('.');
                        if (iDecimalPosition > 0)  // limit to six places after decimal
                        {
                            int iPrecision = DECIMAL_PRECISION_LONG;
                            string sColumnName = dataGridViewLog.Columns[j].Name.ToLower();
                            if (sColumnName.Contains("inc") ||
                                sColumnName.Contains("azim") ||
                                sColumnName.Contains("depth"))
                                iPrecision = DECIMAL_PRECISION_SHORT;

                            // get the length after the decimal position
                            if (dataGridViewLog.Rows[k].Cells[j].Value.ToString().Length > iDecimalPosition + iPrecision)
                                dataGridViewLog.Rows[k].Cells[j].Value = dataGridViewLog.Rows[k].Cells[j].Value.ToString().Substring(0, iDecimalPosition + iPrecision);
                        }
                    }

                }
            }

            try
            {
                this.dataGridViewLog.Columns[0].Visible = false;  // job
                this.dataGridViewLog.Columns[1].Visible = false;  // leg
                this.dataGridViewLog.Columns[3].Visible = false;  // survey id
                //this.dataGridViewLog.Columns[13].Visible = false;  // calculated inc
                //this.dataGridViewLog.Columns[14].Visible = false;  // calculated azm
                this.dataGridViewLog.Columns[15].Visible = false;
                this.dataGridViewLog.Columns[16].Visible = false;
                this.dataGridViewLog.Columns[17].Visible = false;
                this.dataGridViewLog.Columns[18].Visible = false;  
                this.dataGridViewLog.Columns[19].Visible = false;
                // index 20 and 21 are supplied inc and azm, respectively
                for (int i = 22; i < dataGridViewLog.Columns.Count; i++)
                    this.dataGridViewLog.Columns[i].Visible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SetupCorrectedSurveyGrid()
        {          
            if (this.dataGridViewLog.Columns.Count < 1)
            {
                //MessageBox.Show("There are no corrected surveys.", "Corrected Surveys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            this.dataGridViewLog.Columns[0].Visible = false; //leg
            this.dataGridViewLog.Columns[3].Visible = false; //survey id
            this.dataGridViewLog.Columns[4].Visible = false; // correction type
            this.dataGridViewLog.Columns[9].Visible = false; // sstvd
            this.dataGridViewLog.Columns[12].Visible = false; // build rate
            this.dataGridViewLog.Columns[13].Visible = false; // turn rate
            this.dataGridViewLog.Columns[19].Visible = false; // difference

            foreach (DataGridViewColumn dc in dataGridViewLog.Columns)
            {
                int iPositionSquareBracket = dc.Name.IndexOf('[');
                if (iPositionSquareBracket > -1)
                    dc.HeaderText = dc.Name.Substring(0, iPositionSquareBracket);

                if (dc.HeaderText == "DL")
                    dataGridViewLog.Columns[dc.Index].DisplayIndex = 10;
                else if (dc.HeaderText == "DLS")
                    dataGridViewLog.Columns[dc.Index].DisplayIndex = 11;
                else if (dc.HeaderText == "Northing")
                    dataGridViewLog.Columns[dc.Index].DisplayIndex = 14;
                else if (dc.HeaderText == "Easting")
                    dataGridViewLog.Columns[dc.Index].DisplayIndex = 15;
            }

            for (int k = 0; k < dataGridViewLog.Rows.Count; k++)
            {
                for (int j = 0; j < dataGridViewLog.Columns.Count; j++)
                {
                    if (dataGridViewLog.Columns[j].Name.ToLower().Contains("double"))
                    {
                        int iDecimalPosition = dataGridViewLog.Rows[k].Cells[j].Value.ToString().IndexOf('.');
                        if (iDecimalPosition > 0)  // limit to six places after decimal
                        {
                            int iPrecision = DECIMAL_PRECISION_LONG;
                            string sColumnName = dataGridViewLog.Columns[j].Name.ToLower();
                            if (sColumnName.Contains("closure") ||  // closure distance and azimuth
                                sColumnName.Contains("verticalsection[") ||
                                sColumnName.Contains("inc[") ||
                                sColumnName.Contains("azm[") ||
                                sColumnName.Contains("dl[") ||
                                sColumnName.Contains("dls["))
                                iPrecision = DECIMAL_PRECISION_SHORT;
                            // get the length after the decimal position
                            if (dataGridViewLog.Rows[k].Cells[j].Value.ToString().Length > iDecimalPosition + iPrecision)
                                dataGridViewLog.Rows[k].Cells[j].Value = dataGridViewLog.Rows[k].Cells[j].Value.ToString().Substring(0, iDecimalPosition + iPrecision);
                        }
                    }

                }
            }
        }

        private void FormSurveyLog_Load(object sender, EventArgs e)
        {
            dataGridViewLog.DataSource = m_table;
            if (!m_bEditMSA)
            {
                buttonDelete.Hide();
                buttonUpdate.Hide();
                buttonAdd.Hide();                
            }            
            
            if (m_bEditMSA)
            {
                buttonExport.Text = "Export";
                buttonExportSettings.Visible = false;
                buttonRefresh.Enabled = true;
                SetupEditSurveyGrid();                
            }
            else if (this.Text == "Corrected Surveys")
            {
                buttonExport.Text = "Export";
                buttonExportSettings.Visible = false;
                buttonRefresh.Enabled = true;
                SetupCorrectedSurveyGrid();
                dataGridViewLog.MultiSelect = true;
            }
            else  // raw surveys
            {
                buttonExportSettings.Visible = true;
                // get the use nt flag for exporting
                //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
                //WidgetInfoLookupTbl.Load();                
                string sUseNT = m_WidgetInfoLookup.GetValue("FormSurveyExportFilter", "checkBoxUseNT");
                m_bUseNT = sUseNT == "True" ? true: false;

                SetupRawSurveyGrid();

                if (m_bEditRaw)
                    buttonUpdate.Visible = true;

                
                buttonRefresh.Hide();
            }

            dataGridViewLog.AutoResizeColumns();
        }

        public void SetupRawSurveyGrid()
        {
            foreach (DataGridViewColumn dc in dataGridViewLog.Columns)
            {
                if (m_bEditRaw && (dc.Index.Equals(RAW_STATUS_COLUMN) || dc.Index.Equals(RAW_SURVEY_DEPTH_COLUMN)))  // allow edit              
                    dc.ReadOnly = false;
                else
                    dc.ReadOnly = true;

                dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            dataGridViewLog.MultiSelect = true;
        }

        public void SetSurveyLog(ref CSurveyLog log_)
        {
            m_SurveyLog = log_;
        }

        public void SetTitle(string sTitle_)
        {
            this.Text = sTitle_;
        }

        public void SetData(DataTable tbl_)
        {
            m_table = tbl_;
            dataGridViewLog.DataSource = m_table;
        }

        public void SetLastDirToBit(float fVal_)
        {
            m_fLastDirToBit = fVal_;
        }

        public void SetMSAHub(ref CMSAHubClient hub_)
        {
            m_MSAHub = hub_;
            //svyDetailsCollection = m_MSAHub.GetSensorDetailsCollection();
            //m_bOrderByDesc = true;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (dataGridViewLog.Columns.Count < 1)
            {
                MessageBox.Show("Nothing to export.", "Corrected Surveys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // open a file
            string sUserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string sFileName = sUserDesktop + "\\" + this.Text + " Log.csv";
            StreamWriter sw = new StreamWriter(sFileName);

            if (m_bEditMSA || this.Text == "Corrected Surveys")
            {                                 
                // write out the column header line
                string sHeader = "";
                for (int j = 0; j < dataGridViewLog.Columns.Count; j++)
                    sHeader += dataGridViewLog.Columns[j].HeaderText + ",";

                sHeader = sHeader.Substring(0, sHeader.Length - 1);
                sw.WriteLine(sHeader);

                // loop through everything that's shown on the grid
                for (int i = 0; i < dataGridViewLog.Rows.Count; i++)
                {
                    string sOutput = "";
                    for (int j = 0; j < dataGridViewLog.Columns.Count; j++)
                        sOutput += dataGridViewLog[j, i].Value + ",";

                    sOutput = sOutput.Substring(0, sOutput.Length - 1);
                    sOutput = sOutput.Replace("°", "degrees");
                    sw.WriteLine(sOutput);
                }
                
                Process.Start("Notepad", sFileName);
            }
            else  // show the filter window                
            {                
                FormSurveyExportFilter frm = new FormSurveyExportFilter(ref m_WidgetInfoLookup);                                
                float fLastSurveyDepth = m_SurveyLog.GetLastSurveyDepth();
                frm.SetDepths(1, fLastSurveyDepth);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    m_bUseNT = frm.IsNT();
                    // get unit of length for bit depth
                    CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get((int)Command.COMMAND_BIT_DEPTH);     // bit depth

                    // get the filters
                    float fStart, fEnd;
                    frm.GetDepths(out fStart, out fEnd);
                    DataTable dt = m_SurveyLog.Get(frm.IsAll(), fStart, fEnd, frm.IsNT());
                    dataGridViewTemp.DataSource = dt;

                    // write out the column header line
                    string sHeader = "";
                    //for (int j = 0; j < dataGridViewTemp.Columns.Count; j++)
                    sHeader += dataGridViewTemp.Columns[1].HeaderText + "(" + dpi.sUnits + "),";
                    sHeader += dataGridViewTemp.Columns[2].HeaderText + "(NONE),";
                    sHeader += dataGridViewTemp.Columns[3].HeaderText + "(deg),";
                    sHeader += dataGridViewTemp.Columns[4].HeaderText + "(deg),";
                    sHeader += dataGridViewTemp.Columns[5].HeaderText + "(g),";
                    sHeader += dataGridViewTemp.Columns[6].HeaderText + "(g),";
                    sHeader += dataGridViewTemp.Columns[7].HeaderText + "(g),";
                    if (m_bUseNT)
                    {
                        sHeader += dataGridViewTemp.Columns[8].HeaderText + "(nT)"  + ",";
                        sHeader += dataGridViewTemp.Columns[9].HeaderText + "(nT)"  + ",";
                        sHeader += dataGridViewTemp.Columns[10].HeaderText + "(nT)"  + ",";
                        sHeader += dataGridViewTemp.Columns[11].HeaderText + "(nT)" + ",";
                    }
                    else
                    {
                        sHeader += dataGridViewTemp.Columns[8].HeaderText + "(Gs)" + ",";
                        sHeader += dataGridViewTemp.Columns[9].HeaderText + "(Gs)" + ",";
                        sHeader += dataGridViewTemp.Columns[10].HeaderText + "(Gs)" + ",";
                        sHeader += dataGridViewTemp.Columns[11].HeaderText + "(Gs)" + ",";
                    }
                    
                    sHeader += dataGridViewTemp.Columns[12].HeaderText + "(g),";
                    sHeader += dataGridViewTemp.Columns[13].HeaderText + "(deg),";  // dip angle
                    sHeader += dataGridViewTemp.Columns[14].HeaderText + "(" + dpi.sUnits + "),";  // course length
                    sHeader += dataGridViewTemp.Columns[15].HeaderText + "(" + dpi.sUnits + "),";  // tvd
                    sHeader += dataGridViewTemp.Columns[16].HeaderText + "(" + dpi.sUnits + "),";  // ns
                    sHeader += dataGridViewTemp.Columns[17].HeaderText + "(" + dpi.sUnits + "),";  // ew
                    string sDLSUnits = "deg/" + (dpi.sUnits == "ft" ? "30ft" : "100m");
                    sHeader += dataGridViewTemp.Columns[18].HeaderText + "(" + sDLSUnits + ")";  // dls

                    sHeader = sHeader.Substring(0, sHeader.Length);
                    sw.WriteLine(sHeader);

                    // loop through everything that's shown on the grid
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sOutput = "";
                        // skip the bit depth and go to survey depth
                        for (int j = 1; j < dataGridViewTemp.Columns.Count; j++)
                            sOutput += dataGridViewTemp[j, i].Value + ",";

                        sOutput = sOutput.Substring(0, sOutput.Length - 1);
                        sOutput = sOutput.Replace("°", "degrees");
                        sw.WriteLine(sOutput);
                    }

                    Process.Start("Notepad", sFileName);
                }
            }

            sw.Close();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (m_bEditMSA)
            {
                if (dataGridViewLog.SelectedRows.Count > 1)
                {
                    MessageBox.Show("You can't update more than one row at a time. Select one row and try again.", "Update Survey", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (m_iSurveyID > -1)
                {                    
                    DialogResult res = MessageBox.Show("Are you sure you want to update this survey?", "Update Survey", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        // hour glass

                        EnableEditControls(false);

                        CEventUpdateSurvey evtUpdateSvy = new CEventUpdateSurvey();

                        // send message to the msa client
                        evtUpdateSvy.ID = m_iSurveyID;
                        evtUpdateSvy.iBHA = m_iBHA;
                        evtUpdateSvy.rec.fSurveyDepth = System.Convert.ToSingle(dataGridViewLog.Rows[m_iRowSelected].Cells[DEPTH_COLUMN].Value);
                        //evtUpdateSvy.rec.fInclination = m_fInc;
                        //evtUpdateSvy.rec.fAzimuth = m_fAzm;
                        evtUpdateSvy.rec.dtCreated = m_dtUTC;
                        UpdateSurvey(this, evtUpdateSvy);

                        // wait for confirmation
                    }                    
                }
            }
            else if (m_bEditRaw)
            {
                if (dataGridViewLog.SelectedRows.Count < 1)
                {
                    MessageBox.Show("You haven't selected a row to update. Select a row and try again.", "Export Selected Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (dataGridViewLog.SelectedRows.Count > 1)
                {
                    MessageBox.Show("You can't update more than one row at a time. Select one row and try again.", "Update Survey", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (m_iRowSelected > -1)
                {
                    string sStatus = dataGridViewLog.Rows[m_iRowSelected].Cells[RAW_STATUS_COLUMN].Value.ToString();
                    if (sStatus != CSurvey.STATUS.ACCEPT.ToString() &&
                        sStatus != CSurvey.STATUS.REJECT.ToString() &&
                        sStatus != CSurvey.STATUS.NONE.ToString())
                    {
                        MessageBox.Show("Status must be one of the following: " + CSurvey.STATUS.ACCEPT.ToString() + ", " + CSurvey.STATUS.REJECT.ToString() + ", or " + CSurvey.STATUS.NONE.ToString() + ".", "Update Survey", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    DialogResult res = MessageBox.Show("Are you sure you want to update this survey?", "Update Survey", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        float fDirToBit = 0.0f;
                        // get the bit depth and the dirtobit value based on the date time
                        float fBitDepth = m_SurveyLog.GetDirToBitLength(m_dtCreated, out fDirToBit);
                        
                        // get the status and the survey depth
                        float fSurveyDepth = System.Convert.ToSingle(dataGridViewLog.Rows[m_iRowSelected].Cells[RAW_SURVEY_DEPTH_COLUMN].Value);
                        sStatus = dataGridViewLog.Rows[m_iRowSelected].Cells[RAW_STATUS_COLUMN].Value.ToString();
                        // recalculate the bit depth    
                        fBitDepth = fDirToBit + fSurveyDepth;
                        // set the values
                        Cursor.Current = Cursors.WaitCursor;
                        bool bSuccess = m_SurveyLog.Update(m_dtCreated, sStatus, fBitDepth);
                        if (bSuccess)  // set the bit depth to the proper value
                        {
                            dataGridViewLog.Rows[m_iRowSelected].Cells[RAW_SURVEY_DEPTH_COLUMN - 1].Value = fBitDepth;

                            m_SurveyLog.SetLengthUnit(GetLengthUnit());
                            // update all the tvd's
                            m_SurveyLog.RecalculateTVD();

                            // refresh the grid
                            dataGridViewLog.DataSource = null;
                            m_table = m_SurveyLog.Get();
                            dataGridViewLog.DataSource = m_table;
                            SetupRawSurveyGrid();
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Record updated successfully.", "Update Survey", MessageBoxButtons.OK, MessageBoxIcon.Information);                            
                        }                            
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Failed to update record.", "Update Survey", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                                                    
                        // add audit trail
                    }
                }
            }
            
        }

        public void OnSurveyDeleted(object sender, CEventDeleteSurvey ev)
        {
            if (ev.ID > 0)  // success and value is an echo of the survey id
            {
                if (ev.ID == Convert.ToInt16(dataGridViewLog.Rows[m_iRowSelected].Cells[3].Value))
                {
                    dataGridViewLog.Rows.RemoveAt(m_iRowSelected);
                }
            }

            EnableEditControls(true);
        }

        public void OnSurveyUpdated(object sender, CEventUpdateSurvey ev)
        {
            EnableEditControls(true);
        }

        private void EnableEditControls(bool bVal_)
        {
            dataGridViewLog.Enabled = bVal_;
            buttonDelete.Enabled = bVal_;
            buttonUpdate.Enabled = bVal_;
            buttonRefresh.Enabled = bVal_;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (m_iSurveyID > -1)
            {
                DialogResult res = MessageBox.Show("Are you sure you want to delete this survey?", "Delete Survey", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    // hour glass
                    
                    
                    EnableEditControls(false);                    

                    // send message to the msa client
                    CEventDeleteSurvey evtDeleteSvy = new CEventDeleteSurvey();
                    evtDeleteSvy.ID = m_iSurveyID;
                    DeleteSurvey(this, evtDeleteSvy);

                    // wait for confirmation                    
                }
            }
        }

        private void dataGridViewLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                try
                {
                    if (m_bEditMSA)
                    {                        
                        m_iRowSelected = e.RowIndex;
                        m_iBHA = Convert.ToInt16(dataGridViewLog.Rows[e.RowIndex].Cells[2].Value);
                        m_iSurveyID = Convert.ToInt16(dataGridViewLog.Rows[e.RowIndex].Cells[3].Value);
                        m_dtUTC = Convert.ToDateTime(dataGridViewLog.Rows[e.RowIndex].Cells[5].Value);
                        m_fDepth = Convert.ToSingle(dataGridViewLog.Rows[e.RowIndex].Cells[6].Value);
                        m_fInc = Convert.ToSingle(dataGridViewLog.Rows[e.RowIndex].Cells[18].Value);
                        m_fAzm = Convert.ToSingle(dataGridViewLog.Rows[e.RowIndex].Cells[19].Value);
                    }
                    else if (m_bEditRaw)
                    {
                        m_iRowSelected = e.RowIndex;
                        m_dtCreated = Convert.ToDateTime(dataGridViewLog.Rows[e.RowIndex].Cells[1].Value);
                    }
                }
                catch (Exception ex)
                {
                    m_fInc = CCommonTypes.BAD_VALUE;
                    m_fAzm = CCommonTypes.BAD_VALUE;
                    Debug.WriteLine("Error FormSurveyLog::dataGridViewLog_CellClick: " + ex.Message);
                }
                
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            DataTable tbl;
            this.dataGridViewLog.DataSource = null;
            this.dataGridViewLog.Rows.Clear();
            this.dataGridViewLog.Refresh();
            if (m_bEditMSA)
            {                
                tbl = m_MSAHub.GetSensorDetailsTable();                  
                this.dataGridViewLog.DataSource = tbl;
                SetupEditSurveyGrid();
            }                
            else if (this.Text == "Corrected Surveys")
            {
                tbl = m_MSAHub.GetCorrectedTable();                
                this.dataGridViewLog.DataSource = tbl;
                SetupCorrectedSurveyGrid();
            }                

        }

        private void dataGridViewLog_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show("hi");
            if (m_bEditMSA)
            {
                
                //if (e.ColumnIndex == DEPTH_COLUMN)
                //{
                //    DataTable tbl = m_MSAHub.GetSensorDetailsTable();
                //    //DataTable dt2 = new DataTable();

                //    // Create temporary list to sort rows by int value.
                //    List<DataRow> rows = new List<DataRow>();
                //    foreach (DataRow row in tbl.Rows)
                //    {
                //        rows.Add(row);
                //    }

                //    // Sort list in ascending order
                //    rows.Sort(delegate (DataRow row1, DataRow row2)
                //    {
                //        return (System.Convert.ToSingle(row1["SurveyDepth"]))
                //                 .CompareTo((System.Convert.ToSingle(row2["SurveyDepth"])));
                //    });

                //    tbl.Clear();
                //    // Add sorted rows back to datatable.
                    
                //    foreach (DataRow row in rows)
                //    {                       
                //        tbl.Rows.Add(row.ItemArray);
                //    }



                //    dataGridViewLog.DataSource = tbl;
                    //dataGridViewLog.DataBind();

                    //DataRow[] foundRows = tbl.Select("", "SurveyDepth ASC");
                    //DataTable dt = foundRows.CopyToDataTable();

                    //this.dataGridViewLog.DataSource = null;
                    //this.dataGridViewLog.Rows.Clear();
                    //this.dataGridViewLog.Refresh();
                    //this.dataGridViewLog.DataSource = dt;
                    //m_bOrderByDesc = !m_bOrderByDesc;
                    //SetupEditSurveyGrid();
                    //return;

                    //DataView dv = tbl.DefaultView;



                    //if (m_bOrderByDesc)
                    //    dv.Sort = "SurveyDepth desc";
                    //else
                    //    dv.Sort = "SurveyDepth asc";
                    //DataTable sortedDT = dv.ToTable();

                    //this.dataGridViewLog.DataSource = null;
                    //this.dataGridViewLog.Rows.Clear();
                    //this.dataGridViewLog.Refresh();

                    //this.dataGridViewLog.DataSource = sortedDT;
                    //m_bOrderByDesc = !m_bOrderByDesc;
                    //SetupEditSurveyGrid();
                    //if (m_bOrderByDesc)                    
                    //    svyDetailsCollection = svyDetailsCollection.OrderBy(item => item.SurveyDepth);                    
                    //else
                    //    svyDetailsCollection = svyDetailsCollection.OrderByDescending(item => item.SurveyDepth);                    

               // }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormSurveyAdd frm = new FormSurveyAdd();
            // get the last depth
            float fLastDepth = CCommonTypes.BAD_VALUE;
            //
            if (dataGridViewLog.Rows.Count > 0)
            {
                fLastDepth = System.Convert.ToSingle(dataGridViewLog.Rows[dataGridViewLog.Rows.Count - 1].Cells[DEPTH_COLUMN].Value);
            }
            frm.SetLastSurveyDepth(fLastDepth);

            if (frm.ShowDialog(this) == DialogResult.OK)
            {                                
                // save entire record to database
                CSurvey.REC rec = frm.GetRec();
                rec.fBitDepth = m_fLastDirToBit + rec.fSurveyDepth;
                CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                log.Save(rec);
                CSurvey.REC_CALC rekCalc = log.Calculate(rec);
                log.SaveCalc(rekCalc);

                // send to msa server
                CEventUpdateSurvey evtAddSvy = new CEventUpdateSurvey();                              
                evtAddSvy.rec = rec;
                AddSurvey(this, evtAddSvy);
            }
        }
        

        private void buttonExportSelected_Click(object sender, EventArgs e)
        {
            if (dataGridViewLog.SelectedRows.Count < 1)
            {
                MessageBox.Show("There are no highlighted rows.", "Export Selected Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // open a file
            string sUserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string sFileName = sUserDesktop + "\\" + this.Text + " Log.csv";
            StreamWriter sw = new StreamWriter(sFileName);

            if (m_bEditMSA || this.Text == "Corrected Surveys")
            {
                // write out the column header line
                string sHeader = "";
                for (int j = 0; j < dataGridViewLog.Columns.Count; j++)
                    sHeader += dataGridViewLog.Columns[j].HeaderText + ",";

                sHeader = sHeader.Substring(0, sHeader.Length - 1);
                sw.WriteLine(sHeader);

                // loop through everything that's shown on the grid
                for (int i = 0; i < dataGridViewLog.Rows.Count; i++)
                {
                    if (dataGridViewLog.Rows[i].Selected)
                    {
                        string sOutput = "";
                        for (int j = 0; j < dataGridViewLog.Columns.Count; j++)
                            sOutput += dataGridViewLog[j, i].Value + ",";

                        sOutput = sOutput.Substring(0, sOutput.Length - 1);
                        sOutput = sOutput.Replace("°", "degrees");
                        sw.WriteLine(sOutput);
                    }
                        
                }

                sw.Close();
                Process.Start("Notepad", sFileName);
            }
            else
            {
                string sHeader = "";
                string sLenUnit = GetLengthUnit();
                string sDLSUnit = sLenUnit.ToLower() == "ft" ? "deg/30ft" : "deg/100m"; 
                if (m_bUseNT)
                    sHeader = "SurveyDepth(" + sLenUnit + "),TimeDate(NONE),inc(deg),azm(deg),ax(g),ay(g),az(g),mx(nT),my(nT),mz(nT),mTotal(nT),gTotal(g),dipAngle(deg),cl(" + sLenUnit + "),tvd(" + sLenUnit + "),ns(" + sLenUnit + "),ew(" + sLenUnit + "),dls(" + sDLSUnit + ")";
                else
                    sHeader = "SurveyDepth(" + sLenUnit + "),TimeDate(NONE),inc(deg),azm(deg),ax(g),ay(g),az(g),mx(Gs),my(Gs),mz(Gs),mTotal(Gs),gTotal(g),dipAngle(deg),cl(" + sLenUnit + "),tvd(" + sLenUnit + "),ns(" + sLenUnit + "),ew(" + sLenUnit + "),dls(" + sDLSUnit + ")";
                sw.WriteLine(sHeader);

                // loop through all the rows that have been highlighted
                for (int i = 0; i < dataGridViewLog.Rows.Count; i++)
                {
                    if (dataGridViewLog.Rows[i].Selected)
                    {
                        string sRowData = "";
                        sRowData += dataGridViewLog.Rows[i].Cells[6].Value.ToString() + ",";
                        sRowData += dataGridViewLog.Rows[i].Cells[1].Value.ToString() + ",";
                        for (int j = 7; j < dataGridViewLog.Columns.Count; j++)
                        {
                            if (dataGridViewLog.Columns[j].Name.Substring(0, 2).ToLower() == "mx" ||
                                dataGridViewLog.Columns[j].Name.Substring(0, 2).ToLower() == "my" ||
                                dataGridViewLog.Columns[j].Name.Substring(0, 2).ToLower() == "mz" ||
                                dataGridViewLog.Columns[j].Name.Substring(0, 2).ToLower() == "mt")
                            {
                                if (m_bUseNT)
                                {
                                    float f = System.Convert.ToSingle(dataGridViewLog.Rows[i].Cells[j].Value) * 100000;
                                    sRowData += f.ToString() + ",";
                                }                                    
                                else
                                    sRowData += dataGridViewLog.Rows[i].Cells[j].Value.ToString() + ",";
                            }
                            else
                                sRowData += dataGridViewLog.Rows[i].Cells[j].Value.ToString() + ",";
                        }
                        sRowData = sRowData.Substring(0, sRowData.Length - 1);  //remove ending comma
                        sw.WriteLine(sRowData);
                    }
                }

                sw.Close();
                Process.Start("Notepad", sFileName);
            }
                                                  
        }

        private void acceptedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable tbl = m_SurveyLog.Get("ACCEPT");
            dataGridViewLog.DataSource = null;
            dataGridViewLog.DataSource = tbl;
        }

        private void rejectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable tbl = m_SurveyLog.Get("REJECT");
            dataGridViewLog.DataSource = null;
            dataGridViewLog.DataSource = tbl;
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable tbl = m_SurveyLog.Get("NONE");
            dataGridViewLog.DataSource = null;
            dataGridViewLog.DataSource = tbl;
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable tbl = m_SurveyLog.Get();
            dataGridViewLog.DataSource = null;
            dataGridViewLog.DataSource = tbl;
        }

        private void dataGridViewLog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (m_bEditRaw)
            {
                if (e.ColumnIndex != RAW_STATUS_COLUMN)
                {
                    FormSurveyEditDepth frm = new FormSurveyEditDepth();
                    frm.SetUnit(GetLengthUnit());
                    float fBitDepth = System.Convert.ToSingle(dataGridViewLog.Rows[e.RowIndex].Cells[RAW_SURVEY_DEPTH_COLUMN - 1].Value);
                    float fSurveyDepth = System.Convert.ToSingle(dataGridViewLog.Rows[e.RowIndex].Cells[RAW_SURVEY_DEPTH_COLUMN].Value);
                    float fDirToBit = fBitDepth - fSurveyDepth;
                    frm.SetDepthParameters(fBitDepth, fDirToBit, fSurveyDepth);
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        float fNewSurveyDepth = frm.GetSurveyDepth();
                        dataGridViewLog.Rows[e.RowIndex].Cells[RAW_SURVEY_DEPTH_COLUMN].Value = fNewSurveyDepth;
                        dataGridViewLog.Rows[e.RowIndex].Cells[RAW_SURVEY_DEPTH_COLUMN - 1].Value = fNewSurveyDepth + fDirToBit;
                        buttonUpdate.Focus();
                    }
                }
                else if (e.ColumnIndex == RAW_STATUS_COLUMN)
                {
                    FormSurveyEditStatus frm = new FormSurveyEditStatus();
                    string s = dataGridViewLog.Rows[e.RowIndex].Cells[RAW_STATUS_COLUMN].Value.ToString();
                    frm.Init(s);
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        dataGridViewLog.Rows[e.RowIndex].Cells[RAW_STATUS_COLUMN].Value = frm.GetStatus();
                        buttonUpdate.Focus();
                    }
                }                
            }
            else if (m_bEditMSA)
            {
                FormSurveyEditDepth frm = new FormSurveyEditDepth();
                frm.SetUnit(GetLengthUnit());
                float fSurveyDepth = System.Convert.ToSingle(dataGridViewLog.Rows[e.RowIndex].Cells[DEPTH_COLUMN].Value);
                float fBitDepth = fSurveyDepth + m_fLastDirToBit;
                frm.SetDepthParameters(fBitDepth, m_fLastDirToBit, fSurveyDepth);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    float fNewSurveyDepth = frm.GetSurveyDepth();
                    dataGridViewLog.Rows[e.RowIndex].Cells[DEPTH_COLUMN].Value = fNewSurveyDepth;
                }
            }
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET unitSetNew_)
        {            
            m_units = unitSetNew_;            
        }


        private string GetNativeLengthUnit()
        {            
            CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get((int)Command.COMMAND_BIT_DEPTH);
            return dpi.sUnits;
        }

        private string GetLengthUnit()
        {
            string sLengthUnit = "ft";
            CUnitLength unitLength = new CUnitLength();
            if (m_units == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                sLengthUnit = unitLength.GetImperialUnitDesc();
            else if (m_units == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                sLengthUnit = unitLength.GetMetricUnitDesc();
            else  // get the value unit from the specific d-point                            
                sLengthUnit = GetNativeLengthUnit();

            return sLengthUnit;
        }

        private void buttonExportSettings_Click(object sender, EventArgs e)
        {            
            FormSurveyExportFilter frm = new FormSurveyExportFilter(ref m_WidgetInfoLookup);
            float fLastSurveyDepth = m_SurveyLog.GetLastSurveyDepth();
            frm.SetDepths(1, fLastSurveyDepth);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_bUseNT = frm.IsNT();
            }                                    
        }

        private void buttonResendWITS_Click(object sender, EventArgs e)
        {            
            if (dataGridViewLog.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must click on at least one record before sending to WITS.", "WITS Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                CEventSurvey evtSvy = new CEventSurvey();
                for (int i = 0; i < dataGridViewLog.SelectedRows.Count; i++)
                {
                    if (!m_bEditMSA && !m_bEditRaw && this.Text == "Corrected Surveys")
                    {
                        evtSvy.rec.fSurveyDepth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[5].Value);
                        evtSvy.rec.fInclination = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[6].Value);
                        evtSvy.rec.fAzimuth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[7].Value);
                        evtSvy.rec.fGTotal = CCommonTypes.BAD_VALUE;
                        evtSvy.rec.fMTotal = CCommonTypes.BAD_VALUE;
                        evtSvy.rec.fDipAngle = CCommonTypes.BAD_VALUE;
                        // send to edr
                        CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                        evtSvyWITS.m_sData = GatherWITS(evtSvy);
                        TransmitWITS(this, evtSvyWITS);
                    }
                    else if (m_bEditMSA)
                    {
                        evtSvy.rec.fSurveyDepth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[6].Value);
                        evtSvy.rec.fInclination = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[20].Value);
                        evtSvy.rec.fAzimuth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[21].Value);
                        evtSvy.rec.fGTotal = CCommonTypes.BAD_VALUE;
                        evtSvy.rec.fMTotal = CCommonTypes.BAD_VALUE;
                        evtSvy.rec.fDipAngle = CCommonTypes.BAD_VALUE;
                        // send to edr
                        CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                        evtSvyWITS.m_sData = GatherWITS(evtSvy);
                        TransmitWITS(this, evtSvyWITS);
                    }
                    else if (m_bEditRaw)
                    {
                        string sStatus = dataGridViewLog.SelectedRows[i].Cells[2].Value.ToString();
                        if (sStatus == "ACCEPT")
                        {
                            evtSvy.rec.fSurveyDepth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[6].Value);
                            evtSvy.rec.fInclination = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[7].Value);
                            evtSvy.rec.fAzimuth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[8].Value);
                            evtSvy.rec.fMTotal = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[15].Value);
                            evtSvy.rec.fGTotal = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[16].Value);
                            evtSvy.rec.fDipAngle = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[17].Value);
                            // send to edr
                            CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                            evtSvyWITS.m_sData = GatherWITS(evtSvy);
                            TransmitWITS(this, evtSvyWITS);
                        }
                        else
                        {
                            MessageBox.Show("You can't send a survey that does not have 'ACCEPT' status.", "WITS Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        string sStatus = dataGridViewLog.SelectedRows[i].Cells[2].Value.ToString();
                        if (sStatus == "ACCEPT")
                        {
                            evtSvy.rec.fSurveyDepth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[6].Value);
                            evtSvy.rec.fInclination = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[7].Value);
                            evtSvy.rec.fAzimuth = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[8].Value);
                            evtSvy.rec.fMTotal = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[15].Value);
                            evtSvy.rec.fGTotal = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[16].Value);
                            evtSvy.rec.fDipAngle = Convert.ToSingle(dataGridViewLog.SelectedRows[i].Cells[17].Value);
                            // send to edr
                            CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                            evtSvyWITS.m_sData = GatherWITS(evtSvy);
                            TransmitWITS(this, evtSvyWITS);
                        }
                        else
                        {
                            MessageBox.Show("You can't send a survey that does not have 'ACCEPT' status.", "WITS Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
                                      
        }

        private string GatherWITS(CEventSurvey recSvy_)
        {
            CWITSRecordSurvey rec = new CWITSRecordSurvey();
            string sIncWITSID, sAzmWITSID, sDepthWITSID, sGTotWITSID, sMTotWITSID, sDipWITSID;

            //************************************
            // survey channels
            //************************************
            sDepthWITSID = m_LookUpWITS.Find2(10708);
            sIncWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_INCLINATION);
            sAzmWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_AZIMUTH);

            rec.AddChannel(sDepthWITSID);
            rec.AddChannel(sIncWITSID);
            rec.AddChannel(sAzmWITSID);

            rec.SetValue(sDepthWITSID, (float)recSvy_.rec.fSurveyDepth);
            rec.SetValue(sIncWITSID, (float)recSvy_.rec.fInclination);
            rec.SetValue(sAzmWITSID, (float)recSvy_.rec.fAzimuth);

            //************************************
            // append qualifiers if they exist
            //************************************
            if (recSvy_.rec.fGTotal > CCommonTypes.BAD_VALUE)
            {
                sGTotWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GT);
                sMTotWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BT);
                sDipWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_DIPANGLE);

                rec.AddChannel(sMTotWITSID);
                rec.AddChannel(sGTotWITSID);
                rec.AddChannel(sDipWITSID);

                rec.SetValue(sMTotWITSID, (float)recSvy_.rec.fMTotal);
                rec.SetValue(sGTotWITSID, (float)recSvy_.rec.fGTotal);
                rec.SetValue(sDipWITSID, (float)recSvy_.rec.fDipAngle);
            }

            string sRetVal = rec.GatherData();
            sRetVal = "&&\r\n" + sRetVal + "!!\r\n";  // prepend and append wits flags
            return sRetVal;
        }

        private void FormSurveyLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void UpdateList(object sender)
        {
            if (m_SurveyLog != null)
            {
                DataTable tbl = m_SurveyLog.Get();
                SetData(tbl);
            }            
        }
    }
}
