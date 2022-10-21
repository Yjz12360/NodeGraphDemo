using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class ExplodeNode : BaseNode
    {
        public override NodeType GetNodeType() { return NodeType.Explode; }

        public int nEffectId;
        public float nPosX;
        public float nPosY;
        public float nPosZ;
        public float nAffectRadius;
        public float nDisplayScale;
        public int nDamage;
    }

}

