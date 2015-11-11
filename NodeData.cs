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
    class NodeData : NodeBase
    {
        private scsm_MMS_TypeEnum _dataType = scsm_MMS_TypeEnum.structure;
        private string _bType = "";
        private string _type = "";
        private Object _dataValue = null;
        private Object _dataParam = null;
        private Object _valueTag = null;
        public event EventHandler ValueChanged;

        public NodeData(string Name)
            : base(Name)
        {
        }

        public scsm_MMS_TypeEnum DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string FCDesc { get; set; }
        public string DOName { get; set; }

        public string BType
        {
            get { return _bType; }
            set { _bType = value; }
        }

        public Object DataValue
        {
            get
            {
                lock (this)
                    return _dataValue;
            }
            set
            {
                bool fire = false;

                lock (this)
                    if (_dataValue == null || !_dataValue.Equals(value))
                    {
                        _dataValue = value;
                        fire = true;
                    }
                if (fire && ValueChanged != null)
                {
                    ValueChanged(this, new EventArgs());
                }
            }
        }

        public Object DataParam
        {
            get
            {
                return _dataParam;
            }
            set
            {
                _dataParam = value;
            }
        }

        public object ValueTag
        {
            get
            {
                return _valueTag;
            }
            set
            {
                _valueTag = value;
            }
        }

        internal override NodeBase FindNodeByValue(scsm_MMS_TypeEnum dataType, object dataValue, ref NodeBase ContinueAfter)
        {
            if (dataValue == null)
                return null;
            NodeBase res = null;
            if (_childNodes.Count > 0)
            {
                foreach (NodeBase b in _childNodes)
                {
                    res = b.FindNodeByValue(dataType, dataValue, ref ContinueAfter);
                    if (res != null && ContinueAfter == null)
                        return res;
                    if (res != null && ContinueAfter != null && res == ContinueAfter)
                        ContinueAfter = null;
                }
            }
            else
            {
                if (dataType == this.DataType &&
                    this.DataValue != null && dataValue.ToString() == DataValue.ToString())
                {
                    return this;
                }
            }
            return null;
        }

        public string StringValue
        {
            get
            {
                string val = "";
                if (DataValue != null)
                {
                    switch (DataType)
                    {
                        case scsm_MMS_TypeEnum.utc_time:
                            if (DataParam != null)
                            {
                                val = DataValue.ToString() + "." + ((DateTime)(DataValue)).Millisecond.ToString();
                                if (((byte)(DataParam) & 0x40) > 0)     // TimQualTimeBaseErr
                                    val += " TimQualTimeBaseErr";
                            }
                                break;
                        case scsm_MMS_TypeEnum.bit_string:
                            if (DataParam != null)
                            {
                                byte[] bbval = (byte[])DataValue;
                                int blen = bbval.Length;
                                int trail = (int)DataParam;

                                StringBuilder sb = new StringBuilder(32);
                                for (int i = 0; i < blen * 8 - trail; i++)
                                {
                                    if (((bbval[(i / 8)] << (i % 8)) & 0x80) > 0)
                                        sb.Append(1);     //.Insert(0, 1);
                                    else
                                        sb.Append(0);     //.Insert(0, 0);
                                }

                                switch (Name)    
                                {
                                    case "q":       // Quality descriptor
                                        DataQuality dq = DataQuality.NONE;
                                        dq = dq.fromBytes(bbval);
                                        sb.Append(" [");
                                        sb.Append(dq.ToString());
                                        sb.Append("]");
                                        break;
                                    case "TrgOps":  // Trigger Options
                                        TriggerOptions tr = TriggerOptions.NONE;
                                        tr = tr.fromBytes(bbval);
                                        sb.Append(" [");
                                        sb.Append(tr.ToString());
                                        sb.Append("]");
                                        break;
                                    case "OptFlds":  // Optional fields
                                        ReportOptions ro = ReportOptions.NONE;
                                        ro = ro.fromBytes(bbval);
                                        sb.Append(" [");
                                        sb.Append(ro.ToString());
                                        sb.Append("]");
                                        break;
                                }
                                val = sb.ToString();
                            }
                            break;
                        case scsm_MMS_TypeEnum.binary_time:
                            if (DataValue != null)
                            {
                                StringBuilder sbos = new StringBuilder(32);
                                foreach (byte b in (byte[])DataValue)
                                {
                                    sbos.AppendFormat("X", b);
                                }
                                val = sbos.ToString();
                            }
                            break;
                        case scsm_MMS_TypeEnum.octet_string:
                            if (DataValue != null)
                            {
                                byte[] ba = System.Text.Encoding.ASCII.GetBytes(DataValue.ToString());
                                val = BitConverter.ToString(ba);
                            }
                            break;
                        default:
                            val = DataValue.ToString();
                            break;
                    }
                }
                return val;
            }
            set
            {
                if (value != null && value != "")
                {
                    switch (DataType)
                    {
                        case scsm_MMS_TypeEnum.utc_time:
                            // Not supported
                            break;
                        case scsm_MMS_TypeEnum.bit_string:
                            byte[] bbval = (byte[])DataValue;
                            int blen = bbval.Length;
                            int trail = (int)DataParam;

                            StringBuilder sb = new StringBuilder(32);
                            for (int i = 0; i < blen * 8 - trail; i++)
                            {
                                if (((bbval[(i / 8)] << (i % 8)) & 0x80) > 0)
                                    sb.Append(1);     //.Insert(0, 1);
                                else
                                    sb.Append(0);     //.Insert(0, 0);
                            }
                            //val = sb.ToString();
                            break;
                        case scsm_MMS_TypeEnum.boolean:
                            if (value.StartsWith("0") || value.StartsWith("f", StringComparison.CurrentCultureIgnoreCase))
                                DataValue = false;
                            if (value.StartsWith("1") || value.StartsWith("t", StringComparison.CurrentCultureIgnoreCase))
                                DataValue = true;
                            break;
                        case scsm_MMS_TypeEnum.visible_string:
                            DataValue = value;
                            break;
                        case scsm_MMS_TypeEnum.octet_string:
                            DataValue = Encoding.ASCII.GetBytes(value);
                            break;
                        default:
                            //val = DataValue.ToString();
                            break;
                    }
                }
            }
        }
    }

    [Flags]
    public enum DataQuality
    {
        NONE = 0,
        VALIDITY0 = 0x01, // BIT "0" IN MMS INTERPRETATION
        VALIDITY1 = 0x02,
        OVERFLOW = 0x04,
        OUT_OF_RANGE = 0x08,
        BAD_REFERENCE = 0x10,
        OSCILLATORY = 0x20,
        FAILURE = 0x40,
        OLD_DATA = 0x80,
        // byte border
        INCONSISTENT = 0x100,
        INACCURATE = 0x200,
        SOURCE = 0x400,
        TEST = 0x800,
        OPERATOR_BLOCKED = 0x1000, // BIT "12" IN MMS INTERPRETATION
    }

    public static class DataEnumExtensions
    {
        public static DataQuality fromBytes(this DataQuality res, byte[] value)
        {
            res = DataQuality.NONE;
            if (value == null || value.Length < 1) return res;
            if ((value[0] & Scsm_MMS.DatQualValidity0) == Scsm_MMS.DatQualValidity0) res |= DataQuality.VALIDITY0;
            if ((value[0] & Scsm_MMS.DatQualValidity1) == Scsm_MMS.DatQualValidity1) res |= DataQuality.VALIDITY1;
            if ((value[0] & Scsm_MMS.DatQualOverflow) == Scsm_MMS.DatQualOverflow) res |= DataQuality.OVERFLOW;
            if ((value[0] & Scsm_MMS.DatQualOutOfRange) == Scsm_MMS.DatQualOutOfRange) res |= DataQuality.OUT_OF_RANGE;
            if ((value[0] & Scsm_MMS.DatQualBadReference) == Scsm_MMS.DatQualBadReference) res |= DataQuality.BAD_REFERENCE;
            if ((value[0] & Scsm_MMS.DatQualOscillatory) == Scsm_MMS.DatQualOscillatory) res |= DataQuality.OSCILLATORY;
            if ((value[0] & Scsm_MMS.DatQualFailure) == Scsm_MMS.DatQualFailure) res |= DataQuality.FAILURE;
            if ((value[0] & Scsm_MMS.DatQualOldData) == Scsm_MMS.DatQualOldData) res |= DataQuality.OLD_DATA;
            if (value.Length < 2) return res;
            if ((value[1] & Scsm_MMS.DatQualInconsistent) == Scsm_MMS.DatQualInconsistent) res |= DataQuality.INCONSISTENT;
            if ((value[1] & Scsm_MMS.DatQualInaccurate) == Scsm_MMS.DatQualInaccurate) res |= DataQuality.INACCURATE;
            if ((value[1] & Scsm_MMS.DatQualSource) == Scsm_MMS.DatQualSource) res |= DataQuality.SOURCE;
            if ((value[1] & Scsm_MMS.DatQualTest) == Scsm_MMS.DatQualTest) res |= DataQuality.TEST;
            if ((value[1] & Scsm_MMS.DatQualOperatorBlocked) == Scsm_MMS.DatQualOperatorBlocked) res |= DataQuality.OPERATOR_BLOCKED;
            return res;
        }

        public static byte[] toBytes(this DataQuality inp)
        {
            byte[] res = new byte[2];

            if ((inp & DataQuality.VALIDITY0) == DataQuality.VALIDITY0) res[0] |= Scsm_MMS.DatQualValidity0;
            if ((inp & DataQuality.VALIDITY1) == DataQuality.VALIDITY1) res[0] |= Scsm_MMS.DatQualValidity1;
            if ((inp & DataQuality.OVERFLOW) == DataQuality.OVERFLOW) res[0] |= Scsm_MMS.DatQualOverflow;
            if ((inp & DataQuality.OUT_OF_RANGE) == DataQuality.OUT_OF_RANGE) res[0] |= Scsm_MMS.DatQualOutOfRange;
            if ((inp & DataQuality.BAD_REFERENCE) == DataQuality.BAD_REFERENCE) res[0] |= Scsm_MMS.DatQualBadReference;
            if ((inp & DataQuality.OSCILLATORY) == DataQuality.OSCILLATORY) res[0] |= Scsm_MMS.DatQualOscillatory;
            if ((inp & DataQuality.FAILURE) == DataQuality.FAILURE) res[0] |= Scsm_MMS.DatQualFailure;
            if ((inp & DataQuality.OLD_DATA) == DataQuality.OLD_DATA) res[0] |= Scsm_MMS.DatQualOldData;
            if ((inp & DataQuality.INCONSISTENT) == DataQuality.INCONSISTENT) res[1] |= Scsm_MMS.DatQualInconsistent;
            if ((inp & DataQuality.INACCURATE) == DataQuality.INACCURATE) res[1] |= Scsm_MMS.DatQualInaccurate;
            if ((inp & DataQuality.SOURCE) == DataQuality.SOURCE) res[1] |= Scsm_MMS.DatQualSource;
            if ((inp & DataQuality.TEST) == DataQuality.TEST) res[1] |= Scsm_MMS.DatQualTest;
            if ((inp & DataQuality.OPERATOR_BLOCKED) == DataQuality.OPERATOR_BLOCKED) res[1] |= Scsm_MMS.DatQualOperatorBlocked;
            return res;
        }
    }
}
