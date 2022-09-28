using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class CltNodeGraphBehaviour : MonoBehaviour
    {
        public int nNodeGraphId;
        private CltNodeGraph nodeGraph;
        private void Start()
        {
            nodeGraph = CltNodeGraphManager.GetNodeGraph(nNodeGraphId);
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

