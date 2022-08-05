// author: hoan chau
// purpose: maintain a single survey record with identifiers to determine the type of survey 

using MPM.DataAcquisition.Helpers;
using MPM.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MPM.Data
{
    public class CSurvey
    {
        private const int MIN_PACKET_ID = 0;
        private const int MAX_PACKET_ID = 16;        

        public enum STATUS {REJECT, ACCEPT, NONE};

        public struct REC
        {            
            public DateTime dtCreated;
            public STATUS Status;
            public CCommonTypes.SURVEY_TYPE Type;
            public CCommonTypes.TELEMETRY_TYPE TelemetryType;
            

            public float fBitDepth;
            public float fDirToBit;  // distance between directional sensor and drill bit
            public float fSurveyDepth;

            public float fInclination;
            public float fAzimuth;

            // qualifiers
            public float fDipAngle;
            public float fMTotal;
            public float fGTotal;

            // raw axes
            public float fAX;
            public float fAY;
            public float fAZ;
            public float fMX;
            public float fMY;
            public float fMZ;                    
        }

        public struct REC_CALC
        {
            public DateTime dtCreated;  // when record was created
            public DateTime dtCreatedSvy;  // used as a join to the tblSurvey            
            public double fCourseLength;
            public double fTVD;            
            public double fNS;
            public double fEW;
            public double fDLS;
        }

        private REC m_Rec;
        private DateTime m_dtNow;
        private int m_iDBRecCounter;

        private CDetectDataLayer m_DataLayer;

        public delegate void EventSurveyDone(object sender, CEventSurvey e);
        public event EventSurveyDone DisplaySurvey;
        public DbConnection m_dbCnn;
        private CDPointLookupTable m_DPointTable;

        private float m_fLastDirToBit;

        CUnitSelection m_unitSelection;
        public CCommonTypes.UNIT_SET m_iUnitSelection;

        //CWITSRecord m_WITSSurveyRec;   // surveys are sent as a single record     
        //CWITSRecord m_WITSSurveyVectorRec;   // vector surveys are sent as a single record 

        public CSurvey(ref DbConnection dbCnn_, ref CDPointLookupTable DPointTable_, CUnitSelection unitSelection_)
        {
            m_dbCnn = dbCnn_;
            m_DPointTable = DPointTable_;
            m_unitSelection = unitSelection_;
            m_iUnitSelection = m_unitSelection.GetUnitSet();
            CSurveyLog log = new CSurveyLog(ref m_dbCnn);            
            m_iDBRecCounter = log.GetCount() + 1;
            Init();               
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

        public void SetUnit(CUnitSelection unitSelection_)
        {
            m_unitSelection = unitSelection_;
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ProcessDPoint);
        }

        protected virtual void OnReadyToDisplay(CEventSurvey e)
        {
            DisplaySurvey(this, e);
        }

        protected void ProcessDPoint(object sender, CEventDPoint e)
        {            
            float fResult = 0.0f;
            if (float.TryParse(e.m_sValue, out fResult))
                if (!e.m_bIsParityError)                   
                    SetValue(e.m_iTechnology, e.m_ID, fResult);
                else
                    Debug.WriteLine("PARITY ERROR for " + e.m_ID);
            
            // is this survey ready? check the future date to prevent 
            // other d-points from triggering the same survey
            if (IsReady() && m_Rec.dtCreated >= m_dtNow)
            {
                //System.Diagnostics.Debug.WriteLine("Survey is ready!");
                m_dtNow = new DateTime(9999, 12, 31); // year the world ends 
                m_Rec.dtCreated = DateTime.Now;
                
                // save entire record to database
                CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                log.SetLengthUnit(GetLengthUnit());
                log.Save(m_Rec);
                CSurvey.REC_CALC rekCalc = log.Calculate(m_Rec);
                log.SaveCalc(rekCalc);

                // display to user
                CEventSurvey eventData = new CEventSurvey();
                eventData.rec = m_Rec;
                eventData.m_iDatabaseID = m_iDBRecCounter;
                OnReadyToDisplay(eventData);  

                Init();                
            }
        }

        private string GetNativeLengthUnit()
        {
            CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get((int)Command.COMMAND_BIT_DEPTH);
            return dpi.sUnits;
        }

        private string GetLengthUnit()
        {
            string sLengthUnit = "ft";
            CUnitLength unitLength = new CUnitLength();
            if (m_iUnitSelection == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                sLengthUnit = unitLength.GetImperialUnitDesc();
            else if (m_iUnitSelection == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                sLengthUnit = unitLength.GetMetricUnitDesc();
            else  // get the value unit from the specific d-point                            
                sLengthUnit = GetNativeLengthUnit();

            return sLengthUnit;
        }


        public void SetDepth(float fVal_)
        {
            m_Rec.fBitDepth = fVal_;
        }

        public float GetLastDirToBit()
        {
            return m_fLastDirToBit;
        }

        public void SetValue(CCommonTypes.TELEMETRY_TYPE tt_, int iMessageCode_, float fVal_)
        {
            switch (iMessageCode_)
            {
                case (int)Command.COMMAND_BIT_DEPTH:
                    m_Rec.fBitDepth = fVal_;                    
                    break;
                case (int)Command.COMMAND_RESP_PACKET_TYPE:
                    if (fVal_ < MIN_PACKET_ID || fVal_ > MAX_PACKET_ID)  // bogus
                        break;
                    m_Rec.Type = GetEnumeratedType((int)fVal_);
                    CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                    m_iDBRecCounter = log.GetCount() + 1;
                    Init();
                    break;
                case (int)Command.COMMAND_RESP_DIRTOBIT:
                    m_fLastDirToBit = m_Rec.fDirToBit = fVal_ / 100.0f;
                    break;
                case (int)Command.COMMAND_RESP_INCLINATION:
                    m_Rec.TelemetryType = tt_;
                    m_Rec.fInclination = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_AZIMUTH:
                    m_Rec.fAzimuth = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_GT:
                    m_Rec.fGTotal = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_BT:
                    m_Rec.fMTotal = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_DIPANGLE:
                    m_Rec.fDipAngle = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_GX:
                    m_Rec.fAX = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_GY:
                    m_Rec.fAY = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_GZ:
                    m_Rec.fAZ = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_BX:
                    m_Rec.fMX = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_BY:
                    m_Rec.fMY = fVal_;
                    break;
                case (int)Command.COMMAND_RESP_BZ:
                    m_Rec.fMZ = fVal_;
                    break;
                default:
                    break;
            }
        }

        public bool IsReady()
        {
            bool bRetVal = false;
            if (m_Rec.Type.ToString().ToLower().Contains("vector"))
            {
                if (m_Rec.fMX > CCommonTypes.BAD_VALUE &&
                    m_Rec.fMY > CCommonTypes.BAD_VALUE &&
                    m_Rec.fMZ > CCommonTypes.BAD_VALUE &&
                    m_Rec.fAX > CCommonTypes.BAD_VALUE &&
                    m_Rec.fAY > CCommonTypes.BAD_VALUE &&
                    m_Rec.fAZ > CCommonTypes.BAD_VALUE &&
                    m_Rec.fInclination > CCommonTypes.BAD_VALUE &&
                    m_Rec.fAzimuth > CCommonTypes.BAD_VALUE &&
                    m_Rec.fDipAngle > CCommonTypes.BAD_VALUE &&
                    m_Rec.fMTotal > CCommonTypes.BAD_VALUE &&
                    m_Rec.fGTotal > CCommonTypes.BAD_VALUE)
                {
                    bRetVal = true;
                }
            }
            else  // not a vector survey
            {
                if (m_Rec.fInclination > CCommonTypes.BAD_VALUE &&
                    m_Rec.fAzimuth > CCommonTypes.BAD_VALUE &&
                    m_Rec.fDipAngle > CCommonTypes.BAD_VALUE &&
                    m_Rec.fMTotal > CCommonTypes.BAD_VALUE &&
                    m_Rec.fGTotal > CCommonTypes.BAD_VALUE)
                {
                    bRetVal = true;
                }
            }
            
            return bRetVal;
        }

        public void Init()
        {
            m_Rec.TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_NONE;

            m_dtNow = m_Rec.dtCreated = DateTime.Now;            

            m_Rec.fBitDepth = CCommonTypes.BAD_VALUE;
            m_Rec.fDirToBit = 0.0f;
            m_Rec.fSurveyDepth = CCommonTypes.BAD_VALUE;

            
            m_Rec.fInclination = CCommonTypes.BAD_VALUE;
            m_Rec.fAzimuth = CCommonTypes.BAD_VALUE;

            // qualifiers
            m_Rec.fDipAngle = CCommonTypes.BAD_VALUE;
            m_Rec.fMTotal = CCommonTypes.BAD_VALUE;
            m_Rec.fGTotal = CCommonTypes.BAD_VALUE;

            // raw axes
            m_Rec.fAX = CCommonTypes.BAD_VALUE;
            m_Rec.fAY = CCommonTypes.BAD_VALUE;
            m_Rec.fAZ = CCommonTypes.BAD_VALUE;
            m_Rec.fMX = CCommonTypes.BAD_VALUE;
            m_Rec.fMY = CCommonTypes.BAD_VALUE;
            m_Rec.fMZ = CCommonTypes.BAD_VALUE;

            m_Rec.Status = STATUS.NONE;            
        }
    }
}

