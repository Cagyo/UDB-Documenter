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
    public partial class FieldDoc : UserControl
    {
        Field fld;
        TableDoc tableForm;

        public FieldDoc(Field fldInfo, TableDoc docForm, int selectedIndex)
        {
            InitializeComponent();
            fld = fldInfo;
            labelName.Text = fld.ToString();
            labelType.Text = fld.type;
            labelNull.Text = fld.isNull.ToString();
            labelDefault.Text = fld.defaultValue;
            tableForm = docForm;

            List<string> comments = fldInfo.GetComments();

            textBoxEng.Text = comments[0];
            textBoxUkr.Text = comments[1];
            textBoxRus.Text = comments[2];
            tabControl1.SelectedIndex = selectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tableForm.SaveChangesField();
        }

        public List<string> GetComments()
        {
            List<string> comments = new List<string>();
            comments.Add(textBoxEng.Text);
            comments.Add(textBoxUkr.Text);
            comments.Add(textBoxRus.Text);
            return comments;
        }


    }
}
