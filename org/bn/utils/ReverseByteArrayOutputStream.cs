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
	
	/// <summary> This class implements an output stream in which the data is 
	/// written into a reverse byte array. The buffer automatically grows as data 
	/// is written to it. 
	/// The data can be retrieved using <code>toByteArray()</code> and
	/// <code>toString()</code>.
	/// <p>
	/// Closing a <tt>ByteArrayOutputStream</tt> has no effect. The methods in
	/// this class can be called after the stream has been closed without
	/// generating an <tt>IOException</tt>.
	/// 
	/// </summary>
	public class ReverseByteArrayOutputStream: System.IO.Stream
	{
        private byte[] buf = new byte[1024];
        private int count = 0;

		public ReverseByteArrayOutputStream()
		{
		}
		
		public void WriteTo(System.IO.Stream stream)
		{
			byte[] bufTmp = ToArray();
            stream.Write(bufTmp, 0, bufTmp.Length);
		}
		
		public byte[] ToArray()
		{
            byte[] newbuf = new byte[count];
			Array.Copy(buf, buf.Length - count, newbuf, 0, count);
			return newbuf;
		}

        public char[] ToCharArray()
        {
            char[] newbuf = new char[count];
            Array.Copy(buf, buf.Length - count, newbuf, 0, count);
            return newbuf;
        }
		
		public override System.String ToString()
		{
            return new String(ToCharArray());
		}

        protected void ResizeBuffer(int newcount)
        {
            if (newcount > buf.Length)
            {
                byte[] newbuf = new byte[System.Math.Max(buf.Length << 1, newcount)];
                //Array.Copy(buf, 0, newbuf, 0, count);
                Array.Copy(buf, buf.Length - count, newbuf, newbuf.Length - count, count);
                buf = newbuf;
            }
        }

		public  void  WriteByte(int b)
		{
            WriteByte((byte)b);
		}

        public override  void  WriteByte(byte b)
		{
            lock (this)
            {
                int newcount = (int)count + 1;
                ResizeBuffer(newcount);
                buf[buf.Length - 1 - count] = b;
                count = newcount;
            }
        }
		
		public override void  Write(byte[] b, int off, int len)
		{
            lock (this)
            {
                if ((off < 0) || (off > b.Length) || (len < 0) || ((off + len) > b.Length) || ((off + len) < 0))
                {
                    throw new System.IndexOutOfRangeException();
                }
                else if (len == 0)
                {
                    return;
                }
                int newcount = count + len;
                ResizeBuffer(newcount);
                Array.Copy(b, off, buf, buf.Length - count - len, len);
                count = newcount;
            }
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