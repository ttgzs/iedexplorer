using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer.Dialogs
{
    public partial class SplitStringDialog : Form
    {
        string InputString;

        public SplitStringDialog(string _InputString, string Hint, string Desc1, string Desc2)
        {
            InitializeComponent();

            InputString = _InputString;
            labelInput.Text = InputString;
            labelHint.Text = Hint;
            labelPart1desc.Text = Desc1;
            labelPart2desc.Text = Desc2;
            labelPart1.Text = InputString.Substring(0, 1);
            labelPart2.Text = InputString.Substring(1);

            trackBarDivider.Width = labelInput.Width;

            trackBarDivider.Maximum = InputString.Length - 1;
            trackBarDivider.Value = 1;
        }

        private void trackBarDivider_ValueChanged(object sender, EventArgs e)
        {
            labelPart1.Text = InputString.Substring(0, trackBarDivider.Value);
            labelPart2.Text = InputString.Substring(trackBarDivider.Value);
        }

        public string Part1 { get { return labelPart1.Text; } }

        public string Part2 { get { return labelPart2.Text; } }

    }
}
