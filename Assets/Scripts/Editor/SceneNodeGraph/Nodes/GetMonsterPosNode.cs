using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class GetMonsterPosNode : BaseNode
    {
        public int nRefreshId;

        [NodeOutput(NodeAttrType.VecPos)]
        public Vector3 tPos;
    }
}
