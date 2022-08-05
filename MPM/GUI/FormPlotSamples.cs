// author: hoan chau
// purpose: display mud pulse raw pressure samples

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using MPM.Data;
using MPM.Utilities;
using MPM.DataAcquisition.Helpers;
using MPM.DataAcquisition;

namespace MPM.GUI
{
    public partial class FormPlotSamples : ToolWindow
    {
        //mud pulse sample time based on 225 samples per half second        
        private const int DECIMATION_NORMAL = 4;  // 25%
        private const int MAX_CHART_POINTS = CDetect.RAW_MP_SAMPLE_SIZE / DECIMATION_NORMAL * 2 * PLOT_TIME_RANGE_MP;

        public const int LOW_RES_PLOT_HEIGHT_TO_REMOVE = 15;
        
        private const int PLOT_TIME_RANGE_MP = 30;  // seconds

        // low pass filter factor
        public const float LPF_BETA_DEFAULT = 0.02f;  // 0<ß<1

        private const int PLOT_UPDATE_INTERVAL = 7;  // number of sets of samples received before chart is re-painted with new data

        

        //private PACKET_REC m_packetRecMudPulse;
        //private PACKET_REC m_packetRecEM;

        private DateTime dtMinValue, dtMaxValue;
        //private Thread addDataRunner;

        // Thread Add Data delegate
        public delegate void AddDataDelegate();
        public AddDataDelegate addDataDel;

        private CDetectDataLayer m_DataLayer;
        private CPressureTransducer m_PressureTransducer;

        // position relative to other windows when it was docked
        //private Point m_ptDock;
        //private Size m_szDock;
        private float m_fMPPlotMaxValue = float.MinValue;
        private float m_fMPPlotMinValue = float.MaxValue;        
        //private int m_iAutoZoomInCounter;

        //private bool m_bIsLowRes = false;

        private float m_fMPSmoothData = 0.0f;        
        private float m_fLPFBeta = LPF_BETA_DEFAULT;

        private int m_iUpdateCount;

        private bool m_bZoomingEnabled;

        CCommonTypes.UNIT_SET m_iUnitSet;

        CCommonTypes.RECEIVER_TYPE m_ReceiverType;

        private CDPointLookupTable m_DPointTable;

        // for testing fake data            
        private void AddDataThreadLoop()
        {
            while (true)
            {
                try
                {
                    // Invoke method must be used to interact with the chart
                    // control on the form!
                    chart1.Invoke(addDataDel);

                    // Thread is inactive for 200ms
                    Thread.Sleep(200);
                }

                catch (Exception e)
                {
                    System.Diagnostics.Debug.Print(e.Message);
                    // Thread is aborted
                }
            }
        }

        public void AddData()
        {
            DateTime timeStamp = DateTime.Now;

            foreach (Series ptSeries in chart1.Series)
            {
                AddNewPoint(timeStamp, ptSeries);
            }
        }

        public FormPlotSamples(ref CDPointLookupTable DPointTable_)
        {
            InitializeComponent();

            m_DPointTable = DPointTable_;

            // for testing fake data            
            //ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            //addDataRunner = new Thread(addDataThreadStart);
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();

            //string s = WidgetInfoLookupTbl.GetValue("FormReceiverSetting", "comboBoxReceiverType");
            //m_ReceiverType = (CCommonTypes.RECEIVER_TYPE)System.Convert.ToInt32(s);

            //m_packetRecMudPulse.dtLast = new DateTime(1970, 1, 1, 1, 1, 1);
            //m_packetRecMudPulse.iCnt = 0;

            //m_packetRecEM.dtLast = new DateTime(1970, 1, 1, 1, 1, 1);
            //m_packetRecEM.iCnt = 0;

        //addDataDel += new AddDataDelegate(AddData);
        // Predefine the viewing area of the chart

            dtMinValue = DateTime.Now;
            dtMaxValue = dtMinValue.AddSeconds(30);
            numericUpDownBetaFactor.Value = (int)(m_fLPFBeta * 1000);
            //m_iAutoZoomInCounter = 0;  

            m_iUpdateCount = 0;
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void startTrending_Click()
        {            
            // Predefine the viewing area of the chart
            dtMinValue = DateTime.Now;
            dtMaxValue = dtMinValue.AddSeconds(30);

            chart1.ChartAreas[0].AxisX.Minimum = dtMinValue.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = dtMaxValue.ToOADate();
            
            // start worker threads.
            //if (addDataRunner.IsAlive == true)
            //{
                
            //}
            //else
            //{
            //    addDataRunner.Start();
            //}
        }
        
        
        private void FormPlotSamples_FormClosed(object sender, FormClosedEventArgs e)
        {
            //addDataRunner.Abort();
        }

        private void FormPlotSamples_Load(object sender, EventArgs e)
        {
            //this.startTrending_Click();
            //if (m_bIsLowRes)
            //{
            //    chart1.Height -= LOW_RES_PLOT_HEIGHT_TO_REMOVE;
            //}
                // set up the ability to zoom in/out for the user
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;

            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;            

            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = true;

            chart1.ChartAreas[0].CursorX.Interval = 0;
            chart1.ChartAreas[0].CursorY.Interval = 0;

            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSizeType = DateTimeIntervalType.Milliseconds;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 0.001D;
            

            chart1.Series.SuspendUpdates();
            chart1.ChartAreas.SuspendUpdates();
            m_bZoomingEnabled = false;            
        }

        public void AddNewPoint(DateTime timeStamp, System.Windows.Forms.DataVisualization.Charting.Series ptSeries)
        {
            Random rand = new Random();
            // Add new data point to its series.
            //if (chart1.InvokeRequired)
            {
                ptSeries.Points.AddXY(timeStamp.ToOADate(), rand.Next(500, 2000));

                // remove all points from the source series older than 20 seconds.
                double removeBefore = timeStamp.AddSeconds((double)(20) * (-1)).ToOADate();

                //remove oldest values to maintain a constant number of data points
                while (ptSeries.Points[0].XValue < removeBefore)
                {
                    ptSeries.Points.RemoveAt(0);
                }

                chart1.ChartAreas[0].AxisX.Minimum = ptSeries.Points[0].XValue;
                chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddSeconds(30).ToOADate();

                chart1.Invalidate();
            }            
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.AcquiredMPSamples += new MPSampleAcquiredEventHandler(GetSamples);
        }

        private void undockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            //if (this.unlockToolStripMenuItem.Text == "UnDock")
            //{
            //    // save the position of the form
            //    m_ptDock = this.Location;
            //    m_szDock = this.Size;

            //    this.FormBorderStyle = FormBorderStyle.Sizable;
            //    this.ControlBox = false;                
            //    this.unlockToolStripMenuItem.Text = "Dock";                                
            //}
            //else
            //{                
            //    this.FormBorderStyle = FormBorderStyle.None;

            //    // restore to former self
            //    this.Location = m_ptDock;
            //    this.Size = m_szDock;
            //    this.unlockToolStripMenuItem.Text = "UnDock";
            //}
        }
        
        public FormBorderStyle GetDock()
        {
            return this.FormBorderStyle;
        }

        private void hideRawSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hideRawSamplesToolStripMenuItem.Text == "Hide Raw Samples")
            {
                chart1.Series[0].Enabled = false;
                chart1.Series[2].Enabled = false;
                hideRawSamplesToolStripMenuItem.Text = "Show Raw Samples";
            }
            else
            {
                chart1.Series[0].Enabled = true;
                chart1.Series[2].Enabled = true;
                hideRawSamplesToolStripMenuItem.Text = "Hide Raw Samples";
            }
            
        }       

        public void SetPressureTransducer(ref CPressureTransducer pressureTransducer_)
        {
            m_PressureTransducer = pressureTransducer_;
        }

        public void SetReceiverType(CCommonTypes.RECEIVER_TYPE val_)
        {
            m_ReceiverType = val_;
        }

        public void Plot(CEventRawMPSamples evRaw_)
        {
            if (!this.Visible)
                return;

            if (m_bZoomingEnabled)
                return;

            try
            {
                try
                {
                    float fLastValue = evRaw_.m_shArrSample[0];
                    float fCurrentVal;
                    float fLowPassVal;
                    for (int i = 0; i < evRaw_.m_shArrSample.Count(); i++)
                    {
                        fCurrentVal = evRaw_.m_shArrSample[i] * m_PressureTransducer.GetTransducerScale() * m_PressureTransducer.GetTransducerGain() + m_PressureTransducer.GetPressureOffset();

                        if (fCurrentVal > m_fMPPlotMaxValue)
                            m_fMPPlotMaxValue = fCurrentVal;
                        else if (fCurrentVal < m_fMPPlotMinValue)
                            m_fMPPlotMinValue = fCurrentVal;

                        // inline low pass filter to speed up code
                        fLowPassVal = m_fMPSmoothData = m_fMPSmoothData - (m_fLPFBeta * (m_fMPSmoothData - fCurrentVal));
                        if (i % DECIMATION_NORMAL == 0)
                        {
                            chart1.Series[0].Points.AddY(fCurrentVal);
                            chart1.Series[1].Points.AddY(fLowPassVal);
                        }

                        fLastValue = fCurrentVal;
                    }

                    // Keep a constant number of points by removing them from the left                    
                    while (chart1.Series[0].Points.Count > MAX_CHART_POINTS)
                    {
                        // Remove data points on the left side
                        while (chart1.Series[0].Points.Count > MAX_CHART_POINTS)
                        {
                            chart1.Series[0].Points.RemoveAt(0);
                            chart1.Series[1].Points.RemoveAt(0);
                        }

                        // Adjust X axis scale
                        chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
                        chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Maximum;
                    }

                    // automatically zoom out on Y-axis
                    if (m_fMPPlotMaxValue > chart1.ChartAreas[0].AxisY.Maximum)
                        chart1.ChartAreas[0].AxisY.Maximum = m_fMPPlotMaxValue;

                    if (m_fMPPlotMinValue < chart1.ChartAreas[0].AxisY.Minimum)
                        chart1.ChartAreas[0].AxisY.Minimum = m_fMPPlotMinValue;

                    // automatically zoom in on Y-axis                                       
                    DataPoint dpMax = chart1.Series[0].Points.FindMaxByValue("Y1", 0);
                    DataPoint dpMin = chart1.Series[0].Points.FindMinByValue("Y1", 0);
                    if (dpMax.YValues[0] > dpMin.YValues[0])
                    {
                        if (dpMax.YValues[0] < m_fMPPlotMaxValue)
                            chart1.ChartAreas[0].AxisY.Maximum = dpMax.YValues[0];

                        if (dpMin.YValues[0] > m_fMPPlotMinValue)
                            chart1.ChartAreas[0].AxisY.Minimum = dpMin.YValues[0];
                    }

                    chart1.Series.Invalidate();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    System.Diagnostics.Debug.Write("Plot MP Data Error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Plot MP Error: " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        public void GetSamples(object sender, CEventRawMPSamples evRaw_)
        {
            if (!this.Visible)
                return;            

            if (m_bZoomingEnabled)
                return;

            // half a second per set of samples
            double dblMilliseconds = 500.0 / (double)evRaw_.m_shArrSample.Count();
            Console.WriteLine(DateTime.Now);
            try
            {                
                try
                {                        
                    DateTime dt = evRaw_.m_TimeOfSamples;
                    float fLastValue = evRaw_.m_shArrSample[0];
                    float fCurrentVal;
                    float fLowPassVal;
                       
                    for (int i = 0; i < evRaw_.m_shArrSample.Count(); i++)
                    {
                        //fCurrentVal = m_iUnitSet != CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC ? evRaw_.m_shArrSample[i] * 0.145038f : evRaw_.m_shArrSample[i]; // convert kpa to psi
                        fCurrentVal = evRaw_.m_shArrSample[i] * m_PressureTransducer.GetTransducerScale() * m_PressureTransducer.GetTransducerGain() + m_PressureTransducer.GetPressureOffset();
                        if (fCurrentVal > m_fMPPlotMaxValue)
                            m_fMPPlotMaxValue = fCurrentVal;
                        else if (fCurrentVal < m_fMPPlotMinValue)
                            m_fMPPlotMinValue = fCurrentVal;                            

                            
                        //fLowPassVal = ApplyLowPassFilter(fCurrentVal);
                        // inline low pass filter to speed up code
                        fLowPassVal = m_fMPSmoothData = m_fMPSmoothData - (m_fLPFBeta * (m_fMPSmoothData - fCurrentVal));
                        if (i % 4 == 0)  // cut down to a quarter
                        {
                            chart1.Series[0].Points.AddXY(dt, fCurrentVal);
                            chart1.Series[1].Points.AddXY(dt, fLowPassVal);
                        }
                                

                        dt = dt.AddMilliseconds(dblMilliseconds);                            
                        fLastValue = fCurrentVal;
                    }                        

                    double removeBefore = dt.AddSeconds(-PLOT_TIME_RANGE_MP).ToOADate();  // subtract seconds

                    //remove oldest values to maintain a constant number of data points
                    while (chart1.Series[0].Points[0].XValue < removeBefore)
                    {
                        chart1.Series[0].Points.RemoveAt(0);
                        chart1.Series[1].Points.RemoveAt(0);
                    }

                    chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
                    chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(chart1.Series[0].Points[0].XValue).AddSeconds(PLOT_TIME_RANGE_MP).ToOADate();

                    // automatically zoom out on Y-axis
                    if (m_fMPPlotMaxValue > m_fMPPlotMinValue)
                    {
                        if (m_fMPPlotMaxValue > chart1.ChartAreas[0].AxisY.Maximum)
                            chart1.ChartAreas[0].AxisY.Maximum = m_fMPPlotMaxValue;

                        if (m_fMPPlotMinValue < chart1.ChartAreas[0].AxisY.Minimum)
                            chart1.ChartAreas[0].AxisY.Minimum = m_fMPPlotMinValue;
                    }


                    // automatically zoom in on Y-axis                                        
                    DataPoint dpMax = chart1.Series[0].Points.FindMaxByValue("Y1", 0);
                    DataPoint dpMin = chart1.Series[0].Points.FindMinByValue("Y1", 0);
                    if (dpMax.YValues[0] > dpMin.YValues[0])
                    {
                        if (dpMax.YValues[0] < m_fMPPlotMaxValue)
                            chart1.ChartAreas[0].AxisY.Maximum = dpMax.YValues[0];

                        if (dpMin.YValues[0] > m_fMPPlotMinValue)
                            chart1.ChartAreas[0].AxisY.Minimum = dpMin.YValues[0];
                    }

                    m_iUpdateCount++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                
                //*******************************************************************************
                

                // prevent overwhelming the cpu with too many graphical updates
                if (m_iUpdateCount == PLOT_UPDATE_INTERVAL)
                {
                    chart1.Series.ResumeUpdates();
                    chart1.Series.Invalidate();
                    chart1.Series.SuspendUpdates();

                    chart1.ChartAreas.ResumeUpdates();
                    chart1.ChartAreas.Invalidate();
                    chart1.ChartAreas.SuspendUpdates();
                    m_iUpdateCount = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("GetMPSamples Error: " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        

        private void numericUpDownBetaFactor_ValueChanged(object sender, EventArgs e)
        {
            m_fLPFBeta = (float)(numericUpDownBetaFactor.Value) / 1000.0f;
        }

        private void enableZoomingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableZoomingToolStripMenuItem.Text == "Enable Zooming")
            {
                m_bZoomingEnabled = true;  // stop samples from being plotted

                // allow the user to draw rectangular selections for zooming since the plot algorithm might have landed
                // on a suspend upate index
                chart1.Series.ResumeUpdates();                
                chart1.ChartAreas.ResumeUpdates();
                
                enableZoomingToolStripMenuItem.Text = "Disable Zooming";
            }
            else
            {
                m_bZoomingEnabled = false;                                
                enableZoomingToolStripMenuItem.Text = "Enable Zooming";
            }
        }

        private void FormPlotSamples_Resize(object sender, EventArgs e)
        {
            RelocateLowPassFilterWidget();   
        }

        private void RelocateLowPassFilterWidget()
        {
            Point pt = this.Location;            
            pt.Y = 15;
            pt.X = this.Width - 120 - 62;
            numericUpDownBetaFactor.Location = pt;

            pt.Y += 6;
            pt.X += numericUpDownBetaFactor.Width;
            labelLPFactor.Location = pt;
        }

        private void FormPlotSamples_Shown(object sender, EventArgs e)
        {
            RelocateLowPassFilterWidget();
            // allow the user to draw rectangular selections for zooming since the plot algorithm might have landed
            // on a suspend upate index
            chart1.Series.ResumeUpdates();
            chart1.ChartAreas.ResumeUpdates();
        }

        private void hideLowPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hideLowPassToolStripMenuItem.Text == "Hide Low Pass")
            {
                chart1.Series[1].Enabled = false;
                chart1.Series[3].Enabled = false;
                hideLowPassToolStripMenuItem.Text = "Show Low Pass";
            }
            else
            {
                chart1.Series[1].Enabled = true;
                chart1.Series[3].Enabled = true;
                hideLowPassToolStripMenuItem.Text = "Hide Low Pass";
            }
        }

        private void FormPlotSamples_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private string GetNativePressureUnit()
        {
            CDPointLookupTable.DPointInfo dpi = m_DPointTable.Get((int)Command.COMMAND_RESP_A_PRESSURE);  // use annular pressure as the reference
            if (dpi.iMessageCode == -1)
                dpi.sUnits = "psi";

            return dpi.sUnits;
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET iVal_)
        {
            m_iUnitSet = iVal_;
            CUnitPressure pressureUnit = new CUnitPressure();

            if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)            
                chart1.ChartAreas[0].AxisY.Title = pressureUnit.GetImperialUnitDesc();            
            else if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)            
                chart1.ChartAreas[0].AxisY.Title = pressureUnit.GetMetricUnitDesc();            
            else if (iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT ||
                     iVal_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)  // get what's in the table
            {
                string sPressureUnit = GetNativePressureUnit();
                chart1.ChartAreas[0].AxisY.Title = sPressureUnit;
            }                            
        }
    }
}
