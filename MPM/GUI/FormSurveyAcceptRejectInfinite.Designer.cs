namespace MPM.GUI
{
    partial class FormSurveyAcceptRejectInfinite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSurveyAcceptRejectInfinite));
            this.buttonReject = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewSurveyRec = new System.Windows.Forms.DataGridView();
            this.buttonAcceptOnly = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonEditSurveyDepth = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSurveyRec)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonReject
            // 
            this.buttonReject.BackColor = System.Drawing.Color.Red;
            this.buttonReject.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReject.ForeColor = System.Drawing.Color.White;
            this.buttonReject.Location = new System.Drawing.Point(160, 25);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(82, 50);
            this.buttonReject.TabIndex = 0;
            this.buttonReject.Text = "Reject";
            this.buttonReject.UseVisualStyleBackColor = false;
            this.buttonReject.Click += new System.EventHandler(this.buttonReject_Click);
            this.buttonReject.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonReject_MouseClick);
            // 
            // buttonAccept
            // 
            this.buttonAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonAccept.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAccept.ForeColor = System.Drawing.Color.Black;
            this.buttonAccept.Location = new System.Drawing.Point(422, 25);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(237, 50);
            this.buttonAccept.TabIndex = 2;
            this.buttonAccept.Text = "Accept and Send to MSA";
            this.buttonAccept.UseVisualStyleBackColor = false;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            this.buttonAccept.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonAccept_MouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewSurveyRec);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonAcceptOnly);
            this.splitContainer1.Panel2.Controls.Add(this.buttonClose);
            this.splitContainer1.Panel2.Controls.Add(this.buttonEditSurveyDepth);
            this.splitContainer1.Panel2.Controls.Add(this.buttonAccept);
            this.splitContainer1.Panel2.Controls.Add(this.buttonReject);
            this.splitContainer1.Size = new System.Drawing.Size(922, 506);
            this.splitContainer1.SplitterDistance = 411;
            this.splitContainer1.TabIndex = 9;
            // 
            // dataGridViewSurveyRec
            // 
            this.dataGridViewSurveyRec.AllowUserToAddRows = false;
            this.dataGridViewSurveyRec.AllowUserToDeleteRows = false;
            this.dataGridViewSurveyRec.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSurveyRec.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSurveyRec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSurveyRec.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSurveyRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSurveyRec.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSurveyRec.MultiSelect = false;
            this.dataGridViewSurveyRec.Name = "dataGridViewSurveyRec";
            this.dataGridViewSurveyRec.ReadOnly = true;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewSurveyRec.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewSurveyRec.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.dataGridViewSurveyRec.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewSurveyRec.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewSurveyRec.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Yellow;
            this.dataGridViewSurveyRec.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewSurveyRec.RowTemplate.Height = 40;
            this.dataGridViewSurveyRec.Size = new System.Drawing.Size(922, 411);
            this.dataGridViewSurveyRec.TabIndex = 0;
            this.dataGridViewSurveyRec.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSurveyRec_CellClick);
            this.dataGridViewSurveyRec.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSurveyRec_CellDoubleClick);
            // 
            // buttonAcceptOnly
            // 
            this.buttonAcceptOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonAcceptOnly.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAcceptOnly.ForeColor = System.Drawing.Color.Black;
            this.buttonAcceptOnly.Location = new System.Drawing.Point(263, 25);
            this.buttonAcceptOnly.Name = "buttonAcceptOnly";
            this.buttonAcceptOnly.Size = new System.Drawing.Size(141, 50);
            this.buttonAcceptOnly.TabIndex = 5;
            this.buttonAcceptOnly.Text = "Accept Only";
            this.buttonAcceptOnly.UseVisualStyleBackColor = false;
            this.buttonAcceptOnly.Click += new System.EventHandler(this.buttonAcceptOnly_Click);
            this.buttonAcceptOnly.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonAcceptOnly_MouseClick);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonClose.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.ForeColor = System.Drawing.Color.Black;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonClose.Location = new System.Drawing.Point(787, 25);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(113, 50);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            this.buttonClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonClose_MouseClick);
            // 
            // buttonEditSurveyDepth
            // 
            this.buttonEditSurveyDepth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonEditSurveyDepth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonEditSurveyDepth.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEditSurveyDepth.ForeColor = System.Drawing.Color.Black;
            this.buttonEditSurveyDepth.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonEditSurveyDepth.Location = new System.Drawing.Point(22, 25);
            this.buttonEditSurveyDepth.Name = "buttonEditSurveyDepth";
            this.buttonEditSurveyDepth.Size = new System.Drawing.Size(113, 50);
            this.buttonEditSurveyDepth.TabIndex = 3;
            this.buttonEditSurveyDepth.Text = "Edit Survey Depth";
            this.buttonEditSurveyDepth.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.buttonEditSurveyDepth.UseVisualStyleBackColor = false;
            this.buttonEditSurveyDepth.Click += new System.EventHandler(this.buttonEditSurveyDepth_Click);
            // 
            // FormSurveyAcceptRejectInfinite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(922, 506);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSurveyAcceptRejectInfinite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Survey Accept/Reject";
            this.Load += new System.EventHandler(this.FormSurveyAcceptRejectInfinite_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSurveyRec)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonReject;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonEditSurveyDepth;
        private System.Windows.Forms.DataGridView dataGridViewSurveyRec;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonAcceptOnly;
    }
}