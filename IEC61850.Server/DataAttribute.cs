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

            private MmsValue mmsValue;

            public MmsValue MmsValue
            {
                get
                {
                    if (mmsValue == null)
                    {
                        sDataAttribute sda = (sDataAttribute)Marshal.PtrToStructure(self, typeof(sDataAttribute));
                        mmsValue = new MmsValue(sda.mmsValue);
                    }
                    return mmsValue;
                }
            }

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
                self = DataAttribute_create(name, parentNode.GetPtr(), (int)type, (int)fc, triggerOptions, arrayElements, sAddr);
            }

            internal DataAttribute(IntPtr newDA)
            {
                self = newDA;
            }

            ~DataAttribute()
            {
            }

            public static DataAttributeType typeFromString(string type)
            {
                try
                {
                    return (DataAttributeType)Enum.Parse(typeof(DataAttributeType), type);
                }
                catch (ArgumentException)
                {
                    return DataAttributeType.CHECK;
                }
            }

            public static FunctionalConstraint fcFromString(string FC)
            {
                try
                {
                    return (FunctionalConstraint)Enum.Parse(typeof(FunctionalConstraint), FC);
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