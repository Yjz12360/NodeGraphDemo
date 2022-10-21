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
            string subTypeName = $"{nodeType}Node";
            Type type = typeof(BaseNode);
            foreach (Type subType in type.Assembly.GetTypes())
                if (subType.IsSubclassOf(type) && subType.Name.Equals(subTypeName))
                    return subType;

            return null;
        }
    }

}
