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
    public partial class WatchDataView : DockContent
    {
        delegate void OnValueCallback(object sender, EventArgs e);
        public Env environment;

        public WatchDataView(Env env)
        {
            environment = env;
            InitializeComponent();
        }

        void Node_ValueChanged(object sender, EventArgs e)
        {
            if (treeViewAdv_data.InvokeRequired)
            {
                OnValueCallback d = new OnValueCallback(Node_ValueChanged);
                if (!this.Disposing)
                    this.Invoke(d, new object[] { sender, e });

            }
            else
            {
                ((sender as NodeData).ValueTag as ListViewItem).SubItems[2].Text = (sender as NodeData).StringValue;
                //treeViewAdv_data.Invalidate(((sender as NodeData).ValueTag as ListViewItem).SubItems[2].Bounds);
            }
        }

        private void IedDataView_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*foreach (ListViewItem l in this.treeViewAdv_data.Items)
            {
                if (l.Tag is TreeNode)
                    if ((l.Tag as TreeNode).Tag is NodeData)
                        ((l.Tag as TreeNode).Tag as NodeData).ValueChanged -= new EventHandler(Node_ValueChanged);
            }*/
        }

        private void treeViewAdv1_Click(object sender, EventArgs e)
        {

        }

        private void treeViewAdv1_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof(NodeData))) || (e.Data.GetDataPresent(typeof(NodeDO))))
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private void treeViewAdv1_DragDrop(object sender, DragEventArgs e)
        {
            NodeBase d;
            if ((d = (NodeBase)e.Data.GetData(typeof(NodeData))) != null || (d = (NodeBase)e.Data.GetData(typeof(NodeDO))) != null)
            {
                ListViewItem lvi = new ListViewItem(d.IecAddress);
                lvi.Tag = d;
                /*int i = 0;
                for (; i < treeViewAdv1.Items.Count; i++)
                {
                    if (lvi.Tag == treeViewAdv1.Items[i].Tag)
                        break;
                }
                if (i == treeViewAdv1.Items.Count)
                    treeViewAdv1.Items.Add(lvi);*/

            }

        }
    }
}
