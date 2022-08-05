namespace MPM.GUI
{
    partial class FormINCWhileDrilling
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
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBoxINCHistory = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.chart1.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.LabelStyle.Format = "HH:mm";
            chartArea1.AxisX.LabelStyle.Interval = 60D;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorGrid.Interval = 0D;
            chartArea1.AxisX.MajorGrid.IntervalOffset = 0D;
            chartArea1.AxisX.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Interval = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorTickMark.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet;
            chartArea1.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisX.MinorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet;
            chartArea1.AxisX.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisY.MajorGrid.Interval = 0D;
            chartArea1.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            chartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.Title = "Deg";
            chartArea1.AxisY.TitleAlignment = System.Drawing.StringAlignment.Near;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.BorderWidth = 4;
            chartArea1.Name = "ChartArea1";
            chartArea1.ShadowColor = System.Drawing.Color.White;
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsDockedInsideChartArea = false;
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            legend1.TitleBackColor = System.Drawing.Color.White;
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(46, 12);
            this.chart1.Name = "chart1";
            series1.BorderWidth = 4;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Color = System.Drawing.Color.Fuchsia;
            series1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.MarkerSize = 7;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Cs MP";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Color = System.Drawing.Color.Fuchsia;
            series2.Legend = "Legend1";
            series2.MarkerSize = 10;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star5;
            series2.Name = "Svy MP";
            series3.BorderWidth = 4;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series3.Legend = "Legend1";
            series3.MarkerSize = 7;
            series3.Name = "Cs EM";
            series4.BorderWidth = 4;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series4.Legend = "Legend1";
            series4.MarkerSize = 7;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Triangle;
            series4.Name = "Svy EM";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(174, 107);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chartSamples";
            this.chart1.Visible = false;
            // 
            // textBoxINCHistory
            // 
            this.textBoxINCHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.textBoxINCHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxINCHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxINCHistory.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxINCHistory.ForeColor = System.Drawing.Color.White;
            this.textBoxINCHistory.Location = new System.Drawing.Point(0, 0);
            this.textBoxINCHistory.Multiline = true;
            this.textBoxINCHistory.Name = "textBoxINCHistory";
            this.textBoxINCHistory.ReadOnly = true;
            this.textBoxINCHistory.Size = new System.Drawing.Size(256, 140);
            this.textBoxINCHistory.TabIndex = 2;
            this.textBoxINCHistory.TextChanged += new System.EventHandler(this.textBoxINCHistory_TextChanged);
            this.textBoxINCHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBoxINCHistory_MouseDown);
            this.textBoxINCHistory.MouseLeave += new System.EventHandler(this.textBoxINCHistory_MouseLeave);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(152)))), ((int)(((byte)(196)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelTitle);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxINCHistory);
            this.splitContainer1.Size = new System.Drawing.Size(256, 166);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 3;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(256, 25);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "INC History";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTitle.Click += new System.EventHandler(this.labelTitle_Click);
            // 
            // FormINCWhileDrilling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 166);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.chart1);
            this.HideOnClose = true;
            this.Name = "FormINCWhileDrilling";
            this.Text = "INC History";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormINCWhileDrilling_FormClosed);
            this.Load += new System.EventHandler(this.FormINCWhileDrilling_Load);
            this.Resize += new System.EventHandler(this.FormINCWhileDrilling_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox textBoxINCHistory;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labelTitle;
    }
}