/*
 *  Copyright (C) 2013 Pavel Charvat
 * 
 *  This file is part of IEDExplorer.
 *
 *  IEDExplorer is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IEDExplorer is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IEDExplorer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using org.bn.attributes;
using org.bn.attributes.constraints;
using org.bn.coders;
using org.bn.coders.ber;
using org.bn.types;
using org.bn;

namespace IEDExplorer
{
    class OsiEmul
    {
        private const int COTP_HDR_CR_SIZEOF = 18;
        public const int COTP_HDR_DT_SIZEOF = 15 + 2 + 2 + 3;

        private const int COTP_HDR_IDX_HDRLEN = 0;
        private const int COTP_HDR_IDX_CODE = 1;
        private const int COTP_HDR_IDX_DSTREF = 2;
        private const int COTP_HDR_IDX_SRCREF = 4;
        private const int COTP_HDR_IDX_OPTION = 6;

        private const byte COTP_CODE_CR = 0xe0;
        private const byte COTP_CODE_CC = 0xd0;
        private const byte COTP_CODE_DT = 0xf0;

        private const byte COTP_PCODE_TSIZ = 0xc0;
        private const byte COTP_PCODE_DSAP = 0xc2;
        private const byte COTP_PCODE_SSAP = 0xc1;

        private short m_COTP_srcref;
        private short m_COTP_dstref;
        private byte m_COTP_option;
        private byte m_COTP_tpdu_size;	// 1024 byte max PDU size
        private short m_COTP_dst_tsap;
        private short m_COTP_src_tsap;

        public OsiEmul()
        {
            Reset();
        }

        void Reset()
        {
            m_COTP_dstref = 0x0000;
            m_COTP_srcref = 0x0008;
            m_COTP_option = 0x00;
            m_COTP_tpdu_size = 0x0a;	// 1024 byte max PDU size
            m_COTP_dst_tsap = 0x0001;
            m_COTP_src_tsap = 0x0000;
        }

        public int SendCOTPSessionInit(Iec61850State iecs)
        {
            // Make COTP init telegramm
            int offs = Tpkt.TPKT_SIZEOF;

            unchecked
            {
                iecs.sendBuffer[offs + COTP_HDR_IDX_HDRLEN] = (byte)(COTP_HDR_CR_SIZEOF - 1);
            }
            iecs.sendBuffer[offs + COTP_HDR_IDX_CODE] = COTP_CODE_CR;
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(m_COTP_dstref)), 0, iecs.sendBuffer, offs + COTP_HDR_IDX_DSTREF, 2);
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(m_COTP_srcref)), 0, iecs.sendBuffer, offs + COTP_HDR_IDX_SRCREF, 2);
            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION] = m_COTP_option;

            // Parameters
            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION + 1] = COTP_PCODE_TSIZ;
            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION + 2] = 1;
            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION + 3] = m_COTP_tpdu_size;

            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION + 4] = COTP_PCODE_DSAP;
            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION + 5] = 2;
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(m_COTP_dst_tsap)), 0, iecs.sendBuffer, offs + COTP_HDR_IDX_OPTION + 6, 2);

            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION + 8] = COTP_PCODE_SSAP;
            iecs.sendBuffer[offs + COTP_HDR_IDX_OPTION + 9] = 2;
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(m_COTP_src_tsap)), 0, iecs.sendBuffer, offs + COTP_HDR_IDX_OPTION + 10, 2);

            iecs.sendBytes = offs + COTP_HDR_CR_SIZEOF;

            Tpkt.Send(iecs);
            return 0;
        }

        public int SendPresentationInit(Iec61850State iecs)
        {
            // Make session & present. init telegramm
            // Packet includes	- TPKT
            //					- Session init
            //					- Presentation init
            //					- MMS Initiate Request
            byte[] PresentationInit_imprint = {
		    /*TPKT*/ 0x03, 0x00, 0x00, 0xbf, // Placeholder
		    /*COTP*/ 0x02, 0xf0, 0x80,
		    /*Sess*/ 0x0d, 
		    0xb6, 0x05, 0x06, 0x13, 0x01, 0x00, 0x16, 0x01, 
		    0x02, 0x14, 0x02, 0x00, 0x02, 0x33, 0x02, 0x00, 
		    0x01, 0x34, 0x02, 0x00, 0x01, 0xc1, 0xa0,
		    /*Pres*/ 0x31, 
		    0x81, 0x9d, 0xa0, 0x03, 0x80, 0x01, 0x01, 0xa2, 
		    0x81, 0x95, 0x81, 0x04, 0x00, 0x00, 0x00, 0x01, 
		    0x82, 0x04, 0x00, 0x00, 0x00, 0x01, 0xa4, 0x23, 
		    0x30, 0x0f, 0x02, 0x01, 0x01, 0x06, 0x04, 0x52, 
		    0x01, 0x00, 0x01, 0x30, 0x04, 0x06, 0x02, 0x51, 
		    0x01, 0x30, 0x10, 0x02, 0x01, 0x03, 0x06, 0x05, 
		    0x28, 0xca, 0x22, 0x02, 0x01, 0x30, 0x04, 0x06, 
		    0x02, 0x51, 0x01, 0x61, 0x62, 0x30, 0x60, 0x02, 
		    0x01, 0x01, 0xa0, 0x5b, 0x60, 0x59, 0xa1, 0x07, 
		    0x06, 0x05, 0x28, 0xca, 0x22, 0x02, 0x03, 0xa2, 
		    0x07, 0x06, 0x05, 0x29, 0x87, 0x67, 0x01, 0x01, 
		    0xa3, 0x03, 0x02, 0x01, 0x0c, 0xa6, 0x06, 0x06, 
		    0x04, 0x29, 0x01, 0x87, 0x67, 0xa7, 0x03, 0x02, 
		    0x01, 0x0c, 0xbe, 0x33, 0x28, 0x31, 0x06, 0x02, 
		    0x51, 0x01, 0x02, 0x01, 0x03, 0xa0, 0x28, 0xa8, 
		    0x26, 0x80, 0x03, 0x00,
		    //0x9c, 0x40,  // Proposed MMS PDU Size:  20000
		    //0x27, 0x10,  // Proposed MMS PDU Size:  10000
		    0xfd, 0xe8,  // Proposed MMS PDU Size:  65000
		    0x81, 0x01,  
		    0x0a, 0x82, 0x01, 0x0a, 0x83, 0x01, 0x05, 0xa4, 
		    0x16, 0x80, 0x01, 0x01, 0x81, 0x03, 0x05, 0xf1, 
		    0x00, 0x82, 0x0c, 0x03, 0xee, 0x1c, 0x00, 0x00, 
		    0x04, 0x08, 0x00, 0x00, 0x79, 0xef, 0x18 };

            Array.Copy(PresentationInit_imprint, 4, iecs.sendBuffer, 4, PresentationInit_imprint.Length - 4);

            iecs.sendBytes = PresentationInit_imprint.Length;

            Tpkt.Send(iecs);
            return 0;
        }

        public int Receive(Iec61850State iecs)
        {
            iecs.logger.LogDebug("osi.Receive");
            if (iecs.dataBuffer[1] == COTP_CODE_DT)
            {
                iecs.msMMS.Write(iecs.dataBuffer, 3, iecs.dataBufferIndex - 3);
                //iecs.msMMS = new MemoryStream(iecs.dataBuffer, 3, iecs.dataBufferIndex - 3);

                if ((iecs.dataBuffer[2] & 0x80) == 0)
                {
                    return 0;					// waiting for the rest of the datagram from other COTP frames
                }
            }
            else if (iecs.dataBuffer[1] == COTP_CODE_CC)
            {
                iecs.msMMS.Write(iecs.dataBuffer, 0, iecs.dataBufferIndex);
            }

            switch (iecs.ostate)
            {
                case OsiProtocolState.OSI_CONNECT_COTP_WAIT:
                    iecs.logger.LogDebug(String.Format("Calling ParseCOTPSessionInit with data len {0}", iecs.msMMS.Length));
                    if (ParseCOTPSessionInit(iecs) == 0)
                        iecs.ostate = OsiProtocolState.OSI_CONNECT_PRES;
                    else
                        iecs.ostate = OsiProtocolState.OSI_STATE_SHUTDOWN;
                    break;
                case OsiProtocolState.OSI_CONNECT_PRES_WAIT:
                    iecs.logger.LogDebug(String.Format("Calling ParsePresentationInit with data len {0}", iecs.msMMS.Length));
                    if (ParsePresentationInit(iecs) == 0)
                        iecs.ostate = OsiProtocolState.OSI_CONNECTED;
                    else
                        iecs.ostate = OsiProtocolState.OSI_STATE_SHUTDOWN;
                    break;
                case OsiProtocolState.OSI_CONNECTED:
                    iecs.logger.LogDebug(String.Format("Calling mms.ReceiveData with data len {0}", iecs.msMMS.Length));
                    if (iecs.osi.ParsePresentationData(iecs) == 0)
                        iecs.ostate = OsiProtocolState.OSI_CONNECTED;
                    else
                        iecs.ostate = OsiProtocolState.OSI_STATE_SHUTDOWN;
                    break;
            }
            // Reset the stream
            iecs.msMMS = new MemoryStream();
            return 0;
        }

        int ParseCOTPSessionInit(Iec61850State iecs)
        {
            // Read COTP init response

            iecs.msMMS.Seek(1, SeekOrigin.Begin);  // skip hdrlen
            if (iecs.msMMS.ReadByte() != COTP_CODE_CC) return -1;    // code NOK
            iecs.msMMS.Seek(2, SeekOrigin.Current);  // skip dstref
            Byte[] b = new Byte[2];
            iecs.msMMS.Read(b, 0, 2);
            m_COTP_dstref = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(b, 0));    // srcref
            // Parameters
            iecs.msMMS.Seek(2, SeekOrigin.Current);  // skip option
            if (iecs.msMMS.ReadByte() == COTP_PCODE_TSIZ)
            {
                iecs.msMMS.Seek(1, SeekOrigin.Current);  // skip option
                m_COTP_tpdu_size = (byte)iecs.msMMS.ReadByte();
            }

            return 0;	//O.K.
        }

        int ParsePresentationInit(Iec61850State iecs)
        {
            iecs.msMMS.Seek(1, SeekOrigin.Begin);
            int len = iecs.msMMS.ReadByte();   // 
        	for (int i=0; i<len; i++)
            {   
                // empiric seek for MMS initiate response
                if (iecs.msMMS.ReadByte() == 0xa9)
                {
                    iecs.msMMS.ReadByte();
                    if (iecs.msMMS.ReadByte() == 0x80)
                    {
                        iecs.logger.LogDebug("Have MMS Initiate / going to parser");
                        iecs.msMMS.Seek(-3, SeekOrigin.Current);
                        iecs.mms.ReceiveInitiate(iecs);
                        break;
                    }
                }
    		}
	        return 0;	//O.K.
        }

        int ParsePresentationData(Iec61850State iecs)
        {
            iecs.msMMS.Seek(4, SeekOrigin.Begin);       // PDUs
            if ( iecs.msMMS.ReadByte() == 0x61)
            {
                DecodedObject<int> deco = decodeLength(iecs.msMMS);
                if (iecs.msMMS.ReadByte() == 0x30) {
                    deco = decodeLength(iecs.msMMS);
                    if (iecs.msMMS.ReadByte() == 0x02 && iecs.msMMS.ReadByte() == 0x01 && iecs.msMMS.ReadByte() == 0x03)
                        if (iecs.msMMS.ReadByte() == 0xa0)
                        {
                            deco = decodeLength(iecs.msMMS);
                            return iecs.mms.ReceiveData(iecs);
                        }
                    }
            }
            return 0;
        }

        public static DecodedObject<int> decodeLength(System.IO.Stream stream)
        {
            int result = 0;
            int bt = stream.ReadByte();
            if (bt == -1)
                throw new System.ArgumentException("Unexpected EOF when decoding!");

            int len = 1;
            if (bt < 128)
            {
                result = bt;
            }
            else
            {
                // Decode length bug fixed. Thanks to John 
                for (int i = bt - 128; i > 0; i--)
                {
                    int fBt = stream.ReadByte();
                    if (fBt == -1)
                        throw new System.ArgumentException("Unexpected EOF when decoding!");

                    result = result << 8;
                    result = result | fBt;
                    len++;
                }
            }
            return new DecodedObject<int>(result, len);
        }

        public int Send(Iec61850State iecs)
        {
            // Make COTP data telegramm
            int offs = Tpkt.TPKT_SIZEOF;

            iecs.sendBuffer[offs++] = 0x02; // cotp.hdrlen
            iecs.sendBuffer[offs++] = COTP_CODE_DT; // code
            iecs.sendBuffer[offs++] = 0x80; // number "complete"
            iecs.sendBuffer[offs++] = 0x01; // giveTokensPDU.type
            iecs.sendBuffer[offs++] = 0x00; // giveTokensPDU.hdrlen
            iecs.sendBuffer[offs++] = 0x01; // dataTransferPDU.type
            iecs.sendBuffer[offs++] = 0x00; // dataTransferPDU.hdrlen

            iecs.sendBuffer[offs++] = 0x61; // pres.dtpdu_tag
            iecs.sendBuffer[offs++] = 0x82; // pres.dtpdu_len_code
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(iecs.sendBytes + 15 - 4))), 0, iecs.sendBuffer, offs, 2); // pres.dtpdu_len
            offs += 2;
            iecs.sendBuffer[offs++] = 0x30; // pres.sequ_tag
            iecs.sendBuffer[offs++] = 0x82; // pres.sequ_len_code
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(iecs.sendBytes + 15 - 8))), 0, iecs.sendBuffer, offs, 2); // pres.sequ_len
            offs += 2;
            iecs.sendBuffer[offs++] = 0x02; // pres.context_tag
            iecs.sendBuffer[offs++] = 0x01; // pres.context_len
            iecs.sendBuffer[offs++] = 0x03; // pres.context_val
            iecs.sendBuffer[offs++] = 0xa0; // pres.data_tag
            iecs.sendBuffer[offs++] = 0x82; // pres.data_len_code
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(iecs.sendBytes))), 0, iecs.sendBuffer, offs, 2); // pres.sequ_len
            offs += 2;

            iecs.sendBytes += offs;
            Tpkt.Send(iecs);
            return 0;
        }
    }
}
