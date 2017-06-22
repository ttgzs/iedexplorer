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
using IEDExplorer.Dialogs;
using System.Collections.Specialized;

namespace IEDExplorer.Views
{
    public partial class MainWindow : Form
    {
        public Logger logger = Logger.getLogger();
        WindowManager wm;
        Env _env;
        IniFileManager ini;
        IniFileManager ieds;
        Dictionary<string, IsoConnectionParameters> iedsDb = new Dictionary<string, IsoConnectionParameters>();

        Scsm_MMS_Worker worker;

        const int maxHistory = 20;
        IsoConnectionParameters isoPar;

        //SCLServer sclServer = null;

        private WeifenLuo.WinFormsUI.Docking.VS2012LightTheme vS2012LightTheme1 = new VS2012LightTheme();
        private Iec61850State iecf;

        public void Set_iecf (Iec61850State _iecf)
        {
            iecf = _iecf;
        }

        public Iec61850State Get_iecf()
        {
            return iecf;
        }

        public MainWindow ()
        {
            InitializeComponent();
            dockPanel1.Theme = vS2012LightTheme1;
            _env = Env.getEnv();
            _env.logger = new Logger();

            worker = new Scsm_MMS_Worker();

            wm = new WindowManager(dockPanel1, this);
            this.Text = "IED Explorer 0.79b Exp SCL Server & GOOSE";

            logger.LogInfo("Starting main program ...");

            ini = new IniFileManager(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\mruip.ini");
            ieds = new IniFileManager(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\ieds.ini");
            GetMruIp();
            GetMruFiles();
            GetIedsDb();
            if (toolStripComboBox_Hostname.Items.Count > 0)
                toolStripComboBox_Hostname.SelectedIndex = 0;
            toolStripComboBoxLoggingLevel.Items.AddRange(Enum.GetNames(typeof(Logger.Severity)));
            toolStripComboBoxLoggingLevel.SelectedItem = logger.Verbosity.ToString();
            toolStripButtonOpenSCL.DropDownItemClicked += new ToolStripItemClickedEventHandler(toolStripButtonOpenSCL_DropDownItemClicked);
        }

        private void GetIedsDb()
        {
            foreach (string section in ieds.getSections())
            {
                iedsDb.Add(section, new IsoConnectionParameters(ieds.getSection(section)));
            }
        }

        void AddAndSaveMruIp()
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
                    if (toolStripComboBox_Hostname.Items.Count >= maxHistory)
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
            i = 0;
        }

        void AddAndSaveMruFiles(string filename)
        {
            bool existing = false;
            int index = 0;
            foreach (ToolStripMenuItem tsmi in toolStripButtonOpenSCL.DropDownItems)
            {
                if (tsmi.Text == filename)
                {
                    existing = true;
                    break;
                }
                index++;
            }

            int i = 0;
            if (existing)
            {
                toolStripButtonOpenSCL.DropDownItems.RemoveAt(index);
                toolStripButtonOpenSCL.DropDownItems.Insert(0, new ToolStripMenuItem(filename));
            }
            else
            {
                if (toolStripButtonOpenSCL.DropDownItems.Count >= maxHistory)
                {
                    toolStripButtonOpenSCL.DropDownItems.RemoveAt(toolStripButtonOpenSCL.DropDownItems.Count - 1);
                }
                toolStripButtonOpenSCL.DropDownItems.Insert(0, new ToolStripMenuItem(filename));
            }
            foreach (ToolStripMenuItem tsmi in toolStripButtonOpenSCL.DropDownItems)
            {
                ini.writeString("MruFiles", "File" + i++, tsmi.Text);
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

        void GetMruFiles()
        {
            string s;
            for (int i = 0; i < 20; i++)
            {
                s = ini.getString("MruFiles", "File" + i, "");
                if (s != "")
                    toolStripButtonOpenSCL.DropDownItems.Add(s);
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
            AddAndSaveMruIp();
            try { isoPar = iedsDb[toolStripComboBox_Hostname.Text]; }  // read parameters of the current ied
            catch { isoPar = null; }
            if (isoPar == null)
            {
                isoPar = new IsoConnectionParameters((IsoAcse.AcseAuthenticationParameter)null);
                isoPar.hostname = toolStripComboBox_Hostname.Text;
            }
            worker.Start(isoPar);
        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            logger.LogInfo("Communication stopped by user");
            worker.Stop();
            toolStripButton_Stop.Enabled = false;
            toolStripButton_Run.Enabled = true;
        }

        private void toolStripComboBox_Hostname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox_Hostname.SelectedItem != null)
                toolStripComboBox_Hostname.SelectedItem.ToString();
            //isoPar = null;
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

        private void toolStripButtonOpenSCL_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a SCL File";
            ofd.AddExtension = true;
            ofd.AutoUpgradeEnabled = true;
            ofd.Filter =
                "SCD Files(*.icd, *.ssd, *.scd, *.cid, *.iid, *.sed)|*.icd;*.ssd;*.scd;*.cid;*.iid;*.sed";
            ofd.ReadOnlyChecked = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            DialogResult res = ofd.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                AddAndSaveMruFiles(ofd.FileName);
                wm.AddSCLView(ofd.FileName);
            }
        }

        void toolStripButtonOpenSCL_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            wm.AddSCLView(e.ClickedItem.Text);
        }

        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            AboutDialog ad = new AboutDialog();
            ad.ShowDialog();
            //ad.Close();
        }

        private void toolStripButtonConnParam_Click(object sender, EventArgs e)
        {
            try { isoPar = iedsDb[toolStripComboBox_Hostname.Text]; }
            catch { isoPar = null; }   // read parameters of the current ied
            if (isoPar == null)
            {
                isoPar = new IsoConnectionParameters((IsoAcse.AcseAuthenticationParameter)null);
                isoPar.hostname = toolStripComboBox_Hostname.Text;
            }

            ConnParamDialog cd = new ConnParamDialog(isoPar, iedsDb);
            DialogResult res = cd.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                StringDictionary std = new StringDictionary();
                isoPar.Save(std);   // Write parameters to dictionary
                foreach (string key in std.Keys)
                {
                    ieds.writeString(std["hostname"], key, std[key]);
                }
                iedsDb[std["hostname"]] = isoPar;
            }
        }

        private void GooseSender_Click (object sender, EventArgs e)
        {
            try
            {
                /*GooseSender gooseSender = new GooseSender();
                gooseSender.Show();*/
                wm.AddGooseSender();
            }
            catch
            {
                logger.LogError("Problem to initialize PCap !!!");//, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void GooseExplorer_Click (object sender, EventArgs e)
        {
            /*GooseExplorer goose = new GooseExplorer(this.iecf, env.logger);
            goose.Show();*/
            try
            {
                wm.AddGooseExplorer(this.iecf, logger);
            }
            catch
            {
                logger.LogError("Problem to initialize PCap !!!");//, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void toolStripButtonStartupRead_Click(object sender, EventArgs e)
        {
            _env.dataReadOnStartup = toolStripButtonStartupRead.Checked;
            logger.LogInfo("Read Data Values from IED on startup (time consuming, but useful) is set to: " + _env.dataReadOnStartup.ToString());
        }
    }
}
