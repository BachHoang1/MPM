using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Utilities;
using License;
using System.Net.NetworkInformation;

namespace MPM.GUI
{
    public partial class FormRegisterProduct : Form
    {
        private bool m_bValid;
        private CLicense m_License;

        public FormRegisterProduct()
        {
            InitializeComponent();
            m_bValid = false;
            m_License = new CLicense();
        }

        public bool IsLicenseValid()
        {
            return m_bValid;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {            
            m_License.Decrypt(textBoxLicenseKey.Text);
            if (m_License.IsValid())
            {
                m_bValid = true;
                m_License.Save(textBoxLicenseKey.Text);
                MessageBox.Show("Product has been successfully registered.", "Valid License", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }                
            else
            {
                m_bValid = false;
                MessageBox.Show("License key is invalid. Please try another.", "Invalid License", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }                
        }

        private void FormRegisterProduct_Load(object sender, EventArgs e)
        {            
            m_License.Load();
            if (m_License.IsValid())
            {
                textBoxRegistrationInfo.Text += " " + m_License.GetUserName() + " at " + m_License.GetOrganizationName();
            }                
            else
            {
                textBoxRegistrationInfo.BackColor = Color.Red;
                textBoxRegistrationInfo.ForeColor = Color.Black;
                String sMACAddress = NetworkInterface
                                    .GetAllNetworkInterfaces()
                                    .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                                    .Select(nic => nic.GetPhysicalAddress().ToString())
                                    .FirstOrDefault();
                textBoxRegistrationInfo.Text = "This product has not been registered. Send APS this value to register:  " + sMACAddress;
            }
                
        }
    }
}
