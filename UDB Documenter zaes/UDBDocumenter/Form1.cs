using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UDBDocumenter
{
    public partial class Form1 : Form
    {
        List<Server> serverInfoLst;
        public Form1()
        {
            InitializeComponent();
            serverInfoLst = new List<Server>();
        }

        public Form1(Server serverInfo)
        {
            InitializeComponent();
            serverInfoLst = new List<Server>();
            serverInfoLst.Add(serverInfo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddServerForm addServerForm = new AddServerForm(this);
            addServerForm.ShowDialog();

        }

        public void AddServer(Server serverinfo)
        {
            serverInfoLst.Add(serverinfo);
        }

        public void AddServerToTree(string serverName)
        {
            treeView1.Nodes[0].Nodes.Add(serverName);
        }

        public void SortTree()
        {
            treeView1.Sort();
        }

        public void AddDatabasesToTree(string databaseName, int server)
        {
            treeView1.Nodes[0].Nodes[server].Nodes.Add(databaseName);
        }

        public void AddTableToTree(string tableName, int databaseName, int server, Table tbl)
        {
            TreeNode node = treeView1.Nodes[0].Nodes[server].Nodes[databaseName].Nodes.Add(tableName);
            node.Tag = tbl;
        }

        public void ClearTree()
        {
            treeView1.Nodes[0].Nodes[0].Nodes.Clear();
        }

        public void AddColumnToTree(string columnName, int table, int database, int server)
        {
            treeView1.Nodes[0].Nodes[server].Nodes[database].Nodes[table].Nodes.Add(columnName);
        }

        public int FindServerInTree(string serverName) //?
        {
            TreeNode[] nodes = treeView1.Nodes[0].Nodes.Find(serverName,true);
            return nodes[0].Index;
        }

        public void SaveChangesTable()
        {
            Table tbl = (Table)treeView1.SelectedNode.Tag;
            List<string> comments = new List<string>();
            TableDoc tdf = (TableDoc)tabControl1.TabPages[1].Controls[0];
            List<string> commentsForm = tdf.GetComments();
            foreach (string comment in commentsForm)
                comments.Add(comment);
            tbl.addComments(comments);
        }

        public void AddFieldView(FieldDocView fdv)
        {
            flowLayoutPanel1.Controls.Add(fdv);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tabControl1.TabPages[1].Controls.Clear();
            flowLayoutPanel1.Controls.Clear();
            if (e.Node.Level == 3)
            {
                Table tbl = (Table)e.Node.Tag;
                TableDoc tdc = new TableDoc(this, tbl);
                //tdc.Show();
                TableDocView tdv = new TableDocView(this, tbl);
                flowLayoutPanel1.Controls.Add(tdv);
                List<FieldDocView> lst = tdv.GetFieldDocViewList();
                foreach(FieldDocView fldDV in lst)
                    flowLayoutPanel1.Controls.Add(fldDV);
                tabControl1.TabPages[1].Controls.Add(tdc);//данные из тега
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serverInfoLst[0].SaveChanges();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeLanguagesForm chLangForm = new ChangeLanguagesForm();
            chLangForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for(int i=0;i<3;i++)
                serverInfoLst[0].ToReport("D:\\ReportExample"+i+".docx", i);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverInfoLst[0].SaveChanges();
        }

        private void генерацияОтчетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportGenForm rGF = new ReportGenForm();
            if (rGF.ShowDialog() == DialogResult.OK)
            {
                string fileName = rGF.GetFileName();
                List<bool> languages = rGF.GetLanguages();
                string[] file = fileName.Split('.');
                for (int i = 0; i < 3; i++)
                {
                    if (languages[i] == true)
                        switch (i)
                        {
                            case 0: serverInfoLst[0].ToReport(file[0] + "_eng." + file[1], i); break;
                            case 1: serverInfoLst[0].ToReport(file[0] + "_ua." + file[1], i); break;
                            case 2: serverInfoLst[0].ToReport(file[0] + "_rus." + file[1], i); break;
                        }
                }
            }
        }
    }
}
