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
        public Dictionary<string, BaseNode> tNodeMap = new Dictionary<string, BaseNode>();
        public List<NodeTransitionData> tTransitions = new List<NodeTransitionData>();
        public string sStartNodeId;

        public void AddNode(BaseNode node)
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

        public void RemoveNode(string sNodeId)
        {
            HashSet<string> tValidNodes = new HashSet<string>();
            Queue<string> tNodeQueue = new Queue<string>();
            tNodeQueue.Enqueue(sStartNodeId);
            while(tNodeQueue.Count > 0)
            {
                string sCurrNode = tNodeQueue.Dequeue();
                if (sCurrNode == sNodeId) continue;
                tValidNodes.Add(sCurrNode);
                foreach(NodeTransitionData transition in tTransitions)
                {
                    if(transition.sFromNodeId == sCurrNode)
                    {
                        tNodeQueue.Enqueue(transition.sToNodeId);
                    }
                }
            }
            HashSet<string> tRemoveNodes = new HashSet<string>();
            foreach (var pair in tNodeMap)
                if (!tValidNodes.Contains(pair.Key))
                    tRemoveNodes.Add(pair.Key);
            foreach (string sRemoveNode in tRemoveNodes)
                tNodeMap.Remove(sRemoveNode);
            for(int i = tTransitions.Count - 1; i >= 0; --i)
            {
                NodeTransitionData transition = tTransitions[i];
                if (tRemoveNodes.Contains(transition.sFromNodeId) || tRemoveNodes.Contains(transition.sToNodeId))
                    tTransitions.RemoveAt(i);
            }
        }

        public void SetStartNode(BaseNode node)
        {
            if (!tNodeMap.ContainsKey(node.sNodeId))
            {
                Debug.LogError($"SetStartNode Error: sNodeId {node.sNodeId} not exist.");
                return;
            }
            sStartNodeId = node.sNodeId;
        }

        public BaseNode GetNodeData(string sNodeId)
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