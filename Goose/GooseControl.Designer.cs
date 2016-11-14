namespace IEDExplorer
{
    partial class GooseControl
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
            if (_run && _senderThread != null)
            {
                _run = false;
                _senderThread.Abort();
                _senderThread = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
 /*

            if (gooseDataEdit != null && !gooseDataEdit.IsDisposed && !gooseDataEdit.Disposing)                        
                gooseDataEdit.Close();            
*/
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_SendOnce = new System.Windows.Forms.Button();
            this.groupBox_Parameters = new System.Windows.Forms.GroupBox();
            this.checkBox_SqNumLock = new System.Windows.Forms.CheckBox();
            this.checkBox_StNumLock = new System.Windows.Forms.CheckBox();
            this.numericUpDown_CfgRev = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_SqNum = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_StNum = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_TTL = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_AppID = new System.Windows.Forms.NumericUpDown();
            this.label1_EditData = new System.Windows.Forms.Label();
            this.button_EditData = new System.Windows.Forms.Button();
            this.comboBox_NdsCom = new System.Windows.Forms.ComboBox();
            this.comboBox_Test = new System.Windows.Forms.ComboBox();
            this.label_NdsCom = new System.Windows.Forms.Label();
            this.label_Test = new System.Windows.Forms.Label();
            this.label_CfgRev = new System.Windows.Forms.Label();
            this.label_StNum = new System.Windows.Forms.Label();
            this.label_SqNum = new System.Windows.Forms.Label();
            this.textBox_Time = new System.Windows.Forms.TextBox();
            this.label_Time = new System.Windows.Forms.Label();
            this.textBox_GoCBRef = new System.Windows.Forms.TextBox();
            this.label_GoCBRef = new System.Windows.Forms.Label();
            this.textBox_DatSet = new System.Windows.Forms.TextBox();
            this.label_DatSet = new System.Windows.Forms.Label();
            this.textBox_GoID = new System.Windows.Forms.TextBox();
            this.label_GoID = new System.Windows.Forms.Label();
            this.label_AppID = new System.Windows.Forms.Label();
            this.label_TTL = new System.Windows.Forms.Label();
            this.button_Close = new System.Windows.Forms.Button();
            this.groupBox_Ethernet = new System.Windows.Forms.GroupBox();
            this.maskedTextBox_dstMac = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox_srcMac = new System.Windows.Forms.MaskedTextBox();
            this.numericUpDown_VlanVID = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_VlanPrio = new System.Windows.Forms.NumericUpDown();
            this.comboBox_VlanCFI = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_VlanTagEn = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_SrcMac = new System.Windows.Forms.Label();
            this.label_DstMac = new System.Windows.Forms.Label();
            this.button_Run = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label_GoosesCnt = new System.Windows.Forms.Label();
            this.textBox_GoosesCnt = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox_Parameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CfgRev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_SqNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_StNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TTL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AppID)).BeginInit();
            this.groupBox_Ethernet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VlanVID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VlanPrio)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_SendOnce);
            this.groupBox1.Controls.Add(this.groupBox_Parameters);
            this.groupBox1.Controls.Add(this.button_Close);
            this.groupBox1.Controls.Add(this.groupBox_Ethernet);
            this.groupBox1.Controls.Add(this.button_Run);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(687, 243);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Goose";
            // 
            // button_SendOnce
            // 
            this.button_SendOnce.Location = new System.Drawing.Point(533, 214);
            this.button_SendOnce.Name = "button_SendOnce";
            this.button_SendOnce.Size = new System.Drawing.Size(74, 23);
            this.button_SendOnce.TabIndex = 46;
            this.button_SendOnce.Text = "Send 1x";
            this.button_SendOnce.UseVisualStyleBackColor = true;
            this.button_SendOnce.Click += new System.EventHandler(this.button_SendOnce_Click);
            // 
            // groupBox_Parameters
            // 
            this.groupBox_Parameters.Controls.Add(this.textBox_GoosesCnt);
            this.groupBox_Parameters.Controls.Add(this.label_GoosesCnt);
            this.groupBox_Parameters.Controls.Add(this.checkBox_SqNumLock);
            this.groupBox_Parameters.Controls.Add(this.checkBox_StNumLock);
            this.groupBox_Parameters.Controls.Add(this.numericUpDown_CfgRev);
            this.groupBox_Parameters.Controls.Add(this.numericUpDown_SqNum);
            this.groupBox_Parameters.Controls.Add(this.numericUpDown_StNum);
            this.groupBox_Parameters.Controls.Add(this.numericUpDown_TTL);
            this.groupBox_Parameters.Controls.Add(this.numericUpDown_AppID);
            this.groupBox_Parameters.Controls.Add(this.label1_EditData);
            this.groupBox_Parameters.Controls.Add(this.button_EditData);
            this.groupBox_Parameters.Controls.Add(this.comboBox_NdsCom);
            this.groupBox_Parameters.Controls.Add(this.comboBox_Test);
            this.groupBox_Parameters.Controls.Add(this.label_NdsCom);
            this.groupBox_Parameters.Controls.Add(this.label_Test);
            this.groupBox_Parameters.Controls.Add(this.label_CfgRev);
            this.groupBox_Parameters.Controls.Add(this.label_StNum);
            this.groupBox_Parameters.Controls.Add(this.label_SqNum);
            this.groupBox_Parameters.Controls.Add(this.textBox_Time);
            this.groupBox_Parameters.Controls.Add(this.label_Time);
            this.groupBox_Parameters.Controls.Add(this.textBox_GoCBRef);
            this.groupBox_Parameters.Controls.Add(this.label_GoCBRef);
            this.groupBox_Parameters.Controls.Add(this.textBox_DatSet);
            this.groupBox_Parameters.Controls.Add(this.label_DatSet);
            this.groupBox_Parameters.Controls.Add(this.textBox_GoID);
            this.groupBox_Parameters.Controls.Add(this.label_GoID);
            this.groupBox_Parameters.Controls.Add(this.label_AppID);
            this.groupBox_Parameters.Controls.Add(this.label_TTL);
            this.groupBox_Parameters.Location = new System.Drawing.Point(6, 21);
            this.groupBox_Parameters.Name = "groupBox_Parameters";
            this.groupBox_Parameters.Size = new System.Drawing.Size(458, 217);
            this.groupBox_Parameters.TabIndex = 42;
            this.groupBox_Parameters.TabStop = false;
            this.groupBox_Parameters.Text = "Parameters:";
            // 
            // checkBox_SqNumLock
            // 
            this.checkBox_SqNumLock.AutoSize = true;
            this.checkBox_SqNumLock.Location = new System.Drawing.Point(424, 53);
            this.checkBox_SqNumLock.Name = "checkBox_SqNumLock";
            this.checkBox_SqNumLock.Size = new System.Drawing.Size(18, 17);
            this.checkBox_SqNumLock.TabIndex = 47;
            this.checkBox_SqNumLock.UseVisualStyleBackColor = true;
            // 
            // checkBox_StNumLock
            // 
            this.checkBox_StNumLock.AutoSize = true;
            this.checkBox_StNumLock.Location = new System.Drawing.Point(424, 24);
            this.checkBox_StNumLock.Name = "checkBox_StNumLock";
            this.checkBox_StNumLock.Size = new System.Drawing.Size(18, 17);
            this.checkBox_StNumLock.TabIndex = 46;
            this.checkBox_StNumLock.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_CfgRev
            // 
            this.numericUpDown_CfgRev.Location = new System.Drawing.Point(352, 78);
            this.numericUpDown_CfgRev.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numericUpDown_CfgRev.Name = "numericUpDown_CfgRev";
            this.numericUpDown_CfgRev.Size = new System.Drawing.Size(90, 22);
            this.numericUpDown_CfgRev.TabIndex = 45;
            this.numericUpDown_CfgRev.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.numericUpDown_CfgRev.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.numericUpDown_CfgRev.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // numericUpDown_SqNum
            // 
            this.numericUpDown_SqNum.Location = new System.Drawing.Point(352, 50);
            this.numericUpDown_SqNum.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numericUpDown_SqNum.Name = "numericUpDown_SqNum";
            this.numericUpDown_SqNum.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown_SqNum.TabIndex = 44;
            this.numericUpDown_SqNum.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.numericUpDown_SqNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.numericUpDown_SqNum.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // numericUpDown_StNum
            // 
            this.numericUpDown_StNum.Location = new System.Drawing.Point(352, 21);
            this.numericUpDown_StNum.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numericUpDown_StNum.Name = "numericUpDown_StNum";
            this.numericUpDown_StNum.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown_StNum.TabIndex = 43;
            this.numericUpDown_StNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_StNum.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.numericUpDown_StNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.numericUpDown_StNum.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // numericUpDown_TTL
            // 
            this.numericUpDown_TTL.Location = new System.Drawing.Point(84, 50);
            this.numericUpDown_TTL.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numericUpDown_TTL.Name = "numericUpDown_TTL";
            this.numericUpDown_TTL.Size = new System.Drawing.Size(84, 22);
            this.numericUpDown_TTL.TabIndex = 42;
            this.numericUpDown_TTL.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.numericUpDown_TTL.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.numericUpDown_TTL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.numericUpDown_TTL.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // numericUpDown_AppID
            // 
            this.numericUpDown_AppID.Location = new System.Drawing.Point(84, 20);
            this.numericUpDown_AppID.Maximum = new decimal(new int[] {
            16383,
            0,
            0,
            0});
            this.numericUpDown_AppID.Name = "numericUpDown_AppID";
            this.numericUpDown_AppID.Size = new System.Drawing.Size(84, 22);
            this.numericUpDown_AppID.TabIndex = 41;
            this.numericUpDown_AppID.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.numericUpDown_AppID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.numericUpDown_AppID.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // label1_EditData
            // 
            this.label1_EditData.AutoSize = true;
            this.label1_EditData.Location = new System.Drawing.Point(285, 165);
            this.label1_EditData.Name = "label1_EditData";
            this.label1_EditData.Size = new System.Drawing.Size(42, 17);
            this.label1_EditData.TabIndex = 38;
            this.label1_EditData.Text = "Data:";
            // 
            // button_EditData
            // 
            this.button_EditData.Location = new System.Drawing.Point(352, 165);
            this.button_EditData.Name = "button_EditData";
            this.button_EditData.Size = new System.Drawing.Size(90, 23);
            this.button_EditData.TabIndex = 37;
            this.button_EditData.Text = "Edit...";
            this.button_EditData.UseVisualStyleBackColor = true;
            this.button_EditData.Click += new System.EventHandler(this.button_EditData_Click);
            // 
            // comboBox_NdsCom
            // 
            this.comboBox_NdsCom.FormattingEnabled = true;
            this.comboBox_NdsCom.Items.AddRange(new object[] {
            "False",
            "True"});
            this.comboBox_NdsCom.Location = new System.Drawing.Point(352, 137);
            this.comboBox_NdsCom.Name = "comboBox_NdsCom";
            this.comboBox_NdsCom.Size = new System.Drawing.Size(90, 24);
            this.comboBox_NdsCom.TabIndex = 36;
            this.comboBox_NdsCom.Text = "False";
            this.comboBox_NdsCom.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedValueChanged);
            // 
            // comboBox_Test
            // 
            this.comboBox_Test.FormattingEnabled = true;
            this.comboBox_Test.Items.AddRange(new object[] {
            "False",
            "True"});
            this.comboBox_Test.Location = new System.Drawing.Point(352, 109);
            this.comboBox_Test.Name = "comboBox_Test";
            this.comboBox_Test.Size = new System.Drawing.Size(90, 24);
            this.comboBox_Test.TabIndex = 7;
            this.comboBox_Test.Text = "False";
            this.comboBox_Test.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedValueChanged);
            // 
            // label_NdsCom
            // 
            this.label_NdsCom.AutoSize = true;
            this.label_NdsCom.Location = new System.Drawing.Point(285, 137);
            this.label_NdsCom.Name = "label_NdsCom";
            this.label_NdsCom.Size = new System.Drawing.Size(65, 17);
            this.label_NdsCom.TabIndex = 32;
            this.label_NdsCom.Text = "NdsCom:";
            // 
            // label_Test
            // 
            this.label_Test.AutoSize = true;
            this.label_Test.Location = new System.Drawing.Point(285, 109);
            this.label_Test.Name = "label_Test";
            this.label_Test.Size = new System.Drawing.Size(40, 17);
            this.label_Test.TabIndex = 31;
            this.label_Test.Text = "Test:";
            // 
            // label_CfgRev
            // 
            this.label_CfgRev.AutoSize = true;
            this.label_CfgRev.Location = new System.Drawing.Point(285, 81);
            this.label_CfgRev.Name = "label_CfgRev";
            this.label_CfgRev.Size = new System.Drawing.Size(54, 17);
            this.label_CfgRev.TabIndex = 29;
            this.label_CfgRev.Text = "CfgRev";
            // 
            // label_StNum
            // 
            this.label_StNum.AutoSize = true;
            this.label_StNum.Location = new System.Drawing.Point(285, 23);
            this.label_StNum.Name = "label_StNum";
            this.label_StNum.Size = new System.Drawing.Size(54, 17);
            this.label_StNum.TabIndex = 26;
            this.label_StNum.Text = "StNum:";
            // 
            // label_SqNum
            // 
            this.label_SqNum.AutoSize = true;
            this.label_SqNum.Location = new System.Drawing.Point(285, 53);
            this.label_SqNum.Name = "label_SqNum";
            this.label_SqNum.Size = new System.Drawing.Size(54, 17);
            this.label_SqNum.TabIndex = 25;
            this.label_SqNum.Text = "SqNum";
            // 
            // textBox_Time
            // 
            this.textBox_Time.Location = new System.Drawing.Point(84, 162);
            this.textBox_Time.Name = "textBox_Time";
            this.textBox_Time.ReadOnly = true;
            this.textBox_Time.Size = new System.Drawing.Size(190, 22);
            this.textBox_Time.TabIndex = 24;
            this.textBox_Time.Text = "2014-05-30 15:01:22.760888";
            // 
            // label_Time
            // 
            this.label_Time.AutoSize = true;
            this.label_Time.Location = new System.Drawing.Point(14, 165);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(43, 17);
            this.label_Time.TabIndex = 23;
            this.label_Time.Text = "Time:";
            // 
            // textBox_GoCBRef
            // 
            this.textBox_GoCBRef.AcceptsReturn = true;
            this.textBox_GoCBRef.Location = new System.Drawing.Point(84, 134);
            this.textBox_GoCBRef.MaxLength = 65;
            this.textBox_GoCBRef.Name = "textBox_GoCBRef";
            this.textBox_GoCBRef.Size = new System.Drawing.Size(190, 22);
            this.textBox_GoCBRef.TabIndex = 20;
            this.textBox_GoCBRef.Text = "LLN0$DataSet1";
            this.textBox_GoCBRef.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox_GoCBRef.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.textBox_GoCBRef.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label_GoCBRef
            // 
            this.label_GoCBRef.AutoSize = true;
            this.label_GoCBRef.Location = new System.Drawing.Point(14, 136);
            this.label_GoCBRef.Name = "label_GoCBRef";
            this.label_GoCBRef.Size = new System.Drawing.Size(71, 17);
            this.label_GoCBRef.TabIndex = 19;
            this.label_GoCBRef.Text = "GoCBRef:";
            // 
            // textBox_DatSet
            // 
            this.textBox_DatSet.Location = new System.Drawing.Point(84, 106);
            this.textBox_DatSet.MaxLength = 65;
            this.textBox_DatSet.Name = "textBox_DatSet";
            this.textBox_DatSet.Size = new System.Drawing.Size(190, 22);
            this.textBox_DatSet.TabIndex = 18;
            this.textBox_DatSet.Text = "LLN0$DataSet1";
            this.textBox_DatSet.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox_DatSet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.textBox_DatSet.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label_DatSet
            // 
            this.label_DatSet.AutoSize = true;
            this.label_DatSet.Location = new System.Drawing.Point(14, 108);
            this.label_DatSet.Name = "label_DatSet";
            this.label_DatSet.Size = new System.Drawing.Size(55, 17);
            this.label_DatSet.TabIndex = 17;
            this.label_DatSet.Text = "DatSet:";
            // 
            // textBox_GoID
            // 
            this.textBox_GoID.Location = new System.Drawing.Point(84, 78);
            this.textBox_GoID.MaxLength = 65;
            this.textBox_GoID.Name = "textBox_GoID";
            this.textBox_GoID.Size = new System.Drawing.Size(190, 22);
            this.textBox_GoID.TabIndex = 16;
            this.textBox_GoID.Text = "LLN0$DataSet1";
            this.textBox_GoID.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox_GoID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.textBox_GoID.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label_GoID
            // 
            this.label_GoID.AutoSize = true;
            this.label_GoID.Location = new System.Drawing.Point(14, 80);
            this.label_GoID.Name = "label_GoID";
            this.label_GoID.Size = new System.Drawing.Size(44, 17);
            this.label_GoID.TabIndex = 14;
            this.label_GoID.Text = "GoID:";
            // 
            // label_AppID
            // 
            this.label_AppID.AutoSize = true;
            this.label_AppID.Location = new System.Drawing.Point(14, 22);
            this.label_AppID.Name = "label_AppID";
            this.label_AppID.Size = new System.Drawing.Size(50, 17);
            this.label_AppID.TabIndex = 7;
            this.label_AppID.Text = "AppID:";
            // 
            // label_TTL
            // 
            this.label_TTL.AutoSize = true;
            this.label_TTL.Location = new System.Drawing.Point(14, 52);
            this.label_TTL.Name = "label_TTL";
            this.label_TTL.Size = new System.Drawing.Size(38, 17);
            this.label_TTL.TabIndex = 4;
            this.label_TTL.Text = "TTL:";
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(470, 214);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(57, 23);
            this.button_Close.TabIndex = 45;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // groupBox_Ethernet
            // 
            this.groupBox_Ethernet.Controls.Add(this.maskedTextBox_dstMac);
            this.groupBox_Ethernet.Controls.Add(this.maskedTextBox_srcMac);
            this.groupBox_Ethernet.Controls.Add(this.numericUpDown_VlanVID);
            this.groupBox_Ethernet.Controls.Add(this.numericUpDown_VlanPrio);
            this.groupBox_Ethernet.Controls.Add(this.comboBox_VlanCFI);
            this.groupBox_Ethernet.Controls.Add(this.label3);
            this.groupBox_Ethernet.Controls.Add(this.checkBox_VlanTagEn);
            this.groupBox_Ethernet.Controls.Add(this.label1);
            this.groupBox_Ethernet.Controls.Add(this.label2);
            this.groupBox_Ethernet.Controls.Add(this.label_SrcMac);
            this.groupBox_Ethernet.Controls.Add(this.label_DstMac);
            this.groupBox_Ethernet.Location = new System.Drawing.Point(470, 21);
            this.groupBox_Ethernet.Name = "groupBox_Ethernet";
            this.groupBox_Ethernet.Size = new System.Drawing.Size(211, 184);
            this.groupBox_Ethernet.TabIndex = 41;
            this.groupBox_Ethernet.TabStop = false;
            this.groupBox_Ethernet.Text = "Ethernet:";
            // 
            // maskedTextBox_dstMac
            // 
            this.maskedTextBox_dstMac.Location = new System.Drawing.Point(74, 50);
            this.maskedTextBox_dstMac.Mask = "CC:CC:CC:CC:CC:CC";
            this.maskedTextBox_dstMac.Name = "maskedTextBox_dstMac";
            this.maskedTextBox_dstMac.Size = new System.Drawing.Size(131, 22);
            this.maskedTextBox_dstMac.TabIndex = 46;
            this.maskedTextBox_dstMac.Text = "010CCD0100FF";
            this.maskedTextBox_dstMac.Enter += new System.EventHandler(this.maskedTextBox_Enter);
            this.maskedTextBox_dstMac.KeyDown += new System.Windows.Forms.KeyEventHandler(this.maskedTextBox_KeyDown);
            this.maskedTextBox_dstMac.Leave += new System.EventHandler(this.maskedTextBox_Leave);
            // 
            // maskedTextBox_srcMac
            // 
            this.maskedTextBox_srcMac.Location = new System.Drawing.Point(74, 22);
            this.maskedTextBox_srcMac.Mask = "CC:CC:CC:CC:CC:CC";
            this.maskedTextBox_srcMac.Name = "maskedTextBox_srcMac";
            this.maskedTextBox_srcMac.Size = new System.Drawing.Size(131, 22);
            this.maskedTextBox_srcMac.TabIndex = 47;
            this.maskedTextBox_srcMac.Text = "000000000000";
            this.maskedTextBox_srcMac.Enter += new System.EventHandler(this.maskedTextBox_Enter);
            this.maskedTextBox_srcMac.KeyDown += new System.Windows.Forms.KeyEventHandler(this.maskedTextBox_KeyDown);
            this.maskedTextBox_srcMac.Leave += new System.EventHandler(this.maskedTextBox_Leave);
            // 
            // numericUpDown_VlanVID
            // 
            this.numericUpDown_VlanVID.Enabled = false;
            this.numericUpDown_VlanVID.Location = new System.Drawing.Point(74, 157);
            this.numericUpDown_VlanVID.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
            this.numericUpDown_VlanVID.Name = "numericUpDown_VlanVID";
            this.numericUpDown_VlanVID.Size = new System.Drawing.Size(84, 22);
            this.numericUpDown_VlanVID.TabIndex = 45;
            this.numericUpDown_VlanVID.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.numericUpDown_VlanVID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.numericUpDown_VlanVID.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // numericUpDown_VlanPrio
            // 
            this.numericUpDown_VlanPrio.Enabled = false;
            this.numericUpDown_VlanPrio.Location = new System.Drawing.Point(74, 100);
            this.numericUpDown_VlanPrio.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDown_VlanPrio.Name = "numericUpDown_VlanPrio";
            this.numericUpDown_VlanPrio.Size = new System.Drawing.Size(84, 22);
            this.numericUpDown_VlanPrio.TabIndex = 44;
            this.numericUpDown_VlanPrio.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown_VlanPrio.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.numericUpDown_VlanPrio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown_KeyPress);
            this.numericUpDown_VlanPrio.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // comboBox_VlanCFI
            // 
            this.comboBox_VlanCFI.Enabled = false;
            this.comboBox_VlanCFI.FormattingEnabled = true;
            this.comboBox_VlanCFI.Items.AddRange(new object[] {
            "0",
            "1"});
            this.comboBox_VlanCFI.Location = new System.Drawing.Point(74, 127);
            this.comboBox_VlanCFI.Name = "comboBox_VlanCFI";
            this.comboBox_VlanCFI.Size = new System.Drawing.Size(84, 24);
            this.comboBox_VlanCFI.TabIndex = 6;
            this.comboBox_VlanCFI.Text = "0";
            this.comboBox_VlanCFI.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "VID:";
            // 
            // checkBox_VlanTagEn
            // 
            this.checkBox_VlanTagEn.AutoSize = true;
            this.checkBox_VlanTagEn.Location = new System.Drawing.Point(17, 78);
            this.checkBox_VlanTagEn.Name = "checkBox_VlanTagEn";
            this.checkBox_VlanTagEn.Size = new System.Drawing.Size(151, 21);
            this.checkBox_VlanTagEn.TabIndex = 9;
            this.checkBox_VlanTagEn.Text = "VLAN Tag Enabled";
            this.checkBox_VlanTagEn.UseVisualStyleBackColor = true;
            this.checkBox_VlanTagEn.CheckedChanged += new System.EventHandler(this.checkBox_VlanTagEn_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Priority:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "CFI:";
            // 
            // label_SrcMac
            // 
            this.label_SrcMac.AutoSize = true;
            this.label_SrcMac.Location = new System.Drawing.Point(14, 24);
            this.label_SrcMac.Name = "label_SrcMac";
            this.label_SrcMac.Size = new System.Drawing.Size(59, 17);
            this.label_SrcMac.TabIndex = 2;
            this.label_SrcMac.Text = "SrcMac:";
            // 
            // label_DstMac
            // 
            this.label_DstMac.AutoSize = true;
            this.label_DstMac.Location = new System.Drawing.Point(14, 52);
            this.label_DstMac.Name = "label_DstMac";
            this.label_DstMac.Size = new System.Drawing.Size(59, 17);
            this.label_DstMac.TabIndex = 4;
            this.label_DstMac.Text = "DstMac:";
            // 
            // button_Run
            // 
            this.button_Run.Location = new System.Drawing.Point(613, 214);
            this.button_Run.Name = "button_Run";
            this.button_Run.Size = new System.Drawing.Size(68, 23);
            this.button_Run.TabIndex = 43;
            this.button_Run.Text = "Run";
            this.button_Run.UseVisualStyleBackColor = true;
            this.button_Run.Click += new System.EventHandler(this.button_Run_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 254);
            this.panel1.TabIndex = 47;
            // 
            // label_GoosesCnt
            // 
            this.label_GoosesCnt.AutoSize = true;
            this.label_GoosesCnt.Location = new System.Drawing.Point(15, 191);
            this.label_GoosesCnt.Name = "label_GoosesCnt";
            this.label_GoosesCnt.Size = new System.Drawing.Size(62, 17);
            this.label_GoosesCnt.TabIndex = 48;
            this.label_GoosesCnt.Text = "SentCnt:";
            // 
            // textBox_GoosesCnt
            // 
            this.textBox_GoosesCnt.Location = new System.Drawing.Point(84, 188);
            this.textBox_GoosesCnt.Name = "textBox_GoosesCnt";
            this.textBox_GoosesCnt.ReadOnly = true;
            this.textBox_GoosesCnt.Size = new System.Drawing.Size(190, 22);
            this.textBox_GoosesCnt.TabIndex = 49;
            this.textBox_GoosesCnt.Text = "0";
            // 
            // GooseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "GooseControl";
            this.Size = new System.Drawing.Size(704, 254);
            this.groupBox1.ResumeLayout(false);
            this.groupBox_Parameters.ResumeLayout(false);
            this.groupBox_Parameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CfgRev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_SqNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_StNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TTL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AppID)).EndInit();
            this.groupBox_Ethernet.ResumeLayout(false);
            this.groupBox_Ethernet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VlanVID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VlanPrio)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox_Parameters;       
        private System.Windows.Forms.NumericUpDown numericUpDown_CfgRev;
        private System.Windows.Forms.NumericUpDown numericUpDown_SqNum;
        private System.Windows.Forms.NumericUpDown numericUpDown_StNum;
        private System.Windows.Forms.NumericUpDown numericUpDown_TTL;
        private System.Windows.Forms.NumericUpDown numericUpDown_AppID;
        private System.Windows.Forms.Label label1_EditData;
        private System.Windows.Forms.Button button_EditData;
        private System.Windows.Forms.ComboBox comboBox_NdsCom;
        private System.Windows.Forms.ComboBox comboBox_Test;
        private System.Windows.Forms.Label label_NdsCom;
        private System.Windows.Forms.Label label_Test;
        private System.Windows.Forms.Label label_CfgRev;
        private System.Windows.Forms.Label label_StNum;
        private System.Windows.Forms.Label label_SqNum;
        private System.Windows.Forms.TextBox textBox_Time;
        private System.Windows.Forms.Label label_Time;
        private System.Windows.Forms.TextBox textBox_GoCBRef;
        private System.Windows.Forms.Label label_GoCBRef;
        private System.Windows.Forms.TextBox textBox_DatSet;
        private System.Windows.Forms.Label label_DatSet;
        private System.Windows.Forms.TextBox textBox_GoID;
        private System.Windows.Forms.Label label_GoID;
        private System.Windows.Forms.Label label_AppID;
        private System.Windows.Forms.Label label_TTL;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.GroupBox groupBox_Ethernet;
        private System.Windows.Forms.NumericUpDown numericUpDown_VlanVID;
        private System.Windows.Forms.NumericUpDown numericUpDown_VlanPrio;
        private System.Windows.Forms.ComboBox comboBox_VlanCFI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_VlanTagEn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_SrcMac;
        private System.Windows.Forms.Label label_DstMac;
        private System.Windows.Forms.Button button_Run;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_dstMac;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_srcMac;
        private System.Windows.Forms.Button button_SendOnce;
        private System.Windows.Forms.CheckBox checkBox_SqNumLock;
        private System.Windows.Forms.CheckBox checkBox_StNumLock;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox textBox_GoosesCnt;
        private System.Windows.Forms.Label label_GoosesCnt;

    }
}
