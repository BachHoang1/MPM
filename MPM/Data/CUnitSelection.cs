// author: hoan chau
// purpose: data layer between gui and table

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.Data
{
    public class CUnitSelection
    {
        public const int NUM_WIDGETS = 4;
        public const int NUM_UNIT_GROUPS = 3;  // length, pressure, and temperature

        public enum UNIT_GROUPS { LENGTH, PRESSURE, TEMPERATURE };

        private CCommonTypes.UNIT_SET m_unitSet;

        private string m_sFormName;
        private string[] m_sArrWidgetName;
        private string[] m_sArrWidgetValue;

        // when units are set by group
        private bool [] m_bArrUnitByGroup;  // true for imperial, false for metric        

        public CUnitSelection(string sFormName_, string[] sArrWidgetName_)
        {
            m_sFormName = sFormName_;
            m_sArrWidgetName = sArrWidgetName_;
            m_sArrWidgetValue = new string[NUM_WIDGETS];
            if (m_sArrWidgetName.Length != m_sArrWidgetValue.Length)
            {
                MessageBox.Show("The number of widgets names does not match expected number of " + NUM_WIDGETS.ToString() + ". Unit settings object will not work correctly", "Constructor Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            m_bArrUnitByGroup = new bool[NUM_UNIT_GROUPS];
        }

        public CUnitSelection()
        {
            m_sArrWidgetValue = new string[NUM_WIDGETS];
            m_bArrUnitByGroup = new bool[NUM_UNIT_GROUPS];
        }

        public void Set(string sWidgetName_, string sValue_)
        {
            for (int i = 0; i < NUM_WIDGETS; i++)
            {
                if (m_sArrWidgetName[i] == sWidgetName_)
                {
                    m_sArrWidgetValue[i] = sValue_;
                    break;
                }                    
            }
        }
        
        public void SetLength(string sWidgetName_, string sValue_)
        {

        }

        public int GetNumWidgets()
        {
            return NUM_WIDGETS;
        }
        
        public string GetValue(string sWidgetName_)
        {
            string sRetVal = "";
            for (int i = 0; i < NUM_WIDGETS; i++)
            {
                if (m_sArrWidgetName[i] == sWidgetName_)
                {
                    sRetVal = m_sArrWidgetValue[i];
                    break;
                }
            }
            return sRetVal;
        }

        public string GetWidgetName(int iIndex_)
        {
            return m_sArrWidgetName[iIndex_];
        }

        public void Load(ref CWidgetInfoLookupTable WidgetInfoLookupTbl_)
        {                        
            for (int i = 0; i < NUM_WIDGETS; i++)
               m_sArrWidgetValue[i] = WidgetInfoLookupTbl_.GetValue(m_sFormName, m_sArrWidgetName[i]);

            if (m_sArrWidgetValue[0] == "True")
                m_unitSet = CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC;
            else if (m_sArrWidgetValue[1] == "True")
                m_unitSet = CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL;
            else if (m_sArrWidgetValue[3] == "True")
                m_unitSet = CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP;            
            else
                m_unitSet = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;

            // load up the values for length, pressure, and temperature
            // look up length
            string sWhichLength = WidgetInfoLookupTbl_.GetValue("FormUnitSelectionByDimension", "radioButtonFt");
            if (sWhichLength == "True")
                m_bArrUnitByGroup[(int)UNIT_GROUPS.LENGTH] = true;
            else
                m_bArrUnitByGroup[(int)UNIT_GROUPS.LENGTH] = false;

            // look up pressure
            string sWhichPressure = WidgetInfoLookupTbl_.GetValue("FormUnitSelectionByDimension", "radioButtonPsi");
            if (sWhichPressure == "True")
                m_bArrUnitByGroup[(int)UNIT_GROUPS.PRESSURE] = true;
            else
                m_bArrUnitByGroup[(int)UNIT_GROUPS.PRESSURE] = false;

            // look up temperature
            string sWhichTemperature = WidgetInfoLookupTbl_.GetValue("FormUnitSelectionByDimension", "radioButtonF");
            if (sWhichTemperature == "True")
                m_bArrUnitByGroup[(int)UNIT_GROUPS.TEMPERATURE] = true;
            else
                m_bArrUnitByGroup[(int)UNIT_GROUPS.TEMPERATURE] = false;
        }

        public void LoadDefaults()
        {
            m_unitSet = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
            m_bArrUnitByGroup[(int)UNIT_GROUPS.LENGTH] = true;
            m_bArrUnitByGroup[(int)UNIT_GROUPS.PRESSURE] = true;
            m_bArrUnitByGroup[(int)UNIT_GROUPS.TEMPERATURE] = true;
        }

        public bool[] GetUnitGroups()
        {
            return m_bArrUnitByGroup;
        }

        public void SetUnitGroups(bool [] arrUnitByGroup_)
        {
            m_bArrUnitByGroup = arrUnitByGroup_;
        }

        public CCommonTypes.UNIT_SET GetUnitSet()
        {
            return m_unitSet;
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET val_)
        {
            m_unitSet = val_;
        }

        public void Save(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();
            bool bSuccess = true;
            for (int i = 0; i < NUM_WIDGETS; i++)
            {
                bSuccess = widgetInfoLookup_.Set(m_sFormName, m_sArrWidgetName[i], -1, m_sArrWidgetValue[i] == "True" ? "True" : "False");
                if (!bSuccess)  // error
                    break;
            }

            if (m_bArrUnitByGroup[(int)UNIT_GROUPS.LENGTH] == true)
            {
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonFt", "True");
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonM", "False");
            }
            else
            {
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonFt", "False");
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonM", "True");
            }

            if (m_bArrUnitByGroup[(int)UNIT_GROUPS.PRESSURE] == true)
            {
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonPsi", "True");
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonKpa", "False");
            }
            else
            {
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonPsi", "False");
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonKpa", "True");
            }

            if (m_bArrUnitByGroup[(int)UNIT_GROUPS.TEMPERATURE] == true)
            {
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonF", "True");
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonC", "False");
            }
            else
            {
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonF", "False");
                widgetInfoLookup_.SetValue("FormUnitSelectionByDimension", "radioButtonC", "True");
            }

            if (bSuccess)
                widgetInfoLookup_.Save();
            else
                MessageBox.Show("There was an error trying to set the unit selection option. Please try again.", "Save Unit Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
