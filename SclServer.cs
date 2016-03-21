/*
 *  Copyright (C) 2016 Pavel Charvat
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
 * 
 *  SCLParser.cs was created by Joel Kaiser as an add-in to IEDExplorer.
 *  This class parses a SCL-type XML file to create a logical tree similar
 *  to that which Pavel creates from communication with an actual device.
 */

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
        private Thread _workerThread;
        private bool _run;
        //private IsoConnectionParameters isoParameters;
        private Env _env;
        WaitHandle[] _waitHandles = new WaitHandle[1];
        //public Iec61850State iecs;
        Logger logger = Logger.getLogger();
        public Iec61850Model sclModel;
        int tcpPort = 102;

        public SCLServer(Env env)
        {
            _env = env;
            _waitHandles[0] = new ManualResetEvent(false);   // end thread
        }

        public int StartTestServer()
        {
            return StartInternal(new ParameterizedThreadStart(TestServer));
        }

        public int Start(Iec61850Model model, int port)
        {
            sclModel = model;
            tcpPort = port;
            return StartInternal(new ParameterizedThreadStart(WorkerThreadProc));
        }

        int StartInternal(ParameterizedThreadStart ts)
        {
            //// Run Thread
            if (!_run && _workerThread == null)
            {
                _run = true;
                _workerThread = new Thread(ts);
                logger.LogInfo(String.Format("Starting SCL Server"));
                _workerThread.Start(this);
            }
            else
                System.Windows.Forms.MessageBox.Show("Cannot start, communication already running!", "Info");
            return 0;
        }

        public void Stop()
        {
            if (_workerThread != null)
            {
                (_waitHandles[0] as ManualResetEvent).Set();
                _workerThread = null;
                logger.LogInfo(String.Format("Stopping SCL Server"));
            }
        }

        private void WorkerThreadProc(object obj)
        {
            SCLServer self = (SCLServer)obj;

            IedModel model = makeIecModel();

            IedServer server = new IedServer(model);
            server.Start(tcpPort);

            logger.LogInfo(String.Format("SCL Server " + sclModel.iec.Name + " Started at port " + tcpPort.ToString() + "!!!"));

            while (self._run)
            {
                int waitres = WaitHandle.WaitAny(_waitHandles, 500);
                switch (waitres)
                {
                    case 0:     // endthread
                        self._run = false;
                        server.Stop();
                        resetModelObjects(sclModel.iec);
                        sclModel = null;
                        break;
                    case WaitHandle.WaitTimeout:
                        break;
                }
            }
        }

        IedModel makeIecModel()
        {
            IedModel model = new IedModel(sclModel.iec.Name);
            sclModel.iec.SCLServerModelObject = model;

            foreach (NodeLD ld in sclModel.iec.GetChildNodes())
            {
                createModelDevices(ld, model);
            }
            return model;
        }

        void createModelDevices(NodeLD ld, IedModel model)
        {
            LogicalDevice ldevice = new LogicalDevice(ld.Name, model);
            ld.SCLServerModelObject = ldevice;

            foreach (NodeLN ln in ld.GetChildNodes())
            {
                createModelNodes(ln, ldevice);
            }
        }

        void createModelNodes(NodeLN ln, LogicalDevice ldevice)
        {
            LogicalNode lnode = new LogicalNode(ln.Name, ldevice);
            ln.SCLServerModelObject = lnode;

            foreach (NodeBase nb in ln.GetChildNodes())
            {
                if (nb is NodeDO)
                {
                    createData(nb, lnode);
                }
                else if (nb is NodeRCB)
                {
                }
                else if (nb is NodeVL)
                {
                }
            }
        }

        void createData(NodeBase dt, ModelNode mn)
        {
            ModelNode newmn = null;
            if (dt is NodeDO)
                newmn = new DataObject(dt.Name, mn, 0);
            else if (dt is NodeData)
            {   // dt id NodeDA
                NodeData da = (NodeData)dt;
                FunctionalConstraint fc = DataAttribute.fcFromString(da.FCDesc);
                IEC61850.Server.DataAttributeType t = DataAttribute.typeFromString(da.BType);
                newmn = new DataAttribute(dt.Name, mn, t, fc, 0, 0, 0);
            }
            dt.SCLServerModelObject = newmn;
            foreach (NodeBase nb in dt.GetChildNodes())
            {
                // Recursion
                createData(nb, newmn);
            }
        }

        void resetModelObjects(NodeBase nb)
        {
            nb.SCLServerModelObject = null;
            foreach (NodeBase n in nb.GetChildNodes())
            {
                // Recursion
                resetModelObjects(n);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // Test implementations
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

        private void TestServer(object obj)
        {
            SCLServer self = (SCLServer)obj;
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

            server.Start(tcpPort);

            logger.LogInfo(String.Format("SCL Server Started at port 102!!!"));

            float val = 0.0f;

            while (_run)
            {
                server.LockDataModel();
                temperatureValue.MmsValue.SetFloat(val);
                temperatureTimestamp.MmsValue.SetUtcTimeMs(Util.GetTimeInMs());
                server.UnlockDataModel();

                val += 0.1f;

                int waitres = WaitHandle.WaitAny(_waitHandles, 500);
                switch (waitres)
                {
                    case 0:     // endthread
                        self._run = false;
                        break;
                    case WaitHandle.WaitTimeout:
                        break;
                }
            }
            logger.LogInfo(String.Format("SCL Server Finished!!!"));
        }
    }
}
