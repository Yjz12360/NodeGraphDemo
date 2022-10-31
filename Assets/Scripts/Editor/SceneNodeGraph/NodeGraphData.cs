using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{

    public class NodeInputData
    {
        public int nodeId = 0;
        public string attrName;
    }

    public class NodeGraphData
    {
        public Dictionary<int, BaseNode> nodeMap = new Dictionary<int, BaseNode>();
        public Dictionary<int, Dictionary<int, List<int>>> transitions = new Dictionary<int, Dictionary<int, List<int>>>();
        public Dictionary<int, Dictionary<string, NodeInputData>> inputData = new Dictionary<int, Dictionary<string, NodeInputData>>();
        public int startNodeId;

        public void AddNode(BaseNode node)
        {
            if (node == null)
            {
                Debug.LogError($"AddNode Error: node is null.");
                return;
            }
            if (nodeMap.ContainsKey(node.nNodeId))
            {
                Debug.LogError($"AddNode Error: node's id {node.nNodeId} already exists in graph.");
                return;
            }
            nodeMap[node.nNodeId] = node;
        }

        public void RemoveNode(int nodeId)
        {
            HashSet<int> validNodes = new HashSet<int>();
            Queue<int> nodeQueue = new Queue<int>();
            nodeQueue.Enqueue(startNodeId);
            while(nodeQueue.Count > 0)
            {
                int currNode = nodeQueue.Dequeue();
                if (currNode == nodeId) continue;
                validNodes.Add(currNode);
                if (transitions.ContainsKey(currNode))
                {
                    foreach(var nodeTransitions in transitions[currNode])
                    {
                        foreach (int toNodeId in nodeTransitions.Value)
                        {
                            nodeQueue.Enqueue(toNodeId);
                        }
                    }
                }
            }
            HashSet<int> removeNodes = new HashSet<int>();
            foreach (var pair in nodeMap)
                if (!validNodes.Contains(pair.Key))
                    removeNodes.Add(pair.Key);
            foreach (int removeNode in removeNodes)
            {
                nodeMap.Remove(removeNode);
                if(transitions.ContainsKey(removeNode))
                {
                    transitions.Remove(removeNode);
                }
                if(inputData.ContainsKey(removeNode))
                {
                    inputData.Remove(removeNode);
                }
            }
            foreach (int validNode in validNodes)
            {
                if (transitions.ContainsKey(validNode))
                {
                    var transition = transitions[validNode];
                    foreach(List<int> toNodeList in transition.Values)
                    {
                        if (toNodeList.Contains(nodeId))
                        {
                            toNodeList.Remove(nodeId);
                        }
                    }
                }
                if(inputData.ContainsKey(validNode))
                {
                    var attrInputData = inputData[validNode];
                    List<string> toDel = new List<string>();
                    foreach(var pair in attrInputData)
                    {
                        string toNodeAttr = pair.Key;
                        int fromNodeId = pair.Value.nodeId;
                        if (!nodeMap.ContainsKey(fromNodeId))
                            toDel.Add(toNodeAttr);
                    }
                    foreach (string attr in toDel)
                        inputData[validNode].Remove(attr);
                }
            }
        }

        public void SetStartNode(BaseNode node)
        {
            if (!nodeMap.ContainsKey(node.nNodeId))
            {
                Debug.LogError($"SetStartNode Error: nodeId {node.nNodeId} not exist.");
                return;
            }
            startNodeId = node.nNodeId;
        }

        public BaseNode GetNodeData(int nodeId)
        {
            if (!nodeMap.ContainsKey(nodeId))
                return null;
            return nodeMap[nodeId];
        }

        public void AddTransition(int fromNodeId, int toNodeId, int path = 1)
        {
            if (!nodeMap.ContainsKey(fromNodeId))
            {
                Debug.LogError($"AddTransition Error: fromNodeId {fromNodeId} not exist.");
                return;
            }
            if (!nodeMap.ContainsKey(toNodeId))
            {
                Debug.LogError($"AddTransition Error: toNodeId {toNodeId} not exist.");
                return;
            }
            if (!transitions.ContainsKey(fromNodeId))
                transitions[fromNodeId] = new Dictionary<int, List<int>>();
            if (!transitions[fromNodeId].ContainsKey(path))
                transitions[fromNodeId][path] = new List<int>();
            transitions[fromNodeId][path].Add(toNodeId);
        }

    }
}