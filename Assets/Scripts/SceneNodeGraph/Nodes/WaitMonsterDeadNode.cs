using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class WaitMonsterDeadNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.WaitMonsterDead; }

        public string sRefreshId;
    }

    //public class SvrWaitMonsterDeadNode : SvrRuntimeNode
    //{
    //    public override bool SyncFinishNode() { return true; }
    //    public WaitMonsterDeadNodeData NodeData { get { return (WaitMonsterDeadNodeData)baseNodeData; } }

    //    public override void StartNode()
    //    {
    //        Game.SvrObjectData objectData = nodeGraph.game.GetMonsterByStaticId(NodeData.nStaticId);
    //        if (objectData == null)
    //            FinishNode();
    //    }

    //    public override void OnMonsterDead(int nObjectId)
    //    {
    //        Game.SvrObjectData objectData = nodeGraph.game.GetObject(nObjectId);
    //        if (objectData == null) return;
    //        if (NodeData.nStaticId <= 0) return;
    //        if (objectData.nStaticId == NodeData.nStaticId)
    //            FinishNode();
    //    }

    //}
}
