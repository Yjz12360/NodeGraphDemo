using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SceneNodeGraph
{
    public enum NodeAttrType
    {
        VecPos = 1,
    }
    public class NodeInputAttribute : Attribute
    {
        public NodeAttrType attrType;
        public NodeInputAttribute(NodeAttrType attrType)
        {
            this.attrType = attrType;
        }
    }

    public class NodeOutputAttribute : Attribute
    {
        public NodeAttrType attrType;
        public NodeOutputAttribute(NodeAttrType attrType)
        {
            this.attrType = attrType;
        }
    }

}