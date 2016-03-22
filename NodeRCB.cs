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
    class NodeRCB : NodeBase
    {
        //bool _defined = false;

        public NodeRCB(string Name)
            : base(Name)
        {
            Activated = false;
        }

        public bool Activated { get; set; }

        public NodeVL dataset { get; set; }

        public bool isBuffered { get { if (isBufLock) return isBufSet; else return PurgeBuf_present; } set { isBufLock = true; isBufSet = value; } }
        bool isBufSet = false;
        bool isBufLock = false;

        public bool isIndexed { get; set; }

        public uint maxRptEnabled { get; set; }

        NodeData _RptID;
        public string RptID
        {
            get
            {
                if (_RptID == null) _RptID = (NodeData)FindChildNode("RptID");
                if (_RptID != null)
                    return (string)_RptID.DataValue;
                else
                    return "";
            }
            set
            {
                if (_RptID == null) _RptID = (NodeData)FindChildNode("RptID");
                if (_RptID != null)
                    _RptID.DataValue = value;
            }
        }
        public bool RptID_present
        {
            get
            {
                if (_RptID != null) return true;
                _RptID = (NodeData)FindChildNode("RptID");
                if (_RptID != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _RptEna;
        public bool RptEna
        {
            get
            {
                if (_RptEna == null) _RptEna = (NodeData)FindChildNode("RptEna");
                if (_RptEna != null)
                    return (bool)_RptEna.DataValue;
                else
                    return false;
            }
            set
            {
                if (_RptEna == null) _RptEna = (NodeData)FindChildNode("RptEna");
                if (_RptEna != null)
                    _RptEna.DataValue = value;
            }
        }
        public bool RptEna_present
        {
            get
            {
                if (_RptEna != null) return true;
                _RptEna = (NodeData)FindChildNode("RptEna");
                if (_RptEna != null)
                    return true;
                else
                    return false;
            }
        }

        #region UnbufferedReport
        NodeData _Resv;
        public bool Resv
        {
            get
            {
                if (_Resv == null) _Resv = (NodeData)FindChildNode("Resv");
                if (_Resv != null)
                    return (bool)_Resv.DataValue;
                else
                    return false;
            }
            set
            {
                if (_Resv == null) _Resv = (NodeData)FindChildNode("Resv");
                if (_Resv != null)
                    _Resv.DataValue = value;
            }
        }
        public bool Resv_present
        {
            get
            {
                if (_Resv != null) return true;
                _Resv = (NodeData)FindChildNode("Resv");
                if (_Resv != null)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        NodeData _DatSet;
        public string DatSet
        {
            get
            {
                if (_DatSet == null) _DatSet = (NodeData)FindChildNode("DatSet");
                if (_DatSet != null)
                    return (string)_DatSet.DataValue;
                else
                    return "";
            }
            set
            {
                if (_DatSet == null) _DatSet = (NodeData)FindChildNode("DatSet");
                if (_DatSet != null)
                    _DatSet.DataValue = value;
            }
        }
        public bool DatSet_present
        {
            get
            {
                if (_DatSet != null) return true;
                _DatSet = (NodeData)FindChildNode("DatSet");
                if (_DatSet != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _ConfRev;
        public uint ConfRev
        {
            get
            {
                if (_ConfRev == null) _ConfRev = (NodeData)FindChildNode("ConfRev");
                if (_ConfRev != null)
                    return (uint)_ConfRev.DataValue;
                else
                    return 0;
            }
        }
        public bool ConfRev_present
        {
            get
            {
                if (_ConfRev != null) return true;
                _ConfRev = (NodeData)FindChildNode("ConfRev");
                if (_ConfRev != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _OptFlds;
        public ReportOptions OptFlds
        {
            get
            {
                if (_OptFlds == null) _OptFlds = (NodeData)FindChildNode("OptFlds");
                if (_OptFlds != null)
                {
                    byte[] val = (byte[])_OptFlds.DataValue;
                    ReportOptions ro = ReportOptions.NONE;
                    return ro.fromBytes(val);
                }
                else
                    return 0;
            }
            set
            {
                if (_OptFlds == null) _OptFlds = (NodeData)FindChildNode("OptFlds");
                if (_OptFlds != null)
                {
                    _OptFlds.DataValue = value.toBytes();
                }
            }
        }

        public bool OptFlds_present
        {
            get
            {
                if (_OptFlds != null) return true;
                _OptFlds = (NodeData)FindChildNode("OptFlds");
                if (_OptFlds != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _BufTm;
        public uint BufTm
        {
            get
            {
                if (_BufTm == null) _BufTm = (NodeData)FindChildNode("BufTm");
                if (_BufTm != null)
                    return Convert.ToUInt32(_BufTm.DataValue);
                else
                    return 0;
            }
            set
            {
                if (_BufTm == null) _BufTm = (NodeData)FindChildNode("BufTm");
                if (_BufTm != null)
                    _BufTm.DataValue = (long)value;
            }
        }
        public bool BufTm_present
        {
            get
            {
                if (_BufTm != null) return true;
                _BufTm = (NodeData)FindChildNode("BufTm");
                if (_BufTm != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _SqNum;
        public uint SqNum
        {
            get
            {
                if (_SqNum == null) _SqNum = (NodeData)FindChildNode("SqNum");
                if (_SqNum != null)
                    return (uint)_SqNum.DataValue;
                else
                    return 0;
            }
        }
        public bool SqNum_present
        {
            get
            {
                if (_SqNum != null) return true;
                _SqNum = (NodeData)FindChildNode("SqNum");
                if (_SqNum != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _TrgOps;
        public TriggerOptions TrgOps
        {
            get
            {
                if (_TrgOps == null) _TrgOps = (NodeData)FindChildNode("TrgOps");
                if (_TrgOps != null)
                {
                    byte[] val = (byte[])_TrgOps.DataValue;
                    TriggerOptions to = TriggerOptions.NONE;
                    return to.fromBytes(val);
                }
                else
                    return 0;
            }
            set
            {
                if (_TrgOps == null) _TrgOps = (NodeData)FindChildNode("TrgOps");
                if (_TrgOps != null)
                {
                    _TrgOps.DataValue = value.toBytes();
                }
            }
        }

        public bool TrgOps_present
        {
            get
            {
                if (_TrgOps != null) return true;
                _TrgOps = (NodeData)FindChildNode("TrgOps");
                if (_TrgOps != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _IntgPd;
        public uint IntgPd
        {
            get
            {
                if (_IntgPd == null) _IntgPd = (NodeData)FindChildNode("IntgPd");
                if (_IntgPd != null)
                    return Convert.ToUInt32(_IntgPd.DataValue);
                else
                    return 0;
            }
            set
            {
                if (_IntgPd == null) _IntgPd = (NodeData)FindChildNode("IntgPd");
                if (_IntgPd != null)
                    _IntgPd.DataValue = (long)value;
            }
        }
        public bool IntgPd_present
        {
            get
            {
                if (_IntgPd != null) return true;
                _IntgPd = (NodeData)FindChildNode("IntgPd");
                if (_IntgPd != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _GI;
        public bool GI
        {
            get
            {
                if (_GI == null) _GI = (NodeData)FindChildNode("GI");
                if (_GI != null)
                    return (bool)_GI.DataValue;
                else
                    return false;
            }
            set
            {
                if (_GI == null) _GI = (NodeData)FindChildNode("GI");
                if (_GI != null)
                    _GI.DataValue = value;
            }
        }
        public bool GI_present
        {
            get
            {
                if (_GI != null) return true;
                _GI = (NodeData)FindChildNode("GI");
                if (_GI != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _Owner;
        public string Owner
        {
            get
            {
                if (_Owner == null) _Owner = (NodeData)FindChildNode("Owner");
                if (_Owner != null)
                    return (string)_Owner.DataValue;
                else
                    return "";
            }
        }
        public bool Owner_present
        {
            get
            {
                if (_Owner != null) return true;
                _Owner = (NodeData)FindChildNode("Owner");
                if (_Owner != null)
                    return true;
                else
                    return false;
            }
        }

        #region BufferedReport

        NodeData _PurgeBuf;
        public bool PurgeBuf
        {
            get
            {
                if (_PurgeBuf == null) _PurgeBuf = (NodeData)FindChildNode("PurgeBuf");
                if (_PurgeBuf != null)
                    return (bool)_PurgeBuf.DataValue;
                else
                    return false;
            }
            set
            {
                if (_PurgeBuf == null) _PurgeBuf = (NodeData)FindChildNode("PurgeBuf");
                if (_PurgeBuf != null)
                    _PurgeBuf.DataValue = value;
            }
        }
        public bool PurgeBuf_present
        {
            get
            {
                if (_PurgeBuf != null) return true;
                _PurgeBuf = (NodeData)FindChildNode("PurgeBuf");
                if (_PurgeBuf != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _EntryID;
        public string EntryID
        {
            get
            {
                if (_EntryID == null) _EntryID = (NodeData)FindChildNode("EntryID");
                if (_EntryID != null)
                    return (string)_EntryID.DataValue;
                else
                    return "";
            }
            set
            {
                if (_EntryID == null) _EntryID = (NodeData)FindChildNode("EntryID");
                if (_EntryID != null)
                    _EntryID.DataValue = value;
            }
        }
        public bool EntryID_present
        {
            get
            {
                if (_EntryID != null) return true;
                _EntryID = (NodeData)FindChildNode("EntryID");
                if (_EntryID != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _TimeOfEntry;
        public string TimeOfEntry
        {
            get
            {
                if (_TimeOfEntry == null) _TimeOfEntry = (NodeData)FindChildNode("TimeOfEntry");
                if (_TimeOfEntry != null)
                    return (string)_TimeOfEntry.StringValue;
                else
                    return "";
            }
        }
        public bool TimeOfEntry_present
        {
            get
            {
                if (_TimeOfEntry != null) return true;
                _TimeOfEntry = (NodeData)FindChildNode("TimeOfEntry");
                if (_TimeOfEntry != null)
                    return true;
                else
                    return false;
            }
        }

        NodeData _ResvTms;
        public uint ResvTms
        {
            get
            {
                if (_ResvTms == null) _ResvTms = (NodeData)FindChildNode("ResvTms");
                if (_ResvTms != null)
                    return Convert.ToUInt32(_ResvTms.DataValue);
                else
                    return 0;
            }
            set
            {
                if (_ResvTms == null) _ResvTms = (NodeData)FindChildNode("ResvTms");
                if (_ResvTms != null)
                    _ResvTms.DataValue = (long)value;
            }
        }
        public bool ResvTms_present
        {
            get
            {
                if (_ResvTms != null) return true;
                _ResvTms = (NodeData)FindChildNode("ResvTms");
                if (_ResvTms != null)
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }

    [Flags]
    public enum TriggerOptions
    {
        NONE = 0,
        /** send report when value of data changed */
        DATA_CHANGED = 1,
        /** send report when quality of data changed */
        QUALITY_CHANGED = 2,
        /** send report when data or quality is updated */
        DATA_UPDATE = 4,
        /** periodic transmission of all data set values */
        INTEGRITY = 8,
        /** general interrogation (on client request) */
        GI = 16
    }

    [Flags]
    public enum ReportOptions
    {
        NONE = 0,
        SEQ_NUM = 1,
        TIME_STAMP = 2,
        REASON_FOR_INCLUSION = 4,
        DATA_SET = 8,
        DATA_REFERENCE = 16,
        BUFFER_OVERFLOW = 32,
        ENTRY_ID = 64,
        CONF_REV = 128
    }

    public static class OptionsEnumExtensions
    {
        public static ReportOptions fromBytes(this ReportOptions res, byte[] value)
        {
            res = ReportOptions.NONE;
            if (value == null || value.Length < 1) return res;
            if ((value[0] & Scsm_MMS.OptFldsSeqNum) == Scsm_MMS.OptFldsSeqNum) res |= ReportOptions.SEQ_NUM;
            if ((value[0] & Scsm_MMS.OptFldsTimeOfEntry) == Scsm_MMS.OptFldsTimeOfEntry) res |= ReportOptions.TIME_STAMP;
            if ((value[0] & Scsm_MMS.OptFldsReasonCode) == Scsm_MMS.OptFldsReasonCode) res |= ReportOptions.REASON_FOR_INCLUSION;
            if ((value[0] & Scsm_MMS.OptFldsDataSet) == Scsm_MMS.OptFldsDataSet) res |= ReportOptions.DATA_SET;
            if ((value[0] & Scsm_MMS.OptFldsDataReference) == Scsm_MMS.OptFldsDataReference) res |= ReportOptions.DATA_REFERENCE;
            if ((value[0] & Scsm_MMS.OptFldsOvfl) == Scsm_MMS.OptFldsOvfl) res |= ReportOptions.BUFFER_OVERFLOW;
            if ((value[0] & Scsm_MMS.OptFldsEntryID) == Scsm_MMS.OptFldsEntryID) res |= ReportOptions.ENTRY_ID;
            if (value.Length < 2) return res;
            if ((value[1] & Scsm_MMS.OptFldsConfRev) == Scsm_MMS.OptFldsConfRev) res |= ReportOptions.CONF_REV;
            return res;
        }

        public static byte[] toBytes(this ReportOptions inp)
        {
            byte[] res = new byte[2];

            if ((inp & ReportOptions.SEQ_NUM) == ReportOptions.SEQ_NUM) res[0] |= Scsm_MMS.OptFldsSeqNum;
            if ((inp & ReportOptions.TIME_STAMP) == ReportOptions.TIME_STAMP) res[0] |= Scsm_MMS.OptFldsTimeOfEntry;
            if ((inp & ReportOptions.REASON_FOR_INCLUSION) == ReportOptions.REASON_FOR_INCLUSION) res[0] |= Scsm_MMS.OptFldsReasonCode;
            if ((inp & ReportOptions.DATA_SET) == ReportOptions.DATA_SET) res[0] |= Scsm_MMS.OptFldsDataSet;
            if ((inp & ReportOptions.DATA_REFERENCE) == ReportOptions.DATA_REFERENCE) res[0] |= Scsm_MMS.OptFldsDataReference;
            if ((inp & ReportOptions.BUFFER_OVERFLOW) == ReportOptions.BUFFER_OVERFLOW) res[0] |= Scsm_MMS.OptFldsOvfl;
            if ((inp & ReportOptions.ENTRY_ID) == ReportOptions.ENTRY_ID) res[0] |= Scsm_MMS.OptFldsEntryID;
            if ((inp & ReportOptions.CONF_REV) == ReportOptions.CONF_REV) res[1] |= Scsm_MMS.OptFldsConfRev;
            return res;
        }

        public static TriggerOptions fromBytes(this TriggerOptions res, byte[] value)
        {
            res = TriggerOptions.NONE;
            if (value == null || value.Length < 1) return res;
            if ((value[0] & Scsm_MMS.TrgOpsDataChange) == Scsm_MMS.TrgOpsDataChange) res |= TriggerOptions.DATA_CHANGED;
            if ((value[0] & Scsm_MMS.TrgOpsQualChange) == Scsm_MMS.TrgOpsQualChange) res |= TriggerOptions.QUALITY_CHANGED;
            if ((value[0] & Scsm_MMS.TrgOpsDataActual) == Scsm_MMS.TrgOpsDataActual) res |= TriggerOptions.DATA_UPDATE;
            if ((value[0] & Scsm_MMS.TrgOpsIntegrity) == Scsm_MMS.TrgOpsIntegrity) res |= TriggerOptions.INTEGRITY;
            if ((value[0] & Scsm_MMS.TrgOpsGI) == Scsm_MMS.TrgOpsGI) res |= TriggerOptions.GI;
            return res;
        }

        public static byte[] toBytes(this TriggerOptions inp)
        {
            byte[] res = new byte[1];

            if ((inp & TriggerOptions.DATA_CHANGED) == TriggerOptions.DATA_CHANGED) res[0] |= Scsm_MMS.TrgOpsDataChange;
            if ((inp & TriggerOptions.QUALITY_CHANGED) == TriggerOptions.QUALITY_CHANGED) res[0] |= Scsm_MMS.TrgOpsQualChange;
            if ((inp & TriggerOptions.DATA_UPDATE) == TriggerOptions.DATA_UPDATE) res[0] |= Scsm_MMS.TrgOpsDataActual;
            if ((inp & TriggerOptions.INTEGRITY) == TriggerOptions.INTEGRITY) res[0] |= Scsm_MMS.TrgOpsIntegrity;
            if ((inp & TriggerOptions.GI) == TriggerOptions.GI) res[0] |= Scsm_MMS.TrgOpsGI;
            return res;
        }
    }

}
