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
                comboBoxDatSet.Text = RCBpar.self.DatSet;
                try
                {
                    string myld = RCBpar.self.Parent.Name;
                    NodeBase[] nba = RCBpar.self.GetIecs().DataModel.lists.FindChildNode(myld).GetChildNodes();
                    foreach (NodeBase nb in nba)
                    {
                        comboBoxDatSet.Items.Add(nb.Name);
                    }
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
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // test
            RCBpar.self.GI = false;
            RCBpar.self.RptEna = true;
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

    }
}
