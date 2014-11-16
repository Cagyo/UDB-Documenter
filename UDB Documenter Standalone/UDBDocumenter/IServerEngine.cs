using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace UDBDocumenter
{
    public interface IServerEngine
    {
        string GenerateConnectionString(Server serverInfo);
        bool ConnectToServer(string connectionString);
        DataTable InitialGetServerStructure();
        DataTable SelectAllFromTable(string sqlCommand);
        List<string> GetTablesInDatabase(string databaseName);
        string GenerateConnectionString(Server serverInfo, string databaseName);
        List<string> GetColumnsInTable(string tableName);
        List<string> GetColumnInTable(string tableName, string column);
        void ExecuteNonQuery(string query);
        void CreateDocumentationStructure();
        List<DataTable> ExecuteQuery(List<string> sqlCommand, List<string> tableName);
        void ExecuteQuery(List<DataTable> dst);
        bool IsDatabaseDocumented(string databaseName);
        //connect
    }
}
