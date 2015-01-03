using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class OsiAcse
    {
        public enum AcseAuthenticationMechanism
        {
            ACSE_AUTH_NONE = 0,
            ACSE_AUTH_PASSWORD = 1
        }

        public struct sAcseAuthenticationParameter
        {
            AcseAuthenticationMechanism mechanism;
            byte[] paswordOctetString;
            int passwordLength;
        }

        byte[] appContextNameMms = { 0x28, 0xca, 0x22, 0x02, 0x03 };

        byte[] auth_mech_password_oid = { 0x52, 0x03, 0x01 };

        byte[] requirements_authentication = { 0x80 };

        AcseAuthenticationMechanism aAuthenticationMechanism = AcseAuthenticationMechanism.ACSE_AUTH_NONE;

        public OsiAcse()
        {

        }

        public int Receive(Iec61850State iecs)
        {
            int ret = 0;


            if (ret == 0)
                iecs.ostate = OsiProtocolState.OSI_CONNECTED;
            else
                iecs.ostate = OsiProtocolState.OSI_STATE_SHUTDOWN;
            return ret;
        }

        public int Send(Iec61850State iecs)
        {
            return 0;
        }

    }
}
