namespace Export_Import_Tool
{
    partial class DatabaseDecision
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
            this.Exist = new System.Windows.Forms.Button();
            this.CreateNew = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Exist
            // 
            this.Exist.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Exist.Location = new System.Drawing.Point(79, 83);
            this.Exist.Name = "Exist";
            this.Exist.Size = new System.Drawing.Size(101, 51);
            this.Exist.TabIndex = 0;
            this.Exist.Text = "Existing Database";
            this.Exist.UseVisualStyleBackColor = true;
            this.Exist.Click += new System.EventHandler(this.Exist_Click);
            // 
            // CreateNew
            // 
            this.CreateNew.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CreateNew.Location = new System.Drawing.Point(246, 83);
            this.CreateNew.Name = "CreateNew";
            this.CreateNew.Size = new System.Drawing.Size(101, 51);
            this.CreateNew.TabIndex = 1;
            this.CreateNew.Text = "Create New Database";
            this.CreateNew.UseVisualStyleBackColor = true;
            this.CreateNew.Click += new System.EventHandler(this.CreateNew_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.Location = new System.Drawing.Point(31, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Export Into";
            // 
            // DatabaseDecision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 215);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CreateNew);
            this.Controls.Add(this.Exist);
            this.Name = "DatabaseDecision";
            this.Text = "DatabaseDecision";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Exist;
        private System.Windows.Forms.Button CreateNew;
        private System.Windows.Forms.Label label1;
    }
}