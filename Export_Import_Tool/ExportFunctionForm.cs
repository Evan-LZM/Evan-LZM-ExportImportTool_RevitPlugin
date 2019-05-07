using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Configuration;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using System.Data.Odbc;
using System.Collections;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

namespace Export_Import_Tool
{
    public partial class ExportFunctionForm : System.Windows.Forms.Form
    {
        bool databaseExist = false;
        Document doc;
        UIApplication uiApp;
        string selectedCategoryName;
        List<string> read_only_Set = new List<string>();
        List<Element> elementSet = new List<Element>();
        List<string> parameterNames = new List<string>();
        List<ModelPath> pathSet = new List<ModelPath>();
        ExternalCommandData commandData;
        string selectedFilePath = "";
        string selectedFolderPath = "";
        StreamWriter logFileWriter;
        StreamWriter operationRecorder;
        string databaseFileStoragePath = "";
        string logFilePath = "";
        string operationRecordingPath = "";
        WaitingForm waitingForm;
        string SQLservername = "192.168.1.110";
        string SQLdatabasename = "master";
        string SQLusername = "sa";
        string SQLpassword = "Asbuiltdatabase123";
        Dictionary<string, BuiltInCategory> builtInCategoryContainer = new Dictionary<string, BuiltInCategory>();
 

        /*
        string server = "";
        string userId = "";
        string password = "";
        */

        //The constructor when the form is called by the FunctionSelectionForm
        public ExportFunctionForm(ExternalCommandData commandData)
        {
            InitializeComponent();

            this.commandData = commandData;
            CheckedListBoxOfDocuments.HorizontalScrollbar = true;
            ListBoxOfCategories.Sorted = true;
            ListBoxOfCategories.SelectionMode = SelectionMode.One;
            ListBoxOfCategories.HorizontalScrollbar = true;
            ExportButton.Enabled = false;
            DataGridView1.AllowUserToAddRows = false;
            ConfirmButton.Enabled = false;
            GroupExportButton.Enabled = false;
            CompleteProjectControlCheckBox.Checked = false;
            CompleteProjectControlCheckBox.Enabled = false;
            DateTimePicker1.ShowUpDown = true;
            ModelRadioButton.Checked = true;
            CategoryRadioButton.Enabled = false;

        }

        //The constructor when the form is called by the ManageLinkForm
        //Only GroupExportButton is available
        public ExportFunctionForm(ExternalCommandData commandData, List<string> filesList)
        {
            InitializeComponent();
            ListBoxOfCategories.Sorted = true;
            CompleteProjectControlCheckBox.Enabled = false;
            this.commandData = commandData;
            GroupExportButton.Enabled = true;
            ConfirmButton.Enabled = false;
            CheckedListBoxOfDocuments.HorizontalScrollbar = true;
            ListBoxOfCategories.HorizontalScrollbar = true;
            DataGridView1.AllowUserToAddRows = false;
            ExportButton.Enabled = false;
            ModelRadioButton.Checked = true;
            foreach (string filePath in filesList)
            {
                CheckedListBoxOfDocuments.Items.Add(filePath);
            }
            for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
            {
                CheckedListBoxOfDocuments.SetItemChecked(i, true);
            }
            BrowsingButton.Enabled = false;
            ExportButton.Enabled = false;
            DateTimePicker1.ShowUpDown = true;
            CategoryRadioButton.Enabled = false;
        }

        //The method to create database connection string
        public string DatabaseConnectionString()
        {
            string connString = @"Driver={Sql Server}; Server="+ SQLservername + "; Database="+ SQLdatabasename + "; uid="+ SQLusername + "; pwd="+ SQLpassword + "";   //Please customize the database connection information before usage
            //string connString = @"server=LHC-PC\SQLEXPRESS;database=master;Integrated Security=SSPI;driver={sql server}";
            //string connString = ConfigurationManager.AppSettings["conn"];
            return connString;
        }


        private void SetDoc(Document document)
        {
            this.doc = document;
        }

        //When the selected item in the list box is changed, the content of the selected item will be obtained and displayed in the DataGridView
        private void ListBoxOfCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            elementSet.Clear();
            parameterNames.Clear();
            DataGridView1.Columns.Clear();
            if(ListBoxOfCategories.SelectedItem == null)
            {
                return;
            }
            selectedCategoryName = ListBoxOfCategories.SelectedItem.ToString();
            bool specialContent = false;


 
            //Obtain all the instance elements in the selected revit model
            ElementFilter f = new LogicalOrFilter(new ElementIsElementTypeFilter(false), new ElementIsElementTypeFilter(true));
            FilteredElementCollector collector = new FilteredElementCollector(doc).WherePasses(f);
            IList<Element> elementContainer = collector.OfClass(typeof(FamilyInstance)).ToElements();

            
            foreach (KeyValuePair<string, BuiltInCategory> kv in builtInCategoryContainer)
            {
                //Match the displayed category name with the built in Category and get their elements
                string possibleDisplayName1 = kv.Key.Substring(4, kv.Key.Length - 4);
                string possibleDisplayName2 = kv.Key.Substring(4, kv.Key.Length - 4) + "s";
                string possibleDisplayName3 = "MEP" + kv.Key.Substring(4, kv.Key.Length - 4);
                string possibleDisplayName4 = kv.Key.Replace("_", "");
                string possibleDisplayName5 = kv.Key.Substring(4, kv.Key.Length - 5) + "ies";
                string possibleDisplayName6 = kv.Key.Substring(4, kv.Key.Length - 4) + "es";

                if (possibleDisplayName1 == selectedCategoryName 
                    || possibleDisplayName2 == selectedCategoryName 
                    || possibleDisplayName3 == selectedCategoryName
                    || possibleDisplayName4 == selectedCategoryName 
                    || possibleDisplayName5 == selectedCategoryName
                    || possibleDisplayName6 == selectedCategoryName
                    || possibleDisplayName1 == selectedCategoryName.Substring(0, selectedCategoryName.Length - 1) + "Curves")
                {
                    collector = new FilteredElementCollector(doc);
                    elementContainer = collector.OfCategory(kv.Value).WhereElementIsNotElementType().ToElements();
                    specialContent = true;
                    break;
                }
            }

            //Special Categories Handler
            if(selectedCategoryName == "Elevations")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_SpotElevations).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if(selectedCategoryName == "Spaces")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_AnalyticSpaces).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if(selectedCategoryName == "StructuralConnections")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_StructConnections).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if(selectedCategoryName == "StructuralFabricAreas")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_FabricAreas).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if(selectedCategoryName == "ViewReference")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_ReferenceViewer).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if (selectedCategoryName == "ModelGroups")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if (selectedCategoryName == "StructuralBeamSystems")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfClass(typeof(BeamSystem)).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if (selectedCategoryName == "StructuralRebar")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            else if (selectedCategoryName == "ViewTitles")
            {
                collector = new FilteredElementCollector(doc);
                elementContainer = collector.OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType().ToElements();
                specialContent = true;
            }

            //Add elements belonged to the selected category to "elementSet" container
            if (specialContent == false)
            {
                foreach (Element elem in collector)
                {

                    if (elem.Category != null)
                    {
                        //Finding matched instances and format the name of the category
                        string elemCategoryName = elem.Category.Name;
                        elemCategoryName = elemCategoryName.Replace(" ", "");
                        elemCategoryName = elemCategoryName.Replace(".", "_");
                        if (selectedCategoryName == elemCategoryName)
                        {
                            elementSet.Add(elem);
                        }
                    }

                }
            }
            else if (specialContent == true)
            {
                foreach (Element elem in collector)
                {
                    elementSet.Add(elem);
                }
            }
            



            //Obtain parameter Names (Column Names) of the element in the "elementSet"
            foreach (Element elem in elementSet)
            {

                ParameterSet parameters = elem.Parameters;
                foreach (Autodesk.Revit.DB.Parameter parameter in parameters)
                {
                    //Find read-only instances and record them in the form of "RO_CategoryName_ParameterName"
                    if (parameter.IsReadOnly == true)
                    {
                        string read_only_Category = elem.Category.Name.Replace(" ", "").Replace(".", "_");
                        string read_only_Parameter = parameter.Definition.Name;//.Replace(" ", "");
                        string read_only_Pair = "RO_" + read_only_Category + "_" + read_only_Parameter;
                        if (!read_only_Set.Contains(read_only_Pair))
                        {
                            read_only_Set.Add(read_only_Pair);
                        }
                    }
                    //Format the parameter name
                    string name = parameter.Definition.Name;//.Replace(" ", "");
                    if (!parameterNames.Contains(name))
                    {
                        parameterNames.Add(name);
                    }
                    
                }    
            }
            

            

            //Construct the DataGridView
            int i = 0;
            int count = parameterNames.Count() + 1;
            if(parameterNames.Count != 0)
            {
                count = parameterNames.Count() + 5;
            }
            DataGridView1.ColumnCount = count;

            DataGridView1.Columns[i].HeaderText = "ElementId";
            i++;

            string[] temp = new string[count - 1];
            if (parameterNames.Count != 0)
            {
                temp = new string[count - 5];
            }
            int c = 0;
            foreach (string attribute in parameterNames)
            {
                temp[c] = attribute;
                c++;
            }
            Array.Sort(temp);  //Sort the elements in temp[]

            int d = 0;
            foreach (string attribute in parameterNames)
            {
                DataGridView1.Columns[i].HeaderText = temp[d];
                d++;
                i++;
            }

            //Add Coordinate information columns and the count column
            if (parameterNames.Count != 0)
            {
                DataGridView1.Columns[i].HeaderText = "Position_X";
                DataGridView1.Columns[i + 1].HeaderText = "Position_Y";
                DataGridView1.Columns[i + 2].HeaderText = "Position_Z";
                DataGridView1.Columns[i + 3].HeaderText = "Count";
            }


            foreach (Element elem in elementSet)
            {
                string position_X = "";
                string position_Y = "";
                string position_Z = "";

                LocationPoint lp = elem.Location as LocationPoint;
               
                if(lp != null)
                {
                    XYZ position = lp.Point;
                    position_X = Math.Round((position.X * 304.8),0).ToString(); //The data obtained are in feet unit. Transfer the value to milimeter unit.
                    position_Y = Math.Round((position.Y * 304.8),0).ToString();
                    position_Z = Math.Round((position.Z * 304.8),0).ToString();
                }
                

                ParameterSet parameters = elem.Parameters;
                string elementId = elem.Id.ToString();
                string[] eachElementParameterContainer = new string[count];
                eachElementParameterContainer[0] = elementId;
                foreach (Autodesk.Revit.DB.Parameter p in parameters)
                {
                    for (int x = 1; x < count; x++)
                    {
                        if(DataGridView1.Columns[x].HeaderText == "Position_X")
                        {
                            eachElementParameterContainer[x] = position_X;
                        }
                        else if(DataGridView1.Columns[x].HeaderText == "Position_Y")
                        {
                            eachElementParameterContainer[x] = position_Y;
                        }
                        else if (DataGridView1.Columns[x].HeaderText == "Position_Z")
                        {
                            eachElementParameterContainer[x] = position_Z;
                        }
                        else if(DataGridView1.Columns[x].HeaderText == "Count")
                        {
                            eachElementParameterContainer[x] = "1";
                        }
                        else if (p.Definition.Name == DataGridView1.Columns[x].HeaderText) //.Replace(" ", "")
                        {
                            //Obtain parameter values
                            //eachElementParameterContainer[x] = p.AsValueString();
                            eachElementParameterContainer[x] = GetParameterType(p);
                        }
                    }
                }
                DataGridView1.Rows.Add(eachElementParameterContainer);
            }
            
        }



        //The method invoked when a file is selected in the list box of files
        private void CheckedListBoxOfDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxOfCategories.Items.Clear();
            DataGridView1.Columns.Clear();
            builtInCategoryContainer.Clear();
            if(GroupExportButton.Enabled == false)
            {
                ExportButton.Enabled = true;
            }
            CategoryRadioButton.Enabled = false;
            ModelRadioButton.Checked = true;
            string folderPath = selectedFolderPath;
            if (CheckedListBoxOfDocuments.SelectedItem == null)
                return;
            if (CheckedListBoxOfDocuments.SelectedItems.Count == 1)
            {
                ConfirmButton.Enabled = true;
            }
            else
            {
                ConfirmButton.Enabled = false;
            }
            //string fileName = CheckedListBoxOfDocuments.SelectedItem.ToString();
            //string filePath = folderPath + "\\" + fileName;
            this.selectedFilePath = CheckedListBoxOfDocuments.SelectedItem.ToString();
        }



        //The method to display categories of the selected file 
        public void DisplayCategories()
        {
            CategoryRadioButton.Enabled = true;

            ListBoxOfCategories.Items.Clear();
            DataGridView1.Columns.Clear();
            this.uiApp = commandData.Application;
            Document Doc = uiApp.Application.OpenDocumentFile(selectedFilePath);    //Open the document in the background, but not activate
            SetDoc(Doc);
            /*
            Settings set = doc.Settings;        //Get settings which are system information object of the document
            Categories cats = set.Categories;   //Get the Categories object from settings
            foreach (Category cat in cats)      //Traverse each Category in the Categories and obtain their names
            {
                string categoryName = cat.Name;
                if (cat.Name.Contains(" "))
                {
                    categoryName = categoryName.Replace(" ", "");
                }
                if (cat.Name.Contains("."))
                {
                    categoryName = categoryName.Replace(".", "_");
                }
                ListBoxOfCategories.Items.Add(categoryName);
            }
            */
            
            string[] displayCategories = { "AirTerminals","Assemblies","BridgeCables","BridgeDecks","BridgeFoundations"
                                            ,"BridgeGirders","CableTrayFittings","CableTrayRuns","CableTrays","Casework"
                                            ,"Ceilings","Columns","CommunicationDevices","ConduitFittings","ConduitRuns"
                                            ,"Conduits","CurtainGrids","CurtainPanels","CurtainSystems","CurtainWallMullions"
                                            ,"DataDevices","DetailItems","Doors","DuctAccessories","DuctFittings"
                                            ,"DuctInsulations","DuctLinings","Ducts","DuctSystems","ElectricalCircuits"
                                            ,"ElectricalEquipment","ElectricalFixtures","Elevations","Entourage","FireAlarmDevices"
                                            ,"FlexDucts","FlexPipes","Floors","Furniture","FurnitureSystems"
                                            ,"GenericModels","Grids","HVACZones","Levels","LightingDevices","LightingFixtures"
                                            ,"Mass","Materials","MechanicalEquipment","MEPFabricationContainment","MEPFabricationDuctwork"
                                            ,"MEPFabricationHangers","MEPFabricationPipework","ModelGroups","NurseCallDevices","PipeAccessories"
                                            ,"PipeFittings","PipeInsulations","Pipes","PipingSystems","Planting", "ProjectInformation"
                                            ,"Railings","Ramps","Roads","Roofs","Rooms"
                                            ,"RvtLinks","Schedules","SecurityDevices","Sheets","Site"
                                            ,"Spaces","SpecialityEquipment","SpotCoordinates","Sprinklers","StairsPaths"
                                            ,"Stairs","StructuralBeamSystems","StructuralColumns","StructuralConnections","StructuralFabricAreas"
                                            ,"StructuralFoundations","StructuralFraming","StructuralRebar","StructuralStiffeners","StructuralTrusses"
                                            ,"TelephoneDevices","Topography","Viewports","ViewReference","Views"
                                            ,"ViewTitles","Walls","Windows","Wires" };
            foreach(string category in displayCategories)
            {
                ListBoxOfCategories.Items.Add(category);
            }

            ElementFilter f = new LogicalOrFilter(new ElementIsElementTypeFilter(false), new ElementIsElementTypeFilter(true));
            FilteredElementCollector collector = new FilteredElementCollector(doc).WherePasses(f);
            IList<Element> elementContainer = collector.ToElements();

            foreach(Element elem in collector)
            {
                if(elem.Category != null)
                {
                    BuiltInCategory builtInCategory = (BuiltInCategory)elem.Category.Id.IntegerValue;
                    string builtInCategoryName = builtInCategory.ToString();
                    if (!builtInCategoryContainer.Keys.Contains(builtInCategoryName))
                    {
                        builtInCategoryContainer.Add(builtInCategoryName, builtInCategory);
                    }
                }
            }
            /*
            string logFileName = @"E:\AutoDesk\OST_.txt";
            logFileWriter = new StreamWriter(logFileName, false);
            foreach(string key in builtInCategoryContainer.Keys)
            {
                logFileWriter.WriteLine(key + "\n");
            }
            logFileWriter.Close();
            */
        }

        //Obtain the stoarage path of the database
        public void StoragePathObtain()
        {
            string conn = DatabaseConnectionString();
            if (conn != @"Driver={Sql Server}; Server=192.168.1.110; Database=master; uid=sa; pwd=Asbuiltdatabase123")
            {
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                folderBrowser.Description = "Choose the Storage Place for Database Files";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(folderBrowser.SelectedPath))
                    {
                        MessageBox.Show(this, "The folder path can not be empty", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }

                    string folderPath = folderBrowser.SelectedPath.ToString();
                    this.databaseFileStoragePath = folderPath;

                    if (databaseFileStoragePath.Substring(0, 1) == "C")
                    {
                        MessageBox.Show(this, "The database files cannot be stored in C disk","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        this.databaseFileStoragePath = "";
                        return;
                    }
                }
            }
            else
            {
                databaseFileStoragePath = "D:\\MSSQL\\DATA";
            }
        }



        //The basic export method
        public void OdbcExport()
        {
            if (logFilePath == "")
            {
                LogFilePathObtain();
            }
            //Obtain schemaName from the file path in list box of files
            string[] pathSpliter2 = CheckedListBoxOfDocuments.SelectedItem.ToString().Split('\\');
            string selectedFileName = pathSpliter2[pathSpliter2.Length - 1].Substring(0, pathSpliter2[pathSpliter2.Length - 1].Length - 4);
            DateTime currentTime2 = System.DateTime.Now;
            string dateTime = currentTime2.ToString("yyyy_MM_dd").Replace("_", "") + currentTime2.Hour.ToString() + currentTime2.Minute.ToString();
            string logFileName = logFilePath + @"\" + selectedFileName + "_ExportLogFile_" + dateTime + ".txt";
            logFileWriter = new StreamWriter(logFileName, false);
            logFileWriter.WriteLine("Odbc export button is clicked" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));

            DataGridView1.Enabled = false;

            string[] pathSpliter = CheckedListBoxOfDocuments.SelectedItem.ToString().Split('\\');
            string fileName = pathSpliter[pathSpliter.Length - 1];
            string schemaName = fileName.Substring(0, fileName.Length - 4);
            DateTime currentTime = new DateTime();
            currentTime = System.DateTime.Now;
            string strDate = currentTime.ToString("yyyy_MM_dd");

            //If the planned time is later than current time, the waiting form will be invoked
            if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
            {
                waitingForm = new WaitingForm(DateTimePicker1.Value, this);
                waitingForm.ShowDialog();
            }
            //Checking whether the planned time is reached. If not, that's because the user has pressed the cancel button in waitingForm.
            //Then, the export process should be canceled
            if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
            {
                return;
            }
            //Check the existance of the schema and take different actions
            CheckSchema(schemaName);
            logFileWriter.Close();
            
        }

        //Check whether the schema exists
        //If exists, continue the process
        //Otherwise, throw out a warning and stop the process
        public void CheckSchema(string schemaName)
        {
            logFileWriter.WriteLine("Check the existance of the content..." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();

            string sqlDBNObtain = "select name from master..sysdatabases";
            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlDBNObtain, conn);
            List<string> dbNames = new List<string>();
            List<string> possibleDBNames = new List<string>();
            ArrayList dbNameList = new ArrayList();
            DataTable dbNameTable = new DataTable();
            adapter.Fill(dbNameTable);

            foreach (DataRow row in dbNameTable.Rows)
            {
                dbNameList.Add(row["name"]);
            }

            foreach (var dbName in dbNameList)
            {
                dbNames.Add(dbName.ToString());
            }

            foreach (string dbName in dbNames)
            {
                if (dbName.Length > 11)
                {
                    string dbfileName = dbName.Substring(0, dbName.Length - 11);
                    possibleDBNames.Add(dbName);
                }
            }

            bool schemaExist = false;

            foreach (string dbName in possibleDBNames)
            {
                List<string> schemasInDB = new List<string>();
                string dbNameWithoutSuffix = dbName.Substring(0, dbName.Length - 11);
                string useDBSql = "USE [" + dbName + "];";
                OdbcCommand useDBCommand = new OdbcCommand(useDBSql, conn);
                useDBCommand.ExecuteNonQuery();

                //Find all the schema names in the database
                string findSchemaSql = "SELECT DISTINCT TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES;";
                OdbcCommand findSchemaCommand = new OdbcCommand(findSchemaSql, conn);
                OdbcDataReader schemaReader = findSchemaCommand.ExecuteReader();
                while (schemaReader.Read())
                {
                    schemasInDB.Add(schemaReader[0].ToString().ToUpper());
                }
                if (schemasInDB.Contains(schemaName.ToUpper()))   //If the schema exists, performing the update function. Otherwise, pop up an error message.
                {
                    schemaExist = true;
                    UpdateSchema(dbName, schemaName);
                    break;
                }
            }

            if (schemaExist == false)
            {
                MessageBox.Show("The project(database) related has not been exported!\nYou need to export the whole project with Manage Link Function first.","Claim",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

            conn.Close();
        }


        public void UpdateSchema(string databaseName, string schemaName)
        {
            string oldDatabaseName = databaseName;
            logFileWriter.WriteLine("The database which related to the project exists...UPDATE" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string newDatabaseName = databaseName.Substring(0, databaseName.Length - 11) + System.DateTime.Now.ToString("_yyyy_MM_dd");
            GroupUpdateDatabase(newDatabaseName, oldDatabaseName, schemaName);

            OdbcConnection conn = new OdbcConnection(DatabaseConnectionString());
            conn.Open();
            string sqlRename = "EXEC sp_renamedb [" + oldDatabaseName + "], [" + newDatabaseName + "]";
            OdbcCommand reNameCommand = new OdbcCommand(sqlRename, conn);
            reNameCommand.ExecuteNonQuery();
            logFileWriter.WriteLine("The database [" + oldDatabaseName + "] is renamed to [" + newDatabaseName + "]!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            conn.Close();
        }



        //The database creation method in SQL Server
        public void ConstructDatabases(string databaseName)
        {
            databaseName = databaseName.Substring(0, databaseName.Length - 11);
            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();
            string sqlString = "create database [" + databaseName + DateTime.Now.ToString("_yyyy_MM_dd") + "] on primary (name = '" + databaseName + @"',filename='" + databaseFileStoragePath + @"\" + databaseName + @".mdf',size = 5MB, filegrowth = 1024Kb) log on (name = '" + databaseName + @"_log',filename = '" + databaseFileStoragePath + @"\" + databaseName + @"_log.ldf',size = 1024KB,filegrowth = 10%); alter database [" + databaseName + DateTime.Now.ToString("_yyyy_MM_dd") + "] collate Chinese_PRC_CS_AS;";
            OdbcCommand createCommand = new OdbcCommand(sqlString, conn);
            createCommand.ExecuteNonQuery();
            conn.Close();
            logFileWriter.WriteLine("The database [" + databaseName + "] is created" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
        }





        //The method to export the selected file
        private void SelectedFilesExport()
        {

            List<string> selectedFiles = new List<string>();
            foreach (object item in CheckedListBoxOfDocuments.CheckedItems)
            {
                selectedFiles.Add(item.ToString());
            }
            CheckedListBoxOfDocuments.SelectionMode = SelectionMode.One;  //Set back the selection mode to one 

            LogFilePathObtain();
            if(logFilePath == "")
            {
                return;
            }
            for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
            {
                if (selectedFiles.Contains(CheckedListBoxOfDocuments.Items[i]))
                {
                    CheckedListBoxOfDocuments.SelectedItem = CheckedListBoxOfDocuments.Items[i]; //Traversing all the items and find the matched ones
                    ConfirmButton.PerformClick();    //Let the Categories displayed.
                    OdbcExport();
                }
            }

            MessageBox.Show("All Export/Update Successful","Compelete");
            DataGridView1.Enabled = true;
            databaseFileStoragePath = "";
            logFilePath = "";
        }




        //The method is used to transfer all the revit data to number expression
        public string GetParameterType(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsValueString() + "," + parameter.AsDouble().ToString();

                case StorageType.ElementId:
                    return parameter.AsValueString() + "," + parameter.AsElementId().IntegerValue.ToString();

                case StorageType.Integer:
                    return parameter.AsValueString() + "," + parameter.AsInteger().ToString();

                case StorageType.None:
                    return parameter.AsValueString();

                case StorageType.String:
                    return parameter.AsValueString();

                default:
                    return "";
            }
        }


        //The method used to generate the html update report after the update process based on the information recorded in the container
        public void GenerateUpdateHtml(List<string> changedCategoryNames, Dictionary<string, List<string[]>> changedContainer)
        {
            string[] pathSpliter = CheckedListBoxOfDocuments.SelectedItem.ToString().Split('\\');
            string selectedFileName = pathSpliter[pathSpliter.Length - 1].Substring(0, pathSpliter[pathSpliter.Length - 1].Length - 4);
            DateTime currentTime = System.DateTime.Now;
            string dateTime = currentTime.ToString("yyyy_MM_dd").Replace("_", "") + currentTime.Hour.ToString() + currentTime.Minute.ToString();

            string displayChangedCategoryNames = "";
            foreach (string changedCategoryName in changedCategoryNames)
            {
                displayChangedCategoryNames += changedCategoryName + ", ";
            }
            displayChangedCategoryNames = displayChangedCategoryNames.Substring(0, displayChangedCategoryNames.Length - 2);


            string htmlFolderPath = "";
            for (int i = 0; i < pathSpliter.Length - 1; i++)
            {
                htmlFolderPath += pathSpliter[i] + "\\";
            }

            string fileName = logFilePath + @"\" + selectedFileName + "_UpdateReport_" + dateTime + ".html";

            StreamWriter sw = new StreamWriter(fileName, false);
            sw.WriteLine("<!doctype html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<meta charset=\"utf-8\">");
            sw.WriteLine("<title>Export Report</title>");
            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
            sw.WriteLine("<h1>Export Report</h1>");
            sw.WriteLine("<h2 style=\"color:#0000FF\">Changed Category: " + displayChangedCategoryNames + "</h2>");
            foreach (KeyValuePair<string, List<string[]>> kv in changedContainer)
            {
                string categoryName = kv.Key;
                sw.WriteLine("<h3 style=\"color:#33CCCC\">" + categoryName + "</h3>");
                sw.WriteLine("<table border=\"1\">");
                sw.WriteLine("<tr>");
                sw.WriteLine("<th>ElementId</th>");
                sw.WriteLine("<th>Operation</th>");
                sw.WriteLine("<th>Category</th>");
                sw.WriteLine("<th>Parameter</th>");
                sw.WriteLine("<th>OldValue</th>");
                sw.WriteLine("<th>NewValue</th>");
                sw.WriteLine("<th>Date</th>");
                sw.WriteLine("</tr>");
                foreach (string[] valueContainer in kv.Value)
                {
                    sw.WriteLine("<tr>");
                    for (int i = 0; i < 7; i++)
                    {
                        sw.WriteLine("<td>" + valueContainer[i] + "</td>");
                    }
                    sw.WriteLine("</tr>");
                }
                sw.WriteLine("</table>");
            }
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
            sw.Close();
        }

        //When the confirm button is clicked, it will invoke the method to display Categories inside the selected file
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            DisplayCategories();
        }

        //The function to import value back to a parameter
        public static void SetParameterValue(Parameter p, object value)
        {
            try
            {
                if (value.GetType().Equals(typeof(string)))
                {
                    if (p.SetValueString(value as string))
                        return;
                }

                switch (p.StorageType)
                {
                    case StorageType.None:
                        break;
                    case StorageType.Double:
                        if (value.GetType().Equals(typeof(string)))
                        {
                            p.Set(double.Parse(value as string));
                        }
                        else
                        {
                            p.Set(Convert.ToDouble(value));
                        }
                        break;
                    case StorageType.Integer:
                        if (value.GetType().Equals(typeof(string)))
                        {
                            p.Set(int.Parse(value as string));
                        }
                        else
                        {
                            p.Set(Convert.ToInt32(value));
                        }
                        break;
                    case StorageType.ElementId:
                        if (value.GetType().Equals(typeof(ElementId)))
                        {
                            p.Set(value as ElementId);
                        }
                        else if (value.GetType().Equals(typeof(string)))
                        {
                            p.Set(new ElementId(int.Parse(value as string)));
                        }
                        else
                        {
                            p.Set(new ElementId(Convert.ToInt32(value)));
                        }
                        break;
                    case StorageType.String:
                        p.Set(value.ToString());
                        break;
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Invalid Value Input!");
                MessageBox.Show(e.ToString());
            }
        }

        /*
        private void button1_Click(object sender, EventArgs e)
        {
            Transaction ta = new Transaction(doc, "test");
            ta.Start();
            int st = 12;
            ElementId eId = new ElementId(975217);
            Element targetElem = doc.GetElement(eId);
            Parameter p = targetElem.LookupParameter("Closer");
            SetParameterValue(p, st.ToString());
            MessageBox.Show(p.AsValueString());
            ta.Commit();
        }
        */


        //This method will export selected categories(tables)
        private void SelectedCategoryExport()
        {
            LogFilePathObtain();
            if (logFilePath == "")
            {
                return;
            }
            //Obtain the Schema Name
            string[] pathSpliter = CheckedListBoxOfDocuments.SelectedItem.ToString().Split('\\');
            string selectedFileName = pathSpliter[pathSpliter.Length - 1].Substring(0, pathSpliter[pathSpliter.Length - 1].Length - 4);
            string schemaName = selectedFileName.ToUpper();

            DateTime currentTime = System.DateTime.Now;
            string dateTime = currentTime.ToString("yyyy_MM_dd").Replace("_", "") + currentTime.Hour.ToString() + currentTime.Minute.ToString();

            string logFileName = logFilePath + @"\" + selectedFileName + "_ExportLogFile_" + dateTime + ".txt";
            logFileWriter = new StreamWriter(logFileName, false);
            logFileWriter.WriteLine("Selected Category Export Button is clicked" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));


            DataGridView1.Enabled = false;
            //The second version of checking the existance of the schema


            if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
            {
                waitingForm = new WaitingForm(DateTimePicker1.Value, this);
                waitingForm.ShowDialog();
            }
            if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
            {
                return;
            }

            CheckSchema2(schemaName);

            MessageBox.Show("Export/Update Successful");
            logFileWriter.Close();
            ModelRadioButton.Checked = true;
            ListBoxOfCategories.SelectionMode = SelectionMode.One;
            DataGridView1.Enabled = true;
            databaseFileStoragePath = "";
            logFilePath = "";
        }

        public void CheckSchema2(string schemaName)
        {

            logFileWriter.WriteLine("Check the existance of the content..." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();

            string sqlDBNObtain = "select name from master..sysdatabases";
            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlDBNObtain, conn);
            List<string> dbNames = new List<string>();
            List<string> possibleDBNames = new List<string>();
            ArrayList dbNameList = new ArrayList();
            DataTable dbNameTable = new DataTable();
            adapter.Fill(dbNameTable);

            foreach (DataRow row in dbNameTable.Rows)
            {
                dbNameList.Add(row["name"]);
            }

            foreach (var dbName in dbNameList)
            {
                dbNames.Add(dbName.ToString());
            }

            foreach (string dbName in dbNames)
            {
                if (dbName.Length > 11)
                {
                    string dbfileName = dbName.Substring(0, dbName.Length - 11);
                    possibleDBNames.Add(dbName);
                }
            }

            bool schemaExist = false;

            foreach (string dbName in possibleDBNames)
            {
                List<string> schemasInDB = new List<string>();
                string dbNameWithoutSuffix = dbName.Substring(0, dbName.Length - 11);
                string useDBSql = "USE [" + dbName + "];";
                OdbcCommand useDBCommand = new OdbcCommand(useDBSql, conn);
                useDBCommand.ExecuteNonQuery();

                string findSchemaSql = "SELECT DISTINCT TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES;";
                OdbcCommand findSchemaCommand = new OdbcCommand(findSchemaSql, conn);
                OdbcDataReader schemaReader = findSchemaCommand.ExecuteReader();
                while (schemaReader.Read())
                {
                    schemasInDB.Add(schemaReader[0].ToString().ToUpper());
                }
                if (schemasInDB.Contains(schemaName.ToUpper()))
                {
                    schemaExist = true;
                    UpdateSchema2(dbName, schemaName);
                    break;
                }
            }

            if (schemaExist == false)
            {
                MessageBox.Show("The project(database) related has not been exported!\nYou need to export the whole project with Manage Link Function first.");
            }

            conn.Close();
        }

        //Match the selected schema
        public void UpdateSchema2(string databaseName, string schemaName)
        {
            string oldDatabaseName = databaseName;
            string newDatabaseName = databaseName.Substring(0, databaseName.Length - 11) + System.DateTime.Now.ToString("_yyyy_MM_dd"); 

            List<string> categorySelected = new List<string>();
            foreach (var item in ListBoxOfCategories.SelectedItems)
            {
                string selectedCategoryName = item.ToString();
                categorySelected.Add(selectedCategoryName);
            }

            Export_Progress exportForm = new Export_Progress();
            exportForm.Show();
            exportForm.maxValue = ListBoxOfCategories.SelectedItems.Count;

            logFileWriter.WriteLine("The database which related to the project exists...UPDATE" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));

            logFileWriter.WriteLine("The Update process starts!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            ListBoxOfCategories.SelectionMode = SelectionMode.One;

            for (int i = 0; i < ListBoxOfCategories.Items.Count; i++)
            {
                string categoryName = ListBoxOfCategories.Items[i].ToString();
                
                if (categorySelected.Contains(categoryName))
                {
                    //ListBoxOfCategories.SelectionMode = SelectionMode.One;
                    ListBoxOfCategories.SelectedItem = ListBoxOfCategories.Items[i];
                    GroupUpdateDatabase2(newDatabaseName, oldDatabaseName, schemaName, i, exportForm);
                }
            }

            OdbcConnection conn = new OdbcConnection(DatabaseConnectionString());
            conn.Open();
            string sqlRename = "EXEC sp_renamedb [" + oldDatabaseName + "], [" + newDatabaseName + "]";
            OdbcCommand reNameCommand = new OdbcCommand(sqlRename, conn);
            reNameCommand.ExecuteNonQuery();
            logFileWriter.WriteLine("The database [" + oldDatabaseName + "] is renamed to [" + newDatabaseName + "]!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            conn.Close();

            exportForm.Dispose();
        }


        //Start update specific table in the database
        public void GroupUpdateDatabase2(string newDatabaseName, string oldDatabaseName, string schemaName, int i, Export_Progress exportForm)
        {

            string oldDate = oldDatabaseName.Substring(oldDatabaseName.Length - 10, 10);
            oldDate = oldDate.Replace("_", "-");
            string newDate = newDatabaseName.Substring(newDatabaseName.Length - 10, 10);
            newDate = newDate.Replace("_", "-");

            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();

            List<string> changedCategoryNames = new List<string>();
            Dictionary<string, List<string[]>> changedContainer = new Dictionary<string, List<string[]>>();

            
            List<string[]> changedValueContainer = new List<string[]>();
            string tableName = ListBoxOfCategories.Items[i].ToString();
            logFileWriter.WriteLine("Table [" + schemaName + "].[" + tableName + "] starts updating ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            GroupUpdateFunction(i, exportForm, oldDatabaseName, conn, newDate, changedValueContainer, changedCategoryNames, changedContainer, schemaName);


            if (changedCategoryNames.Count > 0)
            {
                GenerateUpdateHtml(changedCategoryNames, changedContainer);
            }

            conn.Close();

            
        }


       

        //Reset the content in the interface
        private void ResetButton_Click(object sender, EventArgs e)
        {
            databaseFileStoragePath = "";
            CheckedListBoxOfDocuments.Items.Clear();
            ListBoxOfCategories.Items.Clear();
            DataGridView1.Columns.Clear();
            ExportButton.Enabled = false;
            ConfirmButton.Enabled = false;
            CategoryRadioButton.Enabled = false;
            ModelRadioButton.Checked = true;
            GroupExportButton.Enabled = false;
            CompleteProjectControlCheckBox.Checked = false;
            CompleteProjectControlCheckBox.Enabled = false;
            DateTimePicker1.Value = DateTime.Now;
            BrowsingButton.Enabled = true;
        }

        private List<string> ListDatbaseName(string servername,string databasename,string username, string password) {
            List<string> listdb = new List<string>();
            string ckconn = @"Driver={Sql Server}; Server=" + servername + "; Database=" + databasename + "; uid=" + username + "; pwd=" + password + "";
            try
            {
                OdbcConnection odbcconn = new OdbcConnection(ckconn);
                odbcconn.Open();
                string searchdblist = "select name from sys.databases";
                OdbcCommand odbcCommand = new OdbcCommand(searchdblist, odbcconn);
                OdbcDataReader odbcData = odbcCommand.ExecuteReader();
                while (odbcData.Read()) {
                    listdb.Add(odbcData[0].ToString());
                }
                odbcconn.Close();
                return listdb;
            }
            catch {
                return null;
            }
        }

        //The button to invoke the creation of nested structure database with the models displayed on the left
        private void GroupExportButton_Click(object sender, EventArgs e)
        {
            using (PreDatabaseSelection predata = new PreDatabaseSelection()) {
                if (predata.ShowDialog() == DialogResult.OK) {
                    if (predata.predecison.Equals("local")) {
                        //  using (SqlSeverConncetion sqlcon = new SqlSeverConncetion()) {
                        SqlSeverConncetion sqlcon = new SqlSeverConncetion();
                        if (sqlcon.ShowDialog() == DialogResult.OK && sqlcon.Checkstate.Equals("OK")&& (!sqlcon.Sqlservername.Contains(".")))
                        {
                            //Check the Connection
                            string ckconn = @"Driver={Sql Server}; Server=" + sqlcon.Sqlservername + "; Database=" + sqlcon.Sqldatabase + "; uid=" + sqlcon.Sqlusername + "; pwd=" + sqlcon.Sqlpassword + "";
                            try
                            {
                                OdbcConnection odbcconn = new OdbcConnection(ckconn);
                                odbcconn.Open();
                                odbcconn.Close();
                                //If success
                                this.SQLservername = sqlcon.Sqlservername;
                                this.SQLusername = sqlcon.Sqlusername;
                                this.SQLpassword = sqlcon.Sqlpassword;
                                this.SQLdatabasename = sqlcon.Sqldatabase;
                                using (DatabaseDecision selec = new DatabaseDecision())
                                {
                                  if (selec.ShowDialog() == DialogResult.OK) {
                                    
                                    if (selec.SelectionResult == 1)
                                    {//CreateNew Local Database
                                        StoragePathObtain();
                                        if (databaseFileStoragePath == "")
                                        {
                                            return;
                                        }
                                        string projectName = CheckedListBoxOfDocuments.Items[0].ToString();
                                        for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
                                        {
                                            string pathString = CheckedListBoxOfDocuments.Items[i].ToString();
                                            string[] pathSpliter = pathString.Split('\\');
                                            string fullFileName = pathSpliter[pathSpliter.Length - 1];
                                            string fileNameWithoutSuffix = fullFileName.Substring(0, fullFileName.Length - 4);
                                            if (fileNameWithoutSuffix.Length < projectName.Length)
                                            {
                                                projectName = fileNameWithoutSuffix;
                                            }
                                        }

                                        string databaseName = projectName;
                                        databaseName = Interaction.InputBox("Enter the Project Name:", "Project Name/DB Name", databaseName);

                                        if (databaseName.Contains("[") || databaseName.Contains("]") || databaseName == "")
                                        {
                                            MessageBox.Show("Database Name entered has errors!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                                            return;
                                        }

                                        // Choose where to save the log file
                                        LogFilePathObtain();
                                        if (logFilePath == "")
                                        {
                                            return;
                                        }

                                        // Create the log file
                                        string logFileName = logFilePath + "\\" + projectName + "_ExportLogFile_" + System.DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
                                        logFileWriter = new StreamWriter(logFileName, false);
                                        databaseName = databaseName + System.DateTime.Now.ToString("_yyyy_MM_dd");

                                        if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                        {
                                            waitingForm = new WaitingForm(DateTimePicker1.Value, this);
                                            waitingForm.ShowDialog();
                                        }
                                        if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                        {
                                            return;
                                        }

                                        CheckGroupDB(databaseName);
                                        logFileWriter.Close();
                                        DataGridView1.Enabled = true;
                                        this.databaseFileStoragePath = "";
                                        this.logFilePath = "";
                                    }
                                    else if (selec.SelectionResult == 0)
                                    {//Exist 插入已存在的数据库中/ Insertinto Local Database
                                        List<string> listdbname = ListDatbaseName(this.SQLservername,this.SQLdatabasename,this.SQLusername,this.SQLpassword);
                                        ExistingDB existingDB = new ExistingDB(listdbname);
                                            if (existingDB.ShowDialog()==DialogResult.OK) {//Choose some one
                                                this.databaseExist = true;
                                               // MessageBox.Show(existingDB.Selectedname);//Selected Database name
                                                string databaseName = existingDB.Selectedname;
                                               // databaseName = Interaction.InputBox("Enter the Project Name:", "Project Name/DB Name", databaseName);

                                                if (databaseName.Contains("[") || databaseName.Contains("]") || databaseName == "")
                                                {
                                                    MessageBox.Show("Database Name entered has errors!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                                                    return;
                                                }

                                                // Choose where to save the log file
                                                LogFilePathObtain();
                                                if (logFilePath == "")
                                                {
                                                    return;
                                                }

                                                // Create the log file
                                                string logFileName = logFilePath + "\\" + databaseName + "_ExportLogFile_" + System.DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
                                                logFileWriter = new StreamWriter(logFileName, false);
                                               // databaseName = databaseName + System.DateTime.Now.ToString("_yyyy_MM_dd");

                                                if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                                {
                                                    waitingForm = new WaitingForm(DateTimePicker1.Value, this);
                                                    waitingForm.ShowDialog();
                                                }
                                                if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                                {
                                                    return;
                                                }
                                                
                                                CheckGroupDB(databaseName);
                                                logFileWriter.Close();
                                                DataGridView1.Enabled = true;
                                                this.databaseFileStoragePath = "";
                                                this.logFilePath = "";

                                            }
                                    }
                                  }
                                }

                            }
                            catch
                            {
                                MessageBox.Show("Failed","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else {

                        }
                       // }
                    }
                    else if (predata.predecison.Equals("cloud"))
                    {
                        using (DatabaseSelection dbselection = new DatabaseSelection())
                        {
                            if (dbselection.ShowDialog() == DialogResult.OK)
                            {
                                if (dbselection.SelectionResult == 1)
                                {//CreateNew
                                    StoragePathObtain();
                                    if (databaseFileStoragePath == "")
                                    {
                                        return;
                                    }
                                    string projectName = CheckedListBoxOfDocuments.Items[0].ToString();
                                    for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
                                    {
                                        string pathString = CheckedListBoxOfDocuments.Items[i].ToString();
                                        string[] pathSpliter = pathString.Split('\\');
                                        string fullFileName = pathSpliter[pathSpliter.Length - 1];
                                        string fileNameWithoutSuffix = fullFileName.Substring(0, fullFileName.Length - 4);
                                        if (fileNameWithoutSuffix.Length < projectName.Length)
                                        {
                                            projectName = fileNameWithoutSuffix;
                                        }
                                    }

                                    string databaseName = projectName;
                                    databaseName = Interaction.InputBox("Enter the Project Name:", "Project Name/DB Name", databaseName);

                                    if (databaseName.Contains("[") || databaseName.Contains("]") || databaseName == "")
                                    {
                                        MessageBox.Show("Database Name entered has errors!","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    // Choose where to save the log file
                                    LogFilePathObtain();
                                    if (logFilePath == "")
                                    {
                                        return;
                                    }

                                    // Create the log file
                                    string logFileName = logFilePath + "\\" + projectName + "_ExportLogFile_" + System.DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
                                    logFileWriter = new StreamWriter(logFileName, false);
                                    databaseName = databaseName + System.DateTime.Now.ToString("_yyyy_MM_dd");

                                    if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                    {
                                        waitingForm = new WaitingForm(DateTimePicker1.Value, this);
                                        waitingForm.ShowDialog();
                                    }
                                    if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                    {
                                        return;
                                    }

                                    CheckGroupDB(databaseName);
                                    logFileWriter.Close();
                                    DataGridView1.Enabled = true;
                                    this.databaseFileStoragePath = "";
                                    this.logFilePath = "";
                                }
                                else if (dbselection.SelectionResult == 0)
                                {//Exist 插入已存在的数据库中/
                                    List<string> listdbname = ListDatbaseName(this.SQLservername, this.SQLdatabasename, this.SQLusername, this.SQLpassword);
                                    ExistingDB existingDB = new ExistingDB(listdbname);
                                    if (existingDB.ShowDialog() == DialogResult.OK)
                                    {//Choose some one
                                        this.databaseExist = true;
                                        // MessageBox.Show(existingDB.Selectedname);//Selected Database name
                                        string databaseName = existingDB.Selectedname;
                                        // databaseName = Interaction.InputBox("Enter the Project Name:", "Project Name/DB Name", databaseName);

                                        if (databaseName.Contains("[") || databaseName.Contains("]") || databaseName == "")
                                        {
                                            MessageBox.Show("Database Name entered has errors!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }

                                        // Choose where to save the log file
                                        LogFilePathObtain();
                                        if (logFilePath == "")
                                        {
                                            return;
                                        }

                                        // Create the log file
                                        string logFileName = logFilePath + "\\" + databaseName + "_ExportLogFile_" + System.DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
                                        logFileWriter = new StreamWriter(logFileName, false);
                                        // databaseName = databaseName + System.DateTime.Now.ToString("_yyyy_MM_dd");

                                        if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                        {
                                            waitingForm = new WaitingForm(DateTimePicker1.Value, this);
                                            waitingForm.ShowDialog();
                                        }
                                        if (DateTime.Compare(DateTimePicker1.Value, System.DateTime.Now) > 0)
                                        {
                                            return;
                                        }

                                        CheckGroupDB(databaseName);
                                        logFileWriter.Close();
                                        DataGridView1.Enabled = true;
                                        this.databaseFileStoragePath = "";
                                        this.logFilePath = "";

                                    }


                                }
                            }
                        }


                    }

                }
            }
        }




        //The method to select the storage place for log files
        public void LogFilePathObtain()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Choose the Storage Place for the LOG file";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(folderBrowser.SelectedPath))
                {
                    MessageBox.Show(this, "The folder path can not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string folderPath = folderBrowser.SelectedPath.ToString();
                this.logFilePath = folderPath;
            }
        }

        //Check the existance of the database when export a whole linked project
        public void CheckGroupDB(string databaseName)
        {
            logFileWriter.WriteLine("Check the existance of the database..." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();

            string fileName = databaseName.Substring(0, databaseName.Length - 11);

            string sqlDBNObtain = "select name from master..sysdatabases";
            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlDBNObtain, conn);
            List<string> dbNames = new List<string>();
            List<string> fileNames = new List<string>();
            List<string> possibleDBNames = new List<string>();
            ArrayList dbNameList = new ArrayList();
            DataTable dbNameTable = new DataTable();
            adapter.Fill(dbNameTable);
            

            foreach (DataRow row in dbNameTable.Rows)
            {
                dbNameList.Add(row["name"]);
            }

            foreach (var dbName in dbNameList)
            {
                dbNames.Add(dbName.ToString());
            }
            /*
            foreach (string dbName in dbNames)
            {
                if (dbName.Length > 11)
                {
                    string dbfileName = dbName.Substring(0, dbName.Length - 11);
                    fileNames.Add(dbfileName);
                }
            }
            */

            
            foreach (string dbName in dbNames)
            {
                if (dbName.Length > 11)
                {
                    string dbfileName = dbName.Substring(0, dbName.Length - 11);
                    possibleDBNames.Add(dbName);
                }
            }

            string[] modelNameSpliter = CheckedListBoxOfDocuments.CheckedItems[0].ToString().Split('\\');
            string modelName = modelNameSpliter[modelNameSpliter.Length - 1];
            string schemaName = modelName.Substring(0, modelName.Length - 4);

            bool schemaExist = false;

            foreach (string dbName in possibleDBNames)
            {
                List<string> schemasInDB = new List<string>();
                string dbNameWithoutSuffix = dbName.Substring(0, dbName.Length - 11);
                string useDBSql = "USE [" + dbName + "];";
                OdbcCommand useDBCommand = new OdbcCommand(useDBSql, conn);
                useDBCommand.ExecuteNonQuery();

                //Find all the schema names in the database
                string findSchemaSql = "SELECT DISTINCT TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES;";
                OdbcCommand findSchemaCommand = new OdbcCommand(findSchemaSql, conn);
                OdbcDataReader schemaReader = findSchemaCommand.ExecuteReader();
                while (schemaReader.Read())
                {
                    schemasInDB.Add(schemaReader[0].ToString().ToUpper());
                }
                if (schemasInDB.Contains(schemaName.ToUpper()))   //If the schema exists, performing the update function. Otherwise, pop up an error message.
                {
                    schemaExist = true;
                    if(dbName.Substring(0, dbName.Length - 11) != databaseName.Substring(0, databaseName.Length - 11))
                    {
                        MessageBox.Show("Project database already exists. But the project name you entered cannot match. Check the project name","Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    break;
                }
            }
            conn.Close();

            if (schemaExist && this.databaseExist.Equals(false))
            //if (fileNames.Contains(fileName))
            {
                GroupUpdateProcedure(fileName, dbNames, databaseName);
                logFileWriter.WriteLine("The update is successful." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            }
            else if (schemaExist.Equals(false) && this.databaseExist.Equals(false))
            {
                ConstructDatabases(databaseName);
                CreateSchema(databaseName);
                GroupExportProcedure(databaseName);
                logFileWriter.WriteLine("The export is successful." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            }
            else if (schemaExist.Equals(true) && this.databaseExist.Equals(true)) {
                GroupUpdateProcedure(fileName, dbNames, databaseName);
                logFileWriter.WriteLine("The update is successful." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            }
            else if (schemaExist.Equals(false) && this.databaseExist.Equals(true)) {
                CreateSchema(databaseName);
                GroupExportProcedure(databaseName);
                logFileWriter.WriteLine("The export is successful." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            }
            MessageBox.Show("Group Export/Update Successful!");

        }

        //The update method for a whole database
        public void GroupUpdateProcedure(string fileName, List<string> dbNames, string databaseName)
        {
            string oldDatabaseName = "";
            foreach (string dbName in dbNames)
            {
                if (dbName.Contains(fileName))
                {
                    oldDatabaseName = dbName;
                    break;
                }
            }
            logFileWriter.WriteLine("The database which related to the project exists...UPDATE" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string newDatabaseName = databaseName;

            for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
            {
                CheckedListBoxOfDocuments.SelectedItem = CheckedListBoxOfDocuments.Items[i];
                ConfirmButton.PerformClick();
                string[] pathSpliter = CheckedListBoxOfDocuments.Items[i].ToString().Split('\\');
                string schemaNameWithSuffix = pathSpliter[pathSpliter.Length - 1];
                string schemaName = schemaNameWithSuffix.Substring(0, schemaNameWithSuffix.Length - 4);
                GroupUpdateDatabase(newDatabaseName, oldDatabaseName, schemaName);
            }

            OdbcConnection conn = new OdbcConnection(DatabaseConnectionString());
            conn.Open();
            string sqlRename = "EXEC sp_renamedb [" + oldDatabaseName + "], [" + newDatabaseName + "]";
            OdbcCommand reNameCommand = new OdbcCommand(sqlRename, conn);
            reNameCommand.ExecuteNonQuery();
            logFileWriter.WriteLine("The database [" + oldDatabaseName + "] is renamed to [" + newDatabaseName + "]!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            conn.Close();
        }


        //Group Update the project, which invoke the GroupUpdateFunction.
        //The project name will not be changed and only the date will changed to the date the update is performed.
        public void GroupUpdateDatabase(string newDatabaseName, string oldDatabaseName, string schemaName)
        {
            Export_Progress exportForm = new Export_Progress();
            exportForm.Show();

            logFileWriter.WriteLine("The Update process starts!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string oldDate = oldDatabaseName.Substring(oldDatabaseName.Length - 10, 10);
            oldDate = oldDate.Replace("_", "-");
            string newDate = newDatabaseName.Substring(newDatabaseName.Length - 10, 10);
            newDate = newDate.Replace("_", "-");

            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();

            List<string> changedCategoryNames = new List<string>();
            Dictionary<string, List<string[]>> changedContainer = new Dictionary<string, List<string[]>>();



            for (int i = 0; i < ListBoxOfCategories.Items.Count; i++)
            {
                exportForm.maxValue = ListBoxOfCategories.Items.Count;
                List<string[]> changedValueContainer = new List<string[]>();

                GroupUpdateFunction(i, exportForm, oldDatabaseName, conn, newDate, changedValueContainer, changedCategoryNames, changedContainer, schemaName);
            }

            if (changedCategoryNames.Count > 0)
            {
                GenerateUpdateHtml(changedCategoryNames, changedContainer);
            }

            
            exportForm.Dispose();
            
            conn.Close();
        }


        //Update a specific schema.
        //This method is invoked in a group update situation.
        //The whole project is exported again.
        public void GroupUpdateFunction(int i, Export_Progress exportForm, string oldDatabaseName, OdbcConnection conn, string newDate, List<string[]> changedValueContainer, List<string> changedCategoryNames, Dictionary<string, List<string[]>> changedContainer, string schemaName)
        {
            try
            {              
                ListBoxOfCategories.SelectedItem = ListBoxOfCategories.Items[i];
                string categoryName = ListBoxOfCategories.Items[i].ToString();
                schemaName = schemaName.ToUpper();

                int newRowCount = DataGridView1.Rows.Count;

                int columnCount = DataGridView1.Columns.Count;

                if (columnCount == 1)
                {
                    exportForm.progressValue++;
                    exportForm.percentageLabelContent = ((int)(exportForm.progressValue * 100 / (exportForm.maxValue))).ToString() + "%";
                    return;
                }

                exportForm.labelContent = categoryName;

                int oldRowCount = 0;

                string useDBSql = "USE [" + oldDatabaseName + "];";
                OdbcCommand useDBCommand = new OdbcCommand(useDBSql, conn);
                useDBCommand.ExecuteNonQuery();

                string sqlCount = "SELECT COUNT(*) FROM [" + schemaName + "].[" + categoryName + "];";
                OdbcCommand rowCountCommand = new OdbcCommand(sqlCount, conn);
                OdbcDataReader numberReader = rowCountCommand.ExecuteReader();
                while (numberReader.Read())
                {
                    oldRowCount = Convert.ToInt32(numberReader[0].ToString());
                }

                int oldColumnCount = 0;
                //select count(*) from syscolumns s  where s.id = object_id('test');
                string sqloldColumnCount = "SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[COLUMNS] c WHERE c.TABLE_NAME=\'" + categoryName + "\' AND c.TABLE_SCHEMA=\'" + schemaName + "\';";
                OdbcCommand oldColumnCountCommand = new OdbcCommand(sqloldColumnCount, conn);
                OdbcDataReader oldColumnReader = oldColumnCountCommand.ExecuteReader();
                while (oldColumnReader.Read())
                {
                    oldColumnCount = Convert.ToInt32(oldColumnReader[0].ToString());
                }


                if (oldColumnCount != columnCount)
                {
                    string[] parameterNameContainer = new string[columnCount];
                    for (int b = 0; b < columnCount; b++)
                    {
                        parameterNameContainer[b] = DataGridView1.Columns[b].HeaderText;
                    }
                    Array.Sort(parameterNameContainer);

                    List<string> currentParameterNameContainer = new List<string>();
                    string oldColumnNamesSQL = "SELECT COLUMN_NAME FROM [INFORMATION_SCHEMA].[COLUMNS] c WHERE c.TABLE_NAME=\'" + categoryName + "\' AND c.TABLE_SCHEMA=\'" + schemaName + "\';";
                    OdbcCommand oldColumnNamesCommand = new OdbcCommand(oldColumnNamesSQL, conn);
                    OdbcDataReader oldColumnNamesReader = oldColumnNamesCommand.ExecuteReader();
                    while (oldColumnNamesReader.Read())
                    {
                        currentParameterNameContainer.Add(oldColumnNamesReader[0].ToString());
                    }

                    string partSQL3 = "";
                    foreach (string eachParameterName in parameterNameContainer)
                    {
                        if (!currentParameterNameContainer.Contains(eachParameterName))
                        {
                            partSQL3 += "[" + eachParameterName + "] Text, ";
                        }
                    }
                    partSQL3 = partSQL3.Substring(0, partSQL3.Length - 2);
                    string alterSql = "USE [" + oldDatabaseName + "]; ALTER TABLE [" + schemaName + "].[" + categoryName + "] ADD " + partSQL3 + ";";
                    OdbcCommand alterCommand = new OdbcCommand(alterSql, conn);
                    alterCommand.ExecuteNonQuery();

                    Dictionary<string, string[]> newValueContainer = new Dictionary<string, string[]>();
                    if (newRowCount > 0)
                    {
                        for (int r = 0; r < newRowCount; r++)
                        {
                            string[] newValueRowContainer = new string[columnCount];
                            for (int c = 0; c < columnCount; c++)
                            {
                                string newValue = "";
                                if ((DataGridView1.Rows[r].Cells[c].Value == null) || (DataGridView1.Rows[r].Cells[c].Value.ToString().Trim() == ""))
                                {
                                    newValue = "null";
                                }
                                else
                                {
                                    if (DataGridView1.Rows[r].Cells[c].Value.ToString().Contains("\x00B2"))
                                    {
                                        newValue = DataGridView1.Rows[r].Cells[c].Value.ToString().Replace("\x00B2", "^2");
                                    }
                                    else if (DataGridView1.Rows[r].Cells[c].Value.ToString().Contains("\x00B3"))
                                    {
                                        newValue = DataGridView1.Rows[r].Cells[c].Value.ToString().Replace("\x00B3", "^3");
                                    }
                                    else
                                    {
                                        newValue = DataGridView1.Rows[r].Cells[c].Value.ToString();
                                    }
                                }
                                newValueRowContainer[c] = newValue;
                            }
                            newValueContainer.Add(newValueRowContainer[0], newValueRowContainer);
                        }
                    }

                    string sqlObtainValue = "USE [" + oldDatabaseName + "]; SELECT * FROM [" + schemaName + "].[" + categoryName + "];";
                    OdbcCommand obtainTableCommand = new OdbcCommand(sqlObtainValue, conn);
                    OdbcDataReader tableReader = obtainTableCommand.ExecuteReader();

                    Dictionary<string, string[]> oldValueContainer = new Dictionary<string, string[]>();
                    if (oldRowCount != 0)
                    {
                        while (tableReader.Read())
                        {
                            string[] oldValueRowContainer = new string[columnCount];
                            for (int c = 0; c < columnCount; c++)
                            {
                                if (Convert.IsDBNull(tableReader[c]))
                                {
                                    oldValueRowContainer[c] = "null";
                                }
                                else
                                {
                                    oldValueRowContainer[c] = tableReader[c].ToString();
                                }
                            }
                            oldValueContainer.Add(oldValueRowContainer[0], oldValueRowContainer);
                        }
                    }

                    foreach (KeyValuePair<string, string[]> kv in newValueContainer)
                    {
                        if (oldValueContainer.Keys.Contains(kv.Key))
                        {
                            continue;
                        }

                        string valueSql = "";

                        for (int c = 0; c < columnCount; c++)
                        {
                            if (kv.Value[c] != "null")
                            {
                                if (kv.Value[c].Contains("'"))
                                {
                                    kv.Value[c] = kv.Value[c].Replace("\'", "\'\'");
                                    valueSql += "\'" + kv.Value[c] + "\', ";
                                }
                                else
                                {

                                    valueSql += "\'" + kv.Value[c] + "\', ";
                                }

                            }
                            else
                            {
                                valueSql += kv.Value[c] + ", ";
                            }
                        }
                        string elementId = kv.Key;
                        valueSql = valueSql.Substring(0, valueSql.Length - 2);
                        string insertSql = "USE [" + oldDatabaseName + "]; INSERT INTO [" + schemaName + "].[" + categoryName + "] VALUES(" + valueSql + ");";
                        OdbcCommand insertCommand = new OdbcCommand(insertSql, conn);
                        insertCommand.ExecuteNonQuery();
                        logFileWriter.WriteLine("Element " + kv.Value[0] + " is inserted into" + categoryName + "!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));


                        string[] tempString = { elementId, "Insert", categoryName, "null", "null", "null", newDate };
                        changedValueContainer.Add(tempString);
                        if (!changedCategoryNames.Contains(categoryName))
                        {
                            changedCategoryNames.Add(categoryName);
                        }
                    }

                }
                else if (oldColumnCount == columnCount)
                {
                    string sqlObtainValue = "USE [" + oldDatabaseName + "]; SELECT * FROM [" + schemaName + "].[" + categoryName + "];";
                    OdbcCommand obtainTableCommand = new OdbcCommand(sqlObtainValue, conn);
                    OdbcDataReader tableReader = obtainTableCommand.ExecuteReader();


                    Dictionary<string, string[]> newValueContainer = new Dictionary<string, string[]>();
                    if (newRowCount > 0)
                    {
                        for (int r = 0; r < newRowCount; r++)
                        {
                            string[] newValueRowContainer = new string[columnCount];
                            for (int c = 0; c < columnCount; c++)
                            {
                                string newValue = "";
                                if ((DataGridView1.Rows[r].Cells[c].Value == null) || (DataGridView1.Rows[r].Cells[c].Value.ToString().Trim() == ""))
                                {
                                    newValue = "null";
                                }
                                else
                                {
                                    if (DataGridView1.Rows[r].Cells[c].Value.ToString().Contains("\x00B2"))
                                    {
                                        newValue = DataGridView1.Rows[r].Cells[c].Value.ToString().Replace("\x00B2", "^2");
                                    }
                                    else if (DataGridView1.Rows[r].Cells[c].Value.ToString().Contains("\x00B3"))
                                    {
                                        newValue = DataGridView1.Rows[r].Cells[c].Value.ToString().Replace("\x00B3", "^3");
                                    }
                                    else
                                    {
                                        newValue = DataGridView1.Rows[r].Cells[c].Value.ToString();
                                    }
                                }
                                newValueRowContainer[c] = newValue;
                            }
                            newValueContainer.Add(newValueRowContainer[0], newValueRowContainer);
                        }
                    }


                    Dictionary<string, string[]> oldValueContainer = new Dictionary<string, string[]>();
                    if (oldRowCount != 0)
                    {
                        while (tableReader.Read())
                        {
                            string[] oldValueRowContainer = new string[columnCount];
                            for (int c = 0; c < columnCount; c++)
                            {
                                if (Convert.IsDBNull(tableReader[c]))
                                {
                                    oldValueRowContainer[c] = "null";
                                }
                                else
                                {
                                    oldValueRowContainer[c] = tableReader[c].ToString();
                                }
                            }
                            oldValueContainer.Add(oldValueRowContainer[0], oldValueRowContainer);
                        }
                    }



                    foreach (KeyValuePair<string, string[]> kv in newValueContainer)
                    {
                        if (!oldValueContainer.Keys.Contains(kv.Key))
                        {

                            string valueSql = "";

                            for (int c = 0; c < columnCount; c++)
                            {
                                if (kv.Value[c] != "null")
                                {
                                    if (kv.Value[c].Contains("'"))
                                    {
                                        kv.Value[c] = kv.Value[c].Replace("\'", "\'\'");
                                        valueSql += "\'" + kv.Value[c] + "\', ";
                                    }
                                    else
                                    {

                                        valueSql += "\'" + kv.Value[c] + "\', ";
                                    }

                                }
                                else
                                {
                                    valueSql += kv.Value[c] + ", ";
                                }
                            }
                            string elementId = kv.Key;
                            valueSql = valueSql.Substring(0, valueSql.Length - 2);
                            string insertSql = "USE [" + oldDatabaseName + "]; INSERT INTO [" + schemaName + "].[" + categoryName + "] VALUES(" + valueSql + ");";
                            OdbcCommand insertCommand = new OdbcCommand(insertSql, conn);
                            insertCommand.ExecuteNonQuery();
                            logFileWriter.WriteLine("Element " + kv.Value[0] + " is inserted into" + categoryName + "!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));

                            string[] tempString = { elementId, "Insert", categoryName, "null", "null", "null", newDate };
                            changedValueContainer.Add(tempString);
                            if (!changedCategoryNames.Contains(categoryName))
                            {
                                changedCategoryNames.Add(categoryName);
                            }

                        }
                        else if (oldValueContainer.Keys.Contains(kv.Key))
                        {
                            foreach (KeyValuePair<string, string[]> okv in oldValueContainer)
                            {
                                if (okv.Key == kv.Key)
                                {
                                    for (int c = 0; c < columnCount; c++)
                                    {
                                        if (okv.Value[c] != kv.Value[c])
                                        {
                                            string elementId = okv.Key;
                                            string newValueRecord = "NULL";
                                            string parameterName = DataGridView1.Columns[c].HeaderText;
                                            if (kv.Value[c] != "null")
                                            {
                                                kv.Value[c] = "\'" + kv.Value[c] + "\'";
                                                newValueRecord = kv.Value[c];
                                            }
                                            string updateSql = "USE [" + oldDatabaseName + "]; UPDATE [" + schemaName + "].[" + categoryName + "] SET \"" + parameterName + "\"=" + kv.Value[c] + " WHERE ElementId=\'" + elementId + "\';";
                                            OdbcCommand updateCommand = new OdbcCommand(updateSql, conn);
                                            updateCommand.ExecuteNonQuery();
                                            logFileWriter.WriteLine("Element " + kv.Value[0] + " is updated from " + okv.Value[c] + " to " + newValueRecord + "!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));

                                            string[] tempString = { elementId, "Update", categoryName, parameterName, okv.Value[c], newValueRecord, newDate };
                                            changedValueContainer.Add(tempString);
                                            if (!changedCategoryNames.Contains(categoryName))
                                            {
                                                changedCategoryNames.Add(categoryName);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (oldValueContainer.Keys.Count > newValueContainer.Keys.Count)
                    {
                        foreach (KeyValuePair<string, string[]> kv in oldValueContainer)
                        {
                            if (!newValueContainer.Keys.Contains(kv.Key))
                            {
                                string elementId = kv.Key;
                                string deleteSql = "USE [" + oldDatabaseName + "]; DELETE FROM [" + schemaName + "].[" + categoryName + "] WHERE ElementId =" + kv.Key + ";";
                                OdbcCommand deleteCommand = new OdbcCommand(deleteSql, conn);
                                deleteCommand.ExecuteNonQuery();
                                logFileWriter.WriteLine("Element " + kv.Value[0] + " is deleted!" + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));

                                string[] tempString = { elementId, "Delete", categoryName, "null", "null", "null", newDate };
                                changedValueContainer.Add(tempString);
                                if (!changedCategoryNames.Contains(categoryName))
                                {
                                    changedCategoryNames.Add(categoryName);
                                }
                            }
                        }
                    }
                }
                if (changedValueContainer.Count > 0)
                {
                    changedContainer.Add(categoryName, changedValueContainer);
                }
            }
            catch(Exception e)
            {
                logFileWriter.WriteLine(e);
            }
            

            exportForm.progressValue++;
            string percentageContent = ((int)(exportForm.progressValue * 100 / (exportForm.maxValue))).ToString() + "%";
            exportForm.percentageLabelContent = percentageContent;
        }



        //The schema creation method.
        //The schema is created with the name of the model file.
        //It should be noticed that the SQL language about schema is active only after the execution of the use of a database.
        public void CreateSchema(string databaseName)
        {
            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();
            List<string> modelNamesList = new List<string>();
            for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
            {
                string filePath = CheckedListBoxOfDocuments.Items[i].ToString();
                string[] pathSpliter = filePath.Split('\\');
                string fullFileName = pathSpliter[pathSpliter.Length - 1];
                string fileName = fullFileName.Substring(0, fullFileName.Length - 4);
                modelNamesList.Add(fileName);
            }
            foreach (string modelName in modelNamesList)
            {
                string useDatabaseSql = "USE [" + databaseName + "];";
                OdbcCommand useDatabaseCommand = new OdbcCommand(useDatabaseSql, conn);
                useDatabaseCommand.ExecuteNonQuery();

                string createSchemaSql = "CREATE SCHEMA [" + modelName + "];";
                OdbcCommand createSchemaCommand = new OdbcCommand(createSchemaSql, conn);
                createSchemaCommand.ExecuteNonQuery();
            }
            conn.Close();
        }


        //Automatically start export process of a project including several model files.
        //All the model files in the CheckedListBoxOfDocuments will be traversed.
        public void GroupExportProcedure(string databaseName)
        {
            for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
            {
                CheckedListBoxOfDocuments.SelectedItem = CheckedListBoxOfDocuments.Items[i];
                string[] pathSpliter = CheckedListBoxOfDocuments.SelectedItem.ToString().Split('\\');
                string fullFileName = pathSpliter[pathSpliter.Length - 1];
                string fileNameWithoutSuffix = fullFileName.Substring(0, fullFileName.Length - 4);
                string modelName = fileNameWithoutSuffix;
                ConfirmButton.PerformClick();
                GroupSendTables(databaseName, modelName);
            }
        }




        //Traversing each Category(Table) in the listbox and invoke the GroupSendFunction() for each Category.
        public void GroupSendTables(string databaseName, string modelName)
        {
            //This is just a test for github commit
            int tableNumber = ListBoxOfCategories.Items.Count;
            DataGridView1.Enabled = false;
            Export_Progress exportForm = new Export_Progress();
            exportForm.Show();
            exportForm.maxValue = tableNumber;

            string connString = DatabaseConnectionString();
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();

            for (int a = 0; a < tableNumber; a++)
            {
                GroupSendFunction(exportForm, a, databaseName, modelName, conn, tableNumber);
            }
            exportForm.Dispose();
            conn.Close();
        }

        //Send the table to database system.
        //The content of the table is directly obtained from the DataGridView1.
        //First, the table is created.
        //Then, adding read-only information to Contraints.
        //After that, clearing the cell value and insert lines to the table.
        public void GroupSendFunction(Export_Progress exportProgressForm, int a, string databaseName, string fileName, OdbcConnection odbcConnection, int tableNumber)
        {
            ListBoxOfCategories.SelectedItem = ListBoxOfCategories.Items[a];
            string tableName = ListBoxOfCategories.Items[a].ToString();
            exportProgressForm.labelContent = tableName;
            int count = DataGridView1.Columns.Count;
            string[] columnNames = new string[count];

            if(count == 1)
            {
                exportProgressForm.progressValue++;
                exportProgressForm.percentageLabelContent = ((int)(exportProgressForm.progressValue * 100 / (tableNumber + 1))).ToString() + "%";
                return;
            }

            for (int i = 0; i < count; i++)
            {
                columnNames[i] = DataGridView1.Columns[i].HeaderText;//.Replace(" ", "");
            }


            string partOfSQL = "";
            partOfSQL = "\"" + columnNames[0] + "\" int primary key, ";
            if (count > 1)
            {
                for (int j = 1; j < count; j++)
                {
                    partOfSQL += "\"" + columnNames[j] + "\"" + " Text, ";
                }
            }
            partOfSQL = partOfSQL.Substring(0, partOfSQL.Length - 2);
            string sqlStr = "USE [" + databaseName + "]; CREATE TABLE [" + fileName + "].[" + tableName + "](" + partOfSQL + @");";
            OdbcCommand attributeCommand = new OdbcCommand(sqlStr, odbcConnection);
            attributeCommand.ExecuteNonQuery();
            logFileWriter.WriteLine("Table [" + fileName + "].[" + tableName + "] is created..." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));

            foreach (string categoryParameterPair in read_only_Set)
            {
                string[] list = categoryParameterPair.Split('_');
                string categoryName = list[1];
                if (tableName == categoryName)
                {
                    string readOnlySql = "USE [" + databaseName + "]; ALTER TABLE [" + fileName + "].[" + tableName + "] ADD CONSTRAINT [" + categoryParameterPair + "] CHECK (" + columnNames[0] + "=" + columnNames[0] + ");";
                    OdbcCommand readOnlyCommand = new OdbcCommand(readOnlySql, odbcConnection);
                    readOnlyCommand.ExecuteNonQuery();
                }
            }


            int totalRowsCount = DataGridView1.Rows.Count;
            int rowLabel = 0;

            for (int rowNumber = 0; rowNumber < DataGridView1.Rows.Count; rowNumber++)
            {
                rowLabel++;
                string[] valueContainer = new string[count];
                for (int columnNumber = 0; columnNumber < count; columnNumber++)
                {
                    if ((DataGridView1.Rows[rowNumber].Cells[columnNumber].Value == null) || (DataGridView1.Rows[rowNumber].Cells[columnNumber].Value.ToString().Trim() == ""))
                    {
                        valueContainer[columnNumber] = "null";
                    }
                    else if ((DataGridView1.Rows[rowNumber].Cells[columnNumber].Value != null))
                    {
                        valueContainer[columnNumber] = DataGridView1.Rows[rowNumber].Cells[columnNumber].Value.ToString();
                    }
                }
                string partOfSQL2 = "";

                if (valueContainer[0] != "null")
                {
                    for (int x = 0; x < count; x++)
                    {
                        if (valueContainer[x] != "null")
                        {
                            if (valueContainer[x].Contains("'"))
                            {
                                valueContainer[x] = valueContainer[x].Replace("\'", "\'\'");
                                partOfSQL2 += "\'" + valueContainer[x] + "\', ";
                            }
                            else if (valueContainer[x].Contains("\x00B2"))
                            {
                                valueContainer[x] = valueContainer[x].Replace("\x00B2", "^2");
                                partOfSQL2 += "\'" + valueContainer[x] + "\', ";
                            }
                            else if (valueContainer[x].Contains("\x00B3"))
                            {
                                valueContainer[x] = valueContainer[x].Replace("\x00B3", "^3");
                                partOfSQL2 += "\'" + valueContainer[x] + "\', ";
                            }
                            else
                            {
                                partOfSQL2 += "\'" + valueContainer[x] + "\', ";
                            }

                        }
                        else
                        {
                            partOfSQL2 += valueContainer[x] + ", ";
                        }
                    }
                    partOfSQL2 = partOfSQL2.Substring(0, partOfSQL2.Length - 2);
                    string valueSqlStr = "USE [" + databaseName + "]; INSERT INTO [" + fileName + "].[" + ListBoxOfCategories.SelectedItem.ToString() + "] VALUES(" + partOfSQL2 + ");";
                    logFileWriter.WriteLine("Element " + valueContainer[0] + " is inserted..." + " ------ " + System.DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
                    exportProgressForm.labelContent = tableName + " " + rowLabel.ToString() + "/" + totalRowsCount;
                    OdbcCommand valueCommand = new OdbcCommand(valueSqlStr, odbcConnection);
                    try
                    {
                        valueCommand.ExecuteNonQuery();
                    }
                    catch (System.Data.Odbc.OdbcException)
                    {
                        MessageBox.Show(valueSqlStr);
                    }
                }

            }
            exportProgressForm.progressValue++;
            string percentageLabelContent = ((int)(exportProgressForm.progressValue * 100 / (tableNumber + 1))).ToString() + "%";
            exportProgressForm.percentageLabelContent = percentageLabelContent;
        }

        //Browsing and choosing desired revit files
        private void BrowsingButton_Click(object sender, EventArgs e)
        {
            databaseFileStoragePath = "";
            ExportButton.Enabled = false;
            CompleteProjectControlCheckBox.Enabled = false;
            ListBoxOfCategories.Items.Clear();
            DataGridView1.Columns.Clear();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] filePaths = fileDialog.FileNames;
                foreach (string filePath in filePaths)
                {
                    CheckedListBoxOfDocuments.Items.Add(filePath);
                }
                int count = CheckedListBoxOfDocuments.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    CheckedListBoxOfDocuments.SetItemChecked(i, true);
                }
                ExportButton.Enabled = true;
                CompleteProjectControlCheckBox.Enabled = true;
            }

        }

        //This method is used to pop up a window for user to select the storage place for the recording file.
        public void OperationRecordingPathObtain()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Choose the Storage Place for the Operation Recording File";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(folderBrowser.SelectedPath))
                {
                    MessageBox.Show(this, "The folder path can not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string folderPath = folderBrowser.SelectedPath.ToString();
                this.operationRecordingPath = folderPath;
            }
        }

        //When the export button is clicked, the method will select corresponding export mode according to the radio button.
        //It will check whether necessary information is available before invoke the export methods.
        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (ModelRadioButton.Checked == true)
            {
                if(CheckedListBoxOfDocuments.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Please tick the file required to be exported!","Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SelectedFilesExport();
            }
            else if (CategoryRadioButton.Checked == true)
            {
                if(ListBoxOfCategories.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select the tables required to be exported", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SelectedCategoryExport();
            }
        }

        //When the checked radio button is changed, the selection mode of the listbox of categories will be set on demand.
        private void CategoryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CategoryRadioButton.Checked == false)
            {
                ListBoxOfCategories.SelectionMode = SelectionMode.One;
                DataGridView1.Enabled = true;
            }
            else if (CategoryRadioButton.Checked == true)
            {
                ListBoxOfCategories.SelectionMode = SelectionMode.MultiSimple;
                //DataGridView1.Enabled = false;
            }
        }

        //This method is used to collect export recording information from the selected file when the load button is press.
        //This method will first read the selected "TXT" file and check whether it is a recording file through the first line in the "TXT" file.
        //Then, it will obtain all the information based on the title of each line and put them into independent containers.
        //Finally, the ActivateRecording() is invoked to activate the information obtained.
        private void OperationLoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "Please select the recording file.";
            dialog.Filter = "TXT FILE|*.txt";
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string recordFilePath = dialog.FileName;
                StreamReader reader = new StreamReader(recordFilePath);
                if(reader.ReadLine() == "Revit Plugin Export Record")
                {
                    string time = "";
                    List<string> modelPaths = new List<string>();
                    string mode = "";
                    List<string> categories = new List<string>();
                    string exportMethod = "";
                    string line = reader.ReadLine();
                    while(line != null)
                    {
                        if(line.Substring(0, 6) == "Time: ")
                        {
                            time = line.Substring(6, line.Length - 6);
                        }
                        else if(line.Substring(0, 7) == "Model: ")
                        {
                            modelPaths.Add(line.Substring(7, line.Length - 7));
                        }
                        else if(line.Substring(0, 6) == "Mode: ")
                        {
                            mode = line.Substring(6, line.Length - 6);
                        }
                        else if(line.Substring(0, 10) == "Category: ")
                        {
                            categories.Add(line.Substring(10, line.Length - 10));
                        }
                        else if (line.Substring(0, 15) == "Export Method: ")
                        {
                            exportMethod = line.Substring(15, line.Length - 15);
                        }
                        line = reader.ReadLine();
                    }
                    reader.Close();
                    ActivateRecording(time, modelPaths, mode, categories, exportMethod);
                }
                else
                {
                    MessageBox.Show("Wrong file selected!!!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                return;
            }
        }

        //This method is to activate the recording on the current interface. However, the rights to start export is assigned to the user. Even some modifications are allowed.
        //This method will be invoked by OperationLoadButton_Click();
        //First, the recording information obtained is sorted out.
        //Second, checking the time. If the time is before the current time, the day will be increased by 1.
        //Third, entering the information to corresponding places. And the situation of the interface is ready to export.
        //Waiting the user to press the corresponding export button to confirm the export.
        private void ActivateRecording(string time, List<string> modelPaths, string mode, List<string> categories, string exportMethod)
        {
            string[] timeSpliter = time.Split(':');
            int hour = int.Parse(timeSpliter[0].Trim());
            int minute = int.Parse(timeSpliter[1].Trim());
            int second = int.Parse(timeSpliter[2].Trim());

            ResetButton.PerformClick();
            int day = DateTimePicker1.Value.Day;

            DateTime setTime = new DateTime(DateTimePicker1.Value.Year,
                                                    DateTimePicker1.Value.Month,
                                                    day,
                                                    hour,
                                                    minute,
                                                    second);

            if(DateTime.Compare(setTime, DateTime.Now) < 0)
            {
                day = DateTimePicker1.Value.Day + 1;
            }
            
            DateTimePicker1.Value = new DateTime(DateTimePicker1.Value.Year,
                                                    DateTimePicker1.Value.Month,
                                                    day,
                                                    hour,
                                                    minute,
                                                    second);

            foreach(string modelPath in modelPaths)
            {
                CheckedListBoxOfDocuments.Items.Add(modelPath);
            }
            if(exportMethod == "GroupExport")
            {
                GroupExportButton.Enabled = true;
                for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
                {
                    CheckedListBoxOfDocuments.SetItemChecked(i, true);
                }
                //GroupExportButton.PerformClick();
            }
            else if(exportMethod == "UpdateExport")
            {
                if(mode == "Completed Models")
                {
                    ModelRadioButton.Checked = true;
                    ExportButton.Enabled = true;
                    for (int i = 0; i < CheckedListBoxOfDocuments.Items.Count; i++)
                    {
                        CheckedListBoxOfDocuments.SetItemChecked(i, true);
                    }
                    //ExportButton.PerformClick();
                }
                else if(mode == "Independent Categories")
                {                   
                    ConfirmButton.Enabled = true;
                    CheckedListBoxOfDocuments.SelectedItem = CheckedListBoxOfDocuments.Items[0];
                    ConfirmButton.PerformClick();
                    CategoryRadioButton.Checked = true;
                    for (int i = 0; i < ListBoxOfCategories.Items.Count; i++) 
                    {
                        if (categories.Contains(ListBoxOfCategories.Items[i]))
                        {
                            ListBoxOfCategories.SelectedItems.Add(ListBoxOfCategories.Items[i]);
                        }
                    }
                    ExportButton.Enabled = true;
                    //ExportButton.PerformClick();
                }
            }
        }

        //This method will generate a file recording the required information of an export process. Through loading the file, it is possible to repeat the action.
        //First, Checking the export mode. "Completed Models" or "Independent Categories".
        //Second, Checking whether all the necessary information is available.
        //Third, Recording the information based on the export mode.
        //Finally, Popping up the successful MessageBox.
        private void OperationRecordingButton_Click(object sender, EventArgs e)
        {
            if(ModelRadioButton.Checked == true)
            {
                if(CheckedListBoxOfDocuments.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Please Check the model files required to be exported!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if(CheckedListBoxOfDocuments.CheckedItems.Count > 0)
                {
                    OperationRecordingPathObtain();
                    if (operationRecordingPath == "")
                    {
                        return;
                    }
                    string recordingFilePath = operationRecordingPath + @"\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_Recording.txt";
                    operationRecorder = new StreamWriter(recordingFilePath, false);
                    operationRecorder.WriteLine("Revit Plugin Export Record");
                    operationRecorder.WriteLine("Time: " + DateTimePicker1.Value.ToString("HH:mm:ss"));

                    List<string> checkedModels = new List<string>();
                    for (int i = 0; i < CheckedListBoxOfDocuments.CheckedItems.Count; i++)
                    {
                        checkedModels.Add(CheckedListBoxOfDocuments.CheckedItems[i].ToString());
                    }

                    foreach (string checkedModel in checkedModels)
                    {
                        operationRecorder.WriteLine("Model: " + checkedModel);
                    }

                    operationRecorder.WriteLine("Mode: Completed Models");

                    if (GroupExportButton.Enabled)
                    {
                        operationRecorder.WriteLine("Export Method: GroupExport");
                    }
                    else
                    {
                        operationRecorder.WriteLine("Export Method: UpdateExport");
                    }
                    operationRecorder.Close();
                    MessageBox.Show("The processes have been successfully recorded!");
                }
            }
            else if(CategoryRadioButton.Checked == true)
            {
                if(ListBoxOfCategories.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please Select the categories required to be exported!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if(ListBoxOfCategories.SelectedItems.Count > 0)
                {
                    OperationRecordingPathObtain();
                    if (operationRecordingPath == "")
                    {
                        return;
                    }
                    string recordingFilePath = operationRecordingPath + @"\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_Recording.txt";
                    operationRecorder = new StreamWriter(recordingFilePath, false);
                    operationRecorder.WriteLine("Revit Plugin Export Record");
                    operationRecorder.WriteLine("Time: " + DateTimePicker1.Value.ToString("HH:mm:ss"));


                    string selectedModelPath = CheckedListBoxOfDocuments.SelectedItem.ToString();
                    operationRecorder.WriteLine("Model: " + selectedModelPath);

                    operationRecorder.WriteLine("Mode: Independent Categories");

                    foreach(var item in ListBoxOfCategories.SelectedItems)
                    {
                        string categoryName = item.ToString();
                        operationRecorder.WriteLine("Category: " + categoryName);
                    }

                    operationRecorder.WriteLine("Export Method: UpdateExport");
                    operationRecorder.Close();
                    MessageBox.Show("The processes have been successfully recorded!");
                }
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ElementNamesDisplayButton_Click(object sender, EventArgs e)
        {
            List<string> elementNames = new List<string>();
            foreach(Element elem in elementSet)
            {
                elementNames.Add(elem.Id.ToString() + "   " + elem.Name + "\n");
            }

            Form1 form = new Form1(elementNames);
            form.Show();
        }

        private void CompleteProjectControlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(CompleteProjectControlCheckBox.Checked == true)
            {
                GroupExportButton.Enabled = true;
            }
            else if(CompleteProjectControlCheckBox.Checked == false)
            {
                GroupExportButton.Enabled = false;
            }
        }
    }
}
