using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public static class CltNodeGraphManager
    {
        private static Dictionary<int, CltNodeGraph> nodeGraphs = new Dictionary<int, CltNodeGraph>();
        private static int nCurrId = 0;
        public static int Register(CltNodeGraph nodeGraph)
        {
            nCurrId++;
            nodeGraphs[nCurrId] = nodeGraph;
            return nCurrId;
        }

        public static CltNodeGraph GetNodeGraph(int nId)
        {
            if (!nodeGraphs.ContainsKey(nId))
                return null;
            return nodeGraphs[nId];
        }

    }
}

