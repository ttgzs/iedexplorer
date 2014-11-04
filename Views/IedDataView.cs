using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace IEDExplorer.Views
{
    public partial class IedDataView : DockContent
    {
        delegate void OnValueCallback(object sender, EventArgs e);
        public Env environment;

        public IedDataView(Env env)
        {
            environment = env;
            InitializeComponent();
        }

        internal void SelectNode(TreeNode tn)
        {
            foreach (ListViewItem l in this.listView_data.Items)
            {
                if (l.Tag is TreeNode)
                    if ((l.Tag as TreeNode).Tag is NodeData)
                        ((l.Tag as TreeNode).Tag as NodeData).ValueChanged -= new EventHandler(Node_ValueChanged);
            }
            this.listView_data.Items.Clear();
            ListViewItem li;
            NodeBase n = (NodeBase)tn.Tag;

            li = this.listView_data.Items.Add(makeRow(n));
            li.Tag = tn;
            if (n.GetChildNodes().Length > 0)
            {
                this.listView_data.Items.Add(new ListViewItem(new string[] { "------------- CHILD NODES -------------", "-------------", "-------------", "-------------" }));
                recursiveAddLine(n, tn);
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
                ListViewItem lvi = new ListViewItem(new string[] { n.Address, (n as NodeData).DataType.ToString(), val, "Dom = " + n.CommAddress.Domain + " Var = " + n.CommAddress.Variable });
                (n as NodeData).ValueChanged += new EventHandler(Node_ValueChanged);
                (n as NodeData).ValueTag = lvi;
                //lvi.SubItems[2].Text;
                return lvi;
            }
            else if (n is NodeVL)
            {
                return new ListViewItem(new string[] { n.Address, n.ToString(), "", "Deletable = " + (n as NodeVL).Deletable.ToString() + ", " + "Defined = " + (n as NodeVL).Defined.ToString() });
            }
            else if (n is NodeFile)
            {
                string val;
                if ((n as NodeFile).isDir)
                    val = "Dir";
                else
                    val = (n as NodeFile).ReportedSize.ToString();
                return new ListViewItem(new string[] { n.Name, n.ToString(), val, (n as NodeFile).FullName });
            }
            else if (n != null)
                return new ListViewItem(new string[] { n.Address, n.ToString(), "", "Dom = " + n.CommAddress.Domain + " Var = " + n.CommAddress.Variable });
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

    }
}
