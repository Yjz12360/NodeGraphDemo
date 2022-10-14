using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class WaitEnterTriggerNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.WaitEnterTrigger; }

        public int nTriggerId;
    }

}
