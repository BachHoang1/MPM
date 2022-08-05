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
    public partial class FormReceiverSetting : Form
    {
        private string m_sReceiverType;
        private CWidgetInfoLookupTable m_WidgetInfoLookup;

        public FormReceiverSetting(ref CWidgetInfoLookupTable widgetInfoLookup_)
        {
            InitializeComponent();
            m_WidgetInfoLookup = widgetInfoLookup_;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_WidgetInfoLookup.SetValue(this.Name, comboBoxReceiverType.Name, comboBoxReceiverType.SelectedIndex.ToString());
            m_WidgetInfoLookup.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormReceiverSetting_Load(object sender, EventArgs e)
        {
            //WidgetInfoLookupTbl = new CWidgetInfoLookupTable();
            //WidgetInfoLookupTbl.Load();

            m_sReceiverType = m_WidgetInfoLookup.GetValue(this.Name, comboBoxReceiverType.Name);
            comboBoxReceiverType.SelectedIndex = Convert.ToInt32(m_sReceiverType);
        }

        public string GetReceiverType()
        {
            return m_sReceiverType;
        }
    }
}
