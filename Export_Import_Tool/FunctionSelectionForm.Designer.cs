namespace Export_Import_Tool
{
    partial class FunctionSelectionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ExportFunctionButton = new System.Windows.Forms.Button();
            this.ImportFuntionButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("DejaVu Sans", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(471, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please Choose the Function:";
            // 
            // ExportFunctionButton
            // 
            this.ExportFunctionButton.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportFunctionButton.Location = new System.Drawing.Point(117, 138);
            this.ExportFunctionButton.Name = "ExportFunctionButton";
            this.ExportFunctionButton.Size = new System.Drawing.Size(538, 92);
            this.ExportFunctionButton.TabIndex = 1;
            this.ExportFunctionButton.Text = "Revit Model ----- Export ----> SQL Server";
            this.toolTip1.SetToolTip(this.ExportFunctionButton, "Export independent models or tables to \r\nan existing database.");
            this.ExportFunctionButton.UseVisualStyleBackColor = true;
            this.ExportFunctionButton.Click += new System.EventHandler(this.ExportFunctionButton_Click);
            // 
            // ImportFuntionButton
            // 
            this.ImportFuntionButton.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportFuntionButton.Location = new System.Drawing.Point(117, 262);
            this.ImportFuntionButton.Name = "ImportFuntionButton";
            this.ImportFuntionButton.Size = new System.Drawing.Size(538, 82);
            this.ImportFuntionButton.TabIndex = 2;
            this.ImportFuntionButton.Text = "SQL Server ----- Import ----> Revit Model";
            this.toolTip1.SetToolTip(this.ImportFuntionButton, "Import data from the database back to selected\r\nrevit files(models).");
            this.ImportFuntionButton.UseVisualStyleBackColor = true;
            this.ImportFuntionButton.Click += new System.EventHandler(this.ImportFuntionButton_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(117, 385);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(537, 72);
            this.button1.TabIndex = 3;
            this.button1.Text = "Manage Revit Links";
            this.toolTip1.SetToolTip(this.button1, "Obtain all the links in a project related to the \r\nselected file, which can furth" +
        "er create a database\r\nwith nested structure.");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FunctionSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 483);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ImportFuntionButton);
            this.Controls.Add(this.ExportFunctionButton);
            this.Controls.Add(this.label1);
            this.Name = "FunctionSelectionForm";
            this.Text = "Data Export/Import Plugin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExportFunctionButton;
        private System.Windows.Forms.Button ImportFuntionButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}