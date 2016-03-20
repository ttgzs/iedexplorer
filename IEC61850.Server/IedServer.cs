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
        class IedServer
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr IedServer_create(IntPtr model);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_destroy(IntPtr self);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_start(IntPtr self, int tcpPort);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_stop(IntPtr self);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_lockDataModel(IntPtr self);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_unlockDataModel(IntPtr self);

            private IntPtr self;

            public IedServer(IedModel model)
            {
                this.self = IedServer_create(model.GetPtr());
            }

            ~IedServer()
            {
                if (self != IntPtr.Zero)
                    IedServer_destroy(self);
            }

            public IntPtr GetPtr()
            {
                return self;
            }

            public void Start(int tcpPort)
            {
                IedServer_start(self, tcpPort);
            }

            public void Stop()
            {
                IedServer_stop(self);
            }

            public void LockDataModel()
            {
                IedServer_lockDataModel(self);
            }

            public void UnlockDataModel()
            {
                IedServer_unlockDataModel(self);
            }
        }
    }
}