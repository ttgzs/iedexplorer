using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEC61850.Client;
using IEC61850.Common;
using IEC61850.Server;
using System.Threading;

namespace IEDExplorer
{
    class SCLServer
    {
        public static void TestClient()
        {
            IedConnection con = new IedConnection();
            try
            {
                System.Windows.Forms.MessageBox.Show("Server Start");
                con.Connect("localhost", 102);

                List<string> serverDirectory = con.GetServerDirectory(false);

                foreach (string entry in serverDirectory)
                {
                    System.Windows.Forms.MessageBox.Show("LD: " + entry);
                }
                con.Abort();
            }
            catch (IedConnectionException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            con.Dispose();
        }

        public static void TestServer()
        {
            IedModel model = new IedModel("bubak");

            LogicalDevice ldevice1 = new LogicalDevice("strasidlo", model);

            LogicalNode lln0 = new LogicalNode("LLN0", ldevice1);

            DataObject lln0_mod = CDCFactory.CDC_ENG("Mod", lln0, CDCOptions.NONE);
            DataObject lln0_health = CDCFactory.CDC_ENG("Health", lln0, CDCOptions.NONE);

            SettingGroupControlBlock sgcb = new SettingGroupControlBlock(lln0, 1, 1);

            /* Add a temperature sensor LN */
            LogicalNode ttmp1 = new LogicalNode("TTMP1", ldevice1);
            DataObject ttmp1_tmpsv = CDCFactory.CDC_SAV("TmpSv", ttmp1, 0, false);

            DataAttribute temperatureValue = ttmp1_tmpsv.GetChild_DataAttribute("instMag.f");
            DataAttribute temperatureTimestamp = ttmp1_tmpsv.GetChild_DataAttribute("t");

            IEC61850.Server.DataSet dataSet = new IEC61850.Server.DataSet("events", lln0);
            DataSetEntry dse = new DataSetEntry(dataSet, "TTMP1$MX$TmpSv$instMag$f", -1, null);

            IEC61850.Common.ReportOptions rptOptions = IEC61850.Common.ReportOptions.SEQ_NUM | IEC61850.Common.ReportOptions.TIME_STAMP | IEC61850.Common.ReportOptions.REASON_FOR_INCLUSION;

            IEC61850.Server.ReportControlBlock rcb1 = new IEC61850.Server.ReportControlBlock("events01", lln0, "events01", false, null, 1, IEC61850.Common.TriggerOptions.DATA_CHANGED, rptOptions, 50, 0);
            IEC61850.Server.ReportControlBlock rcb2 = new IEC61850.Server.ReportControlBlock("events02", lln0, "events02", true, null, 1, IEC61850.Common.TriggerOptions.DATA_CHANGED | IEC61850.Common.TriggerOptions.GI, rptOptions, 50, 0);
            
            IedServer server = new IedServer(model);

            server.Start(102);
            bool running = true;

            /* run until Ctrl-C is pressed */
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                running = false;
            };

            Console.WriteLine("Server is running...");

            float val = 0.0f;

            while (running)
            {
                server.LockDataModel();
                temperatureValue.MmsValue.SetFloat(val);
                temperatureTimestamp.MmsValue.SetUtcTimeMs(Util.GetTimeInMs());
                server.UnlockDataModel();

                val += 0.1f;
                
                Thread.Sleep(100);
            }
            Console.WriteLine("Ctrl-C pressed, ending ...");
            Thread.Sleep(1000);

        }
    }
}
