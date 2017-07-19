using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace IEDExplorer.Dialogs
{
    public partial class FindForm : Form
    {
        bool firstSearch = true;
        Place startPlace;
        DataGridView dgv;
        ListView lvw;

        public FindForm(DataGridView dgv)
        {
            InitializeComponent();
            this.dgv = dgv;
        }

        public FindForm(ListView lvw)
        {
            InitializeComponent();
            this.lvw = lvw;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btFindNext_Click(object sender, EventArgs e)
        {
            FindNext(tbFind.Text);
        }

        public virtual void FindNext(string pattern)
        {
            try
            {
                RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;

                // Create a new wildcard to search for
                Regex wildcard;

                if (cbWholeWord.Checked)
                    pattern = "\\b" + pattern + "\\b";
                //
                if (firstSearch)
                {
                    startPlace = new Place(0, 0);
                    firstSearch = false;
                }
                //
                if (cbRegex.Checked)
                    wildcard = new Wildcard(pattern, opt);
                else
                    wildcard = new Regex(pattern, opt);

                if (dgv != null)
                {
                    for (int row = startPlace.iLine; row < dgv.RowCount; row++)
                    {
                        if (wildcard.IsMatch(dgv[0, row].Value.ToString()))
                        {
                            //tb.ClearSelection();
                            dgv.Rows[row].Selected = true;
                            dgv.CurrentCell = dgv.Rows[row].Cells[0];
                            startPlace.iLine = row + 1;
                            return;
                        }
                    }
                }
                else
                {
                    for (int row = startPlace.iLine; row < lvw.Items.Count; row++)
                    {
                        if (wildcard.IsMatch(lvw.Items[row].Text))
                        {
                            lvw.SelectedItems.Clear();
                            lvw.Items[row].Selected = true;
                            lvw.Items[row].EnsureVisible();
                            //lvw.FindForm().Focus();
                            //lvw.user;
                            startPlace.iLine = row + 1;
                            return;
                        }
                    }
                }
                MessageBox.Show("Not found");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btFindNext.PerformClick();
                e.Handled = true;
                return;
            }
            if (e.KeyChar == '\x1b')
            {
                Hide();
                e.Handled = true;
                return;
            }
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            if (dgv != null)
                this.dgv.Focus();
            else
                this.lvw.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnActivated(EventArgs e)
        {
            tbFind.Focus();
            ResetSearch();
        }

        void ResetSearch()
        {
            firstSearch = true;
        }

        private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            ResetSearch();
        }
    }
}
