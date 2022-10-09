using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class ExplodeNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.Explode; }

        public int nEffectId;
        public float nPosX;
        public float nPosY;
        public float nPosZ;
        public float nAffectRadius;
        public float nDisplayScale;
        //public string sContext;
        //public bool bIsError;
    }

    public class CltExplodeNode : CltRuntimeNode
    {
        public ExplodeNodeData NodeData { get { return (ExplodeNodeData)baseNodeData; } }

        public override void StartNode()
        {
            Vector3 center = new Vector3(NodeData.nPosX, NodeData.nPosY, NodeData.nPosZ);
            nodeGraph.game.PlayExplosion(center, NodeData.nEffectId, NodeData.nDisplayScale);
            FinishNode();
            //string sContext = NodeData.sContext;
            //if (NodeData.bIsError)
            //    Debug.LogError(sContext);
            //else
            //    Debug.Log(sContext);
            //FinishNode();
        }
    }

    public class SvrExplodeNode : SvrRuntimeNode
    {
        public ExplodeNodeData NodeData { get { return (ExplodeNodeData)baseNodeData; } }

        public override void StartNode()
        {
            Vector3 center = new Vector3(NodeData.nPosX, NodeData.nPosY, NodeData.nPosZ);
            List<int> affectedRoles = nodeGraph.game.GetRangeRoles(center, NodeData.nAffectRadius);
            foreach (int nObjectId in affectedRoles)
                nodeGraph.game.MonsterDead(nObjectId);
            FinishNode();
            //base.StartNode();
        }

    }
}

