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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IEDExplorer
{
    class NodeFile: NodeBase
    {
        public NodeFile(string Name, bool isDir)
            : base(Name)
        {
            this.isDir = isDir;
            this.files = new List<NodeFile>();
            frsmId = 0;
        }
        public event EventHandler DirectoryUpdated;

        public bool isDir { get; private set; }

        private bool fileReady;
        public bool FileReady {
            get
            {
                return fileReady;
            }
            set
            {
                fileReady = value;
                if (DirectoryUpdated != null)
                    DirectoryUpdated(this, new EventArgs());
            } 
        }

        public int frsmId { get; set; }

        public int ReportedSize { get; set; }

        public List<NodeFile> files { get; private set; }

        private byte[] data;
        public byte[] Data { get { return data; } set { data = value; } }

        public int AppendData(byte[] chunk)
        {
            if (data == null)
                data = chunk;
            else
            {
                int origLen = data.Length;
                Array.Resize<byte>(ref data, origLen + chunk.Length);
                Array.Copy(chunk, 0, data, origLen, chunk.Length);
            }
            return data.Length;
        }

        public string FullName
        {
            get
            {
                string fn = "";
                //NodeFile nf = this;
                NodeBase nb = this;
                do
                {
                    fn = "/" + nb.Name + fn;
                    nb = nb.Parent;
                }
                while (nb is NodeFile);
                return fn;
            }
        }

        public void SaveFile(string FileName)
        {
            File.Delete(FileName);
            FileStream outst = File.Create(FileName);
            outst.Write(data, 0, data.Length);
            outst.Close();
        }
    }
}
