using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class HasMonsterNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.HasMonster; }
    }

    //public class SvrHasMonsterNode : SvrRuntimeNode
    //{
    //    public override bool SyncFinishNode() { return true; }
    //    public override void StartNode()
    //    {
    //        if (nodeGraph.game.HasMonster())
    //            FinishNode(1);
    //        else
    //            FinishNode(2);
    //    }
    //}
}
