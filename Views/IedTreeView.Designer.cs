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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeViewIed = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeViewIec = new System.Windows.Forms.TreeView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(303, 421);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeViewIed);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(295, 395);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IED View (MMS)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeViewIed
            // 
            this.treeViewIed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewIed.Location = new System.Drawing.Point(3, 3);
            this.treeViewIed.Name = "treeViewIed";
            this.treeViewIed.Size = new System.Drawing.Size(289, 389);
            this.treeViewIed.TabIndex = 1;
            this.treeViewIed.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewIed_AfterSelect);
            this.treeViewIed.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewIed_NodeMouseClick);
            this.treeViewIed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewIed_MouseDown);
            this.treeViewIed.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeViewIed_MouseMove);
            this.treeViewIed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewIed_MouseUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeViewIec);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(295, 395);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IEC View (61850)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeViewIec
            // 
            this.treeViewIec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewIec.Location = new System.Drawing.Point(3, 3);
            this.treeViewIec.Name = "treeViewIec";
            this.treeViewIec.Size = new System.Drawing.Size(289, 389);
            this.treeViewIec.TabIndex = 2;
            this.treeViewIec.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewIed_AfterSelect);
            this.treeViewIec.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewIed_NodeMouseClick);
            this.treeViewIec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewIed_MouseDown);
            this.treeViewIec.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeViewIed_MouseMove);
            this.treeViewIec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewIed_MouseUp);
            // 
            // IedTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 421);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "IedTreeView";
            this.Text = "IedTreeView";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView treeViewIed;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView treeViewIec;

    }
}