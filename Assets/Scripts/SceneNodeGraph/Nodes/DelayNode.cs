using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace SceneNodeGraph
{
    public class DelayNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.Delay; }

        public float nDelayTime;
    }

}
