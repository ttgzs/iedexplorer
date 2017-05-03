namespace IEDExplorer.Views {
    partial class PollView {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PollView));
            this.tsPollView = new System.Windows.Forms.ToolStrip();
            this.tsbStart = new System.Windows.Forms.ToolStripButton();
            this.tsbStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbRefreshInterval = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbImportPollingList = new System.Windows.Forms.ToolStripButton();
            this.tsbExportList = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.PollListView = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PollTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsPollView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsPollView
            // 
            this.tsPollView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.toolStripSeparator3,
            this.tsbStart,
            this.tsbStop,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.cbRefreshInterval,
            this.toolStripSeparator1,
            this.tsbImportPollingList,
            this.tsbExportList,
            this.tsbClear});
            this.tsPollView.Location = new System.Drawing.Point(0, 0);
            this.tsPollView.Name = "tsPollView";
            this.tsPollView.Size = new System.Drawing.Size(653, 25);
            this.tsPollView.TabIndex = 0;
            this.tsPollView.Text = "toolStrip1";
            // 
            // tsbStart
            // 
            this.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbStart.Image")));
            this.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStart.Name = "tsbStart";
            this.tsbStart.Size = new System.Drawing.Size(23, 22);
            this.tsbStart.Text = "toolStripButton1";
            this.tsbStart.ToolTipText = "Start Polling";
            this.tsbStart.Click += new System.EventHandler(this.tsbStart_Click);
            // 
            // tsbStop
            // 
            this.tsbStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStop.Enabled = false;
            this.tsbStop.Image = ((System.Drawing.Image)(resources.GetObject("tsbStop.Image")));
            this.tsbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new System.Drawing.Size(23, 22);
            this.tsbStop.Text = "toolStripButton2";
            this.tsbStop.ToolTipText = "Stop Polling";
            this.tsbStop.Click += new System.EventHandler(this.tsbStop_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(88, 22);
            this.toolStripLabel1.Text = "Refresh Interval";
            // 
            // cbRefreshInterval
            // 
            this.cbRefreshInterval.Items.AddRange(new object[] {
            "0,5 s",
            "1 s",
            "2 s",
            "5 s",
            "10 s",
            "15 s"});
            this.cbRefreshInterval.Name = "cbRefreshInterval";
            this.cbRefreshInterval.Size = new System.Drawing.Size(121, 25);
            this.cbRefreshInterval.SelectedIndexChanged += new System.EventHandler(this.cbRefreshInterval_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbImportPollingList
            // 
            this.tsbImportPollingList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbImportPollingList.Image = ((System.Drawing.Image)(resources.GetObject("tsbImportPollingList.Image")));
            this.tsbImportPollingList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportPollingList.Name = "tsbImportPollingList";
            this.tsbImportPollingList.Size = new System.Drawing.Size(23, 22);
            this.tsbImportPollingList.Tag = "Load Node list";
            this.tsbImportPollingList.Text = "toolStripButton1";
            this.tsbImportPollingList.ToolTipText = "Import Polling List";
            this.tsbImportPollingList.Click += new System.EventHandler(this.tsbImportPollingList_Click);
            // 
            // tsbExportList
            // 
            this.tsbExportList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportList.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportList.Image")));
            this.tsbExportList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportList.Name = "tsbExportList";
            this.tsbExportList.Size = new System.Drawing.Size(23, 22);
            this.tsbExportList.Tag = "Save Node List";
            this.tsbExportList.Text = "Export List";
            this.tsbExportList.ToolTipText = "Export Polling List";
            this.tsbExportList.Click += new System.EventHandler(this.tsbExportList_Click);
            // 
            // tsbClear
            // 
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(23, 22);
            this.tsbClear.Tag = "Clear Polling List";
            this.tsbClear.Text = "Clear Polling List";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // PollListView
            // 
            this.PollListView.AllowDrop = true;
            this.PollListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colType,
            this.colValue});
            this.PollListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PollListView.GridLines = true;
            this.PollListView.Location = new System.Drawing.Point(0, 25);
            this.PollListView.Name = "PollListView";
            this.PollListView.Size = new System.Drawing.Size(653, 316);
            this.PollListView.TabIndex = 1;
            this.PollListView.UseCompatibleStateImageBehavior = false;
            this.PollListView.View = System.Windows.Forms.View.Details;
            this.PollListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.PollListView_DragDrop);
            this.PollListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.PollListView_DragEnter);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 300;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 110;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 200;
            // 
            // PollTimer
            // 
            this.PollTimer.Tick += new System.EventHandler(this.PollTimer_Tick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbRefresh.Text = "toolStripButton1";
            this.tsbRefresh.ToolTipText = "Refresh (Single Read)";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // PollView
            // 
            this.ClientSize = new System.Drawing.Size(653, 341);
            this.Controls.Add(this.PollListView);
            this.Controls.Add(this.tsPollView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "PollView";
            this.Text = "Poll View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PollView_FormClosing);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PollView_DragEnter);
            this.tsPollView.ResumeLayout(false);
            this.tsPollView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsPollView;
        private System.Windows.Forms.ToolStripButton tsbStart;
        private System.Windows.Forms.ListView PollListView;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ToolStripButton tsbStop;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbRefreshInterval;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbExportList;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripButton tsbImportPollingList;
        private System.Windows.Forms.Timer PollTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
    }
}
