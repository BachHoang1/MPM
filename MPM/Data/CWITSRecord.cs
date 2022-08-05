// author: hoan chau
// purpose: base class for processing WITS records

using MPM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CWITSRecord
    {
        protected class WITSInfo
        {
            public string sChannelID;
            public string sValue;  // 
            public bool bSet;  // indicates if value has been set
        }


        protected List<WITSInfo> m_lstChannel; // list of channels that make up a record
        public CCommonTypes.TELEMETRY_TYPE m_iTechnology;
        //private DateTime m_dtStartTime;  // when the record began (when it was received from Detect)
                                         // or should it be based on when it was decoded in Detect?

        protected bool m_bSendMudPulse;
        protected bool m_bSendEM;

        private CDetectDataLayer m_DataLayer;

        public delegate void EventHandler(object sender, CEventSendWITSData e);
        public event EventHandler Transmit;

        protected CWITSLookupTable m_lookupWITS;
        

        public CWITSRecord()
        {
            m_lstChannel = new List<WITSInfo>();
            
        }

        public void SetWITSLookUpTable(ref CWITSLookupTable witsLookUpTbl_)
        {
            m_lookupWITS = witsLookUpTbl_;
        }

        public void SetTechnology(CCommonTypes.TELEMETRY_TYPE iTech_)
        {
            m_iTechnology = iTech_;
        }

        public void AddChannel(string sVal_)
        {
            WITSInfo wi = new WITSInfo();
            wi.bSet = false;
            wi.sChannelID = sVal_;
            wi.sValue = "-9999.0";
            m_lstChannel.Add(wi);
        }

        public void SetChannelID(string sVal_)
        {
            m_lstChannel[0].sChannelID = sVal_;
        }

        public void ClearChannels()
        {
            m_lstChannel.Clear();
        }


        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(WITSChanged);
        }

        protected virtual void OnReadyToTransmit(CEventSendWITSData e)
        {            
            Transmit(this, e);
        }

        protected virtual bool IsReady()
        {
            bool bRetVal = true;
            for (int i = 0; i < m_lstChannel.Count; i++)
            {
                if (!m_lstChannel[i].bSet)
                {
                    bRetVal = false;
                    break;
                }
            }
            return bRetVal;
        }

        public virtual string GatherData()
        {
            string sRetVal = "";
            for (int i = 0; i < m_lstChannel.Count; i++)
                sRetVal += m_lstChannel[i].sChannelID + m_lstChannel[i].sValue + "\r\n";

            return sRetVal;
        }

        protected virtual CWITSLookupTable.WITSChannel Find(CEventDPoint e)
        {            
            CWITSLookupTable.WITSChannel channel = m_lookupWITS.Find(e.m_ID);
            return channel;
        }

        public void SetFilter(bool bSendMudPulse_, bool bSendEM_)
        {
            m_bSendMudPulse = bSendMudPulse_;
            m_bSendEM = bSendEM_;
        }

        public void GetFilter(out bool bSendMudPulse_, out bool bSendEM_)
        {
            bSendMudPulse_ = m_bSendMudPulse;
            bSendEM_ = m_bSendEM;
        }

        protected virtual void WITSChanged(object sender, CEventDPoint e)
        {
            // search for the wits id using the message id           
            CWITSLookupTable.WITSChannel channel = Find(e);
            if (channel.iAPSMessageCode != -1)
            {
                if (e.m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_MP && !m_bSendMudPulse)
                    return;
                else if (e.m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_EM && !m_bSendEM)
                    return;
                //else if (e.m_bIsParityError)
                //    return;
                
                for (int i = 0; i < m_lstChannel.Count; i++)
                {
                    if (m_lstChannel[i].sChannelID == channel.sID)
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
