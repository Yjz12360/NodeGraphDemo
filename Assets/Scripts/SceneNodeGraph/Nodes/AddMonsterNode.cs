using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class AddMonsterNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.AddMonster; }

        public int nConfigId;
        public float nPosX;
        public float nPosY;
        public float nPosZ;
    }

    //public class SvrAddMonsterNode : SvrRuntimeNode
    //{
    //    public override bool SyncFinishNode() { return true; }
    //    public AddMonsterNodeData NodeData { get { return (AddMonsterNodeData)baseNodeData; } }

    //    public override void StartNode()
    //    {
    //        float nPosX = NodeData.nPosX;
    //        float nPosY = NodeData.nPosY;
    //        float nPosZ = NodeData.nPosZ;
    //        nodeGraph.game.AddMonster(NodeData.nConfigId, new Vector3(nPosX, nPosY, nPosZ));
    //        FinishNode();
    //    }
    //}
}
