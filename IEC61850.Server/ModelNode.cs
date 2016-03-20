using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

/// <summary>
/// IEC 61850 API for the libiec61850 .NET wrapper library
/// </summary>
namespace IEC61850
{
    /// <summary>
    /// IEC 61850 server API.
    /// </summary>
    namespace Server
    {


        public abstract class ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr ModelNode_getChild(IntPtr node, string name);

            protected IntPtr self;

            internal IntPtr GetPtr()
            {
                return self;
            }

            public DataAttribute GetChild_DataAttribute(string name)
            {
                IntPtr da = ModelNode_getChild(self, name);
                return new DataAttribute(da);
            }
        }
    }
}