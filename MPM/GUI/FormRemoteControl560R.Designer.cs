
namespace MPM.GUI
{
    partial class FormRemoteControl560R
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRemoteControl560R));
            this.textBoxContents = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxContents
            // 
            this.textBoxContents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.textBoxContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContents.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxContents.ForeColor = System.Drawing.Color.White;
            this.textBoxContents.Location = new System.Drawing.Point(0, 0);
            this.textBoxContents.Multiline = true;
            this.textBoxContents.Name = "textBoxContents";
            this.textBoxContents.Size = new System.Drawing.Size(303, 450);
            this.textBoxContents.TabIndex = 0;
            // 
            // FormRemoteControl560R
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 450);
            this.Controls.Add(this.textBoxContents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRemoteControl560R";
            this.Text = "560R Settings";
            this.Load += new System.EventHandler(this.FormRemoteControl560R_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxContents;
    }
}