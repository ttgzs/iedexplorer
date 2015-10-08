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
        public bool sendEntryID = false;

        public NodeData[] getWriteArray()
        {
            List<NodeData> nlst = new List<NodeData>();
            NodeBase fcn = self.FindChildNode("RptID");
            if (sendRptID && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("RptEna");
            if (sendRptEna && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("Resv");
            if (sendResv && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("DatSet");
            if (sendDatSet && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("OptFlds");
            if (sendOptFlds && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("BufTm");
            if (sendBufTm && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("TrgOps");
            if (sendTrgOps && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("IntgPd");
            if (sendIntgPd && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("GI");
            if (sendGI && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("PurgeBuf");
            if (sendPurgeBuf && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("ResvTms");
            if (sendResvTms && fcn != null) nlst.Add((NodeData)fcn);
            fcn = self.FindChildNode("EntryID");
            if (sendEntryID && fcn != null) nlst.Add((NodeData)fcn);
            return nlst.ToArray();
        }
    }

}
