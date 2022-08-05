using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace License
{
    public partial class FormLicenseRecords : Form
    {
        private const int ACTIVE_COL_NUM = 5;
        private int m_iRowIndex = -1;
        
        public FormLicenseRecords()
        {
            InitializeComponent();
        }

        private void FormLicenseRecords_Load(object sender, EventArgs e)
        {
            CLicenseDatabase db = new CLicenseDatabase();
            dataGridViewLog.DataSource = db.Get();   
            for (int i = 0; i < dataGridViewLog.Rows.Count; i++)
            {
                if (dataGridViewLog.Rows[i].Cells[ACTIVE_COL_NUM].Value.ToString() == "0")
                    dataGridViewLog.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Copy" && dataGridViewLog.CurrentCell.Value != null)
            {
                Clipboard.SetDataObject(dataGridViewLog.CurrentCell.Value.ToString(), false);
            }
        }

        private void EditRecord(int iIndex_)
        {
            FormEditRecord frmEditRec = new FormEditRecord();
            CLicenseRecord rec = new CLicenseRecord();
            rec.iDate = System.Convert.ToInt32(dataGridViewLog.Rows[iIndex_].Cells[1].Value.ToString().Replace("-", ""));
            rec.iTime = System.Convert.ToInt32(dataGridViewLog.Rows[iIndex_].Cells[2].Value.ToString().Replace(":", ""));

            if (dataGridViewLog.Rows[iIndex_].Cells[3].Value.ToString().Length > 0)
                rec.iRemovedDate = System.Convert.ToInt32(dataGridViewLog.Rows[iIndex_].Cells[3].Value.ToString().Replace("-", ""));

            if (dataGridViewLog.Rows[iIndex_].Cells[4].Value.ToString().Length > 0)
                rec.iRemovedTime = System.Convert.ToInt32(dataGridViewLog.Rows[iIndex_].Cells[4].Value.ToString().Replace(":", ""));

            rec.iActive = System.Convert.ToInt32(dataGridViewLog.Rows[iIndex_].Cells[ACTIVE_COL_NUM].Value);
            rec.sComputer = dataGridViewLog.Rows[iIndex_].Cells[6].Value.ToString();
            rec.sUser = dataGridViewLog.Rows[iIndex_].Cells[7].Value.ToString();
            rec.sOrganization = dataGridViewLog.Rows[iIndex_].Cells[8].Value.ToString();
            rec.sMACAddress = dataGridViewLog.Rows[iIndex_].Cells[9].Value.ToString();
            rec.sLicense = dataGridViewLog.Rows[iIndex_].Cells[10].Value.ToString();
            frmEditRec.Init(rec);
            if (frmEditRec.ShowDialog() == DialogResult.OK)
            {
                rec = frmEditRec.GetRecord();
                
                CLicenseDatabase db = new CLicenseDatabase();
                if (db.Update(rec))
                {
                    string sDate = rec.iRemovedDate.ToString(), sTime = rec.iRemovedTime.ToString();
                    //sDate = sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2);
                    //if (sTime.Length < 6)
                    //    sTime = frmEditRec.GetTime();
                    //sTime = sTime.Substring(0, 2) + ":" + sTime.Substring(2, 2) + ":" + sTime.Substring(4, 2);
                    dataGridViewLog.Rows[iIndex_].Cells[3].Value = sDate;
                    dataGridViewLog.Rows[iIndex_].Cells[4].Value = sTime;
                    dataGridViewLog.Rows[iIndex_].Cells[ACTIVE_COL_NUM].Value = rec.iActive.ToString();
                    dataGridViewLog.Rows[iIndex_].Cells[6].Value = rec.sComputer;
                    if (rec.iActive.ToString() == "0")
                        dataGridViewLog.Rows[iIndex_].DefaultCellStyle.BackColor = Color.Red;
                    else
                    {
                        if (iIndex_ % 2 == 0)
                            dataGridViewLog.Rows[iIndex_].DefaultCellStyle.BackColor = dataGridViewLog.RowsDefaultCellStyle.BackColor;
                        else
                            dataGridViewLog.Rows[iIndex_].DefaultCellStyle.BackColor = dataGridViewLog.AlternatingRowsDefaultCellStyle.BackColor;
                    }
                        

                    MessageBox.Show("Succesfully updated record!", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                    
                else
                    MessageBox.Show("Failed record update", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
           

        private void dataGridViewLog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            m_iRowIndex = e.RowIndex;
            EditRecord(m_iRowIndex);            
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditRecord(m_iRowIndex);
        }

        private void dataGridViewLog_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            m_iRowIndex = e.RowIndex;
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            CLicenseDatabase db = new CLicenseDatabase();
            db.Report();         
        }

        private void dataGridViewLog_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridViewLog.Rows.Count; i++)
            {
                if (dataGridViewLog.Rows[i].Cells[ACTIVE_COL_NUM].Value.ToString() == "0")
                    dataGridViewLog.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }
    }
}
