/*
 *  Copyright (C) 2013 Pavel Charvat
 * 
 *  This file is part of IEDExplorer.
 *
 *  IEDExplorer is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IEDExplorer is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IEDExplorer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace IEDExplorer
{
    public partial class AddNVLView : DockContent
    {

        NodeVL list;
        NodeBase lists;
        TreeNode listsNode;

        event EventHandler OnNVListChanged;

        private AddNVLView()
        {
            InitializeComponent();
        }

        internal AddNVLView(NodeVL list, NodeBase lists, TreeNode listsNode, EventHandler onNVListChanged)
        {
            InitializeComponent();
            this.list = list;
            this.lists = lists;
            this.listsNode = listsNode;
            this.OnNVListChanged += onNVListChanged;

            this.textBox1.Text = list.Name;
            foreach (NodeBase b in list.GetChildNodes())
            {
                ListViewItem it = this.listView1.Items.Add(new ListViewItem(b.Address));
                it.Tag = b;
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof(NodeData))) || (e.Data.GetDataPresent(typeof(NodeDO))))
            {
                e.Effect = DragDropEffects.Link;
            }
       }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            NodeBase d;
            if ((d = (NodeBase)e.Data.GetData(typeof(NodeData))) != null || (d = (NodeBase)e.Data.GetData(typeof(NodeDO))) != null)
            {
                ListViewItem lvi = new ListViewItem(d.Address);
                lvi.Tag = d;
                int i = 0;
                for (; i < listView1.Items.Count; i++)
                {
                    if (lvi.Tag == listView1.Items[i].Tag)
                        break;
                }
                if (i == listView1.Items.Count)
                   listView1.Items.Add(lvi);
                
            }

        }

        internal NodeVL List { get { return list; } }
        internal NodeBase Lists { get { return lists; } }
        internal TreeNode ListsNode { get { return listsNode; } }

        private void toolStripButtonOK_Click(object sender, EventArgs e)
        {
            NodeVL existingNode = (NodeVL)lists.FindChildNode(this.textBox1.Text);
            if ( null != existingNode)
            {
                // List name already exist
                if (existingNode.Defined)
                {
                    MessageBox.Show("Cannot change the NVL " + existingNode.Name + ", already defined on the server. Change the name.");
                    return;
                }
            }
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("Empty NVL Name, cannot add it to the local tree. Change the name.");
                return;
            }
            NodeVL newlist = new NodeVL(textBox1.Text);
            newlist.Tag = list.Tag;
            if (newlist.Tag != null && newlist.Tag is TreeNode && (newlist.Tag as TreeNode).Tag == list)
                (newlist.Tag as TreeNode).Tag = newlist;
            newlist.urcb = list.urcb;
            newlist.Parent = list.Parent;
            newlist.Deletable = true;
            foreach (ListViewItem it in listView1.Items)
            {
                newlist.LinkChildNodeByAddress(it.Tag as NodeBase);
            }
            list = newlist;
            if (OnNVListChanged != null)
            {
                OnNVListChanged(this, new EventArgs());
            }
            this.Close();

        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //NodeBase n = (NodeBase)e.Node.Tag;
                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripItem item;

                item = menu.Items.Add("Remove");
                //item.Tag = listView1.SelectedItems[0].Tag;
                item.Click += new EventHandler(OnRemoveClick);

                menu.Show((Control)sender, e.Location);
            }
        }

        void OnRemoveClick(object sender, EventArgs e)
        {
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

    }
}
