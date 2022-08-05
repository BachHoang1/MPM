// author: hoan chau
// purpose: to maintain the settings used to convert pressure transducer values to their proper pressure values
// notes: some values (defaults) based on Russ Tavernetti's email: 
//        560 uses 5000 psi full scale xdcr.  So the full scale factor is:
//        the ADC range = +/-10V.  Thus, the volts per count = 20V / 65536 counts = 305uV / count.
//        Based on a picture example: 160uV x gain = 1 psi or  305/160 counts/psi x gain = 1.9 counts / psi x GAIN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace MPM.Data
{
    public class CPressureTransducer
    {
        private const int PACKET_FIELDS = 4;
        public enum PRESSURE_UNIT { PSI = 0, KPA = 1 };
        
        public const float TRANSDUCER_SCALE_PSI_3000 = 0.3030f;  // 1 psi / 3.3 mV
        public const float TRANSDUCER_SCALE_PSI_5000 = 0.5000f;  // 1 psi / 2.0 mV
        public const float TRANSDUCER_SCALE_PSI_10000 = 1.0f;    // 1 psi / 1.0 mV

        public const float PSI_TO_KPA = 6.894757f;

        private const float DEFAULT_PRESSURE_OFFSET = 0.0f;
        private const float DEFAULT_TRANSDUCER_GAIN = 1.0f;

        private const float DEFAULT_PRESSURE_MAX =   5000.00f; //4/21/11   

        private string m_sTransducerType;  // there are three type of tranduser used: 0 =3000, 1 = 5000, and  2= 10000 PSI
        private int m_iReceiverType;       // Receiver Type: 0 = 560R, 1 = 560C //4/29/22

        private float m_fPressureOffset;
        private float m_fPressureMax; //4/19/22
        private float m_fTransducerScale;  
        private float m_fTransducerGain; 
        private PRESSURE_UNIT m_PressureUnit;

        private DbConnection m_dbCnn;
        private CWidgetInfoLookupTable m_widgetInfoLookup;
        
        public CPressureTransducer(ref DbConnection dbCnn_, ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            m_dbCnn = dbCnn_;
            m_widgetInfoLookup = widgetInfoLookup_;
        }

        public void Init()
        {
            // needs to read from the saved settings
            m_fTransducerScale = TRANSDUCER_SCALE_PSI_10000;
            m_fPressureOffset = DEFAULT_PRESSURE_OFFSET;
            m_fPressureMax = DEFAULT_PRESSURE_MAX;  //4/19/22
            m_fTransducerGain = DEFAULT_TRANSDUCER_GAIN;
            m_PressureUnit = PRESSURE_UNIT.PSI;

            m_iReceiverType = 0; //Default to 560R //4/29/22
        }

        public void Load()
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();

            m_sTransducerType = m_widgetInfoLookup.GetValue("FormPressureTransducerParameters", "comboBoxTransducerType");
            if (m_sTransducerType == "0")
                m_fTransducerScale = TRANSDUCER_SCALE_PSI_3000;
            else if (m_sTransducerType == "1")
                m_fTransducerScale = TRANSDUCER_SCALE_PSI_5000;
            else //if (m_sTransducerType == "2")
                m_fTransducerScale = TRANSDUCER_SCALE_PSI_10000;
            
            string sPressureUnitIndex = m_widgetInfoLookup.GetValue("FormPressureTransducerParameters", "comboBoxPressureUnits");
            m_PressureUnit = (PRESSURE_UNIT)System.Convert.ToInt32(sPressureUnitIndex);            
            if (m_PressureUnit == PRESSURE_UNIT.KPA)            
                m_fTransducerScale *= PSI_TO_KPA;

            string sPressureOffset = m_widgetInfoLookup.GetValue("FormPressureTransducerParameters", "textBoxPressureOffset");
            m_fPressureOffset = System.Convert.ToSingle(sPressureOffset);

            //4/19/22

            //5/4/22 //???
           string sPressureMax = m_widgetInfoLookup.GetValue("FormPressureTransducerParameters", "numericUpDownPressureMax");
           m_fPressureMax = System.Convert.ToSingle(sPressureMax);
           //4/19/22
           
           //4/29/22
           string sReceiverType = m_widgetInfoLookup.GetValue("FormPressureTransducerParameters", "RECEIVER_TYPE_COMBOBOX");
           m_iReceiverType = System.Convert.ToInt32(sReceiverType);
            
            string sTransducerGain = m_widgetInfoLookup.GetValue("FormPressureTransducerParameters", "textBoxTransducerGain");
            m_fTransducerGain = System.Convert.ToSingle(sTransducerGain);

            CLogTransducerSetting log = new CLogTransducerSetting(ref m_dbCnn);
            DataTable dt = log.Get();
            if (dt.Rows.Count == 0)  // must be new install. insert inital record for reference
            {
                CLogTransducerSettingRecord rec = GetRecord();
                log.Save(rec);
            }
            
        }

        private CLogTransducerSettingRecord GetRecord()
        {
            CLogTransducerSettingRecord rec = new CLogTransducerSettingRecord();
            rec.iDate = System.Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            rec.iTime = System.Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
            if (m_sTransducerType == "0")
                rec.sType = "3,000";
            else if (m_sTransducerType == "1")
                rec.sType = "5,000";
            else
                rec.sType = "10,000";
            rec.sUnit = m_PressureUnit.ToString();
            rec.fOffset = m_fPressureOffset;
            rec.fPressureMax = m_fPressureMax; //4/19/22
            rec.iReceiverType= m_iReceiverType; //4/29/22
            rec.fGain = m_fTransducerGain;
            return rec;
        }

        public void Parse(string [] sArrCols_)
        {
            if (sArrCols_.Length > PACKET_FIELDS)  // PacketID is the 5th field
            {
                string[] sArrData = sArrCols_[1].Split('=');
                SetTransducerType(System.Convert.ToInt32(sArrData[1]));
                sArrData = sArrCols_[2].Split('=');
                SetPressureUnit((CPressureTransducer.PRESSURE_UNIT)System.Convert.ToInt32(sArrData[1]));
                sArrData = sArrCols_[3].Split('=');
                SetPressureOffset(System.Convert.ToSingle(sArrData[1]));
                sArrData = sArrCols_[4].Split('=');
                SetTransducerGain(System.Convert.ToSingle(sArrData[1]));
            }            
        }

        public void Save()
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();
            m_widgetInfoLookup.SetValue("FormPressureTransducerParameters", "comboBoxTransducerType", m_sTransducerType);
            m_widgetInfoLookup.SetValue("FormPressureTransducerParameters", "comboBoxPressureUnits", (int)m_PressureUnit);
            m_widgetInfoLookup.SetValue("FormPressureTransducerParameters", "textBoxPressureOffset", m_fPressureOffset.ToString());
            m_widgetInfoLookup.SetValue("FormPressureTransducerParameters", "numericUpDownPressureMax", m_fPressureMax.ToString()); //4/21/11
            m_widgetInfoLookup.SetValue("FormPressureTransducerParameters", "RECEIVER_TYPE_COMBOBOX", m_iReceiverType.ToString()); //4/29/11

            m_widgetInfoLookup.SetValue("FormPressureTransducerParameters", "textBoxTransducerGain", m_fTransducerGain.ToString());
            m_widgetInfoLookup.Save();
            m_fTransducerScale = GetTransducerScale(System.Convert.ToInt32(m_sTransducerType));

            CLogTransducerSetting log = new CLogTransducerSetting(ref m_dbCnn);            
            CLogTransducerSettingRecord rec = GetRecord();
            log.Save(rec);            
        }

        public string GetClientPacket()
        {
            string sRetVal = "";

            sRetVal = CCommonTypes.PACKET_ID + "=" + ((int)CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_PRESSURE_TRANSDUCER_SETTINGS).ToString() +
                      ";comboBoxTransducerType=" + m_sTransducerType +
                      ";comboBoxPressureUnits=" + ((int)m_PressureUnit).ToString() +
                      ";textBoxPressureOffset=" + m_fPressureOffset.ToString() +
                      ";numericUpDownPressureMax=" + m_fPressureMax.ToString() + //4/21/22
                      ";RECEIVER_TYPE_COMBOBOX=" + m_iReceiverType.ToString() + //4/29/22
                      ";textBoxTransducerGain=" + m_fTransducerGain.ToString();

            return sRetVal;
        }
        
        public void SetPressureOffset(float fVal_)
        {
            m_fPressureOffset = fVal_;
        }

        public void SetPressureMax(float fVal_)
        {
            m_fPressureMax = fVal_; //4/21/22
        }

        public void SetTransducerGain(float fVal_)
        {
            m_fTransducerGain = fVal_;
        }

        public void SetPressureUnit(PRESSURE_UNIT iUnitType_)
        {            
            m_PressureUnit = iUnitType_;            
        }

        public PRESSURE_UNIT GetPressureUnit()
        {
            return m_PressureUnit;
        }

        public float GetPressureOffset()
        {
            return m_fPressureOffset;
        }

        public float GetPressureMax() //4/21/11
        {
            return m_fPressureMax;
        }

        public float GetTransducerScale()
        {
            return m_fTransducerScale;
        }

        public float GetTransducerGain()
        {
            return m_fTransducerGain;
        }

        public int GetTransducerType()
        {
            return System.Convert.ToInt32(m_sTransducerType);
        }

        public void SetTransducerType(int iType_)
        {
            m_sTransducerType = iType_.ToString();
        }

        //4/29/22
        public int GetReceiverType()
        {
            return m_iReceiverType;
        }

        public void SetReceiverType(int iType_)
        {
            m_iReceiverType = iType_;
        }
        //4/29/22

        public float GetTransducerScale(int iType_)
        {
            float fRetVal = TRANSDUCER_SCALE_PSI_10000;
            switch (iType_)
            {
                case 0:
                    fRetVal = TRANSDUCER_SCALE_PSI_3000;
                    break;
                case 1:
                    fRetVal = TRANSDUCER_SCALE_PSI_5000;
                    break;
                case 2:
                    fRetVal = TRANSDUCER_SCALE_PSI_10000;
                    break;
            }

            if (m_PressureUnit == PRESSURE_UNIT.KPA)
                fRetVal *= PSI_TO_KPA;

            return fRetVal;
        }
    }
}
