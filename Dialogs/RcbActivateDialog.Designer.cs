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
    partial class RcbActivateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RcbActivateDialog));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDatSet = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxTrgOps_GI = new System.Windows.Forms.CheckBox();
            this.checkBoxTrgOps_Integrity = new System.Windows.Forms.CheckBox();
            this.checkBoxTrgOps_DataUpdate = new System.Windows.Forms.CheckBox();
            this.checkBoxTrgOps_QualityChange = new System.Windows.Forms.CheckBox();
            this.checkBoxTrgOps_DataChange = new System.Windows.Forms.CheckBox();
            this.checkBoxTrgOps_send = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxOptFlds_ConfRev = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_EntryID = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_BufferOverflow = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_DataReference = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_DataSet = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_ReasonForInclusion = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_TimeStamp = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_SeqNum = new System.Windows.Forms.CheckBox();
            this.checkBoxOptFlds_send = new System.Windows.Forms.CheckBox();
            this.checkBoxGI = new System.Windows.Forms.CheckBox();
            this.checkBoxRptEna = new System.Windows.Forms.CheckBox();
            this.checkBoxDatSet_send = new System.Windows.Forms.CheckBox();
            this.checkBoxGI_send = new System.Windows.Forms.CheckBox();
            this.checkBoxRptEna_send = new System.Windows.Forms.CheckBox();
            this.checkBoxIntgPd_send = new System.Windows.Forms.CheckBox();
            this.checkBoxBufTm_send = new System.Windows.Forms.CheckBox();
            this.checkBoxRptID_send = new System.Windows.Forms.CheckBox();
            this.textBoxIntgPd = new System.Windows.Forms.TextBox();
            this.textBoxBufTm = new System.Windows.Forms.TextBox();
            this.textBoxRptID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelBufTm = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxResv_send = new System.Windows.Forms.CheckBox();
            this.checkBoxResv = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxPurgeBuf_send = new System.Windows.Forms.CheckBox();
            this.checkBoxPurgeBuf = new System.Windows.Forms.CheckBox();
            this.textBoxResvTms = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxResvTms_send = new System.Windows.Forms.CheckBox();
            this.textBoxEntryID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxEntryID_send = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.labelReportType = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelReportName = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(65, 592);
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
            this.buttonCancel.Location = new System.Drawing.Point(271, 592);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(79, 33);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(15, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "RptID (Report ID)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "DatSet (Data Set)";
            // 
            // comboBoxDatSet
            // 
            this.comboBoxDatSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDatSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxDatSet.FormattingEnabled = true;
            this.comboBoxDatSet.Location = new System.Drawing.Point(123, 45);
            this.comboBoxDatSet.Name = "comboBoxDatSet";
            this.comboBoxDatSet.Size = new System.Drawing.Size(226, 21);
            this.comboBoxDatSet.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.checkBoxGI);
            this.groupBox2.Controls.Add(this.checkBoxRptEna);
            this.groupBox2.Controls.Add(this.checkBoxDatSet_send);
            this.groupBox2.Controls.Add(this.checkBoxGI_send);
            this.groupBox2.Controls.Add(this.checkBoxRptEna_send);
            this.groupBox2.Controls.Add(this.checkBoxIntgPd_send);
            this.groupBox2.Controls.Add(this.checkBoxBufTm_send);
            this.groupBox2.Controls.Add(this.checkBoxRptID_send);
            this.groupBox2.Controls.Add(this.textBoxIntgPd);
            this.groupBox2.Controls.Add(this.textBoxBufTm);
            this.groupBox2.Controls.Add(this.textBoxRptID);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBoxDatSet);
            this.groupBox2.Controls.Add(this.labelBufTm);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(389, 350);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Common";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxTrgOps_GI);
            this.groupBox4.Controls.Add(this.checkBoxTrgOps_Integrity);
            this.groupBox4.Controls.Add(this.checkBoxTrgOps_DataUpdate);
            this.groupBox4.Controls.Add(this.checkBoxTrgOps_QualityChange);
            this.groupBox4.Controls.Add(this.checkBoxTrgOps_DataChange);
            this.groupBox4.Controls.Add(this.checkBoxTrgOps_send);
            this.groupBox4.Location = new System.Drawing.Point(9, 197);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(374, 67);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "TrgOps (Trigger Options)";
            // 
            // checkBoxTrgOps_GI
            // 
            this.checkBoxTrgOps_GI.AutoSize = true;
            this.checkBoxTrgOps_GI.Location = new System.Drawing.Point(123, 42);
            this.checkBoxTrgOps_GI.Name = "checkBoxTrgOps_GI";
            this.checkBoxTrgOps_GI.Size = new System.Drawing.Size(37, 17);
            this.checkBoxTrgOps_GI.TabIndex = 23;
            this.checkBoxTrgOps_GI.Tag = "Active";
            this.checkBoxTrgOps_GI.Text = "GI";
            this.checkBoxTrgOps_GI.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrgOps_Integrity
            // 
            this.checkBoxTrgOps_Integrity.AutoSize = true;
            this.checkBoxTrgOps_Integrity.Location = new System.Drawing.Point(9, 42);
            this.checkBoxTrgOps_Integrity.Name = "checkBoxTrgOps_Integrity";
            this.checkBoxTrgOps_Integrity.Size = new System.Drawing.Size(63, 17);
            this.checkBoxTrgOps_Integrity.TabIndex = 22;
            this.checkBoxTrgOps_Integrity.Tag = "Active";
            this.checkBoxTrgOps_Integrity.Text = "Integrity";
            this.checkBoxTrgOps_Integrity.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrgOps_DataUpdate
            // 
            this.checkBoxTrgOps_DataUpdate.AutoSize = true;
            this.checkBoxTrgOps_DataUpdate.Location = new System.Drawing.Point(229, 19);
            this.checkBoxTrgOps_DataUpdate.Name = "checkBoxTrgOps_DataUpdate";
            this.checkBoxTrgOps_DataUpdate.Size = new System.Drawing.Size(84, 17);
            this.checkBoxTrgOps_DataUpdate.TabIndex = 19;
            this.checkBoxTrgOps_DataUpdate.Tag = "Active";
            this.checkBoxTrgOps_DataUpdate.Text = "DataUpdate";
            this.checkBoxTrgOps_DataUpdate.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrgOps_QualityChange
            // 
            this.checkBoxTrgOps_QualityChange.AutoSize = true;
            this.checkBoxTrgOps_QualityChange.Location = new System.Drawing.Point(123, 19);
            this.checkBoxTrgOps_QualityChange.Name = "checkBoxTrgOps_QualityChange";
            this.checkBoxTrgOps_QualityChange.Size = new System.Drawing.Size(95, 17);
            this.checkBoxTrgOps_QualityChange.TabIndex = 16;
            this.checkBoxTrgOps_QualityChange.Tag = "Active";
            this.checkBoxTrgOps_QualityChange.Text = "QualityChange";
            this.checkBoxTrgOps_QualityChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrgOps_DataChange
            // 
            this.checkBoxTrgOps_DataChange.AutoSize = true;
            this.checkBoxTrgOps_DataChange.Location = new System.Drawing.Point(9, 19);
            this.checkBoxTrgOps_DataChange.Name = "checkBoxTrgOps_DataChange";
            this.checkBoxTrgOps_DataChange.Size = new System.Drawing.Size(86, 17);
            this.checkBoxTrgOps_DataChange.TabIndex = 17;
            this.checkBoxTrgOps_DataChange.Tag = "Active";
            this.checkBoxTrgOps_DataChange.Text = "DataChange";
            this.checkBoxTrgOps_DataChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrgOps_send
            // 
            this.checkBoxTrgOps_send.AutoSize = true;
            this.checkBoxTrgOps_send.Location = new System.Drawing.Point(355, 33);
            this.checkBoxTrgOps_send.Name = "checkBoxTrgOps_send";
            this.checkBoxTrgOps_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTrgOps_send.TabIndex = 15;
            this.checkBoxTrgOps_send.UseVisualStyleBackColor = true;
            this.checkBoxTrgOps_send.CheckedChanged += new System.EventHandler(this.checkBoxTrgOps_send_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(354, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Send";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_ConfRev);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_EntryID);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_BufferOverflow);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_DataReference);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_DataSet);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_ReasonForInclusion);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_TimeStamp);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_SeqNum);
            this.groupBox1.Controls.Add(this.checkBoxOptFlds_send);
            this.groupBox1.Location = new System.Drawing.Point(9, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(374, 93);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OptFlds (Optional Fields)";
            // 
            // checkBoxOptFlds_ConfRev
            // 
            this.checkBoxOptFlds_ConfRev.AutoSize = true;
            this.checkBoxOptFlds_ConfRev.Location = new System.Drawing.Point(123, 65);
            this.checkBoxOptFlds_ConfRev.Name = "checkBoxOptFlds_ConfRev";
            this.checkBoxOptFlds_ConfRev.Size = new System.Drawing.Size(68, 17);
            this.checkBoxOptFlds_ConfRev.TabIndex = 18;
            this.checkBoxOptFlds_ConfRev.Tag = "Active";
            this.checkBoxOptFlds_ConfRev.Text = "ConfRev";
            this.checkBoxOptFlds_ConfRev.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_EntryID
            // 
            this.checkBoxOptFlds_EntryID.AutoSize = true;
            this.checkBoxOptFlds_EntryID.Location = new System.Drawing.Point(9, 65);
            this.checkBoxOptFlds_EntryID.Name = "checkBoxOptFlds_EntryID";
            this.checkBoxOptFlds_EntryID.Size = new System.Drawing.Size(61, 17);
            this.checkBoxOptFlds_EntryID.TabIndex = 21;
            this.checkBoxOptFlds_EntryID.Tag = "Active";
            this.checkBoxOptFlds_EntryID.Text = "EntryID";
            this.checkBoxOptFlds_EntryID.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_BufferOverflow
            // 
            this.checkBoxOptFlds_BufferOverflow.AutoSize = true;
            this.checkBoxOptFlds_BufferOverflow.Location = new System.Drawing.Point(229, 42);
            this.checkBoxOptFlds_BufferOverflow.Name = "checkBoxOptFlds_BufferOverflow";
            this.checkBoxOptFlds_BufferOverflow.Size = new System.Drawing.Size(96, 17);
            this.checkBoxOptFlds_BufferOverflow.TabIndex = 20;
            this.checkBoxOptFlds_BufferOverflow.Tag = "Active";
            this.checkBoxOptFlds_BufferOverflow.Text = "BufferOverflow";
            this.checkBoxOptFlds_BufferOverflow.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_DataReference
            // 
            this.checkBoxOptFlds_DataReference.AutoSize = true;
            this.checkBoxOptFlds_DataReference.Location = new System.Drawing.Point(123, 42);
            this.checkBoxOptFlds_DataReference.Name = "checkBoxOptFlds_DataReference";
            this.checkBoxOptFlds_DataReference.Size = new System.Drawing.Size(99, 17);
            this.checkBoxOptFlds_DataReference.TabIndex = 23;
            this.checkBoxOptFlds_DataReference.Tag = "Active";
            this.checkBoxOptFlds_DataReference.Text = "DataReference";
            this.checkBoxOptFlds_DataReference.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_DataSet
            // 
            this.checkBoxOptFlds_DataSet.AutoSize = true;
            this.checkBoxOptFlds_DataSet.Location = new System.Drawing.Point(9, 42);
            this.checkBoxOptFlds_DataSet.Name = "checkBoxOptFlds_DataSet";
            this.checkBoxOptFlds_DataSet.Size = new System.Drawing.Size(65, 17);
            this.checkBoxOptFlds_DataSet.TabIndex = 22;
            this.checkBoxOptFlds_DataSet.Tag = "Active";
            this.checkBoxOptFlds_DataSet.Text = "DataSet";
            this.checkBoxOptFlds_DataSet.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_ReasonForInclusion
            // 
            this.checkBoxOptFlds_ReasonForInclusion.AutoSize = true;
            this.checkBoxOptFlds_ReasonForInclusion.Location = new System.Drawing.Point(229, 19);
            this.checkBoxOptFlds_ReasonForInclusion.Name = "checkBoxOptFlds_ReasonForInclusion";
            this.checkBoxOptFlds_ReasonForInclusion.Size = new System.Drawing.Size(120, 17);
            this.checkBoxOptFlds_ReasonForInclusion.TabIndex = 19;
            this.checkBoxOptFlds_ReasonForInclusion.Tag = "Active";
            this.checkBoxOptFlds_ReasonForInclusion.Text = "ReasonForInclusion";
            this.checkBoxOptFlds_ReasonForInclusion.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_TimeStamp
            // 
            this.checkBoxOptFlds_TimeStamp.AutoSize = true;
            this.checkBoxOptFlds_TimeStamp.Location = new System.Drawing.Point(123, 19);
            this.checkBoxOptFlds_TimeStamp.Name = "checkBoxOptFlds_TimeStamp";
            this.checkBoxOptFlds_TimeStamp.Size = new System.Drawing.Size(79, 17);
            this.checkBoxOptFlds_TimeStamp.TabIndex = 16;
            this.checkBoxOptFlds_TimeStamp.Tag = "Active";
            this.checkBoxOptFlds_TimeStamp.Text = "TimeStamp";
            this.checkBoxOptFlds_TimeStamp.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_SeqNum
            // 
            this.checkBoxOptFlds_SeqNum.AutoSize = true;
            this.checkBoxOptFlds_SeqNum.Location = new System.Drawing.Point(9, 19);
            this.checkBoxOptFlds_SeqNum.Name = "checkBoxOptFlds_SeqNum";
            this.checkBoxOptFlds_SeqNum.Size = new System.Drawing.Size(67, 17);
            this.checkBoxOptFlds_SeqNum.TabIndex = 17;
            this.checkBoxOptFlds_SeqNum.Tag = "Active";
            this.checkBoxOptFlds_SeqNum.Text = "SeqNum";
            this.checkBoxOptFlds_SeqNum.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptFlds_send
            // 
            this.checkBoxOptFlds_send.AutoSize = true;
            this.checkBoxOptFlds_send.Location = new System.Drawing.Point(355, 33);
            this.checkBoxOptFlds_send.Name = "checkBoxOptFlds_send";
            this.checkBoxOptFlds_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxOptFlds_send.TabIndex = 15;
            this.checkBoxOptFlds_send.UseVisualStyleBackColor = true;
            this.checkBoxOptFlds_send.CheckedChanged += new System.EventHandler(this.checkBoxOptFlds_send_CheckedChanged);
            // 
            // checkBoxGI
            // 
            this.checkBoxGI.AutoSize = true;
            this.checkBoxGI.Location = new System.Drawing.Point(238, 319);
            this.checkBoxGI.Name = "checkBoxGI";
            this.checkBoxGI.Size = new System.Drawing.Size(37, 17);
            this.checkBoxGI.TabIndex = 19;
            this.checkBoxGI.Tag = "Active";
            this.checkBoxGI.Text = "GI";
            this.checkBoxGI.UseVisualStyleBackColor = true;
            // 
            // checkBoxRptEna
            // 
            this.checkBoxRptEna.AutoSize = true;
            this.checkBoxRptEna.Location = new System.Drawing.Point(238, 296);
            this.checkBoxRptEna.Name = "checkBoxRptEna";
            this.checkBoxRptEna.Size = new System.Drawing.Size(62, 17);
            this.checkBoxRptEna.TabIndex = 19;
            this.checkBoxRptEna.Tag = "Active";
            this.checkBoxRptEna.Text = "RptEna";
            this.checkBoxRptEna.UseVisualStyleBackColor = true;
            // 
            // checkBoxDatSet_send
            // 
            this.checkBoxDatSet_send.AutoSize = true;
            this.checkBoxDatSet_send.Location = new System.Drawing.Point(364, 47);
            this.checkBoxDatSet_send.Name = "checkBoxDatSet_send";
            this.checkBoxDatSet_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxDatSet_send.TabIndex = 12;
            this.checkBoxDatSet_send.UseVisualStyleBackColor = true;
            this.checkBoxDatSet_send.CheckedChanged += new System.EventHandler(this.checkBoxDatSet_send_CheckedChanged);
            // 
            // checkBoxGI_send
            // 
            this.checkBoxGI_send.AutoSize = true;
            this.checkBoxGI_send.Location = new System.Drawing.Point(364, 319);
            this.checkBoxGI_send.Name = "checkBoxGI_send";
            this.checkBoxGI_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxGI_send.TabIndex = 12;
            this.checkBoxGI_send.UseVisualStyleBackColor = true;
            this.checkBoxGI_send.CheckedChanged += new System.EventHandler(this.checkBoxGI_send_CheckedChanged);
            // 
            // checkBoxRptEna_send
            // 
            this.checkBoxRptEna_send.AutoSize = true;
            this.checkBoxRptEna_send.Location = new System.Drawing.Point(364, 296);
            this.checkBoxRptEna_send.Name = "checkBoxRptEna_send";
            this.checkBoxRptEna_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxRptEna_send.TabIndex = 12;
            this.checkBoxRptEna_send.UseVisualStyleBackColor = true;
            this.checkBoxRptEna_send.CheckedChanged += new System.EventHandler(this.checkBoxRptEna_send_CheckedChanged);
            // 
            // checkBoxIntgPd_send
            // 
            this.checkBoxIntgPd_send.AutoSize = true;
            this.checkBoxIntgPd_send.Location = new System.Drawing.Point(364, 273);
            this.checkBoxIntgPd_send.Name = "checkBoxIntgPd_send";
            this.checkBoxIntgPd_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxIntgPd_send.TabIndex = 12;
            this.checkBoxIntgPd_send.UseVisualStyleBackColor = true;
            this.checkBoxIntgPd_send.CheckedChanged += new System.EventHandler(this.checkBoxIntgPd_send_CheckedChanged);
            // 
            // checkBoxBufTm_send
            // 
            this.checkBoxBufTm_send.AutoSize = true;
            this.checkBoxBufTm_send.Location = new System.Drawing.Point(364, 174);
            this.checkBoxBufTm_send.Name = "checkBoxBufTm_send";
            this.checkBoxBufTm_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxBufTm_send.TabIndex = 12;
            this.checkBoxBufTm_send.UseVisualStyleBackColor = true;
            this.checkBoxBufTm_send.CheckedChanged += new System.EventHandler(this.checkBoxBufTm_send_CheckedChanged);
            // 
            // checkBoxRptID_send
            // 
            this.checkBoxRptID_send.AutoSize = true;
            this.checkBoxRptID_send.Location = new System.Drawing.Point(364, 22);
            this.checkBoxRptID_send.Name = "checkBoxRptID_send";
            this.checkBoxRptID_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxRptID_send.TabIndex = 12;
            this.checkBoxRptID_send.UseVisualStyleBackColor = true;
            this.checkBoxRptID_send.CheckedChanged += new System.EventHandler(this.checkBoxRptID_send_CheckedChanged);
            // 
            // textBoxIntgPd
            // 
            this.textBoxIntgPd.Location = new System.Drawing.Point(238, 270);
            this.textBoxIntgPd.Name = "textBoxIntgPd";
            this.textBoxIntgPd.Size = new System.Drawing.Size(111, 20);
            this.textBoxIntgPd.TabIndex = 11;
            // 
            // textBoxBufTm
            // 
            this.textBoxBufTm.Location = new System.Drawing.Point(238, 171);
            this.textBoxBufTm.Name = "textBoxBufTm";
            this.textBoxBufTm.Size = new System.Drawing.Size(111, 20);
            this.textBoxBufTm.TabIndex = 11;
            // 
            // textBoxRptID
            // 
            this.textBoxRptID.Location = new System.Drawing.Point(123, 19);
            this.textBoxRptID.Name = "textBoxRptID";
            this.textBoxRptID.Size = new System.Drawing.Size(226, 20);
            this.textBoxRptID.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(15, 323);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "GI (General Interrogation)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(15, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "RptEna (Report Enable)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(15, 271);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "IntgPd (Integrity Period)";
            // 
            // labelBufTm
            // 
            this.labelBufTm.AutoSize = true;
            this.labelBufTm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelBufTm.Location = new System.Drawing.Point(15, 172);
            this.labelBufTm.Name = "labelBufTm";
            this.labelBufTm.Size = new System.Drawing.Size(101, 13);
            this.labelBufTm.TabIndex = 5;
            this.labelBufTm.Text = "BufTm (Buffer Time)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.checkBoxResv_send);
            this.groupBox3.Controls.Add(this.checkBoxResv);
            this.groupBox3.Location = new System.Drawing.Point(12, 422);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(389, 54);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Unbuffered Report Specific";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(15, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Resv (Reserve)";
            // 
            // checkBoxResv_send
            // 
            this.checkBoxResv_send.AutoSize = true;
            this.checkBoxResv_send.Location = new System.Drawing.Point(364, 21);
            this.checkBoxResv_send.Name = "checkBoxResv_send";
            this.checkBoxResv_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxResv_send.TabIndex = 12;
            this.checkBoxResv_send.UseVisualStyleBackColor = true;
            this.checkBoxResv_send.CheckedChanged += new System.EventHandler(this.checkBoxResv_send_CheckedChanged);
            // 
            // checkBoxResv
            // 
            this.checkBoxResv.AutoSize = true;
            this.checkBoxResv.Location = new System.Drawing.Point(238, 21);
            this.checkBoxResv.Name = "checkBoxResv";
            this.checkBoxResv.Size = new System.Drawing.Size(51, 17);
            this.checkBoxResv.TabIndex = 19;
            this.checkBoxResv.Tag = "Active";
            this.checkBoxResv.Text = "Resv";
            this.checkBoxResv.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.checkBoxPurgeBuf_send);
            this.groupBox5.Controls.Add(this.checkBoxPurgeBuf);
            this.groupBox5.Controls.Add(this.textBoxResvTms);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.checkBoxResvTms_send);
            this.groupBox5.Controls.Add(this.textBoxEntryID);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.checkBoxEntryID_send);
            this.groupBox5.Location = new System.Drawing.Point(12, 482);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(389, 104);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Buffered Report Specific";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(15, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "PurgeBuf (Purge Buffer)";
            // 
            // checkBoxPurgeBuf_send
            // 
            this.checkBoxPurgeBuf_send.AutoSize = true;
            this.checkBoxPurgeBuf_send.Location = new System.Drawing.Point(364, 21);
            this.checkBoxPurgeBuf_send.Name = "checkBoxPurgeBuf_send";
            this.checkBoxPurgeBuf_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxPurgeBuf_send.TabIndex = 12;
            this.checkBoxPurgeBuf_send.UseVisualStyleBackColor = true;
            this.checkBoxPurgeBuf_send.CheckedChanged += new System.EventHandler(this.checkBoxPurgeBuf_send_CheckedChanged);
            // 
            // checkBoxPurgeBuf
            // 
            this.checkBoxPurgeBuf.AutoSize = true;
            this.checkBoxPurgeBuf.Location = new System.Drawing.Point(238, 21);
            this.checkBoxPurgeBuf.Name = "checkBoxPurgeBuf";
            this.checkBoxPurgeBuf.Size = new System.Drawing.Size(70, 17);
            this.checkBoxPurgeBuf.TabIndex = 19;
            this.checkBoxPurgeBuf.Tag = "Active";
            this.checkBoxPurgeBuf.Text = "PurgeBuf";
            this.checkBoxPurgeBuf.UseVisualStyleBackColor = true;
            // 
            // textBoxResvTms
            // 
            this.textBoxResvTms.Location = new System.Drawing.Point(238, 70);
            this.textBoxResvTms.Name = "textBoxResvTms";
            this.textBoxResvTms.Size = new System.Drawing.Size(111, 20);
            this.textBoxResvTms.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(15, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "ResvTms (Reserve Time)";
            // 
            // checkBoxResvTms_send
            // 
            this.checkBoxResvTms_send.AutoSize = true;
            this.checkBoxResvTms_send.Location = new System.Drawing.Point(364, 73);
            this.checkBoxResvTms_send.Name = "checkBoxResvTms_send";
            this.checkBoxResvTms_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxResvTms_send.TabIndex = 12;
            this.checkBoxResvTms_send.UseVisualStyleBackColor = true;
            this.checkBoxResvTms_send.CheckedChanged += new System.EventHandler(this.checkBoxResvTms_send_CheckedChanged);
            // 
            // textBoxEntryID
            // 
            this.textBoxEntryID.Location = new System.Drawing.Point(123, 44);
            this.textBoxEntryID.Name = "textBoxEntryID";
            this.textBoxEntryID.Size = new System.Drawing.Size(226, 20);
            this.textBoxEntryID.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(15, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "EntryID (Entry ID)";
            // 
            // checkBoxEntryID_send
            // 
            this.checkBoxEntryID_send.AutoSize = true;
            this.checkBoxEntryID_send.Location = new System.Drawing.Point(364, 47);
            this.checkBoxEntryID_send.Name = "checkBoxEntryID_send";
            this.checkBoxEntryID_send.Size = new System.Drawing.Size(15, 14);
            this.checkBoxEntryID_send.TabIndex = 12;
            this.checkBoxEntryID_send.UseVisualStyleBackColor = true;
            this.checkBoxEntryID_send.CheckedChanged += new System.EventHandler(this.checkBoxEntryID_send_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.labelReportType);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.labelReportName);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Location = new System.Drawing.Point(12, 15);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(388, 51);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Report Control Block";
            // 
            // labelReportType
            // 
            this.labelReportType.AutoSize = true;
            this.labelReportType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReportType.Location = new System.Drawing.Point(310, 24);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(33, 15);
            this.labelReportType.TabIndex = 0;
            this.labelReportType.Text = "Type";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(273, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Type";
            // 
            // labelReportName
            // 
            this.labelReportName.AutoSize = true;
            this.labelReportName.BackColor = System.Drawing.SystemColors.Control;
            this.labelReportName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReportName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelReportName.Location = new System.Drawing.Point(69, 22);
            this.labelReportName.Name = "labelReportName";
            this.labelReportName.Size = new System.Drawing.Size(37, 15);
            this.labelReportName.TabIndex = 0;
            this.labelReportName.Text = "Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Name";
            // 
            // RcbActivateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 635);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RcbActivateDialog";
            this.Text = " Configure and activate RCB";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxDatSet;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxRptID_send;
        private System.Windows.Forms.TextBox textBoxRptID;
        private System.Windows.Forms.CheckBox checkBoxDatSet_send;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_ConfRev;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_EntryID;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_BufferOverflow;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_DataReference;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_DataSet;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_ReasonForInclusion;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_TimeStamp;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_SeqNum;
        private System.Windows.Forms.CheckBox checkBoxOptFlds_send;
        private System.Windows.Forms.CheckBox checkBoxBufTm_send;
        private System.Windows.Forms.TextBox textBoxBufTm;
        private System.Windows.Forms.Label labelBufTm;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxTrgOps_GI;
        private System.Windows.Forms.CheckBox checkBoxTrgOps_Integrity;
        private System.Windows.Forms.CheckBox checkBoxTrgOps_DataUpdate;
        private System.Windows.Forms.CheckBox checkBoxTrgOps_QualityChange;
        private System.Windows.Forms.CheckBox checkBoxTrgOps_DataChange;
        private System.Windows.Forms.CheckBox checkBoxTrgOps_send;
        private System.Windows.Forms.CheckBox checkBoxIntgPd_send;
        private System.Windows.Forms.TextBox textBoxIntgPd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxRptEna;
        private System.Windows.Forms.CheckBox checkBoxRptEna_send;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxGI;
        private System.Windows.Forms.CheckBox checkBoxGI_send;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxResv_send;
        private System.Windows.Forms.CheckBox checkBoxResv;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxPurgeBuf_send;
        private System.Windows.Forms.CheckBox checkBoxPurgeBuf;
        private System.Windows.Forms.TextBox textBoxResvTms;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxResvTms_send;
        private System.Windows.Forms.TextBox textBoxEntryID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxEntryID_send;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label labelReportType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelReportName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
    }
}