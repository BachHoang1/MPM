// author: hoan chau
// purpose: child class that implements unit conversion for length
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    public class CUnitDensity : CBaseUnitConversion
    {
        const float FACTOR = 0.1198264f;
        public CUnitDensity()
        {
            m_sImperialDesc = "lb/gal";
            m_sMetricDesc = "kg/m3";
        }

        protected override float ConvertToImperial(float fFromVal_)
        {
            float fToVal = fFromVal_ / FACTOR;  // to lb/gal
            return fToVal;
        }

        protected override float ConvertToMetric(float fFromVal_)
        {
            float fToVal = fFromVal_ * FACTOR;  // to t/m3
            return fToVal;
        }
    }
}
