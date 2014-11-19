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
        ByteViewer bv = new ByteViewer();
        WindowManager winMgr;
        public bool CaptureActive;
        CheckBox cb = new CheckBox();
        public delegate void CaptureActiveChanged(bool captureActive);
        public event CaptureActiveChanged OnCaptureActiveChanged;

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
                if (OnCaptureActiveChanged != null) OnCaptureActiveChanged(this.CaptureActive);
            };
            ToolStripControlHost host = new ToolStripControlHost(cb);
            toolStrip1.Items.Insert(0,host);        }
    }
}
