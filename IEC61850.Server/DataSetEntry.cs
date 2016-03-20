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
        public class DataSetEntry : ModelNode
        {
            [DllImport("iec61850", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr DataSetEntry_create(IntPtr dataSet, string variable, int index, string component);

            /**
             * \brief create a new data set entry (FCDA)
             *
             * Create a new FCDA reference and add it to the given data set as a new data set member.
             *
             * Note: Be aware that data set entries are not IEC 61850 object reference but MMS variable names
             * that have to contain the LN name, the FC and subsequent path elements separated by "$" instead of ".".
             * This is due to efficiency reasons to avoid the creation of additional strings.
             *
             * \param dataSet the data set to which the new entry will be added
             * \param variable the name of the variable as MMS variable name  including FC ("$" used as separator!)
             * \param index the index if the FCDA is an array element, otherwise -1
             * \param component the name of the component of the variable if the FCDA is a sub element of an array
             *        element. If this is not the case then NULL should be given here.
             *
             * \return the new data set entry instance
             */
            public DataSetEntry(DataSet dataSet, string variable, int index, string component)
            {
                self = DataSetEntry_create(dataSet.GetPtr(), variable, index, component);
            }

            internal DataSetEntry(IntPtr newDSE)
            {
                self = newDSE;
            }

            ~DataSetEntry()
            {
            }
        }

    }
}