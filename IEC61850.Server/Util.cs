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


        public static class Util
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern ulong Hal_getTimeInMs();

            public static ulong GetTimeInMs()
            {
                return Hal_getTimeInMs();
            }
        }
    }
}