namespace IEDExplorer.Dialogs
{
    partial class ConnParamDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnParamDialog));
            this.comboBoxIED = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxRemoteTSel = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.radioButtonPassword = new System.Windows.Forms.RadioButton();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.checkBoxAuth = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxLocalTSel = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxLocalSSel = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxLocalPSel = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxLocalAEQ = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxLocalAPID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxRemoteTSel = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxRemoteSSel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxRemotePSel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxRemoteAEQ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxRemoteAPID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxLocalTSel = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxIED
            // 
            this.comboBoxIED.FormattingEnabled = true;
            this.comboBoxIED.Location = new System.Drawing.Point(129, 12);
            this.comboBoxIED.Name = "comboBoxIED";
            this.comboBoxIED.Size = new System.Drawing.Size(307, 21);
            this.comboBoxIED.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected IED";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxLocalTSel);
            this.groupBox1.Controls.Add(this.comboBoxRemoteTSel);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.radioButtonPassword);
            this.groupBox1.Controls.Add(this.radioButtonNone);
            this.groupBox1.Controls.Add(this.checkBoxAuth);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBoxLocalTSel);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBoxLocalSSel);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBoxLocalPSel);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBoxLocalAEQ);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.textBoxLocalAPID);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBoxRemoteTSel);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxRemoteSSel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxRemotePSel);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxRemoteAEQ);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxRemoteAPID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxIP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 435);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IED";
            // 
            // comboBoxRemoteTSel
            // 
            this.comboBoxRemoteTSel.FormattingEnabled = true;
            this.comboBoxRemoteTSel.Items.AddRange(new object[] {
            "1 octet",
            "2 octets",
            "3 octets",
            "4 octets"});
            this.comboBoxRemoteTSel.Location = new System.Drawing.Point(362, 206);
            this.comboBoxRemoteTSel.Name = "comboBoxRemoteTSel";
            this.comboBoxRemoteTSel.Size = new System.Drawing.Size(62, 21);
            this.comboBoxRemoteTSel.TabIndex = 34;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 388);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(113, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "Authentication method";
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(44, 412);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 13);
            this.label15.TabIndex = 32;
            this.label15.Text = "Auth Password";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(136, 409);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(288, 20);
            this.textBoxPassword.TabIndex = 31;
            // 
            // radioButtonPassword
            // 
            this.radioButtonPassword.AutoSize = true;
            this.radioButtonPassword.Location = new System.Drawing.Point(136, 386);
            this.radioButtonPassword.Name = "radioButtonPassword";
            this.radioButtonPassword.Size = new System.Drawing.Size(88, 17);
            this.radioButtonPassword.TabIndex = 30;
            this.radioButtonPassword.TabStop = true;
            this.radioButtonPassword.Text = "PASSWORD";
            this.radioButtonPassword.UseVisualStyleBackColor = true;
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Location = new System.Drawing.Point(230, 386);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(56, 17);
            this.radioButtonNone.TabIndex = 29;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "NONE";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            // 
            // checkBoxAuth
            // 
            this.checkBoxAuth.AutoSize = true;
            this.checkBoxAuth.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxAuth.Location = new System.Drawing.Point(14, 364);
            this.checkBoxAuth.Name = "checkBoxAuth";
            this.checkBoxAuth.Size = new System.Drawing.Size(136, 17);
            this.checkBoxAuth.TabIndex = 28;
            this.checkBoxAuth.Text = "Authentication Enabled";
            this.checkBoxAuth.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(37, 340);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Local T Selector";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocalTSel
            // 
            this.textBoxLocalTSel.Location = new System.Drawing.Point(136, 337);
            this.textBoxLocalTSel.Name = "textBoxLocalTSel";
            this.textBoxLocalTSel.Size = new System.Drawing.Size(220, 20);
            this.textBoxLocalTSel.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 314);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Local S Selector";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocalSSel
            // 
            this.textBoxLocalSSel.Location = new System.Drawing.Point(136, 311);
            this.textBoxLocalSSel.Name = "textBoxLocalSSel";
            this.textBoxLocalSSel.Size = new System.Drawing.Size(288, 20);
            this.textBoxLocalSSel.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(37, 288);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "Local P Selector";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocalPSel
            // 
            this.textBoxLocalPSel.Location = new System.Drawing.Point(136, 285);
            this.textBoxLocalPSel.Name = "textBoxLocalPSel";
            this.textBoxLocalPSel.Size = new System.Drawing.Size(288, 20);
            this.textBoxLocalPSel.TabIndex = 22;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(31, 262);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "Local AE Qualifier";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocalAEQ
            // 
            this.textBoxLocalAEQ.Location = new System.Drawing.Point(136, 259);
            this.textBoxLocalAEQ.Name = "textBoxLocalAEQ";
            this.textBoxLocalAEQ.Size = new System.Drawing.Size(288, 20);
            this.textBoxLocalAEQ.TabIndex = 20;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(58, 236);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "Local AP ID";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocalAPID
            // 
            this.textBoxLocalAPID.Location = new System.Drawing.Point(136, 233);
            this.textBoxLocalAPID.Name = "textBoxLocalAPID";
            this.textBoxLocalAPID.Size = new System.Drawing.Size(288, 20);
            this.textBoxLocalAPID.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 209);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Remote T Selector";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxRemoteTSel
            // 
            this.textBoxRemoteTSel.Location = new System.Drawing.Point(136, 206);
            this.textBoxRemoteTSel.Name = "textBoxRemoteTSel";
            this.textBoxRemoteTSel.Size = new System.Drawing.Size(220, 20);
            this.textBoxRemoteTSel.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 183);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Remote S Selector";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxRemoteSSel
            // 
            this.textBoxRemoteSSel.Location = new System.Drawing.Point(136, 180);
            this.textBoxRemoteSSel.Name = "textBoxRemoteSSel";
            this.textBoxRemoteSSel.Size = new System.Drawing.Size(288, 20);
            this.textBoxRemoteSSel.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Remote P Selector";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxRemotePSel
            // 
            this.textBoxRemotePSel.Location = new System.Drawing.Point(136, 154);
            this.textBoxRemotePSel.Name = "textBoxRemotePSel";
            this.textBoxRemotePSel.Size = new System.Drawing.Size(288, 20);
            this.textBoxRemotePSel.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Remote AE Qualifier";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxRemoteAEQ
            // 
            this.textBoxRemoteAEQ.Location = new System.Drawing.Point(136, 128);
            this.textBoxRemoteAEQ.Name = "textBoxRemoteAEQ";
            this.textBoxRemoteAEQ.Size = new System.Drawing.Size(288, 20);
            this.textBoxRemoteAEQ.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Remote AP ID";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxRemoteAPID
            // 
            this.textBoxRemoteAPID.Location = new System.Drawing.Point(136, 102);
            this.textBoxRemoteAPID.Name = "textBoxRemoteAPID";
            this.textBoxRemoteAPID.Size = new System.Drawing.Size(288, 20);
            this.textBoxRemoteAPID.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "TCP Port";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(136, 76);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(288, 20);
            this.textBoxPort.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP Address";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(136, 50);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(288, 20);
            this.textBoxIP.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(136, 24);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(288, 20);
            this.textBoxName.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(142, 497);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(81, 31);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(287, 500);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(81, 28);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // comboBoxLocalTSel
            // 
            this.comboBoxLocalTSel.FormattingEnabled = true;
            this.comboBoxLocalTSel.Items.AddRange(new object[] {
            "1 octet",
            "2 octets",
            "3 octets",
            "4 octets"});
            this.comboBoxLocalTSel.Location = new System.Drawing.Point(362, 337);
            this.comboBoxLocalTSel.Name = "comboBoxLocalTSel";
            this.comboBoxLocalTSel.Size = new System.Drawing.Size(62, 21);
            this.comboBoxLocalTSel.TabIndex = 35;
            // 
            // ConnParamDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(507, 540);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxIED);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConnParamDialog";
            this.Text = "Connection Parameters";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxIED;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxRemoteAEQ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxRemoteAPID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxRemotePSel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxRemoteSSel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxRemoteTSel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxLocalTSel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxLocalSSel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxLocalPSel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxLocalAEQ;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxLocalAPID;
        private System.Windows.Forms.CheckBox checkBoxAuth;
        private System.Windows.Forms.RadioButton radioButtonPassword;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxRemoteTSel;
        private System.Windows.Forms.ComboBox comboBoxLocalTSel;
    }
}