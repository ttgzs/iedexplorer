/*
 *  Copyright (C) 2016 Pavel Charvat
 * 
 *  This file is part of IEDExplorer.
 *
 *  IEDExplorer is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IEDExplorer is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IEDExplorer.  If not, see <http://www.gnu.org/licenses/>.
 * 
 *  SCLParser.cs was created by Joel Kaiser as an add-in to IEDExplorer.
 *  This class parses a SCL-type XML file to create a logical tree similar
 *  to that which Pavel creates from communication with an actual device.
 */

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