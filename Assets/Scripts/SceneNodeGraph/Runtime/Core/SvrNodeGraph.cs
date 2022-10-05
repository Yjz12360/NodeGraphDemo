using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace SceneNodeGraph
{

    public class SvrNodeGraph
    {
        public int nNodeGraphId;
        public NodeGraphData nodeGraphData;

        NodeGraphState nCurrState = NodeGraphState.Pending;
        public Dictionary<string, SvrRuntimeNode> tRuntimeNodeMap = new Dictionary<string, SvrRuntimeNode>();
        public List<string> tRunningNodes = new List<string>();


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
                    if (!RegRuntimeNodeTypes.tSvrTypes.ContainsKey(nodeType))
                        continue;
                    Type runtimeNodeType = RegRuntimeNodeTypes.tSvrTypes[nodeType];
                    SvrRuntimeNode nodeInstance = (SvrRuntimeNode)Activator.CreateInstance(runtimeNodeType);
                    nodeInstance.nodeGraph = this;
                    nodeInstance.baseNodeData = pair.Value;
                    nodeInstance.sNodeId = pair.Key;
                    tRuntimeNodeMap[pair.Key] = nodeInstance;
                }
            }

            tRunningNodes.Clear();
            nCurrState = NodeGraphState.Running;
            TriggerNode(sStartNodeId);
        }

        public void TriggerNode(string sNodeId)
        {
            if (nodeGraphData == null)
            {
                Debug.LogError($"TriggerNode Error: nodeGraphData not exist.");
                FinishNode(sNodeId);
                return;
            }
            if (!tRuntimeNodeMap.ContainsKey(sNodeId))
            {
                //Debug.LogError($"TriggerNode error: sNodeId {sNodeId} not exist.");
                FinishNode(sNodeId);
                return;
            }
            tRunningNodes.Add(sNodeId);
            if (tRuntimeNodeMap.ContainsKey(sNodeId))
            {
                SvrRuntimeNode node = tRuntimeNodeMap[sNodeId];
                node.StartNode();
            }
            else
            {
                FinishNode(sNodeId);
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
            foreach (string sNodeId in tRunningNodes)
            {
                if (tRuntimeNodeMap.ContainsKey(sNodeId))
                {
                    SvrRuntimeNode node = tRuntimeNodeMap[sNodeId];
                    node.UpdateNode(nDeltaTime);
                }
            }
            if (tRunningNodes.Count == 0)
            {
                tRunningNodes.Clear();
                nCurrState = NodeGraphState.Finished;
            }
        }

        public void FinishNode(string sNodeId, int nPath = 1, bool bSync = false)
        {
            if (nodeGraphData == null)
            {
                Debug.LogError($"UpdateNodes Error: nodeGraphData not exist.");
                return;
            }
            Dictionary<string, BaseNodeData> tNodeMap = nodeGraphData.tNodeMap;
            if (nCurrState != NodeGraphState.Running) return;
            tRunningNodes.Remove(sNodeId);
            foreach (NodeTransitionData transition in nodeGraphData.tTransitions)
            {
                if (transition.sFromNodeId == sNodeId && transition.nPath == nPath)
                {
                    if (tNodeMap.ContainsKey(transition.sToNodeId))
                    {
                        TriggerNode(transition.sToNodeId);
                    }
                }
            }
            if (bSync)
                Messager.S2CFinishNode(nNodeGraphId, sNodeId, nPath);
        }

        public bool IsFinished()
        {
            return nCurrState == NodeGraphState.Finished;
        }

        public void OnC2SFinishNode(string sNodeId, int nPath)
        {
            if (nCurrState != NodeGraphState.Running) return;
            if (!tRuntimeNodeMap.ContainsKey(sNodeId)) return;
            if (!tRunningNodes.Contains(sNodeId)) return;
            SvrRuntimeNode node = tRuntimeNodeMap[sNodeId];
            node.FinishNode(nPath);
        }
    }
}
