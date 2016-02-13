using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IEDExplorer.Views;

namespace IEDExplorer.Dialogs
{
    public partial class AutoUpdateStoppedDialog : Form
    {
        IedDataView idv;

        public AutoUpdateStoppedDialog(IedDataView _idv)
        {
            idv = _idv;
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            idv.DoNotShowAUStoppedDialog = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
