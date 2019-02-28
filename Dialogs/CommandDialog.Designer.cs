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

namespace IEDExplorer
{
    partial class CommandDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelFlow = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelAddr = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxTest = new System.Windows.Forms.CheckBox();
            this.checkBoxSynchroCheck = new System.Windows.Forms.CheckBox();
            this.checkBoxInterlockCheck = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxValue = new System.Windows.Forms.ComboBox();
            this.groupBoxOrig = new System.Windows.Forms.GroupBox();
            this.comboBoxCat = new System.Windows.Forms.ComboBox();
            this.textBoxIdent = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dateTimePickerT = new System.Windows.Forms.DateTimePicker();
            this.checkBoxTActive = new System.Windows.Forms.CheckBox();
            this.groupBoxSBO = new System.Windows.Forms.GroupBox();
            this.checkBoxSendSBO = new System.Windows.Forms.CheckBox();
            this.checkBoxSBODiffTime = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.maskedTextBoxSBOTimeout = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxOrig.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxSBO.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(370, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Attention!!! Command will be sent to the IED!";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(68, 453);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(79, 33);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "Send";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(274, 453);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(79, 33);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelFlow
            // 
            this.labelFlow.AutoSize = true;
            this.labelFlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelFlow.Location = new System.Drawing.Point(66, 20);
            this.labelFlow.Name = "labelFlow";
            this.labelFlow.Size = new System.Drawing.Size(69, 13);
            this.labelFlow.TabIndex = 4;
            this.labelFlow.Text = "ControlModel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "CtrlModel: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Address: ";
            // 
            // labelAddr
            // 
            this.labelAddr.AutoSize = true;
            this.labelAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAddr.Location = new System.Drawing.Point(66, 41);
            this.labelAddr.Name = "labelAddr";
            this.labelAddr.Size = new System.Drawing.Size(29, 13);
            this.labelAddr.TabIndex = 7;
            this.labelAddr.Text = "Addr";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxTest);
            this.groupBox1.Controls.Add(this.checkBoxSynchroCheck);
            this.groupBox1.Controls.Add(this.checkBoxInterlockCheck);
            this.groupBox1.Location = new System.Drawing.Point(12, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 60);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Command Options";
            // 
            // checkBoxTest
            // 
            this.checkBoxTest.AutoSize = true;
            this.checkBoxTest.Location = new System.Drawing.Point(218, 28);
            this.checkBoxTest.Name = "checkBoxTest";
            this.checkBoxTest.Size = new System.Drawing.Size(47, 17);
            this.checkBoxTest.TabIndex = 2;
            this.checkBoxTest.Tag = "Test";
            this.checkBoxTest.Text = "Test";
            this.checkBoxTest.UseVisualStyleBackColor = true;
            // 
            // checkBoxSynchroCheck
            // 
            this.checkBoxSynchroCheck.AutoSize = true;
            this.checkBoxSynchroCheck.Location = new System.Drawing.Point(113, 28);
            this.checkBoxSynchroCheck.Name = "checkBoxSynchroCheck";
            this.checkBoxSynchroCheck.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSynchroCheck.TabIndex = 1;
            this.checkBoxSynchroCheck.Tag = "synchroCheck";
            this.checkBoxSynchroCheck.Text = "Synchro Check";
            this.checkBoxSynchroCheck.UseVisualStyleBackColor = true;
            // 
            // checkBoxInterlockCheck
            // 
            this.checkBoxInterlockCheck.AutoSize = true;
            this.checkBoxInterlockCheck.Location = new System.Drawing.Point(6, 28);
            this.checkBoxInterlockCheck.Name = "checkBoxInterlockCheck";
            this.checkBoxInterlockCheck.Size = new System.Drawing.Size(101, 17);
            this.checkBoxInterlockCheck.TabIndex = 0;
            this.checkBoxInterlockCheck.Tag = "interlockCheck";
            this.checkBoxInterlockCheck.Text = "Interlock Check";
            this.checkBoxInterlockCheck.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Value: ";
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.Location = new System.Drawing.Point(69, 68);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(314, 21);
            this.comboBoxValue.TabIndex = 10;
            // 
            // groupBoxOrig
            // 
            this.groupBoxOrig.Controls.Add(this.comboBoxCat);
            this.groupBoxOrig.Controls.Add(this.textBoxIdent);
            this.groupBoxOrig.Controls.Add(this.label6);
            this.groupBoxOrig.Controls.Add(this.label5);
            this.groupBoxOrig.Location = new System.Drawing.Point(10, 274);
            this.groupBoxOrig.Name = "groupBoxOrig";
            this.groupBoxOrig.Size = new System.Drawing.Size(389, 79);
            this.groupBoxOrig.TabIndex = 11;
            this.groupBoxOrig.TabStop = false;
            this.groupBoxOrig.Text = "Originator";
            // 
            // comboBoxCat
            // 
            this.comboBoxCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxCat.FormattingEnabled = true;
            this.comboBoxCat.Location = new System.Drawing.Point(86, 18);
            this.comboBoxCat.Name = "comboBoxCat";
            this.comboBoxCat.Size = new System.Drawing.Size(297, 21);
            this.comboBoxCat.TabIndex = 11;
            // 
            // textBoxIdent
            // 
            this.textBoxIdent.Location = new System.Drawing.Point(86, 48);
            this.textBoxIdent.Name = "textBoxIdent";
            this.textBoxIdent.Size = new System.Drawing.Size(297, 20);
            this.textBoxIdent.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Identifier:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Category:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxValue);
            this.groupBox2.Controls.Add(this.comboBoxValue);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.labelAddr);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.labelFlow);
            this.groupBox2.Location = new System.Drawing.Point(12, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(389, 107);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Command";
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(70, 69);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(310, 20);
            this.textBoxValue.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.dateTimePickerT);
            this.groupBox3.Controls.Add(this.checkBoxTActive);
            this.groupBox3.Location = new System.Drawing.Point(12, 211);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(389, 60);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Activated Command";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(120, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Execute at (UTC):";
            // 
            // dateTimePickerT
            // 
            this.dateTimePickerT.CustomFormat = "dd.MM.yyyy     HH:mm:ss";
            this.dateTimePickerT.Enabled = false;
            this.dateTimePickerT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerT.Location = new System.Drawing.Point(218, 25);
            this.dateTimePickerT.Name = "dateTimePickerT";
            this.dateTimePickerT.Size = new System.Drawing.Size(165, 20);
            this.dateTimePickerT.TabIndex = 3;
            // 
            // checkBoxTActive
            // 
            this.checkBoxTActive.AutoSize = true;
            this.checkBoxTActive.Location = new System.Drawing.Point(6, 28);
            this.checkBoxTActive.Name = "checkBoxTActive";
            this.checkBoxTActive.Size = new System.Drawing.Size(56, 17);
            this.checkBoxTActive.TabIndex = 0;
            this.checkBoxTActive.Tag = "Active";
            this.checkBoxTActive.Text = "Active";
            this.checkBoxTActive.UseVisualStyleBackColor = true;
            this.checkBoxTActive.CheckedChanged += new System.EventHandler(this.checkBoxTActive_CheckedChanged);
            // 
            // groupBoxSBO
            // 
            this.groupBoxSBO.Controls.Add(this.maskedTextBoxSBOTimeout);
            this.groupBoxSBO.Controls.Add(this.checkBoxSBODiffTime);
            this.groupBoxSBO.Controls.Add(this.checkBoxSendSBO);
            this.groupBoxSBO.Controls.Add(this.label8);
            this.groupBoxSBO.Location = new System.Drawing.Point(11, 359);
            this.groupBoxSBO.Name = "groupBoxSBO";
            this.groupBoxSBO.Size = new System.Drawing.Size(387, 73);
            this.groupBoxSBO.TabIndex = 13;
            this.groupBoxSBO.TabStop = false;
            this.groupBoxSBO.Text = "SBO / SBOw";
            // 
            // checkBoxSendSBO
            // 
            this.checkBoxSendSBO.AutoSize = true;
            this.checkBoxSendSBO.Location = new System.Drawing.Point(7, 19);
            this.checkBoxSendSBO.Name = "checkBoxSendSBO";
            this.checkBoxSendSBO.Size = new System.Drawing.Size(140, 17);
            this.checkBoxSendSBO.TabIndex = 1;
            this.checkBoxSendSBO.Tag = "";
            this.checkBoxSendSBO.Text = "Send SBO(w) sequence";
            this.checkBoxSendSBO.UseVisualStyleBackColor = true;
            // 
            // checkBoxSBODiffTime
            // 
            this.checkBoxSBODiffTime.AutoSize = true;
            this.checkBoxSBODiffTime.Location = new System.Drawing.Point(6, 42);
            this.checkBoxSBODiffTime.Name = "checkBoxSBODiffTime";
            this.checkBoxSBODiffTime.Size = new System.Drawing.Size(214, 17);
            this.checkBoxSBODiffTime.TabIndex = 1;
            this.checkBoxSBODiffTime.Tag = "";
            this.checkBoxSBODiffTime.Text = "Send different time in SBO(w) sequence";
            this.checkBoxSBODiffTime.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(162, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(172, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Time between Select/Operate [ms]";
            // 
            // maskedTextBoxSBOTimeout
            // 
            this.maskedTextBoxSBOTimeout.Location = new System.Drawing.Point(337, 17);
            this.maskedTextBoxSBOTimeout.Mask = "00000";
            this.maskedTextBoxSBOTimeout.Name = "maskedTextBoxSBOTimeout";
            this.maskedTextBoxSBOTimeout.Size = new System.Drawing.Size(44, 20);
            this.maskedTextBoxSBOTimeout.TabIndex = 10;
            this.maskedTextBoxSBOTimeout.Text = "100";
            this.maskedTextBoxSBOTimeout.ValidatingType = typeof(int);
            // 
            // CommandDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 506);
            this.Controls.Add(this.groupBoxSBO);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxOrig);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommandDialog";
            this.Text = "Sending a Command";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxOrig.ResumeLayout(false);
            this.groupBoxOrig.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxSBO.ResumeLayout(false);
            this.groupBoxSBO.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelFlow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelAddr;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxInterlockCheck;
        private System.Windows.Forms.CheckBox checkBoxSynchroCheck;
        private System.Windows.Forms.CheckBox checkBoxTest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxValue;
        private System.Windows.Forms.GroupBox groupBoxOrig;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxIdent;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxCat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dateTimePickerT;
        private System.Windows.Forms.CheckBox checkBoxTActive;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.GroupBox groupBoxSBO;
        private System.Windows.Forms.CheckBox checkBoxSBODiffTime;
        private System.Windows.Forms.CheckBox checkBoxSendSBO;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSBOTimeout;
    }
}