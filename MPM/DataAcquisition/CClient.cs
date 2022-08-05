// author: hoan chau
// purpose: tcp-ip client for Display server

using System;
using System.Net.Sockets;
using System.Threading;

namespace MPM.DataAcquisition
{
    public delegate void ClientHandlePacketData(byte[] data, int bytesRead);

    /// <summary>
    /// Implements a simple TCP client which connects to a specified server and
    /// raises C# events when data is received from the server
    /// </summary>
    public class CClient
    {
        const int MAX_BUFF = 1024;        

        Thread m_threadConnect;  // tries to reconnect when the listening thread fails
        Thread m_threadListen;  // listens to server once there's a connection
        
        private EventWaitHandle m_waitHandleListen = new AutoResetEvent(true);

        public delegate void dgEventRaiser(string s);
        public event dgEventRaiser OnReceivePacket;

        private TcpClient m_tcpClient;
        private NetworkStream m_clientStream;

        private byte[] m_buffer;
                        
        private bool m_bStarted = false;

        // network address and port
        private int m_iPort;
        private string m_sIPAddress;

        private bool m_bExit;  // exit all threads  
        private bool m_bHasChangedConnection;  // for changing the IP address during a session

        public delegate void dgEventServerConnect(int iConnections);
        public event dgEventServerConnect OnServerConnect;
        /// <summary>
        /// Constructs a new client
        /// </summary>
        public CClient()
        {
            m_buffer = new byte[MAX_BUFF];
        }

        /// <summary>
        /// Initiates a TCP connection to a TCP server with a given address and port
        /// </summary>
        /// <param name="sIPAddress_">The IP address (IPV4) of the server</param>
        /// <param name="iPort_">The port the server is listening on</param>
        public void ConnectToServer(string sIPAddress_, int iPort_)
        {            
            m_bExit = false;
            m_bHasChangedConnection = false;
            m_iPort = iPort_;
            m_sIPAddress = sIPAddress_;
            
            m_threadConnect = new Thread(new ThreadStart(Connect));            
            m_threadConnect.Start();                                        
        }

        
        /// <summary>
        /// Triggers a re-connection to the server
        /// When the server is connected, the Receive thread will restart
        /// </summary>
        private void Connect()
        {
            while (true)
            {
                if (m_bExit)
                    break;

                if (m_tcpClient != null)
                {
                    Thread.Sleep(3000);
                    if (m_tcpClient.Connected)
                    {                        
                        try  // connected property is not definitive so try sending a zero byte
                        {
                            byte[] byteBuffer = { (byte)'C', (byte)'L', (byte)'I', (byte)'E', (byte)'N', (byte)'T' };
                            //m_tcpClient.Client.Send(byteBuffer);                            
                        }
                        catch
                        {
                            // let it fall through and create a new connection  
                            //Disconnect();                            
                        }
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                    
                try
                {         
                    if (!m_bStarted)
                    {
                        m_tcpClient = new TcpClient(m_sIPAddress, m_iPort);

                        m_clientStream = m_tcpClient.GetStream();
                        Console.WriteLine("CONNECTED ....................");

                        OnServerConnect(1);
                        // start listening to the server to get data
                        m_bStarted = true;
                        m_threadListen = new Thread(new ThreadStart(ListenForPackets));
                        m_threadListen.Start();
                        m_waitHandleListen.Set();
                    }
                    
                }
                catch (Exception ex)
                {                    
                    Thread.Sleep(1000);
                    Console.WriteLine("Connect Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// This method runs on its own thread, and is responsible for
        /// receiving data from the server and raising an event when data
        /// is received
        /// </summary>
        private void ListenForPackets()
        {
            int bytesRead;

            while (m_bStarted)
            {
                bytesRead = 0;
                if (m_bExit)                
                    break;                
                    
                m_waitHandleListen.WaitOne();
                
                try
                {
                    //Blocks until a message is received from the server
                    
                    bytesRead = m_clientStream.Read(m_buffer, 0, MAX_BUFF);

                    string sIncomingMsg = System.Text.Encoding.ASCII.GetString(m_buffer, 0, bytesRead);
                    Console.WriteLine("SERVER MSG: " + sIncomingMsg);
                    
                    if (sIncomingMsg != "PING")
                    {
                        OnReceivePacket(System.Text.Encoding.UTF8.GetString(m_buffer, 0, bytesRead));
                        m_clientStream.Write(m_buffer, 0, bytesRead);  // ack the server by echoing the packet
                    }
                        
                    m_waitHandleListen.Set();                    
                }
                catch (Exception ex)
                {
                    //A socket error has occurred
                    System.Diagnostics.Debug.WriteLine("A socket error has occurred with the client socket " + m_tcpClient.ToString() + ". Error: " + ex.Message);
                    OnServerConnect(0);
                    Disconnect();
                    m_waitHandleListen.Reset();  // wait until there is a reconnection   
                }                                              
            }                       
        }
       
        /// <summary>
        /// Tells whether we're connected to the server
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            if (m_tcpClient == null)
                return false;
            else
                return m_bStarted && m_tcpClient.Connected;
        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            if (m_tcpClient == null)
            {
                return;
            }

            m_clientStream.Close();
            m_clientStream.Dispose();
            m_tcpClient.Close();
            m_tcpClient.Dispose();
            
            if (m_bStarted)
            {                
                m_bStarted = false;
                System.Diagnostics.Debug.WriteLine("Disconnected from server");
            }                                        
        }

        public void Reconnect(string sIPAddress_, int iPort_)
        {
            m_sIPAddress = sIPAddress_;
            m_iPort = iPort_;

            if (m_clientStream != null)
                m_clientStream.Close();
            if (m_tcpClient != null)
                m_tcpClient.Close();

            m_waitHandleListen.Reset();
            m_bHasChangedConnection = true;
        }

        public bool HasChangedConnection()
        {
            return m_bHasChangedConnection;
        }

        public void Stop()
        {
            m_bStarted = false;
            m_bExit = true;  
            if (m_clientStream != null)
                m_clientStream.Close();
            if (m_tcpClient != null)
                m_tcpClient.Close();
            
            m_waitHandleListen.Set();
        }
    }
}
