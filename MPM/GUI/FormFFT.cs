using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSPLib;
using MPM.Data;
using System.Numerics;

namespace MPM.GUI
{
    public partial class FormFFT : ToolWindow
    {
        const int SAMPLES = 512;

        private string mTitle;
        private float m_fScale;

        private CDetectDataLayer m_DataLayer;

        public FormFFT(string mainTitle)
        {
            InitializeComponent();

            mTitle = mainTitle;            
        }

        private void Plot_Load(object sender, EventArgs e)
        {
            // Add the titles
            chart1.Titles["Title"].Text = mTitle;
            this.Text = mTitle;
            chart1.Titles["AxisY"].Text = "";            

            // Enable zooming
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            comboBoxVoltScale.SelectedIndex = 0;
            m_fScale = System.Convert.ToSingle(comboBoxVoltScale.SelectedItem.ToString());
        }

        // Line chart
        public void PlotData(double[] yData, bool bIsMP_)
        {
            try
            {
                if (!Visible)
                    return;

                if (chart1 == null)
                    return;

                if (chart1.Series["MP"] == null)
                    return;
                                
                chart1.Series["MP"].Points.Clear();

                // Start X Data at zero! Not like the chart default of 1!
                double[] xData = DSP.Generate.LinSpace(0, yData.Length - 1, (UInt32)yData.Length);                
                chart1.Series["MP"].Points.DataBindXY(xData, yData);                
            }
            catch
            {

            }
        }
                

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.AcquiredMPSamples += new MPSampleAcquiredEventHandler(GetMPSamples);
        }

        public void PlotData(CEventRawMPSamples evRaw_)
        {
            bool bIsMP = false;
            if (evRaw_.m_iTelemetryType == 0)
                bIsMP = true;

            if (this.Visible)
                CalculateFFT((uint)evRaw_.m_shArrSample.Count(), evRaw_.m_shArrSample, bIsMP);
        }

        public void GetMPSamples(object sender, CEventRawMPSamples evRaw_)
        {
            bool bIsMP = false;
            if (evRaw_.m_iTelemetryType == 0)
                bIsMP = true;

            if (this.Visible)
                CalculateFFT((uint)evRaw_.m_shArrSample.Count(), evRaw_.m_shArrSample, bIsMP);
        }

        void CalculateFFT(UInt32 iSamples_, short[] shSamples_, bool bIsMP_)
        {
            UInt32 N = iSamples_;
            UInt32 zeros = SAMPLES - iSamples_;            

            string selectedWindowName = "None";
            DSPLib.DSP.Window.Type windowToApply = (DSPLib.DSP.Window.Type)Enum.Parse(typeof(DSPLib.DSP.Window.Type), selectedWindowName);
            
            double[] timeSeries = new double[iSamples_];
            for (int i = 0; i < iSamples_; i++)
            {
                timeSeries[i] = (double)shSamples_[i];
                Console.Write(timeSeries[i].ToString() + ",");
            }                        

            // Apply window to the time series data
            double[] wc = DSP.Window.Coefficients(windowToApply, N);

            double windowScaleFactor = DSP.Window.ScaleFactor.Signal(wc);
            double[] windowedTimeSeries = DSP.Math.Multiply(timeSeries, wc);            

            // Instantiate & Initialize the FFT class
            DSPLib.FFT fft = new DSPLib.FFT();
            fft.Initialize(N, zeros);            

            // Perform a DFT
            Complex[] cpxResult = fft.Execute(windowedTimeSeries);            

            // Convert the complex result to a scalar magnitude 
            double[] magResult = DSP.ConvertComplex.ToMagnitude(cpxResult);
            magResult = DSP.Math.Multiply(magResult, windowScaleFactor);

            for (int i = 0; i < magResult.Length; i++)
            {
                if (m_fScale < 0)
                    magResult[i] = Math.Log10(magResult[i]) * 10;
                else
                    magResult[i] /= m_fScale * 100;
            }
            PlotData(magResult, bIsMP_);            
        }

        private void FormFFT_FormClosing(object sender, FormClosingEventArgs e)
        {            
            this.Hide();
            e.Cancel = true;
        }

        private void comboBoxVoltScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxVoltScale.SelectedIndex == comboBoxVoltScale.Items.Count - 1)
                m_fScale = -1;
            else
                m_fScale = System.Convert.ToSingle(comboBoxVoltScale.SelectedItem.ToString());
        }
    }
}



