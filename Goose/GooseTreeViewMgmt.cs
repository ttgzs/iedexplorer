using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer
{
    class GooseTreeViewMgmt
    {
        int recursiveReadData(NodeBase nd, GOOSE_ASN1_Model.Data t, NodeBase ndcn, int id, DateTime captureTime)
        {
            int _id = id;

            if (t == null)
                return -1;

            if (t.Array != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.array;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Array_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.array;
                    (nb as NodeGData).DataValue = t.Array;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Binarytime != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.array;
                    (nd as NodeGData).DataValue = t.Binarytime;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Binarytime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.binary_time;
                    (nb as NodeGData).DataValue = t.Binarytime;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Bitstring != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.bit_string;
                    (nd as NodeGData).DataValue = t.Bitstring.Value;
                    (nd as NodeGData).DataParam = t.Bitstring.TrailBitsCnt;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Bitstring_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.bit_string;
                    (nb as NodeGData).DataValue = t.Bitstring.Value;
                    (nb as NodeGData).DataParam = t.Bitstring.TrailBitsCnt;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;

            }
            else if (t.isBooleanSelected())
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.boolean;
                    (nd as NodeGData).DataValue = t.Boolean;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Boolean_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.boolean;
                    (nb as NodeGData).DataValue = t.Boolean;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.BooleanArray != null)
            {
                return 0;
            }
            else if (t.Floatingpoint != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.floating_point;
                    (nd as NodeGData).DataValue = t.Floatingpoint;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Floatingpoint_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.floating_point;
                    (nb as NodeGData).DataValue = t.Floatingpoint;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Generalizedtime != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.generalized_time;
                    (nd as NodeGData).DataValue = t.Generalizedtime;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Generalizedtime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.generalized_time;
                    (nb as NodeGData).DataValue = t.Generalizedtime;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.isIntegerSelected())
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.integer;
                    (nd as NodeGData).DataValue = t.Integer;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Integer_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.integer;
                    (nb as NodeGData).DataValue = t.Integer;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.MMSString != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.mMSString;
                    (nd as NodeGData).DataValue = t.MMSString;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("MMSString_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.mMSString;
                    (nb as NodeGData).DataValue = t.MMSString;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Octetstring != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.octet_string;
                    (nd as NodeGData).DataValue = t.Octetstring;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Octetstring_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.octet_string;
                    (nb as NodeGData).DataValue = t.Octetstring;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Structure != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.structure;

                    NodeBase[] nd1 = nd.GetChildNodes();

                    int i = 0;
                    int j = 0;

                    foreach (GOOSE_ASN1_Model.Data data in t.Structure.Value)
                        j = recursiveReadData(nd1[i++], data, null, j, captureTime);

                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Structure_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.structure;
                    NodeBase nb1 = ndcn.AddChildNode(nb);
                    int i = 0;
                    foreach (GOOSE_ASN1_Model.Data data in t.Structure.Value)
                        i = recursiveReadData(null, data, nb1, i, captureTime);

                    return ++_id;
                }
                else
                    return -1;

            }
            else if (t.isUnsignedSelected())
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.unsigned;
                    (nd as NodeGData).DataValue = t.Unsigned;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Unsigned_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.unsigned;
                    (nb as NodeGData).DataValue = t.Unsigned;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Utctime != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.utc_time;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Utctime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.utc_time;
                    ndcn.AddChildNode(nb);
                    return ++_id;
                }
                else
                    return -1;
            }
            else if (t.Visiblestring != null)
            {
                if (nd != null)
                {
                    (nd as NodeGData).CaptureTime = captureTime;
                    (nd as NodeGData).DataType = scsm_MMS_TypeEnum.visible_string;
                    (nd as NodeGData).DataValue = t.Visiblestring;
                    return 0;
                }
                else if (ndcn != null)
                {
                    NodeBase nb = new NodeGData("Utctime_" + _id.ToString());
                    (nb as NodeGData).CaptureTime = captureTime;
                    (nb as NodeGData).DataType = scsm_MMS_TypeEnum.visible_string;
                    (nb as NodeGData).DataValue = t.Visiblestring;
                    return ++_id;
                }
                else
                    return -1;
            }
            else
                return -1;
        }

        void makeTree_listNode(NodeBase nb, TreeNode tn, MyTreeView tv)
        {
            foreach (NodeBase b in nb.GetChildNodes())
            {
                TreeNode tn2 = (tn != null) ? tn.Nodes.Add(b.Name) : tv.Nodes.Add(b.Name);
                tn2.Tag = b;
                b.Tag = tn2;
                tn2.ImageIndex = 1;
                tn2.SelectedImageIndex = 1;

                NodeBase[] bcn = b.GetChildNodes();

                if (bcn.Length == 0)
                {
                    tn2.ImageIndex = 2;
                    tn2.SelectedImageIndex = 2;
                }

                foreach (NodeBase b2 in bcn)
                {
                    TreeNode tn3 = tn2.Nodes.Add(b2.CommAddress.Variable);
                    tn3.Tag = b2;
                    tn3.ImageIndex = 1;
                    tn3.SelectedImageIndex = 1;

                    NodeBase[] b2cn = b2.GetChildNodes();

                    if (b2cn.Length > 0)
                        makeTree_listNode(b2, tn3, null);
                    else
                    {
                        tn3.ImageIndex = 2;
                        tn3.SelectedImageIndex = 2;
                    }
                }
            }
        }
    }
}
