// author: hoan chau
// purpose: allow entry of IP address to be used by to connect locally or remotely to Detect

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;

namespace MPM.GUI
{
    public partial class FormIPAddress : Form
    {
        private CWidgetInfoLookupTable m_WidgetInfoLookup;
        private string m_sIPAddressEM;
        private string m_sPortNumberEM;

        private string m_sIPAddressMP;
        private string m_sPortNumberMP;

        public FormIPAddress()
        {
            InitializeComponent();
        }

        public void SetWidgetInfoLookup(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            m_WidgetInfoLookup = widgetInfoLookup_;
        }

        public string GetIPAddressEM()
        {
            return m_sIPAddressEM;
        }

        public int GetPortNumberEM()
        {
            return System.Convert.ToInt32(m_sPortNumberEM);
        }

        public string GetIPAddressMP()
        {
            return m_sIPAddressMP;
        }

        public int GetPortNumberMP()
        {
            return System.Convert.ToInt32(m_sPortNumberMP);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_sIPAddressEM = maskedTextBoxIP1.Text + "." + maskedTextBoxIP2.Text + "." + maskedTextBoxIP3.Text + "." + maskedTextBoxIP4.Text;
            string[] sArrIP = new string[4];
            bool bIPAddressEM = false;
            bool bIPAddressMP = false;

            m_sPortNumberEM = textBoxPortNumberEM.Text;
            if (CheckIPValid(m_sIPAddressEM, ref sArrIP))
            {
                m_WidgetInfoLookup.Set("FormIPAddress", "maskedTextBoxIPAddressEM", -1, m_sIPAddressEM);
                m_WidgetInfoLookup.Set(this.Name, textBoxPortNumberEM.Name, -1, m_sPortNumberEM);
                bIPAddressEM = true;
            }
            else
                MessageBox.Show("Please enter a valid IP address for EM Detect.", "Invalid IP Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


            m_sPortNumberMP = textBoxPortNumberMP.Text;
            m_sIPAddressMP = maskedTextBoxIPMP1.Text + "." + maskedTextBoxIPMP2.Text + "." + maskedTextBoxIPMP3.Text + "." + maskedTextBoxIPMP4.Text;
            string[] sArrIPMP = new string[4];
            if (CheckIPValid(m_sIPAddressMP, ref sArrIPMP))
            {
                m_WidgetInfoLookup.Set("FormIPAddress", "maskedTextBoxIPAddressMP", -1, m_sIPAddressMP);
                m_WidgetInfoLookup.Set(this.Name, textBoxPortNumberMP.Name, -1, m_sPortNumberMP);
                bIPAddressMP = true;
            }
            else
                MessageBox.Show("Please enter a valid IP address for Mudpulse Detect.", "Invalid IP Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //3/9/22if (m_sIPAddressEM == m_sIPAddressMP)
            //3/9/22{
            //3/9/22    bIPAddressEM = bIPAddressMP = false;
            //3/9/22    MessageBox.Show("IP Addresses for Mudpulse and EM Detect should likely be different.", "Duplicate IP Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //3/9/22}

            if (m_sIPAddressEM == m_sIPAddressMP)
            {
                if (m_sPortNumberEM == m_sPortNumberMP)
                {
                    bIPAddressEM = bIPAddressMP = false;
                    MessageBox.Show("'IP Adr + Port Number' for 'Mudpulse' and 'EM' Detect can not be the same. They must be different.", "Ambiguous IP Address & Port Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (bIPAddressEM && bIPAddressMP)
            {
                m_WidgetInfoLookup.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        
        public Boolean CheckIPValid(String strIP_, ref string[]sArrIP_)
        {
            bool bRetVal = true;
            //  Split string by ".", check that array length is 4
            char chrFullStop = '.';
            sArrIP_ = strIP_.Split(chrFullStop);
            if (sArrIP_.Length != 4)
            {
                return false;
            }
            
            Int16 MAXVALUE = 255;            
            foreach (String strOctet in sArrIP_)
            {
                if (strOctet.Length > 3)
                {
                    bRetVal = false;
                    break;
                }

                Int32 iTemp = int.Parse(strOctet);
                if (iTemp > MAXVALUE)
                {
                    bRetVal = false;
                    break;
                }
            }
            return bRetVal;
        }

        private void FormIPAddress_Load(object sender, EventArgs e)
        {           
            // load up settings from last session            
            //m_WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //m_WidgetInfoLookupTbl.Load();
            textBoxPortNumberEM.Text = m_WidgetInfoLookup.GetValue(this.Name, textBoxPortNumberEM.Name);
            string sIPAddress = m_WidgetInfoLookup.GetValue("FormIPAddress", "maskedTextBoxIPAddressEM");
            string[] sArrIP = new string[4];
            if (CheckIPValid(sIPAddress, ref sArrIP))
            {
                maskedTextBoxIP1.Text = sArrIP[0];
                maskedTextBoxIP2.Text = sArrIP[1];
                maskedTextBoxIP3.Text = sArrIP[2];
                maskedTextBoxIP4.Text = sArrIP[3];
            }
            else
            {
                maskedTextBoxIP1.Text = "192";
                maskedTextBoxIP2.Text = "168";
                maskedTextBoxIP3.Text = "25";
                maskedTextBoxIP4.Text = "220";
            } 
            
            if (maskedTextBoxIP1.Text == "192" &&
                maskedTextBoxIP2.Text == "168" &&
                maskedTextBoxIP3.Text == "25"&&
                maskedTextBoxIP4.Text == "220")
            {
                checkBoxAPSEM.Checked = true;
            }
            else if (maskedTextBoxIP1.Text == "127" &&
                maskedTextBoxIP2.Text == "0" &&
                maskedTextBoxIP3.Text == "0" &&
                maskedTextBoxIP4.Text == "1")
            {
                checkBoxLocalHost.Checked = true;
            }

            textBoxPortNumberMP.Text = m_WidgetInfoLookup.GetValue(this.Name, textBoxPortNumberMP.Name);
            string sIPAddressMP = m_WidgetInfoLookup.GetValue("FormIPAddress", "maskedTextBoxIPAddressMP");
            string[] sArrIPMP = new string[4];
            if (CheckIPValid(sIPAddressMP, ref sArrIPMP))
            {
                maskedTextBoxIPMP1.Text = sArrIPMP[0];
                maskedTextBoxIPMP2.Text = sArrIPMP[1];
                maskedTextBoxIPMP3.Text = sArrIPMP[2];
                maskedTextBoxIPMP4.Text = sArrIPMP[3];
            }
            else
            {
                maskedTextBoxIPMP1.Text = "192";
                maskedTextBoxIPMP2.Text = "168";
                maskedTextBoxIPMP3.Text = "25";
                maskedTextBoxIPMP4.Text = "210";
            }

            if (maskedTextBoxIPMP1.Text == "192" &&
                maskedTextBoxIPMP2.Text == "168" &&
                maskedTextBoxIPMP3.Text == "25" &&
                maskedTextBoxIPMP4.Text == "210")
            {
                checkBoxAPSMP.Checked = true;
            }
            else if (maskedTextBoxIPMP1.Text == "127" &&
                maskedTextBoxIPMP2.Text == "0" &&
                maskedTextBoxIPMP3.Text == "0" &&
                maskedTextBoxIPMP4.Text == "1")
            {
                checkBoxLocalHostMP.Checked = true;
            }
        }

        private void checkBoxLocalHost_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLocalHost.Checked)
            {
                maskedTextBoxIP1.Text = "127";
                maskedTextBoxIP2.Text = "0";
                maskedTextBoxIP3.Text = "0";
                maskedTextBoxIP4.Text = "1";

                checkBoxAPSEM.Checked = false;

                //textBoxPortNumberEM.Text = "3300";
            }
        }

        private void SetMaskedTextBoxSelectAll(MaskedTextBox txtbox)
        {
            txtbox.SelectAll();
        }

        private void maskedTextBoxIP1_Enter(object sender, EventArgs e)
        {         
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void maskedTextBoxIP2_Enter(object sender, EventArgs e)
        {            
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void maskedTextBoxIP3_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void maskedTextBoxIP4_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void checkBoxLocalHostMP_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLocalHostMP.Checked)
            {
                maskedTextBoxIPMP1.Text = "127";
                maskedTextBoxIPMP2.Text = "0";
                maskedTextBoxIPMP3.Text = "0";
                maskedTextBoxIPMP4.Text = "1";

                checkBoxAPSMP.Checked = false;

                //textBoxPortNumberMP.Text = "3400";
            }
        }

        private void maskedTextBoxIPMP1_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void maskedTextBoxIPMP2_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void maskedTextBoxIPMP3_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void maskedTextBoxIPMP4_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }

        private void checkBoxAPSEM_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAPSEM.Checked)
            {
                maskedTextBoxIP1.Text = "192";
                maskedTextBoxIP2.Text = "168";
                maskedTextBoxIP3.Text = "25";
                maskedTextBoxIP4.Text = "220";

                checkBoxLocalHost.Checked = false;

                textBoxPortNumberEM.Text = "3300";
            }
        }

        private void checkBoxAPSMP_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAPSMP.Checked)
            {
                maskedTextBoxIPMP1.Text = "192";
                maskedTextBoxIPMP2.Text = "168";
                maskedTextBoxIPMP3.Text = "25";
                maskedTextBoxIPMP4.Text = "210";

                textBoxPortNumberMP.Text = "3400";

                checkBoxLocalHostMP.Checked = false;
            }
        }
    }
}
