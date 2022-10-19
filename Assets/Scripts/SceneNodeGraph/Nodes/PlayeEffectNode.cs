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

}
