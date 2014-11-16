using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace UDBDocumenter
{
    public partial class MainForm : Form
    {
        List<Server> serverInfoLst;
        ReportGenForm rGF;
        public int selectedIndex = 0;
        public MainForm()
        {
            InitializeComponent();
            serverInfoLst = new List<Server>();
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

        public void ClearServers()
        {
            treeView1.Nodes[0].Nodes.Clear();
        }

        public void SortTree()
        {
            treeView1.Sort();
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
                TableDoc tdc = new TableDoc(this, tbl, selectedIndex);
                //tdc.Show();
                TableDocView tdv = new TableDocView(this, tbl, selectedIndex);
                flowLayoutPanel1.Controls.Add(tdv);
                List<FieldDocView> lst = tdv.GetFieldDocViewList();
                foreach(FieldDocView fldDV in lst)
                    flowLayoutPanel1.Controls.Add(fldDV);
                tabControl1.TabPages[1].Controls.Add(tdc);//данные из тега
            }
            if (e.Node.Level == 4)
            {
                Table tbl = (Table)e.Node.Parent.Tag;
                FieldDocView fdv = new FieldDocView(tbl.GetField(e.Node.Text),selectedIndex);
                //List<FieldDocView> lst = tdv.GetFieldDocViewList();
                //foreach (FieldDocView fldDV in lst)
                    flowLayoutPanel1.Controls.Add(fdv);
                //tabControl1.TabPages[1].Controls.Add(tdc);//данные из тега
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serverInfoLst[0].SaveChanges();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeLanguagesForm chLangForm = new ChangeLanguagesForm(this);
            chLangForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for(int i=0;i<3;i++)
                serverInfoLst[0].ToReport("D:\\ReportExample"+i+".docx", i);
        }

        private void подключениеКСерверуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddServerForm addServerForm = new AddServerForm(this);
            addServerForm.ShowDialog();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverInfoLst[0].SaveChanges();
        }

        private void генерацияОтчетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rGF = new ReportGenForm();
            if (rGF.ShowDialog() == DialogResult.OK)
            {
                string fileName = rGF.GetFileName();
                List<bool> languages = rGF.GetLanguages();
                string[] file = fileName.Split('.');
                английскийToolStripMenuItem.Enabled = false;
                русскийToolStripMenuItem.Enabled = false;
                русскийToolStripMenuItem1.Enabled = false;
                for (int i = 0; i < 3; i++)
                {
                    if (languages[i] == true)
                    {
                        switch (i)
                        {
                            case 0: serverInfoLst[0].ToReport(file[0] + "_eng." + file[1], i); break;
                            case 1: serverInfoLst[0].ToReport(file[0] + "_ua." + file[1], i); break;
                            case 2: serverInfoLst[0].ToReport(file[0] + "_rus." + file[1], i); break;
                        }
                        if (i == 0)
                            английскийToolStripMenuItem.Enabled = true;
                        else if (i == 1)
                            русскийToolStripMenuItem.Enabled = true;
                        else
                            русскийToolStripMenuItem1.Enabled = true;
                    }
                }
            }
        }

        private void изменитьЯзыкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguagesForm chLangForm = new ChangeLanguagesForm(this);
            chLangForm.ShowDialog();
        }

        private void английскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var applicationWord = new Microsoft.Office.Interop.Word.Application();
            applicationWord.Visible = true;
            string fileName = rGF.GetFileName();
            List<bool> languages = rGF.GetLanguages();
            string[] file = fileName.Split('.');
            Document document = applicationWord.Documents.Open(file[0]+"_eng."+file[1]);
            
            //document.LoadFromFile("test.doct");
            //Application.Documents.Open(@"C:\Test\NewDocument.docx");
        }

        private void русскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var applicationWord = new Microsoft.Office.Interop.Word.Application();
            applicationWord.Visible = true;
            string fileName = rGF.GetFileName();
            List<bool> languages = rGF.GetLanguages();
            string[] file = fileName.Split('.');
            Document document = applicationWord.Documents.Open(file[0] + "_ua." + file[1]);
        }

        private void русскийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var applicationWord = new Microsoft.Office.Interop.Word.Application();
            applicationWord.Visible = true;
            string fileName = rGF.GetFileName();
            List<bool> languages = rGF.GetLanguages();
            string[] file = fileName.Split('.');
            Document document = applicationWord.Documents.Open(file[0] + "_rus." + file[1]);
        }
    }
}
