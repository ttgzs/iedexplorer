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

namespace IEDExplorer
{
        public enum Iec61850lStateEnum
        {
            IEC61850_STATE_START,
            IEC61850_STATE_SHUTDOWN,
            IEC61850_CONNECT_MMS,
            IEC61850_CONNECT_MMS_WAIT,
            IEC61850_READ_NAMELIST_DOMAIN,
            IEC61850_READ_NAMELIST_DOMAIN_WAIT,
            IEC61850_READ_NAMELIST_VAR,
            IEC61850_READ_NAMELIST_VAR_WAIT,
            IEC61850_READ_ACCESSAT_VAR,
            IEC61850_READ_ACCESSAT_VAR_WAIT,
            IEC61850_READ_CTL_MODEL,
            IEC61850_READ_CTL_MODEL_WAIT,
            IEC61850_READ_MODEL_DATA,
            IEC61850_READ_MODEL_DATA_WAIT,
            IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST,
            IEC61850_READ_NAMELIST_NAMED_VARIABLE_LIST_WAIT,
            IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST,
            IEC61850_READ_ACCESSAT_NAMED_VARIABLE_LIST_WAIT,
            IEC61850_MAKEGUI,
            IEC61850_FREILAUF,
        }
}
