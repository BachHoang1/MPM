// author: hoan chau
// purpose: base class for converting a unit of measurement in one standard to another

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPM.Data;

namespace MPM.Utilities
{
    public class CBaseUnitConversion
    {
        protected CCommonTypes.UNIT_SET m_iUnitType;
        protected string m_sImperialDesc;
        protected string m_sMetricDesc;

        public CBaseUnitConversion()
        {
            m_iUnitType = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
            m_sImperialDesc = "Imperial";
            m_sMetricDesc = "Metric";
        }

        public void SetUnitType(CCommonTypes.UNIT_SET iVal_)
        {
            m_iUnitType = iVal_;
        }

        public string GetImperialUnitDesc()
        {
            return m_sImperialDesc;
        }

        public string GetMetricUnitDesc()
        {
            return m_sMetricDesc;
        }
            
        protected virtual float ConvertToMetric(float fFromVal_)
        {
            float fRetVal = fFromVal_;
            return fRetVal;
        }

        protected virtual float ConvertToImperial(float fFromVal_)
        {
            float fRetVal = fFromVal_;
            return fRetVal;
        }

        public float Convert(float fFromVal_)
        {
            float fToVal = 0.0f;

            if (m_iUnitType == Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                fToVal = ConvertToImperial(fFromVal_);
            else if (m_iUnitType == Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                fToVal = ConvertToMetric(fFromVal_);
            else  // no conversion
                fToVal = fFromVal_;

            return fToVal;
        }

        public float ReverseConvert(float fToVal_)
        {
            float fFromVal = 0.0f;

            if (m_iUnitType == Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                fFromVal = ConvertToMetric(fToVal_);
            else if (m_iUnitType == Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                fFromVal = ConvertToImperial(fToVal_);
            else  // no conversion
                fFromVal = fToVal_;

            return fFromVal;
        }

        public float ConvertInterval(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
