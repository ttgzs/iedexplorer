using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using IEDExplorer.Dialogs;
using System.Globalization;
using IEDExplorer.Resources;

namespace IEDExplorer.Views
{
    public partial class IedDataView : DockContent
    {
        delegate void OnValueCallback(object sender, EventArgs e);
        Env _env;
        NodeBase actualNode;
        public bool DoNotShowAUStoppedDialog { get; set; }

        public IedDataView()
        {
            _env = Env.getEnv();
            InitializeComponent();
            toolStripComboBox_autoUpdate.SelectedIndex = 2;
            DoNotShowAUStoppedDialog = false;
        }

        internal void SelectNode(TreeNode tn)
        {
            // Update logic
            NodeBase nb = (NodeBase)tn.Tag;
            if ((nb is NodeIed || nb.IsIecModel))
            {
                // Stop and disable AutoUpdate
                if (timer_Au.Enabled && !DoNotShowAUStoppedDialog)
                {
                    new AutoUpdateStoppedDialog(this).ShowDialog();
                }
                timer_Au.Enabled = false;
                toolStripButton_RunAu.Enabled = false;
                toolStripButton_StopAu.Enabled = false;
                toolStripButtonRefresh.Enabled = false;
            }
            else
            {
                if (timer_Au.Enabled == false)
                {
                    toolStripButton_RunAu.Enabled = true;
                    toolStripButton_StopAu.Enabled = false;
                    toolStripButtonRefresh.Enabled = true;
                }
            }

            foreach (ListViewItem l in this.listView_data.Items)
            {
                if (l.Tag is TreeNode)
                    if ((l.Tag as TreeNode).Tag is NodeData)
                        ((l.Tag as TreeNode).Tag as NodeData).ValueChanged -= new EventHandler(Node_ValueChanged);
            }
            this.listView_data.Items.Clear();
            ListViewItem li;
            actualNode = (NodeBase)tn.Tag;

            li = this.listView_data.Items.Add(makeRow(actualNode));
            li.Tag = tn;
            if (actualNode.GetChildNodes().Length > 0)
            {
                this.listView_data.Items.Add(new ListViewItem(new string[] { "------------- CHILD NODES -------------", "-------------", "-------------", "-------------" }));
                recursiveAddLine(actualNode, tn);
            }
        }

        void recursiveAddLine(NodeBase n, TreeNode tn)
        {
            if ((n.GetChildNodes().Length > 0))
            {
                if (n is NodeVL)
                {
                    foreach (NodeBase nb in n.GetChildNodes())
                    {
                        ListViewItem li = this.listView_data.Items.Add(makeRow(nb));
                        li.Tag = nb.Tag;
                    }
                }
                else
                {
                    if (tn.Text == "lists")
                    {
                        foreach (NodeBase nb in n.GetChildNodes())
                        {
                            ListViewItem li = this.listView_data.Items.Add(makeRow(nb));
                            li.Tag = nb.Tag;
                        }
                    }
                    if (n is NodeFile || tn.Text == "files")
                    {
                        foreach (NodeBase nb in n.GetChildNodes())
                        {
                            ListViewItem li = this.listView_data.Items.Add(makeRow(nb));
                            li.Tag = nb.Tag;
                        }
                    }
                    else
                    {
                        foreach (NodeBase nb in n.GetChildNodes())
                        {
                            recursiveAddLine(nb, tn);
                        }
                    }
                }
            }
            else
            {
                ListViewItem li = this.listView_data.Items.Add(makeRow(n));
                li.Tag = n.Tag;
            }
        }

        ListViewItem makeRow(NodeBase n)
        {
            if (n is NodeData)
            {
                string val = (n as NodeData).StringValue;
                ListViewItem lvi = new ListViewItem(new string[] { n.IecAddress, (n as NodeData).DataType.ToString(), val, "Dom = " + n.CommAddress.Domain + " Var = " + n.CommAddress.Variable });
                (n as NodeData).ValueChanged += new EventHandler(Node_ValueChanged);
                (n as NodeData).ValueTag = lvi;
                //lvi.SubItems[2].Text;
                return lvi;
            }
            else if (n is NodeVL)
            {
                return new ListViewItem(new string[] { n.IecAddress, n.ToString(), "", "Deletable = " + (n as NodeVL).Deletable.ToString() + ", " + "Defined = " + (n as NodeVL).Defined.ToString() });
            }
            else if (n is NodeFile)
            {
                string val;
                if ((n as NodeFile).isDir)
                    val = "Dir";
                else
                    val = "size=" + (n as NodeFile).ReportedSize.ToString("n0").Replace(NumberFormatInfo.CurrentInfo.NumberGroupSeparator, " ") + " byte, time=" + (n as NodeFile).ReportedTime.ToString();
                return new ListViewItem(new string[] { n.Name, n.ToString(), val, (n as NodeFile).FullName });
            }
            else if (n != null)
                return new ListViewItem(new string[] { n.IecAddress, n.ToString(), "", "Dom = " + n.CommAddress.Domain + " Var = " + n.CommAddress.Variable });
            return null;
        }

        void Node_ValueChanged(object sender, EventArgs e)
        {
            if (listView_data.InvokeRequired)
            {
                OnValueCallback d = new OnValueCallback(Node_ValueChanged);
                if (!this.Disposing)
                    this.Invoke(d, new object[] { sender, e });

            }
            else
            {
                ((sender as NodeData).ValueTag as ListViewItem).SubItems[2].Text = (sender as NodeData).StringValue;
                listView_data.Invalidate(((sender as NodeData).ValueTag as ListViewItem).SubItems[2].Bounds);
            }
        }

        private void IedDataView_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ListViewItem l in this.listView_data.Items)
            {
                if (l.Tag is TreeNode)
                    if ((l.Tag as TreeNode).Tag is NodeData)
                        ((l.Tag as TreeNode).Tag as NodeData).ValueChanged -= new EventHandler(Node_ValueChanged);
            }
            //worker.Stop();
        }

        private void listView_data_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView_data.SelectedItems)
            {
                if (li.Tag != null) (li.Tag as TreeNode).EnsureVisible();
            }
        }

        private void toolStripButton_RunAu_Click(object sender, EventArgs e)
        {
            setInterval_Au();
            timer_Au.Enabled = true;
            toolStripButton_RunAu.Enabled = false;
            toolStripButton_StopAu.Enabled = true;
            toolStripButtonRefresh.Enabled = false;
        }

        private void setInterval_Au()
        {
            int interval = 1000;
            if (!int.TryParse(toolStripComboBox_autoUpdate.Text, out interval)) toolStripComboBox_autoUpdate.Text = interval.ToString();
            if (interval < 100)
            {
                interval = 100;
                toolStripComboBox_autoUpdate.Text = interval.ToString();
            }
            timer_Au.Interval = interval;
        }

        private void toolStripButton_StopAu_Click(object sender, EventArgs e)
        {
            timer_Au.Enabled = false;
            toolStripButton_RunAu.Enabled = true;
            toolStripButton_StopAu.Enabled = false;
            toolStripButtonRefresh.Enabled = true;
        }

        private void timer_Au_Tick(object sender, EventArgs e)
        {
            // Issue reads
            if (actualNode == null) return;
            Iec61850State iecs = actualNode.GetIecs();
            NodeBase[] ndarr = null;// = new NodeBase[1];
            if (actualNode is NodeData || actualNode is NodeDO || actualNode is NodeFC || actualNode is NodeVL || actualNode is NodeRCB)
            {
                ndarr = new NodeBase[1];
                ndarr[0] = actualNode;
            }
            else if (actualNode is NodeLD || actualNode is NodeLN)
            {
                List<NodeBase> nblist = new List<NodeBase>();
                foreach (NodeBase nb in actualNode.GetChildNodes())
                {
                    if (actualNode is NodeLD)
                    {
                        // We are at LD level, we must go to FC level through LN level (grandchildren)
                        foreach (NodeBase nb2 in nb.GetChildNodes())
                        {
                            nblist.Add(nb2);
                        }
                    }
                    else
                    {
                        // We are at LN level, we go FC level / direct children
                        nblist.Add(nb);
                    }
                }
                ndarr = nblist.ToArray();
            }
            if (ndarr != null)
                iecs.Send(ndarr, actualNode.CommAddress, ActionRequested.Read);
        }

        private void toolStripComboBox_autoUpdate_TextChanged(object sender, EventArgs e)
        {
            setInterval_Au();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string filename = saveFileDialog.FileName;

            try
            {
                StreamWriter file = new StreamWriter(filename, false);
                List<string> line = new List<string>();
                foreach (ColumnHeader ch in listView_data.Columns)
                {
                    line.Add(ch.Text);
                    
                }
                writeLine(line, file);
                line.Clear();
                foreach (ListViewItem lvi in listView_data.Items)
                {
                    line.Add(lvi.Text);
                    foreach (ListViewItem.ListViewSubItem sublvi in lvi.SubItems)
                    {
                        line.Add(sublvi.Text);
                    }
                    writeLine(line, file);
                    line.Clear();
                }
                file.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Cannot open file " + filename + " for output! Detail: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        void writeLine(List<string> line, StreamWriter file)
        {
            for (int i = 0; i < line.Count; i++)
            {
                file.Write(line[i]);
                if (i != line.Count - 1) file.Write("\t");
            }
            file.WriteLine();
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            timer_Au_Tick(null, null);
        }

        private void listView_data_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenuStrip ctxmenu = new ContextMenuStrip();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resource1));
                ToolStripMenuItem itCopy = new ToolStripMenuItem("Copy Name to Clipboard", ((System.Drawing.Image)(resources.GetObject("page_copy"))), new EventHandler(ctxMenu_OnClick), Keys.Control | Keys.C);
                ctxmenu.Items.Add(itCopy);
                if (listView_data.SelectedItems.Count > 0)
                    ctxmenu.Show(listView_data, e.Location);
            }
        }

        private void copyNameToClipboard()
        {
            if (listView_data.SelectedItems.Count > 0)
            {
                string result = "";
                bool first = true;
                foreach (ListViewItem lvi in listView_data.SelectedItems)
                {
                    if (first) first = false; else result += "\n";
                    result += lvi.Text;
                }
                System.Windows.Forms.Clipboard.SetText(result);
            }
        }

        private void ctxMenu_OnClick(object sender, EventArgs e)
        {
            copyNameToClipboard();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C))
            {
                copyNameToClipboard();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            Dialogs.FindForm ff = new Dialogs.FindForm(listView_data);
            ff.ShowDialog();
        }
    }
}
