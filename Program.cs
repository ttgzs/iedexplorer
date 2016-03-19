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
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace IEDExplorer
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string resPrefix = "IEDExplorer.Embed.";
            string filename1 = "iec61850dotnet.dll";
            string filename2 = "iec61850.dll";
            EmbeddedAssembly.Load(resPrefix + filename1, filename1);
            EmbeddedAssembly.Load(resPrefix + filename2, filename2);

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Env env = new Env();
            Application.Run(new Views.MainWindow(env));

            string path2 = null;
            try
            {
                path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename2);
                if (File.Exists(path2))
                    File.Delete(path2);
            }
            catch
            {
                try
                {
                    UnloadImportedDll(path2);
                    File.Delete(path2);
                }
                catch { }
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        public static void UnloadImportedDll(string DllPath)
        {
            foreach (System.Diagnostics.ProcessModule mod in System.Diagnostics.Process.GetCurrentProcess().Modules)
            {
                if (mod.FileName.ToUpper() == DllPath.ToUpper())
                {
                    FreeLibrary(mod.BaseAddress);
                }
            }
        }
    }
}
