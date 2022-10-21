using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class PrintNode : BaseNode
    {
        public override NodeType GetNodeType() { return NodeType.Print; }

        public string sContext;
        public bool bIsError;
    }
}

