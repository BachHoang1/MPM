// author: hoan chau
// purpose: child class that implements conversion for pressure units

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    public class CUnitPressure : CBaseUnitConversion
    {     
        public CUnitPressure()
        {
            m_sImperialDesc = "Psi";
            m_sMetricDesc = "KPa";
        }   

        protected override float ConvertToImperial(float fFromVal_)
        {
            float fToVal = fFromVal_ * 0.1450f;  // to psi
            return fToVal;
        }

        protected override float ConvertToMetric(float fFromVal_)
        {
            float fToVal = fFromVal_ * 6.8947f;  // to kpa
            return fToVal;
        }

        public float ToMetric(float fFromVal_)
        {
            float fToVal = fFromVal_ * 6.8947f;  // to kpa
            return fToVal;
        }
    }
}
