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
    public partial class UserControlPulseHeight : UserControlDPoint
    {
        private const int LEFT_PADDING = 15;
        private const int PEN_RECT_STROKE_WIDTH = 5;

        Pen m_penForTwoSidedRect = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), PEN_RECT_STROKE_WIDTH);

        public UserControlPulseHeight()
        {
            InitializeComponent();
        }

        public override void pictureBoxInfo_Paint(object sender, PaintEventArgs e)
        {
            PointF ptCenter = GetCenter();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // *************************************
            // draw 2-sided rectangle
            // *************************************
            // top horizontal line
            //Point pt1 = new Point(LEFT_PADDING + 15, (int)(ptCenter.Y - (ptCenter.Y / 2)));
            Point pt1 = new Point(LEFT_PADDING, (int)(10));
            Point pt2 = new Point(pictureBoxInfo.Right, (int)(10));
            pt2.X -= 15;
            //e.Graphics.DrawLine(m_penForTwoSidedRect, pt1, pt2);

            // bottom horizontal line
            Point pt3 = new Point(LEFT_PADDING, (int)(1) + 70);
            // looks better without the bottom line
            //Point pt4 = new Point(pictureBoxInfo.Right, (int)(ptCenter.Y - (ptCenter.Y / 2)) + 90);
            //e.Graphics.DrawLine(m_penForThreeSidedRect, pt3, pt4);
            // left vertical line
            pt1.Y -= PEN_RECT_STROKE_WIDTH / 2;  // to make the corners meet more nicely
            pt3.Y += PEN_RECT_STROKE_WIDTH / 2;
            //e.Graphics.DrawLine(m_penForTwoSidedRect, pt1, pt3);

            // bottom left short line
            Point pt4 = new Point(pictureBoxInfo.Left, (int)(1) + 70);
            pt3.Y -= PEN_RECT_STROKE_WIDTH / 2;
            //e.Graphics.DrawLine(m_penForTwoSidedRect, pt3, pt4);

            // right vertical line
            Point pt5 = pt2; // new Point(pictureBoxInfo.Right, (int)(ptCenter.Y - (ptCenter.Y / 2)));            
            pt2.X -= PEN_RECT_STROKE_WIDTH / 2;
            pt2.Y -= PEN_RECT_STROKE_WIDTH / 2;
            pt5.Y += 80 + PEN_RECT_STROKE_WIDTH / 2;
            pt5.X -= PEN_RECT_STROKE_WIDTH / 2;
            //e.Graphics.DrawLine(m_penForTwoSidedRect, pt2, pt5);

            // bottom right short line
            Point pt6 = pt5;
            pt5.X -= PEN_RECT_STROKE_WIDTH / 2;
            pt5.Y -= PEN_RECT_STROKE_WIDTH / 2;
            pt6.X += 15;
            pt6.Y -= PEN_RECT_STROKE_WIDTH / 2;
            //e.Graphics.DrawLine(m_penForTwoSidedRect, pt5, pt6);

            // info
            Size szRectangle = new Size(pictureBoxInfo.Right - LEFT_PADDING, pt3.Y - pt1.Y);
            Point ptRightOfCircle = new Point(LEFT_PADDING, (int)(ptCenter.Y - (ptCenter.Y / 2)));
            Rectangle rectRightOfCircle = new Rectangle(ptRightOfCircle, szRectangle);
            rectRightOfCircle.Y -= 15;
            float fVal = 0.0f;
            bool b = float.TryParse(m_sVal, out fVal);
            if (b)
            {
                if (fVal < m_fThresholdLow)
                    e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontSmallVal, m_brushLowThreshold, rectRightOfCircle, m_stringFormat);
                else if (fVal > m_fThresholdHigh)
                    e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontSmallVal, m_brushHighThreshold, rectRightOfCircle, m_stringFormat);
                else // nominal
                    e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontSmallVal, m_brush, rectRightOfCircle, m_stringFormat);
            }
            else
                e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontSmallVal, m_brush, rectRightOfCircle, m_stringFormat);

            Size szRectangleStatus = new Size(pictureBoxInfo.Right - LEFT_PADDING, pt1.Y + 10);
            Rectangle rectStatus = new Rectangle(ptRightOfCircle, szRectangleStatus);
            string sStatus = "Pulse Ht AVG";
            rectStatus.Y -= 15;
            e.Graphics.DrawString(sStatus, m_fontSmallVal, m_brush, rectStatus, m_stringFormat);

            Brush brTechnology = m_brush;
            if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                brTechnology = m_brushEM;
            else if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                brTechnology = m_brushMP;

            szRectangle.Height = pt3.Y - pt2.Y;
            ptRightOfCircle.Y += 7;
            Rectangle rectBottom = new Rectangle(ptRightOfCircle, szRectangle);
            if (!m_bSmallFont)
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontDate, brTechnology, rectBottom, m_stringFormat);
            else
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontSmallDate, brTechnology, rectBottom, m_stringFormat);
        }
    }
}
