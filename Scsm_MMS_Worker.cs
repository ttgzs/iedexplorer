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

using System;
using System.IO;
using org.bn.attributes;
using org.bn.attributes.constraints;
using org.bn.coders;
using org.bn.types;
using org.bn;
using MMS_ASN1_Model;
using System.Threading;
using System.Windows.Forms;

namespace IEDExplorer
{
    class Scsm_MMS_Worker
    {
        private Thread _workerThread;
        private bool _run;
        private string _hostname;
        private int _port;
        private Env _env;
        WaitHandle[] _waitHandles = new WaitHandle[5];
        public Iec61850State iecs;
        Logger logger = Logger.getLogger();

        public Scsm_MMS_Worker(string hostname, int port, Env env)
        {
            _hostname = hostname;
            _port = port;
            _env = env;
        }

        public Scsm_MMS_Worker(Env env)
        {
            _env = env;
        }

        public int Start(string hostname, int port)
        {
            _hostname = hostname;
            _port = port;
            return Start();
        }

        public int Start()
        {
            //// Run Thread
            if (!_run && _workerThread == null)
            {
                _run = true;
                _workerThread = new Thread(new ParameterizedThreadStart(WorkerThreadProc));
                logger.LogInfo(String.Format("Starting new communication, hostname = {0}, port = {1}.", _hostname, _port));
                _workerThread.Start(this);
            }
            else
                MessageBox.Show("Cannot start, communication already running!", "Info");
            return 0;
        }

        public void Stop()
        {
            if (_workerThread != null)
            {
                //_run = false;
                (_waitHandles[3] as ManualResetEvent).Set();
                //_workerThread.Join();
                _workerThread = null;
                logger.LogInfo(String.Format("Communication to hostname = {0}, port = {1} stopped.", _hostname, _port));
            }
        }

        public void SendCommand(Iec61850lStateEnum c)
        {
            this.iecs.istate = c;
        }

        private void WorkerThreadProc(object obj)
        {
            Scsm_MMS_Worker self = (Scsm_MMS_Worker)obj;

            iecs = new Iec61850State();
            iecs.hostname = self._hostname;
            iecs.port = self._port;
            iecs.logger = Logger.getLogger();
            _waitHandles[0] = iecs.connectDone;
            _waitHandles[1] = iecs.receiveDone;
            _waitHandles[2] = iecs.sendDone;
            _waitHandles[3] = new ManualResetEvent(false);   // end thread
            _waitHandles[4] = iecs.sendQueueWritten;
            //DateTime tout = null;

            CommAddress ad = new CommAddress();

            while (self._run)
            {
                switch (iecs.tstate)
                {
                    case TcpProtocolState.TCP_STATE_START:
                        iecs.logger.LogInfo("[TCP_STATE_START]");
                        iecs.kstate = TpktState.TPKT_RECEIVE_START;
                        iecs.ostate = OsiProtocolState.OSI_STATE_START;
                        iecs.istate = Iec61850lStateEnum.IEC61850_STATE_START;
                        TcpRw.StartClient(iecs);
                        break;
                    case TcpProtocolState.TCP_STATE_SHUTDOWN:
                        iecs.logger.LogInfo("[TCP_STATE_SHUTDOWN]");
                        Thread.Sleep(5000);
                        iecs.tstate = TcpProtocolState.TCP_STATE_START;
                        break;
                    case TcpProtocolState.TCP_CONNECTED:
                        switch (iecs.ostate)
                        {
                            case OsiProtocolState.OSI_CONNECT_COTP:
                                iecs.logger.LogInfo("[OSI_CONNECT_COTP]");
                                iecs.osi.SendCOTPSessionInit(iecs);
                                iecs.ostate = OsiProtocolState.OSI_CONNECT_COTP_WAIT;
                                break;
                            case OsiProtocolState.OSI_CONNECT_PRES:
                                iecs.logger.LogInfo("[OSI_CONNECT_PRES]");
                                iecs.osi.SendPresentationInit(iecs);
                                iecs.ostate = OsiProtocolState.OSI_CONNECT_PRES_WAIT;
                                break;
                            case OsiProtocolState.OSI_CONNECTED:
                                //iecs.logger.LogInfo("[OSI_CONNECTED]");
                                switch (iecs.istate)
                                {
                                    case Iec61850lStateEnum.IEC61850_STATE_START:
                                        if (iecs.dataModel.ied.Identify) {
                                            iecs.logger.LogInfo("[IEC61850_STATE_START] (Send IdentifyRequest)");
                                            iecs.mms.SendIdentify(iecs);
                                            iecs.istate = Iec61850lStateEnum.IEC61850_CONNECT_MMS_WAIT;
                                        }
                                        else
                                        {
                                            iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_DOMAIN;
                                        }
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_NAMELIST_DOMAIN:
                                        iecs.logger.LogDebug("[IEC61850_READ_NAMELIST_DOMAIN]");
                                        iecs.mms.SendGetNameListDomain(iecs);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_DOMAIN_WAIT;
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_NAMELIST_VAR:
                                        iecs.logger.LogDebug("[IEC61850_READ_NAMELIST_VAR]");
                                        iecs.mms.SendGetNameListVariables(iecs);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_VAR_WAIT;
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_ACCESSAT_VAR:
                                        iecs.logger.LogDebug("[IEC61850_READ_ACCESSAT_VAR]");
                                        iecs.mms.SendGetVariableAccessAttributes(iecs);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_ACCESSAT_VAR_WAIT;
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_MODEL_DATA:
                                        iecs.logger.LogDebug("[IEC61850_READ_MODEL_DATA]");
                                        CommAddress adr = new CommAddress();
                                        adr.Domain = null;
                                        adr.Variable = null;
                                        adr.owner = null;
                                        NodeBase[] data = new NodeBase[1];
                                        data[0] = iecs.dataModel.ied.GetActualChildNode().GetActualChildNode().GetActualChildNode(); //.GetActualChildNode();
                                        WriteQueueElement wqel = new WriteQueueElement(data, adr, ActionRequested.Read);
                                        iecs.mms.SendRead(iecs, wqel);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_MODEL_DATA_WAIT;
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST:
                                        iecs.logger.LogDebug("[IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST]");
                                        iecs.mms.SendGetNameListNamedVariableList(iecs);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST_WAIT;
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST:
                                        iecs.logger.LogDebug("[IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST]");
                                        if (iecs.mms.SendGetNamedVariableListAttributes(iecs) != -0)
                                            iecs.istate = Iec61850lStateEnum.IEC61850_MAKEGUI;
                                        else
                                            iecs.istate = Iec61850lStateEnum.IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST_WAIT;
                                        break;
                                    case Iec61850lStateEnum.IEC61850_MAKEGUI:
                                        iecs.logger.LogDebug("[IEC61850_MAKEGUI]");
                                        //self._env.mainWindow.makeTree(iecs);
                                        self._env.winMgr.MakeIedTree(iecs);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_FREILAUF;
                                        //iecs.lastOperationData = new NodeBase[1];//() {  };
                                        //iecs.lastOperationData[0] = iecs.files;
                                        //iecs.Send(iecs.lastOperationData, ad, ActionRequested.GetDirectory);
                                        break;
                                    case Iec61850lStateEnum.IEC61850_FREILAUF:
                                        // File service handling
                                        switch (iecs.fstate)
                                        {
                                            case FileTransferState.FILE_DIRECTORY:
                                                if (iecs.lastOperationData[0] is NodeIed) 
                                                    //self._env.mainWindow.makeTree(iecs);
                                                    self._env.winMgr.MakeIedTree(iecs);
                                                iecs.fstate = FileTransferState.FILE_NO_ACTION;
                                                break;
                                            case FileTransferState.FILE_OPENED:
                                            case FileTransferState.FILE_READ:
                                                // issue a read
                                                iecs.Send(iecs.lastOperationData, ad, ActionRequested.ReadFile);
                                                iecs.fstate = FileTransferState.FILE_NO_ACTION;
                                                break;
                                            case FileTransferState.FILE_COMPLETE:
                                                // issue a close
                                                iecs.Send(iecs.lastOperationData, ad, ActionRequested.CloseFile);
                                                /*if (iecs.lastOperationData[0] is NodeFile)
                                                {
                                                    (iecs.lastOperationData[0] as NodeFile).SaveFile((iecs.lastOperationData[0] as NodeFile).Name);
                                                    iecs.logger.LogDebug("File Read completed, file saved to " + (iecs.lastOperationData[0] as NodeFile).Name);
                                                }*/
                                                iecs.fstate = FileTransferState.FILE_NO_ACTION;
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case OsiProtocolState.OSI_STATE_SHUTDOWN:
                                TcpRw.StopClient(iecs);
                                break;
                        }
                        break;
                    default:
                        break;
                }
                int waitres = WaitHandle.WaitAny(_waitHandles, 500);
                switch (waitres)
                {
                    case 0:     // connect
                        if (iecs.ostate == OsiProtocolState.OSI_STATE_START)
                            iecs.ostate = OsiProtocolState.OSI_CONNECT_COTP;
                        iecs.connectDone.Reset();
                        TcpRw.Receive(iecs);    // issue a Receive call
                        break;
                    case 1:     // receive
                        iecs.receiveDone.Reset();
                        TcpRw.Receive(iecs);    // issue a new Receive call
                        break;
                    case 2:     // send
                        iecs.sendDone.Reset();
                        break;
                    case 3:     // endthread
                        self._run = false;
                        break;
                    case 4:     // send data
                        iecs.sendQueueWritten.Reset();
                        lock (iecs.SendQueue)
                        {
                            while (iecs.SendQueue.Count > 0)
                            {
                                WriteQueueElement el = iecs.SendQueue.Dequeue();
                                switch (el.Action)
                                {
                                    case ActionRequested.Write:
                                        iecs.mms.SendWrite(iecs, el);
                                        break;
                                    case ActionRequested.WriteAsStructure:
                                        iecs.mms.SendWriteAsStructure(iecs, el);
                                        break;
                                    case ActionRequested.Read:
                                        iecs.mms.SendRead(iecs, el);
                                        break;
                                    case ActionRequested.DefineNVL:
                                        iecs.mms.SendDefineNVL(iecs, el);
                                        break;
                                    case ActionRequested.DeleteNVL:
                                        iecs.mms.SendDeleteNVL(iecs, el);
                                        break;
                                    case ActionRequested.GetDirectory:
                                        iecs.mms.SendFileDirectory(iecs, el);
                                        break;
                                    case ActionRequested.OpenFile:
                                        iecs.mms.SendFileOpen(iecs, el);
                                        break;
                                    case ActionRequested.ReadFile:
                                        iecs.mms.SendFileRead(iecs, el);
                                        break;
                                    case ActionRequested.CloseFile:
                                        iecs.mms.SendFileClose(iecs, el);
                                        break;
                                }
                            }
                        }
                        break;                    
                    case WaitHandle.WaitTimeout:
                        break;
                }
            }
            TcpRw.StopClient(iecs);
        }
    }
}
