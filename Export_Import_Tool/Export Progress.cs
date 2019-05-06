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
    public partial class Export_Progress : Form
    {
        public string labelContent
        {
            get
            {
                return this.progressLabel.Text;
            }
            set
            {
                this.progressLabel.Text = value;
                Application.DoEvents();
            }
        }

        public string percentageLabelContent
        {
            get
            {
                return this.percentageLabel.Text;
            }
            set
            {
                this.percentageLabel.Text = value;
                Application.DoEvents();
            }
        }

        public int progressValue
        {
            get
            {
                return this.progressBar1.Value;
            }
            set
            {
                this.progressBar1.Value = value;
            }
        }

        public int maxValue
        {
            get
            {
                return this.progressBar1.Maximum;
            }
            set
            {
                this.progressBar1.Maximum = value;
            }
        }

        public Export_Progress()
        {
            InitializeComponent();
            progressBar1.Value = 0;
            progressLabel.Visible = true;
            Application.DoEvents();
        }

        private void Export_Progress_Load(object sender, EventArgs e)
        {

        }
    }
}
