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
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace IEDExplorer
{
    class TcpState
    {
        /// <summary>
        ///  logger.
        /// </summary>
        public Logger logger = null;
        /// <summary>
        ///  Client socket.
        /// </summary>
        public Socket workSocket = null;
        /// <summary>
        /// Size of receive buffer.
        /// </summary>
        public const int recvBufferSize = 65535;
        /// <summary>
        /// Bytes received to buffer.
        /// </summary>
        public int recvBytes = 0;
        /// <summary>
        /// Receive buffer.
        /// </summary>
        public byte[] recvBuffer = new byte[recvBufferSize];
        /// <summary>
        /// Size of send buffer.
        /// </summary>
        public const int sendBufferSize = 65535;
        /// <summary>
        /// Bytes to be sent from send buffer.
        /// </summary>
        public int sendBytes = 0;
        /// <summary>
        /// Send buffer.
        /// </summary>
        public byte[] sendBuffer = new byte[recvBufferSize];
        /// <summary>
        /// Upper level protocol state
        /// </summary>
        public TcpProtocolState tstate = TcpProtocolState.TCP_STATE_START;
        /// <summary>
        /// Hostname
        /// </summary>
        public String hostname;
        /// <summary>
        /// tcp Port
        /// </summary>
        public int port = 102;
        /// <summary>
        /// Keepalive time
        /// </summary>
        public ulong keepalive_time = 10000;
        /// <summary>
        /// Keepalive interval
        /// </summary>
        public ulong keepalive_interval = 15000;
        /// <summary>
        /// Connect Timeout.
        /// </summary>
        public int ConnectTimeoutms = 15000;
        /// <summary>
        /// ManualResetEvent instances signal completion.
        /// </summary>
        public ManualResetEvent connectDone =
            new ManualResetEvent(false);
        public ManualResetEvent sendDone =
            new ManualResetEvent(false);
        public ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        /// <summary>
        /// SafeHandle 
        /// </summary>
    }
}
