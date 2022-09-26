using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class BaseNode
    {
        public static readonly int nNodeType = 0;

        public string sNodeId;

        public NodeGraph nodeGraph;

        public bool bFinished = false;

        public virtual void StartNode()
        {
            FinishNode();
        }
        public virtual void UpdateNode(float nDeltaTime) 
        {

        }
        protected void FinishNode(int nPath = 1) 
        {
            if (bFinished) return;
            bFinished = true;
            if (nodeGraph != null)
                nodeGraph.FinishNode(sNodeId, nPath);
        }
    }

    public class NodeTransition
    {
        public string sFromNodeId;
        public string sToNodeId;
        public int nPath = 1;
    }

    public enum NodeGraphState
    {
        Pending = 1,
        Running = 2,
        Finished = 3,
    }
    public class NodeGraph
    {
        public Dictionary<string, BaseNode> tNodeMap = new Dictionary<string, BaseNode>();
        public List<NodeTransition> tTransitions = new List<NodeTransition>();
        public string sStartNodeId;

        NodeGraphState nCurrState = NodeGraphState.Pending;
        public List<string> tRunningNodes = new List<string>();

        public void SetStartNodeId(string sNodeId)
        {
            if (!tNodeMap.ContainsKey(sNodeId))
            {
                Debug.LogError($"SetStartNodeId Error: sNodeId {sNodeId} not exist.");
                return;
            }
            sStartNodeId = sNodeId;
        }

        public void AddNode(BaseNode node)
        {
            if(node == null)
            {
                Debug.LogError($"AddNode Error: node is null.");
                return;
            }
            if (tNodeMap.ContainsKey(node.sNodeId))
            {
                Debug.LogError($"AddNode Error: node's id {node.sNodeId} already exists in graph.");
                return;
            }
            node.nodeGraph = this;
            tNodeMap[node.sNodeId] = node;
        }

        public void AddTransition(string sFromNodeId, string sToNodeId, int nPath = 1)
        {
            if(!tNodeMap.ContainsKey(sFromNodeId))
            {
                Debug.LogError($"AddNode Error: sFromNodeId {sFromNodeId} not exist.");
                return;
            }
            if (!tNodeMap.ContainsKey(sToNodeId))
            {
                Debug.LogError($"AddNode Error: sToNodeId {sToNodeId} not exist.");
                return;
            }
            NodeTransition nodeTransition = new NodeTransition();
            nodeTransition.sFromNodeId = sFromNodeId;
            nodeTransition.sToNodeId = sToNodeId;
            nodeTransition.nPath = nPath;
            tTransitions.Add(nodeTransition);
        }

        public void StartGraph()
        {
            tRunningNodes.Clear();
            nCurrState = NodeGraphState.Running;
            tRunningNodes.Add(sStartNodeId);
        }

        public void UpdateNodes(float nDeltaTime)
        {
            if (nCurrState != NodeGraphState.Running) return;
            for(int i = tRunningNodes.Count - 1; i >= 0; --i)
            {
                string sNodeId = tRunningNodes[i];
                if (tNodeMap.ContainsKey(sNodeId))
                {
                    BaseNode node = tNodeMap[sNodeId];
                    node.UpdateNode(nDeltaTime);
                }
            }
        }
        public void FinishNode(string sNodeId, int nPath)
        {
            if (nCurrState != NodeGraphState.Running) return;
            tRunningNodes.Remove(sNodeId);
            foreach(NodeTransition transition in tTransitions)
            {
                if(transition.sFromNodeId == sNodeId && transition.nPath == nPath)
                {
                    tRunningNodes.Add(transition.sToNodeId);
                }
            }
            if (tRunningNodes.Count == 0)
                nCurrState = NodeGraphState.Finished;
        }

        public bool IsFinished()
        {
            return nCurrState == NodeGraphState.Finished;
        }
    }

    public class SceneNodeGraphManager: MonoBehaviour
    {
        public NodeGraph nodeGraph = new NodeGraph();

        private void Start()
        {
            if (nodeGraph == null) return;
            SceneNodeGraphLoader.TestLoad(nodeGraph);
            nodeGraph.StartGraph();
        }

        private void Update()
        {
            if (nodeGraph == null) return;
            if (!nodeGraph.IsFinished())
                nodeGraph.UpdateNodes(Time.deltaTime);
        }
    }

    public class SceneNodeGraphLoader
    {
        public static void TestLoad(NodeGraph nodeGraph)
        {
            nodeGraph.SetStartNodeId("1");
            PrintNode printNode = new PrintNode();
            printNode.sNodeId = "2";
            printNode.sContext = "Test Load";
            nodeGraph.AddNode(printNode);
            nodeGraph.AddTransition("1", "2");
        }
    }

}
