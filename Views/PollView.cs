using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer.Views {
    public partial class PollView : WeifenLuo.WinFormsUI.Docking.DockContent {
        Env _env;
        Boolean showOnce = false;
        public Iec61850State Iecs
        {
            set; get;
        }
        public PollView ()
        {
            _env = Env.getEnv();
            InitializeComponent();
            cbRefreshInterval.SelectedIndex = 0;
            PollTimer.Interval = calcInterval(cbRefreshInterval.SelectedIndex);

            //tsbImportPollingList.hide
        }

        private void addVar (string address, string datatype, string val, CommAddress comaddr)
        {
            //public Boolean showErrOnce { set; get; }

            if (PollListView.FindItemWithText(address) == null) {
                tsbStart.Enabled = true;
                ListViewItem lvi = new ListViewItem(new string[] { address, datatype, val });
                lvi.Tag = new CommAddress { Domain = comaddr.Domain, Variable = comaddr.Variable, owner = comaddr.owner };
                PollListView.Items.Add(lvi);
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
            PollTimer.Start();
            tsbStart.Enabled = false;
            tsbStop.Enabled = true;
            tsbRefresh.Enabled = false;
        }

        private void tsbStop_Click (object sender, EventArgs e)
        {
            tsbStart.Enabled = true;
            tsbStop.Enabled = false;
            tsbRefresh.Enabled = true;
            PollTimer.Stop();
        }

        private void cbRefreshInterval_SelectedIndexChanged (object sender, EventArgs e)
        {
            PollTimer.Interval = calcInterval(cbRefreshInterval.SelectedIndex);
        }

        private void PollView_FormClosing (object sender, FormClosingEventArgs e)
        {
            PollTimer.Stop();
            tsbStart.Enabled = true;
            tsbStop.Enabled = false;
        }

        private void PollTimer_Tick (object sender, EventArgs e)
        {
            int i;

            Iecs = _env.winMgr.mainWindow.Get_iecf();
            if (Iecs == null)
                return;

            for (i = 0; i < PollListView.Items.Count; i++) {
                NodeData nd = (NodeData)Iecs.DataModel.ied.FindNodeByAddress(((CommAddress)PollListView.Items[i].Tag).Domain + "/" +
                                                                    ((CommAddress)PollListView.Items[i].Tag).Variable);

                if (nd != null) {
                    if (PollListView.Items[i].SubItems[2].Text != nd.StringValue) {
                        PollListView.Items[i].ForeColor = Color.Red;
                        PollListView.Items[i].SubItems[2].Text = nd.StringValue;
                    } else
                        PollListView.Items[i].ForeColor = Color.Black;

                    NodeBase[] ndarr = new NodeBase[1];
                    ndarr[0] = nd;
                    Iecs.Send(ndarr, nd.CommAddress, ActionRequested.Read);
                }
            }
        }

        private void PollListView_DragDrop (object sender, DragEventArgs e)
        {

            if (PollTimer.Enabled)
            {
                Logger.getLogger().LogWarning("PollView: Stop polling before entering new data to polling list!");
                return;
            }

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

        private void PollListView_DragEnter (object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof(NodeData))))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void PollView_DragEnter (object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof(NodeData))))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            PollTimer_Tick(null, null);
        }

        private void tsbImportPollingList_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Function not yet implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbExportList_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Function not yet implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            tsbStart.Enabled = true;
            tsbStop.Enabled = false;
            tsbRefresh.Enabled = true;
            PollTimer.Stop();
            PollListView.Items.Clear();
        }
    }
}
