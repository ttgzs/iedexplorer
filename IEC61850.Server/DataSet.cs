using System;
using System.Collections.Generic;
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
        public class DataSet : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr DataSet_create(string name, IntPtr parent);

            public DataSet(string name, LogicalNode parentNode)
            {
                self = DataSet_create(name, parentNode.GetPtr());
            }

            internal DataSet(IntPtr newDS)
            {
                self = newDS;
            }

            ~DataSet()
            {
            }
        }

    }
}