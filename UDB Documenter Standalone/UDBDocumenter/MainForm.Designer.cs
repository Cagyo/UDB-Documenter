namespace UDBDocumenter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Сервера");
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.серверToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подключениеКСерверуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.генерацияОтчетаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.языкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьЯзыкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просмотрОтчетаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.английскийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.русскийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.русскийToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(13, 31);
            this.treeView1.Name = "treeView1";
            treeNode6.Name = "Node0";
            treeNode6.Text = "Сервера";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.treeView1.Size = new System.Drawing.Size(229, 352);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(249, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(673, 352);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(665, 326);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Просмотр";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 7);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(652, 313);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(665, 326);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Редактирование";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.серверToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.отчетыToolStripMenuItem,
            this.языкиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(932, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // серверToolStripMenuItem
            // 
            this.серверToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.подключениеКСерверуToolStripMenuItem});
            this.серверToolStripMenuItem.Name = "серверToolStripMenuItem";
            this.серверToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.серверToolStripMenuItem.Text = "Сервер";
            // 
            // подключениеКСерверуToolStripMenuItem
            // 
            this.подключениеКСерверуToolStripMenuItem.Name = "подключениеКСерверуToolStripMenuItem";
            this.подключениеКСерверуToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.подключениеКСерверуToolStripMenuItem.Text = "Подключение к серверу";
            this.подключениеКСерверуToolStripMenuItem.Click += new System.EventHandler(this.подключениеКСерверуToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(206, 20);
            this.сохранитьToolStripMenuItem.Text = "Сохранить описание базы данных";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // отчетыToolStripMenuItem
            // 
            this.отчетыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.генерацияОтчетаToolStripMenuItem,
            this.просмотрОтчетаToolStripMenuItem});
            this.отчетыToolStripMenuItem.Name = "отчетыToolStripMenuItem";
            this.отчетыToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.отчетыToolStripMenuItem.Text = "Отчеты";
            // 
            // генерацияОтчетаToolStripMenuItem
            // 
            this.генерацияОтчетаToolStripMenuItem.Name = "генерацияОтчетаToolStripMenuItem";
            this.генерацияОтчетаToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.генерацияОтчетаToolStripMenuItem.Text = "Генерация отчета";
            this.генерацияОтчетаToolStripMenuItem.Click += new System.EventHandler(this.генерацияОтчетаToolStripMenuItem_Click);
            // 
            // языкиToolStripMenuItem
            // 
            this.языкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.изменитьЯзыкиToolStripMenuItem});
            this.языкиToolStripMenuItem.Name = "языкиToolStripMenuItem";
            this.языкиToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.языкиToolStripMenuItem.Text = "Языки";
            // 
            // изменитьЯзыкиToolStripMenuItem
            // 
            this.изменитьЯзыкиToolStripMenuItem.Name = "изменитьЯзыкиToolStripMenuItem";
            this.изменитьЯзыкиToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.изменитьЯзыкиToolStripMenuItem.Text = "Изменить языки";
            this.изменитьЯзыкиToolStripMenuItem.Click += new System.EventHandler(this.изменитьЯзыкиToolStripMenuItem_Click);
            // 
            // просмотрОтчетаToolStripMenuItem
            // 
            this.просмотрОтчетаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.английскийToolStripMenuItem,
            this.русскийToolStripMenuItem,
            this.русскийToolStripMenuItem1});
            this.просмотрОтчетаToolStripMenuItem.Name = "просмотрОтчетаToolStripMenuItem";
            this.просмотрОтчетаToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.просмотрОтчетаToolStripMenuItem.Text = "Просмотр отчета";
            // 
            // английскийToolStripMenuItem
            // 
            this.английскийToolStripMenuItem.Enabled = false;
            this.английскийToolStripMenuItem.Name = "английскийToolStripMenuItem";
            this.английскийToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.английскийToolStripMenuItem.Text = "Английский";
            this.английскийToolStripMenuItem.Click += new System.EventHandler(this.английскийToolStripMenuItem_Click);
            // 
            // русскийToolStripMenuItem
            // 
            this.русскийToolStripMenuItem.Enabled = false;
            this.русскийToolStripMenuItem.Name = "русскийToolStripMenuItem";
            this.русскийToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.русскийToolStripMenuItem.Text = "Украинский";
            this.русскийToolStripMenuItem.Click += new System.EventHandler(this.русскийToolStripMenuItem_Click);
            // 
            // русскийToolStripMenuItem1
            // 
            this.русскийToolStripMenuItem1.Enabled = false;
            this.русскийToolStripMenuItem1.Name = "русскийToolStripMenuItem1";
            this.русскийToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.русскийToolStripMenuItem1.Text = "Русский";
            this.русскийToolStripMenuItem1.Click += new System.EventHandler(this.русскийToolStripMenuItem1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 395);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "UDB Documenter";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem серверToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подключениеКСерверуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчетыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem генерацияОтчетаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem языкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьЯзыкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просмотрОтчетаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem английскийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem русскийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem русскийToolStripMenuItem1;
    }
}

