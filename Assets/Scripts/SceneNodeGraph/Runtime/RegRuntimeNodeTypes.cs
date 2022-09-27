using System;
using System.Collections.Generic;


namespace SceneNodeGraph
{
    public class RegRuntimeNodeTypes
    {
        public static Dictionary<NodeType, Type> tCltTypes = new Dictionary<NodeType, Type> 
        {
            {NodeType.Print, typeof(CltPrintNode) },
            {NodeType.Move, typeof(CltMoveNode) },
        };

        public static Dictionary<NodeType, Type> tSvrTypes = new Dictionary<NodeType, Type>
        {
            {NodeType.Move, typeof(SvrMoveNode) },
        };
    }
}
