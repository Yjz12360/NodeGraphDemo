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

    public class CltDelayNode : CltRuntimeNode
    {
        public DelayNodeData NodeData { get { return (DelayNodeData)baseNodeData; } }

        public override void StartNode()
        {
            float nDelayTime = NodeData.nDelayTime;
            Task t = Task.Run(async delegate
            {
                await Task.Delay((int)(nDelayTime * 1000));
                FinishNode();
            });
        }
    }

    public class SvrDelayNode : SvrRuntimeNode
    {
        public DelayNodeData NodeData { get { return (DelayNodeData)baseNodeData; } }

        public override void StartNode()
        {
            float nDelayTime = NodeData.nDelayTime;
            Task t = Task.Run(async delegate
            {
                await Task.Delay((int)(nDelayTime * 1000));
                FinishNode();
            });
        }
    }
}
