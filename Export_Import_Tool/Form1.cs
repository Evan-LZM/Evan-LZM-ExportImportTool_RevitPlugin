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
    public partial class Form1 : Form
    {
        public Form1(List<string> elementNames)
        {
            InitializeComponent();
            foreach(string elementName in elementNames)
            {
                listBox1.Items.Add(elementName);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
