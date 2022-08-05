// author: hoan chau
// purpose: child class that implements temperature conversions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    public class CUnitTemperature : CBaseUnitConversion
    {   
        public CUnitTemperature()
        {
            m_sImperialDesc = "°F";
            m_sMetricDesc = "°C";
        }  
           
        protected override float ConvertToImperial(float fFromVal_)
        {
            float fToVal = (fFromVal_ * 9.0f / 5.0f) + 32.0f;  // to fahrenheit
            return fToVal;
        }

        protected override float ConvertToMetric(float fFromVal_)
        {
            float fToVal = (fFromVal_ - 32.0f) * 5.0f / 9.0f;  // to celsius
            return fToVal;
        }        
        
    }
}
