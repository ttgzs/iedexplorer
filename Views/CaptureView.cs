using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel.Design;
using Be.Windows.Forms;
using System.Xml;

namespace IEDExplorer.Views
{
    public partial class CaptureView : DockContent
    {
        delegate void AddPacketDelegate(MMSCapture cap);

        WindowManager winMgr;
        public bool CaptureActive;
        public delegate void CaptureActiveChanged(bool captureActive);
        public event CaptureActiveChanged OnCaptureActiveChanged;
        public delegate void ClearCapture();
        public event ClearCapture OnClearCapture;

        public CaptureView(WindowManager wm)
        {
            winMgr = wm;
            InitializeComponent();
        }

        public void AddPacket(MMSCapture cap)
        {
            if (listViewPackets.InvokeRequired)
            {
                AddPacketDelegate apd = new AddPacketDelegate(AddPacket);
                this.Invoke(apd, new object[] { cap });
            }
            else
            {
                ListViewItem lit = new ListViewItem(cap.PacketNr.ToString());
                lit.Tag = cap;
                lit.BackColor = cap.Direction == MMSCapture.CaptureDirection.In ? Color.LightGray : Color.White;
                ListViewItem.ListViewSubItem lsi = new ListViewItem.ListViewSubItem(lit, cap.Time.ToString("d.MM.yyyy hh:mm:ss.fff"));
                lit.SubItems.Add(lsi);
                lsi = new ListViewItem.ListViewSubItem(lit, cap.Direction == MMSCapture.CaptureDirection.In ? ">>" : "<<");
                lit.SubItems.Add(lsi);
                lsi = new ListViewItem.ListViewSubItem(lit, cap.MMSPduType);
                lit.SubItems.Add(lsi);
                lsi = new ListViewItem.ListViewSubItem(lit, cap.MMSPduService);
                lit.SubItems.Add(lsi);
                lsi = new ListViewItem.ListViewSubItem(lit, cap.EncodedPacket.Length.ToString());
                lit.SubItems.Add(lsi);
                listViewPackets.Items.Add(lit);
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            listViewPackets.Items.Clear();
            textBoxXML.Clear();
            treeViewXML.Nodes.Clear();
            if (OnClearCapture != null) OnClearCapture();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPackets.SelectedItems.Count > 0)
            {
                MMSCapture cap = (listViewPackets.SelectedItems[0].Tag as MMSCapture);
                textBoxXML.Text = cap.XMLPdu;
                fillXMLTree(cap.XMLPdu);
                hexBoxHEX.ByteProvider = new DynamicByteProvider(cap.EncodedPacket);
            }
            else
            {
                textBoxXML.Text = "";
                hexBoxHEX.ByteProvider = null;
            }
        }

        private void fillXMLTree(string inXMLString)
        {
            string XMLString = "<?xml version=\"1.0\"?>" + inXMLString;

            try
            {
                // SECTION 1. Create a DOM Document and load the XML data into it.
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(XMLString);

                // SECTION 2. Initialize the TreeView control.
                treeViewXML.Nodes.Clear();
                treeViewXML.Nodes.Add(new TreeNode(dom.DocumentElement.Name));
                TreeNode tNode = new TreeNode();
                tNode = treeViewXML.Nodes[0];

                // SECTION 3. Populate the TreeView with the DOM nodes.
                xMLTreeAddNode(dom.DocumentElement, tNode);
                treeViewXML.ExpandAll();
            }
            catch (XmlException xmlEx)
            {
                Logger.getLogger().LogError("XML Capture view: " + xmlEx.Message);
                treeViewXML.Nodes.Clear();
            }
            catch (Exception ex)
            {
                Logger.getLogger().LogError("XML Capture view: " + ex.Message);
                treeViewXML.Nodes.Clear();
            }
        }

        private void xMLTreeAddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.Nodes[i];
                    xMLTreeAddNode(xNode, tNode);
                }
            }
            else
            {
                // Here you need to pull the data from the XmlNode based on the
                // type of node, whether attribute values are required, and so forth.
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }
            if (inXmlNode.Attributes != null && inXmlNode.Attributes["type"] != null)
            {
                string s = " (" + inXmlNode.Attributes["type"].Value + ")";
                inTreeNode.Text += s;
            }
        }

        private void toolStripButton_StartCapture_Click(object sender, EventArgs e)
        {
            this.CaptureActive = true;
            this.toolStripButtonClear.Enabled = false;
            if (OnCaptureActiveChanged != null) OnCaptureActiveChanged(this.CaptureActive);
            toolStripButton_StartCapture.Enabled = false;
            toolStripButton_StopCapture.Enabled = true;
        }

        private void toolStripButton_StopCapture_Click(object sender, EventArgs e)
        {
            this.CaptureActive = false;
            this.toolStripButtonClear.Enabled = true;
            if (OnCaptureActiveChanged != null) OnCaptureActiveChanged(this.CaptureActive);
            toolStripButton_StartCapture.Enabled = true;
            toolStripButton_StopCapture.Enabled = false;
        }
    }
}
