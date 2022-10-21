using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class WaitMonsterDeadNode : BaseNode
    {
        public override NodeType GetNodeType() { return NodeType.WaitMonsterDead; }

        public string sRefreshId;
    }
}
