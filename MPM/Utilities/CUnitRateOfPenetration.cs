// author: hoan chau
// purpose: converts rate of penetration units, piggybacking (i.e. inheriting functionality) off of CUnitLength

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    public class CUnitRateOfPenetration : CUnitLength
    {
        public CUnitRateOfPenetration()
        {
            m_sImperialDesc = "ft/Hr";
            m_sMetricDesc = "m/Hr";
        }
    }
}
