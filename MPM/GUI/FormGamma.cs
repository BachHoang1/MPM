// author: hoan chau
// purpose: display gamma log related information
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
    public partial class FormGamma : ToolWindow
    {
        // for testing fake data
        //private Thread m_threadTestGamma;
        private float m_fDepth = 1000.0f;        

        private CDetectDataLayer m_DataLayer;
        private CWidgetInfoLookupTable m_widgetInfoLookup;
        private CDPointLookupTable m_DPointTable;
        CCommonTypes.UNIT_SET m_iUnitSet;

        static void Worker(object obj)
        {
            FormGamma param = (FormGamma)obj;
            while (true)
            {
                param.Redraw();
                Thread.Sleep(3000);
            }
        }


        public FormGamma(ref CWidgetInfoLookupTable widgetInfoLookup_, ref CDPointLookupTable dpointTable_)
        {
            InitializeComponent();

            // for testing fake data
            //m_threadTestGamma = new Thread(Worker);    
            m_widgetInfoLookup = widgetInfoLookup_;
            m_DPointTable = dpointTable_;
        }

        private void Redraw()
        {
            Random rnd = new Random();

            float fVal = rnd.Next(1, 100) / 100.0f;
            userControlDPointGamma.SetVal(99.0f + fVal, "ᴱᴹ");
            
            userControlDPointGammaDepth.SetVal(m_fDepth, "");
            m_fDepth += 0.1f;
            
            userControlDPointROP.SetVal(10.0f, "");            
        }

        
        private void FormGamma_FormClosed(object sender, FormClosedEventArgs e)
        {
            //m_threadTestGamma.Abort();
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
            }
        }

        private void FormGamma_Load(object sender, EventArgs e)
        {
            //m_threadTestGamma.Start(this);
            RefreshScreen();

            userControlDPointGamma.MouseDoubleClicked += userControlDPointGamma_MouseDoubleClick;
            userControlDPointGammaDepth.MouseDoubleClicked += userControlDPointGammaDepth_MouseDoubleClick;
            userControlDPointROP.MouseDoubleClicked += userControlDPointROP_MouseDoubleClick;
        }

        public void RefreshScreen()
        {           
            string sTime = "";
            string sTelemetryType = "";
            string sVal = "";
            bool bParityErr = false;                    

            CDPointLookupTable.DPointInfo dpi;
            int iMessageCode = m_widgetInfoLookup.GetMessageCode(this.Name, userControlDPointGamma.Name);
            if (iMessageCode > -1)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlDPointGamma.Name, out sTime, out sTelemetryType, out bParityErr);
                dpi = m_DPointTable.Get(iMessageCode);
                if (sVal != null)
                    if (sVal.Length > 0)
                        userControlDPointGamma.SetVal(sVal, sTelemetryType, sTime, bParityErr);
            }
            else
                dpi = m_DPointTable.Get(23);     // gamma                                      

            userControlDPointGamma.SetMessageCode(dpi.iMessageCode);
            userControlDPointGamma.SetDesc(dpi.sDisplayName);
            userControlDPointGamma.SetUnits(dpi.sUnits);
            userControlDPointGamma.SetPrecision(1);
            userControlDPointGamma.UseVerticalLayout(false);


            iMessageCode = m_widgetInfoLookup.GetMessageCode(this.Name, userControlDPointGammaDepth.Name);
            if (iMessageCode > -1)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlDPointGammaDepth.Name, out sTime, out sTelemetryType, out bParityErr);
                dpi = m_DPointTable.Get(iMessageCode);
                if (sVal != null)
                    if (sVal.Length > 0)
                        userControlDPointGammaDepth.SetVal(sVal, sTelemetryType, sTime, bParityErr);
            }
            else
                dpi = m_DPointTable.Get(10821);      // gamma ray depth                       

            userControlDPointGammaDepth.SetMessageCode(dpi.iMessageCode);
            userControlDPointGammaDepth.SetDesc(dpi.sDisplayName);
            userControlDPointGammaDepth.SetUnits(dpi.sUnits);
            userControlDPointGammaDepth.SetMessageCode(iMessageCode);
            userControlDPointGammaDepth.SetPrecision(1);
            userControlDPointGammaDepth.UseVerticalLayout(false);


            iMessageCode = m_widgetInfoLookup.GetMessageCode(this.Name, userControlDPointROP.Name);
            if (iMessageCode > -1)
            {
                sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlDPointROP.Name, out sTime, out sTelemetryType, out bParityErr);
                dpi = m_DPointTable.Get(iMessageCode);
                if (sVal != null)
                    if (sVal.Length > 0)
                        userControlDPointROP.SetVal(sVal, sTelemetryType, sTime, bParityErr);
            }
            else
                dpi = m_DPointTable.Get(10113);  // rate of penetration                                                           

            userControlDPointROP.SetMessageCode(dpi.iMessageCode);
            userControlDPointROP.SetDesc(dpi.sDisplayName);
            userControlDPointROP.SetPrecision(1);
            userControlDPointROP.SetUnits(dpi.sUnits);
            userControlDPointROP.UseVerticalLayout(false);
        }

        private void userControlDPointGamma_MouseDoubleClick(object sender, EventArgs e)
        {
            DisplayDPoints(userControlDPointGamma, userControlDPointGamma.GetMessageCode());
        }

        private void userControlDPointGammaDepth_MouseDoubleClick(object sender, EventArgs e)
        {
            DisplayDPoints(userControlDPointGammaDepth, userControlDPointGammaDepth.GetMessageCode());
        }

        private void userControlDPointROP_MouseDoubleClick(object sender, EventArgs e)
        {
            DisplayDPoints(userControlDPointROP, userControlDPointROP.GetMessageCode());
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ListChanged);
        }

        private void ListChanged(object sender, CEventDPoint e)
        {
            //System.Diagnostics.Debug.Print("FormQualifiers::This is called when the event fires.");                                    
            if (userControlDPointGamma.GetMessageCode() == e.m_ID)
                userControlDPointGamma.SetVal(e.m_sValue, e.GetTelemetryType(), e.m_bIsParityError);
            else if (userControlDPointGammaDepth.GetMessageCode() == e.m_ID)
                userControlDPointGammaDepth.SetVal(e.m_sValue, e.GetTelemetryType(), e.m_bIsParityError);
            else if (userControlDPointROP.GetMessageCode() == e.m_ID)
                userControlDPointROP.SetVal(e.m_sValue, e.GetTelemetryType(), e.m_bIsParityError);
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET iVal_)
        {
            m_iUnitSet = iVal_;
            UserControlDPoint[] userControlArr = new UserControlDPoint[3] { userControlDPointGamma, userControlDPointGammaDepth, userControlDPointROP };
            CBaseUnitConversion[] unitConversionArr = new CBaseUnitConversion[5] { new CUnitLength(), new CUnitPressure(), new CUnitTemperature(), new CUnitRateOfPenetration(), new CUnitDensity() };

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

        private void FormGamma_Resize(object sender, EventArgs e)
        {
            if (this.Width < CCommonTypes.MAX_WIDTH_LEFT_SIDE_WIDGETS)
            {
                userControlDPointGamma.UseSmallFont(true);
                userControlDPointGammaDepth.UseSmallFont(true);
                userControlDPointROP.UseSmallFont(true);
            }
            else
            {
                userControlDPointGamma.UseSmallFont(false);
                userControlDPointGammaDepth.UseSmallFont(false);
                userControlDPointROP.UseSmallFont(false);
            }
        }

        private void FormGamma_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CWidgetInfoLookupTable widgetlookupInfo = new CWidgetInfoLookupTable();
            //widgetlookupInfo.Load();


            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlDPointGamma.Name, userControlDPointGamma.GetMessageCode(),
                                            userControlDPointGamma.GetVal(), userControlDPointGamma.GetTime(), 
                                            userControlDPointGamma.GetTelemetryType(), userControlDPointGamma.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlDPointGammaDepth.Name, userControlDPointGammaDepth.GetMessageCode(),
                                            userControlDPointGammaDepth.GetVal(), userControlDPointGammaDepth.GetTime(),
                                            userControlDPointGammaDepth.GetTelemetryType(), userControlDPointGammaDepth.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlDPointROP.Name, userControlDPointROP.GetMessageCode(),
                                            userControlDPointROP.GetVal(), userControlDPointROP.GetTime(),
                                            userControlDPointROP.GetTelemetryType(), userControlDPointROP.GetParityErr());

            //m_widgetInfoLookup.Save();
        }
    }
}
