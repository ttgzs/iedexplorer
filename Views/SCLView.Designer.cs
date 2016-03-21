namespace IEDExplorer.Views
{
    partial class SCLView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SCLView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeViewSCL = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeViewSCL_IEC = new System.Windows.Forms.TreeView();
            this.dataGridView_data = new IEDExplorer.Views.MyDataGridView();
            this.column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_data)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCollapseAll,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(634, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonCollapseAll
            // 
            this.toolStripButtonCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCollapseAll.Image")));
            this.toolStripButtonCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCollapseAll.Name = "toolStripButtonCollapseAll";
            this.toolStripButtonCollapseAll.Size = new System.Drawing.Size(73, 22);
            this.toolStripButtonCollapseAll.Text = "Collapse All";
            this.toolStripButtonCollapseAll.Click += new System.EventHandler(this.toolStripButtonCollapseAll_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView_data);
            this.splitContainer1.Size = new System.Drawing.Size(634, 370);
            this.splitContainer1.SplitterDistance = 211;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(211, 370);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeViewSCL);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(203, 344);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IED View (MMS)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeViewSCL
            // 
            this.treeViewSCL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSCL.Location = new System.Drawing.Point(3, 3);
            this.treeViewSCL.Name = "treeViewSCL";
            this.treeViewSCL.Size = new System.Drawing.Size(197, 338);
            this.treeViewSCL.TabIndex = 0;
            this.treeViewSCL.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeViewSCL.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewSCL_NodeMouseClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeViewSCL_IEC);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(203, 344);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IEC View (61850)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeViewSCL_IEC
            // 
            this.treeViewSCL_IEC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSCL_IEC.Location = new System.Drawing.Point(3, 3);
            this.treeViewSCL_IEC.Name = "treeViewSCL_IEC";
            this.treeViewSCL_IEC.Size = new System.Drawing.Size(197, 338);
            this.treeViewSCL_IEC.TabIndex = 0;
            // 
            // dataGridView_data
            // 
            this.dataGridView_data.AllowUserToAddRows = false;
            this.dataGridView_data.AllowUserToResizeRows = false;
            this.dataGridView_data.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_data.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView_data.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column2,
            this.column3,
            this.column4,
            this.column5,
            this.column6,
            this.column7});
            this.dataGridView_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_data.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_data.Name = "dataGridView_data";
            this.dataGridView_data.RowHeadersVisible = false;
            this.dataGridView_data.RowTemplate.Height = 17;
            this.dataGridView_data.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_data.Size = new System.Drawing.Size(419, 370);
            this.dataGridView_data.TabIndex = 0;
            this.dataGridView_data.SelectionChanged += new System.EventHandler(this.dataGridView_data_SelectionChanged);
            // 
            // column2
            // 
            this.column2.HeaderText = "Name";
            this.column2.Name = "column2";
            this.column2.Width = 70;
            // 
            // column3
            // 
            this.column3.HeaderText = "Type";
            this.column3.Name = "column3";
            this.column3.Width = 69;
            // 
            // column4
            // 
            this.column4.HeaderText = "Value";
            this.column4.Name = "column4";
            this.column4.Width = 70;
            // 
            // column5
            // 
            this.column5.HeaderText = "Dom";
            this.column5.Name = "column5";
            this.column5.Width = 69;
            // 
            // column6
            // 
            this.column6.HeaderText = "Logical Node";
            this.column6.Name = "column6";
            this.column6.Width = 70;
            // 
            // column7
            // 
            this.column7.HeaderText = "Var Path";
            this.column7.Name = "column7";
            this.column7.Width = 69;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // SCLView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 395);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "SCLView";
            this.Text = "SCLView";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_data)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewSCL;
        private System.Windows.Forms.DataGridViewTextBoxColumn column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn column7;
        private MyDataGridView dataGridView_data;
        private System.Windows.Forms.ToolStripButton toolStripButtonCollapseAll;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView treeViewSCL_IEC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}