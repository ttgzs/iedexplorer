using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace IEDExplorer
{
    class SCLParse
    {
        private scsm_MMS_TypeEnum getVarDataType(string bType)
        {
            if (bType != null)
            {
                switch (bType.ToLower())
                {
                    case ("enum"):
                        return scsm_MMS_TypeEnum.integer;
                    case ("quality"):
                        return scsm_MMS_TypeEnum.bit_string;
                    case ("int32"):
                        return scsm_MMS_TypeEnum.integer;
                    case ("timestamp"):
                        return scsm_MMS_TypeEnum.utc_time;
                    case ("visstring255"):
                        return scsm_MMS_TypeEnum.visible_string;
                    case ("visstring64"):
                        return scsm_MMS_TypeEnum.visible_string;
                    case ("boolean"):
                        return scsm_MMS_TypeEnum.boolean;
                    case ("octet64"):
                        return scsm_MMS_TypeEnum.octet_string;
                    case ("int8u"):
                        return scsm_MMS_TypeEnum.unsigned;
                    case ("struct"):
                        return scsm_MMS_TypeEnum.structure;
                    default:
                        return scsm_MMS_TypeEnum.integer;
                }
            }
            else
                return scsm_MMS_TypeEnum.integer;

        }

        private void processStructDAType(NodeBase DOCn, string type, XmlNodeList DATypeNodes)
        {
            foreach (XmlNode DATypeNode in DATypeNodes)
            {
                if (type == getStringAttribute(DATypeNode, "id"))
                {
                    foreach (XmlNode DATypeNodeCn in DATypeNode.ChildNodes)
                    {
                        if (DATypeNodeCn.Name == "BDA")
                        {
                            NodeData Nd = new NodeData(getStringAttribute(DATypeNodeCn, "name"));
                            Nd.DataType = getVarDataType(getStringAttribute(DATypeNodeCn, "bType"));
                            NodeBase DACn = DOCn.AddChildNode(Nd);

                            if (Nd.DataType == scsm_MMS_TypeEnum.structure)
                                processStructDAType(DACn, getStringAttribute(DATypeNodeCn, "type"), DATypeNodes);
                        }
                    }
                }
            }
        }

        private void processLN(XmlNode LNNode, NodeBase LNNb, XmlNodeList LNodeTypeNodes, XmlNodeList DOTypeNodes, XmlNodeList DATypeNodes)
        {
            // Process Instantiated Logical Node 
            foreach (XmlNode LNNodeTypeNodesNd in LNodeTypeNodes)
            {
                if (getStringAttribute(LNNode, "lnType") == getStringAttribute(LNNodeTypeNodesNd, "id") && getStringAttribute(LNNode, "lnClass") == getStringAttribute(LNNodeTypeNodesNd, "lnClass"))
                {
                    foreach (XmlNode LNNodeTypeNodesNdCn in LNNodeTypeNodesNd.ChildNodes)
                    {
                        foreach (XmlNode DOTypeNodesNd in DOTypeNodes)
                        {
                            if (getStringAttribute(LNNodeTypeNodesNdCn, "type") == getStringAttribute(DOTypeNodesNd, "id"))
                            {
                                foreach (XmlNode DOTypeNodesNdCn in DOTypeNodesNd.ChildNodes)
                                {
                                    if (DOTypeNodesNdCn.Name == "DA")
                                    {
                                        NodeBase LNFC = LNNb.AddChildNode(new NodeFC(getStringAttribute(DOTypeNodesNdCn, "fc")));
                                        NodeBase LNFCCn = LNFC.AddChildNode(new NodeData(getStringAttribute(LNNodeTypeNodesNdCn, "name")));
                                        NodeData Nd = new NodeData(getStringAttribute(DOTypeNodesNdCn, "name"));
                                        Nd.DataType = getVarDataType(getStringAttribute(DOTypeNodesNdCn, "bType"));
                                        NodeBase DOCn = LNFCCn.AddChildNode(Nd);

                                        if (Nd.DataType == scsm_MMS_TypeEnum.structure)
                                            processStructDAType(DOCn, getStringAttribute(DOTypeNodesNdCn, "type"), DATypeNodes);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private void processDOI(string LNNodeLnType, string LNNodeCnName, XmlNode LNNodeCn, NodeBase LNNb, XmlNodeList LNodeTypeNodes, XmlNodeList DOTypeNodes, XmlNodeList DATypeNodes)
        {
            // Process Instantiated Data Object 
            foreach (XmlNode LNNodeTypeNodesNd in LNodeTypeNodes)
            {
                foreach (XmlNode LNNodeTypeNodesNdCn in LNNodeTypeNodesNd.ChildNodes)
                {
                    if (LNNodeCnName == getStringAttribute(LNNodeTypeNodesNdCn, "name") && LNNodeLnType == getStringAttribute(LNNodeTypeNodesNd, "id"))
                    {
                        foreach (XmlNode DOTypeNodesNd in DOTypeNodes)
                        {
                            if (getStringAttribute(LNNodeTypeNodesNdCn, "type") == getStringAttribute(DOTypeNodesNd, "id"))
                            {
                                foreach (XmlNode DOTypeNodesNdCn in DOTypeNodesNd.ChildNodes)
                                {
                                    if (DOTypeNodesNdCn.Name == "DA")
                                    {
                                        NodeBase LNFC = LNNb.AddChildNode(new NodeFC(getStringAttribute(DOTypeNodesNdCn, "fc")));
                                        NodeBase LNFCCn = LNFC.AddChildNode(new NodeData(getStringAttribute(LNNodeTypeNodesNdCn, "name")));
                                        NodeData Nd = new NodeData(getStringAttribute(DOTypeNodesNdCn, "name"));
                                        Nd.DataType = getVarDataType(getStringAttribute(DOTypeNodesNdCn, "bType"));

                                        NodeBase DOCn = LNFCCn.AddChildNode(Nd);

                                        if (Nd.DataType == scsm_MMS_TypeEnum.structure)
                                            processStructDAType(DOCn, getStringAttribute(DOTypeNodesNdCn, "type"), DATypeNodes);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (XmlNode LNNodeCnNd in LNNodeCn.ChildNodes)
            {
                if (LNNodeCnNd.Name == "DAI")
                {
                    if (getStringAttribute(LNNodeCnNd, "name") == "d")
                    {
                        foreach (XmlNode LNNodeCnNdCn in LNNodeCnNd)
                        {
                            if (LNNodeCnNdCn.Name == "Val")
                            {
                                NodeBase FCNb = LNNb.FindChildNode("DC");
                                if (FCNb != null)
                                {
                                    NodeBase[] FCNbCn = FCNb.GetChildNodes();
                                    if(FCNbCn.Length > 0)
                                    {
                                        foreach (NodeBase DO in FCNbCn)
                                        {
                                            if (DO.Name == LNNodeCnName)
                                            {
                                                NodeBase[] DOCn = DO.GetChildNodes();

                                                if (DOCn.Length > 0)
                                                {
                                                    foreach (NodeBase DA in DOCn)
                                                    {
                                                        if (DA.Name == "d")
                                                        {
                                                            (DA as NodeData).DataValue = LNNodeCnNdCn.InnerText;                                                            
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }

                                int i = 0;
                            }
                        }
                    }
                }
            }
        }

        private void processDataSet(XmlNode LNNodeCn, NodeBase NLNb, Iec61850State iecf, string IEDName, string LNNodeName)
        {
            NodeVL VL = new NodeVL(LNNodeName + "$"+ getStringAttribute(LNNodeCn, "name"));
            VL.Defined = true;
            NodeBase VLNb = NLNb.AddChildNode(VL);

            foreach (XmlNode DataSetCn in LNNodeCn.ChildNodes)
            {
                if (DataSetCn.Name == "FCDA")
                {
                    string addr = getAddrFromFCDA(DataSetCn);
                    
                    NodeBase Nb = (iecf.DataModel.ied as NodeIed).FindNodeByAddress(IEDName + getStringAttribute(DataSetCn, "ldInst"), getAddrFromFCDA(DataSetCn));
                    
                    if (Nb != null)                    
                        VLNb.LinkChildNode(Nb);     
                }
            }
        }

        private void processGSEControl(XmlNode LNNodeCn, NodeBase LNNb, XmlNodeList DataSets, XmlNodeList GSE)
        {
            NodeBase LNFc = LNNb.AddChildNode(new NodeFC("GO"));
            NodeBase DO = LNFc.AddChildNode(new NodeData(getStringAttribute(LNNodeCn, "name")));
            
            NodeBase Nd = new NodeData("GoID");
            (Nd as NodeData).DataType = scsm_MMS_TypeEnum.visible_string;
            (Nd as NodeData).DataValue = getStringAttribute(LNNodeCn, "appID");
            DO.AddChildNode(Nd);

            Nd = new NodeData("DatSet");
            (Nd as NodeData).DataType = scsm_MMS_TypeEnum.visible_string;
            (Nd as NodeData).DataValue = LNNb.IecAddress + "$" + getStringAttribute(LNNodeCn, "datSet");
            DO.AddChildNode(Nd);

            if (GSE.Count > 0)
            {
                foreach (XmlNode GSENode in GSE)
                {
                    if (GSENode.Name == "GSE" &&  getStringAttribute(LNNodeCn, "name") == getStringAttribute(GSENode, "cbName"))
                    {
                        foreach (XmlNode GSENodeCn in GSENode.ChildNodes)
                        {
                            if (GSENodeCn.Name == "Address")
                            {
                                Nd = new NodeData("DstAddress");
                                (Nd as NodeData).DataType = scsm_MMS_TypeEnum.structure;
                                NodeBase DA = DO.AddChildNode(Nd);

                                foreach (XmlNode GSENodeCnCn in GSENodeCn.ChildNodes)
                                {
                                    switch (getStringAttribute(GSENodeCnCn, "type"))
                                    {
                                        case "MAC-Address":
                                            Nd = new NodeData("Addr");
                                            (Nd as NodeData).DataType = scsm_MMS_TypeEnum.visible_string;
                                            (Nd as NodeData).DataValue = GSENodeCnCn.InnerText;
                                            DA.AddChildNode(Nd);
                                            break;
                                        case "APPID":
                                            Nd = new NodeData("APPID");
                                            (Nd as NodeData).DataType = scsm_MMS_TypeEnum.visible_string;
                                            (Nd as NodeData).DataValue = Convert.ToUInt32(GSENodeCnCn.InnerText).ToString();
                                            DA.AddChildNode(Nd);
                                            break;
                                        case "VLAN-PRIORITY":
                                            Nd = new NodeData("PRIORITY");
                                            (Nd as NodeData).DataType = scsm_MMS_TypeEnum.visible_string;
                                            (Nd as NodeData).DataValue = GSENodeCnCn.InnerText;
                                            DA.AddChildNode(Nd);
                                            break;
                                    }
                                }
                            }                            
                            break;  
                        }
                    }
                }
            }
        }

        private void processReportControl(XmlNode LNNodeCn, NodeBase LNNb)
        {
            NodeBase lnfc = LNNb.AddChildNode(new NodeFC("RP"));
            string[] rptIDSplit = LNNodeCn.Attributes.GetNamedItem("rptID").Value.ToString().Split('$');

            NodeBase lnfccn = lnfc.AddChildNode(new NodeData(rptIDSplit[rptIDSplit.Length - 1]));

            foreach (XmlAttribute Attr in LNNodeCn.Attributes)
            {
                NodeBase nd = null;

                switch (Attr.Name.ToLower())
                {
                    case "rptid":
                        nd = new NodeData("RptId");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.visible_string;
                        (nd as NodeData).StringValue = Attr.Value;
                        lnfccn.AddChildNode(nd);
                        break;
                    case "confrev":
                        nd = new NodeData("ConfRev");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.unsigned;
                        (nd as NodeData).DataValue = Attr.Value;
                        lnfccn.AddChildNode(nd);
                        break;
                    case "intgpd":
                        nd = new NodeData("IntgPd");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.unsigned;
                        (nd as NodeData).DataValue = Attr.Value;
                        lnfccn.AddChildNode(nd);
                        break;
                    case "datset":
                        nd = new NodeData("DatSet");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.visible_string;
                        (nd as NodeData).StringValue = Attr.Value;
                        lnfccn.AddChildNode(nd);
                        break;
                    case "buftime":
                        nd = new NodeData("BufTm");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.unsigned;
                        (nd as NodeData).DataValue = Attr.Value;
                        lnfccn.AddChildNode(nd);
                        break;
                }
            }

            foreach (XmlNode LNNodeCnCn in LNNodeCn.ChildNodes)
            {
                NodeBase nd = null;

                switch (LNNodeCnCn.Name.ToLower())
                {
                    case "trgops":
                        nd = new NodeData("TrgOps");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.bit_string;
                        lnfccn.AddChildNode(nd);
                        break;
                    case "optfields":
                        nd = new NodeData("OptFlds");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.bit_string;
                        lnfccn.AddChildNode(nd);
                        break;
                    case "rptenabled":
                        nd = new NodeData("RprEna");
                        (nd as NodeData).DataType = scsm_MMS_TypeEnum.boolean;
                        (nd as NodeData).DataValue = LNNodeCnCn.Value;
                        lnfccn.AddChildNode(nd);
                        break;
                }

            }
        }

        private string getStringAttribute(XmlNode node, string name)
        {
            XmlNode attr = null;

            if (node.Attributes != null)
                attr = node.Attributes[name];

            if (attr != null)
                return attr.Value.ToString();
            else
                return null;
        }

        private string getLDName(XmlNode IEDNode, XmlNode LDeviceNode)
        {
            string Name = getStringAttribute(IEDNode, "name");
            string Inst = getStringAttribute(LDeviceNode, "inst");

            return Name + Inst;
        }

        private string getLNNodeName(XmlNode LNNnode)
        {
            string LNPrefix = getStringAttribute(LNNnode, "prefix");
            string LNClass = getStringAttribute(LNNnode, "lnClass");
            string LNInstance = getStringAttribute(LNNnode, "inst");

            return LNPrefix + LNClass + LNInstance;
        }

        private string getAddrFromFCDA(XmlNode FCDANode)
        {
            string LDinst = getStringAttribute(FCDANode, "ldinst");
            string Prefix = getStringAttribute(FCDANode, "prefix");
            string LNClass = getStringAttribute(FCDANode, "lnClass");
            string LNInst = getStringAttribute(FCDANode, "lnInst");
            string FC = getStringAttribute(FCDANode, "fc");
            string DOName = getStringAttribute(FCDANode, "doName");
            
            string DAName = getStringAttribute(FCDANode, "daName");
            
            if(DAName != null)
                DAName = "$" + DAName;
            
            return Prefix + LNClass + LNInst + "$" + FC + "$" + DOName + DAName;
        }

        public Iec61850State sclParse(XmlDocument SclDoc)
        {
            XmlNodeList IEDNodes = SclDoc.GetElementsByTagName("IED");
            XmlNodeList LDeviceNodes = SclDoc.GetElementsByTagName("LDevice");
            XmlNodeList LNodeTypeNodes = SclDoc.GetElementsByTagName("LNodeType");
            XmlNodeList DOTypeNodes = SclDoc.GetElementsByTagName("DOType");
            XmlNodeList LN0Node = SclDoc.GetElementsByTagName("LN0");
            XmlNodeList LNNodes = SclDoc.GetElementsByTagName("LN");
            XmlNodeList DATypeNodes = SclDoc.GetElementsByTagName("DAType");
            XmlNodeList DataSets = SclDoc.GetElementsByTagName("DataSet");
            XmlNodeList GSE = SclDoc.GetElementsByTagName("GSE");

            NodeBase LDNb, LNNb, NLNb = null;

            Iec61850State iecf = new Iec61850State();

            foreach (XmlNode IEDNode in IEDNodes)
            {
                foreach (XmlNode LDeviceNode in LDeviceNodes)
                {
                    string LDName = getLDName(IEDNode, LDeviceNode);

                    if (LDName != null)
                    {
                        LDNb = iecf.DataModel.ied.AddChildNode(new NodeLD(LDName));
                        
                        if (DataSets.Count > 0)
                        {
                            NLNb = iecf.DataModel.lists.AddChildNode(new NodeLD(iecf.DataModel.ied.GetActualChildNode().Name));
                            iecf.DataModel.ied.DefineNVL = true;
                        }

                        foreach (XmlNode LNNode in LDeviceNode.ChildNodes)
                        {
                            if (LNNode.Name == "LN")
                            {
                                string LNNodeName = getLNNodeName(LNNode);

                                if (LNNodeName != null)
                                {
                                    LNNb = LDNb.AddChildNode(new NodeLN(LNNodeName));

                                    processLN(LNNode, LNNb, LNodeTypeNodes, DOTypeNodes, DATypeNodes);

                                    foreach (XmlNode LNNodeCn in LNNode.ChildNodes)
                                    {
                                        if(LNNodeCn.Name == "DOI")
                                            processDOI(getStringAttribute(LNNode, "lnType"), getStringAttribute(LNNodeCn, "name"), LNNodeCn, LNNb, LNodeTypeNodes, DOTypeNodes, DATypeNodes);
                                    }
                                }
                            }
                        }

                        foreach (XmlNode LNNode in LDeviceNode.ChildNodes)
                        {
                            if (LNNode.Name == "LN0")
                            {
                                string LNNodeName = getLNNodeName(LNNode);

                                if (LNNodeName != null)
                                {
                                    LNNb = LDNb.AddChildNode(new NodeLN(LNNodeName));

                                    processLN(LNNode, LNNb, LNodeTypeNodes, DOTypeNodes, DATypeNodes);

                                    foreach (XmlNode LNNodeCn in LNNode.ChildNodes)
                                    {
                                        switch (LNNodeCn.Name)
                                        {
                                            case "DOI":
                                                processDOI(getStringAttribute(LNNode, "lnType"), getStringAttribute(LNNodeCn, "name"), LNNodeCn, LNNb, LNodeTypeNodes, DOTypeNodes, DATypeNodes);
                                                break;
                                            case "ReportControl":
                                                processReportControl(LNNodeCn, LNNb);
                                                break;
                                            case "GSEControl":
                                                processGSEControl(LNNodeCn, LNNb, DataSets, GSE);
                                                break;
                                            case "DataSet":
                                                processDataSet(LNNodeCn, NLNb, iecf, getStringAttribute(IEDNode, "name"), getStringAttribute(LNNode, "lnClass"));
                                                break;
                                        }
                                    }
                                }
                                break;
                            }
                        }                      
                    }
                }
            }

            return iecf;
        }
    }
}
