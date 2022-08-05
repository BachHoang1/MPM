// author: hoan chau
// purpose: connect to detect and listen for packets

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MPM.DataAcquisition;
using MPM.DataAcquisition.Helpers;
using System.ComponentModel;
using MPM.Data;
using MPM.Utilities;
using System.Data.Common;
using System.Security;
using MPM.GUI;

namespace MPM.DataAcquisition
{
    public class CDetect
    {
        private const int DEFAULT_DETECT_PORT_NUMBER = 3300;
        public const int RAW_MP_SAMPLE_SIZE = 225;
        private const string DEFAULT_DETECT_IP_ADDRESS = "127.0.0.1";
        private const int MAX_PACKET_PER_SEC = 2;

        private struct TRACK_PACKET_REC
        {
            public DateTime dtLast;  // last time there was a packet
            public int iCnt;
        }

        private TRACK_PACKET_REC m_packetRec;

        // acquisition events
        public delegate void AcquiredEventHandler(object sender, CEventDPoint e);
        public event AcquiredEventHandler Acquired;

        public delegate void AcquiredMPSamplesEventHandler(object sender, CEventRawMPSamples e);
        public event AcquiredMPSamplesEventHandler AcquiredMPSamples;

        public delegate void DetectConnectedEventHandler(object sender, CEventDetect e);
        public event DetectConnectedEventHandler DetectConnected;

        public delegate void AcquiredTextEventHandler(object sender, CEventDPointText e);
        public event AcquiredTextEventHandler AcquiredText;

        ProgressChangedEventHandler m_eventProgress;
        RunWorkerCompletedEventHandler m_eventWorkCompleted;
        DoWorkEventHandler m_eventDoWork;

        BackgroundWorker _backgroundInputProcessor = new BackgroundWorker();
        DataReceiver _dataReceiver;

        DataReceiver.DetectConnectedEventHandler m_DetectConnected; // = new DataReceiver.DetectConnectedEventHandler(ConnectionStatus);

        private string m_sIPAddress;
        private int m_iPortNumber;


        private Command m_cmdCurrent;
        private int m_iPDPNullCount;
        private double[] m_dblArrSample;
        private bool m_bIsConnected;

        CDPointLookupTable m_DPointTable;

        private DateTime m_dtSamples;  // tracks the time for each set of samples
        private CEventDPoint m_evtParityError = new CEventDPoint();

        private CDrillingBitDepth m_BitDepth;

        // for LAS export associated with Gamma, usually, but can be for any LAS series
        private List<CDrillingBase> m_lstDrillingSensor;

        // for unit conversion
        private CCommonTypes.UNIT_SET m_UnitSet;
        private CUnitLength m_LengthUnit;
        private CUnitPressure m_PressureUnit;
        private CUnitTemperature m_TemperatureUnit;
        private CUnitRateOfPenetration m_ROPUnit;

        private CCommonTypes.TELEMETRY_TYPE m_ttID;  // identification of type to determine specific processing

        private bool m_bValidLicense;
        private bool m_bExit;

        private DbConnection m_dbCnn;

        CEquivalentCirculatingDensity m_ECD;
        FormPlotSamples m_frmPlotSamples;
        FormPlotSamplesEM m_frmPlotSamplesEM;
        FormFFT m_frmFFT;
        FormFFTEM m_frmFFTEM;
        CPumpPressure m_pumpPressure;

        public CDetect(CCommonTypes.TELEMETRY_TYPE tt_, ref DbConnection dbCnn_)
        {
            m_ttID = tt_;
            m_dbCnn = dbCnn_;
            m_sIPAddress = DEFAULT_DETECT_IP_ADDRESS;
            m_iPortNumber = DEFAULT_DETECT_PORT_NUMBER;
            m_DPointTable = new CDPointLookupTable();
            m_DPointTable.Load();
            m_dtSamples = new DateTime(1970, 1, 1, 1, 1, 1);

            m_packetRec = new TRACK_PACKET_REC();
            m_packetRec.dtLast = new DateTime(1970, 1, 1, 1, 1, 1);
            m_packetRec.iCnt = 0;

            m_bValidLicense = false;
            m_bExit = false;

            // set up drilling sensors
            int[] iArrDPointCommand = new int[] {(int)Command.COMMAND_HOLE_DEPTH,
                                                 (int)Command.COMMAND_ROP,
                                                 (int)Command.COMMAND_WOB
                                                };

            m_lstDrillingSensor = new List<CDrillingBase>();
            for (int i = 0; i < iArrDPointCommand.Length; i++)
            {
                CDrillingBase drillSensor = new CDrillingBase(ref m_dbCnn, iArrDPointCommand[i]);
                CDPointLookupTable.DPointInfo infoSensor = m_DPointTable.Get(iArrDPointCommand[i]);
                drillSensor.SetDPointInfo(infoSensor);
                m_lstDrillingSensor.Add(drillSensor);
            }

            m_BitDepth = new CDrillingBitDepth(ref m_dbCnn, (int)Command.COMMAND_BIT_DEPTH);
            CDPointLookupTable.DPointInfo info = m_DPointTable.Find("Bit Depth");
            m_BitDepth.SetDPointInfo(info);


            m_UnitSet = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
            m_LengthUnit = new CUnitLength();
            m_PressureUnit = new CUnitPressure();
            m_TemperatureUnit = new CUnitTemperature();
            m_ROPUnit = new CUnitRateOfPenetration();
        }

        public void SetLicense(bool bValid_)
        {
            m_bValidLicense = bValid_;
        }
        
        public void SetPlotForm(ref FormPlotSamples frm_, ref FormFFT frmFFT_, ref CPumpPressure pumpPressure_)
        {
            m_frmPlotSamples = frm_;
            m_frmFFT = frmFFT_;
            m_pumpPressure = pumpPressure_;
        }

        public void SetPlotFormEM(ref FormPlotSamplesEM frm_, ref FormFFTEM frmFFT_, ref CPumpPressure pumpPressure_)
        {
            m_frmPlotSamplesEM = frm_;
            m_frmFFTEM = frmFFT_;
            m_pumpPressure = pumpPressure_;
        }


        public CCommonTypes.TELEMETRY_TYPE GetTelemetryType()
        {
            return m_ttID;
        }

        public void Unload()
        {
            m_bExit = true;
        }

        void BackgroundInputProcessor_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Start Data Receiver
            //if (_dataReceiver == null)
            //  StartDataReceiverInBackground(m_sIPAddress, m_iPortNumber);

            DetectSocketDataSingleton dsds20 = DetectSocketDataSingleton.Instance; //2/18/22

            Thread.Sleep(20); //This will avoid the null get for SurveyDataSemaphoreMP //2/24/22

            //The main thread starts out holding the entire 
            //semaphore count. Calling Release() brings the 
            //semaphore count back to its maximum value, and
            //allows the waiting threads to enter the semaphore,
            //up to two at a time.

            if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_EM)
            {
                dsds20.DetectSockData.SurveyDataSemaphoreEM.Release();  //2/22/22
            }
            else if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_MP)
            {
                dsds20.DetectSockData.SurveyDataSemaphoreMP.Release();  //2/22/22
            }

            while (!worker.CancellationPending)
            { 
                if (m_bExit)
                    break;

                //if (!_dataReceiver.IsConnected)
                //{
                //Thread.Sleep(2000);
                //StartDataReceiverInBackground(m_sIPAddress, m_iPortNumber);
                //  continue;
                //}

                //2/16/22ParsedDataPacket pdp = _dataReceiver.NextPacket();
                //2/16/22
                //2/16/22if (pdp == null)
                //2/16/22{
                //2/16/22    m_iPDPNullCount++;
                //2/16/22    if (m_iPDPNullCount > 50)
                //2/16/22    {
                //2/16/22        Thread.Sleep(1000);
                //2/16/22        m_iPDPNullCount = 0;
                //2/16/22        _backgroundInputProcessor.CancelAsync();
                //2/16/22    }
                //2/16/22    worker.ReportProgress(0, pdp);
                //2/16/22}
                //2/16/22else
                //2/16/22{
                //2/16/22    m_iPDPNullCount = 0;
                //2/16/22    if (pdp.Command == Command.COMMAND_RESP_NULL)
                //2/16/22        continue;
                //2/16/22
                //2/16/22    //Debug.WriteLine(pdp.ValueAsString);                    
                //2/16/22    worker.ReportProgress(1, pdp);
                //2/16/22    Thread.Sleep(50);
                //2/16/22}
                //2/16/22
                //2/16/22CEventDetect evDetectConnected = new CEventDetect();
                //2/16/22if (m_bExit)
                //2/16/22    evDetectConnected.m_iConnected = CEventDetect.CONNECTION.CLOSED;
                //2/16/22else
                //2/16/22    evDetectConnected.m_iConnected = _dataReceiver.IsConnected ? CEventDetect.CONNECTION.OPEN : CEventDetect.CONNECTION.CLOSED;
                //2/16/22
                //2/16/22if (DetectConnected != null)
                //2/16/22    DetectConnected(this, evDetectConnected);
                //2/16/22

                DetectSocketDataSingleton dsds = DetectSocketDataSingleton.Instance;

                if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_EM) //2/22/22
                {
                    if (dsds.DetectSockData.SurveyDataSemaphoreEM.WaitOne(500)) //2/52/22
                    {
                        ParsedDataPacket pdp = _dataReceiver.ProcessDataFromNetwork();

                        if (pdp != null)
                        {
                            worker.ReportProgress(1, pdp);
                            //2/21/22Thread.Sleep(50); //2/18/22
                        }

                        dsds.DetectSockData.BytesReadFromSocketEM = 0; //2/26/22

                        dsds.DetectSockData.SurveyDataSemaphoreEM.Release(); //2/21/22

                        Thread.Sleep(50); //2/22/22
                    }
                }
                else if (m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                {
                    if (dsds.DetectSockData.SurveyDataSemaphoreMP.WaitOne(500)) //2//25/22
                    {
                        ParsedDataPacket pdp = _dataReceiver.ProcessDataFromNetwork();

                        if (pdp != null)
                        {
                            worker.ReportProgress(1, pdp);

                            //RouteDataPackets(pdp);  //2/26/22
                            Thread.Sleep(50); //2/2/22
                        }

                        dsds.DetectSockData.BytesReadFromSocketMP = 0; //2/26/22

                        dsds.DetectSockData.SurveyDataSemaphoreMP.Release(); //2/21/22

                        Thread.Sleep(50); //2/26/22
                    }
                }
            } //while (!worker.CancellationPending)
        }

        public void SetupBackgroundWorker()
        {
            m_dblArrSample = new double[RAW_MP_SAMPLE_SIZE];
            _backgroundInputProcessor.WorkerReportsProgress = true;
            _backgroundInputProcessor.WorkerSupportsCancellation = true;

            m_eventProgress = new ProgressChangedEventHandler(BackgroundInputProcessor_ProgressChanged);
            m_eventWorkCompleted = new RunWorkerCompletedEventHandler(BackgroundInputProcessor_RunWorkerCompleted);
            m_eventDoWork = new DoWorkEventHandler(BackgroundInputProcessor_DoWork);

            _backgroundInputProcessor.ProgressChanged += m_eventProgress;
            _backgroundInputProcessor.RunWorkerCompleted += m_eventWorkCompleted;
            _backgroundInputProcessor.DoWork += m_eventDoWork;

            //_backgroundInputProcessor.RunWorkerAsync();
        }

        public bool IsConnected()
        {
            //get { return MPM.Properties.Settings.Default.IsConnectedProperty; }
            //set { Properties.Settings.Default.IsConnectedProperty = value; Properties.Settings.Default.Save(); }
            //get { return true; }
            //set { IsConnected = true; }
            return m_bIsConnected;
        }
        public void SetFlags()
        {
            m_bIsConnected = _dataReceiver.IsConnected;
            //IsNoCurrentBitRun = JobPathRunManager.CurrentBitRun == null;
        }

        void BackgroundInputProcessor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           StartDataReceiverInBackground(m_sIPAddress, m_iPortNumber);
        }

        #region property int PortNumber
        private int _portNumber;
        public int PortNumber
        {
            get { return _portNumber; }
            set { if (_portNumber != value) { _portNumber = value;/* OnPropertyChanged("PortNumber");*/ } }
        }
        #endregion

        public void SetConnectionInfo(string sIPAddress, int iPortNumber_)
        {
            m_sIPAddress = sIPAddress;
            PortNumber = m_iPortNumber = iPortNumber_;
        }

        public void SetWITSListener(CDetectDataLayer dataLayer_)
        {
            m_BitDepth.SetListener(dataLayer_);
            for (int i = 0; i < m_lstDrillingSensor.Count; i++)
                m_lstDrillingSensor[i].SetListener(dataLayer_);
        }

        public void SetECD(ref CEquivalentCirculatingDensity ECD_)
        {
            m_ECD = ECD_;
        }

        public void StartDataReceiverInBackground(string remoteIPAddress, int iPortNumber_)
        {
            m_sIPAddress = remoteIPAddress;
            PortNumber = m_iPortNumber = iPortNumber_;
            _dataReceiver = new DataReceiver(remoteIPAddress, m_iPortNumber, m_ttID); //2/16/22

            m_DetectConnected = new DataReceiver.DetectConnectedEventHandler(ConnectionStatus);
            _dataReceiver.DetectConnected += m_DetectConnected;
            // Set up class data and structures

            //// Start Data Receiver
            //_dataReceiver.Start();                                                   
            _backgroundInputProcessor.RunWorkerAsync();


            SetFlags();
        }

        private void ConnectionStatus(object sender, CEventDetect e)
        {
            Console.WriteLine("ConnectionStatus: " + e.m_iConnected.ToString());


            if (DetectConnected != null)
                DetectConnected(this, e);
        }

        public void EndDataReceiverInBackground()
        {
            UnsetBackgroundWorker();
            _backgroundInputProcessor.CancelAsync();
            _dataReceiver.CloseConnection();
        }

        public void UnsetBackgroundWorker()
        {
            _backgroundInputProcessor.ProgressChanged -= m_eventProgress;
            _backgroundInputProcessor.RunWorkerCompleted -= m_eventWorkCompleted;
            _backgroundInputProcessor.DoWork -= m_eventDoWork;

            _dataReceiver.DetectConnected -= m_DetectConnected;
        }

        public void CloseConnection()
        {
            _dataReceiver.CloseConnection();
        }

        public void RefreshIPAddress(string sVal_, int iPortNumber_)
        {
           //2/24/22 m_sIPAddress = sVal_;
           //2/24/22 m_iPortNumber = iPortNumber_;
           //2/24/22 //if (_dataReceiver != null)
           //2/24/22 //{
           //2/24/22 _dataReceiver.CloseConnection();
           //2/24/22 _dataReceiver.RefreshIPAddress(m_sIPAddress, m_iPortNumber);
           //2/24/22 _dataReceiver.Start();
            //}
            //else
            //{
            //   StartDataReceiverInBackground(m_sIPAddress, m_iPortNumber);
            //}
        }
        void BackgroundInputProcessor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int flag = e.ProgressPercentage;
            bool isConnected = flag == 1;

            //if (_dataReceiver.IsConnected && !isConnected)
            //    SoundLostConnectionAlarm();                      
            _dataReceiver.IsConnected = isConnected;
            SetFlags();

            //2/16/22if (IsConnected())
            {
                ParsedDataPacket pdp = e.UserState as ParsedDataPacket;

                if (true/*JobPathRunManager.CurrentBitRun != null*/)
                RouteDataPackets(pdp);
            }
        }

        private void ProcessUnicodeString(ParsedDataPacket pdp_)
        {
            string sDesc = "";
            string sVal = "";
            string sDesc2 = "";
            string sVal2 = "";
            bool bDoLookUp = true;

            try
            {
                if (AcquiredText != null)
                {
                    CEventDPointText evtText = new CEventDPointText();
                    evtText.m_sText = pdp_.UnicodeString;
                    evtText.m_ttSource = this.m_ttID;
                    AcquiredText(this, evtText);
                }

                //test
                //pdp_.UnicodeString = "Gamma Shock(12 Bit): Axial(X) = 93.3g gees (Hex: 38), Transverse(Y) = 40.0g gees (Hex: 18)";

                if (pdp_.UnicodeString.Contains("="))  // then try to determine if it's a d-point
                {
                    string[] sArr = pdp_.UnicodeString.Split('=');
                    sDesc = sArr[0].Trim();
                    sVal = sArr[1].Trim();

                    if (sDesc.Contains("TF Type"))   // sometimes there isn't an "=" sign to split
                    {
                        if (sDesc.Contains("TFO"))  // parse the sub value
                        {
                            string sSubDesc = "TFO";
                            string sSubVal = sVal;
                            CDPointLookupTable.DPointInfo infoSub = m_DPointTable.Find(sSubDesc);
                            if (infoSub.iMessageCode > -1)
                            {
                                CEventDPoint evSub = new CEventDPoint();
                                evSub.m_sValue = sSubVal;
                                evSub.m_ID = infoSub.iMessageCode;

                                if (Acquired != null)
                                    Acquired(this, evSub);

                                // save to database
                                if (infoSub.iMessageCode == (int)Command.COMMAND_BIT_DEPTH)
                                    m_BitDepth.Set(System.Convert.ToSingle(sSubVal));
                                LogData(infoSub, DateTime.Now, sSubVal, false);
                            }
                        }
                        sVal = sDesc.ElementAt(8).ToString();
                        sDesc = "TF Type";
                    }
                    else if (sDesc.Contains("Mag Dec"))
                    {
                        sDesc = "Mag Dec";
                    }
                    else if (sDesc.Contains("DLRigCode"))  // avoids dealing with the "[1]" that follows
                    {
                        sDesc = "DLRigCode";
                    }
                    else if (sDesc.ToLower().Contains("swr"))
                    {
                        if (sDesc.ToLower().Substring(0, 3) == "swr")
                        {
                            sVal = sVal.Replace("(Hex:", "");
                            sVal = sVal.Replace(")", "");
                            sVal = sVal.Trim();
                        }
                    }
                    //3/31/22
                    else if (sDesc.ToLower().Contains("accelerometer"))
                    {
                        if (sDesc.ToLower().Substring(0, 13) == "accelerometer")
                        {
                            sVal = sVal.Replace("gees (Hex:", "");
                            sVal = sVal.Replace(")", "");
                            sVal = sVal.Trim();
                        }
                    }
                    else if (sDesc.ToLower().Contains("magnetometer"))
                    {
                        if (sDesc.ToLower().Substring(0, 12) == "magnetometer")
                        {
                            sVal = sVal.Replace("Gauss (Hex:", "");
                            sVal = sVal.Replace(")", "");
                            sVal = sVal.Trim();
                        }
                    }
                    //3/31/22
                    else if (sDesc.ToLower().Contains("gamma shock(12 bit):"))
                    {
                        // strip off the "gamma shock(12 bit):"
                        sDesc = pdp_.UnicodeString.Substring(20);
                        string[] sArrGammaShock = sDesc.Split(',');

                        string[] sArrGammaShockValue = sArrGammaShock[0].Split('=');
                        string sAxialVal = sArrGammaShockValue[1].Substring(0, sArrGammaShockValue[1].IndexOf('g'));
                        sDesc = "Gamma Shk Axial (12 bit)";
                        sVal = sAxialVal.Trim();

                        string[] sArrGammaShockTransverse = sArrGammaShock[1].Split('=');
                        string sTransverseVal = sArrGammaShockTransverse[1].Substring(0, sArrGammaShockTransverse[1].IndexOf('g'));
                        sDesc2 = "Gamma Shk Transverse (12 bit)";
                        sVal2 = sTransverseVal.Trim();
                    }

                    int iSpaceAfterValue = sVal.IndexOf(' ');
                    if (iSpaceAfterValue > 0)
                        sVal = sVal.Substring(0, iSpaceAfterValue);
                }
                else  // no equal sign
                {
                    sDesc = pdp_.UnicodeString;
                    if (sDesc.Contains("Parity Check Error in "))  // hold information on which D-point has the parity error
                    {
                        string sErrorDesc = sDesc;
                        sErrorDesc = sErrorDesc.Replace("Parity Check Error in ", "");
                        int iEndOfDescPos = sErrorDesc.IndexOf(" on");
                        sErrorDesc = sErrorDesc.Substring(0, iEndOfDescPos).Trim();
                        CDPointLookupTable.DPointInfo infoError = m_DPointTable.Find(sErrorDesc);
                        if (infoError.iMessageCode > -1)
                        {
                            m_evtParityError.m_ID = infoError.iMessageCode;
                            m_evtParityError.m_bIsParityError = true;
                        }
                        bDoLookUp = false;
                    }
                    else if (sDesc.Contains("Power"))
                    {
                        sDesc = sDesc.Trim();
                        if (sDesc[0] == 'P' && sDesc[4] == 'r' && sDesc[5] == ' ')
                        {
                            sVal = sDesc.Substring(6);
                            sDesc = "Power";
                        }
                    }
                    else if (sDesc.Contains("Battery"))
                    {
                        sDesc = sDesc.Trim();
                        if (sDesc[0] == 'B' && sDesc[6] == 'y' && sDesc[7] == ' ' && sDesc[8] != 'A')
                        {
                            sVal = sDesc.Substring(8, 1);
                            sDesc = "Battery";
                        }
                        else if (sDesc[0] == 'B' && sDesc[6] == 'y' && sDesc[7] == ' ' && (sDesc[8] == 'A' || sDesc[10] == 'A')) // must be "all"
                        {
                            sVal = "3";  // same as when battery 2 has been switched "on"  
                            sDesc = "Battery";
                        }
                    }
                    else if (sDesc.Contains("Rotary Steerable Diagnostics"))
                    {
                        int kHexPos = sDesc.IndexOf("0x");
                        if (kHexPos > 0)
                            sVal = sDesc.Substring(kHexPos + 2, 1);
                        sDesc = "Rotary Steerable Diag";
                    }
                    else if (sDesc.ToLower().Contains("checksum calc"))
                    {
                        string sSearchText = "net";
                        int iNetPos = sDesc.ToLower().IndexOf(sSearchText);
                        if (iNetPos > 0)
                        {
                            sVal = sDesc.Substring(iNetPos + sSearchText.Length).Trim();
                            sDesc = sSearchText.ToUpper();
                        }
                    }
                    else
                        bDoLookUp = false;
                }


                if (bDoLookUp)
                {
                    CDPointLookupTable.DPointInfo info = m_DPointTable.Find(sDesc);
                    Send(info, sDesc, sVal);
                    if (sDesc2.Length > 0 && sVal.Length > 0)
                    {
                        CDPointLookupTable.DPointInfo info2 = m_DPointTable.Find(sDesc2);
                        Send(info2, sDesc2, sVal2);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Error at CDetect::ProcessUnicodeString: " + ex.Message);
            }

        }

        void Send(CDPointLookupTable.DPointInfo info, string sDesc, string sVal)
        {
            if (info.iMessageCode > -1)
            {
                CEventDPoint ev = new CEventDPoint();
                // handle d-points with mapped values
                if (info.utDataType == CDPointLookupTable.UNIT_TYPE.UT_MAP)
                {
                    string[] sArrMapping = info.sValueMapping.Split(',');
                    int iIndex = Convert.ToInt16(sVal);
                    if (sArrMapping.Count() > iIndex)
                    {
                        sVal = sArrMapping[iIndex];
                    }
                }

                ev.m_iTechnology = m_ttID;
                ev.m_sValue = sVal;
                ev.m_DateTime = DateTime.Now;  // this should come from detect
                ev.m_ID = info.iMessageCode;

                if (m_evtParityError.m_ID == info.iMessageCode)
                {
                    ev.m_bIsParityError = true;
                    m_evtParityError.m_ID = -1;  // reset
                }
                else
                    ev.m_bIsParityError = false;

                // *****************************************************************
                // might need to convert units for display
                // but we keep the original value unchanged when saved to the database
                // *****************************************************************
                if (info.utDataType == CDPointLookupTable.UNIT_TYPE.UT_DOUBLE)
                {
                    string sUnitLowerCase = info.sUnits.ToLower();
                    if (sUnitLowerCase == m_TemperatureUnit.GetImperialUnitDesc().ToLower() || sUnitLowerCase == m_TemperatureUnit.GetMetricUnitDesc().ToLower())
                        ev.m_sValue = System.Convert.ToString(m_TemperatureUnit.Convert(System.Convert.ToSingle(sVal)));
                    else if (sUnitLowerCase == m_PressureUnit.GetImperialUnitDesc().ToLower() || sUnitLowerCase == m_PressureUnit.GetMetricUnitDesc().ToLower())
                        ev.m_sValue = System.Convert.ToString(m_PressureUnit.Convert(System.Convert.ToSingle(sVal)));
                    else if (sUnitLowerCase == m_LengthUnit.GetImperialUnitDesc().ToLower() || sUnitLowerCase == m_LengthUnit.GetMetricUnitDesc().ToLower())
                        ev.m_sValue = System.Convert.ToString(m_LengthUnit.Convert(System.Convert.ToSingle(sVal)));
                    else if (sUnitLowerCase == m_ROPUnit.GetImperialUnitDesc().ToLower() || sUnitLowerCase == m_ROPUnit.GetMetricUnitDesc().ToLower())
                        ev.m_sValue = System.Convert.ToString(m_ROPUnit.Convert(System.Convert.ToSingle(sVal)));
                }

                if (m_bValidLicense)
                    if (Acquired != null)
                        Acquired(this, ev);
                //System.Diagnostics.Debug.Print(sDesc + " " + sVal);                    

                if (info.iMessageCode == (int)Command.COMMAND_BIT_DEPTH)
                    m_BitDepth.Set(System.Convert.ToSingle(sVal));
                DateTime dtCurrent = DateTime.Now;
                LogData(info, dtCurrent, sVal, ev.m_bIsParityError);

                // ***************************************
                // for LAS exports, log the sensor values 
                // ***************************************
                for (int i = 0; i < m_lstDrillingSensor.Count; i++)
                {
                    LogData(m_lstDrillingSensor[i].GetDPointInfo(), dtCurrent, m_lstDrillingSensor[i].Get().ToString(), false);
                }
            }
            else
            {
                System.Diagnostics.Debug.Print("NOT FOUND:" + sDesc);
            }
        }

        void ProcessDouble(ParsedDataPacket pdp_)
        {
            CDPointLookupTable.DPointInfo info = m_DPointTable.Find((int)pdp_.Command);
            if (info.iMessageCode > -1)
            {
                CEventDPoint ev = new CEventDPoint();

                ev.m_iTechnology = m_ttID;
                ev.m_sValue = pdp_.ValueDouble.ToString();
                ev.m_DateTime = DateTime.Now;  // this should come from detect
                ev.m_ID = info.iMessageCode;

                if (m_evtParityError.m_ID == info.iMessageCode)
                {
                    ev.m_bIsParityError = true;
                    m_evtParityError.m_ID = -1;  // reset
                }
                else
                    ev.m_bIsParityError = false;

                if (m_bValidLicense)
                    if (Acquired != null)
                        Acquired(this, ev);

                DateTime dtCurrent = DateTime.Now;
                LogData(info, dtCurrent, ev.m_sValue, ev.m_bIsParityError);
            }
        }


        private void ProcessRaw(ParsedDataPacket pdp_)
        {
            if (m_dtSamples.Year == 1970)
                m_dtSamples = DateTime.Now;
            else
                m_dtSamples = m_dtSamples.AddMilliseconds(500);  // each packet is worth half a second


            if (m_packetRec.dtLast.Year == 1970)  // initial state
            {
                m_packetRec.dtLast = DateTime.Now;
                m_packetRec.iCnt = 1;
            }
            else  // only allow speeds up to 2 so it doesn't lock up the application
            {
                TimeSpan ts = DateTime.Now - m_packetRec.dtLast;
                if (ts.Seconds < 1)
                {
                    m_packetRec.iCnt++;                   

                    if (m_packetRec.iCnt > MAX_PACKET_PER_SEC)
                    {
                        CEventDPoint ev = new CEventDPoint();
                        ev.m_iTechnology = m_ttID;
                        ev.m_sValue = m_packetRec.iCnt.ToString();
                        ev.m_DateTime = DateTime.Now;  // this should come from detect
                        if (ev.m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                            ev.m_ID = (int)Command.COMMAND_RAW_EM_CNT;
                        else
                            ev.m_ID = (int)Command.COMMAND_RAW_MP_CNT;
                        ev.m_bIsParityError = false;
                        if (Acquired != null)
                            Acquired(this, ev);                       
                    }                    
                }                    
                else  // reset
                {
                    CEventDPoint ev = new CEventDPoint();
                    ev.m_iTechnology = m_ttID;
                    ev.m_sValue = m_packetRec.iCnt.ToString();
                    ev.m_DateTime = DateTime.Now;  // this should come from detect
                    if (ev.m_iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                        ev.m_ID = (int)Command.COMMAND_RAW_EM_CNT;
                    else
                        ev.m_ID = (int)Command.COMMAND_RAW_MP_CNT;
                    ev.m_bIsParityError = false;
                    if (Acquired != null)
                        Acquired(this, ev);

                    m_packetRec.dtLast = DateTime.Now;
                    m_packetRec.iCnt = 0;                    
                }                
            }

            CEventRawMPSamples evRaw = new CEventRawMPSamples(pdp_.Raw.Data, pdp_.Raw.Count, m_dtSamples, (int)m_ttID);
            if (this.m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_MP)
            {
                m_frmPlotSamples.Plot(evRaw);
                m_frmFFT.PlotData(evRaw);
            }                
            else
            {
                m_frmPlotSamplesEM.Plot(evRaw);
                m_frmFFTEM.PlotData(evRaw);
            }


            if (this.m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_MP)  //5/2/22 added back
                m_pumpPressure.SetSamples(evRaw);

        }

        private void LogData(CDPointLookupTable.DPointInfo info, DateTime dt_, string sVal_, bool bParityError_)
        {
            CLogDataRecord rec = new CLogDataRecord();
            rec.iMessageCode = info.iMessageCode;
            rec.sCreated = dt_.ToString("yyyy-MM-dd HH:mm:ss.fff");
            rec.iDate = System.Convert.ToInt32(dt_.ToString("yyyyMMdd"));
            rec.iTime = System.Convert.ToInt32(dt_.ToString("HHmmss"));
            rec.sName = info.sDescriptiveName;  // 
            rec.sValue = sVal_;
            rec.sUnit = info.sUnits;
            rec.bParityError = bParityError_;
            rec.fDepth = m_BitDepth.Get();
            rec.sTelemetry = m_ttID == CCommonTypes.TELEMETRY_TYPE.TT_EM ? "EM" : "MP";

            CLogDataLayer log = new CLogDataLayer(ref m_dbCnn);
            log.Save(rec);
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET iVal_)
        {
            m_UnitSet = iVal_;
            m_LengthUnit.SetUnitType(m_UnitSet);
            m_PressureUnit.SetUnitType(m_UnitSet);
            m_TemperatureUnit.SetUnitType(m_UnitSet);
        }

        private void SaveECDToDB(float fECD_, float fHSP_, bool bIsMetric_)
        {
            DateTime dt = DateTime.Now;

            // save the ecd
            CLogDataRecord recECD = new CLogDataRecord();
            recECD.iMessageCode = (int)Command.COMMAND_ECD_TVD;
            recECD.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            recECD.iDate = System.Convert.ToInt32(dt.ToString("yyyyMMdd"));
            recECD.iTime = System.Convert.ToInt32(dt.ToString("HHmmss"));
            recECD.sName = "ECD";
            recECD.sValue = fECD_.ToString();
            recECD.sUnit = bIsMetric_ ? "kg/m3": "lb/gal";
            recECD.bParityError = false;
            recECD.fDepth = m_ECD.GetDepth();
            recECD.sTelemetry = m_ECD.GetTelemetryType() == CCommonTypes.TELEMETRY_TYPE.TT_EM ? "EM" : "MP";

            CLogDataLayer logDetect = new CLogDataLayer(ref m_dbCnn);
            logDetect.Save(recECD);              

            // save the hydrostatic pressure
            CLogDataRecord recHSP = new CLogDataRecord();
            recHSP.iMessageCode = (int)Command.COMMAND_HYDRO_STATIC_PRESSURE;
            recHSP.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            recHSP.iDate = System.Convert.ToInt32(dt.ToString("yyyyMMdd"));
            recHSP.iTime = System.Convert.ToInt32(dt.ToString("HHmmss"));
            recHSP.sName = "Hydrostatic Pressure";
            recHSP.sValue = fHSP_.ToString();
            recHSP.sUnit = bIsMetric_ ? "kPa" : "psi";
            recHSP.bParityError = false;
            recHSP.fDepth = m_ECD.GetDepth();
            recHSP.sTelemetry = m_ECD.GetTelemetryType() == CCommonTypes.TELEMETRY_TYPE.TT_EM ? "EM" : "MP";

            logDetect.Save(recHSP);
        }

        public void RouteDataPackets(ParsedDataPacket pdp)
        {

            if (pdp == null)
                return;

            if (pdp.Command != Command.COMMAND_RESP_CRC_BAD || pdp.Command != Command.COMMAND_RESP_CRC_GOOD)
                m_cmdCurrent = pdp.Command;

            //DataPackets.Add(pdp);
            System.Diagnostics.Debug.WriteLine("CMD " + pdp.Command.ToString() + " " + pdp.UnicodeString + ";  " + DateTime.Now.ToString());
            switch (pdp.Command)
            {
                case Command.COMMAND_RESP_LOG_STRING_UNICODE:
                    ProcessUnicodeString(pdp);
                    System.Diagnostics.Debug.WriteLine("UNICODE " + pdp.UnicodeString);

                    break;
                case Command.COMMAND_RESP_RIGCODE:

                    break;
                case Command.COMMAND_RESP_GAMMA:
                    ///ProcessGamma(pdp);
                    break;
                case Command.COMMAND_RESP_TEMPERATURE:
                    //ProcessTemperature(pdp);
                    break;
                case Command.COMMAND_RESP_POWER:
                    //ProcessPower(pdp);
                    break;
                case Command.COMMAND_SIG_STRENGTH:
                    ProcessDouble(pdp);
                    break;
                case Command.COMMAND_RESP_B_PRESSURE:
                    // is there any unit conversion?                    
                    pdp.ValueDouble = m_PressureUnit.Convert((float)pdp.ValueDouble);
                    ProcessDouble(pdp);
                    break;
                case Command.COMMAND_RESP_A_PRESSURE:
                
                    // is there any unit conversion?                    
                    pdp.ValueDouble = m_PressureUnit.Convert((float)pdp.ValueDouble);

                    ProcessDouble(pdp);

                    try
                    {
                        m_ECD.SetValue(this.m_ttID, (int)pdp.Command, System.Convert.ToSingle(pdp.ValueDouble.ToString()));
                        if (m_ECD.IsReady())
                        {
                            // post ecd 
                            float fECD = m_ECD.CalculateECD();
                            CEventDPoint evECD = new CEventDPoint();
                            evECD.m_iTechnology = m_ttID;
                            evECD.m_DateTime = DateTime.Now;
                            evECD.m_sValue = fECD.ToString("0.00");
                            evECD.m_ID = (int)Command.COMMAND_ECD_TVD;
                            if (Acquired != null)
                                Acquired(this, evECD);

                            // post hydrostatic pressure
                            float fHSP = m_ECD.CalculateHydrostaticPressure();
                            CEventDPoint evHSP = new CEventDPoint();
                            evHSP.m_iTechnology = this.m_ttID;
                            evHSP.m_DateTime = DateTime.Now;
                            evHSP.m_sValue = fHSP.ToString("0.00");
                            evHSP.m_ID = (int)Command.COMMAND_HYDRO_STATIC_PRESSURE;
                            if (Acquired != null)
                                Acquired(this, evHSP);

                            // post the mud density
                            float fMudDensity = m_ECD.GetMudDensity();
                            CEventDPoint evMudDensity = new CEventDPoint();
                            evMudDensity.m_iTechnology = this.m_ttID;
                            evMudDensity.m_DateTime = DateTime.Now;
                            evMudDensity.m_sValue = fMudDensity.ToString("0.00");
                            evMudDensity.m_ID = (int)Command.COMMAND_MUD_DENSITY;
                            if (Acquired != null)
                                Acquired(this, evMudDensity);

                            // save to db
                            SaveECDToDB(fECD, fHSP, m_ECD.GetLengthUnit() == "m");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ECD Error: " + e.Message);
                    }
                    break;
                case Command.COMMAND_RESP_VIBX:
                    //ProcessVibX(pdp);
                    break;
                case Command.COMMAND_RESP_VIBY:
                    //ProcessVibY(pdp);
                    break;

                case Command.COMMAND_RESP_NB_GAMMAUP:
                    //ProcessNBGammaUp(pdp);
                    break;
                case Command.COMMAND_RESP_NB_GAMMADOWN:
                    //ProcessNBGammaDown(pdp);
                    break;

                case Command.COMMAND_RESP_VIB:
                    //DataVib = pdp.ValueInt;
                    break;

                case Command.COMMAND_PASON_DEPTH:
                case Command.COMMAND_DEPTHTRACKER_EXTRAINFO:
                    // ProcessDepthData(pdp);
                    break;

                case Command.COMMAND_RESP_INCLINATION:
                    // ProcessInclination(pdp);
                    break;
                case Command.COMMAND_RESP_AZIMUTH:
                    //ProcessAzimuth(pdp);
                    break;

                case Command.COMMAND_RESP_RAW:
                    //System.Diagnostics.Debug.WriteLine("Getting raw");
                    //try
                    //{
                    //    for (int i = 0; i < pdp.Raw.Data.Count(); i++)
                    //    {
                    //        ListUnfiltered.Add(pdp.Raw.Data[i]);
                    //        //unfilteredGraph.PlotXYAppend((double)NationalInstruments.DataConverter.Convert(DateTime.Now, typeof(double)), pdp.Raw.Data[i]);
                    //        if (i < SampleSize)
                    //        {
                    //            samples[i % SmoothCount] = (double)pdp.Raw.Data[i];
                    //            if (i % SmoothCount == 1)
                    //            {
                    //                currentPressure = getPSI(samples);
                    //                processPressure(currentPressure);
                    //            }
                    //        }
                    //    }


                    //}
                    //catch (Exception Ex) { }
                    ProcessRaw(pdp);
                    Console.WriteLine("COMMAND_RESP_RAW DATA SIZE = " + pdp.Raw.Data.Count());
                    break;
                case Command.COMMAND_RESP_FILTERED:
                    Console.WriteLine("COMMAND_RESP_FILTERED DATA SIZE = " + pdp.Filtered.Data.Count());
                    //System.Diagnostics.Debug.WriteLine("getting filtered data");
                    // ProcessFiltered(pdp);
                    //for (int i = 0; i < pdp.Filtered.Data.Count(); i++)
                    //{
                    //    ListFiltered.Add(pdp.Filtered.Data[i]);
                    //    //unfilteredGraph.PlotXYAppend((double)NationalInstruments.DataConverter.Convert(DateTime.Now, typeof(double)), pdp.Raw.Data[i]);
                    //    if (i < SampleSize)
                    //    {
                    //        samples[i] = (double)pdp.Filtered.Data[i];
                    //    }
                    //}
                    //currentPressure = getPSI(samples);
                    //standPipeTextBox.Text = BigAverageValue.ToString("F2");
                    //if (Properties.Settings.Default.UnitsType == 1)
                    //{ //Convert to KPA
                    //    currentPressure = 6.894757293168361 * currentPressure;
                    //}
                    //if (Properties.Settings.Default.TransducerSize == 1)
                    //{
                    //    currentPressure = currentPressure * 2;
                    //}

                    ////unfilteredGraph.PlotXYAppend((double)NationalInstruments.DataConverter.Convert(DateTime.Now, typeof(double)), currentPressure);
                    //unfilteredGraph.PlotXYAppend(SampleCounter++, currentPressure);

                    ////unfilteredGraph.XAxes[0].MajorDivisions.LabelFormat = new NationalInstruments.UI.FormatString(NationalInstruments.UI.FormatStringMode.DateTime, "h:mm:ss tt");
                    //if (Properties.Settings.Default.LockScale)
                    //{
                    //    unfilteredGraph.InteractionModeDefault = NationalInstruments.UI.GraphDefaultInteractionMode.None;
                    //    unfilteredGraph.Plots[0].YAxis.Range = new NationalInstruments.UI.Range(Properties.Settings.Default.YAxisMin, Properties.Settings.Default.YAxisMax);
                    //}
                    //else
                    //{
                    //    unfilteredGraph.InteractionModeDefault = NationalInstruments.UI.GraphDefaultInteractionMode.ZoomAroundPoint;
                    //}
                    //if (currentPressure > Properties.Settings.Default.PumpsOn)
                    //{
                    //    statusLabel.Text = "Status: Pumps On";
                    //}
                    //else if (currentPressure < 50)
                    //{
                    //    statusLabel.Text = "Status: Pumps Off";
                    //}

                    break;

                case Command.COMMAND_RESP_NOISE:
                    ProcessDouble(pdp);
                    break;
                case Command.COMMAND_RESP_SHOCKMAX_AXIAL:
                case Command.COMMAND_RESP_SHOCKMAX_TRANSVERSE:
                    ProcessDouble(pdp);
                    break;
                case Command.COMMAND_RESP_GX:
                case Command.COMMAND_RESP_GY:
                case Command.COMMAND_RESP_GZ:
                    // ProcessGravityValues(pdp);
                    break;

                case Command.COMMAND_RESP_BX:
                case Command.COMMAND_RESP_BY:
                case Command.COMMAND_RESP_BZ:
                    // ProcessMagneticValues(pdp);
                    break;

                case Command.COMMAND_RESP_deltaMTF:
                    /// ProcessDeltaMTF(pdp);
                    break;

                case Command.COMMAND_MDecl:
                    // ProcessMDecl(pdp);
                    break;

                case Command.COMMAND_RESP_MAGDEC:
                case Command.COMMAND_RESP_TFO:
                case Command.COMMAND_RESP_GAMMACOR:
                    break;
                case Command.COMMAND_RESP_GAMMATOBIT:

                    break;
                case Command.COMMAND_RESP_DIRTOBIT:
                    ProcessDouble(pdp);
                    break;

                case Command.COMMAND_RESP_TF:
                    // ProcessToolFace(pdp);
                    break;

                case Command.COMMAND_RESP_CRC_GOOD:
                    //confidenceLabel.Text = "Confidence: 90%";
                    System.Diagnostics.Debug.WriteLine("Confidence: 90%");
                    // ProcessCRCGood(pdp, true);
                    break;

                case Command.COMMAND_RESP_CRC_BAD:
                    //confidenceLabel.Text = "Confidence: 10%";
                    System.Diagnostics.Debug.WriteLine("Confidence: 10%");
                    // ProcessCRCGood(pdp, false);
                    break;

                case Command.COMMAND_RESP_FORMRES:
                    // DataResistivity = pdp.ValueLong;
                    break;

                case Command.COMMAND_RESP_BATTERY:
                    //  DataBattery = pdp.ValueInt;
                    break;

                case Command.COMMAND_RESP_BT:
                    // DataMagneticTotal = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageMagneticTotal);
                    break;

                case Command.COMMAND_RESP_TFTYPE:
                    // DataToolfaceType = pdp.ToolFaceType;
                    break;

                case Command.COMMAND_RESP_PACKET_TYPE:
                    //statusLabel.Text = "Status: Packet " + pdp.ValueInt.ToString();
                    System.Diagnostics.Debug.WriteLine(string.Format("Packet Type:  {0}", pdp.ValueInt));
                    //if (pdp.ValueInt == 0 ||  // angle survey
                    //    pdp.ValueInt == 1 ||  // angle survey with gamma
                    //    pdp.ValueInt == 4 ||  // vector survey
                    //    pdp.ValueInt == 5 ||  // vector survey with gamma
                    //    pdp.ValueInt == 6 ||  // vector survey with pressure
                    //    pdp.ValueInt == 7 ||  // short angle survey with gamma
                    //    pdp.ValueInt == 15)  // short angle survey
                    //{
                    // post to whoever is listening
                    CEventDPoint ev = new CEventDPoint();
                    ev.m_DateTime = DateTime.Now;  // should really come from detect
                    ev.m_sValue = pdp.ValueInt.ToString();
                    ev.m_ID = (int)Command.COMMAND_RESP_PACKET_TYPE;
                    if (Acquired != null)
                        Acquired(this, ev);

                    try
                    {
                        m_ECD.SetValue(this.m_ttID, (int)Command.COMMAND_RESP_PACKET_TYPE, (float)pdp.ValueInt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Possibly no ECD object defined. Restart Display: " + ex.Message, "ECD Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    //}
                    break;

                case Command.COMMAND_RESP_GT:
                    // DataAccelerationTotal = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAccelerationTotal);
                    break;

                case Command.COMMAND_RESP_DIPANGLE:
                    //  DipAngle = pdp.ValueDouble;
                    //string message = string.Format("Processing DipAngle: {0}", DipAngle);
                    //Debug.WriteLine(message);
                    //  CallOnPropertyChanged(MessageDipAngleUpdateFlag);
                    break;

                case Command.COMMAND_RESP_AUX_AZIMUTH:
                    // AuxAzimuth = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxAzimuth);
                    break;

                case Command.COMMAND_RESP_AUX_INC:
                    // AuxInclination = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxInclination);
                    break;

                case Command.COMMAND_RESP_AUX_TOOLFACE:
                    // AuxToolface = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxToolface);
                    break;

                case Command.COMMAND_RESP_PUMP_PRESSURE:
                case Command.COMMAND_RESP_AUX_PRESSURE:
                    // is there any unit conversion?                    
                    pdp.ValueDouble = m_PressureUnit.Convert((float)pdp.ValueDouble);
                    break;

                case Command.COMMAND_RESP_AUX_TEMP:
                    // AuxTemperature = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxTemps);
                    break;

                case Command.COMMAND_RESP_AUX_GAMMA:
                    // AuxGamma = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxGamma);
                    break;

                case Command.COMMAND_RESP_AUX_SHOCK:
                    // AuxShock = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxShock);
                    break;

                case Command.COMMAND_RESP_PULSER_BAUD:
                    System.Diagnostics.Debug.WriteLine("BAUD: " + (pdp.ValueInt / 100.0).ToString());
                    System.Diagnostics.Debug.WriteLine((pdp.ValueInt == 1500) ? "PW: 0.5 sec" : "PW: 1 sec");
                    break;
            }
        }
    }
}
