using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace IEDExplorer
{
    public class Iec61850Controller
    {
        Iec61850State iecs;
        Iec61850Model model;
        long m_ctlNum = 0;
        System.Threading.Timer delayTimer;

        public delegate void NewReportDelegate(string rptdVarQualityLog, string rptdVarTimestampLog, string rptdVarPathLogstring, string rptdVarDescriptionLog, string rptdVarValueLog);
        public event NewReportDelegate NewReport;

        public Iec61850Controller(Iec61850State iecs, Iec61850Model model)
        {
            this.iecs = iecs;
            this.model = model;
        }

        public void FireNewReport(string rptdVarQualityLog, string rptdVarTimestampLog, string rptdVarPathLog, string rptdVarDescriptionLog, string rptdVarValueLog)
        {
            if (NewReport != null)
                NewReport(rptdVarQualityLog, rptdVarTimestampLog, rptdVarPathLog, rptdVarDescriptionLog, rptdVarValueLog);
        }

        public void DeleteNVL(NodeVL nvl)
        {
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = nvl;
            iecs.Send(ndarr, nvl.CommAddress, ActionRequested.DeleteNVL);
        }

        public void GetFileList(NodeBase nfi)
        {
            CommAddress ad = new CommAddress();
            ad.Variable = "/";  // for the case of reading root
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = nfi;
            /* if (!(nfi is NodeFile))
            {
                NodeData nd = new NodeData("x");
                nd.DataType = scsm_MMS_TypeEnum.visible_string;
                nd.DataValue = "/";
                EditValue ev = new EditValue(nd);
                DialogResult r = ev.ShowDialog();
                if (r == DialogResult.OK)
                {
                    ad.Variable = nd.StringValue;
                }
            } */
            iecs.Send(ndarr, ad, ActionRequested.GetDirectory);
        }

        public void GetFile(NodeFile nfi)
        {
            CommAddress ad = new CommAddress();
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = nfi;

            if((nfi is NodeFile)) {
              NodeData nd = new NodeData("x");
              nd.DataType = scsm_MMS_TypeEnum.visible_string;
              nd.DataValue = nfi.Name;
              EditValue ev = new EditValue(nd);
              System.Windows.Forms.DialogResult r = ev.ShowDialog();
              if(r == System.Windows.Forms.DialogResult.OK) {
                ad.Variable = nd.StringValue;
                nfi.NameSet4Test(ad.Variable);
              }
            }
            nfi.Reset();
            iecs.Send(ndarr, ad, ActionRequested.OpenFile);
        }

        public void FileDelete(NodeFile nfi)
        {
             CommAddress ad = new CommAddress();
             NodeBase[] ndarr = new NodeBase[1];
             ndarr[0] = nfi;
             //nfi.NameSet4Test("anyfile.icd");
             nfi.Reset();
             iecs.Send(ndarr, ad, ActionRequested.FileDelete);
        }

        public void DefineNVL(NodeVL nvl)
        {
            List<NodeBase> ndar = new List<NodeBase>();
            foreach (NodeBase n in nvl.GetChildNodes())
            {
                ndar.Add(n);
            }
            iecs.Send(ndar.ToArray(), nvl.CommAddress, ActionRequested.DefineNVL);
        }

        public CommandParams PrepareSendCommand(NodeBase data)
        {
            if (data != null)
            {
                NodeData d = (NodeData)data.Parent;
                if (d != null)
                {
                    NodeBase b;//, c;
                    CommandParams cPar = new CommandParams();
                    cPar.CommType = CommandType.SingleCommand;
                    if ((b = d.FindChildNode("ctlVal")) != null)
                    {
                        cPar.DataType = ((NodeData)b).DataType;
                        cPar.Address = b.IecAddress;
                        cPar.ctlVal = ((NodeData)b).DataValue;
                    }
                    cPar.T = DateTime.MinValue;
                    cPar.interlockCheck = true;
                    cPar.synchroCheck = true;
                    cPar.orCat = OrCat.STATION_CONTROL;
                    cPar.orIdent = "IEDEXPLORER";
                    cPar.CommandFlowFlag = CommandCtrlModel.Unknown;
                    b = data;
                    List<string> path = new List<string>();
                    do
                    {
                        b = b.Parent;
                        path.Add(b.Name);
                    } while (!(b is NodeFC));
                    path[0] = "ctlModel";
                    path[path.Count - 1] = "CF";
                    b = b.Parent;
                    for (int i = path.Count - 1; i >= 0; i--)
                    {
                        if ((b = b.FindChildNode(path[i])) == null)
                            break;
                    }
                    if (b != null)
                        if (b is NodeData)
                            cPar.CommandFlowFlag = (CommandCtrlModel)((long)((b as NodeData).DataValue));
                    cPar.SBOrun = false;
                    cPar.SBOdiffTime = false;
                    cPar.SBOtimeout = 100;
                    return cPar;
                }
                else
                    Logger.getLogger().LogError("Basic structure for a command not found at " + data.IecAddress + "!");
            }
            return null;
        }

        public void SendCommand(NodeBase data, CommandParams cPar, ActionRequested how)
        {
            if (data != null)
            {
                Logger.getLogger().LogInfo("Sending command " + data.IecAddress);
                NodeData d = (NodeData)data.Parent;
                if (d != null)
                {
                    NodeBase b, c;

                    List<NodeData> ndar = new List<NodeData>();
                    //char *nameo[] = {"$Oper$ctlVal", "$Oper$origin$orCat", "$Oper$origin$orIdent", "$Oper$ctlNum", "$Oper$T", "$Oper$Test", "$Oper$Check"};
                    if ((b = d.FindChildNode("ctlVal")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = cPar.ctlVal;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("origin")) != null)
                    {
                        if (how == ActionRequested.WriteAsStructure)
                        {
                            NodeData n = new NodeData(b.Name);
                            n.DataType = scsm_MMS_TypeEnum.structure;
                            n.DataValue = 2;
                            ndar.Add(n);
                            if ((c = b.FindChildNode("orCat")) != null)
                            {
                                NodeData n2 = new NodeData(b.Name + "$" + c.Name);
                                n2.DataType = ((NodeData)c).DataType;
                                n2.DataValue = (long)cPar.orCat;
                                n.AddChildNode(n2);
                            }
                            if ((c = b.FindChildNode("orIdent")) != null)
                            {
                                NodeData n2 = new NodeData(b.Name + "$" + c.Name);
                                n2.DataType = ((NodeData)c).DataType;
                                byte[] bytes = new byte[cPar.orIdent.Length];
                                int tmp1, tmp2; bool tmp3;
                                Encoder ascii = (new ASCIIEncoding()).GetEncoder();
                                ascii.Convert(cPar.orIdent.ToCharArray(), 0, cPar.orIdent.Length, bytes, 0, cPar.orIdent.Length, true, out tmp1, out tmp2, out tmp3);
                                n2.DataValue = bytes;
                                n.AddChildNode(n2);
                            }
                        }
                        else
                        {
                            if ((c = b.FindChildNode("orCat")) != null)
                            {
                                NodeData n = new NodeData(b.Name + "$" + c.Name);
                                n.DataType = ((NodeData)c).DataType;
                                n.DataValue = (long)cPar.orCat;
                                ndar.Add(n);
                            }
                            if ((c = b.FindChildNode("orIdent")) != null)
                            {
                                NodeData n = new NodeData(b.Name + "$" + c.Name);
                                n.DataType = ((NodeData)c).DataType;
                                byte[] bytes = new byte[cPar.orIdent.Length];
                                int tmp1, tmp2; bool tmp3;
                                Encoder ascii = (new ASCIIEncoding()).GetEncoder();
                                ascii.Convert(cPar.orIdent.ToCharArray(), 0, cPar.orIdent.Length, bytes, 0, cPar.orIdent.Length, true, out tmp1, out tmp2, out tmp3);
                                n.DataValue = bytes;
                                ndar.Add(n);
                            }
                        }
                    }
                    if ((b = d.FindChildNode("ctlNum")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        if (d.Name == "SBO" || d.Name == "SBOw")
                            n.DataValue = m_ctlNum;
                        else
                            n.DataValue = m_ctlNum++;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("T")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        byte[] btm = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
                        n.DataValue = btm;

                        if (cPar.T != DateTime.MinValue)
                        {
                            if (d.Name == "Oper" && cPar.SBOdiffTime && cPar.SBOrun)
                                cPar.T.AddMilliseconds(cPar.SBOtimeout);
                            Scsm_MMS.ConvertToUtcTime(cPar.T, btm);
                        }
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("Test")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = cPar.Test;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("Check")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0x40 };
                        n.DataParam = ((NodeData)b).DataParam;
                        ndar.Add(n);
                    }
                    iecs.Send(ndar.ToArray(), d.CommAddress, how);
                }
                else
                    Logger.getLogger().LogError("Basic structure for a command not found at " + data.IecAddress + "!");
            }
        }

        public static NodeData PrepareWriteData(NodeData data)
        {
            NodeData nd = new NodeData(data.Name);
            nd.DataType = data.DataType;
            nd.DataValue = data.DataValue;
            nd.DataParam = data.DataParam;
            nd.Parent = data.Parent;
            return nd;
        }

        public void WriteData(NodeData data, bool reRead)
        {
            if (data != null && data.DataValue != null)
            {
                NodeData[] ndarr = new NodeData[1];
                ndarr[0] = data;
                iecs.Send(ndarr, data.Parent.CommAddress, ActionRequested.Write);

                if (reRead)
                {
                    delayTimer = new System.Threading.Timer(obj =>
                    {
                        ReadData(data);
                    }, null, 1000, System.Threading.Timeout.Infinite);
                }
            }
            else
                Logger.getLogger().LogError("Iec61850Controller.WriteData: null data (-Value), cannot send");
        }

        public void WriteRcb(RcbActivateParams rpar, bool reRead)
        {
            iecs.Send(rpar.getWriteArray(), rpar.self.CommAddress, ActionRequested.Write);

            if (reRead)
            {
                delayTimer = new System.Threading.Timer(obj =>
                {
                    ReadData(rpar.self);
                }, null, 1000, System.Threading.Timeout.Infinite);
            }
        }

        public void ReadData(NodeBase data)
        {
            NodeBase[] ndarr = new NodeBase[1];
            ndarr[0] = data;
            iecs.Send(ndarr, data.CommAddress, ActionRequested.Read);
        }

        public void ActivateNVL(NodeVL vl)
        {
            //Logger.getLogger().LogError("Function not active, try to configure an RCB!");
            //return;

            NodeBase ur = null;
            Iec61850State iecs = vl.GetIecs();
            bool retry;
            if (iecs != null)
            {
                do
                {
                    ur = (NodeData)iecs.DataModel.ied.FindNodeByValue(scsm_MMS_TypeEnum.visible_string, vl.IecAddress, ref ur);
                    if (ur == null || ur.Parent == null)
                    {
                        Logger.getLogger().LogError("Suitable URCB not found, list cannot be activated!");
                        return;
                    }
                    retry = !ur.Parent.Name.ToLower().Contains("rcb");
                    vl.urcb = (NodeData)ur;
                    NodeData d = (NodeData)vl.urcb.Parent;
                    NodeData b;
                    if ((b = (NodeData)d.FindChildNode("Resv")) != null)
                    {
                        // Resv is always a boolean
                        // If true then the rcb is occupied and we need to find another one
                        if ((bool)b.DataValue) retry = true;
                    }
                } while (retry);

                if (vl.urcb != null)
                {
                    NodeData d = (NodeData)vl.urcb.Parent;
                    List<NodeData> ndar = new List<NodeData>();
                    NodeBase b;
                    if ((b = d.FindChildNode("Resv")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = true;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("DatSet")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = ((NodeData)b).DataValue;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("OptFlds")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0x7c, 0x00 };
                        n.DataParam = 6;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("TrgOps")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = new byte[] { 0x74 };
                        n.DataParam = 2;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("RptEna")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = true;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("GI")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = true;
                        ndar.Add(n);
                    }
                    iecs.Send(ndar.ToArray(), d.CommAddress, ActionRequested.Write);
                    vl.Activated = true;
                }
            }
            else
                Logger.getLogger().LogError("Basic structure not found!");
        }

        public void DeactivateNVL(NodeVL vl)
        {
            Iec61850State iecs = vl.GetIecs();
            if (iecs != null)
            {
                if (vl.urcb != null)
                {
                    NodeData d = (NodeData)vl.urcb.Parent;
                    List<NodeData> ndar = new List<NodeData>();
                    NodeBase b;
                    if ((b = d.FindChildNode("RptEna")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = false;
                        ndar.Add(n);
                    }
                    if ((b = d.FindChildNode("GI")) != null)
                    {
                        NodeData n = new NodeData(b.Name);
                        n.DataType = ((NodeData)b).DataType;
                        n.DataValue = false;
                        ndar.Add(n);
                    }
                    iecs.Send(ndar.ToArray(), d.CommAddress, ActionRequested.Write);
                    vl.Activated = false;
                    vl.urcb = null;
                }
            }
            else
                Logger.getLogger().LogError("Basic structure not found!");
        }

    }
}
