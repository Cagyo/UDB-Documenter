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
    public partial class ReportGenForm : Form
    {
        int currentReport;
        string fileName = "";
        public ReportGenForm()
        {
            InitializeComponent();
            saveFileDialog1.DefaultExt = ".docx";
        }

        public string GetFileName()
        {
            return fileName;
        }

        public List<bool> GetLanguages()
        {
            List<bool> checkBoxes = new List<bool>();
            checkBoxes.Add(checkBox1.Checked);
            checkBoxes.Add(checkBox2.Checked);
            checkBoxes.Add(checkBox3.Checked);
            return checkBoxes;
        }

        public int GetReportType()
        {
            return currentReport;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                saveFileDialog1.DefaultExt = ".docx";
                currentReport = 0;
                string[] file = fileName.Split('.');
                fileName = file[0] + ".docx";
                textBox1.Text = fileName;
            }
            else if (radioButton2.Checked == true)
            {
                saveFileDialog1.DefaultExt = ".pdf";
                currentReport = 1;
                string[] file = fileName.Split('.');
                fileName = file[0] + ".pdf";
                textBox1.Text = fileName;
            }
            
        }

        private void ReportGenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                fileName = saveFileDialog1.FileName;
                textBox1.Text = fileName;
            }
            else
                MessageBox.Show("Выберите файл!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
