// author: hoan chau
// purpose: child class that implements unit conversion for length
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    public class CUnitLength : CBaseUnitConversion
    {
        public CUnitLength()
        {
            m_sImperialDesc = "ft";
            m_sMetricDesc = "m";
        }

        protected override float ConvertToImperial(float fFromVal_)
        {
            float fToVal = fFromVal_ * 3.2808f;  // to feet
            return fToVal;
        }

        protected override float ConvertToMetric(float fFromVal_)
        {
            float fToVal = fFromVal_ * 0.3048f;  // to meters
            return fToVal;
        }       
    }
}
