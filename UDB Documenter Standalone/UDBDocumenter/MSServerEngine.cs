using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace UDBDocumenter
{
    public class MSServerEngine : IServerEngine
    {
        SqlConnection sqlConn;
        string connectionString;
        List<SqlDataAdapter> mainAdapter = new List<SqlDataAdapter>();
        //Server serverInfo;
        public string GenerateConnectionString(Server serverInfo)
        {
            connectionString = "Data Source=" + serverInfo.ipProperty + "," + serverInfo.portProperty + ";Network Library=DBMSSOCN;Initial Catalog=master;User ID=" + serverInfo.userNameProperty + ";Password=" + serverInfo.passwordProperty + ";";
            return connectionString;
        }

        public string GenerateConnectionString(Server serverInfo, string databaseName)
        {
            connectionString = "Data Source=" + serverInfo.ipProperty + "," + serverInfo.portProperty + ";Network Library=DBMSSOCN;Initial Catalog="+databaseName+";User ID=" + serverInfo.userNameProperty + ";Password=" + serverInfo.passwordProperty + ";";
            return connectionString;
        }

        public bool ConnectToServer(string connectionString)
        {
            sqlConn = new SqlConnection(connectionString);
            try
            {
                sqlConn.Open();
                return true;
            }
            catch(Exception exc)
            {
                return false;
            }
        }

        public DataTable InitialGetServerStructure()
        {
            DataTable tbl = SelectAllFromTable("Select name FROM master..sysdatabases");
            return tbl;
        }

        public DataTable SelectAllFromTable(string sqlCommand)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            
            SqlDataAdapter tableAdapter = new SqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            sqlConn.Close();
            return dataTable;
        }

        public List<string> GetTablesInDatabase(string databaseName)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT TABLE_NAME FROM information_schema.tables order by TABLE_NAME;";
            SqlDataAdapter tableAdapter = new SqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            List<string> lst = new List<string>();
            for(int i=0;i<dataTable.Rows.Count; i++)
                lst.Add(dataTable.Rows[i].ItemArray[0].ToString());
            //lst.Sort();
            sqlConn.Close();
            return lst;
        }

        public bool IsDatabaseDocumented(string databaseName)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT TABLE_NAME FROM information_schema.tables order by TABLE_NAME;";
            SqlDataAdapter tableAdapter = new SqlDataAdapter(sqlCommand, sqlConn);
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
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT column_name FROM information_schema.columns where table_name = '"+tableName+"';";
            SqlDataAdapter tableAdapter = new SqlDataAdapter(sqlCommand, sqlConn);
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
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            DataTable dataTable = new DataTable();
            string sqlCommand = "SELECT column_default, is_nullable, data_type, character_maximum_length  FROM information_schema.columns where table_name = '" + tableName + "' and column_name = '"+column+"';";
            SqlDataAdapter tableAdapter = new SqlDataAdapter(sqlCommand, sqlConn);
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
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            SqlCommand command = new SqlCommand(query, sqlConn);
            command.ExecuteNonQuery();
            sqlConn.Close();
        }

        public List<DataTable> ExecuteQuery(List<string> sqlCommand, List<string> tableName)
        {
            mainAdapter.Clear();//!!!
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            List<DataTable> mainDataset = new List<DataTable>();
            //string command = "";
            for (int i = 0; i < sqlCommand.Count;i++ )
            {
                //command += sqlCommand[i];
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand[i], sqlConn);
                mainAdapter.Add(adapter);
                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(mainAdapter[i]);
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
            sqlConn = new SqlConnection(connectionString);
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
            ExecuteNonQuery("CREATE TABLE [dbo].[UDBComments](" +
    "[id] [int] IDENTITY(1,1) NOT NULL," +
    "[commentEng] [text] NULL," +
    "[commentUkr] [text] NULL," +
    "[commentRus] [text] NULL," +
" CONSTRAINT [PK_UDBComments] PRIMARY KEY CLUSTERED " +
"(" +
    "[id] ASC" +
")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

            ExecuteNonQuery("CREATE TABLE [dbo].[UDBTables](" +
    "[id] [int] IDENTITY(1,1) NOT NULL," +
    "[name] [nvarchar](50) NULL," +
    "[commentId] [int] NULL," +
" CONSTRAINT [PK_UDBTables] PRIMARY KEY CLUSTERED " +
"(" +
    "[id] ASC" +
")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
") ON [PRIMARY]");
            ExecuteNonQuery("ALTER TABLE [dbo].[UDBTables]  WITH CHECK ADD  CONSTRAINT [FK_UDBTables_UDBComments] FOREIGN KEY([commentId])" +
"REFERENCES [dbo].[UDBComments] ([id])");
            ExecuteNonQuery("ALTER TABLE [dbo].[UDBTables] CHECK CONSTRAINT [FK_UDBTables_UDBComments]");

            ExecuteNonQuery("CREATE TABLE [dbo].[UDBFields](" +
    "[id] [int] IDENTITY(1,1) NOT NULL," +
    "[name] [nvarchar](50) NOT NULL," +
    "[type] [nvarchar](50) NOT NULL," +
    "[isnull] [bit] NOT NULL," +
    "[defaultvalue] [nvarchar](50) NULL," +
    "[tableId] [int] NULL," +
    "[commentId] [int] NULL," +
" CONSTRAINT [PK_UDBFields] PRIMARY KEY CLUSTERED " +
"(" +
    "[id] ASC" +
")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
") ON [PRIMARY]");
            ExecuteNonQuery("ALTER TABLE [dbo].[UDBFields]  WITH CHECK ADD  CONSTRAINT [FK_UDBFields_UDBComments] FOREIGN KEY([commentId])" +
"REFERENCES [dbo].[UDBComments] ([id])");
            ExecuteNonQuery("ALTER TABLE [dbo].[UDBFields] CHECK CONSTRAINT [FK_UDBFields_UDBComments]");
            ExecuteNonQuery("ALTER TABLE [dbo].[UDBFields]  WITH CHECK ADD  CONSTRAINT [FK_UDBFields_UDBTables] FOREIGN KEY([tableId])" +
"REFERENCES [dbo].[UDBTables] ([id])");
            ExecuteNonQuery("ALTER TABLE [dbo].[UDBFields] CHECK CONSTRAINT [FK_UDBFields_UDBTables]");
        }
    }
}
