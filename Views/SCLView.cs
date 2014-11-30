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

namespace IEDExplorer.Views
{
    public partial class SCLView : DockContent
    {
        string filename;
        Iec61850Model dataModel;
        public SCLView(string fname)
        {
            filename = fname;
            InitializeComponent();
            dataModel = SCLParser.CreateTree(filename);
            makeTree(dataModel);
        }

        internal void makeTree(Iec61850Model dataModel)
        {
                treeViewSCL.ImageList = new ImageList();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resource1));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("computer"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("calculator"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("database"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text_width"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN1"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC1"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO1"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA1"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN2"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC2"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO2"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA2"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN3"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC3"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO3"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA3"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN4"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC4"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO4"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA4"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN5"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC5"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO5"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA5"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("LN6"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("FC6"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO6"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA6"))));
                treeViewSCL.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("folder"))));
                treeViewSCL.Nodes.Clear();
                TreeNode n = treeViewSCL.Nodes.Add(dataModel.ied.Name + " = " + filename +
                                                 ", Vendor = " + (dataModel.ied as NodeIed).VendorName +
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
                    NodeBase rb = dataModel.reports.FindChildNode(b.Name);
                    if (rb != null)
                    {
                        tn3 = tn2.Nodes.Add("Reports");
                        tn3.Tag = rb;
                        tn3.ImageIndex = 3;
                        tn3.SelectedImageIndex = 3;
                        makeTree_reportNode(rb, tn3);
                    }
                }
                nb = dataModel.files;
                TreeNode tn4 = n.Nodes.Add("Files");
                tn4.Tag = dataModel.files;
                tn4.ImageIndex = 3;
                tn4.SelectedImageIndex = 3;
                makeTree_fileNode(nb, tn4);
            
        }

        void makeTree_dataNode(NodeBase nb, TreeNode tn)
        {
            Node_SetIcon(nb);
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
                //Node_StateChanged(b, new EventArgs());
                //b.StateChanged += new EventHandler(Node_StateChanged);

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
                //(b as NodeFile).DirectoryUpdated += Node_DirectoryUpdated;
                makeTree_fileNode(b, tn2);
            }
        }

        void Node_SetIcon(NodeBase b)
        {
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
                    tn.ImageIndex = firsticon + ((int)b.NodeState) * 4;
                    tn.SelectedImageIndex = firsticon + ((int)b.NodeState) * 4;
                    treeViewSCL.Invalidate(tn.Bounds);
                }
        }
    }
}
