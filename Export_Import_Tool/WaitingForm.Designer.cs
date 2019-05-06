namespace Export_Import_Tool
{
    partial class WaitingForm
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
            this.WaitCancelButton = new System.Windows.Forms.Button();
            this.CurrentTimeLabel = new System.Windows.Forms.Label();
            this.PlannedTimeLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // WaitCancelButton
            // 
            this.WaitCancelButton.Location = new System.Drawing.Point(739, 113);
            this.WaitCancelButton.Name = "WaitCancelButton";
            this.WaitCancelButton.Size = new System.Drawing.Size(158, 60);
            this.WaitCancelButton.TabIndex = 0;
            this.WaitCancelButton.Text = "Cancel Export";
            this.toolTip1.SetToolTip(this.WaitCancelButton, "Canel the whole export prcess.");
            this.WaitCancelButton.UseVisualStyleBackColor = true;
            this.WaitCancelButton.Click += new System.EventHandler(this.WaitCancelButton_Click);
            // 
            // CurrentTimeLabel
            // 
            this.CurrentTimeLabel.AutoSize = true;
            this.CurrentTimeLabel.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentTimeLabel.Location = new System.Drawing.Point(221, 27);
            this.CurrentTimeLabel.Name = "CurrentTimeLabel";
            this.CurrentTimeLabel.Size = new System.Drawing.Size(74, 33);
            this.CurrentTimeLabel.TabIndex = 1;
            this.CurrentTimeLabel.Text = "label1";
            // 
            // PlannedTimeLabel
            // 
            this.PlannedTimeLabel.AutoSize = true;
            this.PlannedTimeLabel.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlannedTimeLabel.Location = new System.Drawing.Point(221, 113);
            this.PlannedTimeLabel.Name = "PlannedTimeLabel";
            this.PlannedTimeLabel.Size = new System.Drawing.Size(74, 33);
            this.PlannedTimeLabel.TabIndex = 2;
            this.PlannedTimeLabel.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 33);
            this.label1.TabIndex = 3;
            this.label1.Text = "Export Start At : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 33);
            this.label2.TabIndex = 4;
            this.label2.Text = "Current Time : ";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // WaitingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 181);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PlannedTimeLabel);
            this.Controls.Add(this.CurrentTimeLabel);
            this.Controls.Add(this.WaitCancelButton);
            this.Name = "WaitingForm";
            this.Text = "WaitingForm";
            this.Load += new System.EventHandler(this.WaitingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button WaitCancelButton;
        private System.Windows.Forms.Label CurrentTimeLabel;
        private System.Windows.Forms.Label PlannedTimeLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}