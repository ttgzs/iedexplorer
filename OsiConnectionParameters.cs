using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class OsiConnectionParameters
    {
        public OsiAcse.sAcseAuthenticationParameter acseAuthParameter;

        public byte[] remoteApTitle = new byte[10];
        public int remoteApTitleLen;
        public int remoteAEQualifier;
        public uint remotePSelector;
        public ushort remoteSSelector;
        public OsiCotp.TSelector remoteTSelector;

        public byte[] localApTitle = new byte[10];
        public int localApTitleLen;
        public int localAEQualifier;
        public uint localPSelector;
        public ushort localSSelector;
        public OsiCotp.TSelector localTSelector;
    }
}
