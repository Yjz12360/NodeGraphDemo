using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class StartNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.Start; }
    }
}
