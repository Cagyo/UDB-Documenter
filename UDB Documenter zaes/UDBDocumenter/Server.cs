using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace UDBDocumenter
{
    public class Server
    {
        private string ip;
        public string ipProperty
        {
            set { ip = value; }
            get { return ip; }
        }
        private int port;
        public int portProperty
        {
            set { port = value; }
            get { return port; }
        }
        private string serverName;
        public string serverNameProperty
        {
            set { serverName = value; }
            get { return serverName; }
        }
        private string userName;
        public string userNameProperty
        {
            set { userName = value; }
            get { return userName; }
        }
        private string password;
        public string passwordProperty
        {
            set { password = value; }
            get { return password; }
        }
        private IServerEngine engine;
        public IServerEngine engineProperty
        {
            set { engine = value; }
            get { return engine; }
        }

        List<Database> databases;

        public Server()
        {

        }

        public Server(string ip, int port, string serverName, string userName, string password, MSServerEngine engine)
        {
            databases = new List<Database>();
            this.ip = ip;
            this.port = port;
            this.serverName = serverName;
            this.userName = userName;
            this.password = password;
            this.engine = engine;
        }

        public string GenerateConnectionString()
        {
            return engine.GenerateConnectionString(this);
        }

        public string GenerateConnectionString(string databaseName)
        {
            return engine.GenerateConnectionString(this, databaseName);
        }

        public bool ConnectToServer(string connectionString)
        {
            return engine.ConnectToServer(connectionString);
        }

        public DataTable SelectAllFromTable(string sqlCommand)
        {
            return engine.SelectAllFromTable(sqlCommand);
        }

        public List<string> GetTablesInDatabase(string databaseName)
        {
            return engine.GetTablesInDatabase(databaseName);
        }

        public List<string> GetColumnsInTable(string tableName)
        {
            return engine.GetColumnsInTable(tableName);
        }

        public List<string> GetColumnInTable(string tableName, string column)
        {
            return engine.GetColumnInTable(tableName, column);
        }

        public void AddDatabase(string databaseName)
        {
            Database database = new Database(databaseName, this);
            database.Process();
            databases.Add(database);
        }

        public List<string> GetDatabaseNames()
        {
            List<string> databasesNames = new List<string>();
            foreach (Database dbs in databases)
            {
                GenerateConnectionString(dbs.ToString());
                databasesNames.Add(dbs.ToString());
            }
            return databasesNames;
        }

        public List<string> GetTableNames(string database)
        {
            List<string> tableNames = new List<string>();
            foreach (Database dbs in databases)
            {
                if (dbs.ToString() == database)
                {
                    tableNames = dbs.GetTableNames();
                }
            }
            return tableNames;
        }

        public List<Table> GetTables()
        {
            List<Table> tables = new List<Table>();
            foreach (Database dbs in databases)
            {
                tables = dbs.GetTables();
            }
            return tables;
        }

        public List<string> GetTableFieldNames(string database, string table)
        {
            List<string> fieldNames = new List<string>();
            foreach (Database dbs in databases)
            {
                if (dbs.ToString() == database)
                {
                    fieldNames = dbs.GetTableFieldNames(table);
                }
            }
            return fieldNames;
        }

        public bool isCreated()
        {
            return databases[0].isCreated;
        }

        public void CreateDocumentationStructure()
        {
            engine.CreateDocumentationStructure();
        }

        public List<DataTable> ExecuteQuery(List<string> sqlCommand, List<string> tableName)
        {
            return engine.ExecuteQuery(sqlCommand,tableName);
        }

        public void ExecuteQuery(List<DataTable> dst)
        {
            engine.ExecuteQuery(dst);
        }

        public void LoadDataFromDatabase()
        {
            foreach (Database db in databases)
            {
                List<string> lst = new List<string>();
                lst.Add("select * from UDBComments");
                lst.Add("select * from UDBFields");
                lst.Add("select * from UDBTables");
                List<string> lstNames = new List<string>();
                lstNames.Add("UDBComments");
                lstNames.Add("UDBFields");
                lstNames.Add("UDBTables");

                List<DataTable> dst = engine.ExecuteQuery(lst, lstNames);
                db.Process(dst);
            }
        }

        public void FirstDataWrite()
        {
            List<int> count = new List<int>();
            foreach (Database db in databases)
            {
                count.Add(db.CountElements());

                List<string> lst = new List<string>();
                lst.Add("select * from UDBComments");
                lst.Add("select * from UDBFields");
                lst.Add("select * from UDBTables");
                List<string> lstNames = new List<string>();
                lstNames.Add("UDBComments");
                lstNames.Add("UDBFields");
                lstNames.Add("UDBTables");

                List<DataTable> dst = engine.ExecuteQuery(lst, lstNames);
                for (int i = 0; i < count[0]; i++)
                {
                    DataRow row = dst[0].NewRow();
                    row.SetField(0, i + 1);
                    row.SetField(1, "");
                    row.SetField(2, "");
                    row.SetField(3, "");
                    dst[0].Rows.Add(row);
                }
                engine.ExecuteQuery(dst);
                
                int currentCommentId = 0;
                dst = engine.ExecuteQuery(lst, lstNames);
                List<int> commentIds = new List<int>();
                foreach(DataRow row in dst[0].Rows)
                    commentIds.Add(Convert.ToInt32(row[0]));
                db.AddTablesToDataTable(dst[2], commentIds, ref currentCommentId);
                engine.ExecuteQuery(dst);
                
                dst = engine.ExecuteQuery(lst, lstNames);
                List<int> tableIds = new List<int>();
                foreach (DataRow row in dst[2].Rows)
                    tableIds.Add(Convert.ToInt32(row[0]));
                db.SetTableIds(tableIds);
                db.AddFieldsToDataTable(dst[1],commentIds,currentCommentId);
                engine.ExecuteQuery(dst);
            }
        }

        public void SaveChanges()
        {
            
            foreach (Database db in databases)
            {
                List<string> lst = new List<string>();
                lst.Add("select * from UDBComments");
                lst.Add("select * from UDBFields");
                lst.Add("select * from UDBTables");
                List<string> lstNames = new List<string>();
                lstNames.Add("UDBComments");
                lstNames.Add("UDBFields");
                lstNames.Add("UDBTables");

                List<DataTable> dst = engine.ExecuteQuery(lst, lstNames);
                db.SetTableCommentsInDataTable(dst[0]);
                engine.ExecuteQuery(dst);
            }
        }

        public Table SearchTable(string tableName)
        {
            return databases[0].SearchTable(tableName);
        }

        public DataTable ExecuteQuery(string sqlCommand)
        {
            return engine.ExecuteQuery(sqlCommand);
        }

        public void ExecuteQuery(DataTable dtbl, string tableName, string columnName)
        {
            engine.ExecuteQuery(dtbl,tableName, columnName);
        }

        public void ExecuteQuery(DataTable dst, string tableName)
        {
            engine.ExecuteQuery(dst, tableName);
        }

        public void ExecuteQuery(DataTable dst, string tableName, string columnName1, string columnName2)
        {
            engine.ExecuteQuery(dst, tableName, columnName1, columnName2);
        }

        public void ToReport(string filePath, int language)
        {
            foreach (Database db in databases)
                db.ToReport(filePath, language);
        }
    }
}
