using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public static class SvrNodeGraphManager
    {
        private static Dictionary<int, SvrNodeGraph> nodeGraphs = new Dictionary<int, SvrNodeGraph>();

        private static int nCurrId = 0;

        public static int Register(SvrNodeGraph nodeGraph)
        {
            nCurrId++;
            nodeGraphs[nCurrId] = nodeGraph;
            return nCurrId;
        }

        public static SvrNodeGraph GetNodeGraph(int nId)
        {
            if (!nodeGraphs.ContainsKey(nId))
                return null;
            return nodeGraphs[nId];
        }

    }
}

