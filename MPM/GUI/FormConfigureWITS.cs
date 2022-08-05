using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPM.Data;
using MPM.DataAcquisition;

namespace MPM.GUI
{
    public partial class FormConfigureWITS : Form
    {
        CWidgetInfoLookupTable m_WidgetInfoLookup;
        

        private int m_iPingInterval;
        private int m_iPreviousPasonPort;
        private int m_iPreviousOutgoingPort;
        private int m_iPasonPort;  // incoming and outgoing
        private int m_iOutgoingPort;  // strictly outgoing
        private CWITSLookupTable m_lookupWITS;
        private CPason m_PortPason;
        private CPason m_PortOutgoing;

        public FormConfigureWITS()
        {
            InitializeComponent();
            m_iPingInterval = 0;
        }

        public void SetPingInterval(int iVal_)
        {
            m_iPingInterval = iVal_;
        }

        public void SetWITSLookUp(CWITSLookupTable lookupWITS_)
        {
            m_lookupWITS = lookupWITS_;
        }

        public void SetWidgetInfoLookup(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            m_WidgetInfoLookup = widgetInfoLookup_;
        }

        public void SetCOMS(ref CPason m_Pason, ref CPason m_OutgoingWITS)
        {
            m_PortPason = m_Pason;
            m_PortOutgoing = m_OutgoingWITS;
        }

        public void GetPreviousPortNumbers(out int iPasonPort_, out int iOutgoingPort_)
        {
            iPasonPort_ = m_iPreviousPasonPort;
            iOutgoingPort_ = m_iPreviousOutgoingPort;
        }

        public void GetPortNumbers(out int iPasonPort_, out int iOutgoingPort_)
        {
            iPasonPort_ = m_iPasonPort;
            iOutgoingPort_ = m_iOutgoingPort;
        }

        private void FormConfigureWITS_Load(object sender, EventArgs e)
        {
            //m_WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //m_WidgetInfoLookupTbl.Load();
            string sVal = m_WidgetInfoLookup.GetValue(this.Name, numericUpDownCOMPort.Name);
            int iVal = m_iPreviousPasonPort = System.Convert.ToInt16(sVal);
            if (iVal < numericUpDownCOMPort.Minimum || iVal > numericUpDownCOMPort.Maximum)
                iVal = (Int16)numericUpDownCOMPort.Minimum;
            numericUpDownCOMPort.Value = iVal;

            textBoxPingInterval.Text = m_iPingInterval.ToString();

            sVal = m_WidgetInfoLookup.GetValue(this.Name, numericUpDownCOMPortOut1.Name);
            iVal = m_iPreviousOutgoingPort = System.Convert.ToInt16(sVal);
            if (iVal < numericUpDownCOMPortOut1.Minimum || iVal > numericUpDownCOMPortOut1.Maximum)
                iVal = (Int16)numericUpDownCOMPortOut1.Minimum;
            numericUpDownCOMPortOut1.Value = iVal;

            sVal = m_WidgetInfoLookup.GetValue(this.Name, checkBoxFilterMudPulse.Name);
            bool bVal = System.Convert.ToBoolean(sVal);
            checkBoxFilterMudPulse.Checked = bVal;

            sVal = m_WidgetInfoLookup.GetValue(this.Name, checkBoxFilterEM.Name);
            bVal = System.Convert.ToBoolean(sVal);
            checkBoxFilterEM.Checked = bVal;

            
            DataTable tbl = m_lookupWITS.GetAll();
            AutoCompleteStringCollection data = new AutoCompleteStringCollection();
            AutoCompleteStringCollection dataName = new AutoCompleteStringCollection();
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                data.Add(tbl.Rows[i].Field<string>("ID"));
                comboBoxWITSChannel.Items.Add(tbl.Rows[i].Field<string>("ID"));

                dataName.Add(tbl.Rows[i].Field<string>("Name"));
                comboBoxWITSName.Items.Add(tbl.Rows[i].Field<string>("Name"));
            }
            comboBoxWITSChannel.AutoCompleteCustomSource = data;
            comboBoxWITSChannel.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxWITSChannel.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            comboBoxWITSName.AutoCompleteCustomSource = dataName;
            comboBoxWITSName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxWITSName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (numericUpDownCOMPort.Value == numericUpDownCOMPortOut1.Value)
            {
                if (numericUpDownCOMPort.Value != 0)
                {
                    MessageBox.Show("'From Rig' Port # cannot be the same as the 'Out Only'. Choose different Port #'s.", "Duplicate Port", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }                
            }

            m_iPasonPort = (int)numericUpDownCOMPort.Value;
            m_iOutgoingPort = (int)numericUpDownCOMPortOut1.Value;

            m_WidgetInfoLookup.SetValue(this.Name, numericUpDownCOMPort.Name, (int)numericUpDownCOMPort.Value);

            m_WidgetInfoLookup.SetValue(this.Name, numericUpDownCOMPortOut1.Name, (int)numericUpDownCOMPortOut1.Value);

            m_WidgetInfoLookup.SetValue(this.Name, checkBoxFilterMudPulse.Name, checkBoxFilterMudPulse.Checked.ToString());
            m_WidgetInfoLookup.SetValue(this.Name, checkBoxFilterEM.Name, checkBoxFilterEM.Checked.ToString());

            m_WidgetInfoLookup.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonTestSend_Click(object sender, EventArgs e)
        {
            string sOutput = "&&\r\n" + comboBoxWITSChannel.Text + 
                             textBoxWITSValue.Text + "\r\n!!\r\n";
                             

            // check if the com port changed for WITS Out Only
            if (numericUpDownCOMPortOut1.Value.ToString() != "0")
            {
                if (m_PortOutgoing.GetPort() != "COM" + numericUpDownCOMPortOut1.Value.ToString())
                {
                    m_PortOutgoing.Stop();
                    string sErrMsg = "";
                    bool b = m_PortOutgoing.Start(ref sErrMsg, "COM" + numericUpDownCOMPortOut1.Value.ToString());
                    if (!b)
                    {
                        MessageBox.Show("WITS Out Only Port error:" + sErrMsg + ". Please shut down any applications using port " + numericUpDownCOMPortOut1.Value.ToString() + " and try again.", "WITS Communication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                        m_PortOutgoing.QueueOutgoingPacket(sOutput);

                }
                else
                    m_PortOutgoing.QueueOutgoingPacket(sOutput);
            }
            

            // check if the com port changed for Pason
            if (numericUpDownCOMPort.Value.ToString() != "0")
            {
                if (m_PortPason.GetPort() != "COM" + numericUpDownCOMPort.Value.ToString())
                {
                    m_PortPason.Stop();
                    string sErrMsg = "";
                    bool b = m_PortPason.Start(ref sErrMsg, "COM" + numericUpDownCOMPort.Value.ToString());
                    if (!b)
                    {
                        MessageBox.Show("Pason Port error:" + sErrMsg + ". Please shut down any applications using port " + numericUpDownCOMPort.Value.ToString() + " and try again.", "WITS Communication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                        m_PortPason.QueueOutgoingPacket(sOutput);

                }
                else
                    m_PortPason.QueueOutgoingPacket(sOutput);
            }            
        }

        private void comboBoxWITSChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxWITSName.SelectedIndex = comboBoxWITSChannel.SelectedIndex;
        }

        private void comboBoxWITSName_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxWITSChannel.SelectedIndex = comboBoxWITSName.SelectedIndex;
        }
    }
}
