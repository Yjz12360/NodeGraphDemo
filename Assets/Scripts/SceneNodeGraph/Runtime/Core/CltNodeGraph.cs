using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public enum NodeGraphState
    {
        Pending = 1,
        Running = 2,
        Finished = 3,
    }

    public class CltNodeGraph
    {
        public int nNodeGraphId;
        public NodeGraphData nodeGraphData;
        public Game.CltGame game;

        NodeGraphState nCurrState = NodeGraphState.Pending;
        public Dictionary<string, CltRuntimeNode> tRuntimeNodeMap = new Dictionary<string, CltRuntimeNode>();
        public List<string> tRunningNodes = new List<string>();
        public List<string> tPendingNodes = new List<string>();
        public List<string> tRemoveNodes = new List<string>();
        public void StartGraph()
        {
            if (nodeGraphData == null)
            {
                Debug.LogError($"StartGraph Error: nodeGraphData not exist.");
                return;
            }
            string sStartNodeId = nodeGraphData.sStartNodeId;
            if (!nodeGraphData.tNodeMap.ContainsKey(sStartNodeId))
            {
                Debug.LogError($"StartGraph Error: sStartNodeId {sStartNodeId} not exist.");
                return;
            }

            if (tRuntimeNodeMap.Count == 0)
            {
                foreach (KeyValuePair<string, BaseNodeData> pair in nodeGraphData.tNodeMap)
                {
                    Type nodeDataType = pair.Value.GetType();
                    NodeType nodeType = BaseNodeData.GetNodeType(nodeDataType);
                    if (!RegRuntimeNodeTypes.tCltTypes.ContainsKey(nodeType))
                        continue;
                    Type runtimeNodeType = RegRuntimeNodeTypes.tCltTypes[nodeType];
                    CltRuntimeNode nodeInstance = (CltRuntimeNode)Activator.CreateInstance(runtimeNodeType);
                    nodeInstance.nodeGraph = this;
                    nodeInstance.baseNodeData = pair.Value;
                    nodeInstance.sNodeId = pair.Key;
                    tRuntimeNodeMap[pair.Key] = nodeInstance;
                }
            }

            tRunningNodes.Clear();
            tPendingNodes.Clear();
            nCurrState = NodeGraphState.Running;
            //TriggerNode(sStartNodeId);
            tPendingNodes.Add(sStartNodeId);
        }

        public void TriggerNode(string sNodeId)
        {
            if (nodeGraphData == null)
            {
                Debug.LogError($"TriggerNode Error: nodeGraphData not exist.");
                return;
            }
            tRunningNodes.Add(sNodeId);
            if(tRuntimeNodeMap.ContainsKey(sNodeId))
            {
                CltRuntimeNode node = tRuntimeNodeMap[sNodeId];
                node.StartNode();
            }
        }

        public void UpdateNodes(float nDeltaTime)
        {
            if (nodeGraphData == null)
            {
                Debug.LogError($"UpdateNodes Error: nodeGraphData not exist.");
                return;
            }
            if (nCurrState != NodeGraphState.Running) return;
            if(tPendingNodes.Count > 0)
            {
                List<string> tCloneNodes = new List<string>();
                tPendingNodes.ForEach(sNodeId => tCloneNodes.Add(sNodeId));
                tPendingNodes.Clear();
                foreach (string sNodeId in tCloneNodes)
                {
                    TriggerNode(sNodeId);
                }
            }
            foreach (string sNodeId in tRunningNodes)
            {
                if (tRuntimeNodeMap.ContainsKey(sNodeId))
                {
                    CltRuntimeNode node = tRuntimeNodeMap[sNodeId];
                    node.UpdateNode(nDeltaTime);
                }
            }
            if(tRemoveNodes.Count > 0)
            {
                foreach(string sNodeId in tRemoveNodes)
                {
                    tRunningNodes.Remove(sNodeId);
                }
                tRemoveNodes.Clear();
            }
            if (tRunningNodes.Count == 0 && tPendingNodes.Count == 0)
            {
                nCurrState = NodeGraphState.Finished;
            }
        }

        public void FinishNode(string sNodeId, int nPath)
        {
            if (nodeGraphData == null)
            {
                Debug.LogError($"UpdateNodes Error: nodeGraphData not exist.");
                return;
            }
            Dictionary<string, BaseNodeData> tNodeMap = nodeGraphData.tNodeMap;
            if (nCurrState != NodeGraphState.Running) return;
            //tRunningNodes.Remove(sNodeId);
            tRemoveNodes.Add(sNodeId);
            foreach (NodeTransitionData transition in nodeGraphData.tTransitions)
            {
                if (transition.sFromNodeId == sNodeId && transition.nPath == nPath)
                {
                    if (tNodeMap.ContainsKey(transition.sToNodeId))
                    {
                        tPendingNodes.Add(transition.sToNodeId);
                        //TriggerNode(transition.sToNodeId);
                    }
                }
            }
        }

        public bool IsFinished()
        {
            return nCurrState == NodeGraphState.Finished;
        }

        public void OnSyncFinishGraph()
        {
            tRunningNodes.Clear();
            nCurrState = NodeGraphState.Finished;
        }

        public void OnSyncFinishNode(string sNodeId, int nPath)
        {
            if (nCurrState != NodeGraphState.Running) return;
            if (tRuntimeNodeMap.ContainsKey(sNodeId))
            {
                CltRuntimeNode node = tRuntimeNodeMap[sNodeId];
                node.FinishNode(nPath);
            }
            else
            {
                FinishNode(sNodeId, nPath);
            }
        }
    }
}
