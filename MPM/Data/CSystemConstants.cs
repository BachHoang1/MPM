// author: unknown
// purpose: use to reference defaults for various types of drilling parameters
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    class CSystemConstants
    {
        public static int numMultiGaugeValues = 7;
        public static int NotSetInt = -9999999;
        public static double NotSetDouble = -9999999.9;
        public static double PsiToBarFactor = 0.0689475729;
        public static double DefaultToleranceTotalMag = 0.003;
        public static double DefaultToleranceTotalGrav = 1.0 / 400.0;
        public static double DefaultToleranceDipAngle = 0.45;
        public static double DefaultToleranceSignalNoise = 10.0;
    }
}
