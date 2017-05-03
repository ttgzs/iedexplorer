namespace IEDExplorer.Views
{
    partial class CaptureView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptureView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_StartCapture = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_StopCapture = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewPackets = new System.Windows.Forms.ListView();
            this.PacketNrCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DirCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MMSPduCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MMSServiceCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxXML = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeViewXML = new System.Windows.Forms.TreeView();
            this.hexBoxHEX = new Be.Windows.Forms.HexBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_StartCapture,
            this.toolStripButton_StopCapture,
            this.toolStripSeparator1,
            this.toolStripButtonClear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(837, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_StartCapture
            // 
            this.toolStripButton_StartCapture.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_StartCapture.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StartCapture.Image")));
            this.toolStripButton_StartCapture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartCapture.Name = "toolStripButton_StartCapture";
            this.toolStripButton_StartCapture.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_StartCapture.Text = "toolStripButton1";
            this.toolStripButton_StartCapture.ToolTipText = "Start Capture";
            this.toolStripButton_StartCapture.Click += new System.EventHandler(this.toolStripButton_StartCapture_Click);
            // 
            // toolStripButton_StopCapture
            // 
            this.toolStripButton_StopCapture.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_StopCapture.Enabled = false;
            this.toolStripButton_StopCapture.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StopCapture.Image")));
            this.toolStripButton_StopCapture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StopCapture.Name = "toolStripButton_StopCapture";
            this.toolStripButton_StopCapture.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_StopCapture.Text = "toolStripButton1";
            this.toolStripButton_StopCapture.ToolTipText = "Stop Capture";
            this.toolStripButton_StopCapture.Click += new System.EventHandler(this.toolStripButton_StopCapture_Click);
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClear.Image")));
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonClear.Text = "Clear Capture";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewPackets);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(837, 500);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 2;
            // 
            // listViewPackets
            // 
            this.listViewPackets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PacketNrCol,
            this.TimeCol,
            this.DirCol,
            this.MMSPduCol,
            this.MMSServiceCol,
            this.SizeCol});
            this.listViewPackets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPackets.FullRowSelect = true;
            this.listViewPackets.Location = new System.Drawing.Point(0, 0);
            this.listViewPackets.Name = "listViewPackets";
            this.listViewPackets.Size = new System.Drawing.Size(837, 245);
            this.listViewPackets.TabIndex = 0;
            this.listViewPackets.UseCompatibleStateImageBehavior = false;
            this.listViewPackets.View = System.Windows.Forms.View.Details;
            this.listViewPackets.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // PacketNrCol
            // 
            this.PacketNrCol.Text = "Packet Nr.";
            this.PacketNrCol.Width = 80;
            // 
            // TimeCol
            // 
            this.TimeCol.Text = "Time";
            this.TimeCol.Width = 150;
            // 
            // DirCol
            // 
            this.DirCol.Text = "Dir";
            this.DirCol.Width = 40;
            // 
            // MMSPduCol
            // 
            this.MMSPduCol.Text = "MMS Pdu";
            this.MMSPduCol.Width = 200;
            // 
            // MMSServiceCol
            // 
            this.MMSServiceCol.Text = "MMS Service";
            this.MMSServiceCol.Width = 180;
            // 
            // SizeCol
            // 
            this.SizeCol.Text = "Size";
            this.SizeCol.Width = 100;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.hexBoxHEX);
            this.splitContainer2.Size = new System.Drawing.Size(837, 251);
            this.splitContainer2.SplitterDistance = 416;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(411, 248);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxXML);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(403, 222);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "XML Text";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxXML
            // 
            this.textBoxXML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXML.Location = new System.Drawing.Point(0, 0);
            this.textBoxXML.Multiline = true;
            this.textBoxXML.Name = "textBoxXML";
            this.textBoxXML.ReadOnly = true;
            this.textBoxXML.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxXML.Size = new System.Drawing.Size(403, 222);
            this.textBoxXML.TabIndex = 1;
            this.textBoxXML.WordWrap = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeViewXML);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(403, 222);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "XML Tree";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeViewXML
            // 
            this.treeViewXML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewXML.Location = new System.Drawing.Point(0, 0);
            this.treeViewXML.Name = "treeViewXML";
            this.treeViewXML.Size = new System.Drawing.Size(403, 222);
            this.treeViewXML.TabIndex = 0;
            // 
            // hexBoxHEX
            // 
            this.hexBoxHEX.BytesPerLine = 8;
            this.hexBoxHEX.ColumnInfoVisible = true;
            this.hexBoxHEX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBoxHEX.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.hexBoxHEX.LineInfoVisible = true;
            this.hexBoxHEX.Location = new System.Drawing.Point(0, 0);
            this.hexBoxHEX.Name = "hexBoxHEX";
            this.hexBoxHEX.ReadOnly = true;
            this.hexBoxHEX.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBoxHEX.Size = new System.Drawing.Size(417, 251);
            this.hexBoxHEX.StringViewVisible = true;
            this.hexBoxHEX.TabIndex = 0;
            this.hexBoxHEX.UseFixedBytesPerLine = true;
            this.hexBoxHEX.VScrollBarVisible = true;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // CaptureView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 525);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "CaptureView";
            this.Text = "CaptureView";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listViewPackets;
        private System.Windows.Forms.ColumnHeader TimeCol;
        private System.Windows.Forms.ColumnHeader MMSPduCol;
        private System.Windows.Forms.ColumnHeader DirCol;
        private System.Windows.Forms.ColumnHeader MMSServiceCol;
        private System.Windows.Forms.ColumnHeader SizeCol;
        private System.Windows.Forms.ColumnHeader PacketNrCol;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;
        private Be.Windows.Forms.HexBox hexBoxHEX;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBoxXML;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView treeViewXML;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartCapture;
        private System.Windows.Forms.ToolStripButton toolStripButton_StopCapture;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}