using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IEDExplorer
{
    class IsoLayers
    {
        // ISO layers TPKT, COTP, SESS, PRES, ACSE coordinations
        Iec61850State iecs;
        Logger logger;
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
            isoCotp = new IsoCotp();
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
            IsoConnectionParameters cp = new IsoConnectionParameters();
            bool dbg = true;    // local debug enable var

            //iecs.mms.SendInitiate(iecs);
            // MMS Initiate already encoded in iecs.msMMSout
            if (dbg) iecs.logger.LogDebugBuffer("Send MMS", iecs.msMMSout.GetBuffer(), 0, iecs.msMMSout.Length);

            int len = isoAcse.createAssociateRequestMessage(cp, b1, 0, iecs.msMMSout.GetBuffer(), (int)iecs.msMMSout.Length, null);
            if (dbg) iecs.logger.LogDebugBuffer("Send Acse", b1, 0, len);

            len = isoPres.createConnectPdu(cp, b2, b1, len);
            if (dbg) iecs.logger.LogDebugBuffer("Send Pres", b2, 0, len);

            len = isoSess.createConnectSpdu(cp, b1, b2, len);
            if (dbg) iecs.logger.LogDebugBuffer("Send Sess", b1, 0, len);

            b1.CopyTo(iecs.sendBuffer, IsoCotp.COTP_HDR_DT_SIZEOF + IsoTpkt.TPKT_SIZEOF);
            iecs.sendBytes = len;
            isoCotp.Send(iecs);
            return 0;
        }

        int SendData(Iec61850State iecs)
        {
            return 0;
        }

        public int Receive(Iec61850State iecs)
        {
            iecs.logger.LogDebug("Iso.Receive");
            IsoCotp.CotpReceiveResult res = isoCotp.Receive(iecs);
            byte[] buffer = iecs.msMMS.GetBuffer();
            long len = iecs.msMMS.Length;
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
                        iecs.msMMS.Seek(isoSess.UserDataIndex + dataPos, SeekOrigin.Begin);
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
                    int dataPosPres = isoPres.parseConnect(buffer, (int)isoSess.UserDataIndex, (int)(len - isoSess.UserDataIndex));
                    if (dataPosPres > 0)
                    {
                        isoAcse.parseMessage(buffer, (int)(isoSess.UserDataIndex + dataPosPres), (int)(len - (isoSess.UserDataIndex + dataPosPres)));

                        iecs.msMMS.Seek(isoSess.UserDataIndex + dataPosPres, SeekOrigin.Begin);
                        iecs.mms.ReceiveData(iecs);
                        iecs.ostate = IsoProtocolState.OSI_CONNECTED;
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
                SendPresentationInit(iecs);
            else
                SendData(iecs);
            return 0;
        }
    }
}
