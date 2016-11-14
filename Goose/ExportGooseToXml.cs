using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using org.bn;
using GOOSE_ASN1_Model;

namespace IEDExplorer
{
    class ExportGoosesToXml
    {

        private void recursiveReadData(Data d, XElement el)
        {
            XElement eldn;

            if (d.isStructureSelected())
            {                
                el.Add(new XElement("Data", new XAttribute("Type", "structure")));
                eldn = el.Descendants().Last();
                foreach (GOOSE_ASN1_Model.Data data in d.Structure.Value)
                    recursiveReadData(data, eldn);            
            }
            else if (d.isBitstringSelected())
            {
                NodeGData ngd = new NodeGData("bitstring");
                ngd.DataType = scsm_MMS_TypeEnum.bit_string;
                ngd.DataValue = d.Bitstring.Value;
                ngd.DataParam = d.Bitstring.TrailBitsCnt;

                el.Add(new XElement("Data", new XAttribute("Type", "bit_string"), new XAttribute("Value", ngd.StringValue), (d.Description != null && d.Description !="") ? new XAttribute("Desc", d.Description) : null ));
            }
            else if (d.isIntegerSelected())
                el.Add(new XElement("Data", new XAttribute("Type", "integer"), new XAttribute("Value", d.Integer.ToString()), (d.Description != null && d.Description != "") ? new XAttribute("Desc", d.Description) : null));
            else if (d.isBooleanSelected())
                el.Add(new XElement("Data", new XAttribute("Type", "boolean"), new XAttribute("Value", d.Boolean.ToString()), (d.Description != null && d.Description != "") ? new XAttribute("Desc", d.Description) : null));                      
        }

        public void Export(Control.ControlCollection cc)
        {
            try
            {
                XDocument xmlDoc = new XDocument();

                if (cc.Count > 0)
                {
                    XElement gooses = new XElement("Gooses");
                    xmlDoc.Add(gooses);

                    for (int i = 0; i < cc.Count; i++)
                    {
                        GooseControl gc = cc[cc.Count - i - 1] as GooseControl;

                        if ((gc.gooseParameters.Count == 17) && (gc.dataList.Count > 0))
                        {
                            XElement goose = new XElement(gc.Name.Replace(" ", "_").Replace(":", ""));
                            gooses.Add(goose);

                            XElement parameters = new XElement("Parameters");
                            goose.Add(parameters);

                            foreach (DictionaryEntry param in gc.gooseParameters)
                                parameters.Add(new XElement(param.Key.ToString(), new XAttribute("Value", param.Value.ToString())));

                            XElement dataset = new XElement("DataSet");
                            goose.Add(dataset);

                            foreach (Data d in gc.dataList)
                                recursiveReadData(d, dataset);

                            if (gc.seqData.Count > 0)
                            {
                                XElement seqdata = new XElement("SeqData");
                                goose.Add(seqdata);

                                foreach (SeqData seqd in gc.seqData)
                                    seqdata.Add(new XElement("Data", new XAttribute("Name", seqd.refdata.CommAddress.Domain.ToString() + ((seqd.refdata.CommAddress.Variable.ToString() != "") ? ("/" + seqd.refdata.CommAddress.Variable.ToString().Replace("$", "/")) : "")), new XAttribute("Value", seqd.data), new XAttribute("Duration", seqd.duration.ToString())));                                
                            }
                        }
                        else
                        {
                            MessageBox.Show("Goose not exported. No data to export !", "Export to Xml", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }

                    using (SaveFileDialog exportSaveFileDialog = new SaveFileDialog())
                    {
                        exportSaveFileDialog.Title = "Select XML File";
                        exportSaveFileDialog.Filter = "Xml File(*.xml)|*.xml";

                        if (DialogResult.OK == exportSaveFileDialog.ShowDialog())
                        {
                            string fullFileName = exportSaveFileDialog.FileName;
                            xmlDoc.Save(fullFileName);
                            MessageBox.Show("Gosses exported successfully", "Export to Xml", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                    MessageBox.Show("No goses to Export !", "Export to Xml", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
