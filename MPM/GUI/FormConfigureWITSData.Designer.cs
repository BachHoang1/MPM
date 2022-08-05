namespace MPM.GUI
{
    partial class FormConfigureWITSData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigureWITSData));
            this.dataGridViewWITSRecords = new System.Windows.Forms.DataGridView();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonReloadDefaults = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWITSRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewDPoint
            // 
            this.dataGridViewWITSRecords.AllowUserToAddRows = false;
            this.dataGridViewWITSRecords.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewWITSRecords.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewWITSRecords.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewWITSRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewWITSRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewWITSRecords.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewWITSRecords.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewWITSRecords.GridColor = System.Drawing.Color.White;
            this.dataGridViewWITSRecords.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewWITSRecords.MultiSelect = false;
            this.dataGridViewWITSRecords.Name = "dataGridViewDPoint";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewWITSRecords.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.dataGridViewWITSRecords.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewWITSRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewWITSRecords.Size = new System.Drawing.Size(971, 338);
            this.dataGridViewWITSRecords.TabIndex = 1;
            this.dataGridViewWITSRecords.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDPoint_CellLeave);
            this.dataGridViewWITSRecords.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDPoint_CellValidated);
            this.dataGridViewWITSRecords.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewDPoint_CellValidating);
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(423, 365);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 48);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonReloadDefaults
            // 
            this.buttonReloadDefaults.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonReloadDefaults.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReloadDefaults.ForeColor = System.Drawing.Color.Black;
            this.buttonReloadDefaults.Location = new System.Drawing.Point(23, 365);
            this.buttonReloadDefaults.Name = "buttonReloadDefaults";
            this.buttonReloadDefaults.Size = new System.Drawing.Size(78, 48);
            this.buttonReloadDefaults.TabIndex = 11;
            this.buttonReloadDefaults.Text = "Reload Defaults";
            this.buttonReloadDefaults.UseVisualStyleBackColor = false;
            this.buttonReloadDefaults.Click += new System.EventHandler(this.buttonReloadDefaults_Click);
            // 
            // FormConfigureWITSData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(971, 437);
            this.Controls.Add(this.buttonReloadDefaults);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dataGridViewWITSRecords);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigureWITSData";
            this.Text = "Configure WITS Data";
            this.Load += new System.EventHandler(this.FormConfigureWITSData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWITSRecords)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewWITSRecords;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonReloadDefaults;
    }
}