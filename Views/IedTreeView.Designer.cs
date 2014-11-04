namespace IEDExplorer.Views
{
    partial class IedTreeView
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
            this.treeViewIed = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewIed
            // 
            this.treeViewIed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewIed.Location = new System.Drawing.Point(0, 0);
            this.treeViewIed.Name = "treeViewIed";
            this.treeViewIed.Size = new System.Drawing.Size(284, 262);
            this.treeViewIed.TabIndex = 0;
            this.treeViewIed.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewIed_AfterSelect);
            this.treeViewIed.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewIed_NodeMouseClick);
            // 
            // IedTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.treeViewIed);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "IedTreeView";
            this.Text = "IedTreeView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewIed;
    }
}