using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class NodeGraphServer : MonoBehaviour
    {
        public List<SvrNodeGraph> nodeGraphs = new List<SvrNodeGraph>();

        public void OnTriggerNodeGraph(string sConfigFile)
        {
            SvrNodeGraph nodeGraph = SvrNodeGraphManager.AddNodeGraph(sConfigFile);
            nodeGraph.StartGraph();
            nodeGraphs.Add(nodeGraph);
            Messager.S2CActivateNodeGraph(nodeGraph.nNodeGraphId, sConfigFile);
        }

        public void Update()
        {
            for(int i = nodeGraphs.Count - 1; i >= 0; --i)
            {
                SvrNodeGraph nodeGraph = nodeGraphs[i];
                nodeGraph.UpdateNodes(Time.deltaTime);
                if (nodeGraph.IsFinished())
                {
                    nodeGraphs.Remove(nodeGraph);
                    SvrNodeGraphManager.RemoveNodeGraph(nodeGraph.nNodeGraphId);
                    Messager.S2CFinishNodeGraph(nodeGraph.nNodeGraphId);
                }   
            }
        }

        //public int nNodeGraphId;
        //private SvrNodeGraph nodeGraph;

        //private void Start()
        //{
        //    nodeGraph = SvrNodeGraphManager.GetNodeGraph(nNodeGraphId);
        //    if (nodeGraph == null) return;
        //    nodeGraph.StartGraph();
        //}

        //private void Update()
        //{
        //    if (nodeGraph == null) return;
        //    if (!nodeGraph.IsFinished())
        //        nodeGraph.UpdateNodes(Time.deltaTime);
        //}
    }
}

