using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class PrintNode : BaseNode
    {
        public string sContext;
        public LogType nLogType = LogType.Log;
        public override void StartNode()
        {
            Debug.LogFormat(nLogType, LogOption.None, null, sContext);
            FinishNode();
        }
    }
}
