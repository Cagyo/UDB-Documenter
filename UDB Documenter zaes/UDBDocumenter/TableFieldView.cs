using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UDBDocumenter
{
    public partial class TableFieldView : UserControl
    {
        Server serverInfo;
        public TableFieldView()
        {
            InitializeComponent();
        }

        public void SetServer(Server serverInfo)
        {
            this.serverInfo = serverInfo;
        }

        public void SetDocumentationInfo(Table tbl, string fieldInfo)
        {
            textBox1.Text = "Наименование: "+tbl.ToString()+"\r\nКомментарий: "+tbl.GetComments()[2];//rus comment
            textBox2.Text = fieldInfo;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(serverInfo);
            f.AddServerToTree("Server 1");
            //f.ClearTree();
            int databaseNumber = 0;

            f.AddDatabasesToTree(Settings1.Default.database, 0);//Edit server №, now 0
            //serverInfo.GenerateConnectionString(Settings1.Default.database);
            //serverInfo.AddDatabase(database);
            List<string> lst = serverInfo.GetTableNames(Settings1.Default.database);
            List<Table> tableLst = serverInfo.GetTables();
            int tableNumber = 0;
            foreach (string table in lst)
            {
                f.AddTableToTree(table, databaseNumber, 0, tableLst[tableNumber]);
                List<string> columns = serverInfo.GetTableFieldNames(Settings1.Default.database, table);
                foreach (string column in columns)
                {
                    f.AddColumnToTree(column, tableNumber, databaseNumber, 0);
                }
                tableNumber++;
            }
            databaseNumber++;
            f.SortTree();
            f.Show();

        }
    }
}
