using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public enum NodeGraphState
    {
        Pending = 1,
        Running = 2,
        Finished = 3,
    }

    public class RuntimeNodeGraph
    {
        public NodeGraphData nodeGraphData;
        NodeGraphState nCurrState = NodeGraphState.Pending;
        public List<string> tRunningNodes = new List<string>();

        public void StartGraph()
        {
            if(nodeGraphData == null)
            {
                Debug.LogError($"StartGraph Error: nodeGraphData not exist.");
                return;
            }
            Dictionary<string, BaseNodeData> tNodeMap = nodeGraphData.tNodeMap;
            string sStartNodeId = nodeGraphData.sStartNodeId;
            if (!tNodeMap.ContainsKey(sStartNodeId))
            {
                Debug.LogError($"StartGraph Error: sStartNodeId {nodeGraphData.sStartNodeId} not exist.");
                return;
            }
            tRunningNodes.Clear();
            nCurrState = NodeGraphState.Running;
            //BaseNodeData startNode = tNodeMap[sStartNodeId];
            //startNode.StartNode();
            //if (!startNode.bFinished)
            //    tRunningNodes.Add(sStartNodeId);
        }

        //public void UpdateNodes(float nDeltaTime)
        //{
        //    if (nCurrState != NodeGraphState.Running) return;
        //    for (int i = tRunningNodes.Count - 1; i >= 0; --i)
        //    {
        //        string sNodeId = tRunningNodes[i];
        //        if (tNodeMap.ContainsKey(sNodeId))
        //        {
        //            BaseNode node = tNodeMap[sNodeId];
        //            node.UpdateNode(nDeltaTime);
        //        }
        //    }
        //}
        //public void FinishNode(string sNodeId, int nPath)
        //{
        //    if (nCurrState != NodeGraphState.Running) return;
        //    tRunningNodes.Remove(sNodeId);
        //    foreach (NodeTransition transition in tTransitions)
        //    {
        //        if (transition.sFromNodeId == sNodeId && transition.nPath == nPath)
        //        {
        //            if (tNodeMap.ContainsKey(transition.sToNodeId))
        //            {
        //                BaseNode node = tNodeMap[transition.sToNodeId];
        //                node.StartNode();
        //                if (!node.bFinished)
        //                    tRunningNodes.Add(transition.sToNodeId);
        //            }
        //        }
        //    }
        //    if (tRunningNodes.Count == 0)
        //        nCurrState = NodeGraphState.Finished;
        //}

        //public bool IsFinished()
        //{
        //    return nCurrState == NodeGraphState.Finished;
        //}
    }
}
