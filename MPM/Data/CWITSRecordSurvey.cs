// author: hoan chau
// purpose: allows wits records that contain survey data to bypass 
//          detect event messages and just get their data
//          from the accept/reject mechanism

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CWITSRecordSurvey : CWITSRecord
    {

        public void SetValue(string sChannelID_, float fVal_)
        {
            for (int i = 0; i < m_lstChannel.Count; i++)
            {
                WITSInfo wi = m_lstChannel[i];
                if (wi.sChannelID == sChannelID_)
                {
                    wi.sValue = fVal_.ToString();
                }
            }
        }

    }
}
