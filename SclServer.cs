using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IEC61850.Client;
using IEC61850.Common;

namespace IEDExplorer
{
    class SclServer
    {
        public static void TestClient()
        {
            IedConnection con = new IedConnection ();
            try
            {
                MessageBox.Show("Server Start");
                con.Connect("localhost", 102);

                List<string> serverDirectory = con.GetServerDirectory(false);

                foreach (string entry in serverDirectory)
                {
                    MessageBox.Show("LD: " + entry);
                }
                con.Abort();
            }
            catch (IedConnectionException e)
            {
                MessageBox.Show(e.Message);
            }
			con.Dispose ();
        }
    }
}
