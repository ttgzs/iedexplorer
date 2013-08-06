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
using MMS_ASN1_Model;
using System.Globalization;
using System.Threading;
using org.mkulu.config;
using IEDExplorer.Resources;

namespace IEDExplorer
{
    public partial class MainWindow : Form
    {
        Scsm_MMS_Worker worker;
        delegate void OnMessageCallback(string message);
        delegate void OnNodeCallback(Iec61850State iecs);
        delegate void OnValueCallback(object sender, EventArgs e);
        delegate void OnDirectoryCallback(object sender, EventArgs e);
        
        Env env = new Env();
        Rectangle dragBoxFromMouseDown;
        TreeNode nodeToDrag;
        TreeNode listsNode;
        long m_ctlNum = 0;
        IniFileManager ini;

        public MainWindow()
        {
            InitializeComponent();
            env.logger = new Logger();
            env.mainWindow = this;
            worker = new Scsm_MMS_Worker(env);
            env.logger.OnLogMessage += new Logger.OnLogMessageDelegate(logger_OnLogMessage);
            env.logger.LogInfo("Starting main program ...");
            ini = new IniFileManager(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\mruip.ini");
            GetMruIp();
            if (toolStripComboBox_Hostname.Items.Count > 0)
                toolStripComboBox_Hostname.SelectedIndex = 0;
        }

        void SaveMruIp()
        {
            int i = 0;
            if (toolStripComboBox_Hostname.Text != "")
            {
                if (toolStripComboBox_Hostname.Items.Contains(toolStripComboBox_Hostname.Text))
                {
                    string s = toolStripComboBox_Hostname.Text;
                    int idx = toolStripComboBox_Hostname.Items.IndexOf(s);
                    toolStripComboBox_Hostname.Items.RemoveAt(idx);
                    toolStripComboBox_Hostname.Items.Insert(0, s);
                    toolStripComboBox_Hostname.SelectedIndex = 0;
                }
                else
                {
                    if (toolStripComboBox_Hostname.Items.Count >= 20)
                    {
                        toolStripComboBox_Hostname.Items.RemoveAt(toolStripComboBox_Hostname.Items.Count-1);
                    }
                    toolStripComboBox_Hostname.Items.Insert(0, toolStripComboBox_Hostname.Text);
                    toolStripComboBox_Hostname.SelectedIndex = 0;
                }
            }
            foreach (string s in toolStripComboBox_Hostname.Items)
            {
                ini.writeString("MruIp", "Ip" + i++, s);
            }
        }

        void GetMruIp()
        {
            string s;
            for (int i = 0; i < 20; i++)
            {
                s = ini.getString("MruIp", "Ip" + i, "");
                if (s != "")
                    toolStripComboBox_Hostname.Items.Add(s);
            }
        }

        void logger_OnLogMessage(string message)
        {
            if (listView_Log.InvokeRequired)
            {
                OnMessageCallback d = new OnMessageCallback(logger_OnLogMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                listView_Log.BeginUpdate();
                ListViewItem item = listView_Log.Items.Add(message);
                item.EnsureVisible();
                listView_Log.EndUpdate();
            }
        }

        internal void makeTree(Iec61850State iecs)
        {
            if (treeView1.InvokeRequired)
            {
                OnNodeCallback d = new OnNodeCallback(makeTree);
                this.Invoke(d, new object[] { iecs });
            }
            else
            {
                treeView1.ImageList = new ImageList();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resource1));
                treeView1.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("computer"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("calculator"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("database"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text_width"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN1"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC1"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO1"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA1"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN2"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC2"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO2"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA2"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN3"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC3"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO3"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA3"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN4"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC4"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO4"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA4"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN5"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC5"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO5"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA5"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN6"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC6"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO6"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA6"))));
                treeView1.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("folder"))));
                treeView1.Nodes.Clear();
                TreeNode n = treeView1.Nodes.Add(iecs.ied.Name + " = " + toolStripComboBox_Hostname.Text +
                                                 ", Vendor = " + (iecs.ied as NodeIed).VendorName +
                                                 ", Model = " + (iecs.ied as NodeIed).ModelName +
                                                 ", Revision = " + (iecs.ied as NodeIed).Revision +
                                                 ", DefineNVL = " + (iecs.ied as NodeIed).DefineNVL
                                                 );
                NodeBase nb = iecs.ied;
                n.Tag = nb;
                n.ImageIndex = 0;
                foreach (NodeBase b in nb.GetChildNodes())
                {
                    TreeNode tn2 = n.Nodes.Add(b.Name);
                    tn2.Tag = b;
                    tn2.ImageIndex = 1;
                    tn2.SelectedImageIndex = 1;
                    TreeNode tn3 = tn2.Nodes.Add("data");
                    tn3.Tag = b;
                    tn3.ImageIndex = 2;
                    tn3.SelectedImageIndex = 2;
                    makeTree_dataNode(b, tn3);
                    NodeBase lb = iecs.lists.FindChildNode(b.Name);
                    if (lb != null)
                    {
                        tn3 = tn2.Nodes.Add("lists");
                        tn3.Tag = lb;
                        tn3.ImageIndex = 3;
                        tn3.SelectedImageIndex = 3;
                        makeTree_listNode(lb, tn3);
                    }
                }
                nb = iecs.files;
                TreeNode tn4 = n.Nodes.Add("files");
                tn4.Tag = iecs.files;
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

        private void toolStripButton_Run_Click (object sender, EventArgs e)
		{
			toolStripButton_Stop.Enabled = true;
			toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.LightYellow;
			if (toolStripComboBox_Hostname.Items.Count == 0) {
				toolStripComboBox_Hostname.Items.Add ("192.168.0.1");
			}
			toolStripButton_Run.Enabled = false;
			SaveMruIp ();
            worker.Start(toolStripComboBox_Hostname.Text, 102); //.SelectedItem.ToString(), 102);
        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            worker.Stop();
			toolStripButton_Stop.Enabled = false;
			toolStripButton_Run.Enabled = true;
        }

        private void toolStripComboBox_Hostname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox_Hostname.SelectedItem != null)
				toolStripComboBox_Hostname.SelectedItem.ToString();
        }

        private void TreeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ListViewItem l in this.listView_data.Items)
            {
                if (l.Tag is TreeNode)
                    if ((l.Tag as TreeNode).Tag is NodeData)
                        ((l.Tag as TreeNode).Tag as NodeData).ValueChanged -= new EventHandler(Node_ValueChanged);
            }
            worker.Stop();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (ListViewItem l in this.listView_data.Items)
            {
                if (l.Tag is TreeNode)
                    if ((l.Tag as TreeNode).Tag is NodeData)
                        ((l.Tag as TreeNode).Tag as NodeData).ValueChanged -= new EventHandler(Node_ValueChanged);
            }
            this.listView_data.Items.Clear();
            ListViewItem li;
            NodeBase n = (NodeBase)e.Node.Tag;

            li = this.listView_data.Items.Add(makeRow(n));
            li.Tag = e.Node;
            if (n.GetChildNodes().Length > 0)
            {
                this.listView_data.Items.Add(new ListViewItem(new string[] { "------------- CHILD NODES -------------", "-------------", "-------------", "-------------" }));
                recursiveAddLine(n, e.Node);
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

        void Node_DirectoryUpdated(object sender, EventArgs e)
        {
            if (listView_data.InvokeRequired)
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

        void Node_StateChanged(object sender, EventArgs e)
        {
            if (treeView1.InvokeRequired)
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
                    else if (b.GetType() == typeof(NodeData))
                    {
                        if (b.GetChildNodes().Length == 0)
                        { // Leaf
                            firsticon = 8;
                        }
                        else
                        {
                            firsticon = 7;
                        }
                    }
                    tn.ImageIndex = firsticon + ((int)b.NodeState) * 4;
                    tn.SelectedImageIndex = firsticon + ((int)b.NodeState) * 4;
                    treeView1.Invalidate(tn.Bounds);
                }
            }
        }

        private void listView_data_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView_data.SelectedItems)
            {
                if (li.Tag != null) (li.Tag as TreeNode).EnsureVisible();
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
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
                    else if (e.Node.Text == "lists" && n.GetIecs().ied.DefineNVL)
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
                        item = menu.Items.Add("Send Command");
                        item.Tag = n;
                        item.Click += new EventHandler(OnSendCommandClick);
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

            if (data != null)
            {
                NodeData d = (NodeData)data.Parent;
                if (d != null)
                {
                    CommandDialog dlg = new CommandDialog();
                    DialogResult res = dlg.ShowDialog(this);
                    bool snd = true;
                    if (res == DialogResult.No)
                        snd = false;
                    if (res == DialogResult.Cancel)
                        return;

                    List<NodeData> ndar = new List<NodeData>();
                    NodeBase b, c;
                    //char *nameo[] = {"$Oper$ctlVal", "$Oper$origin$orCat", "$Oper$origin$orIdent", "$Oper$ctlNum", "$Oper$T", "$Oper$Test", "$Oper$Check"};
                    if ((b = d.FindChildNode("ctlVal")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        switch (n.DataType)
                        {
                            case scsm_MMS_TypeEnum.boolean:
                                n.DataValue = snd;
                                break;
                        }
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("origin")) != null)
                    {
                        if ((c = b.FindChildNode("orCat")) != null)
                        {
                            NodeData n = new NodeData(b.Name + "$" + c.Name);
                            n.DataType = ((NodeData)c).DataType;
                            n.DataValue = 2L;
                            ndar.Add(n);
                        }
                        if ((c = b.FindChildNode("orIdent")) != null)
                        {
                            NodeData n = new NodeData(b.Name + "$" + c.Name);
                            n.DataType = ((NodeData)c).DataType;
                            //string orIdent = "Neco odnekud";
                            string orIdent = "ET03: 143.161.047.115 R001 K189 Origin:128";
                            byte[] bytes = new byte[orIdent.Length];
                            int tmp1, tmp2; bool tmp3;
                            Encoder ascii = (new ASCIIEncoding()).GetEncoder();
                            ascii.Convert(orIdent.ToCharArray(), 0, orIdent.Length, bytes, 0, orIdent.Length, true, out tmp1, out tmp2, out tmp3);
                            n.DataValue = bytes;
                            ndar.Add(n);
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
                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        TimeSpan diff = DateTime.Now - origin;
                        uint ui = (uint)Math.Floor(diff.TotalSeconds);
                        byte[] uib = BitConverter.GetBytes(ui);
                        n.DataValue = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("Test")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = false;
                    }
                    if ((b = d.FindChildNode("Check")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0x40 };
                        n.DataParam = ((NodeData)b).DataParam;
                        ndar.Add(n);
                    }
                    iecs.Send(ndar.ToArray(), d.CommAddress, ActionRequested.Write);
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
            if (iecs != null)
            {
                do
                {
                    ur = (NodeData)iecs.ied.FindNodeByValue(scsm_MMS_TypeEnum.visible_string, vl.Address, ref ur);
                    if (ur == null)
                    {
                        MessageBox.Show("Suitable URCB not found, list cannot be activated!");
                        return;
                    }
                } while (!ur.Parent.Name.Contains("rcb"));
                vl.urcb = (NodeData)ur;

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

    }
}
