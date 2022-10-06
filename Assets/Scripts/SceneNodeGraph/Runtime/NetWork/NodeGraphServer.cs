using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class NodeGraphServer : MonoBehaviour
    {
        public TextAsset initConfigFile;
        private List<SvrNodeGraph> nodeGraphs = new List<SvrNodeGraph>();
        private Game.SvrGame game;
        public void Start()
        {
            game = gameObject.GetComponent<Game.SvrGame>();
            if (initConfigFile != null)
                OnTriggerNodeGraph($"{initConfigFile.name}.json");
        }
        public void OnTriggerNodeGraph(string sConfigFile)
        {
            SvrNodeGraph nodeGraph = SvrNodeGraphManager.AddNodeGraph(sConfigFile);
            nodeGraph.game = game;
            nodeGraph.StartGraph();
            nodeGraphs.Add(nodeGraph);
            NodeGraphMessager.S2CActivateNodeGraph(nodeGraph.nNodeGraphId, sConfigFile);
        }

        public void Update()
        {            
            for (int i = nodeGraphs.Count - 1; i >= 0; --i)
            {
                SvrNodeGraph nodeGraph = nodeGraphs[i];
                nodeGraph.UpdateNodes(Time.deltaTime);
                if (nodeGraph.IsFinished())
                {
                    nodeGraphs.Remove(nodeGraph);
                    SvrNodeGraphManager.RemoveNodeGraph(nodeGraph.nNodeGraphId);
                    NodeGraphMessager.S2CFinishNodeGraph(nodeGraph.nNodeGraphId);
                }   
            }
        }
    }
}

