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
        public class ReportControlBlock : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr ReportControlBlock_create(string name, IntPtr parent, string rptId, bool isBuffered, string dataSetName, uint confRef, byte trgOps, byte options, uint bufTm, uint intgPd);

            /**
             * \brief create a new report control block (RCB)
             *
             * Create a new report control block (RCB) and add it to the given logical node (LN).
             *
             * \param name name of the RCB relative to the parent LN
             * \param parent the parent LN.
             * \param rptId of the report. If NULL the default report ID (object reference) is used.
             * \param isBuffered true for a buffered RCB - false for unbuffered RCB
             * \param dataSetName name (object reference) of the default data set or NULL if no data
             *        is set by default
             * \param confRef the configuration revision
             * \param trgOps the trigger options supported by this RCB (bit set)
             * \param options the inclusion options. Specifies what elements are included in a report (bit set)
             * \param bufTm the buffering time of the RCB in milliseconds (time between the first event and the preparation of the report).
             * \param intgPd integrity period in milliseconds
             *
             * \return the new RCB instance.
             */
            public ReportControlBlock(string name, LogicalNode parent, string rptId, bool isBuffered, string dataSetName, uint confRef, TriggerOptions trgOps, ReportOptions options, uint bufTm, uint intgPd)
            {
                self = ReportControlBlock_create(name, parent.GetLibraryObject(), rptId, isBuffered, dataSetName, confRef, (byte)trgOps, (byte)options, bufTm, intgPd);
            }

            internal ReportControlBlock(IntPtr newDO)
            {
                self = newDO;
            }

            ~ReportControlBlock()
            {
            }
        }


    }
}
