using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOOSE_ASN1_Model;
using org.bn.types;

namespace IEDExplorer
{
    class StringToDataConverter
    {        
        public BitString ConvertToBitstring(string input)
        {
            byte[] byteVal = null;
            bool badChar = false;
            BitString output = null;
            

            if ((input.Length > 0) && (input.Length <= 32))
            {
                if (input.Length <= 8)
                    byteVal = new byte[] { 0 };
                else if (input.Length <= 16)
                    byteVal = new byte[] { 0, 0 };
                else if (input.Length <= 24)
                    byteVal = new byte[] { 0, 0, 0 };
                else
                    byteVal = new byte[] { 0, 0, 0, 0 };

                byte mask = 1;

                int len = input.Length;

                for (int i = 0; i < input.Length; i++)
                {
                    if ((i % 8) == 0)
                        mask = 0x80;
                    else
                        mask >>= 1;

                    char test = input[input.Length - 1 - i];

                    if (input[i] == '1')
                        byteVal[i / 8] |= mask;
                    else if (input[i] == '0')
                        badChar = false;
                    else
                    {
                        badChar = true;
                        break;
                    }
                }

                if (!badChar)
                {
                    output = new BitString(byteVal);
                    output.TrailBitsCnt = byteVal.Length * 8 - ((input.Length / 8) * 8) - input.Length % 8;
                }
                else
                    output = null;
            }

            return output;
        }
        
        public byte[] ConvertToTimeEntry(string time)
        {
            DateTime dt = Convert.ToDateTime(time);
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = dt - origin;
            double secs = Math.Floor(diff.TotalSeconds);
            double msecs = (Math.Floor((diff.TotalMilliseconds - (secs * 1000)) * 1000));
            int ms = 0;

            for (int i = 0; i < 24; i++)           
                if (msecs > 0 && msecs >= (1000000 / (1 << (i + 1))) && (1000000 / (1 << (i + 1))) > 0)
                {                    
                    msecs -= (1000000 / (1 << (i + 1)));
                    ms |= ((0x80 >> (i % 8)) << ((i / 8) * 8));
                }
                       
            return new byte[] { (byte)(((long)secs >> 24) & 0xFF), (byte)(((long)secs >> 16) & 0xFF), (byte)(((long)secs >> 8) & 0xFF), (byte)((long)secs & 0xFF), (byte)((ms >> 0) & 0xFF), (byte)((ms >> 8) & 0xFF), (byte)((ms >> 16) & 0xFF), (byte)((ms >> 24)& 0xFF) };
        }

        public bool ConvertToBoolean(string input)
        {
            return false;
        }
    }
}
