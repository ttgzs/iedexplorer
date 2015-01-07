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

            IsoTpkt.Send(iecs);
            return 0;
        }

        public int Receive(Iec61850State iecs)
        {
            iecs.logger.LogDebug("osi.Receive");
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
