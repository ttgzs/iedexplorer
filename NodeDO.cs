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
    //class NodeDO : NodeBase
    class NodeDO : NodeData
    {
        private string _type = "";

        public int SCL_ArraySize { get; set; }

        public string SCL_Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string SCL_UpperDOName { get; set; }

        public NodeDO(string Name)
            : base(Name)
        {
        }

        public override void SaveModel(List<String> lines, bool fromSCL)
        {
            // Syntax: DO(<data object name> <nb of array elements>){…}
            int nrElem = 0;
            NodeBase nextnb = this;

            if (isArray())
            {
                nrElem = getArraySize();
                // Array has got an artificial level with array members, this is not part of model definition
                if (_childNodes.Count > 0)
                    nextnb = _childNodes[0];
            }

            lines.Add("DO(" + Name + " " + nrElem.ToString() + "){");
            foreach (NodeBase b in nextnb.GetChildNodes())
            {
                b.SaveModel(lines, fromSCL);
            }
            lines.Add("}");
        }
    }
}
