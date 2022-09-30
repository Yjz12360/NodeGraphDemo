﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public static class SvrNodeGraphManager
    {
        private static Dictionary<int, SvrNodeGraph> nodeGraphs = new Dictionary<int, SvrNodeGraph>();

        private static int nCurrId = 0;

        public static SvrNodeGraph AddNodeGraph(string sConfigFile)
        {
            SvrNodeGraph svrNodeGraph = new SvrNodeGraph();
            nCurrId++;
            svrNodeGraph.nNodeGraphId = nCurrId;
            svrNodeGraph.nodeGraphData = SceneNodeGraphSerializer.Load(sConfigFile);
            nodeGraphs[nCurrId] = svrNodeGraph;
            return svrNodeGraph;
            
        }

        public static void RemoveNodeGraph(int nId)
        {
            if (!nodeGraphs.ContainsKey(nId))
                return;
            nodeGraphs.Remove(nId);
        }

        public static SvrNodeGraph GetNodeGraph(int nId)
        {
            if (!nodeGraphs.ContainsKey(nId))
                return null;
            return nodeGraphs[nId];
        }

    }
}
