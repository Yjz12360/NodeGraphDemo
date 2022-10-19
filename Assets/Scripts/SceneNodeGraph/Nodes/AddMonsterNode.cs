using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class AddMonsterNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.AddMonster; }

        public string sRefreshId;
        public int nConfigId;
        public float nPosX;
        public float nPosY;
        public float nPosZ;
    }

}
