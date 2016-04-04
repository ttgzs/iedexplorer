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
using IEC61850.Common;

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
        public class IedServer : IDisposable
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

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern bool IedServer_isRunning(IntPtr self);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr IedServer_getAttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            private static extern bool IedServer_getBooleanAttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern int IedServer_getInt32AttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern long IedServer_getInt64AttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern uint IedServer_getUInt32AttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern float IedServer_getFloatAttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern ulong IedServer_getUTCTimeAttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern uint IedServer_getBitStringAttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr IedServer_getStringAttributeValue(IntPtr self, IntPtr dataAttribute);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateFloatAttributeValue(IntPtr self, IntPtr dataAttribute, float value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateInt32AttributeValue(IntPtr self, IntPtr dataAttribute, int value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateInt64AttributeValue(IntPtr self, IntPtr dataAttribute, long value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateUnsignedAttributeValue(IntPtr self, IntPtr dataAttribute, uint value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateBitStringAttributeValue(IntPtr self, IntPtr dataAttribute, uint value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateBooleanAttributeValue(IntPtr self, IntPtr dataAttribute, bool value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateVisibleStringAttributeValue(IntPtr self, IntPtr dataAttribute, string value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateUTCTimeAttributeValue(IntPtr self, IntPtr dataAttribute, ulong value);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern void IedServer_updateQuality(IntPtr self, IntPtr dataAttribute, ushort value);

            private IntPtr self = IntPtr.Zero;

            public IedServer(IedModel model)
            {
                this.self = IedServer_create(model.GetPtr());
            }

            ~IedServer()
            {
                Dispose();
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

            public bool isRunning()
            {
                if (self != IntPtr.Zero)
                    return IedServer_isRunning(self);
                return false;
            }

            public void LockDataModel()
            {
                IedServer_lockDataModel(self);
            }

            public void UnlockDataModel()
            {
                IedServer_unlockDataModel(self);
            }

            public MmsValue GetAttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero) return null;
                IntPtr val = IedServer_getAttributeValue(self, attrib.GetLibraryObject());
                if (val == IntPtr.Zero) return null;
                return new MmsValue(val);
            }

            public bool GetBooleanAttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetBooleanAttributeValue: null argument");
                return IedServer_getBooleanAttributeValue(self, attrib.GetLibraryObject());
            }

            public int GetInt32AttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetInt32AttributeValue: null argument");
                return IedServer_getInt32AttributeValue(self, attrib.GetLibraryObject());
            }

            public long GetInt64AttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetInt64AttributeValue: null argument");
                return IedServer_getInt64AttributeValue(self, attrib.GetLibraryObject());
            }

            public uint GetUInt32AttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetUInt32AttributeValue: null argument");
                return IedServer_getUInt32AttributeValue(self, attrib.GetLibraryObject());
            }

            public float GetFloatAttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetFloatAttributeValue: null argument");
                return IedServer_getFloatAttributeValue(self, attrib.GetLibraryObject());
            }

            public ulong GetUTCTimeAttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetUTCTimeAttributeValue: null argument");
                return IedServer_getUTCTimeAttributeValue(self, attrib.GetLibraryObject());
            }

            public uint GetBitStringAttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetBitStringAttributeValue: null argument");
                return IedServer_getBitStringAttributeValue(self, attrib.GetLibraryObject());
            }

            public string GetStringAttributeValue(DataAttribute attrib)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("GetStringAttributeValue: null argument");
                return Marshal.PtrToStringAnsi(IedServer_getStringAttributeValue(self, attrib.GetLibraryObject()));
            }

            public void UpdateFloatAttributeValue(DataAttribute attrib, float value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateFloatAttributeValue: null argument");
                IedServer_updateFloatAttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateInt32AttributeValue(DataAttribute attrib, int value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateInt32AttributeValue: null argument");
                IedServer_updateInt32AttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateInt64AttributeValue(DataAttribute attrib, long value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateInt64AttributeValue: null argument");
                IedServer_updateInt64AttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateUnsignedAttributeValue(DataAttribute attrib, uint value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateUnsignedAttributeValue: null argument");
                IedServer_updateUnsignedAttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateBitStringAttributeValue(DataAttribute attrib, uint value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateBitStringAttributeValue: null argument");
                IedServer_updateBitStringAttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateBooleanAttributeValue(DataAttribute attrib, bool value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateBooleanAttributeValue: null argument");
                IedServer_updateBooleanAttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateVisibleStringAttributeValue(DataAttribute attrib, string value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateBooleanAttributeValue: null argument");
                IedServer_updateVisibleStringAttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateUTCTimeAttributeValue(DataAttribute attrib, ulong value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateInt64AttributeValue: null argument");
                IedServer_updateUTCTimeAttributeValue(self, attrib.GetLibraryObject(), value);
            }

            public void UpdateQuality(DataAttribute attrib, ushort value)
            {
                if (attrib == null || attrib.GetLibraryObject() == IntPtr.Zero)
                    throw new Exception("UpdateInt64AttributeValue: null argument");
                IedServer_updateQuality(self, attrib.GetLibraryObject(), value);
            }

            public void Dispose()
            {
                if (self != IntPtr.Zero)
                    IedServer_destroy(self);
                self = IntPtr.Zero;
            }
        }
    }
}
