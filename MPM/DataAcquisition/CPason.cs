// author: hoan chau
// purpose: communicate with Pason System to get and put WITS data

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using MPM.Data;
using System.Windows.Forms;

namespace MPM.DataAcquisition
{
    public class CPason
    {
        private const string COM_UNKNOWN = "COM_UNKNOWN";
        private const int PING_INTERVAL = 1;  // seconds

        private bool _continue;
        private SerialPort m_serialPort;

        public delegate void AcquiredWITSHandler(object sender, CEventDPoint e);
        public event AcquiredWITSHandler Acquired;
        private Thread readThread;
        private Thread writeThread;
        
        private string m_sWITSToProcess;

        public delegate void ConnectedEventHandler(object sender, CEventWITS e);
        public event ConnectedEventHandler PasonConnected;
        private List<string> m_lstOutgoingPacket;  // queued packets that are bound for Pason

        private static ManualResetEvent m_mreWrite = new ManualResetEvent(false);

        private bool m_bUnload;
        private List<string> m_lstWITSChannels;
        CWITSLookupTable m_witsLookUpTable;

        public CPason(ref CWITSLookupTable witsLookUpTbl_)
        {
            m_witsLookUpTable = witsLookUpTbl_;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            readThread = new Thread(Read);
            writeThread = new Thread(Write);
            m_lstOutgoingPacket = new List<string>();

            // Create a new SerialPort object with default settings.
            m_serialPort = new SerialPort();            
            m_serialPort.PortName = COM_UNKNOWN;  // com port gets set by caller
            m_serialPort.BaudRate = 9600;
            m_serialPort.Parity = Parity.None;
            m_serialPort.DataBits = 8;
            m_serialPort.StopBits = StopBits.One;
            m_serialPort.Handshake = Handshake.XOnXOff;

            // Allow the user to set the appropriate properties.
            m_serialPort.PortName = SetPortName(m_serialPort.PortName);
            m_serialPort.BaudRate = SetPortBaudRate(m_serialPort.BaudRate);
            m_serialPort.Parity = SetPortParity(m_serialPort.Parity);
            m_serialPort.DataBits = SetPortDataBits(m_serialPort.DataBits);
            m_serialPort.StopBits = SetPortStopBits(m_serialPort.StopBits);
            m_serialPort.Handshake = SetPortHandshake(m_serialPort.Handshake);
            m_serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            // Set the read/write timeouts
            m_serialPort.ReadTimeout = 1000;
            m_serialPort.WriteTimeout = 1500;

            m_lstWITSChannels = GetWITSChannels();

            m_bUnload = false;
        }

        ~CPason()
        {
            m_bUnload = true;
            Stop();
        }        

        private List<string> GetWITSChannels()
        {
            List<string> lstRet = new List<string>();
                        
            string sExclude = "(' ')";  // empty exclusion list
            List<CWITSLookupTable.WITSChannel> lstRigChannels = m_witsLookUpTable.GetFromSource("Rig", sExclude);
            for (int i = 0; i < lstRigChannels.Count; i++)
                lstRet.Add(lstRigChannels[i].sID);
            return lstRet;
        }

        public void SetPort(int iVal_)
        {
            try
            {
                //if (m_serialPort != null)
                //    if (m_serialPort.IsOpen)
                //        m_serialPort.Close();

                //m_serialPort = new SerialPort();
                m_serialPort.PortName = "COM" + iVal_.ToString();
            }
            catch
            {
                
            }            
        }

        public int GetPingInterval()
        {
            return PING_INTERVAL;
        }        

        private void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
        {
            if (m_bUnload)
                return;
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Console.WriteLine("Data Received:");
            //Console.Write(indata);
            //Parse(message);
            //Console.WriteLine(indata);   
            Parse(indata);

            // notify listener that connection is alive
            CEventWITS evPasonConnected = new CEventWITS();
            evPasonConnected.m_bConnected = true;
            if (PasonConnected != null)
                PasonConnected(this, evPasonConnected);
        }

        public void SetUnload()
        {
            m_lstOutgoingPacket.Clear();
            m_bUnload = true;
            m_mreWrite.Set();
        }

        public void Stop()
        {
            if (m_serialPort != null)
                if (m_serialPort.IsOpen)
                {       
                    try
                    {
                        //m_serialPort.Close();  // causes the application to hang on exit  
                        // must call separate thread to close.  see link that follows for thread
                        // https://social.msdn.microsoft.com/Forums/en-US/ce8ce1a3-64ed-4f26-b9ad-e2ff1d3be0a5/serial-port-hangs-whilst-closing?forum=Vsexpressvcs
                        Thread CloseDown = new Thread(new ThreadStart(CloseSerialOnExit)); //close port in new thread to avoid hang
                        CloseDown.Start(); //close port in new thread to avoid hang
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("CPason::Stop() Error: " + ex.Message);
                    }
                    CEventWITS evPasonConnected = new CEventWITS();
                    evPasonConnected.m_bConnected = false;
                    if (PasonConnected != null)
                        PasonConnected(this, evPasonConnected);
                }                           
        }

        
        private void CloseSerialOnExit()
        {
            try
            {
                m_serialPort.Close(); //close the serial port
            }
            catch (Exception ex)
            {
                MessageBox.Show("CPason::CloseSerialOnExit() Error: " + ex.Message);
            }         
        }

        public bool Start(ref string sErrMsg_, string sNewPortName_ = "")
        {
            bool bRetVal = true;
            if (m_serialPort.PortName == COM_UNKNOWN && sNewPortName_ == "")
            {
                CEventWITS evPasonConnected = new CEventWITS();
                evPasonConnected.m_bConnected = false;
                if (PasonConnected != null)
                    PasonConnected(this, evPasonConnected);

                sErrMsg_ = "Cannot connect to WITS provider because the COM Port is not set.\nUse the WITS menu item to configure.";
                bRetVal = false;
            }
            else if (m_serialPort.PortName == "COM0" && sNewPortName_ == "")
            {
                CEventWITS evPasonConnected = new CEventWITS();
                evPasonConnected.m_bConnected = false;
                if (PasonConnected != null)
                    PasonConnected(this, evPasonConnected);

                sErrMsg_ = "COM Port is not set.\nUse the WITS menu item to configure.";
                bRetVal = false;
            }
            else
            {
                try
                {
                    if (m_serialPort.PortName == sNewPortName_ &&
                        m_serialPort.IsOpen)
                        bRetVal = true;
                    else
                    {                       
                        Stop();
                        if (m_serialPort.PortName != sNewPortName_ && sNewPortName_ != "")
                        {
                            m_serialPort.PortName = sNewPortName_;
                        }
                        m_serialPort.Open();
                        _continue = true;
                        if (!readThread.IsAlive)
                        {
                            readThread.Start();
                            writeThread.Start();
                        }                        
                    }

                    m_sWITSToProcess = "";
                    CEventWITS evPasonConnected = new CEventWITS();
                    evPasonConnected.m_bConnected = true;
                    if (PasonConnected != null)
                        PasonConnected(this, evPasonConnected);

                }
                catch (Exception ex)
                {
                    CEventWITS evPasonConnected = new CEventWITS();
                    evPasonConnected.m_bConnected = false;
                    if (PasonConnected != null)
                        PasonConnected(this, evPasonConnected);
                    sErrMsg_ = ex.Message;
                    bRetVal = false;
                }
                
            }
            return bRetVal;
        }       

        public void Read()
        {
            int iCounter = 0;
            while (_continue)
            {
                try
                {
                    if (m_bUnload)
                        _continue = false;

                    if (iCounter == PING_INTERVAL)
                    {
                        if (m_serialPort.IsOpen)
                            m_serialPort.Write("&&\r\n06071\r\n!!\r\n");  // activity to keep the connection alive
                        iCounter = 0;
                    }
                    else
                        iCounter++;

                    //string message = _serialPort.ReadLine();
                    // parse this line
                    //Parse(message);
                    
                    Thread.Sleep(1000);               
                }
                catch (Exception e) {
                    Console.WriteLine("Error in CPason::Read() :" + e.Message);     
                }
            }
        }

        private void Write()
        {            
            while (true)
            {
                if (m_bUnload)
                    break;
                m_mreWrite.WaitOne();
                if (m_lstOutgoingPacket.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < m_lstOutgoingPacket.Count; i++)
                            m_serialPort.Write(m_lstOutgoingPacket[i]);
                    }
                    catch
                    {

                    }
                    
                    m_lstOutgoingPacket.Clear();
                }
                m_mreWrite.Reset();
            }            
        }

        public void QueueOutgoingPacket(string sRecord_)
        {
            m_lstOutgoingPacket.Add(sRecord_);
            m_mreWrite.Set();
        }

        public string GetPort()
        {
            return m_serialPort.PortName;
        }

        public bool Find(string sChannel)
        {
            bool bFound = false;
            for (int i = 0; i < m_lstWITSChannels.Count; i++)
            {
                if (m_lstWITSChannels[i] == sChannel)
                {
                    bFound = true;
                    break;
                }
            }
            return bFound;
        }

        private void Parse(string sVal_)
        {
            // search through all the text for a complete record            
            m_sWITSToProcess += sVal_;
            while (true)
            {
                if (m_sWITSToProcess.Contains("&&") && m_sWITSToProcess.Contains("!!"))
                {
                    int iPosOfAmpersand = m_sWITSToProcess.IndexOf("&&");
                    int iPosOfExclamation = m_sWITSToProcess.IndexOf("!!");
                    if (iPosOfAmpersand < iPosOfExclamation)  // case #1: seems valid 
                    {                        
                        string sRemainder = m_sWITSToProcess.Substring(iPosOfExclamation + 2);  // for the next loop iteration
                        m_sWITSToProcess = m_sWITSToProcess.Substring(iPosOfAmpersand + 2, iPosOfExclamation - 2);
                        m_sWITSToProcess = m_sWITSToProcess.Replace("\r\n", " ");

                        string[] sChannels = m_sWITSToProcess.Split(' ');                        
                        for (int i = 0; i < sChannels.Length; i++)  // search through array for appropriate values
                        {
                            sChannels[i] = sChannels[i].Trim();
                            if (sChannels[i].Length > 4)
                            {
                                bool bFound = Find(sChannels[i].Substring(0, 4));
                                if (bFound)
                                {
                                    string sVal = sChannels[i].Substring(4);
                                    CEventDPoint evSub = new CEventDPoint();
                                    evSub.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_NONE;
                                    evSub.m_sValue = sVal;
                                    evSub.m_ID = System.Convert.ToInt32("1" + sChannels[i].Substring(0, 4));
                                    if (Acquired != null)
                                        Acquired(this, evSub);                                    
                                }
                            }
                        }
                        
                        m_sWITSToProcess = sRemainder;  // possibly another loop iteration left
                    }
                    else  // case #2: the ampersand is after the exclamations which implies we need to start at the && 
                    {
                        m_sWITSToProcess = m_sWITSToProcess.Substring(iPosOfAmpersand);
                        break;
                    }
                }
                else  // case #3
                {
                    // if it contains double ampersands then 
                        // we're good and it hasn't reached the end
                    // else if it contains double exclamations then 
                        // we're at the end which will be handled by case #2?                                   
                    break;
                }
            }                                  
        }

        // Display Port values and prompt user to enter a port.
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            //Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            //portName = Console.ReadLine();
            portName = defaultPortName;

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }

        // Display BaudRate values and prompt user to enter a value.
        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            //Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
            //baudRate = Console.ReadLine();
            baudRate = defaultPortBaudRate.ToString();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        // Display PortParity values and prompt user to enter a value.
        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            //Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
            //parity = Console.ReadLine();
            parity = defaultPortParity.ToString();

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }

        // Display DataBits values and prompt user to enter a value.
        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            //Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
            //dataBits = Console.ReadLine();
            dataBits = defaultPortDataBits.ToString();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        // Display StopBits values and prompt user to enter a value.
        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available StopBits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            //Console.Write("Enter StopBits value (None is not supported and \n" +
            // "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
            //stopBits = Console.ReadLine();
            stopBits = defaultPortStopBits.ToString();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }

        public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;

            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            //Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
            //handshake = Console.ReadLine();
            handshake = defaultPortHandshake.ToString();

            if (handshake == "")
            {
                handshake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }
    }
}
