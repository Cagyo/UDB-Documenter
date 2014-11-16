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
    public partial class ChangeLanguagesForm : Form
    {
        MainForm mainForm;
        public ChangeLanguagesForm(MainForm mainForm)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            this.mainForm = mainForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.selectedIndex = comboBox1.SelectedIndex;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
