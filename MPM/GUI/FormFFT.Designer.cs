namespace MPM.GUI
{
    partial class FormFFT
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFFT));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVoltScale = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.LabelStyle.Interval = 1D;
            chartArea1.AxisX.LabelStyle.IsStaggered = true;
            chartArea1.AxisX.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorGrid.Interval = 1D;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorTickMark.Interval = 1D;
            chartArea1.AxisX.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.Maximum = 48D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            legend1.Enabled = false;
            legend1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            legend1.TitleAlignment = System.Drawing.StringAlignment.Near;
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.BorderWidth = 5;
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.Magenta;
            series1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            series1.Legend = "Legend1";
            series1.Name = "MP";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(809, 323);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            title1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.ForeColor = System.Drawing.Color.White;
            title1.Name = "Title";
            title1.Text = "Fast Fourier Transform (FFT)";
            title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title2.Font = new System.Drawing.Font("Verdana", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.ForeColor = System.Drawing.Color.White;
            title2.Name = "AxisX";
            title2.Text = "Frequency";
            title3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            title3.Font = new System.Drawing.Font("Verdana", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title3.ForeColor = System.Drawing.Color.White;
            title3.Name = "AxisY";
            title3.Text = "Amplitude";
            this.chart1.Titles.Add(title1);
            this.chart1.Titles.Add(title2);
            this.chart1.Titles.Add(title3);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart1);
            this.splitContainer1.Size = new System.Drawing.Size(809, 355);
            this.splitContainer1.SplitterDistance = 28;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.comboBoxVoltScale);
            this.splitContainer2.Size = new System.Drawing.Size(809, 28);
            this.splitContainer2.SplitterDistance = 76;
            this.splitContainer2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Zoom:";
            // 
            // comboBoxVoltScale
            // 
            this.comboBoxVoltScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVoltScale.FormattingEnabled = true;
            this.comboBoxVoltScale.Items.AddRange(new object[] {
            "10",
            "5",
            "2",
            "1",
            ".5",
            ".1",
            ".05",
            ".02",
            ".01",
            "Log10"});
            this.comboBoxVoltScale.Location = new System.Drawing.Point(3, 5);
            this.comboBoxVoltScale.Name = "comboBoxVoltScale";
            this.comboBoxVoltScale.Size = new System.Drawing.Size(79, 21);
            this.comboBoxVoltScale.TabIndex = 1;
            this.comboBoxVoltScale.SelectedIndexChanged += new System.EventHandler(this.comboBoxVoltScale_SelectedIndexChanged);
            // 
            // FormFFT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 355);
            this.Controls.Add(this.splitContainer1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFFT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mud Pulse FFT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFFT_FormClosing);
            this.Load += new System.EventHandler(this.Plot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxVoltScale;
    }
}