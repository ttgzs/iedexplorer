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
                self = ReportControlBlock_create(name, parent.GetPtr(), rptId, isBuffered, dataSetName, confRef, (byte)trgOps, (byte)options, bufTm, intgPd);
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
