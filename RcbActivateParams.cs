using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class RcbActivateParams
    {
        public NodeRCB self;
        public string RptId;
        public bool RptEna;
        public bool Resv;
        public string DatSet;
        public uint ConfRev;
        public ReportOptions OptFlds;
        public uint BufTm;
        public uint SqNum;
        public TriggerOptions TrgOps;
        public uint IntgPd;
        public bool GI;
        public bool PurgeBuf;
        public uint ResvTm;
        public string EntryId;

        public bool isBuffered;

        private bool flagRptId = false;
        private bool flagRptEna = false;
        private bool flagResv = false;
        private bool flagDatSet = false;
        private bool flagConfRev = false;
        private bool flagOptFlds = false;
        private bool flagBufTm = false;
        private bool flagSqNum = false;
        private bool flagTrgOps = false;
        private bool flagIntgPd = false;
        private bool flagGI = false;
        private bool flagPurgeBuf = false;
        private bool flagResvTms = false;
        private bool flagEntryId = false;
    }

    [Flags]
    public enum TriggerOptions
    {
        NONE = 0,
        /** send report when value of data changed */
        DATA_CHANGED = 1,
        /** send report when quality of data changed */
        QUALITY_CHANGED = 2,
        /** send report when data or quality is updated */
        DATA_UPDATE = 4,
        /** periodic transmission of all data set values */
        INTEGRITY = 8,
        /** general interrogation (on client request) */
        GI = 16
    }

    [Flags]
    public enum ReportOptions
    {
        NONE = 0,
        SEQ_NUM = 1,
        TIME_STAMP = 2,
        REASON_FOR_INCLUSION = 4,
        DATA_SET = 8,
        DATA_REFERENCE = 16,
        BUFFER_OVERFLOW = 32,
        ENTRY_ID = 64,
        CONF_REV = 128
    }

}
