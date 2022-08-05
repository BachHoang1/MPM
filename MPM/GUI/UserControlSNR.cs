using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;

namespace MPM.GUI
{
    
    public partial class UserControlSNR : UserControlDPoint
    {
        private const int LEFT_PADDING = 15;
        private const int PEN_RECT_STROKE_WIDTH = 5;
        private const int TOP_LINE_Y_COORD = 10;

        Pen m_penForTwoSidedRect = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), PEN_RECT_STROKE_WIDTH);
        
        private float m_fSignal;
        private float m_fNoise;
        private string m_sSignal;
        private string m_sNoise;

        public UserControlSNR()
        {
            InitializeComponent();
            m_fSignal = 0.0f;
            m_fNoise = 0.0f;
            m_sSignal = NO_VAL_YET;
            m_sNoise = NO_VAL_YET;
        }

        public void SetSignal(float fVal_)
        {
            m_fSignal = fVal_;
            m_sSignal = m_fSignal.ToString("0.00");
            if (Math.Abs(m_fNoise) > 0.0f && Math.Abs(m_fSignal) > 0.0f)
            {
                float m_fSNR = m_fSignal / m_fNoise;
                m_sVal = String.Format("{0:0.00}", m_fSNR);
                SetVal(m_fSNR, "ᴱᴹ", false);                
            }
            else
            {
                m_sVal = NO_VAL_YET;               
            }
            pictureBoxInfo.Invalidate();
        }

        public string GetSignal()
        {
            return m_fSignal.ToString();
        }        

        public void SetNoise(float fVal_)
        {
            m_fNoise = fVal_;
            m_sNoise = m_fNoise.ToString("0.00");
            if (Math.Abs(m_fNoise) > 0.0f && Math.Abs(m_fSignal) > 0.0f)
            {
                float m_fSNR = m_fSignal / m_fNoise;
                m_sVal = String.Format("{0:0.00}", m_fSNR);
                SetVal(m_fSNR, "ᴱᴹ", false);                
            }
            else
            {
                m_sVal = NO_VAL_YET;                
            }
            pictureBoxInfo.Invalidate();
        }

        public string GetNoise()
        {
            return m_fNoise.ToString();
        }

        public override void pictureBoxInfo_Paint(object sender, PaintEventArgs e)
        {
            PointF ptCenter = GetCenter();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // *************************************
            // draw 2-sided rectangle
            // *************************************
            // top horizontal line           
            Point pt1 = new Point(LEFT_PADDING, TOP_LINE_Y_COORD);
            Point pt2 = new Point(pictureBoxInfo.Right, TOP_LINE_Y_COORD);
            pt2.X -= 5;
            e.Graphics.DrawLine(m_penForTwoSidedRect, pt1, pt2);

            // bottom point
            Point pt3 = new Point(LEFT_PADDING, (int)(1) + 70);
           
            // left vertical line
            pt1.Y -= PEN_RECT_STROKE_WIDTH / 2;  // to make the corners meet more nicely
            pt3.Y += PEN_RECT_STROKE_WIDTH / 2;
            e.Graphics.DrawLine(m_penForTwoSidedRect, pt1, pt3);

            // bottom left short line
            Point pt4 = new Point(pictureBoxInfo.Left, (int)(1) + 70);
            pt3.Y -= PEN_RECT_STROKE_WIDTH / 2;
            e.Graphics.DrawLine(m_penForTwoSidedRect, pt3, pt4);

            // right vertical line
            Point pt5 = pt2;
            pt2.X -= PEN_RECT_STROKE_WIDTH / 2;
            pt2.Y -= PEN_RECT_STROKE_WIDTH / 2;
            pt5.Y += 80 + PEN_RECT_STROKE_WIDTH / 2;
            pt5.X -= PEN_RECT_STROKE_WIDTH / 2;
            e.Graphics.DrawLine(m_penForTwoSidedRect, pt2, pt5);

            // bottom right short line
            Point pt6 = pt5;
            pt5.X -= PEN_RECT_STROKE_WIDTH / 2;
            pt5.Y -= PEN_RECT_STROKE_WIDTH / 2;
            pt6.X += 15;
            pt6.Y -= PEN_RECT_STROKE_WIDTH / 2;
            e.Graphics.DrawLine(m_penForTwoSidedRect, pt5, pt6);

            // info
            Size szRectangle = new Size(pictureBoxInfo.Right, pt3.Y - pt1.Y);
            
            Point ptSignalDesc = new Point(40, (int)(TOP_LINE_Y_COORD - 15));
            Rectangle rectSignalDesc = new Rectangle(ptSignalDesc, szRectangle);            
            e.Graphics.DrawString("Signal: " + m_sSignal, m_fontTeenyTinyVal, m_brush, rectSignalDesc, m_stringFormat);

            Point ptNoiseDesc = new Point(40, (int)(TOP_LINE_Y_COORD));
            Rectangle rectNoiseDesc = new Rectangle(ptNoiseDesc, szRectangle);            
            e.Graphics.DrawString(" Noise: " + m_sNoise, m_fontTeenyTinyVal, m_brush, rectNoiseDesc, m_stringFormat);
            
            Point ptSNRValue = new Point(-30, (int)(TOP_LINE_Y_COORD - 15));
            Rectangle rectSNRValue = new Rectangle(ptSNRValue, szRectangle);
            Rectangle rectSNRDesc = rectSNRValue;
            rectSNRDesc.X -= 20;  // need to position label left of center
            e.Graphics.DrawString("SNR", m_fontSmallVal, m_brush, rectSNRDesc, m_stringFormat);

            rectSNRValue.X -= 20;  // need to position value right of center  
            rectSNRValue.Y += 20;
            float fVal = 0.0f;
            bool b = float.TryParse(m_sVal, out fVal);
            string sValAndUnits = m_sVal;
            if (b)
            {
                if (m_fVal < m_fThresholdLow)
                    e.Graphics.DrawString(sValAndUnits, m_fontSmallVal, m_brushLowThreshold, rectSNRValue, m_stringFormat);
                else if (m_fVal > m_fThresholdHigh)
                    e.Graphics.DrawString(sValAndUnits, m_fontSmallVal, m_brushHighThreshold, rectSNRValue, m_stringFormat);
                else // nominal
                    e.Graphics.DrawString(sValAndUnits, m_fontSmallVal, m_brush, rectSNRValue, m_stringFormat);
            }
            else
                e.Graphics.DrawString(sValAndUnits, m_fontSmallVal, m_brush, rectSNRValue, m_stringFormat);            
            
            Brush brTechnology = m_brush;
            if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                brTechnology = m_brushEM;
            else if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                brTechnology = m_brushMP;

            szRectangle.Height = pt3.Y - pt2.Y;
            ptSNRValue.Y += 40;
            Rectangle rectBottom = new Rectangle(ptSNRValue, szRectangle);
            if (!m_bSmallFont)
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontDate, brTechnology, rectBottom, m_stringFormat);
            else
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontSmallDate, brTechnology, rectBottom, m_stringFormat);
        }                  
    }
}
