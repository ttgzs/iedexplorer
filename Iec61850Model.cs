using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    class Iec61850Model
    {
        /// <summary>
        /// Server data
        /// </summary>
        public NodeIed ied = new NodeIed("ied");
        /// <summary>
        /// Server data ordered by IEC61850 data model
        /// </summary>
        public NodeIed iec = new NodeIed("iec");
        /// <summary>
        /// Server named variable lists
        /// </summary>
        public NodeIed lists = new NodeIed("lists");
        /// <summary>
        /// Server RP blocks (reports)
        /// </summary>
        public NodeIed urcbs = new NodeIed("urcbs");
        /// <summary>
        /// Server BR blocks (reports)
        /// </summary>
        public NodeIed brcbs = new NodeIed("brcbs");
        /// <summary>
        /// Server files
        /// </summary>
        public NodeIed files = new NodeIed("files");
        /// Enum types
        /// </summary>
        public NodeIed enums = new NodeIed("enums");

        public Iec61850Model(Iec61850State iecs)
        {
            (ied as NodeIed).iecs = iecs;
            (iec as NodeIed).iecs = iecs;
            (lists as NodeIed).iecs = iecs;
            (files as NodeIed).iecs = iecs;
            (urcbs as NodeIed).iecs = iecs;
            (brcbs as NodeIed).iecs = iecs;
        }

        public void BuildIECModelFromMMSModel()
        {
            foreach (NodeLD ld in ied.GetChildNodes())      // LD level
            {
                NodeLD ild = new NodeLD(ld.Name);
                ild = (NodeLD)iec.AddChildNode(ild);
                foreach (NodeLN ln in ld.GetChildNodes())   // LN level
                {
                    NodeLN iln = new NodeLN(ln.Name);
                    iln = (NodeLN)ild.AddChildNode(iln);
                    foreach (NodeFC fc in ln.GetChildNodes())   // FC level - skipping
                    {
                        // keep knowing FC for DA
                        foreach (NodeDO dO in fc.GetChildNodes())   // DO level
                        {
                            NodeDO ido = new NodeDO(dO.Name);
                            ido = (NodeDO)iln.AddChildNode(ido);
                        }
                    }
                }
            }
        }
    }
}
