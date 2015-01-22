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
        Dictionary<string, IsoConnectionParameters> iedsDb;

        public ConnParamDialog(IsoConnectionParameters param, Dictionary<string, IsoConnectionParameters> ieds)
        {
            InitializeComponent();
            iedsDb = ieds;
            par = param;

            comboBoxIED.Items.Add("");
            foreach (string s in iedsDb.Keys)
            {
                comboBoxIED.Items.Add(s);
            }
            if (iedsDb.ContainsKey(par.hostname))
            {
                comboBoxIED.SelectedItem = par.hostname;
            }
            initValues();
        }

        private void initValues()
        {
            this.textBoxIP.Text = par.hostname;
            this.textBoxPort.Text = par.port.ToString();
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

            if (par.acseAuthParameter != null)
            {
                this.checkBoxAuth.Checked = true;
                if (par.acseAuthParameter.mechanism == IsoAcse.AcseAuthenticationMechanism.ACSE_AUTH_NONE)
                {
                    this.radioButtonPassword.Checked = false;
                    this.radioButtonNone.Checked = true;
                    this.textBoxPassword.Enabled = false;
                }
                else
                {
                    this.radioButtonPassword.Checked = true;
                    this.radioButtonNone.Checked = false;
                    this.textBoxPassword.Enabled = true;
                    this.textBoxPassword.Text = par.acseAuthParameter.password;
                }
            }
            else
            {
                this.checkBoxAuth.Checked = false;
                this.radioButtonPassword.Enabled = false;
                this.radioButtonNone.Enabled = false;

                this.textBoxPassword.Enabled = false;
            }
        }

        private void checkBoxAuth_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                this.radioButtonPassword.Enabled = true;
                this.radioButtonNone.Enabled = true;
                if (this.radioButtonPassword.Checked) this.textBoxPassword.Enabled = true;
            }
            else
            {
                this.radioButtonPassword.Enabled = false;
                this.radioButtonNone.Enabled = false;
                this.textBoxPassword.Enabled = false;
            }
        }

        private void radioButtonPassword_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                this.textBoxPassword.Enabled = true;
            }
        }

        private void radioButtonNone_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                this.textBoxPassword.Enabled = false;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                par.hostname = this.textBoxIP.Text;
                par.port = int.Parse(this.textBoxPort.Text);
                par.localApTitleS = this.textBoxLocalAPID.Text;
                par.localAEQualifier = int.Parse(this.textBoxLocalAEQ.Text);
                par.localPSelector = uint.Parse(this.textBoxLocalPSel.Text);
                par.localSSelector = ushort.Parse(this.textBoxLocalSSel.Text);
                par.localTSelector.value = int.Parse(this.textBoxLocalTSel.Text);
                par.localTSelector.size = (byte)(this.comboBoxLocalTSel.SelectedIndex + 1);

                par.remoteApTitleS = this.textBoxRemoteAPID.Text;
                par.remoteAEQualifier = int.Parse(this.textBoxRemoteAEQ.Text);
                par.remotePSelector = uint.Parse(this.textBoxRemotePSel.Text);
                par.remoteSSelector = ushort.Parse(this.textBoxRemoteSSel.Text);
                par.remoteTSelector.value = int.Parse(this.textBoxRemoteTSel.Text);
                par.remoteTSelector.size = (byte)(this.comboBoxRemoteTSel.SelectedIndex + 1);

                if (this.checkBoxAuth.Checked)
                {
                    par.acseAuthParameter = new IsoAcse.AcseAuthenticationParameter();
                    if (this.radioButtonPassword.Checked)
                    {
                        par.acseAuthParameter.mechanism = IsoAcse.AcseAuthenticationMechanism.ACSE_AUTH_PASSWORD;
                        par.acseAuthParameter.password = textBoxPassword.Text;

                        par.acseAuthParameter.paswordOctetString = Encoding.ASCII.GetBytes(par.acseAuthParameter.password);
                        par.acseAuthParameter.passwordLength = par.acseAuthParameter.paswordOctetString.Length;
                    }
                    else
                        par.acseAuthParameter.mechanism = IsoAcse.AcseAuthenticationMechanism.ACSE_AUTH_NONE;
                }
                else
                    par.acseAuthParameter = null;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Format error: " + ex.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }

        }

        private void comboBoxIED_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (iedsDb.ContainsKey((string)comboBoxIED.SelectedItem))
            {
                par = iedsDb[(string)comboBoxIED.SelectedItem];
                initValues();
            }
        }

    }
}
