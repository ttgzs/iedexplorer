using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class IsoSess
    {
        ushort callingSessionSelector;
        ushort calledSessionSelector;
        ushort sessionRequirement;
        byte protocolOptions;
        int userDataIndex = 0;

        public int UserDataIndex { get { return userDataIndex; } }

        Iec61850State iecs;

        public IsoSess(Iec61850State iec)
        {
            iecs = iec;
        }

        public enum IsoSessionIndication
        {
            SESSION_OK,
            SESSION_ERROR,
            SESSION_CONNECT,
            SESSION_GIVE_TOKEN,
            SESSION_DATA,
            SESSION_ABORT,
            SESSION_FINISH,
            SESSION_DISCONNECT,
            SESSION_NOT_FINISHED
        }

        int parseAcceptParameters(byte[] buffer, int startOffset, int parameterLength)
        {
            byte pi;
            byte param_len;
            byte param_val;
            byte hasProtocolOptions = 0;
            byte hasProtocolVersion = 0;
            int offset = startOffset;
            int maxOffset = offset + parameterLength;

            while (offset < maxOffset)
            {
                pi = buffer[offset++];
                param_len = buffer[offset++];

                switch (pi)
                {
                    case 19: /* Protocol options */
                        if (param_len != 1)
                            return -1;
                        protocolOptions = buffer[offset++];
                        iecs.logger.LogDebug(String.Format("SESSION: Param - Protocol Options: 0x{0:X2}", protocolOptions));
                        hasProtocolOptions = 1;
                        break;
                    case 21: /* TSDU Maximum Size */
                        iecs.logger.LogDebug("SESSION: Param - TODO TSDU Maximum Size");
                        offset += 4;
                        break;
                    case 22: /* Version Number */
                        param_val = buffer[offset++];
                        iecs.logger.LogDebug("SESSION: Param - Version number");
                        if (param_val != 2)
                            return -1;
                        hasProtocolVersion = 1;
                        break;
                    case 23: /* Initial Serial Number */
                        iecs.logger.LogDebug("SESSION: Param - TODO Initial Serial Number");
                        offset += param_len;
                        break;
                    case 26: /* Token Setting Item */
                        param_val = buffer[offset++];
                        iecs.logger.LogDebug(String.Format("SESSION: Param - Token Setting Item: 0x{0:X2}", param_val));
                        break;
                    case 55: /* Second Initial Serial Number */
                        iecs.logger.LogDebug("SESSION: Param - TODO Second Initial Serial Number");
                        offset += param_len;
                        break;
                    case 56: /* Upper Limit Serial Number */
                        iecs.logger.LogDebug("SESSION: Param - TODO Upper Limit Serial Number");
                        offset += param_len;
                        break;
                    case 57: /* Large Initial Serial Number */
                        iecs.logger.LogDebug("SESSION: Param - TODO Large Initial Serial Number");
                        offset += param_len;
                        break;
                    case 58: /* Large Second Initial Serial Number */
                        iecs.logger.LogDebug("SESSION: Param - TODO Large Second Initial Serial Number");
                        offset += param_len;
                        break;
                    default:
                        iecs.logger.LogDebug(String.Format("SESSION: Param - Invalid Parameter with ID 0x{0:X2}", pi));
                        break;
                }
            }

            if (hasProtocolOptions > 0 && hasProtocolVersion > 0)
                return offset - startOffset;
            else
                return -1;
        }

        IsoSessionIndication parseSessionHeaderParameters(byte[] buffer, int parametersOctets)
        {
            int offset = 2;
            byte pgi;
            byte parameterLength;

            while (offset < (parametersOctets + 2))
            {
                pgi = buffer[offset++];
                parameterLength = buffer[offset++];

                switch (pgi)
                {
                    case 1: /* Connection Identifier */
                        iecs.logger.LogDebug("SESSION: PGI - connection identifier");

                        offset += parameterLength;
                        break;
                    case 5: /* Connection/Accept Item */
                        iecs.logger.LogDebug("SESSION: PGI - Connection/Accept Item");

                        int connectAcceptLen;

                        connectAcceptLen = parseAcceptParameters(buffer, offset, parameterLength);

                        if (connectAcceptLen == -1)
                            return IsoSessionIndication.SESSION_ERROR;

                        offset += connectAcceptLen;
                        break;
                    case 17: /* Transport disconnect */
                        offset += parameterLength;
                        break;
                    case 20: /* Session User Requirements */
                        iecs.logger.LogDebug("SESSION: Parameter - Session User Req");
                        if (parameterLength != 2)
                            return IsoSessionIndication.SESSION_ERROR;

                        sessionRequirement = (ushort)(buffer[offset++] * 0x100);
                        sessionRequirement += buffer[offset++];
                        break;
                    case 25: /* Enclosure item */
                        offset += parameterLength;
                        break;
                    case 49:
                        offset += parameterLength;
                        break;
                    case 51: /* Calling Session Selector */
                        iecs.logger.LogDebug("SESSION: Parameter - Calling Session Selector");

                        if (parameterLength != 2)
                            return IsoSessionIndication.SESSION_ERROR;

                        callingSessionSelector = (ushort)(buffer[offset++] * 0x100);
                        callingSessionSelector += buffer[offset++];
                        break;
                    case 52: /* Called Session Selector */
                        iecs.logger.LogDebug("SESSION: Parameter - Called Session Selector");

                        if (parameterLength != 2)
                            return IsoSessionIndication.SESSION_ERROR;

                        calledSessionSelector = (ushort)(buffer[offset++] * 0x100);
                        calledSessionSelector += buffer[offset++];
                        break;
                    case 60: /* Data Overflow */
                        iecs.logger.LogDebug("SESSION: Parameter - Data Overflow");
                        offset += parameterLength;
                        break;
                    case 193: /* User Data */
                        iecs.logger.LogDebug("SESSION: PGI - user data");

                        /* here we should return - the remaining data is for upper layers ! */
                        /*ByteBuffer_wrap(&session->userData, message->buffer + offset,
                                message->size - offset, message->maxSize - offset);*/
                        userDataIndex = offset;
                        return IsoSessionIndication.SESSION_OK;

                    case 194: /* Extended User Data */
                        iecs.logger.LogDebug("SESSION: PGI - extended user data");
                        break;
                    default:
                        iecs.logger.LogDebug("SESSION: invalid parameter/PGI");
                        break;
                }
            }

            return IsoSessionIndication.SESSION_ERROR;
        }

        byte[] dataSpdu = { 0x01, 0x00, 0x01, 0x00 };

        public int createDataSpdu(byte[] buffer, int offset)
        {
            dataSpdu.CopyTo(buffer, offset);
            return offset + 4;
        }

        int encodeConnectAcceptItem(byte[] buf, int offset, byte options)
        {
            buf[offset++] = 5;
            buf[offset++] = 6;
            buf[offset++] = 0x13; /* Protocol Options */
            buf[offset++] = 1;
            buf[offset++] = options;
            buf[offset++] = 0x16; /* Version Number */
            buf[offset++] = 1;
            buf[offset++] = 2; /* Version = 2 */

            return offset;
        }

        int encodeSessionRequirement(byte[] buf, int offset)
        {
            buf[offset++] = 0x14;
            buf[offset++] = 2;
            buf[offset++] = (byte)(sessionRequirement / 0x100);
            buf[offset++] = (byte)(sessionRequirement & 0x00ff);

            return offset;
        }

        int encodeCallingSessionSelector(byte[] buf, int offset)
        {
            buf[offset++] = 0x33;
            buf[offset++] = 2;
            buf[offset++] = (byte)(callingSessionSelector / 0x100);
            buf[offset++] = (byte)(callingSessionSelector & 0x00ff);

            return offset;
        }

        int encodeCalledSessionSelector(byte[] buf, int offset)
        {
            buf[offset++] = 0x34;
            buf[offset++] = 2;
            buf[offset++] = (byte)(calledSessionSelector / 0x100);
            buf[offset++] = (byte)(calledSessionSelector & 0x00ff);

            return offset;
        }

        int encodeSessionUserData(byte[] buf, int offset, byte payloadLength)
        {
            buf[offset++] = 0xc1;
            buf[offset++] = payloadLength;

            return offset;
        }

        public int createConnectSpdu(IsoConnectionParameters isoParameters, byte[] buffer, byte[] payload, int payloadLength)
        {
            int offset = 0;
            int lengthOffset;

            buffer[offset++] = 13; /* CONNECT SPDU */
            lengthOffset = offset;
            offset++; /* Skip byte for length - fill it later */

            calledSessionSelector = isoParameters.remoteSSelector;
            callingSessionSelector = isoParameters.localSSelector;

            offset = encodeConnectAcceptItem(buffer, offset, 0);

            offset = encodeSessionRequirement(buffer, offset);

            offset = encodeCallingSessionSelector(buffer, offset);

            offset = encodeCalledSessionSelector(buffer, offset);

            offset = encodeSessionUserData(buffer, offset, (byte)payloadLength);

            int spduLength = (offset - lengthOffset - 1) + payloadLength;

            buffer[lengthOffset] = (byte)spduLength;

            Array.Copy(payload, 0, buffer, offset, payloadLength); 

            return payloadLength + offset;
        }

        public int createAbortSpdu(byte[] buffer, byte[] payload, byte payloadLength)
        {
            int offset = 0;

            buffer[offset++] = 25; /* ABORT-SPDU code */
            buffer[offset++] = (byte)(5 + payloadLength); /* LI */
            buffer[offset++] = 17; /* PI-Code transport-disconnect */
            buffer[offset++] = 1; /* LI = 1 */
            buffer[offset++] = 11; /* transport-connection-released | user-abort | no-reason */
            buffer[offset++] = 193; /* PGI-Code user data */
            buffer[offset++] = payloadLength; /* LI of user data */

            Array.Copy(payload, 0, buffer, offset, payloadLength);
            return payloadLength + offset;
        }

        public int createFinishSpdu(byte[] buffer, byte[] payload, byte payloadLength)
        {
            int offset = 0;

            buffer[offset++] = 9; /* FINISH-SPDU code */

            buffer[offset++] = (byte)(5 + payloadLength); /* LI */
            buffer[offset++] = 17; /* PI-Code transport-disconnect */
            buffer[offset++] = 1; /* LI = 1 */
            buffer[offset++] = 2; /* transport-connection-released */
            buffer[offset++] = 193; /* PGI-Code user data */
            buffer[offset++] = payloadLength; /* LI of user data */

            Array.Copy(payload, 0, buffer, offset, payloadLength);
            return payloadLength + offset;
        }

        public int createDisconnectSpdu(byte[] buffer, byte[] payload, byte payloadLength)
        {
            int offset = 0;

            buffer[offset++] = 10; /* DISCONNECT-SPDU code */

            buffer[offset++] = (byte)(2 + payloadLength); /* LI */
            buffer[offset++] = 193; /* PGI-Code user data */
            buffer[offset++] = payloadLength; /* LI of user data */

            Array.Copy(payload, 0, buffer, offset, payloadLength);
            return payloadLength + offset;
        }

        public int IsoSession_createAcceptSpdu(byte[] buffer, byte[] payload, byte payloadLength)
        {
            int offset = 0;
            int lengthOffset;

            buffer[offset++] = 14; /* ACCEPT SPDU */
            lengthOffset = offset;
            offset++;

            offset = encodeConnectAcceptItem(buffer, offset, protocolOptions);

            offset = encodeSessionRequirement(buffer, offset);

            offset = encodeCalledSessionSelector(buffer, offset);

            offset = encodeSessionUserData(buffer, offset, payloadLength);

            int spduLength = (offset - lengthOffset - 1) + payloadLength;

            buffer[lengthOffset] = (byte)spduLength;

            Array.Copy(payload, 0, buffer, offset, payloadLength);
            return payloadLength + offset;
        }

        void init()
        {
            sessionRequirement = 0x0002; /* default = duplex functional unit */
            callingSessionSelector = 0x0001;
            calledSessionSelector = 0x0001;
        }

        int getUserDataIndex()
        {
            return userDataIndex;
        }

        public IsoSessionIndication parseMessage(byte[] buffer, int messageLength)
        {
            byte id;
            byte length;

            if (messageLength > 1)
            {
                id = buffer[0];
                length = buffer[1];
            }
            else
                return IsoSessionIndication.SESSION_ERROR;

            switch (id)
            {
                case 13: /* CONNECT(CN) SPDU */
                    if (length != (messageLength - 2))
                        return IsoSessionIndication.SESSION_ERROR;
                    if (parseSessionHeaderParameters(buffer, length) == IsoSessionIndication.SESSION_OK)
                        return IsoSessionIndication.SESSION_CONNECT;
                    else
                    {
                        iecs.logger.LogDebug("SESSION: error parsing connect spdu");
                        return IsoSessionIndication.SESSION_ERROR;
                    }
                case 14: /* ACCEPT SPDU */
                    if (length != (messageLength - 2))
                        return IsoSessionIndication.SESSION_ERROR;
                    if (parseSessionHeaderParameters(buffer, length) == IsoSessionIndication.SESSION_OK)
                        return IsoSessionIndication.SESSION_CONNECT;
                    else
                    {
                        iecs.logger.LogDebug("SESSION: error parsing accept spdu");
                        return IsoSessionIndication.SESSION_ERROR;
                    }
                case 1: /* Give token / data SPDU */
                    if (messageLength < 4)
                        return IsoSessionIndication.SESSION_ERROR;

                    if ((length == 0) && (buffer[2] == 1) && (buffer[3] == 0))
                    {
                        /*ByteBuffer_wrap(&session->userData, message->buffer + 4, message->size - 4, message->maxSize - 4);*/
                        // ???????
                        userDataIndex = 4;
                        return IsoSessionIndication.SESSION_DATA;
                    }
                    return IsoSessionIndication.SESSION_ERROR;

                case 8: /* NOT-FINISHED SPDU */
                    return IsoSessionIndication.SESSION_NOT_FINISHED;

                case 9: /* FINISH SPDU */
                    iecs.logger.LogDebug("SESSION: recvd FINISH SPDU");

                    if (length != (messageLength - 2))
                        return IsoSessionIndication.SESSION_ERROR;

                    if (parseSessionHeaderParameters(buffer, length) == IsoSessionIndication.SESSION_OK)
                        return IsoSessionIndication.SESSION_FINISH;
                    else
                        return IsoSessionIndication.SESSION_ERROR;
                case 10: /* DISCONNECT SPDU */
                    iecs.logger.LogDebug("SESSION: recvd DISCONNECT SPDU");

                    if (length != (messageLength - 2))
                        return IsoSessionIndication.SESSION_ERROR;

                    if (parseSessionHeaderParameters(buffer, length) == IsoSessionIndication.SESSION_OK)
                        return IsoSessionIndication.SESSION_DISCONNECT;
                    else
                        return IsoSessionIndication.SESSION_ERROR;
                case 25: /* ABORT SPDU */
                    return IsoSessionIndication.SESSION_ABORT;

                default:
                    break;
            }

            return IsoSessionIndication.SESSION_ERROR;
        }

    }
}
