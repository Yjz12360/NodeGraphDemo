using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{

    public class NodeTransitionData
    {
        public string sFromNodeId;
        public string sToNodeId;
        public int nPath = 1;
    }
    public class NodeGraphData
    {
        public Dictionary<string, BaseNodeData> tNodeMap = new Dictionary<string, BaseNodeData>();
        public List<NodeTransitionData> tTransitions = new List<NodeTransitionData>();
        public string sStartNodeId;

        public void AddNode(BaseNodeData node)
        {
            if (node == null)
            {
                Debug.LogError($"AddNode Error: node is null.");
                return;
            }
            if (tNodeMap.ContainsKey(node.sNodeId))
            {
                Debug.LogError($"AddNode Error: node's id {node.sNodeId} already exists in graph.");
                return;
            }
            tNodeMap[node.sNodeId] = node;
        }

        public void SetStartNode(BaseNodeData node)
        {
            if (!tNodeMap.ContainsKey(node.sNodeId))
            {
                Debug.LogError($"SetStartNode Error: sNodeId {node.sNodeId} not exist.");
                return;
            }
            sStartNodeId = node.sNodeId;
        }

        public BaseNodeData GetNodeData(string sNodeId)
        {
            if (!tNodeMap.ContainsKey(sNodeId))
                return null;
            return tNodeMap[sNodeId];
        }

        public void AddTransition(string sFromNodeId, string sToNodeId, int nPath = 1)
        {
            if (!tNodeMap.ContainsKey(sFromNodeId))
            {
                Debug.LogError($"AddTransition Error: sFromNodeId {sFromNodeId} not exist.");
                return;
            }
            if (!tNodeMap.ContainsKey(sToNodeId))
            {
                Debug.LogError($"AddTransition Error: sToNodeId {sToNodeId} not exist.");
                return;
            }
            NodeTransitionData nodeTransition = new NodeTransitionData();
            nodeTransition.sFromNodeId = sFromNodeId;
            nodeTransition.sToNodeId = sToNodeId;
            nodeTransition.nPath = nPath;
            tTransitions.Add(nodeTransition);
        }

    }
}