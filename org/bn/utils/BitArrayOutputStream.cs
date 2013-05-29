using System;
namespace org.bn.utils
{
	
	public class BitArrayOutputStream : System.IO.Stream
	{
		internal byte currentBit = 0;
        private byte[] buf = new byte[1024];
        private int count = 0;

		
		public BitArrayOutputStream()
		{
		}
		
		public BitArrayOutputStream(int initialSize)
		{
            if(initialSize > 100)
                buf = new byte[initialSize];
		}
		
		public virtual void  align()
		{
			currentBit = 0;
		}
		
        protected void ResizeBuffer(int newcount)
        {
            if (newcount > buf.Length)
            {
                byte[] newbuf = new byte[System.Math.Max(buf.Length << 1, newcount)];
                Array.Copy(buf, 0, newbuf, 0, count);
                buf = newbuf;
            }
        }

        protected void pushByteToBuffer(byte bt) 
        {
            int newcount = count + 1;
            ResizeBuffer(newcount);
            buf[count] = bt;
            count = newcount;
        }


/*		public override void  WriteByte(byte b)
		{
			lock (this)
			{
				if (currentBit == 0) {
                    pushByteToBuffer( (byte) b);
                }
				else
				{
					byte lBt = buf[count - 1];
					byte nBt = (byte) (lBt | (b >> currentBit));
					buf[count - 1] = nBt;
					lBt = (byte) (b << (8 - currentBit));
					pushByteToBuffer(lBt);
				}
			}
		}
*/
        public void  WriteByte(int b)
		{
			base.WriteByte((byte) b);
		}
		
		public override void  Write(byte[] b, int off, int len)
		{
            if (len <= 0)
                return;
			if (currentBit == 0) {
                int newcount = count + len;
                ResizeBuffer(newcount);
                Array.Copy(b, off, buf, count, len);
                count = newcount;
            }
			else
			{
				byte lBt = buf[count - 1];
				for (int i = off; i < off + len; i++)
				{
					int bufByte = b[i] < 0 ? 256 + b[i]:b[i];
					byte nBt = (byte)(lBt | (bufByte >> currentBit));
					if (i == off)
					{
						buf[count - 1] = nBt;
					}
					else
					{
						pushByteToBuffer(nBt);
					}
					lBt = (byte)(bufByte << (8 - currentBit));
				}
				pushByteToBuffer((byte) lBt);
			}
		}
		
		public virtual void  writeBit(bool val)
		{
			writeBit(val ? 1:0);
		}
		
		public virtual void  writeBit(int bit)
		{
			lock (this)
			{
				if (currentBit < 8 && currentBit > 0)
				{
					if (bit != 0)
					{
						buf[count - 1] |= (byte)(0x80 >> currentBit);
					}
				}
				else
				{
					pushByteToBuffer( (byte)(bit == 0?0:0x80));
				}
				currentBit++;
				if (currentBit >= 8)
				{
					currentBit = 0;
				}
			}
		}

        public void writeBits(int bt, int count) {        
            for(int i=count-1;i>=0;i--) {
                writeBit ( (bt >> i) & 0x1);
            }
        }


        public byte[] ToArray()
        {
            byte[] newbuf = new byte[count];
            Array.Copy(buf, 0, newbuf, 0, count);
            return newbuf;
        }


        public void WriteTo(System.IO.Stream stream)
        {
            byte[] bufTmp = ToArray();
            stream.Write(bufTmp, 0, bufTmp.Length);
        }

		public void  reset()
		{
			currentBit = 0;
		}

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { return count; }
        }

        public override long Position
        {
            get
            {
                return count;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }
 
	}
}