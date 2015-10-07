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

        public NodeData[] getWriteArray()
        {
            List<NodeData> nlst = new List<NodeData>();
            if (sendRptID) nlst.Add((NodeData)self.FindChildNode("RptID"));
            if (sendRptEna) nlst.Add((NodeData)self.FindChildNode("RptEna"));
            if (sendResv) nlst.Add((NodeData)self.FindChildNode("Resv"));
            if (sendDatSet) nlst.Add((NodeData)self.FindChildNode("DatSet"));
            if (sendOptFlds) nlst.Add((NodeData)self.FindChildNode("OptFlds"));
            if (sendBufTm) nlst.Add((NodeData)self.FindChildNode("BufTm"));
            if (sendTrgOps) nlst.Add((NodeData)self.FindChildNode("TrgOps"));
            if (sendIntgPd) nlst.Add((NodeData)self.FindChildNode("IntgPd"));
            if (sendGI) nlst.Add((NodeData)self.FindChildNode("GI"));
            if (sendPurgeBuf) nlst.Add((NodeData)self.FindChildNode("PurgeBuf"));
            if (sendResvTms) nlst.Add((NodeData)self.FindChildNode("ResvTms"));
            if (sendEntryId) nlst.Add((NodeData)self.FindChildNode("EntryId"));
            return nlst.ToArray();
        }
    }

}
