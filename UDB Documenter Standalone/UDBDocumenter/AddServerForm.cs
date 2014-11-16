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
        MainForm mainForm;
        Server serverInfo;
        public AddServerForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            engineComboBox.SelectedIndex = 0;
            button2.Text = "Выберите базу данных";
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.ClearServers();
            if(engineComboBox.SelectedIndex == 0)
                serverInfo = new Server(ipTextBox.Text, Convert.ToInt32(portTextBox.Text), serverTextBox.Text, usernameTextBox.Text, passwordTextBox.Text, new MSServerEngine());
            else
                serverInfo = new Server(ipTextBox.Text, Convert.ToInt32(portTextBox.Text), serverTextBox.Text, usernameTextBox.Text, passwordTextBox.Text, new MySQLServerEngine());
            string connectionString = serverInfo.GenerateConnectionString();
            bool connected = serverInfo.ConnectToServer(connectionString);
            if (connected == true)
            {
                mainForm.AddServerToTree(serverInfo.serverNameProperty);

                
                DataTable tbl = serverInfo.InitialGetServerStructure();
                List<bool> boollst = new List<bool>();
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    serverInfo.GenerateConnectionString(tbl.Rows[i][0].ToString());
                    boollst.Add(serverInfo.IsDatabaseDocumented(tbl.Rows[i][0].ToString()));
                }

                for (int i = 0; i < tbl.Rows.Count; i++)
                    databasesCheckList.Items.Add(tbl.Rows[i][0].ToString(), boollst[i]);

                
                MessageBox.Show("Соединение выполнено, выберите базу данных.");
            }
            else
                MessageBox.Show("Соединение не выполнено проверьте введенные данные!");
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            int databaseNumber = 0;
            //int index = databasesCheckList.SelectedIndex;
            //foreach (string database in databasesCheckList.CheckedItems)
            //{
            string database = databasesCheckList.Items[databasesCheckList.SelectedIndex].ToString();
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
            //}

            if (serverInfo.isCreated() == false)
            {
                serverInfo.CreateDocumentationStructure();
                serverInfo.FirstDataWrite();
            }

            serverInfo.LoadDataFromDatabase();
            mainForm.ClearTree();
            databaseNumber = 0;
            

                mainForm.AddDatabasesToTree(database.ToString(), 0);//Edit server №, now 0
                serverInfo.GenerateConnectionString(database.ToString());
                //serverInfo.AddDatabase(database);
                lst = serverInfo.GetTableNames(database.ToString());
                tableLst = serverInfo.GetTables();
                tableNumber = 0;
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
            
            mainForm.AddServer(serverInfo);
            mainForm.SortTree();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            

        }

        private void AddServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void databasesCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            if (databasesCheckList.GetItemChecked(databasesCheckList.SelectedIndex) == true)
                button2.Text = "Продолжить документирование";
            else
                button2.Text = "Начать документирование";
        }
    }
}
