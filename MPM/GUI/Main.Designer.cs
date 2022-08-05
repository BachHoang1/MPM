namespace MPM.GUI
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (m_DPointTable != null)
                    m_DPointTable.Dispose();
            }
                                               
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStripFile = new System.Windows.Forms.MenuStrip();
            this.jobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surveyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mSAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fFTEMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fFTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gammaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotEMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pumpPressurePowerTemperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.realtimeDetectLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qualifiersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sNRPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surveyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.correctedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.last20ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surveyHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolFaceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rRemoteRxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.iPAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAsServerOrClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiStationAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PressureTransducerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.receiverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wITSToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.communicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lASToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPAddressToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolFaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelDetect = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMP = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelWITS = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusMSA = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatus560R = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIconServerMsg = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemViewServerMsgs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripFile.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.contextMenuStripTrayIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripFile
            // 
            this.menuStripFile.BackColor = System.Drawing.Color.DarkGray;
            this.menuStripFile.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStripFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.menuStripFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jobToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.setToolStripMenuItem,
            this.lASToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.menuStripFile.Location = new System.Drawing.Point(1, -1);
            this.menuStripFile.Name = "menuStripFile";
            this.menuStripFile.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.menuStripFile.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStripFile.Size = new System.Drawing.Size(430, 24);
            this.menuStripFile.TabIndex = 1;
            this.menuStripFile.Text = "menuStripFile";
            // 
            // jobToolStripMenuItem
            // 
            this.jobToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem});
            this.jobToolStripMenuItem.Name = "jobToolStripMenuItem";
            this.jobToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.jobToolStripMenuItem.Text = "Job";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.surveyToolStripMenuItem1});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // surveyToolStripMenuItem1
            // 
            this.surveyToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSAToolStripMenuItem,
            this.rawToolStripMenuItem1});
            this.surveyToolStripMenuItem1.Name = "surveyToolStripMenuItem1";
            this.surveyToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            this.surveyToolStripMenuItem1.Text = "Survey";
            this.surveyToolStripMenuItem1.Click += new System.EventHandler(this.surveyToolStripMenuItem1_Click);
            // 
            // mSAToolStripMenuItem
            // 
            this.mSAToolStripMenuItem.Name = "mSAToolStripMenuItem";
            this.mSAToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.mSAToolStripMenuItem.Text = "MSA";
            this.mSAToolStripMenuItem.Click += new System.EventHandler(this.mSAToolStripMenuItem_Click);
            // 
            // rawToolStripMenuItem1
            // 
            this.rawToolStripMenuItem1.Name = "rawToolStripMenuItem1";
            this.rawToolStripMenuItem1.Size = new System.Drawing.Size(99, 22);
            this.rawToolStripMenuItem1.Text = "Raw";
            this.rawToolStripMenuItem1.Click += new System.EventHandler(this.rawToolStripMenuItem1_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fFTEMToolStripMenuItem,
            this.fFTToolStripMenuItem,
            this.gammaToolStripMenuItem,
            this.logToolStripMenuItem,
            this.plotEMToolStripMenuItem,
            this.plotToolStripMenuItem,
            this.pumpPressurePowerTemperatureToolStripMenuItem,
            this.realtimeDetectLogToolStripMenuItem,
            this.qualifiersToolStripMenuItem,
            this.sNRPlotToolStripMenuItem,
            this.surveyToolStripMenuItem,
            this.toolFaceToolStripMenuItem1});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // fFTEMToolStripMenuItem
            // 
            this.fFTEMToolStripMenuItem.Name = "fFTEMToolStripMenuItem";
            this.fFTEMToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.fFTEMToolStripMenuItem.Text = "FFT EM";
            this.fFTEMToolStripMenuItem.Click += new System.EventHandler(this.fFTEMToolStripMenuItem_Click);
            // 
            // fFTToolStripMenuItem
            // 
            this.fFTToolStripMenuItem.Name = "fFTToolStripMenuItem";
            this.fFTToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.fFTToolStripMenuItem.Text = "FFT MP";
            this.fFTToolStripMenuItem.Click += new System.EventHandler(this.fFTToolStripMenuItem_Click);
            // 
            // gammaToolStripMenuItem
            // 
            this.gammaToolStripMenuItem.Name = "gammaToolStripMenuItem";
            this.gammaToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.gammaToolStripMenuItem.Text = "Gamma";
            this.gammaToolStripMenuItem.Click += new System.EventHandler(this.gammaToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.logToolStripMenuItem.Text = "Log";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // plotEMToolStripMenuItem
            // 
            this.plotEMToolStripMenuItem.Name = "plotEMToolStripMenuItem";
            this.plotEMToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.plotEMToolStripMenuItem.Text = "Plot EM";
            this.plotEMToolStripMenuItem.Click += new System.EventHandler(this.plotEMToolStripMenuItem_Click);
            // 
            // plotToolStripMenuItem
            // 
            this.plotToolStripMenuItem.Name = "plotToolStripMenuItem";
            this.plotToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.plotToolStripMenuItem.Text = "Plot MP";
            this.plotToolStripMenuItem.Click += new System.EventHandler(this.plotToolStripMenuItem_Click);
            // 
            // pumpPressurePowerTemperatureToolStripMenuItem
            // 
            this.pumpPressurePowerTemperatureToolStripMenuItem.Name = "pumpPressurePowerTemperatureToolStripMenuItem";
            this.pumpPressurePowerTemperatureToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.pumpPressurePowerTemperatureToolStripMenuItem.Text = "Pump Pressure, Power, Temperature";
            this.pumpPressurePowerTemperatureToolStripMenuItem.Click += new System.EventHandler(this.pumpPressurePowerTemperatureToolStripMenuItem_Click);
            // 
            // realtimeDetectLogToolStripMenuItem
            // 
            this.realtimeDetectLogToolStripMenuItem.Name = "realtimeDetectLogToolStripMenuItem";
            this.realtimeDetectLogToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.realtimeDetectLogToolStripMenuItem.Text = "Real-Time Detect Log";
            this.realtimeDetectLogToolStripMenuItem.Click += new System.EventHandler(this.realtimeDetectLogToolStripMenuItem_Click);
            // 
            // qualifiersToolStripMenuItem
            // 
            this.qualifiersToolStripMenuItem.Name = "qualifiersToolStripMenuItem";
            this.qualifiersToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.qualifiersToolStripMenuItem.Text = "Qualifiers";
            this.qualifiersToolStripMenuItem.Click += new System.EventHandler(this.qualifiersToolStripMenuItem_Click);
            // 
            // sNRPlotToolStripMenuItem
            // 
            this.sNRPlotToolStripMenuItem.Name = "sNRPlotToolStripMenuItem";
            this.sNRPlotToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.sNRPlotToolStripMenuItem.Text = "SNR Plot";
            this.sNRPlotToolStripMenuItem.Click += new System.EventHandler(this.sNRPlotToolStripMenuItem_Click);
            // 
            // surveyToolStripMenuItem
            // 
            this.surveyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.correctedToolStripMenuItem,
            this.rawToolStripMenuItem,
            this.last20ToolStripMenuItem,
            this.surveyHistoryToolStripMenuItem});
            this.surveyToolStripMenuItem.Name = "surveyToolStripMenuItem";
            this.surveyToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.surveyToolStripMenuItem.Text = "Survey";
            // 
            // correctedToolStripMenuItem
            // 
            this.correctedToolStripMenuItem.Name = "correctedToolStripMenuItem";
            this.correctedToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.correctedToolStripMenuItem.Text = "Corrected";
            this.correctedToolStripMenuItem.Click += new System.EventHandler(this.correctedToolStripMenuItem_Click);
            // 
            // rawToolStripMenuItem
            // 
            this.rawToolStripMenuItem.Name = "rawToolStripMenuItem";
            this.rawToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.rawToolStripMenuItem.Text = "Raw";
            this.rawToolStripMenuItem.Click += new System.EventHandler(this.rawToolStripMenuItem_Click);
            // 
            // last20ToolStripMenuItem
            // 
            this.last20ToolStripMenuItem.Name = "last20ToolStripMenuItem";
            this.last20ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.last20ToolStripMenuItem.Text = "INC History";
            this.last20ToolStripMenuItem.Click += new System.EventHandler(this.last20ToolStripMenuItem_Click);
            // 
            // surveyHistoryToolStripMenuItem
            // 
            this.surveyHistoryToolStripMenuItem.Name = "surveyHistoryToolStripMenuItem";
            this.surveyHistoryToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.surveyHistoryToolStripMenuItem.Text = "Survey History";
            this.surveyHistoryToolStripMenuItem.Click += new System.EventHandler(this.surveyHistoryToolStripMenuItem_Click);
            // 
            // toolFaceToolStripMenuItem1
            // 
            this.toolFaceToolStripMenuItem1.Name = "toolFaceToolStripMenuItem1";
            this.toolFaceToolStripMenuItem1.Size = new System.Drawing.Size(264, 22);
            this.toolFaceToolStripMenuItem1.Text = "Tool Face";
            this.toolFaceToolStripMenuItem1.Click += new System.EventHandler(this.toolFaceToolStripMenuItem1_Click);
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rRemoteRxToolStripMenuItem1,
            this.iPAddressToolStripMenuItem,
            this.displayAsServerOrClientToolStripMenuItem,
            this.eCDToolStripMenuItem,
            this.multiStationAnalysisToolStripMenuItem,
            this.PressureTransducerToolStripMenuItem,
            this.receiverToolStripMenuItem,
            this.unitsToolStripMenuItem,
            this.wITSToolStripMenuItem1});
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.setToolStripMenuItem.Text = "System Settings";
            // 
            // rRemoteRxToolStripMenuItem1
            // 
            this.rRemoteRxToolStripMenuItem1.Name = "rRemoteRxToolStripMenuItem1";
            this.rRemoteRxToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.rRemoteRxToolStripMenuItem1.Text = "560R Remote Rx";
            this.rRemoteRxToolStripMenuItem1.Click += new System.EventHandler(this.rRemoteRxToolStripMenuItem1_Click);
            // 
            // iPAddressToolStripMenuItem
            // 
            this.iPAddressToolStripMenuItem.Name = "iPAddressToolStripMenuItem";
            this.iPAddressToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.iPAddressToolStripMenuItem.Text = "Detect IP Addresses";
            this.iPAddressToolStripMenuItem.Click += new System.EventHandler(this.iPAddressToolStripMenuItem_Click);
            // 
            // displayAsServerOrClientToolStripMenuItem
            // 
            this.displayAsServerOrClientToolStripMenuItem.Name = "displayAsServerOrClientToolStripMenuItem";
            this.displayAsServerOrClientToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.displayAsServerOrClientToolStripMenuItem.Text = "Display As Server or Client";
            this.displayAsServerOrClientToolStripMenuItem.Click += new System.EventHandler(this.displayAsServerOrClientToolStripMenuItem_Click);
            // 
            // eCDToolStripMenuItem
            // 
            this.eCDToolStripMenuItem.Name = "eCDToolStripMenuItem";
            this.eCDToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.eCDToolStripMenuItem.Text = "ECD";
            this.eCDToolStripMenuItem.Click += new System.EventHandler(this.eCDToolStripMenuItem_Click_1);
            // 
            // multiStationAnalysisToolStripMenuItem
            // 
            this.multiStationAnalysisToolStripMenuItem.Name = "multiStationAnalysisToolStripMenuItem";
            this.multiStationAnalysisToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.multiStationAnalysisToolStripMenuItem.Text = "Job  and Survey Management";
            this.multiStationAnalysisToolStripMenuItem.Click += new System.EventHandler(this.multiStationAnalysisToolStripMenuItem_Click);
            // 
            // PressureTransducerToolStripMenuItem
            // 
            this.PressureTransducerToolStripMenuItem.Name = "PressureTransducerToolStripMenuItem";
            this.PressureTransducerToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.PressureTransducerToolStripMenuItem.Text = "Pressure Transducer";
            this.PressureTransducerToolStripMenuItem.Click += new System.EventHandler(this.pressureTransducerToolStripMenuItem_Click);
            // 
            // receiverToolStripMenuItem
            // 
            this.receiverToolStripMenuItem.Name = "receiverToolStripMenuItem";
            this.receiverToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.receiverToolStripMenuItem.Text = "Receiver";
            this.receiverToolStripMenuItem.Visible = false;
            this.receiverToolStripMenuItem.Click += new System.EventHandler(this.receiverToolStripMenuItem_Click);
            // 
            // unitsToolStripMenuItem
            // 
            this.unitsToolStripMenuItem.Name = "unitsToolStripMenuItem";
            this.unitsToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.unitsToolStripMenuItem.Text = "Units";
            this.unitsToolStripMenuItem.Click += new System.EventHandler(this.unitsToolStripMenuItem_Click);
            // 
            // wITSToolStripMenuItem1
            // 
            this.wITSToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.communicationToolStripMenuItem,
            this.dataToolStripMenuItem});
            this.wITSToolStripMenuItem1.Name = "wITSToolStripMenuItem1";
            this.wITSToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.wITSToolStripMenuItem1.Text = "WITS";
            this.wITSToolStripMenuItem1.Click += new System.EventHandler(this.wITSToolStripMenuItem1_Click);
            // 
            // communicationToolStripMenuItem
            // 
            this.communicationToolStripMenuItem.Name = "communicationToolStripMenuItem";
            this.communicationToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.communicationToolStripMenuItem.Text = "Communication";
            this.communicationToolStripMenuItem.Click += new System.EventHandler(this.communicationToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.dataToolStripMenuItem.Text = "Data";
            this.dataToolStripMenuItem.Click += new System.EventHandler(this.dataToolStripMenuItem_Click);
            // 
            // lASToolStripMenuItem
            // 
            this.lASToolStripMenuItem.Name = "lASToolStripMenuItem";
            this.lASToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.lASToolStripMenuItem.Text = "LAS";
            this.lASToolStripMenuItem.Click += new System.EventHandler(this.lASToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iPAddressToolStripMenuItem1,
            this.toolFaceToolStripMenuItem,
            this.registerToolStripMenuItem,
            this.chatToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.windowToolStripMenuItem.Text = "Help";
            // 
            // iPAddressToolStripMenuItem1
            // 
            this.iPAddressToolStripMenuItem1.Name = "iPAddressToolStripMenuItem1";
            this.iPAddressToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
            this.iPAddressToolStripMenuItem1.Text = "IP Address";
            this.iPAddressToolStripMenuItem1.Click += new System.EventHandler(this.iPAddressToolStripMenuItem1_Click);
            // 
            // toolFaceToolStripMenuItem
            // 
            this.toolFaceToolStripMenuItem.Name = "toolFaceToolStripMenuItem";
            this.toolFaceToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.toolFaceToolStripMenuItem.Text = "Legend";
            this.toolFaceToolStripMenuItem.Click += new System.EventHandler(this.legendToolStripMenuItem_Click);
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.registerToolStripMenuItem.Text = "Register";
            this.registerToolStripMenuItem.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // chatToolStripMenuItem
            // 
            this.chatToolStripMenuItem.Name = "chatToolStripMenuItem";
            this.chatToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.chatToolStripMenuItem.Text = "Chat";
            this.chatToolStripMenuItem.Click += new System.EventHandler(this.chatToolStripMenuItem_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.BackColor = System.Drawing.Color.DarkGray;
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelDetect,
            this.toolStripStatusLabelMP,
            this.toolStripStatusLabelWITS,
            this.toolStripStatusLabelServer,
            this.toolStripStatusMSA,
            this.toolStripStatus560R});
            this.statusStripMain.Location = new System.Drawing.Point(0, 770);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.statusStripMain.Size = new System.Drawing.Size(1608, 23);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 3;
            this.statusStripMain.Text = "statusStripDetectCom";
            // 
            // toolStripStatusLabelDetect
            // 
            this.toolStripStatusLabelDetect.BackColor = System.Drawing.Color.White;
            this.toolStripStatusLabelDetect.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelDetect.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatusLabelDetect.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelDetect.Name = "toolStripStatusLabelDetect";
            this.toolStripStatusLabelDetect.Size = new System.Drawing.Size(77, 18);
            this.toolStripStatusLabelDetect.Text = "Detect EM";
            // 
            // toolStripStatusLabelMP
            // 
            this.toolStripStatusLabelMP.BackColor = System.Drawing.Color.White;
            this.toolStripStatusLabelMP.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelMP.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatusLabelMP.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelMP.Name = "toolStripStatusLabelMP";
            this.toolStripStatusLabelMP.Size = new System.Drawing.Size(78, 18);
            this.toolStripStatusLabelMP.Text = "Detect MP";
            // 
            // toolStripStatusLabelWITS
            // 
            this.toolStripStatusLabelWITS.BackColor = System.Drawing.Color.White;
            this.toolStripStatusLabelWITS.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelWITS.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatusLabelWITS.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelWITS.Name = "toolStripStatusLabelWITS";
            this.toolStripStatusLabelWITS.Size = new System.Drawing.Size(48, 18);
            this.toolStripStatusLabelWITS.Text = "WITS";
            // 
            // toolStripStatusLabelServer
            // 
            this.toolStripStatusLabelServer.BackColor = System.Drawing.Color.White;
            this.toolStripStatusLabelServer.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelServer.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatusLabelServer.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelServer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabelServer.Name = "toolStripStatusLabelServer";
            this.toolStripStatusLabelServer.Size = new System.Drawing.Size(56, 18);
            this.toolStripStatusLabelServer.Text = "Clients";
            this.toolStripStatusLabelServer.Click += new System.EventHandler(this.toolStripStatusLabelServer_Click);
            // 
            // toolStripStatusMSA
            // 
            this.toolStripStatusMSA.BackColor = System.Drawing.Color.White;
            this.toolStripStatusMSA.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusMSA.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatusMSA.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusMSA.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusMSA.Name = "toolStripStatusMSA";
            this.toolStripStatusMSA.Size = new System.Drawing.Size(71, 18);
            this.toolStripStatusMSA.Text = "MSA OFF";
            // 
            // toolStripStatus560R
            // 
            this.toolStripStatus560R.BackColor = System.Drawing.Color.White;
            this.toolStripStatus560R.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatus560R.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatus560R.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatus560R.Name = "toolStripStatus560R";
            this.toolStripStatus560R.Size = new System.Drawing.Size(47, 18);
            this.toolStripStatus560R.Text = "560R";
            this.toolStripStatus560R.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // notifyIconServerMsg
            // 
            this.notifyIconServerMsg.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIconServerMsg.BalloonTipText = "Hello World!";
            this.notifyIconServerMsg.BalloonTipTitle = "Greetings";
            this.notifyIconServerMsg.ContextMenuStrip = this.contextMenuStripTrayIcon;
            this.notifyIconServerMsg.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconServerMsg.Icon")));
            this.notifyIconServerMsg.Text = "Notifications from Display Server";
            this.notifyIconServerMsg.Visible = true;
            // 
            // contextMenuStripTrayIcon
            // 
            this.contextMenuStripTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemViewServerMsgs});
            this.contextMenuStripTrayIcon.Name = "contextMenuStripTrayIcon";
            this.contextMenuStripTrayIcon.Size = new System.Drawing.Size(189, 26);
            // 
            // toolStripMenuItemViewServerMsgs
            // 
            this.toolStripMenuItemViewServerMsgs.Name = "toolStripMenuItemViewServerMsgs";
            this.toolStripMenuItemViewServerMsgs.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuItemViewServerMsgs.Text = "View Server Messages";
            this.toolStripMenuItemViewServerMsgs.Click += new System.EventHandler(this.toolStripMenuItemViewServerMsgs_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1608, 793);
            this.Controls.Add(this.menuStripFile);
            this.Controls.Add(this.statusStripMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripFile;
            this.Name = "Main";
            this.Padding = new System.Windows.Forms.Padding(0, 23, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Applied Physics Systems - Display ";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.menuStripFile.ResumeLayout(false);
            this.menuStripFile.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.contextMenuStripTrayIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripFile;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolFaceToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDetect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelWITS;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lASToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMP;
        private System.Windows.Forms.ToolStripMenuItem PressureTransducerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wITSToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem registerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayAsServerOrClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem communicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPAddressToolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelServer;
        private System.Windows.Forms.NotifyIcon notifyIconServerMsg;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTrayIcon;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewServerMsgs;
        private System.Windows.Forms.ToolStripMenuItem multiStationAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surveyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem correctedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rawToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMSA;
        private System.Windows.Forms.ToolStripMenuItem chatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surveyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mSAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rawToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem jobToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem realtimeDetectLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receiverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sNRPlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolFaceToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem qualifiersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gammaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem last20ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surveyHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pumpPressurePowerTemperatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus560R;
        private System.Windows.Forms.ToolStripMenuItem rRemoteRxToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem plotEMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fFTEMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fFTToolStripMenuItem;
    }
}