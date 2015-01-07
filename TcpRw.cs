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
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace IEDExplorer
{
    internal class TcpRw
    {
        public static void StartClient(TcpState tcps)
        {
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;
                //if (tcps.hostname == "localhost" || tcps.hostname == "127.0.0.1")
                //{
                //    ipHostInfo = Dns.GetHostEntry("");
                //}
                //else
                IPAddress ipAddress = null;

                if (!IPAddress.TryParse(tcps.hostname, out ipAddress))
                {
                    ipHostInfo = Dns.GetHostEntry(tcps.hostname);
                    for (int i = 0; i < ipHostInfo.AddressList.Length; i++)
                    {
                        if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = ipHostInfo.AddressList[i];
                            break;
                        }
                    }
                }
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, tcps.port);

                // Create a TCP/IP socket.
                tcps.workSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // KeepAlive`
                if (tcps.keepalive_time > 0 && tcps.keepalive_interval > 0)
                    SetKeepAlive(tcps.workSocket, tcps.keepalive_time, tcps.keepalive_interval);

                // Events reset
                tcps.connectDone.Reset();
                tcps.receiveDone.Reset();
                tcps.sendDone.Reset();

                // Connect to the remote endpoint.
                tcps.workSocket.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), tcps);
                tcps.tstate = TcpProtocolState.TCP_CONNECT_WAIT;
            }
            catch (Exception e)
            {
                StopClient(tcps);
                tcps.logger.LogError(e.ToString());
            }
        }

        public static void StopClient(TcpState tcps)
        {
            // Connect to a remote device.
            tcps.logger.LogInfo("StopClient: Socket shutdowned.");
            try
            {
                tcps.tstate = TcpProtocolState.TCP_STATE_SHUTDOWN;
                // Release the socket.
                if (tcps.workSocket != null)
                {
                    if (tcps.workSocket.Connected)
                    {
                        tcps.workSocket.Shutdown(SocketShutdown.Both);
                        tcps.receiveDone.WaitOne(15000);
                    }
                    tcps.workSocket.Close();
                    tcps.workSocket = null;
                }
            }
            catch (Exception e)
            {
                tcps.logger.LogError("Closing: " + e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            TcpState tcps = (TcpState)ar.AsyncState;
            try
            {
                // Complete the connection.
                tcps.workSocket.EndConnect(ar);

                tcps.logger.LogInfo(String.Format("ConnectCallback: Socket connected to {0}",
                    tcps.workSocket.RemoteEndPoint.ToString()));

                // Signal that the connection has been made.
                tcps.tstate = TcpProtocolState.TCP_CONNECTED;
                tcps.connectDone.Set();
            }
            catch (Exception e)
            {
                StopClient(tcps);
                tcps.logger.LogError(e.ToString());
            }
        }

        public static void Receive(TcpState tcps)
        {
            try
            {
                // Begin receiving the data from the remote device.
                tcps.workSocket.BeginReceive(tcps.recvBuffer, 0, Iec61850State.recvBufferSize, 0,
                    new AsyncCallback(ReceiveCallback), tcps);
            }
            catch (Exception e)
            {
                StopClient(tcps);
                tcps.logger.LogError(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            TcpState tcps = (TcpState)ar.AsyncState;
            //try
            {
                // Complete the connection.
                if (tcps.workSocket != null)
                {
                    try
                    {
                        tcps.recvBytes = tcps.workSocket.EndReceive(ar);
                        //Console.WriteLine("ReceiveCallback: Data received {0}",
                        //    tcps.recvBytes.ToString());

                        IsoTpkt.Parse(tcps);
                    }
                    catch {}
                    // Signal that the data has been received.
                    tcps.receiveDone.Set();
                }
            }
            //catch (Exception e)
            {
            //   StopClient(tcps);
            //    tcps.logger.LogInfo(e.ToString());
            }
        }

        public static void Send(TcpState tcps) //Socket client, byte[] data)
        {
            // Begin sending the data to the remote device.
            tcps.workSocket.BeginSend(tcps.sendBuffer, 0, tcps.sendBytes, 0,
                new AsyncCallback(SendCallback), tcps);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            TcpState tcps = (TcpState)ar.AsyncState;
            try
            {
                // Retrieve the socket from the state object.

                // Complete sending the data to the remote device.
                int bytesSent = tcps.workSocket.EndSend(ar);
                tcps.logger.LogDebug(String.Format("Sent {0} bytes to server.", bytesSent));

                // Signal that all bytes have been sent.
                tcps.sendDone.Set();
            }
            catch (Exception e)
            {
                tcps.logger.LogError(e.ToString());
            }
        }

        /// <summary>
        /// Sets socket to keepalive mode
        /// </summary>
        /// <param name="s">Socket</param>
        /// <param name="keepalive_time">time</param>
        /// <param name="keepalive_interval">interval</param>
        public static void SetKeepAlive(Socket s, ulong keepalive_time, ulong keepalive_interval)
        {
            int bytes_per_long = 32 / 8;
            byte[] keep_alive = new byte[3 * bytes_per_long];
            ulong[] input_params = new ulong[3];
            int i1;
            int bits_per_byte = 8;

            if (keepalive_time == 0 || keepalive_interval == 0)
                input_params[0] = 0;
            else
                input_params[0] = 1;
            input_params[1] = keepalive_time;
            input_params[2] = keepalive_interval;
            for (i1 = 0; i1 < input_params.Length; i1++)
            {
                keep_alive[i1 * bytes_per_long + 3] = (byte)(input_params[i1] >> ((bytes_per_long - 1) * bits_per_byte) & 0xff);
                keep_alive[i1 * bytes_per_long + 2] = (byte)(input_params[i1] >> ((bytes_per_long - 2) * bits_per_byte) & 0xff);
                keep_alive[i1 * bytes_per_long + 1] = (byte)(input_params[i1] >> ((bytes_per_long - 3) * bits_per_byte) & 0xff);
                keep_alive[i1 * bytes_per_long + 0] = (byte)(input_params[i1] >> ((bytes_per_long - 4) * bits_per_byte) & 0xff);
            }
            s.IOControl(IOControlCode.KeepAliveValues, keep_alive, null);
        } /* method AsyncSocket SetKeepAlive */

    }
}
