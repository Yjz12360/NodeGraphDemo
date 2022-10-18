using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class RefreshMonsterGroupNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.RefreshMonsterGroup; }

        public int nRefreshId;
    }



    //public class CltStartNode : CltRuntimeNode
    //{
    //    public override void StartNode()
    //    {
    //        FinishNode();
    //    }
    //}

    //public class SvrStartNode : SvrRuntimeNode
    //{
    //    public override void StartNode()
    //    {
    //        FinishNode();
    //    }
    //}
}
