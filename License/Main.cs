// author: hoan chau
// purpose: to generate a license key that Display 2.0 uses to authenticate a user

using System;
using System.Windows.Forms;


namespace License
{    
    public partial class Main : Form
    {
        const int MAC_ADDRESS_LENGTH = 12;
        const int DEFAULT_DAYS_VALID = 9999;  //
        
        public Main()
        {
            InitializeComponent();
            CLicenseDatabase db = new CLicenseDatabase();
            db.Create();
        }

        private bool IsAlphaNumeric(string sVal_)
        {            
            bool bRetVal = true;
            sVal_ = sVal_.ToUpper();

            if (sVal_.Length < 1)
                bRetVal = false;

            for (int i = 0; i < sVal_.Length; i++)
            {
                if ((sVal_[i] >= 'A' && sVal_[i] <= 'Z') ||
                    (sVal_[i] >= '0' && sVal_[i] <= '9'))
                    continue;
                else
                {
                    bRetVal = false;
                    break;
                }
            }

            return bRetVal;
        }      
        
        private bool IsValid(string s_)
        {
            bool bRetVal = true;
            if (s_.Length == 0 || s_.Trim() == "")
            {
                bRetVal = false;
            }

            return bRetVal;
        }

        private void buttonGenerateLicenseKey_Click(object sender, EventArgs e)
        {                        
            bool bUserValid = IsValid(textBoxUser.Text);
            bool bOrganizationValid = IsValid(textBoxOrganization.Text);
            bool bMACAddress = IsAlphaNumeric(textBoxMACAddress.Text);
            if (!bUserValid)
            {
                MessageBox.Show("User name can't be blank or contain only spaces.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }
            else if (!bOrganizationValid)
            {
                MessageBox.Show("Organization can't be blank or contain only spaces.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!bMACAddress || textBoxMACAddress.Text.Length != MAC_ADDRESS_LENGTH)
            {
                string sMsg = string.Format("MAC address can't be blank, must be alpha-numeric, and length {0}.", MAC_ADDRESS_LENGTH);
                MessageBox.Show(sMsg, "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                CLicenseDatabase db = new CLicenseDatabase();
                CLicenseRecord recDuplicate = db.Find(textBoxMACAddress.Text);
                bool bAddRecord = false;
                if (recDuplicate.iDate != 0)
                {
                    if (MessageBox.Show("An active record with this MAC Address already exists.\nMAC Address belongs to " + recDuplicate.sUser + " @ " + recDuplicate.sOrganization + ".\nProceed anyways?", "Duplicate Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)                    
                        bAddRecord = true;                    
                }
                else
                    bAddRecord = true;

                if (bAddRecord)
                {
                    CLicense license = new CLicense();
                    string sInfo = textBoxUser.Text + "#" + textBoxOrganization.Text + "#" + textBoxMACAddress.Text + DateTime.Now.ToString("#yyyyMMdd_HHmmss#" + DEFAULT_DAYS_VALID.ToString());  // last 4 digits is number of days license is valid which isn't used yet
                    string sCipherText = license.Encrypt(sInfo);
                    textBoxLicenseKey.Text = sCipherText;

                    CLicenseRecord rec = new CLicenseRecord();
                    rec.iDate = System.Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                    rec.iTime = System.Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                    rec.sComputer = textBoxComputer.Text;
                    rec.sUser = textBoxUser.Text;
                    rec.sOrganization = textBoxOrganization.Text;
                    rec.sMACAddress = textBoxMACAddress.Text;
                    rec.iActive = 1;
                    rec.sLicense = textBoxLicenseKey.Text;
                    rec.iDaysValid = DEFAULT_DAYS_VALID;
                    db.Save(rec);
                }
                                
            }                        
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLicenseRecords frm = new FormLicenseRecords();
            frm.ShowDialog();
        }               
    }
}
