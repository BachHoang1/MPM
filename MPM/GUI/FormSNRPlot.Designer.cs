namespace MPM.GUI
{
    partial class FormSNRPlot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSNRPlot));
            this.chartSNR = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartSNR)).BeginInit();
            this.SuspendLayout();
            // 
            // chartSNR
            // 
            this.chartSNR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.LabelStyle.Format = "MM/dd HH:mm";
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.AxisX.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            chartArea1.Name = "ChartArea1";
            this.chartSNR.ChartAreas.Add(chartArea1);
            this.chartSNR.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartSNR.Legends.Add(legend1);
            this.chartSNR.Location = new System.Drawing.Point(0, 0);
            this.chartSNR.Name = "chartSNR";
            series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Color = System.Drawing.Color.Orange;
            series1.LabelBackColor = System.Drawing.Color.White;
            series1.LabelBorderColor = System.Drawing.Color.White;
            series1.LabelForeColor = System.Drawing.Color.White;
            series1.Legend = "Legend1";
            series1.Name = "SNR";
            this.chartSNR.Series.Add(series1);
            this.chartSNR.Size = new System.Drawing.Size(847, 236);
            this.chartSNR.TabIndex = 0;
            this.chartSNR.Text = "chart1";
            // 
            // FormSNRPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(847, 236);
            this.Controls.Add(this.chartSNR);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSNRPlot";
            this.Text = "SNR Plot";
            this.Load += new System.EventHandler(this.FormSNRPlot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartSNR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartSNR;
    }
}