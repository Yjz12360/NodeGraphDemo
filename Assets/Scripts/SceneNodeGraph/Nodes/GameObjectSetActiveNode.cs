using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace SceneNodeGraph
{
    public class GameObjectSetActiveNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.GameObjectSetActive; }

        public string sSourcePath;
        public bool bActive;
    }

    //public class CltGameObjectSetActiveNode : CltRuntimeNode
    //{
    //    public GameObjectSetActiveNodeData NodeData { get { return (GameObjectSetActiveNodeData)baseNodeData; } }

    //    public override void StartNode()
    //    {
    //        GameObject sourceObject = GameObject.Find(NodeData.sSourcePath);
    //        if(sourceObject != null)
    //        {
    //            sourceObject.SetActive(NodeData.bActive);
    //        }

    //        FinishNode();
    //    }
    //}

}
