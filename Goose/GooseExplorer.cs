using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using IEDExplorer.Resources;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using System.Text.RegularExpressions;
using System.Threading;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections;
using org.bn;
using GOOSE_ASN1_Model;

using System.Windows.Forms.DataVisualization.Charting;

namespace IEDExplorer
{
    public partial class GooseExplorer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        IList<LivePacketDevice> _netDevices;
        PacketCommunicator _communicator;

        private Iec61850State _iecf = null;             
        int _GoosesRcvd = 0;
        private Thread _workerThread;
        private bool _run;                
        private int LastSelectedIndex = 0xFF;
        private Logger _logger;
        delegate void OnValueCallback(object sender, EventArgs e);
        delegate void OnAddDataToGraph(object sender, EventArgs e);

        //private Thread addDataRunner;
        private Random rand = new Random();
        public delegate void AddDataDelegate();
        public AddDataDelegate addDataDel;
        public int dataToAdd;
        double offs;                

        public GooseExplorer(Iec61850State iecf, Logger logger)
        {
            InitializeComponent();
            this._iecf = iecf;
            addPlotDataThreadInit();

            _logger = logger;
            _netDevices = LivePacketDevice.AllLocalMachine;

            toolStripButton_Start.Enabled = true;
            toolStripButton_Stop.Enabled = false;
            toolStripComboBox_Ieds.Enabled = false;

            if (_netDevices.Count != 0)
            {
                List<string> netDevNames = new List<string>();
                string description;

                for (int i = 0; i < _netDevices.Count; i++)
                {
                    LivePacketDevice device = _netDevices[i];
                    if (device.Description != null)
                    {
                        description = Regex.Match(device.Description.Replace("(Microsoft's Packet Scheduler)", ""), @"'(.+?)'").Groups[1].Value;
                        description = i.ToString("00") + " : " + description.Trim();                        
                    }
                    else
                        description = i.ToString("00");

                    toolStripComboBox_NedDevices.Items.Add(description);                        
                    netDevNames.Add(device.Name);
                }

                toolStripComboBox_NedDevices.SelectedIndex = 0;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton_Start.Enabled = false;
            toolStripButton_Stop.Enabled = true;

            toolStripComboBox_NedDevices.Enabled = false;

            if (!_run && _workerThread == null)
            {
                _run = true;
                _workerThread = new Thread(new ParameterizedThreadStart(WorkerThreadProc));                
                _workerThread.Start(this);
            }
            else
                MessageBox.Show("Cannot start, communication already running!", "Info");            
        }

        /*
        private void PacketHandler(Packet packet)
        {                               
        }
        */

        private void WorkerThreadProc(object obj)
        {            
            int selectedDev = 0;
            this.Invoke(new MethodInvoker(delegate() { selectedDev = toolStripComboBox_NedDevices.SelectedIndex; }));

            PacketDevice selectedDevice = _netDevices[selectedDev];

            Packet packet;
            IpV4Datagram ip;

            IDecoder decoder = CoderFactory.getInstance().newDecoder("BER");
            
            // Open the device
            using (_communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))                                 
            {
                _communicator.SetFilter("multicast");

                do
                {
                    PacketCommunicatorReceiveResult result = _communicator.ReceivePacket(out packet);

                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:
                            // Timeout elapsed
                            continue;
                        case PacketCommunicatorReceiveResult.Ok:
                            // We've got multicast packet
                            ip = packet.Ethernet.IpV4;

                            if (((int)packet.Ethernet.VLanTaggedFrame.EtherType == 0x88b8) || ((int)packet.Ethernet.EtherType == 0x88b8))
                            {
                                GooseData _RawGoose = new GooseData();

                                _RawGoose.packet = packet;

                                if (_RawGoose.IsGooseType)
                                {
                                    try
                                    {
                                        GOOSE Goose = decoder.decode<GOOSE>(_RawGoose.Ms);
                                        ProcessReceivedGoose(_RawGoose, Goose);
                                    }
                                    catch (Exception err)
                                    {
                                        _logger.LogError("GOOSE.ReceiveData: Malformed GOOSE Packet received!!!: " + err.Message);
                                        //MessageBox.Show("GOOSE.ReceiveData: Malformed GOOSE Packet received!!!: " + err.Message);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                            break;
                        default:
                            throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                    }                       
                } while (true);
            }         
        }

        public delegate void ProcessGooseDelegate(GooseData RawGoose, GOOSE DecodedGoose);

        private void ProcessReceivedGoose(GooseData RawGoose, GOOSE DecodedGoose)
        {
            if (InvokeRequired)
                Invoke(new ProcessGooseDelegate(this.ProcessGoose), new object[] { RawGoose, DecodedGoose });
            else
                ProcessGoose(RawGoose, DecodedGoose);            
        }


        DateTime decodeEntryTime(byte[] entryTime)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);            
            int s, ms;

            if (entryTime != null && entryTime.Length == 8)
            {
                s = (entryTime[0] << 24) + (entryTime[1] << 16) + (entryTime[2] << 8) + (entryTime[3]);

                ms = 0;

                for (int i = 0; i < 24; i++)
                {
                    if (((entryTime[(i / 8) + 4] << (i % 8)) & 0x80) > 0)                    
                        ms += 1000000 / (1 << (i + 1));                    
                }

                ms /= 1000;

                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);                                
                DateTime dt = origin.AddSeconds(s);               
                dt = dt.AddMilliseconds(ms);

                return dt.ToLocalTime();
            }

            return time;
        }

        private void ProcessGoose(GooseData RawGoose, GOOSE DecodedGoose)
        {            
            _GoosesRcvd++;

            //DateTime gtm = ;

            /*
            if (DecodedGoose.GoosePdu.DatSet == "2")
            {
                DateTime timeStamp = RawGoose.packet.Timestamp;

                                      foreach (GOOSE_ASN1_Model.Data AllDataCn in DecodedGoose.GoosePdu.AllData)
                                    {
                                            
                                          dataToAdd = (AllDataCn.Boolean.ToString() == "True") ? 10:0;
                                  
                                      }

                                     
                foreach (Series ptSeries in LiveViewChart.Series)
                {
                    AddNewPoint(timeStamp, ptSeries);
                }
                                      
            }
            */
            toolStripLabel_GoosesRcvdCnt.Text = _GoosesRcvd.ToString();
            addIedItem(RawGoose, DecodedGoose);            
        }

        NodeBase[] isGooseInSCD(GooseData RawGoose, GOOSE DecodedGoose)
        {
            bool foundInIcd = true;
            NodeBase GONb = null;
            NodeBase VLNb = null;
            NodeGVL GVl = new NodeGVL(DecodedGoose.GoosePdu.GocbRef);
           
            Hashtable gooseParameters = new Hashtable();            

            gooseParameters.Add("numericUpDown_AppID", RawGoose.AppId.ToString());
            gooseParameters.Add("numericUpDown_TTL", DecodedGoose.GoosePdu.TimeAllowedtoLive.ToString());
            gooseParameters.Add("textBox_GoID", DecodedGoose.GoosePdu.GoID.ToString());
            gooseParameters.Add("textBox_DatSet", DecodedGoose.GoosePdu.DatSet.ToString());
            gooseParameters.Add("textBox_GoCBRef", DecodedGoose.GoosePdu.GocbRef.ToString());
            gooseParameters.Add("textBox_Time", "2014-05-30 15:01:22.76088");
            gooseParameters.Add("numericUpDown_StNum", DecodedGoose.GoosePdu.StNum.ToString());
            gooseParameters.Add("numericUpDown_SqNum", DecodedGoose.GoosePdu.SqNum.ToString());
            gooseParameters.Add("numericUpDown_CfgRev", DecodedGoose.GoosePdu.ConfRev.ToString());
            gooseParameters.Add("comboBox_Test", (DecodedGoose.GoosePdu.Simulation == true) ? "1" : "0");
            gooseParameters.Add("comboBox_NdsCom", (DecodedGoose.GoosePdu.NdsCom == true) ? "1" : "0");
            gooseParameters.Add("maskedTextBox_srcMac", RawGoose.SrcMac.ToString());
            gooseParameters.Add("maskedTextBox_dstMac", RawGoose.DstMac.ToString());                          
            gooseParameters.Add("checkBox_VlanTagEn", "False");
            gooseParameters.Add("numericUpDown_VlanPrio", "4");
            gooseParameters.Add("comboBox_VlanCFI", "0");
            gooseParameters.Add("numericUpDown_VlanVID", "0");

            GVl.Tag = gooseParameters;
            
            NodeBase[] ret = new NodeBase[2];
            ret[0] = null;
            ret[1] = null;

            GVl.TreeCreated = false;
            GVl.AddedToTreeView = false;
            GVl.GoID = DecodedGoose.GoosePdu.GoID.ToString();
            GVl.DatSet = DecodedGoose.GoosePdu.DatSet.ToString();
            GVl.Addr = RawGoose.DstMac.ToString();
            GVl.APPID = RawGoose.AppId.ToString("X");
            GVl.isGooseInSCLret = 0;

            if (_iecf != null)
            {
                GONb = _iecf.DataModel.ied.FindNodeByAddress(DecodedGoose.GoosePdu.GocbRef);

                if (GONb != null)
                {
                    NodeBase[] GOCn = GONb.GetChildNodes();

                    if (GOCn != null)
                    {
                        foreach (NodeBase DA in GOCn)
                        {
                            switch (DA.Name)
                            {
                                case "GoID":
                                    if ((DA as NodeData).DataValue.ToString() != DecodedGoose.GoosePdu.GoID)
                                    {
                                        foundInIcd = false;
                                        GVl.isGooseInSCLret = -2;
                                    }
                                    break;
                                case "DatSet":
                                    if ((DA as NodeData).DataValue.ToString() != DecodedGoose.GoosePdu.DatSet)
                                    {
                                        foundInIcd = false;
                                        GVl.isGooseInSCLret = -3;
                                    }
                                    break;
                                case "DstAddress":
                                    NodeBase[] DACn = DA.GetChildNodes();
                                    if (DACn != null)
                                    {
                                        foreach (NodeBase DACnCn in DACn)
                                        {
                                            switch (DACnCn.Name)
                                            {
                                                case "Addr":
                                                    if ((DACnCn as NodeData).DataValue.ToString() != RawGoose.DstMac.ToString().Replace(":", "-"))
                                                    {
                                                        foundInIcd = false;
                                                        GVl.isGooseInSCLret = -4;
                                                    }
                                                    break;
                                                case "APPID":
                                                    string apajdi = RawGoose.AppId.ToString("X");
                                                    if ((DACnCn as NodeData).DataValue.ToString() != RawGoose.AppId.ToString("X"))
                                                    {
                                                        foundInIcd = false;
                                                        GVl.isGooseInSCLret = -5;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        GVl.isGooseInSCLret = -1;
                        foundInIcd = false;
                    }
                }
                else
                {
                    GVl.isGooseInSCLret = -1;
                    foundInIcd = false;
                }
            }
            else
            {
                GVl.isGooseInSCLret = -1;
                foundInIcd = false;
            }

            if (foundInIcd)
            {
                // Find dataset coresponding to founded GCB
                NodeBase VLCn = null;

                VLNb = _iecf.DataModel.lists.FindChildNode(DecodedGoose.GoosePdu.DatSet.Substring(0, DecodedGoose.GoosePdu.DatSet.ToString().IndexOf("/")));

                if (VLNb != null)
                    VLCn = VLNb.FindChildNode(DecodedGoose.GoosePdu.DatSet.Substring(DecodedGoose.GoosePdu.DatSet.ToString().IndexOf("/") + 1, DecodedGoose.GoosePdu.DatSet.ToString().Length - DecodedGoose.GoosePdu.DatSet.ToString().IndexOf("/") - 1));
                else
                    ret[0] = null;

                if (VLCn != null)
                    ret[0] = VLCn;
                else
                    ret[0] = null;
                
                ret[1] = GVl;

                return ret;
            }
            else
            {
                ret[0] = null;
                ret[1] = GVl;
                return ret;
            }
        }

        private void addIedItem(GooseData RawGoose, GOOSE DecodedGoose)
        {                                
            bool addIedItem = true;
            bool addIedGooseItem = true;
            int iedItemIndex = 0;

            NodeBase[] ret = isGooseInSCD(RawGoose, DecodedGoose);
            NodeBase DataSet = ret[0];
           
            int i, j = 0;

            for(i = 0; i < toolStripComboBox_Ieds.Items.Count ; i++)
            {   
                if((toolStripComboBox_Ieds.Items[i] as ComboboxItem).RawGoose.Count > 0)
                {
                    for(j = 0; j < (toolStripComboBox_Ieds.Items[i] as ComboboxItem).RawGoose.Count; j++)
                    {
                        if ((toolStripComboBox_Ieds.Items[i] as ComboboxItem).RawGoose[j].SrcMac == RawGoose.SrcMac)
                        {
                            addIedItem = false;
                            iedItemIndex = i;
                        }

                       if ((toolStripComboBox_Ieds.Items[i] as ComboboxItem).DecodedGoose[j].GoosePdu.DatSet.ToString() == DecodedGoose.GoosePdu.DatSet.ToString())
                       {
                           addIedGooseItem = false;
                           (toolStripComboBox_Ieds.Items[i] as ComboboxItem).DecodedGoose[j] = DecodedGoose;
                           (toolStripComboBox_Ieds.Items[i] as ComboboxItem).RawGoose[j] = RawGoose;

                           if ((toolStripComboBox_Ieds.Items[i] as ComboboxItem).GVl[j].TreeCreated == true)
                           {
                               if (toolStripComboBox_Ieds.SelectedIndex == i)
                               {
                                    int k = 0;
  
                                    foreach (GOOSE_ASN1_Model.Data AllDataCn in DecodedGoose.GoosePdu.AllData)
                                    {                                        
                                        NodeBase[] NdCn = (toolStripComboBox_Ieds.Items[i] as ComboboxItem).GVl[j].GetChildNodes();
                                        if (DecodedGoose.GoosePdu.AllData.Count == NdCn.Length)
                                        {
                                            if (recursiveReadData(NdCn[k++], AllDataCn, null, 0, RawGoose.packet.Timestamp) < 0)
                                            {
                                                Logger.getLogger().LogError("Error while updating data in DataSet: " + DecodedGoose.GoosePdu.DatSet + ", data structure mismatch !");
                                                break;
                                            }                                            
                                        }
                                        else
                                        {
                                            Logger.getLogger().LogError("Error while updating data in DataSet: " + DecodedGoose.GoosePdu.DatSet + ", data structure mismatch !");
                                            break;
                                        }
                                    }                                   
                                }
                            }
                        }
                    }
                }
            }

            if (addIedItem)
            {
                toolStripComboBox_Ieds.Enabled = true;

                ComboboxItem newItem = new ComboboxItem();                                               
                
                newItem.Text = RawGoose.SrcMac.ToString();

                if (DataSet == null)
                {
                    newItem.Text += " (Not in SCL)";
                    newItem.DataSet.Add(new NodeBase(""));
                    newItem.GVl.Add(ret[1] as NodeGVL);                    
                }
                else
                {
                    newItem.DataSet.Add(DataSet);
                    newItem.GVl.Add(ret[1] as NodeGVL);                    
                }

                newItem.RawGoose.Add(RawGoose);
                newItem.DecodedGoose.Add(DecodedGoose);
                
                toolStripComboBox_Ieds.Items.Add(newItem);

                if(toolStripComboBox_Ieds.Items.Count == 1)
                    toolStripComboBox_Ieds.SelectedIndex = 0;

                toolStripLabel_FoundIedsCnt.Text = toolStripComboBox_Ieds.Items.Count.ToString();
            }
            else if (addIedGooseItem)
            {
                ret = isGooseInSCD(RawGoose, DecodedGoose);                

                NodeBase ds = ret[0];               

                (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).RawGoose.Add(RawGoose);
                (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).DecodedGoose.Add(DecodedGoose);
                (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).DataSet.Add((ds == null) ? new NodeBase("") : ds);                       
                (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl.Add(ret[1] as NodeGVL);

                i = (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl.Count - 1;

                if (!(toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl[i].TreeCreated)
                {
                    updateTree((toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).RawGoose[i], (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).DecodedGoose[i], (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).DataSet[i], (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl[i]);
                    (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl[i].TreeCreated = true;
                }

                if (!(toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl[i].AddedToTreeView && (toolStripComboBox_Ieds.SelectedIndex == iedItemIndex))
                {
                    TreeNode n = myTreeView_Goose.Nodes.Add(DecodedGoose.GoosePdu.GocbRef);
                    n.Tag = (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl[i];
                    n.ImageIndex = 0;
                    n.SelectedImageIndex = 0;

                    makeTree_listNode((toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl[i], n);
                    (toolStripComboBox_Ieds.Items[iedItemIndex] as ComboboxItem).GVl[i].AddedToTreeView = true;
                }                
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public List<GooseData> RawGoose = new List<GooseData> { };
            public List<GOOSE> DecodedGoose = new List<GOOSE> { };
            public List<NodeBase> DataSet = new List<NodeBase> { };
            public List<NodeGVL> GVl = new List<NodeGVL> { };

            public override string ToString()
            {
                return Text;
            }
        }

        public delegate void AddDataCallback(string txt);

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LastSelectedIndex != toolStripComboBox_Ieds.SelectedIndex)
            {
                LastSelectedIndex = toolStripComboBox_Ieds.SelectedIndex;

                listViewClear();

                this.LiveViewChart.Titles[0].Text = "Select data in TreeView";
                LiveViewChart.Series[0].Points.Clear();
                LiveViewChart.Series[1].Points.Clear();

                ComboboxItem item = toolStripComboBox_Ieds.Items[toolStripComboBox_Ieds.SelectedIndex] as ComboboxItem;

                myTreeView_Goose.ImageList = new ImageList();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resource1));

                myTreeView_Goose.ImageList.Images.Add(((System.Drawing.Image)(resources.GetObject("page_white_text_width"))));
                myTreeView_Goose.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DO1"))));
                myTreeView_Goose.ImageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("DA1"))));
           
                myTreeView_Goose.Nodes.Clear();                

                for (int i = 0; i < item.RawGoose.Count; i++)
                {                
                    updateTree(item.RawGoose[i], item.DecodedGoose[i], item.DataSet[i], item.GVl[i]);
                    item.GVl[i].TreeCreated = true;

                    TreeNode n = myTreeView_Goose.Nodes.Add(item.DecodedGoose[i].GoosePdu.GocbRef);
                    n.Tag = item.GVl[i];
                    n.ImageIndex = 0;
                    n.SelectedImageIndex = 0;
                    makeTree_listNode(item.GVl[i], n);                                

                    item.GVl[i].AddedToTreeView = true;
                }
            }
        }

        private void recursiveCreate(NodeBase actualNode, NodeBase DataSetNode, GOOSE_ASN1_Model.Data RawData)
        {
            if (DataSetNode != null)
            {
                NodeBase[] DataSetNodeCn = DataSetNode.GetChildNodes();

                if (DataSetNodeCn != null)
                {
                    foreach (NodeBase Cn in DataSetNodeCn)
                    {
                        NodeGData Nd = new NodeGData((Cn as NodeData).Name);
                        Nd.DataType = (Cn as NodeData).DataType;

                        string addr = (Cn as NodeData).CommAddress.Domain + "/" + (Cn as NodeData).CommAddress.Variable.Replace("$ST$", "$DC$").Replace("$" + (Cn as NodeData).Name, "") + "$d";

                        NodeBase DescNd = _iecf.DataModel.ied.FindNodeByAddress(addr);
                        if (DescNd != null)
                        {
                            Nd.Description = (DescNd as NodeData).StringValue;
                        }                                                   
                 
                        NodeBase NdCn = actualNode.AddChildNode(Nd);
                        recursiveCreate(NdCn, Cn, null);
                    }
                }
                else
                    return;
            }
        }

        void makeTree_listNode(NodeBase nb, TreeNode tn)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = tn.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
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
                    tn3.ImageIndex = 1;
                    tn3.SelectedImageIndex = 1;

                    NodeBase[] b2cn = b2.GetChildNodes();

                    if (b2cn.Length > 0)                    
                        makeTree_listNode(b2, tn3);                    
                    else
                    {
                       tn3.ImageIndex = 2;
                       tn3.SelectedImageIndex = 2;
                    }
                }
            }
        }

        private void updateTree(GooseData RawGoose, GOOSE DecodedGoose, NodeBase DataSet, NodeBase Vl)
        {            
            NodeBase[] DataSetCn = null;
                                                           
            if (DecodedGoose.isGoosePduSelected())
            {
                DataSetCn = DataSet.GetChildNodes();

                if (DataSetCn.Length > 0 && !((Vl as NodeGVL).isGooseInSCLret < 0))
                {
                    foreach (NodeBase DsCn in DataSet.GetChildNodes())
                    {
                        NodeGData Nd = new NodeGData(DsCn.IecAddress);
                        Nd.DataType = (DsCn as NodeData).DataType;

                        string addr = (DsCn as NodeData).CommAddress.Domain + "/" + (DsCn as NodeData).CommAddress.Variable.Replace("$ST$", "$DC$").Replace("$" + (DsCn as NodeData).Name, "") + "$d";

                        NodeBase DescNd = _iecf.DataModel.ied.FindNodeByAddress(addr);
                        if (DescNd != null)
                        {
                            Nd.Description = (DescNd as NodeData).StringValue;
                        }  

                        NodeBase NdCn = Vl.AddChildNode(Nd);

                        recursiveCreate(NdCn, DsCn, null);                       
                    }

                    int i = 0;

                    foreach (GOOSE_ASN1_Model.Data AllDataCn in DecodedGoose.GoosePdu.AllData)
                    {
                        NodeBase[] NdCn = Vl.GetChildNodes();
                        
                        if ((DecodedGoose.GoosePdu.AllData.Count != NdCn.Length ) || recursiveReadData(NdCn[i++], AllDataCn, null, 0, RawGoose.packet.Timestamp) < 0)
                        {
                            Logger.getLogger().LogError("Error while updating data in DataSet: " + DecodedGoose.GoosePdu.DatSet + ", data structure mismatch !\nCannot associate received DataSet with data included in SCL file.\nData will be displayed in raw mode.");

                            foreach (NodeBase nb in NdCn)
                                Vl.RemoveChildNode(nb);
                            
                            (Vl as NodeGVL).isGooseInSCLret = -6; 

                            foreach (GOOSE_ASN1_Model.Data _AllDataCn in DecodedGoose.GoosePdu.AllData)                            
                                i = recursiveReadData(null, _AllDataCn, Vl, i, RawGoose.packet.Timestamp);                            
                            
                            break;
                        }
                    }
                }
                else
                {
                    int i = 0;

                    foreach (GOOSE_ASN1_Model.Data AllDataCn in DecodedGoose.GoosePdu.AllData)
                    {
                        i = recursiveReadData(null, AllDataCn, Vl, i, RawGoose.packet.Timestamp);
                    }
                }                
            }            
        }

        int recursiveReadData(NodeBase nd, GOOSE_ASN1_Model.Data t, NodeBase ndcn, int id, DateTime captureTime)
        {
            int _id = id;

            if(t == null)
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
                    (nd as NodeGData).DataValue = t.Bitstring.Value;
                    (nd as NodeGData).DataParam = t.Bitstring.TrailBitsCnt;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Bitstring_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.bit_string;
                    (nb as NodeGData).DataValue = t.Bitstring.Value;
                    (nb as NodeGData).DataParam = t.Bitstring.TrailBitsCnt;
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
                    NodeBase nb1 = ndcn.AddChildNode(nb);
                    int i = 0;
                    foreach (GOOSE_ASN1_Model.Data data in t.Structure.Value)
                        i = recursiveReadData(null, data, nb1, i, captureTime);

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
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return - 1;
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
                    return ++_id;
                }
                else
                    return -1;
            }
            else
                return -1;
        }

        private void Goose_FormClosing(object sender, FormClosingEventArgs e)
        {
            listViewClear();
            addDataDel -= new AddDataDelegate(AddData);

            if (_workerThread != null)
            {
                _run = false;
                _workerThread.Abort();
                _workerThread = null;
            }

            /*
            if (addDataRunner != null)
                if (addDataRunner.IsAlive == true)
                    addDataRunner.Abort();
            */      
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (_workerThread != null)
            {
                _run = false;
                _workerThread.Abort();                
                _workerThread = null;                
            }
            
            /*
            if (addDataRunner != null)
                if (addDataRunner.IsAlive)
                    addDataRunner.Abort();
            */
            
            this.LiveViewChart.Titles[0].Text = "Select data in TreeView";
            LiveViewChart.Series[0].Points.Clear();
            LiveViewChart.Series[1].Points.Clear();

            toolStripButton_Start.Enabled = true;
            toolStripButton_Stop.Enabled = false;
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
                ListViewItem lvi = new ListViewItem(new string[] { (n.IecAddress == "") ? n.Name : n.IecAddress, (n as NodeGData).DataType.ToString(), val, (n as NodeGData).Description });
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
                    if (tn.Text == "lists")
                    {
                        foreach (NodeBase nb in n.GetChildNodes())
                        {
                            ListViewItem li = this.listView_Goose.Items.Add(makeRow(nb));
                            li.Tag = nb.Tag;
                        }
                    }
                    if (n is NodeFile || tn.Text == "files")
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
                        {
                            recursiveAddLine(nb, tn);
                        }
                    }
                }
            }
            else
            {
                ListViewItem li = this.listView_Goose.Items.Add(makeRow(n));
                li.Tag = n.Tag;
            }
        }

        private void listViewClear()
        {
            foreach (ListViewItem l in this.listView_Goose.Items)
            {
                if (l.Tag is TreeNode)
                    if ((l.Tag as TreeNode).Tag is NodeGData)
                    {
                        ((l.Tag as TreeNode).Tag as NodeGData).ValueChanged -= new EventHandler(Node_ValueChanged);
                        ((l.Tag as TreeNode).Tag as NodeGData).AddDataToGraph -= new EventHandler(Node_AddDataToGraph);

                        /*
                        if (addDataRunner != null)
                            if (addDataRunner.IsAlive)
                                addDataRunner.Abort();
                        */
                    }

            }

            this.listView_Goose.Items.Clear();
        }

        private void myTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listViewClear();

            ListViewItem li;
            NodeBase n = (NodeBase)e.Node.Tag;
            if (n != null)
            {
                li = this.listView_Goose.Items.Add(makeRow(n));
                li.Tag = e.Node;
                if (n.GetChildNodes().Length > 0)
                {
                    this.listView_Goose.Items.Add(new ListViewItem(new string[] { "------------- CHILD NODES -------------", "-------------", "-------------", "-------------" }));
                    recursiveAddLine(n, e.Node);
                }

                if (n is NodeGData)
                {
                    if ((n as NodeGData).DataType == scsm_MMS_TypeEnum.boolean)
                    {
                        this.LiveViewChart.Titles[0].Text = (n as NodeGData).IecAddress + ((n as NodeGData).Description != null ? "   (" + (n as NodeGData).Description + ")": "");

                        if ((n as NodeGData).DataValue != null)
                            dataToAdd = ((n as NodeGData).DataValue.ToString() == "True") ? 10 : 0;

                        (n as NodeGData).AddDataToGraph += new EventHandler(Node_AddDataToGraph);

                        LiveViewChart.Series[0].Points.Clear();
                        LiveViewChart.Series[1].Points.Clear();

                        DateTime minValue = (n as NodeGData).CaptureTime;
                        DateTime maxValue = minValue.AddSeconds(1);                      
                        
                        LiveViewChart.ChartAreas[0].AxisX.Minimum = minValue.ToOADate();
                        LiveViewChart.ChartAreas[0].AxisX.Maximum = maxValue.ToOADate();

                        LiveViewChart.ChartAreas[0].AxisY.Minimum = 0;
                        LiveViewChart.ChartAreas[0].AxisY.Maximum = 10;
                        LiveViewChart.ChartAreas[0].AxisY.LabelStyle.Interval = 5;

                        LiveViewChart.ChartAreas[0].AxisY.CustomLabels.Clear();

                        LiveViewChart.ChartAreas[0].AxisY.CustomLabels.Add((double)0, (double)1, "FALSE (0)");
                        LiveViewChart.ChartAreas[0].AxisY.CustomLabels.Add((double)9, (double)10, "TRUE (1)");

                        foreach (Series ptSeries in LiveViewChart.Series)
                        {
                            AddNewPoint((n as NodeGData).CaptureTime, ptSeries);
                        }

                        /*
                        ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
                        addDataRunner = new Thread(addDataThreadStart);
                        addDataRunner.Start();
                        */
                    }
                    else if((n as NodeGData).DataType == scsm_MMS_TypeEnum.integer)
                    {
                        this.LiveViewChart.Titles[0].Text = (n as NodeGData).IecAddress + ((n as NodeGData).Description != null ? "   (" + (n as NodeGData).Description + ")" : "");

                        if ((n as NodeGData).DataValue != null)
                            dataToAdd = Convert.ToInt32((n as NodeGData).DataValue.ToString());

                        (n as NodeGData).AddDataToGraph += new EventHandler(Node_AddDataToGraph);

                        LiveViewChart.Series[0].Points.Clear();
                        LiveViewChart.Series[1].Points.Clear();

                        DateTime minValue = (n as NodeGData).CaptureTime;
                        DateTime maxValue = minValue.AddSeconds(3);

                        LiveViewChart.ChartAreas[0].AxisX.Minimum = minValue.ToOADate();
                        LiveViewChart.ChartAreas[0].AxisX.Maximum = maxValue.ToOADate();

                        LiveViewChart.ChartAreas[0].AxisY.Minimum = 0;                        
                        //chart1.ChartAreas[0].AxisY.Maximum = (dataToAdd > 10) ? dataToAdd + 5 : 10;
                        LiveViewChart.ChartAreas[0].AxisY.Maximum = Double.NaN;
                        LiveViewChart.ChartAreas[0].AxisY.LabelStyle.Interval = 0;

                        LiveViewChart.ChartAreas[0].AxisY.CustomLabels.Clear();

                        LiveViewChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

                        foreach (Series ptSeries in LiveViewChart.Series)
                        {
                            AddNewPoint((n as NodeGData).CaptureTime, ptSeries);
                        }

                        /*
                        ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
                        addDataRunner = new Thread(addDataThreadStart);
                        addDataRunner.Start();
                        */
                    }
                    else
                    {
                        LiveViewChart.Series[0].Points.Clear();
                        LiveViewChart.Series[1].Points.Clear();
                        this.LiveViewChart.Titles[0].Text = "DataType not supported";
                    }
                }
                else
                {
                    LiveViewChart.Series[0].Points.Clear();
                    LiveViewChart.Series[1].Points.Clear();
                    this.LiveViewChart.Titles[0].Text = "DataType not supported";
                }
            }
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

        void Node_AddDataToGraph(object sender, EventArgs e)
        {
            if (LiveViewChart.InvokeRequired)
            {
                OnAddDataToGraph d = new OnAddDataToGraph(Node_AddDataToGraph);
                if (!this.Disposing)
                    this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                DateTime timeStamp = (sender as NodeGData).CaptureTime;
                switch ((sender as NodeGData).DataType)
                {
                    case(scsm_MMS_TypeEnum.boolean):
                        dataToAdd = ((sender as NodeGData).DataValue.ToString() == "True") ? 10 : 0; 
                        break;
                    case(scsm_MMS_TypeEnum.integer):
                        dataToAdd = Convert.ToInt32((sender as NodeGData).DataValue.ToString());
                        break;
                }
                
                foreach (Series ptSeries in LiveViewChart.Series)
                {                    
                    AddNewPoint(timeStamp, ptSeries);                   
                }
            }            
        }

        private void addPlotDataThreadInit()
        {           
            this.LiveViewChart.Titles[0].Text = "Select data in TreeView";

            // create a delegate for adding data
            addDataDel += new AddDataDelegate(AddData);
        }

        private void AddDataThreadLoop()
        {
            while (true)
            {
                //LiveViewChart.Invoke(addDataDel);
                Thread.Sleep(100);
            }
        }

        public void AddData()
        {
            if (LiveViewChart.Series[1].Points.Count > 2)
            {
                offs = LiveViewChart.Series[1].Points[LiveViewChart.Series[1].Points.Count - 1].XValue - LiveViewChart.Series[1].Points[LiveViewChart.Series[1].Points.Count - 2].XValue;
                DateTime ts= DateTime.FromOADate(offs);
            }

            if (LiveViewChart.Series[0].Points.Count > 0)
            {
                double ts = LiveViewChart.Series[0].Points[LiveViewChart.Series[0].Points.Count - 1].XValue + offs;                

                AddNewPoint(DateTime.FromOADate(ts), LiveViewChart.Series[0]);
            }                                         
        }

        public void AddNewPoint(DateTime timeStamp, System.Windows.Forms.DataVisualization.Charting.Series ptSeries)
        {

            // Add new data point to its series.
            ptSeries.Points.AddXY(timeStamp.ToOADate(), dataToAdd);
            
            double removeBefore = timeStamp.AddSeconds((double)(0.9) * (-1)).ToOADate();
            
            while (ptSeries.Points[0].XValue < removeBefore)
            {
                ptSeries.Points.RemoveAt(0);
            }
            
            if (ptSeries == LiveViewChart.Series[0])
            {
                LiveViewChart.ChartAreas[0].AxisX.Minimum = ptSeries.Points[0].XValue;
                LiveViewChart.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddSeconds((double)1).ToOADate();
            }

            LiveViewChart.Invalidate();
        }
        
        private void myTreeView_Goose_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && (e.Node.Tag is NodeGVL))
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripItem item = menu.Items.Add("Export data from selected dataset");                     
                item.Tag = (e.Node).Tag;
                item.Click += new EventHandler(OnExportDataFromSelDatasetClick);

                item = menu.Items.Add("Export goose including this dataset");
                item.Tag = (e.Node).Tag;
                item.Click += new EventHandler(OnExportGooseWithSelDatasetClick);


                if (menu.Items.Count > 0)
                    menu.Show((Control)sender, e.Location);
            }
        }

        void OnExportDataFromSelDatasetClick(object sender, EventArgs e)
        {
            NodeGVL gvl = ((sender as ToolStripItem).Tag as NodeGVL);

            if (gvl.GetChildNodes().Length > 0)
            {
                ExportGvlToXml GvlToXml = new ExportGvlToXml();
                GvlToXml.Export(gvl);
            }
            else
                MessageBox.Show("No data to export", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void OnExportGooseWithSelDatasetClick(object sender, EventArgs e)
        {
            NodeGVL gvl = ((sender as ToolStripItem).Tag as NodeGVL);

            ExportGvlToXml GvlToXml = new ExportGvlToXml();
            GvlToXml.ExportGoose((gvl.Tag as Hashtable), gvl);

            /*
            if (gvl.GetChildNodes().Length > 0)
            {
                ExportGvlToXml GvlToXml = new ExportGvlToXml();
                GvlToXml.Export(gvl);
            }
            else
                MessageBox.Show("No data to export", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             */
        }
    }
}
