using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class PrintNodeData : BaseNodeData
    {
        public static new NodeType nNodeType = NodeType.Print;

        public string sContext;
        public bool bIsError;
    }

    public class CltPrintNode : CltRuntimeNode
    {
        public PrintNodeData NodeData { get { return (PrintNodeData)baseNodeData; } }

        public override void StartNode()
        {
            string sContext = NodeData.sContext;
            if (NodeData.bIsError)
                Debug.LogError(sContext);
            else
                Debug.Log(sContext);
            FinishNode();
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
