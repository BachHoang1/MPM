namespace MPM.GUI
{
    partial class FormChat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChat));
            this.sendButton = new System.Windows.Forms.Button();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.messagesList = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelSupportNum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.notifyIconRoundLab = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBoxMessageHistory = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // sendButton
            // 
            this.sendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sendButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sendButton.Enabled = false;
            this.sendButton.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendButton.Location = new System.Drawing.Point(0, 0);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(58, 53);
            this.sendButton.TabIndex = 12;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = false;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // messageTextBox
            // 
            this.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageTextBox.Enabled = false;
            this.messageTextBox.Location = new System.Drawing.Point(0, 0);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(258, 53);
            this.messageTextBox.TabIndex = 11;
            // 
            // messagesList
            // 
            this.messagesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messagesList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.messagesList.FormattingEnabled = true;
            this.messagesList.Location = new System.Drawing.Point(12, 60);
            this.messagesList.Name = "messagesList";
            this.messagesList.Size = new System.Drawing.Size(0, 4);
            this.messagesList.TabIndex = 10;
            this.messagesList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.messagesList_DrawItem);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::MPM.Properties.Resources.RoundLAB_Logo_Red_White;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 56);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // labelSupportNum
            // 
            this.labelSupportNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSupportNum.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSupportNum.ForeColor = System.Drawing.Color.White;
            this.labelSupportNum.Location = new System.Drawing.Point(0, 0);
            this.labelSupportNum.Name = "labelSupportNum";
            this.labelSupportNum.Size = new System.Drawing.Size(180, 25);
            this.labelSupportNum.TabIndex = 15;
            this.labelSupportNum.Text = "Support: 1-844-eddilab ";
            this.labelSupportNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 27);
            this.label2.TabIndex = 16;
            this.label2.Text = "1-844-333-4522";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // notifyIconRoundLab
            // 
            this.notifyIconRoundLab.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIconRoundLab.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconRoundLab.Icon")));
            this.notifyIconRoundLab.Text = "Message from Round Lab";
            this.notifyIconRoundLab.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // textBoxMessageHistory
            // 
            this.textBoxMessageHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessageHistory.Location = new System.Drawing.Point(3, 65);
            this.textBoxMessageHistory.Multiline = true;
            this.textBoxMessageHistory.Name = "textBoxMessageHistory";
            this.textBoxMessageHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessageHistory.Size = new System.Drawing.Size(320, 369);
            this.textBoxMessageHistory.TabIndex = 17;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxMessageHistory, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.70161F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.80645F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.69355F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(326, 496);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 440);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.messageTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sendButton);
            this.splitContainer1.Size = new System.Drawing.Size(320, 53);
            this.splitContainer1.SplitterDistance = 258;
            this.splitContainer1.TabIndex = 18;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(320, 56);
            this.splitContainer2.SplitterDistance = 136;
            this.splitContainer2.TabIndex = 19;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.labelSupportNum);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.label2);
            this.splitContainer3.Size = new System.Drawing.Size(180, 56);
            this.splitContainer3.SplitterDistance = 25;
            this.splitContainer3.TabIndex = 0;
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(326, 496);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.messagesList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormChat";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chat Support";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormChat_FormClosing);
            this.Load += new System.EventHandler(this.FormChat_Load);
            this.Shown += new System.EventHandler(this.FormChat_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.ListBox messagesList;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelSupportNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NotifyIcon notifyIconRoundLab;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBoxMessageHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
    }
}