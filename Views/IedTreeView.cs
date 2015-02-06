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
        long m_ctlNum = 0;
        WindowManager winMgr;

        public IedTreeView(WindowManager wm)
        {
            winMgr = wm;
            InitializeComponent();
        }

        internal void makeTree(Iec61850State iecs)
        {
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
                    NodeBase rb = iecs.DataModel.reports.FindChildNode(b.Name);
                    if (rb != null)
                    {
                        tn3 = tn2.Nodes.Add("Reports");
                        tn3.Tag = rb;
                        tn3.ImageIndex = 3;
                        tn3.SelectedImageIndex = 3;
                        makeTree_reportNode(rb, tn3);
                    }
                }
                nb = iecs.DataModel.files;
                TreeNode tn4 = n.Nodes.Add("Files");
                tn4.Tag = iecs.DataModel.files;
                tn4.ImageIndex = 3;
                tn4.SelectedImageIndex = 3;
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

        void Node_DirectoryUpdated(object sender, EventArgs e)
        {
            if (treeViewIed.InvokeRequired)
            {
                OnDirectoryCallback d = new OnDirectoryCallback(Node_DirectoryUpdated);
                if (!this.Disposing)
                    this.Invoke(d, new object[] { sender, e });

            }
            else
            {
                makeTree_fileNode((sender as NodeFile), (TreeNode)(sender as NodeBase).Tag);
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
                    /*else if (b.GetType() == typeof(NodeData))
                    {
                        if (b.GetChildNodes().Length == 0)
                        { // Leaf
                            firsticon = 8;
                        }
                        else
                        {
                            firsticon = 7;
                        }
                    }*/
                    else if ((b.GetType() == typeof(NodeData)) || (b.GetType() == typeof(NodeDO)))
                    {
                        if (b.GetChildNodes().Length == 0)
                        {
                            // Leaf
                            firsticon = 8;
                        }
                        else
                        {
                            firsticon = 7;
                        }
                    }
                    tn.ImageIndex = firsticon + ((int)b.NodeState) * 4;
                    tn.SelectedImageIndex = firsticon + ((int)b.NodeState) * 4;
                    treeViewIed.Invalidate(tn.Bounds);
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
                            item = menu.Items.Add("Send Define Report Request");
                            item.Tag = n;
                            item.Click += new EventHandler(OnDefineNVLClick);
                        }
                        if ((n as NodeVL).Deletable)
                        {
                            item = menu.Items.Add("Delete Report");
                            item.Tag = n;
                            item.Click += new EventHandler(OnDeleteNVLClick);
                        }
                    }
                    else if (e.Node.Text == "lists" && n.GetIecs().DataModel.ied.DefineNVL)
                    {
                        item = menu.Items.Add("Add New Name List");
                        item.Tag = n;
                        listsNode = e.Node;
                        item.Click += new EventHandler(OnAddNVLClick);
                    }
                    else if (e.Node.Text == "files")
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
                    if (n is NodeData || n is NodeFC)
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
                }

                if (menu.Items.Count > 0)
                    menu.Show((Control)sender, e.Location);
            }
        }

        void OnDeleteNVLClick(object sender, EventArgs e)
        {
            NodeVL nvl = (NodeVL)(sender as ToolStripItem).Tag;
            Iec61850State iecs = nvl.GetIecs();
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = nvl;
            iecs.Send(ndarr, nvl.CommAddress, ActionRequested.DefineNVL);
        }

        void OnFileListClick(object sender, EventArgs e)
        {
            NodeBase nfi = (NodeBase)(sender as ToolStripItem).Tag;
            Iec61850State iecs = nfi.GetIecs();
            CommAddress ad = new CommAddress();
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = nfi;
            if (!(nfi is NodeFile))
            {
                NodeData nd = new NodeData("x");
                nd.DataType = scsm_MMS_TypeEnum.visible_string;
                nd.DataValue = "/";
                EditValue ev = new EditValue(nd);
                DialogResult r = ev.ShowDialog();
                if (r == DialogResult.OK)
                {
                    ad.Variable = nd.StringValue;
                }
            }

            iecs.Send(ndarr, ad, ActionRequested.GetDirectory);
        }

        void OnFileGetClick(object sender, EventArgs e)
        {
            NodeBase nfi = (NodeBase)(sender as ToolStripItem).Tag;
            Iec61850State iecs = nfi.GetIecs();
            CommAddress ad = new CommAddress();
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = nfi;
            iecs.Send(ndarr, ad, ActionRequested.OpenFile);
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

        void OnDefineNVLClick(object sender, EventArgs e)
        {
            NodeVL nvl = (NodeVL)(sender as ToolStripItem).Tag;
            Iec61850State iecs = nvl.GetIecs();
            List<NodeBase> ndar = new List<NodeBase>();
            foreach (NodeBase n in nvl.GetChildNodes())
            {
                ndar.Add(n);
            }
            iecs.Send(ndar.ToArray(), nvl.CommAddress, ActionRequested.DefineNVL);
        }

        void OnSendCommandClick(object sender, EventArgs e)
        {
            NodeBase data = (NodeBase)(sender as ToolStripItem).Tag;
            Iec61850State iecs = data.GetIecs();
            NodeBase[] ndarr = new NodeBase[1];

            SendCommand(data, iecs, ActionRequested.Write);
        }

        void OnSendCommandAsStructureClick(object sender, EventArgs e)
        {
            NodeBase data = (NodeBase)(sender as ToolStripItem).Tag;
            Iec61850State iecs = data.GetIecs();
            NodeBase[] ndarr = new NodeBase[1];

            SendCommand(data, iecs, ActionRequested.WriteAsStructure);
        }

        private void SendCommand(NodeBase data, Iec61850State iecs, ActionRequested how)
        {
            if (data != null)
            {
                NodeData d = (NodeData)data.Parent;
                if (d != null)
                {
                    NodeBase b, c;
                    CommandParams cPar = new CommandParams();
                    cPar.CommType = CommandType.SingleCommand;
                    if ((b = d.FindChildNode("ctlVal")) != null)
                    {
                        cPar.DataType = ((NodeData)b).DataType;
                        cPar.Address = b.Address;
                        cPar.ctlVal = ((NodeData)b).DataValue;
                    }
                    cPar.T = DateTime.MinValue;
                    cPar.interlockCheck = true;
                    cPar.synchroCheck = true;
                    cPar.orCat = OrCat.STATION_CONTROL;
                    cPar.orIdent = "IEDEXPLORER";
                    //cPar.orIdent = "ET03: 192.168.001.001 R001 K189 Origin:128";
                    cPar.CommandFlowFlag = CommandCtrlModel.Unknown;
                    b = data;
                    List<string> path = new List<string>();
                    do
                    {
                        b = b.Parent;
                        path.Add(b.Name);
                    } while (!(b is NodeFC));
                    path[0] = "ctlModel";
                    path[path.Count - 1] = "CF";
                    b = b.Parent;
                    for (int i = path.Count - 1; i >= 0; i--)
                    {
                        if ((b = b.FindChildNode(path[i])) == null)
                            break;
                    }
                    if (b != null)
                        if (b is NodeData)
                            cPar.CommandFlowFlag = (CommandCtrlModel)((long)((b as NodeData).DataValue));

                    CommandDialog dlg = new CommandDialog(cPar);
                    DialogResult res = dlg.ShowDialog(this);

                    if (res == DialogResult.Cancel)
                        return;

                    List<NodeData> ndar = new List<NodeData>();
                    //char *nameo[] = {"$Oper$ctlVal", "$Oper$origin$orCat", "$Oper$origin$orIdent", "$Oper$ctlNum", "$Oper$T", "$Oper$Test", "$Oper$Check"};
                    if ((b = d.FindChildNode("ctlVal")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = cPar.ctlVal;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("origin")) != null)
                    {
                        if (how == ActionRequested.WriteAsStructure)
                        {
                            NodeData n = new NodeData(b.Name);
                            n.DataType = scsm_MMS_TypeEnum.structure;
                            n.DataValue = 2;
                            ndar.Add(n);
                            if ((c = b.FindChildNode("orCat")) != null)
                            {
                                NodeData n2 = new NodeData(b.Name + "$" + c.Name);
                                n2.DataType = ((NodeData)c).DataType;
                                n2.DataValue = (long)cPar.orCat;
                                n.AddChildNode(n2);
                            }
                            if ((c = b.FindChildNode("orIdent")) != null)
                            {
                                NodeData n2 = new NodeData(b.Name + "$" + c.Name);
                                n2.DataType = ((NodeData)c).DataType;
                                byte[] bytes = new byte[cPar.orIdent.Length];
                                int tmp1, tmp2; bool tmp3;
                                Encoder ascii = (new ASCIIEncoding()).GetEncoder();
                                ascii.Convert(cPar.orIdent.ToCharArray(), 0, cPar.orIdent.Length, bytes, 0, cPar.orIdent.Length, true, out tmp1, out tmp2, out tmp3);
                                n2.DataValue = bytes;
                                n.AddChildNode(n2);
                            }
                        }
                        else
                        {
                            if ((c = b.FindChildNode("orCat")) != null)
                            {
                                NodeData n = new NodeData(b.Name + "$" + c.Name);
                                n.DataType = ((NodeData)c).DataType;
                                n.DataValue = (long)cPar.orCat;
                                ndar.Add(n);
                            }
                            if ((c = b.FindChildNode("orIdent")) != null)
                            {
                                NodeData n = new NodeData(b.Name + "$" + c.Name);
                                n.DataType = ((NodeData)c).DataType;
                                byte[] bytes = new byte[cPar.orIdent.Length];
                                int tmp1, tmp2; bool tmp3;
                                Encoder ascii = (new ASCIIEncoding()).GetEncoder();
                                ascii.Convert(cPar.orIdent.ToCharArray(), 0, cPar.orIdent.Length, bytes, 0, cPar.orIdent.Length, true, out tmp1, out tmp2, out tmp3);
                                n.DataValue = bytes;
                                ndar.Add(n);
                            }
                        }
                    }
                    if ((b = d.FindChildNode("ctlNum")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = m_ctlNum++;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("T")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        byte[] btm = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
                        n.DataValue = btm;

                        if (cPar.T != DateTime.MinValue)
                        {
                            int t = (int)Scsm_MMS.ConvertToUnixTimestamp(cPar.T);
                            byte[] uib = BitConverter.GetBytes(t);
                            btm[0] = uib[3];
                            btm[1] = uib[2];
                            btm[2] = uib[1];
                            btm[3] = uib[0];
                        }
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("Test")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = false;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("Check")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0x40 };
                        n.DataParam = ((NodeData)b).DataParam;
                        ndar.Add(n);
                    }
                    iecs.Send(ndar.ToArray(), d.CommAddress, how);
                }
                else
                    MessageBox.Show("Basic structure not found!");
            }
        }

        void OnReadDataClick(object sender, EventArgs e)
        {
            NodeBase data = (NodeBase)(sender as ToolStripItem).Tag;
            Iec61850State iecs = data.GetIecs();
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = data;
            iecs.Send(ndarr, data.CommAddress, ActionRequested.Read);
        }

        void OnWriteDataClick(object sender, EventArgs e)
        {
            NodeData data = (NodeData)(sender as ToolStripItem).Tag;
            Iec61850State iecs = data.GetIecs();
            NodeData[] ndarr = new NodeData[1];
            ndarr[0] = new NodeData(data.Name);
            ndarr[0].DataType = data.DataType;
            ndarr[0].DataValue = data.DataValue;
            ndarr[0].DataParam = data.DataParam;
            EditValue ev = new EditValue(ndarr[0]);
            DialogResult r = ev.ShowDialog();
            if (r == DialogResult.OK)
            {
                iecs.Send(ndarr, data.Parent.CommAddress, ActionRequested.Write);
                Thread.Sleep(300);
                ndarr = new NodeData[1];
                ndarr[0] = data;
                iecs.Send(ndarr, data.CommAddress, ActionRequested.Read);
            }
        }

        void OnAddNVLClick(object sender, EventArgs e)
        {
            NodeBase lists = (NodeBase)(sender as ToolStripItem).Tag;
            NodeVL newnode = new NodeVL("NewNamedVariableList");
            newnode.Parent = lists;
            AddNVLDialog d = new AddNVLDialog(newnode, lists, listsNode, OnNVListChanged);
            d.Show();
        }

        void OnNVListChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("NVLIST Changed");
            TreeNode n = new TreeNode((sender as AddNVLDialog).List.Name);
            n.Tag = (sender as AddNVLDialog).List;
            ((sender as AddNVLDialog).ListsNode).Nodes.Add(n);
            foreach (NodeBase nb in (sender as AddNVLDialog).List.GetChildNodes())
            {
                TreeNode tn = new TreeNode(nb.CommAddress.Variable);
                tn.Tag = nb;
                n.Nodes.Add(tn);
            }
        }

        void OnActivateNVLClick(object sender, EventArgs e)
        {
            NodeVL vl = (NodeVL)(sender as ToolStripItem).Tag;
            NodeBase ur = null;
            Iec61850State iecs = vl.GetIecs();
            bool retry;
            if (iecs != null)
            {
                do
                {
                    ur = (NodeData)iecs.DataModel.ied.FindNodeByValue(scsm_MMS_TypeEnum.visible_string, vl.Address, ref ur);
                    if (ur == null)
                    {
                        MessageBox.Show("Suitable URCB not found, list cannot be activated!");
                        return;
                    }
                    retry = !ur.Parent.Name.ToLower().Contains("rcb");
                    vl.urcb = (NodeData)ur;
                    NodeData d = (NodeData)vl.urcb.Parent;
                    NodeData b;
                    if ((b = (NodeData)d.FindChildNode("Resv")) != null)
                    {
                        // Resv is always a boolean
                        // If true then the rcb is occupied and we need to find another one
                        if ((bool)b.DataValue) retry = true;
                    }
                } while (retry);

                if (vl.urcb != null)
                {
                    NodeData d = (NodeData)vl.urcb.Parent;
                    List<NodeData> ndar = new List<NodeData>();
                    NodeBase b;
                    if ((b = d.FindChildNode("Resv")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = true;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("DatSet")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = ((NodeData)b).DataValue;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("OptFlds")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0x7c, 0x00 };
                        n.DataParam = 6;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("TrgOps")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0x74 };
                        n.DataParam = 2;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("RptEna")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = true;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("GI")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = true;
                        ndar.Add(n);
                    }
                    iecs.Send(ndar.ToArray(), d.CommAddress, ActionRequested.Write);
                    vl.Activated = true;
                }
            }
            else
                MessageBox.Show("Basic structure not found!");
        }

        void OnDeactivateNVLClick(object sender, EventArgs e)
        {
            NodeVL vl = (NodeVL)(sender as ToolStripItem).Tag;
            Iec61850State iecs = vl.GetIecs();
            if (iecs != null)
            {
                if (vl.urcb != null)
                {
                    NodeData d = (NodeData)vl.urcb.Parent;
                    List<NodeData> ndar = new List<NodeData>();
                    NodeBase b;
                    if ((b = d.FindChildNode("RptEna")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = false;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("GI")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = false;
                        ndar.Add(n);
                    }
                    iecs.Send(ndar.ToArray(), d.CommAddress, ActionRequested.Write);
                    vl.Activated = false;
                    vl.urcb = null;
                }
            }
            else
                MessageBox.Show("Basic structure not found!");
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
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

        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    ((TreeView)sender).DoDragDrop(nodeToDrag.Tag, DragDropEffects.All | DragDropEffects.Link);
                }
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
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
