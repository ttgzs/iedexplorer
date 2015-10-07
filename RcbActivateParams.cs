using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class RcbActivateParams
    {
        public NodeRCB self;

        public bool sendRptID = false;
        public bool sendRptEna = true;
        public bool sendResv = true;
        public bool sendDatSet = true;
        public bool sendOptFlds = false;
        public bool sendBufTm = false;
        public bool sendTrgOps = true;
        public bool sendIntgPd = false;
        public bool sendGI = true;
        public bool sendPurgeBuf = false;
        public bool sendResvTms = false;
        public bool sendEntryId = false;
    }

}
