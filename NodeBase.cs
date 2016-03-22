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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public class NodeBase : IComparable<NodeBase>
    {
        private List<String> _fc = new List<string>();
        protected List<NodeBase> _childNodes;
        private int _actualChildNode;
        private string _address;
        private bool _addressLock = false;
        public event EventHandler StateChanged;
        private NodeState _nodeState;
        // Persistence for SCL server library objects
        public object SCLServerModelObject { get; set; }

        public NodeBase(string Name)
        {
            this.Name = Name;
            _childNodes = new List<NodeBase>();
            _nodeState = NodeState.Initial;
            IsIecModel = false;
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

        public string Name { get; private set; }

        public string TypeId { get; set; }

        public bool IsIecModel { get; set; }

        public object Tag { get; set; }
        public object TagR { get; set; }    // reserve for secondary Iec TreeView

        public NodeBase Parent { get; set; }

        /*public List<String> FC
        {
            get
            {
                return _fc;
            }
        }*/

        public NodeBase[] GetChildNodes()
        {
            return (NodeBase[])_childNodes.ToArray();
        }

        public NodeBase GetChildNode(int idx)
        {
            try
            {
                return _childNodes[idx];
            }
            catch
            {
                return null;
            }
        }

        public List<string> GetChildNodeNames()
        {
            List<string> names = new List<string>();
            foreach (NodeBase nb in _childNodes)
            {
                names.Add(nb.Name);
            }
            return names;
        }

        public bool isArray()
        {
            if (isLeaf()) return false;
            foreach (NodeBase nb in _childNodes)
            {
                if (!nb.isArrayElement())
                    return false;
            }
            return true;
        }

        public bool isArrayElement()
        {
            if (!Name.StartsWith("["))
                return false;
            if (!Name.EndsWith("]"))
                return false;
            return true;
        }

        public bool isLeaf()
        {
            return _childNodes.Count == 0;
        }

        public int getArraySize()
        {
            if (isArray()) return _childNodes.Count;
            if (isArrayElement()) return Parent._childNodes.Count;
            return 0;
        }

        public int GetChildCount()
        {
            return _childNodes.Count;
        }

        public NodeBase AddChildNode(NodeBase Node)
        {
            if (Node == null) return null;       // defensive
            foreach (NodeBase n in _childNodes)
            {
                if (Node.Name == n.Name)
                    return n;
            }
            _childNodes.Add(Node);
            Node.Parent = this;
            return Node;
        }

        public NodeBase ForceAddChildNode(NodeBase Node)
        {
            _childNodes.Add(Node);
            Node.Parent = this;
            return Node;
        }

        public NodeBase LinkChildNodeByAddress(NodeBase Node)
        {
            foreach (var n in _childNodes.Where(n => Node.CommAddress.Variable == n.CommAddress.Variable))
            {
                return n;
            }
            _childNodes.Add(Node);
            return Node;
        }

        public NodeBase LinkChildNodeByName(NodeBase Node)
        {
            foreach (var n in _childNodes.Where(n => Node.Name == n.Name))
            {
                return n;
            }
            _childNodes.Add(Node);
            return Node;
        }

        /// <summary>
        /// Links the node without checking to see if a node with the same name exists
        /// </summary>
        /// <param name="Node"></param>
        /// <returns> the linked node </returns>
        public NodeBase ForceLinkChildNode(NodeBase Node)
        {
            _childNodes.Add(Node);
            return Node;
        }

        public void RemoveChildNode(NodeBase Node)
        {
            _childNodes.Remove(Node);
        }

        public void Remove()
        {
            _childNodes.Clear();
            if (Parent != null) Parent.RemoveChildNode(this);
            //Tag = null;
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
            return _childNodes.FirstOrDefault(n => n.Name == Name);
        }

        public NodeBase FindSubNode(string subName)
        {
            string[] parts = subName.Split(new char[] { '/', '.', '$' });
            NodeBase n = this;
            int i = 0;
            do
            {
                n = n.FindChildNode(parts[i]);
                i++;
                if (i == parts.Length) return n;
            } while (i < parts.Length && n != null);
            return null;
        }

        public void ResetActualChildNode()
        {
            _actualChildNode = 0;
        }

        public void ResetAllChildNodes()
        {
            _actualChildNode = 0;
            foreach (var n in _childNodes)
            {
                n.ResetAllChildNodes();
            }
        }

        public string Address
        {
            get
            {
                if (_addressLock)
                    return _address;

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
            set
            {
                _address = value;
                _addressLock = true;
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
                commAddress.VariablePath = "";
                for (int i = parts.Count - 2; i >= 0; i--)
                {
                    if (i == parts.Count - 2)
                    {
                        commAddress.Domain = parts[i];
                    }
                    else
                    {
                        commAddress.Variable += parts[i];
                        if (i == parts.Count - 3)
                            commAddress.LogicalNode = parts[i];
                        if (i != 0)
                            commAddress.Variable += "$";
                    }
                    if (i < parts.Count - 3)
                    {
                        commAddress.VariablePath = String.Concat(commAddress.VariablePath, "$", parts[i]);
                    }
                }
                return commAddress;
            }
        }

        public CommAddress CommAddressDots
        {
            get
            {
                CommAddress commAddress = CommAddress;
                commAddress.Variable = commAddress.Variable.Replace('$', '.');
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

        public void SortImmediateChildren()
        {
            _childNodes = _childNodes.OrderBy(n => n.Name).ToList();
        }

        public int CompareTo(NodeBase other)
        {
            return string.Compare(Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public virtual void SaveModel(List<String> lines, bool fromSCL)
        {
            return;
        }

        public void GetAllLeaves(List<NodeBase> leaves)
        {
            foreach (NodeBase b in _childNodes)
            {
                if (b._childNodes.Count == 0)
                    leaves.Add(b);  // Leaf
                else
                    b.GetAllLeaves(leaves);
            }
        }
    }
}
