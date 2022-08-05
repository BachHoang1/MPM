// author: hoan chau
// pupose: to calculate the equivalent circulating density and then send to WITS

using MPM.DataAcquisition.Helpers;
using MPM.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CEquivalentCirculatingDensity
    {
        private const int MIN_PACKET_ID = 0;
        private const int MAX_PACKET_ID = 16;

        public const float ECD_FACTOR_IMPERIAL = .052f;
        public const float ECD_FACTOR_METRIC = 1.4223f;
        public const float HYDROSTATIC_FACTOR_IMPERIAL = .052f;
        public const float HYDROSTATIC_FACTOR_METRIC = 9.81f;

        public struct REC
        {
            public DateTime dtCreated;  // when record was created 

            public float fBitDepth;

            public float fDirToBit;  // distance between directional sensor and drill bit
            public CCommonTypes.SURVEY_TYPE Type;
            public float fInclination;
            public float fAzimuth;
            
            public float fAnnularPressure;  // psi
            
            public float fMudWeight;  // ppg  value is from settings
        }

        public DbConnection m_dbCnn;
        private CDPointLookupTable m_DPointTable;
        private CWITSLookupTable m_LookUpWITS;

        private REC m_Rec;
        private CDetectDataLayer m_DataLayer;

        public delegate void EventAnnularPressure(object sender, CEventECD e);
        public event EventAnnularPressure DisplayECD;

        public delegate void EventHandler(object sender, CEventSendWITSData e);

        CUnitSelection m_unitSelection;
        public CCommonTypes.UNIT_SET m_iUnitSet;

        private float m_fTVD;
        private float m_fMudDensity;
        private CCommonTypes.TELEMETRY_TYPE m_TelemetryType;
        CUnitPressure uPressure = new CUnitPressure();
        

        public CEquivalentCirculatingDensity(ref DbConnection dbCnn_, ref CWITSLookupTable witsLookUpTbl_, ref CDPointLookupTable DPointTable_, CUnitSelection unitSelection_)
        {
            m_dbCnn = dbCnn_;
            m_LookUpWITS = witsLookUpTbl_;
            m_DPointTable = DPointTable_;
            m_unitSelection = unitSelection_;
            m_iUnitSet = m_unitSelection.GetUnitSet();
            CSurveyLog log = new CSurveyLog(ref m_dbCnn);

            m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_NONE;

            Init();
        }

        public void Init()
        {
            m_Rec.fDirToBit = 0.0f;
            m_Rec.fInclination = CCommonTypes.BAD_VALUE;
            m_Rec.fAzimuth = CCommonTypes.BAD_VALUE;
            m_Rec.fAnnularPressure = CCommonTypes.BAD_VALUE;
            //m_Rec.fBorePressure = CCommonTypes.BAD_VALUE;
        }

        private CCommonTypes.SURVEY_TYPE GetEnumeratedType(int iVal_)
        {
            CCommonTypes.SURVEY_TYPE typeRetVal = CCommonTypes.SURVEY_TYPE.UNKNOWN;
            Array values = Enum.GetValues(typeof(CCommonTypes.SURVEY_TYPE));
            foreach (CCommonTypes.SURVEY_TYPE val in values)
            {
                if (iVal_ == (int)val)
                {
                    typeRetVal = val;
                    break;
                }
            }

            return typeRetVal;
        }
        
        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ProcessDPoint);
        }
        public void SetUnitSet(CCommonTypes.UNIT_SET unitSet_)
        {
            m_iUnitSet = unitSet_;
        }

        public void SetParameters(float fTVD_, float fMudDensity_)
        {
            m_fTVD = fTVD_;
            m_fMudDensity = fMudDensity_;
        }

        public void GetParameters(out float fTVD_, out float fMudDensity_)
        {
            fTVD_ = m_fTVD;
            fMudDensity_ = m_fMudDensity;
        }

        protected void ProcessDPoint(object sender, CEventDPoint e)
        {
            float fResult = 0.0f;
            if (float.TryParse(e.m_sValue, out fResult))
                if (!e.m_bIsParityError)
                    SetValue(e.m_iTechnology, e.m_ID, fResult);
                else
                    Debug.WriteLine("PARITY ERROR for " + e.m_ID);

            // other d-points from triggering the same survey
            if (IsReady())
            {
                try
                {
                    //bool bIsMetric = false;
                    //if (GetLengthUnit().ToLower() == "m")
                    //    bIsMetric = true;

                    // calculate ecd                
                    //float fECD = CalculateECD(m_Rec.fBorePressure, m_Rec.fAnnularPressure, m_fTVD, m_fMudDensity, bIsMetric);
                    //float fHSP = CalculateHydrostaticPressure(m_fTVD, m_fMudDensity, bIsMetric);

                    // get the value and send to WITS
                    //CEventSendWITSData evt = new CEventSendWITSData();

                    
                    //string sChannelECD = m_LookUpWITS.Find2((int)Command.COMMAND_ECD_TVD);
                    //string sChannelHSP = m_LookUpWITS.Find2((int)Command.COMMAND_HYDRO_STATIC_PRESSURE);

                    //string sWITSVal = "&&\r\n" + sChannelECD + fECD.ToString() + "\r\n" + sChannelHSP + fHSP.ToString() + "\r\n" + "!!\r\n";
                    //evt.m_sData = sWITSVal;

                    ////Transmit(this, evt);

                    //SaveToDB(fECD, fHSP);


                    

                    // save ecd to database                
                    // send to WITS
                    // display to user
                    //CEventECD eventData = new CEventECD();
                    //eventData.fAnnularPressure = m_Rec.fAnnularPressure;

                    //OnReadyToDisplay(eventData);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                
                Init();
            }
        }

        private void SaveToDB(float fECD_, float fHSP_)
        {
            DateTime dt = DateTime.Now;

            // save the ecd
            CLogDataRecord recECD = new CLogDataRecord();
            recECD.iMessageCode = (int)Command.COMMAND_ECD_TVD;
            recECD.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            recECD.iDate = System.Convert.ToInt32(dt.ToString("yyyyMMdd"));
            recECD.iTime = System.Convert.ToInt32(dt.ToString("HHmmss"));
            recECD.sName = "ECD";
            recECD.sValue = fECD_.ToString();
            recECD.sUnit = GetDensityUnit();
            recECD.bParityError = false;
            recECD.fDepth = m_Rec.fBitDepth;
            recECD.sTelemetry = GetTelemetryType() == CCommonTypes.TELEMETRY_TYPE.TT_EM ? "EM" : "MP";

            CLogDataLayer logDetect = new CLogDataLayer(ref m_dbCnn);
            logDetect.Save(recECD);

            // save the hydrostatic pressure
            CLogDataRecord recHSP = new CLogDataRecord();
            recHSP.iMessageCode = (int)Command.COMMAND_HYDRO_STATIC_PRESSURE;
            recHSP.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            recHSP.iDate = System.Convert.ToInt32(dt.ToString("yyyyMMdd"));
            recHSP.iTime = System.Convert.ToInt32(dt.ToString("HHmmss"));
            recHSP.sName = "Hydrostatic Pressure";
            recHSP.sValue = fHSP_.ToString();
            recHSP.sUnit = GetPressureUnit();
            recHSP.bParityError = false;
            recHSP.fDepth = m_Rec.fBitDepth;
            recHSP.sTelemetry = GetTelemetryType() == CCommonTypes.TELEMETRY_TYPE.TT_EM ? "EM" : "MP";

            logDetect.Save(recHSP);
        }

        private string GetPressureUnit()
        {
            string sPressureUnit = "psi";
            if (GetLengthUnit().ToLower() == "m")
                sPressureUnit = "kpa";
            return sPressureUnit;
        }

        private string GetDensityUnit()
        {
            string sDensityUnit = "lb/gal";
            if (GetLengthUnit().ToLower() == "m")
                sDensityUnit = "kg/m3";

            return sDensityUnit;
        }

        protected virtual void OnReadyToDisplay(CEventECD e)
        {
            DisplayECD(this, e);
        }

        public float CalculateECD(float fBorePressure_, float fAnnularPressure_, double dblTVD_, float fMudDensity_, bool bIsMetric_)
        {
            float fRetVal = CCommonTypes.BAD_VALUE;

            if (dblTVD_ > 0)
            {
                float fFactor = ECD_FACTOR_IMPERIAL;
                if (bIsMetric_)
                    fFactor = ECD_FACTOR_METRIC;

                fRetVal = ((fAnnularPressure_ - (fMudDensity_ * (float)dblTVD_ * fFactor)) / ((float)dblTVD_ * fFactor)) + fMudDensity_;
            }
                

            return fRetVal;
        }

        public float CalculateECD()
        {
            float fRetVal = CCommonTypes.BAD_VALUE;

            if (m_fTVD > 0)
            {
                float fFactor = ECD_FACTOR_IMPERIAL;
                if (GetLengthUnit().ToLower() == "m")  // metric
                    fFactor = ECD_FACTOR_METRIC;

                fRetVal = ((m_Rec.fAnnularPressure - (m_fMudDensity * m_fTVD * fFactor)) / (m_fTVD * fFactor)) + m_fMudDensity;
                   
            }

            return fRetVal;
        }

        public float CalculateHydrostaticPressure(double dblTVD_, float fMudDensity_, bool bIsMetric_)
        {
            float fRetVal = CCommonTypes.BAD_VALUE;

            if (bIsMetric_)
                fRetVal = fMudDensity_ * (float)dblTVD_ * HYDROSTATIC_FACTOR_METRIC;
            else
                fRetVal = fMudDensity_ * (float)dblTVD_  * HYDROSTATIC_FACTOR_IMPERIAL;

            return fRetVal;
        }

        public float CalculateHydrostaticPressure()
        {
            float fRetVal = CCommonTypes.BAD_VALUE;

            if (GetLengthUnit().ToLower() == "m")  // metric
                fRetVal = m_fMudDensity * (float)m_fTVD * HYDROSTATIC_FACTOR_METRIC;
            else
                fRetVal = m_fMudDensity * (float)m_fTVD * HYDROSTATIC_FACTOR_IMPERIAL;

            return fRetVal;
        }

        public float GetMudDensity()
        {
            return m_fMudDensity;
        }

        public double GetTVD(CSurvey.REC recSvy)
        {
            double dblRetVal = 0;

            DataTable tblSvy = new DataTable();

            CSurveyLog svyLog = new CSurveyLog(ref m_dbCnn);
            // get only the surveys that occurred before this one
            tblSvy = svyLog.Get(true, 0, recSvy.fBitDepth - recSvy.fDirToBit - 1.0f, false);

            CSurvey.REC rec1 = new CSurvey.REC();
            CSurvey.REC rec2 = new CSurvey.REC();

            if (tblSvy.Rows.Count < 1)
            {
                rec1.fAzimuth = 0;
                rec1.fInclination = 0;
                rec1.fBitDepth = 0;
            }
            else
            {
                DataRow dr = tblSvy.Rows[tblSvy.Rows.Count - 1];
                rec1.fBitDepth = System.Convert.ToSingle(dr.ItemArray[0]);
                rec1.fSurveyDepth = System.Convert.ToSingle(dr.ItemArray[1]);
                rec1.dtCreated = System.Convert.ToDateTime(dr.ItemArray[2]);
                rec1.fInclination = System.Convert.ToSingle(dr.ItemArray[3]);
                rec1.fAzimuth = System.Convert.ToSingle(dr.ItemArray[4]);
            }

            rec2 = recSvy;

            // calculate the tvd 
            CSurveyCalculation calc = new CSurveyCalculation();
            dblRetVal = calc.GetTVD(rec1, rec2);
            double dblTVD = svyLog.GetTVD(rec1.dtCreated);
            if (tblSvy.Rows.Count > 0)
                dblRetVal += dblTVD;

            return dblRetVal;
        }

        public void SetValue(CCommonTypes.TELEMETRY_TYPE tt_, int iMessageCode_, float fVal_)
        {            
            switch (iMessageCode_)
            {                
                case (int)Command.COMMAND_RESP_PACKET_TYPE:
                    if ((int)fVal_ < MIN_PACKET_ID || (int)fVal_ > MAX_PACKET_ID)  // bogus
                        break;
                    m_Rec.Type = GetEnumeratedType((int)fVal_);
                    Init();
                    break;
                case (int)Command.COMMAND_BIT_DEPTH:
                    m_Rec.fBitDepth = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_INCLINATION:
                    m_Rec.fInclination = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_AZIMUTH:
                    m_Rec.fAzimuth = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_A_PRESSURE:
                    m_Rec.fAnnularPressure = fVal_;
                    //if (GetPressureUnit().ToLower() == "kpa")
                    //{                        
                    //    m_Rec.fAnnularPressure = uPressure.ToMetric(m_Rec.fAnnularPressure);
                    //}
                    m_TelemetryType = tt_;
                    break;
                case (int)Command.COMMAND_RESP_B_PRESSURE:
                    //m_Rec.fBorePressure = fVal_;
                    //if (GetPressureUnit().ToLower() == "kpa")
                    //{
                    //    m_Rec.fBorePressure = uPressure.ToMetric(m_Rec.fBorePressure);
                    //}
                    break;
                default:
                    break;
            }
        }

        public bool IsReady()
        {
            bool bRetVal = false;
            
            if (//m_Rec.fInclination > CCommonTypes.BAD_VALUE &&
                //m_Rec.fAzimuth > CCommonTypes.BAD_VALUE &&
                m_Rec.fAnnularPressure > CCommonTypes.BAD_VALUE)
            {
                bRetVal = true;
            }
                       
            return bRetVal;
        }

        private string GetNativeLengthUnit()
        {
            CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get((int)Command.COMMAND_BIT_DEPTH);
            return dpi.sUnits;
        }

        public string GetLengthUnit()
        {
            string sLengthUnit = "ft";
            CUnitLength unitLength = new CUnitLength();
            if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                sLengthUnit = unitLength.GetImperialUnitDesc();
            else if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                sLengthUnit = unitLength.GetMetricUnitDesc();
            else  // get the value unit from the specific d-point                            
                sLengthUnit = GetNativeLengthUnit();

            return sLengthUnit;
        }

        public float GetDepth()
        {
            return m_Rec.fBitDepth;
        }

        public CCommonTypes.TELEMETRY_TYPE GetTelemetryType()
        {
            return m_TelemetryType;
        }
    }
}
