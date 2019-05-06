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
    public partial class DatabaseSelection : Form
    {
        private int selectionresult;
        
        public int SelectionResult {
            get { return selectionresult; }
            set { selectionresult = value; }
        }
        public DatabaseSelection()
        {
            InitializeComponent();
        }

        private void Exist_Click(object sender, EventArgs e)
        {
            
            SelectionResult = 0;
            
        }

        private void CreateNew_Click(object sender, EventArgs e)
        {
            
            SelectionResult = 1;
        }
    }
}
