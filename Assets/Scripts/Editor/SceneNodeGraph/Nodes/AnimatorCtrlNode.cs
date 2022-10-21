using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class AnimatorCtrlNode : BaseNode
    {
        public override NodeType GetNodeType() { return NodeType.AnimatorCtrl; }

        public string sRefreshId;
        public string sSetTrigger;
    }

}
