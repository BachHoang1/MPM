// author: hoan chau
// purpose: show historical 

using MPM.Data;
using MPM.DataAcquisition.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MPM.GUI
{
    public partial class FormSNRPlot : ToolWindow
    {
        private List<CLogDataLayer.PLOT_DATA> m_lstData;
        private DbConnection m_dbConn;                

        private bool m_bUnLoad;
        private Thread queryDataThread;
        private delegate void PlotDataDelegate();
        private PlotDataDelegate m_PlotDataDelegate;

        public FormSNRPlot(ref DbConnection dbConn_)
        {
            InitializeComponent();
            m_dbConn = dbConn_;            
            m_bUnLoad = false;
            m_lstData = new List<CLogDataLayer.PLOT_DATA>();
        }                        

        public void Unload()
        {
            m_bUnLoad = true;
        }
        
        private void FormSNRPlot_Load(object sender, EventArgs e)
        {
            SetupChart();

            GetData();

            PlotData();            
            
            m_PlotDataDelegate += new PlotDataDelegate(PlotData);

            queryDataThread = new Thread(QueryData);
            queryDataThread.Start();
        }

        private void QueryData()
        {
            int iCount = 0;
            while (true)
            {                
                try
                {                                                                                
                    Thread.Sleep(2500);

                    if (m_bUnLoad)
                        break;

                    iCount++;
                    if (iCount > 2)
                    {
                        iCount = 0;  // reset counter
                        GetData();
                        // Invoke method must be used to interact with the chart
                        // control on the form!
                        chartSNR.Invoke(m_PlotDataDelegate);
                    }                    
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Print(e.Message);
                }
            }
        }

        private void GetData()
        {
            CLogDataLayer m_LogDataLayer = new CLogDataLayer(ref m_dbConn);
            // = new CLogDataLayer(ref m_dbConn);
            List<CLogDataLayer.PLOT_DATA> lstSigStrength = m_LogDataLayer.GetLast(30, (int)Command.COMMAND_SIG_STRENGTH);
            List<CLogDataLayer.PLOT_DATA> lstNoise = m_LogDataLayer.GetLast(30, (int)Command.COMMAND_RESP_NOISE);
            if (lstSigStrength.Count == 0 || lstNoise.Count == 0)
            {                
                return;
            }

            if (lstSigStrength.Count == lstNoise.Count) // everything lines up
            {
                // compute the SNR
                List<CLogDataLayer.PLOT_DATA> lstSNR = new List<CLogDataLayer.PLOT_DATA>();
                CLogDataLayer.PLOT_DATA rec = new CLogDataLayer.PLOT_DATA();
                for (int i = 0; i < lstSigStrength.Count; i++)
                {
                    rec.sDateTime = lstSigStrength[i].sDateTime;
                    float fNoise = System.Convert.ToSingle(lstNoise[i].fValue);
                    if (Math.Abs(fNoise) > 0.000001)  // no division by near zero
                    {
                        rec.fValue = System.Convert.ToSingle(lstSigStrength[i].fValue) / fNoise;
                        lstSNR.Add(rec);
                    }
                }

                m_lstData = lstSNR;

            }
            else  // try to align the data
            {
                List<CLogDataLayer.PLOT_DATA> lstSNR = new List<CLogDataLayer.PLOT_DATA>();
                CLogDataLayer.PLOT_DATA rec = new CLogDataLayer.PLOT_DATA();
                for (int i = 0; i < lstSigStrength.Count; i++)
                {
                    for (int j = 0; j < lstNoise.Count; j++)
                    {
                        if (lstSigStrength[i].iDate == lstNoise[j].iDate)
                        {
                            if (Math.Abs(lstSigStrength[i].iTime - lstNoise[j].iTime) <= 2)  // found match because it's within 2 seconds
                            {
                                // compute the SNR
                                rec.sDateTime = lstSigStrength[i].sDateTime;
                                float fNoise = System.Convert.ToSingle(lstNoise[j].fValue);
                                if (Math.Abs(fNoise) > 0.000001)  // no division by near zero
                                {
                                    rec.fValue = System.Convert.ToSingle(lstSigStrength[i].fValue) / fNoise;
                                    lstSNR.Add(rec);
                                }
                                break;
                            }
                        }
                    }
                }

                m_lstData = lstSNR;
            }
        }

        private void PlotData()
        {
            chartSNR.Series[0].Points.Clear();
            
            for (int i = 0; i < m_lstData.Count; i++)
            {
                DateTime dt = System.Convert.ToDateTime(m_lstData[i].sDateTime);
                float f = m_lstData[i].fValue;
                chartSNR.Series[0].Points.AddXY(dt, f);
            }
        }

        private void SetupChart()
        {
            // allow user to select areas for zooming
            chartSNR.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartSNR.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;

            chartSNR.ChartAreas[0].CursorY.IsUserEnabled = true;
            chartSNR.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            chartSNR.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chartSNR.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

            chartSNR.ChartAreas[0].CursorX.AutoScroll = true;
            chartSNR.ChartAreas[0].CursorY.AutoScroll = true;

            chartSNR.ChartAreas[0].CursorX.Interval = 0;
            chartSNR.ChartAreas[0].CursorY.Interval = 0;

            chartSNR.ChartAreas[0].AxisX.ScaleView.SmallScrollSizeType = DateTimeIntervalType.Milliseconds;
            chartSNR.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 0.001D;
        }
    }
}
