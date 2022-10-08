using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class CltNodeGraphManager : MonoBehaviour
    {
        private Dictionary<int, CltNodeGraph> nodeGraphs = new Dictionary<int, CltNodeGraph>();
        private Game.CltGame game;

        private void Start()
        {
            game = gameObject.GetComponent<Game.CltGame>();
        }
        private CltNodeGraph AddNodeGraph(int nNodeGraphId, string sConfigFile)
        {
            if (nodeGraphs.ContainsKey(nNodeGraphId))
            {
                Debug.LogError($"AddNodeGraph error: nNodeGraphId {nNodeGraphId} already exists.");
                return null;
            }
            CltNodeGraph nodeGraph = new CltNodeGraph();
            nodeGraph.nNodeGraphId = nNodeGraphId;
            nodeGraph.nodeGraphData = SceneNodeGraphSerializer.Load(sConfigFile);
            nodeGraph.game = game;
            nodeGraphs[nNodeGraphId] = nodeGraph;
            return nodeGraph;
        }

        public void ActivateNodeGraph(int nNodeGraphId, string sConfigFile)
        {
            if(nodeGraphs.ContainsKey(nNodeGraphId))
            {
                Debug.LogError($"ActivateNodeGraph error: nNodeGraphId {nNodeGraphId} already exists.");
                return;
            }
            CltNodeGraph nodeGraph = AddNodeGraph(nNodeGraphId, sConfigFile);
            nodeGraph.StartGraph();
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
            List<int> tRemoveList = null;
            foreach(var pair in nodeGraphs)
            {
                CltNodeGraph nodeGraph = pair.Value;
                nodeGraph.UpdateNodes(Time.deltaTime);
                if(nodeGraph.IsFinished())
                {
                    if (tRemoveList == null)
                        tRemoveList = new List<int>();
                    tRemoveList.Add(pair.Key);
                }
            }
            if(tRemoveList != null)
            {
                foreach (int nNodeGraphId in tRemoveList)
                {
                    nodeGraphs.Remove(nNodeGraphId);
                }
            }
 
        }

    }
}

