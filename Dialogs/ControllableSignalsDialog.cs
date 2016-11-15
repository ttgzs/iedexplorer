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
            NodeData data = (NodeData)listView1.SelectedItems[0].Tag;
            
            CommandParams cPar = iecs.Controller.PrepareSendCommand((NodeBase)listView1.SelectedItems[0].Tag);
            if (cPar != null) {
                if ((String)(((Button)sender).Tag) == "true")
                    cPar.ctlVal = true;
                else
                    cPar.ctlVal = false;
                iecs.Controller.SendCommand(data, cPar, ActionRequested.WriteAsStructure);
            }
            return;

         }             
    }
}
