using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using GOOSE_ASN1_Model;

namespace IEDExplorer.Views
{
    public class WindowManager
    {
        WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;

        List<DockContent> documentViews = new List<DockContent>();
        DockContent currentDocument;

        public MainWindow mainWindow;
        LogView logWindow;
        IedTreeView iedWindow;
        IedDataView dataWindow;
        CaptureView captureWindow;
        ReportsView reportWindow;
        PollView pollWindow;

        public List<int> SCLServers_usedPorts = new List<int>();
        public bool ReportsRunning;

        WatchDataView watchWindow;

        int gseViewsCount = 0;
        int gexViewsCount = 0;

        public Env _env;

        public WindowManager(DockPanel dockPanel, MainWindow mWin)
        {
            this.dockPanel = dockPanel;
            _env = Env.getEnv();
            _env.winMgr = this;
            mainWindow = mWin;
            //Create toolwindows
            iedWindow = new IedTreeView(this);
            iedWindow.CloseButtonVisible = false;
            iedWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            iedWindow.Show(dockPanel, DockState.DockLeft);

            dataWindow = new IedDataView();
            dataWindow.ShowHint = DockState.Document;
            dataWindow.CloseButtonVisible = false;
            dataWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            dataWindow.Show(dockPanel);

            reportWindow = new ReportsView();
            reportWindow.ShowHint = DockState.Document;
            reportWindow.CloseButtonVisible = false;
            reportWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            reportWindow.Show(dockPanel);

            pollWindow = new PollView();
            pollWindow.ShowHint = DockState.Document;
            pollWindow.CloseButtonVisible = false;
            pollWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            pollWindow.Show(dockPanel);

            captureWindow = new CaptureView();
            captureWindow.ShowHint = DockState.Document;
            captureWindow.CloseButtonVisible = false;
            captureWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            captureWindow.Show(dockPanel);

            watchWindow = new WatchDataView();
            watchWindow.ShowHint = DockState.Document;
            watchWindow.CloseButtonVisible = false;
            watchWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            //watchWindow.Show(dockPanel);

            logWindow = new LogView();
            logWindow.ShowHint = DockState.DockBottom;
            logWindow.CloseButtonVisible = false;
            logWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            logWindow.Show(dockPanel);

            //Connect Windows Manager to helper events
            dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
        }

        // Prevention against user closing the window which should be always visible
        void persistentWindows_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
        }

        void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            currentDocument = (DockContent)dockPanel.ActiveDocument;
        }

        public void ForceWindowsClose()
        {
            while (documentViews.Count > 0)
            {
                DockContent doc = documentViews[0];
                doc.Close(); //this window should be removed from documentViews on closing
                documentViews.Remove(doc);
            }
            documentViews.Clear();

            dockPanel.ActiveDocumentChanged -= new EventHandler(OnActiveDocumentChanged);
        }

        MainWindow MainWindow { get { return mainWindow; } }

        internal void MakeIedTree(Iec61850State iecs)
        {
            if (iedWindow != null)
            {
                iedWindow.makeTree(iecs);
            }
        }

        internal void MakeIecTree(Iec61850State iecs)
        {
            if (iedWindow != null)
            {
                iedWindow.makeTreeIec(iecs);
            }
        }

        internal void MakeFileTree(Iec61850State iecs)
        {
            if (iedWindow != null)
            {
                iedWindow.Node_DirectoryUpdated(iecs.DataModel.files, null);
            }
        }

        internal void SelectNode(TreeNode tn)
        {
            if (dataWindow != null)
            {
                dataWindow.SelectNode(tn);
            }
        }

        internal void BindToCapture(Iec61850State iecs)
        {
            captureWindow.OnCaptureActiveChanged += (ca) =>
            {
                iecs.CaptureDb.CaptureActive = ca;
            };
            captureWindow.OnClearCapture += () =>
            {
                iecs.CaptureDb.Clear();
            };
            iecs.CaptureDb.CaptureActive = captureWindow.CaptureActive;
            iecs.CaptureDb.OnNewPacket += (cap) =>
            {
                captureWindow.AddPacket(cap);
            };
            // re-use for Reports
            iecs.Controller.NewReport += reportWindow.ReportsView_OnNewReport;
        }

        internal void UnBindFromCapture(Iec61850State iecs)
        {
            captureWindow.OnCaptureActiveChanged -= (ca) =>
            {
                iecs.CaptureDb.CaptureActive = ca;
            };
            captureWindow.OnClearCapture -= () =>
            {
                iecs.CaptureDb.Clear();
            };
            iecs.CaptureDb.OnNewPacket -= (cap) =>
            {
                captureWindow.AddPacket(cap);
            };
            // re-use for Reports
            iecs.Controller.NewReport -= reportWindow.ReportsView_OnNewReport;
        }

        public void AddSCLView(string filename)
        {
            foreach (DockContent dc in documentViews)
            {
                if (dc is SCLView)
                {
                    if ((dc as SCLView).Filename == filename)
                    {
                        dc.Show();
                        return;
                    }
                }
            }

            DockContent sclView = new SCLView(filename);
            sclView.FormClosed += new FormClosedEventHandler(sclView_FormClosed);
            documentViews.Add(sclView);
            sclView.Show(dockPanel);
        }

        void sclView_FormClosed(object sender, FormClosedEventArgs e)
        {
            SCLView sclView = (sender as SCLView);
            sclView.FormClosed -= new FormClosedEventHandler(sclView_FormClosed);
            sclView.StopServers();
            documentViews.Remove(sclView);
        }

        public void AddGooseExplorer(Iec61850State iecs, Logger logger)
        {
            DockContent gexView = new GooseExplorer(iecs, logger);
            gexView.FormClosed += new FormClosedEventHandler(gexView_FormClosed);
            gexView.TabText = "GooseExplorer " + ++gexViewsCount;
            if (iecs != null && iecs.DataModel != null && iecs.DataModel.ied != null)
            {
                gexView.TabText += ": " + iecs.hostname;
            }
            documentViews.Add(gexView);
            gexView.Show(dockPanel);
        }

        void gexView_FormClosed(object sender, FormClosedEventArgs e)
        {
            GooseExplorer gexView = (sender as GooseExplorer);
            gexView.FormClosed -= new FormClosedEventHandler(gexView_FormClosed);
            documentViews.Remove(gexView);
            --gexViewsCount;
        }

        public GooseDataEdit AddGooseDataEdit(string name, List<Data> dataList, List<SeqData> seqData, EventHandler ValueChanged)
        {
            DockContent gdeView = new GooseDataEdit(name, dataList, seqData, ValueChanged);
            gdeView.FormClosed += new FormClosedEventHandler(gdeView_FormClosed);
            documentViews.Add(gdeView);
            gdeView.Show(dockPanel);
            return (GooseDataEdit)gdeView;
        }

        void gdeView_FormClosed(object sender, FormClosedEventArgs e)
        {
            GooseExplorer gdeView = (sender as GooseExplorer);
            gdeView.FormClosed -= new FormClosedEventHandler(gdeView_FormClosed);
            documentViews.Remove(gdeView);
        }

        public void AddGooseSender()
        {
            DockContent gseView = new GooseSender();
            gseView.FormClosed += new FormClosedEventHandler(gseView_FormClosed);
            gseView.TabText = "GooseSender " + ++gseViewsCount;
            documentViews.Add(gseView);
            gseView.Show(dockPanel, DockState.DockRight);
        }

        void gseView_FormClosed(object sender, FormClosedEventArgs e)
        {
            GooseSender gseView = (sender as GooseSender);
            gseView.FormClosed -= new FormClosedEventHandler(gseView_FormClosed);
            documentViews.Remove(gseView);
            --gseViewsCount;
        }

        internal void AddAddNVLView(NodeVL list, NodeBase lists, TreeNode listsNode, EventHandler onNVListChanged)
        {
            DockContent nvlView = new AddNVLView(list, lists, listsNode, onNVListChanged);
            nvlView.FormClosed += new FormClosedEventHandler(nvlView_FormClosed);
            documentViews.Add(nvlView);
            nvlView.Show(dockPanel);
        }

        void nvlView_FormClosed(object sender, FormClosedEventArgs e)
        {
            (sender as AddNVLView).FormClosed -= new FormClosedEventHandler(nvlView_FormClosed);
            documentViews.Remove(sender as AddNVLView);
        }

        #region IDisposable Members

        public void Dispose()
        {
            ForceWindowsClose();
            //projWindow.SelectNode -= new ProjView.SelectNodeHandler(OnSelectProjectNode);
            dockPanel.ActiveDocumentChanged -= new EventHandler(OnActiveDocumentChanged);

            //Create toolwindows
            iedWindow.Dispose();
        }

        #endregion

    }
}
