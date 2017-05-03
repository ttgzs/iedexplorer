namespace IEDExplorer
{
    partial class GooseDataEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GooseDataEdit));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Drag and drop data item from Data Editor ...");
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.myTreeView_Goose = new IEDExplorer.MyTreeView();
            this.listView_Goose = new IEDExplorer.MyListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_DataEdit = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Import = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Export = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.myListView1 = new IEDExplorer.MyListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_SeqMan = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_StartSeq = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_StopSeq = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_ClearSeq = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.myListView1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(924, 530);
            this.splitContainer1.SplitterDistance = 308;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.myTreeView_Goose);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listView_Goose);
            this.splitContainer2.Size = new System.Drawing.Size(924, 283);
            this.splitContainer2.SplitterDistance = 308;
            this.splitContainer2.TabIndex = 13;
            // 
            // myTreeView_Goose
            // 
            this.myTreeView_Goose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myTreeView_Goose.Location = new System.Drawing.Point(0, 0);
            this.myTreeView_Goose.Name = "myTreeView_Goose";
            this.myTreeView_Goose.Size = new System.Drawing.Size(308, 283);
            this.myTreeView_Goose.TabIndex = 8;
            this.myTreeView_Goose.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.myTreeView_Goose_AfterSelect);
            this.myTreeView_Goose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.myTreeView_Goose_MouseDown);
            this.myTreeView_Goose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.myTreeView_Goose_MouseMove);
            this.myTreeView_Goose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.myTreeView_Goose_MouseUp);
            // 
            // listView_Goose
            // 
            this.listView_Goose.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView_Goose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Goose.FullRowSelect = true;
            this.listView_Goose.GridLines = true;
            this.listView_Goose.Location = new System.Drawing.Point(0, 0);
            this.listView_Goose.Margin = new System.Windows.Forms.Padding(4);
            this.listView_Goose.Name = "listView_Goose";
            this.listView_Goose.ShowItemToolTips = true;
            this.listView_Goose.Size = new System.Drawing.Size(612, 283);
            this.listView_Goose.TabIndex = 9;
            this.listView_Goose.UseCompatibleStateImageBehavior = false;
            this.listView_Goose.View = System.Windows.Forms.View.Details;
            this.listView_Goose.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView_Goose_AfterLabelEdit);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 215;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Additional Info";
            this.columnHeader5.Width = 350;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_DataEdit,
            this.toolStripSeparator2,
            this.toolStripButton_Import,
            this.toolStripButton_Export,
            this.toolStripButton_Clear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(924, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel_DataEdit
            // 
            this.toolStripLabel_DataEdit.Name = "toolStripLabel_DataEdit";
            this.toolStripLabel_DataEdit.Size = new System.Drawing.Size(80, 22);
            this.toolStripLabel_DataEdit.Text = "Data Editor";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Import
            // 
            this.toolStripButton_Import.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Import.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Import.Image")));
            this.toolStripButton_Import.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Import.Name = "toolStripButton_Import";
            this.toolStripButton_Import.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Import.Text = "Import List";
            this.toolStripButton_Import.ToolTipText = "Import Data List";
            this.toolStripButton_Import.Click += new System.EventHandler(this.toolStripButton_Import_Click);
            // 
            // toolStripButton_Export
            // 
            this.toolStripButton_Export.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Export.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Export.Image")));
            this.toolStripButton_Export.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Export.Name = "toolStripButton_Export";
            this.toolStripButton_Export.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Export.Text = "Export List";
            this.toolStripButton_Export.ToolTipText = "Export Data List";
            this.toolStripButton_Export.Click += new System.EventHandler(this.toolStripButton_Export_Click);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Clear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Clear.Image")));
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Clear.Text = "Clear Polling List";
            this.toolStripButton_Clear.ToolTipText = "Clear Data List";
            this.toolStripButton_Clear.Click += new System.EventHandler(this.toolStripButton_Clear_Click);
            // 
            // myListView1
            // 
            this.myListView1.AllowDrop = true;
            this.myListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.myListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myListView1.FullRowSelect = true;
            this.myListView1.GridLines = true;
            this.myListView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.myListView1.Location = new System.Drawing.Point(0, 25);
            this.myListView1.Margin = new System.Windows.Forms.Padding(4);
            this.myListView1.MultiSelect = false;
            this.myListView1.Name = "myListView1";
            this.myListView1.ShowItemToolTips = true;
            this.myListView1.Size = new System.Drawing.Size(924, 193);
            this.myListView1.TabIndex = 14;
            this.myListView1.UseCompatibleStateImageBehavior = false;
            this.myListView1.View = System.Windows.Forms.View.Details;
            this.myListView1.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.myListView1_AfterLabelEdit);
            this.myListView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.myListView1_DragDrop);
            this.myListView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.myListView1_DragEnter);
            this.myListView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.myListView1_KeyDown);
            this.myListView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.myListView1_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 350;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Type";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Value";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Duration [ms]";
            this.columnHeader8.Width = 350;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_SeqMan,
            this.toolStripSeparator1,
            this.toolStripButton_StartSeq,
            this.toolStripButton_StopSeq,
            this.toolStripButton_ClearSeq});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(924, 25);
            this.toolStrip2.TabIndex = 13;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel_SeqMan
            // 
            this.toolStripLabel_SeqMan.Name = "toolStripLabel_SeqMan";
            this.toolStripLabel_SeqMan.Size = new System.Drawing.Size(133, 22);
            this.toolStripLabel_SeqMan.Text = "Sequence Manager";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_StartSeq
            // 
            this.toolStripButton_StartSeq.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_StartSeq.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StartSeq.Image")));
            this.toolStripButton_StartSeq.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartSeq.Name = "toolStripButton_StartSeq";
            this.toolStripButton_StartSeq.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_StartSeq.Text = "Start";
            this.toolStripButton_StartSeq.ToolTipText = "Start Sequence";
            this.toolStripButton_StartSeq.Click += new System.EventHandler(this.toolStripButton_StartSeq_Click);
            // 
            // toolStripButton_StopSeq
            // 
            this.toolStripButton_StopSeq.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_StopSeq.Enabled = false;
            this.toolStripButton_StopSeq.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StopSeq.Image")));
            this.toolStripButton_StopSeq.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StopSeq.Name = "toolStripButton_StopSeq";
            this.toolStripButton_StopSeq.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_StopSeq.Text = "Stop";
            this.toolStripButton_StopSeq.ToolTipText = "Stop Sequence";
            this.toolStripButton_StopSeq.Click += new System.EventHandler(this.toolStripButton_StopSeq_Click);
            // 
            // toolStripButton_ClearSeq
            // 
            this.toolStripButton_ClearSeq.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_ClearSeq.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ClearSeq.Image")));
            this.toolStripButton_ClearSeq.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ClearSeq.Name = "toolStripButton_ClearSeq";
            this.toolStripButton_ClearSeq.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_ClearSeq.Text = "Clear Polling List";
            this.toolStripButton_ClearSeq.ToolTipText = "Clear Sequence List";
            this.toolStripButton_ClearSeq.Click += new System.EventHandler(this.toolStripButton_ClearSeq_Click);
            // 
            // GooseDataeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 530);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GooseDataeEdit";
            this.Text = "Data Editor & Sequence Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GooseDataeEdit_FormClosing_1);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Import;
        private System.Windows.Forms.ToolStripButton toolStripButton_Export;
        private System.Windows.Forms.ToolStripButton toolStripButton_Clear;
        private MyTreeView myTreeView_Goose;
        private MyListView listView_Goose;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private MyListView myListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton_ClearSeq;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartSeq;
        private System.Windows.Forms.ToolStripButton toolStripButton_StopSeq;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_DataEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_SeqMan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;



    }
}