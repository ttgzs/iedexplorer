using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer.Views {
    public partial class ReportsView : WeifenLuo.WinFormsUI.Docking.DockContent {

        //delegate void OnValueCallback (object sender, EventArgs e);
        delegate void OnReportReceivedCallback(string rptdVarQualityLog, string rptdVarTimestampLog, string rptdVarPathLogstring, string rptdVarDescriptionLog, string rptdVarValueLog);

        Env _env;

        public bool DoNotShowAUStoppedDialog
        {
            get;
            set;
        }

        public bool ReportsRunning
        {
            get { return _env.winMgr.ReportsRunning; }
            set { _env.winMgr.ReportsRunning = value; }
        }

        public ReportsView()
        {
            _env = Env.getEnv();
            InitializeComponent();
            DoNotShowAUStoppedDialog = false;
            ReportsRunning = false;
        }

        private void tsbClearReports_Click (object sender, EventArgs e)
        {
            ReportlistView.Items.Clear();
        }

        public void ReportsView_OnNewReport(string rptdVarQualityLog, string rptdVarTimestampLog, string rptdVarPathLogstring, string rptdVarDescriptionLog, string rptdVarValueLog)
        {
            if (ReportlistView.InvokeRequired)
            {
                OnReportReceivedCallback d = new OnReportReceivedCallback(ReportsView_OnNewReport);
                this.Invoke(d, new object[] { rptdVarQualityLog, rptdVarTimestampLog, rptdVarPathLogstring, rptdVarDescriptionLog, rptdVarValueLog });
            }
            else
            {
                ReportlistView.BeginUpdate();

                ListViewItem item = new ListViewItem(new[] { (ReportlistView.Items.Count + 1).ToString(), rptdVarQualityLog, rptdVarTimestampLog, rptdVarPathLogstring, rptdVarDescriptionLog, rptdVarValueLog });
                ReportlistView.Items.Add(item);

                item.EnsureVisible();
                ReportlistView.EndUpdate();
            }
        }

        private void tsbExportCSV_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog exportSaveFileDialog = new SaveFileDialog()) {
                exportSaveFileDialog.Title = "Select CSV File";
                exportSaveFileDialog.Filter = "CSV file(*.csv)|*.csv";

                if (DialogResult.OK == exportSaveFileDialog.ShowDialog()) {
                    string fullFileName = exportSaveFileDialog.FileName;
                    LVToCSV.ListViewToCSV(ReportlistView, exportSaveFileDialog.FileName, false);
                    MessageBox.Show("Events exported successfully", "Exported to CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void tsbRunReports_Click(object sender, EventArgs e)
        {
            ReportsRunning = true;
            tsbRunReports.Enabled = false;
            tsbStopReports.Enabled = true;
        }

        private void tsbStopReports_Click(object sender, EventArgs e)
        {
            ReportsRunning = false;
            tsbRunReports.Enabled = true;
            tsbStopReports.Enabled = false;
        }
    }
}
