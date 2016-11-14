using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PcapDotNet.Packets;

namespace IEDExplorer
{
    public class GooseData
    {
        private static class Offset
        {
            public const int AppId = 0;
            public const int Length = 2;
            public const int Reserved1 = 2;
            public const int Reserved2 = 2;
            public const int GoosePdu = 2;
        }            

        // Goose datagram memory stream        
        private MemoryStream _msGOOSE = null;
        
        private Packet _packet;
        private bool _IsGooseType;
        private int _AppId = 0;
        private int _Length = 0;
        private PcapDotNet.Packets.Ethernet.MacAddress _SrcMac;
        private PcapDotNet.Packets.Ethernet.MacAddress _DstMac;
 
        public int AppId { get { return _AppId; } }
        public int Length { get { return _Length; } }
        public PcapDotNet.Packets.Ethernet.MacAddress SrcMac { get { return _SrcMac; } }
        public PcapDotNet.Packets.Ethernet.MacAddress DstMac { get { return _DstMac; } }
        public bool IsGooseType { get { return _IsGooseType; } }

        private int getIntFromMs(MemoryStream ms, int offs)
        {
            byte[] buf = new byte[2];
            long msPos = ms.Position;
            ms.Position += offs;
            ms.Read(buf, 0, 2);
            ms.Position = msPos;
            return ((buf[0] << 8) | buf[1]);
        }

        private bool isGooseType(Packet packet)
        {
            int GooseEthFrameOffset = 0;

            if ((int)packet.Ethernet.VLanTaggedFrame.EtherType == 0x88b8)
                GooseEthFrameOffset = packet.Ethernet.HeaderLength + packet.Ethernet.VLanTaggedFrame.HeaderLength;
            else if ((int)packet.Ethernet.EtherType == 0x88b8)
                GooseEthFrameOffset = packet.Ethernet.HeaderLength;
            else
                GooseEthFrameOffset = 0;

            if (GooseEthFrameOffset > 0)
            {
                _msGOOSE = packet.Ethernet.ToMemoryStream();
                _msGOOSE.Position = GooseEthFrameOffset;                

                return true;
            }
            else
                return false;
        }

        public Packet packet
        {
            set
            {
                _packet = value;

                if (isGooseType(_packet))
                {
                    _SrcMac = _packet.Ethernet.Source;
                    _DstMac = _packet.Ethernet.Destination;
                    _AppId = getIntFromMs(_msGOOSE, Offset.AppId);
                    _Length = getIntFromMs(_msGOOSE, Offset.AppId + Offset.Length);
                    _msGOOSE.Position += (Offset.AppId + Offset.Length + Offset.Reserved1 + Offset.Reserved2 + Offset.GoosePdu);
                    _IsGooseType = true;
                }
                else
                    _IsGooseType = false;
            }
            get
            {
                return _packet;
            }
        }

        public MemoryStream Ms { get { return _msGOOSE; } }
    }
}
