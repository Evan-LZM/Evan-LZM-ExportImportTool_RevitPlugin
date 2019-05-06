using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Export_Import_Tool
{
    public partial class ExistingDB : Form
    {
        public ExistingDB()
        {
            InitializeComponent();

        }
        private string selectedname;
        public string Selectedname {
            set { selectedname = value; }
            get { return selectedname; }
        }
        private List<string> list;
        public ExistingDB(List<string> listdb) {
            InitializeComponent();
            this.list = listdb;
            foreach (string namedb in listdb)
            {
                localDatabaseList.Items.Add(namedb.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Selectedname = localDatabaseList.CheckedItems.ToString();
            int a = localDatabaseList.CheckedItems.Count;
            foreach (string item in localDatabaseList.CheckedItems) {
                     Selectedname = item;
            }
            if (a==0)
            {
                MessageBox.Show("Please Select!","Alert",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void localDatabaseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelectedIndex = localDatabaseList.SelectedIndex;
            if (iSelectedIndex == -1)
                return;
            for (int iIndex = 0; iIndex < localDatabaseList.Items.Count; iIndex++)
                localDatabaseList.SetItemCheckState(iIndex, CheckState.Unchecked);
            localDatabaseList.SetItemCheckState(iSelectedIndex, CheckState.Checked);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int iIndex = 0; iIndex < localDatabaseList.Items.Count; iIndex++)
            {
                localDatabaseList.SetItemCheckState(iIndex, CheckState.Unchecked);
                localDatabaseList.SetSelected(iIndex,false);
            }
               
        }
    }
}
