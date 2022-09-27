using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public enum NodeType
    {
        Print = 1,
        Move = 2,
    }

    public class BaseNodeData
    {

        public static readonly int nNodeType = 0;
        public string sNodeId;
    }

}
