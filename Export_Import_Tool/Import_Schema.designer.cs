namespace Execute_Forms
{
    partial class Import_Schema
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
            this.addModel = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.confirm = new System.Windows.Forms.Button();
            this.Import = new System.Windows.Forms.Button();
            this.elementInfo = new System.Windows.Forms.DataGridView();
            this.QueryOneTable = new System.Windows.Forms.Button();
            this.clearTable = new System.Windows.Forms.Button();
            this.modelbox = new System.Windows.Forms.CheckedListBox();
            this.modelselect = new System.Windows.Forms.Button();
            this.searchTable = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.searchBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.elementInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // addModel
            // 
            this.addModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addModel.Location = new System.Drawing.Point(60, 232);
            this.addModel.Name = "addModel";
            this.addModel.Size = new System.Drawing.Size(75, 30);
            this.addModel.TabIndex = 1;
            this.addModel.Text = "Add Model";
            this.addModel.UseVisualStyleBackColor = true;
            this.addModel.Click += new System.EventHandler(this.addModel_Click);
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(344, 233);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 30);
            this.clear.TabIndex = 2;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // confirm
            // 
            this.confirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirm.Location = new System.Drawing.Point(500, 232);
            this.confirm.Name = "confirm";
            this.confirm.Size = new System.Drawing.Size(75, 30);
            this.confirm.TabIndex = 3;
            this.confirm.Text = "Confirm";
            this.confirm.UseVisualStyleBackColor = true;
            this.confirm.Click += new System.EventHandler(this.search_Click);
            // 
            // Import
            // 
            this.Import.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Import.Location = new System.Drawing.Point(676, 510);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(540, 38);
            this.Import.TabIndex = 6;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // elementInfo
            // 
            this.elementInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.elementInfo.Location = new System.Drawing.Point(676, 27);
            this.elementInfo.Name = "elementInfo";
            this.elementInfo.ReadOnly = true;
            this.elementInfo.Size = new System.Drawing.Size(540, 462);
            this.elementInfo.TabIndex = 7;
            this.elementInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.elementInfo_CellContentClick);
            // 
            // QueryOneTable
            // 
            this.QueryOneTable.Location = new System.Drawing.Point(500, 515);
            this.QueryOneTable.Name = "QueryOneTable";
            this.QueryOneTable.Size = new System.Drawing.Size(75, 28);
            this.QueryOneTable.TabIndex = 10;
            this.QueryOneTable.Text = "Query Table";
            this.QueryOneTable.UseVisualStyleBackColor = true;
            this.QueryOneTable.Click += new System.EventHandler(this.QueryOneTable_Click);
            // 
            // clearTable
            // 
            this.clearTable.Location = new System.Drawing.Point(344, 516);
            this.clearTable.Name = "clearTable";
            this.clearTable.Size = new System.Drawing.Size(75, 28);
            this.clearTable.TabIndex = 11;
            this.clearTable.Text = "Clear";
            this.clearTable.UseVisualStyleBackColor = true;
            this.clearTable.Click += new System.EventHandler(this.clearTable_Click);
            // 
            // modelbox
            // 
            this.modelbox.FormattingEnabled = true;
            this.modelbox.Location = new System.Drawing.Point(32, 27);
            this.modelbox.Name = "modelbox";
            this.modelbox.Size = new System.Drawing.Size(543, 199);
            this.modelbox.TabIndex = 12;
            // 
            // modelselect
            // 
            this.modelselect.Location = new System.Drawing.Point(209, 232);
            this.modelselect.Name = "modelselect";
            this.modelselect.Size = new System.Drawing.Size(75, 30);
            this.modelselect.TabIndex = 14;
            this.modelselect.Text = "Select All";
            this.modelselect.UseVisualStyleBackColor = true;
            this.modelselect.Click += new System.EventHandler(this.modelselect_Click);
            // 
            // searchTable
            // 
            this.searchTable.Location = new System.Drawing.Point(209, 515);
            this.searchTable.Name = "searchTable";
            this.searchTable.Size = new System.Drawing.Size(75, 28);
            this.searchTable.TabIndex = 17;
            this.searchTable.Text = "Search";
            this.searchTable.UseVisualStyleBackColor = true;
            this.searchTable.Click += new System.EventHandler(this.searchTable_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(32, 278);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(543, 211);
            this.treeView1.TabIndex = 18;
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(32, 520);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(136, 20);
            this.searchBox.TabIndex = 19;
            // 
            // Import_Schema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 583);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.searchTable);
            this.Controls.Add(this.modelselect);
            this.Controls.Add(this.modelbox);
            this.Controls.Add(this.clearTable);
            this.Controls.Add(this.QueryOneTable);
            this.Controls.Add(this.elementInfo);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.addModel);
            this.Name = "Import_Schema";
            this.Text = "Import_Schema";
            ((System.ComponentModel.ISupportInitialize)(this.elementInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button addModel;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Button confirm;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.DataGridView elementInfo;
        private System.Windows.Forms.Button QueryOneTable;
        private System.Windows.Forms.Button clearTable;
        private System.Windows.Forms.CheckedListBox modelbox;
        private System.Windows.Forms.Button modelselect;
        private System.Windows.Forms.Button searchTable;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox searchBox;
    }
}