using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace License
{
    public partial class FormEditRecord : Form
    {
        CLicenseRecord m_licenseRec;        
        private string m_sTime;  // in case the conversion to integer removes the leading zeros (i.e., 093421 -> 93421)

        public FormEditRecord()
        {
            InitializeComponent();
            m_licenseRec = new CLicenseRecord();
            m_sTime = "120000";  // 12 PM
        }

        public void Init(CLicenseRecord rec_)
        {
            m_licenseRec = rec_;                        
        }

        public CLicenseRecord GetRecord()
        {
            return m_licenseRec;
        }

        public string GetTime()
        {
            return m_sTime;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // assigned date and time, user, organization, mac address
            // and license text don't change

            m_licenseRec.sComputer = textBoxComputer.Text;            
            if (checkBoxActive.Checked)
            {
                m_licenseRec.iActive = 1;
                m_licenseRec.iRemovedDate = 99991231;
                m_licenseRec.iRemovedTime = 235959;
            }                
            else
            {
                m_licenseRec.iActive = 0;                
                m_licenseRec.iRemovedDate = System.Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                m_sTime = DateTime.Now.ToString("HHmmss");
                m_licenseRec.iRemovedTime = System.Convert.ToInt32(m_sTime);                
            }

            this.DialogResult = DialogResult.OK;
        }

        private void FormEditRecord_Load(object sender, EventArgs e)
        {
            textBoxComputer.Text = m_licenseRec.sComputer;
            textBoxUser.Text = m_licenseRec.sUser;
            textBoxOrganization.Text = m_licenseRec.sOrganization;
            textBoxMACAddress.Text = m_licenseRec.sMACAddress;
            string sDate = m_licenseRec.iDate.ToString(), sTime = m_licenseRec.iTime.ToString();
            if (sDate.Length > 7)
            {
                sDate = sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2);
                if (sTime.Length > 5)
                    sTime = sTime.Substring(0, 2) + ":" + sTime.Substring(2, 2) + ":" + sTime.Substring(4, 2);
                else if (sTime.Length == 5)
                {
                    sTime = "0" + sTime;
                    sTime = sTime.Substring(0, 2) + ":" + sTime.Substring(2, 2) + ":" + sTime.Substring(4, 2);
                }
            }
            
            
            textBoxAssignedDate.Text = sDate + " " + sTime;
            textBoxLicenseKey.Text = m_licenseRec.sLicense;
            if (m_licenseRec.iActive == 1)
                checkBoxActive.Checked = true;
            else
                checkBoxActive.Checked = false;
            string sRemovedDate = m_licenseRec.iRemovedDate.ToString(), sRemovedTime = m_licenseRec.iRemovedTime.ToString();
            if (sRemovedDate.Length > 7)
            {
                sRemovedDate = sRemovedDate.Substring(0, 4) + "-" + sRemovedDate.Substring(4, 2) + "-" + sRemovedDate.Substring(6, 2);
                if (sRemovedTime.Length > 5)
                    sRemovedTime = sRemovedTime.Substring(0, 2) + ":" + sRemovedTime.Substring(2, 2) + ":" + sRemovedTime.Substring(4, 2);
                else if (sRemovedTime.Length == 5)
                {
                    sRemovedTime = "0" + sRemovedTime;
                    sRemovedTime = sRemovedTime.Substring(0, 2) + ":" + sRemovedTime.Substring(2, 2) + ":" + sRemovedTime.Substring(4, 2);
                }

                textBoxRemovedDate.Text = sRemovedDate + " " + sRemovedTime;
            }
        }       
    }
}
