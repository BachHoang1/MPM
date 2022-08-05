// author: hoan chau
// purpose: shows a temperature gauge graphic with tick marks
//
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
    public partial class UserControlTemperature : UserControlDPoint
    {
        private const int BULB_DIAMETER = 30;
        private const int LEFT_PADDING = 0;
        private const int PEN_RECT_STROKE_WIDTH = 5;
        
        Pen m_penForThreeSidedRect = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), PEN_RECT_STROKE_WIDTH);  
        Pen m_penForBulb = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), 2);
        Pen m_penForMercury = new Pen(Color.White, 2);
        Pen m_penMercuryTick = new Pen(Color.White, 3);
        Brush m_brushBackgroundBulb = new SolidBrush(Color.FromArgb(0x3C, 0x98, 0xC4));   
             

        public UserControlTemperature()
        {
            InitializeComponent();
        }

        public override void pictureBoxInfo_Paint(object sender, PaintEventArgs e)
        {
            PointF ptCenter = GetCenter();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // *************************************
            // draw 2-sides
            // *************************************
            // top horizonatal line
            Point pt1 = new Point(LEFT_PADDING + BULB_DIAMETER / 2, (int)(10));
            Point pt2 = new Point(pictureBoxInfo.Right, (int)(10));
            e.Graphics.DrawLine(m_penForThreeSidedRect, pt1, pt2);

            // bottom point line
            Point pt3 = new Point(LEFT_PADDING + BULB_DIAMETER / 2, (int)(10) + (int)(ptCenter.Y * 2 - BULB_DIAMETER / 2));            

            // left vertical line
            pt1.Y -= PEN_RECT_STROKE_WIDTH / 2;  // to make the corners meet more nicely
            pt3.Y += PEN_RECT_STROKE_WIDTH / 2;
            e.Graphics.DrawLine(m_penForThreeSidedRect, pt1, pt3);

            // draw the bulb at the bottom left            
            e.Graphics.FillEllipse(m_brushBackgroundBulb, pt3.X - BULB_DIAMETER / 2, pt3.Y - BULB_DIAMETER, BULB_DIAMETER, BULB_DIAMETER);
            e.Graphics.DrawEllipse(m_penForBulb, pt3.X - BULB_DIAMETER / 2, pt3.Y - BULB_DIAMETER, BULB_DIAMETER, BULB_DIAMETER);
                            
            // draw mercury 
            //pt1.Y += PEN_RECT_STROKE_WIDTH;  // to make the corners meet more nicely
            //pt3.Y -= PEN_RECT_STROKE_WIDTH;
            //e.Graphics.DrawLine(m_penForMercury, pt1, pt3);
            // draw tick marks
            // divide the height by 3 - 1) hot, 2) middle, and 3) cold
            int iThermometerHt = (pt3.Y - BULB_DIAMETER / 2) - pt1.Y;
            int iThermometerTick = iThermometerHt / 4;
            Point ptTick1 = new Point(LEFT_PADDING + PEN_RECT_STROKE_WIDTH + 15, pt1.Y + iThermometerTick);
            Point ptTick2 = new Point(LEFT_PADDING + PEN_RECT_STROKE_WIDTH + 25, pt1.Y + iThermometerTick); ;
            e.Graphics.DrawLine(m_penMercuryTick, ptTick1, ptTick2);
            ptTick2.Y = ptTick1.Y += iThermometerTick;
            e.Graphics.DrawLine(m_penMercuryTick, ptTick1, ptTick2);
            ptTick2.Y = ptTick1.Y += iThermometerTick;
            e.Graphics.DrawLine(m_penMercuryTick, ptTick1, ptTick2);            

            Size szRectangle = new Size(pictureBoxInfo.Right - LEFT_PADDING, pt3.Y - pt1.Y);
            Point ptRightOfCircle = new Point(LEFT_PADDING, (int)(ptCenter.Y - (ptCenter.Y / 2) - 30));
            Rectangle rectRightOfCircle = new Rectangle(ptRightOfCircle, szRectangle);
            e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontSmallVal, m_brush, rectRightOfCircle, m_stringFormat);    // top right


            Brush brTechnology = m_brush;
            if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                brTechnology = m_brushEM;
            else if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                brTechnology = m_brushMP;

            szRectangle.Height = pt3.Y - pt2.Y;
            ptRightOfCircle.Y += BULB_DIAMETER;
            Rectangle rectBottom = new Rectangle(ptRightOfCircle, szRectangle);
            if (!m_bSmallFont)
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontDate, brTechnology, rectBottom, m_stringFormat);
            else
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontSmallDate, brTechnology, rectBottom, m_stringFormat);
        }
    }
}
