// author: hoan chau
// purpose: display specific mud pulse and em information 

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
using MPM.DataAcquisition.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPM.GUI
{
    public partial class FormPumpPowerTemp : ToolWindow
    {
        public delegate void dgEventRaiser(string s);       

        
        private const int PUMP_STATUS_TIME_CHECK = 120000;  // 2 minutes

        private const int BATTERY_POWER_COMMAND_ID = 27;
        private const int BATTERY_STATUS_COMMAND_ID = 28;
        private const int TEMPERATURE_COMMAND_ID = 12;
        private const int PUMP_PRESSURE_COMMAND_ID = 88;
        private const int PULSE_HEIGHT_COMMAND_ID = 20000;

        //private Thread m_threadTestPump;
        private Thread m_threadCheckPumpStatus;
        private DateTime m_dtLastPumpStatusUpdate;

        private CDetectDataLayer m_DataLayer;
        private bool m_bUnload;

        private CPumpPressure m_PumpPressure;        

        private static ManualResetEvent m_mreUpdatePumpStatus = new ManualResetEvent(false);

        private float m_fFormationResistanceLow;
        private float m_fFormationResistanceHigh;

        private CWidgetInfoLookupTable m_widgetInfoLookup;
        private CDPointLookupTable m_DPointTable;

        private decimal m_dPumpPressure = 0;  //4/29/22
        private decimal m_dPulseHeight = 0;   //4/29/22
        private string m_TelemetryType = "";  //4/29/22

        static void Worker(object obj)
        {
            FormPumpPowerTemp param = (FormPumpPowerTemp)obj;
            while (true)
            {
                param.Redraw();
                //2/25/22Thread.Sleep(2000);
                Thread.Sleep(500);
            }
        }

        static void WorkerPumpStatus(object obj)
        {
            FormPumpPowerTemp param = (FormPumpPowerTemp)obj;
            while (true)
            {
                m_mreUpdatePumpStatus.WaitOne();
                if (param.m_bUnload)
                    break;

                param.UpdatePumpStatus();

                try
                {
                    Thread.Sleep(PUMP_STATUS_TIME_CHECK);
                    param.UpdatePumpStatus();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void UpdatePumpStatus()
        {
            DateTime dtNow = DateTime.Now;
            TimeSpan tsDiff = dtNow - m_dtLastPumpStatusUpdate;
            if (tsDiff.TotalMinutes > 1)
                userControlPump.SetStatus(-1);

            m_mreUpdatePumpStatus.Set();
        }

        private void Redraw()
        {
            Random rnd = new Random();

            float fVal = rnd.Next(1, 100) / 100.0f;
            userControlPump.SetVal(99.0f + fVal, "");
        }

        public FormPumpPowerTemp(ref CWidgetInfoLookupTable widgetInfoLookup_, ref CDPointLookupTable dpointTable_)
        {
            InitializeComponent();
            m_widgetInfoLookup = widgetInfoLookup_;
            m_DPointTable = dpointTable_;

            //m_threadTestPump = new Thread(Worker);
            m_threadCheckPumpStatus = new Thread(WorkerPumpStatus);
            m_dtLastPumpStatusUpdate = DateTime.Now;                       
        }  
        
        public void Unload()
        {
            m_bUnload = true;
            m_mreUpdatePumpStatus.Set();
        }
                       

        private void FormPumpPowerTemp_Load(object sender, EventArgs e)
        {
            //m_threadTestPump.Start(this);
            m_threadCheckPumpStatus.Start(this);

            RefreshScreen();
        }

        public void RefreshScreen()
        {
            string sTime = "";
            string sTelemetryType = "";
            string sVal = "";
            bool bParityErr = false;
            m_bUnload = false;
            
            // pump pressure
            CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get(PUMP_PRESSURE_COMMAND_ID);  // pump pressure            
            userControlPump.SetDesc(dpi.sDisplayName);
            userControlPump.SetPrecision(1);
            userControlPump.SetUnits(dpi.sUnits);
            userControlPump.SetMessageCode(dpi.iMessageCode);

            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlPump.Name, out sTime, out sTelemetryType, out bParityErr);
              
            if (sVal != null)
                if (sVal.Length > 0)
                    userControlPump.SetVal(sVal, sTelemetryType, sTime, bParityErr);

            // temperature
            userControlTemperature.SetDesc("Temp");
            userControlTemperature.SetPrecision(1);
            userControlTemperature.SetUnits("°C");
            userControlTemperature.SetMessageCode(TEMPERATURE_COMMAND_ID);
            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlTemperature.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                {
                    float f;
                    bool b = float.TryParse(sVal, out f);
                    if (b)
                    {
                        sVal = string.Format("{0:0.#}", f);
                        userControlTemperature.SetVal(sVal, sTelemetryType, sTime, bParityErr);
                    }
                }


            // battery
            userControlBattery.SetDesc("Battery");
            userControlBattery.SetUnits("W");
            userControlBattery.SetMessageCode(BATTERY_POWER_COMMAND_ID);
            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlBattery.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                    userControlBattery.SetVal(sVal, sTelemetryType, sTime, bParityErr);

            // pulse height
            dpi = m_DPointTable.Get(PULSE_HEIGHT_COMMAND_ID);
            userControlPulseHeight.SetDesc(dpi.sDisplayName);
            userControlPulseHeight.SetPrecision(1);
            userControlPulseHeight.SetUnits(dpi.sUnits);
            userControlPulseHeight.SetMessageCode(dpi.iMessageCode);
            userControlPulseHeight.SetThresholds(dpi.sLowThreshold, dpi.sHighThreshold);
            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlPulseHeight.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                    userControlPulseHeight.SetVal(sVal, sTelemetryType, sTime, bParityErr);

            // snr   
            dpi = m_DPointTable.Get(20001);
            userControlSNR.SetDesc(dpi.sDisplayName);
            userControlSNR.SetPrecision(1);
            userControlSNR.SetUnits(dpi.sUnits);
            userControlSNR.SetMessageCode(dpi.iMessageCode);
            userControlSNR.SetThresholds(dpi.sLowThreshold, dpi.sHighThreshold);
            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, userControlSNR.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                    userControlSNR.SetVal(sVal, sTelemetryType, sTime, bParityErr);

            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, "userControlSignalPart", out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                    userControlSNR.SetSignal(System.Convert.ToSingle(sVal));

            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, "userControlNoisePart", out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                    userControlSNR.SetNoise(System.Convert.ToSingle(sVal));

            userControlPulseHeight.MouseDoubleClicked += userControlPulseHeight_MouseDoubleClick;
            userControlSNR.MouseDoubleClicked += userControlSNR_MouseDoubleClick;

            // net
            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, labelNETValue.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                {
                    labelNETValue.Text = sVal;
                    int iValPos = sVal.IndexOf(":");
                    if (iValPos > 0)
                    {
                        string s = sVal.Substring(iValPos + 1);
                        s = s.Trim();
                        if (s != "0" && s != "---")
                        {
                            labelNETValue.BackColor = Color.Red;
                        }
                    }
                }


            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, labelNETDate.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                    labelNETDate.Text = sVal;

            // formation resistance
            dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_FORMRES);
            m_fFormationResistanceLow = System.Convert.ToSingle(dpi.sLowThreshold);
            m_fFormationResistanceHigh = System.Convert.ToSingle(dpi.sHighThreshold);
            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, labelFormationResistanceValue.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                {
                    labelFormationResistanceValue.Text = sVal;
                    int iValPos = sVal.IndexOf(":");
                    if (iValPos > 0)
                    {
                        string s = sVal.Substring(iValPos + 1);
                        s = s.Trim();
                        float fVal;
                        bool b = float.TryParse(s, out fVal);
                        if (b)
                        {
                            if (fVal < m_fFormationResistanceLow || fVal > m_fFormationResistanceHigh)
                                labelFormationResistanceValue.BackColor = Color.Red;
                        }
                    }
                }


            sVal = m_widgetInfoLookup.GetLastSessionInfo(this.Name, labelFormationResistanceDate.Name, out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                    labelFormationResistanceDate.Text = sVal;
        }

        private void FormPumpPowerTemp_FormClosed(object sender, FormClosedEventArgs e)
        {
            //m_threadTestPump.Abort();
            m_bUnload = true;
            m_mreUpdatePumpStatus.Set();
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ListChanged);
        }

        public void SetListenerForPumpPressure(CPumpPressure pumpPressure_)
        {
            m_PumpPressure = pumpPressure_;
            m_PumpPressure.Changed += new ChangedEventHandler(ListChanged);
        }


        private void ListChanged(object sender, CEventDPoint e)
        {
            decimal temp; //4/29/22
            string stemp; //4/29/22

            m_TelemetryType = e.GetTelemetryType();  //4/29/22

            if (userControlTemperature.GetMessageCode() == e.m_ID)
            {
                float f;
                bool b = float.TryParse(e.m_sValue, out f);
                if (b)
                {
                    f = System.Convert.ToSingle(e.m_sValue);
                    e.m_sValue = string.Format("{0:0.0#}", f);
                }                
                userControlTemperature.SetVal(e.m_sValue, e.GetTelemetryType());
            }                
            else if (userControlBattery.GetMessageCode() == e.m_ID)
                userControlBattery.SetVal(e.m_sValue, e.GetTelemetryType());
            else if (userControlPump.GetMessageCode() == e.m_ID)
            {
                //4/29/22     
                m_dPumpPressure = Convert.ToDecimal(e.m_sValue);
              
                if ((int)m_PumpPressure.GetPressureTransducer().GetPressureUnit() == 1) //{ PSI = 0, KPA = 1 };
                {
                    temp = m_dPumpPressure * (decimal) 6.8947;

                    stemp = temp.ToString("0.00");

                    userControlPump.SetVal(stemp, e.GetTelemetryType());

                } //4/29/22
                else
                    userControlPump.SetVal(e.m_sValue, e.GetTelemetryType());

                m_dtLastPumpStatusUpdate = DateTime.Now;
            }
            else if (userControlPulseHeight.GetMessageCode() == e.m_ID)
            {
                //4/29/22  
                m_dPulseHeight = Convert.ToDecimal(e.m_sValue);

                if ((int)m_PumpPressure.GetPressureTransducer().GetPressureUnit() == 1) //{ PSI = 0, KPA = 1 };
                {
                    temp = m_dPulseHeight * (decimal) 6.8947;

                    stemp = temp.ToString("0.00");

                    userControlPulseHeight.SetVal(stemp, e.GetTelemetryType());

                } //4/29/22
                else
                  userControlPulseHeight.SetVal(e.m_sValue, e.GetTelemetryType());
            }
            else if (e.m_ID == (int)Command.COMMAND_RESP_NOISE ||
                e.m_ID == (int)Command.COMMAND_SIG_STRENGTH)
            {
                if (e.m_ID == (int)Command.COMMAND_RESP_NOISE)
                    userControlSNR.SetNoise(Convert.ToSingle(e.m_sValue));
                else
                    userControlSNR.SetSignal(Convert.ToSingle(e.m_sValue));
            }
            else if (e.m_ID.ToString() == labelNETValue.Tag.ToString())
            {
                labelNETValue.Text = "NET: " + e.m_sValue;
                if (e.m_sValue != "0")
                    labelNETValue.BackColor = Color.Red;
                else
                    labelNETValue.BackColor = Color.FromArgb(63, 63, 63);
                labelNETDate.Text = DateTime.Now.ToString("HH:mm:ss tt");
            }
            else if (e.m_ID.ToString() == labelFormationResistanceValue.Tag.ToString())
            {
                float fVal = System.Convert.ToSingle(e.m_sValue);
                if (fVal < m_fFormationResistanceLow || fVal > m_fFormationResistanceHigh)
                    labelFormationResistanceValue.BackColor = Color.Red;
                else
                    labelFormationResistanceValue.BackColor = Color.FromArgb(63, 63, 63);
                labelFormationResistanceValue.Text = "FrmRes: " + fVal.ToString("0.0");
                labelFormationResistanceDate.Text = DateTime.Now.ToString("HH:mm:ss tt");
            }

            if (e.m_ID == BATTERY_STATUS_COMMAND_ID)  // status of battery
            {
                if (e.m_sValue == "All")
                    userControlBattery.SetStatus(3);
                else
                    userControlBattery.SetStatus(System.Convert.ToInt16(e.m_sValue));
            }


            if (e.m_ID == PUMP_PRESSURE_COMMAND_ID)  // status pump pressure
            {
                float fVal = Convert.ToSingle(e.m_sValue);
                if (fVal <= 0.0f)
                    userControlPump.SetStatus(0);
                else
                    userControlPump.SetStatus(1);
            }
        }           
        public void SetUnitsFromTransducer(CPressureTransducer.PRESSURE_UNIT iVal_)
        {
            decimal temp; //4/29/22
            string stemp; //4/29/22

            if (iVal_ == CPressureTransducer.PRESSURE_UNIT.PSI)
            {
                userControlPulseHeight.SetUnits("psi");
                userControlPump.SetUnits("psi");

                temp = m_dPumpPressure;
                stemp = temp.ToString("0.00");
                userControlPump.SetVal(stemp, m_TelemetryType);

                temp = m_dPulseHeight;
                stemp = temp.ToString("0.00");
                userControlPulseHeight.SetVal(stemp, m_TelemetryType);

            } //4/29/22
     
            else //if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
            {
                userControlPulseHeight.SetUnits("KPa");
                userControlPump.SetUnits("KPa");

                //4/29/22
                temp = m_dPumpPressure * (decimal) 6.8947;
                stemp = temp.ToString("0.00");
                userControlPump.SetVal(stemp, m_TelemetryType);
                                
                temp = m_dPulseHeight * (decimal)6.8947;
                stemp = temp.ToString("0.00");
                userControlPulseHeight.SetVal(stemp, m_TelemetryType);
                //4/29/22
            }
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET iVal_)
        {
            if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
            {
                // pressure units handled by pressure transducer settings
                //userControlPulseHeight.SetUnits("psi");
                //userControlPump.SetUnits("psi");
                userControlTemperature.SetUnits("°F");
            }
            else if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
            {
                // pressure units handled by pressure transducer settings
                //userControlPulseHeight.SetUnits("KPa");
                //userControlPump.SetUnits("KPa");
                userControlTemperature.SetUnits("°C");
            }
            else
            {                
                // pressure units handled by pressure transducer settings
                //CDPointLookupTable.DPointInfo dpi = parent.m_DPointTable.Get(PULSE_HEIGHT_COMMAND_ID);
                //userControlPulseHeight.SetUnits(dpi.sUnits);

                //dpi = parent.m_DPointTable.Get(PUMP_PRESSURE_COMMAND_ID);
                //userControlPump.SetUnits(dpi.sUnits);

                CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get(TEMPERATURE_COMMAND_ID);  // temperature
                userControlTemperature.SetUnits(dpi.sUnits);
            }
        }

        private float GetScreenWidthScaleFactor()
        {
            return (float)ClientSize.Width / (float)CCommonTypes.MAX_WIDTH_RIGHT_SIDE_WIDGETS;
        }

        private float GetScreenHeightScaleFactor()
        {
            return (float)ClientSize.Height / (float)CCommonTypes.MAX_HEIGHT_RIGHT_SIDE_WIDGETS;
        }

        private void FormPumpPowerTemp_Resize(object sender, EventArgs e)
        {
            float fScaleWidth = GetScreenWidthScaleFactor();
            float fScaleHeight = GetScreenHeightScaleFactor();
            if (fScaleWidth < 1.0f || fScaleHeight < 1.0f)
            {
                userControlPulseHeight.UseSmallFont(true);
                userControlTemperature.UseSmallFont(true);
                userControlBattery.UseSmallFont(true);
                userControlPump.UseSmallFont(true);
                userControlSNR.UseSmallFont(true);
            }
            else
            {
                userControlPulseHeight.UseSmallFont(false);
                userControlTemperature.UseSmallFont(false);
                userControlBattery.UseSmallFont(false);
                userControlPump.UseSmallFont(false);
                userControlSNR.UseSmallFont(false);
            }
        }

        private void FormPumpPowerTemp_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CWidgetInfoLookupTable widgetlookupInfo = new CWidgetInfoLookupTable();
            //widgetlookupInfo.Load();

            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlPulseHeight.Name, userControlPulseHeight.GetMessageCode(),
                                            userControlPulseHeight.GetVal(), userControlPulseHeight.GetTime(),
                                            userControlPulseHeight.GetTelemetryType(), userControlPulseHeight.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlPump.Name, userControlPump.GetMessageCode(),
                                            userControlPump.GetVal(), userControlPump.GetTime(),
                                            userControlPump.GetTelemetryType(), userControlPump.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlBattery.Name, userControlBattery.GetMessageCode(),
                                            userControlBattery.GetVal(), userControlBattery.GetTime(),
                                            userControlBattery.GetTelemetryType(), userControlBattery.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlTemperature.Name, userControlTemperature.GetMessageCode(),
                                            userControlTemperature.GetVal(), userControlTemperature.GetTime(),
                                            userControlTemperature.GetTelemetryType(), userControlTemperature.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, userControlSNR.Name, userControlSNR.GetMessageCode(),
                                            userControlSNR.GetVal(), userControlSNR.GetTime(),
                                            userControlSNR.GetTelemetryType(), userControlSNR.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, "userControlSignalPart", userControlSNR.GetMessageCode(),
                                            userControlSNR.GetSignal(), userControlSNR.GetTime(),
                                            userControlSNR.GetTelemetryType(), userControlSNR.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, "userControlNoisePart", userControlSNR.GetMessageCode(),
                                           userControlSNR.GetNoise(), userControlSNR.GetTime(),
                                           userControlSNR.GetTelemetryType(), userControlSNR.GetParityErr());

            m_widgetInfoLookup.SetSessionInfo(this.Name, labelNETValue.Name, 20002, labelNETValue.Text, labelNETDate.Text, CCommonTypes.EM_SUPER_SCRIPT, true);
            m_widgetInfoLookup.SetSessionInfo(this.Name, labelNETDate.Name, 20002, labelNETDate.Text, labelNETDate.Text, CCommonTypes.EM_SUPER_SCRIPT, true);

            m_widgetInfoLookup.SetSessionInfo(this.Name, labelFormationResistanceValue.Name, 47, labelFormationResistanceValue.Text, labelFormationResistanceDate.Text, "em", true);
            m_widgetInfoLookup.SetSessionInfo(this.Name, labelFormationResistanceDate.Name, 47, labelFormationResistanceDate.Text, labelFormationResistanceDate.Text, "em", true);

            //widgetlookupInfo.Save();                       
        }                     
                   
        private void userControlSNR_MouseDoubleClick(object sender, EventArgs e)
        {
            FormThresholdDPoint frm = new FormThresholdDPoint();
            float fLow, fHigh;
            userControlSNR.GetThresholds(out fLow, out fHigh);
            frm.SetThresholds(fLow.ToString(), fHigh.ToString());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                frm.GetThresholds(out fLow, out fHigh);
                userControlSNR.SetThresholds(fLow.ToString(), fHigh.ToString());                
                m_DPointTable.SetThresholds(20001, fLow.ToString(), fHigh.ToString());
            }
        }

        private void userControlPulseHeight_MouseDoubleClick(object sender, EventArgs e)
        {
            FormThresholdDPoint frm = new FormThresholdDPoint();
            float fLow, fHigh;
            userControlPulseHeight.GetThresholds(out fLow, out fHigh);
            frm.SetThresholds(fLow.ToString(), fHigh.ToString());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                frm.GetThresholds(out fLow, out fHigh);
                userControlPulseHeight.SetThresholds(fLow.ToString(), fHigh.ToString());                
                m_DPointTable.SetThresholds(20000, fLow.ToString(), fHigh.ToString());
            }
        }

        private void userControlSNR_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void userControlPulseHeight_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void labelFormationResistanceValue_DoubleClick(object sender, EventArgs e)
        {
            FormThresholdDPoint frm = new FormThresholdDPoint();
            float fLow = m_fFormationResistanceLow, fHigh = m_fFormationResistanceHigh;            
            frm.SetThresholds(fLow.ToString(), fHigh.ToString());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                frm.GetThresholds(out fLow, out fHigh);
                m_fFormationResistanceLow = fLow;
                m_fFormationResistanceHigh = fHigh;                
                m_DPointTable.SetThresholds((int)Command.COMMAND_RESP_FORMRES, fLow.ToString(), fHigh.ToString());
            }
        }

       
    }
}
