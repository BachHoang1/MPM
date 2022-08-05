namespace MPM.GUI
{
    partial class FormLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLog));
            this.dataGridViewLog = new System.Windows.Forms.DataGridView();
            this.contextMenuStripLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).BeginInit();
            this.contextMenuStripLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewLog
            // 
            this.dataGridViewLog.AllowUserToAddRows = false;
            this.dataGridViewLog.AllowUserToDeleteRows = false;
            this.dataGridViewLog.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
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
            this.dataGridViewLog.ContextMenuStrip = this.contextMenuStripLog;
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
            this.dataGridViewLog.Name = "dataGridViewLog";
            this.dataGridViewLog.ReadOnly = true;
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
            this.dataGridViewLog.RowTemplate.ReadOnly = true;
            this.dataGridViewLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLog.Size = new System.Drawing.Size(670, 485);
            this.dataGridViewLog.TabIndex = 1;
            this.dataGridViewLog.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLog_CellClick);
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterToolStripMenuItem,
            this.exportToCSVToolStripMenuItem});
            this.contextMenuStripLog.Name = "contextMenuStripLog";
            this.contextMenuStripLog.Size = new System.Drawing.Size(146, 48);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.filterToolStripMenuItem.Text = "Filter";
            this.filterToolStripMenuItem.Click += new System.EventHandler(this.filterToolStripMenuItem_Click);
            // 
            // exportToCSVToolStripMenuItem
            // 
            this.exportToCSVToolStripMenuItem.Name = "exportToCSVToolStripMenuItem";
            this.exportToCSVToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.exportToCSVToolStripMenuItem.Text = "Export to CSV";
            this.exportToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportToCSVToolStripMenuItem_Click);
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 485);
            this.Controls.Add(this.dataGridViewLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLog";
            this.Text = "Logged Data from Detect";
            this.Load += new System.EventHandler(this.FormLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).EndInit();
            this.contextMenuStripLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLog;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToCSVToolStripMenuItem;
    }
}