using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer
{       
    public partial class ControllableSignalsDialog : Form
    {
        class MyListViewAlwaysSelected : ListView
        {
            protected override void WndProc(ref Message m)
            {
                // Swallow mouse messages that are not in the client area
                if (m.Msg >= 0x201 && m.Msg <= 0x209)
                {
                    Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                    var hit = this.HitTest(pos);
                    switch (hit.Location)
                    {
                        case ListViewHitTestLocations.AboveClientArea:
                        case ListViewHitTestLocations.BelowClientArea:
                        case ListViewHitTestLocations.LeftOfClientArea:
                        case ListViewHitTestLocations.RightOfClientArea:
                        case ListViewHitTestLocations.None:
                            return;
                    }
                }
                base.WndProc(ref m);
            }
        }

        NodeBase data;
        static long m_ctlNum = 0;
        
        private ControllableSignalsDialog()
        {
            InitializeComponent();
        }

        internal ControllableSignalsDialog(NodeBase data/*, EventHandler onNVListChanged)*/)
        {
            InitializeComponent();            
            this.data = data;
            this.TopMost = true;          

            button2.Enabled = false;
            button3.Enabled = false;
                        
            Iec61850State iecs = data.GetIecs();

            NodeBase[] datacns = data.GetChildNodes();
                     
            foreach(NodeLN datacn in datacns)
            {
                NodeBase[] nfccns = datacn.GetChildNodes();

                foreach (NodeFC nfccn in nfccns)
                {
                    if (nfccn.Name == "CO")
                    {
                        NodeBase[] ndatacns = nfccn.GetChildNodes();

                        foreach (NodeDO ndatacn in ndatacns)
                        {
                            if (ndatacn.Name.Contains("SPCSO") || ndatacn.Name.Contains("RcdTrg") || ndatacn.Name.Contains("Pos"))
                            {
                                NodeBase ctlval = iecs.DataModel.ied.FindNodeByAddress(ndatacn.CommAddress.Domain + "/" + ndatacn.CommAddress.Variable + "$Oper$ctlVal");
                                NodeBase desc = iecs.DataModel.ied.FindNodeByAddress(ndatacn.CommAddress.Domain + "/" + ndatacn.CommAddress.Variable.Replace("$CO$", "$DC$") + "$d");
                                
                                if (ctlval != null)
                                {
                                    ListViewItem item = new ListViewItem(new[] { ctlval.IecAddress, ((desc != null) ? (desc as NodeData).StringValue : ""),  });
                                    item.Tag = ctlval;
                                    listView1.Items.Add(item);                                  
                                }
                            }
                        }
                    }
                }
            }
                         
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {         
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button_Click(object sender, EventArgs e)
        {
            NodeData var = (NodeData)listView1.SelectedItems[0].Tag;
            Iec61850State iecs = var.GetIecs();
            NodeData[] ndarr = new NodeData[1];                        

            if ((var != null) && (iecs != null))
            {
                NodeData d = (NodeData)var.Parent;
                NodeData b, c, f = null;
                NodeData g = null;
                
                if(d != null)
                {                                    
                    f = new NodeData(d.Name);
                    ndarr[0] = f;

                    if((b = (NodeData)d.FindChildNode("ctlVal")) != null)
                    {                        
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        switch (n.DataType)
                        {
                            case scsm_MMS_TypeEnum.boolean:
                                n.DataValue = (((Button)sender).Text == button2.Text) ? true : false;                                                              
                                break;
                        }                                                
                        f.AddChildNode(n);
                    }
                    if ((b = (NodeData)d.FindChildNode("origin")) != null)
                    {                        
                        g = (NodeData)f.FindChildNode("origin");
                     
                        if (g == null)
                        {
                            g = new NodeData(b.Name);
                            g.DataType = ((NodeData)b).DataType;
                        }           

                        if ((c = (NodeData)b.FindChildNode("orCat")) != null)
                        {
                            NodeData n = new NodeData(c.Name);
                            n.DataType = ((NodeData)c).DataType;
                            n.DataValue = 2L;
                            ((NodeData)c).DataValue = 2L;                            
                            g.AddChildNode(n);
                        }
                        if ((c = (NodeData)b.FindChildNode("orIdent")) != null)
                        {
                            NodeData n = new NodeData(c.Name);
                            n.DataType = ((NodeData)c).DataType;
                            string orIdent = "IEDExplorer";                            
                            byte[] bytes = new byte[orIdent.Length];
                            int tmp1, tmp2; bool tmp3;
                            Encoder ascii = (new ASCIIEncoding()).GetEncoder();
                            ascii.Convert(orIdent.ToCharArray(), 0, orIdent.Length, bytes, 0, orIdent.Length, true, out tmp1, out tmp2, out tmp3);
                            n.DataValue = bytes;                                                       
                            g.AddChildNode(n);                            
                        }
                        
                        if((g.FindChildNode("orCat") != null) && (g.FindChildNode("orIdent") != null))
                            f.AddChildNode(g);
                    }
                    if ((b = (NodeData)d.FindChildNode("ctlNum")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = m_ctlNum++;                                                                        
                        f.AddChildNode(n);
                    }
                    if ((b = (NodeData)d.FindChildNode("T")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        TimeSpan diff = DateTime.Now - origin;
                        uint ui = (uint)Math.Floor(diff.TotalSeconds);
                        byte[] uib = BitConverter.GetBytes(ui);
                        n.DataValue = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };                                                
                        f.AddChildNode(n);
                    }
                    if ((b = (NodeData)d.FindChildNode("Test")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = false;                        
                        f.AddChildNode(n);
                    }
                    if ((b = (NodeData)d.FindChildNode("Check")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0xC0 };
                        //n.DataParam = ((NodeData)b).DataParam;
                        n.DataParam = 6;                                             
                        f.AddChildNode(n);
                    }                    
                                        
                    iecs.Send(ndarr, d.CommAddress, ActionRequested.WriteAsStructure);
                }
                else
                    MessageBox.Show("Basic structure not found!");
            }                
         }             
    }
}
