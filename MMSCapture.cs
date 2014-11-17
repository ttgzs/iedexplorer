using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMS_ASN1_Model;
using System.IO;
using org.bn;

namespace IEDExplorer
{
    class MMSCapture
    {
        public enum CaptureDirection
        {
            In,
            Out
        }

        byte[] encodedPacket;
        MMSpdu pdu;
        CaptureDirection direction;
        DateTime time;

        public MMSCapture(byte[] pkt, long pos, CaptureDirection dir)
        {
            encodedPacket = new Byte[pkt.Length - pos];
            Array.Copy(pkt, pos, encodedPacket, 0, pkt.Length - pos);
            direction = dir;
            time = DateTime.Now;
        }

        public MMSpdu MMSPdu { get { return pdu; } set { pdu = value; } }

        public string XMLPdu
        {
            get
            {
                if (pdu != null)
                {
                    IEncoder xmlencoder = new IEDExplorer.BNExtension.XMLEncoder();

                    MemoryStream ms = new MemoryStream();
                    xmlencoder.encode<MMSpdu>(pdu, ms);
                    byte[] buf = ms.ToArray();
                    string st = System.Text.Encoding.ASCII.GetString(buf, 0, buf.Length);
                    return st;
                }
                else
                    return "No PDU available!";
            }
        }

        public string MMSPduType
        {
            get
            {
                if (pdu != null)
                {
                    pdu.
                }
                else
                    return "No PDU available!";
            }
        }

        public string Packet
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in encodedPacket)
                {
                    sb.Append(b.ToString("X2"));
                    sb.Append(' ');
                }
                return sb.ToString();
            }
        }
    }
}
