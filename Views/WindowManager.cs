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
            envir.winMgr = this;
            mainWindow = mWin;
            //Create toolwindows
            iedWindow = new IedTreeView(this);
            iedWindow.CloseButtonVisible = false;
            iedWindow.Show(dockPanel, DockState.DockLeft);
            //iedWindow.Parent = (Form)mainWindow;
            //iedWindow.SelectNode += new ProjView.SelectNodeHandler(OnSelectProjectNode);

            dataWindow = new IedDataView(env);
            dataWindow.ShowHint = DockState.Document;
            dataWindow.CloseButtonVisible = false;
            dataWindow.Show(dockPanel);

            captureWindow = new CaptureView(this);
            captureWindow.ShowHint = DockState.Document;
            captureWindow.CloseButtonVisible = false;
            captureWindow.Show(dockPanel);

            logWindow = new LogView(env);
            logWindow.ShowHint = DockState.DockBottom;
            logWindow.CloseButtonVisible = false;
            //logWindow.Parent = (Form)mainWindow;
            logWindow.Show(dockPanel);

            //Connect Windows Manager to helper events
            dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
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

        internal void SelectNode(TreeNode tn)
        {
            if (dataWindow != null)
            {
                dataWindow.SelectNode(tn);
            }
        }

        internal void GetCaptureActive(Iec61850State iecs)
        {
            captureWindow.OnCaptureActiveChanged += (ca) =>
            {
                iecs.CaptureDb.CaptureActive = ca;
            };
            iecs.CaptureDb.CaptureActive = captureWindow.CaptureActive;
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
