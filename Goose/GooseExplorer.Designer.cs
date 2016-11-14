namespace IEDExplorer
{
    partial class GooseExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GooseExplorer));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Start = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBox_NedDevices = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel_FoundIeds = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_FoundIedsCnt = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox_Ieds = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel_GoosesRcvd = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_GoosesRcvdCnt = new System.Windows.Forms.ToolStripLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.LiveViewChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.myTreeView_Goose = new IEDExplorer.MyTreeView();
            this.listView_Goose = new IEDExplorer.MyListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Start,
            this.toolStripButton_Stop,
            this.toolStripComboBox_NedDevices,
            this.toolStripLabel_FoundIeds,
            this.toolStripLabel_FoundIedsCnt,
            this.toolStripComboBox_Ieds,
            this.toolStripLabel_GoosesRcvd,
            this.toolStripLabel_GoosesRcvdCnt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1032, 26);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_Start
            // 
            this.toolStripButton_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Start.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Start.Image")));
            this.toolStripButton_Start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Start.Name = "toolStripButton_Start";
            this.toolStripButton_Start.Size = new System.Drawing.Size(23, 23);
            this.toolStripButton_Start.Text = "Start";
            this.toolStripButton_Start.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton_Stop
            // 
            this.toolStripButton_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Stop.Image")));
            this.toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Stop.Name = "toolStripButton_Stop";
            this.toolStripButton_Stop.Size = new System.Drawing.Size(23, 23);
            this.toolStripButton_Stop.Text = "Stop";
            this.toolStripButton_Stop.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripComboBox_NedDevices
            // 
            this.toolStripComboBox_NedDevices.DropDownHeight = 50;
            this.toolStripComboBox_NedDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox_NedDevices.IntegralHeight = false;
            this.toolStripComboBox_NedDevices.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolStripComboBox_NedDevices.Name = "toolStripComboBox_NedDevices";
            this.toolStripComboBox_NedDevices.Size = new System.Drawing.Size(350, 26);
            // 
            // toolStripLabel_FoundIeds
            // 
            this.toolStripLabel_FoundIeds.Name = "toolStripLabel_FoundIeds";
            this.toolStripLabel_FoundIeds.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolStripLabel_FoundIeds.Size = new System.Drawing.Size(99, 23);
            this.toolStripLabel_FoundIeds.Text = "Found IEDs:";
            this.toolStripLabel_FoundIeds.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripLabel_FoundIedsCnt
            // 
            this.toolStripLabel_FoundIedsCnt.Name = "toolStripLabel_FoundIedsCnt";
            this.toolStripLabel_FoundIedsCnt.Size = new System.Drawing.Size(16, 23);
            this.toolStripLabel_FoundIedsCnt.Text = "0";
            // 
            // toolStripComboBox_Ieds
            // 
            this.toolStripComboBox_Ieds.DropDownHeight = 50;
            this.toolStripComboBox_Ieds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox_Ieds.DropDownWidth = 150;
            this.toolStripComboBox_Ieds.IntegralHeight = false;
            this.toolStripComboBox_Ieds.Margin = new System.Windows.Forms.Padding(7, 0, 1, 0);
            this.toolStripComboBox_Ieds.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolStripComboBox_Ieds.Name = "toolStripComboBox_Ieds";
            this.toolStripComboBox_Ieds.Size = new System.Drawing.Size(220, 26);
            this.toolStripComboBox_Ieds.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox2_SelectedIndexChanged);
            // 
            // toolStripLabel_GoosesRcvd
            // 
            this.toolStripLabel_GoosesRcvd.Name = "toolStripLabel_GoosesRcvd";
            this.toolStripLabel_GoosesRcvd.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolStripLabel_GoosesRcvd.Size = new System.Drawing.Size(108, 23);
            this.toolStripLabel_GoosesRcvd.Text = "Gooses Rcvd:";
            this.toolStripLabel_GoosesRcvd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripLabel_GoosesRcvdCnt
            // 
            this.toolStripLabel_GoosesRcvdCnt.Name = "toolStripLabel_GoosesRcvdCnt";
            this.toolStripLabel_GoosesRcvdCnt.Size = new System.Drawing.Size(16, 23);
            this.toolStripLabel_GoosesRcvdCnt.Text = "0";
            // 
            // LiveViewChart
            // 
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            chartArea1.AxisX.LabelStyle.Format = "hh:mm:ss:ms";
            chartArea1.AxisX.LabelStyle.Interval = 1D;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorGrid.Interval = 1D;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorTickMark.Interval = 1D;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.Maximum = 2D;
            chartArea1.AxisX.Minimum = 2D;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.Interval = 100D;
            chartArea1.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.MinorTickMark.Interval = 100D;
            chartArea1.AxisX.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.ScaleView.MinSize = 1D;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            chartArea1.AxisY.LabelStyle.Interval = 5D;
            chartArea1.AxisY.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorGrid.Interval = 10D;
            chartArea1.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorTickMark.Interval = 10D;
            chartArea1.AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.ScaleBreakStyle.LineColor = System.Drawing.Color.Gray;
            chartArea1.Name = "ChartArea1";
            this.LiveViewChart.ChartAreas.Add(chartArea1);
            this.LiveViewChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.LiveViewChart.Legends.Add(legend1);
            this.LiveViewChart.Location = new System.Drawing.Point(0, 0);
            this.LiveViewChart.Name = "LiveViewChart";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series1.CustomProperties = "LabelStyle=Top";
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Color = System.Drawing.Color.Red;
            series2.IsVisibleInLegend = false;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.LiveViewChart.Series.Add(series1);
            this.LiveViewChart.Series.Add(series2);
            this.LiveViewChart.Size = new System.Drawing.Size(1032, 97);
            this.LiveViewChart.TabIndex = 6;
            this.LiveViewChart.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            title1.Name = "Title1";
            title1.Text = "Test";
            this.LiveViewChart.Titles.Add(title1);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.myTreeView_Goose);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView_Goose);
            this.splitContainer1.Size = new System.Drawing.Size(1032, 346);
            this.splitContainer1.SplitterDistance = 344;
            this.splitContainer1.TabIndex = 7;
            // 
            // myTreeView_Goose
            // 
            this.myTreeView_Goose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myTreeView_Goose.Location = new System.Drawing.Point(0, 0);
            this.myTreeView_Goose.Name = "myTreeView_Goose";
            this.myTreeView_Goose.Size = new System.Drawing.Size(344, 346);
            this.myTreeView_Goose.TabIndex = 4;
            this.myTreeView_Goose.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.myTreeView1_AfterSelect);
            this.myTreeView_Goose.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.myTreeView_Goose_NodeMouseClick);
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
            this.listView_Goose.Size = new System.Drawing.Size(684, 346);
            this.listView_Goose.TabIndex = 5;
            this.listView_Goose.UseCompatibleStateImageBehavior = false;
            this.listView_Goose.View = System.Windows.Forms.View.Details;
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
            this.columnHeader5.Text = "Additional Info";
            this.columnHeader5.Width = 350;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 26);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.LiveViewChart);
            this.splitContainer2.Size = new System.Drawing.Size(1032, 447);
            this.splitContainer2.SplitterDistance = 346;
            this.splitContainer2.TabIndex = 8;
            // 
            // GooseExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 473);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GooseExplorer";
            this.Text = "Goose Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Goose_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewChart)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Start;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox_NedDevices;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private MyTreeView myTreeView_Goose;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_FoundIeds;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_FoundIedsCnt;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox_Ieds;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_GoosesRcvd;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_GoosesRcvdCnt;
        private MyListView listView_Goose;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.DataVisualization.Charting.Chart LiveViewChart;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;

    }
}