using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDBDocumenter
{
    class Comment
    {
        int id;
        List<string> data;

        public Comment(int id)
        {
            this.id = id;
            data = new List<string>();
        }

        public Comment(int id, List<string> commentData)
        {
            this.id = id;
            data = new List<string>();
            foreach (string comment in commentData)
                data.Add(comment);
        }

        public void AddComments(List<string> comments)
        {
            data.Clear();
            foreach(string comment in comments)
                data.Add(comment);
        }

        public List<string> GetComments()
        {
            return data;
        }

        public void SetComments(List<string> comments)
        {
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = comments[i];
            }
        }

        public int GetCommentsId()
        {
            return id;
        }
    }
}
