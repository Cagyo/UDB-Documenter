using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace UDBDocumenter
{
    public partial class AddServerForm : Form
    {
        Form1 mainForm;
        Server serverInfo;
        public AddServerForm(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serverInfo = new Server(ipTextBox.Text, Convert.ToInt32(portTextBox.Text), serverTextBox.Text, usernameTextBox.Text, passwordTextBox.Text, new MSServerEngine());
            string connectionString = serverInfo.GenerateConnectionString();
            bool connected = serverInfo.ConnectToServer(connectionString);
            mainForm.AddServerToTree(serverInfo.serverNameProperty);
            MessageBox.Show(connected.ToString());
            DataTable tbl = serverInfo.SelectAllFromTable("Select name FROM master..sysdatabases"); //

            for (int i = 0; i < tbl.Rows.Count; i++)
                databasesCheckList.Items.Add(tbl.Rows[i][0].ToString());
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            int databaseNumber = 0;
            foreach (string database in databasesCheckList.CheckedItems)
            {
                
                mainForm.AddDatabasesToTree(database.ToString(), 0);//Edit server №, now 0
                serverInfo.GenerateConnectionString(database.ToString());
                serverInfo.AddDatabase(database);
                List<string> lst = serverInfo.GetTableNames(database.ToString());
                List<Table> tableLst = serverInfo.GetTables();
                int tableNumber = 0;
                foreach (string table in lst)
                {
                    mainForm.AddTableToTree(table, databaseNumber, 0, tableLst[tableNumber]);
                    List<string> columns = serverInfo.GetTableFieldNames(database, table);
                    foreach (string column in columns)
                    {
                        mainForm.AddColumnToTree(column, tableNumber, databaseNumber, 0);
                    }
                    tableNumber++;
                }
                databaseNumber++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serverInfo.isCreated() == false)
            {
                serverInfo.CreateDocumentationStructure();
                serverInfo.FirstDataWrite();
            }
            
                serverInfo.LoadDataFromDatabase();
                mainForm.ClearTree();
                int databaseNumber = 0;
                foreach (string database in databasesCheckList.CheckedItems)
                {

                    mainForm.AddDatabasesToTree(database.ToString(), 0);//Edit server №, now 0
                    serverInfo.GenerateConnectionString(database.ToString());
                    //serverInfo.AddDatabase(database);
                    List<string> lst = serverInfo.GetTableNames(database.ToString());
                    List<Table> tableLst = serverInfo.GetTables();
                    int tableNumber = 0;
                    foreach (string table in lst)
                    {
                        mainForm.AddTableToTree(table, databaseNumber, 0, tableLst[tableNumber]);
                        List<string> columns = serverInfo.GetTableFieldNames(database, table);
                        foreach (string column in columns)
                        {
                            mainForm.AddColumnToTree(column, tableNumber, databaseNumber, 0);
                        }
                        tableNumber++;
                    }
                    databaseNumber++;
                }
            

        }

        private void AddServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.AddServer(serverInfo);
        }
    }
}
