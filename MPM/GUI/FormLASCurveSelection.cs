// author: hoan chau
// purpose: allow selection of curves to be exported to LAS

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

namespace MPM.GUI
{
    public partial class FormLASCurveSelection : Form
    {
        private const int CHECKBOX_COLUMN_INDEX = 0;
        private const int MESSAGE_CODE_COLUMN_INDEX = 3;

        private DataTable m_table;

        private List<CLogASCIIStandard.CURVE_INFO> m_lstMessageCode;

        public FormLASCurveSelection()
        {
            InitializeComponent();
            m_table = new DataTable();
            m_lstMessageCode = new List<CLogASCIIStandard.CURVE_INFO>();
        }

        private void FormLASCurveSelection_Load(object sender, EventArgs e)
        {
            this.dataGridViewCurves.DataSource = m_table;
            foreach (DataGridViewColumn dc in dataGridViewCurves.Columns)
            {
                if (dc.Index.Equals(CHECKBOX_COLUMN_INDEX))                
                    dc.ReadOnly = false;
                else
                    dc.ReadOnly = true;   
                
                if (dc.Index.Equals(MESSAGE_CODE_COLUMN_INDEX))
                    dc.Visible = false;
            }

            this.dataGridViewCurves.Sort(this.dataGridViewCurves.Columns[1], ListSortDirection.Ascending);
        }

        public void SetData(DataTable tbl_)
        {
            m_table = tbl_;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewCurves.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridViewCurves.Rows[i];
                if (row.Cells[0].FormattedValue.Equals(true))
                {
                    CLogASCIIStandard.CURVE_INFO info = new CLogASCIIStandard.CURVE_INFO();
                    info.sName = row.Cells[1].FormattedValue.ToString();
                    info.sUnit = row.Cells[2].FormattedValue.ToString();
                    info.iMsgCode = System.Convert.ToInt32(row.Cells[3].FormattedValue);                    
                    m_lstMessageCode.Add(info);
                }
            }

            if (m_lstMessageCode.Count < 1)
            {
                MessageBox.Show("You have to select at least one curve.", "Curve Selection Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<CLogASCIIStandard.CURVE_INFO> GetCurveSelections()
        {
            return m_lstMessageCode;
        }
    }
}
