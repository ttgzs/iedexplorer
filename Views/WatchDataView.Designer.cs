namespace IEDExplorer.Views
{
    partial class WatchDataView
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.treeViewAdv_data = new Aga.Controls.Tree.TreeViewAdv();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(691, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // treeViewAdv_data
            // 
            this.treeViewAdv_data.AllowDrop = true;
            this.treeViewAdv_data.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv_data.BackColor2 = System.Drawing.SystemColors.Window;
            this.treeViewAdv_data.BackgroundPaintMode = Aga.Controls.Tree.BackgroundPaintMode.Default;
            this.treeViewAdv_data.ColumnHeaderHeight = 0;
            this.treeViewAdv_data.DefaultToolTipProvider = null;
            this.treeViewAdv_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAdv_data.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv_data.HighlightColorActive = System.Drawing.SystemColors.Highlight;
            this.treeViewAdv_data.HighlightColorInactive = System.Drawing.SystemColors.InactiveBorder;
            this.treeViewAdv_data.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdv_data.Location = new System.Drawing.Point(0, 25);
            this.treeViewAdv_data.Model = null;
            this.treeViewAdv_data.Name = "treeViewAdv_data";
            this.treeViewAdv_data.OnVisibleOverride = null;
            this.treeViewAdv_data.SelectedNode = null;
            this.treeViewAdv_data.Size = new System.Drawing.Size(691, 380);
            this.treeViewAdv_data.TabIndex = 2;
            this.treeViewAdv_data.Text = "treeViewAdv1";
            this.treeViewAdv_data.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewAdv1_DragDrop);
            this.treeViewAdv_data.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewAdv1_DragEnter);
            // 
            // WatchDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 405);
            this.Controls.Add(this.treeViewAdv_data);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "WatchDataView";
            this.Text = "WatchView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IedDataView_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Aga.Controls.Tree.TreeViewAdv treeViewAdv_data;


    }
}