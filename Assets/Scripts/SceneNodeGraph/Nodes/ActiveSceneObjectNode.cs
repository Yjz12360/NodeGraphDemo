using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace SceneNodeGraph
{
    public class ActiveSceneObjectNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.ActiveSceneObject; }

        public string sSourcePath;
        public bool bActive;
    }

}
