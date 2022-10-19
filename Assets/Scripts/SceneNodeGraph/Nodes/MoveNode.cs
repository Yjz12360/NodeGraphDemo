using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class MoveNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.Move; }

        public int nObjectId;
        public Vector3 vTargetPos;
        public float nSpeed;
    }

}

