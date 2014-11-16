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
        SqlDataAdapter adapterForEdit;
        //Server serverInfo;
        public string GenerateConnectionString(Server serverInfo)
        {
            connectionString = @"Data Source=127.0.0.1;Initial Catalog=master;Integrated Security=SSPI;";
            return connectionString;
        }

        public string GenerateConnectionString(Server serverInfo, string databaseName)
        {
            connectionString = @"Data Source=127.0.0.1;Initial Catalog=D:\ЗАЭС\DATABASE\HIDROHIMSQL.MDF;Integrated Security=SSPI;";
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
            string sqlCommand = "SELECT TABLE_NAME FROM information_schema.tables;";
            SqlDataAdapter tableAdapter = new SqlDataAdapter(sqlCommand, sqlConn);
            tableAdapter.Fill(dataTable);
            //mainAdapter.Fill(dst, tableName);
            List<string> lst = new List<string>();
            for(int i=0;i<dataTable.Rows.Count; i++)
                lst.Add(dataTable.Rows[i].ItemArray[0].ToString());
            sqlConn.Close();
            return lst;
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

        public DataTable ExecuteQuery(string sqlCommand)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            string[] command = sqlCommand.Split(' ');
            DataTable mainDataset = new DataTable();

            adapterForEdit = new SqlDataAdapter(sqlCommand, sqlConn);

            

            DataTable dataTable = new DataTable();

            adapterForEdit.Fill(dataTable);
            return dataTable;
        }

        public void ExecuteQuery(DataTable dst, string tableName)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(adapterForEdit);
            adapterForEdit.InsertCommand = objCommandBuilder.GetInsertCommand();
            int i = 0;
            string uCommand = "update " + tableName + " set ";
            foreach (DataColumn clmn in dst.Columns)
            {
                    if (i == dst.Columns.Count - 1)
                        uCommand += "[" + clmn.ColumnName + "]"+" = @p3 ";
                i++;
            }
            uCommand += "where ";
            i = 0;
            string dCommand = "delete from " + tableName + " where ";
            foreach (DataColumn clmn in dst.Columns)
            {
                if (i == 0 || i==1)
                {
                    uCommand += "[" + clmn.ColumnName + "] = @p" + i + " and ";
                    dCommand += "[" + clmn.ColumnName + "] = @p" + i + " and ";
                }
                else if (i==2)
                {
                    uCommand += "[" + clmn.ColumnName + "] = @p" + i + "";
                    dCommand += "[" + clmn.ColumnName + "] = @p" + i + "";
                }
                i++;
            }

            SqlCommand updateCommand = new SqlCommand(uCommand, sqlConn);
            SqlCommand deleteCommand = new SqlCommand(dCommand, sqlConn);

            i = 0;
            foreach (DataColumn clmn in dst.Columns)
            {
                    updateCommand.Parameters.Add("@p" + i, SqlDbTypeConvertor.ToSqlDbType(clmn.DataType));
                        updateCommand.Parameters[i].SourceColumn = clmn.ColumnName;

                        deleteCommand.Parameters.Add("@p" + i, SqlDbTypeConvertor.ToSqlDbType(clmn.DataType));
                        deleteCommand.Parameters[i].SourceColumn = clmn.ColumnName;
                i++;
            }
            adapterForEdit.UpdateCommand = updateCommand;
            adapterForEdit.DeleteCommand = deleteCommand;
            
            int res = adapterForEdit.Update(dst);

            sqlConn.Close();
        }

        public void ExecuteQuery(DataTable dst, string tableName, string columnName)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(adapterForEdit);
            adapterForEdit.InsertCommand = objCommandBuilder.GetInsertCommand();
            int i = 0;
            string uCommand="update "+tableName+" set ";
            string dCommand = "delete from " + tableName + " where "+columnName+" = @p1";
            
            foreach (DataColumn clmn in dst.Columns)
            {
                if (clmn.ColumnName != columnName)
                {
                    if (i != dst.Columns.Count - 1)
                        uCommand += "["+clmn.ColumnName + "] = @p" + i + ", ";
                    else
                        uCommand += "["+clmn.ColumnName + "] = @p" + i + " ";
                }
                i++;
            }
            if (tableName == "tblAddShifrSkvVar")
                uCommand = uCommand.Remove(uCommand.Count() - 2,1);
            uCommand += "where " + columnName + " = @p" + i;

            SqlCommand updateCommand = new SqlCommand(uCommand, sqlConn);
            SqlCommand deleteCommand = new SqlCommand(dCommand, sqlConn);
            
            
            i = 0;
            Type type = typeof(Int32);
            foreach (DataColumn clmn in dst.Columns)
            {
                if (clmn.ColumnName != columnName)
                {
                    updateCommand.Parameters.Add("@p" + i, SqlDbTypeConvertor.ToSqlDbType(clmn.DataType));
                    //if (clmn.ColumnName == columnName)
                    if (tableName == "tblAddShifrSkvVar")
                        updateCommand.Parameters[i].SourceColumn = clmn.ColumnName;
                    else 
                        updateCommand.Parameters[i-1].SourceColumn = clmn.ColumnName;
                }
                else
                    type = clmn.DataType;
                
                i++;
                //sqlParameter.SqlDbType = clmn.DataType;
            }
            deleteCommand.Parameters.Add("@p1", SqlDbTypeConvertor.ToSqlDbType(type), 55, columnName);
            updateCommand.Parameters.Add("@p" + i, SqlDbTypeConvertor.ToSqlDbType(type), 55, columnName);
            adapterForEdit.UpdateCommand = updateCommand;
            adapterForEdit.DeleteCommand = deleteCommand;
            //adapterForEdit.UpdateCommand = objCommandBuilder.GetUpdateCommand();
            //adapterForEdit.DeleteCommand = objCommandBuilder.GetDeleteCommand(); //CommandText = "DELETE FROM  WHERE (([id] = @p1))"

                int res = adapterForEdit.Update(dst);

            sqlConn.Close();
            //return mainDataset;
        }

        public void ExecuteQuery(DataTable dst, string tableName, string columnName1, string columnName2)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(adapterForEdit);
            adapterForEdit.InsertCommand = objCommandBuilder.GetInsertCommand();
            int i = 0;
            string uCommand = "update " + tableName + " set ";
            string dCommand = "delete from " + tableName + " where " + columnName1 + " = @p1 and " +columnName2 + " = @p2";

            foreach (DataColumn clmn in dst.Columns)
            {
                if (clmn.ColumnName != columnName1 && clmn.ColumnName != columnName2)
                {
                    if (i != dst.Columns.Count - 1)
                        uCommand += "[" + clmn.ColumnName + "] = @p" + i + ", ";
                    else
                        uCommand += "[" + clmn.ColumnName + "] = @p" + i + " ";
                    i++;
                }
                
            }
            //if (tableName == "tblAddShifrSkvVar")
                uCommand = uCommand.Remove(uCommand.Count() - 2, 1);
            uCommand += "where " + columnName1 + " = @p" + i + " and " + columnName2 + " = @p" + (i+1);

            SqlCommand updateCommand = new SqlCommand(uCommand, sqlConn);
            SqlCommand deleteCommand = new SqlCommand(dCommand, sqlConn);


            i = 0;
            Type type1 = typeof(Int32);
            Type type2 = typeof(Int32);
            foreach (DataColumn clmn in dst.Columns)
            {
                if (clmn.ColumnName != columnName1 && clmn.ColumnName != columnName2)
                {
                    updateCommand.Parameters.Add("@p" + i, SqlDbTypeConvertor.ToSqlDbType(clmn.DataType));
                    //if (clmn.ColumnName == columnName)
                    updateCommand.Parameters[i].SourceColumn = clmn.ColumnName;
                    i++;
                }
                else if (clmn.ColumnName == columnName1)
                    type1 = clmn.DataType;
                else 
                    type2 = clmn.DataType;

                
                //sqlParameter.SqlDbType = clmn.DataType;
            }
            deleteCommand.Parameters.Add("@p1", SqlDbTypeConvertor.ToSqlDbType(type1), 55, columnName1);
            deleteCommand.Parameters.Add("@p2", SqlDbTypeConvertor.ToSqlDbType(type2), 55, columnName2);
            updateCommand.Parameters.Add("@p" + i, SqlDbTypeConvertor.ToSqlDbType(type1), 55, columnName1);
            updateCommand.Parameters.Add("@p" + (i+1), SqlDbTypeConvertor.ToSqlDbType(type2), 55, columnName2);
            adapterForEdit.UpdateCommand = updateCommand;
            adapterForEdit.DeleteCommand = deleteCommand;
            //adapterForEdit.UpdateCommand = objCommandBuilder.GetUpdateCommand();
            //adapterForEdit.DeleteCommand = objCommandBuilder.GetDeleteCommand(); //CommandText = "DELETE FROM  WHERE (([id] = @p1))"

            int res = adapterForEdit.Update(dst);

            sqlConn.Close();
            //return mainDataset;
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
