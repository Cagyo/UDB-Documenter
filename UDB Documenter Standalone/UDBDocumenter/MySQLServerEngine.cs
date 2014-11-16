using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace UDBDocumenter
{
    public class MySQLServerEngine : IServerEngine
    {
        
        MySqlConnection sqlConn;
        string connectionString;
        string dbname;
        List<MySqlDataAdapter> mainAdapter = new List<MySqlDataAdapter>();
        public string GenerateConnectionString(Server serverInfo)
        {
            //Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd = myPassword;
            connectionString = "Server=" + serverInfo.ipProperty + ";Port=" + serverInfo.portProperty + ";Database=information_schema;Uid=" + serverInfo.userNameProperty + ";Pwd=" + serverInfo.passwordProperty + ";";
            return connectionString;
        }

        public string GenerateConnectionString(Server serverInfo, string databaseName)
        {
            connectionString = "Server=" + serverInfo.ipProperty + ";Port=" + serverInfo.portProperty + ";Database=" + databaseName + ";Uid=" + serverInfo.userNameProperty + ";Pwd=" + serverInfo.passwordProperty + ";";
            return connectionString;
        }

        public bool ConnectToServer(string connectionString)
        {
            sqlConn = new MySqlConnection(connectionString);
            try
            {
                sqlConn.Open();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public DataTable InitialGetServerStructure()
        {
            DataTable tbl = SelectAllFromTable("show databases"); // !!!
            return tbl;
        }

        public DataTable SelectAllFromTable(string sqlCommand)
        {
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();

            MySqlDataAdapter tableAdapter = new MySqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            sqlConn.Close();
            return dataTable;
        }

        public List<string> GetTablesInDatabase(string databaseName)
        {
            dbname = databaseName;
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT TABLE_NAME FROM information_schema.COLUMNS where TABLE_SCHEMA = '"+databaseName+"' group by table_name";
            MySqlDataAdapter tableAdapter = new MySqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            List<string> lst = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
                lst.Add(dataTable.Rows[i].ItemArray[0].ToString());
            //lst.Sort();
            sqlConn.Close();
            return lst;
        }

        public bool IsDatabaseDocumented(string databaseName)
        {
            //dbname = databaseName;
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT TABLE_NAME FROM information_schema.COLUMNS where TABLE_SCHEMA = '" + databaseName + "' group by table_name";
            MySqlDataAdapter tableAdapter = new MySqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            List<string> lst = new List<string>();
            bool isDocumented = false;
            for (int i = 0; i < dataTable.Rows.Count; i++)
                if (dataTable.Rows[i].ItemArray[0].ToString().ToLower() == "udbcomments")
                    isDocumented = true;
            //lst.Sort();
            sqlConn.Close();
            return isDocumented;
        }

        public List<string> GetColumnsInTable(string tableName)
        {
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT column_name FROM information_schema.columns where table_name = '" + tableName + "' and table_schema = '"+dbname+"';";
            MySqlDataAdapter tableAdapter = new MySqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            List<string> lst = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
                lst.Add(dataTable.Rows[i].ItemArray[0].ToString());
            sqlConn.Close();
            return lst;
        }

        public List<string> GetColumnInTable(string tableName, string column)
        {
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT column_default, is_nullable, data_type, character_maximum_length  FROM information_schema.columns where table_name = '" + tableName + "' and column_name = '" + column + "' and table_schema = '"+dbname+"';";
            MySqlDataAdapter tableAdapter = new MySqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            List<string> lst = new List<string>();
            for (int i = 0; i < dataTable.Columns.Count; i++)
                lst.Add(dataTable.Rows[0].ItemArray[i].ToString());
            sqlConn.Close();
            return lst;
        }

        public void ExecuteNonQuery(string query)
        {
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            MySqlCommand command = new MySqlCommand(query, sqlConn);
            command.ExecuteNonQuery();
            sqlConn.Close();
        }

        public List<DataTable> ExecuteQuery(List<string> sqlCommand, List<string> tableName)
        {
            mainAdapter.Clear();//!!!
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            List<DataTable> mainDataset = new List<DataTable>();
            //string command = "";
            for (int i = 0; i < sqlCommand.Count; i++)
            {
                //command += sqlCommand[i];
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqlCommand[i], sqlConn);
                mainAdapter.Add(adapter);
                MySqlCommandBuilder objCommandBuilder = new MySqlCommandBuilder(mainAdapter[i]);
                mainAdapter[i].InsertCommand = objCommandBuilder.GetInsertCommand();
                mainAdapter[i].UpdateCommand = objCommandBuilder.GetUpdateCommand();
                mainAdapter[i].DeleteCommand = objCommandBuilder.GetDeleteCommand();
                DataTable dataTable = new DataTable();

                mainAdapter[i].Fill(dataTable);
                mainDataset.Add(dataTable);
                /*if (i == 0)
                    mainDataset[i].TableName = "UDBComments";*/

            }

            /*mainAdapter.TableMappings.Add("Table", "UDBComments");
            mainAdapter.TableMappings.Add("Table1", "UDBFields");
            mainAdapter.TableMappings.Add("Table2", "UDBTables");*/


            sqlConn.Close();
            return mainDataset;
        }

        public void ExecuteQuery(List<DataTable> dst)
        {
            sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            for (int i = 0; i < mainAdapter.Count; i++)
            {
                int res = mainAdapter[i].Update(dst[i]);
            }
            sqlConn.Close();
            //return mainDataset;
        }

        public void CreateDocumentationStructure()
        {
            ExecuteNonQuery("CREATE TABLE `udbcomments` ("+
 " `id` int(11) NOT NULL AUTO_INCREMENT,"+
 " `commentEng` text,"+
 " `commentUkr` text,"+
 " `commentRus` text,"+
 " PRIMARY KEY (`id`)"+
") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            ExecuteNonQuery("CREATE TABLE `udbtables` (" +
 " `id` int(11) NOT NULL AUTO_INCREMENT," +
 " `name` varchar(50) DEFAULT NULL," +
 " `commentID` int(11) DEFAULT NULL," +
 " PRIMARY KEY (`id`)," +
 " KEY `commentk_idx` (`commentID`)," +
 " CONSTRAINT `commentk` FOREIGN KEY (`commentID`) REFERENCES `udbcomments` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION" +
") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            ExecuteNonQuery("CREATE TABLE `udbfields` ("+
 " `id` int(11) NOT NULL AUTO_INCREMENT," +
 " `name` varchar(50) DEFAULT NULL,"+
 " `type` varchar(50) DEFAULT NULL,"+
 " `isnull` bit(1) DEFAULT NULL,"+
 " `defaultvalue` varchar(50) DEFAULT NULL,"+
 " `tableId` int(11) DEFAULT NULL,"+
 " `commentId` int(11) DEFAULT NULL,"+
 " PRIMARY KEY (`id`),"+
 " KEY `tablek_idx` (`tableId`),"+
 " KEY `commentk_idx` (`commentId`),"+
 " CONSTRAINT `tablek` FOREIGN KEY (`tableId`) REFERENCES `udbtables` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,"+
 " CONSTRAINT `commentfk` FOREIGN KEY (`commentId`) REFERENCES `udbcomments` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION"+
") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            

        }
    }
}
