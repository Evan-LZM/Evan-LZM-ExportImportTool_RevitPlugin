namespace Export_Import_Tool
{
    partial class ExportFunctionForm
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
            this.ListBoxOfCategories = new System.Windows.Forms.ListBox();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.GroupExportButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.BrowsingButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.OperationLoadButton = new System.Windows.Forms.Button();
            this.OperationRecordingButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CheckedListBoxOfDocuments = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CompleteProjectControlCheckBox = new System.Windows.Forms.CheckBox();
            this.DateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.ModelRadioButton = new System.Windows.Forms.RadioButton();
            this.CategoryRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ElementNamesDisplayButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListBoxOfCategories
            // 
            this.ListBoxOfCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListBoxOfCategories.FormattingEnabled = true;
            this.ListBoxOfCategories.Location = new System.Drawing.Point(2, 15);
            this.ListBoxOfCategories.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ListBoxOfCategories.Name = "ListBoxOfCategories";
            this.ListBoxOfCategories.Size = new System.Drawing.Size(323, 459);
            this.ListBoxOfCategories.TabIndex = 0;
            this.ListBoxOfCategories.SelectedIndexChanged += new System.EventHandler(this.ListBoxOfCategories_SelectedIndexChanged);
            // 
            // DataGridView1
            // 
            this.DataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Location = new System.Drawing.Point(3, 9);
            this.DataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.RowTemplate.Height = 30;
            this.DataGridView1.Size = new System.Drawing.Size(423, 382);
            this.DataGridView1.TabIndex = 5;
            this.DataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellContentClick);
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(9, 478);
            this.ConfirmButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(341, 37);
            this.ConfirmButton.TabIndex = 15;
            this.ConfirmButton.Text = "Confirm";
            this.toolTip1.SetToolTip(this.ConfirmButton, "The confirm button to open the selected file \r\nand list all the Categories on the" +
        " right.");
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // GroupExportButton
            // 
            this.GroupExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupExportButton.Location = new System.Drawing.Point(38, 405);
            this.GroupExportButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GroupExportButton.Name = "GroupExportButton";
            this.GroupExportButton.Size = new System.Drawing.Size(389, 32);
            this.GroupExportButton.TabIndex = 18;
            this.GroupExportButton.Text = "Export All Model Files Of A Project";
            this.toolTip1.SetToolTip(this.GroupExportButton, "Export all the related files to the database \r\nin a nested structure:\r\n[DB] - [Sc" +
        "hema].[Table]\r\n[Project Name] - [Model Name].[Category Name]");
            this.GroupExportButton.UseVisualStyleBackColor = true;
            this.GroupExportButton.Click += new System.EventHandler(this.GroupExportButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(9, 519);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(341, 37);
            this.ResetButton.TabIndex = 19;
            this.ResetButton.Text = "Reset";
            this.toolTip1.SetToolTip(this.ResetButton, "Reset the condition of the window to the initial.");
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // BrowsingButton
            // 
            this.BrowsingButton.Location = new System.Drawing.Point(9, 40);
            this.BrowsingButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BrowsingButton.Name = "BrowsingButton";
            this.BrowsingButton.Size = new System.Drawing.Size(341, 30);
            this.BrowsingButton.TabIndex = 20;
            this.BrowsingButton.Text = "Browsing";
            this.toolTip1.SetToolTip(this.BrowsingButton, "Browsing and selecing \"Revit\" Models.");
            this.BrowsingButton.UseVisualStyleBackColor = true;
            this.BrowsingButton.Click += new System.EventHandler(this.BrowsingButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "Select Model Files";
            // 
            // OperationLoadButton
            // 
            this.OperationLoadButton.Location = new System.Drawing.Point(237, 505);
            this.OperationLoadButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OperationLoadButton.Name = "OperationLoadButton";
            this.OperationLoadButton.Size = new System.Drawing.Size(189, 37);
            this.OperationLoadButton.TabIndex = 29;
            this.OperationLoadButton.Text = "Load Recording Process";
            this.toolTip1.SetToolTip(this.OperationLoadButton, "Load the recording file saved.");
            this.OperationLoadButton.UseVisualStyleBackColor = true;
            this.OperationLoadButton.Click += new System.EventHandler(this.OperationLoadButton_Click);
            // 
            // OperationRecordingButton
            // 
            this.OperationRecordingButton.Location = new System.Drawing.Point(4, 505);
            this.OperationRecordingButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OperationRecordingButton.Name = "OperationRecordingButton";
            this.OperationRecordingButton.Size = new System.Drawing.Size(212, 37);
            this.OperationRecordingButton.TabIndex = 28;
            this.OperationRecordingButton.Text = "Save the Process";
            this.toolTip1.SetToolTip(this.OperationRecordingButton, "Save the whole export process into a recording\r\nfile. The whole process can be av" +
        "aible through \r\nloading the recording file.");
            this.OperationRecordingButton.UseVisualStyleBackColor = true;
            this.OperationRecordingButton.Click += new System.EventHandler(this.OperationRecordingButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(3, 454);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(423, 33);
            this.ExportButton.TabIndex = 19;
            this.ExportButton.Text = "Export Checked Independent Models Files Or Selected Tables";
            this.toolTip1.SetToolTip(this.ExportButton, "Export independent model files or \r\ncategories(tables) in a file to an \r\nexisting" +
        " database(project) in the \r\ndatabase server.");
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CheckedListBoxOfDocuments);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.BrowsingButton);
            this.groupBox1.Controls.Add(this.ResetButton);
            this.groupBox1.Controls.Add(this.ConfirmButton);
            this.groupBox1.Location = new System.Drawing.Point(10, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(365, 572);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // CheckedListBoxOfDocuments
            // 
            this.CheckedListBoxOfDocuments.FormattingEnabled = true;
            this.CheckedListBoxOfDocuments.Location = new System.Drawing.Point(9, 85);
            this.CheckedListBoxOfDocuments.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CheckedListBoxOfDocuments.Name = "CheckedListBoxOfDocuments";
            this.CheckedListBoxOfDocuments.Size = new System.Drawing.Size(343, 379);
            this.CheckedListBoxOfDocuments.TabIndex = 23;
            this.CheckedListBoxOfDocuments.SelectedIndexChanged += new System.EventHandler(this.CheckedListBoxOfDocuments_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ListBoxOfCategories);
            this.groupBox2.Location = new System.Drawing.Point(382, 63);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(327, 476);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.CompleteProjectControlCheckBox);
            this.groupBox3.Controls.Add(this.OperationLoadButton);
            this.groupBox3.Controls.Add(this.ExportButton);
            this.groupBox3.Controls.Add(this.OperationRecordingButton);
            this.groupBox3.Controls.Add(this.GroupExportButton);
            this.groupBox3.Controls.Add(this.DataGridView1);
            this.groupBox3.Location = new System.Drawing.Point(723, 26);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(435, 563);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            // 
            // CompleteProjectControlCheckBox
            // 
            this.CompleteProjectControlCheckBox.AutoSize = true;
            this.CompleteProjectControlCheckBox.Location = new System.Drawing.Point(10, 411);
            this.CompleteProjectControlCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CompleteProjectControlCheckBox.Name = "CompleteProjectControlCheckBox";
            this.CompleteProjectControlCheckBox.Size = new System.Drawing.Size(15, 14);
            this.CompleteProjectControlCheckBox.TabIndex = 30;
            this.CompleteProjectControlCheckBox.UseVisualStyleBackColor = true;
            this.CompleteProjectControlCheckBox.CheckedChanged += new System.EventHandler(this.CompleteProjectControlCheckBox_CheckedChanged);
            // 
            // DateTimePicker1
            // 
            this.DateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePicker1.Location = new System.Drawing.Point(123, 9);
            this.DateTimePicker1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DateTimePicker1.Name = "DateTimePicker1";
            this.DateTimePicker1.Size = new System.Drawing.Size(195, 20);
            this.DateTimePicker1.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 15);
            this.label1.TabIndex = 27;
            this.label1.Text = "Export Time: ";
            // 
            // ModelRadioButton
            // 
            this.ModelRadioButton.AutoSize = true;
            this.ModelRadioButton.Location = new System.Drawing.Point(13, 12);
            this.ModelRadioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ModelRadioButton.Name = "ModelRadioButton";
            this.ModelRadioButton.Size = new System.Drawing.Size(112, 17);
            this.ModelRadioButton.TabIndex = 28;
            this.ModelRadioButton.TabStop = true;
            this.ModelRadioButton.Text = "Completed Models";
            this.ModelRadioButton.UseVisualStyleBackColor = true;
            // 
            // CategoryRadioButton
            // 
            this.CategoryRadioButton.AutoSize = true;
            this.CategoryRadioButton.Location = new System.Drawing.Point(167, 12);
            this.CategoryRadioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CategoryRadioButton.Name = "CategoryRadioButton";
            this.CategoryRadioButton.Size = new System.Drawing.Size(138, 17);
            this.CategoryRadioButton.TabIndex = 29;
            this.CategoryRadioButton.TabStop = true;
            this.CategoryRadioButton.Text = "Independent Categories";
            this.CategoryRadioButton.UseVisualStyleBackColor = true;
            this.CategoryRadioButton.CheckedChanged += new System.EventHandler(this.CategoryRadioButton_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.CategoryRadioButton);
            this.groupBox4.Controls.Add(this.ModelRadioButton);
            this.groupBox4.Location = new System.Drawing.Point(382, 35);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox4.Size = new System.Drawing.Size(325, 36);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.DateTimePicker1);
            this.groupBox5.Location = new System.Drawing.Point(386, 543);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox5.Size = new System.Drawing.Size(323, 32);
            this.groupBox5.TabIndex = 31;
            this.groupBox5.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(383, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 32;
            this.label3.Text = "Category";
            // 
            // ElementNamesDisplayButton
            // 
            this.ElementNamesDisplayButton.Location = new System.Drawing.Point(603, 13);
            this.ElementNamesDisplayButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ElementNamesDisplayButton.Name = "ElementNamesDisplayButton";
            this.ElementNamesDisplayButton.Size = new System.Drawing.Size(101, 27);
            this.ElementNamesDisplayButton.TabIndex = 33;
            this.ElementNamesDisplayButton.Text = "Element Names";
            this.ElementNamesDisplayButton.UseVisualStyleBackColor = true;
            this.ElementNamesDisplayButton.Visible = false;
            this.ElementNamesDisplayButton.Click += new System.EventHandler(this.ElementNamesDisplayButton_Click);
            // 
            // ExportFunctionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 591);
            this.Controls.Add(this.ElementNamesDisplayButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ExportFunctionForm";
            this.Text = "Export Function Window";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxOfCategories;
        private System.Windows.Forms.DataGridView DataGridView1;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button GroupExportButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button BrowsingButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox CheckedListBoxOfDocuments;
        private System.Windows.Forms.DateTimePicker DateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OperationRecordingButton;
        private System.Windows.Forms.Button OperationLoadButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.RadioButton ModelRadioButton;
        private System.Windows.Forms.RadioButton CategoryRadioButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ElementNamesDisplayButton;
        private System.Windows.Forms.CheckBox CompleteProjectControlCheckBox;
    }
}