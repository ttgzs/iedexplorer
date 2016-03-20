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
        public class IedModel
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr IedModel_create(string name);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedModel_destroy(IntPtr self);

            IntPtr self;

            /// <summary>
            /// brief create a new IedModel instance
            /// The IedModel object is the root node of an IEC 61850 service data model.
            /// param name the name of the IedModel or NULL (optional - NOT YET USED)
            /// </summary>
            public IedModel(string name)
            {
                self = IedModel_create(name);
            }

            ~IedModel()
            {
                if (self != IntPtr.Zero)
                    IedModel_destroy(self);
            }

            public IntPtr GetPtr()
            {
                return self;
            }
        }
    }
}