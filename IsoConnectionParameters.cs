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
        public string remoteApTitleS = "";
        public int remoteApTitleLen;
        public int remoteAEQualifier;
        public uint remotePSelector;
        public ushort remoteSSelector;
        public IsoCotp.TSelector remoteTSelector;

        public byte[] localApTitle = new byte[10];
        public string localApTitleS = "";
        public int localApTitleLen;
        public int localAEQualifier;
        public uint localPSelector;
        public ushort localSSelector;
        public IsoCotp.TSelector localTSelector;

        public IsoConnectionParameters()
        {
            // Defaults
            IsoCotp.TSelector selector1 = new IsoCotp.TSelector(2, 0);
            IsoCotp.TSelector selector2 = new IsoCotp.TSelector(2, 0);
            setLocalAddresses(1, 1, selector1);
            setLocalApTitle("1.1.1.999", 12);
            setRemoteAddresses(1, 1, selector2);
            setRemoteApTitle("1.1.1.999.1", 12);
        }
        public void setLocalAddresses(uint pSelector, ushort sSelector, IsoCotp.TSelector tSelector)
        {
            localPSelector = pSelector;
            localSSelector = sSelector;
            localTSelector = tSelector;
        }
        public void setLocalApTitle(string ApTitle, int AEQualifier)
        {
            localApTitleS = ApTitle;
            localApTitleLen = IsoUtil.BerEncoder_encodeOIDToBuffer(ApTitle, localApTitle, 10);
            localAEQualifier = AEQualifier;
        }

        public void setRemoteAddresses(uint pSelector, ushort sSelector, IsoCotp.TSelector tSelector)
        {
            remotePSelector = pSelector;
            remoteSSelector = sSelector;
            remoteTSelector = tSelector;
        }

        public void setRemoteApTitle(string ApTitle, int AEQualifier)
        {
            remoteApTitleS = ApTitle;
            remoteApTitleLen = IsoUtil.BerEncoder_encodeOIDToBuffer(ApTitle, remoteApTitle, 10);
            remoteAEQualifier = AEQualifier;
        }

        public string getRemoteApTitle()
        {
            return remoteApTitleS;
        }

        public string geLocalApTitle()
        {
            return localApTitleS;
        }

    }
}
