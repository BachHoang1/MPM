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
    public partial class FormDPointSelection : Form
    {
        CDPointLookupTable m_DPointTable;
        private int m_iMessageCode;
        private int m_iRowSelected;
        private string m_sDisplayName;
        private string m_sUnits;

        public FormDPointSelection()
        {
            InitializeComponent();
        }

        public void SetDpointTable(CDPointLookupTable tbl_)
        {
            m_DPointTable = tbl_;
        }

        public void SetCurrentMessageCode(int iVal_)
        {
            m_iMessageCode = iVal_;
        }

        private void FormDPointSelection_Load(object sender, EventArgs e)
        {
            dataGridViewDPoint.DataSource = m_DPointTable.GetAllCommands();
            for (int i = 0; i < dataGridViewDPoint.Rows.Count; i++)
            {
                if ((string)dataGridViewDPoint.Rows[i].Cells[0].Value == m_iMessageCode.ToString())
                {
                    dataGridViewDPoint.Rows[i].Selected = true;
                    dataGridViewDPoint.FirstDisplayedScrollingRowIndex = i;
                    break;
                }                    
            }            
        }

        private void dataGridViewDPoint_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                m_iRowSelected = e.RowIndex;
                m_iMessageCode = Convert.ToInt16(dataGridViewDPoint.Rows[e.RowIndex].Cells[0].Value);
                m_sDisplayName = (string)dataGridViewDPoint.Rows[e.RowIndex].Cells[2].Value;
                m_sUnits = (string)dataGridViewDPoint.Rows[e.RowIndex].Cells[4].Value;
            }            
        }

        public void GetUserSelection(ref int iMessageCode_, ref string sDisplayName_, ref string sUnits_)
        {
            iMessageCode_ = m_iMessageCode;
            sDisplayName_ = m_sDisplayName;
            sUnits_ = m_sUnits;
        }

        private void dataGridViewDPoint_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }                        
        }
    }
}
