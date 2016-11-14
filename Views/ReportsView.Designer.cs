namespace IEDExplorer.Views {
    partial class ReportsView {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportsView));
            this.tsReportView = new System.Windows.Forms.ToolStrip();
            this.tsbClearReports = new System.Windows.Forms.ToolStripButton();
            this.tsbExportCSV = new System.Windows.Forms.ToolStripButton();
            this.ReportlistView = new System.Windows.Forms.ListView();
            this.colNr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTimestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tsReportView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsReportView
            // 
            this.tsReportView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClearReports,
            this.tsbExportCSV});
            this.tsReportView.Location = new System.Drawing.Point(0, 0);
            this.tsReportView.Name = "tsReportView";
            this.tsReportView.Size = new System.Drawing.Size(805, 25);
            this.tsReportView.TabIndex = 0;
            this.tsReportView.Text = "Report View";
            // 
            // tsbClearReports
            // 
            this.tsbClearReports.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClearReports.Image = ((System.Drawing.Image)(resources.GetObject("tsbClearReports.Image")));
            this.tsbClearReports.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearReports.Name = "tsbClearReports";
            this.tsbClearReports.Size = new System.Drawing.Size(23, 22);
            this.tsbClearReports.Text = "toolStripButton1";
            this.tsbClearReports.Click += new System.EventHandler(this.tsbClearReports_Click);
            // 
            // tsbExportCSV
            // 
            this.tsbExportCSV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportCSV.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportCSV.Image")));
            this.tsbExportCSV.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportCSV.Name = "tsbExportCSV";
            this.tsbExportCSV.Size = new System.Drawing.Size(23, 22);
            this.tsbExportCSV.Text = "toolStripButton1";
            this.tsbExportCSV.Click += new System.EventHandler(this.tsbExportCSV_Click);
            // 
            // ReportlistView
            // 
            this.ReportlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNr,
            this.colInfo,
            this.colTimestamp,
            this.colPath,
            this.colDescription,
            this.colValue});
            this.ReportlistView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportlistView.GridLines = true;
            this.ReportlistView.Location = new System.Drawing.Point(0, 25);
            this.ReportlistView.Name = "ReportlistView";
            this.ReportlistView.Size = new System.Drawing.Size(805, 356);
            this.ReportlistView.TabIndex = 1;
            this.ReportlistView.UseCompatibleStateImageBehavior = false;
            this.ReportlistView.View = System.Windows.Forms.View.Details;
            // 
            // colNr
            // 
            this.colNr.Text = "Nr";
            this.colNr.Width = 48;
            // 
            // colInfo
            // 
            this.colInfo.Text = "Info";
            this.colInfo.Width = 44;
            // 
            // colTimestamp
            // 
            this.colTimestamp.Text = "Timestamp";
            this.colTimestamp.Width = 189;
            // 
            // colPath
            // 
            this.colPath.Text = "Path";
            this.colPath.Width = 202;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 298;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 97;
            // 
            // ReportsView
            // 
            this.ClientSize = new System.Drawing.Size(805, 381);
            this.Controls.Add(this.ReportlistView);
            this.Controls.Add(this.tsReportView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "ReportsView";
            this.Text = "ReportsView";
            this.tsReportView.ResumeLayout(false);
            this.tsReportView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsReportView;
        public System.Windows.Forms.ListView ReportlistView;
        private System.Windows.Forms.ColumnHeader colNr;
        private System.Windows.Forms.ColumnHeader colInfo;
        private System.Windows.Forms.ColumnHeader colTimestamp;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ToolStripButton tsbClearReports;
        private System.Windows.Forms.ToolStripButton tsbExportCSV;
    }
}
