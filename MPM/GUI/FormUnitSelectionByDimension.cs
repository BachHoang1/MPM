// author: hoan chau
// purpose: allow user to specify the units for an entire group of quantites

using MPM.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.GUI
{
    public partial class FormUnitSelectionByDimension : Form
    {
        CUnitSelection m_UnitSelection;
        private bool m_bIsServer;
        private bool[] m_bArrUnitGroup;

        public FormUnitSelectionByDimension()
        {
            InitializeComponent();
        }

        public void Set(CUnitSelection unitSelectionObj_)
        {
            m_UnitSelection = unitSelectionObj_;
        }

        public void SetServerMode(bool bVal_)
        {
            m_bIsServer = bVal_;
        }

        private void FormUnitSelectionByDimension_Load(object sender, EventArgs e)
        {
            if (!m_bIsServer)
            {
                buttonOK.Enabled = false;
                this.Text += " (View Only)";
            }
                

            m_bArrUnitGroup = m_UnitSelection.GetUnitGroups();
            if (m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.LENGTH])
                radioButtonFt.Checked = true;
            else
                radioButtonM.Checked = true;

            if (m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.PRESSURE])
                radioButtonPsi.Checked = true;
            else
                radioButtonKpa.Checked = true;

            if (m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.TEMPERATURE])
                radioButtonF.Checked = true;
            else
                radioButtonC.Checked = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonFt.Checked)
                m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.LENGTH] = true;
            else
                m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.LENGTH] = false;

            if (radioButtonPsi.Checked)
                m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.PRESSURE] = true;
            else
                m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.PRESSURE] = false;

            if (radioButtonF.Checked)
                m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.TEMPERATURE] = true;
            else
                m_bArrUnitGroup[(int)CUnitSelection.UNIT_GROUPS.TEMPERATURE] = false;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<string> GetWidgetNames()
        {
            List<string> lst = new List<string>();

            lst.Add(radioButtonFt.Name);
            lst.Add(radioButtonM.Name);
            lst.Add(radioButtonPsi.Name);
            lst.Add(radioButtonKpa.Name);
            lst.Add(radioButtonF.Name);
            lst.Add(radioButtonC.Name);

            return lst;
        }

        public bool[] Get()
        {
            return m_bArrUnitGroup;
        }
    }
}
