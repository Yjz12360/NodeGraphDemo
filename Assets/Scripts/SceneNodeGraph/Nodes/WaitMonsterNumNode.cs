using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public enum CompareType
    {
        LessThan = 1,
        MoreThan = 2,
        Equal = 3,
    }
    public class WaitMonsterNumNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.WaitMonsterNum; }

        public CompareType nCompareType = CompareType.LessThan;
        public int nNum = 0;
        public string sMonsterGroupId = "";
    }

    //public class SvrWaitMonsterNumNode : SvrRuntimeNode
    //{
    //    public override bool SyncFinishNode() { return true; }
    //    public WaitMonsterNumNodeData NodeData { get { return (WaitMonsterNumNodeData)baseNodeData; } }

    //    public override void OnMonsterNumChange(int nNum)
    //    {
    //        if ((NodeData.nCompareType == CompareType.LessThan && nNum < NodeData.nNum) ||
    //            (NodeData.nCompareType == CompareType.MoreThan && nNum > NodeData.nNum) ||
    //            (NodeData.nCompareType == CompareType.Equal && nNum == NodeData.nNum))
    //            FinishNode();
    //    }

    //}
}
