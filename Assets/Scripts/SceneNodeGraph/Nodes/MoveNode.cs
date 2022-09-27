using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class MoveNodeData : BaseNodeData
    {
        public static new NodeType nNodeType = NodeType.Move;

        public int nObjectId;
        public Vector3 vTargetPos;
        public float nSpeed;
    }

    public class CltMoveNode : CltRuntimeNode
    {
        public MoveNodeData NodeData { get { return (MoveNodeData)baseNodeData; } }


        public override void StartNode()
        {

        }

        public override void UpdateNode(float nDeltaTime)
        {

        }
    }

    public class SvrMoveNode : SvrRuntimeNode
    {
        public static new bool bSyncFinishNode = true;

        public override void StartNode()
        {

        }

        public override void UpdateNode(float nDeltaTime)
        {

        }
    }
}

//namespace SceneNodeGraph
//{
//    public class PrintNode : BaseNode
//    {
//        public string sContext;
//        public LogType nLogType = LogType.Log;
//        public override void StartNode()
//        {
//            Debug.LogFormat(nLogType, LogOption.None, null, sContext);
//            FinishNode();
//        }
//    }
//}
