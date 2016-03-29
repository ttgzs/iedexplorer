/*
 *  Copyright (C) 2014 Pavel Charvat
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using MMS_ASN1_Model;

namespace IEDExplorer
{
    class SCLParserDOM
    {
        private List<Iec61850Model> _dataModels = new List<Iec61850Model>();
        private List<NodeBase> _nodeTypes;
        private List<NodeBase> _dataObjectTypes;
        private List<NodeBase> _dataAttributeTypes;

        private Logger logger = Logger.getLogger();

        /// <summary>
        /// Reads through the specified SCL file and creates a logical tree
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="env"></param>
        /// <returns>IECS tree state to be displayed</returns>
        public List<Iec61850Model> CreateTree(String fileName) //, Env env)
        {
            //try
            {
                // Model [0] is a master for types etc.
                _dataModels.Add(new Iec61850State().DataModel);
                _nodeTypes = new List<NodeBase>();
                _dataObjectTypes = new List<NodeBase>();
                _dataAttributeTypes = new List<NodeBase>();
                int i = 0;

                logger.LogInfo("Reading SCL file " + fileName);
                XDocument doc = XDocument.Load(fileName);
                XNamespace ns = doc.Root.Name.Namespace;
                GetTypes(doc, ns);
                _dataModels[0].enums.SortImmediateChildren();

                //logger.LogInfo("Reading XML tree.");
                foreach (XElement ied in doc.Root.Elements(ns + "IED"))
                {
                    // Create model. model 0 (master model) is already created above
                    if (i > 0) _dataModels.Add(new Iec61850State().DataModel);
                    XAttribute a = ied.Attribute("name");
                    if (a != null) _dataModels[i].ied = new NodeIed(a.Value, _dataModels[i]);
                    else continue;  // cannot proceed
                    if (a != null) _dataModels[i].iec = new NodeIed(a.Value, _dataModels[i]);
                    a = ied.Attribute("manufacturer");
                    if (a != null) _dataModels[i].ied.VendorName = a.Value;
                    if (a != null) _dataModels[i].iec.VendorName = a.Value;
                    a = ied.Attribute("type");
                    if (a != null) _dataModels[i].ied.ModelName = a.Value;
                    if (a != null) _dataModels[i].iec.ModelName = a.Value;
                    a = ied.Attribute("revision");
                    if (a != null) _dataModels[i].ied.Revision = a.Value;
                    if (a != null) _dataModels[i].iec.Revision = a.Value;

                    // IED MMS Tree
                    CreateLogicalDevices(_dataModels[i].ied, ied.Descendants(ns + "LDevice"), ns);
                    // data sets and reports
                    int ldidx = 0;
                    foreach (XElement ld in ied.Descendants(ns + "LDevice"))
                    {
                        NodeBase ldroot = _dataModels[i].ied.GetChildNode(ldidx++);
                        int lnidx = 0;
                        IEnumerable<XElement> lns = (from el in ld.Elements() where el.Name.LocalName.StartsWith("LN") select el);
                        int cnt = lns.Count();
                        foreach (XElement ln in lns)
                        {
                            // Datasets
                            NodeBase lnroot = ldroot.GetChildNode(lnidx++);
                            //if (lnroot == null || lnroot.Parent == null || lnroot.Parent.Parent == null)
                            //    Logger.getLogger().LogError("Something is null 1" + cnt);
                            CreateDataSets(lnroot, ln.Elements(ns + "DataSet"), ns);
                            CreateReports(lnroot, ln.Elements(ns + "ReportControl"), ns);
                            lnroot.SortImmediateChildren();
                        }
                        ldroot.SortImmediateChildren();
                    }

                    // IEC 61850 Tree
                    CreateLogicalDevicesIEC(_dataModels[i].iec, ied.Descendants(ns + "LDevice"), ns);
                    
                    ldidx = 0;
                    foreach (XElement ld in ied.Descendants(ns + "LDevice"))
                    {
                        NodeBase ldroot = _dataModels[i].iec.GetChildNode(ldidx++);
                        int lnidx = 0;
                        IEnumerable<XElement> lns = (from el in ld.Elements() where el.Name.LocalName.StartsWith("LN") select el);
                        int cnt = lns.Count();
                        foreach (XElement ln in lns)
                        {
                            NodeBase lnroot = ldroot.GetChildNode(lnidx++);
                            ReadDataInstanceValues(lnroot, ln.Elements(ns + "DOI"), ns);
                        }
                    }
                    i++;
                }
            }
            //catch (Exception e)
            //{
            //    logger.LogError("Error reading file " + fileName + ": " + e.Message);
            //    throw e;
            //}

            return _dataModels;
        }

        private void ReadDataInstanceValues(NodeBase lnroot, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement inst in elements)
            {
                XAttribute a = inst.Attribute("name");
                if (a == null)
                {
                    logger.LogDebug("SCL DAI/DOI attribute 'name' not found for " + inst.ToString() + ", node " + lnroot.Address);
                    return;
                }
                NodeBase child = lnroot.FindChildNode(a.Value);
                if (child == null)
                {
                    logger.LogDebug("SCL DAI/DOI child " + a.Value + " not found for " + inst.ToString() + ", node " + lnroot.Address);
                    return;
                }
                XElement val = inst.Element(ns + "Val");
                if (val != null && child is NodeData)
                {
                    // Read value in
                    NodeData data = child as NodeData;
                    logger.LogDebug("SCL Value found for " + child.Address + ": val = " + val.Value);
                    if (data.SCL_BType.StartsWith("Enum"))
                    {
                        NodeBase myEnum = _dataModels[0].enums.FindChildNode(data.SCL_Type);
                        if (myEnum != null)
                        {
                            NodeData myVal = (NodeData)myEnum.FindChildNode(val.Value);
                            if (myVal != null)
                            {
                                try
                                {
                                    data.DataValue = int.Parse(myVal.DataValue.ToString());
                                }
                                catch { }
                            }
                        }
                    }
                    else if (data.SCL_BType.Contains("String"))
                    {
                        data.DataValue = val.Value;
                    }
                }
                else
                {
                    // Try children DOI
                    ReadDataInstanceValues(child, inst.Elements(ns + "DOI"), ns);
                    // Try children DAI
                    ReadDataInstanceValues(child, inst.Elements(ns + "DAI"), ns);
                }
            }
        }

        /// <summary>
        /// Creates a logical device (LD) node and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a LD node </returns>
        private void CreateLogicalDevices(NodeBase root, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement ld in elements)
            {
                NodeLD logicalDevice = new NodeLD(String.Concat(root.Name, ld.Attribute("inst").Value));
                root.AddChildNode(logicalDevice);
                CreateLogicalNodes(logicalDevice, from el in ld.Elements() where el.Name.LocalName.StartsWith("LN") select el, ns);
            }
        }

        /// <summary>
        /// Creates a logical device (LD) node for IEC type tree and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a LD node </returns>
        private void CreateLogicalDevicesIEC(NodeBase root, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement ld in elements)
            {
                //NodeLD logicalDevice = new NodeLD(String.Concat(root.Name, ld.Attribute("inst").Value));
                NodeLD logicalDevice = new NodeLD(ld.Attribute("inst").Value);
                root.AddChildNode(logicalDevice);
                CreateLogicalNodesIEC(logicalDevice, from el in ld.Elements() where el.Name.LocalName.StartsWith("LN") select el, ns);
            }
        }

        /// <summary>
        /// Creates a logical node (LN) and all of its children, including
        /// FCs, DO's, and DA's
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a LN node </returns>
        private void CreateLogicalNodes(NodeBase root, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement ln in elements)
            {
                XAttribute a = ln.Attribute("prefix");
                string prefix = a != null ? a.Value : "";
                a = ln.Attribute("lnClass");
                string lnClass = a != null ? a.Value : "";
                a = ln.Attribute("inst");
                string inst = a != null ? a.Value : "";
                a = ln.Attribute("lnType");
                string type = a != null ? a.Value : "";

                // LN name is a combination of prefix, lnCLass, and inst
                var name = !String.IsNullOrWhiteSpace(prefix) ? String.Concat(prefix, lnClass, inst) : String.Concat(lnClass, inst);

                NodeLN logicalNode = new NodeLN(name);
                logicalNode.TypeId = type;

                Hashtable functionalConstraints = new Hashtable();
                NodeBase nodeType;
                try
                {
                    nodeType = _nodeTypes.Single(nt => nt.Name.Equals(type));
                }
                catch (Exception e)
                {
                    logger.LogError("SCL Parser: LN type template not found: " + type.ToString() + ", for Node: " + name.ToString() + ", Exception: " + e.Message);
                    continue;
                }

                // for each DO in the LNodeType
                foreach (var dataObject in nodeType.GetChildNodes())
                {
                    NodeBase doType = null;
                    try
                    {
                        doType = _dataObjectTypes.Single(dot => dot.Name.Equals((dataObject as NodeDO).SCL_Type));
                    }
                    catch (Exception e)
                    {
                        logger.LogError("SCL Parser: DO type template not found: " + (dataObject as NodeDO).SCL_Type + ", for LN type: " + nodeType.Name + ", in node: " + name.ToString() + ", Exception: " + e.Message);
                        continue;
                    }

                    // for each DA in the DOType
                    foreach (var dataAttribute in doType.GetChildNodes())
                    {
                        if (dataAttribute is NodeDO)
                        {
                            // SDO (sub Data Object)
                            NodeBase subDoType = null;
                            try
                            {
                                subDoType = _dataObjectTypes.Single(dot => dot.Name.Equals((dataAttribute as NodeDO).SCL_Type));
                            }
                            catch (Exception e)
                            {
                                logger.LogError("SCL Parser: SDO type template not found: " + (dataAttribute as NodeDO).SCL_Type + ", for DO type: " + doType.Name + ", for LN type: " + nodeType.Name + ", in node: " + name.ToString() + ", Exception: " + e.Message);
                                continue;
                            }
                            NodeDO subDataObject = new NodeDO(dataAttribute.Name);
                            subDataObject.SCL_ArraySize = (dataAttribute as NodeDO).SCL_ArraySize;
                            subDataObject.SCL_UpperDOName = dataObject.Name;
                            if (subDataObject.SCL_ArraySize > 0)
                            {
                                for (int i = 0; i < subDataObject.SCL_ArraySize; i++)
                                {
                                    NodeDO arrDataObject = new NodeDO("[" + i.ToString() + "]");
                                    subDataObject.AddChildNode(arrDataObject);
                                    arrDataObject.SCL_UpperDOName = dataObject.Name;
                                    foreach (var dataAttribute2 in subDoType.GetChildNodes())
                                    {
                                        CreateDataAttributes(functionalConstraints, dataObject, dataAttribute2, arrDataObject);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var dataAttribute2 in subDoType.GetChildNodes())
                                {
                                    CreateDataAttributes(functionalConstraints, dataObject, dataAttribute2, subDataObject);
                                }
                            }

                        }
                        if (dataAttribute is NodeData)
                        {
                            CreateDataAttributes(functionalConstraints, dataObject, dataAttribute, null);
                        }
                    }
                }

                // for each hashtable element
                foreach (var key in functionalConstraints.Keys)
                {
                    var doList = new List<NodeDO>();

                    // for each data attribute of the functional constraint
                    foreach (var da in (functionalConstraints[key] as NodeBase).GetChildNodes())
                    {
                        string doName;
                        if (da is NodeData)
                            doName = (da as NodeData).SCL_DOName;
                        else
                            doName = (da as NodeDO).SCL_UpperDOName;
                        if (doList.Exists(x => x.Name.Equals(doName)))
                        {
                            doList.Single(x => x.Name.Equals(doName)).AddChildNode(da);
                        }
                        else
                        {
                            var temp = new NodeDO(doName);
                            temp.AddChildNode(da);
                            doList.Add(temp);
                        }
                    }

                    var nodeFC = new NodeFC(key as string);
                    foreach (NodeDO x in doList)
                    {
                        nodeFC.AddChildNode(x);
                    }
                    nodeFC.SortImmediateChildren(); // alphabetical
                    logicalNode.AddChildNode(nodeFC);
                }

                logicalNode.SortImmediateChildren(); // alphabetical

                root.AddChildNode(logicalNode);

            }
        }

        private void CreateDataAttributes(Hashtable functionalConstraints, NodeBase dataObject, NodeBase dataAttribute, NodeDO subNode)
        {
            var fc = (dataAttribute as NodeData).SCL_FCDesc;
            (dataAttribute as NodeData).SCL_DOName = dataObject.Name;
            NodeData newNode = new NodeData(dataAttribute.Name);
            newNode.SCL_Type = (dataAttribute as NodeData).SCL_Type;
            newNode.SCL_BType = (dataAttribute as NodeData).SCL_BType;
            newNode.SCL_DOName = (dataAttribute as NodeData).SCL_DOName;
            newNode.SCL_FCDesc = (dataAttribute as NodeData).SCL_FCDesc;
            newNode.SCL_ArraySize = (dataAttribute as NodeData).SCL_ArraySize;

            // when the type is specified (ie. when it's a struct), get the struct child nodes
            if (!String.IsNullOrWhiteSpace(newNode.SCL_Type) && !newNode.SCL_BType.StartsWith("Enum"))
            {
                var dataType =
                    _dataAttributeTypes.Single(dat => dat.Name.Equals((newNode.SCL_Type)));
                foreach (NodeBase child in dataType.GetChildNodes())
                {
                    var tempChild = new NodeData(child.Name);
                    tempChild.SCL_BType = (child as NodeData).SCL_BType;
                    if (!String.IsNullOrWhiteSpace((child as NodeData).SCL_Type) && !(child as NodeData).SCL_BType.StartsWith("Enum"))
                    {
                        var subDataType = _dataAttributeTypes.Single(dat => dat.Name.Equals((child as NodeData).SCL_Type));
                        foreach (NodeBase subChild in subDataType.GetChildNodes())
                        {
                            var tempSubChild = new NodeData(subChild.Name);
                            tempSubChild.SCL_BType = (subChild as NodeData).SCL_BType;
                            tempChild.AddChildNode(tempSubChild);
                        }
                    }
                    newNode.AddChildNode(tempChild);
                }
            }
            if (!functionalConstraints.ContainsKey(fc))
            {
                NodeFC nodeFC = new NodeFC(fc);
                if (subNode != null)
                {
                    subNode.AddChildNode(newNode);
                    nodeFC.ForceAddChildNode(subNode);
                }
                else
                    nodeFC.ForceAddChildNode(newNode);
                functionalConstraints.Add(fc, nodeFC);
            }
            else
            {
                if (subNode != null)
                {
                    subNode.AddChildNode(newNode);
                    (functionalConstraints[fc] as NodeBase).ForceAddChildNode(subNode);
                }
                else
                    (functionalConstraints[fc] as NodeBase).ForceAddChildNode(newNode);
            }
        }

        /// <summary>
        /// Creates a logical node (LN) and all of its children, including
        /// FCs, DO's, and DA's
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a LN node </returns>
        private void CreateLogicalNodesIEC(NodeBase ld, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement ln in elements)
            {
                XAttribute a = ln.Attribute("prefix");
                string prefix = a != null ? a.Value : "";
                a = ln.Attribute("lnClass");
                string lnClass = a != null ? a.Value : "";
                a = ln.Attribute("inst");
                string inst = a != null ? a.Value : "";
                a = ln.Attribute("lnType");
                string type = a != null ? a.Value : "";

                // LN name is a combination of prefix, lnCLass, and inst
                var name = !String.IsNullOrWhiteSpace(prefix) ? String.Concat(prefix, lnClass, inst) : String.Concat(lnClass, inst);

                NodeLN logicalNode = new NodeLN(name);
                logicalNode.TypeId = type;

                //Hashtable functionalConstraints = new Hashtable();
                NodeBase nodeType;
                try
                {
                    nodeType = _nodeTypes.Single(nt => nt.Name.Equals(type));
                }
                catch (Exception e)
                {
                    logger.LogError("SCL Parser: LN type template not found: " + type.ToString() + ", for Node: " + name.ToString() + ", Exception: " + e.Message);
                    continue;
                }

                // for each DO in the LNodeType
                foreach (var dataObject in nodeType.GetChildNodes())
                {
                    NodeBase doType = null;
                    try
                    {
                        doType = _dataObjectTypes.Single(dot => dot.Name.Equals((dataObject as NodeDO).SCL_Type));
                    }
                    catch (Exception e)
                    {
                        logger.LogError("SCL Parser: DO type template not found: " + (dataObject as NodeDO).SCL_Type + ", for LN type: " + nodeType.Name + ", in node: " + name.ToString() + ", Exception: " + e.Message);
                        continue;
                    }

                    // for each DA in the DOType
                    foreach (var dataAttribute in doType.GetChildNodes())
                    {
                        if (dataAttribute is NodeDO)
                        {
                            // SDO (sub Data Object)
                            NodeBase subDoType = null;
                            try
                            {
                                subDoType = _dataObjectTypes.Single(dot => dot.Name.Equals((dataAttribute as NodeDO).SCL_Type));
                            }
                            catch (Exception e)
                            {
                                logger.LogError("SCL Parser: SDO type template not found: " + (dataAttribute as NodeDO).SCL_Type + ", for DO type: " + doType.Name + ", for LN type: " + nodeType.Name + ", in node: " + name.ToString() + ", Exception: " + e.Message);
                                continue;
                            }
                            NodeDO subDataObject = new NodeDO(dataAttribute.Name);
                            subDataObject.SCL_ArraySize = (dataAttribute as NodeDO).SCL_ArraySize;

                            dataObject.AddChildNode(subDataObject);
                            if (subDataObject.SCL_ArraySize > 0)
                            {
                                for (int i = 0; i < subDataObject.SCL_ArraySize; i++)
                                {
                                    NodeDO arrDataObject = new NodeDO("[" + i.ToString() + "]");
                                    subDataObject.AddChildNode(arrDataObject);
                                    arrDataObject.SCL_UpperDOName = dataObject.Name;

                                    foreach (var dataAttribute2 in subDoType.GetChildNodes())
                                    {
                                        CreateDataAttributesIEC(arrDataObject, dataAttribute2);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var dataAttribute2 in subDoType.GetChildNodes())
                                {
                                    CreateDataAttributesIEC(subDataObject, dataAttribute2);
                                }
                            }
                        }
                        if (dataAttribute is NodeData)
                        {
                            CreateDataAttributesIEC(dataObject, dataAttribute);
                        }
                    }
                    logicalNode.AddChildNode(dataObject);
                }
                ld.AddChildNode(logicalNode);

                foreach (XElement el in ln.Elements(ns + "DataSet"))
                {
                    NodeVL nodeVL = new NodeVL(el.Attribute("name").Value);
                    logicalNode.AddChildNode(nodeVL);
                    foreach (XElement dsMember in el.Elements(ns + "FCDA"))
                    {
                        try
                        {
                            a = dsMember.Attribute("prefix");
                            string prefix2 = a != null ? a.Value : "";
                            a = dsMember.Attribute("lnClass");
                            string lnClass2 = a != null ? a.Value : "";
                            a = dsMember.Attribute("lnInst");
                            string lnInst = a != null ? a.Value : "";
                            string fullName = String.Concat(prefix2, lnClass2, lnInst);
                            a = dsMember.Attribute("ldInst");
                            string ldInst = a != null ? a.Value : "";
                            a = dsMember.Attribute("doName");
                            string doName = a != null ? a.Value : "";
                            a = dsMember.Attribute("fc");
                            string fc = a != null ? a.Value : "";

                            // We are at the LN level, up 2 levels is an ied
                            NodeIed iec = ld.Parent as NodeIed;

                            //string memberName = ld.Name + "/" + fullName + "." + doName;
                            string memberName = ld.Name + "/" + fullName + "." + fc + "." + doName;
                            memberName = memberName.Replace('.', '$');

                            // Cannot be done, the data are not yet present (instantiated)
                            //var nodeData = iec.FindSubNode(memberName);
                            //if (nodeData != null)
                            //    nodeVL.ForceLinkChildNode(nodeData);
                            //else
                            //    logger.LogWarning("SCL Iec model: DataSet member not found: " + memberName);

                            // instead, add a temporary object
                            nodeVL.AddChildNode(new NodeVLM(memberName));
                        }
                        catch (Exception e)
                        {
                            Logger.getLogger().LogError("CreateDataSets IEC: " + e.Message);
                        }
                    }
                }

                CreateReports(logicalNode, ln.Elements(ns + "ReportControl"), ns, true);

                logicalNode.SortImmediateChildren(); // alphabetical

            }
        }

        private void CreateDataAttributesIEC(NodeBase dataObject, NodeBase dataAttribute)
        {
            (dataAttribute as NodeData).SCL_DOName = dataObject.Name;
            NodeData dataAttributeInst = new NodeData(dataAttribute.Name);
            dataAttributeInst.SCL_Type = (dataAttribute as NodeData).SCL_Type;
            dataAttributeInst.SCL_BType = (dataAttribute as NodeData).SCL_BType;
            dataAttributeInst.SCL_DOName = (dataAttribute as NodeData).SCL_DOName;
            dataAttributeInst.SCL_FCDesc = (dataAttribute as NodeData).SCL_FCDesc;
            dataAttributeInst.SCL_TrgOps = (dataAttribute as NodeData).SCL_TrgOps;
            dataAttributeInst.SCL_ArraySize = (dataAttribute as NodeData).SCL_ArraySize;

            // when the type is specified (ie. when it's a struct), get the struct child nodes
            if (dataAttributeInst.SCL_ArraySize > 0)
            {
                for (int i = 0; i < dataAttributeInst.SCL_ArraySize; i++)
                {
                    NodeData arrNode = new NodeData("[" + i.ToString() + "]");
                    dataAttributeInst.AddChildNode(arrNode);
                    CreateDataAttributesChildIEC(arrNode);
                }
            }
            else
                CreateDataAttributesChildIEC(dataAttributeInst);
            dataObject.AddChildNode(dataAttributeInst);
        }

        private void CreateDataAttributesChildIEC(NodeData dataAttributeInst)
        {
            if (!String.IsNullOrWhiteSpace(dataAttributeInst.SCL_Type)  && !dataAttributeInst.SCL_BType.StartsWith("Enum"))
            {
                var dataType =
                    _dataAttributeTypes.Single(dat => dat.Name.Equals((dataAttributeInst.SCL_Type)));
                foreach (NodeBase child in dataType.GetChildNodes())
                {
                    var dataAttributeInstChild = new NodeData(child.Name);
                    dataAttributeInstChild.SCL_BType = (child as NodeData).SCL_BType;
                    dataAttributeInstChild.SCL_FCDesc = (child as NodeData).SCL_FCDesc;
                    if (dataAttributeInstChild.SCL_FCDesc == null) dataAttributeInstChild.SCL_FCDesc = dataAttributeInst.SCL_FCDesc;
                    dataAttributeInstChild.SCL_TrgOps = (child as NodeData).SCL_TrgOps;
                    if (dataAttributeInstChild.SCL_TrgOps == 0) dataAttributeInstChild.SCL_TrgOps = dataAttributeInst.SCL_TrgOps;

                    if (dataAttributeInstChild.SCL_ArraySize > 0)
                    {
                        for (int i = 0; i < dataAttributeInstChild.SCL_ArraySize; i++)
                        {
                            NodeData arrNode = new NodeData("[" + i.ToString() + "]");
                            dataAttributeInstChild.AddChildNode(arrNode);
                            CreateDataAttributesSubChildIEC(child, arrNode);
                        }
                    }
                    else
                        CreateDataAttributesSubChildIEC(child, dataAttributeInstChild);

                    dataAttributeInst.AddChildNode(dataAttributeInstChild);
                }
            }
        }

        private void CreateDataAttributesSubChildIEC(NodeBase child, NodeData dataAttributeInstChild)
        {
            if (!String.IsNullOrWhiteSpace((child as NodeData).SCL_Type) && !(child as NodeData).SCL_BType.StartsWith("Enum"))
            {
                var subDataType = _dataAttributeTypes.Single(dat => dat.Name.Equals((child as NodeData).SCL_Type));
                foreach (NodeBase subChild in subDataType.GetChildNodes())
                {
                    var dataAttributeInstSubChild = new NodeData(subChild.Name);
                    dataAttributeInstSubChild.SCL_BType = (subChild as NodeData).SCL_BType;
                    dataAttributeInstSubChild.SCL_FCDesc = (subChild as NodeData).SCL_FCDesc;
                    if (dataAttributeInstSubChild.SCL_FCDesc == null) dataAttributeInstSubChild.SCL_FCDesc = dataAttributeInstChild.SCL_FCDesc;
                    dataAttributeInstSubChild.SCL_TrgOps = (subChild as NodeData).SCL_TrgOps;
                    if (dataAttributeInstSubChild.SCL_TrgOps == 0) dataAttributeInstSubChild.SCL_TrgOps = dataAttributeInstChild.SCL_TrgOps;

                    if (dataAttributeInstSubChild.SCL_ArraySize > 0)
                    {
                        for (int i = 0; i < dataAttributeInstSubChild.SCL_ArraySize; i++)
                        {
                            NodeData arrNode = new NodeData("[" + i.ToString() + "]");
                            dataAttributeInstSubChild.AddChildNode(arrNode);
                        }
                    }
                    else
                        dataAttributeInstChild.AddChildNode(dataAttributeInstSubChild);
                }
            }
        }

        private void CreateLogicalNodeTypes(List<NodeBase> list, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement el in elements)
            {
                NodeLN logicalNode = new NodeLN(el.Attribute("id").Value);
                CreateDataObjects(logicalNode, el.Elements(ns + "DO"), ns);
                list.Add(logicalNode);
            }
        }

        /// <summary>
        /// Create a DO from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a DO Node representing a DO </returns>
        private void CreateDataObjects(NodeBase root, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement el in elements)
            {
                NodeDO dataObject = new NodeDO(el.Attribute("name").Value);
                var type = el.Attribute("type");
                if (type != null)
                    dataObject.SCL_Type = type.Value;
                //CreateDataAttributes(dataObject, el.Elements(ns + "DAI"), ns);
                dataObject.SortImmediateChildren();
                root.AddChildNode(dataObject);
            }
        }
        /// <summary>
        /// Creates a DO Type node from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a DO node representing a DOType </returns>
        private void CreateDataObjectTypes(List<NodeBase> list, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement el in elements)
            {
                NodeDO dataObject = new NodeDO(el.Attribute("id").Value);
                // SDOs
                foreach (XElement sdoel in el.Elements(ns + "SDO"))
                {
                    NodeDO subDataObject = new NodeDO(sdoel.Attribute("name").Value);
                    int cnt = 0;
                    if (sdoel.Attribute("count") != null)
                        int.TryParse(sdoel.Attribute("count").Value, out cnt);
                    subDataObject.SCL_ArraySize = cnt;
                    if (sdoel.Attribute("type") != null)
                        subDataObject.SCL_Type = sdoel.Attribute("type").Value;

                    dataObject.AddChildNode(subDataObject);
                }
                // DAs
                CreateDataAttributes(dataObject, el.Elements(ns + "DA"), ns);
                list.Add(dataObject);
            }
        }

        /// <summary>
        /// Creates a DA Type node from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a node representing a DAType </returns>
        private void CreateDataAttributeTypes(List<NodeBase> list, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement el in elements)
            {
                NodeData data = new NodeData(el.Attribute("id").Value);
                CreateDataAttributes(data, el.Elements(ns + "BDA"), ns);
                list.Add(data);
            }
        }

        /// <summary>
        /// Creates a DA node from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private void CreateDataAttributes(NodeBase root, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement el in elements)
            {
                if (el.Attribute("name") != null)
                {
                    NodeData data = new NodeData(el.Attribute("name").Value);
                    if (el.Attribute("fc") != null) data.SCL_FCDesc = el.Attribute("fc").Value;
                    else if (root is NodeData) data.SCL_FCDesc = (root as NodeData).SCL_FCDesc;
                    var bType = el.Attribute("bType");
                    if (bType == null)
                    {
                        XElement en = el.Element(ns + "Val");
                        if (en != null) data.DataValue = en.Value;
                        // Inheritance
                        if (root is NodeData) data.SCL_BType = (root as NodeData).SCL_BType;
                    }
                    else
                    {
                        data.SCL_BType = bType.Value;
                        if (data.SCL_BType.Equals("Struct") && null != el.Attribute("type"))
                            data.SCL_Type = el.Attribute("type").Value;
                        else if (data.SCL_BType.Equals("Enum"))
                        {
                            data.SCL_BType = String.Concat(data.SCL_BType, " (Integer)");
                            if (null != el.Attribute("type")) data.SCL_Type = el.Attribute("type").Value;
                        }
                    }
                    IEC61850.Common.TriggerOptions trgOptions = IEC61850.Common.TriggerOptions.NONE;
                    XAttribute a = el.Attribute("dchg");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        trgOptions |= IEC61850.Common.TriggerOptions.DATA_CHANGED;
                    a = el.Attribute("qchg");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        trgOptions |= IEC61850.Common.TriggerOptions.QUALITY_CHANGED;
                    a = el.Attribute("dupd");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        trgOptions |= IEC61850.Common.TriggerOptions.DATA_UPDATE;
                    data.SCL_TrgOps = (byte)trgOptions;
                    // Inheritance
                    if ((root is NodeData) && trgOptions == IEC61850.Common.TriggerOptions.NONE) data.SCL_TrgOps = (root as NodeData).SCL_TrgOps;
                    int cnt = 0;
                    if (el.Attribute("count") != null)
                        int.TryParse(el.Attribute("count").Value, out cnt);
                    data.SCL_ArraySize = cnt;

                    root.AddChildNode(data);
                }
            }
        }

        /// <summary>
        /// Creates an Enum type from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> an Enum Type node </returns>
        private void CreateEnumTypes(NodeBase root, IEnumerable<XElement> elements, XNamespace ns)
        {
            foreach (XElement el in elements)
            {
                try
                {
                    if (el.Attribute("id") != null)
                    {
                        NodeBase enumType = new NodeBase(el.Attribute("id").Value);

                        foreach (XElement ev in el.Elements(ns + "EnumVal"))
                        {
                            if (ev.Attribute("ord") != null)
                            {
                                var id = ev.Attribute("ord").Value;
                                var name = ev.Value;
                                var enumVal = enumType.AddChildNode(new NodeData(name));
                                (enumVal as NodeData).DataValue = id;
                            }
                        }
                        root.AddChildNode(enumType);
                    }
                }
                catch (Exception e)
                {
                    Logger.getLogger().LogError("Creating Enum " + el.Name + ", Exception: " + e);
                    throw e;
                }
            }
        }

        /// <summary>
        /// Parses the XML file for all LN, DO, DA, and Enum types
        /// </summary>
        /// <param name="filename"></param>
        private void GetTypes(XDocument doc, XNamespace ns)
        {
            ns = doc.Root.Name.Namespace;

            XElement templates = doc.Root.Element(ns + "DataTypeTemplates");

            CreateLogicalNodeTypes(_nodeTypes, templates.Elements(ns + "LNodeType"), ns);
            CreateDataObjectTypes(_dataObjectTypes, templates.Elements(ns + "DOType"), ns);
            CreateDataAttributeTypes(_dataAttributeTypes, templates.Elements(ns + "DAType"), ns);
            CreateEnumTypes(_dataModels[0].enums, templates.Elements(ns + "EnumType"), ns);

        }

        /// <summary>
        /// Reads through a data set subtree and creates a data set node from it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="nodeName"></param>
        /// <param name="deviceName"></param>
        /// <returns> a VL node </returns>
        private void CreateDataSets(NodeBase lnode, IEnumerable<XElement> elements, XNamespace ns)
        {
            if (lnode == null || elements == null || lnode.Parent == null || lnode.Parent.Parent == null)
                Logger.getLogger().LogError("CreateDataSets: Something is null");

            // We are at the LN level, up 2 levels is an ied
            Iec61850Model _dataModel = (lnode.Parent.Parent as NodeIed).Model;

            foreach (XElement el in elements)
            {
                NodeVL nodeVL = new NodeVL(String.Concat(lnode.Name, "$", el.Attribute("name").Value));
                _dataModel.lists.AddChildNode(new NodeLD(lnode.Parent.Name)).AddChildNode(nodeVL);
                foreach (XElement dsMember in el.Elements(ns + "FCDA"))
                {
                    try
                    {
                        XAttribute a = dsMember.Attribute("prefix");
                        string prefix = a != null ? a.Value : "";
                        a = dsMember.Attribute("lnClass");
                        string lnClass = a != null ? a.Value : "";
                        a = dsMember.Attribute("lnInst");
                        string lnInst = a != null ? a.Value : "";
                        string fullName = String.Concat(prefix, lnClass, lnInst);
                        a = dsMember.Attribute("ldInst");
                        string ldInst = a != null ? a.Value : "";
                        a = dsMember.Attribute("doName");
                        string doName = a != null ? a.Value : "";
                        a = dsMember.Attribute("fc");
                        string fc = a != null ? a.Value : "";

                        var nodeData = _dataModel.ied.AddChildNode(new NodeLD(String.Concat(_dataModel.ied.Name, ldInst)))
                            .AddChildNode(new NodeLN(fullName))
                            .AddChildNode(new NodeFC(fc)).AddChildNode(new NodeData(doName));

                        nodeVL.ForceLinkChildNode(nodeData);
                    }
                    catch (Exception e)
                    {
                        Logger.getLogger().LogError("CreateDataSets: " + e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Reads through a report subtree and creates a report from it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="nodeName"></param>
        /// <param name="deviceName"></param>
        /// <returns> a RP node </returns>

        private void CreateReports(NodeBase lnode, IEnumerable<XElement> elements, XNamespace ns, bool isIecTree = false)
        {
            if (lnode == null || elements == null || lnode.Parent == null || lnode.Parent.Parent == null)
                Logger.getLogger().LogError("CreateReports: Something is null");

            // We are at the LN level, up 2 levels is an ied
            Iec61850Model _dataModel = (lnode.Parent.Parent as NodeIed).Model;

            foreach (XElement el in elements)
            {
                List<NodeRCB> nodeRCBs = new List<NodeRCB>();

                XAttribute a = el.Attribute("buffered");
                bool buffered = a != null ? (a.Value.ToLower() == "true") : false;
                string fc = buffered ? "BR" : "RP";
                
                a = el.Attribute("indexed");
                bool indexed = a != null ? (a.Value.ToLower() == "true") : true;    // default true???
                uint maxRptEnabled = 1;
                XElement xeRptEnabled = el.Element(ns + "RptEnabled");
                if (xeRptEnabled != null)
                {
                    a = xeRptEnabled.Attribute("max");
                    try { maxRptEnabled = uint.Parse(a.Value); }
                    catch { }
                }
                // correction necessary???
                if (!indexed) maxRptEnabled = 1;
                if (maxRptEnabled < 1) maxRptEnabled = 1;
                if (maxRptEnabled > 99) maxRptEnabled = 99;

                for (int i = 0; i < maxRptEnabled; i++)
                {
                    if (isIecTree)
                    {
                        nodeRCBs.Add(new NodeRCB(el.Attribute("name").Value + (indexed ? (i+1).ToString("D2") : "")));
                        lnode.AddChildNode(nodeRCBs[i]);
                    }
                    else
                    {
                        nodeRCBs.Add(new NodeRCB(String.Concat(lnode.Name, "$", fc, "$", el.Attribute("name").Value) + (indexed ? (i+1).ToString("D2") : "")));
                        if (buffered)
                            _dataModel.brcbs.AddChildNode(new NodeLD(lnode.Parent.Name)).AddChildNode(nodeRCBs[i]);
                        else
                            _dataModel.urcbs.AddChildNode(new NodeLD(lnode.Parent.Name)).AddChildNode(nodeRCBs[i]);
                    }
                    nodeRCBs[i].isBuffered = buffered;
                }

                // rptID
                NodeData RptId = new NodeData("RptID");
                RptId.DataType = scsm_MMS_TypeEnum.visible_string;
                a = el.Attribute("rptID");
                RptId.DataValue = a != null ? a.Value : "";

                // datSet
                NodeData DatSet = new NodeData("DatSet");
                DatSet.DataType = scsm_MMS_TypeEnum.visible_string;
                a = el.Attribute("datSet");
                if (isIecTree)
                    DatSet.DataValue = a != null ? a.Value : null; // null accepted
                else
                    DatSet.DataValue = a != null ? lnode.Name + "$" + a.Value : "";

                // confRev
                NodeData ConfRev = new NodeData("ConfRev");
                ConfRev.DataType = scsm_MMS_TypeEnum.unsigned;
                a = el.Attribute("confRev");
                try
                {
                    ConfRev.DataValue = uint.Parse(a.Value);
                }
                catch
                {
                    ConfRev.DataValue = (uint)1;
                }

                // bufTime
                NodeData BufTm = new NodeData("BufTm");
                BufTm.DataType = scsm_MMS_TypeEnum.unsigned;
                a = el.Attribute("bufTime");
                try
                {
                    BufTm.DataValue = uint.Parse(a.Value);
                }
                catch
                {
                    BufTm.DataValue = (uint)0;
                }

                // intgPd
                NodeData IntgPd = new NodeData("IntgPd");
                IntgPd.DataType = scsm_MMS_TypeEnum.unsigned;
                a = el.Attribute("intgPd");
                try
                {
                    IntgPd.DataValue = uint.Parse(a.Value);
                }
                catch
                {
                    IntgPd.DataValue = (uint)0;
                }

                // <TrgOps dchg="true" qchg="false" dupd="false" period="true" />
                NodeData TrgOps = new NodeData("TrgOps");
                TrgOps.DataType = scsm_MMS_TypeEnum.integer;
                IEC61850.Common.TriggerOptions trgOptions = IEC61850.Common.TriggerOptions.NONE;
                XElement xeTrgOps = el.Element(ns + "TrgOps");
                if (xeTrgOps != null)
                {
                    a = xeTrgOps.Attribute("dchg");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        trgOptions |= IEC61850.Common.TriggerOptions.DATA_CHANGED;
                    a = xeTrgOps.Attribute("qchg");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        trgOptions |= IEC61850.Common.TriggerOptions.QUALITY_CHANGED;
                    a = xeTrgOps.Attribute("dupd");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        trgOptions |= IEC61850.Common.TriggerOptions.DATA_UPDATE;
                    a = xeTrgOps.Attribute("period");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        trgOptions |= IEC61850.Common.TriggerOptions.INTEGRITY;
                    a = xeTrgOps.Attribute("gi");
                    if ((a != null ? a.Value : "true").ToLower() == "true") // default true
                        trgOptions |= IEC61850.Common.TriggerOptions.GI;
                }
                TrgOps.DataValue = trgOptions;

                // <OptFields seqNum="true" timeStamp="true" dataSet="true" reasonCode="true" dataRef="false" entryID="true" configRef="true" bufOvfl="true" />
                NodeData OptFlds = new NodeData("OptFlds");
                OptFlds.DataType = scsm_MMS_TypeEnum.integer;
                IEC61850.Common.ReportOptions rptOptions = IEC61850.Common.ReportOptions.NONE;
                XElement xeOptFields = el.Element(ns + "OptFields");
                if (xeOptFields != null)
                {
                    a = xeOptFields.Attribute("seqNum");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.SEQ_NUM;
                    a = xeOptFields.Attribute("timeStamp");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.TIME_STAMP;
                    a = xeOptFields.Attribute("dataSet");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.DATA_SET;
                    a = xeOptFields.Attribute("reasonCode");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.REASON_FOR_INCLUSION;
                    a = xeOptFields.Attribute("dataRef");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.DATA_REFERENCE;
                    a = xeOptFields.Attribute("entryID");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.ENTRY_ID;
                    a = xeOptFields.Attribute("configRef");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.CONF_REV;
                    a = xeOptFields.Attribute("bufOvfl");
                    if ((a != null ? a.Value : "false").ToLower() == "true")
                        rptOptions |= IEC61850.Common.ReportOptions.BUFFER_OVERFLOW;
                }
                OptFlds.DataValue = rptOptions;

                for (int i = 0; i < maxRptEnabled; i++)
                {
                    if (isIecTree)
                    {
                        nodeRCBs[i].AddChildNode(RptId);
                        nodeRCBs[i].AddChildNode(DatSet);
                        nodeRCBs[i].AddChildNode(ConfRev);
                        nodeRCBs[i].AddChildNode(OptFlds);
                        nodeRCBs[i].AddChildNode(BufTm);
                        nodeRCBs[i].AddChildNode(TrgOps);
                        nodeRCBs[i].AddChildNode(IntgPd);
                    }
                    else
                    {
                        lnode.AddChildNode(new NodeFC(fc)).AddChildNode(new NodeDO(el.Attribute("name").Value)).AddChildNode(RptId);
                        nodeRCBs[i].LinkChildNodeByAddress(RptId);
                        lnode.AddChildNode(new NodeFC(fc)).AddChildNode(new NodeDO(el.Attribute("name").Value)).AddChildNode(DatSet);
                        nodeRCBs[i].LinkChildNodeByAddress(DatSet);
                        lnode.AddChildNode(new NodeFC(fc)).AddChildNode(new NodeDO(el.Attribute("name").Value)).AddChildNode(ConfRev);
                        nodeRCBs[i].LinkChildNodeByAddress(ConfRev);
                        lnode.AddChildNode(new NodeFC(fc)).AddChildNode(new NodeDO(el.Attribute("name").Value)).AddChildNode(OptFlds);
                        nodeRCBs[i].LinkChildNodeByAddress(OptFlds);
                        lnode.AddChildNode(new NodeFC(fc)).AddChildNode(new NodeDO(el.Attribute("name").Value)).AddChildNode(BufTm);
                        nodeRCBs[i].LinkChildNodeByAddress(BufTm);
                        lnode.AddChildNode(new NodeFC(fc)).AddChildNode(new NodeDO(el.Attribute("name").Value)).AddChildNode(TrgOps);
                        nodeRCBs[i].LinkChildNodeByAddress(TrgOps);
                        lnode.AddChildNode(new NodeFC(fc)).AddChildNode(new NodeDO(el.Attribute("name").Value)).AddChildNode(IntgPd);
                        nodeRCBs[i].LinkChildNodeByAddress(IntgPd);
                    }
                }
            }
        }
    }
}
