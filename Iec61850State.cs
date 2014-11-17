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
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using MMS_ASN1_Model;

namespace IEDExplorer
{
    class Iec61850State: TcpState
    {
        /// <summary>
        /// Size of data buffer.
        /// </summary>
        public const int dataBufferSize = 2048;
        /// <summary>
        /// Index of receive buffer.
        /// </summary>
        public int dataBufferIndex = 0;
        /// <summary>
        /// TPKT Length
        /// </summary>
        public int TpktLen = 0;
        /// <summary>
        /// TPKT Datagram buffer.
        /// </summary>
        public byte[] dataBuffer = new byte[dataBufferSize];
        /// <summary>
        /// Upper level protocol state
        /// </summary>
        public Iec61850lStateEnum istate = Iec61850lStateEnum.IEC61850_STATE_START;
        /// <summary>
        /// TPKT Receive state
        /// </summary>
        public TpktState kstate = TpktState.TPKT_RECEIVE_START;
        /// <summary>
        /// OSI Receive state
        /// </summary>
        public OsiProtocolState ostate = OsiProtocolState.OSI_STATE_START;
        /// <summary>
        /// MMS File service state
        /// </summary>
        public FileTransferState fstate = FileTransferState.FILE_NO_ACTION;
        /// <summary>
        /// OSI Protocol emulation
        /// </summary>
        public OsiEmul osi = new OsiEmul();
        /// <summary>
        /// MMS Protocol emulation
        /// </summary>
        public Scsm_MMS mms = new Scsm_MMS();
        /// <summary>
        /// Input stream of MMS parsing
        /// </summary>
        public MemoryStream msMMS = new MemoryStream();
        /// <summary>
        /// Memory for continuation of requests
        /// </summary>
        public Identifier continueAfter;
        /// <summary>
        /// Memory for continuation of file directory requests
        /// </summary>
        public FileName continueAfterFileDirectory;
        /// <summary>
        /// Server data
        /// </summary>
        public Iec61850Model dataModel;
        /// <summary>
        /// Queue for sending data from another threads
        /// </summary>
        public Queue<WriteQueueElement> SendQueue = new Queue<WriteQueueElement>();
        public ManualResetEvent sendQueueWritten = new ManualResetEvent(false);
        public NodeBase[] lastOperationData = null;
        /// <summary>
        /// List of captured MMS packets (PDUs)
        /// </summary>
        public List<MMSCapture> CapturedData = new List<MMSCapture>();
        /// <summary>
        /// Capture of MMS packets (PDUs) active
        /// </summary>
        public bool CaptureActive = false;

        public Iec61850State()
        {
            dataModel = new Iec61850Model(this);
        }

        public void NextState()
        {
        }

        public void Send(NodeBase[] Data, CommAddress Address, ActionRequested Action)
        {
            WriteQueueElement el = new WriteQueueElement(Data, Address, Action);
            lock (SendQueue)
            {
                SendQueue.Enqueue(el);
            }
            sendQueueWritten.Set();
        }
    }

}
