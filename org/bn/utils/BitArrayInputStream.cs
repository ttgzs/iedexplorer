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
namespace org.bn.utils
{
	
	public class BitArrayInputStream:System.IO.Stream
	{
		private System.IO.Stream byteStream;
		private int currentBit = 0, currentByte;
		
		public BitArrayInputStream(System.IO.Stream byteStream)
		{
			this.byteStream = byteStream;
		}
		
		public override int ReadByte()
		{
			if (currentBit == 0)
			{
				return byteStream.ReadByte();
			}
			else
			{
				int nextByte = byteStream.ReadByte();
				int result = ((currentByte << currentBit) | (nextByte >> (8 - currentBit))) & 0xFF;
				currentByte = nextByte;
				return result;
			}
		}
		
		public virtual int readBit()
		{
			lock (this)
			{
				if (currentBit == 0)
				{
					currentByte = byteStream.ReadByte();
				}
				currentBit++;
				int result = currentByte >> (8 - currentBit) & 0x1;
				if (currentBit > 7)
					currentBit = 0;
				return result;
			}
		}
		
		public virtual int readBits(int nBits)
		{
			lock (this)
			{
				int result = 0;
				for (int i = 0; i < nBits && i <= 32; i++)
				{
					result = ((result << 1) | readBit());
				}
				return result;
			}
		}
		
		public virtual void  skipUnreadedBits()
		{
			currentBit = 0;
		}

        public override System.Int32 Read(System.Byte[] buffer, System.Int32 offset, System.Int32 count)
        {
            if (currentBit == 0)
            {
                return byteStream.Read(buffer, offset, count);
            }
            else
            {
                int readCnt = 0;
                for (; readCnt < buffer.Length && readCnt < byteStream.Length && readCnt < count; readCnt++)
                {
                    buffer[readCnt] = (byte) ReadByte();
                }
                return readCnt;
            }
        }


        public override void  Flush()
		{
		}

        public override System.Int64 Seek(System.Int64 offset, System.IO.SeekOrigin origin)            
		{
            return -1;
		}
		
		public override void  SetLength(System.Int64 value)
		{
		}

        public override void  Write(System.Byte[] buffer, System.Int32 offset, System.Int32 count)
		{
		}

        public override System.Boolean CanRead
		{
			get
			{
				return true;
			}
			
		}
		public override System.Boolean CanSeek
		{
			get
			{
				return false;
			}
			
		}
		public override System.Boolean CanWrite
		{
			get
			{
				return false;
			}
			
		}
		public override System.Int64 Length
		{
			get
			{
				return byteStream.Length;
			}
			
		}

        public override Int64 Position
		{
			get
			{
                return byteStream.Position;
			}
			
			set
			{
			}
			
		}
	}
}