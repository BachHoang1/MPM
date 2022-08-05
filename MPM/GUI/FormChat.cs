using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MPM.DataAcquisition.MultiStationAnalysis;

namespace MPM.GUI
{
    public partial class FormChat : Form
    {        
        private const string HUB_NAME = "SubscribeAuth";
        private const string URL_SUFFIX = "SurveyChatHub";

        private HubConnection m_chatConn;

        private string m_sURL = "";
        private string m_sAPIKey = ""; //"OQA=-D5ADB5BF-664E-44A9-813F-EE7DE399A459";

        private Thread reconnectThread;
        private bool m_bUnload;

        private delegate void GotMessageDelegate(IEnumerable<CMSAChatMessage> message);
        private GotMessageDelegate gotMessageDelegate;

        public FormChat()
        {
            InitializeComponent();
            m_bUnload = false;
            gotMessageDelegate += new GotMessageDelegate(GotMessage);
        }

        void GotMessage(IEnumerable<CMSAChatMessage> message)
        {
            for (int i = 0; i < message.Count(); i++)
            {
                //messagesList.Items.Add(message.ElementAt(i).From + ":  " + message.ElementAt(i).GetMessageString());
                textBoxMessageHistory.AppendText(message.ElementAt(i).From + ":  " + message.ElementAt(i).GetMessageString() + "\r\n");
            }

            
            textBoxMessageHistory.ScrollToCaret();
            //messagesList.SelectedIndex = messagesList.Items.Count - 1;

            UpdateState(connected: true);

            messageTextBox.Focus();

            if (message.Count() > 0)
            {
                notifyIconRoundLab.ShowBalloonTip(1000, "MSA Chat", message.ElementAt(message.Count() - 1).MessageString, ToolTipIcon.Info);
                this.WindowState = FormWindowState.Normal;
                this.Show();
            }
                          
        }

        ~FormChat()
        {
            m_bUnload = true;
        }

        public void SetInfo(string sURL_, string sAPIKey_)
        {
            m_sURL = sURL_ + "/" + URL_SUFFIX;
            m_sAPIKey = sAPIKey_;            
        }

        public void Register()
        {
            //Connect();
            reconnectThread = new Thread(Reconnect);
            reconnectThread.Start();
        }

        private void ListenForNewMessage()
        {
            try
            {
                m_chatConn.On<IEnumerable<CMSAChatMessage>, int>("SurveyChatMessages", (message, jobId) =>
                {
                    this.Invoke(gotMessageDelegate, message);
                    //Console.WriteLine(string.Format(@"SurveyChatMessages event Returned {0} cached messages", message.Count()));
                    //for (int i = 0; i < message.Count(); i++)
                    //    messagesList.Items.Add(message.ElementAt(i).From + ":  " + message.ElementAt(i).GetMessageString());

                    //messagesList.SelectedIndex = messagesList.Items.Count - 1;
                    //notifyIconRoundLab.ShowBalloonTip(1000, "MSA Chat", message.ElementAt(message.Count() - 1).MessageString, ToolTipIcon.Info);
                    //this.WindowState = FormWindowState.Normal;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in FormChat::ListenForNewMessage: " + ex.Message);
            }
            
        }

        private async void ListenForCachedMessages()
        {
            try
            {
                IEnumerable<CMSAChatMessage> messages = await m_chatConn.InvokeAsync<IEnumerable<CMSAChatMessage>>(HUB_NAME, m_sAPIKey);
                Console.WriteLine(string.Format(@"Subscribe Returned {0} cached messages", messages.Count()));
                this.Invoke(gotMessageDelegate, messages);
                //for (int i = 0; i < messages.Count(); i++)                
                //    messagesList.Items.Add(messages.ElementAt(i).MessageString);

                //messagesList.SelectedIndex = messagesList.Items.Count - 1;

                //notifyIconRoundLab.ShowBalloonTip(1000, "MSA Chat", messages.ElementAt(messages.Count() - 1).MessageString, ToolTipIcon.Info);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in FormChat::ListenForCachedMessages: " + ex.Message);
            }
        }
        

        public async void Connect()
        {
            //UpdateState(connected: false);

            m_chatConn = new HubConnectionBuilder()
                   .WithUrl(m_sURL)
                   .WithAutomaticReconnect(new TimeSpan[] { TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(25) })
                   .Build();

            //m_chatConn.On<IEnumerable<CMSAChatMessage>, int>("SurveyChatMessages", (message, jobId) =>
            //{
            //    this.Invoke(gotMessageDelegate, message);
            //    //Console.WriteLine(string.Format(@"SurveyChatMessages event Returned {0} cached messages", message.Count()));
            //    //for (int i = 0; i < message.Count(); i++)
            //    //    messagesList.Items.Add(message.ElementAt(i).From + ":  " + message.ElementAt(i).GetMessageString());
                
            //    //messagesList.SelectedIndex = messagesList.Items.Count - 1;
            //    //notifyIconRoundLab.ShowBalloonTip(1000, "MSA Chat", message.ElementAt(message.Count() - 1).MessageString, ToolTipIcon.Info);
            //    //this.WindowState = FormWindowState.Normal;
            //});

            try
            {
                await m_chatConn.StartAsync();
                ListenForNewMessage();
                ListenForCachedMessages();
                //this.WindowState = FormWindowState.Minimized;
                //this.Show();                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in FormChat::Connect: " + ex.Message);
                
            }

            //try
            //{
            //    IEnumerable<CMSAChatMessage> messages = await m_chatConn.InvokeAsync<IEnumerable<CMSAChatMessage>>(HUB_NAME, m_sAPIKey);
            //    Console.WriteLine(string.Format(@"Subscribe Returned {0} cached messages", messages.Count()));
            //    this.Invoke(gotMessageDelegate, messages);
            //    //for (int i = 0; i < messages.Count(); i++)                
            //    //    messagesList.Items.Add(messages.ElementAt(i).MessageString);

            //    //messagesList.SelectedIndex = messagesList.Items.Count - 1;

            //    //notifyIconRoundLab.ShowBalloonTip(1000, "MSA Chat", messages.ElementAt(messages.Count() - 1).MessageString, ToolTipIcon.Info);

            //}
            //catch (Exception ex)
            //{

            //}

            //Log(Color.Gray, "Connection established.");

            
        }


        private void Reconnect()
        {
            while (true)
            {
                try
                {
                    if (m_bUnload)
                        break;
                    
                    if (m_chatConn == null)
                    {
                        Connect();
                    }
                    else if (m_chatConn.ConnectionId == null ||
                        m_chatConn.ConnectionId == "")
                    {
                        Connect();
                    }

                    Thread.Sleep(7000);
                }
                catch (TimeoutException) { }
            }
        }

        public void Unload()
        {
            m_bUnload = true;
            if (m_chatConn != null)
                m_chatConn.StopAsync();
        }

        private void OnSend(string name, string message)
        {
            Log(Color.Black, name + ": " + message);            
        }

        private void Log(Color color, string message)
        {
            Action callback = () =>
            {
                messagesList.Items.Add(new LogMessage(color, message));
            };

            Invoke(callback);
        }

        private class LogMessage
        {
            public Color MessageColor { get; }

            public string Content { get; }

            public LogMessage(Color messageColor, string content)
            {
                MessageColor = messageColor;
                Content = content;
            }
        }

        private void UpdateState(bool connected)
        {           
            messageTextBox.Enabled = connected;
            sendButton.Enabled = connected;
        }
        

        public async void Disconnect(bool bReconnect = false)
        {
            //Log(Color.Gray, "Stopping connection...");
            try
            {
                await m_chatConn.StopAsync();
                if (bReconnect)
                    Connect();
            }
            catch (Exception ex)
            {
                //Log(Color.Red, ex.ToString());
                Debug.WriteLine("Error on FormChat::Disconnect " + ex.Message);
            }

            //Log(Color.Gray, "Connection terminated.");

            UpdateState(connected: false);
        }

        private async void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                //await m_chatConn.InvokeAsync("PostChatMessageAuth", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"Test Message {DateTime.Now:g}")), Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Field User")), null, m_sAPIKey);
                await m_chatConn.InvokeAsync("PostChatMessageAuth", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(messageTextBox.Text)), Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Field")), null, m_sAPIKey);                
                messageTextBox.Text = "";
            }
            catch (Exception ex)
            {
                //Log(Color.Red, ex.ToString());
                Debug.WriteLine("Error on FormChat::sendButton_Click " + ex.Message);
            }
        }

        private void FormChat_Load(object sender, EventArgs e)
        {
            Register();
        }       

        private void messagesList_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                var message = messagesList.Items[e.Index];
                e.Graphics.DrawString(
                    message.ToString(),
                    messagesList.Font,
                    new SolidBrush(Color.FromArgb(0, 0, 0)),
                    e.Bounds);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in FormChat::messagesList_DrawItem " + ex.Message);
            }
        }

        private void FormChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void FormChat_Shown(object sender, EventArgs e)
        {            
        }
    }
}
