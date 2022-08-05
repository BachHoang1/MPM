// author: hoan chau
// purpose: show em and mud pulse raw text coming from detect

using MPM.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.GUI
{
    public partial class FormRealTimeDetectLog : ToolWindow
    {
        private CDetectDataLayer m_DataLayer;
        private bool m_bScrolling;
        private int m_iCaretPosEM;
        private int m_iCaretPosMP;
        private bool m_bIsEMTextBoxSelected;  // indicates which box the user wants to search

        public FormRealTimeDetectLog()
        {
            InitializeComponent();
            m_bScrolling = true;
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.AcquiredEventText += new AcquiredTextEventHandler(TextReceived);
        }

        private void TextReceived(object sender, CEventDPointText ev_)
        {
            if (m_bScrolling)
            {
                if (ev_.m_ttSource == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                    textBoxEM.AppendText(ev_.m_sText);
                else
                    textBoxMudPulse.AppendText(ev_.m_sText);                
            }
            else
            {
                if (ev_.m_ttSource == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                {
                    textBoxEM.Text += ev_.m_sText;
                    textBoxEM.Select(m_iCaretPosEM, 0);
                    textBoxEM.ScrollToCaret();
                }                    
                else
                {                    
                    textBoxMudPulse.Text += ev_.m_sText;
                    textBoxMudPulse.Select(m_iCaretPosMP, 0);
                    textBoxMudPulse.ScrollToCaret();
                }                                    
            }
            
        }

        private void FormRealTimeDetectLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void buttonScroll_Click(object sender, EventArgs e)
        {
            if (m_bScrolling)
            {
                m_bScrolling = false;
                buttonScroll.Image = global::MPM.Properties.Resources.scrolling;
                m_iCaretPosEM = textBoxEM.Text.Length;
                m_iCaretPosMP = textBoxMudPulse.Text.Length;
                
            }
            else
            {
                m_bScrolling = true;
                buttonScroll.Image = global::MPM.Properties.Resources.scrollingStop;
            }
        }

        private void buttonSearchForward_Click(object sender, EventArgs e)
        {
            // stop the scrolling
            m_bScrolling = false;            
            buttonScroll.Image = global::MPM.Properties.Resources.scrolling;

            TextBox textBoxToSearch;
            try
            {
                // get the current textbox that has focus
                if (m_bIsEMTextBoxSelected)
                {
                    if (textBoxEM.SelectionStart >= textBoxEM.Text.Length ||
                        m_iCaretPosEM > textBoxEM.Text.Length)  // start at the beginning
                        m_iCaretPosEM = 0;

                    textBoxToSearch = textBoxEM;
                }
                else
                {
                    if (textBoxMudPulse.SelectionStart >= textBoxMudPulse.Text.Length ||
                        m_iCaretPosMP > textBoxMudPulse.Text.Length)  // start at the beginning
                        m_iCaretPosMP = 0;

                    textBoxToSearch = textBoxMudPulse;
                }


                int iIndex = -1;
                if (m_bIsEMTextBoxSelected)
                    iIndex = textBoxToSearch.Text.IndexOf(textBoxSearch.Text, m_iCaretPosEM);
                else
                    iIndex = textBoxToSearch.Text.IndexOf(textBoxSearch.Text, m_iCaretPosMP);

                int iIndexAny = textBoxToSearch.Text.IndexOf(textBoxSearch.Text, 0);
                if (iIndex > -1)
                {
                    //textBoxEM.Select(m_iCaretPosEM, textBoxSearch.Text.Length);
                    textBoxToSearch.SelectionStart = iIndex;
                    textBoxToSearch.SelectionLength = textBoxSearch.Text.Length;

                    if (m_bIsEMTextBoxSelected)
                        m_iCaretPosEM = iIndex + textBoxSearch.Text.Length;
                    else
                        m_iCaretPosMP = iIndex + textBoxSearch.Text.Length;

                    textBoxToSearch.Focus();
                    textBoxToSearch.ScrollToCaret();
                }
                else if (iIndexAny > -1)
                {
                    textBoxToSearch.SelectionStart = iIndexAny;
                    textBoxToSearch.SelectionLength = textBoxSearch.Text.Length;

                    if (m_bIsEMTextBoxSelected)
                        m_iCaretPosEM = iIndexAny + textBoxSearch.Text.Length;
                    else
                        m_iCaretPosMP = iIndexAny + textBoxSearch.Text.Length;

                    textBoxToSearch.Focus();
                    textBoxToSearch.ScrollToCaret();
                }
                else
                {
                    MessageBox.Show("'" + textBoxSearch.Text + "' could not be found! Click in one of the text boxes below and try again.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void textBoxEM_MouseClick(object sender, MouseEventArgs e)
        {
            m_bIsEMTextBoxSelected = true;
        }

        private void textBoxMudPulse_MouseClick(object sender, MouseEventArgs e)
        {
            m_bIsEMTextBoxSelected = false;
        }

        private void buttonSearchBackward_Click(object sender, EventArgs e)
        {
            // stop the scrolling
            m_bScrolling = false;
            buttonScroll.Image = global::MPM.Properties.Resources.scrolling;

            TextBox textBoxToSearch;
            try
            {
                // get the current textbox that has focus
                if (m_bIsEMTextBoxSelected)
                {
                    if (textBoxEM.SelectionStart < 1 ||
                        m_iCaretPosEM < 1)  // start at the beginning
                        m_iCaretPosEM = 0;

                    textBoxToSearch = textBoxEM;
                }
                else
                {
                    if (textBoxMudPulse.SelectionStart < 1 ||
                        m_iCaretPosMP < 1)  // start at the beginning
                        m_iCaretPosMP = 0;

                    textBoxToSearch = textBoxMudPulse;
                }


                int iIndex = -1;
                if (m_bIsEMTextBoxSelected)
                    iIndex = textBoxToSearch.Text.LastIndexOf(textBoxSearch.Text, m_iCaretPosEM);
                else
                    iIndex = textBoxToSearch.Text.LastIndexOf(textBoxSearch.Text, m_iCaretPosMP);

                int iIndexAny = textBoxToSearch.Text.LastIndexOf(textBoxSearch.Text, textBoxToSearch.Text.Length);
                if (iIndex > -1)
                {
                    //textBoxEM.Select(m_iCaretPosEM, textBoxSearch.Text.Length);
                    textBoxToSearch.SelectionStart = iIndex;
                    textBoxToSearch.SelectionLength = textBoxSearch.Text.Length;

                    if (m_bIsEMTextBoxSelected)
                        m_iCaretPosEM = iIndex - textBoxSearch.Text.Length;
                    else
                        m_iCaretPosMP = iIndex - textBoxSearch.Text.Length;

                    textBoxToSearch.Focus();
                    textBoxToSearch.ScrollToCaret();
                }
                else if (iIndexAny > -1)
                {
                    textBoxToSearch.SelectionStart = iIndexAny;
                    textBoxToSearch.SelectionLength = textBoxSearch.Text.Length;

                    if (m_bIsEMTextBoxSelected)
                        m_iCaretPosEM = iIndexAny - textBoxSearch.Text.Length;
                    else
                        m_iCaretPosMP = iIndexAny - textBoxSearch.Text.Length;

                    textBoxToSearch.Focus();
                    textBoxToSearch.ScrollToCaret();
                }
                else
                {
                    MessageBox.Show("'" + textBoxSearch.Text + "' could not be found! Click in one of the text boxes below and try again.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
