using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer.Views {
    public partial class ReportsView : WeifenLuo.WinFormsUI.Docking.DockContent {

        delegate void OnValueCallback (object sender, EventArgs e);
        public Env environment;
        public bool DoNotShowAUStoppedDialog
        {
            get; set;
        }

        public ReportsView (Env env)
        {
            environment = env;
            InitializeComponent();
            DoNotShowAUStoppedDialog = false;
        }

        private void tsbClearReports_Click (object sender, EventArgs e)
        {
            ReportlistView.Items.Clear();
        }

        private void tsbExportCSV_Click (object sender, EventArgs e)
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
    }
}
