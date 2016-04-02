/*
 *  Copyright (C) 2013 Pavel Charvat
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
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    public enum scsm_MMS_TypeEnum
    {
        array,
        structure,
        boolean,
        bit_string,
        integer,
        unsigned,
        floating_point,
        octet_string,
        visible_string,
        generalized_time,
        binary_time,
        bcd,
        objId,
        mMSString,
        utc_time,
        data_access_error
    }
    /*
        MMS_ARRAY = 0,
        MMS_STRUCTURE = 1,
        MMS_BOOLEAN = 2,
        MMS_BIT_STRING = 3,
        MMS_INTEGER = 4,
        MMS_UNSIGNED = 5,
        MMS_FLOAT = 6,
        MMS_OCTET_STRING = 7,
        MMS_VISIBLE_STRING = 8,
        MMS_GENERALIZED_TIME = 9,
        MMS_BINARY_TIME = 10,
        MMS_BCD = 11,
        MMS_OBJ_ID = 12,
        MMS_STRING = 13,
        MMS_UTC_TIME = 14,
        MMS_DATA_ACCESS_ERROR = 15,
    */
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
