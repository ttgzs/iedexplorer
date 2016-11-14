using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace IEDExplorer
{
    public class IsoLayers
    {
        // ISO layers TPKT, COTP, SESS, PRES, ACSE coordinations
        Iec61850State iecs;
        /// <summary>
        /// OSI Protocol ACSE layer (new implementation)
        /// </summary>
        public IsoAcse isoAcse;
        /// <summary>
        /// OSI Protocol PRES layer (new implementation)
        /// </summary>
        public IsoPres isoPres;
        /// <summary>
        /// OSI Protocol SESS layer (new implementation)
        /// </summary>
        public IsoSess isoSess;
        /// <summary>
        /// OSI Protocol COTP layer (new implementation)
        /// </summary>
        public IsoCotp isoCotp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iec"></param>
        public IsoLayers(Iec61850State iec)
        {
            iecs = iec;
            Reset();
        }

        void Reset()
        {
            isoAcse = new IsoAcse(iecs);
            isoPres = new IsoPres(iecs);
            isoSess = new IsoSess(iecs);
            isoCotp = new IsoCotp(iecs.cp);
        }

        public int SendCOTPSessionInit(Iec61850State iecs)
        {
            // Make COTP init telegramm
            return isoCotp.SendInit(iecs);
        }

        int SendPresentationInit(Iec61850State iecs)
        {
            // Make session & present. init telegramm
            byte[] b1 = new byte[1024];
            byte[] b2 = new byte[1024];
            //cp = new IsoConnectionParameters();
            bool dbg = false;    // local debug enable var

            // MMS Initiate already encoded in iecs.msMMSout
            if (dbg) iecs.logger.LogDebugBuffer("Send MMS", iecs.msMMSout.GetBuffer(), 0, iecs.msMMSout.Length);

            int len = isoAcse.createAssociateRequestMessage(iecs.cp, b1, 0, iecs.msMMSout.GetBuffer(), (int)iecs.msMMSout.Length);
            if (dbg) iecs.logger.LogDebugBuffer("Send Acse", b1, 0, len);

            len = isoPres.createConnectPdu(iecs.cp, b2, b1, len);
            if (dbg) iecs.logger.LogDebugBuffer("Send Pres", b2, 0, len);

            len = isoSess.createConnectSpdu(iecs.cp, b1, b2, len);
            if (dbg) iecs.logger.LogDebugBuffer("Send Sess", b1, 0, len);

            b1.CopyTo(iecs.sendBuffer, IsoCotp.COTP_HDR_DT_SIZEOF + IsoTpkt.TPKT_SIZEOF);
            iecs.sendBytes = len;
            isoCotp.Send(iecs);
            return 0;
        }

        int SendData(Iec61850State iecs)
        {
            //fastSend(iecs);
            layeredSend(iecs);

            return 0;
        }

        int layeredSend(Iec61850State iecs)
        {
            // Make COTP data telegramm directly
            // MMS already encoded in iecs.msMMSout
            iecs.sendBytes = (int)iecs.msMMSout.Length;

            int spos = isoSess.createDataSpdu(iecs.sendBuffer, IsoCotp.COTP_HDR_DT_SIZEOF + IsoTpkt.TPKT_SIZEOF);

            int dpos = isoPres.createUserData(iecs.sendBuffer, spos, iecs.sendBytes);

            iecs.msMMSout.Seek(0, SeekOrigin.Begin);
            iecs.msMMSout.Read(iecs.sendBuffer, dpos, iecs.sendBytes);

            iecs.sendBytes += dpos - IsoCotp.COTP_HDR_DT_SIZEOF - IsoTpkt.TPKT_SIZEOF;

            isoCotp.Send(iecs);
            return 0;
        }

        int fastSend(Iec61850State iecs)
        {
            // Make COTP data telegramm directly
            int offs = IsoTpkt.TPKT_SIZEOF;
            // MMS already encoded in iecs.msMMSout
            iecs.sendBytes = (int)iecs.msMMSout.Length;

            iecs.sendBuffer[offs++] = 0x02; // cotp.hdrlen
            iecs.sendBuffer[offs++] = IsoCotp.COTP_CODE_DT; // code
            iecs.sendBuffer[offs++] = 0x80; // number "complete"
            iecs.sendBuffer[offs++] = 0x01; // giveTokensPDU.type
            iecs.sendBuffer[offs++] = 0x00; // giveTokensPDU.hdrlen
            iecs.sendBuffer[offs++] = 0x01; // dataTransferPDU.type
            iecs.sendBuffer[offs++] = 0x00; // dataTransferPDU.hdrlen

            iecs.sendBuffer[offs++] = 0x61; // pres.dtpdu_tag
            iecs.sendBuffer[offs++] = 0x82; // pres.dtpdu_len_code
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(iecs.sendBytes + 15 - 4))), 0, iecs.sendBuffer, offs, 2); // pres.dtpdu_len
            offs += 2;
            iecs.sendBuffer[offs++] = 0x30; // pres.sequ_tag
            iecs.sendBuffer[offs++] = 0x82; // pres.sequ_len_code
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(iecs.sendBytes + 15 - 8))), 0, iecs.sendBuffer, offs, 2); // pres.sequ_len
            offs += 2;
            iecs.sendBuffer[offs++] = 0x02; // pres.context_tag
            iecs.sendBuffer[offs++] = 0x01; // pres.context_len
            iecs.sendBuffer[offs++] = 0x03; // pres.context_val
            iecs.sendBuffer[offs++] = 0xa0; // pres.data_tag
            iecs.sendBuffer[offs++] = 0x82; // pres.data_len_code
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(iecs.sendBytes))), 0, iecs.sendBuffer, offs, 2); // pres.sequ_len
            offs += 2;

            iecs.msMMSout.Seek(0, SeekOrigin.Begin);
            iecs.msMMSout.Read(iecs.sendBuffer, offs, iecs.sendBytes);

            iecs.sendBytes += offs;
            IsoTpkt.Send(iecs);
            return 0;
        }

        public int Receive(Iec61850State iecs)
        {
            iecs.logger.LogDebug("Iso.Receive");
            IsoCotp.CotpReceiveResult res = isoCotp.Receive(iecs);
            byte[] buffer = iecs.msMMS.GetBuffer();
            long len = iecs.msMMS.Length;
            iecs.logger.LogDebugBuffer("Rec buffer", buffer, 0, len);
            if (res == IsoCotp.CotpReceiveResult.DATA)
            {
                // Incoming data
                iecs.logger.LogDebug(String.Format("Calling isoSess.parseMessage with data len {0}", iecs.msMMS.Length));
                IsoSess.IsoSessionIndication sess = isoSess.parseMessage(buffer, (int)len);
                if (sess == IsoSess.IsoSessionIndication.SESSION_DATA)
                {
                    int dataPos = isoPres.parseUserData(buffer, (int)isoSess.UserDataIndex, (int)(len - isoSess.UserDataIndex));
                    if (dataPos > 0)
                    {
                        // Adjust the stream position to the MMS message start
                        iecs.msMMS.Seek(dataPos, SeekOrigin.Begin);
                        iecs.mms.ReceiveData(iecs);
                    }
                    else
                    {
                        iecs.ostate = IsoProtocolState.OSI_STATE_SHUTDOWN;
                    }
                }
                else if (sess == IsoSess.IsoSessionIndication.SESSION_CONNECT)
                {
                    iecs.ostate = IsoProtocolState.OSI_STATE_SHUTDOWN;
                    int dataPosPres = isoPres.parseAcceptMessage(buffer, isoSess.UserDataIndex, (int)(len - isoSess.UserDataIndex));
                    if (dataPosPres > 0)
                    {
                        IsoAcse.AcseIndication acseRes = isoAcse.parseMessage(buffer, isoPres.UserDataIndex, (int)(len - isoPres.UserDataIndex));
                        if (acseRes == IsoAcse.AcseIndication.ACSE_ASSOCIATE)
                        {
                            iecs.msMMS.Seek(isoAcse.UserDataIndex, SeekOrigin.Begin);
                            iecs.logger.LogDebug("Read at " + isoAcse.UserDataIndex);
                            iecs.mms.ReceiveData(iecs);
                            iecs.ostate = IsoProtocolState.OSI_CONNECTED;
                        }
                    }
                }
                else
                {
                    iecs.ostate = IsoProtocolState.OSI_STATE_SHUTDOWN;
                }
                iecs.msMMS = new MemoryStream();
            }
            else if (res == IsoCotp.CotpReceiveResult.INIT)
            {
                iecs.ostate = IsoProtocolState.OSI_CONNECT_PRES;
            }
            else if (res == IsoCotp.CotpReceiveResult.ERROR)
            {
                iecs.ostate = IsoProtocolState.OSI_STATE_SHUTDOWN;
            }
            return 0;
        }

        public int Send(Iec61850State iecs)
        {
            if (iecs.ostate == IsoProtocolState.OSI_CONNECT_PRES)
            {
                iecs.ostate = IsoProtocolState.OSI_CONNECT_PRES_WAIT;
                SendPresentationInit(iecs);
            }
            else
                SendData(iecs);
            return 0;
        }
    }
}
