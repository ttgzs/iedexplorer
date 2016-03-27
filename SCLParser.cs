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
using System.Xml;
using System.Xml.Linq;
using MMS_ASN1_Model;

namespace IEDExplorer
{
    class SCLParser
    {
        //private Iec61850State _iecs;
        private List<Iec61850Model> _dataModels = new List<Iec61850Model>();
        private List<NodeBase> _nodeTypes;
        private List<NodeBase> _dataObjectTypes;
        private List<NodeBase> _dataAttributeTypes;

        //private string _iedName = "";
        private Logger logger = Logger.getLogger();

        /// <summary>
        /// Reads through the specified SCL file and creates a logical tree
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="env"></param>
        /// <returns>IECS tree state to be displayed</returns>
        public List<Iec61850Model> CreateTree(String fileName) //, Env env)
        {
            try
            {
                _dataModels.Add(new Iec61850State().DataModel);
                _nodeTypes = new List<NodeBase>();
                _dataObjectTypes = new List<NodeBase>();
                _dataAttributeTypes = new List<NodeBase>();
                int i = 0;

                logger.LogInfo("Reading node (LN) and data object (DO) types.");
                // Model [0] is a master for types etc.
                GetTypes(fileName);

                using (var reader = XmlReader.Create(fileName))
                {
                    logger.LogInfo("Reading XML tree.");
                    reader.ReadToDescendant("IED");
                    do
                    {
                        // Create model. model 0 (master model) is already created above
                        if (i > 0) _dataModels.Add(new Iec61850State().DataModel);
                        _dataModels[i].ied = new NodeIed(reader.GetAttribute("name"), _dataModels[i]);  //reader.Name);
                        //_iedName = reader.GetAttribute("name");
                        _dataModels[i].ied.VendorName = reader.GetAttribute("manufacturer");
                        _dataModels[i].ied.ModelName = reader.GetAttribute("type");
                        _dataModels[i].ied.Revision = reader.GetAttribute("revision");

                        XmlReader sreader = reader.ReadSubtree();
                        while (sreader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                switch (reader.Name)
                                {
                                    case "LDevice":
                                        _dataModels[i].ied.AddChildNode(CreateLogicalDevice(reader.ReadSubtree(), _dataModels[i].ied.Name));
                                        break;
                                }
                            }
                        }
                        _dataModels[i].ied.SortImmediateChildren(); //alphabetical
                        _dataModels[i].enums.SortImmediateChildren();
                        logger.LogInfo("Reading data sets and reports.");
                        GetDataSetsAndReports(fileName, _dataModels[i]);
                        i++;
                    } while (reader.ReadToNextSibling("IED"));
                }
            }
            catch (Exception e)
            {
                logger.LogError("Error reading file " + fileName + ": " + e.Message);
            }

            return _dataModels;
        }

        /// <summary>
        /// Creates a logical device (LD) node and returns it
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a LD node </returns>
        private NodeLD CreateLogicalDevice(XmlReader reader, string _iedName)
        {
            reader.Read();
            var logicalDevice = new NodeLD(String.Concat(_iedName, reader.GetAttribute("inst")));
            
            while (reader.Read())
            {
                // if a logical node element, create and add a LN child node
                if (reader.IsStartElement() && reader.Name.StartsWith("LN"))
                {
                    logicalDevice.AddChildNode(CreateLogicalNode2(reader.ReadSubtree()));
                }
            }
            logicalDevice.SortImmediateChildren(); // alphabetical sort
            return logicalDevice;
        }

        /// <summary>
        /// Unused
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a LN node </returns>
        private NodeLN CreateLogicalNode(XmlReader reader)
        {
            reader.Read();
            
            var prefix = reader.GetAttribute("prefix");
            var lnClass = reader.GetAttribute("lnClass");
            var inst = reader.GetAttribute("inst");
            var name = "";
            var type = reader.GetAttribute("lnType");

            //var fcs = CreateFunctionalConstraints(type);

            name = !String.IsNullOrWhiteSpace(prefix) ? String.Concat(prefix, lnClass, inst) : String.Concat(lnClass, inst);

            var logicalNode = new NodeLN(name);
            logicalNode.TypeId = type;

            //foreach (var key in fcs.Keys)
            //{
            //    NodeFC fc = new NodeFC(key.ToString());

            //    logicalNode.AddChildNode(fcs[key] as NodeBase);
            //}

            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals("DOI"))
                {
                    logicalNode.AddChildNode(CreateDigitalObject(reader.ReadSubtree()));
                }
            }
            logicalNode.SortImmediateChildren();
            return logicalNode;
        }

        /// <summary>
        /// Creates a logical node (LN) and all of its children, including
        /// FCs, DO's, and DA's
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a LN node </returns>
        private NodeLN CreateLogicalNode2(XmlReader reader)
        {
            reader.Read();
            
            var prefix = reader.GetAttribute("prefix");
            var lnClass = reader.GetAttribute("lnClass");
            var inst = reader.GetAttribute("inst");
            var type = reader.GetAttribute("lnType");

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
                return null;
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
                    var fc = (dataAttribute as NodeData).SCL_FCDesc;
                    (dataAttribute as NodeData).SCL_DOName = dataObject.Name;
                    NodeData newNode = new NodeData(dataAttribute.Name);
                    newNode.SCL_Type = (dataAttribute as NodeData).SCL_Type;
                    newNode.SCL_BType = (dataAttribute as NodeData).SCL_BType;
                    newNode.SCL_DOName = (dataAttribute as NodeData).SCL_DOName;
                    newNode.SCL_FCDesc = (dataAttribute as NodeData).SCL_FCDesc;
                    
                    // when the type is specified (ie. when it's a struct), get the struct child nodes
                    if (!String.IsNullOrWhiteSpace(newNode.SCL_Type))
                    {
                        var dataType =
                            _dataAttributeTypes.Single(dat => dat.Name.Equals((newNode.SCL_Type)));
                        foreach (NodeBase child in dataType.GetChildNodes())
                        {
                            var tempChild = new NodeData(child.Name);
                            tempChild.SCL_BType = (child as NodeData).SCL_BType;
                            if (!String.IsNullOrWhiteSpace((child as NodeData).SCL_Type))
                            {
                                var subDataType = _dataAttributeTypes.Single(dat => dat.Name.Equals((child as NodeData).SCL_Type));
                                foreach (NodeBase subChild in subDataType.GetChildNodes())
                                {
                                    var tempSubChild = new NodeData(subChild.Name);
                                    tempSubChild.SCL_BType = (subChild as NodeData).SCL_BType;
                                    tempChild.AddChildNode(subChild);
                                }
                            }
                            newNode.AddChildNode(tempChild);
                        }
                    }
                    if (!functionalConstraints.ContainsKey(fc))
                    {
                        NodeFC nodeFC = new NodeFC(fc);
                        nodeFC.ForceAddChildNode(newNode);
                        functionalConstraints.Add(fc, nodeFC);
                    }
                    else
                    {
                        (functionalConstraints[fc] as NodeBase).ForceAddChildNode(newNode);
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
                    var doName = (da as NodeData).SCL_DOName;
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

            return logicalNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private NodeLN CreateLogicalNodeType(XmlReader reader)
        {
            reader.Read();
            var logicalNode = new NodeLN(reader.GetAttribute("id"));
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals("DO"))
                {
                    logicalNode.AddChildNode(CreateDigitalObject(reader.ReadSubtree()));
                }
            }
            return logicalNode;
        }

        /// <summary>
        /// Create a DO from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a DO Node representing a DO </returns>
        private NodeDO CreateDigitalObject(XmlReader reader)
        {
            reader.Read();
            var digitalObject = new NodeDO(reader.GetAttribute("name"));
            if (reader.AttributeCount > 1)
                digitalObject.SCL_Type = reader.GetAttribute("type");
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals("DAI"))
                {
                    digitalObject.AddChildNode(CreateDataAttribute(reader.ReadSubtree()));
                }
            }
            digitalObject.SortImmediateChildren();
            return digitalObject;
        }

        /// <summary>
        /// Creates a DO Type node from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a DO node representing a DOType </returns>
        private NodeDO CreateDigitalObjectType(XmlReader reader)
        {
            reader.Read();
            var digitalObjectType = new NodeDO(reader.GetAttribute("id"));
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals("DA"))
                {
                    digitalObjectType.AddChildNode(CreateDataAttribute(reader.ReadSubtree()));
                }
            }
            return digitalObjectType;
        }

        /// <summary>
        /// Creates a DA Type node from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> a node representing a DAType </returns>
        private NodeBase CreateDataAttributeType(XmlReader reader)
        {
            reader.Read();
            var dataAttributeType = new NodeBase(reader.GetAttribute("id"));
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals("BDA"))
                {
                    dataAttributeType.AddChildNode(CreateDataAttribute(reader.ReadSubtree()));
                }
            }
            return dataAttributeType;
            
        }

        /// <summary>
        /// Creates a DA node from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private NodeData CreateDataAttribute(XmlReader reader)
        {
            reader.Read();
            var data = new NodeData(reader.GetAttribute("name"));
            data.SCL_FCDesc = reader.GetAttribute("fc");
            if (null == reader.GetAttribute("bType"))
            {
                reader.ReadToDescendant("Val");
                data.DataValue = reader.ReadElementString();
            }
            else
            {
                data.SCL_BType = reader.GetAttribute("bType");
                if (data.SCL_BType.Equals("Struct") && null != reader.GetAttribute("type"))
                    data.SCL_Type = reader.GetAttribute("type");
                else if (data.SCL_BType.Equals("Enum"))
                    data.SCL_BType = String.Concat(data.SCL_BType, " (Integer)");
            }
            return data;
        }

        /// <summary>
        /// Creates an Enum type from parsed XML
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> an Enum Type node </returns>
        private NodeBase CreateEnumType(XmlReader reader)
        {
            reader.Read();
            var enumType = new NodeBase(reader.GetAttribute("id"));

            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals("EnumVal"))
                {
                    var id = reader.GetAttribute("ord");
                    var name = reader.ReadString(); //ReadElementString();
                    var enumVal = enumType.AddChildNode(new NodeData(name));
                    (enumVal as NodeData).DataValue = id;
                }
            }

            return enumType;
        }

        /// <summary>
        /// Parses the XML file for all LN, DO, DA, and Enum types
        /// </summary>
        /// <param name="filename"></param>
        private void GetTypes(String filename)
        {
            var reader = new XmlTextReader(filename);
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "LNodeType":
                            _nodeTypes.Add(CreateLogicalNodeType(reader.ReadSubtree()));
                            break;
                        case "DOType":
                            _dataObjectTypes.Add(CreateDigitalObjectType(reader.ReadSubtree()));
                            break;
                        case "DAType":
                            _dataAttributeTypes.Add(CreateDataAttributeType(reader.ReadSubtree()));
                            break;
                        case "EnumType":
                            _dataModels[0].enums.AddChildNode(CreateEnumType(reader.ReadSubtree()));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Reads through the SCL file and looks for data sets and reports
        /// </summary>
        /// <param name="filename"></param>
        private void GetDataSetsAndReports(String filename, Iec61850Model _dataModel)
        {
            var reader = new XmlTextReader(filename);
            var iedName = "";
            var deviceName = "";
            var lnName = "";
            _dataModel.lists = new NodeIed("lists", _dataModel);
            _dataModel.urcbs = new NodeIed("reports", _dataModel);
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    NodeBase parent;
                    switch (reader.Name)
                    {
                        case "IED":
                            iedName = reader.GetAttribute("name");
                            break;
                        case "LDevice":
                            deviceName = reader.GetAttribute("inst");
                            break;
                        case "LN0":
                            lnName = String.Concat(reader.GetAttribute("prefix"), reader.GetAttribute("lnClass"),
                                reader.GetAttribute("inst"));
                            break;
                        case "LN":
                            lnName = String.Concat(reader.GetAttribute("prefix"), reader.GetAttribute("lnClass"),
                                reader.GetAttribute("inst"));
                            break;                        
                        case "DataSet":
                            parent = _dataModel.lists.AddChildNode(new NodeBase(String.Concat(iedName, deviceName))); // will return if already exists
                            parent.AddChildNode(CreateDataSet(reader.ReadSubtree(), lnName, deviceName, _dataModel));
                            break;
                        case "ReportControl":
                            parent = _dataModel.urcbs.AddChildNode(new NodeBase(String.Concat(iedName, deviceName))); // will return if already exists
                            parent.AddChildNode(CreateReport(reader.ReadSubtree(), lnName, deviceName));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Reads through a data set subtree and creates a data set node from it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="nodeName"></param>
        /// <param name="deviceName"></param>
        /// <returns> a VL node </returns>
        private NodeVL CreateDataSet(XmlReader reader, string nodeName, string deviceName, Iec61850Model _dataModel)
        {
            reader.Read();

            var nodeVL = new NodeVL(String.Concat(nodeName, "$", reader.GetAttribute("name")));
            var test = reader.GetAttribute("name");
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name.Equals("FCDA"))
                {
                    var prefix = reader.GetAttribute("prefix");
                    var lnClass = reader.GetAttribute("lnClass");
                    var lnInst = reader.GetAttribute("lnInst");
                    var fullName = String.Concat(prefix, lnClass, lnInst);
                    var ldInst = reader.GetAttribute("ldInst");
                    var doName = reader.GetAttribute("doName");
                    var fc = reader.GetAttribute("fc");

                    var nodeData = _dataModel.ied.AddChildNode(new NodeLD(String.Concat(_dataModel.ied.Name, ldInst)))
                        .AddChildNode(new NodeLN(fullName))
                       .AddChildNode(new NodeFC(fc)).AddChildNode(new NodeData(doName));

                    nodeVL.ForceLinkChildNode(nodeData);
                }
            }
            return nodeVL;
        }

        /// <summary>
        /// Reads through a report subtree and creates a report from it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="nodeName"></param>
        /// <param name="deviceName"></param>
        /// <returns> a RP node </returns>
        private NodeRCB CreateReport(XmlReader reader, string nodeName, string deviceName)
        {
            reader.Read();

            NodeRCB nodeRP = new NodeRCB(String.Concat(nodeName, "$RP$", reader.GetAttribute("name")));

            // rptID
            NodeData RptId = new NodeData("RptID");
            RptId.DataType = scsm_MMS_TypeEnum.visible_string;
            RptId.DataValue = reader.GetAttribute("rptID");
            RptId.Address = String.Concat(deviceName, "/", nodeName, ".", nodeRP.Name, ".", RptId.Name);

            // datSet
            NodeData DatSet = new NodeData("DatSet");
            DatSet.DataType = scsm_MMS_TypeEnum.visible_string;
            DatSet.DataValue = reader.GetAttribute("datSet");
            DatSet.Address = String.Concat(deviceName, "/", nodeName, ".", nodeRP.Name, ".", DatSet.Name);

            // confRev
            NodeData ConfRev = new NodeData("ConfRev");
            ConfRev.DataType = scsm_MMS_TypeEnum.unsigned;
            ConfRev.DataValue = reader.GetAttribute("confRev");
            ConfRev.Address = String.Concat(deviceName, "/", nodeName, ".", nodeRP.Name, ".", ConfRev.Name);

            // bufTime
            NodeData BufTm = new NodeData("BufTm");
            BufTm.DataType = scsm_MMS_TypeEnum.unsigned;
            BufTm.DataValue = reader.GetAttribute("bufTime");
            BufTm.Address = String.Concat(deviceName, "/", nodeName, ".", nodeRP.Name, ".", BufTm.Name);

            nodeRP.AddChildNode(RptId);
            nodeRP.AddChildNode(DatSet);
            nodeRP.AddChildNode(ConfRev);
            nodeRP.AddChildNode(BufTm);

            return nodeRP;
        }
    }
}
