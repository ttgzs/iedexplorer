using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using PcapDotNet.Core;
using PcapDotNet.Base;
using PcapDotNet.Core.Extensions;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Transport;
using System.Collections;
using GOOSE_ASN1_Model;
using System.IO;
using org.bn;

namespace IEDExplorer
{
    public partial class GooseControl : UserControl
    {
        private Thread _senderThread;
        private bool _run;
        PacketDevice _netDevice;
        GOOSE Goose;
        GoosePdu Pdu = new GoosePdu();
        bool dataChange = false;        
        public GooseDataEdit gooseDataEdit = null;
        private object _monitor = new object();
        int _gooseCnt = 0;        
        
        public List<Data> dataList = new List<Data>();
        public List<SeqData> seqData = new List<SeqData>();
        public Hashtable gooseParameters = new Hashtable(); 

        public  PacketDevice netDevice 
        { 
            get
            {
                return _netDevice;
            }
            set
            {
                _netDevice = value;
            }
        }

        class textBoxData
        {
            public Boolean RestoreOnEnterText { get; set; }
            public string TextOnEnter { get; set; }

            public textBoxData()
            {
                this.RestoreOnEnterText = true;
                this.TextOnEnter = "";
            }
        }

        class numUpDownData
        {
            public Boolean RestoreOnEnterVal { get; set; }
            public Decimal ValOnEnter { get; set; }
            public Boolean RestoreValueChangedHandler { get; set; }

            public numUpDownData()
            {
                this.RestoreOnEnterVal = true;
                this.ValOnEnter = 0;
                this.RestoreValueChangedHandler = false;
            }
        }

        public GooseControl(string name, string mac, PacketDevice selectedDevice)
        {
            InitializeComponent();
            this.groupBox1.Text = name;
            this.Name = name;            
            this._netDevice = selectedDevice;            

            this.numericUpDown_AppID.Tag = new numUpDownData();
            this.numericUpDown_TTL.Tag = new numUpDownData();
            this.numericUpDown_StNum.Tag = new numUpDownData();
            this.numericUpDown_SqNum.Tag = new numUpDownData();
            this.numericUpDown_CfgRev.Tag = new numUpDownData();

            this.numericUpDown_VlanPrio.Tag = new numUpDownData();
            this.numericUpDown_VlanVID.Tag = new numUpDownData();

            this.textBox_GoID.Tag = new textBoxData();
            this.textBox_DatSet.Tag = new textBoxData();
            this.textBox_GoCBRef.Tag = new textBoxData();
            this.maskedTextBox_srcMac.Tag = new textBoxData();
            this.maskedTextBox_dstMac.Tag = new textBoxData();           
            

            toolTip.SetToolTip(checkBox_SqNumLock, "Lock SqNum");
            toolTip.SetToolTip(checkBox_StNumLock, "Lock StNum");

            gooseParameters.Add("numericUpDown_AppID", "0");
            gooseParameters.Add("numericUpDown_TTL", "2048");
            gooseParameters.Add("textBox_GoID", "LLN0$DataSet1");            
            gooseParameters.Add("textBox_DatSet", "LLN0$DataSet1");
            gooseParameters.Add("textBox_GoCBRef", "LLN0$DataSet1");
            gooseParameters.Add("textBox_Time", "2014-05-30 15:01:22.76088");
            gooseParameters.Add("numericUpDown_StNum", "1");
            gooseParameters.Add("numericUpDown_SqNum", "0");
            gooseParameters.Add("numericUpDown_CfgRev", "0");
            gooseParameters.Add("comboBox_Test", "0");
            gooseParameters.Add("comboBox_NdsCom", "0");
            gooseParameters.Add("maskedTextBox_srcMac", mac);
            gooseParameters.Add("maskedTextBox_dstMac", "01:0C:CD:01:00:FF");
            gooseParameters.Add("checkBox_VlanTagEn", "False");
            gooseParameters.Add("numericUpDown_VlanPrio", "4");
            gooseParameters.Add("comboBox_VlanCFI", "0");
            gooseParameters.Add("numericUpDown_VlanVID", "0");

            updateComponents();
        }

        public void updateComponents()
        {
            this.numericUpDown_AppID.Value = Convert.ToDecimal(gooseParameters["numericUpDown_AppID"]);
            this.numericUpDown_TTL.Value = Convert.ToDecimal(gooseParameters["numericUpDown_TTL"]);
            this.textBox_GoID.Text = (String)gooseParameters["textBox_GoID"];
            this.textBox_DatSet.Text = (String)gooseParameters["textBox_DatSet"];
            this.textBox_GoCBRef.Text = (String)gooseParameters["textBox_GoCBRef"];
            this.textBox_Time.Text = (String)gooseParameters["textBox_Time"];
            this.numericUpDown_StNum.Value = Convert.ToDecimal(gooseParameters["numericUpDown_StNum"]);
            this.numericUpDown_SqNum.Value = Convert.ToDecimal(gooseParameters["numericUpDown_SqNum"]);
            this.numericUpDown_CfgRev.Value = Convert.ToDecimal(gooseParameters["numericUpDown_CfgRev"]);
            this.comboBox_Test.SelectedIndex = Convert.ToInt32(gooseParameters["comboBox_Test"]);
            this.comboBox_NdsCom.SelectedIndex = Convert.ToInt32(gooseParameters["comboBox_NdsCom"]);
            this.maskedTextBox_srcMac.Text = (String)gooseParameters["maskedTextBox_srcMac"];
            this.maskedTextBox_dstMac.Text = (String)gooseParameters["maskedTextBox_dstMac"];
            this.checkBox_VlanTagEn.Checked = ((String)gooseParameters["checkBox_VlanTagEn"] == "True") ? true : false;
            this.numericUpDown_VlanPrio.Value = Convert.ToDecimal(gooseParameters["numericUpDown_VlanPrio"]);
            this.comboBox_VlanCFI.SelectedIndex = Convert.ToInt32(gooseParameters["comboBox_VlanCFI"]);
            this.numericUpDown_VlanVID.Value = Convert.ToDecimal(gooseParameters["numericUpDown_VlanVID"]);
            this.textBox_GoosesCnt.Text = this._gooseCnt.ToString();
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();

            this.Parent.Controls.RemoveByKey((sender as Button).Parent.Text);
        }

        public void Close()
        {
            if (_run && _senderThread != null)
            {
                _run = false;
                _senderThread.Abort();
                _senderThread = null;                
            }

            if (gooseDataEdit != null)
                gooseDataEdit.Close(); 
        }

        private void WorkerThreadProc(object obj)
        {
            int resendTime = 1;

            //lock (_monitor)
            //{
                while (true)
                {
                    // Open the device                
                    if (dataChange)
                    {
                        this.Invoke(new MethodInvoker(delegate() 
                                                      {
                                                          lock (gooseParameters)
                                                          {
                                                              if (!this.checkBox_StNumLock.Checked)
                                                              {
                                                                  this.numericUpDown_StNum.Value++;
                                                                  gooseParameters["numericUpDown_StNum"] = this.numericUpDown_StNum.Value.ToString();
                                                              }

                                                              if (!this.checkBox_SqNumLock.Checked)
                                                              {
                                                                  this.numericUpDown_SqNum.Value = 1;
                                                                  gooseParameters["numericUpDown_SqNum"] = this.numericUpDown_SqNum.Value.ToString();
                                                              }                                     
                                                          }
                                                      }));                       
                        dataChange = false;
                        resendTime = 1;
                    }

                    this.Invoke(new MethodInvoker(delegate()
                                                  {
                                                      DateTime timeNow = DateTime.UtcNow;
                                                      this.textBox_Time.Text = timeNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fffff");
                                                      lock(gooseParameters)
                                                          gooseParameters["textBox_Time"] = timeNow.ToString("yyyy-MM-dd HH:mm:ss.fffff");
                                                  }));

                    using (PacketCommunicator communicator = _netDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                        communicator.SendPacket(BuildGoosePacket());

                    this.Invoke(new MethodInvoker(delegate()
                    {
                        lock (gooseParameters)
                        {
                            if (!this.checkBox_SqNumLock.Checked)
                            {
                                this.numericUpDown_SqNum.Value++;
                                gooseParameters["numericUpDown_SqNum"] = this.numericUpDown_SqNum.Value.ToString();                               
                            }
                            
                            this._gooseCnt++;
                            this.textBox_GoosesCnt.Text = this._gooseCnt.ToString();
                        }
                    }));                       

                    if (resendTime < (int)this.numericUpDown_TTL.Value)
                        resendTime += resendTime * 2;

                    lock (_monitor)
                        Monitor.Wait(_monitor, TimeSpan.FromMilliseconds((resendTime / 2 >= (int)this.numericUpDown_TTL.Value) ? (int)this.numericUpDown_TTL.Value : resendTime / 2));                    
                //}
            }
        }

        private Packet BuildGoosePacket()
        {
            EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    Source = new MacAddress((String)gooseParameters["maskedTextBox_srcMac"]),
                    Destination = new MacAddress((String)gooseParameters["maskedTextBox_dstMac"]),
                    EtherType = (EthernetType)0x88b8,                    
                };

            VLanTaggedFrameLayer vlanLayer =
                new VLanTaggedFrameLayer
                {
                    PriorityCodePoint = ClassOfService.Background,
                    CanonicalFormatIndicator = false,
                    VLanIdentifier = 50,
                    EtherType = (EthernetType)0x88b8,

                };

            StringToDataConverter StringToUtc = new StringToDataConverter();            
            UtcTime T = new UtcTime(StringToUtc.ConvertToTimeEntry(gooseParameters["textBox_Time"].ToString()));

            Pdu.GoID = (String)gooseParameters["textBox_GoID"];
            Pdu.GocbRef = (String)gooseParameters["textBox_GoCBRef"];
            Pdu.ConfRev = (long)Convert.ToDecimal(gooseParameters["numericUpDown_CfgRev"]);
            Pdu.DatSet = (String)gooseParameters["textBox_DatSet"]; ;
            Pdu.NdsCom = ((String)gooseParameters["comboBox_NdsCom"] == "1") ? true : false;
            Pdu.SqNum = (long)Convert.ToDecimal(gooseParameters["numericUpDown_SqNum"]);
            Pdu.StNum = (long)Convert.ToDecimal(gooseParameters["numericUpDown_StNum"]);
            Pdu.TimeAllowedtoLive = (long)Convert.ToDecimal(gooseParameters["numericUpDown_TTL"]);
            Pdu.Simulation = ((String)gooseParameters["comboBox_Test"] == "1") ? true : false ;
            Pdu.T = T;
            Pdu.AllData = dataList;
            Pdu.NumDatSetEntries = dataList.Count;
            
            Goose = new GOOSE(); 
            Goose.selectGoosePdu(Pdu);

            MemoryStream ms = new MemoryStream();
            IEncoder encoder = CoderFactory.getInstance().newEncoder("BER");
            encoder.encode<GOOSE>(Goose, ms);

            byte[] rawGooseData = new byte[ms.Length + 8];                        
            // Set APPID
            rawGooseData[0] = (byte)(((long)Convert.ToDecimal(gooseParameters["numericUpDown_AppID"]) & 0xFF00) >> 8);
            rawGooseData[1] = (byte)(((long)Convert.ToDecimal(gooseParameters["numericUpDown_AppID"])) & 0xFF);
            // Set Length
            rawGooseData[2] = (byte)(((ms.Length + 8) & 0xFF00) >> 8);
            rawGooseData[3] = (byte)((ms.Length + 8) & 0xFF);
            // Set Reserved 1 and Reserved 2
            rawGooseData[4] = 0;
            rawGooseData[5] = 0;
            rawGooseData[6] = 0;
            rawGooseData[7] = 0;

            ms.Position = 0;
            ms.Read(rawGooseData, 8, (int)ms.Length);

            PayloadLayer payloadLayer =
            new PayloadLayer
            {
                Data = new Datagram(rawGooseData),
            };

            PacketBuilder builder;

            if (Convert.ToString(gooseParameters["checkBox_VlanTagEn"]) == "True")
            {
                builder = new PacketBuilder(ethernetLayer, vlanLayer, payloadLayer);
                ethernetLayer.EtherType = EthernetType.None;                    
            }
            else          
                builder = new PacketBuilder(ethernetLayer, payloadLayer);
            
            return builder.Build(DateTime.Now);
        }

        public void StopAndLockSending()
        {
            if (button_Run.Text == "Stop")
                button_Run_Click(button_Run, null);

            button_Run.Enabled = false;
            button_SendOnce.Enabled = false;
        }

        public void UnlockSending()
        {
            button_Run.Enabled = true;
            button_SendOnce.Enabled = true;
        }

        private void button_Run_Click(object sender, EventArgs e)
        {
            if (!_run && _senderThread == null)
            {
                _run = true;
                _senderThread = new Thread(new ParameterizedThreadStart(WorkerThreadProc));
                _senderThread.Start(this);
                (sender as Button).Text = "Stop";

                if (gooseDataEdit == null)
                    this.gooseDataEdit = new GooseDataEdit(this.Name, dataList, seqData, Data_ValueChanged);

                button_SendOnce.Enabled = false;

                this._gooseCnt = 0;
                this.textBox_GoosesCnt.Text = this._gooseCnt.ToString();

                gooseDataEdit.DataEditModeOnly(true);

            }
            else
            {                
                _run = false;
                _senderThread.Abort();
                _senderThread = null;
                gooseDataEdit.DataEditModeOnly(false);
                (sender as Button).Text = "Run";

                button_SendOnce.Enabled = true;
            }                 
        }
 
        void Data_ValueChanged(object sender, EventArgs e)
        {
            lock (_monitor)
            {
                dataChange = true;
                Monitor.Pulse(_monitor);
            }
        }


        private void button_EditData_Click(object sender, EventArgs e)
        {
            if(gooseDataEdit == null)            
                this.gooseDataEdit = Env.getEnv().winMgr.AddGooseDataEdit(this.Name, dataList, seqData, Data_ValueChanged);
                    //new GooseDataEdit(this.Name, dataList, seqData, Data_ValueChanged);

            gooseDataEdit.Show();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            if (((sender as TextBox).Tag != null) && ((sender as TextBox).Tag is textBoxData) && (((sender as TextBox).Tag as textBoxData).RestoreOnEnterText))
                (sender as TextBox).Text = ((sender as TextBox).Tag as textBoxData).TextOnEnter;
            else
                if (gooseParameters.ContainsKey((sender as TextBox).Name))
                    gooseParameters[(sender as TextBox).Name] = (sender as TextBox).Text;
                else
                    MessageBox.Show("Destination for modified parameter: " + (sender as TextBox).Name + " not found !");                                 
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            if (((sender as TextBox).Tag != null) && ((sender as TextBox).Tag is textBoxData))
            {
                ((sender as TextBox).Tag as textBoxData).RestoreOnEnterText = true;
                ((sender as TextBox).Tag as textBoxData).TextOnEnter = (sender as TextBox).Text;
            }                        
        }   

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            { 
                if (((sender as TextBox).Tag != null) && ((sender as TextBox).Tag is textBoxData))
                    ((sender as TextBox).Tag as textBoxData).RestoreOnEnterText = true;
             
                groupBox_Ethernet.Focus();
                e.Handled = true;
            }
            else if (e.KeyChar == (char)13)
            {
                if (((sender as TextBox).Tag != null) && ((sender as TextBox).Tag is textBoxData))
                    ((sender as TextBox).Tag as textBoxData).RestoreOnEnterText = false;
                                 
                groupBox_Ethernet.Focus();
                e.Handled = true;
            }
        }

        private void numericUpDown_Enter(object sender, EventArgs e)
        {
            (sender as NumericUpDown).Focus();
            (sender as NumericUpDown).Select();             

            if (((sender as NumericUpDown).Tag != null) && ((sender as NumericUpDown).Tag is numUpDownData))
            {
                ((sender as NumericUpDown).Tag as numUpDownData).RestoreOnEnterVal = true;
                ((sender as NumericUpDown).Tag as numUpDownData).ValOnEnter = (sender as NumericUpDown).Value;
            }            
        }

        private void numericUpDown_Leave(object sender, EventArgs e)
        {
            if (((sender as NumericUpDown).Tag != null) && ((sender as NumericUpDown).Tag is numUpDownData) && (((sender as NumericUpDown).Tag as numUpDownData).RestoreOnEnterVal))
                (sender as NumericUpDown).Value = ((sender as NumericUpDown).Tag as numUpDownData).ValOnEnter;
            else
                if (gooseParameters.ContainsKey((sender as NumericUpDown).Name))
                    gooseParameters[(sender as NumericUpDown).Name] = (sender as NumericUpDown).Value.ToString();
                else
                    MessageBox.Show("Destination for modified parameter: " + (sender as NumericUpDown).Name + " not found !");

            if (((sender as NumericUpDown).Tag as numUpDownData).RestoreValueChangedHandler)
            {
                ((sender as NumericUpDown).Tag as numUpDownData).RestoreValueChangedHandler = false;
                (sender as NumericUpDown).ValueChanged += new EventHandler(numericUpDown_ValueChanged);
            }            
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (((sender as NumericUpDown).Tag != null) && ((sender as NumericUpDown).Tag is numUpDownData))
                ((sender as NumericUpDown).Tag as numUpDownData).RestoreOnEnterVal = false;
            
            (sender as NumericUpDown).ValueChanged -= new EventHandler(numericUpDown_ValueChanged);
            
            groupBox_Ethernet.Focus();
        }

        private void numericUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                if (((sender as NumericUpDown).Tag != null) && ((sender as NumericUpDown).Tag is numUpDownData))
                    ((sender as NumericUpDown).Tag as numUpDownData).RestoreOnEnterVal = true;
                
                ((sender as NumericUpDown).Tag as numUpDownData).RestoreValueChangedHandler = true;
                    (sender as NumericUpDown).ValueChanged -= new EventHandler(numericUpDown_ValueChanged);
                                
                groupBox_Ethernet.Focus();
                e.Handled = true;
                
            }
            else if (e.KeyChar == (char)13)
            {
                if (((sender as NumericUpDown).Tag != null) && ((sender as NumericUpDown).Tag is numUpDownData))
                    ((sender as NumericUpDown).Tag as numUpDownData).RestoreOnEnterVal = false;

                groupBox_Ethernet.Focus();
                e.Handled = true;
            }
        }

        private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                if (((sender as MaskedTextBox).Tag != null) && ((sender as MaskedTextBox).Tag is textBoxData))
                    ((sender as MaskedTextBox).Tag as textBoxData).RestoreOnEnterText = true;

                groupBox_Ethernet.Focus();                
            }
            else if (e.KeyData == Keys.Enter)
            {
                if (((sender as MaskedTextBox).Tag != null) && ((sender as MaskedTextBox).Tag is textBoxData))
                    ((sender as MaskedTextBox).Tag as textBoxData).RestoreOnEnterText = false;

                groupBox_Ethernet.Focus();                
            }
            else if (((e.KeyData >= Keys.D0) && (e.KeyData <= Keys.D9))           || 
                     ((e.KeyData >= Keys.A) && (e.KeyData <= Keys.F))             || 
                     ((e.KeyData >= Keys.NumPad0) && (e.KeyData <= Keys.NumPad9)) ||
                     (e.KeyData == Keys.Left || e.KeyData == Keys.Right))
            {

            }
            else if (e.Shift == true)
            {
                if (!((e.KeyCode >= Keys.A) && (e.KeyCode <= Keys.F)))                
                    e.SuppressKeyPress = true;                
            }
            else
                e.SuppressKeyPress = true;
        }

        private void maskedTextBox_Enter(object sender, EventArgs e)
        {
            if (((sender as MaskedTextBox).Tag != null) && ((sender as MaskedTextBox).Tag is textBoxData))
            {
                ((sender as MaskedTextBox).Tag as textBoxData).RestoreOnEnterText = true;
                ((sender as MaskedTextBox).Tag as textBoxData).TextOnEnter = (sender as MaskedTextBox).Text;
            }  
        }

        private void maskedTextBox_Leave(object sender, EventArgs e)
        {
            Regex regexMacAddress = new Regex(@"^[0-9a-fA-F]{2}(((:[0-9a-fA-F]{2}){5})|((:[0-9a-fA-F]{2}){5}))$");

            if (((sender as MaskedTextBox).Tag != null) && ((sender as MaskedTextBox).Tag is textBoxData) && (((sender as MaskedTextBox).Tag as textBoxData).RestoreOnEnterText || !regexMacAddress.IsMatch((sender as MaskedTextBox).Text)))
                (sender as MaskedTextBox).Text = ((sender as MaskedTextBox).Tag as textBoxData).TextOnEnter;
            else
            {
                (sender as MaskedTextBox).Text = (sender as MaskedTextBox).Text.ToUpper();

                if (gooseParameters.ContainsKey((sender as MaskedTextBox).Name))
                    gooseParameters[(sender as MaskedTextBox).Name] = (sender as MaskedTextBox).Text;
                else
                    MessageBox.Show("Destination for modified parameter: " + (sender as MaskedTextBox).Name + " not found !");
            }
        }

        private void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (gooseParameters.ContainsKey((sender as ComboBox).Name))
                gooseParameters[(sender as ComboBox).Name] = (sender as ComboBox).SelectedIndex.ToString();
            else
                MessageBox.Show("Destination for modified parameter: " + (sender as ComboBox).Name + " not found !");            
        }

        private void button_SendOnce_Click(object sender, EventArgs e)
        {
            this.textBox_Time.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff");
            gooseParameters["textBox_Time"] = this.textBox_Time.Text;

            using (PacketCommunicator communicator = _netDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                communicator.SendPacket(BuildGoosePacket());
        }

        private void checkBox_VlanTagEn_CheckedChanged(object sender, EventArgs e)
        {
            lock(gooseParameters)
                if (checkBox_VlanTagEn.Checked)
                {
                    gooseParameters["checkBox_VlanTagEn"] = "True";
                    comboBox_VlanCFI.Enabled = true;
                    numericUpDown_VlanPrio.Enabled = true;
                    numericUpDown_VlanVID.Enabled = true;
                }
                else
                {
                    gooseParameters["checkBox_VlanTagEn"] = "False";
                    comboBox_VlanCFI.Enabled = false;
                    numericUpDown_VlanPrio.Enabled = false;
                    numericUpDown_VlanVID.Enabled = false;
                }
        }
    }
}
