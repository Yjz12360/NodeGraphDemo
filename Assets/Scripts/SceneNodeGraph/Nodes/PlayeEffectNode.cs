using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class PlayEffectNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.PlayEffect; }

        public int nEffectId;
        public float nPosX;
        public float nPosY;
        public float nPosZ;
        public float nDisplayScale;

    }

    public class CltPlayEffectNode : CltRuntimeNode
    {
        public PlayEffectNodeData NodeData { get { return (PlayEffectNodeData)baseNodeData; } }

        public override void StartNode()
        {
            Vector3 center = new Vector3(NodeData.nPosX, NodeData.nPosY, NodeData.nPosZ);
            nodeGraph.game.PlayEffect(center, NodeData.nEffectId, NodeData.nDisplayScale);
            //nodeGraph.game.PlayExplosion(center, NodeData.nEffectId, NodeData.nDisplayScale);
            FinishNode();
        }
    }

}
