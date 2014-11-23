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

namespace IEDExplorer.Views
{
    public partial class CaptureView : DockContent
    {
        delegate void AddPacketDelegate(MMSCapture cap);

        ByteViewer bv = new ByteViewer();
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
            bv.Location = new Point(0, 0);
            bv.Size = new Size(20, 20);
            bv.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            bv.AutoSize = true;
            this.splitContainer2.Panel2.Controls.Add(bv);
            bv.SetBytes(new byte[] { 5, 33, 16, 172, 55 });

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
                lit.SubItems.Add(cap.Time.ToString());
                lit.SubItems.Add(cap.Direction == MMSCapture.CaptureDirection.In ? ">>" : "<<");
                lit.SubItems.Add(cap.MMSPduType);
                lit.SubItems.Add(cap.MMSPduService);
                listView1.Items.Add(lit);
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (OnClearCapture != null) OnClearCapture();
        }
    }
}
