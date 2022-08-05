// author: hoan chau
// purpose: shows the driller the direction, inclination in which they are drilling

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using MPM.Data;
using System.Drawing.Drawing2D;

namespace MPM.GUI
{
    public partial class FormToolFace : ToolWindow
    {
        static float fFakeAZM = 0.0f;
        static float fFakeINC = 0.0f;

        private const int TOOL_TIP_TIMER = 3;  // seconds
        private const int MAX_POINTS = 5;
        private enum CIRCLE_DIAMETER { ONE = 110, TWO = 150, THREE = 225, FOUR = 300, FIVE = 375, SIX = 450 };

        private Color m_clrOfCircle = Color.FromArgb(0x3C, 0x98, 0xC4);
        private Color m_clrOfPlots = Color.Orange;
        private Color m_clrOfLines = Color.White;

        private Pen m_penForLine;
        private Pen m_penForCircle;
        private Pen m_penForCircleThin;

        private Brush m_brushSolidCircle;
        private Brush m_brushPlots;
        private Brush m_brushPlots2;

        private bool m_bIsGTF;

        Font m_fontCenterTF = new Font("Verdana", 23, FontStyle.Bold);
        Font m_fontMediumCenterTF = new Font("Verdana", 20, FontStyle.Bold);
        Font m_fontSmallCenterTF = new Font("Verdana", 10, FontStyle.Bold);
        Font m_fontGeorgiaCenterTF = new Font("Georgia", 27, FontStyle.Bold);
        Brush m_brushCenterTF = new SolidBrush(Color.Black);
        Brush m_brushInvalidLicense = new SolidBrush(Color.White);

        private bool m_bValidLicense;

        private CWidgetInfoLookupTable m_WidgetLookupInfo;

        struct TFPlot
        {
            public float fVal { get; set; }
            public DateTime dtVal { get; set; }
            public CCommonTypes.TELEMETRY_TYPE iTechnology { get; set; }

            public TFPlot(float fVal_, DateTime dtVal_, CCommonTypes.TELEMETRY_TYPE iTechnology_)
            {
                this.fVal = fVal_;
                this.dtVal = dtVal_;
                this.iTechnology = iTechnology_;
            }
        }

        private TFPlot[] m_tfPlotArrData;  // should be in a separate class

        private PointF[] m_ptArrPosition;
        private float[] m_fArrRadius;  // 
        private float[] m_fArrPlotDiameter; //
        private bool m_bShowToolTip;
        ToolTip m_tt = new ToolTip();

        private CDetectDataLayer m_DataLayer;

        private Thread m_threadTestPlot;

        private bool m_bUnload;


        static void Worker(object obj)
        {
            FormToolFace param = (FormToolFace)obj;
            int iToolTipCounter = 0;
            //bool bFirstTime = false;
            int iCounterToRepaintCenter = 0;
            try
            {
                while (true)
                {
                    if (iCounterToRepaintCenter % 5 == 0)
                    {
                        iCounterToRepaintCenter = 0;
                        param.pictureBoxToolFace.Invalidate();
                    }
                    iCounterToRepaintCenter++;

                    //if (!bFirstTime)
                    //{
                    //    bFirstTime = true;
                    //    param.Redraw();
                    //}

                    if (param.m_bUnload)
                        break;
                    //2/25/22Thread.Sleep(1000);

                    Thread.Sleep(20);
                    if (param.m_bShowToolTip)
                        iToolTipCounter++;

                    if (iToolTipCounter > TOOL_TIP_TIMER)
                    {
                        param.m_bShowToolTip = false;
                        iToolTipCounter = 0;
                    }
                }
            }
            catch (ThreadAbortException abortException)
            {
                System.Diagnostics.Debug.WriteLine((string)abortException.ExceptionState);
            }
        }

        public FormToolFace(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            InitializeComponent();
            m_WidgetLookupInfo = widgetInfoLookup_;

            m_penForLine = new Pen(m_clrOfLines, 2);
            m_penForCircle = new Pen(m_clrOfCircle, 10);
            m_penForCircleThin = new Pen(m_clrOfCircle, 3);

            m_brushSolidCircle = new SolidBrush(Color.White);
            m_brushPlots = new SolidBrush(Color.Orange);
            m_brushPlots2 = new SolidBrush(Color.Fuchsia);

            m_bIsGTF = false;
            m_tfPlotArrData = new TFPlot[MAX_POINTS] { new TFPlot(-9999.0f, DateTime.Now, CCommonTypes.TELEMETRY_TYPE.TT_MP),
                                                       new TFPlot(-9999.0f, DateTime.Now, CCommonTypes.TELEMETRY_TYPE.TT_MP),
                                                       new TFPlot(-9999.0f, DateTime.Now, CCommonTypes.TELEMETRY_TYPE.TT_MP),
                                                       new TFPlot(-9999.0f, DateTime.Now, CCommonTypes.TELEMETRY_TYPE.TT_MP),
                                                       new TFPlot(-9999.0f, DateTime.Now, CCommonTypes.TELEMETRY_TYPE.TT_MP) };
            //MessageBox.Show(Screen.PrimaryScreen.Bounds.Height.ToString());
            //MessageBox.Show(Screen.PrimaryScreen.Bounds.Width.ToString());
            //m_fArrRadius = new float[MAX_POINTS] { (float)CIRCLE_DIAMETER.TWO, (float)CIRCLE_DIAMETER.THREE, (float)CIRCLE_DIAMETER.FOUR, (float)CIRCLE_DIAMETER.FIVE, (float) CIRCLE_DIAMETER.SIX };

            m_fArrRadius = new float[MAX_POINTS] { (float)CIRCLE_DIAMETER.TWO, (float)CIRCLE_DIAMETER.THREE, (float)CIRCLE_DIAMETER.FOUR, (float)CIRCLE_DIAMETER.FIVE, (float)CIRCLE_DIAMETER.SIX };
            m_fArrPlotDiameter = new float[MAX_POINTS] { 18.0f, 24.0f, 31.0f, 39.0f, 50.0f };//{ 18.0f, 22.0f, 26.0f, 30.0f, 34.0f };
            m_ptArrPosition = new PointF[MAX_POINTS];
            m_bShowToolTip = false;

            m_bValidLicense = false;



            // for tool tip
            m_threadTestPlot = new Thread(Worker);
            m_bUnload = false;
        }

        public void Unload()
        {
            m_bUnload = true;
        }

        public void SetLicense(bool bValid_)
        {
            m_bValidLicense = bValid_;
            Refresh();
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(ListChanged);
        }

        public void Correct(object sender, CEventCorrectedDPoint evInc, CEventCorrectedDPoint evAzm)
        {            
            userControlDPointINC.SetVal(evInc.m_fValue, CCommonTypes.ROUND_LAB_DESCRIPTOR);
            userControlDPointAZM.SetVal(evAzm.m_fValue, CCommonTypes.ROUND_LAB_DESCRIPTOR);
        }

        private void ListChanged(object sender, CEventDPoint e)
        {
            //System.Diagnostics.Debug.Print("FormToolFace ::This is called when the event fires.");                        
            try
            {
                if (userControlDPointINC.GetMessageCode() == e.m_ID)
                {
                    float fVal = Convert.ToSingle(e.m_sValue);
                    userControlDPointINC.SetVal(fVal, e.GetTelemetryType(), e.m_bIsParityError);                    
                    userControlDPointArcINC.SetVal(fVal, e.GetTelemetryType());
                }
                else if (userControlDPointAZM.GetMessageCode() == e.m_ID)
                {
                    float fVal = Convert.ToSingle(e.m_sValue);
                    userControlDPointAZM.SetVal(fVal, e.GetTelemetryType(), e.m_bIsParityError);
                    userControlDPointArcAZM.SetVal(fVal, e.GetTelemetryType());
                }
                else if (e.m_ID == 7) // toolface
                {
                    float fVal = Convert.ToSingle(e.m_sValue);
                    for (int i = 0; i < MAX_POINTS - 1; i++)
                    {
                        m_tfPlotArrData[i] = m_tfPlotArrData[i + 1];
                    }                    
                    m_tfPlotArrData[MAX_POINTS - 1].fVal = fVal;
                    m_tfPlotArrData[MAX_POINTS - 1].dtVal = e.m_DateTime;
                    m_tfPlotArrData[MAX_POINTS - 1].iTechnology = e.GetTechnology();
                                       
                    pictureBoxToolFace.Invalidate();
                }
                else if (e.m_ID == 6) // tf type
                {
                    if (e.m_sValue == "G")
                    {
                        m_bIsGTF = true;                        
                    }                        
                    else
                    {
                        m_bIsGTF = false;                        
                    }
                    pictureBoxToolFace.Invalidate();
                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Error: " + ex.Message);
            }                                                
        }

        private PointF GetCenter()
        {            
            // get the center of the screen
            Rectangle rect = pictureBoxToolFace.Bounds;
            PointF ptCenter = new PointF(rect.Width * 0.5f, rect.Height * 0.5f);
            return ptCenter;
        }

        private void DrawCenterTF(PaintEventArgs e, float fVal_)
        {
            if (fVal_ < -9998.0f)
                return;

            // get the center of the screen
            Rectangle rect = pictureBoxToolFace.Bounds;
            int x = rect.Width / 2;
            int y = rect.Height / 2;            
            
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //e.Graphics.TextContrast = 0;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            
            string sVal = "";
            if (m_bIsGTF)
            {
                e.Graphics.DrawString("G", m_fontGeorgiaCenterTF, m_brushCenterTF, x - 20.0f, y - 55.0f);
                if (fVal_ > 180.0f)
                {
                    fVal_ = (360.0f - fVal_);
                    sVal = String.Format("{0:F1}L", fVal_);
                }                
                else
                    sVal = String.Format("{0:F1}R", fVal_);
            }                
            else
            {
                e.Graphics.DrawString("M", m_fontGeorgiaCenterTF, m_brushCenterTF, x - 23.0f, y - 55.0f);
                sVal = String.Format("{0:F1}", fVal_);
            }
                                       
            if (fVal_ < 100 || !m_bIsGTF)  // use larger font
            {
                e.Graphics.DrawString(sVal, m_fontCenterTF, m_brushCenterTF, x - 55.0f, y - 20.0f);
                e.Graphics.DrawString(m_tfPlotArrData[MAX_POINTS - 1].dtVal.ToString("HH:mm:ss"), m_fontSmallCenterTF, m_brushCenterTF, x - 32.0f, y + 12.0f);
                TimeSpan ts = DateTime.Now - m_tfPlotArrData[MAX_POINTS - 1].dtVal;
                string sTime = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
                e.Graphics.DrawString(sTime, m_fontSmallCenterTF, m_brushCenterTF, x - 32.0f, y + 26.0f);
            }                
            else  // use smaller font
            {
                e.Graphics.DrawString(sVal, m_fontMediumCenterTF, m_brushCenterTF, x - 58.0f, y - 15.0f);
                e.Graphics.DrawString(m_tfPlotArrData[MAX_POINTS - 1].dtVal.ToString("HH:mm:ss"), m_fontSmallCenterTF, m_brushCenterTF, x - 32.0f, y + 12.0f);
                TimeSpan ts = DateTime.Now - m_tfPlotArrData[MAX_POINTS - 1].dtVal;
                string sTime = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
                e.Graphics.DrawString(sTime, m_fontSmallCenterTF, m_brushCenterTF, x - 32.0f, y + 26.0f);
            }
                
        }

        private void DrawInvalid(PaintEventArgs e)
        {
            Rectangle rect = pictureBoxToolFace.Bounds;
            int x = rect.Width / 2;
            int y = rect.Height / 2;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //e.Graphics.TextContrast = 0;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.DrawString("UNLICENSED USAGE.", m_fontMediumCenterTF, m_brushInvalidLicense, x - 150, y);
            e.Graphics.DrawString("PLEASE REGISTER WITH APS.", m_fontMediumCenterTF, m_brushInvalidLicense, x - 225, y + 30);
        }

        private void PositionDirectionLabels(bool bIsGTF, float fFactor_)
        {
            if (bIsGTF)
            {
                labelUp.Text = "0 Up";
                labelRight.Text = "90R";
                labelDown.Text = "180 Dn";
                labelLeft.Text = "90L";                
            }
            else
            {
                labelUp.Text = "0 North";
                labelRight.Text = "90";
                labelDown.Text = "180";
                labelLeft.Text = "270";                
            }
            PointF ptCenter = GetCenter();
                                    
            float fSixthCircleDiameter = (float)CIRCLE_DIAMETER.SIX * fFactor_;            
            
            labelUp.Location = new Point((int)(ptCenter.X - labelUp.Width * 0.5f), (int)(ptCenter.Y - fSixthCircleDiameter * 0.5f - labelUp.Height * 2.2f));
            labelUp.BringToFront();

            labelDown.Location = new Point((int)(ptCenter.X - labelDown.Width * 0.5f), (int)(ptCenter.Y + fSixthCircleDiameter * 0.5f + labelDown.Height * 1.5f));
            labelDown.BringToFront();

            float fLabelLeftFactor = 1.75f;
            float fLabelRightFactor = 0.9f;
            float fScale = GetAZMINCScaleFactor();
            if (fScale < 1.0f)
            {
                fLabelLeftFactor = 1.2f;
                fLabelRightFactor = 0.35f;
            }
                

            labelLeft.Location = new Point((int)(ptCenter.X - fSixthCircleDiameter * 0.5f - labelLeft.Width * fLabelLeftFactor), (int)ptCenter.Y - (int)(labelLeft.Height * 0.5f));
            labelLeft.BringToFront();

            labelRight.Location = new Point((int)(ptCenter.X + fSixthCircleDiameter * 0.5f + labelRight.Width * fLabelRightFactor), (int)ptCenter.Y - (int)(labelRight.Height * 0.5f));
            labelRight.BringToFront();            
        }

        private void PositionAZMWidgets(PointF ptCenter, float fScaleWidthFactor)
        {
            //userControlDPointAZM.Width = (int)(userControlDPointAZM.Width * fScaleWidthFactor);            
            //userControlDPointAZM.Width = 121;
            // keep the vertical position static
            // vary the distance between the circle and the dpoint with a minimum and a maximum           
            int iOffset = ClientSize.Width - (int)(userControlDPointArcAZM.ClientSize.Width);
            //userControlDPointArcAZM.Location = new Point(iOffset, userControlDPointArcAZM.Location.Y);
            //userControlDPointAZM.Location = new Point(iOffset - userControlDPointAZM.Width, userControlDPointAZM.Location.Y);

            userControlDPointArcAZM.Location = new Point(pictureBoxToolFace.Right - userControlDPointArcAZM.Width - userControlDPointArcAZM.Width / 2, userControlDPointArcAZM.Top);

            float fSixthCircleDiameter = (float)CIRCLE_DIAMETER.SIX * fScaleWidthFactor;
            userControlDPointAZM.Location = new Point((int)(ptCenter.X + fSixthCircleDiameter * 0.5f), userControlDPointAZM.Top);
        }

        private void PositionINCWidgets(PointF ptCenter, float fScaleWidthFactor)
        {
            //if (fScaleWidthFactor < 1.0f)
            //    userControlDPointINC.Width = 80;
            //else
            //    userControlDPointINC.Width = 121;
            //userControlDPointINC.Width = (int)(userControlDPointINC.Width * fScaleWidthFactor);
            //2/17/22Console.WriteLine("SCALE WIDTH FACTOR: " + fScaleWidthFactor.ToString());
                // keep the vertical position static
                // move the inc widget so that the wedge part still stays inside of the 
                // TF window when the screen real estate is getting smaller
            int iOffset = 0; // 
            if (fScaleWidthFactor < 1.0f)                
                iOffset = (int)(CCommonTypes.INC_WIDGET_OFFSET * fScaleWidthFactor);
            

            //userControlDPointArcINC.Location = new Point(iOffset, userControlDPointArcINC.Location.Y);
            //userControlDPointINC.Location = new Point(iOffset + userControlDPointArcINC.Width, userControlDPointINC.Location.Y);            

            userControlDPointArcINC.Location = new Point(pictureBoxToolFace.Left + userControlDPointArcINC.Width / 2, userControlDPointArcINC.Top);

            float fSixthCircleDiameter = (float)CIRCLE_DIAMETER.SIX * fScaleWidthFactor;
            userControlDPointINC.Location = new Point((int)(ptCenter.X - fSixthCircleDiameter * 0.55f - userControlDPointArcINC.Width), userControlDPointINC.Top);            
        }

        private float GetScreenHeightScaleFactor()
        {
           return (float)ClientSize.Height / (float)CCommonTypes.IDEAL_TF_DIMENSION;
        }

        private float GetScreenWidthScaleFactor()
        {
            return (float)ClientSize.Width / (float)CCommonTypes.IDEAL_TF_DIMENSION;
        }

        private float GetAZMINCScaleFactor()
        {
            return (float)ClientSize.Width / (float)CCommonTypes.MIN_WIDTH_OF_TF_BEFORE_INC_AZM_SHRINK;
        }

        private void DrawCircles(PaintEventArgs e, PointF ptCenter, float fFactor_)
        {                                                                             
            // circles
            int width = (int)CIRCLE_DIAMETER.ONE;
            int height = (int)CIRCLE_DIAMETER.ONE;
            e.Graphics.DrawEllipse(m_penForCircle, ptCenter.X - width * 0.5f, ptCenter.Y - height * 0.5f, width, height);
            e.Graphics.FillEllipse(m_brushSolidCircle, ptCenter.X - width * 0.5f, ptCenter.Y - height * 0.5f, width, height);  // draw solid circle in the middle            
            
            width = height = (int)((float)CIRCLE_DIAMETER.TWO * fFactor_);
            e.Graphics.DrawEllipse(m_penForCircle, ptCenter.X - width * 0.5f, ptCenter.Y - height * 0.5f, width, height);
            width = height = (int)((float)CIRCLE_DIAMETER.THREE * fFactor_);
            e.Graphics.DrawEllipse(m_penForCircle, ptCenter.X - width * 0.5f, ptCenter.Y - height * 0.5f, width, height);
            width = height = (int)((float)CIRCLE_DIAMETER.FOUR * fFactor_);
            e.Graphics.DrawEllipse(m_penForCircle, ptCenter.X - width * 0.5f, ptCenter.Y - height * 0.5f, width, height);
            width = height = (int)((float)CIRCLE_DIAMETER.FIVE * fFactor_);
            e.Graphics.DrawEllipse(m_penForCircle, ptCenter.X - width * 0.5f, ptCenter.Y - height * 0.5f, width, height);
            width = height = (int)((float)CIRCLE_DIAMETER.SIX * fFactor_);
            e.Graphics.DrawEllipse(m_penForCircle, ptCenter.X - width * 0.5f, ptCenter.Y - height * 0.5f, width, height);            
        }

        // for testing fake data
        private void Redraw()
        {
            //m_tt.Hide(pictureBoxToolFace);
            float fTemp = fFakeAZM; // m_tfPlotArrData[0].fVal;
            for (int i = 0; i < MAX_POINTS - 1; i++)
            {
                m_tfPlotArrData[i] = m_tfPlotArrData[i + 1];
            }

            fTemp += 10.0f;
            if (fTemp > 359.0f)
            {
                fTemp = 0.0f;
            }

            m_tfPlotArrData[MAX_POINTS - 1].fVal = fTemp;
            if (m_tfPlotArrData[MAX_POINTS - 1].fVal < 10.0f && m_tfPlotArrData[MAX_POINTS - 2].fVal > 340.0f)
            {
                m_bIsGTF = !m_bIsGTF;
            }
            
            fFakeAZM += 10.0f;
            if (fFakeAZM > 360.0f)
                fFakeAZM = 0.0f;
            userControlDPointAZM.SetVal(fFakeAZM, CCommonTypes.MP_SUPER_SCRIPT);
            userControlDPointArcAZM.SetVal(fFakeAZM, "");

            fFakeINC += 5.0f;
            if (fFakeINC > 90.0f)
                fFakeINC = 0.0f;
            userControlDPointINC.SetVal(fFakeINC, CCommonTypes.EM_SUPER_SCRIPT);
            userControlDPointArcINC.SetVal(fFakeINC, "");

            pictureBoxToolFace.Invalidate();                        
        }

        private void pictureBoxToolFace_Paint(object sender, PaintEventArgs e)
        {
            // get center of toolface
            PointF ptCenter = GetCenter();

            // prevent jagged lines
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float fHeightFactor = GetScreenHeightScaleFactor();
            float fWidthFactor = GetScreenWidthScaleFactor();            
            float fFactor = fHeightFactor > fWidthFactor ? fWidthFactor : fHeightFactor;

            float fAZMINCFactor = GetAZMINCScaleFactor();

            if (m_bValidLicense)
            {                                
                DrawLines(e, ptCenter, fFactor);
                DrawCircles(e, ptCenter, fFactor);
                PositionDirectionLabels(m_bIsGTF, fFactor);
                
                PositionAZMWidgets(ptCenter, fAZMINCFactor);
                PositionINCWidgets(ptCenter, fAZMINCFactor);

                Plot(e, ptCenter, fFactor);

                DrawCenterTF(e, m_tfPlotArrData[MAX_POINTS - 1].fVal);
            }
            else
            {
                PositionDirectionLabels(m_bIsGTF, fFactor);
                PositionAZMWidgets(ptCenter, fAZMINCFactor);
                PositionINCWidgets(ptCenter, fAZMINCFactor);
                DrawInvalid(e);
            }
                
        }

        private void DrawLines(PaintEventArgs e, PointF ptCenter, float fFactor_)
        {                        
            
            float fSixthCircleDiameter = (float)CIRCLE_DIAMETER.SIX * fFactor_;
            // draw vertical
            e.Graphics.DrawLine(m_penForLine, ptCenter.X, ptCenter.Y - fSixthCircleDiameter * 0.5f,
                                              ptCenter.X, ptCenter.Y + fSixthCircleDiameter * 0.5f);            

            // draw horizontal
            e.Graphics.DrawLine(m_penForLine, ptCenter.X - fSixthCircleDiameter * 0.5f, ptCenter.Y,
                                              ptCenter.X + fSixthCircleDiameter * 0.5f, ptCenter.Y);

            // draw diagonals
            // top left to bottom right
            float x1 = fSixthCircleDiameter * 0.5f * (float)Math.Cos(Math.PI * 0.25f);
            float y1 = fSixthCircleDiameter * 0.5f * (float)Math.Sin(Math.PI * 0.25f);            
            e.Graphics.DrawLine(m_penForLine, ptCenter.X - x1, ptCenter.Y - y1, ptCenter.X + x1, ptCenter.Y + y1);

            // top right to bottom left
            float x2 = fSixthCircleDiameter * 0.5f * (float)Math.Cos(Math.PI * -0.25f);
            float y2 = fSixthCircleDiameter * 0.5f * (float)Math.Sin(Math.PI * -0.25f);
            e.Graphics.DrawLine(m_penForLine, ptCenter.X + x2, ptCenter.Y + y2, ptCenter.X - x2, ptCenter.Y - y2);
        }

        private void Plot(PaintEventArgs e, PointF ptCenter, float fFactor)
        {
            // plot the most recent at the outer circle and the oldest in the inner circle            
            for (int i = 0; i < MAX_POINTS; i++)   
            {
                // take the value
                if (m_tfPlotArrData[i].fVal < -9998.0f)                
                    continue;
                
                float f = m_tfPlotArrData[i].fVal + 90.0f;  // polar is off by 90
                float fRad = f * (float)Math.PI / 180.0f;  // convert to radians
                float fY = (float)Math.Sin(fRad) * m_fArrRadius[i] * fFactor;
                float fX = (float)Math.Cos(fRad) * m_fArrRadius[i] * fFactor;
                m_ptArrPosition[i].X = ptCenter.X - fX * 0.5f - m_fArrPlotDiameter[i] * 0.5f;
                m_ptArrPosition[i].Y = ptCenter.Y - fY * 0.5f - m_fArrPlotDiameter[i] * 0.5f;
                if (m_tfPlotArrData[i].iTechnology == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                {
                    e.Graphics.FillEllipse(m_brushPlots, ptCenter.X - fX * 0.5f - m_fArrPlotDiameter[i] * 0.5f, ptCenter.Y - fY * 0.5f - m_fArrPlotDiameter[i] * 0.5f, m_fArrPlotDiameter[i], m_fArrPlotDiameter[i]);
                    e.Graphics.DrawEllipse(m_penForCircleThin, ptCenter.X - fX * 0.5f - m_fArrPlotDiameter[i] * 0.5f, ptCenter.Y - fY * 0.5f - m_fArrPlotDiameter[i] * 0.5f, m_fArrPlotDiameter[i], m_fArrPlotDiameter[i]);
                }                    
                else  // must be mudpulse
                {                    
                    SizeF rectSize = new SizeF(m_fArrPlotDiameter[i], m_fArrPlotDiameter[i]);
                    PointF ptSquare = new PointF(ptCenter.X - fX * 0.5f - m_fArrPlotDiameter[i] * 0.5f, ptCenter.Y - fY * 0.5f - m_fArrPlotDiameter[i] * 0.5f);
                    RectangleF rect = new RectangleF(ptSquare, rectSize);
                    
                    e.Graphics.FillRectangle(m_brushPlots2, rect);
                    e.Graphics.DrawRectangle(m_penForCircleThin, ptSquare.X, ptSquare.Y, m_fArrPlotDiameter[i], m_fArrPlotDiameter[i]);                    

                    RotateRectangle(e.Graphics, rect, 45, m_fArrPlotDiameter[i]);
                }                                    
            }                
        }

        public void RotateRectangle(Graphics g, RectangleF r, float angle, float length_)
        {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(angle, new PointF(r.Left + (r.Width / 2),
                                          r.Top + (r.Height / 2)));
                g.Transform = m;
                g.FillRectangle(m_brushPlots2, r);                                
                g.ResetTransform();
            }
        }

        private void pictureBoxToolFace_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = System.Windows.Forms.Cursor.Position;
            double fDistance = float.MaxValue;            
            
            //System.Diagnostics.Debug.WriteLine("X: " + point.X + "\n Y: " + point.Y);
            for (int i = 0; i < MAX_POINTS; i++)
            {
                fDistance = Math.Sqrt(((double)e.X - (double)m_ptArrPosition[i].X) * ((double)e.X - (double)m_ptArrPosition[i].X) +
                ((double)e.Y - (double)m_ptArrPosition[i].Y) * ((double)e.Y - (double)m_ptArrPosition[i].Y));
                if (fDistance < 25 && !m_bShowToolTip)
                {
                    m_bShowToolTip = true;  // will get reset by worker thread
                    string s = String.Format("{0} Value {1} @ {2}", m_bIsGTF ? "GTF" : "MTF", m_tfPlotArrData[i].fVal, m_tfPlotArrData[i].dtVal.ToShortTimeString());
                    m_tt.Show(s, pictureBoxToolFace, e.Location, TOOL_TIP_TIMER * 1000);
                    break;
                }                                    
            }
            // useful for saving an image that you can reload if you're going to avoid 
            // re-drawing the circles and lines every time
            //this.pictureBoxToolFace.Image.Save("C:\\testdata\\test.png", System.Drawing.Imaging.ImageFormat.Png);

            //Bitmap newBitmap = new Bitmap(pictureBoxToolFace.Width, pictureBoxToolFace.Height);
            //PictureBox pictureBox2 = new PictureBox();
            //RectangleF rectCropArea = new RectangleF();
            //using (Graphics g = Graphics.FromImage(newBitmap))
            //{
                
            //    g.DrawImage(pictureBoxToolFace.InitialImage, new Rectangle(0, 0, pictureBox2.Width, pictureBox2.Height), rectCropArea, GraphicsUnit.Pixel);
            //}
            //pictureBox2.Image = newBitmap;
            //string file = "C:\\testdata\\blah.jpg"; // System.IO.Path.Combine(new string[] { System.IO.Directory.GetCurrentDirectory(), "Debug", "patch1.jpg" });
            //newBitmap.Save(file, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void FormToolFace_Resize(object sender, EventArgs e)
        {
            // pictureBoxToolFace.Refresh();   
            //float fScale = GetAZMINCScaleFactor();
            //if (fScale < 1.0f)
            //{
            //    userControlDPointINC.UseSmallFont(true);
            //    userControlDPointAZM.UseSmallFont(true);
            //}
            //else
            //{
            //    userControlDPointINC.UseSmallFont(false);
            //    userControlDPointAZM.UseSmallFont(false);
            //}
            Refresh();
        }

        private void FormToolFace_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_DataLayer.Changed -= new ChangedEventHandler(ListChanged);
            //m_threadTestPlot.Abort("Quitting from FormToolface");
            //m_threadTestPlot.Join();            
        }

        private void FormToolFace_Load(object sender, EventArgs e)
        {            
            m_threadTestPlot.Start(this);
            float fFactor = GetScreenHeightScaleFactor();
            PositionDirectionLabels(m_bIsGTF, fFactor);
            userControlDPointAZM.SetDesc("AZM");
            userControlDPointAZM.SetUnits("°");
            userControlDPointAZM.SetMessageCode(9);
            userControlDPointAZM.SetPrecision(2);
            userControlDPointAZM.UseSmallFont(false);

            userControlDPointINC.SetDesc("INC");
            userControlDPointINC.SetUnits("°");
            userControlDPointINC.SetMessageCode(8);
            userControlDPointINC.SetPrecision(2);
            userControlDPointINC.UseSmallFont(false);

            userControlDPointArcAZM.SetType(UserControlDPointArc.ARC_TYPE.AZM);
            userControlDPointArcAZM.SetMessageCode(9);

            userControlDPointArcINC.SetMessageCode(8);

            RefreshScreen();
        }

        public void RefreshScreen()
        {
            string sTime = "";
            string sTelemetryType = "";
            string sVal = "";
            bool bParityErr = false;

            UserControlDPoint[] ucdpAngle = new UserControlDPoint[4] { userControlDPointINC, userControlDPointArcINC, userControlDPointAZM, userControlDPointArcAZM };
            for (int i = 0; i < 4; i++)
            {
                sVal = m_WidgetLookupInfo.GetLastSessionInfo(this.Name, ucdpAngle[i].Name, out sTime, out sTelemetryType, out bParityErr);
                if (sVal != null)
                {
                    if (sVal.Length > 0)
                        ucdpAngle[i].SetVal(sVal, sTelemetryType, sTime, bParityErr);
                    else
                        ucdpAngle[i].SetVal("---", "", "12:00:00 AM", bParityErr);
                }                    
            }

            sVal = m_WidgetLookupInfo.GetLastSessionInfo(this.Name, "TFCenter", out sTime, out sTelemetryType, out bParityErr);
            if (sVal != null)
                if (sVal.Length > 0)
                {
                    m_bIsGTF = sTelemetryType == "1" ? true : false;
                }

            // load toolface plots
            for (int i = 0; i < MAX_POINTS; i++)
            {
                sVal = m_WidgetLookupInfo.GetLastSessionInfo(this.Name, "TFPlot" + (i + 1).ToString(), out sTime, out sTelemetryType, out bParityErr);
                if (sVal != null)
                {
                    if (sVal.Length > 0)
                    {
                        m_tfPlotArrData[i].fVal = float.Parse(sVal);
                        m_tfPlotArrData[i].dtVal = System.Convert.ToDateTime(sTime);
                        m_tfPlotArrData[i].iTechnology = sTelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_EM.ToString() ? CCommonTypes.TELEMETRY_TYPE.TT_EM : CCommonTypes.TELEMETRY_TYPE.TT_MP;
                    }
                }
            }

            Refresh();
        }

        private void FormToolFace_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bUnload = true;
            //CWidgetInfoLookupTable widgetlookupInfo = new CWidgetInfoLookupTable();
            //widgetlookupInfo.Load();

            UserControlDPoint[] ucdpAngle = new UserControlDPoint[4] { userControlDPointINC, userControlDPointArcINC, userControlDPointAZM, userControlDPointArcAZM };
            for (int i = 0; i < 4; i++)
            {
                m_WidgetLookupInfo.SetSessionInfo(this.Name, ucdpAngle[i].Name, ucdpAngle[i].GetMessageCode(),
                                           ucdpAngle[i].GetVal(), ucdpAngle[i].GetTime(),
                                           ucdpAngle[i].GetTelemetryType(), ucdpAngle[i].GetParityErr());                
            }

            m_WidgetLookupInfo.SetSessionInfo(this.Name, "TFCenter", 7,
                                           m_tfPlotArrData[MAX_POINTS - 1].fVal.ToString(), m_tfPlotArrData[MAX_POINTS - 1].dtVal.ToString(),
                                           m_bIsGTF ? "1" : "0", false);

            // load toolface plots
            for (int i = 0; i < MAX_POINTS; i++)
            {
                m_WidgetLookupInfo.SetSessionInfo(this.Name, "TFPlot" + (i + 1).ToString(), 7,
                                           m_tfPlotArrData[i].fVal.ToString(), m_tfPlotArrData[i].dtVal.ToString(),
                                           m_tfPlotArrData[i].iTechnology.ToString(), false);
            }

            //widgetlookupInfo.Save();
        }        
    }
}
