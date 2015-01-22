using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace IEDExplorer
{
    class IsoConnectionParameters
    {
        public IsoAcse.AcseAuthenticationParameter acseAuthParameter;

        public string hostname;
        public int port;

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

        public IsoConnectionParameters(IsoAcse.AcseAuthenticationParameter acseAuthPar)
        {
            init(acseAuthPar);
        }

        void init(IsoAcse.AcseAuthenticationParameter acseAuthPar)
        {
            // Defaults
            hostname = "localhost";
            port = 102;

            IsoCotp.TSelector selector1 = new IsoCotp.TSelector(2, 0);
            IsoCotp.TSelector selector2 = new IsoCotp.TSelector(2, 0);
            setLocalAddresses(1, 1, selector1);
            setLocalApTitle("1.1.1.999", 12);
            setRemoteAddresses(1, 1, selector2);
            setRemoteApTitle("1.1.1.999.1", 12);
            acseAuthParameter = acseAuthPar;
        }

        public IsoConnectionParameters(StringDictionary stringDictionary)
        {
            init(null);
            int remoteTSelectorVal = remoteTSelector.value;
            int remoteTSelectorSize = remoteTSelector.value;
            foreach (string key in stringDictionary.Keys)
            {
                switch (key.ToLower())
                {
                    case "hostname":
                        hostname = stringDictionary[key];
                        break;
                    case "port":
                        int.TryParse(stringDictionary[key], out port);
                        break;
                    case "remoteaptitle":
                        remoteApTitleS = stringDictionary[key];
                        remoteApTitleLen = IsoUtil.BerEncoder_encodeOIDToBuffer(remoteApTitleS, remoteApTitle, 10);
                        break;
                    case "remoteaequalifier":
                        int.TryParse(stringDictionary[key], out remoteAEQualifier);
                        break;
                    case "remotepselector":
                        uint.TryParse(stringDictionary[key], out remotePSelector);
                        break;
                    case "remotesselector":
                        ushort.TryParse(stringDictionary[key], out remoteSSelector);
                        break;
                    case "remotetselectorvalue":
                        int.TryParse(stringDictionary[key], out remoteTSelector.value);
                        break;
                    case "remotetselectorsize":
                        byte.TryParse(stringDictionary[key], out remoteTSelector.size);
                        break;
                    case "localaptitle":
                        localApTitleS = stringDictionary[key];
                        localApTitleLen = IsoUtil.BerEncoder_encodeOIDToBuffer(localApTitleS, localApTitle, 10);
                        break;
                    case "localaequalifier":
                        int.TryParse(stringDictionary[key], out localAEQualifier);
                        break;
                    case "localpselector":
                        uint.TryParse(stringDictionary[key], out localPSelector);
                        break;
                    case "localsselector":
                        ushort.TryParse(stringDictionary[key], out localSSelector);
                        break;
                    case "localtselectorvalue":
                        int.TryParse(stringDictionary[key], out localTSelector.value);
                        break;
                    case "localtselectorsize":
                        byte.TryParse(stringDictionary[key], out localTSelector.size);
                        break;
                    case "authenticationmechanism":
                        if (acseAuthParameter == null) acseAuthParameter = new IsoAcse.AcseAuthenticationParameter();
                        Enum.TryParse<IsoAcse.AcseAuthenticationMechanism>(stringDictionary[key], out acseAuthParameter.mechanism);
                        break;
                    case "authenticationpassword":
                        if (acseAuthParameter == null) acseAuthParameter = new IsoAcse.AcseAuthenticationParameter();
                        acseAuthParameter.password = stringDictionary[key];
                        acseAuthParameter.paswordOctetString = Encoding.ASCII.GetBytes(acseAuthParameter.password);
                        acseAuthParameter.passwordLength = acseAuthParameter.paswordOctetString.Length;
                        break;
                }
            }
        }

        public void Save(StringDictionary stringDictionary)
        {
            if (stringDictionary == null) stringDictionary = new StringDictionary();
            stringDictionary.Clear();
            stringDictionary.Add("hostname", hostname);
            stringDictionary.Add("port", port.ToString());
            stringDictionary.Add("remoteApTitle", remoteApTitleS);
            stringDictionary.Add("remoteAEQualifier", remoteAEQualifier.ToString());
            stringDictionary.Add("remotePSelector", remotePSelector.ToString());
            stringDictionary.Add("remoteSSelector", remoteSSelector.ToString());
            stringDictionary.Add("remoteTSelectorValue", remoteTSelector.value.ToString());
            stringDictionary.Add("remoteTSelectorSize", remoteTSelector.size.ToString());
            stringDictionary.Add("localApTitle", localApTitleS);
            stringDictionary.Add("localAEQualifier", localAEQualifier.ToString());
            stringDictionary.Add("localPSelector", localPSelector.ToString());
            stringDictionary.Add("localSSelector", localSSelector.ToString());
            stringDictionary.Add("localTSelectorValue", localTSelector.value.ToString());
            stringDictionary.Add("localTSelectorSize", localTSelector.size.ToString());
            if (acseAuthParameter != null)
            {
                stringDictionary.Add("authenticationMechanism", acseAuthParameter.mechanism.ToString());
                if (acseAuthParameter.mechanism == IsoAcse.AcseAuthenticationMechanism.ACSE_AUTH_PASSWORD)
                    stringDictionary.Add("authenticationPassword", acseAuthParameter.password);
            }
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
