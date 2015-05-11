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
        string filename_short;
        Iec61850Model dataModel;
        public string Filename { get { return filename; } }
        private int _currentRow = -1;

        public SCLView(string fname)
        {
            filename = fname;
            InitializeComponent();
            //try
            {
                dataModel = SCLParser.CreateTree(filename);
            }
            //catch (Exception e)
            {
            //    Logger.getLogger().LogError(" Reading SCL: " + e.Message);
            }
            makeTree(dataModel);
            string[] fparts = filename.Split(new char[] { '/', '\\' });
            filename_short  = fparts[fparts.Length - 1];
            this.Text = filename_short;
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
                TreeNode tn5 = n.Nodes.Add("Enums");
                tn5.Tag = dataModel.enums;
                tn5.ImageIndex = 3;
                tn5.SelectedImageIndex = 3;
                makeTree_enumNode(nb, tn5);
            
        }

        void makeTree_dataNode(NodeBase nb, TreeNode tn)
        {
            Node_SetIcon(nb);
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;

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
                makeTree_fileNode(b, tn2);
            }
        }

        private void makeTree_enumNode(NodeBase nb, TreeNode tn)
        {
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
                    treeViewSCL.Invalidate(tn.Bounds);
                }
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
                string type = String.IsNullOrWhiteSpace((n as NodeData).BType)
                    ? (n as NodeData).DataType.ToString()
                    : (n as NodeData).BType;
                var dgvr =
                    new string[]
                    {
                        n.Address, type, val,
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
                        n.Address, n.ToString(), "",
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
                    new string[] { n.Address, n.ToString(), "", n.CommAddress.Domain, n.CommAddress.LogicalNode, n.CommAddress.VariablePath };
            return null;
        }

        private void toolStripButtonCollapseAll_Click(object sender, EventArgs e)
        {
            dataGridView_data.Rows.Clear();
            treeViewSCL.CollapseAll();
            dataGridView_data.Focus();
        }
    }
}
