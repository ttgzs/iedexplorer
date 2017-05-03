using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Core.Extensions;
using System.Text.RegularExpressions;

namespace IEDExplorer
{
    public partial class GooseSender : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public int gooseItems = 0;
        IList<LivePacketDevice> _netDevices;

        public GooseSender()
        {
            InitializeComponent();

            _netDevices = LivePacketDevice.AllLocalMachine;

            toolStripButton_Start.Enabled = true;
            toolStripButton_Stop.Enabled = false;


            if (_netDevices.Count != 0)
            {
                List<string> netDevNames = new List<string>();
                string description;

                for (int i = 0; i < _netDevices.Count; i++)
                {
                    LivePacketDevice device = _netDevices[i];

                    if (device.Description != null)
                    {
                        description = Regex.Match(device.Description.Replace("(Microsoft's Packet Scheduler)", ""), @"'(.+?)'").Groups[1].Value;
                        description = i.ToString("00") + " : " + description.Trim();
                    }
                    else
                        description = i.ToString("00");

                    toolStripComboBox_NedDevices.Items.Add(description);
                    netDevNames.Add(device.Name);
                }

                toolStripComboBox_NedDevices.SelectedIndex = 0;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string mac = "00:00:00:00:00:00";

            if (toolStripComboBox_NedDevices.SelectedIndex <= _netDevices.Count)
                mac = _netDevices[toolStripComboBox_NedDevices.SelectedIndex].GetMacAddress().ToString();

            GooseControl gc = new GooseControl("Goose " + gooseItems++.ToString() + ":", mac, _netDevices[toolStripComboBox_NedDevices.SelectedIndex]);
            gc.Dock = DockStyle.Top;

            if (toolStripButton_Start.Enabled == true)
                gc.StopAndLockSending();

            panel1.Controls.Add(gc);
            panel1.Controls.SetChildIndex(gc, 0);
        }

        private void toolStripButton_Start_Click(object sender, EventArgs e)
        {
            if (_netDevices.Count > 0)
            {
                toolStripButton_Stop.Enabled = true;
                toolStripButton_Start.Enabled = false;
                toolStripButton_AddGoose.Enabled = true;
                toolStripButton_Import.Enabled = true;
                toolStripButton_Export.Enabled = true;
                toolStripButton_Clear.Enabled = true;
                toolStripButton_Toggle.Enabled = true;

                if (panel1.Controls.Count > 0)
                {
                    foreach (Control ctrl in panel1.Controls)
                        if (ctrl is GooseControl)
                        {
                            (ctrl as GooseControl).netDevice = _netDevices[toolStripComboBox_NedDevices.SelectedIndex];
                            (ctrl as GooseControl).UnlockSending();
                        }
                }

                toolStripComboBox_NedDevices.Enabled = false;
            }
            else
            {
                toolStripButton_Stop.Enabled = false;
                toolStripButton_AddGoose.Enabled = false;
                toolStripButton_Import.Enabled = false;
                toolStripButton_Export.Enabled = false;
                toolStripButton_Clear.Enabled = false;
                toolStripButton_Toggle.Enabled = false;
            }
        }

        private void toolStripButton_Import_Click(object sender, EventArgs e)
        {
            ImportGooseFromXml GoosesFromXml = new ImportGooseFromXml();
            GoosesFromXml.Import(ref gooseItems, panel1.Controls,_netDevices[toolStripComboBox_NedDevices.SelectedIndex]);
        }

        private void toolStripButton_Export_Click(object sender, EventArgs e)
        {
            ExportGoosesToXml GoosesToXml = new ExportGoosesToXml();

            GoosesToXml.Export(panel1.Controls);
        }

        private void toolStripButton_Clear_Click(object sender, EventArgs e)
        {
            foreach (Control gc in panel1.Controls)
            {
                if (gc is GooseControl)
                    (gc as GooseControl).Close();
            }

            panel1.Controls.Clear();
            gooseItems = 0;
        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is GooseControl)
                    (ctrl as GooseControl).StopAndLockSending();
            }

            toolStripButton_Start.Enabled = true;
            toolStripButton_Stop.Enabled = false;
            toolStripButton_Toggle.Enabled = false;
            toolStripComboBox_NedDevices.Enabled = true;
        }

        private void GooseSender_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Control gc in panel1.Controls)
            {
                if (gc is GooseControl)
                    (gc as GooseControl).Close();
            }

            panel1.Controls.Clear();
        }

        private void toolStripButton_Toggle_Click(object sender, EventArgs e)
        {
            foreach (Control gc in panel1.Controls)
            {
                if (gc is GooseControl)
                {
                    var gooseControl = gc as GooseControl;

                    var booleanDatas = gooseControl.dataList.Where(x => x.isBooleanSelected()).ToList();

                    if (booleanDatas != null && booleanDatas.Count() > 0)
                    {
                        for(int i = 0; i < booleanDatas.Count(); i++)
                        {
                            var booleanData = booleanDatas[i];

                            booleanData.selectBoolean(!booleanData.Boolean);

                            if (gooseControl.gooseDataEdit != null && gooseControl.gooseDataEdit.Gvl != null)
                            {
                                var gvlData = gooseControl.gooseDataEdit.Gvl.GetChildNodes().Single(x => x.Tag == booleanData);
                                if (gvlData is NodeGData)
                                {
                                    if ((gvlData as NodeGData).DataType == scsm_MMS_TypeEnum.boolean)
                                        (gvlData as NodeGData).DataValue = booleanData.Boolean;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
