using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IEDExplorer.Resources;
using GOOSE_ASN1_Model;
using org.bn.types;
using System.Xml;
using System.Xml.Linq;
using System.Threading;

namespace IEDExplorer
{
    public partial class GooseDataEdit : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private Thread _seqThread;
        private bool _run;

        
        private List<Data> _dataList;
        private EventHandler _ValueChanged;        
        private Boolean _DataEditModeOnly = false;
        private string _prevText;
        private List<SeqData> _seqDataList = new List<SeqData>();
        //string _parentName;

        Rectangle dragBoxFromMouseDown;
        TreeNode nodeToDrag;

        public NodeGVL Gvl = new NodeGVL("Data");

        delegate void OnValueCallback(object sender, EventArgs e);

        public void DataEditModeOnly(Boolean value) 
        {
            if (value && !_DataEditModeOnly )
            {
                _prevText = this.Text;
                toolStripButton_Import.Enabled = false;
                toolStripButton_Clear.Enabled = false;
                this.Text += " (Data Edit mode only)";
                _DataEditModeOnly = value;
            }
            else if(!value && _DataEditModeOnly)
            {
                this.Text = _prevText;
                if (this.toolStripButton_StartSeq.Enabled)
                {
                    toolStripButton_Import.Enabled = true;
                    toolStripButton_Clear.Enabled = true;
                }
                _DataEditModeOnly = value;
            }
        }        

        public GooseDataEdit(string name, List<Data> dataList, List<SeqData> seqData, EventHandler ValueChanged)
        {
            InitializeComponent();
            _dataList = dataList;
            _seqDataList = seqData;
            _ValueChanged = ValueChanged;          
            
            listView_Goose.HideComboAfterSelChange = true;
            listView_Goose.HideSelection = true;

            foreach (GOOSE_ASN1_Model.Data dataListCn in _dataList)
                recursiveAddDataChangedHandler(dataListCn, true);

            this.Text = name.Replace(":", " - ") + this.Text;            

            updateTree();
        }

        public GooseDataEdit()
        {
            InitializeComponent();
        }

        public void EnableSeq()
        {
            toolStripButton_StartSeq.Enabled = true;
        }
        
        private void myTreeView_Goose_MouseDown(object sender, MouseEventArgs e)
        {
            nodeToDrag = ((TreeView)sender).GetNodeAt(e.Location);

            if (e.Button == MouseButtons.Right && !_DataEditModeOnly)
            {
                dragBoxFromMouseDown = Rectangle.Empty;

                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripItem item;
                string[] itemList = null;

                TreeNode node = ((TreeView)sender).GetNodeAt(e.Location);
                object tag = (node != null) ? node.Tag : _dataList;

                if (((tag is NodeGData) && ((tag as NodeGData).DataType == scsm_MMS_TypeEnum.structure)))
                    itemList = new string[] { "Add Structure", "Add Boolean", "Add Integer", /*"Add Float",*/ "Add Bitstring", "Delete" }; 
                else if(tag is NodeGData)
                    itemList = new string[] { "Delete" }; 
                else if(tag == _dataList)
                    itemList = new string[] { "Add Structure", "Add Boolean", "Add Integer", /*"Add Float",*/ "Add Bitstring" };

                if (itemList == null)
                    return;

                for (int i = 0; i < itemList.Length; i++)
                {
                    item = menu.Items.Add(itemList[i]);
                    item.Tag = tag;
                    item.Click += new EventHandler(OnAddDataClick);
                }

                if (menu.Items.Count > 0)
                    menu.Show((Control)sender, e.Location);
            }
            else if (e.Button == MouseButtons.Left && nodeToDrag != null && nodeToDrag.Tag is NodeGData)
            {             
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        public int recursiveReadData(NodeBase nd, GOOSE_ASN1_Model.Data t, NodeBase ndcn, int id, DateTime captureTime)
        {
            int _id = id;

            if (t == null)
                return -1;

            if (t.Array != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.array;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Array_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.array;
                    (nb as NodeGData).DataValue = t.Array;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Binarytime != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.array;
                    (nd as NodeGData).DataValue = t.Binarytime;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Binarytime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.binary_time;
                    (nb as NodeGData).DataValue = t.Binarytime;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Bitstring != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.bit_string;
                    (nd as NodeGData).DataParam = t.Bitstring.TrailBitsCnt;
                    (nd as NodeGData).DataValue = t.Bitstring.Value;                    
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Bitstring_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.bit_string;
                    (nb as NodeGData).DataParam = t.Bitstring.TrailBitsCnt;
                    (nb as NodeGData).DataValue = t.Bitstring.Value;                    
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;

            }
            else if (t.isBooleanSelected())
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.boolean;
                    (nd as NodeGData).DataValue = t.Boolean;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Boolean_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.boolean;
                    (nb as NodeGData).DataValue = t.Boolean;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.BooleanArray != null)
            {
                return 0;
            }
            else if (t.Floatingpoint != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.floating_point;
                    (nd as NodeGData).DataValue = t.Floatingpoint;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Floatingpoint_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.floating_point;
                    (nb as NodeGData).DataValue = t.Floatingpoint;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Generalizedtime != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.generalized_time;
                    (nd as NodeGData).DataValue = t.Generalizedtime;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Generalizedtime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.generalized_time;
                    (nb as NodeGData).DataValue = t.Generalizedtime;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.isIntegerSelected())
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.integer;
                    (nd as NodeGData).DataValue = t.Integer;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Integer_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.integer;
                    (nb as NodeGData).DataValue = t.Integer;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.MMSString != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.mMSString;
                    (nd as NodeGData).DataValue = t.MMSString;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("MMSString_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.mMSString;
                    (nb as NodeGData).DataValue = t.MMSString;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Octetstring != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.octet_string;
                    (nd as NodeGData).DataValue = t.Octetstring;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Octetstring_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.octet_string;
                    (nb as NodeGData).DataValue = t.Octetstring;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Structure != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.structure;

                    NodeBase[] nd1 = nd.GetChildNodes();

                    int i = 0;
                    int j = 0;

                    foreach (GOOSE_ASN1_Model.Data data in t.Structure.Value)
                        j = recursiveReadData(nd1[i++], data, null, j, captureTime);

                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Structure_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.structure;
                    (nb as NodeGData).Tag = t;
                    NodeBase nb1 = ndcn.AddChildNode(nb);

                    int i = 0;
                    foreach (GOOSE_ASN1_Model.Data data in t.Structure.Value)
                    {
                        i = recursiveReadData(null, data, nb1, i, captureTime);
                    }

                    return ++_id;
                }
                else
                    return -1;

            }
            else if (t.isUnsignedSelected())
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.unsigned;
                    (nd as NodeGData).DataValue = t.Unsigned;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Unsigned_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.unsigned;
                    (nb as NodeGData).DataValue = t.Unsigned;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Utctime != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.utc_time;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Utctime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.utc_time;
                    (nb as NodeGData).Tag = t;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Visiblestring != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.visible_string;
                    (nd as NodeGData).DataValue = t.Visiblestring;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Utctime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.visible_string;
                    (nb as NodeGData).DataValue = t.Visiblestring;
                    (nb as NodeGData).Tag = t;
                    return ++_id;
                }
                else
                    return -1;
            }
            else
                return -1;
        }

        void makeTree_listNode(NodeBase nb, TreeNode tn, MyTreeView tv)
        {                           
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = (tn != null) ? tn.Nodes.Add(b.Name) : tv.Nodes.Add(b.Name);
                tn2.Tag = b;
                //tn2.Tag = b.Tag;
               // b.Tag = tn2;
                tn2.ImageIndex = 1;
                tn2.SelectedImageIndex = 1;

                NodeBase[] bcn = b.GetChildNodes();

                if (bcn.Length == 0)
                {
                    tn2.ImageIndex = 2;
                    tn2.SelectedImageIndex = 2;
                }

                foreach (NodeBase b2 in bcn)
                {
                    TreeNode tn3 = tn2.Nodes.Add(b2.CommAddress.Variable);
                    tn3.Tag = b2;
                    //tn3.Tag = b2.Tag;
                    tn3.ImageIndex = 1;
                    tn3.SelectedImageIndex = 1;

                    NodeBase[] b2cn = b2.GetChildNodes();

                    if (b2cn.Length > 0)
                        makeTree_listNode(b2, tn3, null);
                    else
                    {
                        tn3.ImageIndex = 2;
                        tn3.SelectedImageIndex = 2;
                    }
                }
            }
        }        

        void OnAddDataClick(object sender, EventArgs e)
        {               
            switch ((sender as ToolStripItem).Text)
            {
                case "Add Structure":
                    Data nd = new Data();                  
                    DataSequence sd = new DataSequence();
                    sd.initValue();
                    nd.selectStructure(sd);
                    if ((sender as ToolStripItem).Tag is List<Data>)
                        ((sender as ToolStripItem).Tag as List<Data>).Add(nd);
                    else if(((sender as ToolStripItem).Tag is NodeGData))
                    {
                        if((((sender as ToolStripItem).Tag as NodeGData).Tag is Data) && ((((sender as ToolStripItem).Tag as NodeGData).Tag as Data).isStructureSelected()))                        
                            (((sender as ToolStripItem).Tag as NodeGData).Tag as Data).Structure.Value.Add(nd);
                        else
                            MessageBox.Show("Error: Invalid parent data type !");                         

                    }
                    else
                        MessageBox.Show("Error: Invalid parent data type !");                                            
                    break;
                case "Add Boolean":
                    nd = new Data();                                                          
                    nd.selectBoolean(false);
                    nd.ValueChanged += new EventHandler(_ValueChanged);
                    if ((sender as ToolStripItem).Tag is List<Data>)
                        ((sender as ToolStripItem).Tag as List<Data>).Add(nd);
                    else if (((sender as ToolStripItem).Tag is NodeGData))
                    {
                        if ((((sender as ToolStripItem).Tag as NodeGData).Tag is Data) && ((((sender as ToolStripItem).Tag as NodeGData).Tag as Data).isStructureSelected()))
                            (((sender as ToolStripItem).Tag as NodeGData).Tag as Data).Structure.Value.Add(nd);
                        else
                            MessageBox.Show("Error: Invalid parent data type !");

                    }
                    else
                        MessageBox.Show("Error: Invalid parent data type !");  
                    break;
                case "Add Integer":
                    nd = new Data();                                                          
                    nd.selectInteger(0);
                    nd.ValueChanged += new EventHandler(_ValueChanged);
                    if ((sender as ToolStripItem).Tag is List<Data>)
                        ((sender as ToolStripItem).Tag as List<Data>).Add(nd);
                    else if (((sender as ToolStripItem).Tag is NodeGData))
                    {
                        if ((((sender as ToolStripItem).Tag as NodeGData).Tag is Data) && ((((sender as ToolStripItem).Tag as NodeGData).Tag as Data).isStructureSelected()))
                            (((sender as ToolStripItem).Tag as NodeGData).Tag as Data).Structure.Value.Add(nd);
                        else
                            MessageBox.Show("Error: Invalid parent data type !");

                    }
                    else
                        MessageBox.Show("Error: Invalid parent data type !");  
                    break;
                case "Add Float":
                    nd = new Data();                                                          
                    nd.selectFloatingpoint(new FloatingPoint(new byte[] { 0 }));
                    nd.ValueChanged += new EventHandler(_ValueChanged);
                    if ((sender as ToolStripItem).Tag is List<Data>)
                        ((sender as ToolStripItem).Tag as List<Data>).Add(nd);
                    else if (((sender as ToolStripItem).Tag is NodeGData))
                    {
                        if ((((sender as ToolStripItem).Tag as NodeGData).Tag is Data) && ((((sender as ToolStripItem).Tag as NodeGData).Tag as Data).isStructureSelected()))
                            (((sender as ToolStripItem).Tag as NodeGData).Tag as Data).Structure.Value.Add(nd);
                        else
                            MessageBox.Show("Error: Invalid parent data type !");

                    }
                    else
                        MessageBox.Show("Error: Invalid parent data type !");    
                    break;                    
                case "Add Bitstring":
                    nd = new Data();                                                          
                    nd.selectBitstring(new BitString());
                    nd.ValueChanged += new EventHandler(_ValueChanged);
                    if ((sender as ToolStripItem).Tag is List<Data>)
                        ((sender as ToolStripItem).Tag as List<Data>).Add(nd);
                    else if (((sender as ToolStripItem).Tag is NodeGData))
                    {
                        if ((((sender as ToolStripItem).Tag as NodeGData).Tag is Data) && ((((sender as ToolStripItem).Tag as NodeGData).Tag as Data).isStructureSelected()))
                            (((sender as ToolStripItem).Tag as NodeGData).Tag as Data).Structure.Value.Add(nd);
                        else
                            MessageBox.Show("Error: Invalid parent data type !");

                    }
                    else
                        MessageBox.Show("Error: Invalid parent data type !");  
                    break;

                case "Delete":                             

                    if (((sender as ToolStripItem).Tag is NodeGData))
                    {
                        listViewGoose_Clear();
                                                                                                                      
                        if ((((sender as ToolStripItem).Tag as NodeGData).Tag is Data))
                        {                                                       
                            (((sender as ToolStripItem).Tag as NodeGData).Tag as Data).ValueChanged -= new EventHandler(_ValueChanged);
                            _dataList.Remove((((sender as ToolStripItem).Tag as NodeGData).Tag as Data));
                        }
                    }
                    break;
                default:
                    break;
            }

            updateTree();
        }


        private void OnDelSeqDataClick(object sender, EventArgs e)
        {
            if ((sender is ToolStripItem) && ((sender as ToolStripItem).Tag is SeqData))
            {
                _seqDataList.Remove(((sender as ToolStripItem).Tag as SeqData));
                myListView1_Refresh();
            }
        }

        private void listViewGoose_Clear()
        {
            listView_Goose.ClearCustomCells();

            foreach (ListViewItem l in this.listView_Goose.Items)
            {
                if (l.Tag is NodeGData)
                    (l.Tag as NodeGData).ValueChanged -= new EventHandler(Node_ValueChanged);                                         
            }

            this.listView_Goose.Items.Clear();
        }

        void Node_ValueChanged(object sender, EventArgs e)
        {
            if (listView_Goose.InvokeRequired)
            {
                OnValueCallback d = new OnValueCallback(Node_ValueChanged);
                if (!this.Disposing)
                    this.Invoke(d, new object[] { sender, e });

            }
            else
            {
                ((sender as NodeGData).ValueTag as ListViewItem).SubItems[2].Text = (sender as NodeGData).StringValue;
                listView_Goose.Invalidate(((sender as NodeGData).ValueTag as ListViewItem).SubItems[2].Bounds);
            }
        }

        ListViewItem makeRow(NodeBase n)
        {
            if (n is NodeGVL)
            {
                ListViewItem lvi = new ListViewItem(new string[] { (n.IecAddress == "") ? n.Name : n.IecAddress, "GODatSet", "", "Matches SCL: " + (n as NodeGVL).MatchesSCL + ", GoID = " + (n as NodeGVL).GoID + " DatSet = " + (n as NodeGVL).DatSet + " Addr = " + (n as NodeGVL).Addr + " APPID = " + (n as NodeGVL).APPID });                
                (n as NodeGVL).ValueTag = lvi;
                return lvi;
            }
            else if (n is NodeGData)
            {
                string val = (n as NodeGData).StringValue;
                string address = n.CommAddress.Domain.ToString() + ((n.CommAddress.Variable.ToString() != "") ? ("/" + n.CommAddress.Variable.ToString().Replace("$", "/")) : "");
                ListViewItem lvi = new ListViewItem(new string[] { address, (n as NodeGData).DataType.ToString(), val, ((n as NodeGData).Tag as Data).Description });
                (n as NodeGData).ValueChanged += new EventHandler(Node_ValueChanged);
                (n as NodeGData).ValueTag = lvi;               
                return lvi;
            }
            else if (n is NodeVL)
            {
                return new ListViewItem(new string[] { n.IecAddress, n.ToString(), "", "Deletable = " + (n as NodeVL).Deletable.ToString() + ", " + "Defined = " + (n as NodeVL).Defined.ToString() });
            }
            else if (n != null)
                return new ListViewItem(new string[] { n.IecAddress, n.ToString(), "", "Dom = " + n.CommAddress.Domain + " Var = " + n.CommAddress.Variable });
            return null;
        }

        void recursiveAddLine(NodeBase n, TreeNode tn)
        {
            if ((n.GetChildNodes().Length > 0))
            {
                if (n is NodeVL)
                {
                    foreach (NodeBase nb in n.GetChildNodes())
                    {
                        ListViewItem li = this.listView_Goose.Items.Add(makeRow(nb));                        
                        li.Tag = nb.Tag;                        
                    }
                }
                else
                {
                    foreach (NodeBase nb in n.GetChildNodes())                        
                        recursiveAddLine(nb, tn);                                                     
                }
            }
            else
            {
                ListViewItem li = this.listView_Goose.Items.Add(makeRow(n));
                li.Tag = n;

                if (n.Tag is Data)
                    addCustomCell(n.Tag as Data);                                                                    
            }
        }


        private void addCustomCell(Data d)
        {
            bool addDescEditableCell = false;

            if (d.isBooleanSelected())
            {
                listView_Goose.AddComboBoxCell(this.listView_Goose.Items.Count - 1, 2, new string[] { "True", "False" });
                addDescEditableCell = true;
            }
            else if (d.isIntegerSelected())
            {
                listView_Goose.AddNumericUpDownCell(this.listView_Goose.Items.Count - 1, 2, int.MinValue, int.MaxValue);
                addDescEditableCell = true;
            }
            else if (d.isBitstringSelected())
            {
                listView_Goose.AddEditableCell(this.listView_Goose.Items.Count - 1, 2, 32, new char[] { '0', '1' });
                addDescEditableCell = true;
            }
                    
            if(addDescEditableCell)
                listView_Goose.AddEditableCell(this.listView_Goose.Items.Count - 1, 3, 100, null);     
        }

        private void myTreeView_Goose_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ListViewItem li;

            listViewGoose_Clear();            
       
            NodeBase n = (NodeBase)e.Node.Tag;           
            if (n != null)
            {                
                li = this.listView_Goose.Items.Add(makeRow(n));
                li.Tag = n;

                if (n.Tag is Data)
                    addCustomCell(n.Tag as Data);  

                if (n.GetChildNodes().Length > 0)
                {
                    this.listView_Goose.Items.Add(new ListViewItem(new string[] { "------------- CHILD NODES -------------", "-------------", "-------------", "-------------" }));                    
                    recursiveAddLine(n, e.Node);                                                    
                }
            }
        }

        private void toolStripButton_Import_Click(object sender, EventArgs e)
        {
            ImportDataListFromXml DataListFromXml = new ImportDataListFromXml();

            dataListClear(_dataList);

            DataListFromXml.Import(_dataList);
            
            foreach (GOOSE_ASN1_Model.Data dataListCn in _dataList)            
                recursiveAddDataChangedHandler(dataListCn, true);            
            
            updateTree();            
        }

        private void dataListClear(List<Data> dl)
        {
            listViewGoose_Clear();            

            foreach (GOOSE_ASN1_Model.Data dataListCn in dl)
                recursiveAddDataChangedHandler(dataListCn, false);

            dl.Clear();
        }

        private void recursiveAddDataChangedHandler(GOOSE_ASN1_Model.Data d, Boolean add)
        {
            if (d != null)
            {
                if (d.isStructureSelected())
                {
                    foreach (GOOSE_ASN1_Model.Data data in d.Structure.Value)
                        recursiveAddDataChangedHandler(data, add);
                }
                else
                    if (add)
                        d.ValueChanged += new EventHandler(_ValueChanged);
                    else
                        d.ValueChanged -= new EventHandler(_ValueChanged);
            }
        }

        void updateTree()
        {
            Gvl = new NodeGVL("Data");

            int i = 0;

            foreach (GOOSE_ASN1_Model.Data dataListCn in _dataList)
            {
                i = recursiveReadData(null, dataListCn, Gvl, i, DateTime.Now);
            }

            myTreeView_Goose.ImageList = new ImageList();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resource1));

            myTreeView_Goose.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text_width"))));
            myTreeView_Goose.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO1"))));
            myTreeView_Goose.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA1"))));

            myTreeView_Goose.Nodes.Clear();
            makeTree_listNode(Gvl, null, myTreeView_Goose);

            myListView1_CheckDataIntegrity();
        }

        private void myListView1_CheckDataIntegrity()
        {
            if(_seqDataList.Count > 0)
            {
                List<SeqData> itemsToDel = new List<SeqData>();

                foreach(SeqData sqd in _seqDataList)
                {
                    if(Gvl != null && Gvl.GetChildNodes().Length > 0)
                    {
                        NodeGData ret = null;

                        foreach(NodeGData ngdcn in Gvl.GetChildNodes())
                        {
                            ret = recursiveCheckIfDataPresent(ngdcn, sqd.refdata);
                            if (ret != null)
                                break;       
                        }

                        if (ret == null)
                            itemsToDel.Add(sqd);
                        else
                        {
                            if (sqd.refdata != ret)
                                sqd.refdata = ret;
                        }
                    }
                    else
                        _seqDataList.Clear();                    
                }

                if (itemsToDel.Count > 0)
                {
                    foreach (SeqData sqd in itemsToDel)
                        _seqDataList.Remove(sqd);                    
                }
            }
            myListView1_Refresh();       
        }


        private NodeGData recursiveCheckIfDataPresent(NodeGData ngdcn, NodeGData ngd)
        {
            if(ngdcn.DataType == scsm_MMS_TypeEnum.structure)
            {
                if(ngdcn.GetChildNodes().Length > 0)
                {
                    foreach(NodeGData ngdcncn in ngdcn.GetChildNodes())
                    {
                       NodeGData ret = recursiveCheckIfDataPresent(ngdcncn, ngd);                    
                       if(ret != null)
                           return ret;
                    }
                    return null;
                }
                else
                    return null;
            }
            else
            {
                if(ngd.Tag == ngdcn.Tag)
                    return ngdcn;
                else
                    return null;
            }
        }

        private void listViewGooseLabelEdit(ListView lv, LabelEditEventArgs e)
        {
            switch ((lv.Items[e.Item].Tag as NodeGData).DataType)
            {
                case scsm_MMS_TypeEnum.boolean:
                    (lv.Items[e.Item].Tag as NodeGData).DataValue = (lv.Items[e.Item].SubItems[2].Text == "True") ? true : false;

                    if (((lv.Items[e.Item].Tag as NodeGData).Tag as Data).isBooleanSelected())
                        ((lv.Items[e.Item].Tag as NodeGData).Tag as Data).selectBoolean((lv.Items[e.Item].SubItems[2].Text == "True") ? true : false);
                    break;

                case scsm_MMS_TypeEnum.integer:
                    (lv.Items[e.Item].Tag as NodeGData).DataValue = Convert.ToInt32(lv.Items[e.Item].SubItems[2].Text);

                    if (((lv.Items[e.Item].Tag as NodeGData).Tag as Data).isIntegerSelected())
                        ((lv.Items[e.Item].Tag as NodeGData).Tag as Data).selectInteger(Convert.ToInt32(lv.Items[e.Item].SubItems[2].Text));
                    break;

                case scsm_MMS_TypeEnum.bit_string:                    
                    StringToDataConverter Converter = new StringToDataConverter();
                    BitString bs = Converter.ConvertToBitstring(lv.Items[e.Item].SubItems[2].Text);

                    (lv.Items[e.Item].Tag as NodeGData).DataParam = bs.TrailBitsCnt;
                    (lv.Items[e.Item].Tag as NodeGData).DataValue = bs.Value;
                    

                    if (((lv.Items[e.Item].Tag as NodeGData).Tag as Data).isBitstringSelected())
                        ((lv.Items[e.Item].Tag as NodeGData).Tag as Data).selectBitstring(bs);
                    break;

                default:
                    break;
            }

            if (((lv.Items[e.Item].Tag as NodeGData).Tag as Data).Description != lv.Items[e.Item].SubItems[3].Text)
                ((lv.Items[e.Item].Tag as NodeGData).Tag as Data).Description = lv.Items[e.Item].SubItems[3].Text;           
        }

        private void listViewSeqLabelEdit(ListView lv, LabelEditEventArgs e)
        {
            switch ((lv.Items[e.Item].Tag as SeqData).refdata.DataType)
            {
                case scsm_MMS_TypeEnum.boolean:
                    (lv.Items[e.Item].Tag as NodeGData).DataValue = (lv.Items[e.Item].SubItems[2].Text == "True") ? true : false;

                    if (((lv.Items[e.Item].Tag as NodeGData).Tag as Data).isBooleanSelected())
                        ((lv.Items[e.Item].Tag as NodeGData).Tag as Data).selectBoolean((lv.Items[e.Item].SubItems[2].Text == "True") ? true : false);
                    break;

                case scsm_MMS_TypeEnum.integer:
                    (lv.Items[e.Item].Tag as NodeGData).DataValue = Convert.ToInt32(lv.Items[e.Item].SubItems[2].Text);

                    if (((lv.Items[e.Item].Tag as NodeGData).Tag as Data).isIntegerSelected())
                        ((lv.Items[e.Item].Tag as NodeGData).Tag as Data).selectInteger(Convert.ToInt32(lv.Items[e.Item].SubItems[2].Text));
                    break;

                case scsm_MMS_TypeEnum.bit_string:
                    StringToDataConverter Converter = new StringToDataConverter();
                    BitString bs = Converter.ConvertToBitstring(lv.Items[e.Item].SubItems[2].Text);

                    (lv.Items[e.Item].Tag as NodeGData).DataParam = bs.TrailBitsCnt;
                    (lv.Items[e.Item].Tag as NodeGData).DataValue = bs.Value;                    

                    if (((lv.Items[e.Item].Tag as NodeGData).Tag as Data).isBitstringSelected())
                        ((lv.Items[e.Item].Tag as NodeGData).Tag as Data).selectBitstring(bs);
                    break;
                
                default:
                    break;
            }
        }

        private void listView_Goose_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            listViewGooseLabelEdit(listView_Goose, e);           
        }

        private void GooseDataeEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void toolStripButton_Clear_Click(object sender, EventArgs e)
        {
            dataListClear(_dataList);
            updateTree();
        }

        private void toolStripButton_Export_Click(object sender, EventArgs e)
        {
            ExportGvlToXml GvlToXml = new ExportGvlToXml();
            GvlToXml.Export(Gvl);
        }

        private void myListView1_DragDrop(object sender, DragEventArgs e)
        {
            this.toolStripButton_StartSeq.Enabled = true;
            this.toolStripButton_ClearSeq.Enabled = true;
            this.toolStripButton_StopSeq.Enabled = false;

            SeqData sd = new SeqData((e.Data.GetData(typeof(NodeGData)) as NodeGData), 500);
            _seqDataList.Add(sd);

            myListView1_Refresh();           
        }

        private void myListView1_Refresh()
        {
            myListView1.Items.Clear();
            myListView1.ClearCustomCells();

            string address = null;

            foreach (SeqData seqd in _seqDataList)
            {
                address = seqd.refdata.CommAddress.Domain.ToString() + ((seqd.refdata.CommAddress.Variable.ToString() != "") ? ("/" + seqd.refdata.CommAddress.Variable.ToString().Replace("$", "/")) : "");
                ListViewItem lvi = new ListViewItem(new string[] { address, (seqd.refdata as NodeGData).DataType.ToString(), seqd.data, seqd.duration.ToString() });

                lvi.Tag = seqd;

                myListView1.Items.Add(lvi);

                if (seqd.refdata.Tag is Data)
                {
                    if ((seqd.refdata.Tag as Data).isBooleanSelected())
                        myListView1.AddComboBoxCell(this.myListView1.Items.Count - 1, 2, new string[] { "True", "False" });
                    else if ((seqd.refdata.Tag as Data).isIntegerSelected())
                        myListView1.AddNumericUpDownCell(this.myListView1.Items.Count - 1, 2, int.MinValue, int.MaxValue);
                    else if ((seqd.refdata.Tag as Data).isBitstringSelected())
                        myListView1.AddEditableCell(this.myListView1.Items.Count - 1, 2, 32, new char[] { '0', '1' });
                    else
                        myListView1.AddEditableCell(this.myListView1.Items.Count - 1, 2, 20, null);
                }

                myListView1.AddNumericUpDownCell(this.myListView1.Items.Count - 1, 3, 5, int.MaxValue);
            }
        }

        private void myTreeView_Goose_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    ((TreeView)sender).DoDragDrop(nodeToDrag.Tag, DragDropEffects.All | DragDropEffects.Link);
                }
        }

        private void myTreeView_Goose_MouseUp(object sender, MouseEventArgs e)
        {
            dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void myListView1_DragEnter(object sender, DragEventArgs e)
        {                    
            if ((e.Data.GetDataPresent(typeof(NodeGData))) && ((e.Data.GetData(typeof(NodeGData)) as NodeGData).DataType != scsm_MMS_TypeEnum.structure) && !_run)           
                e.Effect = DragDropEffects.Link;                                
            else           
                e.Effect = DragDropEffects.None;     
        }

        private void myListView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            (myListView1.Items[e.Item].Tag as SeqData).data = myListView1.Items[e.Item].SubItems[2].Text;
            (myListView1.Items[e.Item].Tag as SeqData).duration = Convert.ToInt32(myListView1.Items[e.Item].SubItems[3].Text);
        }

        private void WorkerThreadProc(object obj)
        {
            while (true)
            {
                foreach (SeqData sd in _seqDataList)
                {
                    switch (sd.refdata.DataType)
                    {
                        case scsm_MMS_TypeEnum.boolean:
                            sd.refdata.DataValue = (sd.data == "True") ? true : false;

                            if ((sd.refdata.Tag as Data).isBooleanSelected())
                                (sd.refdata.Tag as Data).selectBoolean((sd.data == "True") ? true : false);
                            break;

                        case scsm_MMS_TypeEnum.integer:
                            sd.refdata.DataValue = Convert.ToInt32(sd.data);

                            if ((sd.refdata.Tag as Data).isIntegerSelected())
                                (sd.refdata.Tag as Data).selectInteger(Convert.ToInt32(sd.data));
                            break;

                        case scsm_MMS_TypeEnum.bit_string:                            
                            StringToDataConverter Converter = new StringToDataConverter();
                            BitString bs = Converter.ConvertToBitstring(sd.data);

                            sd.refdata.DataParam = bs.TrailBitsCnt;
                            sd.refdata.DataValue = bs.Value;
                                                        
                            if ((sd.refdata.Tag as Data).isBitstringSelected())
                                (sd.refdata.Tag as Data).selectBitstring(bs);
                            break;
                        default:
                            break;
                    }

                    Thread.Sleep(sd.duration);
                }
            }
        }

        private void myListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete && !_run)
            {
                ListView.SelectedIndexCollection indexes = myListView1.SelectedIndices;
                if (!(indexes.Count > 0))
                    return;

                foreach (int index in indexes)                
                    _seqDataList.RemoveAt(index);
               
                myListView1_Refresh();
            }
        }

        private void toolStripButton_StartSeq_Click(object sender, EventArgs e)
        {
            if (!_run && _seqThread == null)
            {
                if (_seqDataList.Count > 0)
                {
                    _run = true;
                    _seqThread = new Thread(new ParameterizedThreadStart(WorkerThreadProc));
                    _seqThread.Start(this);
                    this.toolStripButton_StopSeq.Enabled = true;
                    this.toolStripButton_StartSeq.Enabled = false;
                    this.toolStripButton_ClearSeq.Enabled = false;

                    this.toolStripButton_Import.Enabled = false;
                    this.toolStripButton_Clear.Enabled = false;                    
                }
                else
                    MessageBox.Show("Sequence List is empty !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton_StopSeq_Click(object sender, EventArgs e)
        {
            if (_run && _seqThread != null)
            {
                _run = false;
                _seqThread.Abort();
                _seqThread = null;
                this.toolStripButton_StartSeq.Enabled = true;
                this.toolStripButton_StopSeq.Enabled = false;
                this.toolStripButton_ClearSeq.Enabled = true;

                if (!_DataEditModeOnly)
                {
                    this.toolStripButton_Import.Enabled = true;
                    this.toolStripButton_Clear.Enabled = true;
                }
            }
        }

        private void toolStripButton_ClearSeq_Click(object sender, EventArgs e)
        {
            if (!_run)
            {
                _seqDataList.Clear();
                myListView1_Refresh();
                this.toolStripButton_StartSeq.Enabled = false;
            }
        }

        private void myListView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (((ListView)sender).Items.Count > 0) && !_run )
            {
                dragBoxFromMouseDown = Rectangle.Empty;

                ContextMenuStrip menu = new ContextMenuStrip();         
               
                ListViewItem lvItem = ((ListView)sender).GetItemAt(e.Location.X, e.Location.Y);
                
                if (lvItem != null && (lvItem.Tag is SeqData))
                {
                    ToolStripItem item = menu.Items.Add("Delete");

                    item.Tag = lvItem.Tag;
                    item.Click += new EventHandler(OnDelSeqDataClick);
                }

                if (menu.Items.Count > 0)
                    menu.Show((Control)sender, e.Location);                
            }
        }

        private void GooseDataeEdit_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
