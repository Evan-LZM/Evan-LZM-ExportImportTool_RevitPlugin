namespace Export_Import_Tool
{
    partial class ManageLinkForm
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
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.selCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LinkCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReferenceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SavedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PathCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalAliasCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RevitPathText = new System.Windows.Forms.TextBox();
            this.SelFileBtn = new System.Windows.Forms.Button();
            this.ExportBtn = new System.Windows.Forms.Button();
            this.ShowBtn = new System.Windows.Forms.Button();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridView1
            // 
            this.DataGridView1.AllowUserToAddRows = false;
            this.DataGridView1.AllowUserToDeleteRows = false;
            this.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selCol,
            this.LinkCol,
            this.StatusCol,
            this.ReferenceCol,
            this.PositionCol,
            this.SavedCol,
            this.PathCol,
            this.LocalAliasCol});
            this.DataGridView1.Location = new System.Drawing.Point(35, 36);
            this.DataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.RowTemplate.Height = 27;
            this.DataGridView1.Size = new System.Drawing.Size(1276, 478);
            this.DataGridView1.TabIndex = 0;
            this.DataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellContentClick);
            // 
            // selCol
            // 
            this.selCol.HeaderText = "Select Links";
            this.selCol.Name = "selCol";
            this.selCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.selCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LinkCol
            // 
            this.LinkCol.HeaderText = "Link Name";
            this.LinkCol.Name = "LinkCol";
            this.LinkCol.ReadOnly = true;
            // 
            // StatusCol
            // 
            this.StatusCol.HeaderText = "Status";
            this.StatusCol.Name = "StatusCol";
            this.StatusCol.ReadOnly = true;
            // 
            // ReferenceCol
            // 
            this.ReferenceCol.HeaderText = "Reference Type";
            this.ReferenceCol.Name = "ReferenceCol";
            this.ReferenceCol.ReadOnly = true;
            // 
            // PositionCol
            // 
            this.PositionCol.HeaderText = "Position Not Saved";
            this.PositionCol.Name = "PositionCol";
            this.PositionCol.ReadOnly = true;
            this.PositionCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PositionCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // SavedCol
            // 
            this.SavedCol.HeaderText = "Saved Path";
            this.SavedCol.Name = "SavedCol";
            // 
            // PathCol
            // 
            this.PathCol.HeaderText = "Path Type";
            this.PathCol.Name = "PathCol";
            this.PathCol.ReadOnly = true;
            // 
            // LocalAliasCol
            // 
            this.LocalAliasCol.HeaderText = "Local Alias";
            this.LocalAliasCol.Name = "LocalAliasCol";
            this.LocalAliasCol.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(33, 158);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1351, 556);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Links Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RevitPathText);
            this.groupBox2.Controls.Add(this.SelFileBtn);
            this.groupBox2.Location = new System.Drawing.Point(33, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1345, 117);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Revit File";
            // 
            // RevitPathText
            // 
            this.RevitPathText.Location = new System.Drawing.Point(218, 60);
            this.RevitPathText.Multiline = true;
            this.RevitPathText.Name = "RevitPathText";
            this.RevitPathText.Size = new System.Drawing.Size(1093, 33);
            this.RevitPathText.TabIndex = 1;
            // 
            // SelFileBtn
            // 
            this.SelFileBtn.Location = new System.Drawing.Point(55, 59);
            this.SelFileBtn.Name = "SelFileBtn";
            this.SelFileBtn.Size = new System.Drawing.Size(147, 35);
            this.SelFileBtn.TabIndex = 0;
            this.SelFileBtn.Text = "Choose File";
            this.toolTip1.SetToolTip(this.SelFileBtn, "Press to select the target revit file(model)");
            this.SelFileBtn.UseVisualStyleBackColor = true;
            this.SelFileBtn.Click += new System.EventHandler(this.SelFileBtn_Click);
            // 
            // ExportBtn
            // 
            this.ExportBtn.Location = new System.Drawing.Point(1410, 275);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.Size = new System.Drawing.Size(146, 39);
            this.ExportBtn.TabIndex = 3;
            this.ExportBtn.Text = "Export Data";
            this.toolTip1.SetToolTip(this.ExportBtn, "Invoke the export form to export all the\r\nlinked files displayed into a database." +
        "");
            this.ExportBtn.UseVisualStyleBackColor = true;
            this.ExportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
            // 
            // ShowBtn
            // 
            this.ShowBtn.Location = new System.Drawing.Point(1410, 194);
            this.ShowBtn.Name = "ShowBtn";
            this.ShowBtn.Size = new System.Drawing.Size(146, 39);
            this.ShowBtn.TabIndex = 5;
            this.ShowBtn.Text = "Show Links";
            this.toolTip1.SetToolTip(this.ShowBtn, "Display link information which can be obatined\r\nthrough the selected revit file(m" +
        "odel).");
            this.ShowBtn.UseVisualStyleBackColor = true;
            this.ShowBtn.Click += new System.EventHandler(this.ShowBtn_Click);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(1410, 353);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(145, 43);
            this.ClearBtn.TabIndex = 6;
            this.ClearBtn.Text = "Clear";
            this.toolTip1.SetToolTip(this.ClearBtn, "Clear all the information in this interface.");
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // ManageLinkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1621, 794);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.ShowBtn);
            this.Controls.Add(this.ExportBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ManageLinkForm";
            this.Text = "Manage Link";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox RevitPathText;
        private System.Windows.Forms.Button SelFileBtn;
        private System.Windows.Forms.Button ExportBtn;
        private System.Windows.Forms.Button ShowBtn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReferenceCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PositionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SavedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PathCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalAliasCol;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}