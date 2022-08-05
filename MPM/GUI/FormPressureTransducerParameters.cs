// author: hoan chau
// purpose: to allow users to change pressure transducer settings without having to close the window
//          to see the changes take effect
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;

namespace MPM.GUI
{
    public partial class FormPressureTransducerParameters : Form
    {
        private CPressureTransducer m_pressureTransducer;
        private bool m_bServerMode;

        private DbConnection m_dbCnn;

        public FormPressureTransducerParameters(ref DbConnection dbCnn_)
        {
            InitializeComponent();
            m_bServerMode = false;
            m_dbCnn = dbCnn_;
        }

        public void SetServerMode(bool bVal_)
        {
            m_bServerMode = bVal_;
        }

        public void SetPressureTransducer(ref CPressureTransducer obj_)
        {
            m_pressureTransducer = obj_;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FormPressureTransducerParameters_Load(object sender, EventArgs e)
        {
            if (m_pressureTransducer != null)
            {
                comboBoxTransducerType.SelectedIndex = m_pressureTransducer.GetTransducerType();

                RECEIVER_TYPE_COMBOBOX.SelectedIndex = m_pressureTransducer.GetReceiverType(); //4/29/22

                comboBoxPressureUnits.SelectedIndex = (int)m_pressureTransducer.GetPressureUnit();
                    
                textBoxPressureOffset.Text = m_pressureTransducer.GetPressureOffset().ToString();
                
                textBoxTransducerScale.Text = m_pressureTransducer.GetTransducerScale().ToString();

                textBoxTransducerGain.Text = m_pressureTransducer.GetTransducerGain().ToString();
            }

            CLogTransducerSetting log = new CLogTransducerSetting(ref m_dbCnn);
            DataTable tbl = log.Get();
            dataGridViewLog.DataSource = tbl;

            if (!m_bServerMode)
            {
                this.Text +=  " (View Only)";
                buttonOK.Enabled = false;
            }                
        }

        private bool ValidateFields()
        {
            bool bRetVal = true;

            m_pressureTransducer.SetPressureUnit((CPressureTransducer.PRESSURE_UNIT)comboBoxPressureUnits.SelectedIndex);

            float fTransducerTypeValue = System.Convert.ToSingle(comboBoxTransducerType.SelectedItem.ToString().Replace(",", ""));

            float fVal = 0.0f;
            bool bIsNumeric = System.Single.TryParse(textBoxPressureOffset.Text, out fVal);
            if (!bIsNumeric)
            {
                bRetVal = false;
                MessageBox.Show("Pressure offset must be a decimal value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } 
            else if (fVal > fTransducerTypeValue)
            {
                bRetVal = false;
                if (comboBoxPressureUnits.SelectedIndex == (int)CPressureTransducer.PRESSURE_UNIT.KPA)
                    fTransducerTypeValue *= CPressureTransducer.PSI_TO_KPA;

                string sMsg = String.Format("Pressure offset cannot be greater than {0} {1}.", fTransducerTypeValue, comboBoxPressureUnits.SelectedItem.ToString());
                MessageBox.Show(sMsg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                m_pressureTransducer.SetPressureOffset(fVal);            

            bIsNumeric = System.Single.TryParse(textBoxTransducerGain.Text, out fVal);
            if (!bIsNumeric)
            {
                bRetVal = false;
                MessageBox.Show("Transducer gain must be a decimal value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }                
            else
            {
                if (fVal > 0.0f)
                    m_pressureTransducer.SetTransducerGain(fVal);
                else
                {
                    bRetVal = false;
                    MessageBox.Show("Transducer gain must greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
                
            return bRetVal;
        }

        

        private void comboBoxPressureUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            CPressureTransducer.PRESSURE_UNIT PreviousUnit = m_pressureTransducer.GetPressureUnit();
            CPressureTransducer.PRESSURE_UNIT NewUnit = (CPressureTransducer.PRESSURE_UNIT)comboBoxPressureUnits.SelectedIndex;

            if (PreviousUnit != NewUnit)  // then convert
            {                
                float fOffset = System.Convert.ToSingle(textBoxPressureOffset.Text);
                float fGain = System.Convert.ToSingle(textBoxTransducerGain.Text);
                if (NewUnit == CPressureTransducer.PRESSURE_UNIT.PSI)  // convert from kpa to psi
                {
                    fOffset /= CPressureTransducer.PSI_TO_KPA;
                    fGain /= CPressureTransducer.PSI_TO_KPA;
                }
                else  // convert from psi to kpa
                {
                    fOffset *= CPressureTransducer.PSI_TO_KPA;
                    fGain *= CPressureTransducer.PSI_TO_KPA;
                }

                textBoxPressureOffset.Text = fOffset.ToString();
                textBoxTransducerGain.Text = fGain.ToString();                
            }
            // else there was no change

            m_pressureTransducer.SetPressureUnit((CPressureTransducer.PRESSURE_UNIT)comboBoxPressureUnits.SelectedIndex);
            textBoxTransducerScale.Text = m_pressureTransducer.GetTransducerScale(comboBoxTransducerType.SelectedIndex).ToString();            
        }        

        private void comboBoxTransducerType_SelectedIndexChanged(object sender, EventArgs e)
        {            
            float fTransducerScale;

            if (comboBoxTransducerType.SelectedIndex == 0)
                fTransducerScale = CPressureTransducer.TRANSDUCER_SCALE_PSI_3000;                
            else if (comboBoxTransducerType.SelectedIndex == 1)
                fTransducerScale = CPressureTransducer.TRANSDUCER_SCALE_PSI_5000;            
            else
                fTransducerScale = CPressureTransducer.TRANSDUCER_SCALE_PSI_10000;

            m_pressureTransducer.SetTransducerType(comboBoxTransducerType.SelectedIndex);

            fTransducerScale = m_pressureTransducer.GetTransducerScale(comboBoxTransducerType.SelectedIndex);

            textBoxTransducerScale.Text = fTransducerScale.ToString();            
        }

        //4/29/22
        private void RECEIVER_TYPE_COMBOBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_pressureTransducer.SetReceiverType(RECEIVER_TYPE_COMBOBOX.SelectedIndex);
        }
        //4/29/22
    }
}
