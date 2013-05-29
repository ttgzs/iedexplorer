/*
* Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
* 
* Licensed under the LGPL, Version 2 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
*      http://www.gnu.org/copyleft/lgpl.html
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* 
* With any your questions welcome to my e-mail 
* or blog at http://abdulla-a.blogspot.com.
*/
using System;
using System.Reflection;
using System.Collections.Generic;
using org.bn.utils;
using org.bn.attributes;
using org.bn.attributes.constraints;

namespace org.bn.coders.per
{
	
	public class PERUnalignedDecoder:PERAlignedDecoder
	{
		
		protected override void  skipAlignedBits(System.IO.Stream stream)
		{
			// Do nothing! Unaligned encoding ;)
		}
		
		protected override long decodeConstraintNumber(long min, long max, BitArrayInputStream stream)
		{
            long result = 0;
			long valueRange = max - min;
			// !!! int narrowedVal = value - min; !!!
			int maxBitLen = PERCoderUtils.getMaxBitLength(valueRange);
			
			if (valueRange == 0)
			{
				return max;
			}
			//For the UNALIGNED variant the value is always encoded in the minimum 
			// number of bits necessary to represent the range (defined in 10.5.3). 
			int currentBit = maxBitLen;
			while (currentBit > 7)
			{
				currentBit -= 8;
                uint t = (uint)(byte)(stream.ReadByte()) << currentBit;
                result |= t;
			}
			if (currentBit > 0)
			{
				result |= (byte)stream.readBits(currentBit);
			}
			result += min;
			return result;
		}
		
		public override DecodedObject<object> decodeString(DecodedObject<object> decodedTag, System.Type objectClass, ElementInfo elementInfo, System.IO.Stream stream)
		{
            if (!PERCoderUtils.is7BitEncodedString(elementInfo)) 
                return base.decodeString(decodedTag, objectClass, elementInfo, stream);
			else
			{
                DecodedObject<object> result = new DecodedObject<object>();
                int strLen = decodeLength(elementInfo, stream);

                if (strLen <= 0)
                {
                    result.Value = ("");
                    return result;
                }
			
				BitArrayInputStream bitStream = (BitArrayInputStream) stream;
				byte[] buffer = new byte[strLen];
				// 7-bit decoding of string
				for (int i = 0; i < strLen; i++)
					buffer[i] = (byte)bitStream.readBits(7);
                result.Value = new string(
                    System.Text.ASCIIEncoding.ASCII.GetChars(buffer)
                );
                return result;
			}
			
		}
	}
}