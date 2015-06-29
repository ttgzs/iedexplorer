using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace IEDExplorer.Views
{
    public class WindowManager
    {
        WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;

        List<DockContent> documentViews = new List<DockContent>();
        DockContent currentDocument;

        MainWindow mainWindow;
        LogView logWindow;
        IedTreeView iedWindow;
        IedDataView dataWindow;
        CaptureView captureWindow;

        public Env env;

        public WindowManager(DockPanel dockPanel, Env envir, MainWindow mWin)
        {
            this.dockPanel = dockPanel;
            env = envir;
            env.winMgr = this;
            mainWindow = mWin;
            //Create toolwindows
            iedWindow = new IedTreeView(this);
            iedWindow.CloseButtonVisible = false;
            iedWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            iedWindow.Show(dockPanel, DockState.DockLeft);

            dataWindow = new IedDataView(env);
            dataWindow.ShowHint = DockState.Document;
            dataWindow.CloseButtonVisible = false;
            dataWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            dataWindow.Show(dockPanel);

            captureWindow = new CaptureView(this);
            captureWindow.ShowHint = DockState.Document;
            captureWindow.CloseButtonVisible = false;
            captureWindow.FormClosing += new FormClosingEventHandler(persistentWindows_FormClosing);
            captureWindow.Show(dockPanel);

            logWindow = new LogView(env);
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
            (sender as SCLView).FormClosed -= new FormClosedEventHandler(sclView_FormClosed);
            documentViews.Remove(sender as SCLView);
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
