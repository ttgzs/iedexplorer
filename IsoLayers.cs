using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            logger = iecs.logger;
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
            return isoCotp.SendCOTPSessionInit(iecs);
        }

        public int SendPresentationInit(Iec61850State iecs)
        {
            // Make session & present. init telegramm
            byte[] b1 = new byte[1024];
            byte[] b2 = new byte[1024];
            IsoConnectionParameters cp = new IsoConnectionParameters();

            iecs.mms.SendInitiate(iecs);
            int len = isoAcse.createAssociateRequestMessage(cp, b1, 0, iecs.msMMS.GetBuffer(), (int)iecs.msMMS.Length, null);
            len = isoPres.createConnectPdu(cp, b2, b1, len);
            len = isoSess.createConnectSpdu(cp, b1, b2, len);
            b1.CopyTo(iecs.sendBuffer, IsoCotp.COTP_HDR_DT_SIZEOF + IsoTpkt.TPKT_SIZEOF);

            iecs.sendBytes = len + IsoCotp.COTP_HDR_DT_SIZEOF + IsoTpkt.TPKT_SIZEOF;
            isoCotp.Send(iecs);
            return 0;
        }

        public int Receive(Iec61850State iecs)
        {
            iecs.logger.LogDebug("Iso.Receive");
            IsoCotp.CotpReceiveResult res = isoCotp.Receive(iecs);
            if (res == IsoCotp.CotpReceiveResult.DATA)
            {
                // Incoming data
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
            // Make COTP data telegramm
            IsoTpkt.Send(iecs);
            return 0;
        }
    }
}
