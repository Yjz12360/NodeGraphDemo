using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public class BaseNodeData
    {
        public string sNodeId;

        public virtual NodeType GetNodeType()
        {
            return 0;
        }

        public Type Type
        {
            get
            {
                return GetType(GetNodeType());
            }
        }

        public static Type GetType(NodeType nodeType)
        {
            string subTypeName = $"{nodeType}NodeData";
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
