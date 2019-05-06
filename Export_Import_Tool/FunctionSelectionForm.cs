using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Odbc;
using System.Data.SqlClient;

using System.Collections;

namespace Export_Import_Tool
{
    public partial class FunctionSelectionForm : System.Windows.Forms.Form
    {
        ExternalCommandData CommandData;

        public FunctionSelectionForm(ExternalCommandData commandData)
        {
            InitializeComponent();
            this.CommandData = commandData;
        }

        private void ExportFunctionButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Function is only available after the Initial Export of a Project");
            ExportFunctionForm exportForm = new ExportFunctionForm(CommandData);
            exportForm.ShowDialog();
        }

        private void ImportFuntionButton_Click(object sender, EventArgs e)
        {
            /*
            Form2 importForm = new Form2(CommandData);
            importForm.ShowDialog();
            */
            Execute_Forms.Import_Schema importForm = new Execute_Forms.Import_Schema(CommandData);
            importForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ManageLinkForm manageForm = new ManageLinkForm(CommandData);
            manageForm.ShowDialog();
        }
    }
}
