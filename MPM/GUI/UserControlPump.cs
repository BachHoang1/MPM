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
    public partial class UserControlPump : UserControlDPoint
    {
        enum STATUS { NONE = -1, OFF = 0, ON = 1 };

        private const int DIAMETER = 80;
        private const int LEFT_PADDING = 5;
        private const int PEN_CIRCLE_STROKE_WIDTH = 5;
        private const int VAL_LENGTH_BEFORE_WRAP = 7;  // if the text is longer than this many characters, it wraps and doesn't look pretty.
        private const int OFFSET_FROM_TOP_OF_CIRCLE = 10;
        private const int OFFSET_FROM_DESC = 20;

        Pen m_penForCircle = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), PEN_CIRCLE_STROKE_WIDTH);
        Brush m_brushSolidCircle = new SolidBrush(Color.White);

        private STATUS m_iIsOn;  // no state (-1), "off" (0), or "on" (1)

        public UserControlPump()
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
            
            e.Graphics.DrawEllipse(m_penForCircle, LEFT_PADDING, ptCenter.Y - DIAMETER / 2, DIAMETER, DIAMETER);

            Point pt1 = new Point(LEFT_PADDING + DIAMETER / 2, (int)(ptCenter.Y - (DIAMETER / 2)));
            Point pt2 = new Point(pictureBoxInfo.Right, (int)(ptCenter.Y - (DIAMETER / 2)));
            e.Graphics.DrawLine(m_penForCircle, pt1, pt2);

            Size szCircle = new Size(DIAMETER, DIAMETER / 2);
            Point ptTopLeftOfCircle = new Point(LEFT_PADDING, (int)(ptCenter.Y - (DIAMETER / 2) + OFFSET_FROM_TOP_OF_CIRCLE));
            Rectangle rectTopOfCircle = new Rectangle(ptTopLeftOfCircle, szCircle);            
            e.Graphics.DrawString(m_sDesc, m_fontDesc, m_brush, rectTopOfCircle, m_stringFormat);

            Size szRectangleStatus = new Size(LEFT_PADDING, (int)(ptCenter.Y + 5));
            Rectangle rectStatus = rectTopOfCircle;
            rectStatus.Y += OFFSET_FROM_DESC;
            string sStatus = "---";
            if (m_iIsOn == STATUS.ON)
                sStatus = STATUS.ON.ToString();
            else if (m_iIsOn == STATUS.OFF)
                sStatus = STATUS.OFF.ToString();
            e.Graphics.DrawString(sStatus, m_fontSmallVal, m_brush, rectStatus, m_stringFormat);


            Size szRectangle = new Size(pictureBoxInfo.Right - DIAMETER - LEFT_PADDING, DIAMETER);
            Point ptRightOfCircle = new Point(LEFT_PADDING + DIAMETER, (int)(ptCenter.Y - (DIAMETER / 2)));
            Rectangle rectRightOfCircle = new Rectangle(ptRightOfCircle, szRectangle);
            if (m_sVal.Length < VAL_LENGTH_BEFORE_WRAP)
                e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontSmallVal, m_brush, rectRightOfCircle, m_stringFormat);    // top right        
            else                            
                e.Graphics.DrawString(m_sVal + " " + m_sUnits, m_fontTinyVal, m_brush, rectRightOfCircle, m_stringFormat);    // top right                    

            Brush brTechnology = m_brush;
            if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                brTechnology = m_brushEM;
            else if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                brTechnology = m_brushMP;

            szRectangle.Height = 15;
            ptRightOfCircle.Y += 60;            
            Rectangle rectBottom = new Rectangle(ptRightOfCircle, szRectangle);
            if (!m_bSmallFont)
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontDate, brTechnology, rectBottom, m_stringFormat);
            else
                e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_fontSmallDate, brTechnology, rectBottom, m_stringFormat);
        }
    }
}
