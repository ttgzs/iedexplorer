/*
 *  Copyright (C) 2013 Pavel Charvat
 * 
 *  This file is part of IEDExplorer.
 *
 *  IEDExplorer is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IEDExplorer is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IEDExplorer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer
{
    partial class RcbActivateDialog : Form
    {
        public RcbActivateParams RCBpar;

        public RcbActivateDialog(RcbActivateParams rcbpar)
        {
            RCBpar = rcbpar;
            InitializeComponent();

            labelReportName.Text = RCBpar.self.Address;
            labelReportType.Text = RCBpar.self.isBuffered ? "Buffered" : "Unbuffered";

            if (RCBpar.self.RptID_present)
            {
                textBoxRptID.Text = RCBpar.self.RptID;
                checkBoxRptID_send.Checked = RCBpar.sendRptID;
                if (!checkBoxRptID_send.Checked)
                    textBoxRptID.Enabled = false;
            }
            else
            {
                textBoxRptID.Enabled = false;
                checkBoxRptID_send.Enabled = false;
            }
            if (RCBpar.self.DatSet_present)
            {
                try
                {
                    string myld = RCBpar.self.Parent.Name;
                    NodeBase[] nba = RCBpar.self.GetIecs().DataModel.lists.FindChildNode(myld).GetChildNodes();
                    foreach (NodeBase nb in nba)
                    {
                        comboBoxDatSet.Items.Add(nb.Address);
                    }
                    comboBoxDatSet.SelectedItem = RCBpar.self.DatSet;
                }
                catch { }
                checkBoxDatSet_send.Checked = RCBpar.sendDatSet;
                if (!checkBoxDatSet_send.Checked)
                    comboBoxDatSet.Enabled = false;
            }
            else
            {
                comboBoxDatSet.Enabled = false;
                checkBoxDatSet_send.Enabled = false;
            }
            if (RCBpar.self.OptFlds_present)
            {
                checkBoxOptFlds_SeqNum.Checked = (RCBpar.self.OptFlds & ReportOptions.SEQ_NUM) > 0;
                checkBoxOptFlds_TimeStamp.Checked = (RCBpar.self.OptFlds & ReportOptions.TIME_STAMP) > 0;
                checkBoxOptFlds_ReasonForInclusion.Checked = (RCBpar.self.OptFlds & ReportOptions.REASON_FOR_INCLUSION) > 0;
                checkBoxOptFlds_DataSet.Checked = (RCBpar.self.OptFlds & ReportOptions.DATA_SET) > 0;
                checkBoxOptFlds_DataReference.Checked = (RCBpar.self.OptFlds & ReportOptions.DATA_REFERENCE) > 0;
                checkBoxOptFlds_BufferOverflow.Checked = (RCBpar.self.OptFlds & ReportOptions.BUFFER_OVERFLOW) > 0;
                checkBoxOptFlds_EntryID.Checked = (RCBpar.self.OptFlds & ReportOptions.ENTRY_ID) > 0;
                checkBoxOptFlds_ConfRev.Checked = (RCBpar.self.OptFlds & ReportOptions.CONF_REV) > 0;

                checkBoxOptFlds_send.Checked = RCBpar.sendOptFlds;
                if (!checkBoxOptFlds_send.Checked)
                {
                    enableOptFlds(false);
                }
            }
            else
            {
                enableOptFlds(false);
                checkBoxOptFlds_send.Enabled = false;
            }
            if (RCBpar.self.BufTm_present)
            {
                textBoxBufTm.Text = RCBpar.self.BufTm.ToString();
                checkBoxBufTm_send.Checked = RCBpar.sendBufTm;
                if (!checkBoxBufTm_send.Checked)
                    textBoxBufTm.Enabled = false;
            }
            else
            {
                textBoxBufTm.Enabled = false;
                checkBoxBufTm_send.Enabled = false;
            }
            if (RCBpar.self.TrgOps_present)
            {
                checkBoxTrgOps_DataChange.Checked = (RCBpar.self.TrgOps & TriggerOptions.DATA_CHANGED) > 0;
                checkBoxTrgOps_QualityChange.Checked = (RCBpar.self.TrgOps & TriggerOptions.QUALITY_CHANGED) > 0;
                checkBoxTrgOps_DataUpdate.Checked = (RCBpar.self.TrgOps & TriggerOptions.DATA_UPDATE) > 0;
                checkBoxTrgOps_Integrity.Checked = (RCBpar.self.TrgOps & TriggerOptions.INTEGRITY) > 0;
                checkBoxTrgOps_GI.Checked = (RCBpar.self.TrgOps & TriggerOptions.GI) > 0;

                checkBoxTrgOps_send.Checked = RCBpar.sendTrgOps;
                if (!checkBoxTrgOps_send.Checked)
                {
                    enableTrgOps(false);
                }
            }
            else
            {
                enableTrgOps(false);
                checkBoxTrgOps_send.Enabled = false;
            }
            if (RCBpar.self.IntgPd_present)
            {
                textBoxIntgPd.Text = RCBpar.self.IntgPd.ToString();
                checkBoxIntgPd_send.Checked = RCBpar.sendIntgPd;
                if (!checkBoxIntgPd_send.Checked)
                    textBoxIntgPd.Enabled = false;
            }
            else
            {
                textBoxIntgPd.Enabled = false;
                checkBoxIntgPd_send.Enabled = false;
            }
            if (RCBpar.self.RptEna_present)
            {
                checkBoxRptEna.Text = RCBpar.self.RptEna.ToString();
                checkBoxRptEna_send.Checked = RCBpar.sendRptEna;
                if (!checkBoxRptEna_send.Checked)
                    checkBoxRptEna.Enabled = false;
            }
            else
            {
                checkBoxRptEna.Enabled = false;
                checkBoxRptEna_send.Enabled = false;
            }
            if (RCBpar.self.GI_present)
            {
                checkBoxGI.Text = RCBpar.self.GI.ToString();
                checkBoxGI_send.Checked = RCBpar.sendGI;
                if (!checkBoxGI_send.Checked)
                    checkBoxGI.Enabled = false;
            }
            else
            {
                checkBoxGI.Enabled = false;
                checkBoxGI_send.Enabled = false;
            }
            if (RCBpar.self.Resv_present)
            {
                checkBoxResv.Text = RCBpar.self.Resv.ToString();
                checkBoxResv_send.Checked = RCBpar.sendResv;
                if (!checkBoxResv_send.Checked)
                    checkBoxResv.Enabled = false;
            }
            else
            {
                checkBoxResv.Enabled = false;
                checkBoxResv_send.Enabled = false;
            }
            if (RCBpar.self.PurgeBuf_present)
            {
                checkBoxPurgeBuf.Text = RCBpar.self.PurgeBuf.ToString();
                checkBoxPurgeBuf_send.Checked = RCBpar.sendPurgeBuf;
                if (!checkBoxPurgeBuf_send.Checked)
                    checkBoxPurgeBuf.Enabled = false;
            }
            else
            {
                checkBoxPurgeBuf.Enabled = false;
                checkBoxPurgeBuf_send.Enabled = false;
            }
            if (RCBpar.self.EntryID_present)
            {
                textBoxEntryID.Text = RCBpar.self.EntryID;
                checkBoxEntryID_send.Checked = RCBpar.sendEntryID;
                if (!checkBoxEntryID_send.Checked)
                    textBoxEntryID.Enabled = false;
            }
            else
            {
                textBoxEntryID.Enabled = false;
                checkBoxEntryID_send.Enabled = false;
            }
            if (RCBpar.self.ResvTms_present)
            {
                textBoxResvTms.Text = RCBpar.self.ResvTms.ToString();
                checkBoxResvTms_send.Checked = RCBpar.sendResvTms;
                if (!checkBoxResvTms_send.Checked)
                    textBoxResvTms.Enabled = false;
            }
            else
            {
                textBoxResvTms.Enabled = false;
                checkBoxResvTms_send.Enabled = false;
            }
        }

        private void enableOptFlds(bool enable)
        {
            checkBoxOptFlds_SeqNum.Enabled = enable;
            checkBoxOptFlds_TimeStamp.Enabled = enable;
            checkBoxOptFlds_ReasonForInclusion.Enabled = enable;
            checkBoxOptFlds_DataSet.Enabled = enable;
            checkBoxOptFlds_DataReference.Enabled = enable;
            checkBoxOptFlds_BufferOverflow.Enabled = enable;
            checkBoxOptFlds_EntryID.Enabled = enable;
            checkBoxOptFlds_ConfRev.Enabled = enable;
        }

        private void enableTrgOps(bool enable)
        {
            checkBoxTrgOps_DataChange.Enabled = enable;
            checkBoxTrgOps_QualityChange.Enabled = enable;
            checkBoxTrgOps_DataUpdate.Enabled = enable;
            checkBoxTrgOps_Integrity.Enabled = enable;
            checkBoxTrgOps_GI.Enabled = enable;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // test
            //RCBpar.self.GI = false;
            //RCBpar.self.RptEna = true;
            RCBpar.sendRptID = checkBoxRptID_send.Checked;
            if (RCBpar.sendRptID)
                RCBpar.self.RptID = textBoxRptID.Text;
            RCBpar.sendDatSet = checkBoxDatSet_send.Checked;
            if (RCBpar.sendDatSet)
                RCBpar.self.DatSet = comboBoxDatSet.Text;
            RCBpar.sendOptFlds = checkBoxOptFlds_send.Checked;
            if (RCBpar.sendOptFlds)
            {
                ReportOptions ro = ReportOptions.NONE;
                if (checkBoxOptFlds_SeqNum.Checked) ro |= ReportOptions.SEQ_NUM;
                if (checkBoxOptFlds_TimeStamp.Checked) ro |= ReportOptions.TIME_STAMP;
                if (checkBoxOptFlds_ReasonForInclusion.Checked) ro |= ReportOptions.REASON_FOR_INCLUSION;
                if (checkBoxOptFlds_DataSet.Checked) ro |= ReportOptions.DATA_SET;
                if (checkBoxOptFlds_DataReference.Checked) ro |= ReportOptions.DATA_REFERENCE;
                if (checkBoxOptFlds_BufferOverflow.Checked) ro |= ReportOptions.BUFFER_OVERFLOW;
                if (checkBoxOptFlds_EntryID.Checked) ro |= ReportOptions.ENTRY_ID;
                if (checkBoxOptFlds_ConfRev.Checked) ro |= ReportOptions.CONF_REV;
                RCBpar.self.OptFlds = ro;
            }
            RCBpar.sendBufTm = checkBoxBufTm_send.Checked;
            if (RCBpar.sendBufTm)
                try
                {
                    RCBpar.self.BufTm = uint.Parse(textBoxBufTm.Text);
                }
                catch { }
            RCBpar.sendTrgOps = checkBoxTrgOps_send.Checked;
            if (RCBpar.sendTrgOps)
            {
                TriggerOptions to = TriggerOptions.NONE;
                if (checkBoxTrgOps_DataChange.Checked) to |= TriggerOptions.DATA_CHANGED;
                if (checkBoxTrgOps_QualityChange.Checked) to |= TriggerOptions.QUALITY_CHANGED;
                if (checkBoxTrgOps_DataUpdate.Checked) to |= TriggerOptions.DATA_UPDATE;
                if (checkBoxTrgOps_Integrity.Checked) to |= TriggerOptions.INTEGRITY;
                if (checkBoxTrgOps_GI.Checked) to |= TriggerOptions.GI;
                RCBpar.self.TrgOps = to;
            }
            RCBpar.sendIntgPd = checkBoxIntgPd_send.Checked;
            if (RCBpar.sendIntgPd)
                try
                {
                    RCBpar.self.IntgPd = uint.Parse(textBoxIntgPd.Text);
                }
                catch { }
            RCBpar.sendRptEna = checkBoxRptEna_send.Checked;
            if (RCBpar.sendRptEna)
                RCBpar.self.RptEna = checkBoxRptEna.Checked;
            RCBpar.sendGI = checkBoxGI_send.Checked;
            if (RCBpar.sendGI)
                RCBpar.self.GI = checkBoxGI.Checked;
            RCBpar.sendResv = checkBoxResv_send.Checked;
            if (RCBpar.sendResv)
                RCBpar.self.Resv = checkBoxResv.Checked;
            RCBpar.sendPurgeBuf = checkBoxPurgeBuf_send.Checked;
            if (RCBpar.sendPurgeBuf)
                RCBpar.self.PurgeBuf = checkBoxPurgeBuf.Checked;
            RCBpar.sendEntryID = checkBoxEntryID_send.Checked;
            if (RCBpar.sendEntryID)
                RCBpar.self.EntryID = textBoxEntryID.Text;
            RCBpar.sendResvTms = checkBoxResvTms_send.Checked;
            if (RCBpar.sendResvTms)
                try
                {
                    RCBpar.self.ResvTms = uint.Parse(textBoxResvTms.Text);
                }
                catch { }

        }

        private void checkBoxRptID_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxRptID_send.Checked)
                textBoxRptID.Enabled = false;
            else
                textBoxRptID.Enabled = true;
        }

        private void checkBoxDatSet_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxDatSet_send.Checked)
                comboBoxDatSet.Enabled = false;
            else
                comboBoxDatSet.Enabled = true;
        }

        private void checkBoxOptFlds_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxOptFlds_send.Checked)
                enableOptFlds(false);
            else
                enableOptFlds(true);
        }

        private void checkBoxBufTm_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxBufTm_send.Checked)
                textBoxBufTm.Enabled = false;
            else
                textBoxBufTm.Enabled = true;
        }

        private void checkBoxTrgOps_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxTrgOps_send.Checked)
                enableTrgOps(false);
            else
                enableTrgOps(true);
        }

        private void checkBoxIntgPd_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxIntgPd_send.Checked)
                textBoxIntgPd.Enabled = false;
            else
                textBoxIntgPd.Enabled = true;
        }

        private void checkBoxRptEna_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxRptEna_send.Checked)
                checkBoxRptEna.Enabled = false;
            else
                checkBoxRptEna.Enabled = true;
        }

        private void checkBoxGI_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxGI_send.Checked)
                checkBoxGI.Enabled = false;
            else
                checkBoxGI.Enabled = true;
        }

        private void checkBoxResv_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxResv_send.Checked)
                checkBoxResv.Enabled = false;
            else
                checkBoxResv.Enabled = true;
        }

        private void checkBoxPurgeBuf_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxPurgeBuf_send.Checked)
                checkBoxPurgeBuf.Enabled = false;
            else
                checkBoxPurgeBuf.Enabled = true;
        }

        private void checkBoxEntryID_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxEntryID_send.Checked)
                textBoxEntryID.Enabled = false;
            else
                textBoxEntryID.Enabled = true;
        }

        private void checkBoxResvTms_send_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxResvTms_send.Checked)
                textBoxResvTms.Enabled = false;
            else
                textBoxResvTms.Enabled = true;
        }

    }
}
