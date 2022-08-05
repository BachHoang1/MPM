using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.GUI
{
    public partial class FormRemoteControl560R : Form
    {
       
        public FormRemoteControl560R()
        {
            InitializeComponent();
        }

        private void FormRemoteControl560R_Load(object sender, EventArgs e)
        {
            string sApplicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sProfileFolder = Path.Combine(sApplicationFolder, "APSRemoteReceiver");
           
            Thread.Sleep(1000);

            bool bFailToRead = true;
            do
            {
                try
                {
                    StreamReader fileSettings = new StreamReader(sProfileFolder + "\\Settings.txt");
                    string sContents = fileSettings.ReadToEnd();
                    fileSettings.Close();
                    textBoxContents.Text = sContents;
                    textBoxContents.SelectionStart = textBoxContents.Text.Length;
                    bFailToRead = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Read Settings.txt Error: " + ex.Message);
                    Thread.Sleep(500);
                }
            } while (bFailToRead);                      
        }        
    }
}
