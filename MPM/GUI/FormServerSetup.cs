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
    public partial class FormServerSetup : Form
    {
        private bool m_bIsServer = false;        
        private string m_sServerIPAddress;

        private CWidgetInfoLookupTable m_WidgetInfoLookup;

        public FormServerSetup(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            InitializeComponent();
            m_WidgetInfoLookup = widgetInfoLookup_;
        }

        private void FormServerSetup_Load(object sender, EventArgs e)
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();
            string sServerStatus = m_WidgetInfoLookup.GetValue("FormServerSetup", "checkBoxServer");
            if (sServerStatus == "True")
                checkBoxServer.Checked = m_bIsServer = true;
            else
                checkBoxServer.Checked = m_bIsServer = false;

            maskedTextBoxIP1.Enabled = maskedTextBoxIP2.Enabled = maskedTextBoxIP3.Enabled = maskedTextBoxIP4.Enabled = !checkBoxServer.Checked;

            string sIPAddress = m_WidgetInfoLookup.GetValue("FormServerSetup", "maskedTextBoxServerIPAddress");
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
                maskedTextBoxIP1.Text = "127";
                maskedTextBoxIP2.Text = "0";
                maskedTextBoxIP3.Text = "0";
                maskedTextBoxIP4.Text = "1";
            }

            string sPortNumber = m_WidgetInfoLookup.GetValue("FormServerSetup", "textBoxPortNumber");
            textBoxPortNumber.Text = sPortNumber;
        }// load up settings from last session         

        public Boolean CheckIPValid(String strIP_, ref string[] sArrIP_)
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

        public string GetServerIPAddress()
        {
            return m_sServerIPAddress;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //CWidgetInfoLookupTable WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();
            m_sServerIPAddress = maskedTextBoxIP1.Text + "." + maskedTextBoxIP2.Text + "." + maskedTextBoxIP3.Text + "." + maskedTextBoxIP4.Text;
            string[] sArrIPMP = new string[4];
            bool bIPAddress = false;
            if (CheckIPValid(m_sServerIPAddress, ref sArrIPMP))
            {
                m_WidgetInfoLookup.Set("FormServerSetup", "maskedTextBoxServerIPAddress", -1, m_sServerIPAddress);
                bIPAddress = true;
            }
            else
                MessageBox.Show("Please enter a valid IP address for the Display Server.", "Invalid IP Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


            m_WidgetInfoLookup.SetValue("FormServerSetup", "checkBoxServer", checkBoxServer.Checked ? "True" : "False");
            m_bIsServer = checkBoxServer.Checked;
            if (!m_bIsServer && bIPAddress)
            {
                m_WidgetInfoLookup.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (m_bIsServer)
            {
                m_WidgetInfoLookup.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
                
        }

        public bool GetServerMode()
        {
            return m_bIsServer;
        }

        private void checkBoxServer_CheckedChanged(object sender, EventArgs e)
        {            
            maskedTextBoxIP1.Enabled = maskedTextBoxIP2.Enabled = maskedTextBoxIP3.Enabled = maskedTextBoxIP4.Enabled = !checkBoxServer.Checked;
            if (!checkBoxServer.Checked)
            {
                maskedTextBoxIP1.Text = "192";
                maskedTextBoxIP2.Text = "168";
                maskedTextBoxIP3.Text = "25";
                maskedTextBoxIP4.Text = "230";
            }
            else
            {
                maskedTextBoxIP1.Text = "0";
                maskedTextBoxIP2.Text = "0";
                maskedTextBoxIP3.Text = "0";
                maskedTextBoxIP4.Text = "0";
            }
        }
    }
}
