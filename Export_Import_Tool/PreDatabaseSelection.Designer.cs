namespace Export_Import_Tool
{
    partial class PreDatabaseSelection
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
            this.localdriver = new System.Windows.Forms.Button();
            this.clouddriver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // localdriver
            // 
            this.localdriver.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.localdriver.Location = new System.Drawing.Point(64, 58);
            this.localdriver.Name = "localdriver";
            this.localdriver.Size = new System.Drawing.Size(114, 54);
            this.localdriver.TabIndex = 0;
            this.localdriver.Text = "Local";
            this.localdriver.UseVisualStyleBackColor = true;
            this.localdriver.Click += new System.EventHandler(this.localdriver_Click);
            // 
            // clouddriver
            // 
            this.clouddriver.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.clouddriver.Location = new System.Drawing.Point(257, 58);
            this.clouddriver.Name = "clouddriver";
            this.clouddriver.Size = new System.Drawing.Size(128, 54);
            this.clouddriver.TabIndex = 1;
            this.clouddriver.Text = "Cloud";
            this.clouddriver.UseVisualStyleBackColor = true;
            this.clouddriver.Click += new System.EventHandler(this.clouddriver_Click);
            // 
            // PreDatabaseSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 179);
            this.Controls.Add(this.clouddriver);
            this.Controls.Add(this.localdriver);
            this.Name = "PreDatabaseSelection";
            this.Text = "PreDatabaseSelection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button localdriver;
        private System.Windows.Forms.Button clouddriver;
    }
}