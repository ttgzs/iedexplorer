namespace IEDExplorer.Views
{
    partial class IedDataView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IedDataView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_RunAu = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_StopAu = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBox_autoUpdate = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel_autoupdate = new System.Windows.Forms.ToolStripLabel();
            this.listView_data = new IEDExplorer.MyListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer_Au = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_RunAu,
            this.toolStripButton_StopAu,
            this.toolStripComboBox_autoUpdate,
            this.toolStripLabel_autoupdate});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(914, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_RunAu
            // 
            this.toolStripButton_RunAu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_RunAu.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RunAu.Image")));
            this.toolStripButton_RunAu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RunAu.Name = "toolStripButton_RunAu";
            this.toolStripButton_RunAu.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_RunAu.Text = "toolStripButton1";
            this.toolStripButton_RunAu.Click += new System.EventHandler(this.toolStripButton_RunAu_Click);
            // 
            // toolStripButton_StopAu
            // 
            this.toolStripButton_StopAu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_StopAu.Enabled = false;
            this.toolStripButton_StopAu.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StopAu.Image")));
            this.toolStripButton_StopAu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StopAu.Name = "toolStripButton_StopAu";
            this.toolStripButton_StopAu.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_StopAu.Text = "toolStripButton2";
            this.toolStripButton_StopAu.Click += new System.EventHandler(this.toolStripButton_StopAu_Click);
            // 
            // toolStripComboBox_autoUpdate
            // 
            this.toolStripComboBox_autoUpdate.Items.AddRange(new object[] {
            "100",
            "500",
            "1000",
            "5000"});
            this.toolStripComboBox_autoUpdate.Name = "toolStripComboBox_autoUpdate";
            this.toolStripComboBox_autoUpdate.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox_autoUpdate.TextChanged += new System.EventHandler(this.toolStripComboBox_autoUpdate_TextChanged);
            // 
            // toolStripLabel_autoupdate
            // 
            this.toolStripLabel_autoupdate.Name = "toolStripLabel_autoupdate";
            this.toolStripLabel_autoupdate.Size = new System.Drawing.Size(98, 22);
            this.toolStripLabel_autoupdate.Text = "AutoUpdate [ms]";
            // 
            // listView_data
            // 
            this.listView_data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_data.FullRowSelect = true;
            this.listView_data.GridLines = true;
            this.listView_data.Location = new System.Drawing.Point(0, 25);
            this.listView_data.Name = "listView_data";
            this.listView_data.Size = new System.Drawing.Size(914, 554);
            this.listView_data.TabIndex = 3;
            this.listView_data.UseCompatibleStateImageBehavior = false;
            this.listView_data.View = System.Windows.Forms.View.Details;
            this.listView_data.SelectedIndexChanged += new System.EventHandler(this.listView_data_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 350;
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
            this.columnHeader5.Text = "Communication Address";
            this.columnHeader5.Width = 350;
            // 
            // timer_Au
            // 
            this.timer_Au.Interval = 2000;
            this.timer_Au.Tick += new System.EventHandler(this.timer_Au_Tick);
            // 
            // IedDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 579);
            this.Controls.Add(this.listView_data);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "IedDataView";
            this.Text = "IedDataView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IedDataView_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private MyListView listView_data;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripButton toolStripButton_RunAu;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox_autoUpdate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_autoupdate;
        private System.Windows.Forms.Timer timer_Au;
        private System.Windows.Forms.ToolStripButton toolStripButton_StopAu;

    }
}