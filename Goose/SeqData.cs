using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    public class SeqData
    {
        public NodeGData refdata = null;
        public int duration = 0;
        public string data = "";

        public SeqData(NodeGData dat, int dur)
        {
            this.refdata = dat;
            this.duration = dur;
            this.data = dat.StringValue;
        }

        public SeqData(NodeGData dat, string val, int dur)
        {
            this.refdata = dat;
            this.duration = dur;
            this.data = val;
        }

    }
}
