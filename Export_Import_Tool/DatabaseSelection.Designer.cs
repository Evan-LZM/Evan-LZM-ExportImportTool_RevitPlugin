namespace Export_Import_Tool
{
    partial class DatabaseSelection
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
            this.SuspendLayout();
            // 
            // Exist
            // 
            this.Exist.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Exist.Location = new System.Drawing.Point(65, 70);
            this.Exist.Name = "Exist";
            this.Exist.Size = new System.Drawing.Size(130, 68);
            this.Exist.TabIndex = 0;
            this.Exist.Text = "Existing Project/Database";
            this.Exist.UseVisualStyleBackColor = true;
            this.Exist.Click += new System.EventHandler(this.Exist_Click);
            // 
            // CreateNew
            // 
            this.CreateNew.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CreateNew.Location = new System.Drawing.Point(280, 70);
            this.CreateNew.Name = "CreateNew";
            this.CreateNew.Size = new System.Drawing.Size(130, 68);
            this.CreateNew.TabIndex = 1;
            this.CreateNew.Text = "Create New Project/Database";
            this.CreateNew.UseVisualStyleBackColor = true;
            this.CreateNew.Click += new System.EventHandler(this.CreateNew_Click);
            // 
            // DatabaseSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 236);
            this.Controls.Add(this.CreateNew);
            this.Controls.Add(this.Exist);
            this.Name = "DatabaseSelection";
            this.Text = "  ";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button Exist;
        public System.Windows.Forms.Button CreateNew;
    }
}