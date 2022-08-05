
// author: hoan chau
// purpose: asynchronously listen for clients and send them some info about the job

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using MPM.Data;

namespace MPM.DataAcquisition
{

    public class CDisplayAsyncServer
    {
        Socket m_socketListener;

        // Thread signal.  
        public   ManualResetEvent allDone = new ManualResetEvent(false);
        private   bool m_bExit;
        private   bool m_bIsRunning;
        private int m_iPortNumber;

        public CDisplayAsyncServer()
        {
            m_bExit = false;
            m_bIsRunning = false;
            m_iPortNumber = 11000;
        }

        public void SetPort(string sVal_)
        {
            m_iPortNumber = System.Convert.ToInt32(sVal_);
        }

        public bool IsRunning()
        {
            return m_bIsRunning;
        }

        ~CDisplayAsyncServer()
        {            
            
        }

        public void Unload()
        {                       
            StopListening();            
        }

        public void StopListening()
        {
            m_bIsRunning = false;
            
            try
            {
                if (m_socketListener != null)
                {
                    m_bExit = true;
                    if (m_socketListener.Connected)
                    {
                        m_socketListener.Shutdown(SocketShutdown.Both);
                        m_socketListener.Disconnect(false);
                    }
                    m_socketListener.Close();
                    allDone.Set();
                }  
            }
            catch (Exception e)
            { }
        }

        public void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer              
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, m_iPortNumber);

            m_bExit = false;

            // Create a TCP/IP socket.              
            m_socketListener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                m_socketListener.Bind(localEndPoint);
                m_socketListener.Listen(100);
                
                m_bIsRunning = true;
                while (true)
                {                      
                    if (m_bExit)
                        break;

                    // Set the event to nonsignaled state.
                    allDone.Reset();
                    
                    // Start an asynchronous socket to listen for connections.  
                    //Console.WriteLine("Waiting for a connection...");
                    m_socketListener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        m_socketListener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            try
            { 
                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;                        
                Socket handler = listener.EndAccept(ar);

                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch
            {

            }
            
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    //    content.Length, content);
                    // send the job and rig infor
                    CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
                    WidgetInfoLookupTbl.Load();
                                        
                    string sJob = WidgetInfoLookupTbl.GetValue("FormPumpPowerTemp", "maskedTextBoxJobID");                    
                    string sRig = WidgetInfoLookupTbl.GetValue("FormPumpPowerTemp", "maskedTextBoxRig");                    

                    String sTest = "JobID=" + sJob + ";Rig=" + sRig;
                    //MessageBox.Show(sTest);
                    Send(handler, sTest);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private   void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private   void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        
    }

} 
