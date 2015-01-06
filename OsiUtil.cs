using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    static class OsiUtil
    {
        public static int BerEncoder_encodeLength(uint length, byte[] buffer, int bufPos)
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

        public static int BerEncoder_encodeTL(byte tag, uint length, byte[] buffer, int bufPos)
        {
            buffer[bufPos++] = tag;
            bufPos = BerEncoder_encodeLength(length, buffer, bufPos);

            return bufPos;
        }

        public static int BerEncoder_determineLengthSize(uint length)
        {
            if (length < 128)
                return 1;
            if (length < 256)
                return 2;
            else
                return 3;
        }

        public static int BerDecoder_decodeLength(byte[] buffer, ref int length, int bufPos, int maxBufPos)
        {
            if (bufPos >= maxBufPos)
                return -1;

            byte len1 = buffer[bufPos++];

            if ((len1 & 0x80) > 0)
            {
                int lenLength = len1 & 0x7f;

                if (lenLength == 0)
                { /* indefinite length form */
                    length = -1;
                }
                else
                {
                    length = 0;

                    int i;
                    for (i = 0; i < lenLength; i++)
                    {
                        if (bufPos >= maxBufPos)
                            return -1;

                        length <<= 8;
                        length += buffer[bufPos++];
                    }
                }

            }
            else
            {
                length = len1;
            }

            return bufPos;
        }

        public static uint BerDecoder_decodeUint32(byte[] buffer, int intlen, int bufPos)
        {
            uint value = 0;

            int i;
            for (i = 0; i < intlen; i++)
            {
                value <<= 8;
                value += buffer[bufPos + i];
            }

            return value;
        }

        public static int BerEncoder_encodeUInt32(uint value, byte[] buffer, int bufPos)
        {
            byte[] valueArray = BitConverter.GetBytes(value);
            byte[] valueBuffer = new byte[5];

            valueBuffer[0] = 0;

            if (BitConverter.IsLittleEndian)
                BerEncoder_revertByteOrder(valueArray, 4);
            for (int i = 0; i < 4; i++)
            {
                valueBuffer[i + 1] = valueArray[i];
            }

            int size = BerEncoder_compressInteger(valueBuffer, 5);

            for (int i = 0; i < size; i++)
            {
                buffer[bufPos++] = valueBuffer[i];
            }

            return bufPos;
        }

        public static int BerEncoder_UInt32determineEncodedSize(uint value)
        {
            byte[] valueArray = BitConverter.GetBytes(value);
            byte[] valueBuffer = new byte[5];

            valueBuffer[0] = 0;

            if (BitConverter.IsLittleEndian)
                BerEncoder_revertByteOrder(valueArray, 4);

            for (int i = 0; i < 4; i++)
            {
                valueBuffer[i + 1] = valueArray[i];
            }

            int size = BerEncoder_compressInteger(valueBuffer, 5);

            return size;
        }

        static int BerEncoder_compressInteger(byte[] integer, int originalSize)
        {
            int integerEnd = originalSize - 1;
            int bytePosition;

            for (bytePosition = 0; bytePosition < integerEnd; bytePosition++)
            {

                if (integer[bytePosition] == 0x00)
                {
                    if ((integer[bytePosition + 1] & 0x80) == 0)
                        continue;
                }
                else if (integer[bytePosition] == 0xff)
                {
                    if ((integer[bytePosition + 1] & 0x80) > 0)
                        continue;
                }

                break;
            }

            int bytesToDelete = bytePosition;
            int newSize = originalSize;

            if (bytesToDelete != 0)
            {
                newSize -= bytesToDelete;
                int newEnd = newSize;

                int newBytePosition;

                for (newBytePosition = 0; newBytePosition < newEnd; newBytePosition++)
                {
                    integer[newBytePosition] = integer[bytePosition];
                    bytePosition++;
                }
            }

            return newSize;
        }

        static void BerEncoder_revertByteOrder(byte[] octets, int size)
        {
            int i;
            byte temp;

            for (i = 0; i < size / 2; i++)
            {
                temp = octets[i];
                octets[i] = octets[(size - 1) - i];
                octets[(size - 1) - i] = temp;
            }
        }

    }
}
