using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public static class CltNodeGraphManager
    {
        private static Dictionary<int, CltNodeGraph> nodeGraphs = new Dictionary<int, CltNodeGraph>();

        public static CltNodeGraph AddNodeGraph(int nNodeGraphId, string sConfigFile)
        {
            if(nodeGraphs.ContainsKey(nNodeGraphId))
            {
                Debug.LogError($"AddNodeGraph error: nNodeGraphId {nNodeGraphId} already exists.");
                return null;
            }
            CltNodeGraph cltNodeGraph = new CltNodeGraph();
            cltNodeGraph.nNodeGraphId = nNodeGraphId;
            cltNodeGraph.nodeGraphData = SceneNodeGraphSerializer.Load(sConfigFile);
            nodeGraphs[nNodeGraphId] = cltNodeGraph;
            return cltNodeGraph;
        }

        public static void RemoveNodeGraph(int nId)
        {
            if (!nodeGraphs.ContainsKey(nId))
                return;
            nodeGraphs.Remove(nId);
        }

        public static CltNodeGraph GetNodeGraph(int nId)
        {
            if (!nodeGraphs.ContainsKey(nId))
                return null;
            return nodeGraphs[nId];
        }

    }
}

