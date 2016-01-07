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
using System.Threading;
using System.IO;

namespace IEDExplorer.Views
{
    public partial class IedTreeView : DockContent
    {
        delegate void OnNodeCallback(Iec61850State iecs);
        delegate void OnValueCallback(object sender, EventArgs e);
        delegate void OnDirectoryCallback(object sender, EventArgs e);

        Rectangle dragBoxFromMouseDown;
        TreeNode nodeToDrag;
        TreeNode listsNode;
        WindowManager winMgr;
        Iec61850Controller ctrl = null;

        public IedTreeView(WindowManager wm)
        {
            winMgr = wm;
            InitializeComponent();
        }

        internal void makeTree(Iec61850State iecs)
        {
            ctrl = iecs.Controller;
            if (treeViewIed.InvokeRequired)
            {
                OnNodeCallback d = new OnNodeCallback(makeTree);
                this.Invoke(d, new object[] { iecs });
            }
            else
            {
                treeViewIed.ImageList = new ImageList();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resource1));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("computer"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("calculator"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("database"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text_width"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN1"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC1"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO1"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA1"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN2"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC2"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO2"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA2"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN3"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC3"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO3"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA3"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN4"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC4"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO4"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA4"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN5"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC5"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO5"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA5"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN6"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC6"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO6"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA6"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("folder"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_database"))));
                treeViewIed.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_put"))));
                treeViewIed.Nodes.Clear();
                TreeNode n = treeViewIed.Nodes.Add(iecs.DataModel.ied.Name + " = " + iecs.hostname +
                                                 ", Vendor = " + (iecs.DataModel.ied as NodeIed).VendorName +
                                                 ", Model = " + (iecs.DataModel.ied as NodeIed).ModelName +
                                                 ", Revision = " + (iecs.DataModel.ied as NodeIed).Revision +
                                                 ", DefineNVL = " + (iecs.DataModel.ied as NodeIed).DefineNVL
                                                 );
                NodeBase nb = iecs.DataModel.ied;
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
                    NodeBase lb = iecs.DataModel.lists.FindChildNode(b.Name);
                    if (lb != null)
                    {
                        tn3 = tn2.Nodes.Add("DataSets");
                        tn3.Tag = lb;
                        tn3.ImageIndex = 3;
                        tn3.SelectedImageIndex = 3;
                        makeTree_listNode(lb, tn3);
                    }
                    NodeBase ur = iecs.DataModel.urcbs.FindChildNode(b.Name);
                    if (ur != null)
                    {
                        tn3 = tn2.Nodes.Add("Unbuffered Reports");
                        tn3.Tag = ur;
                        tn3.ImageIndex = 3;
                        tn3.SelectedImageIndex = 3;
                        makeTree_reportNode(ur, tn3);
                    }
                    NodeBase br = iecs.DataModel.brcbs.FindChildNode(b.Name);
                    if (br != null)
                    {
                        tn3 = tn2.Nodes.Add("Buffered Reports");
                        tn3.Tag = br;
                        tn3.ImageIndex = 3;
                        tn3.SelectedImageIndex = 3;
                        makeTree_reportNode(br, tn3);
                    }
                }
                nb = iecs.DataModel.files;
                TreeNode tn4 = n.Nodes.Add("Files");
                tn4.Tag = iecs.DataModel.files;
                tn4.ImageIndex = 3;
                tn4.SelectedImageIndex = 3;
                nb.Tag = tn4;
                makeTree_fileNode(nb, tn4);
            }
        }

        void makeTree_dataNode(NodeBase nb, TreeNode tn)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
                Node_StateChanged(b, new EventArgs());
                b.StateChanged += new EventHandler(Node_StateChanged);

                if (b.FC.Count > 0)
                {
                    tn2.ToolTipText = "FC=";
                    for (int i = 0; i < b.FC.Count; i++)
                    {
                        tn2.ToolTipText += b.FC[i];
                        if (i != b.FC.Count - 1) tn2.ToolTipText += ",";
                    }
                }
                makeTree_dataNode(b, tn2);
            }
        }

        void makeTree_listNode(NodeBase nb, TreeNode tn)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
                tn2.ImageIndex = 4;
                tn2.SelectedImageIndex = 4;
                foreach (NodeBase b2 in b.GetChildNodes())
                {
                    TreeNode tn3 = tn2.Nodes.Add(b2.CommAddress.Variable);
                    tn3.Tag = b2;
                    tn3.ImageIndex = 7;
                    tn3.SelectedImageIndex = 7;
                    /*Node_StateChanged(b2, new EventArgs());
                    b2.StateChanged += new EventHandler(Node_StateChanged);*/
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
                tn2.ImageIndex = 4;
                tn2.SelectedImageIndex = 4;
                foreach (NodeBase b2 in b.GetChildNodes())
                {
                    TreeNode tn3 = tn2.Nodes.Add(b2.CommAddress.Variable);
                    tn3.Tag = b2;
                    tn3.ImageIndex = 7;
                    tn3.SelectedImageIndex = 7;
                    /*Node_StateChanged(b2, new EventArgs());
                    b2.StateChanged += new EventHandler(Node_StateChanged);*/
                }
            }
        }

        void makeTree_fileNode(NodeBase nb, TreeNode tn)
        {
            tn.Nodes.Clear();
            if ((nb is NodeIed) || (nb as NodeFile).isDir)
            {
                foreach (NodeBase b in nb.GetChildNodes())
                {
                    TreeNode tn2 = tn.Nodes.Add(b.Name);
                    tn2.Tag = b;
                    b.Tag = tn2;
                    if (b is NodeFile && (b as NodeFile).isDir)
                    {
                        tn2.ImageIndex = 29;
                        tn2.SelectedImageIndex = 29;
                    }
                    else
                    {
                            tn2.ImageIndex = 4;
                            tn2.SelectedImageIndex = 4;
                    }
                    (b as NodeFile).DirectoryUpdated += Node_DirectoryUpdated;
                    makeTree_fileNode(b, tn2);
                }
            }
            else
                updateTree_fileNode(nb, tn);
        }

        void updateTree_fileNode(NodeBase nb, TreeNode tn)
        {
            if ((nb as NodeFile).FileReady)
            {
                tn.ImageIndex = 31;
                tn.SelectedImageIndex = 31;
            }
            else if ((nb as NodeFile).FileSaved)
            {
                tn.ImageIndex = 30;
                tn.SelectedImageIndex = 30;
            }
            else
            {
                tn.ImageIndex = 4;
                tn.SelectedImageIndex = 4;
            }
        }

        public void Node_DirectoryUpdated(object sender, EventArgs e)
        {
            if (treeViewIed.InvokeRequired)
            {
                OnDirectoryCallback d = new OnDirectoryCallback(Node_DirectoryUpdated);
                if (!this.Disposing)
                    this.Invoke(d, new object[] { sender, e });

            }
            else
            {
                makeTree_fileNode((sender as NodeBase), (TreeNode)(sender as NodeBase).Tag);
            }
        }

        void Node_StateChanged(object sender, EventArgs e)
        {
            if (treeViewIed.InvokeRequired)
            {
                OnValueCallback d = new OnValueCallback(Node_StateChanged);
                if (!this.Disposing)
                    this.Invoke(d, new object[] { sender, e });

            }
            else
            {
                NodeBase b = (sender as NodeBase);
                if (b.Tag is TreeNode)
                {
                    TreeNode tn = (b.Tag as TreeNode);
                    int firsticon = 0;
                    if (b.GetType() == typeof(NodeLN))
                    {
                        firsticon = 5;
                    }
                    else if (b.GetType() == typeof(NodeFC))
                    {
                        firsticon = 6;
                    }
                    else if (b.GetType() == typeof(NodeDO))
                    {
                        firsticon = 7;
                    }
                    else if (b.GetType() == typeof(NodeData))
                    {
                        firsticon = 8;
                    }
                    int newIconIndex = firsticon + ((int)b.NodeState) * 4;
                    if (tn.ImageIndex != newIconIndex)
                    {
                        tn.ImageIndex = newIconIndex;
                        tn.SelectedImageIndex = newIconIndex;
                        treeViewIed.Invalidate(tn.Bounds);
                    }
                }
            }
        }

        private void treeViewIed_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node != null)
            {
                NodeBase n = (NodeBase)e.Node.Tag;

                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripItem item;

                if (n != null)
                {
                    if (n is NodeVL)
                    {

                        if ((n as NodeVL).Defined)
                        {
                            if ((n as NodeVL).Activated)
                            {
                                item = menu.Items.Add("Deactivate Reports");
                                item.Tag = n;
                                item.Click += new EventHandler(OnDeactivateNVLClick);
                            }
                            else
                            {
                                item = menu.Items.Add("Activate Reports");
                                item.Tag = n;
                                item.Click += new EventHandler(OnActivateNVLClick);
                            }
                        }
                        else
                        {
                            item = menu.Items.Add("Send Define DataSet Request");
                            item.Tag = n;
                            item.Click += new EventHandler(OnDefineNVLClick);
                        }
                        if ((n as NodeVL).Deletable && !(n as NodeVL).Activated)
                        {
                            item = menu.Items.Add("Delete DataSet");
                            item.Tag = n;
                            item.Click += new EventHandler(OnDeleteNVLClick);
                        }
                    }
                    else if (e.Node.Text == "DataSets" && n.GetIecs().DataModel.ied.DefineNVL)
                    {
                        item = menu.Items.Add("Add New DataSet");
                        item.Tag = n;
                        listsNode = e.Node;
                        item.Click += new EventHandler(OnAddNVLClick);
                    }
                    else if (e.Node.Text == "Files")
                    {
                        item = menu.Items.Add("Read File List");
                        item.Tag = n;
                        listsNode = e.Node;
                        item.Click += new EventHandler(OnFileListClick);
                    }
                    if (n is NodeFile && (n as NodeFile).isDir)
                    {
                        item = menu.Items.Add("Read File List");
                        item.Tag = n;
                        listsNode = e.Node;
                        item.Click += new EventHandler(OnFileListClick);
                    }
                    if (n is NodeFile && !(n as NodeFile).isDir && !(n as NodeFile).FileReady)
                    {
                        item = menu.Items.Add("Get File");
                        item.Tag = n;
                        listsNode = e.Node;
                        item.Click += new EventHandler(OnFileGetClick);
                    }
                    if (n is NodeFile && !(n as NodeFile).isDir && (n as NodeFile).FileReady)
                    {
                        item = menu.Items.Add("Save File");
                        item.Tag = n;
                        listsNode = e.Node;
                        item.Click += new EventHandler(OnFileSaveClick);
                    }
                    if (n is NodeData && n.Name == "ctlVal")
                    {
                        item = menu.Items.Add("Send Command (Writes)");
                        item.Tag = n;
                        item.Click += new EventHandler(OnSendCommandClick);
                        item = menu.Items.Add("Send Command (Structure)");
                        item.Tag = n;
                        item.Click += new EventHandler(OnSendCommandAsStructureClick);
                    }
                    if (n is NodeData || n is NodeDO || n is NodeFC || n is NodeVL)
                    {
                        item = menu.Items.Add("Read Data");
                        item.Tag = n;
                        item.Click += new EventHandler(OnReadDataClick);
                    }
                    if (n is NodeData && n.GetChildNodes().Length == 0)
                    {
                        item = menu.Items.Add("Write Data");
                        item.Tag = n;
                        item.Click += new EventHandler(OnWriteDataClick);
                    }
                    if (n is NodeRCB && n.GetChildNodes().Length > 0)
                    {
                        item = menu.Items.Add("Configure RCB");
                        item.Tag = n;
                        item.Click += new EventHandler(OnConfigureRcb);
                    }
                    if (n is NodeIed)
                    {
                        item = menu.Items.Add("Save model for libiec61850");
                        item.Tag = n;
                        item.Click += new EventHandler(OnSaveModel);
                    }
                }

                if (menu.Items.Count > 0)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }
                item = menu.Items.Add("Expand Subtree");
                item.Tag = e.Node;
                item.Click += new EventHandler(OnExpandSubtree);

                item = menu.Items.Add("Collapse Subtree");
                item.Tag = e.Node;
                item.Click += new EventHandler(OnCollapseSubtree);

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

        void OnSaveModel(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "cfg files (*.cfg)|*.cfg";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string filename = saveFileDialog.FileName;

            try
            {
                StreamWriter file = new StreamWriter(filename, false);
                List<string> lines = new List<string>();
                NodeIed n = (NodeIed)(sender as ToolStripItem).Tag;
                n.SaveModel(lines, false);
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
                file.Close();
                MessageBox.Show("Model successfully exported! To " + filename, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Cannot open file " + filename + " for output! Detail: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        void OnFileListClick(object sender, EventArgs e)
        {
            ctrl.GetFileList((NodeBase)(sender as ToolStripItem).Tag);
        }

        void OnFileGetClick(object sender, EventArgs e)
        {
            ctrl.GetFile((NodeFile)(sender as ToolStripItem).Tag);
        }

        void OnFileSaveClick(object sender, EventArgs e)
        {
            NodeFile nfi = (NodeFile)(sender as ToolStripItem).Tag;
            SaveFileDialog fd = new SaveFileDialog();
            fd.FileName = nfi.Name;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nfi.SaveFile(fd.FileName);
                    nfi.FileReady = false;
                }
                catch
                {
                    MessageBox.Show("An error saving MMS file " + nfi.FullName + " to file " + fd.FileName);
                }
            }
        }

        void OnSendCommandClick(object sender, EventArgs e)
        {
            NodeBase data = (NodeBase)(sender as ToolStripItem).Tag;
            CommandParams cPar = ctrl.PrepareSendCommand((NodeBase)(sender as ToolStripItem).Tag);
            if (cPar != null)
            {
                CommandDialog cdlg = new CommandDialog(cPar);
                if (cdlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ctrl.SendCommand(data, cPar, ActionRequested.Write);
                }
            }
        }

        void OnSendCommandAsStructureClick(object sender, EventArgs e)
        {
            NodeBase data = (NodeBase)(sender as ToolStripItem).Tag;
            CommandParams cPar = ctrl.PrepareSendCommand((NodeBase)(sender as ToolStripItem).Tag);
            if (cPar != null)
            {
                CommandDialog cdlg = new CommandDialog(cPar);
                if (cdlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ctrl.SendCommand(data, cPar, ActionRequested.WriteAsStructure);
                }
            }
        }

        void OnReadDataClick(object sender, EventArgs e)
        {
            ctrl.ReadData((NodeBase)(sender as ToolStripItem).Tag);
        }

        void OnWriteDataClick(object sender, EventArgs e)
        {
            NodeData data = (NodeData)(sender as ToolStripItem).Tag;
            NodeData newdata = ctrl.PrepareWriteData(data);

            EditValue ev = new EditValue(newdata);
            DialogResult r = ev.ShowDialog();
            if (r == DialogResult.OK)
            {
                data.DataValue = newdata.DataValue;
                ctrl.WriteData(data, true);
            }
        }

        void OnConfigureRcb(object sender, EventArgs e)
        {
            NodeRCB rcb = (NodeRCB)(sender as ToolStripItem).Tag;
            RcbActivateParams par=new RcbActivateParams();
            par.self = rcb;

            RcbActivateDialog rad = new RcbActivateDialog(par);
            DialogResult r = rad.ShowDialog();
            if (r == DialogResult.OK)
            {
                ctrl.WriteRcb(par, true);
            }
        }

        void OnAddNVLClick(object sender, EventArgs e)
        {
            NodeBase lists = (NodeBase)(sender as ToolStripItem).Tag;
            NodeVL newnode = new NodeVL("NewDataSet");
            newnode.Parent = lists;
            AddNVLDialog d = new AddNVLDialog(newnode, lists, listsNode, OnNVListChanged);
            d.Show();
        }

        void OnNVListChanged(object sender, EventArgs e)
        {
            TreeNode n = new TreeNode((sender as AddNVLDialog).List.Name);
            n.Tag = (sender as AddNVLDialog).List;
            (sender as AddNVLDialog).List.Tag = n;
            ((sender as AddNVLDialog).ListsNode).Nodes.Add(n);
            foreach (NodeBase nb in (sender as AddNVLDialog).List.GetChildNodes())
            {
                TreeNode tn = new TreeNode(nb.CommAddress.Variable);
                tn.Tag = nb;
                n.Nodes.Add(tn);
            }
        }

        void OnDefineNVLClick(object sender, EventArgs e)
        {
            ctrl.DefineNVL((NodeVL)(sender as ToolStripItem).Tag);
        }

        void OnDeleteNVLClick(object sender, EventArgs e)
        {
            NodeVL nvl = (NodeVL)(sender as ToolStripItem).Tag;
            if (nvl.Defined)
            {
                ctrl.DeleteNVL(nvl);
            }
            // TODO Propagate from SCSM after! delete acknowledged!
            (nvl.Tag as TreeNode).Remove();
            nvl.Remove();

        }

        void OnActivateNVLClick(object sender, EventArgs e)
        {
            ctrl.ActivateNVL((NodeVL)(sender as ToolStripItem).Tag);
        }

        void OnDeactivateNVLClick(object sender, EventArgs e)
        {
            ctrl.DeactivateNVL((NodeVL)(sender as ToolStripItem).Tag);
        }

        private void treeViewIed_MouseDown(object sender, MouseEventArgs e)
        {
            nodeToDrag = ((TreeView)sender).GetNodeAt(e.Location);
            if (e.Button == MouseButtons.Left && nodeToDrag != null && nodeToDrag.Tag is NodeData)
            {
                // Remember the point where the mouse down occurred. The DragSize indicates
                // the size that the mouse can move before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void treeViewIed_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    ((TreeView)sender).DoDragDrop(nodeToDrag.Tag, DragDropEffects.All | DragDropEffects.Link);
                }
        }

        private void treeViewIed_MouseUp(object sender, MouseEventArgs e)
        {
            // Reset the drag rectangle when the mouse button is raised.
            dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void treeViewIed_AfterSelect(object sender, TreeViewEventArgs e)
        {
            winMgr.SelectNode(e.Node);
        }

    }
}
