namespace MPM.GUI
{
    partial class FormSurveyLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSurveyLog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewLog = new System.Windows.Forms.DataGridView();
            this.contextMenuStripSurveyFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.acceptedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rejectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonExportSettings = new System.Windows.Forms.Button();
            this.buttonExportSelected = new System.Windows.Forms.Button();
            this.dataGridViewTemp = new System.Windows.Forms.DataGridView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonResendWITS = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).BeginInit();
            this.contextMenuStripSurveyFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewLog);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.splitContainer1.Panel2.Controls.Add(this.buttonResendWITS);
            this.splitContainer1.Panel2.Controls.Add(this.buttonExportSettings);
            this.splitContainer1.Panel2.Controls.Add(this.buttonExportSelected);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewTemp);
            this.splitContainer1.Panel2.Controls.Add(this.buttonAdd);
            this.splitContainer1.Panel2.Controls.Add(this.buttonRefresh);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDelete);
            this.splitContainer1.Panel2.Controls.Add(this.buttonUpdate);
            this.splitContainer1.Panel2.Controls.Add(this.buttonExport);
            this.splitContainer1.Size = new System.Drawing.Size(921, 500);
            this.splitContainer1.SplitterDistance = 427;
            this.splitContainer1.TabIndex = 3;
            // 
            // dataGridViewLog
            // 
            this.dataGridViewLog.AllowUserToAddRows = false;
            this.dataGridViewLog.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLog.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLog.ContextMenuStrip = this.contextMenuStripSurveyFilter;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewLog.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLog.GridColor = System.Drawing.Color.White;
            this.dataGridViewLog.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLog.MultiSelect = false;
            this.dataGridViewLog.Name = "dataGridViewLog";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.dataGridViewLog.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLog.Size = new System.Drawing.Size(921, 427);
            this.dataGridViewLog.TabIndex = 2;
            this.dataGridViewLog.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLog_CellClick);
            this.dataGridViewLog.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLog_CellDoubleClick);
            this.dataGridViewLog.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewLog_ColumnHeaderMouseClick);
            // 
            // contextMenuStripSurveyFilter
            // 
            this.contextMenuStripSurveyFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acceptedToolStripMenuItem,
            this.rejectedToolStripMenuItem,
            this.noneToolStripMenuItem,
            this.allToolStripMenuItem});
            this.contextMenuStripSurveyFilter.Name = "contextMenuStripSurveyFilter";
            this.contextMenuStripSurveyFilter.Size = new System.Drawing.Size(112, 92);
            // 
            // acceptedToolStripMenuItem
            // 
            this.acceptedToolStripMenuItem.Name = "acceptedToolStripMenuItem";
            this.acceptedToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.acceptedToolStripMenuItem.Text = "Accept";
            this.acceptedToolStripMenuItem.Click += new System.EventHandler(this.acceptedToolStripMenuItem_Click);
            // 
            // rejectedToolStripMenuItem
            // 
            this.rejectedToolStripMenuItem.Name = "rejectedToolStripMenuItem";
            this.rejectedToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.rejectedToolStripMenuItem.Text = "Reject";
            this.rejectedToolStripMenuItem.Click += new System.EventHandler(this.rejectedToolStripMenuItem_Click);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.noneToolStripMenuItem.Text = "None";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // buttonExportSettings
            // 
            this.buttonExportSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonExportSettings.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExportSettings.ForeColor = System.Drawing.Color.Black;
            this.buttonExportSettings.Location = new System.Drawing.Point(663, 10);
            this.buttonExportSettings.Name = "buttonExportSettings";
            this.buttonExportSettings.Size = new System.Drawing.Size(120, 48);
            this.buttonExportSettings.TabIndex = 17;
            this.buttonExportSettings.Text = "Export Settings";
            this.buttonExportSettings.UseVisualStyleBackColor = false;
            this.buttonExportSettings.Click += new System.EventHandler(this.buttonExportSettings_Click);
            // 
            // buttonExportSelected
            // 
            this.buttonExportSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonExportSelected.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExportSelected.ForeColor = System.Drawing.Color.Black;
            this.buttonExportSelected.Location = new System.Drawing.Point(136, 10);
            this.buttonExportSelected.Name = "buttonExportSelected";
            this.buttonExportSelected.Size = new System.Drawing.Size(120, 48);
            this.buttonExportSelected.TabIndex = 16;
            this.buttonExportSelected.Text = "Export Selected";
            this.buttonExportSelected.UseVisualStyleBackColor = false;
            this.buttonExportSelected.Click += new System.EventHandler(this.buttonExportSelected_Click);
            // 
            // dataGridViewTemp
            // 
            this.dataGridViewTemp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTemp.Location = new System.Drawing.Point(707, 37);
            this.dataGridViewTemp.Name = "dataGridViewTemp";
            this.dataGridViewTemp.Size = new System.Drawing.Size(132, 55);
            this.dataGridViewTemp.TabIndex = 15;
            this.dataGridViewTemp.Visible = false;
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonAdd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.ForeColor = System.Drawing.Color.Black;
            this.buttonAdd.Location = new System.Drawing.Point(566, 10);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(78, 48);
            this.buttonAdd.TabIndex = 14;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonRefresh.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRefresh.ForeColor = System.Drawing.Color.Black;
            this.buttonRefresh.Location = new System.Drawing.Point(275, 10);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(78, 48);
            this.buttonRefresh.TabIndex = 13;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = false;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDelete.ForeColor = System.Drawing.Color.Black;
            this.buttonDelete.Location = new System.Drawing.Point(469, 10);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(78, 48);
            this.buttonDelete.TabIndex = 12;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonUpdate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpdate.ForeColor = System.Drawing.Color.Black;
            this.buttonUpdate.Location = new System.Drawing.Point(372, 10);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(78, 48);
            this.buttonUpdate.TabIndex = 11;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = false;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonExport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExport.ForeColor = System.Drawing.Color.Black;
            this.buttonExport.Location = new System.Drawing.Point(12, 10);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(105, 48);
            this.buttonExport.TabIndex = 10;
            this.buttonExport.Text = "Export Range";
            this.buttonExport.UseVisualStyleBackColor = false;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonResendWITS
            // 
            this.buttonResendWITS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonResendWITS.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonResendWITS.ForeColor = System.Drawing.Color.Black;
            this.buttonResendWITS.Location = new System.Drawing.Point(801, 10);
            this.buttonResendWITS.Name = "buttonResendWITS";
            this.buttonResendWITS.Size = new System.Drawing.Size(110, 48);
            this.buttonResendWITS.TabIndex = 18;
            this.buttonResendWITS.Text = "Resend WITS";
            this.buttonResendWITS.UseVisualStyleBackColor = false;
            this.buttonResendWITS.Click += new System.EventHandler(this.buttonResendWITS_Click);
            // 
            // FormSurveyLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 500);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "FormSurveyLog";
            this.Text = "Survey Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSurveyLog_FormClosing);
            this.Load += new System.EventHandler(this.FormSurveyLog_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).EndInit();
            this.contextMenuStripSurveyFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTemp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.DataGridView dataGridViewLog;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.DataGridView dataGridViewTemp;
        private System.Windows.Forms.Button buttonExportSelected;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSurveyFilter;
        private System.Windows.Forms.ToolStripMenuItem acceptedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rejectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.Button buttonExportSettings;
        private System.Windows.Forms.Button buttonResendWITS;
    }
}