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
    public partial class FieldDocView : UserControl
    {
        Field fld;
        TableDoc tableForm;

        public FieldDocView(Field fldInfo)
        {
            InitializeComponent();
            //tabControl1.TabPages.RemoveAt(0);
            //tabControl1.TabPages.RemoveAt(2);
            fld = fldInfo;
            labelName.Text = fld.ToString();
            labelType.Text = fld.type;
            labelNull.Text = fld.isNull.ToString();
            labelDefault.Text = fld.defaultValue;

            List<string> comments = fldInfo.GetComments();

            textBoxEng.Text = comments[0];
            textBoxUkr.Text = comments[1];
            textBoxRus.Text = comments[2];
        }
    }
}
