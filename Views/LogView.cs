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
        Env environment;

        public LogView(Env env)
        {
            environment = env;
            InitializeComponent();
            logger.OnLogMessage += new Logger.OnLogMessageDelegate(logger_OnLogMessage);
            logger.OnClearLog += new Logger.OnClearLogDelegate(logger_OnClearLog);
        }

        #region Logger

        void logger_OnClearLog()
        {
            listViewLog.Items.Clear();
        }

        void logger_OnLogMessage(string message)
        {
            if (listViewLog.InvokeRequired)
            {
                OnMessageCallback d = new OnMessageCallback(logger_OnLogMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                listViewLog.BeginUpdate();
                ListViewItem item = listViewLog.Items.Add(message);
                if (message.Contains("Error"))
                    item.ForeColor = Color.Red;
                else if (message.Contains("Warning"))
                    item.ForeColor = Color.Blue;
                item.EnsureVisible();
                listViewLog.EndUpdate();
            }
        }

        private void listViewLog_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(listViewLog, e.X, e.Y);
        }
        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
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
                foreach (ListViewItem t in listViewLog.Items)
                {
                    writer.WriteLine(t.Text);
                }
                writer.Close();
            }
        }
        #endregion Logger

    }
}
