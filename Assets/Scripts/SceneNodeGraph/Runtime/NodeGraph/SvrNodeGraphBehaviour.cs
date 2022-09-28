using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class SvrNodeGraphBehaviour : MonoBehaviour
    {
        public int nNodeGraphId;
        private SvrNodeGraph nodeGraph;

        private void Start()
        {
            nodeGraph = SvrNodeGraphManager.GetNodeGraph(nNodeGraphId);
            if (nodeGraph == null) return;
            nodeGraph.StartGraph();
        }

        private void Update()
        {
            if (nodeGraph == null) return;
            if (!nodeGraph.IsFinished())
                nodeGraph.UpdateNodes(Time.deltaTime);
        }
    }
}

