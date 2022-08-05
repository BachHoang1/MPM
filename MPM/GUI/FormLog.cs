// author: hoan chau
// purpose: display logged data for sorting, filtering, and exporting

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MPM.GUI
{
    public partial class FormLog : Form
    {
        private const int MESSAGE_CODE_INDEX = 3;

        private DataTable m_table;
        private int m_iMessageCode;        
        
        public FormLog()
        {
            InitializeComponent();
            m_table = new DataTable();            
        }

        private void FormLog_Load(object sender, EventArgs e)
        {
            this.dataGridViewLog.DataSource = m_table;            
        }

        public void SetData(DataTable tbl_)
        {
            m_table = tbl_;
        }        

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (filterToolStripMenuItem.Text == "Filter")
            {
                // find all the unique message codes that were selected, if any
                List<int> lstSelection = new List<int>();
                for (int i = 0; i <  dataGridViewLog.SelectedRows.Count; i++)
                {
                    bool bExists = false;
                    int iMsgCode = Convert.ToInt16(dataGridViewLog.SelectedRows[i].Cells[MESSAGE_CODE_INDEX].Value);
                    for (int j = 0; j < lstSelection.Count; j++)
                    {
                        if (iMsgCode == lstSelection[j])
                        {
                            bExists = true;
                            break;
                        }
                    }

                    if (!bExists)  // new record
                        lstSelection.Add(iMsgCode);                        
                }

                if (lstSelection.Count > 0)
                {
                    string sFilter = "messageCode IN (";
                    string sSelectedMsgCodes = "";
                    for (int i = 0; i < lstSelection.Count; i++)
                        sSelectedMsgCodes += lstSelection[i].ToString() + ",";
                    // remove the last comma
                    sFilter += sSelectedMsgCodes.Substring(0, sSelectedMsgCodes.Length - 1) + ")";  
                    DataView dv = new DataView(m_table, sFilter, "created ASC", DataViewRowState.CurrentRows);                    
                    dataGridViewLog.DataSource = dv;

                    filterToolStripMenuItem.Text = "Unfilter";
                }                
            }
            else
            {                
                dataGridViewLog.DataSource = m_table;
                filterToolStripMenuItem.Text = "Filter";
            }                
        }

        private void dataGridViewLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                m_iMessageCode = Convert.ToInt16(dataGridViewLog.Rows[e.RowIndex].Cells[MESSAGE_CODE_INDEX].Value);                
            }
        }

        private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open a file 
            string sUserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            StreamWriter sw = new StreamWriter(sUserDesktop + "\\Log.csv");

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
                        
            sw.Close();
            Process.Start("Notepad", sUserDesktop + "\\Log.csv");
        }
    }
}
