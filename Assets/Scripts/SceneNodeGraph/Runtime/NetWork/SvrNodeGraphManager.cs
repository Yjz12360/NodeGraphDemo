using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class SvrNodeGraphManager : MonoBehaviour
    {

        public TextAsset initConfigFile;

        private int nCurrId = 0;
        private Dictionary<int, SvrNodeGraph> nodeGraphs = new Dictionary<int, SvrNodeGraph>();
        private Game.SvrGame game;
        public void Start()
        {
            game = gameObject.GetComponent<Game.SvrGame>();
            if (initConfigFile != null)
                OnTriggerNodeGraph($"{initConfigFile.name}.json");
        }
        private SvrNodeGraph AddNodeGraph(string sConfigFile)
        {
            SvrNodeGraph nodeGraph = new SvrNodeGraph();
            nCurrId++;
            nodeGraph.nNodeGraphId = nCurrId;
            nodeGraph.nodeGraphData = SceneNodeGraphSerializer.Load(sConfigFile);
            nodeGraph.game = game;
            nodeGraphs[nodeGraph.nNodeGraphId] = nodeGraph;
            return nodeGraph;
        }
        public void OnTriggerNodeGraph(string sConfigFile)
        {
            SvrNodeGraph nodeGraph = AddNodeGraph(sConfigFile);
            nodeGraph.StartGraph();
            NodeGraphMessager.S2CActivateNodeGraph(nodeGraph.nNodeGraphId, sConfigFile);
        }

        public void Update()
        {
            List<int> tRemoveList = null;
            foreach(var pair in nodeGraphs)
            {
                int nNodeGraphId = pair.Key;
                SvrNodeGraph nodeGraph = pair.Value;
                nodeGraph.UpdateNodes(Time.deltaTime);
                if (nodeGraph.IsFinished())
                {
                    if (tRemoveList == null)
                        tRemoveList = new List<int>();
                    tRemoveList.Add(nNodeGraphId);
                    NodeGraphMessager.S2CFinishNodeGraph(nNodeGraphId);
                }
            }
            if(tRemoveList != null)
            {
                foreach (int nNodeGraphId in tRemoveList)
                {
                    if (nodeGraphs.ContainsKey(nNodeGraphId))
                        nodeGraphs.Remove(nNodeGraphId);
                }
            }
        }

        public void OnMonsterDead(int nObjectId)
        {
            foreach (SvrNodeGraph nodeGraph in nodeGraphs.Values)
                nodeGraph.OnMonsterDead(nObjectId);
        }

        public void OnMonsterNumChange(int nNum)
        {
            foreach (SvrNodeGraph nodeGraph in nodeGraphs.Values)
                nodeGraph.OnMonsterNumChange(nNum);
        }
    }
}

