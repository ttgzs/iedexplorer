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


        public class LogicalNode : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr LogicalNode_create(string name, IntPtr ldevice);

            public LogicalNode(string name, LogicalDevice ldevice)
            {
                self = LogicalNode_create(name, ldevice.GetPtr());
            }

            ~LogicalNode()
            {
            }
        }
    }
}