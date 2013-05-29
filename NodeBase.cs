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

namespace IEDExplorer
{
    public enum NodeState
    {
        Initial,
        Read,
        Reported,
        GI,
        Periodic,
        Written
    }

    public class NodeBase 
    {
        private string _name;
        private List<String> _fc = new List<string>();
        private object _tag;
        protected List<NodeBase> _childNodes;
        private int _actualChildNode;
        private NodeBase _parent;
        public event EventHandler StateChanged;
        private NodeState _nodeState;

        public NodeBase(string Name)
        {
            _name = Name;
            _childNodes = new List<NodeBase>();
            _nodeState = NodeState.Initial;
        }

        public NodeState NodeState
        {
            get
            {
                lock (this)
                    return _nodeState;
            }
            set
            {
                bool fire = false;

                lock (this)
                    if (!_nodeState.Equals(value))
                    {
                        _nodeState = value;
                        fire = true;
                    }
                if (fire && StateChanged != null)
                {
                    StateChanged(this, new EventArgs());
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        public NodeBase Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public List<String> FC
        {
            get
            {
                return _fc;
            }
        }

        public NodeBase[] GetChildNodes()
        {
            return (NodeBase[])_childNodes.ToArray();
        }

        public NodeBase AddChildNode(NodeBase Node)
        {
            foreach (NodeBase n in _childNodes)
            {
                if (Node._name == n._name)
                    return n;
            }
            _childNodes.Add(Node);
            Node.Parent = this;
            return Node;
        }

        public NodeBase LinkChildNode(NodeBase Node)
        {
            foreach (NodeBase n in _childNodes)
            {
                if (Node._name == n._name)
                    return n;
            }
            _childNodes.Add(Node);
            return Node;
        }

        public void RemoveChildNode(NodeBase Node)
        {
            _childNodes.Remove(Node);
        }

        public NodeBase GetActualChildNode()
        {
            if (_childNodes.Count <= _actualChildNode) return null;
            return (NodeBase)_childNodes[_actualChildNode];
        }

        public NodeBase NextActualChildNode()
        {
            _actualChildNode++;
            if (_actualChildNode >= _childNodes.Count)
            {
                _actualChildNode = 0;
                return null;
            }
            return (NodeBase)_childNodes[_actualChildNode];
        }

        public NodeBase FindChildNode(string Name)
        {
            foreach (NodeBase n in _childNodes)
            {
                if (n.Name == Name)
                    return n;
            }
            return null;
        }

        public void ResetActualChildNode()
        {
            _actualChildNode = 0;
        }

        public void ResetAllChildNodes()
        {
            _actualChildNode = 0;
            foreach (NodeBase n in _childNodes)
            {
                n.ResetAllChildNodes();
            }
        }

        public string Address
        {
            get
            {
                string address = "";
                NodeBase tmpn = this;
                List<string> parts = new List<string>();

                do
                {
                    parts.Add(tmpn.Name);
                    tmpn = tmpn.Parent;
                } while (tmpn != null);

                for (int i = parts.Count - 2; i >= 0; i--)
                {
                    if (i == parts.Count - 4)
                    {
                        continue;
                    }
                    address += parts[i];
                    if (i == parts.Count - 2)
                    {
                        if (i != 0)
                            address += "/";
                    }
                    else if (i != 0)
                        address += ".";
                }
                return address;
            }
        }

        public CommAddress CommAddress
        {
            get
            {
                CommAddress commAddress = new CommAddress(); ;
                NodeBase tmpn = this;
                commAddress.owner = this;

                List<string> parts = new List<string>();

                do
                {
                    parts.Add(tmpn.Name);
                    tmpn = tmpn.Parent;
                } while (tmpn != null);

                commAddress.Variable = "";
                for (int i = parts.Count - 2; i >= 0; i--)
                {
                    if (i == parts.Count - 2)
                    {
                        commAddress.Domain = parts[i];
                    }
                    else
                    {
                        commAddress.Variable += parts[i];
                        if (i != 0)
                            commAddress.Variable += "$";
                    }
                }
                return commAddress;
            }
        }

        internal Iec61850State GetIecs()
        {
            NodeBase b = this;
            do
            {
                if (b is NodeIed)
                    return (b as NodeIed).iecs;
                b = b.Parent;
            } while (b != null);
            return null;
        }

        internal virtual NodeBase FindNodeByValue(scsm_MMS_TypeEnum dataType, object dataValue, ref NodeBase ContinueAfter)
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
                }
            }
            return null;
        }
    }
}
