// author: hoan chau
// purpose: display all the pending messages by the server sent to a client

using MPM.Data;
using MPM.DataAcquisition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MPM.DataAcquisition.CServerMessage;

namespace MPM.GUI
{
    public partial class FormServerMessage : Form
    {
        private enum COLUMNS {TYPE = 0, ACCEPT, REJECT, DETAIL}  // should match the grid column numbers

        private List<CServerMessage.MESSAGE_REC> m_lstMessages;
        private List<CServerMessage.MESSAGE_REC> m_lstMessagesChanged;
        private List<CServer.SENT_PACKET_REC> m_lstSentMessages;

        private bool m_bServerMode;
        public FormServerMessage()
        {
            InitializeComponent();
            m_lstMessages = new List<CServerMessage.MESSAGE_REC>();
            m_bServerMode = false;
        }

        public void SetServerMode()
        {
            m_bServerMode = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!m_bServerMode)
            {
                m_lstMessagesChanged = new List<MESSAGE_REC>();

                for (int i = 0; i < dataGridViewMessages.Rows.Count; i++)
                {
                    if (m_lstMessages.Count == 0)  // strange that grid has rows but this list is empty
                        break;

                    MESSAGE_REC rec = new MESSAGE_REC();
                    rec.ID = m_lstMessages[i].ID;
                    rec.sID = m_lstMessages[i].sID;
                    rec.bAccept = System.Convert.ToBoolean(dataGridViewMessages.Rows[i].Cells[(int)COLUMNS.ACCEPT].Value);
                    rec.bReject = System.Convert.ToBoolean(dataGridViewMessages.Rows[i].Cells[(int)COLUMNS.REJECT].Value);
                    rec.sData = m_lstMessages[i].sData;

                    if (rec.bAccept != m_lstMessages[i].bAccept ||
                        rec.bReject != m_lstMessages[i].bReject)  // don't add to list if user doesn't make a choice                
                        m_lstMessagesChanged.Add(rec);
                }
            }
                
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<MESSAGE_REC> GetChanges()
        {
            return m_lstMessagesChanged;
        }

        private void FormServerMessage_Load(object sender, EventArgs e)
        {
            if (m_bServerMode)
            {
                dataGridViewMessages.Columns.Add("Date", "Date");
                dataGridViewMessages.Columns.Add("Type", "Type");
                dataGridViewMessages.Columns.Add("Sent To", "Sent To");
                dataGridViewMessages.Columns.Add("Received", "Received");
                dataGridViewMessages.Columns.Add("Retries", "Retries");
                dataGridViewMessages.Columns.Add("Packet Details", "Packet Details");

                for (int i = 0; i < m_lstSentMessages.Count; i++)
                {
                    CCommonTypes.SERVER_PACKET_ID svrPacketID = CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_JOB_OR_RIG_DESCRIPTION;
                    string [] sArrCols = m_lstSentMessages[i].sData.Split(';');
                    if (sArrCols.Length > 0)
                    {
                        string[] sArrData = sArrCols[0].Split('=');
                        if (sArrData[0] == CCommonTypes.PACKET_ID)
                        {
                            svrPacketID = (CCommonTypes.SERVER_PACKET_ID)System.Convert.ToInt32(sArrData[1]);
                        }
                    }
                    dataGridViewMessages.Rows.Add(m_lstSentMessages[i].sDateTime, 
                        svrPacketID.ToString(), 
                        m_lstSentMessages[i].sClientIPAddress, 
                        m_lstSentMessages[i].bAcked,
                        m_lstSentMessages[i].iRetries,
                        m_lstSentMessages[i].sData);
                }

                dataGridViewMessages.AutoResizeColumns();
                for (int k = 0; k < dataGridViewMessages.Columns.Count; k++)
                {
                    dataGridViewMessages.Columns[k].ReadOnly = true;                    
                }
            }
            else
            {
                dataGridViewMessages.Columns.Add("Type", "Type");
                dataGridViewMessages.Columns.Add("Packet Details", "Packet Details");

                DataGridViewCheckBoxColumn colReject = new DataGridViewCheckBoxColumn();
                colReject.HeaderText = "Reject";
                colReject.FalseValue = false;
                colReject.TrueValue = true;
                dataGridViewMessages.Columns.Insert(1, colReject);

                DataGridViewCheckBoxColumn colAccept = new DataGridViewCheckBoxColumn();
                colAccept.HeaderText = "Accept";
                colAccept.FalseValue = false;
                colAccept.TrueValue = true;
                dataGridViewMessages.Columns.Insert(1, colAccept);


                for (int i = 0; i < m_lstMessages.Count; i++)
                    dataGridViewMessages.Rows.Add(m_lstMessages[i].sID, m_lstMessages[i].bAccept, m_lstMessages[i].bReject, m_lstMessages[i].sData);

                // additional settings on the grid
                dataGridViewMessages.AutoResizeColumns();
                dataGridViewMessages.Columns[(int)COLUMNS.TYPE].ReadOnly = true;
                dataGridViewMessages.Columns[(int)COLUMNS.DETAIL].ReadOnly = true;
                foreach (DataGridViewColumn column in dataGridViewMessages.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
                       
        }

        public void SetMessages(List<CServerMessage.MESSAGE_REC> lst_)
        {
            m_lstMessages = lst_;
        } 
        
        public void SetSentMessages(List<CServer.SENT_PACKET_REC> lst_)
        {
            m_lstSentMessages = lst_;
        }

        private void dataGridViewMessages_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)COLUMNS.ACCEPT || e.ColumnIndex == (int)COLUMNS.REJECT)
            {
                try
                {
                    string s = dataGridViewMessages.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (e.ColumnIndex == (int)COLUMNS.REJECT && s.ToLower() == "true") // check that the accept column is false
                    {
                        string t = dataGridViewMessages.Rows[e.RowIndex].Cells[(int)COLUMNS.ACCEPT].Value.ToString();
                        if (t.ToLower() == "true")  // can't have both accept and reject being checked
                            dataGridViewMessages.Rows[e.RowIndex].Cells[(int)COLUMNS.ACCEPT].Value = false;

                    }
                    else if (e.ColumnIndex == (int)COLUMNS.ACCEPT && s.ToLower() == "true")
                    {
                        string t = dataGridViewMessages.Rows[e.RowIndex].Cells[(int)COLUMNS.REJECT].Value.ToString();
                        if (t.ToLower() == "true")  // can't have both accept and reject being checked
                            dataGridViewMessages.Rows[e.RowIndex].Cells[(int)COLUMNS.REJECT].Value = false;
                    }                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }
    }
}
