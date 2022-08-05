// author: hoan chau
// purpose: allows user to select their unit of measurement for display. in addition, when user wants to change
//          the native unit of measurement, this allows them to make individual changes 

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
    public partial class FormUnitSelection : Form
    {
        public struct DPOINT_UNIT_REC
        {
            public short iMessageCode;
            public string sAPSName;
            public string sDisplayName;
            public string sUnitOfMeasurement;
        }

        public struct UNIT_REC
        {
            public string sWidgetName;
            public string sWidgetValue;
        }

        private CCommonTypes.UNIT_SET m_iUnitSelection;
        private CDPointLookupTable m_DPointTable;

        private List<DPOINT_UNIT_REC> m_lstDPointsEdited = new List<DPOINT_UNIT_REC>();
        private List<DataRow> m_lstRowsChanged;       
        private List<string> m_lstColNames;
        private CUnitSelection m_UnitSelection;
        private bool[] m_bArrUnitGroup;

        List<string> m_lstFormWidget;

        List<UNIT_REC> m_lstUnitOption;

        private bool m_bServerMode;

        public FormUnitSelection()
        {
            InitializeComponent();
            m_lstRowsChanged = new List<DataRow>();
            m_lstColNames = new List<string>();
            m_lstFormWidget = new List<string>();
            m_lstUnitOption = new List<UNIT_REC>();
            m_bServerMode = false;
        }

        public void SetServerMode(bool bVal_)
        {
            m_bServerMode = bVal_;
        }

        public List<DataRow> GetChanges()
        {          
            // create table
            DataTable tbl = new DataTable();
            for (int i = 0; i < dataGridViewByEachDPoint.Columns.Count; i++)
                tbl.Columns.Add(dataGridViewByEachDPoint.Columns[i].Name);
            
            // populate table
            for (int i = 0; i < m_lstDPointsEdited.Count; i++)
            {
                tbl.Rows.Add(m_lstDPointsEdited[i].iMessageCode,
                             m_lstDPointsEdited[i].sAPSName,
                             m_lstDPointsEdited[i].sDisplayName,
                             m_lstDPointsEdited[i].sUnitOfMeasurement);                
            }

            // massage into list<datarow> that caller wants
            for (int i = 0; i < tbl.Rows.Count; i++)
                m_lstRowsChanged.Add(tbl.Rows[i]);
            
            return m_lstRowsChanged;
        }

        public List<string> GetColNames()
        {
            m_lstColNames.Clear();
            for (int i = 0; i < dataGridViewByEachDPoint.Columns.Count; i++)
                m_lstColNames.Add(dataGridViewByEachDPoint.Columns[i].Name);
            return m_lstColNames;
        }

        public List<string> GetFormWidgetChanges()
        {            
            return m_lstFormWidget;
        }

        public List<UNIT_REC> GetUnitOption()
        {
            return m_lstUnitOption;
        }

        public void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonImperialToMetric.Checked)
                m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC;
            else if (radioButtonMetricToImperial.Checked)
                m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL;
            else if (radioButtonByEachDPoint.Checked)
                m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
            else
                m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP;
            
            // record the option settings
            UNIT_REC unitOpt = new UNIT_REC();
            unitOpt.sWidgetName = radioButtonImperialToMetric.Name;
            unitOpt.sWidgetValue = radioButtonImperialToMetric.Checked ? "True" : "False";
            m_lstUnitOption.Add(unitOpt);
            unitOpt.sWidgetName = radioButtonMetricToImperial.Name;
            unitOpt.sWidgetValue = radioButtonMetricToImperial.Checked ? "True" : "False";
            m_lstUnitOption.Add(unitOpt);
            unitOpt.sWidgetName = radioButtonByEachDPoint.Name;
            unitOpt.sWidgetValue = radioButtonByEachDPoint.Checked ? "True" : "False";
            m_lstUnitOption.Add(unitOpt);
            unitOpt.sWidgetName = radioButtonByDimension.Name;
            unitOpt.sWidgetValue = radioButtonByDimension.Checked ? "True" : "False";
            m_lstUnitOption.Add(unitOpt);
            
            // save edits to the display and/or unit of measurement
            if (radioButtonByEachDPoint.Checked)
            {
                if (m_lstDPointsEdited.Count > 0)
                {
                    for (int i = 0; i < m_lstDPointsEdited.Count; i++)
                    {
                        m_DPointTable.SetDisplayName(m_lstDPointsEdited[i].iMessageCode, m_lstDPointsEdited[i].sAPSName, m_lstDPointsEdited[i].sDisplayName);
                        m_DPointTable.SetUnitName(m_lstDPointsEdited[i].iMessageCode, m_lstDPointsEdited[i].sAPSName, m_lstDPointsEdited[i].sUnitOfMeasurement);
                    }
                }
            }
            else if (radioButtonByDimension.Checked)
            {
                if (m_bArrUnitGroup == null)
                {                    
                    // default to all metric
                    bool[] bArr = new bool[CUnitSelection.NUM_UNIT_GROUPS] { false, false, false };
                    
                    m_bArrUnitGroup = bArr;
                }

                CUnitLength len = new CUnitLength();
                CUnitRateOfPenetration rate = new CUnitRateOfPenetration();
                if (m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.LENGTH])  // update all native units to feet
                {                    
                    ChangeUnit(len.GetMetricUnitDesc(), len.GetImperialUnitDesc());                    
                    ChangeUnit(rate.GetMetricUnitDesc(), rate.GetImperialUnitDesc());  // rates like ROP
                }
                else  // update all native units to meters
                {                 
                    ChangeUnit(len.GetImperialUnitDesc(), len.GetMetricUnitDesc());                 
                    ChangeUnit(rate.GetImperialUnitDesc(), rate.GetMetricUnitDesc());   // rates like ROP
                }

                CUnitPressure pressure = new CUnitPressure();
                if (m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.PRESSURE])  // update all native units to psi
                {                                        
                    ChangeUnit(pressure.GetMetricUnitDesc(), pressure.GetImperialUnitDesc());
                }
                else  // update all native units to kpa
                {                    
                    ChangeUnit(pressure.GetImperialUnitDesc(), pressure.GetMetricUnitDesc());
                }

                CUnitTemperature temp = new CUnitTemperature();
                if (m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.TEMPERATURE])  // update all native units to fahrenheit
                {                    
                    ChangeUnit(temp.GetMetricUnitDesc(), temp.GetImperialUnitDesc());
                }
                else  // update all native units to celsius
                {                    
                    ChangeUnit(temp.GetImperialUnitDesc(), temp.GetMetricUnitDesc());
                }

                // save the changes, if any
                if (m_lstDPointsEdited.Count > 0)
                {
                    for (int i = 0; i < m_lstDPointsEdited.Count; i++)
                    {
                        m_DPointTable.SetDisplayName(m_lstDPointsEdited[i].iMessageCode, m_lstDPointsEdited[i].sAPSName, m_lstDPointsEdited[i].sDisplayName);
                        m_DPointTable.SetUnitName(m_lstDPointsEdited[i].iMessageCode, m_lstDPointsEdited[i].sAPSName, m_lstDPointsEdited[i].sUnitOfMeasurement);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        } 
        
        private void ChangeUnit(string sFromUnit_, string sToUnit_)
        {
            for (int i = 0; i < dataGridViewByEachDPoint.Rows.Count; i++)
            {                
                if (sFromUnit_.ToLower() == dataGridViewByEachDPoint.Rows[i].Cells[3].Value.ToString().ToLower())
                {
                    dataGridViewByEachDPoint.Rows[i].Cells[3].Value = sToUnit_;
                    DPOINT_UNIT_REC rec = new DPOINT_UNIT_REC();
                    rec.iMessageCode = System.Convert.ToInt16(dataGridViewByEachDPoint.Rows[i].Cells[0].Value.ToString());
                    rec.sAPSName = dataGridViewByEachDPoint.Rows[i].Cells[1].Value.ToString();
                    rec.sDisplayName = dataGridViewByEachDPoint.Rows[i].Cells[2].Value.ToString();
                    rec.sUnitOfMeasurement = dataGridViewByEachDPoint.Rows[i].Cells[3].Value.ToString();
                    m_lstDPointsEdited.Add(rec);
                }
            }
        }

        public MPM.Data.CCommonTypes.UNIT_SET GetSelectedUnitSet()
        {
            if (m_UnitSelection != null)
            {
                radioButtonImperialToMetric.Checked = System.Convert.ToBoolean(m_UnitSelection.GetValue(radioButtonImperialToMetric.Name));
                radioButtonMetricToImperial.Checked = System.Convert.ToBoolean(m_UnitSelection.GetValue(radioButtonMetricToImperial.Name));
                radioButtonByEachDPoint.Checked = System.Convert.ToBoolean(m_UnitSelection.GetValue(radioButtonByEachDPoint.Name));
                radioButtonByDimension.Checked = System.Convert.ToBoolean(m_UnitSelection.GetValue(radioButtonByDimension.Name));

                if (radioButtonImperialToMetric.Checked)
                    m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC;
                else if (radioButtonMetricToImperial.Checked)
                    m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL;
                else if (radioButtonByEachDPoint.Checked)
                    m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
                else
                    m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP;
            }
            else  // default
                m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
            
            return m_iUnitSelection;
        }

        public void SetUnitSelection(ref CUnitSelection unitSelection_)
        {
            m_UnitSelection = unitSelection_;
        }
        
        public string[] GetOptionWidgets()
        {
            string []sArrRet = new string[CUnitSelection.NUM_WIDGETS]{radioButtonImperialToMetric.Name, 
                                             radioButtonMetricToImperial.Name, 
                                             radioButtonByEachDPoint.Name,
                                             radioButtonByDimension.Name};            
            return sArrRet;
        }        


        private void FormUnitSelection_Load(object sender, EventArgs e)
        {
            GetSelectedUnitSet();

            if (radioButtonByEachDPoint.Checked)
                dataGridViewByEachDPoint.Enabled = true;

            if (m_DPointTable != null)
            {
                dataGridViewByEachDPoint.DataSource = m_DPointTable.GetDPointAndUnit();
                foreach (DataGridViewColumn dc in dataGridViewByEachDPoint.Columns)
                {
                    if (dc.Index.Equals(2) || dc.Index.Equals(3))  // allow user to edit the display name and unit of measurement
                        dc.ReadOnly = false;                    
                    else                    
                        dc.ReadOnly = true;                    
                }
            }

            if (!m_bServerMode)
            {
                this.Text += " (View Only)";
                buttonOK.Enabled = false;
            }
        }

        public void SetDpointTable(ref CDPointLookupTable tbl_)
        {
            m_DPointTable = tbl_;            
        }

        private void dataGridViewByEachDPoint_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // store the change for saving later
            DPOINT_UNIT_REC rec = new DPOINT_UNIT_REC();
            rec.iMessageCode = System.Convert.ToInt16(dataGridViewByEachDPoint.Rows[e.RowIndex].Cells[0].Value.ToString());
            rec.sAPSName = dataGridViewByEachDPoint.Rows[e.RowIndex].Cells[1].Value.ToString();
            rec.sDisplayName = dataGridViewByEachDPoint.Rows[e.RowIndex].Cells[2].Value.ToString();
            rec.sUnitOfMeasurement = dataGridViewByEachDPoint.Rows[e.RowIndex].Cells[3].Value.ToString();
            m_lstDPointsEdited.Add(rec);
        }

        private void radioButtonByEachDPoint_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewByEachDPoint.Enabled = true;            
            dataGridViewByEachDPoint.RowsDefaultCellStyle.BackColor = Color.FromArgb(60, 152, 196);
            buttonEditUnitGroup.Enabled = false;
        }

        private void radioButtonImperialToMetric_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewByEachDPoint.Enabled = false;            
            dataGridViewByEachDPoint.RowsDefaultCellStyle.BackColor = Color.Gray;
            buttonEditUnitGroup.Enabled = false;
        }

        private void radioButtonMetricToImperial_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewByEachDPoint.Enabled = false;            
            dataGridViewByEachDPoint.RowsDefaultCellStyle.BackColor = Color.Gray;
            buttonEditUnitGroup.Enabled = false;
        }
      
        private void radioButtonByDimension_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewByEachDPoint.Enabled = false;
            dataGridViewByEachDPoint.RowsDefaultCellStyle.BackColor = Color.Gray;
            buttonEditUnitGroup.Enabled = true;
        }

        private void buttonEditUnitGroup_Click(object sender, EventArgs e)
        {
            FormUnitSelectionByDimension frm = new FormUnitSelectionByDimension();
            frm.Set(m_UnitSelection);
            frm.SetServerMode(m_bServerMode);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_bArrUnitGroup = frm.Get();
            }
        }

        public bool[] GetUnitGroup()
        {
            return m_bArrUnitGroup;
        }
    }
}
