using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public class BaseNode
    {
        public string sNodeId;

        private static Dictionary<Type, NodeType> typesMap = new Dictionary<Type, NodeType>();
        public virtual NodeType GetNodeType()
        {
            Type currType = GetType();
            if(!typesMap.ContainsKey(currType))
            {
                foreach(NodeType nodeType in Enum.GetValues(typeof(NodeType)))
                {
                    Type type = BaseNode.GetType(nodeType);
                    typesMap[type] = nodeType;
                }
            }
            return typesMap[currType];
        }

        public static Type GetType(NodeType nodeType)
        {
            string subTypeName = $"{nodeType}Node";
            Type type = typeof(BaseNode);
            foreach (Type subType in type.Assembly.GetTypes())
                if (subType.IsSubclassOf(type) && subType.Name.Equals(subTypeName))
                    return subType;

            return null;
        }
    }

}
