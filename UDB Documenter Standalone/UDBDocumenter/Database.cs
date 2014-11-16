using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Novacode;

namespace UDBDocumenter
{
    public class Database
    {
        string name;
        List<Table> tables;
        Server serverInfo;
        public bool isCreated = false;

        public Database(string databaseName, Server serverInfo)
        {
            tables = new List<Table>();
            name = databaseName;
            this.serverInfo = serverInfo;
        }

        public void Process()
        {
            List<string> tables = serverInfo.GetTablesInDatabase(name);
            foreach (string tableName in tables)
            {
                if (tableName.ToLower() != "udbcomments" && tableName.ToLower() != "udbfields" && tableName.ToLower() != "udbtables")
                {
                    Table tableDefinition = new Table(tableName, serverInfo);
                    tableDefinition.Process();
                    this.tables.Add(tableDefinition);
                    //isCreated = false;
                }
                else isCreated = true;
            }
        }

        public void Process(List<DataTable> dataTables)
        {
            tables.Clear();
            foreach (DataRow row in dataTables[2].Rows)
            {
                tables.Add(new Table(row, serverInfo, dataTables[0], dataTables[1]));
            }
            //tables.Sort();
        }

        public override string ToString()
        {
            return name;
        }

        public List<Table> GetTables()
        {
            List<Table> tableNames = new List<Table>();
            foreach (Table tbl in tables)
            {
                tableNames.Add(tbl);
            }
            return tableNames;
        }

        public List<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();
            foreach (Table tbl in tables)
            {
                tableNames.Add(tbl.ToString());
            }
            return tableNames;
        }

        public List<string> GetTableFieldNames(string table)
        {
            List<string> fieldNames = new List<string>();
            foreach (Table tbl in tables)
            {
                if (tbl.ToString() == table)
                    fieldNames = tbl.GetFields();
            }
            return fieldNames;
        }


        //int commentCount = 0;
        public int CountElements()
        {
            int count = 0;
            foreach (Table tbl in tables)
            {
                count += tbl.CountElements();
                count++;
            }
            return count;
        }

        public DataTable AddTablesToDataTable(DataTable dTbl, List<int> commentIds, ref int currentCommentId) //careful call!
        {
            int i = 0;
            foreach (Table tbl in tables)
            {
                tbl.SetComment(commentIds[i]);
                tbl.AddToDataTable(dTbl);
                i++;
            }
            currentCommentId = i;
            return dTbl;
        }

        public void SetTableIds(List<int> tableIds)
        {
            int i = 0;
            foreach (Table tbl in tables)
            {
                tbl.SetId(tableIds[i]);
                i++;
            }

        }

        public DataTable AddFieldsToDataTable(DataTable tbl, List<int> commentIds, int currentCommentIdIndex)
        {
            foreach(Table table in tables)
                table.AddFieldsToDataTable(tbl, commentIds, ref currentCommentIdIndex);
            return tbl;
        }

        public DataTable SetTableCommentsInDataTable(DataTable tbl)
        {
            foreach (Table table in tables)
            {
                tbl = table.SetCommentInDataTable(tbl);
                
            }
            return tbl;
        }

        public void ToReport(string filePath, int language)
        {
            List<string> tableString = new List<string>();
            tableString.Add("Table: ");
            tableString.Add("Таблиця: ");
            tableString.Add("Таблица: ");
            List<string> databaseString = new List<string>();
            databaseString.Add("Database: ");
            databaseString.Add("База даних: ");
            databaseString.Add("База данных: ");
            List<string> tablesString = new List<string>();
            tablesString.Add("Tables: ");
            tablesString.Add("Таблиць: ");
            tablesString.Add("Таблиц: ");
            List<string> dateString = new List<string>();
            dateString.Add("Creation date: ");
            dateString.Add("Дата створення: ");
            dateString.Add("Дата создания: ");
            DocX doc = DocX.Create(filePath);
            doc.InsertParagraph(databaseString[language] + name + "        " + tablesString[language] + tables.Count + "        " + dateString[language] + DateTime.Now + "\r\n");
            foreach (Table tbl in tables)
            {
                doc.InsertParagraph(tableString[language]+tbl.ToString()+"\r\n");
                tbl.ToReport(filePath, ref doc, language);
            }
            doc.Save();
        }
        /*public List<DataTable> ToDataTables(List<DataTable> tbl)
        {
            foreach (Table table in tables)
            {
                DataRow row = tbl[0].NewRow();
                row.SetField(0, 1);
                row.SetField(1, table.ToString());
                row.SetField(2, commentCount);
                commentCount++;
                table.ToDataTable(
            }
            return tbl;
        }*/

        /*public TreeView ToTree(TreeView treeView)
        {
            //TreeView treeView = new TreeView();
            foreach (Table tbl in tables)
            {
                treeView.Nodes.Add(tbl.ToString());
            }
            
            return treeView;
        }*/
    }
}
