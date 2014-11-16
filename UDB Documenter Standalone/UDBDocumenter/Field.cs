using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Novacode;

namespace UDBDocumenter
{
    public class Field
    {
        int id;
        //int commentsId;
        int tableId;
        string name;
        string _type;
        public string type
        {
            get { return _type; }
        }
        //bool primaryKey;
        //bool index;
        bool _isNull;
        public bool isNull
        {
            get { return _isNull; }
        }
        string _defaultValue;
        public string defaultValue
        {
            get { return _defaultValue; }
        }
        Server serverInfo;
        string tableName;
        Comment comment;

        public Field(string fieldName, string tableName, Server serverInfo)
        {
            name = fieldName;
            this.tableName = tableName;
            this.serverInfo = serverInfo;
        }

        public Field(DataRow row, Server serverInfo, DataTable commentsTable)
        {
            id = Convert.ToInt32(row[0]);
            name = row[1].ToString();
            _type = row[2].ToString();
            _isNull = Convert.ToBoolean(row[3]);
            _defaultValue = row[4].ToString();
            tableId = Convert.ToInt32(row[5]);

            int commentId = Convert.ToInt32(row[6]);
            DataRow[] row1 = commentsTable.Select("id = " + commentId);
            List<string> lst = new List<string>();
            lst.Add(row1[0][1].ToString());
            lst.Add(row1[0][2].ToString());
            lst.Add(row1[0][3].ToString());
            comment = new Comment(Convert.ToInt32(row[6]), lst);
            //this.tableName = tableName;
            this.serverInfo = serverInfo;
        }

        public int GetCommentsID()
        {
            return comment.GetCommentsId();
        }

        public List<string> GetComments()
        {
            return comment.GetComments();
        }

        public void addComments(List<string> lst)
        {
            comment.AddComments(lst);
        }

        public void SetComment(int commentId)
        {
            comment = new Comment(commentId);
        }

        public DataTable SetCommentInDataTable(DataTable tbl)
        {
            DataRow[] row = tbl.Select("id = " + comment.GetCommentsId());
            List<string> comments = comment.GetComments();
            row[0].SetField(1, comments[0]);
            row[0].SetField(2, comments[1]);
            row[0].SetField(3, comments[2]);
            return tbl;
        }

        public void SetTable(int tableId)
        {
            this.tableId = tableId;
        }

        public DataTable AddToDataTable(DataTable tbl, int id)//, out int commentCount
        {
            DataRow row = tbl.NewRow();
            row.SetField(0, id);
            row.SetField(1, name);
            row.SetField(2, _type);
            row.SetField(3, _isNull);
            row.SetField(4, _defaultValue);
            row.SetField(5, tableId);
            row.SetField(6, comment.GetCommentsId());
            tbl.Rows.Add(row);
            return tbl;
        }

        /*public DataTable ToDataTable(DataTable tbl) //, out int commentCount
        {
            DataRow row = tbl.NewRow();
            row.SetField(0, 1);
            row.SetField(1, name);
            row.SetField(2, type);
            row.SetField(3, isNull);
            row.SetField(4, defaultValue);
            row.SetField(5, tableId);
            return tbl;
        }*/

        public void Process()
        {
            List<string> options = serverInfo.GetColumnInTable(tableName, name);
            _defaultValue = options[0];
            if (options[1] == "YES")
                _isNull = true;
            else _isNull = false;
            _type = options[2];
            if (options[3] != "")
                _type += "(" + options[3] + ")";
        }

        public override string ToString()
        {
            return name;
        }

        public void ToReport(string filePath, ref Novacode.Table tbl, int i, int language)
        {
            
            List<string> comments = comment.GetComments();
            tbl.Rows[i].Cells[0].Paragraphs.First().Append(name);
            tbl.Rows[i].Cells[1].Paragraphs.First().Append(_type);
            tbl.Rows[i].Cells[2].Paragraphs.First().Append(_isNull.ToString());
            tbl.Rows[i].Cells[3].Paragraphs.First().Append(_defaultValue);
            tbl.Rows[i].Cells[4].Paragraphs.First().Append(comments[language]);

        }
    }
}
