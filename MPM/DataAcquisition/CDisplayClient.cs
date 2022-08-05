// author: hoan chau
// purpose: synchrously connect to a server, send a message, and receive info

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MPM.DataAcquisition
{

    // State object for receiving data from remote device.  
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class CDisplayClient
    {
               
        // ManualResetEvent instances signal completion.  
        private ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        

        // The response from the remote device.  
        private String response = String.Empty;
        private static int m_iPortNumber;  // The port number for the remote device.  

        public CDisplayClient()
        {
            m_iPortNumber = 11000;
        }

        public void SetPorNumber(string sVal_)
        {
            m_iPortNumber = System.Convert.ToInt32(sVal_);
        }

        public static string StartClient(string sIPAddress_)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
            string sRetVal = "";
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.                  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(sIPAddress_);
                if (ipHostInfo.AddressList.Length < 1)
                    return sRetVal;
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, m_iPortNumber);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    //Console.WriteLine("Socket connected to {0}",
                    //    sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("This is a client<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    sRetVal = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //Console.WriteLine("Echoed test = {0}", sRetVal);

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return sRetVal;
        }

        public string Connect(string sIPAddress_)
        {
            string sRetVal = StartClient(sIPAddress_);
            return sRetVal;
        }
    }

}