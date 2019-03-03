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
    public partial class EditValue : Form
    {
        NodeBase tNode = null;
        NodeData data;

        EditValue()
        {
            InitializeComponent();
        }

        internal EditValue(NodeData n)
        {
            InitializeComponent();
            chbSetZeroEntryID.Checked = false;
            chbSetZeroEntryID.Enabled = true;
            data = n;
            this.textBox1.Text = n.StringValue;
            NodeBase nb = data;
            bool tFound = false;
            if (data.Name == "t")
            {
                checkBoxTimestamp.Checked = false;
                checkBoxTimestamp.Enabled = false;
            }
            else
            {
                while (nb.Parent != null && (nb.Parent is NodeData || nb.Parent is NodeDO))
                {
                    nb = nb.Parent;
                    tNode = nb.FindChildNode("t");
                    if (tNode != null && (tNode is NodeData && !(tNode is NodeDO)))
                    {
                        tFound = true;
                        break;
                    }
                }
                if (!tFound)
                {
                    checkBoxTimestamp.Checked = false;
                    checkBoxTimestamp.Enabled = false;
                }

            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(chbSetZeroEntryID.Checked == true)
                data.StringValue = "\0\0\0\0\0\0\0\0";
            else
                data.StringValue = this.textBox1.Text;
        }

        internal bool UpdateTimestamp { get { return checkBoxTimestamp.Checked; } }

        internal NodeData TimeNode { get { if (tNode is NodeData && !(tNode is NodeDO)) return (NodeData)tNode; return null; } }

        internal void HideUpdateTimestamp()
        {
            checkBoxTimestamp.Hide();
        }
    }
}
