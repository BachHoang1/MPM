// author: hoan chau
// purpose: encapsulate the rendering, functionality of a D-point's information and 
// to display it in a nice format. "Nice" means pretty and readable from several feet away

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
    public partial class UserControlDPoint : UserControl
    {
        protected enum VALUE_ALERT { VA_GOOD, VA_BAD, VA_NEUTRAL };
        protected enum COLOR_MAP_INEQUALITY { CMI_EQUALS, CMI_LESS_THAN, CMI_GREATER_THAN, CMI_NONE };
        const float FONT_REGULAR_SZ = 16.5f;
        const int FONT_SMALL_SZ = 14;
        const int FONT_TINY_SZ = 12;
        const int FONT_TEENY_TINY_SZ = 8;
        const int COLOR_MAP_SZ = 4;
        const string NO_COLOR_MAP = "none";
        public const string NO_VAL_YET = "---";
        const int MAX_LENGTH_OF_SMALL_FONT = 8;
        const string DEGREE_SYMBOL = "°";

        private const int MAX_LABEL_WIDTH = 150;  // pixels
        private const int MAX_LABEL_LARGE_WIDTH = 200; // 30% more than normal label
        private const int MAX_LABEL_HEIGHT = 150;        

        private const int MAX_LABEL_HORIZONTAL_WIDTH = 300;  // pixels
        private const int MAX_LABEL_LARGE_HORIZONTAL_WIDTH = 400;
        private const int MAX_LABEL_HORIZONTAL_HEIGHT = 200;

        private const int FONT_RANGE_COUNT = 12;  // 9, 10, 11, 12, 13, 14, and 15

        private const int MIN_FONT_DESC_SIZE = 4;  // for normal font display
        private const int MIN_FONT_DESC_LARGE_SIZE = 7;  // for large font display like the qualifiers

        private const int MIN_FONT_VALUE_SIZE = 3;  // for normal font display
        private const int MIN_FONT_VALUE_LARGE_SIZE = 9;  // for large font display like the qualifiers
        
        private const int MIN_FONT_DATE_SIZE = 3;  // for normal font display
        private const int MIN_FONT_DATE_LARGE_SIZE = 6;  // for large font display like the qualifiers

        protected int m_iMessageCode;  // id of d-point
        protected string m_sDesc;
        protected string m_sVal;  // the value's string representation which may differ from its original value when displayed due to precision truncation
        protected float m_fVal;  // the value's actual float value
        protected string m_sDate;
        protected string m_sTelemetryType;

        struct COLOR_THRESHOLDS
        {
            public COLOR_MAP_INEQUALITY cmiInequality;
            public float fThreshold;
            public Brush brColor;
        }
        
        private COLOR_THRESHOLDS[] m_clrArrMap;
        private bool m_bHasColorMap;
        protected bool m_bParityError;

        protected float m_fThresholdLow;
        protected float m_fThresholdHigh;

        private Byte m_iPrecision;  // places after decimal
        protected string m_sPrecisionFormat;
        // private int m_iDataType; // to be used for conversion
        protected string m_sUnits; // 
        // private int m_iPrecision; // for decimal places
        // private float m_fMinVal;  // threshold for warnings or alarms such as red colored font
        // private float m_fMaxVal;  // ditto

        // for coloring fonts good, bad, or neutral
        private float m_fMinGoodVal;

        protected Font m_fontDesc = new Font("Verdana", 12, FontStyle.Bold);
        protected Font m_fontVal = new Font("Verdana", FONT_REGULAR_SZ, FontStyle.Bold);
        protected Font m_fontDate = new Font("Verdana", 12, FontStyle.Bold);

        protected bool m_bSmallFont;
        protected bool m_bVerticalLayout;  // determines if the desc, value, and date are stacked vertically or horizontally (actually, the time sits below value)
        protected Font m_fontSmallDesc = new Font("Verdana", 9, FontStyle.Bold);
        protected Font m_fontSmallVal = new Font("Verdana", FONT_SMALL_SZ, FontStyle.Bold);
        protected Font m_fontSmallDate = new Font("Verdana", 9, FontStyle.Bold);

        protected Font m_fontTinyVal = new Font("Verdana", FONT_TINY_SZ, FontStyle.Bold);
        protected Font m_fontTeenyTinyVal = new Font("Verdana", FONT_TEENY_TINY_SZ, FontStyle.Bold);

        // bad decodes use strikethrough
        private Font m_fontValStrikeThrough = new Font("Verdana", FONT_REGULAR_SZ, FontStyle.Strikeout); 
        private Font m_fontSmallValStrikeThrough = new Font("Verdana", FONT_SMALL_SZ, FontStyle.Strikeout);
        private Font m_fontTinyValStrikeThrough = new Font("Verdana", FONT_TINY_SZ, FontStyle.Strikeout);

        protected Brush m_brush = new SolidBrush(Color.White);
        protected Brush m_brushGood = new SolidBrush(System.Drawing.Color.FromArgb(((int)(((byte)(0x00)))), ((int)(((byte)(0xCC)))), ((int)(((byte)(0x00))))));

        // alert colors
        private Brush m_brushAlertRed = new SolidBrush(Color.Red);
        private Brush m_brushAlertOrange = new SolidBrush(Color.Orange);
        private Brush m_brushAlertYellow = new SolidBrush(Color.Yellow);
        private Brush m_brushAlertGreen = new SolidBrush(Color.Green);

        // threshold colors
        protected Brush m_brushLowThreshold = new SolidBrush(Color.Crimson);
        protected Brush m_brushHighThreshold = new SolidBrush(Color.LimeGreen);

        // technology color used to distinguish between em and mp
        protected CCommonTypes.TELEMETRY_TYPE m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_NONE;
        protected Brush m_brushEM = new SolidBrush(Color.Orange);
        protected Brush m_brushMP = new SolidBrush(Color.Fuchsia);
        protected Brush m_brushMSA = new SolidBrush(Color.Lime);

        protected StringFormat m_stringFormat = new StringFormat();
        protected StringFormat m_stringFormatHorizontal = new StringFormat();
        public event EventHandler MouseDoubleClicked;

        // arrays of font sizes for scaling according to resizing
        private Font[] m_arrFontDesc;
        private Font[] m_arrFontVal;  // use the appropriately scaled font when form is resized                                      
        private Font[] m_arrFontStrikeOut;
        private Font[] m_arrFontDate;

        private Font[] m_arrFontDescLarge;
        private Font[] m_arrFontValLarge;
        private Font[] m_arrFontStrikeOutLarge;
        private Font[] m_arrFontDateLarge;

        public UserControlDPoint()
        {
            InitializeComponent();
            // defaults
            m_iMessageCode = -1;
            m_sDesc = "NA";
            m_sVal = NO_VAL_YET;
            m_sUnits = "";
            m_sDate = "12:00:00 AM";
            m_sTelemetryType = "";
            m_clrArrMap = new COLOR_THRESHOLDS[COLOR_MAP_SZ];           
            m_bHasColorMap = false;
            m_bParityError = false;
            m_iPrecision = 1;
            m_sPrecisionFormat = "0.0";
            m_bSmallFont = false;  // start with big
            m_bVerticalLayout = true; // start with vertically positioned desc, value, and time
            m_fMinGoodVal = float.MaxValue;            

            // string alignment object for centering to a rectangle
            m_stringFormat.Alignment = StringAlignment.Center;
            m_stringFormat.LineAlignment = StringAlignment.Center;

            m_stringFormatHorizontal.Alignment = StringAlignment.Near;
            m_stringFormatHorizontal.LineAlignment = StringAlignment.Center;

            InitializeScalingFonts();
        }

        private void InitializeScalingFonts()
        {
            m_arrFontDesc = new Font[FONT_RANGE_COUNT];
            m_arrFontVal = new Font[FONT_RANGE_COUNT];
            m_arrFontStrikeOut = new Font[FONT_RANGE_COUNT];
            m_arrFontDate = new Font[FONT_RANGE_COUNT];

            m_arrFontDescLarge = new Font[FONT_RANGE_COUNT];
            m_arrFontValLarge = new Font[FONT_RANGE_COUNT];
            m_arrFontStrikeOutLarge = new Font[FONT_RANGE_COUNT];
            m_arrFontDateLarge = new Font[FONT_RANGE_COUNT];

            for (int i = 0; i < FONT_RANGE_COUNT; i++)
            {
                m_arrFontDesc[i] = new Font("Verdana", i + MIN_FONT_DESC_SIZE, FontStyle.Bold);
                m_arrFontVal[i] = new Font("Verdana", i + MIN_FONT_VALUE_SIZE, FontStyle.Bold);
                m_arrFontStrikeOut[i] = new Font("Verdana", i + MIN_FONT_VALUE_SIZE, FontStyle.Strikeout);
                m_arrFontDate[i] = new Font("Verdana", i + MIN_FONT_DATE_SIZE, FontStyle.Bold);

                m_arrFontDescLarge[i] = new Font("Verdana", i + MIN_FONT_DESC_LARGE_SIZE, FontStyle.Bold);
                m_arrFontValLarge[i] = new Font("Verdana", i + MIN_FONT_VALUE_LARGE_SIZE, FontStyle.Bold);
                m_arrFontStrikeOutLarge[i] = new Font("Verdana", i + MIN_FONT_VALUE_LARGE_SIZE, FontStyle.Strikeout);
                m_arrFontDateLarge[i] = new Font("Verdana", i + MIN_FONT_DATE_LARGE_SIZE, FontStyle.Bold);
            }
        }

        public float ConvertInterval(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        protected PointF GetCenter()
        {
            // get the center of the screen
            Rectangle rect = pictureBoxInfo.Bounds;
            PointF ptCenter = new PointF(rect.Width * 0.5f, rect.Height * 0.5f);
            return ptCenter;
        }        

        public void SetMessageCode(int iVal_)
        {
            m_iMessageCode = iVal_;
        }

        public int GetMessageCode()
        {
            return m_iMessageCode;
        }

        public string GetVal()
        {
            return m_sVal;
        }

        public string GetTime()
        {
            return m_sDate;
        }

        public string GetTelemetryType()
        {
            return m_sTelemetryType;
        }

        public bool GetParityErr()
        {
            return m_bParityError;
        }

        public void SetDesc(string sVal_)
        {
            m_sDesc = sVal_;                        
        }

        private COLOR_THRESHOLDS Map(string sColor_)
        {
            COLOR_THRESHOLDS clrThresholdRet = new COLOR_THRESHOLDS();
                        
            sColor_ = sColor_.ToLower();           
            if (sColor_.Contains("green"))
                clrThresholdRet.brColor = new SolidBrush(Color.FromArgb(((int)(((byte)(0x57)))), ((int)(((byte)(0xA6)))), ((int)(((byte)(0x4A))))));
            else if (sColor_.Contains("yellow"))
                clrThresholdRet.brColor = new SolidBrush(Color.FromArgb(((int)(((byte)(0xFF)))), ((int)(((byte)(0xD7)))), ((int)(((byte)(0x00))))));
            else if (sColor_.Contains("orange"))
                clrThresholdRet.brColor = new SolidBrush(Color.FromArgb(((int)(((byte)(0xCA)))), ((int)(((byte)(0x51)))), ((int)(((byte)(0x00)))))); 
            else if (sColor_.Contains("red"))
                clrThresholdRet.brColor = new SolidBrush(Color.FromArgb(((int)(((byte)(0xFF)))), ((int)(((byte)(0x4D)))), ((int)(((byte)(0x3C))))));
                        
            if (sColor_.Contains("<"))
            {
                clrThresholdRet.cmiInequality = COLOR_MAP_INEQUALITY.CMI_LESS_THAN;
                string[] sArr = sColor_.Split('<');
                clrThresholdRet.fThreshold = System.Convert.ToSingle(sArr[1]);
            }                                                      
            else if (sColor_.Contains(">"))
            {
                clrThresholdRet.cmiInequality = COLOR_MAP_INEQUALITY.CMI_GREATER_THAN;
                string[] sArr = sColor_.Split('>');
                clrThresholdRet.fThreshold = System.Convert.ToSingle(sArr[1]);
            }                
            else if (sColor_.Contains("="))
            {
                string[] sArr = sColor_.Split('=');
                clrThresholdRet.fThreshold = System.Convert.ToSingle(sArr[1]);
                clrThresholdRet.cmiInequality = COLOR_MAP_INEQUALITY.CMI_EQUALS;
            }
            else
                clrThresholdRet.cmiInequality = COLOR_MAP_INEQUALITY.CMI_NONE;            

            return clrThresholdRet;
        }

        private COLOR_MAP_INEQUALITY MapInequality(string sColor_)
        {
            COLOR_MAP_INEQUALITY cmiRetVal_ = COLOR_MAP_INEQUALITY.CMI_EQUALS;
            if (sColor_.Contains("<"))
                cmiRetVal_ = COLOR_MAP_INEQUALITY.CMI_LESS_THAN;
            else if (sColor_.Contains(">"))
                cmiRetVal_ = COLOR_MAP_INEQUALITY.CMI_GREATER_THAN;
            else if (sColor_.Contains("="))
                cmiRetVal_ = COLOR_MAP_INEQUALITY.CMI_EQUALS;

            return cmiRetVal_;
        }

        public void SetColorMap(string sVal_)
        {            
            // try and parse the color map
            if (sVal_ == NO_COLOR_MAP)  // use plain white for everything
            {
                m_clrArrMap[0] = Map(sVal_);                
                m_bHasColorMap = false;
            }
            else  // split the string into an array of indexed colors
            {
                m_bHasColorMap = true;
                string []sArrColor = sVal_.Split(',');
                for (int i = 0; i < sArrColor.Count(); i++)
                {
                    if (i < COLOR_MAP_SZ)
                    {
                        m_clrArrMap[i] = Map(sArrColor[i]);
                    }
                    //else  
                        // do nothing
                }
            }
        }

        public void SetPrecision(byte iVal_)
        {
            m_iPrecision = iVal_;
            if (m_iPrecision < 0)  // validated
                m_iPrecision = 1;
            else if (m_iPrecision > 6)
                m_iPrecision = 6;
            
            if (m_iPrecision == 0)  // whole number
                m_sPrecisionFormat = "0";  
            else
            {
                m_sPrecisionFormat = "0.";  // initialize
                for (int i = 0; i < m_iPrecision; i++)
                    m_sPrecisionFormat += "0";
            }            
        }

        public void SetVal(float fVal_, string sTelemetryType_, bool bParityErr_ = false)
        {
            m_fVal = fVal_;

            if (sTelemetryType_ == CCommonTypes.EM_SUPER_SCRIPT)
                m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_EM;
            else if (sTelemetryType_ == CCommonTypes.MP_SUPER_SCRIPT)
                m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_MP;
            else if (sTelemetryType_ == CCommonTypes.ROUND_LAB_DESCRIPTOR)
                m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_RL;

            m_bParityError = bParityErr_;
            m_sVal = fVal_.ToString(m_sPrecisionFormat);
            m_sTelemetryType = sTelemetryType_;
            m_sDate = DateTime.Now.ToString("HH:mm:ss");
            pictureBoxInfo.Invalidate();
        }

        public void SetVal(string sVal_, string sTelemetryType_, bool bParityErr_ = false)
        {            
            if (sVal_.IndexOf(".") > 0)
            {
                float fRes;
                bool b = float.TryParse(sVal_, out fRes);
                if (b)
                {
                    sVal_ = fRes.ToString(m_sPrecisionFormat);
                }
            }
                
            m_sVal = sVal_;

            if (sTelemetryType_ == CCommonTypes.EM_SUPER_SCRIPT)
                m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_EM;
            else if (sTelemetryType_ == CCommonTypes.MP_SUPER_SCRIPT)
                m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_MP;

            m_bParityError = bParityErr_;
            m_sTelemetryType = sTelemetryType_;
            m_sDate = DateTime.Now.ToString("HH:mm:ss");
            pictureBoxInfo.Invalidate();
        }

        public void SetVal(string sVal_, string sTelemetryType_, string sTime_, bool bParityErr_)
        {
            m_sVal = sVal_;
            float fMaybe = 0.0f;
            bool bNumeric = System.Single.TryParse(sVal_, out fMaybe);
            if (bNumeric)
                m_fVal = fMaybe;

            if (sTelemetryType_ == CCommonTypes.EM_SUPER_SCRIPT)
                m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_EM;
            else if (sTelemetryType_ == CCommonTypes.MP_SUPER_SCRIPT)
                m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_MP;
            //else if (sTelemetryType_ == CCommonTypes.RL_SUPER_SCRIPT)
            //    m_TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_RL;

            m_bParityError = bParityErr_;
            m_sTelemetryType = sTelemetryType_;
            m_sDate = sTime_;
            pictureBoxInfo.Invalidate();
        }

        public string GetUnits()
        {
            return m_sUnits;
        }

        public void SetUnits(string sVal_)
        {
            m_sUnits = sVal_;
            pictureBoxInfo.Invalidate();
        }

        public void UseSmallFont(bool bVal_)
        {
            m_bSmallFont = bVal_;
            pictureBoxInfo.Invalidate();
        }    
        
        public void UseVerticalLayout(bool bVal_)
        {
            m_bVerticalLayout = bVal_;
        }    

        public void SetMinGoodVal(float fVal_)
        {
            m_fMinGoodVal = fVal_;
        }

        private void UserControlDPoint_Resize(object sender, EventArgs e)
        {
            pictureBoxInfo.Refresh();
        }

        protected VALUE_ALERT GetValueAlert(float fVal_)
        {
            VALUE_ALERT vaRet = VALUE_ALERT.VA_NEUTRAL;            
            if (fVal_ > m_fMinGoodVal)
            {
                vaRet = VALUE_ALERT.VA_GOOD;
            }

            return vaRet;
        }

        public virtual void pictureBoxInfo_Paint(object sender, PaintEventArgs e)
        {
            // divide the box into three cells for desc, value, and date-time
            Rectangle rect = pictureBoxInfo.Bounds;
            Rectangle rectTop, rectBottom;
            Rectangle rectLeft, rectRightTop, rectRightBottom;
            int iCellHeight, iCellWidth;            

            if (m_bVerticalLayout)
            {
                rectLeft = rectRightTop = rectRightBottom = rect;  // not used
                iCellHeight = rect.Height / 3;
                iCellWidth = rect.Width;
                rectTop = rect;
                rectTop.Height = iCellHeight;
                rectBottom = new Rectangle(rect.Left, rect.Top + iCellHeight * 2, rect.Width, iCellHeight);
            }
            else  // almost horizontal layout
            {
                rectTop = rectBottom = rect; // not used
                iCellHeight = rect.Height / 2;
                iCellWidth = rect.Width / 5;
                rectLeft = new Rectangle(rect.Left, rect.Top, (int)(iCellWidth * 2.5), iCellHeight);
                rectRightTop = new Rectangle(rect.Left + (int)(iCellWidth * 2.5), rect.Top, (int)(iCellWidth * 2.5), iCellHeight);
                rectRightBottom = new Rectangle(rect.Left + (int)(iCellWidth * 2.5), rect.Top + iCellHeight, (int)(iCellWidth * 2.5), iCellHeight);
            }

            Brush brAlert = m_brush;
            Brush brTechnology = m_brush;
            if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                brTechnology = m_brushEM;
            else if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                brTechnology = m_brushMP;
            else if (m_TelemetryType == CCommonTypes.TELEMETRY_TYPE.TT_RL)
                brTechnology = m_brushMSA;
            //VALUE_ALERT va = GetValueAlert(m_fVal);
            //if (va == VALUE_ALERT.VA_GOOD)
            //    brAlert = m_brushGood;

                // get the right brush
            if (m_bHasColorMap && !m_sVal.Contains(NO_VAL_YET))
            {
                //int iVal = Convert.ToInt16(m_sVal);
                //if (iVal < COLOR_MAP_SZ)
                //{
                //    brAlert = m_clrArrMap[iVal].brColor;
                //}
                //else  // must be using thresholds
                //{
                    // loop through the inequalities
                    for (int i = 0; i < COLOR_MAP_SZ; i++)
                    {
                        if (m_clrArrMap[i].cmiInequality == COLOR_MAP_INEQUALITY.CMI_EQUALS)
                        {
                            if (Convert.ToSingle(m_sVal) == m_clrArrMap[i].fThreshold)
                            {
                                brAlert = m_clrArrMap[i].brColor;
                                break;
                            }
                        }
                        else if (m_clrArrMap[i].cmiInequality == COLOR_MAP_INEQUALITY.CMI_LESS_THAN)
                        {
                            if (Convert.ToSingle(m_sVal) < m_clrArrMap[i].fThreshold)
                            {
                                brAlert = m_clrArrMap[i].brColor;
                                break;
                            }
                        }
                        else if (m_clrArrMap[i].cmiInequality == COLOR_MAP_INEQUALITY.CMI_GREATER_THAN)
                        {
                            if (Convert.ToSingle(m_sVal) > m_clrArrMap[i].fThreshold)
                            {
                                brAlert = m_clrArrMap[i].brColor;
                                break;
                            }
                        }
                    }  // end for loop
                //}
            }
            
            // Draw the text and the surrounding rectangle.
            if (!m_bSmallFont)
            {
                int iFontSize;
                if (m_bVerticalLayout)
                    iFontSize = (int)ConvertInterval(this.Width, 0, MAX_LABEL_LARGE_WIDTH, MIN_FONT_VALUE_LARGE_SIZE - 1, MIN_FONT_VALUE_LARGE_SIZE + FONT_RANGE_COUNT);
                else
                    iFontSize = (int)ConvertInterval(this.Width, 0, MAX_LABEL_LARGE_HORIZONTAL_WIDTH, MIN_FONT_VALUE_LARGE_SIZE - 1, MIN_FONT_VALUE_LARGE_SIZE + FONT_RANGE_COUNT);

                int iFontSizeBasedOnHeight = (int)ConvertInterval(this.Height, 0, MAX_LABEL_HEIGHT, MIN_FONT_VALUE_LARGE_SIZE - 1, MIN_FONT_VALUE_LARGE_SIZE + FONT_RANGE_COUNT);
                if (iFontSizeBasedOnHeight < iFontSize)
                    iFontSize = iFontSizeBasedOnHeight;

                int iIndex = iFontSize - MIN_FONT_VALUE_LARGE_SIZE;
                if (iIndex > m_arrFontVal.Length - 1)
                    iIndex = m_arrFontVal.Length - 1;
                else if (iIndex < 0)
                    iIndex = 0;

                if (m_bVerticalLayout)
                {
                    e.Graphics.DrawString(m_sDesc, m_arrFontDescLarge[iIndex], m_brush, rectTop, m_stringFormat);
                    
                    string sCombined = m_sUnits == DEGREE_SYMBOL ? m_sVal + m_sUnits: m_sVal + " " + m_sUnits;

                    if (m_bParityError)
                        e.Graphics.DrawString(sCombined, m_arrFontStrikeOutLarge[iIndex], brAlert, rect, m_stringFormat);    // the middle 
                    else
                        e.Graphics.DrawString(sCombined, m_arrFontValLarge[iIndex], brAlert, rect, m_stringFormat);    // the middle                    

                    e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_arrFontDateLarge[iIndex], brTechnology, rectBottom, m_stringFormat);
                }
                else
                {
                    e.Graphics.DrawString(m_sDesc, m_arrFontDescLarge[iIndex], m_brush, rectLeft, m_stringFormatHorizontal);

                    string sCombined = m_sUnits == DEGREE_SYMBOL ? m_sVal + m_sUnits : m_sVal + " " + m_sUnits;

                    if (m_bParityError)
                        e.Graphics.DrawString(sCombined, m_arrFontStrikeOutLarge[iIndex], brAlert, rectRightTop, m_stringFormatHorizontal);    // top right        
                    else
                        e.Graphics.DrawString(sCombined, m_arrFontValLarge[iIndex], brAlert, rectRightTop, m_stringFormatHorizontal);    // top right        
                
                    e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_arrFontDateLarge[iIndex], brTechnology, rectRightBottom, m_stringFormatHorizontal);  // bottom
                    
                }
            }
            else  // normal font
            {
                int iFontSize;
                if (m_bVerticalLayout)
                    iFontSize = (int)ConvertInterval(this.Width, 0, MAX_LABEL_WIDTH, MIN_FONT_VALUE_SIZE - 1, MIN_FONT_VALUE_SIZE + FONT_RANGE_COUNT);
                else
                    iFontSize = (int)ConvertInterval(this.Width, 0, MAX_LABEL_HORIZONTAL_WIDTH, MIN_FONT_VALUE_SIZE - 1, MIN_FONT_VALUE_SIZE + FONT_RANGE_COUNT);

                int iFontSizeBasedOnHeight = (int)ConvertInterval(this.Height, 0, MAX_LABEL_HEIGHT, MIN_FONT_VALUE_SIZE - 1, MIN_FONT_VALUE_SIZE + FONT_RANGE_COUNT);
                if (iFontSizeBasedOnHeight < iFontSize)
                    iFontSize = iFontSizeBasedOnHeight;

                int iIndex = iFontSize - MIN_FONT_VALUE_SIZE;
                if (iIndex > m_arrFontVal.Length - 1)
                    iIndex = m_arrFontVal.Length - 1;
                else if (iIndex < 0)
                    iIndex = 0;

                if (m_bVerticalLayout)
                {
                    e.Graphics.DrawString(m_sDesc, m_arrFontDesc[iIndex], m_brush, rectTop, m_stringFormat);

                    string sCombined = m_sUnits == DEGREE_SYMBOL ? m_sVal + m_sUnits : m_sVal + " " + m_sUnits;
                    
                    if (m_bParityError)
                        e.Graphics.DrawString(sCombined, m_arrFontStrikeOut[iIndex], brAlert, rect, m_stringFormat);    // the middle        
                    else
                        e.Graphics.DrawString(sCombined, m_arrFontVal[iIndex], brAlert, rect, m_stringFormat);    // the middle        

                    e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_arrFontDate[iIndex], brTechnology, rectBottom, m_stringFormat);                                        
                }
                else
                {
                    e.Graphics.DrawString(m_sDesc, m_arrFontDesc[iIndex], m_brush, rectLeft, m_stringFormatHorizontal);

                    string sCombined = m_sUnits == DEGREE_SYMBOL ? m_sVal + m_sUnits : m_sVal + " " + m_sUnits;
                    
                    if (m_bParityError)
                        e.Graphics.DrawString(sCombined, m_arrFontStrikeOut[iIndex], brAlert, rectRightTop, m_stringFormatHorizontal);    // top right  
                    else
                        e.Graphics.DrawString(sCombined, m_arrFontVal[iIndex], brAlert, rectRightTop, m_stringFormatHorizontal);    // top right  

                    e.Graphics.DrawString(m_sDate + " " + m_sTelemetryType, m_arrFontDate[iIndex], brTechnology, rectRightBottom, m_stringFormatHorizontal);  // bottom right
                }
            }            
        }

        public void SetThresholds(string sLow_, string sHigh_)
        {
            float fLow = 0.0f;
            float fHigh = 0.0f;
            bool b = float.TryParse(sLow_, out fLow);
            if (b)
                m_fThresholdLow = fLow;

            b = float.TryParse(sHigh_, out fHigh);
            if (b)
                m_fThresholdHigh = fHigh;
        }

        public void GetThresholds(out float fLow_, out float fHigh_)
        {
            fLow_ = m_fThresholdLow;
            fHigh_ = m_fThresholdHigh;
        }

        protected virtual void OnMouseDoubleClicked(EventArgs e)
        {
            var handler = MouseDoubleClicked;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void pictureBoxInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClicked(e);
        }
    }
}
