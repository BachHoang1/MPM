// author: hoan chau
// purpose: to provide a screen to edit WITS channels and related fields

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;
using MPM.Utilities;

namespace MPM.GUI
{
    public partial class FormConfigureWITSData : Form
    {
        private enum COLUMNS { WITS_ID = 1, MATH = 5, SEND_IF_ERROR = 6, OUTLINER_MIN = 7, OUTLINER_MAX = 8 };
        private CWITSLookupTable m_Table;
        private DataTable m_tblOriginal;
        private DataTable m_tblChanged;  // table that will be compared to the original
        private List<DataRow> m_lstRowsChanged;
        private List<string> m_lstColNames;

        private bool m_bServerMode;

        public FormConfigureWITSData()
        {
            InitializeComponent();
            m_lstRowsChanged = new List<DataRow>();
            m_lstColNames = new List<string>();
            m_bServerMode = false;
        }
        public void SetServerMode(bool bVal_)
        {
            m_bServerMode = bVal_;
        }

        public void SetTable(ref CWITSLookupTable tbl_)
        {
            m_Table = tbl_;
            m_tblOriginal = tbl_.GetAll().Copy();
        }

        public CWITSLookupTable GetTable()
        {            
            return m_Table;
        }

        private bool IsValid()
        {
            bool bRetVal = true;

            for (int i = 0; i < dataGridViewWITSRecords.Rows.Count; i++)
            {
                string s = dataGridViewWITSRecords.Rows[i].Cells[(int)COLUMNS.MATH].Value.ToString().ToLower();
                if (s != "none")  // 
                {                    
                    string sExecuteFormula = s.Replace("this", "1");
                    CEvaluate eval = new CEvaluate();
                    double dblVal = eval.Do(sExecuteFormula);
                    if (dblVal == CEvaluate.BAD_VALUE)
                    {
                        buttonOK.Enabled = false;
                        MessageBox.Show("Bad math or invalid formula: " + dataGridViewWITSRecords[(int)COLUMNS.MATH, i].Value.ToString() + "\nUse something like 'this + 4'.", "Bad Math", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        bRetVal = false;
                        break;                        
                    }
                    else
                        buttonOK.Enabled = true;
                }

                s = dataGridViewWITSRecords.Rows[i].Cells[(int)COLUMNS.SEND_IF_ERROR].Value.ToString().ToLower();                
                if (s.ToLower() != "yes" && s.ToLower() != "no")
                {
                    buttonOK.Enabled = false;
                    MessageBox.Show("Send If Error value must be 'yes' or 'no' and not '" + dataGridViewWITSRecords[(int)COLUMNS.SEND_IF_ERROR, i].Value.ToString() + "'.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                    
                    bRetVal = false;
                    break;
                }
                else
                    buttonOK.Enabled = true;
                

                
                s = dataGridViewWITSRecords[(int)COLUMNS.OUTLINER_MIN, i].Value.ToString();
                if (s.ToLower() != "none")
                {
                    double dblVal;
                    bool bIsNumeric = double.TryParse(s, out dblVal);
                    if (!bIsNumeric)
                    {
                        buttonOK.Enabled = false;
                        MessageBox.Show("Outliner Min value must be numeric and not '" + dataGridViewWITSRecords[(int)COLUMNS.OUTLINER_MIN, i].Value.ToString() + "'.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                        
                        bRetVal = false;
                        break;
                    }
                    else
                        buttonOK.Enabled = true;
                }
                

                
                s = dataGridViewWITSRecords[(int)COLUMNS.OUTLINER_MAX, i].Value.ToString();
                if (s.ToLower() != "none")
                {
                    double dblVal;
                    bool bIsNumeric = double.TryParse(s, out dblVal);
                    if (!bIsNumeric)
                    {
                        buttonOK.Enabled = false;
                        MessageBox.Show("Outliner Max value must be numeric and not '" + dataGridViewWITSRecords[(int)COLUMNS.OUTLINER_MAX, i].Value.ToString() + "'.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                        
                        bRetVal = false;
                        break;
                    }
                    else
                        buttonOK.Enabled = true;
                }
                
            }

            return bRetVal;
        }

        public List<DataRow> GetChanges()
        {
            return m_lstRowsChanged;
        }

        public List<string> GetColNames()
        {
            return m_lstColNames;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                for (int i = 0; i < dataGridViewWITSRecords.Columns.Count; i++)
                {
                    string sColName = dataGridViewWITSRecords.Columns[i].Name;
                    m_lstColNames.Add(sColName);
                    //System.Diagnostics.Debug.Write(sColName + ", ");
                }

                m_tblChanged = m_Table.GetAll();
                                
                var diff = m_tblChanged.AsEnumerable().Except(m_tblOriginal.AsEnumerable(), DataRowComparer.Default);

                for (int i = 0; i < diff.Count(); i++)
                {
                    DataRow row = diff.ElementAt<DataRow>(i);
                    m_lstRowsChanged.Add(row);
                    //for (int j = 0; j < row.ItemArray.Count(); j++)
                    //    System.Diagnostics.Debug.Write(row[j]);
                }
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            
        }

        private void FormConfigureWITSData_Load(object sender, EventArgs e)
        {
            dataGridViewWITSRecords.DataSource = m_Table.GetAll();
            foreach (DataGridViewColumn dc in dataGridViewWITSRecords.Columns)
            {
                if (dc.Index.Equals((int)COLUMNS.WITS_ID) ||
                    dc.Index.Equals((int)COLUMNS.MATH) || 
                    dc.Index.Equals((int)COLUMNS.SEND_IF_ERROR) || 
                    dc.Index.Equals((int)COLUMNS.OUTLINER_MIN) || 
                    dc.Index.Equals((int)COLUMNS.OUTLINER_MAX))  // allow user to edit the display name and unit of measurement
                    dc.ReadOnly = false;
                else
                    dc.ReadOnly = true;
            }

            dataGridViewWITSRecords.AutoResizeColumns();

            if (!m_bServerMode)
            {
                this.Text += " (View Only)";
                buttonOK.Enabled = false;
            }
        }

        private void buttonReloadDefaults_Click(object sender, EventArgs e)
        {
            CWITSLookupTable m_tblDefaults = new CWITSLookupTable();
            m_tblDefaults.Load(true);
            dataGridViewWITSRecords.DataSource = m_tblDefaults.GetAll();            
            m_Table = m_tblDefaults;
        }

        private void dataGridViewDPoint_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridViewDPoint_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            
        }

        private void dataGridViewDPoint_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)COLUMNS.MATH)  // 
            {
                string s = dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (s.ToLower() != "none")  // validate the formula
                {                         
                    string sExecuteFormula = s.Replace("this", "1");
                    CEvaluate eval = new CEvaluate();
                    double dblVal = eval.Do(sExecuteFormula);
                    if (dblVal == CEvaluate.BAD_VALUE)
                    {
                        buttonOK.Enabled = false;
                        MessageBox.Show("Bad math or invalid formula: " + dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString() + "\nUse something like 'this + 4'.", "Bad Math", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                       
                    }
                    else
                        buttonOK.Enabled = true;                        
                }                                    
            }
            else if (e.ColumnIndex == (int)COLUMNS.SEND_IF_ERROR)
            {
                string s = dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (s.ToLower() != "yes" && s.ToLower() != "no")
                {
                    buttonOK.Enabled = false;
                    MessageBox.Show("Send If Error value must be 'yes' or 'no' and not '" + dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString() + "'.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                    
                }
                else
                    buttonOK.Enabled = true;
            }
            else if (e.ColumnIndex == (int)COLUMNS.OUTLINER_MIN)
            {
                string s = dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (s.ToLower() != "none")
                {
                    double dblVal;
                    bool bIsNumeric = double.TryParse(s, out dblVal);
                    if (!bIsNumeric)
                    {
                        buttonOK.Enabled = false;
                        MessageBox.Show("Outliner Min value must be numeric and not '" + dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString() + "'.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                        
                    }
                    else
                        buttonOK.Enabled = true;
                }                
            }
            else if (e.ColumnIndex == (int)COLUMNS.OUTLINER_MAX)
            {
                string s = dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (s.ToLower() != "none")
                {
                    double dblVal;
                    bool bIsNumeric = double.TryParse(s, out dblVal);
                    if (!bIsNumeric)
                    {
                        buttonOK.Enabled = false;
                        MessageBox.Show("Outliner Max value must be numeric and not '" + dataGridViewWITSRecords[e.ColumnIndex, e.RowIndex].Value.ToString() + "'.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                        
                    }
                    else
                        buttonOK.Enabled = true;
                }                
            }
        }
    }
}
