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
            (lists as NodeIed).iecs = iecs;
            (files as NodeIed).iecs = iecs;
            (urcbs as NodeIed).iecs = iecs;
        }
    }
}
