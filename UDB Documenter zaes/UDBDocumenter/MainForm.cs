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
    public partial class MainForm : Form
    {
        Server serverInfo;
        string currentTable;
        string currentTableInfo;
        DataTable dataTblMain;
        TableCommands tcmds;
        public MainForm()
        {
            InitializeComponent();
            //dataGridView1.AutoSize = true;
            //dataGridView1.AutoSizeColumnsMode = true;
            serverInfo = new Server(Settings1.Default.ip, Settings1.Default.port, "Server 1", Settings1.Default.user, Settings1.Default.password, new MSServerEngine());
            string connectionString = serverInfo.GenerateConnectionString(Settings1.Default.database);
            bool connected = serverInfo.ConnectToServer(connectionString);
            serverInfo.AddDatabase(Settings1.Default.database);
            serverInfo.LoadDataFromDatabase();
            tableFieldView1.SetServer(serverInfo);
            //List<string> lst = serverInfo.GetTablesInDatabase(Settings1.Default.database);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Level == 2)
                {
                    tcmds = (TableCommands)e.Node.Tag;
                    string tbl = tcmds.ToString();
                    currentTable = tbl;
                    dataTblMain = serverInfo.ExecuteQuery("select * from " + tbl);//.SelectAllFromTable("select * from " + tbl)
                    /*List<string> queries = new List<string>();
                    queries.Add("select * from " + tbl);
                    List<string> tables = new List<string>();
                    tables.Add(tbl);
                    List<DataTable> dataTbl = serverInfo.ExecuteQuery(queries, tables);*/

                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = dataTblMain;
                    dataGridView1.DataSource = SBind;

                    Table fTbl = serverInfo.SearchTable(tbl);

                    //Show documentation for all fields
                    /*string fieldInfo = "";
                    for (int i = 0; i < fTbl.CountElements(); i++)
                    {
                        Field fld = fTbl.GetField(i);
                        List<string> comments = fld.GetComments();
                        fieldInfo += fld.ToString() + ": " + comments[2]+"\r\n";
                    }*/

                    //Show documentation for choosen field in datagrid 

                    string fieldInfo = "";
                    Field fld = fTbl.GetField(0);
                    List<string> lst = fld.GetComments();
                    fieldInfo += "Наименование: " + fld.ToString() + "\r\nКомментарий: " + lst[2] + "\r\n" + "Тип: " + fld.type + "\r\nПустые значения: " + fld.isNull + "\r\nЗначение по умолчанию: ";
                    if (fld.defaultValue == "")
                        fieldInfo += "Отсутствует";
                    else fieldInfo += fld.defaultValue;
                    tableFieldView1.SetDocumentationInfo(fTbl, fieldInfo);
                    //Field fld = fTbl.GetField(dataGridView1.SelectedColumns[0].Index);

                    //tableFieldView1.SetDocumentationInfo(fTbl, fieldInfo);
                    //tableFieldView1
                    //serverInfo.AddDatabase(database);


                    //dataGridView1.AutoSize = true;
                    //dataGridView1.
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show("Произошла ошибка: " + exc.Message);
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != -1)
                {
                    Table fTbl = serverInfo.SearchTable(currentTable);



                    //Show documentation for choosen field in datagrid 
                    string fieldInfo = "";
                    Field fld = fTbl.GetField(e.ColumnIndex);
                    List<string> lst = fld.GetComments();
                    fieldInfo += fld.ToString() + ": " + lst[2] + "\r\n" + "Тип: " + fld.type + "\r\nПустые значения: " + fld.isNull + "\r\nЗначение по умолчанию: ";
                    if (fld.defaultValue == "")
                        fieldInfo += "Отсутствует";
                    else fieldInfo += fld.defaultValue;
                    tableFieldView1.SetDocumentationInfo(fTbl, fieldInfo);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Произошла ошибка: " + exc.Message);
            }
        }

        private void документацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentTable == "tblIspolnitel")
                    serverInfo.ExecuteQuery(dataTblMain, currentTable);
                else if (currentTable == "tblAllVyrPrint")
                    serverInfo.ExecuteQuery(dataTblMain, currentTable, "intShifrVyrabotki", "sngDepth");
                else if (currentTable == "forvibor")
                    serverInfo.ExecuteQuery(dataTblMain, currentTable, "intShifr", "dtmDataZ");
                else
                    serverInfo.ExecuteQuery(dataTblMain, currentTable, tcmds.GetFieldName());
            }
            catch (Exception exc)
            {
                MessageBox.Show("Произошла ошибка: " + exc.Message);
            }
        }

        private void редактированиеДокументацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                tableFieldView1.button1_Click(sender, e);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Произошла ошибка: " + exc.Message);
            }
        }


    }
}
