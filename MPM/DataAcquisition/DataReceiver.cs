using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MPM.Data;
using MPM.DataAcquisition.Helpers;

namespace MPM.DataAcquisition
{
    public class DataReceiver
    {
        NetworkStream networkStream;
        private Socket clientSocket;

        //2/15/22const int BUFFER_LENGTH = 1000000;
        const int BUFFER_LENGTH = 55000;
        string RemoteIPAddress; //  = "192.168.1.179";
        IPAddress _ipAddress;
        IPEndPoint _ipEndPoint;

        public delegate void DetectConnectedEventHandler(object sender, CEventDetect e);
        public event DetectConnectedEventHandler DetectConnected;

        private List<List<byte>> Lists = new List<List<byte>>();
        private CCommonTypes.TELEMETRY_TYPE m_ttID;  // identification of type to determine specific processing //2/16/22

        #region IsConnected
        // public bool IsConnected
        //  {
        //     get { return (bool)GetValue(IsConnectedProperty); }
        //      set { SetValue(IsConnectedProperty, value); }
        //  }
        public bool IsConnected;

        // Using a DependencyProperty as the backing store for IsConnected.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IsConnectedProperty =
        //    DependencyProperty.Register("IsConnected", typeof(bool), typeof(DataReceiver), new PropertyMetadata());
        #endregion

        int _bytesRead;
        private int _port { get; set; }

        //private byte[] _byteData = new byte[1024];
        byte[] _buffer = new byte[BUFFER_LENGTH];

        DataParser parser;        

        public ushort htons(ushort host)
        {
            byte[] bytes = BitConverter.GetBytes(host);
            Array.Reverse(bytes); //reverse it so we get big endian.
            ushort i = BitConverter.ToUInt16(bytes, 0);
            return i;
        }
  
        //2/16/22
        public DataReceiver(string remoteIPAddress, int portNumber, CCommonTypes.TELEMETRY_TYPE tt_)
        {
            RemoteIPAddress = remoteIPAddress;
            _port = htons((ushort)portNumber);
            parser = new DataParser();

            _ipAddress = IPAddress.Parse(RemoteIPAddress);
            _ipEndPoint = new IPEndPoint(_ipAddress, _port);

            IsConnected = false;

            m_ttID = tt_; //2/16/22
        }
        //2/16/22

        public void RefreshIPAddress(string remoteIPAddress_, int iPortNumber_)
        {
            RemoteIPAddress = remoteIPAddress_;
            _port = iPortNumber_;
            _ipAddress = IPAddress.Parse(RemoteIPAddress);
            _ipEndPoint = new IPEndPoint(_ipAddress, _port);
        }

        public bool Start()
        {
            try
            {
                if (clientSocket == null || !clientSocket.Connected)
                {
                    CEventDetect evDetectConnected = new CEventDetect();
                    evDetectConnected.m_iConnected = CEventDetect.CONNECTION.TRY;
                    if (DetectConnected != null)
                        DetectConnected(this, evDetectConnected);

                    //We're using TCP sockets
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    //Connect to the server
                    clientSocket.Connect(_ipEndPoint);
                }

                if (clientSocket.Connected)
                {
                    networkStream = new NetworkStream(clientSocket);
                    CEventDetect evDetectConnected = new CEventDetect();
                    evDetectConnected.m_iConnected = CEventDetect.CONNECTION.OPEN;
                    if (DetectConnected != null)
                        DetectConnected(this, evDetectConnected);
                }
                    
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                CEventDetect evDetectConnected = new CEventDetect();
                evDetectConnected.m_iConnected = 0;
                if (DetectConnected != null)
                    DetectConnected(this, evDetectConnected);

                Thread.Sleep(1000);
            }

            return clientSocket.Connected;
        }
        public ParsedDataPacket NextPacket()
        {
            ParsedDataPacket retValue = null;

            if (networkStream == null)
            {
                Start();
                if (networkStream == null)
                    return retValue;
            }

            try
            {
                //                Task<ParsedDataPacket> result = ReadAndProcessFromNetwork();
                //                retValue = result.Result;
                ParsedDataPacket result = ReadAndProcessFromNetwork();
                retValue = result;
            }
            catch
            {
                retValue = null;
            }

            return retValue;
        }

        //        private async Task<ParsedDataPacket> ReadAndProcessFromNetwork()
        private ParsedDataPacket ReadAndProcessFromNetwork()
        {
             //2/15/22
            ParsedDataPacket retValue = null;

            //            await TaskEx.Yield();
            // TaskEx.Yield();

            //Debug.WriteLine("Inside ReadAndProcessFromNetwork");
            if (NoCollectedData)
            {
                //Debug.WriteLine("In NoCollected Data");
                _bytesRead = 0;                
                do
                {
                    //                    _bytesRead = await networkStream.ReadAsync(_buffer, 0, _buffer.Length);
                    try
                    {
                        _bytesRead = networkStream.Read(_buffer, 0, _buffer.Length);

                        if (_bytesRead > BUFFER_LENGTH - 1)
                            Debug.WriteLine("Buffer Overflow");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    
                    // If the buffer is completely filled, then we have overflow,
                    // so dump this buffer and get a new one.
                } while (_bytesRead > BUFFER_LENGTH - 1);
                Debug.WriteLine("BYTES READ: " + _bytesRead.ToString());
                List<byte> processingList = _buffer.ToList<byte>().GetRange(0, _bytesRead);
                //Debug.WriteLine("Adding List");
                Lists.Add(processingList);
            }

            List<byte> list0 = Lists[0];
            retValue = ExtractDataItem(retValue, ref list0);
            if (list0.Count() < PacketT.HeaderSize)
                Lists.RemoveAt(0);
            else
                //try
                //{
                    if (Lists.Count > 0)
                        Lists[0] = list0;
                //}
                //catch
                //{

                //}

            return retValue;
        }

        //2/15/22

        public ParsedDataPacket AcquireNextPacket()
        {
            ParsedDataPacket retValue = null;

            if (networkStream == null)
            {
                Start();
                if (networkStream == null)
                    return retValue;
            }

            try
            {
                //                Task<ParsedDataPacket> result = ReadAndProcessFromNetwork();
                //                retValue = result.Result;
                ParsedDataPacket result = ReadFromNetwork();
                retValue = result;
            }
            catch
            {
                retValue = null;
            }

            return retValue;
        }

        //        private async Task<ParsedDataPacket> ReadAndProcessFromNetwork()
        private ParsedDataPacket ReadFromNetwork()
        {
            ParsedDataPacket goodReturn = new ParsedDataPacket(Command.COMMAND_REQ_STATUS);
            ParsedDataPacket badReturn = null; 

            try
            {
                //Debug.WriteLine("In NoCollected Data");
                _bytesRead = 0;
                do
                {   //                    _bytesRead = await networkStream.ReadAsync(_buffer, 0, _buffer.Length);
                    try
                    {
                        _bytesRead = networkStream.Read(_buffer, 0, _buffer.Length);

                        if (_bytesRead > BUFFER_LENGTH - 1)
                        {
                            Debug.WriteLine("Buffer Overflow");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }

                    // If the buffer is completely filled, then we have overflow,
                    // so dump this buffer and get a new one.
                } while (_bytesRead > BUFFER_LENGTH - 1);

                //int bytesRead = networkStream.Read(_buffer, 0, BUFFER_LENGTH);

                //Debug.WriteLine("BYTES READ: " + bytesRead.ToString());

                if (_bytesRead <= 0)
                {
                    return badReturn;
                }
                else
                {
                    DetectSocketDataSingleton dsds = DetectSocketDataSingleton.Instance;

                    if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                    {
                        if (dsds.DetectSockData.SurveyDataSemaphoreEM.WaitOne(500)) //2/25/22
                        {      
                            dsds.DetectSockData.BytesReadFromSocketEM = _bytesRead;

                            dsds.DetectSockData.SockeReadBufferEM = _buffer.ToArray(); //2/4/22
          
                            dsds.DetectSockData.SurveyDataSemaphoreEM.Release(); //2/18/22
                            Thread.Sleep(50); //2/2/22
                        }
                    }
                    else if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                    {
                        if (dsds.DetectSockData.SurveyDataSemaphoreMP.WaitOne(500)) //2/25/22
                        {
                            dsds.DetectSockData.BytesReadFromSocketMP = _bytesRead;

                            dsds.DetectSockData.SockeReadBufferMP = _buffer.ToArray(); //2/4/22
                         
                            dsds.DetectSockData.SurveyDataSemaphoreMP.Release(); //2/18/22
                            Thread.Sleep(50); //2/22/22
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                return badReturn;
            }

            return goodReturn;
        }


        public ParsedDataPacket ProcessDataFromNetwork()
        {
            ParsedDataPacket retValue = null;
            DetectSocketDataSingleton dsds = DetectSocketDataSingleton.Instance;
            if (NoCollectedData)
            {
                if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                {
                    //if (dsds.DetectSockData.BytesReadFromSocketEM <= 0)
                        //return retValue;

                    List<byte> processingList = dsds.DetectSockData.SockeReadBufferEM.ToList<byte>().GetRange(0,
                                     dsds.DetectSockData.BytesReadFromSocketEM);

                    Lists.Add(processingList);

                }
                else if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                {
                    //if (dsds.DetectSockData.BytesReadFromSocketMP <= 0)
                        //return retValue;

                    List<byte> processingList = dsds.DetectSockData.SockeReadBufferMP.ToList<byte>().GetRange(0,
                                    dsds.DetectSockData.BytesReadFromSocketMP);

                    Lists.Add(processingList);

                }
            }

            List<byte> list0 = Lists[0];

            retValue = ExtractDataItem(retValue, ref list0);

            if (list0.Count() < PacketT.HeaderSize)
            {
                Lists.RemoveAt(0);
            }     
            else
            {
                if (Lists.Count > 0)
                {
                    Lists[0] = list0;
                }
            }
            return retValue;
        }
        //2/15/22

        private ParsedDataPacket ExtractDataItem(ParsedDataPacket retValue, ref List<byte> processingList)
        {
            //Debug.WriteLine("Inside ExtractDataItem");
            int udSize = MPM.DataAcquisition.Helpers.Utilities.BytesToInt(processingList, PacketT.PositionUDSize);
            int totalPacketSize = PacketT.PacketSize(udSize);

            //Debug.WriteLine("Total Packet Size:  {0}", totalPacketSize);
            if (totalPacketSize < PacketT.HeaderSize || totalPacketSize > BUFFER_LENGTH)
            {
                Debug.WriteLine("REMOVING LIST");
                Lists.Remove(processingList);
                return null;
            }

            // The next line trims the buffer to the right length
            if (processingList.Count > 0)
            {
                List<byte> packetInfo = processingList.GetRange(0, totalPacketSize);

                processingList = processingList.GetRange(totalPacketSize, processingList.Count - totalPacketSize);
                retValue = parser.ParseInput(packetInfo);
            }
            
            return retValue;
        }

        private bool NoCollectedData
        {
            get
            {
                if (Lists.Count() == 0)
                    return true;

                return Lists[0].Count() < PacketT.HeaderSize;
            }
        }

        public void CloseConnection()
        {
            if (clientSocket != null)
                clientSocket.Close();
        }
    }
}
