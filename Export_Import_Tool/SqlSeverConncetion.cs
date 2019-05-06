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
    public partial class SqlSeverConncetion : Form
    {
        private string sqlpassword;
        private string sqlusername;
        private string sqlservername;
        private string sqldatabase;
        private string checkstate;
        public string Checkstate{
            set { checkstate = value; }
            get { return checkstate; }
        }
        public string Sqlpassword {
            get { return sqlpassword; }
            set { sqlpassword = value; }
        }
        public string Sqlusername {
            get { return sqlusername; }
            set { sqlusername = value; }
        }
        public string Sqlservername {
            get { return sqlservername; }
            set { sqlservername = value; }
        }
        public string Sqldatabase {
            get { return sqldatabase; }
            set { sqldatabase = value; }
        }
        public SqlSeverConncetion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(serverName.Text)) {
                MessageBox.Show("Enter ServerName", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (serverName.Text.Contains("."))
            {
                //Checkstate = "NO";
                // this.DialogResult = DialogResult.No;
                MessageBox.Show("ServerName Contain dot notation", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(password.Text))
            {
                //Checkstate = "NO";
                //this.DialogResult = DialogResult.No;
                MessageBox.Show("Enter Password", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                this.DialogResult = DialogResult.OK;
                Checkstate = "OK";
                Sqldatabase = database.Text;
                Sqlusername = userName.Text;
                Sqlpassword = password.Text;
                Sqlservername = serverName.Text;
            }
        }

        private void serverName_TextChanged(object sender, EventArgs e)
        {
            if (serverName.Text.Contains("."))
            {
                MessageBox.Show("ServerName Contain dot notation", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serverName.Text = String.Empty;
            password.Text=String.Empty;
        }
    }
}
