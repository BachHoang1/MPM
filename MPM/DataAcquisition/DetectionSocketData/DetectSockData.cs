using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

//using ConstantDefines;

namespace MPM.DataAcquisition
{
    public class DetectSockData : IDisposable //3/25/21
    {
        public Socket SocketClient { get; set; }
        public EventHandler SocketStatusEvent { get; set; }
        //2/23/22public EventHandler SocketDataReceiveEventEM { get; set; } //2/16/22
        //2/23/22public EventHandler SocketDataReceiveEventMP { get; set; } //2/26/22
        public EventHandler SocketDataWriteEvent { get; set; }
        //1/10/22public EventHandler ReconnectDisplayDetectEvent { get; set; } //1/7/22

        //Status _SocketStatus; 

        public byte[] SockeReadBufferEM { get; set; } //2/16/22
        public int BytesReadFromSocketEM{ get; set; } //2/16/22

        public byte[] SockeReadBufferMP { get; set; } //2/16/22
        public int BytesReadFromSocketMP { get; set; } //2/16/22

        public byte[] SockeWriteBuffer { get; set; }

        public int SockeWriteBufferIndex { get; set; }

        public bool KeepSockeBckgrdReadWorkerOpen { get; set; }

        public Semaphore SurveyDataSemaphoreEM { get; set; } //2//22/22
        public Semaphore SurveyDataSemaphoreMP { get; set; } //2//22/22

        //2/26/22
        public bool EnableEMSocket { get; set; }
        public bool EnableMPSocket { get; set; }
        //2/26/22


        //3/25/21
        public void Dispose()
        {
            SocketClient.Dispose();
            SocketStatusEvent = null;
            SocketDataWriteEvent = null;
        }
        //3/25/21
        public DetectSockData()
        {
            SocketClient = null;

            SocketStatusEvent = null;
            //2/23/22ocketDataReceiveEventEM = null;
            //2/23/22ocketDataReceiveEventMP = null;
            SocketDataWriteEvent = null;

            //1/10/22ReconnectDisplayDetectEvent = null; //1/5/22

           // _SocketStatus = Status.DISCONNECTED;

            SockeReadBufferEM = new byte[5500];
            SockeReadBufferMP = new byte[5500];
            SockeWriteBuffer  = new byte[5500];

            //1/19/22SockeReadBuffer  = new byte[1000000];
            //1/19/22SockeWriteBuffer = new byte[1000000];

            SockeWriteBufferIndex = 0;
             
            KeepSockeBckgrdReadWorkerOpen = true;

            BytesReadFromSocketEM = 0;
            BytesReadFromSocketMP = 0;

            //Create two semaphore that can sstisfy up to two
            //concurrent reeusts. Use an intial count of zero,
            //so that the entire semaphore count is initially
            //owned by the main program thread.

            SurveyDataSemaphoreEM = new Semaphore(0, 2); //2//22/22
            SurveyDataSemaphoreMP = new Semaphore(0, 2); //2//22/22

            EnableEMSocket = true; //2/26/22
            EnableMPSocket = true; //2/26/22
        }

        //public Status ConnectionStatus
        //{
        //    get
        //    {
        //        return _SocketStatus;
        //    }
        //
        //    set
        //    {
        //        _SocketStatus = value;
        //
        //        //12/10/20Thread.Sleep(50);
        //        Thread.Sleep(10);
        //
        //        if (SocketStatusEvent != null)
        //        {
        //            SocketStatusEvent(this, null);
        //        }
        //    }
        //}
    }
}
