using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class NodeGraphClient : MonoBehaviour
    {
        public Dictionary<int, CltNodeGraph> nodeGraphs = new Dictionary<int, CltNodeGraph>();

        public void ActivateNodeGraph(int nNodeGraphId, string sConfigFile)
        {
            if(nodeGraphs.ContainsKey(nNodeGraphId))
            {
                Debug.LogError($"ActivateNodeGraph error: nNodeGraphId {nNodeGraphId} already exists.");
                return;
            }
            CltNodeGraph nodeGraph = CltNodeGraphManager.AddNodeGraph(nNodeGraphId, sConfigFile);
            nodeGraph.StartGraph();
            nodeGraphs[nNodeGraphId] = nodeGraph;
        }

        public void FinishNode(int nNodeGraphId, string sNodeId, int nPath)
        {
            if (!nodeGraphs.ContainsKey(nNodeGraphId))
                return;
            nodeGraphs[nNodeGraphId].OnSyncFinishNode(sNodeId, nPath);
        }

        public void FinishNodeGraph(int nNodeGraphId)
        {
            if (!nodeGraphs.ContainsKey(nNodeGraphId))
                return;
            nodeGraphs[nNodeGraphId].OnSyncFinishGraph();
        }

        private void Update()
        {
            List<int> tRemoveList = new List<int>();
            foreach(var pair in nodeGraphs)
            {
                CltNodeGraph nodeGraph = pair.Value;
                nodeGraph.UpdateNodes(Time.deltaTime);
                if(nodeGraph.IsFinished())
                {
                    tRemoveList.Add(pair.Key);
                }
            }
            foreach(int nNodeGraphId in tRemoveList)
            {
                nodeGraphs.Remove(nNodeGraphId);
                CltNodeGraphManager.RemoveNodeGraph(nNodeGraphId);
            }
        }

    }
}

