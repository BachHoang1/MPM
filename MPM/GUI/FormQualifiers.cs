// author: hoan chau
// purpose: show survey qualifiers; vibration and shock; and decoder stats

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MPM.Data;
using MPM.Utilities;

namespace MPM.GUI
{
    public partial class FormQualifiers : ToolWindow
    {
        private const int MAG_AXES = 3;
        private const int GRAV_AXES = 3;
        private const int QUALIFIERS = 3;
        // for testing fake data
        //private Thread m_threadTestQualifiers;

        private CDetectDataLayer m_DataLayer;
        private CCommonTypes.UNIT_SET m_iUnitSet;

        private CWidgetInfoLookupTable m_widgetInfoLookup;
        private CDPointLookupTable m_DPointTable;       

        static void Worker(object obj)
        {
            FormQualifiers param = (FormQualifiers)obj;            
            while (true)
            {
                param.Redraw();
                Thread.Sleep(500);
            }
        }
        public FormQualifiers(ref CWidgetInfoLookupTable widgetInfoLookup_, ref CDPointLookupTable dpointTable_)
        {
            InitializeComponent();
            m_widgetInfoLookup = widgetInfoLookup_;
            m_DPointTable = dpointTable_;

            // for testing fake data
            //m_threadTestQualifiers = new Thread(Worker);              
        }

        private void Redraw()
        {            
            Random rnd = new Random();
            float fVal = rnd.Next(1, 100) / 100.0f;
            userControlDPoint4.SetVal(0.808f + fVal, CCommonTypes.EM_SUPER_SCRIPT);
            fVal = rnd.Next(1, 84) / 84.0f;
            userControlDPoint5.SetVal(84.55f + fVal, CCommonTypes.EM_SUPER_SCRIPT);
            fVal = rnd.Next(1, 100) / 100.0f;
            userControlDPoint6.SetVal(0.999f + fVal, CCommonTypes.EM_SUPER_SCRIPT);

            fVal = rnd.Next(1, 10) / 5.0f;
            userControlDPoint1.SetVal(79.0f + fVal, CCommonTypes.EM_SUPER_SCRIPT);

            fVal = rnd.Next(1, 10) / 10.0f;
            userControlDPoint2.SetVal(90.0f + fVal, CCommonTypes.EM_SUPER_SCRIPT);

            fVal = rnd.Next(1, 10) / 10.0f;
            userControlDPoint3.SetVal(90.0f + fVal, CCommonTypes.EM_SUPER_SCRIPT);

            fVal = rnd.Next(0, 4);
            userControlDPoint7.SetVal(fVal, CCommonTypes.EM_SUPER_SCRIPT);
            fVal = rnd.Next(0, 4);
            userControlDPoint8.SetVal(fVal, CCommonTypes.EM_SUPER_SCRIPT);
            fVal = rnd.Next(0, 4);
            userControlDPoint9.SetVal(fVal, CCommonTypes.MP_SUPER_SCRIPT);

        }

        private void FormQualifiers_FormClosed(object sender, FormClosedEventArgs e)
        {
            //m_threadTestQualifiers.Abort();
        }

        private void FormQualifiers_Load(object sender, EventArgs e)
        {
            RefreshScreen();

            userControlDPoint1.MouseDoubleClicked += userControlDPoint1_MouseDoubleClick;
            userControlDPoint2.MouseDoubleClicked += userControlDPoint2_MouseDoubleClick;
            userControlDPoint3.MouseDoubleClicked += userControlDPoint3_MouseDoubleClick;
            userControlDPoint4.MouseDoubleClicked += userControlDPoint4_MouseDoubleClick;
            userControlDPoint5.MouseDoubleClicked += userControlDPoint5_MouseDoubleClick;
            userControlDPoint6.MouseDoubleClicked += userControlDPoint6_MouseDoubleClick;
            userControlDPoint7.MouseDoubleClicked += userControlDPoint7_MouseDoubleClick;
            userControlDPoint8.MouseDoubleClicked += userControlDPoint8_MouseDoubleClick;
            userControlDPoint9.MouseDoubleClicked += userControlDPoint9_MouseDoubleClick;
            //m_threadTestQualifiers.Start(this);
        }

        public void RefreshScreen()
        {
            string sTime = "";
            string sTelemetryType = "";
            string sVal = "";
            bool bParityErr = false;

            CDPointLookupTable.DPointInfo dpi;

            int iMessageCode = 0;

            // ***************************
            // mag axes and accelerometers
            // ***************************
            UserControlDPoint[] ucdpArrMagGrav = new UserControlDPoint[MAG_AXES + GRAV_AXES] { userControlDPoint1, userControlDPoint2, userControlDPoint3, userControlDPoint7, userControlDPoint8, userControlDPoint9 };
            int[] iArrMagGravDefaultMsgCodes = new int[MAG_AXES + GRAV_AXES] { 18, 19, 20, 15, 16, 17 };
            for (int i = 0; i < MAG_AXES + GRAV_AXES; i++)
            {
                iMessageCode = m_widgetInfoLookup.GetMessageCode(this.Name, ucdpArrMagGrav[i].Name);
                if (iMessageCode > -1)
                {
                    sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, ucdpArrMagGrav[i].Name, out sTime, out sTelemetryType, out bParityErr);
                    dpi = m_DPointTable.Get(iMessageCode);
                    if (sVal != null)
                        if (sVal.Length > 0)
                            ucdpArrMagGrav[i].SetVal(sVal, sTelemetryType, sTime, bParityErr);
                }
                else  // default
                {
                    dpi = m_DPointTable.Get(iArrMagGravDefaultMsgCodes[i]);
                }

                ucdpArrMagGrav[i].SetMessageCode(dpi.iMessageCode);
                ucdpArrMagGrav[i].SetDesc(dpi.sDisplayName);
                ucdpArrMagGrav[i].SetUnits(dpi.sUnits);
                ucdpArrMagGrav[i].SetPrecision(4);
                ucdpArrMagGrav[i].UseSmallFont(true);
                ucdpArrMagGrav[i].SetColorMap(dpi.sThresholdColors);
            }

            // ***************************
            // qualifiers
            // ***************************
            UserControlDPoint[] ucdpArrQualifier = new UserControlDPoint[QUALIFIERS] { userControlDPoint4, userControlDPoint5, userControlDPoint6 };
            int[] iArrQualifierDefaultMsgCodes = new int[MAG_AXES] { 11, 67, 10 };
            for (int i = 0; i < QUALIFIERS; i++)
            {
                iMessageCode = m_widgetInfoLookup.GetMessageCode(this.Name, ucdpArrQualifier[i].Name);
                if (iMessageCode > -1)
                {
                    sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, ucdpArrQualifier[i].Name, out sTime, out sTelemetryType, out bParityErr);
                    dpi = m_DPointTable.Get(iMessageCode);
                    if (sVal != null)
                        if (sVal.Length > 0)
                            ucdpArrQualifier[i].SetVal(sVal, sTelemetryType, sTime, bParityErr);
                }
                else  // default
                {
                    dpi = m_DPointTable.Get(iArrQualifierDefaultMsgCodes[i]);
                }

                ucdpArrQualifier[i].SetMessageCode(dpi.iMessageCode);
                ucdpArrQualifier[i].SetDesc(dpi.sDisplayName);
                ucdpArrQualifier[i].SetUnits(dpi.sUnits);
                ucdpArrQualifier[i].SetPrecision(3);
                ucdpArrQualifier[i].SetColorMap(dpi.sThresholdColors);
            }
        }        

        private void DisplayDPoints(UserControlDPoint ctrl_, int iMessageCode_)
        {
            FormDPointSelection frm = new FormDPointSelection();
            
            frm.SetCurrentMessageCode(iMessageCode_);
            frm.SetDpointTable(m_DPointTable);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                int iMessageCode = 0;
                string sDisplayName = "";
                string sUnits = "";
                frm.GetUserSelection(ref iMessageCode, ref sDisplayName, ref sUnits);
                ctrl_.SetDesc(sDisplayName);
                ctrl_.SetMessageCode(iMessageCode);

                // determine unit of measurement
                CBaseUnitConversion[] unitConversionArr = new CBaseUnitConversion[4] { new CUnitLength(), new CUnitPressure(), new CUnitTemperature(), new CUnitRateOfPenetration() };
                if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT ||
                    m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)
                    ctrl_.SetUnits(sUnits);
                else if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                {
                    for (int i = 0; i < unitConversionArr.Length; i++)
                    {
                        if (sUnits.ToLower() == unitConversionArr[i].GetMetricUnitDesc().ToLower())
                        {
                            ctrl_.SetUnits(unitConversionArr[i].GetImperialUnitDesc());
                            break;
                        }
                    }
                }
                else if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                {
                    for (int i = 0; i < unitConversionArr.Length; i++)
                    {
                        if (sUnits.ToLower() == unitConversionArr[i].GetImperialUnitDesc().ToLower())
                        {
                            ctrl_.SetUnits(unitConversionArr[i].GetMetricUnitDesc());
                            break;
                        }
                    }
                }
                
                ctrl_.SetVal(UserControlDPoint.NO_VAL_YET, "");
                CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get(iMessageCode);
                ctrl_.SetColorMap(dpi.sThresholdColors);
            }
        }

        private bool SaveWidgetInfo(UserControlDPoint userCtrl_, int iMessageCode_)
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();
            m_widgetInfoLookup.Set(this.Name, userCtrl_.Name, iMessageCode_);
            bool bSaved = m_widgetInfoLookup.Save();
            return bSaved;
        }

        private void userControlDPoint1_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint1.GetMessageCode();
            DisplayDPoints(userControlDPoint1, userControlDPoint1.GetMessageCode());

            if (userControlDPoint1.GetMessageCode() != iMessageCode && userControlDPoint1.GetMessageCode() != -1)            
                SaveWidgetInfo(userControlDPoint1, userControlDPoint1.GetMessageCode());                                          
        }
        private void userControlDPoint2_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint2.GetMessageCode();
            DisplayDPoints(userControlDPoint2, userControlDPoint2.GetMessageCode());
            if (userControlDPoint2.GetMessageCode() != iMessageCode && userControlDPoint2.GetMessageCode() != -1)            
                SaveWidgetInfo(userControlDPoint2, userControlDPoint2.GetMessageCode());            
        }
        private void userControlDPoint3_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint3.GetMessageCode();
            DisplayDPoints(userControlDPoint3, userControlDPoint3.GetMessageCode());
            if (userControlDPoint3.GetMessageCode() != iMessageCode && userControlDPoint3.GetMessageCode() != -1)
                SaveWidgetInfo(userControlDPoint3, userControlDPoint3.GetMessageCode());
        }
        private void userControlDPoint4_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint4.GetMessageCode();
            DisplayDPoints(userControlDPoint4, userControlDPoint4.GetMessageCode());
            if (userControlDPoint4.GetMessageCode() != iMessageCode && userControlDPoint4.GetMessageCode() != -1)
                SaveWidgetInfo(userControlDPoint4, userControlDPoint4.GetMessageCode());
        }
        private void userControlDPoint5_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint5.GetMessageCode();
            DisplayDPoints(userControlDPoint5, userControlDPoint5.GetMessageCode());
            if (userControlDPoint5.GetMessageCode() != iMessageCode && userControlDPoint5.GetMessageCode() != -1)
                SaveWidgetInfo(userControlDPoint5, userControlDPoint5.GetMessageCode());
        }
        private void userControlDPoint6_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint6.GetMessageCode();
            DisplayDPoints(userControlDPoint6, userControlDPoint6.GetMessageCode());
            if (userControlDPoint6.GetMessageCode() != iMessageCode && userControlDPoint6.GetMessageCode() != -1)
                SaveWidgetInfo(userControlDPoint6, userControlDPoint6.GetMessageCode());
        }
        private void userControlDPoint7_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint7.GetMessageCode();
            DisplayDPoints(userControlDPoint7, userControlDPoint7.GetMessageCode());
            if (userControlDPoint7.GetMessageCode() != iMessageCode && userControlDPoint7.GetMessageCode() != -1)
                SaveWidgetInfo(userControlDPoint7, userControlDPoint7.GetMessageCode());
        }
        private void userControlDPoint8_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint8.GetMessageCode();
            DisplayDPoints(userControlDPoint8, userControlDPoint8.GetMessageCode());
            if (userControlDPoint8.GetMessageCode() != iMessageCode && userControlDPoint8.GetMessageCode() != -1)
                SaveWidgetInfo(userControlDPoint8, userControlDPoint8.GetMessageCode());
        }
        private void userControlDPoint9_MouseDoubleClick(object sender, EventArgs e)
        {
            int iMessageCode = userControlDPoint9.GetMessageCode();
            DisplayDPoints(userControlDPoint9, userControlDPoint9.GetMessageCode());
            if (userControlDPoint9.GetMessageCode() != iMessageCode && userControlDPoint9.GetMessageCode() != -1)
                SaveWidgetInfo(userControlDPoint9, userControlDPoint9.GetMessageCode());
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ListChanged);
        }

        private void ListChanged(object sender, CEventDPoint e)
        {
            //System.Diagnostics.Debug.Print("FormQualifiers::This is called when the event fires.");  
            string sTelemetryType = e.GetTelemetryType();

            if (userControlDPoint1.GetMessageCode() == e.m_ID)
                userControlDPoint1.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint2.GetMessageCode() == e.m_ID)
                userControlDPoint2.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint3.GetMessageCode() == e.m_ID)
                userControlDPoint3.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint4.GetMessageCode() == e.m_ID)
                userControlDPoint4.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint5.GetMessageCode() == e.m_ID)
                userControlDPoint5.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint6.GetMessageCode() == e.m_ID)
                userControlDPoint6.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint7.GetMessageCode() == e.m_ID)
                userControlDPoint7.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint8.GetMessageCode() == e.m_ID)
                userControlDPoint8.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            else if (userControlDPoint9.GetMessageCode() == e.m_ID)
                userControlDPoint9.SetVal(e.m_sValue, sTelemetryType, e.m_bIsParityError);
            
        }

        

        public void SetUnitSet(CCommonTypes.UNIT_SET iVal_)
        {
            UserControlDPoint[] userControlArr = new UserControlDPoint[9] { userControlDPoint1, userControlDPoint2, userControlDPoint3, userControlDPoint4, userControlDPoint5, userControlDPoint6, userControlDPoint7, userControlDPoint8, userControlDPoint9 };
            CBaseUnitConversion[] unitConversionArr = new CBaseUnitConversion[5] { new CUnitLength(), new CUnitPressure(), new CUnitTemperature(), new CUnitRateOfPenetration(), new CUnitDensity() };

            m_iUnitSet = iVal_;

            if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
            {
                for (int i = 0; i < userControlArr.Length; i++)
                {
                    string sUnit = userControlArr[i].GetUnits().ToLower();
                    for (int j = 0; j < unitConversionArr.Length; j++)
                    {
                        if (sUnit == unitConversionArr[j].GetMetricUnitDesc().ToLower())
                        {
                            string sImperialUnit = unitConversionArr[j].GetImperialUnitDesc();
                            userControlArr[i].SetUnits(sImperialUnit);
                            break;
                        }
                    }
                }
            }
            else if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
            {
                for (int i = 0; i < userControlArr.Length; i++)
                {
                    string sUnit = userControlArr[i].GetUnits().ToLower();
                    for (int j = 0; j < unitConversionArr.Length; j++)
                    {
                        if (sUnit == unitConversionArr[j].GetImperialUnitDesc().ToLower())
                        {
                            string sImperialUnit = unitConversionArr[j].GetMetricUnitDesc();
                            userControlArr[i].SetUnits(sImperialUnit);
                            break;
                        }
                    }
                }
            }
            else
            {
                var parent = this.MdiParent as Main;
                for (int i = 0; i < userControlArr.Length; i++)
                {
                    string sUnit = userControlArr[i].GetUnits().ToLower();
                    for (int j = 0; j < unitConversionArr.Length; j++)
                    {
                        CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get(userControlArr[i].GetMessageCode());
                        if (dpi.iMessageCode > -1)
                            userControlArr[i].SetUnits(dpi.sUnits);
                    }
                }
            }
        }

        private void FormQualifiers_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CWidgetInfoLookupTable widgetlookupInfo = new CWidgetInfoLookupTable();
            //widgetlookupInfo.Load();
            UserControlDPoint[] ucdpArrMagGrav = new UserControlDPoint[MAG_AXES + GRAV_AXES] { userControlDPoint1, userControlDPoint2, userControlDPoint3, userControlDPoint7, userControlDPoint8, userControlDPoint9 };
            
            for (int i = 0; i < MAG_AXES + GRAV_AXES; i++)
            {
                m_widgetInfoLookup.SetSessionInfo(this.Name, ucdpArrMagGrav[i].Name, ucdpArrMagGrav[i].GetMessageCode(),
                                                ucdpArrMagGrav[i].GetVal(), ucdpArrMagGrav[i].GetTime(),
                                                ucdpArrMagGrav[i].GetTelemetryType(), ucdpArrMagGrav[i].GetParityErr());
            }


            UserControlDPoint[] ucdpArrQualifier = new UserControlDPoint[QUALIFIERS] { userControlDPoint4, userControlDPoint5, userControlDPoint6 };            
            for (int i = 0; i < QUALIFIERS; i++)
            {
                m_widgetInfoLookup.SetSessionInfo(this.Name, ucdpArrQualifier[i].Name, ucdpArrQualifier[i].GetMessageCode(),
                                                ucdpArrQualifier[i].GetVal(), ucdpArrQualifier[i].GetTime(),
                                                ucdpArrQualifier[i].GetTelemetryType(), ucdpArrQualifier[i].GetParityErr());

            }

            //widgetlookupInfo.Save();
        }
    }
}
