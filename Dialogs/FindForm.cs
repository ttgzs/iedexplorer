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
        int selectedRow = -1;
        Color Back;
        Color Fore;

        // FindForm can serve either for a DataGridView
        public FindForm(DataGridView dgv)
        {
            InitializeComponent();
            this.dgv = dgv;
        }

        // ... or for a ListView
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
                            dgv.Rows[row].Selected = true;
                            dgv.CurrentCell = dgv.Rows[row].Cells[0];   // make a new selection and clear others
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
                            if (selectedRow > -1)
                            {
                                lvw.Items[selectedRow].BackColor = Back;
                                lvw.Items[selectedRow].ForeColor = Fore;
                                lvw.Items[selectedRow].Selected = true;
                            }
                            lvw.Items[row].EnsureVisible();
                            // Active selection simulation when Listview does not have focus
                            Back = lvw.Items[row].BackColor;
                            Fore = lvw.Items[row].ForeColor;
                            lvw.Items[row].BackColor = SystemColors.Highlight;
                            lvw.Items[row].ForeColor = SystemColors.HighlightText;
                            selectedRow = row;
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
            {
                if (selectedRow > -1)
                {
                    lvw.SelectedItems.Clear();
                    lvw.Items[selectedRow].BackColor = Back;
                    lvw.Items[selectedRow].ForeColor = Fore;
                    lvw.Items[selectedRow].Selected = true;
                }
                this.lvw.FindForm().Focus();
                this.lvw.Focus();
            }
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
