// author: hoan chau
// purpose: draw a pie chart representing the angle of inclination, azimuth, etc.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.GUI
{
    public partial class UserControlDPointArc : UserControlDPoint
    {
        public enum ARC_TYPE { INC, AZM };

        private const float REDUCTION_FACTOR = 0.8f;
        private const float OFFSET = 90.0f;        
    
        Pen m_penForCircle = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), 3);
        Pen m_penThinForCircle = new Pen(Color.FromArgb(0x3C, 0x98, 0xC4), 1);
        Brush m_brushSolidCircle = new SolidBrush(Color.White);
        Brush m_brushBackgroundCircle = new SolidBrush(Color.FromArgb(0x3C, 0x98, 0xC4));

        ARC_TYPE m_arcType;

        public UserControlDPointArc()
        {
            InitializeComponent();
            m_arcType = ARC_TYPE.INC;            
        }

        public void SetType(ARC_TYPE atVal_)
        {
            m_arcType = atVal_;
        }

        public override void pictureBoxInfo_Paint(object sender, PaintEventArgs e)
        {
            PointF ptCenter = GetCenter();
            
            Rectangle rect = new Rectangle(new Point(5, 5), new Size(pictureBoxInfo.Bounds.Width, pictureBoxInfo.Bounds.Height));                                                
            rect.Width = (int)((float)rect.Width * REDUCTION_FACTOR);
            rect.Height = (int)((float)rect.Height * REDUCTION_FACTOR);
            
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (m_arcType == ARC_TYPE.INC)
            {
                e.Graphics.FillPie(m_brushBackgroundCircle, rect, OFFSET, -90.0f);
                e.Graphics.FillPie(m_brushSolidCircle, rect, OFFSET, -1.0f * m_fVal);  // pie filling                 
            }
            else  // must be azimuth
            {
                e.Graphics.FillPie(m_brushBackgroundCircle, rect, -OFFSET, 360.0f);
                e.Graphics.FillPie(m_brushSolidCircle, rect, -OFFSET, m_fVal);  // pie filling                
            }
        }
    }
}
