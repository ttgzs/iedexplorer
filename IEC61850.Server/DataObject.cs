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
        public class DataObject : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr DataObject_create(string name, IntPtr parent, int arrayElements);

            public DataObject(string name, ModelNode parentNode, int arrayElements)
            {
                self = DataObject_create(name, parentNode.GetLibraryObject(), arrayElements);
            }

            internal DataObject(IntPtr newDO)
            {
                self = newDO;
            }

            ~DataObject()
            {
            }
        }

        public class CDCFactory
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr CDC_ENG_create(string dataObjectName, IntPtr parent, uint options);

            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr CDC_SAV_create(string dataObjectName, IntPtr parent, uint options, bool isIntegerNotFloat);

            public static DataObject CDC_ENG(string dataObjectName, ModelNode parent, CDCOptions options)
            {
                return new DataObject(CDC_ENG_create(dataObjectName, parent.GetLibraryObject(), (uint)options));
            }

            public static DataObject CDC_SAV(string dataObjectName, ModelNode parent, CDCOptions options, bool isIntegerNotFloat)
            {
                return new DataObject(CDC_SAV_create(dataObjectName, parent.GetLibraryObject(), (uint)options, isIntegerNotFloat));
            }
        }

        [Flags]
        public enum CDCOptions
        {
            NONE = 0,
            PICS_SUBST = (1 << 0),
            BLK_ENA = (1 << 1),
            DESC = (1 << 2),
            DESC_UNICODE = (1 << 3),
            AC_DLNDA = (1 << 4),
            AC_DLN = (1 << 5),
            UNIT = (1 << 6),
            FROZEN_VALUE = (1 << 7),
            ADDR = (1 << 8),
            ADDINFO = (1 << 9),
            INST_MAG = (1 << 10),
            RANGE = (1 << 11),
            UNIT_MULTIPLIER = (1 << 12),
            AC_SCAV = (1 << 13),
            MIN = (1 << 14),
            MAX = (1 << 15),
            AC_CLC_O = (1 << 16),
            RANGE_ANG = (1 << 17),
            PHASE_A = (1 << 18),
            PHASE_B = (1 << 19),
            PHASE_C = (1 << 20),
            PHASE_NEUT = (1 << 21),
            PHASES_ABC = (PHASE_A | PHASE_B | PHASE_C),
            PHASES_ALL = (PHASE_A | PHASE_B | PHASE_C | PHASE_NEUT),
            STEP_SIZE = (1 << 22),
            ANGLE_REF = (1 << 23),
        }

        [Flags]
        public enum CDCControl
        {
            MODEL_NONE = 0,
            MODEL_DIRECT_NORMAL = 1,
            MODEL_SBO_NORMAL = 2,
            MODEL_DIRECT_ENHANCED = 3,      //????
            MODEL_SBO_ENHANCED = 4,
            MODEL_HAS_CANCEL = (1 << 4),
            MODEL_IS_TIME_ACTIVATED = (1 << 5),
            OPTION_ORIGIN = (1 << 6),
            OPTION_CTL_NUM = (1 << 7),
            OPTION_ST_SELD = (1 << 8),
            OPTION_OP_RCVD = (1 << 9),
            OPTION_OP_OK = (1 << 10),
            OPTION_T_OP_OK = (1 << 11),
            OPTION_SBO_TIMEOUT = (1 << 12),
            OPTION_SBO_CLASS = (1 << 13),
            OPTION_OPER_TIMEOUT = (1 << 14),
        }

        [Flags]
        public enum CDCOptions61400
        {
            MIN_MX_VAL = (1 << 10),
            MAX_MX_VAL = (1 << 11),
            TOT_AV_VAL = (1 << 12),
            SDV_VAL = (1 << 13),
            INC_RATE = (1 << 14),
            DEC_RATE = (1 << 15),
            SP_ACS = (1 << 16),
            CHA_PER_RS = (1 << 17),
            CM_ACS = (1 << 18),
            TM_TOT = (1 << 19),
            COUNTING_DAILY = (1 << 20),
            COUNTING_MONTHLY = (1 << 21),
            COUNTING_YEARLY = (1 << 22),
            COUNTING_TOTAL = (1 << 23),
            COUNTING_ALL = (COUNTING_DAILY | COUNTING_MONTHLY | COUNTING_YEARLY | COUNTING_TOTAL),
        }

    }
}