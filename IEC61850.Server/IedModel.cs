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
        public class IedModel : IDisposable
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr IedModel_create(string name);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedModel_destroy(IntPtr self);

            IntPtr self = IntPtr.Zero;

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
                Dispose();
            }

            public IntPtr GetPtr()
            {
                return self;
            }

            public void Dispose()
            {
                if (self != IntPtr.Zero)
                    IedModel_destroy(self);
                self = IntPtr.Zero;
            }

            public IntPtr GetLibraryObject()
            {
                return self;
            }
        }
    }
}