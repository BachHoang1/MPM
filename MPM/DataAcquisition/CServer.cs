using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MPM.Utilities;
using System.Data;
using System.Windows.Forms;
using MPM.Data;

namespace MPM.DataAcquisition
{
    public class CServer
    {
        const int MAX_NUMBER_OF_ZERO_BYTES = 50;

        const int RETRY_TIMER = 2500;  // milliseconds
        const int MAX_RETRIES = 720;  // 720 * 2.5 seconds is about 30 minutes
        

        public struct SENT_PACKET_REC
        {
            public string sDateTime;
            public string sClientIPAddress;
            public bool bAcked; // acknowledged by client
            public string sData;
            public int iRetries;  // number of times packet was resent when it wasn't acknowledged by the client
        }

        public delegate void dgEventClientConnect(int iConnections);
        public event dgEventClientConnect OnClientConnect;

        private Thread m_threadSendAsync;
        private Thread m_threadReSendAsync;

        Socket m_socketListener;
        
        private bool m_bExit;
        private bool m_bIsRunning;
        private string m_sIPAddress;
        private int m_iPortNumber;
        private List<Socket> m_lstSocket;

        private List<string> m_lstOutgoingQueue;  // outgoing packets sent asynchronously
        private List<string> m_lstIncomingAck;  // acknowledgements from the clients
        private List<SENT_PACKET_REC> m_lstSentPackets;  // packets sent to clients 

        public CServer()
        {
            m_bExit = false;
            m_bIsRunning = false;
            m_iPortNumber = 11001;            
            m_lstSocket = new List<Socket>();
            m_threadSendAsync = new Thread(new ThreadStart(SendAsyncThread));
            m_threadSendAsync.Start();

            m_threadReSendAsync = new Thread(new ThreadStart(ResendAsyncThread));
            m_threadReSendAsync.Start();

            m_lstOutgoingQueue = new List<string>();
            m_lstIncomingAck = new List<string>();
            m_lstSentPackets = new List<SENT_PACKET_REC>();
        }

        ~CServer()
        {

        }

        public void SetPort(string sVal_)
        {            
            m_iPortNumber = System.Convert.ToInt32(sVal_);
        }

        public bool IsRunning()
        {
            return m_bIsRunning;
        }

        public void Unload()
        {
            m_bExit = true;
            Stop();
        }

        public void Stop()
        {
            m_bIsRunning = false;
            
            try
            {
                if (m_socketListener != null)
                {                    
                    for (int i = 0; i < m_lstSocket.Count; i++)
                    {
                        m_lstSocket[i].Disconnect(false);                        
                    }

                    if (m_socketListener.Connected)
                    {
                        m_socketListener.Shutdown(SocketShutdown.Both);
                        m_socketListener.Disconnect(false);
                    }
                    m_socketListener.Close();                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CServer::Stop Error: " + ex.Message);
            }            
        }

        public void Start()
        {                                    
            try
            {
                CLocalAreaNetwork localNetwork = new CLocalAreaNetwork();
                m_sIPAddress = localNetwork.GetMyIPAddress();
                m_socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(m_sIPAddress), m_iPortNumber);

                m_socketListener.Bind(ep);
                m_socketListener.Listen(100);
                System.Diagnostics.Debug.WriteLine("Server is Listening...");
                Socket ClientSocket = default(Socket);

                int counter = 0;
                m_bExit = false;
                m_bIsRunning = true;

                while (true)
                {
                    if (m_bExit)
                        break;

                    counter++;
                    try
                    {
                        ClientSocket = m_socketListener.Accept();
                        m_lstSocket.Add(ClientSocket);
                        OnClientConnect(m_lstSocket.Count);
                        System.Diagnostics.Debug.WriteLine(counter + " Clients connected");
                        Thread UserThread = new Thread(new ThreadStart(() => User(ClientSocket)));
                        UserThread.Start();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("CServer::Start ClientSocket accept error: " + ex.Message);
                        Thread.Sleep(250);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CServer::Start Socket listener error: " + ex.Message);
                Thread.Sleep(250);
            }
        }

        public void User(Socket client_)
        {
            int iZeroByteCount = 0;
            while (true)
            {
                if (m_bExit)
                    break;

                try
                {                    
                    byte[] msg = new byte[1024];
                    
                    if (client_.Available > 0)
                    {
                        int size = client_.Receive(msg);
                        if (size == 0)
                            iZeroByteCount++;
                        else  // log the message
                        {
                            string s = System.Text.Encoding.UTF8.GetString(msg, 0, size);
                            System.Diagnostics.Debug.WriteLine("RECEIVED CLIENT MSG: " + s);
                            m_lstIncomingAck.Add(s);

                            // check to see if it matches any of the existing sent messages so we can 
                            // ack it
                            for (int i = 0; i < m_lstSentPackets.Count; i++)
                            {
                                string sIPAddress = client_.RemoteEndPoint.ToString();
                                if (m_lstSentPackets[i].sClientIPAddress == sIPAddress &&
                                    m_lstSentPackets[i].sData == s)
                                {
                                    SENT_PACKET_REC rec = m_lstSentPackets[i];
                                    rec.bAcked = true;
                                    m_lstSentPackets[i] = rec;
                                }
                            }
                        }
                    }
                    else
                    {
                        // try sending a byte
                        byte[] byteBuffer = { (byte)'P', (byte)'I', (byte)'N', (byte)'G' };
                        client_.Send(byteBuffer, byteBuffer.Length, SocketFlags.None);
                    }                                                                

                    Thread.Sleep(1500);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("CServer::User receive error: " + ex.Message);
                    try  // to disconnect and remove the client
                    {
                        RemoveClient(client_);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("CServer::User disconnect error: " + e.Message);
                    }
                    
                    break;
                }
            }
        }

        public List<string> GetAckPackets()
        {
            return m_lstIncomingAck;
        }

        public List<SENT_PACKET_REC> GetSentPackets()
        {
            return m_lstSentPackets;
        }

        public void RemoveClient(Socket client_)
        {
            for (int i = 0; i < m_lstSocket.Count; i++)
            {
                if (client_ == m_lstSocket[i])
                {
                    m_lstSocket[i].Disconnect(false);
                    m_lstSocket.RemoveAt(i);
                }
            }
            OnClientConnect(m_lstSocket.Count);
        }

        public void Send(String sData)
        {                          
            m_lstOutgoingQueue.Add(sData);                                
        }

        private void SendAsyncThread()
        {
            Thread.Sleep(2000);  // delay so list object gets new'ed before while loop gets started
            while (true)
            {                
                if (m_bExit)
                    break;

                bool bClear = false;

                // send to all clients
                try
                {
                    for (int j = 0; j < m_lstOutgoingQueue.Count; j++)  // send one messages to one client at a time
                    {
                        for (int i = 0; i < m_lstSocket.Count; i++)
                        {                            
                            byte[] byteData = Encoding.UTF8.GetBytes(m_lstOutgoingQueue[j]);
                            m_lstSocket[i].Send(byteData, 0, byteData.Length, SocketFlags.None);

                            // log the message that's sent
                            SENT_PACKET_REC rec = new SENT_PACKET_REC();
                            rec.sDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
                            rec.sData = m_lstOutgoingQueue[j];
                            rec.sClientIPAddress = m_lstSocket[i].RemoteEndPoint.ToString();
                            rec.bAcked = false;
                            rec.iRetries = 0;
                            m_lstSentPackets.Add(rec);
                            Thread.Sleep(50);
                        }
                        bClear = true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("CServer::Send Error: " + ex.Message);
                }

                if (bClear)
                    m_lstOutgoingQueue.Clear();                
                
                Thread.Sleep(500);
            }
        }

        private void ResendAsyncThread()
        {
            while (true)
            {
                if (m_bExit)
                    break;
                                
                try
                {
                    if (m_lstSentPackets == null)
                    {
                        Thread.Sleep(RETRY_TIMER);
                        continue;
                    }    

                    for (int k = 0; k < m_lstSentPackets.Count; k++)
                    {
                        SENT_PACKET_REC rec = m_lstSentPackets[k];
                        if (!rec.bAcked && rec.iRetries < MAX_RETRIES)  // resend
                        {
                            for (int i = 0; i < m_lstSocket.Count; i++)
                            {
                                if (m_lstSocket[i].RemoteEndPoint.ToString() == rec.sClientIPAddress)
                                {
                                    byte[] byteData = Encoding.UTF8.GetBytes(rec.sData);
                                    m_lstSocket[i].Send(byteData, 0, byteData.Length, SocketFlags.None);
                                    rec.iRetries++;
                                    m_lstSentPackets[k] = rec;
                                }
                                Thread.Sleep(50); // give some time before sending the next packet
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("CServer::Resend Error: " + ex.Message);
                }
                
                Thread.Sleep(RETRY_TIMER);
            }
        }

        public void SendList(int iPacketID_, List<string> lstColNames_, List<DataRow> lstData_)
        {            
            for (int i = 0; i < lstData_.Count; i++)
            {
                string s = "";
                for (int j = 0; j < lstColNames_.Count; j++)
                    s += lstColNames_[j] + "=" + lstData_[i].ItemArray[j] + ";";

                s = CCommonTypes.PACKET_ID + "=" + iPacketID_.ToString() + ";" + s.Substring(0, s.Length - 1);  // remove ending semicolon
                m_lstOutgoingQueue.Add(s);
            }
        }
        
    }
}
