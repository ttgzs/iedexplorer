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


        public class SettingGroupControlBlock : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr SettingGroupControlBlock_create(IntPtr parent, byte actSG, byte numOfSGs);

            public SettingGroupControlBlock(ModelNode parentNode, byte actSG, byte numOfSGs)
            {
                self = SettingGroupControlBlock_create(parentNode.GetPtr(), actSG, numOfSGs);
            }

            internal SettingGroupControlBlock(IntPtr newDO)
            {
                self = newDO;
            }

            ~SettingGroupControlBlock()
            {
            }
        }

    }
}
