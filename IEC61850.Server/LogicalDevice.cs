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
        public class LogicalDevice : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr LogicalDevice_create(string name, IntPtr model);

            public LogicalDevice(string name, IedModel model)
            {
                self = LogicalDevice_create(name, model.GetPtr());
            }

            ~LogicalDevice()
            {
            }
        }
    }
}