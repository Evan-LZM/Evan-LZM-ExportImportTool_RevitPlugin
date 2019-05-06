using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Export_Import_Tool
{
    public partial class ManageLinkForm : System.Windows.Forms.Form
    {
        ExternalCommandData commandData;
        Document doc;
        UIApplication uiApp;

        public ManageLinkForm(ExternalCommandData commandData)
        {

            InitializeComponent();
            this.commandData= commandData;
            ExportBtn.Enabled = false;
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;  //Eliminate the last row of the DataGridView
            ShowBtn.Enabled = false;    //If the target file is not selected, users cannot press the show button.

        }
        
        private void SelFileBtn_Click(object sender, EventArgs e)
        {
            //When the button is clicked, clearing the DataGridView and invoking the method to get selected file path
            DataGridView1.Rows.Clear();
            GetRevitFilePath();
        }

        private void GetRevitFilePath()
        {
              try
            {
                string filePath = "";
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Revit Files (*.rvt)|*.rvt";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = false;
                openFileDialog1.Multiselect = false;        //not allow multiline selection at the file selection level
                openFileDialog1.Title = "Open Revit File";   //define the name of openfileDialogC:\Users\WennyPan\source\repos\ElementInfo\ElementInfo\Form1.cs
                                                             //  openFileDialog1.InitialDirectory = @"Desktop"; //define the initial directory
                openFileDialog1.InitialDirectory = @"Desktop";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filePath = Path.GetFullPath(openFileDialog1.FileName);
                    RevitPathText.Text = filePath;
                    ShowBtn.Enabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Open File Error!");
            }
        }

        //To call the ExportFunctionForm and transfer List<string> contains the paths of all the files linked in the same project
        private void ExportBtn_Click(object sender, EventArgs e)
        {
            foreach (string path in GetSelectedItem())
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("The file selected is not valid");
                    return;
                }
            }
            ExportFunctionForm exportForm = new ExportFunctionForm(commandData, GetSelectedItem());
            exportForm.ShowDialog();
        }

        //This function return the List<string> of paths linked (displayed in the DataGridView). Every path in the list should be unique.
        public List<string> GetSelectedItem()
        {
            List<string> selectedContainer = new List<string>();
            for(int i=0; i<DataGridView1.Rows.Count; i++)
            {
                if(DataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString() == "True")
                {
                    if (!selectedContainer.Contains(DataGridView1.Rows[i].Cells[5].Value.ToString()))
                    {
                        selectedContainer.Add(DataGridView1.Rows[i].Cells[5].Value.ToString().ToUpper());
                    }
                }
            }
            if (!selectedContainer.Contains(RevitPathText.Text.ToUpper()))
            {
                selectedContainer.Add(RevitPathText.Text.ToUpper());
            }
            return selectedContainer;
        }

        //The button displays all the links which can be obtained through the selected file.
        private void ShowBtn_Click(object sender, EventArgs e)
        {
            DataGridView1.Rows.Clear();
            String revitOriFilePath = RevitPathText.Text.ToString();
            this.uiApp = commandData.Application;
            this.doc = uiApp.Application.OpenDocumentFile(revitOriFilePath);
            ObtainLinkFiles obtainLinks = new ObtainLinkFiles(doc);
            List<string[]> container = obtainLinks.GetAllRevitInstances(doc);

            foreach(string[] content in container)
            {
                string linkName = content[0];
                string status = content[1];
                string referenceType = content[2];
                string savePosition = content[3];
                string filePath = content[4];
                string pathType = content[5];
                string alias = "";
                string[] linkInformation = { "true", linkName, status, referenceType, savePosition, filePath, pathType, alias };
                DataGridView1.Rows.Add(linkInformation);
            }

            ExportBtn.Enabled = true;
        }

        //The user can select the path for "Unloaded" file. If the file has different files related and never displayed, the link will be shown.
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5)
            {
                OpenFileDialog pathSelection = new OpenFileDialog();
                pathSelection.ShowDialog();
                string selectedFilePath = pathSelection.FileName;
                string[] pathSpliter = selectedFilePath.Split('\\');
                string fileName = pathSpliter[pathSpliter.Length - 1];
                
                if(fileName.ToUpper().Trim() != DataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().ToUpper().Trim())
                {
                    MessageBox.Show("The wrong file is selected");
                    return;
                }
                DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = selectedFilePath;
                Document advancedDoc = uiApp.Application.OpenDocumentFile(selectedFilePath);
                ObtainLinkFiles obtainLinks = new ObtainLinkFiles(advancedDoc);
                List<string[]> advancedContainer = obtainLinks.GetAllRevitInstances(advancedDoc);
                List<string> linkNamesDisplayed = new List<string>();
                foreach (DataGridViewRow r in DataGridView1.Rows)
                {
                    linkNamesDisplayed.Add(r.Cells[1].Value.ToString().Trim());
                }

                foreach (string[] content in advancedContainer)
                {
                    string linkName = content[0];
                    string status = content[1];
                    string referenceType = content[2];
                    string savePosition = content[3];
                    string filePath = content[4];
                    string pathType = content[5];
                    string alias = "";
                    string[] linkInformation = { "true", linkName, status, referenceType, savePosition, filePath, pathType, alias };
                    
                    if (!linkNamesDisplayed.Contains(linkName.Trim()))
                    {
                        DataGridView1.Rows.Add(linkInformation);
                    }
                }
            }
        }

        //Set the condition of the interface to the initial.
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ExportBtn.Enabled = false;
            DataGridView1.Rows.Clear();
            RevitPathText.Text = "";
            ShowBtn.Enabled = false;
        }
    }
}
