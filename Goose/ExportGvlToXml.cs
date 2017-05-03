using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using org.bn;
using GOOSE_ASN1_Model;
using System.Collections;

namespace IEDExplorer
{
    class ExportGvlToXml
    {
        public void Export(NodeGVL gvl)
        {
            try
            {
                XDocument xmlDoc = new XDocument(new XElement("DataSet"));

                if (gvl.GetChildNodes().Length == 0)
                {
                    MessageBox.Show("DataSet not exported. No data to export !", "Export to Xml", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (NodeGData ngd in gvl.GetChildNodes())
                    recursiveReadGvl(ngd, xmlDoc.Element("DataSet"));

                using (SaveFileDialog exportSaveFileDialog = new SaveFileDialog())
                {
                    exportSaveFileDialog.Title = "Select XML File";
                    exportSaveFileDialog.Filter = "Xml File(*.xml)|*.xml";

                    if (DialogResult.OK == exportSaveFileDialog.ShowDialog())
                    {
                        string fullFileName = exportSaveFileDialog.FileName;
                        xmlDoc.Save(fullFileName);
                        MessageBox.Show("DataSet exported successfully", "Export to Xml", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.getLogger().LogError("DataSet export Exception" + ex.Message);
            }
        }

        public void ExportGoose(Hashtable gp, NodeGVL gvl)
        {
            try
            {
                XDocument xmlDoc = new XDocument();

                XElement gooses = new XElement("Gooses");
                xmlDoc.Add(gooses);

                if ((gp.Count == 17) && (gvl.GetChildNodes().Length > 0))
                {
                    XElement goose = new XElement("Goose_0");
                    gooses.Add(goose);

                    XElement parameters = new XElement("Parameters");
                    goose.Add(parameters);

                    foreach (DictionaryEntry param in gp)
                        parameters.Add(new XElement(param.Key.ToString(), new XAttribute("Value", param.Value.ToString())));

                    XElement dataset = new XElement("DataSet");
                    goose.Add(dataset);

                    foreach (NodeGData ngd in gvl.GetChildNodes())
                        recursiveReadGvl(ngd, dataset);


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
                {
                    MessageBox.Show("Goose not exported. No data to export !", "Export to Xml", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.getLogger().LogError("Goose export Exception" + ex.Message);
            }
        }

        private void recursiveReadGvl(NodeGData ngd, XElement el)
        {
            el.Add(new XElement("Data", new XAttribute("Type", ngd.DataType.ToString()), new XAttribute("Value", ngd.StringValue), 
                   (ngd.Tag is Data) ? (((ngd.Tag as Data).Description != null && (ngd.Tag as Data).Description != "") ? new XAttribute("Desc", (ngd.Tag as Data).Description) : null) : 
                   ((ngd.Description != null && ngd.Description != "") ? new XAttribute("Desc", ngd.Description) : null)));

            XElement eldn = el.Descendants().Last();

            if (ngd.GetChildNodes().Length > 0)
            {
                foreach (NodeGData ngdcn in ngd.GetChildNodes())
                    recursiveReadGvl(ngdcn, eldn);
            }
        }
    }
}
