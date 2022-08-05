namespace MPM.GUI
{
    partial class FormPlotSamplesEM
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlotSamplesEM));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStripFormSamples = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideRawSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideLowPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableZoomingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDownBetaFactor = new System.Windows.Forms.NumericUpDown();
            this.labelLPFactor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStripFormSamples.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBetaFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chart1.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart1.BorderlineWidth = 0;
            chartArea1.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea1.AxisX.LabelStyle.Interval = 5D;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisX.MajorGrid.Interval = 0D;
            chartArea1.AxisX.MajorGrid.IntervalOffset = 0D;
            chartArea1.AxisX.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisX.MajorTickMark.Interval = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.InterlacedColor = System.Drawing.Color.White;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LabelStyle.Format = "d";
            chartArea1.AxisY.LabelStyle.IsStaggered = true;
            chartArea1.AxisY.LabelStyle.TruncatedLabels = true;
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisY.MajorGrid.Interval = 0D;
            chartArea1.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorTickMark.Interval = 0D;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.Title = "uV";
            chartArea1.AxisY.TitleAlignment = System.Drawing.StringAlignment.Near;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.BorderWidth = 4;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 80F;
            chartArea1.Position.Width = 79.36066F;
            chartArea1.Position.X = 3F;
            chartArea1.Position.Y = 3F;
            chartArea1.ShadowColor = System.Drawing.Color.White;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.contextMenuStripFormSamples;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            legend1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Column;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            this.chart1.Padding = new System.Windows.Forms.Padding(5);
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.Name = "Raw EM  ";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Color = System.Drawing.Color.White;
            series2.Legend = "Legend1";
            series2.Name = "Low Pass EM";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1221, 124);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chartSamples";
            // 
            // contextMenuStripFormSamples
            // 
            this.contextMenuStripFormSamples.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripFormSamples.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideRawSamplesToolStripMenuItem,
            this.hideLowPassToolStripMenuItem,
            this.enableZoomingToolStripMenuItem});
            this.contextMenuStripFormSamples.Name = "contextMenuStripFormSamples";
            this.contextMenuStripFormSamples.Size = new System.Drawing.Size(172, 70);
            // 
            // hideRawSamplesToolStripMenuItem
            // 
            this.hideRawSamplesToolStripMenuItem.Name = "hideRawSamplesToolStripMenuItem";
            this.hideRawSamplesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.hideRawSamplesToolStripMenuItem.Text = "Hide Raw Samples";
            this.hideRawSamplesToolStripMenuItem.Click += new System.EventHandler(this.hideRawSamplesToolStripMenuItem_Click);
            // 
            // hideLowPassToolStripMenuItem
            // 
            this.hideLowPassToolStripMenuItem.Name = "hideLowPassToolStripMenuItem";
            this.hideLowPassToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.hideLowPassToolStripMenuItem.Text = "Hide Low Pass";
            this.hideLowPassToolStripMenuItem.Click += new System.EventHandler(this.hideLowPassToolStripMenuItem_Click);
            // 
            // enableZoomingToolStripMenuItem
            // 
            this.enableZoomingToolStripMenuItem.Name = "enableZoomingToolStripMenuItem";
            this.enableZoomingToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.enableZoomingToolStripMenuItem.Text = "Enable Zooming";
            this.enableZoomingToolStripMenuItem.Click += new System.EventHandler(this.enableZoomingToolStripMenuItem_Click);
            // 
            // numericUpDownBetaFactor
            // 
            this.numericUpDownBetaFactor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownBetaFactor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.numericUpDownBetaFactor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownBetaFactor.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownBetaFactor.ForeColor = System.Drawing.Color.White;
            this.numericUpDownBetaFactor.Location = new System.Drawing.Point(1039, 12);
            this.numericUpDownBetaFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownBetaFactor.Name = "numericUpDownBetaFactor";
            this.numericUpDownBetaFactor.Size = new System.Drawing.Size(64, 26);
            this.numericUpDownBetaFactor.TabIndex = 1;
            this.numericUpDownBetaFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownBetaFactor.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numericUpDownBetaFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownBetaFactor.ValueChanged += new System.EventHandler(this.numericUpDownBetaFactor_ValueChanged);
            // 
            // labelLPFactor
            // 
            this.labelLPFactor.AutoSize = true;
            this.labelLPFactor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.labelLPFactor.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLPFactor.ForeColor = System.Drawing.Color.White;
            this.labelLPFactor.Location = new System.Drawing.Point(1109, 19);
            this.labelLPFactor.Name = "labelLPFactor";
            this.labelLPFactor.Size = new System.Drawing.Size(66, 13);
            this.labelLPFactor.TabIndex = 2;
            this.labelLPFactor.Text = "Low Pass";
            // 
            // FormPlotSamplesEM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.ClientSize = new System.Drawing.Size(1221, 124);
            this.Controls.Add(this.labelLPFactor);
            this.Controls.Add(this.numericUpDownBetaFactor);
            this.Controls.Add(this.chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPlotSamplesEM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plot EM Samples";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlotSamples_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPlotSamples_FormClosed);
            this.Load += new System.EventHandler(this.FormPlotSamples_Load);
            this.Shown += new System.EventHandler(this.FormPlotSamples_Shown);
            this.Resize += new System.EventHandler(this.FormPlotSamples_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStripFormSamples.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBetaFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFormSamples;
        private System.Windows.Forms.ToolStripMenuItem hideRawSamplesToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown numericUpDownBetaFactor;
        private System.Windows.Forms.ToolStripMenuItem enableZoomingToolStripMenuItem;
        private System.Windows.Forms.Label labelLPFactor;
        private System.Windows.Forms.ToolStripMenuItem hideLowPassToolStripMenuItem;
    }
}