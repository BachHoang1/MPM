// author: hoan chau
// purpose: event that gets triggered when a dpoint has been sent from Detect

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CEventDPoint: EventArgs
    {
        public int m_ID { get; set; }  // uniquely identifies the D-point for whoever is listening
        
        public string m_sValue { get; set; }
        public bool m_bIsParityError {get; set; }
        public DateTime m_DateTime { get; set; }
        public CCommonTypes.TELEMETRY_TYPE m_iTechnology;

        public string GetTelemetryType()
        {
            string sRetVal = "";

            if (m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                sRetVal = CCommonTypes.EM_SUPER_SCRIPT;
            else if (m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                sRetVal = CCommonTypes.MP_SUPER_SCRIPT;

            return sRetVal;
        }

        public CCommonTypes.TELEMETRY_TYPE GetTechnology()
        {
            return m_iTechnology;
        }
    }

    public class CEventCorrectedDPoint : EventArgs
    {
        public int m_ID { get; set; }  // uniquely identifies the D-point for whoever is listening

        public float m_fValue { get; set; }
        public bool m_bIsParityError { get; set; }
        public DateTime m_DateTime { get; set; }
        public CCommonTypes.TELEMETRY_TYPE m_iTechnology;

        public string GetTelemetryType()
        {
            string sRetVal = "";

            if (m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                sRetVal = CCommonTypes.EM_SUPER_SCRIPT;
            else if (m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                sRetVal = CCommonTypes.MP_SUPER_SCRIPT;

            return sRetVal;
        }

        public CCommonTypes.TELEMETRY_TYPE GetTechnology()
        {
            return m_iTechnology;
        }
    }
}
