// author: hoan chau
// purpose: inherited class that specifically addresses toolface WITS channel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    class CWITSRecordTF : CWITSRecord
    {
        private int m_iIsMagnetic = -1;  // not determined to be magnetic or gravity yet

        protected override CWITSLookupTable.WITSChannel Find(CEventDPoint e)
        {            
            CWITSLookupTable.WITSChannel channel = m_lookupWITS.Find(e.m_ID);
            return channel;
        }

        public override string GatherData()
        {
            string sRetVal = "";
            if (m_iIsMagnetic == 0)   // gravity         
                sRetVal = m_lstChannel[0].sChannelID + m_lstChannel[0].sValue + "\r\n";
            else if (m_iIsMagnetic == 1)  // magnetic
                sRetVal = m_lstChannel[1].sChannelID + m_lstChannel[1].sValue + "\r\n";

            return sRetVal;
        }

        protected override bool IsReady()
        {
            bool bRetVal = false;
            if (m_lstChannel.Count == 2)
            {
                if (m_iIsMagnetic == 0 && m_lstChannel[0].bSet)
                    bRetVal = true;
                else if (m_iIsMagnetic == 1 && m_lstChannel[1].bSet)
                    bRetVal = true;
            }
            return bRetVal;
        }

        protected override void WITSChanged(object sender, CEventDPoint e)
        {
            // search for the wits id using the message id  
                        
            CWITSLookupTable.WITSChannel channel = m_lookupWITS.Find(e.m_ID);
            
            if (channel.iAPSMessageCode != -1)
            {
                if (e.m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_MP && !m_bSendMudPulse)
                    return;
                else if (e.m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_EM && !m_bSendEM)
                    return;
                else if (e.m_bIsParityError)
                    return;

                if (channel.iAPSMessageCode == 6)
                {
                    if (e.m_sValue == "M")
                        m_iIsMagnetic = 1;
                    else if (e.m_sValue == "G")
                        m_iIsMagnetic = 0;
                    else
                        m_iIsMagnetic = -1;
                }
                else
                {
                    CWITSLookupTable.WITSChannel channelTF = new CWITSLookupTable.WITSChannel();
                    if (m_iIsMagnetic == 1)
                        channelTF = m_lookupWITS.Find(e.m_ID, CCommonTypes.MTF);
                    else if (m_iIsMagnetic == 0)
                        channelTF = m_lookupWITS.Find(e.m_ID, CCommonTypes.GTF);
                    else
                        channelTF = m_lookupWITS.Find(e.m_ID, CCommonTypes.MTF);

                    for (int i = 0; i < m_lstChannel.Count; i++)
                    {
                        if (m_lstChannel[i].sChannelID == channelTF.sID)
                        {
                            m_lstChannel[i].bSet = true;
                            m_lstChannel[i].sValue = e.m_sValue;
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
                        for (int i = 0; i < m_lstChannel.Count; i++)
                        {
                            m_lstChannel[i].bSet = false;
                            m_lstChannel[i].sValue = "-9999.0";
                        }
                    }
                }
                

            }

        }
    }
}
