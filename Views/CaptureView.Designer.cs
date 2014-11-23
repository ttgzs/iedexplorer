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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.PacketNrCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DirCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MMSPduCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MMSServiceCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textBoxXML = new System.Windows.Forms.TextBox();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonClear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(837, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
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
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(837, 500);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PacketNrCol,
            this.TimeCol,
            this.DirCol,
            this.MMSPduCol,
            this.MMSServiceCol,
            this.SizeCol});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(837, 245);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // PacketNrCol
            // 
            this.PacketNrCol.Text = "Nr.";
            // 
            // TimeCol
            // 
            this.TimeCol.Text = "Time";
            this.TimeCol.Width = 120;
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
            this.splitContainer2.Panel1.Controls.Add(this.textBoxXML);
            this.splitContainer2.Size = new System.Drawing.Size(837, 251);
            this.splitContainer2.SplitterDistance = 416;
            this.splitContainer2.TabIndex = 0;
            // 
            // textBoxXML
            // 
            this.textBoxXML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXML.Location = new System.Drawing.Point(3, 0);
            this.textBoxXML.Multiline = true;
            this.textBoxXML.Name = "textBoxXML";
            this.textBoxXML.Size = new System.Drawing.Size(410, 248);
            this.textBoxXML.TabIndex = 0;
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClear.Image")));
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(83, 22);
            this.toolStripButtonClear.Text = "Clear Capture";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
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
            this.splitContainer2.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBoxXML;
        private System.Windows.Forms.ColumnHeader TimeCol;
        private System.Windows.Forms.ColumnHeader MMSPduCol;
        private System.Windows.Forms.ColumnHeader DirCol;
        private System.Windows.Forms.ColumnHeader MMSServiceCol;
        private System.Windows.Forms.ColumnHeader SizeCol;
        private System.Windows.Forms.ColumnHeader PacketNrCol;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;

    }
}