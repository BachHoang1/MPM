namespace MPM.GUI
{
    partial class FormRealTimeDetectLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRealTimeDetectLog));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxEM = new System.Windows.Forms.TextBox();
            this.textBoxMudPulse = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonSearchForward = new System.Windows.Forms.Button();
            this.buttonSearchBackward = new System.Windows.Forms.Button();
            this.buttonScroll = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 63);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxEM);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxMudPulse);
            this.splitContainer1.Size = new System.Drawing.Size(603, 454);
            this.splitContainer1.SplitterDistance = 307;
            this.splitContainer1.TabIndex = 0;
            // 
            // textBoxEM
            // 
            this.textBoxEM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.textBoxEM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEM.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.textBoxEM.Location = new System.Drawing.Point(0, 0);
            this.textBoxEM.Multiline = true;
            this.textBoxEM.Name = "textBoxEM";
            this.textBoxEM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEM.Size = new System.Drawing.Size(307, 454);
            this.textBoxEM.TabIndex = 0;
            this.textBoxEM.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxEM_MouseClick);
            // 
            // textBoxMudPulse
            // 
            this.textBoxMudPulse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.textBoxMudPulse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMudPulse.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMudPulse.ForeColor = System.Drawing.Color.Magenta;
            this.textBoxMudPulse.Location = new System.Drawing.Point(0, 0);
            this.textBoxMudPulse.Multiline = true;
            this.textBoxMudPulse.Name = "textBoxMudPulse";
            this.textBoxMudPulse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMudPulse.Size = new System.Drawing.Size(292, 454);
            this.textBoxMudPulse.TabIndex = 0;
            this.textBoxMudPulse.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxMudPulse_MouseClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(609, 520);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxSearch, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSearchForward, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSearchBackward, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonScroll, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(603, 54);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearch.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSearch.Location = new System.Drawing.Point(3, 3);
            this.textBoxSearch.Multiline = true;
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(426, 48);
            this.textBoxSearch.TabIndex = 2;
            this.toolTips.SetToolTip(this.textBoxSearch, "Enter your search text then click on the search forward or backwards button to fi" +
        "nd the text.");
            // 
            // buttonSearchForward
            // 
            this.buttonSearchForward.BackColor = System.Drawing.Color.White;
            this.buttonSearchForward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearchForward.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSearchForward.ForeColor = System.Drawing.Color.White;
            this.buttonSearchForward.Image = ((System.Drawing.Image)(resources.GetObject("buttonSearchForward.Image")));
            this.buttonSearchForward.Location = new System.Drawing.Point(435, 3);
            this.buttonSearchForward.Name = "buttonSearchForward";
            this.buttonSearchForward.Size = new System.Drawing.Size(50, 48);
            this.buttonSearchForward.TabIndex = 3;
            this.toolTips.SetToolTip(this.buttonSearchForward, "Search forward for text that was entered in the text field on the left. \r\nScrolli" +
        "ng automatically stops when you click search.");
            this.buttonSearchForward.UseVisualStyleBackColor = false;
            this.buttonSearchForward.Click += new System.EventHandler(this.buttonSearchForward_Click);
            // 
            // buttonSearchBackward
            // 
            this.buttonSearchBackward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.buttonSearchBackward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearchBackward.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSearchBackward.ForeColor = System.Drawing.Color.White;
            this.buttonSearchBackward.Image = ((System.Drawing.Image)(resources.GetObject("buttonSearchBackward.Image")));
            this.buttonSearchBackward.Location = new System.Drawing.Point(491, 3);
            this.buttonSearchBackward.Name = "buttonSearchBackward";
            this.buttonSearchBackward.Size = new System.Drawing.Size(53, 48);
            this.buttonSearchBackward.TabIndex = 4;
            this.toolTips.SetToolTip(this.buttonSearchBackward, "Searches backwards for text that was entered in the text field on the left.\r\nScro" +
        "lling automatically stops when you click search.");
            this.buttonSearchBackward.UseVisualStyleBackColor = false;
            this.buttonSearchBackward.Click += new System.EventHandler(this.buttonSearchBackward_Click);
            // 
            // buttonScroll
            // 
            this.buttonScroll.BackColor = System.Drawing.Color.White;
            this.buttonScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonScroll.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonScroll.ForeColor = System.Drawing.Color.Black;
            this.buttonScroll.Image = global::MPM.Properties.Resources.scrollingStop;
            this.buttonScroll.Location = new System.Drawing.Point(550, 3);
            this.buttonScroll.Name = "buttonScroll";
            this.buttonScroll.Size = new System.Drawing.Size(50, 48);
            this.buttonScroll.TabIndex = 1;
            this.toolTips.SetToolTip(this.buttonScroll, "Stop or resume scrolling of the incoming raw text from Detect.");
            this.buttonScroll.UseVisualStyleBackColor = false;
            this.buttonScroll.Click += new System.EventHandler(this.buttonScroll_Click);
            // 
            // FormRealTimeDetectLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(609, 520);
            this.Controls.Add(this.tableLayoutPanel1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRealTimeDetectLog";
            this.Text = "Real-Time Detect Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRealTimeDetectLog_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxEM;
        private System.Windows.Forms.TextBox textBoxMudPulse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonScroll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonSearchForward;
        private System.Windows.Forms.Button buttonSearchBackward;
        private System.Windows.Forms.ToolTip toolTips;
    }
}