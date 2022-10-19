using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace SceneNodeGraph
{
    public class ActiveComponentNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.ActiveComponent; }

        public string sSourcePath;
        public string sComponentName;
        public bool bActive;
    }

}
