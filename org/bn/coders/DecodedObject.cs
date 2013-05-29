using System;
using System.Collections.Generic;
using System.Text;

namespace org.bn.coders
{
    public class DecodedObject<T>
    {
        internal T value;
        internal int size;

        public T Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }

        }

        virtual public int Size
        {
            get
            {
                return size;
            }

            set
            {
                this.size = value;
            }

        }

        public DecodedObject()
        {
        }

        public DecodedObject(T result)
        {
            Value = result;
        }

        public DecodedObject(T result, int size)
        {
            Value = result;
            Size = size;
        }
    }
}
