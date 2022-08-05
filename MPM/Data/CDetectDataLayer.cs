// author: hoan chau
// purpose: to disseminate data from Detect via event listeners

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MPM.DataAcquisition;


namespace MPM.Data
{
    public delegate void ChangedEventHandler(object sender, CEventDPoint e);
    public delegate void MPSampleAcquiredEventHandler(object sender, CEventRawMPSamples e);
    public delegate void WITSAcquiredEventHandler(object sender, CEventDPoint e);
    public delegate void AcquiredTextEventHandler(object sender, CEventDPointText e);
    public class CDetectDataLayer
    {        
        //private Thread m_threadTestPostData;

        // acquisition events
        public event ChangedEventHandler Changed;
        public event MPSampleAcquiredEventHandler AcquiredMPSamples;
        public event AcquiredTextEventHandler AcquiredEventText;

        CDetect.AcquiredEventHandler m_EventHandler;
        CDetect.AcquiredMPSamplesEventHandler m_EventSamples;
        CDetect.AcquiredTextEventHandler m_EventText;

        //2/23/22
        DataAcqDetect.AcquiredEventHandler m_AcqEventHandler;
        DataAcqDetect.AcquiredMPSamplesEventHandler m_AcqEventSamples;
        DataAcqDetect.AcquiredTextEventHandler m_AcqEventText;
        //2/23/2

        CPason.AcquiredWITSHandler m_EventWITS;

        private CDetect m_Detect;

        private DataAcqDetect m_DataAcqDetect; //2/23/22

        // Pason object was considered a possible way to get WITS directly.  
        // However, it was decided that Detect would send the WITS data over TCP
        private CPason m_Pason;        

        static void Worker(object obj)
        {
            CDetectDataLayer param = (CDetectDataLayer)obj;
            while (true)
            {
                //param.PostData();
                Thread.Sleep(5000);
            }
        }

        // constructor
        public CDetectDataLayer()
        {                        
            //m_threadTestPostData = new Thread(Worker);
            //m_threadTestPostData.Start(this);
        }

        //~CDetectDataLayer()
        //{
        //    // free up thread
        //    //m_threadTestPostData.Abort();
        //}

        public void Init()
        {
            // start the thread that will send data to the GUI windows
        }

        public void Quit()
        {
            //m_threadTestPostData.Abort();
        }        

        public void SetListener(CDetect detect_, CPason pason_)
        {
            m_Detect = detect_;

            m_EventHandler =  new CDetect.AcquiredEventHandler(AcquiredDPointEvent);
            m_Detect.Acquired += m_EventHandler;

            m_EventSamples = new CDetect.AcquiredMPSamplesEventHandler(AcquiredMPSamplesEvent);
            m_Detect.AcquiredMPSamples += m_EventSamples;

            m_EventText = new CDetect.AcquiredTextEventHandler(AcquiredDPointTextEvent);                        
            m_Detect.AcquiredText += m_EventText;

            m_Pason = pason_;

            m_EventWITS = new CPason.AcquiredWITSHandler(AcquiredWITSEvent);
            m_Pason.Acquired += m_EventWITS;
        }


        public void RemoveListener()
        {
            m_Detect.Acquired -= m_EventHandler;
            m_Detect.AcquiredMPSamples -= m_EventSamples;
            m_Detect.AcquiredText -= m_EventText;
            m_Pason.Acquired -= m_EventWITS;
        }

        //2/23/22
        public void SetAcqListener(DataAcqDetect detect_, CPason pason_)
        {
            m_DataAcqDetect = detect_;

            m_EventHandler = new CDetect.AcquiredEventHandler(AcquiredDPointEvent);
            m_DataAcqDetect.Acquired += m_AcqEventHandler;

            m_EventSamples = new CDetect.AcquiredMPSamplesEventHandler(AcquiredMPSamplesEvent);
            m_DataAcqDetect.AcquiredMPSamples += m_AcqEventSamples;

            m_EventText = new CDetect.AcquiredTextEventHandler(AcquiredDPointTextEvent);
            m_DataAcqDetect.AcquiredText += m_AcqEventText;

            m_Pason = pason_;

            m_EventWITS = new CPason.AcquiredWITSHandler(AcquiredWITSEvent);
            m_Pason.Acquired += m_EventWITS;
        }

        public void RemoveAcqListener()
        {
            m_DataAcqDetect.Acquired -= m_AcqEventHandler;
            m_DataAcqDetect.AcquiredMPSamples -= m_AcqEventSamples;
            m_DataAcqDetect.AcquiredText -= m_AcqEventText;
            m_Pason.Acquired -= m_EventWITS;
        }
        //2/23/22




        private void AcquiredDPointEvent(object sender_, CEventDPoint ev_)
        {
            PostData(ev_);
        }

        private void AcquiredMPSamplesEvent(object sender_, CEventRawMPSamples evRaw_)
        {
            PostMPSamples(evRaw_);
        }

        private void AcquiredDPointTextEvent(object sender_, CEventDPointText evText_)
        {
            PostText(evText_);
        }

        private void AcquiredWITSEvent(object sender_, CEventDPoint ev_)
        {
            // m_Pason.QueueOutgoingPacket("&&\r\n0713100\r\n071590\r\n!!\r\n");
            //m_Pason.QueueOutgoingPacket("&&\r\n082180\r\n0823100\r\n!!\r\n");
            //m_Pason.QueueOutgoingPacket("&&\r\n0716180\r\n071745\r\n!!\r\n");
            PostData(ev_);
        }

        private void PostData(CEventDPoint ev_)
        {
            //System.Diagnostics.Debug.Print("PostData");
            
            CEventDPoint ev = new CEventDPoint();
            ev = ev_;
            if (Changed != null)
                Changed(this, ev);
        }


        private void PostMPSamples(CEventRawMPSamples ev_)
        {            
            CEventRawMPSamples evRaw = new CEventRawMPSamples();
            evRaw = ev_;
            if (AcquiredMPSamples != null)
                AcquiredMPSamples(this, evRaw);
        }

        private void PostText(CEventDPointText ev_)
        {
            CEventDPointText evText = new CEventDPointText();
            evText = ev_;
            if (AcquiredEventText != null)
                AcquiredEventText(this, evText);
        }
    }
}
