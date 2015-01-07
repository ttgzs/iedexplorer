using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class IsoConnectionParameters
    {
        public IsoAcse.AcseAuthenticationParameter acseAuthParameter;

        public byte[] remoteApTitle = new byte[10];
        public int remoteApTitleLen;
        public int remoteAEQualifier;
        public uint remotePSelector;
        public ushort remoteSSelector;
        public IsoCotp.TSelector remoteTSelector;

        public byte[] localApTitle = new byte[10];
        public int localApTitleLen;
        public int localAEQualifier;
        public uint localPSelector;
        public ushort localSSelector;
        public IsoCotp.TSelector localTSelector;
    }
}
