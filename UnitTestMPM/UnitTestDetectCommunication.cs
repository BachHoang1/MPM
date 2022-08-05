using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.DataAcquisition;
using MPM.DataAcquisition.Helpers;
using System.ComponentModel;
using System.Threading;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestDetectCommunication
    {
        BackgroundWorker _backgroundInputProcessor = new BackgroundWorker();
        DataReceiver _dataReceiver;
        private const int _detectPortNumber = 58380;
        int SampleSize = 225;        
        Command CurrentCommand;
        int pdpNullCount;
        double[] samples;
        bool bIsConnected;
        string m_sRemoteIPAddress;

        void BackgroundInputProcessor_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Start Data Receiver
            _dataReceiver.Start();

            while (!worker.CancellationPending)
            {
                ParsedDataPacket pdp = _dataReceiver.NextPacket();

                if (pdp == null)
                {
                    pdpNullCount++;
                    if (pdpNullCount > 50)
                    {
                        Thread.Sleep(1000);
                        pdpNullCount = 0;
                        _backgroundInputProcessor.CancelAsync();
                    }
                    worker.ReportProgress(0, pdp);
                }
                else
                {
                    pdpNullCount = 0;
                    if (pdp.Command == Command.COMMAND_RESP_NULL)
                        continue;

                    //Debug.WriteLine(pdp.ValueAsString);
                    //if (pdp.Command != Command.COMMAND_RESP_RAW && pdp.Command != Command.COMMAND_RESP_FILTERED)
                    {
                        worker.ReportProgress(1, pdp);
                    }
                    
                }
            }
        }

        private void SetupBackgroundWorker()
        {            
            samples = new double[SampleSize];
            _backgroundInputProcessor.WorkerReportsProgress = true;
            _backgroundInputProcessor.WorkerSupportsCancellation = true;
            _backgroundInputProcessor.ProgressChanged += new ProgressChangedEventHandler(BackgroundInputProcessor_ProgressChanged);
            _backgroundInputProcessor.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundInputProcessor_RunWorkerCompleted);
            _backgroundInputProcessor.DoWork += new DoWorkEventHandler(BackgroundInputProcessor_DoWork);
        }

        public bool IsConnected()
        {
            //get { return MPM.Properties.Settings.Default.IsConnectedProperty; }
            //set { Properties.Settings.Default.IsConnectedProperty = value; Properties.Settings.Default.Save(); }
            //get { return true; }
            //set { IsConnected = true; }
            return bIsConnected;
        }
        public void SetFlags()
        {
            bIsConnected = _dataReceiver.IsConnected;
            //IsNoCurrentBitRun = JobPathRunManager.CurrentBitRun == null;
        }

        void BackgroundInputProcessor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartDataReceiverInBackground(m_sRemoteIPAddress);
        }

        #region property int PortNumber
        private int _portNumber;
        public int PortNumber
        {
            get { return _portNumber; }
            set { if (_portNumber != value) { _portNumber = value;/* OnPropertyChanged("PortNumber");*/ } }
        }
        #endregion

        public void StartDataReceiverInBackground(string remoteIPAddress)
        {
            _dataReceiver = new DataReceiver(remoteIPAddress, _detectPortNumber, MPM.Data.CCommonTypes.TELEMETRY_TYPE.TT_MP); //2/22/22
            m_sRemoteIPAddress = remoteIPAddress;
            // Set up class data and structures
            PortNumber = _detectPortNumber;

            //// Start Data Receiver
            //_dataReceiver.Start();

            _backgroundInputProcessor.RunWorkerAsync();

            SetFlags();
        }

        void BackgroundInputProcessor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int flag = e.ProgressPercentage;
            bool isConnected = flag == 1;

            //if (_dataReceiver.IsConnected && !isConnected)
            //    SoundLostConnectionAlarm();

            _dataReceiver.IsConnected = isConnected;
            SetFlags();

            if (IsConnected())
            {
                ParsedDataPacket pdp = e.UserState as ParsedDataPacket;

                if (true/*JobPathRunManager.CurrentBitRun != null*/)
                    RouteDataPackets(pdp);
            }
        }

        public void RouteDataPackets(ParsedDataPacket pdp)
        {

            if (pdp == null)
                return;

            if (pdp.Command != Command.COMMAND_RESP_CRC_BAD || pdp.Command != Command.COMMAND_RESP_CRC_GOOD)
                CurrentCommand = pdp.Command;

            //DataPackets.Add(pdp);

            switch (pdp.Command)
            {
                case Command.COMMAND_RESP_LOG_STRING_UNICODE:
                    //ProcessUnicodeString(pdp);
                    System.Diagnostics.Debug.Write(pdp.UnicodeString);
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
                    //ProcessSignalStrength(pdp);
                    break;
                case Command.COMMAND_RESP_A_PRESSURE:
                case Command.COMMAND_RESP_B_PRESSURE:
                    //ProcessPressure(pdp);
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
                    System.Diagnostics.Debug.WriteLine("Getting raw");
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
                    //ProcessRaw(pdp);
                    break;
                case Command.COMMAND_RESP_FILTERED:
                    System.Diagnostics.Debug.WriteLine("getting filtered data");
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
                    // ProcessNoise(pdp);
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
                case Command.COMMAND_RESP_GAMMATOBIT:
                case Command.COMMAND_RESP_DIRTOBIT:
                    //ProcessReturns(pdp);
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
                    // AuxPumpPressure = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxPumpPressure);
                    break;

                case Command.COMMAND_RESP_AUX_PRESSURE:
                    // AuxPressure = pdp.ValueDouble;
                    // CallOnPropertyChanged(MessageAuxPressure);
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

        [TestMethod]
        public void TestConnect()
        {
            SetupBackgroundWorker();
            StartDataReceiverInBackground("127.7.0.1");
            while (true)
                ;
        }
    }
}
