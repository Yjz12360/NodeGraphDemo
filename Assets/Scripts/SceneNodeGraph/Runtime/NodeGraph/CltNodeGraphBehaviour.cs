using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class CltNodeGraphBehaviour : MonoBehaviour
    {
        public CltNodeGraph nodeGraph = new CltNodeGraph();

        private void Start()
        {
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

