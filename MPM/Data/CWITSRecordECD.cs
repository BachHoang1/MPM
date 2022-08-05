// author: hoan chau
// purpose: handle ecd and hydrostatic pressure wits out

using MPM.DataAcquisition.Helpers;
using MPM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    class CWITSRecordECD : CWITSRecord
    {
        protected override CWITSLookupTable.WITSChannel Find(CEventDPoint e)
        {
            CWITSLookupTable.WITSChannel channel = m_lookupWITS.Find(e.m_ID);
            return channel;
        }

        public override string GatherData()
        {
            string sRetVal = "";
            // ecd         
            sRetVal += m_lstChannel[0].sChannelID + m_lstChannel[0].sValue + "\r\n";
            // hydrostatic pressure
            sRetVal += m_lstChannel[1].sChannelID + m_lstChannel[1].sValue + "\r\n";

            return sRetVal;
        }

        protected override bool IsReady()
        {
            bool bRetVal = false;
            if (m_lstChannel.Count == 2)
            {
                if (m_lstChannel[0].bSet && m_lstChannel[1].bSet)
                    bRetVal = true;
            }
            return bRetVal;
        } 
        
        private void Reset()
        {
            for (int i = 0; i < m_lstChannel.Count; i++)
            {
                m_lstChannel[i].bSet = false;
                m_lstChannel[i].sValue = "-9999.0";
            }
        }

        protected override void WITSChanged(object sender, CEventDPoint e)
        {            
            if (e.m_ID == (int)Command.COMMAND_RESP_PACKET_TYPE)
                Reset();

            if (e.m_ID == (int)Command.COMMAND_ECD_TVD ||
                e.m_ID == (int)Command.COMMAND_HYDRO_STATIC_PRESSURE)
            {                
                CWITSLookupTable.WITSChannel channel = m_lookupWITS.Find(e.m_ID);

                if (channel.iAPSMessageCode != -1)
                {
                    string sChannelID = m_lookupWITS.Find2(e.m_ID);

                    for (int i = 0; i < m_lstChannel.Count; i++)
                    {
                        if (m_lstChannel[i].sChannelID == sChannelID)
                        {
                            if (channel.sOutlinerMin.ToLower() != "none")
                            {
                                float fMin;
                                bool bParseable = System.Single.TryParse(channel.sOutlinerMin, out fMin);
                                if (bParseable)
                                {
                                    float fVal = System.Single.Parse(e.m_sValue);
                                    if (fVal < fMin)  // out of bounds
                                        continue;
                                }
                            }

                            if (channel.sOutlinerMax.ToLower() != "none")
                            {
                                float fMax;
                                bool bParseable = System.Single.TryParse(channel.sOutlinerMax, out fMax);
                                if (bParseable)
                                {
                                    float fVal = System.Single.Parse(e.m_sValue);
                                    if (fVal > fMax)  // out of bounds
                                        continue;
                                }
                            }

                            if (e.m_bIsParityError)
                                if (channel.sSendIfError.ToLower() != "yes")
                                    continue;

                            if (channel.sMath.ToLower() != "none")
                            {
                                string sExecuteFormula = channel.sMath.Replace("this", e.m_sValue);
                                CEvaluate eval = new CEvaluate();
                                double dblVal = eval.Do(sExecuteFormula);
                                if (dblVal != CEvaluate.BAD_VALUE)
                                    m_lstChannel[i].sValue = dblVal.ToString();
                            }
                            else
                                m_lstChannel[i].sValue = e.m_sValue;

                            m_lstChannel[i].bSet = true;
                            
                            break;
                        }
                    }

                    if (IsReady())
                    {
                        string sData = GatherData();
                        if (sData.Length > 0)
                        {
                            CEventSendWITSData eventData = new CEventSendWITSData();
                            eventData.m_sData = "&&\r\n" + sData + "!!\r\n";
                            OnReadyToTransmit(eventData);
                        }

                        // reset
                        Reset();
                    }
                }
            }
            // search for the wits id using the message id  
            

        }

    }
}
