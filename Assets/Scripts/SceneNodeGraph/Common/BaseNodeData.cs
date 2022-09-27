using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public enum NodeType
    {
        Print = 1,
        Move = 2,
    }

    public class BaseNodeData
    {
        public string sNodeId;

        public static Type GetSubHitDataType(NodeType nodeType)
        {
            string subTypeName = $"{nodeType}Node";
            Type type = typeof(BaseNodeData);
            foreach (Type subType in type.Assembly.GetTypes())
                if (subType.IsSubclassOf(type) && subType.Name.Equals(subTypeName))
                    return subType;

            return null;
        }

        public static NodeType GetNodeType(Type subType)
        {
            string enumName = subType.Name.Replace("Node", "");
            return (NodeType)Enum.Parse(typeof(NodeType), enumName);
        }
    }

}
