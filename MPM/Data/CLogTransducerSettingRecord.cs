using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CLogTransducerSettingRecord
    {
        
        public int iDate;  // yyyymmdd
        public int iTime;  // hhmmss
        public string sType;  // 3,000, 5,000, or 10,000
        public string sUnit;  // psi, kpa                
        public float fOffset;
        public float fGain;
        public float fPressureMax; //4/19/22

        public int iReceiverType; //4/29/22
    }
}
