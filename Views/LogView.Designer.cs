using System.Windows.Forms;
namespace IEDExplorer.Views
{
    partial class LogView
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
            this.dataGridView_log = new IEDExplorer.Views.MyDataGridView();
            this.column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_log)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_log
            // 
            this.dataGridView_log.AllowUserToAddRows = false;
            this.dataGridView_log.AllowUserToResizeRows = false;
            this.dataGridView_log.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_log.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_log.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView_log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_log.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column1});
            this.dataGridView_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_log.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_log.Name = "dataGridView_log";
            this.dataGridView_log.RowHeadersVisible = false;
            this.dataGridView_log.RowTemplate.Height = 17;
            this.dataGridView_log.Size = new System.Drawing.Size(284, 262);
            this.dataGridView_log.TabIndex = 0;
            this.dataGridView_log.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewLog_MouseUp);
            // 
            // column1
            // 
            this.column1.HeaderText = "Info";
            this.column1.Name = "column1";
            this.column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Message
            // 
            this.Message.Text = "Message";
            this.Message.Width = 928;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem,
            this.saveLogToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // saveLogToolStripMenuItem
            // 
            this.saveLogToolStripMenuItem.Name = "saveLogToolStripMenuItem";
            this.saveLogToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveLogToolStripMenuItem.Text = "Save Log";
            this.saveLogToolStripMenuItem.Click += new System.EventHandler(this.saveLogToolStripMenuItem_Click);
            // 
            // LogView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.dataGridView_log);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "LogView";
            this.Text = "LogView";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_log)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.ListView listViewLog;
        private MyDataGridView dataGridView_log;
        private System.Windows.Forms.ColumnHeader Message;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLogToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private DataGridViewTextBoxColumn column1;
    }
}