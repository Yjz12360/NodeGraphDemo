using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public class BaseNode
    {
        public int nNodeId;
        public virtual Type GetPathType() { return typeof(SequencePath); }

        private static Dictionary<Type, NodeType> typesEnumMap = new Dictionary<Type, NodeType>();
        public virtual NodeType GetNodeType()
        {
            Type currType = GetType();
            if(!typesEnumMap.ContainsKey(currType))
            {
                foreach(NodeType nodeType in Enum.GetValues(typeof(NodeType)))
                {
                    Type type = BaseNode.GetType(nodeType);
                    typesEnumMap[type] = nodeType;
                }
            }
            return typesEnumMap[currType];
        }

        private static Dictionary<NodeType, Type> enumTypesMap = new Dictionary<NodeType, Type>();
        public static Type GetType(NodeType nodeType)
        {
            if (!enumTypesMap.ContainsKey(nodeType))
            {
                string subTypeName = $"{nodeType}Node";
                Type type = typeof(BaseNode);
                foreach (Type subType in type.Assembly.GetTypes())
                    if (subType.IsSubclassOf(type) && subType.Name.Equals(subTypeName))
                        enumTypesMap[nodeType] = subType;
            }
            return enumTypesMap[nodeType];
        }
    }

}
