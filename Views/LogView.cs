using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace IEDExplorer.Views
{
    public partial class LogView : DockContent
    {
        Logger logger = Logger.getLogger();
        delegate void OnMessageCallback(string message);
        Env _env;

        public LogView()
        {
            _env = Env.getEnv();
            InitializeComponent();
            logger.OnLogMessage += new Logger.OnLogMessageDelegate(logger_OnLogMessage);
            logger.OnClearLog += new Logger.OnClearLogDelegate(logger_OnClearLog);
        }

        #region Logger

        void logger_OnClearLog()
        {
            dataGridView_log.Rows.Clear();
        }

        void logger_OnLogMessage(string message)
        {
            if (dataGridView_log.InvokeRequired)
            {
                OnMessageCallback d = new OnMessageCallback(logger_OnLogMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                dataGridView_log.Rows.Add(message);
                dataGridView_log.FirstDisplayedScrollingRowIndex = dataGridView_log.RowCount - 1;
                //listViewLog.BeginUpdate();
                //ListViewItem item = dataGridView_log.Items.Add(message);
                if (message.Contains("Error"))
                    dataGridView_log.Rows[dataGridView_log.RowCount - 1].Cells[0].Style.ForeColor = Color.Red;
                    //item.ForeColor = Color.Red;
                else if (message.Contains("Warning"))
                    dataGridView_log.Rows[dataGridView_log.RowCount - 1].Cells[0].Style.ForeColor = Color.Blue;
                    //item.ForeColor = Color.Blue;
                //item.EnsureVisible();
                //listViewLog.EndUpdate();
            }
        }

        private void listViewLog_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(dataGridView_log, e.X, e.Y);
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView_log.Rows.Clear();
        }

        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Log files|*.txt";
            saveFileDialog1.Title = "Save Log to File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                FileStream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter writer = new StreamWriter(stream);
                foreach (DataGridViewRow r in dataGridView_log.Rows)
                {
                    writer.WriteLine(r.Cells[0].Value.ToString());
                }
                writer.Close();
            }
        }
        #endregion Logger

    }
}
