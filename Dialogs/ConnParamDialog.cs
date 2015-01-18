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
    partial class ConnParamDialog : Form
    {
        IsoConnectionParameters par;

        public ConnParamDialog(IsoConnectionParameters param)
        {
            InitializeComponent();

            par = param;

            this.textBoxLocalAPID.Text = par.localApTitleS;
            this.textBoxLocalAEQ.Text = par.localAEQualifier.ToString();
            this.textBoxLocalPSel.Text = par.localPSelector.ToString();
            this.textBoxLocalSSel.Text = par.localSSelector.ToString();
            this.textBoxLocalTSel.Text = par.localTSelector.value.ToString();
            this.comboBoxLocalTSel.SelectedIndex = par.localTSelector.size - 1;

            this.textBoxRemoteAPID.Text = par.remoteApTitleS;
            this.textBoxRemoteAEQ.Text = par.remoteAEQualifier.ToString();
            this.textBoxRemotePSel.Text = par.remotePSelector.ToString();
            this.textBoxRemoteSSel.Text = par.remoteSSelector.ToString();
            this.textBoxRemoteTSel.Text = par.remoteTSelector.value.ToString();
            this.comboBoxRemoteTSel.SelectedIndex = par.remoteTSelector.size - 1;
        }

    }
}
