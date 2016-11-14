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
        private IsoConnectionParameters isoParameters;
        private Env _env;
        WaitHandle[] _waitHandles = new WaitHandle[5];
        public Iec61850State iecs;
        Logger logger = Logger.getLogger();

        public Scsm_MMS_Worker(Env env)
        {
            _env = env;
        }

        public int Start(IsoConnectionParameters par)
        {
            isoParameters = par;
            return Start();
        }

        int Start()
        {
            //// Run Thread
            if (!_run && _workerThread == null)
            {
                _run = true;
                _workerThread = new Thread(new ParameterizedThreadStart(WorkerThreadProc));
                logger.LogInfo(String.Format("Starting new communication, hostname = {0}, port = {1}.", isoParameters.hostname, isoParameters.port));
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
                (_waitHandles[3] as ManualResetEvent).Set();
                _workerThread = null;
                logger.LogInfo(String.Format("Communication to hostname = {0}, port = {1} stopped.", isoParameters.hostname, isoParameters.port));
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
            iecs.hostname = self.isoParameters.hostname;    // due to tcps inheritance
            iecs.port = self.isoParameters.port;            // due to tcps inheritance
            iecs.cp = self.isoParameters;
            iecs.logger = Logger.getLogger();
            _env.winMgr.BindToCapture(iecs);

            _waitHandles[0] = iecs.connectDone;
            _waitHandles[1] = iecs.receiveDone;
            _waitHandles[2] = iecs.sendDone;
            _waitHandles[3] = new ManualResetEvent(false);   // end thread
            _waitHandles[4] = iecs.sendQueueWritten;
            //DateTime tout = null;

            CommAddress ad = new CommAddress();
            DateTime IdentifyTimeoutBase = new DateTime();
            TimeSpan IdentifyTimeout = new TimeSpan(0, 0, 5);   // 5 sec

            while (self._run)
            {
                switch (iecs.tstate)
                {
                    case TcpProtocolState.TCP_STATE_START:
                        iecs.logger.LogInfo("[TCP_STATE_START]");
                        iecs.kstate = IsoTpktState.TPKT_RECEIVE_START;
                        iecs.ostate = IsoProtocolState.OSI_STATE_START;
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
                            case IsoProtocolState.OSI_CONNECT_COTP:
                                iecs.logger.LogInfo("[OSI_CONNECT_COTP]");
                                iecs.ostate = IsoProtocolState.OSI_CONNECT_COTP_WAIT;
                                iecs.iso.SendCOTPSessionInit(iecs);
                                break;
                            case IsoProtocolState.OSI_CONNECT_PRES:
                                iecs.logger.LogInfo("[OSI_CONNECT_PRES]");
                                // This cannot be before Send, but is issued inside Send chain before TCP send
                                // iecs.ostate = IsoProtocolState.OSI_CONNECT_PRES_WAIT;
                                iecs.mms.SendInitiate(iecs);
                                break;
                            case IsoProtocolState.OSI_CONNECTED:
                                switch (iecs.istate)
                                {
                                    case Iec61850lStateEnum.IEC61850_STATE_START:
                                        if (iecs.DataModel.ied.Identify)
                                        {
                                            iecs.logger.LogInfo("[IEC61850_STATE_START] (Send IdentifyRequest)");
                                            iecs.istate = Iec61850lStateEnum.IEC61850_CONNECT_MMS_WAIT;
                                            iecs.mms.SendIdentify(iecs);
                                            IdentifyTimeoutBase = DateTime.UtcNow;
                                        }
                                        else
                                        {
                                            iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_DOMAIN;
                                        }
                                        break;
                                    case Iec61850lStateEnum.IEC61850_CONNECT_MMS_WAIT:
                                        // If we wait for Identify response too long, continue without it
                                        if (DateTime.UtcNow.Subtract(IdentifyTimeout).CompareTo(IdentifyTimeoutBase) > 0)
                                        {
                                            // Timeout expired
                                            iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_DOMAIN;
                                            iecs.logger.LogWarning("MMS Identify message not supported by server, although declared in ServicesSupportedCalled bitstring");
                                        }
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_NAMELIST_DOMAIN:
                                        iecs.logger.LogDebug("[IEC61850_READ_NAMELIST_DOMAIN]");
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_DOMAIN_WAIT;
                                        iecs.mms.SendGetNameListDomain(iecs);
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_NAMELIST_VAR:
                                        iecs.logger.LogDebug("[IEC61850_READ_NAMELIST_VAR]");
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_VAR_WAIT;
                                        iecs.mms.SendGetNameListVariables(iecs);
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_ACCESSAT_VAR:
                                        iecs.logger.LogDebug("[IEC61850_READ_ACCESSAT_VAR]");
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_ACCESSAT_VAR_WAIT;
                                        iecs.mms.SendGetVariableAccessAttributes(iecs);
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_MODEL_DATA:
                                        iecs.logger.LogDebug("[IEC61850_READ_MODEL_DATA]");
                                        CommAddress adr = new CommAddress();
                                        adr.Domain = null;
                                        adr.Variable = null;
                                        adr.owner = null;
                                        NodeBase[] data = new NodeBase[1];
                                        // Issue reads by FC level
                                        data[0] = iecs.DataModel.ied.GetActualChildNode().GetActualChildNode().GetActualChildNode();
                                        WriteQueueElement wqel = new WriteQueueElement(data, adr, ActionRequested.Read);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_MODEL_DATA_WAIT;
                                        iecs.mms.SendRead(iecs, wqel);
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST:
                                        iecs.logger.LogDebug("[IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST]");
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST_WAIT;
                                        iecs.mms.SendGetNameListNamedVariableList(iecs);
                                        break;
                                    case Iec61850lStateEnum.IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST:
                                        iecs.logger.LogDebug("[IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST]");
                                        iecs.istate = Iec61850lStateEnum.IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST_WAIT;
                                        if (iecs.mms.SendGetNamedVariableListAttributes(iecs) != 0)
                                        {
                                            // No VarLists
                                            iecs.logger.LogInfo("Init end: [IEC61850_FREILAUF]");
                                            iecs.istate = Iec61850lStateEnum.IEC61850_MAKEGUI;
                                        }
                                        break;
                                    case Iec61850lStateEnum.IEC61850_MAKEGUI:
                                        iecs.logger.LogDebug("[IEC61850_MAKEGUI]");
                                        iecs.DataModel.BuildIECModelFromMMSModel();
                                        self._env.winMgr.MakeIedTree(iecs);
                                        self._env.winMgr.MakeIecTree(iecs);
                                        self._env.winMgr.mainWindow.Set_iecf(iecs);
                                        iecs.istate = Iec61850lStateEnum.IEC61850_FREILAUF;
                                        break;
                                    case Iec61850lStateEnum.IEC61850_FREILAUF:
                                        // File service handling
                                        switch (iecs.fstate)
                                        {
                                            case FileTransferState.FILE_DIRECTORY:
                                                if (iecs.lastFileOperationData[0] is NodeIed)
                                                    self._env.winMgr.MakeFileTree(iecs);
                                                iecs.fstate = FileTransferState.FILE_NO_ACTION;
                                                break;
                                            case FileTransferState.FILE_OPENED:
                                            case FileTransferState.FILE_READ:
                                                // issue a read
                                                iecs.Send(iecs.lastFileOperationData, ad, ActionRequested.ReadFile);
                                                iecs.fstate = FileTransferState.FILE_NO_ACTION;
                                                break;
                                            case FileTransferState.FILE_COMPLETE:
                                                // issue a close
                                                // file can be saved from context menu
                                                iecs.Send(iecs.lastFileOperationData, ad, ActionRequested.CloseFile);
                                                iecs.fstate = FileTransferState.FILE_NO_ACTION;
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case IsoProtocolState.OSI_STATE_SHUTDOWN:
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
                        if (iecs.ostate == IsoProtocolState.OSI_STATE_START)
                            iecs.ostate = IsoProtocolState.OSI_CONNECT_COTP;
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
                        Logger.getLogger().LogDebug("SendQueue Waiting for lock in Worker!");
                        WriteQueueElement el;
                        while (iecs.SendQueue.TryDequeue(out el))
                        {
                            switch (el.Action)
                            {
                                case ActionRequested.Write:
                                    iecs.mms.SendWrite(iecs, el);
                                    break;
                                case ActionRequested.WriteAsStructure:
                                    iecs.mms.SendWriteAsStructure(iecs, el);
                                    break;
                                case ActionRequested.Read:
                                    if (el.Data[0] is NodeVL)
                                        iecs.mms.SendReadVL(iecs, el);
                                    else
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
                        break;
                    case WaitHandle.WaitTimeout:
                        break;
                }
            }
            TcpRw.StopClient(iecs);
            _env.winMgr.UnBindFromCapture(iecs);
        }
    }
}
