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
                self = DataSetEntry_create(dataSet.GetLibraryObject(), variable, index, component);
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