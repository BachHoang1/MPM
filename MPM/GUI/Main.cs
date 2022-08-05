
// author: hoan chau
// purpose: to be the parent container for most windows; and to be the main interface for users

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MPM.Data;
using MPM.DataAcquisition;
using MPM.Utilities;
using License;
using System.Threading;
using MPM.Properties;
using System.Data.Common;
using MPM.DataAcquisition.Helpers;
using MPM.DataAcquisition.MultiStationAnalysis;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MPM.GUI
{    
    public partial class Main : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private const int LOW_RES_HEIGHT_OFFSET = 30;
        private const int BLUE_BORDER_THICKNESS_TO_REMOVE = 5;  // the blue line that separates groups of widgets

        // indicates which mode Display is in
        public const string SERVER_MODE = "SERVER"; 
        public const string CLIENT_MODE = "CLIENT";

        private const string BALLOON_TIP_TITLE = "Display Server";
        

        private const int MAX_DETECT_LOG_LINES = 256;
        private const int PIXELS_TO_SLIDE = 25;


        DbProviderFactory m_dbFactory = DbProviderFactories.GetFactory("System.Data.SQLite");
        DbConnection m_dbConn;            

        //private bool m_bIsLowRes;  // flag to indicate if screen resolution is poor

        // unit selection
        public CCommonTypes.UNIT_SET m_iUnitSelection;

        // communication and data acquisition
        private CDetect m_DetectEM;
        private CDetect m_DetectMP;

        //2/15/22
        private DataAcqDetect m_DataAcqDetectEM;
        private DataAcqDetect m_DataAcqDetectMP;
        //2/15/22

        private CEventDetect.CONNECTION m_iDetectStatus;
        private CEventDetect.CONNECTION m_iDetectStatusMP;
        private delegate void ChangeDetectStatusDelegate();
        private ChangeDetectStatusDelegate changeDetectstatusDel;

        private CPason m_Pason;
        private bool m_bWITSStatus;
        private delegate void ChangeWITSStatusDelegate();
        private ChangeWITSStatusDelegate changeWITSStatusDelegate;

        private CPason m_OutgoingWITS;

        CPumpPressure m_PumpPressure;

        // data access layer
        CDetectDataLayer m_DataLayer;
        public CDPointLookupTable m_DPointTable;
        CPressureTransducer m_pressureTransducer;
        CUnitSelection m_unitSelection;

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private DeserializeDockContent m_deserializeDockContent;
        private bool m_bSaveLayout = true;

        // windows
        FormToolFace m_frmTF;
        FormQualifiers m_frmQualifiers;
        FormPlotSamples m_frmPlotSamples;
        FormPlotSamplesEM m_frmPlotSamplesEM;
        FormFFT m_frmFFT;
        FormFFTEM m_frmFFTEM;
        FormGamma m_frmGamma;
        FormINCWhileDrilling m_frmINCWhileDrilling;
        FormPumpPowerTemp m_frmPumpPowerTemp;
        FormLastSurvey m_frmLastSurvey;
        FormSNRPlot m_frmSNR;
        FormSurveyAcceptRejectInfinite m_frmSurveyAcceptRejectInf;
        FormChat m_frmChat;
        FormSurveyLog m_frmMSAEdit;
        FormSurveyLog m_frmMSACorrected;
        FormSurveyLog m_frmRawEdit;
        FormSurveyLog m_frmRawView;
        //FormECD m_frmECD;
        FormRealTimeDetectLog m_frmRealTimeDetectLog;

        // survey 
        CSurvey m_Survey;

        private CJobInfo m_JobInfo;

        // equivalent circulating density
        CEquivalentCirculatingDensity m_ECD; 

        CWidgetInfoLookupTable m_WidgetInfoLookupTbl;
        CSettingsLookupTable m_SettingsLookupTbl;

        // WITS
        CWITSLookupTable m_lookupWITS;
        //CWITSRecord m_WITSSurveyRec;   // surveys are sent as a single record     
        //CWITSRecord m_WITSSurveyVectorRec;   // vector surveys are sent as a single record 
        CWITSRecordTF m_WITSToolFace;  // mtf and gtf are sent based on tf flag
        CWITSRecordECD m_WITSECD;
        CWITSRecord[] m_arrWITSGeneric;  // remaining wits are sent as single channel records
        //CWITSRecord[] m_arrWITSFromRig;  

        // client-server for Display applications
        private Thread m_threadClientServer;
        private bool m_bUnload;
        private string m_sServerIPAddress;
        private string m_sMyIPAddress;

        private string m_sDBPath;

        private CServer m_Server;
        private CClient m_Client;
        private CServerMessage m_ServerMessage;
        private bool m_bDisplayServer; // "true" if it is installed or setup as the server; "false" otherwise
        private string m_sPortNumber;  // of server and client
        private delegate void ChangeJobInfoDelegate();
        private ChangeJobInfoDelegate changeJobInfoDelegate;

        private delegate void ReceiveServerMsgDelegate();
        private ReceiveServerMsgDelegate receiveServerMsgDelegate;

        private delegate void StartDetectLater();

        private delegate void MSAConnectedDelegate();
        private MSAConnectedDelegate MSADelegate;
        private bool m_bMSAConnected;

        private string m_sMSAURL;
        //private string m_sMSAAPIKey;

        private bool m_bUseAcceptRejectFeature;

        private delegate void dgUpdateNumClients();
        private dgUpdateNumClients UpdateNumClients;
        private int m_iConnectedClients = 0;           

        private string m_sIPAddressEM;
        private int m_iPortNumberEM;

        private string m_sIPAddressMP;
        private int m_iPortNumberMP;
        
        private CMSAHubClient m_MSAHub;

        // 560R application status
        private Thread m_thread560RStatus;
        private delegate void RemoteRx560RStatus();
        private RemoteRx560RStatus change560RStatusDelegate;
        private bool m_b560R;

        private void InitDetectObjects(CWidgetInfoLookupTable WidgetInfoLookupTbl, bool bValidLicense)
        {
            m_DetectEM = new CDetect(CCommonTypes.TELEMETRY_TYPE.TT_EM, ref m_dbConn);
            m_DetectMP = new CDetect(CCommonTypes.TELEMETRY_TYPE.TT_MP, ref m_dbConn);

            //2/15/22        
            m_DataAcqDetectEM = new DataAcqDetect(CCommonTypes.TELEMETRY_TYPE.TT_EM);
            m_DataAcqDetectMP = new DataAcqDetect(CCommonTypes.TELEMETRY_TYPE.TT_MP);
            //2/15/22

            m_DetectEM.SetLicense(bValidLicense);
            m_DetectEM.SetPlotFormEM(ref m_frmPlotSamplesEM, ref m_frmFFTEM, ref m_PumpPressure);
            m_DetectMP.SetLicense(bValidLicense);
            m_DetectMP.SetPlotForm(ref m_frmPlotSamples, ref m_frmFFT, ref m_PumpPressure);

            WidgetInfoLookupTbl.Load();
            string sIPAddressEM = m_sIPAddressEM = WidgetInfoLookupTbl.GetValue("FormIPAddress", "maskedTextBoxIPAddressEM");
            string sIPAddressMP = m_sIPAddressMP = WidgetInfoLookupTbl.GetValue("FormIPAddress", "maskedTextBoxIPAddressMP");
            int iPortNumberEM = m_iPortNumberEM = System.Convert.ToInt32(WidgetInfoLookupTbl.GetValue("FormIPAddress", "textBoxPortNumberEM"));
            int iPortNumberMP = m_iPortNumberMP = System.Convert.ToInt32(WidgetInfoLookupTbl.GetValue("FormIPAddress", "textBoxPortNumberMP"));

            m_DetectEM.SetupBackgroundWorker();
            m_DetectEM.SetConnectionInfo(sIPAddressEM, iPortNumberEM);
            m_DetectEM.StartDataReceiverInBackground(sIPAddressEM, iPortNumberEM);
           
            m_DetectMP.SetupBackgroundWorker();
            m_DetectMP.SetConnectionInfo(sIPAddressMP, iPortNumberMP);
            m_DetectMP.StartDataReceiverInBackground(sIPAddressMP, iPortNumberMP);

            //2/15/22

            m_DataAcqDetectEM.SetupBackgroundWorker();
            m_DataAcqDetectEM.SetConnectionInfo(sIPAddressEM, iPortNumberEM);
            m_DataAcqDetectEM.StartDataReceiverInBackground(sIPAddressEM, iPortNumberEM);

            m_DataAcqDetectMP.SetupBackgroundWorker();
            m_DataAcqDetectMP.SetConnectionInfo(sIPAddressMP, iPortNumberMP);
            m_DataAcqDetectMP.StartDataReceiverInBackground(sIPAddressMP, iPortNumberMP);

            //2/15/22

            SetListener();
        }

        public Main()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
                        
            m_dbConn = m_dbFactory.CreateConnection();

            m_SettingsLookupTbl = new CSettingsLookupTable();
            m_SettingsLookupTbl.Load();
            m_sDBPath = m_SettingsLookupTbl.GetValue("DATA_SOURCE");

            // one and only instance of the widget lookup table
            m_WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            m_WidgetInfoLookupTbl.Load();


            m_DPointTable = new CDPointLookupTable();
            string sTable = m_DPointTable.Load();

            m_dbConn.ConnectionString = m_sDBPath;
            m_dbConn.Open();

            // the dock panel that allows forms to be docked
            dockPanel = new DockPanel();
            dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            Controls.Add(this.dockPanel);
            var theme = new VS2015DarkTheme();
            this.dockPanel.Theme = theme;
            
            m_frmTF = new FormToolFace(ref m_WidgetInfoLookupTbl);
            m_frmQualifiers = new FormQualifiers(ref m_WidgetInfoLookupTbl, ref m_DPointTable);
            m_frmGamma = new FormGamma(ref m_WidgetInfoLookupTbl, ref m_DPointTable);
            m_frmINCWhileDrilling = new FormINCWhileDrilling(ref m_lookupWITS, ref m_dbConn, ref m_DPointTable);
            m_frmLastSurvey = new FormLastSurvey(ref m_WidgetInfoLookupTbl, ref m_DPointTable);
            m_frmPumpPowerTemp = new FormPumpPowerTemp(ref m_WidgetInfoLookupTbl, ref m_DPointTable);
            m_frmFFT = new FormFFT("Mud Pulse FFT");
            m_frmFFTEM = new FormFFTEM("EM FFT"); 
            m_frmRealTimeDetectLog = new FormRealTimeDetectLog();
            m_frmPlotSamples = new FormPlotSamples(ref m_DPointTable);
            m_frmPlotSamplesEM = new FormPlotSamplesEM(ref m_DPointTable);
            m_frmSNR = new FormSNRPlot(ref m_dbConn);

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            m_frmTF.Show(dockPanel);
            m_frmQualifiers.Show(dockPanel);
            m_frmGamma.Show(dockPanel);
            m_frmINCWhileDrilling.Show(dockPanel);
            m_frmLastSurvey.Show(dockPanel);
            m_frmPumpPowerTemp.Show(dockPanel);
            m_frmFFT.Show(dockPanel);
            m_frmFFTEM.Show(dockPanel);
            m_frmRealTimeDetectLog.Show(dockPanel);
            m_frmPlotSamples.Show(dockPanel);
            m_frmPlotSamplesEM.Show(dockPanel);
            m_frmSNR.Show(dockPanel);

            m_b560R = false;
        }

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            m_frmTF.DockPanel = null;
            m_frmQualifiers.DockPanel = null;
            m_frmGamma.DockPanel = null;
            m_frmINCWhileDrilling.DockPanel = null;
            m_frmLastSurvey.DockPanel = null;
            m_frmPumpPowerTemp.DockPanel = null;
            m_frmFFT.DockPanel = null;
            m_frmFFTEM.DockPanel = null;
            m_frmRealTimeDetectLog.DockPanel = null;
            m_frmPlotSamples.DockPanel = null;
            m_frmPlotSamplesEM.DockPanel = null;
            m_frmSNR.DockPanel = null;

            // Close all other document windows
            CloseAllDocuments();

            // IMPORTANT: dispose all float windows.
            foreach (var window in dockPanel.FloatWindows.ToList())
                window.Dispose();

            System.Diagnostics.Debug.Assert(dockPanel.Panes.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.Contents.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.FloatWindows.Count == 0);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(FormToolFace).ToString())
                return m_frmTF;
            else if (persistString == typeof(FormQualifiers).ToString())
                return m_frmQualifiers;
            else if (persistString == typeof(FormGamma).ToString())
                return m_frmGamma;
            else if (persistString == typeof(FormINCWhileDrilling).ToString())
                return m_frmINCWhileDrilling;
            else if (persistString == typeof(FormLastSurvey).ToString())
                return m_frmLastSurvey;
            else if (persistString == typeof(FormPumpPowerTemp).ToString())
                return m_frmPumpPowerTemp;
            else if (persistString == typeof(FormFFT).ToString())
                return m_frmFFT;
            else if (persistString == typeof(FormFFTEM).ToString())
                return m_frmFFTEM;
            else if (persistString == typeof(FormRealTimeDetectLog).ToString())
                return m_frmRealTimeDetectLog;
            else if (persistString == typeof(FormPlotSamples).ToString())
                return m_frmPlotSamples;
            else if (persistString == typeof(FormPlotSamplesEM).ToString())
                return m_frmPlotSamplesEM;
            else if (persistString == typeof(FormSNRPlot).ToString())
                return m_frmSNR;
            else
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(DummyDoc).ToString())
                    return null;

                DummyDoc dummyDoc = new DummyDoc();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];

                return dummyDoc;
            }
        }        

        private void Main_Load(object sender, EventArgs e)
        {            
            if (!m_DPointTable.IsLoaded())
            {
                Application.Exit();
                return;
            }

            CLicense license = new CLicense();
            license.Load();
            bool bValidLicense = license.IsValid();

            m_bUnload = false;                                  

            // add version number to title
            this.Text += "v" + Application.ProductVersion;
                                    
            m_PumpPressure = new CPumpPressure();

            m_PumpPressure.Init();  //4/22/22


            InitDetectObjects(m_WidgetInfoLookupTbl, bValidLicense);           

            changeDetectstatusDel += new ChangeDetectStatusDelegate(ChangeDetectStatus);

            m_DataLayer = new CDetectDataLayer();

            m_pressureTransducer = new CPressureTransducer(ref m_dbConn, ref m_WidgetInfoLookupTbl);
            m_pressureTransducer.Load();

            m_PumpPressure.SetListener(m_DataLayer);
            m_PumpPressure.SetPressureTransducer(ref m_pressureTransducer);


            m_lookupWITS = new CWITSLookupTable();
            m_lookupWITS.Load();

            m_Pason = new CPason(ref m_lookupWITS);
            changeWITSStatusDelegate += new ChangeWITSStatusDelegate(ChangeWITSStatus);                       

            string sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "numericUpDownCOMPort");
            m_Pason.SetPort(Convert.ToInt16(sVal));
            string sErrMsg = "";
            if (!m_Pason.Start(ref sErrMsg))  // do nothing
            {                
                statusStripMain.Items["toolStripStatusLabelWITS"].BackColor = Color.Red;
                statusStripMain.Items["toolStripStatusLabelWITS"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelWITS"].Text = "WITS OFF";
            }
            else
            {
                statusStripMain.Items["toolStripStatusLabelWITS"].BackColor = Color.FromArgb(0, 192, 0);
                statusStripMain.Items["toolStripStatusLabelWITS"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelWITS"].Text = "WITS ON";
            }

            statusStripMain.Items["toolStripStatusLabelDetect"].BackColor = Color.Red;
            statusStripMain.Items["toolStripStatusLabelMP"].BackColor = Color.Red;
            statusStripMain.Items["toolStripStatusLabelDetect"].Text = "EM OFF";
            statusStripMain.Items["toolStripStatusLabelMP"].Text = "MP OFF";

            m_OutgoingWITS = new CPason(ref m_lookupWITS);
            sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "numericUpDownCOMPortOut1");
            m_OutgoingWITS.SetPort(Convert.ToInt16(sVal));
            m_OutgoingWITS.Start(ref sErrMsg);
                                                                                        
            m_frmINCWhileDrilling.Transmit += new FormINCWhileDrilling.EventHandler(SendOutgoingWITS);


            m_frmMSAEdit = new FormSurveyLog(ref m_dbConn, ref m_DPointTable, ref m_lookupWITS, ref m_WidgetInfoLookupTbl, true);           
            m_frmMSAEdit.SetTitle("Edit MSA Survey");
            m_frmMSAEdit.Hide();
                        
            m_frmSurveyAcceptRejectInf = new FormSurveyAcceptRejectInfinite(ref m_dbConn, ref m_lookupWITS, ref m_frmMSAEdit);
            m_frmSurveyAcceptRejectInf.Transmit += new FormSurveyAcceptRejectInfinite.EventHandler(SendOutgoingWITS);
            m_frmSurveyAcceptRejectInf.UpdateRaw += new FormSurveyAcceptRejectInfinite.EventHandlerUpdateRaw(UpdateRawSurveyLists);
            m_frmSurveyAcceptRejectInf.Hide();
                                   
            m_frmMSAEdit.DeleteSurvey += new FormSurveyLog.EventHandler(m_frmSurveyAcceptRejectInf.DeleteSurvey);
            m_frmMSAEdit.UpdateSurvey += new FormSurveyLog.EventUpdate(m_frmSurveyAcceptRejectInf.UpdateSurvey);
            m_frmMSAEdit.AddSurvey += new FormSurveyLog.EventAdd(m_frmSurveyAcceptRejectInf.AddSurvey);
            m_frmMSAEdit.TransmitWITS += new FormSurveyLog.EventHandlerWITS(SendOutgoingWITS);

            // edit survey log window
            m_frmRawEdit = new FormSurveyLog(ref m_dbConn, ref m_DPointTable, ref m_lookupWITS, ref m_WidgetInfoLookupTbl, false, true);
            m_frmRawEdit.TransmitWITS += new FormSurveyLog.EventHandlerWITS(SendOutgoingWITS);
            m_frmRawEdit.SetTitle("Edit Raw Surveys");
            m_frmRawEdit.Hide();

            m_frmRawView = new FormSurveyLog(ref m_dbConn, ref m_DPointTable, ref m_lookupWITS, ref m_WidgetInfoLookupTbl);
            m_frmRawView.TransmitWITS += new FormSurveyLog.EventHandlerWITS(SendOutgoingWITS);           
            m_frmRawView.SetTitle("View Raw Surveys");
            m_frmRawView.Hide();
            
            m_frmChat = new FormChat();
            m_frmChat.Hide();            
            

            // **************************
            // toolface window
            // **************************
            m_frmTF.SetLicense(bValidLicense);                                        
                                                                       
            m_frmSurveyAcceptRejectInf.SetBounds(0, statusStripMain.Height * 3, m_frmSurveyAcceptRejectInf.Width + SystemInformation.BorderSize.Width * 13, m_frmSurveyAcceptRejectInf.Height + statusStripMain.Height);
            
            m_JobInfo = new CJobInfo(ref m_dbConn, ref m_WidgetInfoLookupTbl);
            m_JobInfo.Load();
                        
            m_frmSurveyAcceptRejectInf.SetBHA(m_JobInfo.GetBHA().ToString());                                    
            
            // set the mdi background to color matching widgets
            //foreach (Control control in this.Controls) { if (control is MdiClient) { control.BackColor = Color.FromArgb(0x3F, 0x3F, 0x3F); break; } }
                     
            m_DataLayer.Init();
            m_DataLayer.SetListener(m_DetectEM, m_Pason);
            m_DataLayer.SetListener(m_DetectMP, m_Pason);

            m_DataLayer.SetAcqListener(m_DataAcqDetectEM, m_Pason); //2/23/22
            m_DataLayer.SetAcqListener(m_DataAcqDetectMP, m_Pason); //2/23/22

            m_frmINCWhileDrilling.SetListener(m_DataLayer);
            m_frmTF.SetListener(m_DataLayer);
            m_frmQualifiers.SetListener(m_DataLayer);
            
            //m_frmPlotSamples.SetListener(m_DataLayer);
            m_frmPlotSamples.SetPressureTransducer(ref m_pressureTransducer);

            //m_frmPlotSamplesEM.SetListener(m_DataLayer);
            m_frmPlotSamplesEM.SetPressureTransducer(ref m_pressureTransducer);

            m_frmFFT.SetListener(m_DataLayer);
            m_frmFFTEM.SetListener(m_DataLayer);

            m_frmGamma.SetListener(m_DataLayer);            

            m_frmPumpPowerTemp.SetListener(m_DataLayer);
            m_frmPumpPowerTemp.SetListenerForPumpPressure(m_PumpPressure);
            m_frmPumpPowerTemp.SetUnitsFromTransducer(m_pressureTransducer.GetPressureUnit());
            
            m_frmRealTimeDetectLog.SetListener(m_DataLayer);

            m_frmLastSurvey.SetListener(m_DataLayer);
            

            FormUnitSelection frmTmp = new FormUnitSelection();
            frmTmp.Hide();  // called for side-effect
            string[] sArrRadioButtonName = frmTmp.GetOptionWidgets();
            m_unitSelection = new CUnitSelection("FormUnitSelection", sArrRadioButtonName);
            m_unitSelection.Load(ref m_WidgetInfoLookupTbl);
            m_iUnitSelection = m_unitSelection.GetUnitSet();
            
            m_frmPlotSamples.SetUnitSet(m_iUnitSelection);
            m_frmPlotSamplesEM.SetUnitSet(m_iUnitSelection);
            m_frmGamma.SetUnitSet(m_iUnitSelection);

            // ****************************************************************
            // WITS stuff            
            // ****************************************************************
            SetWITSListener();                      
            InitWITSObjects(m_WidgetInfoLookupTbl, true);


            m_Survey = new CSurvey(ref m_dbConn, ref m_DPointTable, m_unitSelection);
            m_Survey.SetListener(m_DataLayer);
            m_Survey.DisplaySurvey += new CSurvey.EventSurveyDone(ShowSurvey);

            // ****************************************************************
            // equivalent circulating density 
            // ****************************************************************
            m_ECD = new CEquivalentCirculatingDensity(ref m_dbConn, ref m_lookupWITS, ref m_DPointTable, m_unitSelection);
            
            string sTVD = m_WidgetInfoLookupTbl.GetValue("FormECD", "textBoxTVD");
            string sMudDensity = m_WidgetInfoLookupTbl.GetValue("FormECD", "textBoxMudDensity");
            m_ECD.SetParameters(System.Convert.ToSingle(sTVD), System.Convert.ToSingle(sMudDensity));
            
            m_DetectEM.SetWITSListener(m_DataLayer);
            m_DetectMP.SetWITSListener(m_DataLayer);
            m_DetectEM.SetECD(ref m_ECD);
            m_DetectMP.SetECD(ref m_ECD);

            // ****************************************************************
            // MSA stuff            
            // ****************************************************************
            m_sMSAURL = m_WidgetInfoLookupTbl.GetValue("FormMSASettings", "textBoxURL");
            m_bUseAcceptRejectFeature = System.Convert.ToBoolean(m_WidgetInfoLookupTbl.GetValue("FormMSASettings", "checkBoxUseAcceptRejectFeature"));
            //m_sMSAAPIKey = WidgetInfoLookupTbl.GetValue("FormMSASettings", "textBoxAPIKey");
            m_MSAHub = new CMSAHubClient(ref m_lookupWITS, ref m_dbConn, m_SettingsLookupTbl.GetJobPath());
            m_MSAHub.Transmit += new CMSAHubClient.EventHandler(SendOutgoingWITS);
            m_MSAHub.CorrectToolface += new CMSAHubClient.CorrectedEventHandler(m_frmTF.Correct);
            m_MSAHub.CorrectLastSurvey += new CMSAHubClient.CorrectedLastSurveyEventHandler(this.m_frmLastSurvey.CorrectLastSurvey);
            m_MSAHub.SetInfo(m_sMSAURL, m_JobInfo.GetAPIKey(), m_SettingsLookupTbl.GetJobPath());
            m_MSAHub.Register();
            
            m_frmChat.SetInfo(m_sMSAURL, m_JobInfo.GetAPIKey());
            this.WindowState = FormWindowState.Minimized;
            if (m_JobInfo.GetAPIKey().Trim().Length == 0)
                m_frmChat.Hide();
            else
                m_frmChat.Show();            
            
            m_frmSurveyAcceptRejectInf.SetInfo(m_sMSAURL, m_JobInfo.GetAPIKey());

            m_frmMSACorrected = new FormSurveyLog(ref m_dbConn, ref m_DPointTable, ref m_lookupWITS, ref m_WidgetInfoLookupTbl);
            m_frmMSACorrected.TransmitWITS += new FormSurveyLog.EventHandlerWITS(SendOutgoingWITS);
            m_frmMSACorrected.SetMSAHub(ref m_MSAHub);
            m_frmMSACorrected.SetTitle("Corrected Surveys");
            m_frmMSACorrected.Hide();

            m_bMSAConnected = false;
                       
            m_MSAHub.HubConnected += new CMSAHubClient.HubConnectedEventHandler(ReceiveMSAConnectionStatus);
            ReceiveMSAConnection();  // initialize call
                                                          
            MSADelegate = new MSAConnectedDelegate(ReceiveMSAConnection);

            // ****************************************************************
            // create both server and client objects so user can switch
            // ****************************************************************                                                
            string sServerStatus = m_WidgetInfoLookupTbl.GetValue("FormServerSetup", "checkBoxServer");
            if (sServerStatus == "True")
                m_bDisplayServer = true;
            else
                m_bDisplayServer = false;

            this.Text += " " + (m_bDisplayServer ? SERVER_MODE: CLIENT_MODE);            
            UpdateTitleBar(m_JobInfo);
            UpdateNumberOfClients();

            m_sPortNumber = m_WidgetInfoLookupTbl.GetValue("FormServerSetup", "textBoxPortNumber");
            m_sServerIPAddress = m_WidgetInfoLookupTbl.GetValue("FormServerSetup", "maskedTextBoxServerIPAddress");

            CLocalAreaNetwork localNetwork = new CLocalAreaNetwork();
            m_sMyIPAddress = localNetwork.GetMyIPAddress();

            m_ServerMessage = new CServerMessage(ref m_lookupWITS);

            m_Server = new CServer();
            m_Server.SetPort(m_sPortNumber);
            m_Server.OnClientConnect += new CServer.dgEventClientConnect(ClientConnected);
            UpdateNumClients += new dgUpdateNumClients(UpdateNumberOfClients);

            m_Client = new CClient();
            m_Client.OnReceivePacket += new CClient.dgEventRaiser(PacketReceived);
            m_Client.OnServerConnect += new CClient.dgEventServerConnect(ClientConnected);

            m_threadClientServer = new Thread(ClientServerWorker);
            m_threadClientServer.Start(this);
            // ****************************************************************                          
            
            changeJobInfoDelegate = new ChangeJobInfoDelegate(ChangeJobInfo);
            receiveServerMsgDelegate = new ReceiveServerMsgDelegate(ReceiveServerMessage);            

            // called after all the windows have been created
            UpdateWindowsUnits(m_iUnitSelection);

            // dock panel loading
            string configFile = Path.Combine(CCommonTypes.DATA_FOLDER, "DockPanel.temp.config");
            dockPanel.SaveAsXml(configFile);
            CloseAllContents();

            string configFile2 = Path.Combine(CCommonTypes.DATA_FOLDER, "DockPanel.config");
            if (File.Exists(configFile2))
                dockPanel.LoadFromXml(configFile2, m_deserializeDockContent);


            // 560R connection stuff
            m_thread560RStatus = new Thread(Check560RStatus);
            m_thread560RStatus.Start();

            change560RStatusDelegate += new RemoteRx560RStatus(Change560RStatus);

            this.WindowState = FormWindowState.Maximized;
        }

        private void Check560RStatus()
        {
            while (true)
            {
                if (m_bUnload)
                    break;                

                Process[] process = Process.GetProcessesByName("apsremotereceiver");
                if (process.Length > 0)
                    m_b560R = true;
                else 
                    m_b560R = false;

                if (IsHandleCreated)
                    statusStripMain.BeginInvoke(change560RStatusDelegate);

                Thread.Sleep(2000);
            }
        }

        private void Change560RStatus()
        {
            if (statusStripMain != null)
            {
                if (m_b560R)
                {
                    statusStripMain.Items["toolStripStatus560R"].BackColor = Color.FromArgb(0, 192, 0);
                    statusStripMain.Items["toolStripStatus560R"].ForeColor = Color.Black;
                    statusStripMain.Items["toolStripStatus560R"].Text = "560R APP Running";
                }
                else
                {
                    statusStripMain.Items["toolStripStatus560R"].BackColor = Color.Red;
                    statusStripMain.Items["toolStripStatus560R"].ForeColor = Color.Black;
                    statusStripMain.Items["toolStripStatus560R"].Text = "Launch 560R APP";
                }
            }
            
                
        }

        private void CloseAllDocuments()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    // IMPORANT: dispose all panes.
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }
        }
               
        private void ReceiveMSAConnection()
        {            
            if (m_bMSAConnected)
            {
                statusStripMain.Items["toolStripStatusMSA"].BackColor = Color.FromArgb(0, 192, 0);
                statusStripMain.Items["toolStripStatusMSA"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusMSA"].Text = "MSA ON";
            }                
            else
            {
                statusStripMain.Items["toolStripStatusMSA"].BackColor = Color.Red;
                statusStripMain.Items["toolStripStatusMSA"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusMSA"].Text = "MSA OFF";
                m_frmSurveyAcceptRejectInf.ClearMSAJobID();
            }
                
        }

        private string GetExcludedWITSChannels()
        {
            string sRetVal = "";
            // exclude the special WITS channels (survey, vector survey, and tf) from the generic records
            int[] iArrExclude = {(int)Command.COMMAND_RESP_TF, (int)Command.COMMAND_RESP_INCLINATION, (int)Command.COMMAND_RESP_AZIMUTH,
                                 (int)Command.COMMAND_CS_INC, (int)Command.COMMAND_CS_AZM,
                                 (int)Command.COMMAND_RESP_GT, (int)Command.COMMAND_RESP_BT, (int)Command.COMMAND_RESP_DIPANGLE,                                 
                                 (int)Command.COMMAND_RESP_GX, (int)Command.COMMAND_RESP_GY, (int)Command.COMMAND_RESP_GZ,
                                 (int)Command.COMMAND_RESP_BX, (int)Command.COMMAND_RESP_BY, (int)Command.COMMAND_RESP_BZ,
                                 (int)Command.COMMAND_ECD_TVD, (int)Command.COMMAND_HYDRO_STATIC_PRESSURE};
            
            for (int i = 0; i < iArrExclude.Length; i++)
            {
                if (i > 0)  // separate by comma
                    sRetVal += ",'" + iArrExclude[i].ToString() + "'";
                else
                    sRetVal += "'" + iArrExclude[i].ToString() + "'";
            }
            sRetVal = "(" + sRetVal + ")";
            return sRetVal;
        }

        private void ClearWITSObjects()
        {
            m_WITSToolFace.ClearChannels();
            m_WITSECD.ClearChannels();
            for (int i = 0; i < m_arrWITSGeneric.Length; i++)
            {
                m_arrWITSGeneric[i].ClearChannels();
            }
        }

        private void InitWITSObjects(CWidgetInfoLookupTable WidgetInfoLookupTbl_, bool bAttachTransmit)
        {
            // get WITS filters            
            string sVal = WidgetInfoLookupTbl_.GetValue("FormConfigureWITS", "checkBoxFilterMudPulse");
            bool bVal = System.Convert.ToBoolean(sVal);
            bool bSendMudPulse = bVal;

            sVal = WidgetInfoLookupTbl_.GetValue("FormConfigureWITS", "checkBoxFilterEM");
            bVal = System.Convert.ToBoolean(sVal);
            bool bSendEM = bVal;
                                
            // **************************************
            // toolface
            // **************************************                       
            if (bAttachTransmit)
            {
                m_WITSToolFace = new CWITSRecordTF();
                m_WITSToolFace.SetListener(m_DataLayer);                
                m_WITSToolFace.Transmit += new CWITSRecordTF.EventHandler(SendOutgoingWITS);
            }
            m_WITSToolFace.SetWITSLookUpTable(ref m_lookupWITS);
            m_WITSToolFace.AddChannel(m_lookupWITS.Find(CCommonTypes.GTF)); // gravity. order matters
            m_WITSToolFace.AddChannel(m_lookupWITS.Find(CCommonTypes.MTF)); // magnetic. order matters            
            m_WITSToolFace.SetFilter(bSendMudPulse, bSendEM);
                        
            if (bAttachTransmit)
            {
                m_WITSECD = new CWITSRecordECD();
                m_WITSECD.SetListener(m_DataLayer);                
                m_WITSECD.Transmit += new CWITSRecordECD.EventHandler(SendOutgoingWITS);
            }
            m_WITSECD.SetWITSLookUpTable(ref m_lookupWITS);
            m_WITSECD.AddChannel(m_lookupWITS.Find2((int)Command.COMMAND_ECD_TVD));
            m_WITSECD.AddChannel(m_lookupWITS.Find2((int)Command.COMMAND_HYDRO_STATIC_PRESSURE));
            m_WITSECD.SetFilter(bSendMudPulse, bSendEM);



            string sExclude = GetExcludedWITSChannels();
            List<CWITSLookupTable.WITSChannel> lstWITSGeneric = m_lookupWITS.GetFromSource("Detect", sExclude);
            if (lstWITSGeneric.Count > 0)
            {
                if (bAttachTransmit)
                    m_arrWITSGeneric = new CWITSRecord[lstWITSGeneric.Count];

                for (int i = 0; i < lstWITSGeneric.Count; i++)
                {
                    if (i < m_arrWITSGeneric.Length)  // array bounds check
                    {
                        if (bAttachTransmit)
                        {
                            m_arrWITSGeneric[i] = new CWITSRecord();
                            m_arrWITSGeneric[i].SetListener(m_DataLayer);
                            m_arrWITSGeneric[i].Transmit += new CWITSRecord.EventHandler(SendOutgoingWITS);
                        }
                        m_arrWITSGeneric[i].SetWITSLookUpTable(ref m_lookupWITS);
                        m_arrWITSGeneric[i].AddChannel(lstWITSGeneric[i].sID);
                        m_arrWITSGeneric[i].SetFilter(bSendMudPulse, bSendEM);
                    }                    
                }
            }                 
        }

        void ClientServerWorker(object obj)
        {
            Main param = (Main)obj;
            bool bFirstTime = true;

            while (true)
            {
                try
                {
                    if (param.m_bUnload)                                            
                        break;

                    Thread.Sleep(2000);
                    if (bFirstTime)
                    {
                        bFirstTime = false;                                                
                    }

                    if (m_bDisplayServer)  // scan the network for a possible server
                    {
                        if (!m_Server.IsRunning())
                            m_Server.Start();
                    }
                    else
                    { 
                        if (m_sServerIPAddress != CCommonTypes.NO_IP_ADDRESS)
                        {
                            if (!m_Client.IsConnected())
                            {
                                if (param.m_bUnload)
                                {
                                    m_Client.Stop();
                                    break;
                                }
                                    
                                m_Client.ConnectToServer(m_sServerIPAddress, System.Convert.ToInt32(m_sPortNumber));
                                while (!m_Client.IsConnected())
                                {
                                    if (param.m_bUnload)
                                        break;
                                    if (m_Client.HasChangedConnection())
                                        break;
                                    Thread.Sleep(500);
                                }
                            }                                                           
                        }                                               
                    }
                    
                }
                catch (ThreadAbortException abortException)
                {
                    System.Diagnostics.Debug.WriteLine((string)abortException.ExceptionState);
                }
            }
            
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_bUnload = true;

            m_WidgetInfoLookupTbl.Save();

            if (m_DetectEM != null)  
            {
                m_DetectEM.Unload();
                m_DetectEM.EndDataReceiverInBackground();
            }
                
            if (m_DetectMP != null)
            {
                m_DetectMP.Unload();
                m_DetectMP.EndDataReceiverInBackground();
            }

            //2/15/22
            if (m_DataAcqDetectEM != null)
            {
                m_DataAcqDetectEM.Unload();
                m_DataAcqDetectEM.EndDataReceiverInBackground();
            }

            if (m_DataAcqDetectMP != null)
            {
                m_DataAcqDetectMP.Unload();
                m_DetectMP.EndDataReceiverInBackground();
            }
            //2/15/22

            if (m_DPointTable != null)
                m_DPointTable.Save();

            if (m_DataLayer != null)
                m_DataLayer.Quit();
            
            m_Server.Unload();
            m_Client.Stop();

            m_MSAHub.Unload();
            m_frmChat.Unload();
            m_frmSNR.Unload();
            m_frmSurveyAcceptRejectInf.Unload();

            m_frmLastSurvey.Unload();
            m_frmTF.Unload();
            m_frmPumpPowerTemp.Unload();

            m_Pason.SetUnload();
            m_Pason.Stop();
            m_OutgoingWITS.SetUnload();
            m_OutgoingWITS.Stop();

            m_dbConn.Close();
        }

        private void SetListener()
        {
           m_DetectEM.DetectConnected += new CDetect.DetectConnectedEventHandler(ConnectionStatusEM);
           m_DetectMP.DetectConnected += new CDetect.DetectConnectedEventHandler(ConnectionStatusMP);

            //2/15/22
            m_DataAcqDetectEM.DetectConnected += new DataAcqDetect.DetectConnectedEventHandler(ConnectionStatusEM);
            m_DataAcqDetectMP.DetectConnected += new DataAcqDetect.DetectConnectedEventHandler(ConnectionStatusMP);
            //2/15/22
        }
        
        private void ConnectionStatusEM(object sender, CEventDetect e)
        {
            m_iDetectStatus = e.m_iConnected;
            if (statusStripMain != null)
            {
                try
                {
                    if (IsHandleCreated)
                        statusStripMain.BeginInvoke(changeDetectstatusDel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }                
        }

        private void ConnectionStatusMP(object sender, CEventDetect e)
        {
            //Console.WriteLine("ConnectionStatusMP");
            m_iDetectStatusMP = e.m_iConnected;
            if (statusStripMain != null)
            {
                try
                {
                    if (IsHandleCreated)
                        statusStripMain.BeginInvoke(changeDetectstatusDel);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }                
        }

        private void SetWITSListener()
        {
            m_Pason.PasonConnected += new CPason.ConnectedEventHandler(WITSStatus);
        }

        private void SendOutgoingWITS(object sender, CEventSendWITSData e)
        {            
            m_Pason.QueueOutgoingPacket(e.m_sData);
            m_OutgoingWITS.QueueOutgoingPacket(e.m_sData);
        }

        private string GatherWITS(CEventSurvey recSvy_)
        {
            CWITSRecordSurvey rec = new CWITSRecordSurvey();
            string sIncWITSID, sAzmWITSID, sDepthWITSID, sGTotWITSID, sMTotWITSID, sDipWITSID;

            //************************************
            // survey channels
            //************************************
            sDepthWITSID = m_lookupWITS.Find2(10708);
            sIncWITSID = m_lookupWITS.Find2((int)Command.COMMAND_RESP_INCLINATION);
            sAzmWITSID = m_lookupWITS.Find2((int)Command.COMMAND_RESP_AZIMUTH);

            rec.AddChannel(sDepthWITSID);
            rec.AddChannel(sIncWITSID);
            rec.AddChannel(sAzmWITSID);

            rec.SetValue(sDepthWITSID, (float)recSvy_.rec.fSurveyDepth);
            rec.SetValue(sIncWITSID, (float)recSvy_.rec.fInclination);
            rec.SetValue(sAzmWITSID, (float)recSvy_.rec.fAzimuth);

            //************************************
            // append qualifiers if they exist
            //************************************
            if (recSvy_.rec.fGTotal > CCommonTypes.BAD_VALUE)
            {
                sGTotWITSID = m_lookupWITS.Find2((int)Command.COMMAND_RESP_GT);
                sMTotWITSID = m_lookupWITS.Find2((int)Command.COMMAND_RESP_BT);
                sDipWITSID = m_lookupWITS.Find2((int)Command.COMMAND_RESP_DIPANGLE);

                rec.AddChannel(sMTotWITSID);
                rec.AddChannel(sGTotWITSID);
                rec.AddChannel(sDipWITSID);

                rec.SetValue(sMTotWITSID, (float)recSvy_.rec.fMTotal);
                rec.SetValue(sGTotWITSID, (float)recSvy_.rec.fGTotal);
                rec.SetValue(sDipWITSID, (float)recSvy_.rec.fDipAngle);
            }

            string sRetVal = rec.GatherData();
            sRetVal = "&&\r\n" + sRetVal + "!!\r\n";  // prepend and append wits flags
            return sRetVal;
        }

        private void ShowSurvey(object sender, CEventSurvey e)
        {   
            if (m_JobInfo.GetSurveyManagementMode() != CJobInfo.SURVEY_MANAGEMENT_MODE.FULL_AUTOMATION)
                m_frmSurveyAcceptRejectInf.Add(e);

            if (!m_frmSurveyAcceptRejectInf.Visible)
            {
                if ((m_bUseAcceptRejectFeature || m_bDisplayServer) &&
                    m_JobInfo.GetSurveyManagementMode() != CJobInfo.SURVEY_MANAGEMENT_MODE.FULL_AUTOMATION)
                    m_frmSurveyAcceptRejectInf.Show();               
            }

            if (m_JobInfo.GetSurveyManagementMode() == CJobInfo.SURVEY_MANAGEMENT_MODE.FULL_AUTOMATION)
            {
                // send WITS without confirmation                  
                CEventSendWITSData eventWITS = new CEventSendWITSData();
                eventWITS.m_sData = GatherWITS(e);
                SendOutgoingWITS(this, eventWITS);
            }

            // update the survey log list
            m_frmRawView.UpdateList(e);
            m_frmRawEdit.UpdateList(e);
        }

        private void ShowECD(object sender, CEventECD e)
        {
            //m_frmECD.Set(e.fAnnularPressure);
            //if (!m_frmECD.Visible)
            //    m_frmECD.Show();
        }

        private void WITSStatus(object sender, CEventWITS e)
        {
            m_bWITSStatus = e.m_bConnected;
            if (statusStripMain != null)
            {
                try
                {
                    statusStripMain.Invoke(changeWITSStatusDelegate);
                }
                catch 
                { 
                }
            }
                
        }

        private void ChangeWITSStatus()
        {
            if (m_bWITSStatus)
            {
                statusStripMain.Items["toolStripStatusLabelWITS"].BackColor = Color.FromArgb(0, 192, 0);
                statusStripMain.Items["toolStripStatusLabelWITS"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelWITS"].Text = "WITS ON";
            }
            else
            {
                statusStripMain.Items["toolStripStatusLabelWITS"].BackColor = Color.Red;
                statusStripMain.Items["toolStripStatusLabelWITS"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelWITS"].Text = "WITS OFF";
            }
        }

        private void ChangeDetectStatus()
        {
            if (m_iDetectStatus == CEventDetect.CONNECTION.OPEN)
            {
                statusStripMain.Items["toolStripStatusLabelDetect"].BackColor = Color.FromArgb(0, 192, 0);
                statusStripMain.Items["toolStripStatusLabelDetect"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelDetect"].Text = "EM ON";
            }
            else if (m_iDetectStatus == CEventDetect.CONNECTION.CLOSED)
            {
                statusStripMain.Items["toolStripStatusLabelDetect"].BackColor = Color.Red;
                statusStripMain.Items["toolStripStatusLabelDetect"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelDetect"].Text = "EM OFF";
            }
            else
            {
                statusStripMain.Items["toolStripStatusLabelDetect"].BackColor = Color.Yellow;
                statusStripMain.Items["toolStripStatusLabelDetect"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelDetect"].Text = "EM TRY";
            }

            if (m_iDetectStatusMP == CEventDetect.CONNECTION.OPEN)
            {
                statusStripMain.Items["toolStripStatusLabelMP"].BackColor = Color.FromArgb(0, 192, 0);
                statusStripMain.Items["toolStripStatusLabelMP"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelMP"].Text = "MP ON";
            }
            else if (m_iDetectStatusMP == CEventDetect.CONNECTION.CLOSED)
            {
                statusStripMain.Items["toolStripStatusLabelMP"].BackColor = Color.Red;
                statusStripMain.Items["toolStripStatusLabelMP"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelMP"].Text = "MP OFF";
            }
            else
            {
                statusStripMain.Items["toolStripStatusLabelMP"].BackColor = Color.Yellow;
                statusStripMain.Items["toolStripStatusLabelMP"].ForeColor = Color.Black;
                statusStripMain.Items["toolStripStatusLabelMP"].Text = "MP TRY";
            }
        }

        private void ChangeJobInfo()
        {
            //CJobInfo jobInfo = new CJobInfo(ref m_dbConn, ref m_WidgetInfoLookupTbl);
            //jobInfo.Load();
            
            UpdateTitleBar(m_JobInfo);
            m_JobInfo.Update(m_JobInfo.GetJobID(), m_JobInfo.GetRig(), m_JobInfo.GetBHA(), m_JobInfo.GetMSA(), m_JobInfo.GetAPIKey());
            //m_frmSurveyAcceptReject.SetBHA(m_sBHA);
            m_frmSurveyAcceptRejectInf.SetBHA(m_JobInfo.GetBHA().ToString());
        }

        private void UpdateTitleBar(CJobInfo jobInfo_)
        {
            int iLeftBracketPosition = this.Text.IndexOf("[");            
            if (iLeftBracketPosition > -1)
            {           
                this.Text = this.Text.Substring(0, iLeftBracketPosition);
            }
            
            this.Text += "   [Job: " + jobInfo_.GetJobID() + "   Rig: " + jobInfo_.GetRig() + "   BHA: " + jobInfo_.GetBHA().ToString() + "]";
        }

        private void ReceiveServerMessage()  // visually cue the user with a yellow highlighted envelope
        {
            statusStripMain.Items["toolStripStatusLabelServer"].BackColor = Color.Yellow;
            statusStripMain.Items["toolStripStatusLabelServer"].ForeColor = Color.Black;
            statusStripMain.Items["toolStripStatusLabelServer"].Image = Resources.envelope;
        }

        //private void GetJobInfo()
        //{
        //    m_frmPumpPowerTemp.GetInfo(out m_sJobID, out m_sRig);
        //    //m_DisplayAsyncServer.SetPort(m_sJobID, m_sRig);
        //}

        private void ReceiveMSAConnectionStatus(object sender, bool b)
        {
            m_bMSAConnected = b;
            if (statusStripMain != null)
            {
                try
                {
                    if (IsHandleCreated)
                        statusStripMain.BeginInvoke(MSADelegate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }           
        }

        private void legendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLegend frm = new FormLegend();
            frm.ShowDialog();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLogDataLayer log = new CLogDataLayer(ref m_dbConn);
            CDPointLookupTable.DPointInfo infoDepth = m_DPointTable.Find("Bit Depth");
            DataTable tbl = log.Get(infoDepth.sUnits);
            FormLog frmLog = new FormLog();            
            frmLog.SetData(tbl);            
            frmLog.ShowDialog();
        }

        private void lASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLASExport frm = new FormLASExport(ref m_dbConn, m_MSAHub, ref m_WidgetInfoLookupTbl);
            frm.SetUnitSet(m_iUnitSelection);
            frm.SetAPIKey(m_JobInfo.GetAPIKey());
            frm.ShowDialog();
        }        

        private void iPAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIPAddress frm = new FormIPAddress();
            frm.SetWidgetInfoLookup(ref m_WidgetInfoLookupTbl);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ReconnectToDetect();
            }
        }

        private void ReconnectToDetect()
        {
            // kill existing connections
            //2/23/22m_DataLayer.RemoveListener();
            m_DataLayer.RemoveAcqListener(); //2/23/22

            //2/15/22
            if (m_DataAcqDetectEM != null)
            {
                m_DataAcqDetectEM.Unload();
                m_DataAcqDetectEM.EndDataReceiverInBackground();
            }

            if (m_DataAcqDetectMP != null)
            {
                m_DataAcqDetectMP.Unload();
                m_DataAcqDetectMP.EndDataReceiverInBackground();
            }
            //2/15/22

            // initialize new connections
            CLicense license = new CLicense();
            license.Load();
            bool bValidLicense = license.IsValid();

            m_WidgetInfoLookupTbl.Load();

            InitDetectObjects(m_WidgetInfoLookupTbl, bValidLicense);

           m_DataLayer.SetListener(m_DetectEM, m_Pason);
           m_DataLayer.SetListener(m_DetectMP, m_Pason);
           
           m_DetectEM.SetECD(ref m_ECD);
           m_DetectMP.SetECD(ref m_ECD);

            //2/23/22
            m_DataLayer.SetAcqListener(m_DataAcqDetectEM, m_Pason);
            m_DataLayer.SetAcqListener(m_DataAcqDetectMP, m_Pason);
            
            m_DataAcqDetectEM.SetECD(ref m_ECD);
            m_DataAcqDetectMP.SetECD(ref m_ECD);
            //2/23/22
        }

        private void UpdateWindowsUnits(CCommonTypes.UNIT_SET us_)
        {
            m_ECD.SetUnitSet(us_);
            m_DetectEM.SetUnitSet(us_);
            m_DetectMP.SetUnitSet(us_);
            m_frmPumpPowerTemp.SetUnitSet(us_);
            m_frmGamma.SetUnitSet(us_);
            m_frmQualifiers.SetUnitSet(us_);            
            m_PumpPressure.SetUnitSet(us_);
            //m_frmSurveyAcceptReject.SetUnitSet(us_);
            m_frmSurveyAcceptRejectInf.SetUnitSet(us_);
        }

        private void unitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUnitSelection frm = new FormUnitSelection();   
            frm.SetServerMode(m_bDisplayServer);
            frm.SetDpointTable(ref m_DPointTable);
            frm.SetUnitSelection(ref m_unitSelection);
            CCommonTypes.UNIT_SET iUnitsOld = m_iUnitSelection;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_DPointTable.Save();               

                // save form settings
                List<FormUnitSelection.UNIT_REC> lstUnitOpt = frm.GetUnitOption();
                for (int i = 0; i < lstUnitOpt.Count; i++)
                    m_unitSelection.Set(lstUnitOpt[i].sWidgetName, lstUnitOpt[i].sWidgetValue);

                bool[] bArrUnitGroup = frm.GetUnitGroup();
                if (frm.GetSelectedUnitSet() == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)                
                    m_unitSelection.SetUnitGroups(bArrUnitGroup);                
                                
                CCommonTypes.UNIT_SET iUnitsNew = m_iUnitSelection = frm.GetSelectedUnitSet();
                UpdateWindowsUnits(iUnitsNew);
                m_unitSelection.SetUnitSet(iUnitsNew);
                m_unitSelection.Save(ref m_WidgetInfoLookupTbl);

                m_Survey.SetUnit(m_unitSelection);
                m_frmPlotSamples.SetUnitSet(m_unitSelection.GetUnitSet());
                m_frmPlotSamplesEM.SetUnitSet(m_unitSelection.GetUnitSet());
                //m_frmECD.SetUnit(m_unitSelection);

                // send changes to clients
                if (m_bDisplayServer && m_iConnectedClients > 0)
                {
                    if (iUnitsNew == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC && iUnitsNew != iUnitsOld)
                    {
                        if (m_ServerMessage.PromptToSend("Unit of Measurement Changes"))
                        {
                            string s = CCommonTypes.PACKET_ID + "=" + ((int)CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_METRIC);
                            m_Server.Send(s);
                        }
                    }
                    else if (iUnitsNew == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL && iUnitsNew != iUnitsOld)
                    {
                        if (m_ServerMessage.PromptToSend("Unit of Measurement Changes"))
                        {                            
                            string s = CCommonTypes.PACKET_ID + "=" + ((int)CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_IMPERIAL);                            
                            m_Server.Send(s);
                        }
                    }
                    else if (iUnitsNew == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)
                    {
                        List<DataRow> lstDataChanges = frm.GetChanges();
                        if (lstDataChanges.Count > 0)
                        {                            
                            if (m_ServerMessage.PromptToSend("Unit of Measurement Changes"))
                            {
                                // update unit selection screen
                                string sClientMsg = CCommonTypes.PACKET_ID + "=" + ((int)CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_GROUP);
                                m_Server.Send(sClientMsg);

                                // update the unit group changes   
                                FormUnitSelectionByDimension frmUnitSelectionByDim = new FormUnitSelectionByDimension();
                                frmUnitSelectionByDim.Hide();
                                List<string> lstWidgetNames = frmUnitSelectionByDim.GetWidgetNames();

                                m_WidgetInfoLookupTbl.Load();
                                for (int i = 0; i < lstWidgetNames.Count; i++)
                                {
                                    string sVal = m_WidgetInfoLookupTbl.GetValue(frmUnitSelectionByDim.Name, lstWidgetNames[i]);
                                    sClientMsg = GetUnitSetGroupChange(lstWidgetNames[i], sVal);
                                    m_Server.Send(sClientMsg);
                                }                                                                

                                //List<string> lstCols = frm.GetColNames();
                                //m_Server.SendList((int)CCommonTypes.SERVER_PACKET_ID.UPDATE_A_GROUP_OF_MEASUREMENTS, lstCols, lstDataChanges);
                            }
                        }


                    }
                    else  // per d-point
                    {                                               
                        List<DataRow> lstDataChanges = frm.GetChanges();
                        bool bUnitsChanged = (iUnitsNew == CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT && iUnitsNew != iUnitsOld);
                        if (bUnitsChanged &&  
                            lstDataChanges.Count == 0)
                        {
                            if (m_ServerMessage.PromptToSend("Unit of Measurement Changes"))
                            {
                                string s = CCommonTypes.PACKET_ID + "=" + ((int)CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_DPOINT);
                                m_Server.Send(s);
                            }
                        }
                        else if (bUnitsChanged &&
                                 lstDataChanges.Count > 0)
                        {
                            if (m_ServerMessage.PromptToSend("Unit of Measurement Changes"))
                            {
                                string s = CCommonTypes.PACKET_ID + "=" + ((int)CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_DPOINT);
                                m_Server.Send(s);

                                List<string> lstCols = frm.GetColNames();
                                m_Server.SendList(((int)CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_A_UNIT_OF_MEASUREMENT), lstCols, lstDataChanges);
                            }
                        }
                        else if (!bUnitsChanged &&
                                 lstDataChanges.Count > 0)
                        {
                            if (m_ServerMessage.PromptToSend("Unit of Measurement Changes"))
                            {
                                List<string> lstCols = frm.GetColNames();
                                m_Server.SendList(((int)CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_A_UNIT_OF_MEASUREMENT), lstCols, lstDataChanges);
                            }
                        }
                        // else
                        // nothing to send
                    }                    
                }
                                              
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            //ReAdjustWindows();   
            //string sDimensions = string.Format("{0}x{1}", this.ClientSize.Width, this.ClientSize.Height);
            //Console.WriteLine("Dimensions: " + sDimensions);
        }

        private void pressureTransducerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPressureTransducerParameters frmPressureTransducer = new FormPressureTransducerParameters(ref m_dbConn);
            frmPressureTransducer.SetServerMode(m_bDisplayServer);            
            frmPressureTransducer.SetPressureTransducer(ref m_pressureTransducer);
            if (frmPressureTransducer.ShowDialog() == DialogResult.OK)
            {
                if (m_pressureTransducer.GetPressureUnit() == CPressureTransducer.PRESSURE_UNIT.PSI)                
                    m_frmPlotSamples.SetUnitSet(CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);                                                                            
                else                
                    m_frmPlotSamples.SetUnitSet(CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);
                                    
                m_frmPumpPowerTemp.SetUnitsFromTransducer(m_pressureTransducer.GetPressureUnit());
                m_pressureTransducer.Save();

                // send to clients                
                if (m_bDisplayServer && m_iConnectedClients > 0)
                {                                        
                    if (m_Server != null)
                    {
                        if (m_ServerMessage.PromptToSend("Pressure Transducer Changes"))
                        {
                            string s = m_pressureTransducer.GetClientPacket();
                            m_Server.Send(s);
                        }                        
                    }                        
                }                
            }
        }

        //private void ReAdjustWindows()
        //{
        //    //string sDimensions = string.Format("{0}x{1}", this.ClientSize.Width, this.ClientSize.Height);
        //    //this.Text = sDimensions;
            
            
        //    if (m_frmQualifiers != null && m_frmPumpPowerTemp != null && m_frmPlotSamples != null)
        //    {
        //        // start shrinking the left side widgets
        //        if (this.ClientSize.Width < CCommonTypes.MIN_WIDTH_BEFORE_SHRINKING_SIDES)
        //        {
        //            int iLeftSidePanelWidth = (int)(CCommonTypes.MAX_WIDTH_LEFT_SIDE_WIDGETS * this.ClientSize.Width / (float)CCommonTypes.MIN_WIDTH_BEFORE_SHRINKING_SIDES);                    
        //            m_frmLastSurvey.Width = iLeftSidePanelWidth;
        //            m_frmINCWhileDrilling.Width = iLeftSidePanelWidth;
        //            m_frmGamma.Width = iLeftSidePanelWidth;

        //            int iRightSidePanelWidth = (int)(CCommonTypes.MAX_WIDTH_RIGHT_SIDE_WIDGETS * this.ClientSize.Width / (float)CCommonTypes.MIN_WIDTH_BEFORE_SHRINKING_SIDES);
        //            m_frmPumpPowerTemp.Width = iRightSidePanelWidth;
        //        }
        //        else
        //        {
        //            m_frmLastSurvey.Width = CCommonTypes.MAX_WIDTH_LEFT_SIDE_WIDGETS;
        //            m_frmINCWhileDrilling.Width = CCommonTypes.MAX_WIDTH_LEFT_SIDE_WIDGETS;
        //            m_frmGamma.Width = CCommonTypes.MAX_WIDTH_LEFT_SIDE_WIDGETS;

        //            m_frmPumpPowerTemp.Width = CCommonTypes.MAX_WIDTH_RIGHT_SIDE_WIDGETS;
        //        }

        //        // start shrinking the last survey window
        //        if (this.ClientSize.Height < CCommonTypes.MIN_HEIGHT_BEFORE_SHRINKING_SIDES)
        //        {
        //            int iLeftSidePanelHeight = (int)(CCommonTypes.MAX_HEIGHT_LEFT_SIDE_WIDGETS * this.ClientSize.Height / (float)CCommonTypes.MIN_HEIGHT_BEFORE_SHRINKING_SIDES);
        //            m_frmLastSurvey.Height = iLeftSidePanelHeight;
        //        }
        //        else
        //        {
        //            m_frmLastSurvey.Height = CCommonTypes.MAX_HEIGHT_LAST_SURVEY_SIDE_WIDGETS;
        //        }

        //        Point ptMDIHorizontalCenter = new Point((this.Width / 2) - (m_frmTF.Width / 2), 0);
        //        m_frmTF.Width = this.ClientSize.Width - m_frmGamma.ClientSize.Width - m_frmPumpPowerTemp.ClientSize.Width - 2 * BLUE_BORDER_THICKNESS_TO_REMOVE;                

                     
        //        //if (m_frmMudPulsePlot.Visible)  // plot window is at the bottom
        //        //{
        //        //    if (m_frmMudPulsePlot.GetDock() == FormBorderStyle.None)
        //        //    {
        //        //        m_frmMudPulsePlot.Width = this.ClientSize.Width - 2 * BLUE_BORDER_THICKNESS_TO_REMOVE;
        //        //        m_frmMudPulsePlot.Height = 124; // this.Height - m_frmQualifiers.Height - m_frmPumpPowerTemp.Height - 80;  // had to play around with this 80 to prevent horizontal and vertical scrollbars from popping up during resizing
        //        //        Point ptPlot = new Point(SystemInformation.BorderSize.Width, this.ClientSize.Height - (m_frmMudPulsePlot.Height + menuStripFile.Height + SystemInformation.CaptionHeight));
        //        //        m_frmMudPulsePlot.Location = ptPlot;

        //        //        // qualifiers window
        //        //        m_frmQualifiers.Width = this.ClientSize.Width - 4 * BLUE_BORDER_THICKNESS_TO_REMOVE;
        //        //        Point ptQualifiers = new Point((this.ClientSize.Width / 2) - (m_frmQualifiers.ClientSize.Width / 2), this.ClientSize.Height - (m_frmMudPulsePlot.Height + m_frmQualifiers.ClientSize.Height + menuStripFile.Height + SystemInformation.CaptionHeight));
        //        //        m_frmQualifiers.Location = ptQualifiers;
        //        //    }
        //        //    else
        //        //    {
        //        //        // qualifiers window
        //        //        m_frmQualifiers.Width = this.ClientSize.Width - 4 * BLUE_BORDER_THICKNESS_TO_REMOVE;
        //        //        Point ptQualifiers = new Point((this.ClientSize.Width / 2) - (m_frmQualifiers.ClientSize.Width / 2), this.ClientSize.Height - (m_frmMudPulsePlot.Height + m_frmQualifiers.ClientSize.Height + menuStripFile.Height + SystemInformation.CaptionHeight));
        //        //        m_frmQualifiers.Location = ptQualifiers;
        //        //    }

        //        //    // tf window
        //        //    m_frmTF.Height = this.ClientSize.Height - (m_frmMudPulsePlot.Height + m_frmQualifiers.ClientSize.Height + menuStripFile.Height + SystemInformation.CaptionHeight);                    
        //        //    ptMDIHorizontalCenter.X = m_frmGamma.ClientSize.Width - SystemInformation.BorderSize.Width;
        //        //    m_frmTF.Location = ptMDIHorizontalCenter;

        //        //    Point ptPumpPowerTemp = new Point();
        //        //    // pump, power, and temperature
        //        //    ptPumpPowerTemp.X = m_frmQualifiers.Right - m_frmPumpPowerTemp.Width;
        //        //    ptPumpPowerTemp.Y = 0; // menuStripFile.Height + SystemInformation.CaptionHeight;
        //        //    m_frmPumpPowerTemp.Height = this.ClientSize.Height - (m_frmMudPulsePlot.Height + m_frmQualifiers.ClientSize.Height + menuStripFile.Height + SystemInformation.CaptionHeight);                    
        //        //    m_frmPumpPowerTemp.Location = ptPumpPowerTemp;

        //        //}
        //        //else  // qualifiers window is at the bottom
        //        {
        //            // qualifiers window
        //            m_frmQualifiers.Width = this.ClientSize.Width - 4 * BLUE_BORDER_THICKNESS_TO_REMOVE;
        //            Point ptQualifiers = new Point((this.ClientSize.Width / 2) - (m_frmQualifiers.ClientSize.Width / 2), this.ClientSize.Height - (m_frmQualifiers.ClientSize.Height + menuStripFile.Height + SystemInformation.CaptionHeight));
        //            m_frmQualifiers.Location = ptQualifiers;

        //            // tf window
        //            m_frmTF.Height = this.ClientSize.Height - (m_frmQualifiers.ClientSize.Height + menuStripFile.Height + SystemInformation.CaptionHeight);
        //            ptMDIHorizontalCenter.X = m_frmGamma.ClientSize.Width - SystemInformation.BorderSize.Width;
        //            m_frmTF.Location = ptMDIHorizontalCenter;

        //            Point ptPumpPowerTemp = new Point();
        //            // pump, power, and temperature
        //            ptPumpPowerTemp.X = m_frmQualifiers.Right - m_frmPumpPowerTemp.Width;
        //            ptPumpPowerTemp.Y = 0; // menuStripFile.Height + SystemInformation.CaptionHeight;
        //            m_frmPumpPowerTemp.Height = this.ClientSize.Height - (m_frmQualifiers.ClientSize.Height + menuStripFile.Height + SystemInformation.CaptionHeight);                    
        //            m_frmPumpPowerTemp.Location = ptPumpPowerTemp;
        //        }

        //        Point pt = new Point();

        //        // position gamma window
        //        pt.X = 0;
        //        pt.Y = m_frmQualifiers.Top - m_frmGamma.Height;                          
        //        pt.Y -= LOW_RES_HEIGHT_OFFSET;
        //        m_frmGamma.Location = pt;

        //        // position inc while drilling window
        //        pt.X = 0;                
        //        pt.Y = m_frmLastSurvey.Bottom;
                
        //        m_frmINCWhileDrilling.Location = pt;
        //        m_frmINCWhileDrilling.Height = m_frmGamma.Top - m_frmLastSurvey.Bottom;                                    
        //    }
        //}

        private void plotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_frmPlotSamples.Show(dockPanel);
        }

        private void wITSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRegisterProduct frmRegister = new FormRegisterProduct();
            frmRegister.ShowDialog();
            if (frmRegister.IsLicenseValid())
            {
                m_DetectEM.SetLicense(true);
                m_DetectMP.SetLicense(true);
                m_frmTF.SetLicense(true);
            }                
        }

        private void displayAsServerOrClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormServerSetup frm = new FormServerSetup(ref m_WidgetInfoLookupTbl);
            bool bPreviousServerMode = m_bDisplayServer;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_bDisplayServer = frm.GetServerMode();                
                m_sServerIPAddress = frm.GetServerIPAddress();
                
                m_Server.Stop();
                m_Client.Reconnect(m_sServerIPAddress, System.Convert.ToInt32(m_sPortNumber));

                if (m_bDisplayServer)
                {
                    statusStripMain.Items["toolStripStatusLabelServer"].BackColor = Color.Red;
                    statusStripMain.Items["toolStripStatusLabelServer"].ForeColor = Color.Black;
                    statusStripMain.Items["toolStripStatusLabelServer"].Text = "Clients (0)";
                    this.Text = this.Text.Replace(CLIENT_MODE, SERVER_MODE);
                }                    
                else
                {
                    statusStripMain.Items["toolStripStatusLabelServer"].BackColor = Color.Red;
                    statusStripMain.Items["toolStripStatusLabelServer"].ForeColor = Color.Black;
                    statusStripMain.Items["toolStripStatusLabelServer"].Text = "Server NA";
                    this.Text = this.Text.Replace(SERVER_MODE, CLIENT_MODE);
                }                                
            }
        }

        private void communicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigureWITS frm = new FormConfigureWITS();
            frm.SetPingInterval(m_Pason.GetPingInterval());
            frm.SetWITSLookUp(m_lookupWITS);
            frm.SetWidgetInfoLookup(ref m_WidgetInfoLookupTbl);
            frm.SetCOMS(ref m_Pason, ref m_OutgoingWITS);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                int iPasonPort, iOutgoingPort;
                frm.GetPortNumbers(out iPasonPort, out iOutgoingPort);
                int iPreviousPasonPort, iPreviousOutgoingPort;
                frm.GetPreviousPortNumbers(out iPreviousPasonPort, out iPreviousOutgoingPort);                                                                       

                m_WidgetInfoLookupTbl.Load();
                string sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "numericUpDownCOMPort");

                string sErrMsg = "";
                if (iPasonPort == 0)
                {
                    m_Pason.Stop();
                }
                else if (!m_Pason.Start(ref sErrMsg, "COM" + sVal))
                    MessageBox.Show(sErrMsg, "WITS Communication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "numericUpDownCOMPortOut1");

                if (iOutgoingPort == 0)
                {
                    m_OutgoingWITS.Stop();
                }
                else if (!m_OutgoingWITS.Start(ref sErrMsg, "COM" + sVal))
                    MessageBox.Show(sErrMsg, "WITS Outgoing Communication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // get WITS filters            
                sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "checkBoxFilterMudPulse");
                bool bVal = System.Convert.ToBoolean(sVal);
                bool bSendMudPulse = bVal;

                sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "checkBoxFilterEM");
                bVal = System.Convert.ToBoolean(sVal);
                bool bSendEM = bVal;

                //m_WITSSurveyRec.SetFilter(bSendMudPulse, bSendEM);
                ////m_WITSGammaRec.SetFilter(bSendMudPulse, bSendEM);                
                //m_WITSToolFace.SetFilter(bSendMudPulse, bSendEM);
                for (int i = 0; i < m_arrWITSGeneric.Length; i++)
                {
                    m_arrWITSGeneric[i].SetFilter(bSendMudPulse, bSendEM);
                }
            }
        }

        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigureWITSData frm = new FormConfigureWITSData();
            
            
            frm.SetServerMode(m_bDisplayServer);
            frm.SetTable(ref m_lookupWITS);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_lookupWITS = frm.GetTable();
                m_lookupWITS.Save();

                List<DataRow> lstRowsChanged = frm.GetChanges();
                if (m_bDisplayServer && 
                    m_iConnectedClients > 0 && 
                    lstRowsChanged.Count > 0)
                {                   
                    if (m_ServerMessage.PromptToSend("WITS Changes"))
                    {                        
                        List<string> lstColNames = frm.GetColNames();
                        m_Server.SendList((int)CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_WITS_RECORD, lstColNames, lstRowsChanged);
                    }                    
                }

                string sExclude = GetExcludedWITSChannels();

                List<CWITSLookupTable.WITSChannel> lstWITSGeneric = m_lookupWITS.GetFromSource("Detect", sExclude);
                if (lstWITSGeneric.Count > 0)
                {
                    // get the send mudpulse and em flags from the survey record                    
                    //bool bSendMudPulse, bSendEM;
                    //m_WITSSurveyRec.GetFilter(out bSendMudPulse, out bSendEM);

                    //m_WITSSurveyVectorRec.SetFilter(bSendMudPulse, bSendEM);
                    m_WidgetInfoLookupTbl.Load();
                    string sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "checkBoxFilterMudPulse");
                    bool bVal = System.Convert.ToBoolean(sVal);
                    bool bSendMudPulse = bVal;

                    sVal = m_WidgetInfoLookupTbl.GetValue("FormConfigureWITS", "checkBoxFilterEM");
                    bVal = System.Convert.ToBoolean(sVal);
                    bool bSendEM = bVal;

                    for (int i = 0; i < lstWITSGeneric.Count; i++)
                    {                        
                        m_arrWITSGeneric[i].SetFilter(bSendMudPulse, bSendEM);
                        m_arrWITSGeneric[i].SetChannelID(lstWITSGeneric[i].sID);
                    }
                }                
            }
        }

        private void iPAddressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your IP Address is: " + m_sMyIPAddress + "\nApply this to the Client Display's Server IP Address.", "IP Address", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void JobInfoChanged(string s)
        {            
            if (m_Server != null)
            {
                if (s.ToLower().Contains("bha")) // update MSA objects
                {
                    string[] sArr = s.Split('=');
                    //m_frmSurveyAcceptReject.SetBHA(sArr[1]);
                    m_frmSurveyAcceptRejectInf.SetBHA(sArr[1]);
                }

                if (m_bDisplayServer && m_iConnectedClients > 0)
                {
                    int iPacketID = (int)CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_JOB_OR_RIG_DESCRIPTION;
                    m_Server.Send(CCommonTypes.PACKET_ID + "=" + iPacketID.ToString() + ";" + s);
                }                
            }                
        }

        private string GetUnitSetGroupChange(string sWidgetName_, string sWidgetVal_)
        {
            string sRetVal = "";

            sRetVal = CCommonTypes.PACKET_ID + "=" + ((int)CCommonTypes.SERVER_PACKET_ID.UPDATE_A_GROUP_OF_MEASUREMENT_TYPE);
            sRetVal += ";" + sWidgetName_ + "=" + sWidgetVal_;

            return sRetVal;
        }

        void PacketReceived(string s)
        {                
            s.Trim();
            if (s.Length < 1)
                return;

            string[] sArrCols = s.Split(';');

            if (sArrCols.Length > 0)
            {
                string[] sArrData = sArrCols[0].Split('=');
                if (sArrData.Length > 0)
                {
                    if (sArrData[0] == CCommonTypes.PACKET_ID)
                    {
                        CCommonTypes.SERVER_PACKET_ID iID = (CCommonTypes.SERVER_PACKET_ID)System.Convert.ToInt32(sArrData[1]);
                        switch (iID)
                        {
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_WITS_RECORD:  // WITS                                
                                if (m_ServerMessage == null)
                                    m_ServerMessage = new CServerMessage(ref m_lookupWITS);

                                m_ServerMessage.Add(s);
                                this.Invoke(receiveServerMsgDelegate);

                                notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "A WITS record was updated.", ToolTipIcon.Info);
                                break;
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_JOB_OR_RIG_DESCRIPTION:  // rig and job info

                                m_WidgetInfoLookupTbl.Load();
                                sArrData = sArrCols[1].Split('=');
                                m_WidgetInfoLookupTbl.Set("FormPumpPowerTemp", sArrData[0], -1, sArrData[1]);
                                bool bSaved = m_WidgetInfoLookupTbl.Save();
                                
                                if (bSaved)
                                {                                                                        
                                    if (sArrData[0].ToLower().Contains("jobid"))
                                        m_JobInfo.SetJobID(sArrData[1]);
                                    else if (sArrData[0].ToLower().Contains("rig"))
                                        m_JobInfo.SetRig(sArrData[1]);
                                    else
                                    {
                                        m_JobInfo.SetBHA(System.Convert.ToInt32(sArrData[1]));
                                        //m_frmSurveyAcceptReject.SetBHA(sArrData[1]);
                                        m_frmSurveyAcceptRejectInf.SetBHA(sArrData[1]);
                                    }
                                        
                                    this.Invoke(changeJobInfoDelegate);
                                    notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "Job info was updated.", ToolTipIcon.Info);
                                }
                                break;
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_A_GROUP_OF_MEASUREMENT_TYPE:


                                m_WidgetInfoLookupTbl.Load();
                                sArrData = sArrCols[1].Split('=');
                                m_WidgetInfoLookupTbl.Set("FormUnitSelectionByDimension", sArrData[0], -1, sArrData[1]);
                                bool bSuccess = m_WidgetInfoLookupTbl.Save();
                                
                                m_unitSelection.Load(ref m_WidgetInfoLookupTbl);

                                // update the 
                                bool[] bArrGroup = m_unitSelection.GetUnitGroups();

                                if (bArrGroup[(int)CUnitSelection.UNIT_GROUPS.LENGTH])
                                {
                                    m_DPointTable.SetUnits("m", "ft");
                                    m_DPointTable.SetUnits("m/hr", "ft/hr");
                                }                                    
                                else
                                {
                                    m_DPointTable.SetUnits("ft", "m");
                                    m_DPointTable.SetUnits("ft/hr", "m/hr");
                                }
                                    

                                if (bArrGroup[(int)CUnitSelection.UNIT_GROUPS.PRESSURE])
                                    m_DPointTable.SetUnits("KPa", "Psi");
                                else
                                    m_DPointTable.SetUnits("Psi", "KPa");

                                if (bArrGroup[(int)CUnitSelection.UNIT_GROUPS.TEMPERATURE])
                                    m_DPointTable.SetUnits("°C", "°F");
                                else
                                    m_DPointTable.SetUnits("°F", "°C");

                                m_DPointTable.Save();
                                CCommonTypes.UNIT_SET iUnitsNew = m_iUnitSelection = CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP ;
                                UpdateWindowsUnits(iUnitsNew);

                                break;
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_PRESSURE_TRANSDUCER_SETTINGS:
                                m_pressureTransducer.Parse(sArrCols);
                                m_pressureTransducer.Save();
                                if (m_pressureTransducer.GetPressureUnit() == CPressureTransducer.PRESSURE_UNIT.PSI)
                                    m_frmPlotSamples.SetUnitSet(CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);
                                else
                                    m_frmPlotSamples.SetUnitSet(CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);

                                m_frmPumpPowerTemp.SetUnitsFromTransducer(m_pressureTransducer.GetPressureUnit());
                                notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "Pressure Transducer Settings were updated.", ToolTipIcon.Info);
                                break;
                                                                                     
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_A_UNIT_OF_MEASUREMENT:
                                m_unitSelection.SetUnitSet(CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT);
                                m_iUnitSelection = m_unitSelection.GetUnitSet();
                                m_unitSelection.Set(m_unitSelection.GetWidgetName((int)CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT), "True");
                                m_unitSelection.Save(ref m_WidgetInfoLookupTbl);

                                UpdateWindowsUnits(CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT);                                

                                m_ServerMessage.Add(s);
                                this.Invoke(receiveServerMsgDelegate);

                                notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "Unit of Measurement was updated.", ToolTipIcon.Info);                                
                                break;
                            //case CCommonTypes.SERVER_PACKET_ID.UPDATE_A_GROUP_OF_MEASUREMENTS:
                            //    m_unitSelection.SetUnitSet(CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP);
                            //    m_iUnitSelection = m_unitSelection.GetUnitSet();
                            //    m_unitSelection.Set(m_unitSelection.GetWidgetName((int)CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP), "True");
                            //    m_unitSelection.Save();

                            //    // need to update the options for the groups
                            //    // 

                            //    UpdateWindowsUnits(CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP);

                            //    m_ServerMessage.Add(s);
                            //    this.Invoke(receiveServerMsgDelegate);

                            //    notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "Groups of measurements updated.", ToolTipIcon.Info);

                            //    break;
                            case CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_METRIC:
                            case CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_IMPERIAL:
                            case CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_DPOINT:
                            case CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_GROUP:

                                CCommonTypes.UNIT_SET iUnitSet = CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL;  // default
                                if (iID == CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_METRIC)
                                    iUnitSet = CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC;
                                else if (iID == CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_DPOINT)
                                    iUnitSet = CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT;
                                else if (iID == CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_GROUP)
                                    iUnitSet = CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP;

                                    // initialize all the options to false
                                for (int i = 0; i < m_unitSelection.GetNumWidgets(); i++)
                                    m_unitSelection.Set(m_unitSelection.GetWidgetName(i), "False");

                                m_unitSelection.Set(m_unitSelection.GetWidgetName((int)iUnitSet), "True");  // set the chosen option
                                m_unitSelection.SetUnitSet(iUnitSet);
                                m_unitSelection.Save(ref m_WidgetInfoLookupTbl);

                                UpdateWindowsUnits(iUnitSet);                                

                                m_iUnitSelection = iUnitSet;
                                if (iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT)
                                    notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "Units of Measurement set to use each D-Point.", ToolTipIcon.Info);
                                else if (iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)
                                    notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "Units of Measurement set for groups of length, pressure, and temperature.", ToolTipIcon.Info);
                                else
                                    notifyIconServerMsg.ShowBalloonTip(1000, BALLOON_TIP_TITLE, "Units of Measurement set to" + (iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC ? " metric.": " imperial."), ToolTipIcon.Info);
                                break;
                            default:
                                break;
                        }
                    }
                    else  // bogus data
                    {
                        // ignore
                    }
                }
                else  // no data
                {
                    // ignore
                }
            }

        }

        private void ClientConnected(int iConnections)
        {
            m_iConnectedClients = iConnections;
            System.Diagnostics.Debug.WriteLine("Clients connected: " + iConnections.ToString());
            try
            {
                statusStripMain.Invoke(UpdateNumClients);
            }
            catch
            {

            }
        }

        private void UpdateNumberOfClients()
        {
            if (m_bDisplayServer)  // show number of clients
            {                                
                statusStripMain.Items["toolStripStatusLabelServer"].Text = "Clients (" + m_iConnectedClients.ToString() + ")";
            }                
            else  // show if there's a server
            {
                if (m_iConnectedClients > 0)                                   
                    statusStripMain.Items["toolStripStatusLabelServer"].Text = "Server ON";                
                else                
                    statusStripMain.Items["toolStripStatusLabelServer"].Text = "Server OFF";                                 
            }

            if (m_iConnectedClients > 0)
            {
                statusStripMain.Items["toolStripStatusLabelServer"].BackColor = Color.FromArgb(0, 192, 0);
                statusStripMain.Items["toolStripStatusLabelServer"].ForeColor = Color.Black;
            }
            else
            {
                statusStripMain.Items["toolStripStatusLabelServer"].BackColor = Color.Red;
                statusStripMain.Items["toolStripStatusLabelServer"].ForeColor = Color.Black;
            }
        }

        private void toolStripStatusLabelServer_Click(object sender, EventArgs e)
        {
            // test
            //string sMsg = CCommonTypes.PACKET_ID + "=1;APSMessageCode=9;ID=0715;Name=AZM;DataType=FLOAT;NativeUnit=Deg;Math=this + 10;SendIfError=Yes;OutlinerMin=None;OutlinerMax=None;Source=Detect";
            //m_ServerMessage.Add(sMsg);
            //sMsg = CCommonTypes.PACKET_ID + "=1;APSMessageCode=25;ID=0913;Name=A Pressure;DataType=FLOAT;NativeUnit=psi;Math=this - 2;SendIfError=Yes;OutlinerMin=None;OutlinerMax=None;Source=Detect";
            //m_ServerMessage.Add(sMsg);
            if (m_bDisplayServer)
                ShowClientMessages();
            else
                ShowServerMessages();
        }

        private void toolStripMenuItemViewServerMsgs_Click(object sender, EventArgs e)
        {
            if (m_bDisplayServer)
                ShowClientMessages();
            else
                ShowServerMessages();
        }

        private void ShowClientMessages()
        {
            FormServerMessage frm = new FormServerMessage();
            frm.SetSentMessages(m_Server.GetSentPackets());
            frm.SetServerMode();
            frm.ShowDialog();
        }

        private void ShowServerMessages()
        {
            FormServerMessage frm = new FormServerMessage();

            frm.SetMessages(m_ServerMessage.Get());
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // check if there are records to update
                List<CServerMessage.MESSAGE_REC> lst = frm.GetChanges();
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].bAccept)
                    {
                        if (m_ServerMessage.Process(lst[i].sData, ref m_DPointTable))
                            m_ServerMessage.Remove(lst[i].sData);
                    }                        
                    else if (lst[i].bReject)
                        m_ServerMessage.Remove(lst[i].sData);
                }                
            }

            if (m_ServerMessage.Get().Count == 0)  // yellow highlight and envelope goes away
            {
                statusStripMain.Items["toolStripStatusLabelServer"].Image = null;
                UpdateNumberOfClients();
            }
        }

        private void multiStationAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            FormMSASettings frm = new FormMSASettings();
            frm.SetWidgetInfoLookup(ref m_WidgetInfoLookupTbl);
            frm.SetServerMode(m_bDisplayServer);
            //CJobInfo jobInfo = new CJobInfo(ref m_dbConn, ref m_WidgetInfoLookupTbl);
            //jobInfo.Load();
            frm.SetJob(false, m_JobInfo);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string sURL, sAPIKey;
                
                frm.GetSettings(out sURL, out sAPIKey, out m_bUseAcceptRejectFeature);
                if (m_sMSAURL != sURL || m_JobInfo.GetAPIKey() != sAPIKey)
                {
                    m_sMSAURL = sURL;
                    m_JobInfo.SetAPIKey(sAPIKey);
                    m_MSAHub.SetInfo(m_sMSAURL, sAPIKey, m_SettingsLookupTbl.GetJobPath());
                    //m_frmSurveyAcceptReject.SetInfo(m_sMSAURL, m_sMSAAPIKey);
                    m_frmSurveyAcceptRejectInf.SetInfo(m_sMSAURL, sAPIKey);
                    m_frmChat.SetInfo(m_sMSAURL, sAPIKey);
                    m_frmChat.Disconnect(true); // the async to reconnect after the disconnect
                    if (sAPIKey.Trim().Length == 0 || !frm.IsMSA())
                        m_frmChat.Hide();
                    else
                        m_frmChat.Show();
                }
                
                string sJob, sRig, sBHA;
                int iMSADummy = 0;
                CJobInfo.SURVEY_MANAGEMENT_MODE svyMgmtMode;
                frm.GetJobInfo(out sJob, out sRig, out sBHA, out iMSADummy, out svyMgmtMode);
                if (m_JobInfo.GetJobID() != sJob)
                {
                    m_JobInfo.SetJobID(sJob);
                    JobInfoChanged("maskedTextBoxJobID=" + sJob);
                }

                if (m_JobInfo.GetRig() != sRig)
                {
                    m_JobInfo.SetRig(sRig);
                    JobInfoChanged("maskedTextBoxRig=" + sRig);
                }

                if (m_JobInfo.GetBHA().ToString() != sBHA)
                {
                    m_JobInfo.SetBHA(System.Convert.ToInt32(sBHA));
                    JobInfoChanged("maskedTextBoxBHA=" + sBHA);
                }
                
                m_JobInfo.SetSurveyManagmentMode(svyMgmtMode);
                m_JobInfo.Save();
                
                UpdateTitleBar(m_JobInfo);
                m_frmSurveyAcceptRejectInf.SetBHA(m_JobInfo.GetBHA().ToString());
            }
        }
        

        private void rawToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            CSurveyLog log = new CSurveyLog(ref m_dbConn);
            DataTable tbl = log.Get();
            m_frmRawView.SetSurveyLog(ref log);
            m_frmRawView.SetData(tbl);
            if (!m_frmRawView.Visible)
                m_frmRawView.Show();            
        }

        private void correctedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_JobInfo.GetAPIKey().Trim().Length == 0)
            {
                MessageBox.Show("Viewing Corrected Surveys only works with MSA enabled.  See menu System Settings->Job and Survey Management.", "View Corrected Surveys", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                DataTable tbl = m_MSAHub.GetCorrectedTable();
                m_frmMSACorrected.SetTitle("Corrected Surveys");
                m_frmMSACorrected.SetData(tbl);
                if (!m_frmMSACorrected.Visible)
                {
                    m_frmMSACorrected.Show();
                }                    
            }            
        }

        private void chatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_JobInfo.GetAPIKey().Trim().Length == 0)
            {
                MessageBox.Show("Chat only works with MSA enabled.  See menu System Settings->Job and Survey Management.", "Help Chat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                m_frmChat.Hide();
            }                
            else
            {
                // force it to appear if already normal so that it appears on top
                if (m_frmChat.WindowState == FormWindowState.Normal)
                    m_frmChat.WindowState = FormWindowState.Minimized;

                m_frmChat.WindowState = FormWindowState.Normal;
                m_frmChat.Show();
            }
            
        }

        private void surveyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
                                          
        }

        private void surveyRawToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rawToolStripMenuItem1_Click(object sender, EventArgs e)
        {           
            CSurveyLog log = new CSurveyLog(ref m_dbConn);
            DataTable tbl = log.Get();            
            m_frmRawEdit.SetSurveyLog(ref log);
            m_frmRawEdit.SetData(tbl);

            m_frmRawEdit.SetUnitSet(m_iUnitSelection);
            m_frmRawEdit.SetTitle("Edit Raw Surveys");
            
            if (!m_frmRawEdit.Visible)
                m_frmRawEdit.Show();
        }

        private void mSAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_JobInfo.GetAPIKey().Trim().Length == 0)
            {
                MessageBox.Show("Editing MSA Surveys only works with MSA enabled.  See menu System Settings->Job and Survey Management.", "Edit MSA Survey", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable tbl = m_MSAHub.GetSensorDetailsTable();
                m_frmMSAEdit.SetMSAHub(ref m_MSAHub);
                m_frmMSAEdit.SetUnitSet(m_iUnitSelection);
                m_frmMSAEdit.SetData(tbl);
                m_frmMSAEdit.SetLastDirToBit(m_Survey.GetLastDirToBit());
                if (!m_frmMSAEdit.Visible)
                    m_frmMSAEdit.Show();
            }
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            FormMSASettings frm = new FormMSASettings();
            frm.SetWidgetInfoLookup(ref m_WidgetInfoLookupTbl);
            frm.SetServerMode(m_bDisplayServer);

            CJobInfo jobInfoNew = new CJobInfo(ref m_dbConn, ref m_WidgetInfoLookupTbl);            
            frm.SetJob(true, jobInfoNew);

            int iMSA = 0;  // flag indicating they are using msa

            bool bEverythingIsOK = false;
            string sPreviousJobID = m_JobInfo.GetJobID();
            int iPreviousMSAFlag = m_JobInfo.GetMSA();
            string sNewFolder = "";
            if (frm.ShowDialog() == DialogResult.OK)
            {                
                try
                {                    
                    string sNewJob, sNewRig, sNewBHA;
                    CJobInfo.SURVEY_MANAGEMENT_MODE svyMgmtMode;
                    frm.GetJobInfo(out sNewJob, out sNewRig, out sNewBHA, out iMSA, out svyMgmtMode);
                    jobInfoNew.SetSurveyManagmentMode(svyMgmtMode);

                    // determine if it's already an existing job
                    sNewFolder = CCommonTypes.DATA_FOLDER + "Jobs\\" + sNewJob;
                    try
                    {
                        Directory.CreateDirectory(sNewFolder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating folder " + sNewFolder + ": " + ex.Message, "New Job Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (jobInfoNew.GetJobID() != sNewJob)
                    {
                        jobInfoNew.SetJobID(sNewJob);
                        JobInfoChanged("maskedTextBoxJobID=" + sNewJob);
                    }

                    if (jobInfoNew.GetRig() != sNewRig)
                    {
                        jobInfoNew.SetRig(sNewRig);
                        JobInfoChanged("maskedTextBoxRig=" + sNewRig);
                    }

                    if (jobInfoNew.GetBHA().ToString() != sNewBHA)
                    {
                        jobInfoNew.SetBHA(System.Convert.ToInt32(sNewBHA));
                        JobInfoChanged("maskedTextBoxBHA=" + sNewBHA);
                    }

                    string sURL, sAPIKey;
                    
                    frm.GetSettings(out sURL, out sAPIKey, out m_bUseAcceptRejectFeature);
                    if (m_sMSAURL != sURL || jobInfoNew.GetAPIKey() != sAPIKey)
                    {
                        m_sMSAURL = sURL;
                        jobInfoNew.SetAPIKey(sAPIKey);
                        m_MSAHub.SetInfo(m_sMSAURL, sAPIKey, sNewFolder);

                        m_frmSurveyAcceptRejectInf.SetInfo(m_sMSAURL, sAPIKey);

                        m_frmChat.SetInfo(m_sMSAURL, sAPIKey);
                        m_frmChat.Disconnect(true); // the async to reconnect after the disconnect
                        if (sAPIKey.Trim().Length == 0 || !frm.IsMSA())
                            m_frmChat.Hide();
                        else
                            m_frmChat.Show();
                    }

                    //m_sDBPath = sNewFolder + "log.db";

                    bEverythingIsOK = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when creating new job information: " + ex.Message + ".", "New Job Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //  verify that the job id is not the same as any existing subfolder
            //  and verify that the job id is not the same as the current one

            // backup existing database to backups using date-time stamp
            // copy over all the files and database
            if (bEverythingIsOK)
            {
                m_JobInfo = jobInfoNew;

                string sDefaultName = "";
                string sName = "";
                // copy session files over                
                try
                {
                    // Will not overwrite if the destination file already exists.
                    sDefaultName = "XMLFileFormSettingsDefault.xml";
                    sName = "XMLFileFormSettings.xml";
                    File.Copy(Path.Combine(CCommonTypes.DATA_FOLDER, sDefaultName), Path.Combine(sNewFolder, sName));
                }
                // Catch exception if the file was already copied.
                catch (IOException copyError)
                {
                    MessageBox.Show("Error copying '" + Path.Combine(CCommonTypes.DATA_FOLDER, sDefaultName) + " to '" + Path.Combine(sNewFolder, sName) + "' folder: " + copyError.Message + ".", "New Job", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    // Will not overwrite if the destination file already exists.
                    sDefaultName = "XMLFileProtocolCommandsDefault.xml";
                    sName = "XMLFileProtocolCommands.xml";
                    File.Copy(Path.Combine(CCommonTypes.DATA_FOLDER, sDefaultName), Path.Combine(sNewFolder, sName));
                }
                // Catch exception if the file was already copied.
                catch (IOException copyError)
                {
                    MessageBox.Show("Error copying '" + Path.Combine(CCommonTypes.DATA_FOLDER, sDefaultName) + " to '" + Path.Combine(sNewFolder, sName) + "' folder: " + copyError.Message + ".", "New Job", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    // Will not overwrite if the destination file already exists.
                    sDefaultName = "XMLFileWITSDefault.xml";
                    sName = "XMLFileWITS.xml";
                    File.Copy(Path.Combine(CCommonTypes.DATA_FOLDER, sDefaultName), Path.Combine(sNewFolder, sName));
                }
                // Catch exception if the file was already copied.
                catch (IOException copyError)
                {
                    MessageBox.Show("Error copying '" + Path.Combine(CCommonTypes.DATA_FOLDER, sDefaultName) + " to '" + Path.Combine(sNewFolder, sName) + "' folder: " + copyError.Message + ".", "New Job", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                

                //  copy blank.db to new path as log.db                
                try
                {
                    string sDBName = "log.db";
                    string sDBBlankName = "BlankLog.db";
                    File.Copy(Path.Combine(CCommonTypes.DATA_FOLDER, sDBBlankName), Path.Combine(sNewFolder, sDBName), true);
                }
                catch (IOException copyError)
                {                    
                    MessageBox.Show("Error overwriting 'c:\\aps\\data\\log.db' with blank.db: " + copyError.Message + ".", "New Job Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    m_dbConn.Close();
                    
                    m_dbConn.ConnectionString = "Data Source=" + sNewFolder + "\\log.db";
                    m_dbConn.Open();

                    //  save path of current database to local settings            
                    m_SettingsLookupTbl.Load();
                    m_SettingsLookupTbl.Set("DATA_SOURCE", m_dbConn.ConnectionString);
                    m_SettingsLookupTbl.Save();

                    // refresh lookup objects
                    m_lookupWITS.Init();
                    m_lookupWITS.Load();

                    m_DPointTable.Init();
                    m_DPointTable.Load();

                    m_WidgetInfoLookupTbl.Init();
                    m_WidgetInfoLookupTbl.Load();

                    m_JobInfo.Create(m_JobInfo.GetJobID(), m_JobInfo.GetRig(), System.Convert.ToInt32(m_JobInfo.GetBHA()), iMSA, m_JobInfo.GetAPIKey());
                    m_JobInfo.SaveSession();
                    
                    UpdateTitleBar(m_JobInfo);
                    //m_frmPumpPowerTemp.SetInfo(ref jobInfo, true);
                    m_frmSurveyAcceptRejectInf.SetBHA(m_JobInfo.GetBHA().ToString());

                    // clear session information
                    m_frmGamma.RefreshScreen();
                    m_frmLastSurvey.RefreshScreen();
                    m_frmQualifiers.RefreshScreen();
                    m_frmPumpPowerTemp.RefreshScreen();
                    m_frmTF.RefreshScreen();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening 'c:\\aps\\data\\log.db': " + ex.Message + ". Try creating another Job.", "New Job Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }            
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {                      
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "C:\\aps\\data\\jobs\\";
            dlg.Filter = "Database files (*.db)|*.db";
            dlg.Title = "Open Database file";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string sNewDBPath = dlg.FileName;
                try
                {                    
                    m_dbConn.Close();

                    m_dbConn.ConnectionString = "Data Source=" + sNewDBPath;
                    m_dbConn.Open();

                    // save path
                    m_SettingsLookupTbl.Load();
                    m_SettingsLookupTbl.Set("DATA_SOURCE", m_dbConn.ConnectionString);
                    m_SettingsLookupTbl.Save();
                    
                    // refresh lookup objects
                    m_lookupWITS.Init();
                    m_lookupWITS.Load();

                    m_DPointTable.Init();
                    m_DPointTable.Load();

                    m_WidgetInfoLookupTbl.Init();
                    m_WidgetInfoLookupTbl.Load();

                    ClearWITSObjects();
                    InitWITSObjects(m_WidgetInfoLookupTbl, false);

                    m_JobInfo = new CJobInfo(ref m_dbConn, ref m_WidgetInfoLookupTbl);
                    m_JobInfo.Load();
                    

                    UpdateTitleBar(m_JobInfo);
                    m_frmSurveyAcceptRejectInf.SetBHA(m_JobInfo.GetBHA().ToString());                    

                    // refresh all the screens
                    m_frmTF.RefreshScreen();
                    m_frmQualifiers.RefreshScreen();
                    m_frmPumpPowerTemp.RefreshScreen();
                    m_frmLastSurvey.RefreshScreen();
                    m_frmGamma.RefreshScreen();                    

                    // MSA stuff                                         
                    m_sMSAURL = m_WidgetInfoLookupTbl.GetValue("FormMSASettings", "textBoxURL");
                    m_bUseAcceptRejectFeature = System.Convert.ToBoolean(m_WidgetInfoLookupTbl.GetValue("FormMSASettings", "checkBoxUseAcceptRejectFeature"));

                    m_MSAHub.SetInfo(m_sMSAURL, m_JobInfo.GetAPIKey(), m_SettingsLookupTbl.GetJobPath());
                    m_MSAHub.Register();

                    // MSA chat                    
                    m_frmChat.SetInfo(m_sMSAURL, m_JobInfo.GetAPIKey());
                    m_frmChat.Disconnect(true); // the async to reconnect after the disconnect                    
                    if (m_JobInfo.GetAPIKey().Trim().Length == 0)
                        m_frmChat.Hide();
                    else
                        m_frmChat.Show();

                    m_frmSurveyAcceptRejectInf.SetInfo(m_sMSAURL, m_JobInfo.GetAPIKey());
                    m_bMSAConnected = false;

                    ReconnectToDetect();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error found opening " + sNewDBPath + ": " + ex.Message + ". Try another database file.", "Open Job Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        

        private void realtimeDetectLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_frmRealTimeDetectLog.Show(dockPanel);
        }        
                     
        private void listViewDetectLog_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            //e.DrawDefault = true;            
            //if ((e.ItemIndex % 2) == 1)
            //{
            //    e.Item.BackColor = Color.FromArgb(60, 152, 196);
            //   // e.Item.UseItemStyleForSubItems = true;
            //}
        }

        private void fFTToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            m_frmFFT.Show(dockPanel);                            
        }

        private void receiverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReceiverSetting frm = new FormReceiverSetting(ref m_WidgetInfoLookupTbl);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string sReceiverType = frm.GetReceiverType();
                m_frmPlotSamples.SetReceiverType((CCommonTypes.RECEIVER_TYPE)System.Convert.ToInt32(sReceiverType));
            }
        }

        private void sNRPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_frmSNR.Show(dockPanel);            
        }

        private void UpdateRawSurveyLists(object sender)
        {
            m_frmRawView.UpdateList(sender);
            m_frmRawEdit.UpdateList(sender);
        }

        private void eCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!m_frmECD.Visible)
            //    m_frmECD.Show();
            //else
            //    m_frmECD.WindowState = FormWindowState.Normal;
        }

        private void eCDToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormECD frmECD = new FormECD(ref m_dbConn, ref m_lookupWITS, ref m_ECD, m_unitSelection);
            if (frmECD.ShowDialog() == DialogResult.OK)
            {
                float fTVD, fMudDensity;
                
                m_ECD.GetParameters(out fTVD, out fMudDensity);
                m_ECD.SetParameters(fTVD, fMudDensity);
                m_WidgetInfoLookupTbl.SetValue("FormECD", "textBoxTVD", fTVD.ToString());
                m_WidgetInfoLookupTbl.SetValue("FormECd", "textBoxMudDensity", fMudDensity.ToString());
                m_WidgetInfoLookupTbl.Save();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string configFile = Path.Combine(CCommonTypes.DATA_FOLDER, "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        private void toolFaceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
            if (m_frmTF.DockState != DockState.Float)
                m_frmTF.DockState = DockState.Float;

            m_frmTF.Show(dockPanel);
        }

        private void qualifiersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
            if (m_frmQualifiers.DockState != DockState.Float)
                m_frmQualifiers.DockState = DockState.Float;

            m_frmQualifiers.Show(dockPanel);
        }

        private void gammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
            if (m_frmGamma.DockState != DockState.Float)
                m_frmGamma.DockState = DockState.Float;

            m_frmGamma.Show(dockPanel);
        }

        private void last20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
            if (m_frmINCWhileDrilling.DockState != DockState.Float)
                m_frmINCWhileDrilling.DockState = DockState.Float;
            m_frmINCWhileDrilling.Show(dockPanel);
        }

        private void surveyHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
            if (m_frmLastSurvey.DockState != DockState.Float)
                m_frmLastSurvey.DockState = DockState.Float;

            m_frmLastSurvey.Show(dockPanel);
        }

        private void pumpPressurePowerTemperatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
            if (m_frmPumpPowerTemp.DockState != DockState.Float)
                m_frmPumpPowerTemp.DockState = DockState.Float;

            m_frmPumpPowerTemp.Show(dockPanel);
        }       

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            if (!m_b560R)
            {
                Process p = Process.Start(@"C:\Program Files (x86)\Applied Physics Systems\Rx Controller\apsremotereceiver.exe");
                p.WaitForInputIdle();
                SetParent(p.MainWindowHandle, this.Handle);
            }
            else
            {
                MessageBox.Show("The 560R Remote Rx is already running.", "560R Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }        

        private void rRemoteRxToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (statusStripMain.Items["toolStripStatus560R"].Text == "Launch 560R APP")
                MessageBox.Show("Please launch the 560R application first before attempting to request the settings.\nYou can do this by clicking on the button on the status bar at the bottom.", "560R Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                m_Server.Send("560RSETTINGS");
                FormRemoteControl560R frm = new FormRemoteControl560R();
                frm.ShowDialog();
            }
            
        }

        private void plotEMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
           if (m_frmPlotSamplesEM.DockState != DockState.Float)
               m_frmPlotSamplesEM.DockState = DockState.Float;
           
           m_frmPlotSamplesEM.Show(dockPanel);
        }

        private void fFTEMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force form to float so that it lay over all other forms
            if (m_frmFFTEM.DockState != DockState.Float)
                m_frmFFTEM.DockState = DockState.Float;

            m_frmFFTEM.Show(dockPanel);
        }
    }
}
