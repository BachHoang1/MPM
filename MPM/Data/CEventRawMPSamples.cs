
// author: hoan chau
// purpose: event containing mud pulse samples that were acquired from Detect
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CEventRawMPSamples : EventArgs
    {
        private const int SAMPLE_SIZE = 225;
        public short[] m_shArrSample; // = new short[SAMPLE_SIZE];
        public DateTime m_TimeOfSamples;  // 
        public int m_iTelemetryType;  // MP = 0 or EM = 1

        public CEventRawMPSamples()
        {

        }

        public CEventRawMPSamples(short []shArrVal_, int iArrSize_, DateTime dt_, int iTelemetryType_)
        {
            // Create the array and fill it
            m_shArrSample = new short[iArrSize_];
            for (int i = 0; i < iArrSize_; i++)
            {
                m_shArrSample[i] = shArrVal_[i];
            }
                

            m_TimeOfSamples = dt_;
            m_iTelemetryType = iTelemetryType_;

            //Console.WriteLine(DateTime.Now + " Sample Count = " + iArrSize_.ToString());
        }        
    }
}
