namespace MPM.GUI
{
    partial class FormUnitSelection
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();                
            }
            else
            {
                
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUnitSelection));
            this.tableLayoutPanelUnitSelection = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonByEachDPoint = new System.Windows.Forms.RadioButton();
            this.radioButtonImperialToMetric = new System.Windows.Forms.RadioButton();
            this.radioButtonMetricToImperial = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.dataGridViewByEachDPoint = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonByDimension = new System.Windows.Forms.RadioButton();
            this.buttonEditUnitGroup = new System.Windows.Forms.Button();
            this.tableLayoutPanelUnitSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByEachDPoint)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelUnitSelection
            // 
            this.tableLayoutPanelUnitSelection.ColumnCount = 3;
            this.tableLayoutPanelUnitSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanelUnitSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94F));
            this.tableLayoutPanelUnitSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanelUnitSelection.Controls.Add(this.buttonOK, 1, 3);
            this.tableLayoutPanelUnitSelection.Controls.Add(this.dataGridViewByEachDPoint, 1, 2);
            this.tableLayoutPanelUnitSelection.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.tableLayoutPanelUnitSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUnitSelection.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelUnitSelection.Name = "tableLayoutPanelUnitSelection";
            this.tableLayoutPanelUnitSelection.RowCount = 4;
            this.tableLayoutPanelUnitSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanelUnitSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanelUnitSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUnitSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelUnitSelection.Size = new System.Drawing.Size(710, 533);
            this.tableLayoutPanelUnitSelection.TabIndex = 0;
            // 
            // radioButtonByEachDPoint
            // 
            this.radioButtonByEachDPoint.AutoSize = true;
            this.radioButtonByEachDPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonByEachDPoint.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonByEachDPoint.ForeColor = System.Drawing.Color.White;
            this.radioButtonByEachDPoint.Location = new System.Drawing.Point(3, 83);
            this.radioButtonByEachDPoint.Name = "radioButtonByEachDPoint";
            this.radioButtonByEachDPoint.Size = new System.Drawing.Size(157, 34);
            this.radioButtonByEachDPoint.TabIndex = 6;
            this.radioButtonByEachDPoint.TabStop = true;
            this.radioButtonByEachDPoint.Text = "By Each D-Point";
            this.radioButtonByEachDPoint.UseVisualStyleBackColor = true;
            this.radioButtonByEachDPoint.CheckedChanged += new System.EventHandler(this.radioButtonByEachDPoint_CheckedChanged);
            // 
            // radioButtonImperialToMetric
            // 
            this.radioButtonImperialToMetric.AutoSize = true;
            this.radioButtonImperialToMetric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonImperialToMetric.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonImperialToMetric.ForeColor = System.Drawing.Color.White;
            this.radioButtonImperialToMetric.Location = new System.Drawing.Point(3, 3);
            this.radioButtonImperialToMetric.Name = "radioButtonImperialToMetric";
            this.radioButtonImperialToMetric.Size = new System.Drawing.Size(157, 34);
            this.radioButtonImperialToMetric.TabIndex = 3;
            this.radioButtonImperialToMetric.TabStop = true;
            this.radioButtonImperialToMetric.Text = "Imperial to Metric";
            this.radioButtonImperialToMetric.UseVisualStyleBackColor = true;
            this.radioButtonImperialToMetric.CheckedChanged += new System.EventHandler(this.radioButtonImperialToMetric_CheckedChanged);
            // 
            // radioButtonMetricToImperial
            // 
            this.radioButtonMetricToImperial.AutoSize = true;
            this.radioButtonMetricToImperial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonMetricToImperial.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonMetricToImperial.ForeColor = System.Drawing.Color.White;
            this.radioButtonMetricToImperial.Location = new System.Drawing.Point(3, 43);
            this.radioButtonMetricToImperial.Name = "radioButtonMetricToImperial";
            this.radioButtonMetricToImperial.Size = new System.Drawing.Size(157, 34);
            this.radioButtonMetricToImperial.TabIndex = 4;
            this.radioButtonMetricToImperial.TabStop = true;
            this.radioButtonMetricToImperial.Text = "Metric to Imperial";
            this.radioButtonMetricToImperial.UseVisualStyleBackColor = true;
            this.radioButtonMetricToImperial.CheckedChanged += new System.EventHandler(this.radioButtonMetricToImperial_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.Black;
            this.buttonOK.Location = new System.Drawing.Point(325, 465);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(85, 56);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // dataGridViewByEachDPoint
            // 
            this.dataGridViewByEachDPoint.AllowUserToAddRows = false;
            this.dataGridViewByEachDPoint.AllowUserToDeleteRows = false;
            this.dataGridViewByEachDPoint.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewByEachDPoint.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewByEachDPoint.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewByEachDPoint.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewByEachDPoint.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewByEachDPoint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewByEachDPoint.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewByEachDPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewByEachDPoint.GridColor = System.Drawing.Color.White;
            this.dataGridViewByEachDPoint.Location = new System.Drawing.Point(24, 178);
            this.dataGridViewByEachDPoint.MultiSelect = false;
            this.dataGridViewByEachDPoint.Name = "dataGridViewByEachDPoint";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewByEachDPoint.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.dataGridViewByEachDPoint.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewByEachDPoint.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewByEachDPoint.Size = new System.Drawing.Size(661, 272);
            this.dataGridViewByEachDPoint.TabIndex = 11;
            this.dataGridViewByEachDPoint.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewByEachDPoint_CellValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.81089F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.18911F));
            this.tableLayoutPanel1.Controls.Add(this.radioButtonByEachDPoint, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonByDimension, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonMetricToImperial, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonImperialToMetric, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonEditUnitGroup, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(661, 154);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // radioButtonByDimension
            // 
            this.radioButtonByDimension.AutoSize = true;
            this.radioButtonByDimension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonByDimension.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonByDimension.ForeColor = System.Drawing.Color.White;
            this.radioButtonByDimension.Location = new System.Drawing.Point(3, 123);
            this.radioButtonByDimension.Name = "radioButtonByDimension";
            this.radioButtonByDimension.Size = new System.Drawing.Size(157, 34);
            this.radioButtonByDimension.TabIndex = 12;
            this.radioButtonByDimension.Text = "By Each Unit Group";
            this.radioButtonByDimension.UseVisualStyleBackColor = true;
            this.radioButtonByDimension.CheckedChanged += new System.EventHandler(this.radioButtonByDimension_CheckedChanged);
            // 
            // buttonEditUnitGroup
            // 
            this.buttonEditUnitGroup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonEditUnitGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonEditUnitGroup.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEditUnitGroup.Location = new System.Drawing.Point(166, 123);
            this.buttonEditUnitGroup.Name = "buttonEditUnitGroup";
            this.buttonEditUnitGroup.Size = new System.Drawing.Size(75, 33);
            this.buttonEditUnitGroup.TabIndex = 13;
            this.buttonEditUnitGroup.Text = "Edit";
            this.buttonEditUnitGroup.UseVisualStyleBackColor = false;
            this.buttonEditUnitGroup.Click += new System.EventHandler(this.buttonEditUnitGroup_Click);
            // 
            // FormUnitSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(710, 533);
            this.Controls.Add(this.tableLayoutPanelUnitSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormUnitSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unit Selection";
            this.Load += new System.EventHandler(this.FormUnitSelection_Load);
            this.tableLayoutPanelUnitSelection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByEachDPoint)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUnitSelection;
        private System.Windows.Forms.RadioButton radioButtonImperialToMetric;
        private System.Windows.Forms.RadioButton radioButtonByEachDPoint;
        private System.Windows.Forms.RadioButton radioButtonMetricToImperial;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridView dataGridViewByEachDPoint;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButtonByDimension;
        private System.Windows.Forms.Button buttonEditUnitGroup;
    }
}