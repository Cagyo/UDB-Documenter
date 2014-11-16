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
    public partial class TableDocView : UserControl
    {
        MainForm f;
        Table tbl;
        List<FieldDocView> fdvLst;
        int selectedIndex;

        public TableDocView(MainForm f, Table tbl, int selectedIndex)
        {
            InitializeComponent();
            this.f = f;
            this.tbl = tbl;
            tabControl1.SelectedIndex = selectedIndex;
            fdvLst = new List<FieldDocView>();
            List<string> lst = tbl.GetFields();
            this.selectedIndex = selectedIndex;
            
            labelName.Text = tbl.ToString();
            //List<string> newComments = new List<string>();
            List<string> comments = tbl.GetComments();
            
            textBoxEng.Text = comments[0];
            textBoxUkr.Text = comments[1];
            textBoxRus.Text = comments[2];
            for (int i = 0; i < lst.Count; i++)
            {
                Field fld = this.tbl.GetField(i);
                FieldDocView fdv = new FieldDocView(fld,selectedIndex);
                fdvLst.Add(fdv);
                //f.AddFieldView(fdv);
            }
        }

        public List<FieldDocView> GetFieldDocViewList()
        {
            return fdvLst;
        }

    }
}
