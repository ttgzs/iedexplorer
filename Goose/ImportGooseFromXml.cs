using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using PcapDotNet.Core;
using org.bn;
using org.bn.types;
using GOOSE_ASN1_Model;


namespace IEDExplorer
{
    class ImportGooseFromXml
    {
        public void Import(ref int gooseItems, Control.ControlCollection cc, PacketDevice netDev)
        {            
            using (OpenFileDialog importOpenFileDialog = new OpenFileDialog())
            {
                importOpenFileDialog.Title = "Select XML File";
                importOpenFileDialog.Filter = "Xml File(*.xml)|*.xml";

                if (DialogResult.OK == importOpenFileDialog.ShowDialog())
                {
                    XDocument xmlDoc = XDocument.Load(importOpenFileDialog.FileName);
                    XElement gooses = xmlDoc.Element("Gooses");

                    if (gooses != null && xmlDoc.Elements("Gooses").Count() == 1)
                    {
                        if (gooses.Elements().Count() > 0)
                        {
                            foreach (XElement goose in gooses.Elements())
                            {
                                GooseControl gc = new GooseControl("Goose " + gooseItems++.ToString() + ":", "00:00:00:00:00:00", netDev);
       
                                XElement parameters =  goose.Element("Parameters");
                                if (parameters != null && goose.Elements("Parameters").Count() == 1)
                                {
                                    if (parameters.Elements().Count() == 17)
                                    {
                                        foreach (XElement parameter in parameters.Elements())
                                        {
                                            if (parameter.Attribute("Value") != null)                                                                                                                        
                                                gc.gooseParameters[parameter.Name.ToString()] = parameter.Attribute("Value").Value.ToString();                                                                                                                                                                         
                                            else
                                            {
                                                MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return;
                                            }
                                        }

                                        gc.updateComponents();                                 
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    XElement dataset = goose.Element("DataSet");

                                    if (dataset != null && goose.Elements("DataSet").Count() == 1)
                                    {
                                        if (dataset.Elements("Data").Count() > 0)
                                        {
                                            foreach (XElement data in dataset.Elements("Data"))                                            
                                                recursiveCreateDataList(gc.dataList, data);                                            
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    XElement seqdata = goose.Element("SeqData");

                                    if(seqdata != null && goose.Elements("SeqData").Count() == 1)
                                    {
                                        if (seqdata.Elements("Data").Count() > 0)
                                        {
                                            NodeGVL gvl = new NodeGVL("Data");
                                            
                                            GooseDataEdit gde = new GooseDataEdit();                                           

                                            int i = 0;

                                            foreach (GOOSE_ASN1_Model.Data dataListCn in gc.dataList)                                                                                           
                                                i = gde.recursiveReadData(null, dataListCn, gvl, i, DateTime.Now);

                                            gc.seqData.Clear();

                                            foreach (XElement seqd in seqdata.Elements("Data"))
                                            {
                                                if (seqd.Attribute("Name") != null)
                                                {
                                                    if (gvl.GetChildNodes().Length > 0)
                                                    {
                                                        foreach (NodeGData ngdcn in gvl.GetChildNodes())
                                                        {
                                                            NodeGData fngd;
                                                            if ((fngd = recursiveFindNodeGData(ngdcn, seqd.Attribute("Name").Value)) != null)
                                                            {
                                                                if (seqd.Attribute("Duration") != null && seqd.Attribute("Value") != null)                                                                    
                                                                    gc.seqData.Add(new SeqData(fngd, seqd.Attribute("Value").Value, Convert.ToInt32(seqd.Attribute("Duration").Value)));
                                                                else
                                                                {
                                                                    MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                                    return;
                                                                }

                                                                break;
                                                            }
                                                        }                                                        
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                gc.Dock = DockStyle.Top;
                                cc.Add(gc);
                                cc.SetChildIndex(gc, 0);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }


        private NodeGData recursiveFindNodeGData(NodeGData ngd, string name)
        {
            if (name == ngd.CommAddress.Domain.ToString() + ((ngd.CommAddress.Variable.ToString() != "") ? ("/" + ngd.CommAddress.Variable.ToString().Replace("$", "/")) : ""))
                return ngd;

            if (ngd.GetChildNodes().Length > 0)
            {
                foreach (NodeGData ngdcn in ngd.GetChildNodes())
                {
                    NodeGData ret = recursiveFindNodeGData(ngdcn, name);
                    
                    if (ret != null)
                        return ret;
                }
            }

            return null;
        }

        private void recursiveCreateDataList(object dl, XElement el)
        {
            Data nd = new Data();

            switch (el.Attribute("Type").Value)
            {
                case "structure":
                    nd = new Data();
                    DataSequence sd = new DataSequence();
                    sd.initValue();
                    nd.selectStructure(sd);
                    if (dl is List<Data>)
                        (dl as List<Data>).Add(nd);
                    else if (dl is Data)
                    {
                        if ((dl as Data).isStructureSelected())
                            (dl as Data).Structure.Value.Add(nd);
                        else
                            Logger.getLogger().LogError("Error: Invalid parent data type !");
                    }
                    else
                        Logger.getLogger().LogError("Error: Invalid parent data type !");
                    break;
                case "boolean":
                    nd = new Data();
                    nd.selectBoolean(el.Attribute("Value").Value == "True" ? true : false);

                    if (el.Attribute("Desc") != null)
                        nd.Description = el.Attribute("Desc").Value;
                    
                    if (dl is List<Data>)
                        (dl as List<Data>).Add(nd);
                    else if (dl is Data)
                    {
                        if ((dl as Data).isStructureSelected())
                            (dl as Data).Structure.Value.Add(nd);
                        else
                            Logger.getLogger().LogError("Error: Invalid parent data type !");
                    }
                    else
                        Logger.getLogger().LogError("Error: Invalid parent data type !");
                    break;
                case "integer":
                    nd = new Data();
                    nd.selectInteger(Convert.ToInt32(el.Attribute("Value").Value));

                    if (el.Attribute("Desc") != null)
                        nd.Description = el.Attribute("Desc").Value;

                    if (dl is List<Data>)
                        (dl as List<Data>).Add(nd);
                    else if (dl is Data)
                    {
                        if ((dl as Data).isStructureSelected())
                            (dl as Data).Structure.Value.Add(nd);
                        else
                            Logger.getLogger().LogError("Error: Invalid parent data type !");
                    }
                    else
                        Logger.getLogger().LogError("Error: Invalid parent data type !");
                    break;

                case "bit_string":
                    nd = new Data();
                    StringToDataConverter Converter = new StringToDataConverter();
                    BitString bs = Converter.ConvertToBitstring(el.Attribute("Value").Value);

                    if (el.Attribute("Desc") != null)
                        nd.Description = el.Attribute("Desc").Value;

                    if (bs != null)
                        nd.selectBitstring(bs);
                    else
                    {
                        nd.selectBitstring(new BitString(new byte[] { 0 }));
                        nd.Bitstring.TrailBitsCnt = 8;
                    }
                    if (dl is List<Data>)
                        (dl as List<Data>).Add(nd);
                    else if (dl is Data)
                    {
                        if ((dl as Data).isStructureSelected())
                            (dl as Data).Structure.Value.Add(nd);
                        else
                            Logger.getLogger().LogError("Error: Invalid parent data type !");
                    }
                    else
                        Logger.getLogger().LogError("Error: Invalid parent data type !");
                    break;
                default:
                    break;
            }

            if (el.HasElements)
            {
                foreach (XElement ndcn in el.Elements())
                    recursiveCreateDataList(nd, ndcn);
            }
        }
    }
}
