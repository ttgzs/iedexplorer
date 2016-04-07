/*
 *  Copyright (C) 2016 Pavel Charvat
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
 * 
 *  SCLParser.cs was created by Joel Kaiser as an add-in to IEDExplorer.
 *  This class parses a SCL-type XML file to create a logical tree similar
 *  to that which Pavel creates from communication with an actual device.
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
using IEDExplorer.Resources;
using IEC61850.Server;
using System.Threading;

namespace IEDExplorer.Views
{
    public partial class SCLView : DockContent
    {
        string filename;
        string filename_short;
        List<Iec61850Model> dataModels;
        public string Filename { get { return filename; } }
        private int _currentRow = -1;
        public Env env;
        Dictionary<NodeIed, SCLServer> runningServers = new Dictionary<NodeIed, SCLServer>();

        public SCLView(string fname, Env envir)
        {
            filename = fname;
            env = envir;
            InitializeComponent();
            this.toolStrip1.Renderer = new MyRenderer();
            //try
            {
                //dataModels = new SCLParser().CreateTree(filename);
                dataModels = new SCLParserDOM().CreateTree(filename);
            }
            //catch (Exception e)
            {
            //    Logger.getLogger().LogError(" Reading SCL file: " + filename + ": " + e.Message);
            }
            IedTreeView.makeImageList(treeViewSCL);
            IedTreeView.makeImageList(treeViewSCL_IEC);
            makeTreeScl(dataModels[0], treeViewSCL);
            makeTreeScl(dataModels[0], treeViewSCL_IEC);
            foreach (Iec61850Model dataModel in dataModels)
            {
                makeTreeIed(dataModel);
                makeTreeIec(dataModel);
            }
            string[] fparts = filename.Split(new char[] { '/', '\\' });
            filename_short  = fparts[fparts.Length - 1];
            this.Text = filename_short;
        }

        internal void makeTreeScl(Iec61850Model dataModel, TreeView treeViewSCL)
        {
            TreeNode n = treeViewSCL.Nodes.Add("SCL = " + filename);
            n.ImageIndex = 4;
            n.SelectedImageIndex = 4;

            TreeNode tn5 = n.Nodes.Add("Enums");
            tn5.Tag = dataModel.enums;
            tn5.ImageIndex = 3;
            tn5.SelectedImageIndex = 3;
            makeTree_enumNode(dataModel.enums, tn5);
        }

        internal void makeTreeIed(Iec61850Model dataModel)
        {
            TreeNode n = treeViewSCL.Nodes.Add(dataModel.ied.Name + // " = " + filename +
                                             " = Vendor = " + (dataModel.ied as NodeIed).VendorName +
                                             ", Model = " + (dataModel.ied as NodeIed).ModelName +
                                             ", Revision = " + (dataModel.ied as NodeIed).Revision +
                                             ", DefineNVL = " + (dataModel.ied as NodeIed).DefineNVL
                                             );
            NodeBase nb = dataModel.ied;
            n.Tag = nb;
            n.ImageIndex = 0;
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = n.Nodes.Add(b.Name);
                tn2.Tag = b;
                tn2.ImageIndex = 1;
                tn2.SelectedImageIndex = 1;
                TreeNode tn3 = tn2.Nodes.Add("Data");
                tn3.Tag = b;
                tn3.ImageIndex = 2;
                tn3.SelectedImageIndex = 2;
                makeTree_dataNode(b, tn3);
                NodeBase lb = dataModel.lists.FindChildNode(b.Name);
                if (lb != null)
                {
                    tn3 = tn2.Nodes.Add("DataSets");
                    tn3.Tag = lb;
                    tn3.ImageIndex = 3;
                    tn3.SelectedImageIndex = 3;
                    makeTree_listNode(lb, tn3);
                }
                NodeBase ur = dataModel.urcbs.FindChildNode(b.Name);
                if (ur != null)
                {
                    tn3 = tn2.Nodes.Add("Unbuffered Reports");
                    tn3.Tag = ur;
                    tn3.ImageIndex = 3;
                    tn3.SelectedImageIndex = 3;
                    makeTree_reportNode(ur, tn3);
                }
                NodeBase br = dataModel.brcbs.FindChildNode(b.Name);
                if (br != null)
                {
                    tn3 = tn2.Nodes.Add("Buffered Reports");
                    tn3.Tag = br;
                    tn3.ImageIndex = 3;
                    tn3.SelectedImageIndex = 3;
                    makeTree_reportNode(br, tn3);
                }
            }
        }

        internal void makeTreeIec(Iec61850Model dataModel)
        {
            TreeNode n = treeViewSCL_IEC.Nodes.Add(dataModel.iec.Name + //" = " + iecs.hostname +
                                             " = Vendor = " + (dataModel.iec as NodeIed).VendorName +
                                             ", Model = " + (dataModel.iec as NodeIed).ModelName +
                                             ", Revision = " + (dataModel.iec as NodeIed).Revision +
                                             ", DefineNVL = " + (dataModel.iec as NodeIed).DefineNVL
                                             );
            NodeBase nb = dataModel.iec;
            n.Tag = nb;
            n.ImageIndex = 0;
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn3 = n.Nodes.Add(b.Name);
                tn3.Tag = b;
                tn3.ImageIndex = 1;
                tn3.SelectedImageIndex = 1;
                makeTreeIec_dataNode(b, tn3);
            }
        }

        void makeTree_dataNode(NodeBase nb, TreeNode tn)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                string name = b.Name;
                if (b is NodeRCB || b is NodeVL)
                    name = b.Name.Substring(b.Name.LastIndexOf("$") + 1);
                //if (b is NodeData && (b as NodeData).FCDesc != null && (b as NodeData).FCDesc != "") name += " [" + (b as NodeData).FCDesc + "]";
                TreeNode tn2 = tn.Nodes.Add(name);
                tn2.Tag = b;
                b.Tag = tn2;
                Node_SetIcon(b, tn2);
                if (b is NodeRCB)
                {
                    if ((b as NodeRCB).isBuffered)
                    {
                        tn2.ImageIndex = 33;
                        tn2.SelectedImageIndex = 33;
                    }
                    else
                    {
                        tn2.ImageIndex = 32;
                        tn2.SelectedImageIndex = 32;
                    }
                }
                if (b is NodeVL)
                {
                    tn2.ImageIndex = 34;
                    tn2.SelectedImageIndex = 34;
                }
                makeTree_dataNode(b, tn2);
            }
        }

        void makeTreeIec_dataNode(NodeBase nb, TreeNode tn)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                string name = b.Name;
                if (b is NodeRCB || b is NodeVL)
                    name = b.Name.Substring(b.Name.LastIndexOf("$") + 1);
                if (b is NodeData && (b as NodeData).SCL_FCDesc != null && (b as NodeData).SCL_FCDesc != "") name += " [" + (b as NodeData).SCL_FCDesc + "]";
                TreeNode tn2 = tn.Nodes.Add(name);
                tn2.Tag = b;
                b.TagR = tn2;
                //Node_StateChanged(b, new EventArgs());
                Node_SetIcon(b, tn2);
                if (b is NodeRCB)
                {
                    if ((b as NodeRCB).isBuffered)
                    {
                        tn2.ImageIndex = 33;
                        tn2.SelectedImageIndex = 33;
                    }
                    else
                    {
                        tn2.ImageIndex = 32;
                        tn2.SelectedImageIndex = 32;
                    }
                }
                if (b is NodeVL)
                {
                    tn2.ImageIndex = 34;
                    tn2.SelectedImageIndex = 34;
                }
                makeTreeIec_dataNode(b, tn2);
            }
        }

        void makeTree_listNode(NodeBase nb, TreeNode tn)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
                tn2.ImageIndex = 34;
                tn2.SelectedImageIndex = 34;
                foreach (NodeBase b2 in b.GetChildNodes())
                {
                    TreeNode tn3 = tn2.Nodes.Add(b2.CommAddress.Variable);
                    tn3.Tag = b2;
                    tn3.ImageIndex = 7;
                    tn3.SelectedImageIndex = 7;
                }
            }
        }

        void makeTree_reportNode(NodeBase nb, TreeNode tn)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
                tn2.ImageIndex = 32;
                tn2.SelectedImageIndex = 32;
                if (b is NodeRCB)
                {
                    if ((b as NodeRCB).isBuffered)
                    {
                        tn2.ImageIndex = 33;
                        tn2.SelectedImageIndex = 33;
                    }
                }
                foreach (NodeBase b2 in b.GetChildNodes())
                {
                    TreeNode tn3 = tn2.Nodes.Add(b2.CommAddress.Variable);
                    tn3.Tag = b2;
                    tn3.ImageIndex = 7;
                    tn3.SelectedImageIndex = 7;
                }
            }
        }

        private void makeTree_enumNode(NodeBase nb, TreeNode tn)
        {
            if (nb == null) return;
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
                tn2.ImageIndex = 3;
                tn2.SelectedImageIndex = 3;
                foreach (NodeBase b2 in b.GetChildNodes())
                {
                    TreeNode tn3 = tn2.Nodes.Add(String.Concat(b2.Name));
                    tn3.Tag = b2;
                    tn3.ImageIndex = 3;
                    tn3.SelectedImageIndex = 3;
                }
            }
        }

        void Node_SetIcon(NodeBase b, TreeNode tn)
        {
                //if (b.Tag is TreeNode)
                //{
                //    TreeNode tn = (b.Tag as TreeNode);
                    int firsticon = 0;
                    if (b is NodeLN)
                    {
                        firsticon = 5;
                    }
                    else if (b is NodeFC)
                    {
                        firsticon = 6;
                    }
                    else if (b is NodeDO)
                    {
                        firsticon = 7;
                    }
                    else if (b is NodeData)
                    {
                        firsticon = 8;
                    }
                    else if (b is NodeRCB)
                    {
                        if ((b as NodeRCB).isBuffered)
                            firsticon = 33;
                        else
                            firsticon = 32;
                    }
                    else if (b is NodeVL)
                    {
                        firsticon = 34;
                    }
                    int newIconIndex = firsticon + ((int)b.NodeState) * 4;
                    tn.ImageIndex = newIconIndex;
                    tn.SelectedImageIndex = newIconIndex;
                    treeViewSCL.Invalidate(tn.Bounds);
                //}
        }

        private void dataGridView_data_SelectionChanged(object sender, EventArgs e)
        {

            if (dataGridView_data.CurrentRow.Index != _currentRow)
            {
                foreach (DataGridViewCell cell in dataGridView_data.SelectedCells)
                {
                    if (dataGridView_data.Rows[cell.RowIndex].Tag != null)
                        (dataGridView_data.Rows[cell.RowIndex].Tag as TreeNode).EnsureVisible();
                }
                _currentRow = dataGridView_data.CurrentRow.Index;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dataGridView_data.Rows.Clear();
            var n = (NodeBase)e.Node.Tag;
            if (n == null) return;

            var row = dataGridView_data.Rows.Add(makeRow(n));
            dataGridView_data.Rows[row].Tag = e.Node;
            if (n.GetChildNodes().Length > 0)
            {
                dataGridView_data.Rows.Add(
                    new string[] { "------------- CHILD NODES -------------", "-------------", "-------------", "-------------", "-------------", "-------------" });
                recursiveAddLine(n, e.Node);
            }
            dataGridView_data.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells | DataGridViewAutoSizeColumnsMode.ColumnHeader);
        }

        private void recursiveAddLine(NodeBase n, TreeNode tn)
        {
            if ((n.GetChildNodes().Length > 0))
            {
                if (n is NodeVL)
                {
                    foreach (NodeBase nb in n.GetChildNodes())
                    {
                        var li = dataGridView_data.Rows.Add(makeRow(nb));
                        dataGridView_data.Rows[li].Tag = n.Tag;
                    }
                }
                else
                {
                    if (tn.Text == "lists")
                    {
                        foreach (NodeBase nb in n.GetChildNodes())
                        {
                            var li = dataGridView_data.Rows.Add(makeRow(nb));
                            dataGridView_data.Rows[li].Tag = n.Tag;
                        }
                    }
                    if (n is NodeFile || tn.Text == "files")
                    {
                        foreach (NodeBase nb in n.GetChildNodes())
                        {
                            var li = dataGridView_data.Rows.Add(makeRow(nb));
                            dataGridView_data.Rows[li].Tag = n.Tag;
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
                var li = dataGridView_data.Rows.Add(makeRow(n));
                dataGridView_data.Rows[li].Tag = n.Tag;
            }
        }

        private string[] makeRow(NodeBase n)
        {
            if (n is NodeData)
            {
                string val = (n as NodeData).StringValue;
                string type = String.IsNullOrWhiteSpace((n as NodeData).SCL_BType)
                    ? (n as NodeData).DataType.ToString()
                    : (n as NodeData).SCL_BType;
                var dgvr =
                    new string[]
                    {
                        n.IecAddress, type, val,
                        n.CommAddress.Domain, n.CommAddress.LogicalNode, n.CommAddress.VariablePath
                    };
                (n as NodeData).ValueTag = dgvr;
                return dgvr;
            }
            else if (n is NodeVL)
            {
                return
                    new string[]
                    {
                        n.IecAddress, n.ToString(), "",
                        "Deletable = " + (n as NodeVL).Deletable.ToString() + ", " + "Defined = " +
                        (n as NodeVL).Defined.ToString()
                    };
            }
            else if (n is NodeFile)
            {
                string val;
                if ((n as NodeFile).isDir)
                    val = "Dir";
                else
                    val = (n as NodeFile).ReportedSize.ToString();
                return new string[] { n.Name, n.ToString(), val, (n as NodeFile).FullName };
            }
            else if (n != null)
                return
                    new string[] { n.IecAddress, n.ToString(), "", n.CommAddress.Domain, n.CommAddress.LogicalNode, n.CommAddress.VariablePath };
            return null;
        }

        private void toolStripButtonCollapseAll_Click(object sender, EventArgs e)
        {
            dataGridView_data.Rows.Clear();
            treeViewSCL.CollapseAll();
            dataGridView_data.Focus();
        }

        private void treeViewSCL_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node != null)
            {
                NodeBase n = (NodeBase)e.Node.Tag;

                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripItem item;

                if (n != null)
                {
                    if (n is NodeIed)
                    {
                        if (runningServers.ContainsKey((NodeIed)n))
                        {
                            item = menu.Items.Add("Stop SCLServer");
                            item.Tag = n;
                            item.Click += new EventHandler(OnStopServer);
                        }
                        else
                        {
                            item = menu.Items.Add("Run SCLServer");
                            item.Tag = n;
                            item.Click += new EventHandler(OnRunServer);
                        }
                    }
                    if (n is NodeData && n.isLeaf() && n.SCLServerModelObject != null)
                    {
                        //if (
                        item = menu.Items.Add("Write Data");
                        item.Tag = n;
                        item.Click += new EventHandler(OnWriteDataClick);
                    }
                }
                if (!n.isLeaf())
                {
                    if (menu.Items.Count > 0 && !n.isLeaf())
                        menu.Items.Add(new ToolStripSeparator());

                    item = menu.Items.Add("Expand Subtree");
                    item.Tag = e.Node;
                    item.Click += new EventHandler(OnExpandSubtree);

                    item = menu.Items.Add("Collapse Subtree");
                    item.Tag = e.Node;
                    item.Click += new EventHandler(OnCollapseSubtree);
                }
                if (menu.Items.Count > 0)
                    menu.Show((Control)sender, e.Location);
            }
        }

        void OnExpandSubtree(object sender, EventArgs e)
        {
            ExpandNode(((sender as ToolStripItem).Tag as TreeNode));
            ((sender as ToolStripItem).Tag as TreeNode).EnsureVisible();
        }

        void ExpandNode(TreeNode node)
        {
            node.Expand();
            foreach (TreeNode tn in node.Nodes)
            {
                ExpandNode(tn);
            }
        }

        void OnCollapseSubtree(object sender, EventArgs e)
        {
            CollapseNode(((sender as ToolStripItem).Tag as TreeNode));
            ((sender as ToolStripItem).Tag as TreeNode).EnsureVisible();
        }

        void CollapseNode(TreeNode node)
        {
            node.Collapse();
            foreach (TreeNode tn in node.Nodes)
            {
                CollapseNode(tn);
            }
        }

        void OnRunServer(object sender, EventArgs e)
        {
            NodeIed n = (NodeIed)(sender as ToolStripItem).Tag;
            foreach (Iec61850Model m in dataModels)
            {
                if (m.ied.Name == n.Name)
                {
                    SCLServer s = new SCLServer(env);
                    runningServers.Add(n, s);
                    int port = getFreePort();
                    s.Start(m, port);
                    m.iec.SCLServerRunning = s;
                    addStartedServer(port);
                }
            }
        }

        void OnStopServer(object sender, EventArgs e)
        {
            NodeIed n = (NodeIed)(sender as ToolStripItem).Tag;
            if (runningServers.ContainsKey(n))
            {
                int port = runningServers[n].TcpPort;
                runningServers[n].Stop();
                n.SCLServerRunning = null;
                runningServers.Remove(n);
                removeStartedServer(port);
            }
        }

        public void StopServers()
        {
            foreach (NodeIed n in runningServers.Keys)
            {
                int port = runningServers[n].TcpPort;
                runningServers[n].Stop();
                n.SCLServerRunning = null;
            }
            runningServers.Clear();
            toolStripLabelServers.BackColor = Color.Yellow;
            toolStripLabelServers.Text = "SCL Servers not running";
        }

        void OnWriteDataClick(object sender, EventArgs e)
        {
            NodeData data = (NodeData)(sender as ToolStripItem).Tag;

            EditValue ev = new EditValue(data);
            DialogResult r = ev.ShowDialog();
            if (r == DialogResult.OK)
            {
                if (data != null && data.GetIedNode() != null && data.GetIedNode().SCLServerRunning != null)
                    data.GetIedNode().SCLServerRunning.UpdateServerData(data, null, false, ev.timeNode, ev.UpdateTimestamp);
            }
        }

    /*private static void UpdateServerData(NodeData data, NodeData time, bool updateTimestamp)
        {
            DataAttribute da = (DataAttribute)data.SCLServerModelObject;
            IedServer iedSvr = data.GetIedNode().SCLServerRunning.GetIedServer();
            if (da != null && iedSvr != null)
            {
                iedSvr.LockDataModel();
                da.UpdateValue(iedSvr, data.DataValue);
                if (time != null && updateTimestamp)
                {
                    DataAttribute dat = (DataAttribute)time.SCLServerModelObject;
                    if (dat != null)
                    {
                        dat.UpdateValue(iedSvr, Util.GetTimeInMs());
                    }
                }
                iedSvr.UnlockDataModel();
            }
        }*/

        int getFreePort()
        {
            int i = 102;
            while (env.winMgr.SCLServers_usedPorts.Contains(i)) i++;
            return i;
        }

        void addStartedServer(int port)
        {
            env.winMgr.SCLServers_usedPorts.Add(port);
            toolStripLabelServers.Text = "SCL Servers running, ports: ";
            foreach (int p in env.winMgr.SCLServers_usedPorts)
            {
                toolStripLabelServers.Text += p.ToString() + " ";
            }
            toolStripLabelServers.BackColor = Color.LightGreen;
        }

        void removeStartedServer(int port)
        {
            env.winMgr.SCLServers_usedPorts.Remove(port);
            toolStripLabelServers.Text = "SCL Servers running, ports: ";
            foreach (int p in env.winMgr.SCLServers_usedPorts)
            {
                toolStripLabelServers.Text += p.ToString() + " ";
            }
            if (env.winMgr.SCLServers_usedPorts.Count == 0)
            {
                toolStripLabelServers.BackColor = Color.Yellow;
                toolStripLabelServers.Text = "SCL Servers not running";
            }
        }

        // For ToolStripLabel background color
        private class MyRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
            {
                using (var brush = new SolidBrush(e.Item.BackColor))
                {
                    e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
                }
            }
        }
    }

}
