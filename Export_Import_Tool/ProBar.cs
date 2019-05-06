using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Execute_Forms
{
    public partial class ProBar : Form
    {
        public ProBar()
        {
            InitializeComponent();
            progressBar1.Value = 0;
        }

        public int proBarSG {
            set {
                this.progressBar1.Value = value;
            }
            get {
                return this.progressBar1.Value;
            }
        }
        public int proBarMax {
            set {
                this.progressBar1.Maximum = value;
            }
            get {
                return this.progressBar1.Maximum;
            }
        }

        public int proBarMin {
            set {
                this.progressBar1.Minimum = value;
            }
            get {
                return this.progressBar1.Minimum;
            }

        }

        public string label2show {
            set {
                this.label2.Text = value;
                Application.DoEvents();
            }
            get {
                return label2.Text;                
            }
        }

        public string labelShow {
            set {
                this.label1.Text = value;
                Application.DoEvents();
            }
            get {
                return this.label1.Text;
            }

        }

        public void proBarincrease() {
            this.progressBar1.Value += this.progressBar1.Value + 1;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
