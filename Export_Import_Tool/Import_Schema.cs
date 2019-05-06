using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using System.Configuration;
using System.IO;

namespace Execute_Forms
{
    public partial class Import_Schema : System.Windows.Forms.Form
    {
        List<string> modelSrcs = new List<string>();//store all path of the model.
        OdbcConnection odbcConnection;
        //string connectDatabase = "Driver={Sql Server};Server=WINDOWS-0VT4EA0;Database=master;";
        string connectDatabase = @"Driver={Sql Server};Server=192.168.1.110;Database=master;uid=sa; pwd=Asbuiltdatabase123;";
        List<string> databaseName=new List<string>();
        List<string> existRvt = new List<string>();
        UIApplication uiApp;
        ExternalCommandData CommandData;
        Document doc;
        public Import_Schema(ExternalCommandData commandData)
        {
            InitializeComponent();
            CommandData = commandData;
            elementInfo.AllowUserToAddRows=false;
        }
        private void SetDoc(Document document)
        {
            this.doc = document;
        }
        private void odbcDatabase()//change the name of database;
        {
            odbcConnection = new OdbcConnection(connectDatabase);
        }

        private void addModel_Click(object sender, EventArgs e)
        {
            this.modelbox.HorizontalScrollbar = true;
            this.modelbox.ScrollAlwaysVisible = true;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(ofd.SafeFileName))
                {
                    MessageBox.Show("Please select a file");
                    return;
                }
            }
            foreach (string fileN in ofd.FileNames)
            {
                string checkRvt = fileN.Substring(fileN.Length - 4, 4);

                if (checkRvt.Equals(".rvt") || checkRvt.Equals(".rfa"))
                {

                    modelbox.Items.Add(fileN);
                }
                else
                {
                    string[] typeA = fileN.Split('\\');
                    string typeB = typeA[typeA.Length - 1];
                    MessageBox.Show(typeB + " is not a rvt document");
                }

            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            modelbox.Items.Clear();
        }

        private void tableBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void modelselect_Click(object sender, EventArgs e)
        {
            if (modelbox.Items.Count > 0) {
                if (modelbox.Items.Count == modelbox.CheckedItems.Count)
                {
                    for (int i = 0; i < modelbox.Items.Count; i++)
                    {
                        modelbox.SetItemChecked(i, false);
                    }

                }
                else {
                    for (int i = 0; i < modelbox.Items.Count; i++)
                    {
                        modelbox.SetItemChecked(i, true);
                    }

                }
               
            }

        }

        public string getRvtName(string path){
            string[] splitSrc = path.Split('\\');
            string rvtName = splitSrc[splitSrc.Length - 1];
            return rvtName;
        }

        public string getSrcPath(string path) {
            string[] splitSrc = path.Split('\\');
            string rvtName = splitSrc[splitSrc.Length - 1];
            string source = path.Substring(0, path.Length - rvtName.Length);
            return source;
        }


        private void search_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            existRvt.Clear();
            databaseName.Clear();
            modelSrcs.Clear();
            /*      TreeNode root = new TreeNode("gen");
                  root.Text = "1";
                  treeView1.Nodes.Add(root);
                  TreeNode root1 = new TreeNode("leaves");
                  TreeNode root2 = new TreeNode("leaves");
                  TreeNode root3 = new TreeNode("leaves");
                  root1.Text = "2";
                  root1.Tag = "3";
                  root2.Text = "2";
                  root3.Text = "2";
                  root.Nodes.Add(root1);
                  root.Nodes.Add(root2);
                  root.Nodes.Add(root3);*/
            List<string> clearRoot = new List<string>();
            List<string> unique = new List<string>();
                  if (modelbox.CheckedItems.Count>0) {
                      for (int i=0;i<modelbox.CheckedItems.Count;i++) {
                          modelSrcs.Add(modelbox.CheckedItems[i].ToString());
                      }
                      List<string> Container = new List<string>();
                      foreach(string src in modelSrcs) {
                          //obtain the rvtname and source which the users select
                          string rvtName = getRvtName(src);
                          string rvtNameMinus = rvtName.Substring(0, rvtName.Length - 4).ToUpper();
                          string sourcepath = getSrcPath(src);
                         // MessageBox.Show("rvtName:" +rvtName+",Source:"+sourcepath+",rvtName_rvt:"+rvtNameMinus);
                          //connect to the database
                          odbcDatabase();
                          odbcConnection.Open();
                          string search_databasename = "select * from sysdatabases;";
                          OdbcCommand odbc = new OdbcCommand(search_databasename,odbcConnection);
                          OdbcDataReader odbcDataReader = odbc.ExecuteReader();
                          while (odbcDataReader.Read()) {
                              databaseName.Add(odbcDataReader[0].ToString());//acquire all of the name of database
                          }
                          odbcDataReader.Close();

                          //show the databasename and rvt file name.
                          if (databaseName.Count>0) {
                              foreach (string dN in databaseName)
                              {
                                  string search_DBRvtName = "use [" + dN + "] select distinct TABLE_SCHEMA from INFORMATION_SCHEMA.TABLES;";
                                  OdbcCommand odbc_searchRvtName = new OdbcCommand(search_DBRvtName,odbcConnection);
                                  OdbcDataReader odbc_dataRead = odbc_searchRvtName.ExecuteReader();
                                  while (odbc_dataRead.Read()) {
                                     if (odbc_dataRead[0].ToString().Equals(rvtNameMinus) && (!Container.Contains(rvtNameMinus)))
                                               {
                                    string rvt = dN + '\\' + odbc_dataRead[0].ToString();
                                    existRvt.Add(rvt);//拥有数据库名和databasename
                                    TreeNode rootDatabase = new TreeNode(rvtNameMinus);
                                    treeView1.Nodes.Add(rootDatabase);//加入数据库名
                                    Container.Add(rvtNameMinus);
                                    //MessageBox.Show(dN + '\\' + odbc_dataRead[0].ToString());
                                               }
                                  }
                                  odbc_dataRead.Close();
                              }
                          }

                          //search all of the tables 
                          if (existRvt.Count>0)
                          {
                              List<TreeNode> rootNode = new List<TreeNode>();
                              foreach (string rvt in existRvt)
                              {
                                  string[] rvtDB = rvt.Split('\\');
                                  if (!unique.Contains(rvtDB[1])) {
                                      unique.Add(rvtDB[1]);
                                      string search_TableName = "use [" + rvtDB[0] + "] select distinct TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='" + rvtDB[1] + "';";
                                      OdbcCommand odbc_searchTable = new OdbcCommand(search_TableName, odbcConnection);
                                      OdbcDataReader readTableName = odbc_searchTable.ExecuteReader();
                                      List<string> sorttableName = new List<string>();
                               
                                    clearRoot.Add(rvtDB[1]);
                                    TreeNode root = new TreeNode();
                                    root.Text = rvtDB[1];
                                    treeView1.Nodes.Add(root);
                                    if (readTableName.Read())
                                    {
                                        // this.treeView1.Scrollable = true;
                                        // tableBox.Items.Add(rvtDB[0]);
                                        while (readTableName.Read())
                                        {
                                            sorttableName.Add(readTableName[0].ToString());

                                        }
                                        sorttableName.Sort();
                                        foreach (string tN in sorttableName)
                                        {
                                            TreeNode cnode = new TreeNode();
                                            cnode.Text = tN;
                                            root.Nodes.Add(cnode);
                                            //treeDB.Add(2, tN+"\\"+rvtDB[1]);//tableName+rvtfileName

                                        }
                                    }
                                   }        
                                  // treeDB.Add(1, rvtDB[1]+"\\"+"Root");
                              }
                          }
                    odbcConnection.Close();
                      }
                      //remove the duplicate root Text.
                    for (int i=0;i<treeView1.Nodes.Count;i++) {
                        if (treeView1.Nodes[i].Nodes.Count==0) {
                        treeView1.Nodes.Remove(treeView1.Nodes[i]);
                        }
                    }
                //Alter the not exist revit file.

            }
        }

    /*    public class Orag
        {
            public int id { get; set; }
            public int pid { get; set; }
            public string name { get; set; }

            public Orag(int id, int pid, string name)
            {
                this.id = id;
                this.pid = pid;
                this.name = name;
            }
        }

        public void Addnode(List<Orag> treeDB)
        {
            for (int i=0;i<treeDB.Count;i++) {
                if (treeDB[i].pid==0) {
                    TreeNode pnode = new TreeNode();
                    pnode.Text = treeDB[i].name;
                    pnode.Tag = treeDB[i].id;
                    treeView1.Nodes.Add(pnode);
                    Addchildnode(treeDB[i].id,pnode,treeDB);
                }
            }
        }
        public void Addchildnode(int pid,TreeNode pnode,List<Orag> treeDB) {
            for (int i=0;i<treeDB.Count;i++) {
                if (treeDB[i].pid==pid) {
                    TreeNode cnode = new TreeNode();
                    cnode.Text = treeDB[i].name;
                    cnode.Tag = treeDB[i].id;
                    Addchildnode(treeDB[i].pid, cnode, treeDB);
                }

            }
        }*/

        private void selectAll_Click(object sender, EventArgs e)
        {

        }

        private void clearTable_Click(object sender, EventArgs e)
        {
            existRvt.Clear();
            databaseName.Clear();
            treeView1.Nodes.Clear();
            modelSrcs.Clear();
        }

        private void QueryOneTable_Click(object sender, EventArgs e)
        {
            elementInfo.Rows.Clear();
            if (treeView1.SelectedNode== null)
            {
                MessageBox.Show("Please select a node");
            }
            else {
                bool checkDB = false;
                foreach (TreeNode rootindex in treeView1.Nodes) {
                    if (rootindex.Text.Equals(treeView1.SelectedNode.Text)) {
                        checkDB = true;
                    }
                }
                if (checkDB)
                {
                    MessageBox.Show("Please select a table name");
                }
                else {
                    //Table Name:treeView1.SelectedNode.Text, Schema Name:treeView1.SelectedNode.Parent.Text
                    string[] seprate = new string[2];
                    seprate[0] = treeView1.SelectedNode.Parent.Text;
                    seprate[1] = treeView1.SelectedNode.Text;
                    //separete获得schema和表名 combo[0]为数据库名称
                    foreach (string combo in existRvt)
                    {
                        string[] comBo = combo.Split('\\');
                        if (comBo[1].Equals(seprate[0]))
                        {//comBo[0]: databasename, comBo[1] rvtname, 
                            odbcDatabase();
                            odbcConnection.Open();
                            //search columnName of table
                            List<string> attributeName = new List<string>();//store the attribute name
                            string columnName = @"use[" + comBo[0] + "] select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + seprate[1] + "' and TABLE_SCHEMA = '" + seprate[0] + "'";
                            OdbcCommand odbcCommand = new OdbcCommand(columnName, odbcConnection);
                            OdbcDataReader dataReader = odbcCommand.ExecuteReader();
                            while (dataReader.Read())
                            {
                                attributeName.Add(dataReader[0].ToString());
                            }
                            elementInfo.ColumnCount = attributeName.Count;
                            for (int i = 0; i < attributeName.Count; i++)
                            {
                                elementInfo.Columns[i].HeaderText = attributeName[i];
                            }
                            dataReader.Close();
                            string searchOneTable = "use [" + comBo[0] + "] select * from [" + seprate[0] + "].[" + seprate[1] + "];";//select all info
                            odbcCommand = new OdbcCommand(searchOneTable, odbcConnection);
                            OdbcDataReader DataReader = odbcCommand.ExecuteReader();
                            while (DataReader.Read())
                            {
                                string[] rowsAdd = new string[attributeName.Count];
                                for (int i = 0; i < attributeName.Count; i++)
                                {
                                    rowsAdd[i] = DataReader[i].ToString();
                                }
                                elementInfo.Rows.Add(rowsAdd);
                            }
                            DataReader.Close();
                            odbcConnection.Close();
                        }
                    }
                }
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Please select a Node");
            }
            else {
                bool checkDB = false;
                foreach (TreeNode rootindex in treeView1.Nodes)
                {
                    if (rootindex.Text.Equals(treeView1.SelectedNode.Text))
                    {
                        checkDB = true;//is a schema
                    }
                }
                if (checkDB) {//is a database

                    string schemadb = treeView1.SelectedNode.Text;//Table Name
                    string Info = "";
                    string verification = "";
                    string Src = "";
                    foreach (string schema in existRvt)
                    {
                        string[] comBo = schema.Split('\\');
                        //   MessageBox.Show("DatabaseName:" + comBo[0] + ",RevitName:" + comBo[1]+",TableName:"+ seprate[1]);
                        if (schemadb.Equals(comBo[1]))
                        {
                            Info = comBo[0] + "\\" + comBo[1];
                            verification = comBo[1];
                        }
                    }
                    //Info: DatabaseName+ SchemaName
                    foreach (string src in modelbox.Items)
                    {
                        string[] a = src.Split('\\');
                        string RevitFilename = a[a.Length - 1].Substring(0, a[a.Length - 1].Length - 4).ToUpper();
                        if ((RevitFilename == verification) || (RevitFilename.Equals(verification)))
                        {
                            Src = src;

                        }
                    }
                    Import_DB(Info, Src);

                }
                else {//is a table
                    string[] seprate = new string[2];
                    seprate[0] = treeView1.SelectedNode.Parent.Text;//schema name
                    seprate[1] = treeView1.SelectedNode.Text;//table name
                    string[] SqlInfo = new string[3];
                    string Src = "";
                    string skr = "";
                    foreach (string schema in existRvt) {
                        string[] comBo = schema.Split('\\');
                     //   MessageBox.Show("DatabaseName:" + comBo[0] + ",RevitName:" + comBo[1]+",TableName:"+ seprate[1]);
                        if (seprate[0].Equals(comBo[1])) {
                            skr = comBo[1];
                            SqlInfo[0] = comBo[0];
                            SqlInfo[1] = comBo[1];
                            SqlInfo[2] = seprate[1];
                        }
                    }

                    foreach (string src in modelbox.Items) {
                        string[] a=src.Split('\\');
                        string RevitFilename = a[a.Length - 1].Substring(0, a[a.Length - 1].Length- 4).ToUpper();
                        if ((RevitFilename==skr)||(RevitFilename.Equals(skr))) {
                            Src = src;
      
                        }
                    }
                    //MessageBox.Show("DatabaseName:" + SqlInfo[0] + ",RevitName(SchemaName):" + SqlInfo[1] + ",TableName:" + SqlInfo[2] + ",Src:" + Src);
                    Import_table(SqlInfo,Src);
                }
            }
        }
        public int sqlColumnNumber(string[] Sqlselect)
        {
            int totalcount = 0;
            odbcDatabase();
            odbcConnection.Open();
            string sql_get_table_name = "use["+Sqlselect[0]+"] select count(*) from ["+Sqlselect[1]+"].["+Sqlselect[2]+"];";//获取数据文件.
            OdbcCommand odbcCommand = new OdbcCommand(sql_get_table_name, odbcConnection);
            OdbcDataReader odbcDataReader = odbcCommand.ExecuteReader();
            List<string> table_name = new List<string>();
            while (odbcDataReader.Read())
            {
                totalcount = Int32.Parse(odbcDataReader[0].ToString());
            }
            return totalcount;
        }

        public void Import_table(string[] SqlInfo, string src) {
            //   MessageBox.Show("DatabaseName:" + comBo[0] + ",RevitName:" + comBo[1]+",TableName:"+ seprate[1]);
            ProBar pro = new ProBar();
            pro.Show();
            pro.proBarMax = sqlColumnNumber(SqlInfo);
            pro.proBarMin = 0;
            Double mother = Convert.ToDouble(pro.proBarMax);
            Double son = 0;
            DateTime time = System.DateTime.Now;//获取时间
            string datatime = time.ToString("dd_mm_yyyy");
            datatime = datatime.Contains("_").ToString().Replace("_", "") + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString();
            string createfile = src.Substring(0,(src.Length-SqlInfo[1].Length-4))+ SqlInfo[1] +"_"+ SqlInfo[2] + "_ImportLogFile_" + datatime + ".txt";
            StreamWriter sw = new StreamWriter(createfile, false);
            sw.WriteLine("This is the journal for \" RevitFileName :" +SqlInfo[1] + "\" at " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string load_fold = "Loading the folder:\'" + src + "\'...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
            sw.WriteLine(load_fold);
            this.uiApp = CommandData.Application;
            Document Doc = uiApp.Application.OpenDocumentFile(src);
            SetDoc(Doc);//这里可以用小的doc
            DataTable dt = new DataTable();
            odbcDatabase();
            odbcConnection.Open();
            string connect_dbinfo = "Connect the database: \'" + SqlInfo[0] + "\' ...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
            sw.WriteLine(connect_dbinfo);
            //search columnName of table
            List<string> txt_info = new List<string>();
            List<string> attributeName = new List<string>();//store the attribute name

            //string columnName= @"select distinct TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_CATALOG='503-HGS-FED_18_2019_02_18' and TABLE_SCHEMA='503.002_25 HARGREAVES ST_RCP_RVT18';";
            string columnName = @"use[" + SqlInfo[0] + "] select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + SqlInfo[2] + "' and TABLE_SCHEMA = '" + SqlInfo[1] + "'";
            OdbcCommand odbcCommand = new OdbcCommand(columnName, odbcConnection);
            OdbcDataReader dataReader = odbcCommand.ExecuteReader();
            string sqlSelect = "Execute the sql select language for all the column name......." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
            sw.WriteLine(sqlSelect);
            while (dataReader.Read())
            {
                if (dataReader[0].ToString().Equals("Position_X") || string.Equals(dataReader[0].ToString(), "Position_Y") || string.Equals(dataReader[0].ToString(), "Count"))
                {
                    continue;
                }
                else {
                    attributeName.Add(dataReader[0].ToString());
                }
               
            }
            dataReader.Close();
            string searchOneTable = "use [" + SqlInfo[0] + "] select * from [" + SqlInfo[1] + "].[" + SqlInfo[2] + "];";//select all info
            string sqlSelectAll = "Search all the information for the table: ["+ SqlInfo[1]+"].[" + SqlInfo[2]+ "] at " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
            sw.WriteLine(sqlSelectAll);
            odbcCommand = new OdbcCommand(searchOneTable, odbcConnection);
            OdbcDataReader DataReader = odbcCommand.ExecuteReader();
            while (DataReader.Read())
            {
                //Get the element.
                int id = int.Parse(DataReader[0].ToString());
                pro.labelShow = "Process the element:" + id;
                ElementId eid = new ElementId(id);
                Element pig = doc.GetElement(eid);
                string attri_empty = "Read the element ：" + id + "    " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                sw.WriteLine(attri_empty);
                if (whetherElementInGroup(pig))
                {
                    string ungroup = "The element :" + DataReader[0].ToString() + " in the group family" + "    " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                    sw.WriteLine(ungroup);
                }
                for (int i = 1; i < attributeName.Count-1; i++)
                {
                    string[] parentValue = DataReader[i].ToString().Split(',');
                    string value = parentValue[parentValue.Length-1];// acquire the value of attribute
                    string propertyName = attributeName[i];
                    Transaction tt = new Transaction(doc, "Change Parameter");
                    FailureHandlingOptions failureHandlingOptions = tt.GetFailureHandlingOptions();
                    FailureHandler failureHandler = new FailureHandler();
                    failureHandlingOptions.SetFailuresPreprocessor(failureHandler);
                    // 这句话是关键  
                    failureHandlingOptions.SetClearAfterRollback(true);
                    tt.SetFailureHandlingOptions(failureHandlingOptions);
                    tt.Start();
                    List<string> world = new List<string>();
                    if (whetherElementInGroup(pig))
                    {
                        tt.Commit();
                        if (failureHandler.ErrorMessage != "")

                        {
                            string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                            sw.WriteLine(erro_warning);
                        }
                        continue;
                    }
                  //  string attributeSpace = AddSpacesToColumnName(propertyName);
                    Parameter peter = pig.LookupParameter(propertyName);//kvp.key为属性名
                                                                        //判断该参数是否是ReadOnly (p.IsReadOnly)
                                                                        //判断该参数交互时是否可修改(p.UserModifiable)
                                                                        //判断该参数的StorageType
                                                                        //将要写入该参数的值转换为相应的数据类型，然后Set().
                    if (peter == null)
                    {
                        string peter_null = "The parameter " + propertyName + " (elementid:" + id + ") is empty" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                        sw.WriteLine(peter_null);
                        tt.Commit();
                        if (failureHandler.ErrorMessage != "")

                        {
                            string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                            sw.WriteLine(erro_warning);
                        }
                        continue;
                    }
                    else if (peter.IsReadOnly)
                    {
                        string peter_readonly = "The parameter " + propertyName + " (elementid =" + id + ") is readonly" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                        sw.WriteLine(peter_readonly);
                        //   MessageBox.Show("The parameter is read only:" + kvp.Key);
                        tt.Commit();
                        if (failureHandler.ErrorMessage != "")
                        {
                            string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                            sw.WriteLine(erro_warning);
                        }
                        continue;
                    }
                    else
                    {
                        StorageType sT = peter.StorageType;
                        if (StorageType.ElementId == sT && value.Equals(""))
                        {
                            // MessageBox.Show("NUll ELEMENTID"+","+kvp.Value);
                            string petter_noelementid = "The parameter " + propertyName + " (elementid=" + id + ") does not have elementid" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                            sw.WriteLine(petter_noelementid);
                            tt.Commit();
                            if (failureHandler.ErrorMessage != "")

                            {
                                string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                sw.WriteLine(erro_warning);
                            }
                        }
                        else
                        {
                            //  MessageBox.Show("Set");
                            string originalData = GetParameterType(peter);
                            // MessageBox.Show(originalData);
                            string dbData = value.ToString();
                            // MessageBox.Show(dbData);
                            if (string.IsNullOrEmpty(originalData) && string.IsNullOrEmpty(dbData))
                            {
                                string petterSet = "The parameter " + propertyName + " (elementid=" + id + ") do not need to change because the value is the same" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                sw.WriteLine(petterSet);
                                tt.Commit();
                            }
                            else if (string.IsNullOrEmpty(originalData) && (!string.IsNullOrEmpty(dbData)))
                            {
                                SetParameterValue(peter, value);
                                string petter_set = "The parameter " + propertyName + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                sw.WriteLine(petter_set);
                                tt.Commit();
                            }
                            else if (string.IsNullOrEmpty(dbData) && (!string.IsNullOrEmpty(originalData)))
                            {
                                SetParameterValue(peter, value);
                                string petter_set = "The parameter " + propertyName + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                sw.WriteLine(petter_set);
                                tt.Commit();
                            }
                            else if (originalData.Equals(dbData))
                            {
                                string petterSet = "The parameter " + propertyName + " (elementid=" + id + ") do not need to change because the value is the same" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                sw.WriteLine(petterSet);
                                tt.Commit();

                            }
                            else
                            {
                                SetParameterValue(peter, value);
                                string petter_set = "The parameter " + propertyName + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                sw.WriteLine(petter_set);
                                tt.Commit();
                            }
                            if (failureHandler.ErrorMessage != "")
                            {
                                string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                sw.WriteLine(erro_warning);
                            }
                        }
                    }
                    /*if (peter.UserModifiable) {
                              string dbData = value.ToString();
                              // MessageBox.Show(dbData);
                              SetParameterValue(peter, value);
                              string petter_set = "The parameter " + propertyName + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                              sw.WriteLine(petter_set);
                              tt.Commit();
                              if (failureHandler.ErrorMessage != "")

                              {
                                  string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                  sw.WriteLine(erro_warning);
                              }
                              continue;
                          }*/
                }
                
                
                son++;
                Int32 percent1 = Convert.ToInt32((son / mother) * 100);
                pro.label2show = percent1.ToString() + "%";
                pro.proBarSG++;
            }
            pro.Close();
            DataReader.Close();
            odbcConnection.Close();
            string success_import = "Import successful";
            sw.WriteLine(success_import);
            sw.Close();
            MessageBox.Show("import success");


        }

        public int sqlSearchElement(string Info)
        {
            int totalcount = 0;
            string[] datainfo = Info.Split('\\');
            string database_name = datainfo[0];//数据库名多十一位
            odbcDatabase();
            odbcConnection.Open();
            string sql_get_table_name = "use ["+datainfo[0]+"]select distinct TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_CATALOG='"+ datainfo[0] + "' and TABLE_SCHEMA='"+ datainfo[1]+ "';";//获取数据文件.
            OdbcCommand odbcCommand = new OdbcCommand(sql_get_table_name, odbcConnection);
            OdbcDataReader odbcDataReader = odbcCommand.ExecuteReader();
            List<string> table_name = new List<string>();
            while (odbcDataReader.Read())
            {
                string result = odbcDataReader[0].ToString();
                if (result != "RevitUpdateHistory")
                {
                    table_name.Add(result);//获得所有表名和表的总数
                }

            }
            for (int i = 1; i <= table_name.Count; i++)
            {
                string sql_getnumof_table = "select count(*) as number from ["+datainfo[1]+"].[" + table_name[i - 1] + "]";
                OdbcCommand orange = new OdbcCommand(sql_getnumof_table, odbcConnection);
                orange.ExecuteNonQuery();
                OdbcDataReader count = orange.ExecuteReader();
                List<int> elementid = new List<int>();
                while (count.Read())
                {
                    int countnumber = Int32.Parse(count[0].ToString());
                    if (countnumber == 0)
                    {
                        totalcount += 1;
                    }
                    else
                    {
                        totalcount += countnumber;
                    }
                }

            }

            return totalcount;
        }



        public void Import_DB(string Info, string src)
        {
            string[] SqlInfo = Info.Split('\\');
            DateTime time = System.DateTime.Now;//获取时间
            string datatime = time.ToString("dd_mm_yyyy");
            datatime = datatime.Contains("_").ToString().Replace("_", "") + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString();
            string createfile = src.Substring(0, (src.Length - SqlInfo[1].Length - 4)) + SqlInfo[1]  + "_ImportLogFile_" + datatime + ".txt";
            StreamWriter sw = new StreamWriter(createfile, false);
            sw.WriteLine("This is the journal for \" RevitFileName :" + SqlInfo[1] + "\" at " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss"));
            string load_fold = "Loading the folder:\'" + src + "\'...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
            sw.WriteLine(load_fold);
            this.uiApp = CommandData.Application;
            Document Doc = uiApp.Application.OpenDocumentFile(src);
            SetDoc(Doc);//这里可以用小的doc
            odbcDatabase();
            odbcConnection.Open();
            string connect_dbinfo = "Connect the database: \'" + SqlInfo[0] + "\' ...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
            sw.WriteLine(connect_dbinfo);
            string tableName = @"use ["+SqlInfo[0]+ "] select distinct TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='" + SqlInfo[1]+"';";
            //string tableName = @"use[503-HGS-FED_18_2019_02_18] select * from[503.002_25 HARGREAVES ST_RCP_RVT18].[Doors]";
           // string tableName = @"select distinct TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_CATALOG='"+SqlInfo[0]+"' and TABLE_SCHEMA='"+SqlInfo[1]+"';";
            OdbcCommand comd = new OdbcCommand(tableName, odbcConnection);
            OdbcDataReader odbcData = comd.ExecuteReader();
            List<string> table_name = new List<string>();
            while (odbcData.Read())
            {
                    string result = odbcData[0].ToString();
                    if (result != "RevitUpdateHistory"&&(!table_name.Contains(result)))
                    {
                        table_name.Add(result);//获得所有表名和表的总数
                    }          
            }

            //loop the information in the table
            ProBar pro = new ProBar();
            pro.Show();
            pro.proBarMax = sqlSearchElement(Info);
            pro.proBarMin = 0;
            Double mother = Convert.ToDouble(pro.proBarMax);
            Double son = 0;

            foreach (string tableInfo in table_name) {
                List<string> txt_info = new List<string>();
                List<string> attributeName = new List<string>();//store the attribute name
                string columnName = @"use[" + SqlInfo[0] + "] select * from [" + SqlInfo[1] + "].[" + tableInfo + "];";
                OdbcCommand odbcCommand = new OdbcCommand(columnName, odbcConnection);
                OdbcDataReader DataReader = odbcCommand.ExecuteReader();
                while (DataReader.Read())
                {
                    //Get the element.
                    int id = int.Parse(DataReader[0].ToString());
                    pro.labelShow = "Process the element:" + id;
                    ElementId eid = new ElementId(id);
                    Element pig = doc.GetElement(eid);
                    string attri_empty = "Read the element ：" + id + " from the table:"+tableInfo+"    " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                    sw.WriteLine(attri_empty);
                    if (whetherElementInGroup(pig))
                    {
                        string ungroup = "The element :" + DataReader[0].ToString() + " in the group family" + "    " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                        sw.WriteLine(ungroup);
                    }
                    for (int i = 1; i < DataReader.FieldCount; i++)
                    {
                        if (DataReader.GetName(i).Equals("Position_X") || string.Equals(DataReader.GetName(i), "Position_Y") || string.Equals(DataReader.GetName(i), "Count"))
                        {
                            continue;
                        }
                        else {
                            string[] parentValue = DataReader[i].ToString().Split(',');
                            string value = parentValue[parentValue.Length - 1];// acquire the value of attribute
                            string propertyName = DataReader.GetName(i);
                            Transaction tt = new Transaction(doc, "Change Parameter");
                            FailureHandlingOptions failureHandlingOptions = tt.GetFailureHandlingOptions();
                            FailureHandler failureHandler = new FailureHandler();
                            failureHandlingOptions.SetFailuresPreprocessor(failureHandler);
                            // 这句话是关键  
                            failureHandlingOptions.SetClearAfterRollback(true);
                            tt.SetFailureHandlingOptions(failureHandlingOptions);
                            tt.Start();
                            List<string> world = new List<string>();
                            if (whetherElementInGroup(pig))
                            {
                                tt.Commit();
                                if (failureHandler.ErrorMessage != "")

                                {
                                    string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                    sw.WriteLine(erro_warning);
                                }
                                continue;
                            }
                            Parameter peter = pig.LookupParameter(propertyName);//kvp.key为属性名
                                                                                //判断该参数是否是ReadOnly (p.IsReadOnly)
                                                                                //判断该参数交互时是否可修改(p.UserModifiable)
                                                                                //判断该参数的StorageType
                                                                                //将要写入该参数的值转换为相应的数据类型，然后Set().
                            if (peter == null)
                            {
                                string peter_null = "The parameter " + propertyName + " (elementid:" + id + ") is empty" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                sw.WriteLine(peter_null);
                                tt.Commit();
                                if (failureHandler.ErrorMessage != "")

                                {
                                    string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                    sw.WriteLine(erro_warning);
                                }
                                continue;
                            }
                            else if (peter.IsReadOnly)
                            {
                                string peter_readonly = "The parameter " + propertyName + " (elementid =" + id + ") is readonly" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                sw.WriteLine(peter_readonly);
                                //   MessageBox.Show("The parameter is read only:" + kvp.Key);
                                tt.Commit();
                                if (failureHandler.ErrorMessage != "")
                                {
                                    string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                    sw.WriteLine(erro_warning);
                                }
                                continue;
                            }
                            else
                            {
                                StorageType sT = peter.StorageType;
                                if (StorageType.ElementId == sT && value.Equals(""))
                                {
                                    // MessageBox.Show("NUll ELEMENTID"+","+kvp.Value);
                                    string petter_noelementid = "The parameter " + propertyName + " (elementid=" + id + ") does not have elementid" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                    sw.WriteLine(petter_noelementid);
                                    tt.Commit();
                                    if (failureHandler.ErrorMessage != "")

                                    {
                                        string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                        sw.WriteLine(erro_warning);
                                    }
                                }
                                else
                                {
                                    //  MessageBox.Show("Set");
                                    string originalData = GetParameterType(peter);
                                    // MessageBox.Show(originalData);
                                    string dbData = value.ToString();
                                    // MessageBox.Show(dbData);
                                    if (string.IsNullOrEmpty(originalData) && string.IsNullOrEmpty(dbData))
                                    {
                                        string petterSet = "The parameter " + propertyName + " (elementid=" + id + ") do not need to change because the value is the same" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                        sw.WriteLine(petterSet);
                                        tt.Commit();
                                    }
                                    else if (string.IsNullOrEmpty(originalData) && (!string.IsNullOrEmpty(dbData)))
                                    {
                                        SetParameterValue(peter, value);
                                        string petter_set = "The parameter " + propertyName + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                        sw.WriteLine(petter_set);
                                        tt.Commit();
                                    }
                                    else if (string.IsNullOrEmpty(dbData) && (!string.IsNullOrEmpty(originalData)))
                                    {
                                        SetParameterValue(peter, value);
                                        string petter_set = "The parameter " + propertyName + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                        sw.WriteLine(petter_set);
                                        tt.Commit();
                                    }
                                    else if (originalData.Equals(dbData))
                                    {
                                        string petterSet = "The parameter " + propertyName + " (elementid=" + id + ") do not need to change because the value is the same" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                        sw.WriteLine(petterSet);
                                        tt.Commit();

                                    }
                                    else
                                    {
                                        SetParameterValue(peter, value);
                                        string petter_set = "The parameter " + propertyName + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                                        sw.WriteLine(petter_set);
                                        tt.Commit();
                                    }
                                    if (failureHandler.ErrorMessage != "")
                                    {
                                        string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                                        sw.WriteLine(erro_warning);
                                    }
                                }
                            }

                        }
                        
                    }
                    son++;
                    Int32 percent1 = Convert.ToInt32((son / mother) * 100);
                    pro.label2show = percent1.ToString() + "%";
                    pro.proBarSG++;
                }
                
            }
            //search columnName of table

            pro.Close();
            odbcConnection.Close();
            string success_import = "Import successful";
            sw.WriteLine(success_import);
            sw.Close();
            MessageBox.Show("import success");

        }
        public static string AddSpacesToColumnName(string columnCaption)
        {
            if (string.IsNullOrWhiteSpace(columnCaption))
                return "";
            StringBuilder newCaption = new StringBuilder(columnCaption.Length * 2);
            newCaption.Append(columnCaption[0]);
            int pos = 1;
            for (pos = 1; pos < columnCaption.Length - 1; pos++)
            {
                if (char.IsUpper(columnCaption[pos]) && !(char.IsUpper(columnCaption[pos - 1]) && char.IsUpper(columnCaption[pos + 1])))
                    newCaption.Append(' ');
                newCaption.Append(columnCaption[pos]);
            }
            newCaption.Append(columnCaption[pos]);
            //MessageBox.Show(newCaption.Length.ToString());
            return newCaption.ToString();
        }

        public string GetParameterType(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsDouble().ToString();

                case StorageType.ElementId:
                    return parameter.AsElementId().IntegerValue.ToString();

                case StorageType.Integer:
                    return parameter.AsInteger().ToString();

                case StorageType.None:
                    return parameter.AsValueString();

                case StorageType.String:
                    return parameter.AsValueString();

                default:
                    return "";
            }
        }
        public bool whetherElementInGroup(Element elem)
        {
            foreach (GroupType groupType in new FilteredElementCollector(doc).OfClass(typeof(GroupType)).Cast<GroupType>().Where(gt => gt.Category.Name == "Model Groups" && gt.Groups.Size > 0))
            {
                foreach (Group group in groupType.Groups)
                {

                    if (group.GetMemberIds().Contains(elem.Id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


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
            catch
            {

                throw new Exception("Invalid Value Input!");
            }
        }

        private List<string> ChangedParameterValue(int id, Dictionary<string, string> table)
        {//第一个是列名，第二个是列值
         //   Autodesk.Revit.DB.Transaction tt = new Autodesk.Revit.DB.Transaction(doc, "Change Parameter");
         //Dictionary<string, string>.ValueCollection valuecollection = table.Values;
            List<string> txt_info = new List<string>();
            ElementId eid = new ElementId(id);
            Element pig = doc.GetElement(eid);
            if (whetherElementInGroup(pig))
            {
                string ungroup = "The element :" + id + " in the group family" + "    " + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                txt_info.Add(ungroup);
            }
            foreach (KeyValuePair<string, string> kvp in table)
            {//遍历每一个值
                Transaction tt = new Transaction(doc, "Change Parameter");
                FailureHandlingOptions failureHandlingOptions = tt.GetFailureHandlingOptions();
                FailureHandler failureHandler = new FailureHandler();
                failureHandlingOptions.SetFailuresPreprocessor(failureHandler);
                // 这句话是关键  
                failureHandlingOptions.SetClearAfterRollback(true);
                tt.SetFailureHandlingOptions(failureHandlingOptions);
                tt.Start();
                List<string> world = new List<string>();
                if (whetherElementInGroup(pig))
                {
                    tt.Commit();
                    if (failureHandler.ErrorMessage != "")

                    {
                        string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                        txt_info.Add(erro_warning);
                    }
                    continue;
                }
                Parameter peter = pig.LookupParameter(kvp.Key);//kvp.key为属性名
                if (peter == null)
                {
                    string peter_null = "The parameter " + kvp.Key + " (elementid:" + id + ") is empty" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                    txt_info.Add(peter_null);
                    tt.Commit();
                    if (failureHandler.ErrorMessage != "")

                    {
                        string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                        txt_info.Add(erro_warning);
                    }
                    continue;
                }
                else if (peter.IsReadOnly)
                {
                    string peter_readonly = "The parameter " + kvp.Key + " (elementid =" + id + ") is readonly" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                    txt_info.Add(peter_readonly);
                    //   MessageBox.Show("The parameter is read only:" + kvp.Key);
                    tt.Commit();
                    if (failureHandler.ErrorMessage != "")
                    {
                        string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                        txt_info.Add(erro_warning);
                    }
                    continue;
                }
                else
                {
                    StorageType sT = peter.StorageType;
                    if (StorageType.ElementId == sT && kvp.Value.Equals(""))
                    {
                        // MessageBox.Show("NUll ELEMENTID"+","+kvp.Value);
                        string petter_noelementid = "The parameter " + kvp.Key + " (elementid=" + id + ") does not have elementid" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                        txt_info.Add(petter_noelementid);
                        tt.Commit();
                        if (failureHandler.ErrorMessage != "")

                        {
                            string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                            txt_info.Add(erro_warning);
                        }
                    }
                    else
                    {
                        //  MessageBox.Show("Set");
                        string originalData = GetParameterType(peter);
                        // MessageBox.Show(originalData);
                        string dbData = kvp.Value.ToString();
                        // MessageBox.Show(dbData);
                        if (string.IsNullOrEmpty(originalData) && string.IsNullOrEmpty(dbData))
                        {
                            string petterSet = "The parameter " + kvp.Key + " (elementid=" + id + ") do not need to change because the value is the same" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                            txt_info.Add(petterSet);
                            tt.Commit();
                        }
                        else if (string.IsNullOrEmpty(originalData) && (!string.IsNullOrEmpty(dbData)))
                        {
                            SetParameterValue(peter, kvp.Value);
                            string petter_set = "The parameter " + kvp.Key + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                            txt_info.Add(petter_set);
                            tt.Commit();
                        }
                        else if (string.IsNullOrEmpty(dbData) && (!string.IsNullOrEmpty(originalData)))
                        {
                            SetParameterValue(peter, kvp.Value);
                            string petter_set = "The parameter " + kvp.Key + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                            txt_info.Add(petter_set);
                            tt.Commit();
                        }
                        else if (originalData.Equals(dbData))
                        {
                            string petterSet = "The parameter " + kvp.Key + " (elementid=" + id + ") do not need to change because the value is the same" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                            txt_info.Add(petterSet);
                            tt.Commit();

                        }
                        else
                        {
                            SetParameterValue(peter, kvp.Value);
                            string petter_set = "The parameter " + kvp.Key + " (elementid=" + id + ") changed" + "...." + DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                            txt_info.Add(petter_set);
                            tt.Commit();
                        }
                        if (failureHandler.ErrorMessage != "")
                        {
                            string erro_warning = "There is an error:" + failureHandler.ErrorSeverity + ", There is an warning: " + failureHandler.ErrorMessage;
                            txt_info.Add(erro_warning);
                        }
                    }
                }
            }
            return txt_info;
        }


        private void elementInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        List<TreeNode> searchtreeNodes;
        List<TreeNode> clearcolorNodes;
        private void searchTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("Please input search content");
            }
            else {
                clearcolorNodes = new List<TreeNode>();
                foreach (TreeNode node in treeView1.Nodes) {
                    clearColor(node);
                }
                searchtreeNodes = new List<TreeNode>();
                foreach (TreeNode node in treeView1.Nodes)
                {
                    SearchLayer(node, this.searchBox.Text);

                }
                for (int i = 0; i < searchtreeNodes.Count; i++)
                {
                    TreeNode temp = searchtreeNodes[i];
                    ExpandNode(temp);
                    if (i == 0 && temp.Parent != null)
                    {
                        treeView1.SelectedNode = temp.Parent;
                    }
                    temp.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        private void clearColor(TreeNode node) {
            if (node.Nodes.Count != 0) {
                for (int i = 0; i < node.Nodes.Count; i++) {
                    clearColor(node.Nodes[i]);
                }
            }
            else if (node.BackColor.Equals(System.Drawing.Color.Red)) {
                node.BackColor = System.Drawing.Color.Transparent;
            }
        }

        private void SearchLayer(TreeNode node, string name)
        {
            if (node.Nodes.Count != 0)
            {
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    SearchLayer(node.Nodes[i], name);
                }
            }
            else if (string.Equals(node.Text, name) || node.Text.Contains(name))
            {
                searchtreeNodes.Add(node);
            }
        }

        private void ExpandNode(TreeNode node)
        {
            if (node.Parent != null)
            {
                node.Expand();
                ExpandNode(node.Parent);
            }
        }

    }
}
