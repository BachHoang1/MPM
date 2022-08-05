// author: hoan chau
// purpose: display the fields that are used for the ECD (equiavalent circulating density) value
//          and allow them to send it to WITS

using MPM.Data;
using MPM.DataAcquisition.Helpers;
using MPM.Utilities;
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

namespace MPM.GUI
{
    public partial class FormECD : Form
    {
        DbConnection m_dbCnn;
        CWITSLookupTable m_LookUpWITS;

        private float m_fAnnularPressure;

        public delegate void EventHandler(object sender, CEventSendWITSData e);
        //public event EventHandler Transmit;

        private CEquivalentCirculatingDensity m_ECD;

        private CUnitSelection m_unitSelection;

        private float m_fTVD;
        private float m_fMudDensity;

        public FormECD(ref DbConnection dbCnn_, ref CWITSLookupTable witsLookUpTbl_, ref CEquivalentCirculatingDensity ECD_, CUnitSelection unitSelection_)
        {
            InitializeComponent();
            m_dbCnn = dbCnn_;
            m_LookUpWITS = witsLookUpTbl_;
            m_ECD = ECD_;
            m_unitSelection = unitSelection_;
        }

        private void FormECD_Load(object sender, EventArgs e)
        {
            ChangeUnitsOnLabels();
            m_ECD.GetParameters(out m_fTVD, out m_fMudDensity);
            textBoxTVD.Text = m_fTVD.ToString();
            textBoxMudDensity.Text = m_fMudDensity.ToString();
        }

        private void ChangeUnitsOnLabels()
        {
            // get the annular pressure
            //int iParenthesesPos = labelAnnularPressure.Text.IndexOf("(");
            string sLengthUnit = GetLengthUnit();
            //labelAnnularPressure.Text = labelAnnularPressure.Text.Substring(0, iParenthesesPos + 1) + sLengthUnit + ")";

            // get the tvd
            int iParenthesesPos = labelTVD.Text.IndexOf("(");
            labelTVD.Text = labelTVD.Text.Substring(0, iParenthesesPos + 1) + sLengthUnit + ")";

            // get the mud density
            iParenthesesPos = labelMudDensity.Text.IndexOf("(");
            labelMudDensity.Text = labelMudDensity.Text.Substring(0, iParenthesesPos + 1) + GetDensityUnit(sLengthUnit.ToLower() == "m") + ")";

            // change formulas
            string sFactor = CEquivalentCirculatingDensity.ECD_FACTOR_IMPERIAL.ToString();
            if (GetLengthUnit() == "m")            
                sFactor = CEquivalentCirculatingDensity.ECD_FACTOR_METRIC.ToString();            
                                        
        }

        private string GetLengthUnit()
        {
            string sLengthUnit = "ft";
            CUnitLength ulength = new CUnitLength();
            if (m_unitSelection.GetUnitSet() == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                sLengthUnit = ulength.GetImperialUnitDesc();
            else if (m_unitSelection.GetUnitSet() == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                sLengthUnit = ulength.GetMetricUnitDesc();
            else  // get the value unit from the specific d-point   
            {
                CDPointLookupTable tbl = new CDPointLookupTable();
                tbl.Load();
                CDPointLookupTable.DPointInfo dpi = tbl.Get((int)Command.COMMAND_BIT_DEPTH);
                sLengthUnit = dpi.sUnits;
            }
                
            return sLengthUnit;
        }

        private string GetPressureUnit()
        {
            string sPressureUnit = "psi";
            CUnitPressure uPressure = new CUnitPressure();
            if (m_unitSelection.GetUnitSet() == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                sPressureUnit = uPressure.GetImperialUnitDesc();
            else if (m_unitSelection.GetUnitSet() == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                sPressureUnit = uPressure.GetMetricUnitDesc();
            else  // get the value unit from the specific d-point   
            {
                CDPointLookupTable tbl = new CDPointLookupTable();
                tbl.Load();
                CDPointLookupTable.DPointInfo dpi = tbl.Get((int)Command.COMMAND_RESP_A_PRESSURE);
                sPressureUnit = dpi.sUnits;
            }

            return sPressureUnit;
        }

        private string GetDensityUnit(bool bIsMetric_)
        {
            string sDensityUnit = "lb/gal";
            if (bIsMetric_)
                sDensityUnit = "kg/m3";            

            return sDensityUnit;
        }

        public void Set(float fAnnularPressure_)
        {
            m_fAnnularPressure = fAnnularPressure_;
            // set the value on the text box
            if (GetPressureUnit().ToLower() == "kpa")
            {
                CUnitPressure uPressure = new CUnitPressure();
                m_fAnnularPressure = uPressure.ToMetric(m_fAnnularPressure);
            }
            textBoxAPressure.Text = m_fAnnularPressure.ToString();

            Refresh();
        }

        public void SetUnit(CUnitSelection unitSelection_)
        {            
            m_unitSelection = unitSelection_;
            ChangeUnitsOnLabels();                       
        }                           

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool bParsed;
            bool bValid = true;
            float fTVD, fMudDensity;

            bParsed = float.TryParse(textBoxTVD.Text, out fTVD);
            if (!bParsed)
            {
                MessageBox.Show("TVD value is invalid.  Check the value for invalid characters and try again.", "ECD Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bValid = false;
            }

            bParsed = float.TryParse(textBoxMudDensity.Text, out fMudDensity);
            if (!bParsed)
            {
                MessageBox.Show("Mud Density value is invalid.  Check the value for invalid characters and try again.", "ECD Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bValid = false;
            }

            if (bValid)
            {
                m_ECD.SetParameters(fTVD, fMudDensity);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
