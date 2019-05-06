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
    public partial class PreDatabaseSelection : Form
    {
        private string decision;
        public string predecison {
            get { return decision; }
            set { decision = value; }
        }
        public PreDatabaseSelection()
        {
            InitializeComponent();
        }

        private void localdriver_Click(object sender, EventArgs e)
        {
            predecison = "local";
        }

        private void clouddriver_Click(object sender, EventArgs e)
        {
            predecison = "cloud";
        }
    }
}
