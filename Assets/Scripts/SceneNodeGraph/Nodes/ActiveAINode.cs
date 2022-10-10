using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class ActiveAINodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.ActiveAI; }

        public int nStaticId;
        public bool bActive;
    }

    public class SvrActiveAINode : SvrRuntimeNode
    {
        public override bool SyncFinishNode() { return true; }
        public ActiveAINodeData NodeData { get { return (ActiveAINodeData)baseNodeData; } }

        public override void StartNode()
        {
            Game.SvrObjectData svrObjectData = nodeGraph.game.GetMonsterByStaticId(NodeData.nStaticId);
            if(svrObjectData != null)
            {
                nodeGraph.game.ActiveAI(svrObjectData.nGameObjectId, NodeData.bActive);
            }
            FinishNode();
        }
    }
}
