// author: hoan chau
// purpose: displays battery graphic along with information about power and status

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
    public partial class UserControlBattery : UserControlDPoint
    {
        private const int LEFT_PADDING = 15;
        private const int PEN_RECT_STROKE_WIDTH = 5;
        private const int TERMINAL_SIDE_OFFSET = 40;

        enum STATUS {NONE = -1, PRIMARY = 2, BACKUP = 3};

        Pen m_penForTwoSidedRect = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), PEN_RECT_STROKE_WIDTH);                
        Pen m_penForTerminal = new Pen(Color.White, 14);

        private STATUS m_iIsOn;  // no state (-1), "off" (2), or "on" (3)
        
        public UserControlBattery()
        {
            InitializeComponent();
            m_iIsOn = STATUS.NONE;
        }

        public void SetStatus(int iVal_)
        {
            m_iIsOn = (STATUS)iVal_;
            pictureBoxInfo.Invalidate();
        }

        public override void pictureBoxInfo_Paint(object sender, PaintEventArgs e)
        {
            PointF ptCenter = GetCenter();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // *************************************
            // draw 2-sided rectangle
            // *************************************
            // top horizontal line
            Point pt1 = new Point(LEFT_PADDING, (int)(ptCenter.Y - (ptCenter.Y / 2)));
            Point pt2 = new Point(pictureBoxInfo.Right, (int)(ptCenter.Y - (ptCenter.Y / 2)));
            e.Graphics.DrawLine(m_penForTwoSidedRect, pt1, pt2);
            // bottom horizontal line
            Point pt3 = new Point(LEFT_PADDING, (int)(ptCenter.Y - (ptCenter.Y / 2)) + 90);
            // looks better without the bottom line
            //Point pt4 = new Point(pictureBoxInfo.Right, (int)(ptCenter.Y - (ptCenter.Y / 2)) + 90);
            //e.Graphics.DrawLine(m_penForThreeSidedRect, pt3, pt4);
            // left vertical line
            pt1.Y -= PEN_RECT_STROKE_WIDTH / 2;  // to make the corners meet more nicely
            pt3.Y += PEN_RECT_STROKE_WIDTH / 2;
            e.Graphics.DrawLine(m_penForTwoSidedRect, pt1, pt3);
            // terminals
            // draw left terminal and "+"
            Point ptTerminalLeftBottom = pt1;
            ptTerminalLeftBottom.X = (int)(ptCenter.X - TERMINAL_SIDE_OFFSET);
            Point ptTerminalLeftTop = pt1;
            ptTerminalLeftTop.X = (int)(ptCenter.X - TERMINAL_SIDE_OFFSET);
            ptTerminalLeftTop.Y -= 10;
            e.Graphics.DrawLine(m_penForTerminal, ptTerminalLeftBottom, ptTerminalLeftTop);            
            // draw right terminal and "-"
            Point ptTerminalRightBottom = pt1;
            ptTerminalRightBottom.X = (int)(this.Right - TERMINAL_SIDE_OFFSET);            
            Point ptTerminalRightTop = pt1;
            ptTerminalRightTop.X = (int)(this.Right - TERMINAL_SIDE_OFFSET);
            ptTerminalRightTop.Y -= 10;
            e.Graphics.DrawLine(m_penForTerminal, ptTerminalRightBottom, ptTerminalRightTop);

            // info
            Size szRectangle = new Size(pictureBoxInfo.Right - LEFT_PADDING, pt3.Y - pt1.Y);
            Point ptRightOfCircle = new Point(LEFT_PADDING, pt1.Y);
            Rectangle rectRightOfCircle = new Rectangle(ptRightOfCircle, szRectangle);
            e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontSmallVal, m_brush, rectRightOfCircle, m_stringFormat);

            Size szRectangleStatus = new Size(pictureBoxInfo.Right - LEFT_PADDING, pt1.Y + 10);
            Rectangle rectStatus = new Rectangle(ptRightOfCircle, szRectangleStatus);
            string sStatus = "---";
            if (m_iIsOn == STATUS.BACKUP)
                sStatus = STATUS.BACKUP.ToString();
            else if (m_iIsOn == STATUS.PRIMARY)
                sStatus = STATUS.PRIMARY.ToString();
            e.Graphics.DrawString(sStatus, m_fontSmallVal, m_brush, rectStatus, m_stringFormat);

            Brush brTechnology = m_brush;
            if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                brTechnology = m_brushEM;
            else if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                brTechnology = m_brushMP;

            szRectangle.Height = pt3.Y - pt2.Y;
            ptRightOfCircle.Y += 25;
            Rectangle rectBottom = new Rectangle(ptRightOfCircle, szRectangle);
            if (!m_bSmallFont)
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontDate, brTechnology, rectBottom, m_stringFormat);
            else
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontSmallDate, brTechnology, rectBottom, m_stringFormat);
        }
    }
}
