using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class ActiveAINodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.ActiveAI; }

        public string sRefreshId;
        public string sGroupId;
        public bool bActive;
    }

}
