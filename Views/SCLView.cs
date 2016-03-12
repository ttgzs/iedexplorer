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
        List<Iec61850Model> dataModels;
        public string Filename { get { return filename; } }
        private int _currentRow = -1;

        public SCLView(string fname)
        {
            filename = fname;
            InitializeComponent();
            try
            {
                //dataModels = new SCLParser().CreateTree(filename);
                dataModels = new SCLParserDOM().CreateTree(filename);
            }
            catch (Exception e)
            {
                Logger.getLogger().LogError(" Reading SCL: " + e.Message);
                return;
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
                Node_SetIcon(b);
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
                if (b is NodeData && (b as NodeData).FCDesc != null && (b as NodeData).FCDesc != "") name += " [" + (b as NodeData).FCDesc + "]";
                TreeNode tn2 = tn.Nodes.Add(name);
                tn2.Tag = b;
                b.TagR = tn2;
                //Node_StateChanged(b, new EventArgs());
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

        void Node_SetIcon(NodeBase b)
        {
                if (b.Tag is TreeNode)
                {
                    TreeNode tn = (b.Tag as TreeNode);
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
