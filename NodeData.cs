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
                                if (((byte)(DataParam) & 0x40) > 0)     // TimQualTimeBaseErr
                                    val = DataValue.ToString() + "." + ((DateTime)(DataValue)).Millisecond.ToString() + " TimQualTimeBaseErr";
                                else
                                    val = DataValue.ToString() + "." + ((DateTime)(DataValue)).Millisecond.ToString();
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
                                val = sb.ToString();
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
}
