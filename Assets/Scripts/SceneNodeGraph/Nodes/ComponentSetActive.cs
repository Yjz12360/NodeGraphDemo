using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace SceneNodeGraph
{
    public class ComponentSetActiveNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.ComponentSetActive; }

        public string sSourcePath;
        public string sComponentName;
        public bool bActive;
    }

    public class CltComponentSetActiveNode : CltRuntimeNode
    {
        public ComponentSetActiveNodeData NodeData { get { return (ComponentSetActiveNodeData)baseNodeData; } }

        public override void StartNode()
        {
            GameObject sourceObject = GameObject.Find(NodeData.sSourcePath);
            if(sourceObject != null)
            {
                Component component = sourceObject.GetComponent(NodeData.sComponentName);
                if (component != null && component.GetType().IsSubclassOf(typeof(Behaviour)))
                    ((Behaviour)component).enabled = NodeData.bActive;
            }

            FinishNode();
        }
    }

}
