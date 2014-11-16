using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UDBDocumenter
{
    public partial class TableDoc : UserControl
    {
        MainForm f;
        Table tbl;
        Field fld;
        int selectedIndex;
        public TableDoc(MainForm f, Table tbl, int selectedIndex)
        {
            InitializeComponent();
            this.f = f;
            this.tbl = tbl;
            List<string> lst = tbl.GetFields();
            this.selectedIndex = selectedIndex;
            tabControl1.SelectedIndex = selectedIndex;
            comboBox1.Items.Clear();
            foreach (string str in lst)
            {
                comboBox1.Items.Add(str);
            }
            labelName.Text = tbl.ToString();
            //List<string> newComments = new List<string>();
            List<string> comments = tbl.GetComments();
            
            textBoxEng.Text = comments[0];
            textBoxUkr.Text = comments[1];
            textBoxRus.Text = comments[2];
        }

        public List<string> GetComments()
        {
            List<string> comments = new List<string>();
            comments.Add(textBoxEng.Text);
            comments.Add(textBoxUkr.Text);
            comments.Add(textBoxRus.Text);
            return comments;
        }

        public void SaveChangesField()
        {
            //Field tbl = (Table)treeView1.SelectedNode.Tag;
            List<string> comments = new List<string>();
            FieldDoc fldDoc = new FieldDoc(fld, this,selectedIndex);
            foreach (Control ctrl in Controls)
            {
                if (ctrl.GetType() == typeof(FieldDoc))
                    fldDoc = (FieldDoc)ctrl;
            }
            List<string> commentsForm = fldDoc.GetComments();
            foreach (string comment in commentsForm)
                comments.Add(comment);
            fld.addComments(comments);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f.SaveChangesTable();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i=0;
            foreach (Control ctrl in Controls)
            {
                if (ctrl.GetType() == typeof(FieldDoc))
                    Controls.RemoveAt(i);
                i++;
            }
            fld = tbl.GetField(comboBox1.SelectedIndex);
            FieldDoc fldDoc = new FieldDoc(fld, this,selectedIndex);
            fldDoc.Location = new Point(15, 169);
            this.Controls.Add(fldDoc);
            
        }
    }
}
