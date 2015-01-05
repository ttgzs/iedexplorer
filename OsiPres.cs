using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class OsiPres
    {
        uint callingPresentationSelector;
        uint calledPresentationSelector;
        byte nextContextId;
        byte acseContextId;
        byte mmsContextId;
        
        Iec61850State iecs;
        Logger logger;

        public OsiPres(Iec61850State iec)
        {
            iecs = iec;
            logger = iecs.logger;
        }

        public int Receive(Iec61850State iecs)
        {
            return 0;
        }

        public int Send(Iec61850State iecs)
        {
            return 0;
        }

byte [] def_calledPresentationSelector = { 0x00, 0x00, 0x00, 0x01 };

byte [] asn_id_as_acse = { 0x52, 0x01, 0x00, 0x01 };

byte [] asn_id_mms = { 0x28, 0xca, 0x22, 0x02, 0x01 };

byte [] ber_id = { 0x51, 0x01 };

        int BerEncoder_encodeLength(uint length, byte[] buffer, int bufPos)
{
    if (length < 128)
    {
        buffer[bufPos++] = (byte)length;
    }
    else if (length < 256)
    {
        buffer[bufPos++] = 0x81;
        buffer[bufPos++] = (byte)length;
    }
    else
    {
        buffer[bufPos++] = 0x82;

        buffer[bufPos++] = (byte)(length / 256);
        buffer[bufPos++] = (byte)(length % 256);
    }

    return bufPos;
}

        int BerEncoder_encodeTL(byte tag, uint length, byte[] buffer, int bufPos)
{
    buffer[bufPos++] = tag;
    bufPos = BerEncoder_encodeLength(length, buffer, bufPos);

    return bufPos;
}

int BerEncoder_determineLengthSize(uint length)
{
    if (length < 128)
        return 1;
    if (length < 256)
        return 2;
    else
        return 3;
}

int encodeAcceptBer(byte[] buffer, int bufPos)
{
    bufPos = BerEncoder_encodeTL(0x30, 7, buffer, bufPos);
    bufPos = BerEncoder_encodeTL(0x80, 1, buffer, bufPos);
    buffer[bufPos++] = 0;
    bufPos = BerEncoder_encodeTL(0x81, 2, buffer, bufPos);
    buffer[bufPos++] = 0x51;
    buffer[bufPos++] = 0x01;

    return bufPos;
}

int encodeUserData(byte[] buffer, int bufPos, byte[] payload, int payloadLength, bool encode, byte contextId)
{
    int encodedDataSetLength = 3; /* presentation-selector */

    /* presentation-data */
    encodedDataSetLength += payloadLength + 1;
    encodedDataSetLength += BerEncoder_determineLengthSize((uint)payloadLength);

    int fullyEncodedDataLength = encodedDataSetLength;

    fullyEncodedDataLength += BerEncoder_determineLengthSize((uint)encodedDataSetLength) + 1;

    if (encode) {
        /* fully-encoded-data */
        bufPos = BerEncoder_encodeTL(0x61, (uint)fullyEncodedDataLength, buffer, bufPos);
        bufPos = BerEncoder_encodeTL(0x30, (uint)encodedDataSetLength, buffer, bufPos);

        /* presentation-selector acse */
        bufPos = BerEncoder_encodeTL(0x02, 1, buffer, bufPos);
        buffer[bufPos++] = contextId;

        /* presentation-data (= acse payload) */
        bufPos = BerEncoder_encodeTL(0xa0, (uint)payloadLength, buffer, bufPos);

        return bufPos;
    }
    else {
        int encodedUserDataLength = fullyEncodedDataLength + 1;
        encodedUserDataLength += BerEncoder_determineLengthSize((uint)fullyEncodedDataLength);

        return encodedUserDataLength;
    }
}

int createConnectPdu(byte[] buffer, byte[] payload, int payloadLength)
{
    int contentLength = 0;

    /* mode-selector */
    contentLength += 5;

    int normalModeLength = 0;

    /* called- and calling-presentation-selector */
    normalModeLength += 12;

    int pclLength = 35;

    normalModeLength += pclLength;

    normalModeLength += encodeUserData(null, 0, payload, payloadLength, false, acseContextId);

    normalModeLength += 2;

    contentLength += normalModeLength; // + 2;

    contentLength += 1 + BerEncoder_determineLengthSize((uint)normalModeLength);

    int bufPos = 0;

    bufPos = BerEncoder_encodeTL(0x31, (uint)contentLength, buffer, bufPos);

    /* mode-selector */
    bufPos = BerEncoder_encodeTL(0xa0, 3, buffer, bufPos);
    bufPos = BerEncoder_encodeTL(0x80, 1, buffer, bufPos);
    buffer[bufPos++] = 1; /* 1 = normal-mode */

    /* normal-mode-parameters */
    bufPos = BerEncoder_encodeTL(0xa2, (uint)normalModeLength, buffer, bufPos);

    /* calling-presentation-selector */
    bufPos = BerEncoder_encodeTL(0x81, 4, buffer, bufPos);
    buffer[bufPos++] = (byte) ((callingPresentationSelector >> 24) & 0xff);
    buffer[bufPos++] = (byte)((callingPresentationSelector >> 16) & 0xff);
    buffer[bufPos++] = (byte)((callingPresentationSelector >> 8) & 0xff);
    buffer[bufPos++] = (byte)(callingPresentationSelector & 0xff);

    /* called-presentation-selector */
    bufPos = BerEncoder_encodeTL(0x82, 4, buffer, bufPos);
    buffer[bufPos++] = (byte) ((calledPresentationSelector >> 24) & 0xff);
    buffer[bufPos++] = (byte) ((calledPresentationSelector >> 16) & 0xff);
    buffer[bufPos++] = (byte) ((calledPresentationSelector >> 8) & 0xff);
    buffer[bufPos++] = (byte) (calledPresentationSelector & 0xff);

    /* presentation-context-id list */
    bufPos = BerEncoder_encodeTL(0xa4, 35, buffer, bufPos);

    /* acse context list item */
    bufPos = BerEncoder_encodeTL(0x30, 15, buffer, bufPos);

    bufPos = BerEncoder_encodeTL(0x02, 1, buffer, bufPos);
    buffer[bufPos++] = 1;

    bufPos = BerEncoder_encodeTL(0x06, 4, buffer, bufPos);
    //memcpy(buffer + bufPos, asn_id_as_acse, 4);
    asn_id_as_acse.CopyTo(buffer, bufPos);
    bufPos += 4;

    bufPos = BerEncoder_encodeTL(0x30, 4, buffer, bufPos);
    bufPos = BerEncoder_encodeTL(0x06, 2, buffer, bufPos);
    //memcpy(buffer + bufPos, ber_id, 2);
    ber_id.CopyTo(buffer, bufPos);
    bufPos += 2;

    /* mms context list item */
    bufPos = BerEncoder_encodeTL(0x30, 16, buffer, bufPos);

    bufPos = BerEncoder_encodeTL(0x02, 1, buffer, bufPos);
    buffer[bufPos++] = 3;

    bufPos = BerEncoder_encodeTL(0x06, 5, buffer, bufPos);
    //memcpy(buffer + bufPos, asn_id_mms, 5);
    asn_id_mms.CopyTo(buffer, bufPos);
    bufPos += 5;

    bufPos = BerEncoder_encodeTL(0x30, 4, buffer, bufPos);
    bufPos = BerEncoder_encodeTL(0x06, 2, buffer, bufPos);
    //memcpy(buffer + bufPos, ber_id, 2);
    ber_id.CopyTo(buffer, bufPos);
    bufPos += 2;

    /* encode user data */
    bufPos = encodeUserData(buffer, bufPos, payload, payloadLength, true, acseContextId);

    payload.CopyTo(buffer, bufPos);

    /*
    writeBuffer->partLength = bufPos;
    writeBuffer->length = bufPos + payload->length;
    writeBuffer->nextPart = payload;*/
    return bufPos + payloadLength;
}

int parseFullyEncodedData(byte[] buffer, int len, int bufPos)
{
    int presentationSelector = -1;
    bool userDataPresent = false;

    int endPos = bufPos + len;

    if (buffer[bufPos++] != 0x30) {
        logger.LogDebug("PRES: user-data parse error");
        return -1;
    }

    bufPos = BerDecoder_decodeLength(buffer, len, bufPos, endPos);

    endPos = bufPos + len;

    if (bufPos < 0) {
        logger.LogDebug("PRES: wrong parameter length\n");
        return -1;
    }

    while (bufPos < endPos) {
        byte tag = buffer[bufPos++];
        int length;

        bufPos = BerDecoder_decodeLength(buffer, &length, bufPos, endPos);

        if (bufPos < 0) {
            logger.LogDebug("PRES: wrong parameter length\n");
            return -1;
        }

        switch (tag) {
        case 0x02: /* presentation-context-identifier */
            logger.LogDebug("PRES: presentation-context-identifier\n");
            {
                presentationSelector = BerDecoder_decodeUint32(buffer, length, bufPos);
                nextContextId = presentationSelector;
                bufPos += length;
            }
            break;

        case 0xa0:
            logger.LogDebug("PRES: fully-encoded-data\n");

            userDataPresent = true;

            nextPayload.buffer = buffer + bufPos;
            nextPayload.size = length;

            bufPos += length;
            break;
        default:
            logger.LogDebug("PRES: fed: unknown tag %02x\n", tag);

            bufPos += length;
            break;
        }
    }

    if (!userDataPresent) {
        logger.LogDebug("PRES: user-data not present\n");
        return -1;
    }

    return bufPos;
}

static int
parsePCDLEntry(IsoPresentation* self, byte* buffer, int totalLength, int bufPos)
{
    int endPos = bufPos + totalLength;

    int contextId = -1;
    bool isAcse = false;
    bool isMms = false;

    while (bufPos < endPos) {
        byte tag = buffer[bufPos++];
        int len;

        bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, endPos);

        switch (tag) {
        case 0x02: /* presentation-context-identifier */
            contextId = BerDecoder_decodeUint32(buffer, len, bufPos);
            bufPos += len;
            break;
        case 0x06: /* abstract-syntax-name */
            logger.LogDebug("PRES: abstract-syntax-name with len %i\n", len);

            if (len == 5) {
                if (memcmp(buffer + bufPos, asn_id_mms, 5) == 0)
                    isMms = true;
            }
            else if (len == 4) {
                if (memcmp(buffer + bufPos, asn_id_as_acse, 4) == 0)
                    isAcse = true;
            }

            bufPos += len;

            break;
        case 0x30: /* transfer-syntax-name */
            logger.LogDebug("PRES: ignore transfer-syntax-name\n");

            bufPos += len;
            break;
        default:
            logger.LogDebug("PRES: unknown tag in presentation-context-definition-list-entry\n");
            bufPos += len;
            break;
        }
    }

    if (contextId < 0) {
        logger.LogDebug("PRES: ContextId not defined!\n");
        return -1;
    }

    if ((isAcse == false) && (isMms == false)) {
        logger.LogDebug("PRES: not an ACSE or MMS context definition\n");

        return -1;
    }

    if (isMms) {
        mmsContextId = (byte) contextId;
        logger.LogDebug("PRES: MMS context id is %i\n", contextId);
    }
    else {
        acseContextId = (byte) contextId;
        logger.LogDebug("PRES: ACSE context id is %i\n", contextId);
    }

    return bufPos;
}

static int
parsePresentationContextDefinitionList(IsoPresentation* self, byte* buffer, int totalLength, int bufPos)
{
    int endPos = bufPos + totalLength;

    while (bufPos < endPos) {
        byte tag = buffer[bufPos++];
        int len;

        bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, endPos);

        switch (tag) {
        case 0x30:
            logger.LogDebug("PRES: parse pcd entry\n");
            bufPos = parsePCDLEntry(self, buffer, len, bufPos);
            if (bufPos < 0)
                return -1;
            break;
        default:
            logger.LogDebug("PRES: unknown tag in presentation-context-definition-list\n");
            bufPos += len;
            break;
        }
    }

    return bufPos;
}

static int
parseNormalModeParameters(IsoPresentation* self, byte* buffer, int totalLength, int bufPos)
{
    int endPos = bufPos + totalLength;

    while (bufPos < endPos) {
        byte tag = buffer[bufPos++];
        int len;

        bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, endPos);

        if (bufPos < 0) {
            logger.LogDebug("PRES: wrong parameter length\n");
            return -1;
        }

        switch (tag) {
        case 0x81: /* calling-presentation-selector */
            logger.LogDebug("PRES: calling-pres-sel\n");
            bufPos += len;
            break;
        case 0x82: /* calling-presentation-selector */
            logger.LogDebug("PRES: calling-pres-sel\n");
            bufPos += len;
            break;
        case 0xa4: /* presentation-context-definition list */
            logger.LogDebug("PRES: pcd list\n");
            bufPos = parsePresentationContextDefinitionList(self, buffer, len, bufPos);
            break;
        case 0x61: /* user data */
            logger.LogDebug("PRES: user-data\n");

            bufPos = parseFullyEncodedData(self, buffer, len, bufPos);

            if (bufPos < 0)
                return -1;

            break;

        default:
            logger.LogDebug("PRES: unknown tag in normal-mode\n");
            bufPos += len;
            break;
        }
    }

    return bufPos;
}

int
IsoPresentation_parseAcceptMessage(IsoPresentation* self, ByteBuffer* byteBuffer)
{
    byte* buffer = byteBuffer->buffer;
    int maxBufPos = byteBuffer->size;

    int bufPos = 0;

    byte cpTag = buffer[bufPos++];

    if (cpTag != 0x31) {
        logger.LogDebug("PRES: not a CPA message\n");
        return 0;
    }

    int len;

    bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, maxBufPos);

    while (bufPos < maxBufPos) {
        byte tag = buffer[bufPos++];

        bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, maxBufPos);

        if (bufPos < 0) {
            logger.LogDebug("PRES: wrong parameter length\n");
            return 0;
        }

        switch (tag) {
        case 0xa0: /* mode-selector */
            bufPos += len; /* ignore content since only normal mode is allowed */
            break;
        case 0xa2: /* normal-mode-parameters */
            bufPos = parseNormalModeParameters(self, buffer, len, bufPos);

            if (bufPos < 0) {
                if (DEBUG_PRES)
                    printf("PRES: error parsing normal-mode-parameters\n");
                return 0;
            }

            break;
        default:
            logger.LogDebug("PRES: CPA unknown tag %i\n", tag);
            bufPos += len;
            break;
        }
    }

    return 1;
}

void
IsoPresentation_init(IsoPresentation* self)
{

}

void
IsoPresentation_createUserData(IsoPresentation* self, BufferChain writeBuffer, BufferChain payload)
{
    int bufPos = 0;
    byte* buffer = writeBuffer->buffer;

    int payloadLength = payload->length;

    int userDataLengthFieldSize = BerEncoder_determineLengthSize(payloadLength);
    ;
    int pdvListLength = payloadLength + (userDataLengthFieldSize + 4);

    int pdvListLengthFieldSize = BerEncoder_determineLengthSize(pdvListLength);
    int presentationLength = pdvListLength + (pdvListLengthFieldSize + 1);

    bufPos = BerEncoder_encodeTL(0x61, presentationLength, buffer, bufPos);

    bufPos = BerEncoder_encodeTL(0x30, pdvListLength, buffer, bufPos);

    buffer[bufPos++] = (byte) 0x02;
    buffer[bufPos++] = (byte) 0x01;
    buffer[bufPos++] = (byte) mmsContextId; /* mms context id */

    bufPos = BerEncoder_encodeTL(0xa0, payloadLength, buffer, bufPos);

    writeBuffer->partLength = bufPos;
    writeBuffer->length = bufPos + payloadLength;
    writeBuffer->nextPart = payload;
}

void
IsoPresentation_createUserDataACSE(IsoPresentation* self, BufferChain writeBuffer, BufferChain payload)
{
    int bufPos = 0;
    byte* buffer = writeBuffer->buffer;

    int payloadLength = payload->length;

    int userDataLengthFieldSize = BerEncoder_determineLengthSize(payloadLength);
    ;
    int pdvListLength = payloadLength + (userDataLengthFieldSize + 4);

    int pdvListLengthFieldSize = BerEncoder_determineLengthSize(pdvListLength);
    int presentationLength = pdvListLength + (pdvListLengthFieldSize + 1);

    bufPos = BerEncoder_encodeTL(0x61, presentationLength, buffer, bufPos);

    bufPos = BerEncoder_encodeTL(0x30, pdvListLength, buffer, bufPos);

    buffer[bufPos++] = (byte) 0x02;
    buffer[bufPos++] = (byte) 0x01;
    buffer[bufPos++] = (byte) acseContextId; /* ACSE context id */

    bufPos = BerEncoder_encodeTL(0xa0, payloadLength, buffer, bufPos);

    writeBuffer->partLength = bufPos;
    writeBuffer->length = bufPos + payloadLength;
    writeBuffer->nextPart = payload;
}

int
IsoPresentation_parseUserData(IsoPresentation* self, ByteBuffer* readBuffer)
{
    int length = readBuffer->size;
    byte* buffer = readBuffer->buffer;

    int bufPos = 0;

    if (length < 9)
        return 0;

    if (buffer[bufPos++] != 0x61)
        return 0;

    int len;

    bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, length);

    if (buffer[bufPos++] != 0x30)
        return 0;

    bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, length);

    if (buffer[bufPos++] != 0x02)
        return 0;

    if (buffer[bufPos++] != 0x01)
        return 0;

    nextContextId = buffer[bufPos++];

    if (buffer[bufPos++] != 0xa0)
        return 0;

    int userDataLength;

    bufPos = BerDecoder_decodeLength(buffer, &userDataLength, bufPos, length);

    ByteBuffer_wrap(&(nextPayload), buffer + bufPos, userDataLength, userDataLength);

    return 1;
}

int
IsoPresentation_parseConnect(IsoPresentation* self, ByteBuffer* byteBuffer)
{
    byte* buffer = byteBuffer->buffer;
    int maxBufPos = byteBuffer->size;

    int bufPos = 0;

    byte cpTag = buffer[bufPos++];

    if (cpTag != 0x31) {
        logger.LogDebug("PRES: not a CP type\n");
        return 0;
    }

    int len;

    bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, maxBufPos);

    if (DEBUG_PRES)
        printf("PRES: CPType with len %i\n", len);

    while (bufPos < maxBufPos) {
        byte tag = buffer[bufPos++];

        bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, maxBufPos);

        if (bufPos < 0) {
            logger.LogDebug("PRES: wrong parameter length\n");
            return 0;
        }

        switch (tag) {
        case 0xa0: /* mode-selection */
            {
                if (buffer[bufPos++] != 0x80) {
                    if (DEBUG_PRES)
                        printf("PRES: mode-value of wrong type!\n");
                    return 0;
                }
                bufPos = BerDecoder_decodeLength(buffer, &len, bufPos, maxBufPos);
                uint32_t modeSelector = BerDecoder_decodeUint32(buffer, len, bufPos);
                if (DEBUG_PRES)
                    printf("PRES: modesel %ui\n", modeSelector);
                bufPos += len;
            }
            break;
        case 0xa2: /* normal-mode-parameters */
            bufPos = parseNormalModeParameters(self, buffer, len, bufPos);

            if (bufPos < 0) {
                if (DEBUG_PRES)
                    printf("PRES: error parsing normal-mode-parameters\n");
                return 0;
            }

            break;
        default: /* unsupported element */
            logger.LogDebug("PRES: tag %i not recognized\n", tag);
            bufPos += len;
            break;
        }
    }

    return 1;
}

void
IsoPresentation_createConnectPdu(IsoPresentation* self, IsoConnectionParameters parameters,
        BufferChain buffer, BufferChain payload)
{
    acseContextId = 1;
    mmsContextId = 3;
    callingPresentationSelector = parameters->localPSelector;
    calledPresentationSelector = parameters->remotePSelector;
    createConnectPdu(self, buffer, payload);
}

void
IsoPresentation_createAbortUserMessage(IsoPresentation* self, BufferChain writeBuffer, BufferChain payload)
{
    int contentLength = 0;

    contentLength = +encodeUserData(NULL, 0, payload, false, acseContextId);

    contentLength += BerEncoder_determineLengthSize(contentLength) + 1;

    byte* buffer = writeBuffer->buffer;
    int bufPos = 0;

    bufPos = BerEncoder_encodeTL(0xa0, contentLength, buffer, bufPos);

    /* encode user data */
    bufPos = encodeUserData(buffer, bufPos, payload, true, acseContextId);

    writeBuffer->partLength = bufPos;
    writeBuffer->length = bufPos + payload->length;
    writeBuffer->nextPart = payload;
}

void
IsoPresentation_createCpaMessage(IsoPresentation* self, BufferChain writeBuffer, BufferChain payload)
{
    int contentLength = 0;

    /* mode-selector */
    contentLength += 5;

    int normalModeLength = 0;

    normalModeLength += 6; /* responding-presentation-selector */

    normalModeLength += 20; /* context-definition-result-list */

    normalModeLength += encodeUserData(NULL, 0, payload, false, acseContextId);

    contentLength += normalModeLength;

    contentLength += BerEncoder_determineLengthSize(normalModeLength) + 1;

    byte* buffer = writeBuffer->buffer;
    int bufPos = 0;

    bufPos = BerEncoder_encodeTL(0x31, contentLength, buffer, bufPos);

    /* mode-selector */
    bufPos = BerEncoder_encodeTL(0xa0, 3, buffer, bufPos);
    bufPos = BerEncoder_encodeTL(0x80, 1, buffer, bufPos);
    buffer[bufPos++] = 1; /* 1 = normal-mode */

    /* normal-mode-parameters */
    bufPos = BerEncoder_encodeTL(0xa2, normalModeLength, buffer, bufPos);

    /* responding-presentation-selector */
    bufPos = BerEncoder_encodeTL(0x83, 4, buffer, bufPos);
    memcpy(buffer + bufPos, calledPresentationSelector, 4);
    bufPos += 4;

    /* context-definition-result-list */
    bufPos = BerEncoder_encodeTL(0xa5, 18, buffer, bufPos);
    bufPos = encodeAcceptBer(buffer, bufPos); /* accept for acse */
    bufPos = encodeAcceptBer(buffer, bufPos); /* accept for mms */

    /* encode user data */
    bufPos = encodeUserData(buffer, bufPos, payload, true, acseContextId);

    writeBuffer->partLength = bufPos;
    writeBuffer->length = bufPos + payload->length;
    writeBuffer->nextPart = payload;
}
    }
}
