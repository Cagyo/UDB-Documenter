using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace UDBDocumenter
{
    class DatabaseProcessor
    {
        Server serverInfo;

        public DatabaseProcessor(Server serverInfo, string databaseName)
        {
            this.serverInfo = serverInfo;
        }

        public List<string> ProcessDatabase()
        {
            List<string> lst = ProcessTables();
            return lst;
        }

        public List<string> ProcessTables()
        {
            DataTable tbl = serverInfo.SelectAllFromTable("SELECT Distinct * FROM information_schema.TABLES");
            List<String> lst = new List<string>();
            foreach (DataRow row in tbl.Rows)
                lst.Add(row[0].ToString());
            return lst;
        }
    }
}
