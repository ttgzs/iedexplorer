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

        public string SCL_Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string SCL_FCDesc { get; set; }
        public string SCL_DOName { get; set; }
        public byte SCL_TrgOps { get; set; }
        public int SCL_ArraySize { get; set; }

        public string SCL_BType
        {
            get { return _bType; }
            set { _bType = value; }
        }

        public string FC
        {
            get
            {
                NodeBase nb = Parent;
                if (nb != null) do
                {
                    if (nb is NodeFC)
                        return nb.Name;
                } while (nb != null);
                return "";
            }
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
                            if (DataValue != null) val = DataValue.ToString() + "." + ((DateTime)(DataValue)).Millisecond.ToString() + " [LOC]";
                            if (DataParam != null)
                            {
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
                            /*if (DataValue != null)
                            {
                                StringBuilder sbos = new StringBuilder(32);
                                foreach (byte b in (byte[])DataValue)
                                {
                                    sbos.AppendFormat("X{0} ", b);
                                }
                                val = sbos.ToString();
                            }*/
                            if (DataValue != null) val = DataValue.ToString() + "." + ((DateTime)(DataValue)).Millisecond.ToString() + " [LOC]";
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
                    try
                    {
                        switch (DataType)
                        {
                            //case scsm_MMS_TypeEnum.utc_time:
                            // Not supported
                            //    break;
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
                            case scsm_MMS_TypeEnum.unsigned:
                                long uns;
                                if (long.TryParse(value, out uns))
                                {
                                    DataValue = uns;
                                }
                                else
                                {
                                    Logger.getLogger().LogError("NodeData.StringValue - cannot parse '" + value + "' to unsigned (internally int64)");
                                }
                                break;
                            case scsm_MMS_TypeEnum.integer:
                                long sint;
                                if (long.TryParse(value, out sint))
                                {
                                    DataValue = sint;
                                }
                                else
                                {
                                    Logger.getLogger().LogError("NodeData.StringValue - cannot parse '" + value + "' to integer (internally int64)");
                                }
                                break;
                            case scsm_MMS_TypeEnum.floating_point:
                                float fval;
                                if (float.TryParse(value, out fval))
                                {
                                    DataValue = fval;
                                }
                                else
                                {
                                    Logger.getLogger().LogError("NodeData.StringValue - cannot parse '" + value + "' to float");
                                }
                                break;
                            default:
                                Logger.getLogger().LogError("NodeData.StringValue - type '" + DataType.ToString() + "' not implemented");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.getLogger().LogError("NodeData.StringValue - cannot parse '" + value + "' to type '" + DataType.ToString() + "', exception: " + e.Message);
                    }
                }
            }
        }

        public int sAddr { get; set; }

        public override void SaveModel(List<String> lines, bool fromSCL)
        {
            // DA(<data attribute name> <nb of array elements> <type> <FC> <trigger options> <sAddr>)[=value];
            // Constructed>
            // DA(<data attribute name> <nb of array elements> 27 <FC> <trigger options> <sAddr>){…}
            if (isLeaf() || (isArray() && _childNodes[0].isLeaf()))
            {
                string line = "DA(" + Name;
                int nrElem = getArraySize();
                line += " " + nrElem + " " + MapLibiecType(DataType) + " " + MapLibiecFC(SCL_FCDesc) + " " + MapTrgOps() + " " + sAddr + ")";
                bool writeVal = false;
                // Some conditions for writing the value
                if (Name == "ctlModel") writeVal = true;

                // Finish the line
                if (writeVal)
                    line += " value=" + StringValue;
                lines.Add(line);
            }
            else
            {
                // Constructed
                int nrElem = 0;
                NodeBase nextnb = this;

                if (isArray())
                {
                    nrElem = getArraySize();
                    // Array has got an artificial level with array members, this is not part of model definition
                    if (_childNodes.Count > 0)
                        nextnb = _childNodes[0];
                }

                lines.Add("DA(" + Name + " " + nrElem + " 27 " + MapLibiecFC(SCL_FCDesc) + " " + MapTrgOps() + " " + sAddr + "){");
                foreach (NodeBase b in nextnb.GetChildNodes())
                {
                    b.SaveModel(lines, fromSCL);
                }
                lines.Add("}");
            }
        }

        public string GetFC()
        {
            NodeBase b = Parent;
            while (b != null)
            {
                if (b is NodeFC) return b.Name;
                b = b.Parent;
            }
            return "??";
        }

        public string GetDOName()
        {
            List<String> dOName = new List<string>();
            dOName.Add(Name);
            NodeBase b = Parent;
            while (b != null)
            {
                if (b is NodeFC)
                {
                    String ret = "";
                    foreach (String s in dOName)
                    {
                        ret = "." + s + ret;
                    }
                    return ret;
                }
                dOName.Add(b.Name);
                b = b.Parent;
            }
            return "";
        }

         /** FCs (Functional constraints) according to IEC 61850-7-2 */
        enum LibIecFunctionalConstraint
        {
            /** Status information */
            IEC61850_FC_ST = 0,
            /** Measurands - analog values */
            IEC61850_FC_MX = 1,
            /** Setpoint */
            IEC61850_FC_SP = 2,
            /** Substitution */
            IEC61850_FC_SV = 3,
            /** Configuration */
            IEC61850_FC_CF = 4,
            /** Description */
            IEC61850_FC_DC = 5,
            /** Setting group */
            IEC61850_FC_SG = 6,
            /** Setting group editable */
            IEC61850_FC_SE = 7,
            /** Service response / Service tracking */
            IEC61850_FC_SR = 8,
            /** Operate received */
            IEC61850_FC_OR = 9,
            /** Blocking */
            IEC61850_FC_BL = 10,
            /** Extended definition */
            IEC61850_FC_EX = 11,
            /** Control */
            IEC61850_FC_CO = 12,
            IEC61850_FC_ALL = 99,
            IEC61850_FC_NONE = -1
        }

       int MapLibiecFC(string FC)
        {
            int fco = 0;
            foreach (string s in Enum.GetNames(typeof(LibIecFunctionalConstraint)))
            {
                if (s.Substring(s.LastIndexOf("_") + 1) == FC)
                {
                    return (int)Enum.GetValues(typeof(LibIecFunctionalConstraint)).GetValue(fco);
                }
                fco++;
            }
            return -1;
        }

        enum LibIecDataAttributeType
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

        int MapLibiecType(scsm_MMS_TypeEnum DataType)
        {
            int type = 0;
            switch (DataType)
            {
                case scsm_MMS_TypeEnum.boolean:
                    type = (int)LibIecDataAttributeType.BOOLEAN;
                    break;
                case scsm_MMS_TypeEnum.floating_point:
                    type = (int)LibIecDataAttributeType.FLOAT32;
                    break;
                case scsm_MMS_TypeEnum.utc_time:
                    type = (int)LibIecDataAttributeType.TIMESTAMP;
                    break;
                case scsm_MMS_TypeEnum.bit_string:
                    type = (int)LibIecDataAttributeType.CODEDENUM;
                    if (Name == "q")
                        type = (int)LibIecDataAttributeType.QUALITY;
                    break;
                case scsm_MMS_TypeEnum.integer:
                    type = (int)LibIecDataAttributeType.INT32;
                    break;
                case scsm_MMS_TypeEnum.unsigned:
                    type = (int)LibIecDataAttributeType.INT32U;
                    break;
                case scsm_MMS_TypeEnum.binary_time:
                    type = (int)LibIecDataAttributeType.ENTRY_TIME;
                    break;
                case scsm_MMS_TypeEnum.mMSString:
                    type = (int)LibIecDataAttributeType.UNICODE_STRING_255;
                    break;
                case scsm_MMS_TypeEnum.visible_string:
                    type = (int)LibIecDataAttributeType.VISIBLE_STRING_255;
                    break;
                case scsm_MMS_TypeEnum.octet_string:
                    type = (int)LibIecDataAttributeType.OCTET_STRING_64;
                    break;
            }
            return type;
        }

        int MapTrgOps()
        {
            int trgOps = 0;


            return trgOps;
        }

    }   // class NodeData

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
