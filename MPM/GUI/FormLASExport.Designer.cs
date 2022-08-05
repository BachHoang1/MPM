namespace MPM.GUI
{
    partial class FormLASExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLASExport));
            this.propertyGridInfo = new System.Windows.Forms.PropertyGrid();
            this.buttonExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGridInfo
            // 
            this.propertyGridInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.propertyGridInfo.CategoryForeColor = System.Drawing.SystemColors.Highlight;
            this.propertyGridInfo.CategorySplitterColor = System.Drawing.SystemColors.Highlight;
            this.propertyGridInfo.CommandsForeColor = System.Drawing.SystemColors.Highlight;
            this.propertyGridInfo.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.propertyGridInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGridInfo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGridInfo.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.propertyGridInfo.HelpForeColor = System.Drawing.Color.White;
            this.propertyGridInfo.HelpVisible = false;
            this.propertyGridInfo.LineColor = System.Drawing.Color.White;
            this.propertyGridInfo.Location = new System.Drawing.Point(0, 0);
            this.propertyGridInfo.Name = "propertyGridInfo";
            this.propertyGridInfo.SelectedItemWithFocusForeColor = System.Drawing.Color.White;
            this.propertyGridInfo.Size = new System.Drawing.Size(336, 346);
            this.propertyGridInfo.TabIndex = 0;
            this.propertyGridInfo.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.propertyGridInfo.ViewForeColor = System.Drawing.Color.White;
            this.propertyGridInfo.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridInfo_PropertyValueChanged);
            this.propertyGridInfo.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGridInfo_SelectedGridItemChanged);
            this.propertyGridInfo.SelectedObjectsChanged += new System.EventHandler(this.propertyGridInfo_SelectedObjectsChanged);
            // 
            // buttonExport
            // 
            this.buttonExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonExport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExport.Location = new System.Drawing.Point(110, 355);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(116, 58);
            this.buttonExport.TabIndex = 1;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = false;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // FormLASExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(336, 422);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.propertyGridInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLASExport";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LAS Export";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLASExport_FormClosing);
            this.Load += new System.EventHandler(this.FormLASExport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGridInfo;
        private System.Windows.Forms.Button buttonExport;
    }
}