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
                _workerThread.Name = "SCL_Server_thread";
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

            logger.LogInfo(String.Format("SCL Server " + sclModel.iec.Name + " - Creating model. It takes a while..."));
            IedModel model = makeIecModel();

            IedServer server = new IedServer(model);
            server.Start(tcpPort);
            //Thread.Sleep(100);
            if (!server.isRunning())
            {
                logger.LogError(String.Format("SCL Server " + sclModel.iec.Name + " Failed at port " + tcpPort.ToString() + "!!!"));
                self._run = false;
            }
            else
                logger.LogInfo(String.Format("SCL Server " + sclModel.iec.Name + " Started at port " + tcpPort.ToString() + "!!!"));
            
            while (self._run)
            {
                int waitres = WaitHandle.WaitAny(_waitHandles, 500);
                switch (waitres)
                {
                    case 0:     // endthread
                        self._run = false;
                        break;
                    case WaitHandle.WaitTimeout:
                        if (!server.isRunning()) //logger.LogInfo("Server not running???");
                            self._run = false;
                        break;
                }
            }
            server.Stop();
            resetModelObjects(sclModel.iec);
            server.Dispose();
            server = null;
            model.Dispose();
            model = null;
            sclModel = null;
            logger.LogInfo(String.Format("SCL Server Stopped"));
        }

        IedModel makeIecModel()
        {
            IedModel model = new IedModel(sclModel.iec.Name);
            sclModel.iec.SCLServerModelObject = model;

            // Data
            foreach (NodeLD ld in sclModel.iec.GetChildNodes())
            {
                createModelDevices(ld, model);
            }

            // VLs
            foreach (NodeLD ld in sclModel.iec.GetChildNodes())
            {
                foreach (NodeLN ln in ld.GetChildNodes())
                {
                    foreach (NodeBase vl in ln.GetChildNodes())
                    {
                        if (vl is NodeVL)
                        {
                            createVL((NodeVL)vl, (LogicalNode)ln.SCLServerModelObject);
                        }
                    }
                }
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
                    IEC61850.Common.ReportOptions rptOptions = (IEC61850.Common.ReportOptions)(nb.FindChildNode("OptFlds") as NodeData).DataValue;
                    IEC61850.Common.TriggerOptions trgOptions = (IEC61850.Common.TriggerOptions)(nb.FindChildNode("TrgOps") as NodeData).DataValue;
                    string rptId = (string)(nb.FindChildNode("RptID") as NodeData).DataValue;
                    string datSet = (string)(nb.FindChildNode("DatSet") as NodeData).DataValue;
                    uint confRev = (uint)(nb.FindChildNode("ConfRev") as NodeData).DataValue;
                    uint bufTm = (uint)(nb.FindChildNode("BufTm") as NodeData).DataValue;
                    uint intgPd = (uint)(nb.FindChildNode("IntgPd") as NodeData).DataValue;

                    datSet = datSet == "" ? null : datSet;

                    IEC61850.Server.ReportControlBlock rcb =
                        new IEC61850.Server.ReportControlBlock(nb.Name, lnode, rptId, (nb as NodeRCB).isBuffered, datSet, confRev, trgOptions, rptOptions, bufTm, intgPd);
                    nb.SCLServerModelObject = rcb;
                }
            }
        }

        void createVL(NodeVL vl, LogicalNode ln)
        {
            IEC61850.Server.DataSet dataSet = new IEC61850.Server.DataSet(vl.Name, ln);
            vl.SCLServerModelObject = dataSet;
            foreach (NodeVLM vlm in vl.GetChildNodes())
            {
                DataSetEntry dse = new DataSetEntry(dataSet, vlm.Name, -1, null);
                vlm.SCLServerModelObject = dse;
            }
        }

        void createData(NodeBase dt, ModelNode mn)
        {
            ModelNode newmn = null;
            NodeBase iter = null;
            bool isArr = false;
            if (dt is NodeDO)
            {
                NodeDO dO = (dt as NodeDO);
                isArr = dO.SCL_ArraySize > 0;
                newmn  = new DataObject(dt.Name, mn, dO.SCL_ArraySize);
            }
            else if (dt is NodeData)
            {   // dt id NodeDA
                NodeData dA = (NodeData)dt;
                isArr = dA.SCL_ArraySize > 0;
                FunctionalConstraint fc = DataAttribute.fcFromString(dA.SCL_FCDesc);
                IEC61850.Server.DataAttributeType t = DataAttribute.typeFromSCLString(dA.SCL_BType);
                newmn = new DataAttribute(dt.Name, mn, t, fc, dA.SCL_TrgOps, dA.SCL_ArraySize, 0);
            }
            dt.SCLServerModelObject = newmn;
            if (isArr)
                iter = dt.GetChildNode(0);
            else
                iter = dt;
            foreach (NodeBase nb in iter.GetChildNodes())
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
