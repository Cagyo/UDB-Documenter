using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Novacode;

namespace UDBDocumenter
{
    public class Table
    {
        int id;
        //int commentsId;
        List<Field> fields;
        string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        Comment comment;
        Server serverInfo;

        public Table(string tableName, Server serverInfo)
        {
            fields = new List<Field>();
            _name = tableName;
            //comment = new Comment();
            this.serverInfo = serverInfo;
        }

        public Table(DataRow row, Server serverInfo, DataTable commentsTable, DataTable fieldsTable)
        {
            fields = new List<Field>();
            _name = row[1].ToString();
            //comment = new Comment();
            this.serverInfo = serverInfo;
            this.id = Convert.ToInt32(row[0]);
            int commentId = Convert.ToInt32(row[2]);
            DataRow[] row1 = commentsTable.Select("id = " + commentId);
            List<string> lst = new List<string>();
            lst.Add(row1[0][1].ToString());
            lst.Add(row1[0][2].ToString());
            lst.Add(row1[0][3].ToString());
            comment = new Comment(commentId, lst);
            DataRow[] rows = fieldsTable.Select("tableId = " + id);
            foreach (DataRow fieldRow in rows)
            {
                fields.Add(new Field(fieldRow, serverInfo, commentsTable));
            }
        }

        public int GetCommentsID()
        {
            return comment.GetCommentsId();
        }

        public void addComments(List<string> lst)
        {
            comment.AddComments(lst);
        }

        public DataTable SetCommentInDataTable(DataTable tbl)
        {
            DataRow[] row = tbl.Select("id = "+ comment.GetCommentsId());
            List<string> comments = comment.GetComments();
            row[0].SetField(1, comments[0]);
            row[0].SetField(2, comments[1]);
            row[0].SetField(3, comments[2]);
            foreach (Field fld in fields)
                fld.SetCommentInDataTable(tbl);
            return tbl;
        }

        public void Process()
        {
            List<string> fields = serverInfo.GetColumnsInTable(_name);
            foreach (string fieldName in fields)
            {
                Field fieldDefinition = new Field(fieldName, _name, serverInfo);
                fieldDefinition.Process();
                this.fields.Add(fieldDefinition);
            }
        }

        public void SetComment(List<string> comments)
        {
            comment.SetComments(comments);
        }

        public List<string> GetComments()
        {
            return comment.GetComments();
        }

        public override string ToString()
        {
            return _name;
        }

        public Field GetField(int index)
        {
            return fields[index];
        }

        public List<string> GetFields()
        {
            List<string> cols = new List<string>();
            foreach (Field fld in fields)
            {
                cols.Add(fld.ToString());
            }
            return cols;
        }

        public int CountElements()
        {
            return fields.Count;
        }

        public void SetComment(int commentId)
        {
            comment = new Comment(commentId);
        }

        public void SetId(int tableId)
        {
            id = tableId;
        }

        public DataTable AddToDataTable(DataTable tbl)//, out int commentCount
        {
            DataRow row = tbl.NewRow();
            row.SetField(0, 1);
            row.SetField(1, name);
            row.SetField(2, comment.GetCommentsId());
            tbl.Rows.Add(row);
            return tbl;
        }

        public DataTable AddFieldsToDataTable(DataTable tbl, List<int> commentIds, ref int currentCommentIdIndex)
        {
            int i = 0;
            foreach (Field fld in fields)
            {
                fld.SetComment(commentIds[currentCommentIdIndex+i]);
                fld.SetTable(id);
                fld.AddToDataTable(tbl, i+1);
                i++;
            }
            currentCommentIdIndex += i;
            return tbl;
        }

        public void ToReport(string filePath, ref DocX document, int language)
        {
            List<string> commentString = new List<string>();
            commentString.Add("Comment: ");
            commentString.Add("Коментар: ");
            commentString.Add("Комментарий: ");

            List<string> comments = comment.GetComments();
            document.InsertParagraph(commentString[language] + comments[language] + "\r\n");
            Novacode.Table table = document.AddTable(fields.Count, 5);
            table.Alignment = Alignment.center;
            table.Design = TableDesign.LightShadingAccent2;
            Novacode.Table tbl = document.InsertTable(table);

            if (language == 0)
            {
                tbl.Rows[0].Cells[0].Paragraphs.First().Append("Name").FontSize(12);
                tbl.Rows[0].Cells[1].Paragraphs.First().Append("Type");
                tbl.Rows[0].Cells[2].Paragraphs.First().Append("Is Null");
                tbl.Rows[0].Cells[3].Paragraphs.First().Append("Default value");
                tbl.Rows[0].Cells[4].Paragraphs.First().Append("Comment");
            }
            else if (language == 1)
            {
                tbl.Rows[0].Cells[0].Paragraphs.First().Append("Назва");
                tbl.Rows[0].Cells[1].Paragraphs.First().Append("Тип");
                tbl.Rows[0].Cells[2].Paragraphs.First().Append("Пусті значення");
                tbl.Rows[0].Cells[3].Paragraphs.First().Append("Значення по замовченню");
                tbl.Rows[0].Cells[4].Paragraphs.First().Append("Коментар");
            }
            else
            {
                tbl.Rows[0].Cells[0].Paragraphs.First().Append("Название");
                tbl.Rows[0].Cells[1].Paragraphs.First().Append("Тип");
                tbl.Rows[0].Cells[2].Paragraphs.First().Append("Пустые значения");
                tbl.Rows[0].Cells[3].Paragraphs.First().Append("Значение по умолчанию");
                tbl.Rows[0].Cells[4].Paragraphs.First().Append("Комментарий");
            }
            for (int i = 1; i < fields.Count; i++)
                fields[i].ToReport(filePath, ref tbl, i, language);
            document.InsertParagraph("\r\n");
        }
    }
}
