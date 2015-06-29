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
            this.treeViewAdv_data = new Aga.Controls.Tree.TreeViewAdv();
            this.SuspendLayout();
            // 
            // treeViewAdv_data
            // 
            this.treeViewAdv_data.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv_data.ColumnHeaderHeight = 0;
            this.treeViewAdv_data.DefaultToolTipProvider = null;
            this.treeViewAdv_data.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv_data.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdv_data.Location = new System.Drawing.Point(308, 43);
            this.treeViewAdv_data.Model = null;
            this.treeViewAdv_data.Name = "treeViewAdv_data";
            this.treeViewAdv_data.SelectedNode = null;
            this.treeViewAdv_data.Size = new System.Drawing.Size(510, 456);
            this.treeViewAdv_data.TabIndex = 0;
            this.treeViewAdv_data.Text = "treeViewAdv1";
            this.treeViewAdv_data.Click += new System.EventHandler(this.treeViewAdv1_Click);
            // 
            // WatchDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 579);
            this.Controls.Add(this.treeViewAdv_data);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "WatchDataView";
            this.Text = "IedDataView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IedDataView_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv treeViewAdv_data;

    }
}