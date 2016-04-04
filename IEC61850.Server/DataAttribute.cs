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
        enum ModelNodeType
        {
            LogicalDeviceModelType,
            LogicalNodeModelType,
            DataObjectModelType,
            DataAttributeModelType
        }


        [StructLayout(LayoutKind.Sequential)]
        struct sDataAttribute
        {
            ModelNodeType modelType;
            string name;
            IntPtr parent;
            IntPtr sibling;
            IntPtr firstChild;

            int elementCount;  /* > 0 if this is an array */

            FunctionalConstraint fc;
            DataAttributeType type;

            byte triggerOptions; /* TRG_OPT_DATA_CHANGED | TRG_OPT_QUALITY_CHANGED | TRG_OPT_DATA_UPDATE */

            public IntPtr mmsValue;

            uint sAddr;
        }

        public class DataAttribute : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr DataAttribute_create(string name, IntPtr parent, int type, int fc, byte triggerOptions, int arrayElements, uint sAddr);

            private MmsValue mmsValue = null;
            private MmsType mmsType = MmsType.MMS_DATA_ACCESS_ERROR;
            private DataAttributeType daType;
            private FunctionalConstraint daFc;

            /**
             * \brief create a new data attribute and add it to a parent model node
             *
             * The parent model node has to be of type LogicalNode or DataObject
             *
             * \param name the name of the data attribute (e.g. "stVal")
             * \param parent the parent model node
             * \param type the type of the data attribute (CONSTRUCTED if the type contains sub data attributes)
             * \param fc the functional constraint (FC) of the data attribte
             * \param triggerOptions the trigger options (dupd, dchg, qchg) that cause an event notification
             * \param arrayElements the number of array elements if the data attribute is an array or 0
             * \param sAddr an optional short address
             *
             * \return the newly create DataAttribute instance
             */
            public DataAttribute(string name, ModelNode parentNode, DataAttributeType type, FunctionalConstraint fc, byte triggerOptions, int arrayElements, uint sAddr)
            {
                self = DataAttribute_create(name, parentNode.GetLibraryObject(), (int)type, (int)fc, triggerOptions, arrayElements, sAddr);
                daType = type;
                daFc = fc;
            }

            internal DataAttribute(IntPtr newDA)
            {
                self = newDA;
            }

            ~DataAttribute()
            {
            }

            public MmsValue MmsValue
            {
                get
                {
                    if (mmsValue == null)
                        mmsValueInit();
                    return mmsValue;
                }
            }

            void mmsValueInit()
            {
                sDataAttribute sda = (sDataAttribute)Marshal.PtrToStructure(self, typeof(sDataAttribute));
                if (sda.mmsValue != IntPtr.Zero)
                {
                    mmsValue = new MmsValue(sda.mmsValue);
                    mmsType = mmsValue.GetType();
                }
            }

            public MmsType GetMmsValueType()
            {
                if (mmsValue == null)
                    mmsValueInit();
                return mmsType;
            }

            public void UpdateValue(IedServer server, object DataValue)
            {
                if (DataValue == null)
                    return;
                if (server == null)
                    return;
                if (mmsValue == null)
                    mmsValueInit();
                switch (mmsType)
                {
                    /** array type (multiple elements of the same type) */
                    case MmsType.MMS_ARRAY:
                        // not supported
                        break;
                    /** structure type (multiple elements of different types) */
                    case MmsType.MMS_STRUCTURE:
                        // not supported
                        break;
                    /** boolean */
                    case MmsType.MMS_BOOLEAN:
                        server.UpdateBooleanAttributeValue(this, (bool)DataValue);
                        break;
                    /** bit string */
                    case MmsType.MMS_BIT_STRING:
                        if (DataValue is uint)
                            server.UpdateBitStringAttributeValue(this, (uint)DataValue);
                        break;
                    case MmsType.MMS_INTEGER:
                        if (daType == DataAttributeType.INT8 ||
                            daType == DataAttributeType.INT16 ||
                            daType == DataAttributeType.INT32)
                        {
                            server.UpdateInt32AttributeValue(this, (int)DataValue);
                        }
                        else if (daType == DataAttributeType.INT64)
                        {
                            server.UpdateInt64AttributeValue(this, (long)DataValue);
                        }
                        else if (daType == DataAttributeType.ENUMERATED)
                        {
                            server.UpdateInt32AttributeValue(this, (int)DataValue);
                        }
                        break;
                    /** unsigned integer */
                    case MmsType.MMS_UNSIGNED:
                        if (daType == DataAttributeType.INT8U ||
                            daType == DataAttributeType.INT16U ||
                            daType == DataAttributeType.INT32U)
                        {
                            server.UpdateUnsignedAttributeValue(this, (uint)DataValue);
                        }
                        break;
                    /** floating point value (32 or 64 bit) */
                    case MmsType.MMS_FLOAT:
                        if (daType == DataAttributeType.FLOAT32)
                        {
                            server.UpdateFloatAttributeValue(this, (float)DataValue);
                        }
                        else if (daType == DataAttributeType.FLOAT64)
                        {
                            // !!! Attention does not generate reports !!!
                            mmsValue.SetDouble((double)DataValue);
                        }
                        break;
                    /** octet string */
                    case MmsType.MMS_OCTET_STRING:
                        // !!! Attention does not generate reports !!!
                        mmsValue.setOctetString((byte[])DataValue);
                        break;
                    /** visible string - ANSI string */
                    case MmsType.MMS_VISIBLE_STRING:
                        server.UpdateVisibleStringAttributeValue(this, (string)DataValue);
                        break;
                    /** Generalized time */
                    case MmsType.MMS_GENERALIZED_TIME:
                        // not supported
                        break;
                    case MmsType.MMS_BINARY_TIME:
                        // !!! Attention does not generate reports !!!
                        mmsValue.SetBinaryTime((ulong)DataValue);
                        break;
                    /** Binary coded decimal (BCD) - not used */
                    case MmsType.MMS_BCD:
                        // Not supported
                        break;
                    /** object ID - not used */
                    case MmsType.MMS_OBJ_ID:
                        // Not supported
                        break;
                    /** Unicode string */
                    case MmsType.MMS_STRING:
                        // Not supported
                        //mmsValue.SetMmsString((string)DataValue);
                        break;
                    /** UTC time */
                    case MmsType.MMS_UTC_TIME:
                        server.UpdateUTCTimeAttributeValue(this, (ulong)DataValue);
                        //mmsValue.SetUtcTimeMs((ulong)DataValue);
                        break;
                    /** will be returned in case of an error (contains error code) */
                    case MmsType.MMS_DATA_ACCESS_ERROR:
                        // Not supported
                        break;
                }
            }

            public static DataAttributeType typeFromSCLString(string type)
            {
                try
                {
                    string validType = type.ToUpper();
                    if (validType.Contains("ENUM")) validType = "ENUMERATED";
                    if (validType.Contains("VISSTRING")) validType = validType.Replace("VISSTRING", "VISIBLE_STRING_");
                    if (validType.Contains("OCTET")) validType = validType.Replace("OCTET", "OCTET_STRING_");
                    if (validType.Contains("UNICODE")) validType = validType.Replace("UNICODE", "UNICODE_STRING_");
                    if (validType.Contains("STRUCT")) validType = "CONSTRUCTED";
                    if (validType == "DBPOS" || validType == "TCMD") validType = "CODEDENUM";
                    if (validType == "ENTRYTIME") validType = "ENTRY_TIME";
                    return (DataAttributeType)Enum.Parse(typeof(DataAttributeType), validType);
                }
                catch (ArgumentException)
                {
                    return DataAttributeType.BOOLEAN;
                }
            }

            public static FunctionalConstraint fcFromString(string FC)
            {
                try
                {
                    return (FunctionalConstraint)Enum.Parse(typeof(FunctionalConstraint), FC.ToUpper());
                }
                catch (ArgumentException)
                {
                    return FunctionalConstraint.NONE;
                }
            }
        }

        public enum DataAttributeType
        {
            BOOLEAN = 0,/* int */
            INT8 = 1,   /* int8_t */
            INT16 = 2,  /* int16_t */
            INT32 = 3,  /* int32_t */
            INT64 = 4,  /* int64_t */
            INT128 = 5, /* no native mapping! */
            INT8U = 6,  /* uint8_t */
            INT16U = 7, /* uint16_t */
            INT24U = 8, /* uint32_t */
            INT32U = 9, /* uint32_t */
            FLOAT32 = 10, /* float */
            FLOAT64 = 11, /* double */
            ENUMERATED = 12,
            OCTET_STRING_64 = 13,
            OCTET_STRING_6 = 14,
            OCTET_STRING_8 = 15,
            VISIBLE_STRING_32 = 16,
            VISIBLE_STRING_64 = 17,
            VISIBLE_STRING_65 = 18,
            VISIBLE_STRING_129 = 19,
            VISIBLE_STRING_255 = 20,
            UNICODE_STRING_255 = 21,
            TIMESTAMP = 22,
            QUALITY = 23,
            CHECK = 24,
            CODEDENUM = 25,
            GENERIC_BITSTRING = 26,
            CONSTRUCTED = 27,
            ENTRY_TIME = 28,
            PHYCOMADDR = 29
        }

    }
}