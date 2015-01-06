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

        public struct AcseAuthenticationParameter
        {
            public AcseAuthenticationMechanism mechanism;
            public byte[] paswordOctetString;
            public int passwordLength;
        }

        byte[] appContextNameMms = { 0x28, 0xca, 0x22, 0x02, 0x03 };

        byte[] auth_mech_password_oid = { 0x52, 0x03, 0x01 };

        byte[] requirements_authentication = { 0x80 };

        AcseAuthenticationMechanism aAuthenticationMechanism = AcseAuthenticationMechanism.ACSE_AUTH_NONE;

enum AcseConnectionState
{
    idle, requestIndicated, connected
}

enum AcseIndication
{
    ACSE_ERROR,
    ACSE_ASSOCIATE,
    ACSE_ASSOCIATE_FAILED,
    ACSE_OK,
    ACSE_ABORT,
    ACSE_RELEASE_REQUEST,
    ACSE_RELEASE_RESPONSE
}

    AcseConnectionState state;
    long nextReference;
    int userDataBufferIndex;
    int userDataBufferSize;
    AcseAuthenticationParameter authentication;
    string password;

    const int ACSE_RESULT_ACCEPT = 0;
    const int ACSE_RESULT_REJECT_PERMANENT = 1;
    const int ACSE_RESULT_REJECT_TRANSIENT = 2;

    Iec61850State iecs;
    Logger logger;

    public OsiAcse(Iec61850State iec)
    {
        iecs = iec;
        logger = iecs.logger;
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

        AcseAuthenticationMechanism checkAuthMechanismName(byte[] buffer, int authMechanismPos, int authMechLen)
        {
            AcseAuthenticationMechanism authenticationMechanism = AcseAuthenticationMechanism.ACSE_AUTH_NONE;

            if (buffer != null )
            {
                if (authMechLen == 3)
                {
                    //if (memcmp(auth_mech_password_oid, authMechanism, 3) == 0) {
                    authenticationMechanism = AcseAuthenticationMechanism.ACSE_AUTH_PASSWORD;
                    for (int i = 0; i < 3; i++)
                        if (buffer[authMechanismPos + i] != auth_mech_password_oid[i])
                            authenticationMechanism = AcseAuthenticationMechanism.ACSE_AUTH_NONE;
                }
            }
            return authenticationMechanism;
        }

bool authenticateClient(AcseAuthenticationMechanism mechanism, byte[] buffer, int authValuePos, int authValueLen, byte[] password)
{
    if (mechanism == AcseAuthenticationMechanism.ACSE_AUTH_PASSWORD) {
        for (int i = 0; i < authValueLen; i++)
            if (buffer[authValuePos + i] != password[i])
                return false;
            }
    return true;
    //return authenticator(authenticatorParameter, authParameter, &(securityToken));
}

bool checkAuthentication(byte[] buffer, int authMechanismPos, int authMechLen, int authValuePos, int authValueLen)
{
    if (password != null && password != "" ) {
        AcseAuthenticationMechanism mechanism = checkAuthMechanismName(buffer, authMechanismPos, authMechLen);
        return authenticateClient(mechanism, buffer, authValuePos, authValueLen, Encoding.ASCII.GetBytes(password));
    }
    else
        return true;
}

int parseUserInformation(byte[] buffer, int bufPos, int maxBufPos, ref bool userInfoValid)
{
	logger.LogDebug(String.Format("ACSE: parseUserInformation {0} {1}", bufPos, maxBufPos));

	bool hasindirectReference = false;
	bool isDataValid = false;

	while (bufPos < maxBufPos) {
		byte tag = buffer[bufPos++];
		int len;

		bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, maxBufPos);

		switch (tag) {

		case 0x02: /* indirect-reference */
			nextReference = OsiUtil.BerDecoder_decodeUint32(buffer, len, bufPos);
			bufPos += len;
			hasindirectReference = true;
			break;

		case 0xa0: /* encoding */
			isDataValid = true;

			userDataBufferSize = len;
            userDataBufferIndex = bufPos;

			bufPos += len;

			break;

		default: /* ignore unknown tag */
			bufPos += len;
            break;
		}
	}


		if (!hasindirectReference) logger.LogDebug("ACSE: User data has no indirect reference!");

		if (!isDataValid) logger.LogDebug("ACSE: No valid user data");

	if (hasindirectReference && isDataValid)
		userInfoValid = true;
	else
		userInfoValid = false;

	return bufPos;
}

AcseIndication parseAarePdu(byte[] buffer, int bufPos, int maxBufPos)
{
	logger.LogDebug("ACSE: parse AARE PDU");

	bool userInfoValid = false;

	uint result = 99;

	while (bufPos < maxBufPos) {
		byte tag = buffer[bufPos++];
		int len;

		bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, maxBufPos);

		switch (tag) {
		case 0xa1: /* application context name */
			bufPos += len;
			break;

		case 0xa2: /* result */
			bufPos++;

			bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, maxBufPos);
			result = OsiUtil.BerDecoder_decodeUint32(buffer, len, bufPos);

			bufPos += len;
			break;

		case 0xa3: /* result source diagnostic */
			bufPos += len;
			break;

		case 0xbe: /* user information */
			if (buffer[bufPos]  != 0x28) {
				logger.LogDebug("ACSE: invalid user info");
				bufPos += len;
			}
			else {
				bufPos++;

				bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, maxBufPos);

				bufPos = parseUserInformation(buffer, bufPos, bufPos + len, ref userInfoValid);
			}
			break;

		default: /* ignore unknown tag */
            logger.LogDebug(String.Format("ACSE: parseAarePdu: unknown tag 0x{0:X2}", tag));

			bufPos += len;
			break;
		}
	}

	if (!userInfoValid)
        return AcseIndication.ACSE_ERROR;

	if (result != 0)
        return AcseIndication.ACSE_ASSOCIATE_FAILED;

    return AcseIndication.ACSE_ASSOCIATE;
}

AcseIndication parseAarqPdu(byte[] buffer, int bufPos, int maxBufPos)
{
	logger.LogDebug("ACSE: parse AARQ PDU\n");

	int authValuePos = 0;
	int authValueLen = 0;
	int authMechanismPos = 0;
	int authMechLen = 0;
	bool userInfoValid = false;

	while (bufPos < maxBufPos) {
		byte tag = buffer[bufPos++];
		int len;

		bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, maxBufPos);

		if (bufPos < 0) {
		    logger.LogDebug("ACSE: parseAarqPdu: user info invalid!\n");
            return AcseIndication.ACSE_ASSOCIATE_FAILED;
		}

		switch (tag) {
		case 0xa1: /* application context name */
			bufPos += len;
			break;

		case 0xa2: /* called AP title */
			bufPos += len;
			break;
		case 0xa3: /* called AE qualifier */
			bufPos += len;
			break;

		case 0xa6: /* calling AP title */
			bufPos += len;
			break;

		case 0xa7: /* calling AE qualifier */
			bufPos += len;
			break;

		case 0x8a: /* sender ACSE requirements */
            bufPos += len;
			break;

		case 0x8b: /* (authentication) mechanism name */
			authMechLen = len;
			authMechanismPos = bufPos;
			bufPos += len;
			break;

		case 0xac: /* authentication value */
			bufPos++;
			bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, maxBufPos);
			authValueLen = len;
			authValuePos = bufPos;
			bufPos += len;
			break;

		case 0xbe: /* user information */
			if (buffer[bufPos]  != 0x28) {
				logger.LogDebug("ACSE: invalid user info\n");
				bufPos += len;
			}
			else {
				bufPos++;

				bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, maxBufPos);

				bufPos = parseUserInformation(buffer, bufPos, bufPos + len, ref userInfoValid);
			}
			break;

		default: /* ignore unknown tag */
		    logger.LogDebug(String.Format("ACSE: parseAarqPdu: unknown tag 0x{0:X2}", tag));

			bufPos += len;
			break;
		}
	}

    if (checkAuthentication(buffer, authMechanismPos, authMechLen, authValuePos, authValueLen) == false) {
        logger.LogDebug("ACSE: parseAarqPdu: check authentication failed!");

        return AcseIndication.ACSE_ASSOCIATE_FAILED;
    }

    if (userInfoValid == false) {
        logger.LogDebug("ACSE: parseAarqPdu: user info invalid!");

        return AcseIndication.ACSE_ASSOCIATE_FAILED;
    }

    return AcseIndication.ACSE_ASSOCIATE;
}

public void init()
{
    state = AcseConnectionState.idle;
    nextReference = 0;
    userDataBufferIndex = 0;
    userDataBufferSize = 0;
}

public AcseIndication AcseConnection_parseMessage(byte[] buffer, int length)
{
    AcseIndication indication;

    int messageSize = length;

    int bufPos = 0;

    byte messageType = buffer[bufPos++];

    int len = 0;

    bufPos = OsiUtil.BerDecoder_decodeLength(buffer, ref len, bufPos, messageSize);

    if (bufPos < 0) {
        logger.LogDebug("ACSE: AcseConnection_parseMessage: invalid ACSE message!");

        return AcseIndication.ACSE_ERROR;
    }

    switch (messageType) {
    case 0x60:
    	indication = parseAarqPdu(buffer, bufPos, messageSize);
    	break;
    case 0x61:
    	indication = parseAarePdu(buffer, bufPos, messageSize);
		break;
    case 0x62: /* A_RELEASE.request RLRQ-apdu */
        indication = AcseIndication.ACSE_RELEASE_REQUEST;
        break;
    case 0x63: /* A_RELEASE.response RLRE-apdu */
        indication = AcseIndication.ACSE_RELEASE_RESPONSE;
        break;
    case 0x64: /* A_ABORT */
        indication = AcseIndication.ACSE_ABORT;
        break;
    default:
    	logger.LogDebug("ACSE: Unknown ACSE message\n");
        indication = AcseIndication.ACSE_ERROR;
    	break;
    }

    return indication;
}

public int AcseConnection_createAssociateFailedMessage(byte[] buffer, int bufPos)
{
    return AcseConnection_createAssociateResponseMessage(ACSE_RESULT_REJECT_PERMANENT, buffer, bufPos, null, 0);
}

public int AcseConnection_createAssociateResponseMessage(byte acseResult, byte[] buffer, int bufIndex, byte[] payload, int payloadLength) 
{
    int appContextLength = 9;
    int resultLength = 5;
    int resultDiagnosticLength = 5;

    int fixedContentLength = appContextLength + resultLength + resultDiagnosticLength;

    int variableContentLength = 0;

    int assocDataLength;
    int userInfoLength;
    int nextRefLength;

    /* single ASN1 type tag */
    variableContentLength += payloadLength;
    variableContentLength += 1;
    variableContentLength += OsiUtil.BerEncoder_determineLengthSize((uint)payloadLength);

    /* indirect reference */
    nextRefLength = OsiUtil.BerEncoder_UInt32determineEncodedSize((uint)nextReference);
    variableContentLength += nextRefLength;
    variableContentLength += 2;

    /* association data */
    assocDataLength = variableContentLength;
    variableContentLength += OsiUtil.BerEncoder_determineLengthSize((uint)assocDataLength);
    variableContentLength += 1;

    /* user information */
    userInfoLength = variableContentLength;
    variableContentLength += OsiUtil.BerEncoder_determineLengthSize((uint)userInfoLength);
    variableContentLength += 1;

    variableContentLength += 2;

    int contentLength = fixedContentLength + variableContentLength;

    int bufPos = 0;

    bufPos = OsiUtil.BerEncoder_encodeTL(0x61, (uint)contentLength, buffer, bufPos);

    /* application context name */
    bufPos = OsiUtil.BerEncoder_encodeTL(0xa1, 7, buffer, bufPos);
    bufPos = OsiUtil.BerEncoder_encodeTL(0x06, 5, buffer, bufPos);
    //memcpy(buffer + bufPos, appContextNameMms, 5);
    appContextNameMms.CopyTo(buffer, bufPos);
    bufPos += 5;

    /* result */
    bufPos = OsiUtil.BerEncoder_encodeTL(0xa2, 3, buffer, bufPos);
    bufPos = OsiUtil.BerEncoder_encodeTL(0x02, 1, buffer, bufPos);
    buffer[bufPos++] = acseResult;

    /* result source diagnostics */
    bufPos = OsiUtil.BerEncoder_encodeTL(0xa3, 5, buffer, bufPos);
    bufPos = OsiUtil.BerEncoder_encodeTL(0xa1, 3, buffer, bufPos);
    bufPos = OsiUtil.BerEncoder_encodeTL(0x02, 1, buffer, bufPos);
    buffer[bufPos++] = 0;

    /* user information */
    bufPos = OsiUtil.BerEncoder_encodeTL(0xbe, (uint)userInfoLength, buffer, bufPos);

    /* association data */
    bufPos = OsiUtil.BerEncoder_encodeTL(0x28, (uint)assocDataLength, buffer, bufPos);

    /* indirect-reference */
    bufPos = OsiUtil.BerEncoder_encodeTL(0x02, (uint)nextRefLength, buffer, bufPos);
    bufPos = OsiUtil.BerEncoder_encodeUInt32((uint)nextReference, buffer, bufPos);

    /* single ASN1 type */
    bufPos = OsiUtil.BerEncoder_encodeTL(0xa0, (uint)payloadLength, buffer, bufPos);

    /*writeBuffer->partLength = bufPos;
    writeBuffer->length = bufPos + payloadLength;
    writeBuffer->nextPart = payload;*/

    payload.CopyTo(buffer, bufPos);
    return bufPos + payloadLength;
}

void
AcseConnection_createAssociateRequestMessage(AcseConnection* self,
        IsoConnectionParameters isoParameters,
        BufferChain writeBuffer,
        BufferChain payload,
        AcseAuthenticationParameter authParameter)
{
    assert(self != NULL);
    assert(writeBuffer != NULL);
    assert(payload != NULL);

	int payloadLength = payload->length;
	int authValueLength;
	int authValueStringLength = 0;

	int passwordLength = 0;

	int contentLength = 0;

	/* application context name */
	contentLength += 9;

	int calledAEQualifierLength = 0;

	if (isoParameters->remoteApTitleLen > 0) {

        /* called AP title */
        contentLength += (4 + isoParameters->remoteApTitleLen);

        calledAEQualifierLength = OsiUtil.BerEncoder_UInt32determineEncodedSize(isoParameters->remoteAEQualifier);

        /* called AP qualifier */
        contentLength += (4 + calledAEQualifierLength);
	}

	int callingAEQualifierLength = 0;

	if (isoParameters->localApTitleLen > 0) {
        /* calling AP title */
        contentLength += (4 + isoParameters->localApTitleLen);

        callingAEQualifierLength = OsiUtil.BerEncoder_UInt32determineEncodedSize(isoParameters->localAEQualifier);

        /* calling AP qualifier */
        contentLength += (4 + callingAEQualifierLength);
	}

	if (authParameter != NULL) {

		/* sender ACSE requirements */
		contentLength += 4;

		/* mechanism name */
		contentLength += 5;

		/* authentication value */
		if (authParameter->mechanism == ACSE_AUTH_PASSWORD) {
			contentLength += 2;

			//if (authParameter->value.password.passwordLength == 0)

			passwordLength = authParameter->value.password.passwordLength;

			authValueStringLength = OsiUtil.BerEncoder_determineLengthSize(passwordLength);

			contentLength += passwordLength  + authValueStringLength;

			authValueLength = OsiUtil.BerEncoder_determineLengthSize(passwordLength + authValueStringLength + 1);

			contentLength += authValueLength;
		}
		else {
			contentLength += 2;
		}
	}

	/* user information */
	int userInfoLength = 0;

	/* single ASN1 type tag */
	userInfoLength += payloadLength;
	userInfoLength += 1;
	userInfoLength += OsiUtil.BerEncoder_determineLengthSize(payloadLength);

	/* indirect reference */
	userInfoLength += 1;
	userInfoLength += 2;

	/* association data */
	int assocDataLength = userInfoLength;
	userInfoLength += OsiUtil.BerEncoder_determineLengthSize(assocDataLength);
	userInfoLength += 1;

	/* user information */
	int userInfoLen = userInfoLength;
	userInfoLength += OsiUtil.BerEncoder_determineLengthSize(userInfoLength);
	userInfoLength += 1;

	contentLength += userInfoLength;

	byte[] buffer = writeBuffer->buffer;
	int bufPos = 0;

	bufPos = OsiUtil.BerEncoder_encodeTL(0x60, contentLength, buffer, bufPos);

	/* application context name */
	bufPos = OsiUtil.BerEncoder_encodeTL(0xa1, 7, buffer, bufPos);
	bufPos = OsiUtil.BerEncoder_encodeTL(0x06, 5, buffer, bufPos);
	memcpy(buffer + bufPos, appContextNameMms, 5);
	bufPos += 5;

	if (isoParameters->remoteApTitleLen > 0) {

	    /* called AP title */
        bufPos = OsiUtil.BerEncoder_encodeTL(0xa2, isoParameters->remoteApTitleLen + 2, buffer, bufPos);
        bufPos = OsiUtil.BerEncoder_encodeTL(0x06, isoParameters->remoteApTitleLen, buffer, bufPos);

        memcpy(buffer + bufPos, isoParameters->remoteApTitle, isoParameters->remoteApTitleLen);
	    bufPos += isoParameters->remoteApTitleLen;

        /* called AE qualifier */
        bufPos = OsiUtil.BerEncoder_encodeTL(0xa3, calledAEQualifierLength + 2, buffer, bufPos);
        bufPos = OsiUtil.BerEncoder_encodeTL(0x02, calledAEQualifierLength, buffer, bufPos);
        bufPos = OsiUtil.BerEncoder_encodeUInt32(isoParameters->remoteAEQualifier, buffer, bufPos);
	}

	if (isoParameters->localApTitleLen > 0) {
        /* calling AP title */
        bufPos = OsiUtil.BerEncoder_encodeTL(0xa6, isoParameters->localApTitleLen + 2, buffer, bufPos);
        bufPos = OsiUtil.BerEncoder_encodeTL(0x06, isoParameters->localApTitleLen, buffer, bufPos);
        memcpy(buffer + bufPos, isoParameters->localApTitle, isoParameters->localApTitleLen);
        bufPos += isoParameters->localApTitleLen;

        /* calling AE qualifier */
        bufPos = OsiUtil.BerEncoder_encodeTL(0xa7, callingAEQualifierLength + 2, buffer, bufPos);
        bufPos = OsiUtil.BerEncoder_encodeTL(0x02, callingAEQualifierLength, buffer, bufPos);
        bufPos = OsiUtil.BerEncoder_encodeUInt32(isoParameters->localAEQualifier, buffer, bufPos);
	}

	if (authParameter != NULL) {
		/* sender requirements */
		bufPos = OsiUtil.BerEncoder_encodeTL(0x8a, 2, buffer, bufPos);
		buffer[bufPos++] = 0x04;

		if (authParameter->mechanism == ACSE_AUTH_PASSWORD) {
		    buffer[bufPos++] = requirements_authentication[0];

			bufPos = OsiUtil.BerEncoder_encodeTL(0x8b, 3, buffer, bufPos);
			memcpy(buffer + bufPos, auth_mech_password_oid, 3);
			bufPos += 3;

			/* authentication value */
			bufPos = OsiUtil.BerEncoder_encodeTL(0xac, authValueStringLength + passwordLength + 1, buffer, bufPos);
			bufPos = OsiUtil.BerEncoder_encodeTL(0x80, passwordLength, buffer, bufPos);
			memcpy(buffer + bufPos, authParameter->value.password.octetString, passwordLength);
			bufPos += passwordLength;
		}
		else { /* AUTH_NONE */
		    buffer[bufPos++] = 0;
		}
	}

    /* user information */
    bufPos = OsiUtil.BerEncoder_encodeTL(0xbe, userInfoLen, buffer, bufPos);

    /* association data */
    bufPos = OsiUtil.BerEncoder_encodeTL(0x28, assocDataLength, buffer, bufPos);

    /* indirect-reference */
    bufPos = OsiUtil.BerEncoder_encodeTL(0x02, 1, buffer, bufPos);
    buffer[bufPos++] = 3;

    /* single ASN1 type */
    bufPos = OsiUtil.BerEncoder_encodeTL(0xa0, payloadLength, buffer, bufPos);

	writeBuffer->partLength = bufPos;
	writeBuffer->length = bufPos + payload->length;
	writeBuffer->nextPart = payload;
}

/**
 * \param isProvider specifies abort source (false = user/client; true = provider/server)
 */
void
AcseConnection_createAbortMessage(AcseConnection* self, BufferChain writeBuffer, bool isProvider)
{
    byte[] buffer = writeBuffer->buffer;

    buffer[0] = 0x64; /* [APPLICATION 4] */
    buffer[1] = 3;
    buffer[2] = 0x80;
    buffer[3] = 1;

    if (isProvider)
        buffer[4] = 1;
    else
        buffer[4] = 0;

    writeBuffer->partLength = 5;
    writeBuffer->length = 5;
    writeBuffer->nextPart = NULL;
}

void
AcseConnection_createReleaseRequestMessage(AcseConnection* self, BufferChain writeBuffer)
{
    byte[] buffer = writeBuffer->buffer;

    buffer[0] = 0x62;
    buffer[1] = 3;
    buffer[2] = 0x80;
    buffer[3] = 1;
    buffer[4] = 0;

    writeBuffer->partLength = 5;
    writeBuffer->length = 5;
    writeBuffer->nextPart = NULL;
}

void
AcseConnection_createReleaseResponseMessage(AcseConnection* self, BufferChain writeBuffer)
{
    byte[] buffer = writeBuffer->buffer;

    buffer[0] = 0x63;
    buffer[1] = 0;

    writeBuffer->partLength = 2;
    writeBuffer->length = 2;
    writeBuffer->nextPart = NULL;
}

    }
}
