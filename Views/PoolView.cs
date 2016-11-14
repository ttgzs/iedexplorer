using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer.Views {
    public partial class PoolView : WeifenLuo.WinFormsUI.Docking.DockContent {
        public Env environment;
        Boolean showOnce = false;
        public Iec61850State Iecs
        {
            set; get;
        }
        public PoolView (Env env)
        {
            environment = env;
            InitializeComponent();
            cbRefreshInterval.SelectedIndex = 0;
            PoolTimer.Interval = calcInterval(cbRefreshInterval.SelectedIndex);
        }

        private void addVar (string address, string datatype, string val, CommAddress comaddr)
        {
            //public Boolean showErrOnce { set; get; }

            if (PoolListView.FindItemWithText(address) == null) {
                tsbStart.Enabled = true;
                ListViewItem lvi = new ListViewItem(new string[] { address, datatype, val });
                lvi.Tag = new CommAddress { Domain = comaddr.Domain, Variable = comaddr.Variable, owner = comaddr.owner };
                PoolListView.Items.Add(lvi);
            } else {
                if (!showOnce) {
                    MessageBox.Show("Data exists in list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    showOnce = true;
                }
            }
        }
        private int calcInterval (int index)
        {
            switch (index) {
                case 0:
                    return 500;
                case 1:
                    return 1000;
                case 2:
                    return 2000;
                case 3:
                    return 5000;
                case 4:
                    return 10000;
                case 5:
                    return 15000;
                default:
                    return -1;
            }
        }

        private void tsbStart_Click (object sender, EventArgs e)
        {
            PoolTimer.Start();
            tsbStart.Enabled = false;
            tsbStop.Enabled = true;
        }

        private void tsbStop_Click (object sender, EventArgs e)
        {
            tsbStart.Enabled = true;
            tsbStop.Enabled = false;
            PoolTimer.Stop();
        }

        private void cbRefreshInterval_SelectedIndexChanged (object sender, EventArgs e)
        {
            PoolTimer.Interval = calcInterval(cbRefreshInterval.SelectedIndex);
        }

        private void PoolView_FormClosing (object sender, FormClosingEventArgs e)
        {
            PoolTimer.Stop();
            tsbStart.Enabled = true;
            tsbStop.Enabled = false;
            this.Hide();
            e.Cancel = true;
        }

        private void PoolTimer_Tick (object sender, EventArgs e)
        {
            int i;

            Iecs = environment.winMgr.mainWindow.Get_iecf();
            if (Iecs == null)
                return;

            for (i = 0; i < PoolListView.Items.Count; i++) {
                NodeData nd = (NodeData)Iecs.DataModel.ied.FindNodeByAddress(((CommAddress)PoolListView.Items[i].Tag).Domain + "/" +
                                                                    ((CommAddress)PoolListView.Items[i].Tag).Variable);

                if (nd != null) {
                    if (PoolListView.Items[i].SubItems[2].Text != nd.StringValue) {
                        PoolListView.Items[i].ForeColor = Color.Red;
                        PoolListView.Items[i].SubItems[2].Text = nd.StringValue;
                    } else
                        PoolListView.Items[i].ForeColor = Color.Black;

                    NodeBase[] ndarr = new NodeBase[1];
                    ndarr[0] = nd;
                    Iecs.Send(ndarr, nd.CommAddress, ActionRequested.Read);
                }

            }

        }

        private void PoolListView_DragDrop (object sender, DragEventArgs e)
        {
            NodeData d;

            if ((d = (NodeData)e.Data.GetData(typeof(NodeData))) != null) {
                NodeBase[] cn = d.GetChildNodes();

                if (cn != null) {
                    if (cn.Length > 0) {
                        showOnce = false;

                        foreach (NodeData nd in cn)
                            addVar(nd.IecAddress, nd.DataType.ToString(), nd.StringValue, nd.CommAddress);
                    } else {
                        showOnce = false;

                        addVar(d.IecAddress, d.DataType.ToString(), d.StringValue, d.CommAddress);
                    }
                }
            }
        }

        private void PoolListView_DragEnter (object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof(NodeData))))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void PoolView_DragEnter (object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof(NodeData))))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
