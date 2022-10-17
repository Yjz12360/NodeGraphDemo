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

    //public class CltMoveNode : CltRuntimeNode
    //{
    //    public MoveNodeData NodeData { get { return (MoveNodeData)baseNodeData; } }


    //    public override void StartNode()
    //    {

    //    }

    //    public override void UpdateNode(float nDeltaTime)
    //    {

    //    }
    //}

    //public class SvrMoveNode : SvrRuntimeNode
    //{
    //    public override bool SyncFinishNode() { return true; }

    //    public override void StartNode()
    //    {

    //    }

    //    public override void UpdateNode(float nDeltaTime)
    //    {

    //    }
    //}
}

