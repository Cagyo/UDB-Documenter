using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDBDocumenter
{
    class TableCommands
    {
        string tableName;
        string fieldName;

        public TableCommands(string table, string field)
        {
            tableName = table;
            fieldName = field;
        }

        public string GetFieldName()
        {
            return fieldName;
        }

        override public string ToString()
        {
            return tableName;
        }
    }
}
