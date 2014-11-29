using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using org.mkulu.config;

namespace IEDExplorer.Views
{
    public partial class MainWindow : Form
    {
        public Logger logger = Logger.getLogger();
        WindowManager wm;
        Env env;
        IniFileManager ini;
        Scsm_MMS_Worker worker;

        private WeifenLuo.WinFormsUI.Docking.VS2012LightTheme vS2012LightTheme1 = new VS2012LightTheme();

        public MainWindow(Env envir)
        {
            InitializeComponent();
            dockPanel1.Theme = vS2012LightTheme1;
            env = envir;

            worker = new Scsm_MMS_Worker(env);

            wm = new WindowManager(dockPanel1, env, this);
            this.Text = "IED Explorer 0.7";

            logger.LogInfo("Starting main program ...");

            ini = new IniFileManager(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\mruip.ini");
            GetMruIp();
            if (toolStripComboBox_Hostname.Items.Count > 0)
                toolStripComboBox_Hostname.SelectedIndex = 0;
            toolStripComboBoxLoggingLevel.Items.AddRange(Enum.GetNames(typeof(Logger.Severity)));
            toolStripComboBoxLoggingLevel.SelectedItem = logger.Verbosity.ToString();
        }

        void SaveMruIp()
        {
            int i = 0;
            if (toolStripComboBox_Hostname.Text != "")
            {
                if (toolStripComboBox_Hostname.Items.Contains(toolStripComboBox_Hostname.Text))
                {
                    string s = toolStripComboBox_Hostname.Text;
                    int idx = toolStripComboBox_Hostname.Items.IndexOf(s);
                    toolStripComboBox_Hostname.Items.RemoveAt(idx);
                    toolStripComboBox_Hostname.Items.Insert(0, s);
                    toolStripComboBox_Hostname.SelectedIndex = 0;
                }
                else
                {
                    if (toolStripComboBox_Hostname.Items.Count >= 20)
                    {
                        toolStripComboBox_Hostname.Items.RemoveAt(toolStripComboBox_Hostname.Items.Count - 1);
                    }
                    toolStripComboBox_Hostname.Items.Insert(0, toolStripComboBox_Hostname.Text);
                    toolStripComboBox_Hostname.SelectedIndex = 0;
                }
            }
            foreach (string s in toolStripComboBox_Hostname.Items)
            {
                ini.writeString("MruIp", "Ip" + i++, s);
            }
        }

        void GetMruIp()
        {
            string s;
            for (int i = 0; i < 20; i++)
            {
                s = ini.getString("MruIp", "Ip" + i, "");
                if (s != "")
                    toolStripComboBox_Hostname.Items.Add(s);
            }
        }

        private void toolStripButton_Run_Click(object sender, EventArgs e)
        {
            toolStripButton_Stop.Enabled = true;
            toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.LightYellow;
            if (toolStripComboBox_Hostname.Items.Count == 0)
            {
                toolStripComboBox_Hostname.Items.Add("localhost");
            }
            toolStripButton_Run.Enabled = false;
            SaveMruIp();
            worker.Start(toolStripComboBox_Hostname.Text, 102); //.SelectedItem.ToString(), 102);
        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            worker.Stop();
            toolStripButton_Stop.Enabled = false;
            toolStripButton_Run.Enabled = true;
        }

        private void toolStripComboBox_Hostname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox_Hostname.SelectedItem != null)
                toolStripComboBox_Hostname.SelectedItem.ToString();
        }

        private void toolStripComboBoxLoggingLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            logger.Verbosity = (Logger.Severity)Enum.Parse(typeof(Logger.Severity), ((ToolStripComboBox)sender).Text);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null)
                worker.Stop();
        }

    }
}
