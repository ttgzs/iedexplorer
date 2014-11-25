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

namespace IEDExplorer.Views
{
    public partial class CaptureView : DockContent
    {
        delegate void AddPacketDelegate(MMSCapture cap);

        //ByteViewer bv = new ByteViewer();
        //HexBox hb = new HexBox();
        WindowManager winMgr;
        public bool CaptureActive;
        CheckBox cb = new CheckBox();
        public delegate void CaptureActiveChanged(bool captureActive);
        public event CaptureActiveChanged OnCaptureActiveChanged;
        public delegate void ClearCapture();
        public event ClearCapture OnClearCapture;

        public CaptureView(WindowManager wm)
        {
            winMgr = wm;
            InitializeComponent();
            /*bv.Location = new Point(0, 0);
            bv.Size = new Size(20, 20);
            bv.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            bv.AutoSize = true;*/
            /*hb.Parent = splitContainer2.Panel2;
            this.splitContainer2.Panel2.Controls.Add(hb);
            hb.Location = new Point(0, 0);
            hb.Size = new Size(200, 200);
            hb.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            hb.AutoSize = true;*/
            //bv.SetBytes(new byte[] { 5, 33, 16, 172, 55 });

            cb.Text = "Start capture";
            cb.CheckStateChanged += (s, ex) =>
            {
                this.CaptureActive = cb.CheckState == CheckState.Checked ? true : false;
                this.toolStripButtonClear.Enabled = cb.CheckState == CheckState.Checked ? false : true;
                if (OnCaptureActiveChanged != null) OnCaptureActiveChanged(this.CaptureActive);
            };
            ToolStripControlHost host = new ToolStripControlHost(cb);
            toolStrip1.Items.Insert(0,host);        
        }

        public void AddPacket(MMSCapture cap)
        {
            if (listView1.InvokeRequired)
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
                listView1.Items.Add(lit);
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (OnClearCapture != null) OnClearCapture();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                MMSCapture cap = (listView1.SelectedItems[0].Tag as MMSCapture);
                textBoxXML.Text = cap.XMLPdu;
                //bv.SetBytes(cap.EncodedPacket);
                hexBox1.ByteProvider = new DynamicByteProvider(cap.EncodedPacket);
            }
            else
            {
                textBoxXML.Text = "";
                //bv.SetBytes(new byte[] { });
                hexBox1.ByteProvider = null;
            }
        }
    }
}
