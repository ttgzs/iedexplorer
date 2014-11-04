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
    public partial class CommandDialog : Form
    {
        public CommandParams Cpar;

        public CommandDialog(CommandParams cpar)
        {
            Cpar = cpar;
            InitializeComponent();
            labelFlow.Text = Cpar.CommandFlowFlag.ToString().Replace('_',' ');
            labelAddr.Text = Cpar.Address;
            checkBoxInterlockCheck.Checked = Cpar.interlockCheck;
            checkBoxSynchroCheck.Checked = Cpar.synchroCheck;
            checkBoxTest.Checked = Cpar.Test;
            //.Value = (decimal)Cpar.orCat;
            comboBoxCat.Items.AddRange(Enum.GetNames(typeof(OrCat)));
            comboBoxCat.SelectedIndex = (int)Cpar.orCat;
            textBoxIdent.Text = Cpar.orIdent;
            comboBoxValue.Items.Clear();
            if (Cpar.T != DateTime.MinValue)
                dateTimePickerT.Value = Cpar.T;
            switch (Cpar.DataType)
            {
                case scsm_MMS_TypeEnum.boolean:
                    comboBoxValue.Items.Add("OFF / False");
                    comboBoxValue.Items.Add("ON  / True");
                    comboBoxValue.SelectedIndex = (bool)Cpar.ctlVal ? 0 : 1;
                    break;
                default:
                    break;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Cpar.interlockCheck = checkBoxInterlockCheck.Checked;
            Cpar.synchroCheck = checkBoxSynchroCheck.Checked;
            Cpar.Test = checkBoxTest.Checked;
            Cpar.orCat = (OrCat)comboBoxCat.SelectedIndex;
            Cpar.orIdent = textBoxIdent.Text;
            switch (Cpar.DataType)
            {
                case scsm_MMS_TypeEnum.boolean:
                    Cpar.ctlVal = comboBoxValue.SelectedIndex > 0 ? true : false;
                    break;
                default:
                    break;
            }
            if (dateTimePickerT.Enabled)
                Cpar.T = dateTimePickerT.Value;
        }

        private void checkBoxTActive_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTActive.Checked)
            {
                dateTimePickerT.Enabled = true;
                dateTimePickerT.Value = DateTime.UtcNow;
            }
            else
            {
                dateTimePickerT.Enabled = false;
                Cpar.T = DateTime.MinValue;
            }
        }
    }
}
