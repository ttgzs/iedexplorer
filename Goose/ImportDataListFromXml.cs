using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using org.bn;
using GOOSE_ASN1_Model;
using org.bn.types;

namespace IEDExplorer
{
    class ImportDataListFromXml
    {
        public void Import(List<Data> dataList)
        {            
            using (OpenFileDialog importOpenFileDialog = new OpenFileDialog())
            {
                importOpenFileDialog.Title = "Select XML File";
                importOpenFileDialog.Filter = "Xml File(*.xml)|*.xml";                

                if (DialogResult.OK == importOpenFileDialog.ShowDialog())
                {
                    XDocument xmlDoc = XDocument.Load(importOpenFileDialog.FileName);
                    XElement dsnd = xmlDoc.Element("DataSet");
                    List<string> notSupportedTypes = new List<string>();

                    if (dsnd != null)
                    {                        
                        try
                        {
                            foreach (XElement nd in dsnd.Elements())
                            {
                                if(nd.Attribute("Type") != null && nd.Attribute("Value") != null)
                                    recursiveCreateGvl(dataList, nd, notSupportedTypes);
                                else
                                {
                                    MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            if (notSupportedTypes.Count > 0)
                            {
                                string nst = "";
                                foreach (string type in notSupportedTypes)
                                    nst += " - " + type + "\r\n";

                                Logger.getLogger().LogWarning("Not supported data type(s) detected: The following data types are not supported: \r\n" + nst);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.getLogger().LogError("Exception" + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid XML file !", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("DataSet imported successfully", "Import from Xml", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }                        
        }

        private void recursiveCreateGvl(object dl, XElement el, List<string> nst)
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
                    //nd.ValueChanged += new EventHandler(_ValueChanged);

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
                    //nd.ValueChanged += new EventHandler(_ValueChanged);

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
                /* 
            case "float":
                nd = new Data();                                                          
                nd.selectFloatingpoint(new FloatingPoint(new byte[] { 0 }));
                nd.ValueChanged += new EventHandler(_ValueChanged);
                if (dl is List<Data>)
                    (dl as List<Data>).Add(nd);
                else if (dl is Data)
                {
                    if ((dl as Data).isStructureSelected())
                        (dl as Data).Structure.Value.Add(nd);
                    else
                        MessageBox.Show("Error: Invalid parent data type !");
                }
                else
                    MessageBox.Show("Error: Invalid parent data type !");          
                break;  
                 */
                case "bit_string":
                    nd = new Data();
                    StringToDataConverter Converter = new StringToDataConverter();
                    BitString bs = Converter.ConvertToBitstring(el.Attribute("Value").Value);
                    if (bs != null)
                        nd.selectBitstring(bs);
                    else
                    {
                        nd.selectBitstring(new BitString(new byte[] { 0 }));
                        nd.Bitstring.TrailBitsCnt = 8;
                    }

                    if (el.Attribute("Desc") != null)
                        nd.Description = el.Attribute("Desc").Value;                    

                    //nd.ValueChanged += new EventHandler(_ValueChanged);
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
                    if (!nst.Contains(el.Attribute("Type").Value))
                        nst.Add(el.Attribute("Type").Value);
                    break;
            }

            if (el.HasElements)
            {
                foreach (XElement ndcn in el.Elements())
                    recursiveCreateGvl(nd, ndcn, nst);
            }
        }
    }
}
